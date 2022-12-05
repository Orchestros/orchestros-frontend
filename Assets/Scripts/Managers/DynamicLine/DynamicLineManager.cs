using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

namespace Managers.DynamicLine
{
    public class DynamicLineManager : MonoBehaviourWithState
    {
        public float dynamicDistance = 5f;
        public float lineWidth = 2f;
        public ArenaObjectsManager arenaObjectsManager;
        public GameObject panel;
        public Canvas canvas;
        private readonly Dictionary<int, Renderer> _renderers = new();
        private Camera _camera;
        private RectTransform _canvasRect;

        private readonly List<GameObject> _lines = new();
        private readonly List<GameObject> _texts = new();

        private Bounds _currentBounds;

        private void Start()
        {
            _canvasRect = canvas.GetComponent<RectTransform>();
            _camera = Camera.main;
            arenaObjectsManager.addOnObjectAddedCallback(OnObjectAdded);
            arenaObjectsManager.addOnObjectRemovedCallback(OnObjectRemoved);
        }

        private void OnObjectAdded(GameObject newGameObject)
        {
            _renderers.Add(newGameObject.GetInstanceID(), newGameObject.GetComponent<Renderer>());
        }

        private void OnObjectRemoved(GameObject removedGameObject)
        {
            _renderers.Remove(removedGameObject.GetInstanceID());
        }

        public List<DynamicLine> UpdateBounds(Bounds boundsOfCurrentObjectAdder, GameObject gameObjectToIgnore = null)
        {
            ClearLinesAndTexts();

            var linesTuple = new List<Tuple<DynamicLine, float>>();

            var bottomLeftCorner = boundsOfCurrentObjectAdder.center - boundsOfCurrentObjectAdder.extents;
            var topRightCorner = boundsOfCurrentObjectAdder.center + boundsOfCurrentObjectAdder.extents;

            var renderers = _renderers.Values.ToList();

            if (gameObjectToIgnore != null)
            {
                renderers = _renderers.Where((a) => a.Key != gameObjectToIgnore.GetInstanceID()).Select(x => x.Value)
                    .ToList();
            }

            linesTuple.AddRange(ComputeLinesForPoint(bottomLeftCorner, Direction.Top, Direction.Left, renderers));
            linesTuple.AddRange(ComputeLinesForPoint(topRightCorner, Direction.Bottom, Direction.Right, renderers));
            linesTuple.AddRange(ComputeLinesForPoint(boundsOfCurrentObjectAdder.center, Direction.Center,
                Direction.Center, renderers));

            var dynamicLines = new List<DynamicLine>();


            linesTuple.Sort((a, b) => a.Item2.CompareTo(b.Item2));


            var hasHorizontalAlign = false;
            var hasVerticalAlign = false;

            var linesFound = linesTuple.Select(lineTuple => lineTuple.Item1).GroupBy(s => new { s.Renderer, s.Axis })
                .Select(grp => grp.FirstOrDefault())
                .ToList();


            foreach (var dynamicLine in linesFound)
            {
                if (!hasVerticalAlign && dynamicLine.Direction == Direction.Center && dynamicLine.IsVertical())
                {
                    dynamicLines.Add(dynamicLine);
                    hasVerticalAlign = true;
                }

                if (!hasHorizontalAlign && dynamicLine.Direction == Direction.Center && !dynamicLine.IsVertical())
                {
                    dynamicLines.Add(dynamicLine);
                    hasHorizontalAlign = true;
                }

                if (!hasVerticalAlign && dynamicLine.Direction is Direction.Bottom or Direction.Top)
                {
                    hasVerticalAlign = true;
                    dynamicLines.Add(dynamicLine);
                }

                if (!hasHorizontalAlign && dynamicLine.Direction is Direction.Right or Direction.Left)
                {
                    hasHorizontalAlign = true;
                    dynamicLines.Add(dynamicLine);
                }

                if (hasHorizontalAlign && hasVerticalAlign)
                {
                    break;
                }
            }

            foreach (var line in dynamicLines)
            {
                line.IsUsedToSnap = true;
            }

            foreach (var addTextForLine in dynamicLines.Select(AddTextForLine))
            {
                _texts.Add(addTextForLine);
            }

            foreach (var newLineObject in linesFound.Select(AddLine))
            {
                _lines.Add(newLineObject);
            }

            return dynamicLines;
        }


        private IEnumerable<Tuple<DynamicLine, float>> ComputeLinesForPoint(Vector3 currenPoint,
            Direction horizontalDirection,
            Direction verticalDirection, IEnumerable<Renderer> renderers)
        {
            var objectLines = new List<DynamicLine>();

            foreach (var renderToCompareWith in renderers)
            {
                var bounds = renderToCompareWith.bounds;
                var centerPoint = bounds.center;
                var bottomLeftPoint = (centerPoint - bounds.extents);
                var topRightPoint = (centerPoint + bounds.extents);

                objectLines.Add(new DynamicLine(bottomLeftPoint.x, RectTransform.Axis.Horizontal,
                    false, verticalDirection, currenPoint, bottomLeftPoint, renderToCompareWith));
                objectLines.Add(new DynamicLine(topRightPoint.x, RectTransform.Axis.Horizontal,
                    false, verticalDirection, currenPoint, bottomLeftPoint, renderToCompareWith));
                objectLines.Add(new DynamicLine(bottomLeftPoint.z, RectTransform.Axis.Vertical,
                    false, horizontalDirection, currenPoint, topRightPoint, renderToCompareWith));
                objectLines.Add(new DynamicLine(topRightPoint.z, RectTransform.Axis.Vertical,
                    false, horizontalDirection, currenPoint, topRightPoint, renderToCompareWith));
                objectLines.Add(new DynamicLine(centerPoint.x, RectTransform.Axis.Horizontal, true, verticalDirection,
                    currenPoint, centerPoint, renderToCompareWith));
                objectLines.Add(new DynamicLine(centerPoint.z, RectTransform.Axis.Vertical, true,
                    horizontalDirection, currenPoint, centerPoint, renderToCompareWith));
            }

            return (from dynamicLine in objectLines
                let diff = Math.Abs(dynamicLine.Delta - (dynamicLine.IsVertical()
                    ? currenPoint.z
                    : currenPoint.x))
                where diff < dynamicDistance
                select new Tuple<DynamicLine, float>(dynamicLine, diff)).ToList();
        }

        private GameObject AddLine(DynamicLine line)
        {
            var newLineObject = new GameObject();
            var newImage = newLineObject.AddComponent<Image>();

            var lineColor = line.IsUsedToSnap ? Color.magenta : Color.blue;
            newImage.color = lineColor.WithAlpha(line.OriginatesFromCenter ? .8f : .5f);

            var rect = newLineObject.GetComponent<RectTransform>();
            rect.SetParent(panel.transform);
            rect.localScale = Vector3.one;

            Vector3 screenPointLine;
            Vector2 sizeDelta;
            Vector3 screenToCanvasPosition;

            if (line.IsVertical())
            {
                sizeDelta = new Vector2(Screen.width, lineWidth);
                screenPointLine = _camera.WorldToScreenPoint(new Vector3(0, 0, line.Delta));
                screenToCanvasPosition = ScreenToCanvasPosition(new Vector2(screenPointLine.x, screenPointLine.y));
            }
            else
            {
                sizeDelta = new Vector2(lineWidth, Screen.height);
                screenPointLine = _camera.WorldToScreenPoint(new Vector3(line.Delta, 0, 0));
                screenToCanvasPosition = ScreenToCanvasPosition(new Vector2(screenPointLine.x, screenPointLine.z));
            }

            rect.localPosition = screenToCanvasPosition;
            rect.sizeDelta = sizeDelta;

            return newLineObject;
        }

        private GameObject AddTextForLine(DynamicLine line)
        {
            var newTextObject = new GameObject();
            var newText = newTextObject.AddComponent<TextMeshProUGUI>();
            newText.SetText(
                ((int)(line.OriginPoint - line.DestinationPoint).magnitude).ToString(CultureInfo.InvariantCulture));
            newText.fontSize = 10;
            newText.alignment = TextAlignmentOptions.Center;
            var rect = newText.GetComponent<RectTransform>();
            rect.SetParent(panel.transform);
            rect.localScale = Vector3.one;


            var meanPoint = (line.OriginPoint + line.DestinationPoint) / 2;
            var screenPointLine = _camera.WorldToScreenPoint(meanPoint);


            var screenToCanvasPosition = ScreenToCanvasPosition(new Vector2(screenPointLine.x, screenPointLine.y));

            rect.sizeDelta = new Vector2(100, 14);
            rect.localPosition = screenToCanvasPosition;

            return newTextObject;
        }

        private Vector3 ScreenToCanvasPosition(Vector2 screenPosition)
        {
            var viewportPosition = new Vector2(screenPosition.x / Screen.width,
                screenPosition.y / Screen.height);
            return ViewportToCanvasPosition(viewportPosition);
        }

        private Vector3 ViewportToCanvasPosition(Vector2 viewportPosition)
        {
            var centerBasedViewPortPosition = viewportPosition - new Vector2(0.5f, 0.5f);
            var scale = _canvasRect.sizeDelta;
            return Vector3.Scale(centerBasedViewPortPosition, scale);
        }

        public override bool ShouldBeEnabled(HashSet<Type> activeStates)
        {
            var prohibitedStates = new HashSet<Type>()
            {
                typeof(MoverManager),
                typeof(ArenaObjectsManager),
            };

            return activeStates.Any(x => prohibitedStates.Contains(x));
        }

        private void OnDisable()
        {
            ClearLinesAndTexts();
        }

        public void ClearLinesAndTexts()
        {
            foreach (var lineToDestroy in _lines)
            {
                Destroy(lineToDestroy);
            }

            foreach (var textToDestroy in _texts)
            {
                Destroy(textToDestroy);
            }

            _lines.Clear();
            _texts.Clear();
        }
    }
}
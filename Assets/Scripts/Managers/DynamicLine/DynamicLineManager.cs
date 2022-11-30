using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
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

        public List<DynamicLine> UpdateBounds(Bounds boundsOfCurrentObjectAdder)
        {
            ClearLines();

            var linesTuple = new List<Tuple<DynamicLine, float>>();


            var bottomLeftCorner = boundsOfCurrentObjectAdder.center - boundsOfCurrentObjectAdder.extents;
            var topRightCorner = boundsOfCurrentObjectAdder.center + boundsOfCurrentObjectAdder.extents;

            linesTuple.AddRange(ComputeLinesForPoint(bottomLeftCorner, Direction.Top, Direction.Left));
            linesTuple.AddRange(ComputeLinesForPoint(topRightCorner, Direction.Bottom, Direction.Right));
            linesTuple.AddRange(ComputeLinesForPoint(boundsOfCurrentObjectAdder.center, Direction.Center,
                Direction.Center));


            var dynamicLines = new List<DynamicLine>();

            linesTuple.Sort((a, b) => a.Item2.CompareTo(b.Item2));

            var hasHorizontalAlign = false;
            var hasVerticalAlign = false;

            foreach (var dynamicLine in linesTuple.Select(lineTuple => lineTuple.Item1))
            {
                if (!hasVerticalAlign && dynamicLine.Direction == Direction.Center && !dynamicLine.IsHorizontal())
                {
                    dynamicLines.Add(dynamicLine);
                    hasVerticalAlign = true;
                }                
                if (!hasHorizontalAlign && dynamicLine.Direction == Direction.Center && dynamicLine.IsHorizontal())
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
            
            foreach (var newLineObject in dynamicLines.Select(AddLine))
            {
                _lines.Add(newLineObject);
            }

            return dynamicLines;
        }


        private IEnumerable<Tuple<DynamicLine, float>> ComputeLinesForPoint(Vector3 currenPoint, Direction horizontalDirection,
            Direction verticalDirection)
        {
            var objectLines = new List<DynamicLine>();

            foreach (var (key, arenaObjectRenderer) in _renderers)
            {
                var bounds = arenaObjectRenderer.bounds;

                objectLines.Add(new DynamicLine((bounds.center - bounds.extents).x, RectTransform.Axis.Vertical,
                    false, verticalDirection));
                objectLines.Add(new DynamicLine((bounds.center + bounds.extents).x, RectTransform.Axis.Vertical,
                    false, verticalDirection));
                objectLines.Add(new DynamicLine((bounds.center - bounds.extents).z, RectTransform.Axis.Horizontal,
                    false, horizontalDirection));
                objectLines.Add(new DynamicLine((bounds.center + bounds.extents).z, RectTransform.Axis.Horizontal,
                    false, horizontalDirection));
                objectLines.Add(new DynamicLine(bounds.center.x, RectTransform.Axis.Vertical, true, verticalDirection));
                objectLines.Add(new DynamicLine(bounds.center.z, RectTransform.Axis.Horizontal, true,
                    horizontalDirection));
            }

            var linesWithDelta = new List<Tuple<DynamicLine, float>>();

            foreach (var dynamicLine in objectLines)
            {
                var diff = Math.Abs(dynamicLine.Delta - (dynamicLine.IsHorizontal()
                    ? currenPoint.z
                    : currenPoint.x));

                if (!(diff < dynamicDistance)) continue;
                var tuple = new Tuple<DynamicLine, float>(dynamicLine, diff);
                linesWithDelta.Add(tuple);
            }

            return linesWithDelta;
        }

        private GameObject AddLine(DynamicLine line)
        {
            var newLineObject = new GameObject();
            var newImage = newLineObject.AddComponent<Image>();
            newImage.color = Color.blue.WithAlpha(line.OriginatesFromCenter ? .8f : .5f);
            var rect = newLineObject.GetComponent<RectTransform>();
            rect.SetParent(panel.transform);
            rect.localScale = Vector3.one;

            Vector3 screenPointLine;
            Vector2 sizeDelta;
            Vector3 screenToCanvasPosition;

            if (line.IsHorizontal())
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


        private Vector3 ScreenToCanvasPosition(Vector2 screenPosition)
        {
            var viewportPosition = new Vector2(screenPosition.x / Screen.width,
                screenPosition.y / Screen.height);
            return ViewportToCanvasPosition(viewportPosition);
        }

        private Vector3 ViewportToCanvasPosition(Vector3 viewportPosition)
        {
            var centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
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
            ClearLines();
        }

        private void ClearLines()
        {
            foreach (var lineToDestroy in _lines)
            {
                Destroy(lineToDestroy);
            }

            _lines.Clear();
        }
    }
}
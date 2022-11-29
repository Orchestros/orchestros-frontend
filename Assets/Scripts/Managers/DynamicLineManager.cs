using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

namespace Managers
{
    public class DynamicLineManager : MonoBehaviourWithState
    {
        public float dynamicDistance = 5f;
        public float lineWidth = 2f;
        public Sprite lineSprite;
        public ArenaObjectsManager arenaObjectsManager;
        public GameObject panel;
        public Canvas canvas;
        private readonly Dictionary<int, Renderer> _renderers = new();
        private readonly Dictionary<int, GameObject> _objects = new();
        private Camera _camera;
        private RectTransform _canvasRect;

        private List<GameObject> lines = new();

        private void Start()
        {
            _canvasRect = canvas.GetComponent<RectTransform>();
            _camera = Camera.main;
            arenaObjectsManager.addOnObjectAddedCallback(OnObjectAdded);
        }

        private void OnObjectAdded(GameObject newGameObject)
        {
            _objects.Add(newGameObject.GetInstanceID(), newGameObject);
            _renderers.Add(newGameObject.GetInstanceID(), newGameObject.GetComponent<Renderer>());
        }

        private void Update()
        {
                foreach (var lineToDestroy in lines)
                {
                    Destroy(lineToDestroy);
                }
                
                var ray = _camera.ScreenPointToRay(Input.mousePosition);

                Physics.Raycast(ray, out var hit);
                var currenPoint = hit.point;
                var linesToDisplay = new Dictionary<GameObject, List<Vector2>>();

                foreach (var (key, arenaObjectRenderer) in _renderers)
                {
                    var arenaObject = _objects[key];

                    var linesToDisplayForObject = new List<Vector2>();
                    linesToDisplay.Add(arenaObject, linesToDisplayForObject);

                    var linesX = new List<float>();
                    var linesZ = new List<float>();

                    var bounds = arenaObjectRenderer.bounds;
                    linesX.Add((bounds.center - bounds.extents).x);
                    linesX.Add((bounds.center + bounds.extents).x);
                    linesZ.Add((bounds.center - bounds.extents).z);
                    linesZ.Add((bounds.center + bounds.extents).z);

                    linesToDisplayForObject.AddRange(
                        from lineXDelta in linesX
                        where Math.Abs(lineXDelta - currenPoint.x) < dynamicDistance
                        select new Vector2(lineXDelta, 0)
                    );

                    linesToDisplayForObject.AddRange(
                        from lineZDelta in linesZ
                        where Math.Abs(lineZDelta - currenPoint.z) < dynamicDistance
                        select new Vector2(0, lineZDelta)
                    );
                }

                foreach (var pair in linesToDisplay)
                {
                    foreach (var line in pair.Value)
                    {
                        Debug.Log(line);

                        Vector3 screenPointLine = _camera.WorldToScreenPoint(new Vector3(line.x, 1, line.y));


                        GameObject newLineObject;

                        if (line.x != 0)
                        {
                            newLineObject = AddLine(new Vector2(screenPointLine.x, 0));
                        }
                        else
                        {
                            newLineObject = AddLine(new Vector2(0, screenPointLine.y));
                        }
                        
                        lines.Add(newLineObject);
                    }
                }
        }

        private GameObject AddLine(Vector2 line)
        {
            var newLineObject = new GameObject();
            var newImage = newLineObject.AddComponent<Image>();
            newImage.sprite = lineSprite;
            newImage.color = Color.red;
            var rect = newLineObject.GetComponent<RectTransform>();
            rect.SetParent(panel.transform);
            rect.localScale = Vector3.one;
            rect.localPosition = ScreenToCanvasPosition(line);
            var sizeDelta = line.x == 0 ? new Vector2(Screen.width, lineWidth) : new Vector2(lineWidth, Screen.height);
            rect.sizeDelta = sizeDelta;

            return newLineObject;
        }


        private Vector3 ScreenToCanvasPosition(Vector3 screenPosition)
        {
            var viewportPosition = new Vector3(screenPosition.x / Screen.width,
                screenPosition.y / Screen.height,
                0);
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
    }
}
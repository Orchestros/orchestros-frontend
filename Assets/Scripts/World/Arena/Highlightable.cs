using Unity.VisualScripting;
using UnityEngine;

namespace World.Arena
{
    public class Highlightable : MonoBehaviour
    {
        private const string HighlightedPlaneName = "HighlightedPlane";
        public Material circleMaterial;
        private ArenaObject _arenaObject;
        private LineRenderer _lineRenderer;
        private GameObject _plane;

        // Start is called before the first frame update
        private void Start()
        {
            // Check if there is already a highlighted plane
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.gameObject.name == HighlightedPlaneName)
                {
                    _plane = child.gameObject;
                }
            }

            // If there is already a highlighted plane, disable it and return
            if (_plane != null)
            {
                _plane.SetActive(false);
                return;
            }

            // Create a new highlighted plane
            var localTransform = transform;
            var position = localTransform.position;
            _plane = new GameObject
            {
                name = HighlightedPlaneName,
                transform =
                {
                    parent = localTransform,
                    position = new Vector3(position.x, position.y + 0.1f, position.z),
                    localScale = new Vector3(.3f, .3f, .3f)
                }
            };

            _plane.SetActive(false);

            // Get the ArenaObject and add a LineRenderer to the highlighted plane
            _arenaObject = GetComponent<ArenaObject>();
            _lineRenderer = _plane.AddComponent<LineRenderer>();
        }

        // Destroy the highlighted plane when the object is destroyed
        private void OnDestroy()
        {
            Destroy(_plane);
        }

        // Set the display of the highlighted plane and circle
        public void SetDisplay(bool display)
        {
            // If the highlight should be displayed, add a LineRenderer component to the highlighted plane
            if (display)
            {
                _arenaObject = GetComponent<ArenaObject>();
                _lineRenderer = _plane.GetOrAddComponent<LineRenderer>();

                // Set the color of the circle based on whether the object can be edited
                var color = _arenaObject.CanBeEdited ? Color.blue : Color.black;
                _lineRenderer.startColor = color;
                _lineRenderer.endColor = color;

                // Draw a circle around the object
                DrawCircle(4f, 2);
            }

            _plane.SetActive(display);
        }

        // Draw a circle around the object
        private void DrawCircle(float radius, float lineWidth)
        {
            // Set the material and properties of the LineRenderer
            _lineRenderer.material = circleMaterial;
            const int segments = 360;
            _lineRenderer.useWorldSpace = false;
            _lineRenderer.startWidth = lineWidth;
            _lineRenderer.endWidth = lineWidth;
            _lineRenderer.positionCount = segments + 1;

            // Create an array of points to define the circle
            const int
                pointCount =
                    segments + 1; // add extra point to make start point and endpoint the same to close the circle
            var points = new Vector3[pointCount];

            for (var i = 0; i < pointCount; i++)
            {
                var rad = Mathf.Deg2Rad * (i * 360f / segments);
                points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
            }

            _lineRenderer.SetPositions(points);
        }
    }
}
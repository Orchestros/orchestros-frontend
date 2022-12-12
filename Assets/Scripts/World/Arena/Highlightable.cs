using Unity.VisualScripting;
using UnityEngine;

namespace World.Arena
{
    public class Highlightable : MonoBehaviour
    {
        private ArenaObject _arenaObject;
        private LineRenderer _lineRenderer;
        private GameObject _plane;
        public Material circleMaterial;

        private const string HighlightedPlaneName = "HighlightedPlane";

        // Start is called before the first frame update
        private void Start()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.gameObject.name == HighlightedPlaneName)
                {
                    _plane = child.gameObject;
                }
            }

            if (_plane != null) // If there already was a plane (if the object was copied for instance)
            {
                _plane.SetActive(false);
                return;
            }

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
            
            _arenaObject = GetComponent<ArenaObject>();
            _lineRenderer = _plane.AddComponent<LineRenderer>();

        }

        public void SetDisplay(bool display)
        {
            if (display)
            {
                _arenaObject = GetComponent<ArenaObject>();
                _lineRenderer = _plane.GetOrAddComponent<LineRenderer>();

                var color = _arenaObject.CanBeEdited ? Color.blue : Color.black;
                _lineRenderer.startColor = color;
                _lineRenderer.endColor = color;
                DrawCircle(4f, 2);

            }

            _plane.SetActive(display);
        }

        private void OnDestroy()
        {
            Destroy(_plane);
        }

        private void DrawCircle(float radius, float lineWidth)
        {

            _lineRenderer.material = circleMaterial;
            const int segments = 360;
            _lineRenderer.useWorldSpace = false;
            _lineRenderer.startWidth = lineWidth;
            _lineRenderer.endWidth = lineWidth;
            _lineRenderer.positionCount = segments + 1;

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
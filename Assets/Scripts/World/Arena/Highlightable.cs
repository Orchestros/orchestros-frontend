using UnityEngine;

namespace World.Arena
{
    public class Highlightable : MonoBehaviour
    {
        public Texture selectedTexture;
        private GameObject _plane;
        
        private const string HighlightedPlaneName = "HighlightedPlane";

        // Start is called before the first frame update
        private void Start()
        {
            Debug.Log("STARTING");
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

            _plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            var mesh = _plane.GetComponent<MeshRenderer>();
            var localTransform = transform;
            _plane.name = HighlightedPlaneName;
            var position = localTransform.position;
            _plane.transform.parent = localTransform;
            mesh.material.mainTexture = selectedTexture;
            mesh.material.shader = Shader.Find("Sprites/Default");
            _plane.transform.position = new Vector3(position.x, position.y, position.z);
            _plane.transform.localScale = new Vector3(.3f, .3f, .3f);
            _plane.SetActive(false);
        }

        public void SetDisplay(bool display)
        {
            _plane.SetActive(display);
        }

        private void OnDestroy()
        {
            Destroy(_plane);
        }
    }
}
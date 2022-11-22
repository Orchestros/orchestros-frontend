using UnityEngine;

public class Highlighted : MonoBehaviour
{
    public Texture selectedTexture;

    private GameObject _plane;
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


        if (_plane != null) return;

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
    }

    private void OnDestroy()
    {
        Destroy(_plane);
    }
}
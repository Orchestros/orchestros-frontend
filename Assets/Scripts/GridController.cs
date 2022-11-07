using UnityEngine;

public class GridController : MonoBehaviour
{
    public float lineCount = 100;
    public float lineWidth = 0.0001f;
    public float smallToLargeLine = 3;
    private MeshFilter _meshFilter;


    // Start is called before the first frame update
    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        UpdateGrid();
    }

    private void UpdateGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        var materialColor = new Color(0.69f, 0.76f, 1f);

        var planeMesh = _meshFilter.mesh;
        var bounds = planeMesh.bounds;

        // size in pixels
        var localScale = transform.localScale;

        var halfPlaneWidth = localScale.x * bounds.size.x / 2;
        var halfPlaneHeight = localScale.z * bounds.size.z / 2;


        var largeScaleX = new Vector3(lineWidth, 1, 1);
        var scaleX = new Vector3(lineWidth / smallToLargeLine, 1, 1);

        var t = 0;

        for (var x = -halfPlaneWidth; x < halfPlaneWidth; x += halfPlaneWidth / lineCount)
        {
            var point = new Vector3(x, 0.01f, 0);

            t += 1;
            AddLine(materialColor, point, t % 10 == 0 ? largeScaleX : scaleX);
        }

        var largeScaleZ = new Vector3(1, 1, lineWidth);
        var scaleZ = new Vector3(1, 1, lineWidth / smallToLargeLine);

        for (var z = -halfPlaneHeight; z < halfPlaneHeight; z += halfPlaneHeight / lineCount * 16 / 9)
        {
            var point = new Vector3(0, 0.01f, z);
            t += 1;

            AddLine(materialColor, point, t % 10 == 0 ? largeScaleZ : scaleZ);
        }
    }

    private void AddLine(Color materialColor, Vector3 position, Vector3 scale)
    {
        var line = GameObject.CreatePrimitive(PrimitiveType.Plane);
        Destroy(line.GetComponent<MeshCollider>());
        line.name = "GridLine";
        line.transform.parent = transform;

        var meshRenderer = line.GetComponent<MeshRenderer>();
        meshRenderer.material.color = materialColor;
        line.transform.position = (transform.position + position);
        line.transform.localScale = scale;
    }
}
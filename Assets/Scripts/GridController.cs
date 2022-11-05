using System;
using Unity.VisualScripting;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public float fieldOfViewLineSpacingRatio = 5;
    public float lineRelativeWidth = 0.00005f;
    private float _fieldOfView = 10;
    private Camera _camera;
    private MeshFilter _meshFilter;

    // Start is called before the first frame update
    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _camera = Camera.main;
        _fieldOfView = _camera!.fieldOfView;
        UpdateGrid();
    }

    private void Update()
    {
        if (Math.Abs(_fieldOfView - _camera.fieldOfView) > 0.01)
        {
            _fieldOfView = _camera.fieldOfView;
            UpdateGrid();
        }
    }

    private void UpdateGrid()
    {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        
        var materialColor = new Color(0.69f, 0.76f, 1f);

        var planeMesh = _meshFilter.mesh;
        var bounds = planeMesh.bounds;

        // size in pixels
        var localScale = transform.localScale;

        var halfPlaneWidth = localScale.x * bounds.size.x / 2;
        var halfPlaneHeight = localScale.z * bounds.size.z / 2;

        var lineSpacing = _fieldOfView / fieldOfViewLineSpacingRatio;
        var lineWidth = lineRelativeWidth*lineSpacing;
        
        var scaleX = new Vector3(lineWidth, 1, 1);

        for (var x = -halfPlaneWidth; x < halfPlaneWidth; x += lineSpacing)
        {
            var point = new Vector3(x, 0.01f, 0);
            AddLine(materialColor, point, scaleX);
        }

        var scaleZ = new Vector3(1, 1, lineWidth);

        for (var z = -halfPlaneHeight; z < halfPlaneHeight; z += lineSpacing)
        {
            var point = new Vector3(0, 0.01f, z);
            AddLine(materialColor, point, scaleZ);
        }
    }

    private void AddLine(Color materialColor, Vector3 position, Vector3 scale)
    {
        var line = GameObject.CreatePrimitive(PrimitiveType.Plane);
        line.name = "GridLine";
        line.transform.parent = transform;

        var meshRenderer = line.GetComponent<MeshRenderer>();
        meshRenderer.material.color = materialColor;
        line.transform.position = (transform.position + position);
        line.transform.localScale = scale;
    }
}
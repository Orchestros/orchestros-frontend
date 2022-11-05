using System;
using Unity.VisualScripting;
using UnityEngine;

public class GridController : MonoBehaviour
{        
    private float fieldOfView = 10;
    private Camera _camera;
    private MeshFilter _meshFilter;

    // Start is called before the first frame update
    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _camera = Camera.main;
        fieldOfView = _camera!.fieldOfView;
        UpdateGrid();
    }

    private void Update()
    {
        if (Math.Abs(fieldOfView - _camera.fieldOfView) > 0.01)
        {
            fieldOfView = _camera.fieldOfView;
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


        var scaleX = new Vector3(0.002f, 1, 1);

        for (var x = -halfPlaneWidth; x < halfPlaneWidth; x += fieldOfView)
        {
            var point = new Vector3(x, 0.01f, 0);
            AddLine(materialColor, point, scaleX);
        }

        var scaleZ = new Vector3(1, 1, 0.002f);

        for (var z = -halfPlaneHeight; z < halfPlaneHeight; z += fieldOfView)
        {
            var point = new Vector3(0, 0.01f, z);
            AddLine(materialColor, point, scaleZ);
        }
    }

    private void AddLine(Color materialColor, Vector3 position, Vector3 scale)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Plane);
        cube.name = "GridLine";
        cube.transform.parent = transform;

        var meshRenderer = cube.GetComponent<MeshRenderer>();
        meshRenderer.material.color = materialColor;
        cube.transform.position = (transform.position + position);
        cube.transform.localScale = scale;
    }
}
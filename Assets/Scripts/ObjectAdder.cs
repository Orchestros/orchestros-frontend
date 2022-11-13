using System;
using UnityEngine;

public class ObjectAdder : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private ObjectAdder _planeHandler;
    private Color _initialColor;
    private Camera _mainCamera;

    public Action OnCompleted;

    private void Start()
    {
        _mainCamera = Camera.main;
        _planeHandler = GetComponent<ObjectAdder>();
        _meshRenderer = GetComponent<MeshRenderer>();

        var color = _meshRenderer.material.color;
        _initialColor = color;
        color.a = .5f;
        GetComponent<MeshRenderer>().material.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit))
        {
            var transformPosition = hit.point;
            transform.position = transformPosition;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Destroy(gameObject);
            OnCompleted();
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            var o = gameObject;
            o.layer = 1;
            o.tag = "SelectableObject";
            _meshRenderer.material.color = _initialColor;
            Destroy(_planeHandler);
            OnCompleted();
        }
    }
}
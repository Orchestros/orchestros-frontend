using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Managers.DynamicLine;
using UnityEngine;

public class ObjectAdder : MonoBehaviour
{
    private static readonly int Color1 = Shader.PropertyToID("_Color");
    public DynamicLineManager dynamicLineManager;
    private readonly List<Color> _initialColors = new();

    private int _initialLayer;
    private Camera _mainCamera;


    private List<MeshRenderer> _meshRenderers;
    public Action OnCanceled;

    public Action OnCompleted;

    private void Start()
    {
        _initialLayer = gameObject.layer;
        Debug.Log(_initialLayer);
        _mainCamera = Camera.main;

        _meshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();

        gameObject.layer = 2; // invisible to raytracing in layer 2 

        foreach (var meshRenderer in _meshRenderers)
        {
            var color = meshRenderer.material.color;
            _initialColors.Add(color);
            color.a = .5f;
            meshRenderer.material.SetColor(Color1, color);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit))
        {
            var transformPosition = hit.point;

            var bounds = _meshRenderers.First().bounds;

            foreach (var meshRenderer in _meshRenderers) bounds.Encapsulate(meshRenderer.bounds);

            bounds.center = hit.point;

            transformPosition =
                DynamicLineMoverHelper.RetrieveNewPosition(dynamicLineManager, transformPosition, bounds);
            
            // Object real size
            transformPosition.y = bounds.extents.y;
            
            transform.position = transformPosition.Round();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            OnCanceled();
            Destroy(gameObject);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            for (var i = 0; i < _meshRenderers.Count; i++)
            {
                _meshRenderers[i].material.color = _initialColors[i];
                Destroy(this);
            }

            gameObject.layer = _initialLayer;
            OnCompleted();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Managers.DynamicLine;
using UnityEngine;
using World.Arena;

public class ObjectAdder : MonoBehaviour
{
    public DynamicLineManager dynamicLineManager;


    private List<MeshRenderer> _meshRenderers;
    private readonly List<Color> _initialColors = new();
    private Camera _mainCamera;

    public Action OnCompleted;
    public Action OnCanceled;

    private int _initialLayer;
    
    private static readonly int Color1 = Shader.PropertyToID("_Color");

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

            foreach (var meshRenderer in _meshRenderers)
            {
                bounds.Encapsulate(meshRenderer.bounds);
            }

            bounds.center = hit.point;
            
            transformPosition = DynamicLineMoverHelper.RetrieveNewPosition(dynamicLineManager, transformPosition, bounds);

            transformPosition.y += 0.01f; // prevent flat objects from merging with the ground
            transform.position = transformPosition.Round();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            OnCanceled();
            Destroy(gameObject);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < _meshRenderers.Count; i++)
            {
                _meshRenderers[i].material.color = _initialColors[i];
                Destroy(this);
            }

            gameObject.layer = _initialLayer;
            OnCompleted();
        }
    }

}
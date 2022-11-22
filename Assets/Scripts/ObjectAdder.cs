using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectAdder : MonoBehaviour
{
    private List<MeshRenderer> _meshRenderers;
    private readonly List<Color> _initialColors = new();
    private Camera _mainCamera;

    public Action OnCompleted;
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    private void Start()
    {
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
            transform.position = transformPosition;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Destroy(gameObject);
            OnCompleted();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            var o = gameObject;
            o.layer = 1;
            o.tag = "SelectableObject";

            for (int i = 0; i < _meshRenderers.Count; i++)
            {
                _meshRenderers[i].material.color = _initialColors[i];
                Destroy(this);
            }
            
            OnCompleted(); 
        }
    }
}
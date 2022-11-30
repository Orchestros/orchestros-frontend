using System;
using System.Collections.Generic;
using System.Linq;
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

            var bounds = _meshRenderers.First().bounds;

            foreach (var meshRenderer in _meshRenderers)
            {
                bounds.Encapsulate(meshRenderer.bounds);
            }

            bounds.center = hit.point;

            var dynamicLines = dynamicLineManager.UpdateBounds(bounds);

            Debug.Log(dynamicLines.Count());

            foreach (var line in dynamicLines)
            {
                Debug.Log(line.Direction);
                switch (line.Direction)
                {
                    case Direction.Left:
                        transformPosition.x = line.Delta + bounds.extents.x;
                        break;
                    case Direction.Right:
                        transformPosition.x = line.Delta - bounds.extents.x;
                        break;
                    case Direction.Top:
                        transformPosition.z = line.Delta + bounds.extents.z;
                        break;
                    case Direction.Bottom:
                        transformPosition.z = line.Delta - bounds.extents.z;
                        break;
                    case Direction.Center:
                        if (line.IsHorizontal())
                        {
                            transformPosition.z = line.Delta;
                        }
                        else
                        {
                            transformPosition.x = line.Delta;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            transform.position = transformPosition;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Destroy(gameObject);
            OnCompleted();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < _meshRenderers.Count; i++)
            {
                _meshRenderers[i].material.color = _initialColors[i];
                Destroy(this);
            }

            OnCompleted();
        }
    }
}
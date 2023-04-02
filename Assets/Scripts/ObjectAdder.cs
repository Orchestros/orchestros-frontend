using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Managers.DynamicLine;
using UnityEngine;

/// <summary>
/// Allows the user to add an object to the scene by clicking on a point in the scene and then placing the object at that point.
/// </summary>
public class ObjectAdder : MonoBehaviour
{
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    [Tooltip("The DynamicLineManager instance for handling dynamic line management.")]
    public DynamicLineManager dynamicLineManager;

    private readonly List<Color> _initialColors = new List<Color>();
    private int _initialLayer;
    private Camera _mainCamera;
    private List<MeshRenderer> _meshRenderers;

    [Tooltip("The Action delegate called when the object adding process is canceled.")]
    public Action OnCanceled;

    [Tooltip("The Action delegate called when the object adding process is completed.")]
    public Action OnCompleted;

    private void Start()
    {
        // Retrieve the initial layer of the object and the main camera in the scene
        _initialLayer = gameObject.layer;
        _mainCamera = Camera.main;

        // Get the mesh renderers of the child objects
        _meshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();

        // Set the layer of the object to be invisible to raytracing in layer 2
        gameObject.layer = 2;

        // Change the color of the object's mesh renderers to a partially transparent color
        foreach (var meshRenderer in _meshRenderers)
        {
            var color = meshRenderer.material.color;
            _initialColors.Add(color);
            color.a = .5f;
            meshRenderer.material.SetColor(Color1, color);
        }
    }

    private void Update()
    {
        // Raycast from the mouse position to the scene to find the point where the user has clicked
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit))
        {
            var transformPosition = hit.point;

            // Calculate the size of the object to be added by determining the bounds of its mesh renderers
            var bounds = _meshRenderers.First().bounds;
            foreach (var meshRenderer in _meshRenderers) bounds.Encapsulate(meshRenderer.bounds);
            bounds.center = hit.point;

            // Use the DynamicLineMoverHelper to get a new position for the object that takes into account dynamic line management
            transformPosition = DynamicLineMoverHelper.RetrieveNewPosition(dynamicLineManager, transformPosition, bounds);

            // Set the position of the object and round it to the nearest integer value
            transform.position = transformPosition.Round();
        }

        // If the user presses the escape key, cancel the object adding process and destroy the object
        if (Input.GetKey(KeyCode.Escape))
        {
            OnCanceled?.Invoke();
            Destroy(gameObject);
        }
        // If the user clicks the left mouse button, place the object in the scene and reset its mesh renderer colors
        else if (Input.GetMouseButtonDown(0))
        {
            for (var i = 0; i < _meshRenderers.Count; i++)
            {
                _meshRenderers[i].material.color = _initialColors[i];
            }
            gameObject.layer = _initialLayer;

            // Call the OnCompleted delegate
            OnCompleted?.Invoke();

            // Remove the ObjectAdder script component from the object
            Destroy(this);
        }
    }
}

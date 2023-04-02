using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    // The distance between grid lines in world units
    public float lineSpacing = 5;

    // The thickness of the grid lines in world units
    public float lineWidth = 0.001f;

    // The number of small lines between each large line
    public float smallToLargeLine = 3;

    // The mesh filter component attached to this game object
    private MeshFilter _meshFilter;

    private readonly List<GameObject> _gridLines = new();

    private int _currentLine;

    // This function is called when the script is enabled
    private void Start()
    {
        // Get the mesh filter component attached to this game object
        _meshFilter = GetComponent<MeshFilter>();

        // Update the grid based on the current settings
        UpdateGrid();
    }

    // This function updates the grid based on the current settings
    private void UpdateGrid()
    {
        _currentLine = 0;

        foreach (var t1 in _gridLines)
        {
            t1.SetActive(false);
        }

        // The color of the grid lines
        var materialColor = new Color(0.6f, 0.6f, 0.6f);

        // Get the mesh of the plane object
        var planeMesh = _meshFilter.mesh;

        // Get the bounds of the mesh
        var bounds = planeMesh.bounds;

        // Get the local scale of the game object
        var localScale = transform.localScale;

        // Calculate the half width and height of the plane object in world units
        var halfPlaneWidth = localScale.x * bounds.size.x / 2;
        var halfPlaneHeight = localScale.z * bounds.size.z / 2;

        // The scale of a large line in the x-axis direction
        var largeScaleX = new Vector3(lineWidth, 1, 1);

        // The scale of a small line in the x-axis direction
        var scaleX = new Vector3(lineWidth / smallToLargeLine, 1, 1);

        // Calculate the correction needed to align the grid lines with the plane object
        var xCorrection = halfPlaneWidth % (lineSpacing * 10) - 5 * lineSpacing;

        // Calculate the starting index for the grid line labels
        var t = (int)xCorrection / 10;

        // Loop through the x-axis and add grid lines
        for (var x = -halfPlaneWidth + xCorrection % lineSpacing; x < halfPlaneWidth; x += lineSpacing)
        {
            // Create a point at the current x position and a small height off the ground
            var point = new Vector3(x, 0.01f, 0);

            // Increment the label index
            t += 1;

            // Add the grid line
            AddLine(materialColor, point, t % 10 == 0 ? largeScaleX : scaleX);
        }

        // The scale of a large line in the z-axis direction
        var largeScaleZ = new Vector3(1, 1, lineWidth);

        // The scale of a small line in the z-axis direction
        var scaleZ = new Vector3(1, 1, lineWidth / smallToLargeLine);

        // Calculate the correction needed to align the grid lines with the plane object
        var zCorrection = halfPlaneHeight % (lineSpacing * 10) - 1 * lineSpacing;

        // Reset the label index
        t = (int)zCorrection / 10;

        // Loop through the z-axis and add grid lines
        for (var z = -halfPlaneHeight + zCorrection % lineSpacing; z < halfPlaneHeight; z += lineSpacing)
        {
            // Create a point at the current z position and a small height off the ground
            var point = new Vector3(0, 0.01f, z);

            // Increment the label index
            t += 1;

            // Add the grid line
            AddLine(materialColor, point, t % 10 == 0 ? largeScaleZ : scaleZ);
        }
    }

// This function adds a grid line to the scene
    private void AddLine(Color materialColor, Vector3 position, Vector3 scale)
    {
        GameObject line;
        if (_currentLine < _gridLines.Count)
        {
            line = _gridLines[_currentLine];
            line.SetActive(true);
            line.transform.position = transform.position + position;
            line.transform.localScale = scale;
            return;
        }

        // Create a plane object to represent the grid line
        line = GameObject.CreatePrimitive(PrimitiveType.Plane);

        _gridLines.Add(line);

        // Remove the mesh collider from the plane object to avoid unnecessary collisions
        Destroy(line.GetComponent<MeshCollider>());

        // Rename the object to identify it as a grid line
        line.name = "GridLine";

        // Set the parent of the line object to this game object
        line.transform.parent = transform;

        // Set the color of the line material
        var meshRenderer = line.GetComponent<MeshRenderer>();
        meshRenderer.material.color = materialColor;

        // Set the position and scale of the line object
        line.transform.position = transform.position + position;
        line.transform.localScale = scale;

        _currentLine++;
    }
}
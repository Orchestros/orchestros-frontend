using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Calculates and displays the scale of the current camera view in centimeters.
/// </summary>
public class ScaleIndicator : MonoBehaviour
{
    [Tooltip(
        "The maximum ratio between the absolute delta of the camera view's start and end points and the scale displayed.")]
    public int maxScaleViewportRatio = 10;

    [Tooltip("The GameObject that holds a TextMeshProUGUI component that will display the scale value.")]
    public GameObject text;

    private Camera _camera;
    private TextMeshProUGUI _innerTextMesh;

    private void Start()
    {
        // Get the TextMeshProUGUI component from the text GameObject
        _innerTextMesh = text.GetComponent<TextMeshProUGUI>();

        // Get the Camera component of the main camera in the scene
        _camera = Camera.main;
    }

    private void Update()
    {
        // Calculate the start and end points of the camera view in world space
        var startX = _camera.ViewportToWorldPoint(new Vector2(0, 0)).x;
        var endX = _camera.ViewportToWorldPoint(new Vector2(1, 0)).x;

        // Calculate the absolute delta between the start and end points of the camera view
        var absDelta = Math.Abs(startX - endX);

        // Calculate the scale value by dividing the absolute delta by the maxScaleViewportRatio value
        var t = absDelta / maxScaleViewportRatio;

        // Update the text object's text property with the scale value in centimeters
        _innerTextMesh.text = t.ToString("0.00") + " cm";
    }
}
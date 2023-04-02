using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    // The minimum orthographic size of the camera
    public float minOrthographicSize = 10;

    // The main camera component
    private Camera _camera;

    // The maximum orthographic size of the camera
    private float _maxOrthographicSize;

    // This function is called when the script is enabled
    private void Start()
    {
        // Get the main camera component
        _camera = Camera.main;

        // If a main camera component was found, set the maximum orthographic size
        if (_camera != null)
        {
            _maxOrthographicSize = _camera.orthographicSize;
        }
    }

    // This function is called every frame
    private void Update()
    {
        // If the left control key is held down, do not zoom the camera
        if (Input.GetKey(KeyCode.LeftControl))
        {
            return;
        }

        // If the mouse wheel is not scrolling, do not zoom the camera
        if (Input.mouseScrollDelta.y == 0)
        {
            return;
        }

        // Calculate the new orthographic size based on the mouse wheel input
        var orthographicSize = _camera.orthographicSize + Input.mouseScrollDelta.y * 5;

        // Clamp the orthographic size to the maximum and minimum values
        if (_maxOrthographicSize < orthographicSize)
        {
            orthographicSize = _maxOrthographicSize;
        }
        else if (orthographicSize < minOrthographicSize)
        {
            orthographicSize = minOrthographicSize;
        }

        // Set the orthographic size of the camera to the new value
        _camera.orthographicSize = orthographicSize;
    }
}
using UnityEngine;
using Utils;

public class CameraController : MonoBehaviour
{
    // The movement speed of the camera
    public float speed = 0.5f;

    // The relative drag speed of the camera
    public float relativeDragSpeed = 2f;

    // The main camera component
    private Camera _camera;

    // The origin of the drag gesture
    private Vector3 _dragOrigin;

    int cameraMode = 0;
    
    // This function is called when the script is enabled
    private void Start()
    {
        // Get the main camera component
        _camera = Camera.main;
    }

    // This function is called every frame
    private void Update()
    {
        // Center the camera on the origin if the Ctrl + Alt + C keys are pressed
        CenterCameraOnCtrlAltC();

        // Do not process input if the left control key is held down
        if (Input.GetKey(KeyCode.LeftControl))
        {
            return;
        }
        
        // On tab pressed, toggle the camera between orthographic and perspective
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            cameraMode = (cameraMode + 1) % 3;

            switch (cameraMode)
            {
                case 0:
                    // set x rotation to 90 degrees
                    transform.rotation = Quaternion.Euler(90, 0, 0);
                    _camera.orthographic = true;
                    break;
                case 1:
                    _camera.orthographic = false;
                    break;
                default:
                    _camera.orthographic = false;
                    // rotate the camera 45 degrees
                    transform.rotation = Quaternion.Euler(45, 0, 0);
                    break;
            }
        }

        // Handle drag input
        HandleDragInput();

        // Move the camera based on the continuously retrieved input delta
        transform.position += Mover.RetrieveDeltaContinuously(speed);
    }

    // This function centers the camera on the origin if the Ctrl + Alt + C keys are pressed
    private void CenterCameraOnCtrlAltC()
    {
        if (!Input.GetKey(KeyCode.LeftAlt) || !Input.GetKey(KeyCode.LeftControl) || !Input.GetKey(KeyCode.C))
        {
            return;
        }

        var cameraTransform = _camera.transform;
        cameraTransform.position = new Vector3(0, cameraTransform.position.y, 0);
    }

    // This function handles drag input to move the camera
    private void HandleDragInput()
    {
        // Check if the middle mouse button is pressed down
        if (Input.GetMouseButtonDown(2))
        {
            // Save the origin of the drag gesture
            _dragOrigin = Input.mousePosition;

            return;
        }

        // Check if the middle mouse button is being held down
        if (!Input.GetMouseButton(2)) return;
        
        // Calculate the drag delta as a viewport point
        var delta = -_camera.ScreenToViewportPoint(
            (Input.mousePosition - _dragOrigin) *
            (relativeDragSpeed * _camera.orthographicSize)
        );

        // Translate the camera in the x and z-axis based on the drag delta
        transform.Translate(new Vector3(delta.x, 0, delta.y), Space.World);

        // Save the current mouse position as the new drag origin
        _dragOrigin = Input.mousePosition;
    }
}
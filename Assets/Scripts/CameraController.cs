using UnityEngine;
using Utils;

public class CameraController : MonoBehaviour
{
    public float speed = 0.5f;
    public float relativeDragSpeed = 2f;

    private Vector3 _dragOrigin;
    private Camera _camera;


    // Update is called once per frame
    private void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        CenterCameraOnCtrlAltC();

        if (Input.GetKey(KeyCode.LeftControl)) return;

        if (Input.GetMouseButtonDown(2))
        {
            _dragOrigin = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(2))
        {
            var delta = -_camera.ScreenToViewportPoint(
                (Input.mousePosition - _dragOrigin) *
                (relativeDragSpeed * _camera.orthographicSize)
            );

            transform.Translate(new Vector3(delta.x, 0, delta.y), Space.World);

            _dragOrigin = Input.mousePosition;
        }


        transform.position += Mover.RetrieveDeltaContinuously(speed);
    }

    private void CenterCameraOnCtrlAltC()
    {
        if (!Input.GetKey(KeyCode.LeftAlt) || !Input.GetKey(KeyCode.LeftControl) || !Input.GetKey(KeyCode.C)) return;
        var cameraTransform = _camera.transform;
        cameraTransform.position = new Vector3(0, cameraTransform.position.y, 0);
    }
}
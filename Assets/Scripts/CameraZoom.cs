using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    public float minFieldOfView = 2;
    private float _maxFieldOfView;

    private Camera _camera;
    

    private void Start()
    {
        _camera = Camera.main;
        if (_camera != null) _maxFieldOfView = _camera.fieldOfView;
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            var newFieldOfView = _camera.fieldOfView + Input.mouseScrollDelta.y;

            if (_maxFieldOfView < newFieldOfView)
            {
                newFieldOfView = _maxFieldOfView;
            } else if (newFieldOfView < minFieldOfView)
            {
                newFieldOfView = minFieldOfView;
            }
            
            _camera.fieldOfView = newFieldOfView;
        }
    }
}
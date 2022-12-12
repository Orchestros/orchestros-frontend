using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    public float minOrthographicSize = 10;
    private float _maxOrthographicSize;

    private Camera _camera;
    

    private void Start()
    {
        _camera = Camera.main;
        if (_camera != null) _maxOrthographicSize = _camera.orthographicSize;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl)) return;
        if (Input.mouseScrollDelta.y == 0) return;
        
        var orthographicSize = _camera.orthographicSize + Input.mouseScrollDelta.y*5;

        if (_maxOrthographicSize < orthographicSize)
        {
            orthographicSize = _maxOrthographicSize;
        } else if (orthographicSize < minOrthographicSize)
        {
            orthographicSize = minOrthographicSize;
        }
            
        _camera.orthographicSize = orthographicSize;
    }
}
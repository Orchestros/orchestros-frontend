using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using MouseButton = Unity.VisualScripting.MouseButton;

public class Draggable : MonoBehaviour
{
    public float relativeDragSpeed = 2f;

    private Vector3 _dragOrigin;
    private Camera _camera;


    // Update is called once per frame
    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _dragOrigin = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 delta = -Camera.main.ScreenToViewportPoint(
                (Input.mousePosition - _dragOrigin) *
                (relativeDragSpeed * _camera.orthographicSize)
            );

            transform.Translate(new Vector3(delta.x, 0, delta.y), Space.World);

            _dragOrigin = Input.mousePosition;
        }
    }
}
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using MouseButton = Unity.VisualScripting.MouseButton;

public class Movable : MonoBehaviour
{
    private Vector3 _dragOrigin;
    public float speed = 0.5f;

    
    // Update is called once per frame
    private void Update()
    {
        var deltaX = 0f;
        var deltaZ = 0f;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            deltaZ += speed;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            deltaZ -= speed;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            deltaX += speed;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            deltaX -= speed;
        }

        if (deltaX != 0 || deltaZ != 0)
        {
            transform.position += new Vector3(deltaX, 0, deltaZ);
        }
    }
}
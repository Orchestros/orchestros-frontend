using UnityEngine;
using Utils;

public class Movable : MonoBehaviour
{
    private Vector3 _dragOrigin;
    public float speed = 0.5f;


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl)) return;
        
        transform.position += Mover.RetrieveDeltaContinuously(speed);
    }
}
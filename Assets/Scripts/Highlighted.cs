using System;
using UnityEngine;

public class Highlighted : MonoBehaviour
{
    private GameObject _plane;
    // Start is called before the first frame update
    void Start()
    {
        _plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        var mesh = _plane.GetComponent<MeshRenderer>();
        var localTransform = transform;
        var position = localTransform.position;
        
        _plane.transform.parent = localTransform;
        mesh.material.color = Color.red;
        _plane.transform.position = new Vector3(position.x, position.y, position.z);
        _plane.transform.localScale = new Vector3(.3f, .3f, .3f);
    }

    private void OnDestroy()
    {
     Destroy(_plane);   
    }
}
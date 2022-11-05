using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    private Dictionary<GameObject, Highlighted> _selectedObjects = new Dictionary<GameObject, Highlighted>();
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                var colliderGameObject = hit.collider.gameObject;

                if (_selectedObjects.ContainsKey(colliderGameObject))
                {
                    Destroy(_selectedObjects[colliderGameObject]);
                    _selectedObjects.Remove(colliderGameObject);
                }
                else
                {
                    Highlighted highlighted =  colliderGameObject.AddComponent<Highlighted>();
                    _selectedObjects[colliderGameObject] = highlighted;
                }
            }
        }
    }
}
using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using ObjectManager = Managers.ObjectManager;

public class ObjectButton : MonoBehaviour
{
    public GameObject prefab;
    public ObjectManager manager;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => manager.AddObject(prefab));
    }
}
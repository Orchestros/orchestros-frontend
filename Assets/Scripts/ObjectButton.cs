using System;
using System.Runtime.Serialization;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class ObjectButton : MonoBehaviour
{
    public GameObject prefab;
    public ArenaObjectsManager manager;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => manager.AddObject(prefab));
    }
}
using Managers;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Creates a button that adds a new object to the game arena using a prefab and an ArenaObjectsManager instance.
/// </summary>
public class ObjectButton : MonoBehaviour
{
    [Tooltip("The GameObject that serves as a template for the new object to be added to the game arena.")]
    public GameObject prefab;

    [Tooltip("The instance of the ArenaObjectsManager class, which manages the objects in the game arena.")]
    public ArenaObjectsManager manager;

    private void Start()
    {
        // Add an event listener to the button component of the game object
        gameObject.GetComponent<Button>().onClick.AddListener(() => manager.AddObject(prefab));
    }
}
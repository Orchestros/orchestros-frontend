using UnityEngine;
using UnityEngine.UI;

public class SceneBuilder : MonoBehaviour
{
    public Button addCylinderButton;
    public Button addCubeButton;
    public GameObject cylinder;
    public GameObject cube;

    public bool isAdding;

    // Start is called before the first frame update
    private void Start()
    {
        addCylinderButton.onClick.AddListener(AddCylinder);
        addCubeButton.onClick.AddListener(AddCube);
    }


    private void AddCylinder()
    {
        AddObject(cylinder);
    }

    private void AddCube()
    {
        AddObject(cube);
    }
    

    private void AddObject(GameObject sourceObject)
    {
        if (isAdding) return;

        isAdding = true;

        var newObject = Instantiate(sourceObject, sourceObject.transform.position,
            sourceObject.transform.rotation);

        newObject.layer = 2; // invisible to raytracing in layer 2 

        var c = newObject.AddComponent<ObjectAdder>();
        c.OnCompleted = OnCompleted;
    }

    private void OnCompleted()
    {
        isAdding = false;
    }
}
using UnityEngine;
using UnityEngine.UI;

public class SceneBuilder : MonoBehaviour
{
    public Button button1;
    public Button button2;

    // Start is called before the first frame update
    private void Start()
    {
        button1.onClick.AddListener(X);
    }

    private static void X()
    {
        Debug.Log("OAOA");
    }

    // Update is called once per frame
}
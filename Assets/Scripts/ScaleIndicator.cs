using System;
using TMPro;
using UnityEngine;

public class ScaleIndicator : MonoBehaviour
{
    public int maxScaleViewportRatio = 10;
    public GameObject text;
    public GameObject scale;
    private Camera _camera;
    private TextMeshProUGUI _innerTextMesh;


    // Start is called before the first frame update
    void Start()
    {
        _innerTextMesh = text.GetComponent<TextMeshProUGUI>();
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var startX = _camera.ViewportToWorldPoint(new Vector2(0, 0)).x;
        var endX = _camera.ViewportToWorldPoint(new Vector2(1, 0)).x;

        var absDelta = Math.Abs(startX - endX);

        var t = absDelta / maxScaleViewportRatio;
        _innerTextMesh.text = t.ToString("0.00") + " cm";
        
    }
}
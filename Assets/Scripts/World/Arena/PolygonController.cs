using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using XML;

namespace World.Arena
{
    public class PolygonController : EditableItem.EditableItem
    {
        public GameObject wall;

        private float _borderLength = 10;
        private float _borderWidth = 3;
        private int _bordersCount = 3;

        public readonly List<GameObject> Walls = new();

        private Renderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            gameObject.layer = 2;   
            UpdatePolygon(_borderWidth, _borderLength, _bordersCount);
        }
        
        private void UpdatePolygon(float borderWidth, float borderLength, int bordersCount)
        {
            foreach (var o in Walls) Destroy(o);
            Walls.Clear();

            _borderLength = borderLength;
            _borderWidth = borderWidth;
            _bordersCount = bordersCount;

            var apothemSize = borderLength / (2 * Mathf.Tan(Mathf.PI / bordersCount));
            var paddingFactorForSelectionCircle = 2.3f;
            var scaledApothemSize = apothemSize*paddingFactorForSelectionCircle; 
            var polygonCenter = Vector3.zero - new Vector3(0, 0, apothemSize/scaledApothemSize + borderWidth/2/scaledApothemSize);
            
            for (var i = 0; i < bordersCount; i++)
            {
                var currentWall = Instantiate(wall, transform);
                
                currentWall.GetComponent<Renderer>().material.color = _renderer.material.color;
                currentWall.layer = 0;
                currentWall.transform.localScale = new Vector3(borderWidth/scaledApothemSize, 10/scaledApothemSize, borderLength/scaledApothemSize);
                currentWall.transform.localPosition =  Quaternion.Euler(0, 360f / bordersCount * i, 0) * polygonCenter;
                var degree = 90f + 360f / bordersCount * i;
                currentWall.transform.Rotate(new Vector3(0, degree, 0));
                Walls.Add(currentWall);
            }

            transform.localScale = new Vector3(scaledApothemSize, scaledApothemSize, scaledApothemSize);
            Debug.Log(gameObject.layer);
        }

        public override Dictionary<string, string> GetEditableValues()
        {
            return new Dictionary<string, string>
            {
                {
                    "borders_count", _bordersCount.ToString()
                },
                {
                    "width", _borderWidth.ToString(CultureInfo.InvariantCulture)
                },
                {
                    "length", _borderLength.ToString(CultureInfo.InvariantCulture)
                }
            };
        }

        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            UpdatePolygon(
                float.Parse(newValues["width"]),
                float.Parse(newValues["length"]),
                int.Parse(newValues["borders_count"])
            );
        }
    }
}
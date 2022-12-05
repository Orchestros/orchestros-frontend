using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace World.Arena
{
    public class PolygonController : EditableItem.EditableItem
    {
        public GameObject wall;

        private float _borderLength = 10;
        private float _borderWidth = 3;
        private int _bordersCount = 3;

        private readonly List<GameObject> _gameObjects = new();

        private Renderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            UpdatePolygon(_borderWidth, _borderLength, _bordersCount);
        }

        private void UpdatePolygon(float borderWidth, float borderLength, int bordersCount)
        {
            foreach (var o in _gameObjects) Destroy(o);
            _gameObjects.Clear();

            _borderLength = borderLength;
            _borderWidth = borderWidth;
            _bordersCount = bordersCount;

            var apothemSize = borderLength / (2 * Mathf.Tan(Mathf.PI / bordersCount));
            var parentScale = apothemSize*2.3f;
            var polygonCenter = Vector3.zero - new Vector3(0, 0, apothemSize/parentScale + borderWidth/2/parentScale);
            
            for (var i = 0; i < bordersCount; i++)
            {
                var currentWall = Instantiate(wall, transform);
                currentWall.GetComponent<Renderer>().material.color = _renderer.material.color;
                currentWall.transform.localScale = new Vector3(borderWidth/parentScale, 10, borderLength/parentScale);
                currentWall.transform.localPosition =  Quaternion.Euler(0, 360f / bordersCount * i, 0) * polygonCenter;
                var degree = 90f + 360f / bordersCount * i;
                currentWall.transform.Rotate(new Vector3(0, degree, 0));
                _gameObjects.Add(currentWall);
            }

            transform.localScale = new Vector3(parentScale, 1, parentScale);
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
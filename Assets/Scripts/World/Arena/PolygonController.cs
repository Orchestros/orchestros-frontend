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

        private void Start()
        {
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
            var polygonCenter = Vector3.zero - new Vector3(0, 0, apothemSize);


            for (var i = 0; i < bordersCount; i++)
            {
                var currentWall = Instantiate(wall, transform);
                currentWall.transform.localScale = new Vector3(borderWidth, 10, borderLength);
                currentWall.transform.localPosition =
                    polygonCenter + Quaternion.Euler(0, 360 / bordersCount * i, 0) * polygonCenter;
                currentWall.transform.Rotate(new Vector3(0, 90 + 360 / bordersCount * i, 0));
                _gameObjects.Add(currentWall);
                Quaternion.Euler(0, 360 / bordersCount * i, 0);
            }
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
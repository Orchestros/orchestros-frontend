using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace World.Arena
{
    /// <summary>
    /// Controller for creating a polygonal wall for an arena.
    /// </summary>
    public class PolygonController : EditableItem.EditableItem
    {
        [SerializeField] private GameObject wallPrefab; // Prefab for individual walls
        [SerializeField] public List<GameObject> walls; // List of wall objects that make up the polygon

        private const float PaddingFactorForSelectionCircle = 2.3f; // Factor to scale apothem size by
        private new Renderer _renderer; // Renderer component of this object

        // Polygon parameters
        private float _borderWidth = 3;
        private float _borderLength = 10;
        private int _bordersCount = 3;

        private void Start()
        {
            // Get the renderer component and set the layer to 2
            _renderer = GetComponent<Renderer>();
            gameObject.layer = 2;

            // Generate the initial polygon
            GeneratePolygon();
        }

        /// <summary>
        /// Updates the polygon based on the current parameters.
        /// </summary>
        private void GeneratePolygon()
        {
            // Destroy any existing walls and clear the list
            foreach (var wall in walls)
            {
                Destroy(wall);
            }

            walls.Clear();

            // Calculate the apothem size of the polygon
            var apothemSize = _borderLength / (2 * Mathf.Tan(Mathf.PI / _bordersCount));

            // Calculate the scaled apothem size and the polygon center point
            var scaledApothemSize = apothemSize * PaddingFactorForSelectionCircle;
            var polygonCenter = Vector3.zero -
                                new Vector3(0, 0,
                                    apothemSize / scaledApothemSize + _borderWidth / 2 / scaledApothemSize);

            // Generate the walls for the polygon
            for (var i = 0; i < _bordersCount; i++)
            {
                var currentWall = Instantiate(wallPrefab, transform);
                var wallRenderer = currentWall.GetComponent<Renderer>();

                // Set the wall's color to match the renderer's material color
                wallRenderer.material.color = _renderer.material.color;

                // Set the wall's layer and scale
                currentWall.layer = 0;
                currentWall.transform.localScale = new Vector3(_borderWidth / scaledApothemSize, 10 / scaledApothemSize,
                    _borderLength / scaledApothemSize);

                // Calculate the wall's position and rotation
                var degree = 90f + 360f / _bordersCount * i;
                currentWall.transform.localPosition = Quaternion.Euler(0, 360f / _bordersCount * i, 0) * polygonCenter;
                currentWall.transform.Rotate(new Vector3(0, degree, 0));

                // Add the wall to the list
                walls.Add(currentWall);
            }

            // Scale the polygon
            transform.localScale = new Vector3(scaledApothemSize, scaledApothemSize, scaledApothemSize);
        }

        /// <summary>
        /// Gets a dictionary of editable parameters and their current values.
        /// </summary>
        /// <returns>A dictionary of editable parameters and their current values.</returns>
        public override Dictionary<string, string> GetEditableValues()
        {
            return new Dictionary<string, string>
            {
                { "border_count", _bordersCount.ToString() },
                { "border_width", _borderWidth.ToString(CultureInfo.InvariantCulture) },
                {
                    "border_length", _borderLength
                        .ToString(CultureInfo.InvariantCulture)
                }
            };
        }

        /// <summary>
        /// Updates the polygon with new parameter values.
        /// </summary>
        /// <param name="newValues">A dictionary of new parameter values.</param>
        public override void UpdateValues(Dictionary<string, string> newValues)
        {
            // Parse the new parameter values
            var newBordersCount = int.Parse(newValues["border_count"]);
            var newBorderWidth = float.Parse(newValues["border_width"], CultureInfo.InvariantCulture);
            var newBorderLength = float.Parse(newValues["border_length"], CultureInfo.InvariantCulture);

            // Update the polygon with the new values
            _bordersCount = newBordersCount;
            _borderWidth = newBorderWidth;
            _borderLength = newBorderLength;
            GeneratePolygon();
        }
    }
}
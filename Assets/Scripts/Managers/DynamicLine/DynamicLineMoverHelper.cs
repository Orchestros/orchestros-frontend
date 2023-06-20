using System;
using System.Linq;
using UnityEngine;

namespace Managers.DynamicLine
{
    /// <summary>
    /// A static helper class for moving dynamic lines in the game world.
    /// </summary>
    public static class DynamicLineMoverHelper
    {
        /// <summary>
        /// Retrieves a new position for the given bounds using the dynamic line manager.
        /// </summary>
        /// <param name="dynamicLineManager">The dynamic line manager to use.</param>
        /// <param name="initialPosition">The initial position to move from.</param>
        /// <param name="bounds">The bounds of the object being moved.</param>
        /// <param name="gameObject">The game object being moved.</param>
        /// <returns>The new position of the object.</returns>
        public static Vector3 RetrieveNewPosition(DynamicLineManager dynamicLineManager, Vector3 initialPosition,
            Bounds bounds, GameObject gameObject = null)
        {
            // Update the dynamic lines based on the new bounds.
            var dynamicLines = dynamicLineManager.UpdateBounds(bounds, gameObject);


            // Initialize the transform position to the initial position.
            var transformPosition = initialPosition;

            // Iterate through each dynamic line and update the transform position accordingly.
            foreach (var line in dynamicLines.Where(line => line.IsUsedToSnap))
            {
                switch (line.Direction)
                {
                    case Direction.Left:
                        transformPosition.x = line.Delta + bounds.extents.x;
                        break;
                    case Direction.Right:
                        transformPosition.x = line.Delta - bounds.extents.x;
                        break;
                    case Direction.Top:
                        transformPosition.z = line.Delta + bounds.extents.z;
                        break;
                    case Direction.Bottom:
                        transformPosition.z = line.Delta - bounds.extents.z;
                        break;
                    case Direction.Center:
                        if (line.IsVertical())
                        {
                            transformPosition.z = line.Delta;
                        }
                        else
                        {
                            transformPosition.x = line.Delta;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Compute the delta between the initial position and the transform position.
            var distance = Vector3.Distance(initialPosition, transformPosition);
            

            return distance > 1 ? initialPosition : transformPosition;
        }
    }
}
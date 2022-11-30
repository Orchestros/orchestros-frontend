using System;
using UnityEngine;

namespace Managers.DynamicLine
{
    public static class DynamicLineMoverHelper
    {
        public static Vector3 RetrieveNewPosition(DynamicLineManager dynamicLineManager, Vector3 initialPosition, Bounds bounds, GameObject gameObject = null)
        {
            var dynamicLines = dynamicLineManager.UpdateBounds(bounds, gameObject);

            var transformPosition = initialPosition;
        
            foreach (var line in dynamicLines)
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

            return transformPosition;
        }
    }
}
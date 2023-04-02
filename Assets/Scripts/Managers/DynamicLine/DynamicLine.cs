using UnityEngine;

namespace Managers.DynamicLine
{
    /// <summary>
    /// A class for managing dynamic lines in the game world.
    /// </summary>
    public class DynamicLine
    {
        // The delta value of the dynamic line.
        public readonly float Delta;

        // The axis of the dynamic line.
        public readonly RectTransform.Axis Axis;

        // Whether the dynamic line originates from the center of an object.
        public readonly bool OriginatesFromCenter;

        // The direction of the dynamic line.
        public readonly Direction Direction;

        // The origin point of the dynamic line.
        public readonly Vector3 OriginPoint;

        // The destination point of the dynamic line.
        public readonly Vector3 DestinationPoint;

        // The renderer associated with the dynamic line.
        public readonly Renderer Renderer;

        // Whether the dynamic line is used to snap objects to a grid.
        public bool IsUsedToSnap;

        /// <summary>
        /// Creates a new dynamic line with the given parameters.
        /// </summary>
        /// <param name="delta">The delta value of the dynamic line.</param>
        /// <param name="axis">The axis of the dynamic line.</param>
        /// <param name="originatesFromCenter">Whether the dynamic line originates from the center of an object.</param>
        /// <param name="direction">The direction of the dynamic line.</param>
        /// <param name="originPoint">The origin point of the dynamic line.</param>
        /// <param name="destinationPoint">The destination point of the dynamic line.</param>
        /// <param name="renderer">The renderer associated with the dynamic line.</param>
        public DynamicLine(float delta, RectTransform.Axis axis, bool originatesFromCenter, Direction direction,
            Vector3 originPoint, Vector3 destinationPoint, Renderer renderer)
        {
            Delta = delta;
            Renderer = renderer;
            OriginatesFromCenter = originatesFromCenter;
            Axis = axis;
            Direction = direction;
            OriginPoint = originPoint;
            DestinationPoint = destinationPoint;
        }

        /// <summary>
        /// Returns true if the dynamic line is vertical, false otherwise.
        /// </summary>
        /// <returns>True if the dynamic line is vertical, false otherwise.</returns>
        public bool IsVertical()
        {
            return Axis == RectTransform.Axis.Vertical;
        }
    }
}

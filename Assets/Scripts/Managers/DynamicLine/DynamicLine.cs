using UnityEngine;

namespace Managers.DynamicLine
{
    public class DynamicLine
    {
        public DynamicLine(float delta, RectTransform.Axis axis, bool originatesFromCenter, Direction direction, Vector3 originPoint, Vector3 destinationPoint, Renderer renderer)
        {
            Delta = delta;
            Renderer = renderer;
            OriginatesFromCenter = originatesFromCenter;
            Axis = axis;
            Direction = direction;
            OriginPoint = originPoint;
            DestinationPoint = destinationPoint;
        }

        public readonly Renderer Renderer;
        public readonly Vector3 OriginPoint;
        public readonly Vector3 DestinationPoint;
        public readonly float Delta;
        public readonly bool OriginatesFromCenter;
        public readonly RectTransform.Axis Axis;
        public readonly Direction Direction;
        public bool IsUsedToSnap;

        public bool IsVertical()
        {
            return Axis == RectTransform.Axis.Vertical;
        }
    }
}
using Unity.VisualScripting;
using UnityEngine;

namespace Managers.DynamicLine
{
    public class DynamicLine
    {
        public DynamicLine(float delta, RectTransform.Axis axis, bool originatesFromCenter, Direction direction, Vector3 originPoint, Vector3 destinationPoint)
        {
            Delta = delta;
            OriginatesFromCenter = originatesFromCenter;
            _axis = axis;
            Direction = direction;
            OriginPoint = originPoint;
            DestinationPoint = destinationPoint;
        }

        public readonly Vector3 OriginPoint;
        public readonly Vector3 DestinationPoint;
        public readonly float Delta;
        public readonly bool OriginatesFromCenter;
        private readonly RectTransform.Axis _axis;
        public readonly Direction Direction;
        
        public bool IsVertical()
        {
            return _axis == RectTransform.Axis.Vertical;
        }
    }
}
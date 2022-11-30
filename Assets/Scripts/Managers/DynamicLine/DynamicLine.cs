using UnityEngine;

namespace Managers.DynamicLine
{
    public class DynamicLine
    {
        public DynamicLine(float delta, RectTransform.Axis axis, bool originatesFromCenter, Direction direction)
        {
            Delta = delta;
            OriginatesFromCenter = originatesFromCenter;
            _axis = axis;
            Direction = direction;
        }

        public readonly float Delta;
        public readonly bool OriginatesFromCenter;
        private readonly RectTransform.Axis _axis;
        public readonly Direction Direction;
        
        public bool IsHorizontal()
        {
            return _axis == RectTransform.Axis.Horizontal;
        }
    }
}
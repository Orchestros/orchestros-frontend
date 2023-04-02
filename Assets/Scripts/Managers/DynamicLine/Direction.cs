namespace Managers.DynamicLine
{
    /// <summary>
    /// An enum for the possible directions of a dynamic line.
    /// </summary>
    public enum Direction
    {
        Left, // The dynamic line is to the left of the object being moved.
        Right, // The dynamic line is to the right of the object being moved.
        Top, // The dynamic line is above the object being moved.
        Bottom, // The dynamic line is below the object being moved.
        Center // The dynamic line is at the center of the object being moved.
    }
}

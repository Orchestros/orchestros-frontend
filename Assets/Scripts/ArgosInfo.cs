using UnityEngine;

public class ArgosInfo : MonoBehaviour
{
    // The Argos tag for this game object
    public new ArgosTag tag;
}

// The available Argos tags for game objects
public enum ArgosTag
{
    Cube,
    Polygon,
    Plane,
    Circle,
    Cylinder,
    Robot,
    Light
}
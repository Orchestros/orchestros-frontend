using System;
using UnityEngine;

namespace Utils
{
    public static  class  Dragger
    {
        public static Vector3 RetrieveDragDelta(Camera camera, Vector3 dragOrigin, float relativeDragSpeed)
        {
            var delta = -camera.ScreenToViewportPoint(
                (Input.mousePosition - dragOrigin) *
                (relativeDragSpeed * camera.orthographicSize)
            );

            var translation = new Vector3(delta.x, 0, delta.y);
            return translation;
        }
    }
}
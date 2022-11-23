using System;
using UnityEngine;

namespace Utils
{
    public static class Mover
    {
        public static Vector3 RetrieveDeltaContinuously(float speed)
        {
            return RetrieveDelta(speed, Input.GetKey);
        }
        
        public static Vector3 RetrieveDeltaOneTime(float speed)
        {
            return RetrieveDelta(speed, Input.GetKeyUp);
        }

        private static Vector3 RetrieveDelta(float speed, Func<KeyCode, bool> check)
        {
            var deltaX = 0f;
            var deltaZ = 0f;

            if (check(KeyCode.UpArrow))
            {
                deltaZ += speed;
            }

            if (check(KeyCode.DownArrow))
            {
                deltaZ -= speed;
            }

            if (check(KeyCode.RightArrow))
            {
                deltaX += speed;
            }

            if (check(KeyCode.LeftArrow))
            {
                deltaX -= speed;
            }

            return new Vector3(deltaX, 0, deltaZ);
        }
    }
}
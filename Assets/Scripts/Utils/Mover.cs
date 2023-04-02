using System;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// A static utility class that provides methods for retrieving movement deltas based on keyboard input.
    /// </summary>
    public static class Mover
    {
        /// <summary>
        /// Retrieves the movement delta continuously while a certain key is held down.
        /// </summary>
        /// <param name="speed">The speed of the movement.</param>
        /// <returns>A Vector3 representing the movement delta.</returns>
        public static Vector3 RetrieveDeltaContinuously(float speed)
        {
            return RetrieveDelta(speed, Input.GetKey);
        }

        /// <summary>
        /// Retrieves the movement delta once when a certain key is released.
        /// </summary>
        /// <param name="speed">The speed of the movement.</param>
        /// <returns>A Vector3 representing the movement delta.</returns>
        public static Vector3 RetrieveDeltaOneTime(float speed)
        {
            return RetrieveDelta(speed, Input.GetKeyUp);
        }

        /// <summary>
        /// Retrieves the movement delta based on keyboard input.
        /// </summary>
        /// <param name="speed">The speed of the movement.</param>
        /// <param name="check">A delegate that checks whether a certain key is pressed or released.</param>
        /// <returns>A Vector3 representing the movement delta.</returns>
        private static Vector3 RetrieveDelta(float speed, Func<KeyCode, bool> check)
        {
            var deltaX = 0f;
            var deltaZ = 0f;

            if (check(KeyCode.UpArrow)) deltaZ += speed;

            if (check(KeyCode.DownArrow)) deltaZ -= speed;

            if (check(KeyCode.RightArrow)) deltaX += speed;

            if (check(KeyCode.LeftArrow)) deltaX -= speed;

            return new Vector3(deltaX, 0, deltaZ);
        }
    }
}

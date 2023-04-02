// Import required namespaces

using System;
using System.Globalization;
using System.Linq;
using System.Xml;
using UnityEngine;

// Define the namespace and class name
namespace Managers.Argos.XML
{
    public static class ArgosHelper
    {
// Convert a Unity vector to an Argos vector
        public static string VectorToArgosVector(Vector3 vector)
        {
            return FloatToStringWithArgosFactor(-vector.z) + "," +
                   FloatToStringWithArgosFactor(-vector.x) + "," +
                   FloatToStringWithArgosFactor(vector.y);
        }

        // Convert an Argos vector to a Unity vector
        public static Vector3 ArgosVectorToVector(string argosPosition)
        {
            var values = argosPosition.Split(",").Select(StringToFloatWithInverseArgosFactor).ToList();
            return values.Count == 2
                ? new Vector3(-values[1], 0, values[0])
                : new Vector3(-values[1], values[2], values[0]);
        }

        // Convert an Argos vector to a Unity vector with absolute values
        public static Vector3 ArgosVectorToVectorAbsolute(string argosPosition)
        {
            var values = argosPosition.Split(",").Select(x => Mathf.Abs(StringToFloatWithInverseArgosFactor(x)))
                .ToList();

            return new Vector3(values[1], values[2], values[0]);
        }

        // Convert a Unity vector to an Argos vector without the y component
        public static string VectorToArgosVectorNoHeight(Vector3 vector)
        {
            return FloatToStringWithArgosFactor(vector.z) + "," +
                   FloatToStringWithArgosFactor(-vector.x) + "," +
                   0;
        }

        // Convert a Unity vector to an Argos vector without the y component (for 2D vectors)
        public static string VectorToArgosVectorNoHeight2D(Vector3 vector)
        {
            return FloatToStringWithArgosFactor(vector.z) + "," +
                   FloatToStringWithArgosFactor(-vector.x);
        }

        // Convert a Unity quaternion to an Argos vector
        public static string QuaternionToArgosVector(Quaternion quaternion)
        {
            var vector = quaternion.eulerAngles;

            // Argos treats rotation differently (z,y,x) where z in Argos <=> y in Unity
            return FloatToString(-vector.y) + "," +
                   FloatToString(vector.z) + "," +
                   FloatToString(vector.x);
        }

        // Convert an Argos vector to a Unity quaternion
        public static Quaternion ArgosVectorToQuaternion(string argosPosition)
        {
            var values = argosPosition.Split(",").Select(StringToFloat).ToList();

            return Quaternion.Euler(values[2], -values[0], values[1]);
        }

        // Insert a "body" tag in an XML element containing the position and orientation of a transform
        public static void InsertBodyTagFromTransform(XmlDocument document, XmlElement parentNode, Transform transform)
        {
            var body = document.CreateElement(string.Empty, "body", string.Empty);
            body.SetAttribute("position", VectorToArgosVectorNoHeight(transform.position));
            body.SetAttribute("orientation", QuaternionToArgosVector(transform.rotation));
            parentNode.AppendChild(body);
        }

        // Set the position and orientation of a game object from the "body" tag of an XML element
        public static void SetPositionAndOrientationFromBody(XmlElement element, GameObject gameObject)
        {
            var body = (XmlElement)element.GetElementsByTagName("body")[0];

            gameObject.transform.SetPositionAndRotation(
                ArgosVectorToVector(body.GetAttribute("position")),
                ArgosVectorToQuaternion(body.GetAttribute("orientation"))
            );
        }

        // Convert a
        // Convert a string representing an Argos number to a floating-point number with the inverse Argos factor
        public static float StringToFloatWithInverseArgosFactor(string source)
        {
            return (float)Math.Round(StringToFloat(source) * 100);
        }

        // Convert a string to a floating-point number
        public static float StringToFloat(string source)
        {
            if (source.Length == 0)
            {
                return 0;
            }

            return float.Parse(source, CultureInfo.InvariantCulture);
        }

        // Convert a floating-point number to a string using the invariant culture
        public static string FloatToString(double source)
        {
            return source.ToString(CultureInfo.InvariantCulture);
        }


        // Convert a floating-point number to a string using the Argos factor
        public static string FloatToStringWithArgosFactor(float source)
        {
            return FloatToString(Math.Round(source) / 100);
        }
    }
}
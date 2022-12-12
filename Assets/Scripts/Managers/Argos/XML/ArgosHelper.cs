using System.Globalization;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace Managers.Argos.XML
{
    public static class ArgosHelper
    {
        public static string VectorToArgosVector(Vector3 vector)
        {
            return FloatToStringWithArgosFactor(-vector.z) + "," +
                   FloatToStringWithArgosFactor(-vector.x) + "," +
                   FloatToStringWithArgosFactor(vector.y);
        }

        public static Vector3 ArgosVectorToVector(string argosPosition)
        {
            var values = argosPosition.Split(",").Select(StringToFloatWithInverseArgosFactor).ToList();

            return new Vector3(-values[1], values[2], -values[0]);
        }

        public static Vector3 ArgosVectorToVectorAbsolute(string argosPosition)
        {
            var values = argosPosition.Split(",").Select(x => Mathf.Abs(StringToFloatWithInverseArgosFactor(x)))
                .ToList();

            return new Vector3(values[1], values[2], values[0]);
        }

        public static string VectorToArgosVectorNoHeight(Vector3 vector)
        {
            return FloatToStringWithArgosFactor(vector.z) + "," +
                   FloatToStringWithArgosFactor(-vector.x) + "," +
                   0;
        }


        public static string VectorToArgosVectorNoHeight2D(Vector3 vector)
        {
            return FloatToStringWithArgosFactor(vector.z) + "," +
                   FloatToStringWithArgosFactor(-vector.x);
        }

        private static string QuaternionToArgosVector(Quaternion quaternion)
        {
            var vector = quaternion.eulerAngles;

            // argos treats rotation different (z,y,x) where z i argos <=> y in unity
            return FloatToString(-vector.y) + "," +
                   FloatToString(vector.z) + "," +
                   FloatToString(vector.x);
        }

        private static Quaternion ArgosVectorToQuaternion(string argosPosition)
        {
            var values = argosPosition.Split(",").Select(StringToFloat).ToList();

            return Quaternion.Euler(values[2], values[0], values[1]);
        }

        public static void InsertBodyTagFromTransform(XmlDocument document, XmlElement parentNode, Transform transform)
        {
            var y = document.CreateElement(string.Empty, "body", string.Empty);
            y.SetAttribute("position", VectorToArgosVectorNoHeight(transform.position));
            y.SetAttribute("orientation", QuaternionToArgosVector(transform.rotation));
            parentNode.AppendChild(y);
        }

        public static void SetPositionAndOrientationFromBody(XmlElement element, GameObject gameObject)
        {
            var body = (XmlElement)element.GetElementsByTagName("body")[0];

            gameObject.transform.SetPositionAndRotation(
                ArgosVectorToVector(body.GetAttribute("position")),
                ArgosVectorToQuaternion(body.GetAttribute("orientation"))
            );
        }

        public static string FloatToStringWithArgosFactor(float source)
        {
            return FloatToString(source / 30);
        }

        public static float StringToFloatWithInverseArgosFactor(string source)
        {
            return StringToFloat(source) * 30;
        }

        public static float StringToFloat(string source)
        {
            return float.Parse(source, CultureInfo.InvariantCulture);
        }

        public static string FloatToString(double source)
        {
            return source.ToString(CultureInfo.InvariantCulture);
        }
    }
}
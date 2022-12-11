using System.Globalization;
using System.Xml;
using UnityEngine;

namespace XML
{
    public static class ArgosHelper
    {
        private const double ToArgosFactor = .25;

        public static string VectorToArgosVector(Vector3 vector)
        {
            return FloatToStringWithArgosFactor(vector.x) + "," +
                   FloatToStringWithArgosFactor(vector.y) + "," +
                   FloatToStringWithArgosFactor(vector.z)
                ;
        }

        public static string VectorToArgosVectorNoHeight(Vector3 vector)
        {
            return FloatToStringWithArgosFactor(vector.x) + "," +
                   FloatToStringWithArgosFactor(vector.z) + "," +
                   0;
        }

        private static string QuaternionToArgosVectorNoHeight(Quaternion quaternion)
        {
            var vector = quaternion.eulerAngles;

            return FloatToString(vector.y) + "," +
                   FloatToString(vector.z) + "," +
                   FloatToString(vector.x);
        }

        public static void InsertBodyTagFromTransform(XmlDocument document, XmlElement parentNode, Transform transform)
        {
            var y = document.CreateElement(string.Empty, "body", string.Empty);
            y.SetAttribute("position", VectorToArgosVectorNoHeight(transform.position));
            y.SetAttribute("orientation", QuaternionToArgosVectorNoHeight(transform.rotation));
            parentNode.AppendChild(y);
        }

        public static string FloatToStringWithArgosFactor(float source)
        {
            return FloatToString(source * ToArgosFactor);
        }

        private static string FloatToString(double source)
        {
            return source.ToString(CultureInfo.InvariantCulture);
        }
    }
}
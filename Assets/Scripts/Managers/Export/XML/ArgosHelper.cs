using System.Globalization;
using System.Xml;
using UnityEngine;

namespace Managers.Export.XML
{
    public static class ArgosHelper
    {
        public static string VectorToArgosVector(Vector3 vector)
        {
            return FloatToStringWithArgosFactor(-vector.z) + "," +
                   FloatToStringWithArgosFactor(-vector.x) + "," +
                   FloatToStringWithArgosFactor(vector.y)
                ;
        }

        public static string VectorToArgosVectorNoHeight(Vector3 vector)
        {
            return FloatToStringWithArgosFactor(vector.z) + "," +
                   FloatToStringWithArgosFactor(-vector.x) + "," +
                   0;
        }

        private static string QuaternionToArgosVectorNoHeight(Quaternion quaternion)
        {
            var vector = quaternion.eulerAngles;

            // argos treats rotation different (z,y,x) where z i argos <=> y in unity
            return FloatToString(-vector.y) + "," +
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
            return FloatToString(source/30);
        }

        public static string FloatToString(double source)
        {
            return source.ToString(CultureInfo.InvariantCulture);
        }
    }
}
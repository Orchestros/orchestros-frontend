using System.Globalization;
using System.Xml;
using UnityEngine;

namespace XML
{
    public static class ArgosHelper
    {
        
        public static string VectorToArgosVector(Vector3 vector)
        {
            return vector.x.ToString(CultureInfo.InvariantCulture) + "," +
                   vector.z.ToString(CultureInfo.InvariantCulture) + "," +
                   vector.y.ToString(CultureInfo.InvariantCulture)
                ;
        }

        public static string VectorToArgosVectorNoHeight(Vector3 vector)
        {
            return vector.x.ToString(CultureInfo.InvariantCulture) + "," +
                   vector.z.ToString(CultureInfo.InvariantCulture) + "," +
                   0;
        }
        
        public static void InsertBodyTagFromTransform(XmlDocument document, XmlElement parentNode, Transform transform)
        {
            var y = document.CreateElement(string.Empty, "body", string.Empty);
            y.SetAttribute("position", VectorToArgosVectorNoHeight(transform.position));
            y.SetAttribute("orientation", VectorToArgosVector(transform.rotation.eulerAngles));
            parentNode.AppendChild(y);
        }
    }
}
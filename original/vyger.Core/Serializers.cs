using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace vyger.Core
{
    public static class Serializers
    {
        #region XML Serializers

        public static T FromXml<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(xml))
            {
                T results = (T)serializer.Deserialize(sr);

                return results;
            }
        }

        public static string ToXml(object value)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(value.GetType());

            using (StringWriter sw = new StringWriter(sb))
            {
                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Indent = true
                };

                using (XmlWriter xw = XmlWriter.Create(sw, settings))
                {
                    serializer.Serialize(xw, value);
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
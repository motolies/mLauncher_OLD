using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MLauncher.Class
{
    public static class XMLSerializer
    {
        public static T FromXML<T>(string xml)
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(sr);
            }
        }
        public static T FromXMLFile<T>(string path) 
        {
            string xml = string.Empty;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    sr.BaseStream.Seek(0, SeekOrigin.Begin);
                    while (sr.Peek() > -1)
                    {
                        xml = sr.ReadToEnd();
                    }
                }
            }
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(sr);
            }

        }
        public static string ToXML<T>(T obj, string rootName = null)
        {
            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
            {
                XmlSerializer xmlSerializer;
                if (string.IsNullOrEmpty(rootName))
                {
                    xmlSerializer = new XmlSerializer(typeof(T));
                }
                else
                {
                    xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));
                }
                xmlSerializer.Serialize(sw, obj);

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
        public static void ToXMLFile<T>(T obj, string path, string rootName = null)
        {

            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
            {
                XmlSerializer xmlSerializer;
                if (string.IsNullOrEmpty(rootName))
                {
                    xmlSerializer = new XmlSerializer(typeof(T));
                }
                else
                {
                    xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));
                }
                xmlSerializer.Serialize(sw, obj);

                string strXML = Encoding.UTF8.GetString(stream.ToArray());
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter swrw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        swrw.WriteLine(strXML);
                    }
                }
            }
        }
    }
}

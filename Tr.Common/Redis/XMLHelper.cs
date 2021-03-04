using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tr.Common.XML
{
   public class XMLHelper
    {
        /// <summary>
        /// 反序列化XML为类实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlObj"></param>
        /// <returns></returns>
        public static T DeserializeXML<T>(string xmlObj)
        {
             
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xmlObj))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
        /// <summary>
        /// 序列化实例为XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeXML<T>(T obj,bool omit_namespace=false)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter textWriter = new StreamWriter(ms);
            //StreamWriter textWriter = new StreamWriter(ms,Encoding.UTF8);
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            if (omit_namespace)
            {
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                serializer.Serialize(textWriter, obj, namespaces);
            }
            else
            {
                serializer.Serialize(textWriter, obj);
            }
           
            string xmlstr = Encoding.UTF8.GetString(ms.GetBuffer());
            ms.Close();
            textWriter.Close();
            //根级别上的数据无效
            //xmlstr = "<" + xmlstr.Substring(xmlstr.IndexOf('<') + 1);
            return xmlstr.Trim();
            
        }
    }
}

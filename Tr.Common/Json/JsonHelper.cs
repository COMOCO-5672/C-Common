using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Tr.Common.Json
{
    /// <summary>
    /// Json操作辅助类
    /// </summary>
    public class JsonHelper
    {
        static JsonHelper()
        {
            JsonSerializerSettings setting = new Newtonsoft.Json.JsonSerializerSettings();
            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            {
                setting.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                return setting;
            });
        }

        /// <summary>
        /// 将对象序列化成json格式字符串
        /// </summary>
        /// <param name="objectToSerialize">对象实体</param>
        /// <returns></returns>
        public static string SerializeObject(object objectToSerialize)
        {
            try
            {
                string json = JsonConvert.SerializeObject(objectToSerialize);
                return json;
            }
            catch { return ""; }
        }

        /// <summary>
        /// 将json格式字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonString">json</param>
        /// <returns></returns>
        public static T DeserializeJsonToObject<T>(string jsonString) where T : class
        {
            try
            {
                T obj = JsonConvert.DeserializeObject<T>(jsonString);
                return obj;
            }
            catch { return null; }
        }

        /// <summary>
        /// 将json格式字符串反序列化为匿名对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonString">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns></returns>
        public static T DeserializeAnonymousType<T>(string jsonString, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(jsonString, anonymousTypeObject);
            return t;
        }


        /// <summary>
        /// 将json字符串的节点反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonString">json字符串</param>
        /// <param name="objectName">节点名称</param>
        /// <returns></returns>
        public static T DeserializeJsonToObject<T>(string jsonString, string objectName) where T : class
        {
            try
            {
                return DeserializeJsonToObject<T>(getSubObjectStr(jsonString, objectName));
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 返回一个objectName的子json字符串
        /// </summary>
        /// <param name="jsonString">json字符串</param>
        /// <param name="objectName">节点属性名称</param>
        /// <returns></returns>
        public static string getSubObjectStr(string jsonString, string objectName)
        {
            var obj = JObject.Parse(jsonString).GetValue(objectName);
            return obj != null ? JObject.Parse(jsonString).GetValue(objectName).ToString() : "";
        }
    }
}

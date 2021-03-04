
namespace Tr.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Web;
    using System.Web;
    using Tr.Common.Json;

    /// <summary>
    /// WCF服务REST接口服务辅助类
    /// </summary>
    public class RestInterfaceHelper
    {
        /// <summary>
        /// 获取访问节点的IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetEndpointIPAddress()
        {
            MessageProperties properties = OperationContext.Current.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return endpoint.Address.ToString();
        }

        /// <summary>
        /// 将字符串序列化为内存流
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static MemoryStream SerializeToStream(string str)
        {
            string json = JsonHelper.SerializeObject(str);
            MemoryStream ms = GetMemoryStreamFromString(str);
            return ms;
        }

        /// <summary>
        /// 根据字符串构造流
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static MemoryStream GetMemoryStreamFromString(string str)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms);
            sw.AutoFlush = true;
            sw.Write(str);
            ms.Position = 0;
            return ms;
        }
    }
}
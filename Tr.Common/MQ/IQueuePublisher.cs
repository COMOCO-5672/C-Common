using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tr.Common.MQ
{
    /// <summary>
    /// 消息发送者
    /// </summary>
    public interface IQueuePublisher : IDisposable, IQueueBase
    {
        bool Register_durable_Exchange_and_Queue(string serverIp, int serverPort, string userName, string password, string exchangeName, string queueName, string typeName = "topic", string routeKey = "", bool exchangeDurable = true, bool exchangeAutoDelete = false, bool queueDurable = true, bool queueAutoDelete = false, bool queueExclusive = false,string virtualHost="", string timeout = "");

        bool MessageEnqueue(string message, bool persistent = true);

       

    }
}

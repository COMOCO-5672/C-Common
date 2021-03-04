using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tr.Common.MQ
{
    public interface IQueueSubscriber : IDisposable,IQueueBase
    {
        event Action<IQueueSubscriber, ulong, byte[]> MessageCallback;
     // bool Register_durable_Exchange_and_Queue(string serverIp, int serverPort, string userName, string password, string exchangeName, string queueName, string typeName = "topic", string routeKey = "", bool exchangeDurable = true, bool exchangeAutoDelete = false, bool queueDurable = true, bool queueAutoDelete = false, bool queueExclusive = false);
        bool Register_durable_Exchange_and_Queue(string serverIp, int serverPort, string userName, string password, string exchangeName, string queueName, string typeName = "topic", string routeKey = "", bool exchangeDurable = true, bool exchangeAutoDelete = false, bool queueDurable = true, bool queueAutoDelete = false, bool queueExclusive = false, ushort prefetchCount = 0, bool noAck = false,string virtualHost="", string timeout = "");

        bool AckAnswer(ulong deliveryTag, bool multiple = false);
      
    }
}

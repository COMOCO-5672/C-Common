using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tr.Common.MQ
{
    public static class MessageQueueManager
    {
        private static string IP = "172.0.0.1";
        private static int Port = 5672;
        private static string UserName = "trkj";
        private static string Password = "trkj";

        private static bool bCreateExchange = true;
        private static bool bCreateQueue = true;
        

        public static void CreateExchangeAndQueue(bool b_exchange=true, bool b_queue=true)
        {
            bCreateExchange = b_exchange;
            bCreateQueue = b_queue;
        }
        public static void InitMessageQueueServer(string ip, int port, string userName, string password)
        {
            IP = ip;
            Port = port;
            UserName = userName;
            Password = password;
        }

        /// <summary>
        /// 注册Topic模式的持久化Exchange和Queue
        /// </summary>
        /// <param name="exchangeName">Exchange</param>
        /// <param name="queueName">Queue</param>
        /// <param name="typeName">队列模式</param>
        /// <param name="routeKey">路由Key</param>
        /// <param name="exchangeDurable">Exchange持久化</param>
        /// <param name="exchangeAutoDelete">Exchange断线自动删除</param>
        /// <param name="queueDurable">Queue持久化</param>
        /// <param name="queueAutoDelete">Queue断线自动删除</param>
        /// <param name="queueExclusive">Queue排他性</param>
        /// <param name="prefetchCount">订阅者预取消息数量</param>
        /// <param name="noAck">无需Ack回复</param>
        /// <returns></returns>
        public static IQueueSubscriber GerarateIQueueSubscriber(string exchangeName, string queueName, string typeName = "topic", string routeKey = "", bool exchangeDurable = true, bool exchangeAutoDelete = false, bool queueDurable = true, bool queueAutoDelete = false, bool queueExclusive = false, ushort prefetchCount = 0, bool noAck = false,string virtualHost="",string timeout="")
        {
            IQueueSubscriber client = new MessageQueueClient();
            client.CreateExchange(bCreateExchange);
            client.CreateQueue(bCreateQueue);
            client.Register_durable_Exchange_and_Queue(IP, Port, UserName, Password, exchangeName, queueName, typeName, routeKey, exchangeDurable, exchangeAutoDelete, queueDurable, queueAutoDelete, queueExclusive, prefetchCount, noAck,virtualHost,timeout);
            return client;
        }

        /// <summary>
        /// 注册Topic模式的持久化Exchange和Queue
        /// </summary>
        /// <param name="exchangeName">Exchange</param>
        /// <param name="queueName">Queue</param>
        /// <param name="typeName">队列模式</param>
        /// <param name="routeKey">路由Key</param>
        /// <param name="exchangeDurable">Exchange持久化</param>
        /// <param name="exchangeAutoDelete">Exchange断线自动删除</param>
        /// <param name="queueDurable">Queue持久化</param>
        /// <param name="queueAutoDelete">Queue断线自动删除</param>
        /// <param name="queueExclusive">Queue排他性</param>
        /// <returns></returns>
        public static IQueuePublisher GerarateIQueuePublisher(string exchangeName, string queueName, string typeName = "topic", string routeKey = "", bool exchangeDurable = true, bool exchangeAutoDelete = false, bool queueDurable = true, bool queueAutoDelete = false, bool queueExclusive = false,string virtualHost="",string timeout="")
        {
            IQueuePublisher client = new MessageQueueClient();
            client.CreateExchange(bCreateExchange);
            client.CreateQueue(bCreateQueue);
            client.Register_durable_Exchange_and_Queue(IP, Port, UserName, Password, exchangeName, queueName, typeName, routeKey, exchangeDurable, exchangeAutoDelete, queueDurable, queueAutoDelete, queueExclusive,virtualHost,timeout);
            return client;
        }
    }
}

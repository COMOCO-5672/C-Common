using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tr.Common.Log;

namespace Tr.Common.MQ
{
    public class MessageQueueClient : IQueueSubscriber, IQueuePublisher
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        private string ServerIp;
        /// <summary>
        /// 服务端口
        /// </summary>
        private int ServerPort;
        /// <summary>
        /// 虚拟主机
        /// </summary>
        private string VirtualHost;

        /// <summary>
        /// 队列上消息过期时间
        /// </summary>
        private string TimeOut;
        /// <summary>
        /// 用户名
        /// </summary>
        private string UserName;
        /// <summary>
        /// 密码
        /// </summary>
        private string Password;
        /// <summary>
        /// Exchange的名称
        /// </summary>
        private string ExchangeName;
        /// <summary>
        /// Queue的名称
        /// </summary>
        private string QueueName;
        /// <summary>
        /// 类型
        /// </summary>
        private string TypeName;
        /// <summary>
        /// 路由Key名称
        /// </summary>
        private string RoutingKey;
        /// <summary>
        /// Exchange持久化
        /// </summary>
        private bool ExchangeDurable = true;
        /// <summary>
        /// Exchange自动删除
        /// </summary>
        private bool ExchangeAutoDelete = false;
        /// <summary>
        /// Queue持久化
        /// </summary>
        private bool QueueDurable = true;
        /// <summary>
        /// Queue自动删除
        /// </summary>
        private bool QueueAutoDelete = false;
        /// <summary>
        /// Queue排他性
        /// </summary>
        private bool QueueExclusive = false;
        /// <summary>
        /// 无需Ack回复
        /// </summary>
        private bool NoAck = true;
        /// <summary>
        /// 订阅者预取总数
        /// </summary>
        private ushort PrefetchCount = 0;
        /// <summary>
        /// 订阅者模式
        /// </summary>
        private bool IsSubscriber = false;
        /// <summary>
        /// 
        /// </summary>
        private IBasicProperties Props;

        /// <summary>
        /// 通道
        /// </summary>
        private IModel channel;
        /// <summary>
        /// 连接
        /// </summary>
        private IConnection connection;
        /// <summary>
        /// 基于事件订阅者
        /// </summary>
        private EventingBasicConsumer eventintBasicConsumer;

        private bool bCreatedExchange = true;

        private bool bCreatedQueue = true;
        /// <summary>
        /// 消息回调事件委托
        /// </summary>
        public event Action<IQueueSubscriber, ulong, byte[]> MessageCallback;

        /// <summary>
        /// 当前缓存的连接信息
        /// </summary>
        public static List<IConnectionResource> IConnectionResourceList = new List<IConnectionResource>();
        /// <summary>
        /// 线程安全锁对象
        /// </summary>
        public static object o = new object();


        public static IConnection GetConnectionByServerInfo(string serverIp, int serverPort, string userName, string password,string virtualHost="")
        {
            lock (o)
            {
                IConnectionResource res = IConnectionResourceList.FirstOrDefault(x => x.ServerIp.Equals(serverIp) && x.ServerPort == serverPort &&x.VirtualHost==virtualHost);
                if (res == null)
                {
                    ConnectionFactory factory = new ConnectionFactory
                    {
                        HostName = serverIp,
                        Port = serverPort,
                        UserName = userName,
                        Password = password,
                        VirtualHost =virtualHost,
                        AutomaticRecoveryEnabled = true,    //自动重连
                        TopologyRecoveryEnabled = true,     //恢复拓扑结构
                        UseBackgroundThreadsForIO = true,   //后台处理消息
                        RequestedHeartbeat = 60,             //心跳超时时间
                        
                    };

                    //string tempStr = string.Format("serverIp:{0},serverPort:{1},userName:{2},password:{3}", serverIp, serverPort, userName, password);
                    //Log.TrLog.Default.Info(tempStr);
                    IConnection connection = factory.CreateConnection();
                   
                    connection.ConnectionShutdown += connection_ConnectionShutdown;
                    connection.CallbackException += connection_CallbackException;
                    connection.ConnectionBlocked += connection_ConnectionBlocked;
                    connection.ConnectionUnblocked += connection_ConnectionUnblocked;

                    res = new IConnectionResource()
                    {
                        ServerIp = serverIp,
                        ServerPort = serverPort,
                        Connection = connection,
                        VirtualHost=virtualHost,
                        ReferenceCount = 1
                    };
                    IConnectionResourceList.Add(res);
                }
                else
                {
                    res.ReferenceCount += 1;
                }
                //
                return res.Connection;
            }
        }

        /// <summary>
        /// 注册Topic模式的持久化Exchange和Queue
        /// </summary>
        /// <param name="serverIp">服务地址</param>
        /// <param name="serverPort">服务端口(默认5672)</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="exchangeName">Exchange</param>
        /// <param name="queueName">Queue</param>
        /// <param name="typeName">队列模式</param>
        /// <param name="routeKey">路由Key</param>
        /// <param name="exchangeDurable">Exchange持久化</param>
        /// <param name="exchangeAutoDelete">Exchange断线自动删除</param>
        /// <param name="queueDurable">Queue持久化</param>
        /// <param name="queueAutoDelete">Queue断线自动删除</param>
        /// <param name="queueExclusive">Queue排他性</param>
        /// <param name="isSubscriber">是订阅者</param>
        /// <param name="prefetchCount">订阅者预取消息数量</param>
        /// <param name="noAck">无需Ack回复</param>
        /// <returns></returns>
        public bool Register_durable_Exchange_and_Queue(string serverIp, int serverPort, string userName, string password, string exchangeName, string queueName, string typeName = "topic", string routeKey = "", bool exchangeDurable = true, bool exchangeAutoDelete = false, bool queueDurable = true, bool queueAutoDelete = false, bool queueExclusive = false, bool isSubscriber = false, ushort prefetchCount = 0, bool noAck = false ,string virtualHost="",string timeout="")
        {
            this.ServerIp = serverIp;
            this.ServerPort = serverPort;
            this.UserName = userName;
            this.Password = password;
            this.ExchangeName = exchangeName;
            this.QueueName = queueName;
            this.RoutingKey = queueName;
            this.TypeName = typeName;
            this.ExchangeDurable = exchangeDurable;
            this.ExchangeAutoDelete = exchangeAutoDelete;
            this.QueueDurable = queueDurable;
            this.QueueAutoDelete = queueAutoDelete;
            this.QueueExclusive = queueExclusive;
            this.RoutingKey = string.IsNullOrWhiteSpace(routeKey) ? ExchangeName + "." + QueueName : routeKey;
            this.IsSubscriber = isSubscriber;
            this.NoAck = noAck;
            this.PrefetchCount = prefetchCount;
            this.VirtualHost = virtualHost;
            this.TimeOut = timeout;
            return Register_durable_Exchange_and_Queue();
        }
        /// <summary>
        /// 注册队列及生成连接信息
        /// </summary>
        /// <returns></returns>
        public bool Register_durable_Exchange_and_Queue()
        {
            bool success = true;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("x-message-ttl", Convert.ToInt32(this.TimeOut));//队列上消息过期时间，应小于队列过期时间
            try
            {
                connection = GetConnectionByServerInfo(this.ServerIp, this.ServerPort, this.UserName, this.Password,this.VirtualHost);
                //
                channel = connection.CreateModel();
                channel.ModelShutdown += Channel_ModelShutdown;
                channel.BasicRecoverOk += Channel_BasicRecoverOk;
                channel.CallbackException += Channel_CallbackException;

                if (bCreatedExchange)
                {
                    channel.ExchangeDeclare(
                        exchange: this.ExchangeName,        //Exchange的名称
                        type: this.TypeName,                //类型
                        durable: this.ExchangeDurable,      //持久化 true
                        autoDelete: this.ExchangeAutoDelete,//自动删除 false
                        arguments: null                     //
                        );
                }
                if (bCreatedQueue)
                {
                    channel.QueueDeclare(
                        queue: this.QueueName,              //队列名
                        durable: this.QueueDurable,         //持久化
                        exclusive: this.QueueExclusive,     //排他性
                        autoDelete: this.QueueAutoDelete,   //客户端断线则自动删除
                        arguments:dic                       //队列消息过期时间
                        );

                    
                }
                channel.QueueBind(
                        queue: this.QueueName,
                        exchange: this.ExchangeName,
                        routingKey: this.RoutingKey);

                if (this.IsSubscriber)
                {
                    eventintBasicConsumer = new EventingBasicConsumer(channel);
                    eventintBasicConsumer.Received += eventintBasicConsumer_Received;
                    //
                    channel.BasicQos(0, this.PrefetchCount, false);  //每次只取一条数据
                    channel.BasicConsume(this.QueueName, this.NoAck, eventintBasicConsumer);
                }
                else
                {
                    Props = channel.CreateBasicProperties();
                    Props.Persistent = false;
                }
            }
            catch (Exception ex)
            {
                success = false;
                Console.WriteLine(string.Format("{0}\n{1}\n{2}", ex.Message, ex.StackTrace, ex.Source));
                TrLog.Default.Error(ex.Message);
            }
            return success;
        }


        #region IConnection事件

        private static void connection_ConnectionUnblocked(object sender, EventArgs e)
        {
            TrLog.Default.Info(e.ToString());
        }

        private static void connection_ConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
           TrLog.Default.Info(e.Reason);
        }

        private static void connection_CallbackException(object sender, CallbackExceptionEventArgs e)
        {
            TrLog.Default.Error(e.Exception.Message);
        }

        private static void connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            TrLog.Default.Info(e.ReplyText);
        }

        #endregion


        #region Channel事件回调

        private void Channel_CallbackException(object sender, CallbackExceptionEventArgs e)
        {
            TrLog.Default.Error(e.Exception.Message);
        }

        private void Channel_BasicRecoverOk(object sender, EventArgs e)
        {
            TrLog.Default.Info(e.ToString());
        }

        private void Channel_ModelShutdown(object sender, ShutdownEventArgs e)
        {
            TrLog.Default.Info(e.ReplyText);
        }

        #endregion


        #region 消息订阅

        public bool Register_durable_Exchange_and_Queue(string serverIp, int serverPort, string userName, string password, string exchangeName, string queueName, string typeName = "topic", string routeKey = "", bool exchangeDurable = true, bool exchangeAutoDelete = false, bool queueDurable = true, bool queueAutoDelete = false, bool queueExclusive = false, ushort prefetchCount = 0, bool noAck = false,string virtualHost="", string timeout = "")
        {
            bool success = Register_durable_Exchange_and_Queue(serverIp, serverPort, userName, password, exchangeName, queueName, typeName, routeKey, exchangeDurable, exchangeAutoDelete, queueDurable, queueAutoDelete, queueExclusive, true, prefetchCount, noAck,virtualHost,timeout);
            return success;
        }

        /// <summary>
        /// 消息回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eventintBasicConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            if (MessageCallback != null)
            {
                MessageCallback(this, e.DeliveryTag, e.Body);
            }
        }

        /// <summary>
        /// 消息应答
        /// </summary>
        /// <param name="deliveryTag"></param>
        /// <param name="multiple"></param>
        public bool AckAnswer(ulong deliveryTag, bool multiple = false)
        {
            bool success = true;
            try
            {
                channel.BasicAck(deliveryTag, multiple);
            }
            catch (Exception ex)
            {
               TrLog.Default.Error(ex.Message);
                success = false;
            }
            return success;
        }

        #endregion


        #region 消息发送

        public bool Register_durable_Exchange_and_Queue(string serverIp, int serverPort, string userName, string password, string exchangeName, string queueName, string typeName = "topic", string routeKey = "", bool exchangeDurable = true, bool exchangeAutoDelete = false, bool queueDurable = true, bool queueAutoDelete = false, bool queueExclusive = false,string virtualHost="", string timeout = "")
        {
            bool success = Register_durable_Exchange_and_Queue(serverIp, serverPort, userName, password, exchangeName, queueName, typeName, routeKey, exchangeDurable, exchangeAutoDelete, queueDurable, queueAutoDelete, queueExclusive,true,1,false,virtualHost,timeout);
            return success;
        }

        /// <summary>
        /// 消息入队
        /// </summary>
        /// <param name="message">消息字符串</param>
        /// <param name="persistent">持久化</param>
        /// <returns></returns>
        public bool MessageEnqueue(string message, bool persistent = true)
        {
            bool success = true;
            try
            {
                var msgBody = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: this.ExchangeName, routingKey: this.RoutingKey, basicProperties: this.Props, body: msgBody);
                return true;
            }
            catch (Exception ex)
            {
                TrLog.Default.Error(ex.Message);
                success = false;
            }
            return success;
        }

        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (this.IsSubscriber)
                    eventintBasicConsumer.Received -= eventintBasicConsumer_Received;
                //
                channel.Close();
                channel.Dispose();
                //
                lock (o)
                {
                    IConnectionResource res = IConnectionResourceList.FirstOrDefault(x => x.ServerIp.Equals(this.ServerIp) && x.ServerPort == ServerPort);
                    if (res != null)
                    {
                        res.ReferenceCount--;
                        if (res.ReferenceCount < 1)
                        {
                            connection.Close();
                            connection.Dispose();
                            //
                            IConnectionResourceList.Remove(res);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                TrLog.Default.Error("资源释放异常：" + ex.Message);
            }
        }

        public void CreateExchange(bool bCreated = true)
        {
            bCreatedExchange = bCreated;
        }

        public void CreateQueue(bool bCreated = true)
        {
            bCreatedQueue = bCreated;
        }
    }

    /// <summary>
    /// 一个RabbitMq连接资源的定义
    /// </summary>
    public class IConnectionResource
    {
        public string ServerIp { get; set; }

        public int ServerPort { get; set; }
        public string VirtualHost { get; set; }

        public IConnection Connection { get; set; }

        public int ReferenceCount { get; set; }
    }
}

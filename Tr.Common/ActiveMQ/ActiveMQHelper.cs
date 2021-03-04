using System;
using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;
using Apache.NMS.Util;
using System.IO;

namespace Tr.Common.ActiveMQ
{
    public class ActiveMQHelper
    {
        protected  IConnectionFactory _factory=null;
        protected  IConnection _connection=null;
        protected  ISession _session=null;
        protected IDestination _destination = null;
        protected  IMessageConsumer _consumer=null;
        protected IMessageProducer _producer = null;

        protected  string _address=null;
        protected  string _username=null;
        protected  string _passwd=null;

        protected  string _clientId=null;
        protected  string _topicName=null;
        protected  bool _anonymous=false;

        protected string _name = null;

        
        public  void UnInit()
        {
            
            _session.Close();
            _connection.Close();
        }
        public void CreateConsumer()
        {
            try
            {
                _factory = new NMSConnectionFactory(_address);
                if (_anonymous)
                    _connection = _factory.CreateConnection();
                else
                    _connection = _factory.CreateConnection(_username, _passwd);

                _connection.ClientId = _clientId;
                _connection.Start();
                _session = _connection.CreateSession();
                _consumer = _session.CreateDurableConsumer(
                        new ActiveMQTopic(_topicName), _clientId, null, false);
                _consumer.Listener += new MessageListener(consumer_Listener);
                Tr.Common.Log.TrLog.Default.Info($"[{_name}] ActiveMQ_Address:{_address};ActiveMQ_UserName:{_username};ActiveMQ_Passwd:{_passwd};" +
                    $"ActiveMQ_TopicName:{_topicName}");

            }
            catch (System.Exception ex)
            {
                Tr.Common.Log.TrLog.Default.Error(ex.Message, ex);
            }
        }
        public void CreateProducer()
        {
            try
            {
                _factory = new NMSConnectionFactory(_address);
                if (_anonymous)
                    _connection = _factory.CreateConnection();
                else
                    _connection = _factory.CreateConnection(_username, _passwd);

                //_connection.ClientId = _clientId;
                _connection.Start();
                _session = _connection.CreateSession();

                _destination = SessionUtil.GetDestination(_session,"topic://"+_topicName);
                _producer = _session.CreateProducer(_destination);
                _producer.DeliveryMode = MsgDeliveryMode.Persistent;

                Log.TrLog.Default.Info($"[{_name}] ActiveMQ_Address:{_address};ActiveMQ_UserName:{_username};ActiveMQ_Passwd:{_passwd};" +
                    $"ActiveMQ_TopicName:{_topicName}");

            }
            catch (System.Exception ex)
            {
                Log.TrLog.Default.Error(ex.Message, ex);
            }
        }
        public virtual void consumer_Listener(IMessage message)
        {
        }
        public virtual void producer_Send(String text)
        {
            try
            {
                var msg = _session.CreateTextMessage(text);
                _producer.Send(msg);
            }
            catch(System.Exception ex)
            {
                Log.TrLog.Default.Error(ex.Message, ex);
            }
        }
    }
}

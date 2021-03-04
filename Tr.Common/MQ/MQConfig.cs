using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tr.Common.MQ
{
    /// <summary>
    /// MQ 实体信息
    /// </summary>
    public abstract class MQConfig
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string Address { get { return Config.GetValue("RabbitMQ_Address"); } }

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get { return Config.GetValue("RabbitMQ_Port").ToInt32(); } }

        /// <summary>
        /// 用户名
        /// </summary>
        public string User { get { return Config.GetValue("RabbitMQ_User"); } }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get { return Config.GetValue("RabbitMQ_Password"); } }
               

        public abstract void Start();
        public abstract void Stop();
    }
}

using System;
namespace Tr.Common.Log
{
    public class TrLog
    {
        NLog.Logger logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        private TrLog(NLog.Logger logger)
        {
            this.logger = logger;
        }

        public TrLog(string name)
            : this(NLog.LogManager.GetLogger(name))
        {
        }

        public static TrLog Default { get; private set; }
        static TrLog()
        {
            Default = new TrLog(NLog.LogManager.GetCurrentClassLogger());
        }

        public void Debug(string msg, params object[] args)
        {
            logger.Debug(msg, args);
        }

        public void Debug(string msg, Exception err)
        {
            logger.Debug(err, msg);
        }

        public void Info(string msg, params object[] args)
        {
            logger.Info(msg, args);
        }

        public void Info(string msg, Exception err)
        {
            logger.Info(err, msg);
        }

        public void Trace(string msg, params object[] args)
        {
            logger.Trace(msg, args);
        }

        public void Trace(string msg, Exception err)
        {
            logger.Trace(err, msg);
        }

        public void Error(string msg, params object[] args)
        {
            logger.Error(msg, args);
        }

        public void Error(string msg, Exception err)
        {
            logger.Error(err, msg);
        }

        public void Fatal(string msg, params object[] args)
        {
            logger.Fatal(msg, args);
        }

        public void Fatal(string msg, Exception err)
        {
            logger.Fatal(err, msg);
        }
    }
}

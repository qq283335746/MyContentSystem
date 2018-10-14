using System;
using System.Configuration;
using System.Messaging;
using System.ServiceProcess;
using TygaSoft.TaskProcessor;

namespace TygaSoft.TaskWS.Afd
{
    public class Service : ServiceBase
    {
        public Service()
        {
            ServiceName = "TygaSoft.Afd.Service";
        }

        public static void Main()
        {
            ServiceBase.Run(new Service());
        }

        protected override void OnStart(string[] args)
        {
            InitLog4Net();

            #region 检查并创建 MSMQ 消息队列

            string afdRunQueue = ConfigurationManager.AppSettings["AfdRunQueue"];
            if (!MessageQueue.Exists(afdRunQueue)) MessageQueue.Create(afdRunQueue, true);

            #endregion

            BaseTask.Run();

        }

        protected override void OnStop()
        {
            
        }

        private static void InitLog4Net()
        {
            var logCfg = new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "Log4net.config");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(logCfg);
        }
    }
}

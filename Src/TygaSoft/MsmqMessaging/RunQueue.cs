using System;
using System.Configuration;
using System.Messaging;
using TygaSoft.IMessaging;
using TygaSoft.Model;

namespace TygaSoft.MsmqMessaging
{
    public class RunQueue : TygaSoftQueue, IRunQueue
    {
        private static readonly string queuePath = ConfigurationManager.AppSettings["AfdRunQueue"];
        private static int queueTimeout = 20;

        public RunQueue()
            : base(queuePath, queueTimeout)
        {
            queue.Formatter = new BinaryMessageFormatter();
        }
    }
}

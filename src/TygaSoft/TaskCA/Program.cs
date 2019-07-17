using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Messaging;
using System.Text;
using TygaSoft.TaskProcessor;
using TygaSoft.Converter;

namespace TygaSoft.TaskCA
{
    class Program
    {
        static void Main(string[] args)
        {
            AsposeConvert.AsposeConvertTest();
            //OoxmlConvert.OoxmlConvertTest(); //OK

            //var service = new Service();
            //var result = service.GetContentDetailList(new ListModel { Keyword = "中央", ParentId= "83A1C060-5AA7-4BE3-BCE9-0B7CAEA099EB" });

            //Log.Info("日志测试2223333--");

            //Docx2Html.ConvertToHtml(@"D:\Download\系统统计功能.docx", "Docx to Html");

            //OnStart();

            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("终止服务请按任意键！");
            Console.ReadLine();
        }

        private static void OnStart()
        {
            string afdRunQueue = ConfigurationManager.AppSettings["AfdRunQueue"];
            if (!MessageQueue.Exists(afdRunQueue)) MessageQueue.Create(afdRunQueue, true);

            BaseTask.Run();
        }
    }
}

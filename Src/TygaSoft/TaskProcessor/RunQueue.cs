using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using Newtonsoft.Json.Linq;
using TygaSoft.BLL;
using TygaSoft.Model;
using TygaSoft.SysException;
using TygaSoft.SysHelper;

namespace TygaSoft.TaskProcessor
{
    public sealed class RunQueue : BaseTask, ITask
    {
        static readonly string BaiduMapAk = ConfigurationManager.AppSettings["BaiduMapAk"];
        static string IpUri = "http://api.map.baidu.com/location/ip?ip={0}&coor=bd09ll&ak=" + BaiduMapAk + "";
        static string GeocodingUri = "http://api.map.baidu.com/geocoder/v2/?coordtype=bd09ll&pois=0&location={0}&output=json&ak=" + BaiduMapAk + "";
        //static string LatlngUri = "http://api.map.baidu.com/geoconv/v1/?coords={0}&from=1&to=5&ak="+ BaiduMapAk + "";
        //static string PlaceUri = "http://api.map.baidu.com/place/v2/search?coord_type=1&page_size=1&location={0}&radius=500&output=json&ak=" + BaiduMapAk + "";

        public void TaskStart()
        {
            var workThread = new Thread(new ThreadStart(WorkProcess));
            workThread.IsBackground = true;
            workThread.SetApartmentState(ApartmentState.STA);
            workThread.Start();
        }

        public void WorkProcess()
        {
            try
            {
                TimeSpan tsTimeout = TimeSpan.FromSeconds(Convert.ToDouble(transactionTimeout * batchSize));

                //var bll = new RunQueueAsyn();

                while (true)
                {
                    TimeSpan datetimeStarting = new TimeSpan(DateTime.Now.Ticks);
                    double elapsedTime = 0;

                    //var list = new List<RunQueueInfo>();

                    for (int j = 0; j < batchSize; j++)
                    {
                        try
                        {
                            if ((elapsedTime + queueTimeout + transactionTimeout) < tsTimeout.TotalSeconds)
                            {
                                //list.Add(bll.ReceiveFromQueue(queueTimeout));
                            }
                            else
                            {
                                j = batchSize;
                            }

                            elapsedTime = new TimeSpan(DateTime.Now.Ticks).TotalSeconds - datetimeStarting.TotalSeconds;
                        }
                        catch (TimeoutException)
                        {
                            j = batchSize;
                        }
                    }

                    //if (list.Count > 0)
                    //{
                    //    foreach (var model in list)
                    //    {
                    //        if(model.RunType == EnumData.EnumRunQueue.BaiduMapRestApi.ToString())
                    //        {
                    //            DoBaiduMapRestApi(model);
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                new CustomException(string.Format("来自{0}异常：{1}", "AfdRunQueue", ex.Message), ex);
            }
        }
    }
}

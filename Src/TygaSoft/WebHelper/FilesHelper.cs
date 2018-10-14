using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.Web;
using SelectPdf;
using TygaSoft.Model;

namespace TygaSoft.WebHelper
{
    public class FilesHelper
    {
        public static readonly string FileRoot = ConfigurationManager.AppSettings["FilesRoot"];

        public static string GetFullPath(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath)) return string.Empty;
            if (!virtualPath.StartsWith("~")) virtualPath = "~" + virtualPath;
            return HttpContext.Current.Server.MapPath(virtualPath);
        }

        public static string GetRandomFolder(string key, DateTime currTime,bool isOldRemove)
        {
            var dir = string.Format("{0}/{1}/{2}/{3}", FileRoot, key, currTime.ToString("yyyyMM"), (new Random().NextDouble() * int.MaxValue).ToString().PadLeft(10, '0'));
            var fullPath = HttpContext.Current.Server.MapPath(dir);
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);

            return dir;
        }

        public static string GetFullPathByWcf(string virtualPath)
        {
            var fullPath = System.Web.Hosting.HostingEnvironment.MapPath(virtualPath);
            if (!Directory.Exists(Path.GetDirectoryName(fullPath))) Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            return fullPath;
        }

        public static string CreateDateTimeString()
        {
            //确保产生的字符串唯一性，使用线程休眠
            Thread.Sleep(2);
            Random random = new Random(); ;
            return DateTime.Now.ToString("yyyyMMddHHmmssffff", System.Globalization.DateTimeFormatInfo.InvariantInfo) + random.Next(0, 9999).ToString().PadLeft(4, '0');
        }

        public static string GetFormatDateTime()
        {
            Thread.Sleep(2);
            return DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TygaSoft.SysHelper
{
    public class Common
    {
        public static string GetFullPath(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath)) return string.Empty;
            return string.Format("{0}{1}", Directory.GetCurrentDirectory(), virtualPath.Trim('~').Replace("/", @"\"));
        }
    }
}

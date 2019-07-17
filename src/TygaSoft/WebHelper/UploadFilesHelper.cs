using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Web.Configuration;
using TygaSoft.SysHelper;

namespace TygaSoft.WebHelper
{
    public class UploadFilesHelper
    {
        static readonly string FilesRoot = WebConfigurationManager.AppSettings["FilesRoot"];

        /// <summary>
        /// 使用文件固定字节法验证文件是否合法
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileLen"></param>
        /// <returns></returns>
        public static bool IsFileValidated(Stream stream, int fileLen,string ext)
        {
            if (fileLen == 0) return false;

            byte[] imgArray = new byte[fileLen];
            stream.Read(imgArray, 0, fileLen);
            MemoryStream ms = new MemoryStream(imgArray);
            BinaryReader br = new BinaryReader(ms);
            string fileBuffer = "";
            byte buffer;
            try
            {
                buffer = br.ReadByte();
                fileBuffer = buffer.ToString();
                buffer = br.ReadByte();
                fileBuffer += buffer.ToString();
            }
            catch{
            }
            br.Close();
            ms.Close();

            var name = EnumHelper.GetName(typeof(EnumData.FileExtension), int.Parse(fileBuffer));
            return name !=null;
        }

        public bool IsImage(HttpPostedFile file)
        {
            string[] arr = { ".jpg", ".gif", ".bmp", ".png" };
            return arr.Contains(VirtualPathUtility.GetExtension(file.FileName));
        }

        /// <summary>
        /// 指定存储目录key，是否生成缩略图，上传文件
        /// 返回所有文件路径，如果是生成缩略图，则包含缩略图文件路径
        /// </summary>
        /// <param name="file"></param>
        /// <param name="key"></param>
        /// <param name="isCreateThumbnail"></param>
        /// <returns></returns>
        public string[] Upload(HttpPostedFile file, string key, bool isCreateThumbnail)
        {
            if (file == null || file.ContentLength == 0) throw new ArgumentException("没有获取到任何上传的文件", "file");
            int size = file.ContentLength;
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!IsFileValidated(file.InputStream, size, fileExtension)) throw new ArgumentException("上传文件不在规定的上传文件范围内");
            if (isCreateThumbnail)
            {
                if ((fileExtension != ".jpg") || (fileExtension != ".jpg"))
                {
                    throw new ArgumentException("创建缩略图只支持.jpg格式的文件，请检查");
                }
            }
            string dir = ConfigHelper.GetValueByKey(key);
            if (string.IsNullOrWhiteSpace(dir))
            {
                throw new ArgumentException("未找到" + key + "的相关配置，请检查", "key");
            }

            string paths = "";

            dir = VirtualPathUtility.AppendTrailingSlash(dir);
            string rndName = FilesHelper.GetFormatDateTime();
            string fName = rndName + fileExtension;
            string filePath = dir + rndName.Substring(0, 8) + "/";
            string fullPath = HttpContext.Current.Server.MapPath(filePath);
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
            file.SaveAs(fullPath + fName);

            paths += filePath + fName;
            if (isCreateThumbnail)
            {
                ImagesHelper ih = new ImagesHelper();
                string[] whBPicture = ConfigHelper.GetValueByKey("BPicture").Split(',');
                string[] whMPicture = ConfigHelper.GetValueByKey("MPicture").Split(',');
                string[] whSPicture = ConfigHelper.GetValueByKey("SPicture").Split(',');
                string bPicturePath = filePath + rndName + "_b" + fileExtension;
                string mPicturePath = filePath + rndName + "_m" + fileExtension;
                string sPicturePath = filePath + rndName + "_s" + fileExtension;
                ih.CreateThumbnailImage(fullPath + fName, HttpContext.Current.Server.MapPath(bPicturePath), int.Parse(whBPicture[0]), int.Parse(whBPicture[1]));
                ih.CreateThumbnailImage(fullPath + fName, HttpContext.Current.Server.MapPath(mPicturePath), int.Parse(whMPicture[0]), int.Parse(whMPicture[1]));
                ih.CreateThumbnailImage(fullPath + fName, HttpContext.Current.Server.MapPath(sPicturePath), int.Parse(whSPicture[0]), int.Parse(whSPicture[1]));
                paths += "," + bPicturePath;
                paths += "," + mPicturePath;
                paths += "," + sPicturePath;
            }
            else
            {
                paths += "," + filePath + fName;
                paths += "," + filePath + fName;
                paths += "," + filePath + fName;
            }

            return paths.Split(',');
        }

        /// <summary>
        /// 上传文件，并返回文件存储的虚拟路径
        /// </summary>
        /// <param name="file"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string UploadOriginalFile(HttpPostedFile file, string key, DateTime currTime)
        {
            var rndFolder = GetRndFolder(key, currTime);
            string fileName = string.Format("{0}{1}", rndFolder, VirtualPathUtility.GetExtension(file.FileName).ToLower());
            string saveVirtualDir = string.Format("{0}/{1}/{2}/{3}", FilesRoot, key, currTime.ToString("yyyyMM"), rndFolder);

            file.SaveAs(string.Format("{0}\\{1}", HttpContext.Current.Server.MapPath(saveVirtualDir), fileName));
            return string.Format("{0}/{1}", saveVirtualDir, fileName);
        }

        public static string ToFullPath(string virtualPath)
        {
            if (!virtualPath.StartsWith("~")) virtualPath = "~/" + virtualPath.Trim('/');
            return HttpContext.Current.Server.MapPath(virtualPath);
        }

        public static string ToVirtualPath(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath)) return string.Empty;
            return fullPath.Replace(HttpContext.Current.Server.MapPath("~"), "").Replace("\\", "/");
        }

        /// <summary>
        /// 获取唯一随机数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="currTime"></param>
        /// <returns></returns>
        public static string GetRandomFolder(string key,DateTime currTime)
        {
            string rndFolder = "";
            string fullPath = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}/{2}", FilesRoot, key, currTime.ToString("yyyyMM")));
            if (!Directory.Exists(fullPath))
            {
                rndFolder = string.Format("{0}{1}", currTime.ToString("dd"),"0001");
                fullPath = string.Format("{0}\\{1}",fullPath, rndFolder);
                Directory.CreateDirectory(fullPath);
            }
            else
            {
                rndFolder = string.Format("{0}{1}", currTime.ToString("dd"),(Directory.GetDirectories(fullPath).Length + 1).ToString().PadLeft(4, '0'));
                fullPath = string.Format("{0}\\{1}", fullPath, rndFolder);
                Directory.CreateDirectory(fullPath);
            }

            return rndFolder;
        }

        public string GetRndFolder(string key, DateTime currTime)
        {
            string rndFolder = (new Random().NextDouble() * int.MaxValue).ToString().PadLeft(10, '0');
            string fullPath = HttpContext.Current.Server.MapPath(string.Format("{0}/{1}/{2}", FilesRoot, key, currTime.ToString("yyyyMM")));
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);
            var folders = Directory.GetDirectories(fullPath);
            var q = folders.Where(m => m == rndFolder);
            if(q != null && q.Count() > 0)
            {
                rndFolder = string.Format("{0}{1}", rndFolder, q.Count());
            }
            fullPath = Path.Combine(fullPath, rndFolder);
            if (!Directory.Exists(fullPath)) Directory.CreateDirectory(fullPath);

            return rndFolder;
        }
    }
}

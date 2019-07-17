using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Transactions;
using System.Web.Configuration;
using TygaSoft.WebHelper;
using TygaSoft.BLL;
using TygaSoft.Model;
using TygaSoft.SysHelper;
using TygaSoft.SysException;
using TygaSoft.Converter;

namespace TygaSoft.Web.Handlers
{
    public class HandlerUpload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            try
            {
                string reqName = string.Empty;
                switch (context.Request.HttpMethod.ToUpper())
                {
                    case "GET":
                        reqName = context.Request.QueryString["ReqName"];
                        break;
                    case "POST":
                        reqName = context.Request.Form["ReqName"];
                        break;
                    default:
                        break;
                }
                if(string.IsNullOrEmpty(reqName))
                    throw new ArgumentException(MC.Request_Params_InvalidError);

                switch (reqName)
                {
                    case "UploadContentFile":
                        UploadContentFile(context);
                        break;
                    default:
                        throw new ArgumentException(MC.Request_Params_InvalidError);
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ResResult.ResJsonString(false, ex.Message, ""));
            }
        }

        private void UploadContentFile(HttpContext context)
        {
            #region 请求参数集

            var contentId = Guid.Empty;
            if (context.Request.Form["ContentId"] != null) Guid.TryParse(context.Request.Form["ContentId"], out contentId);
            if (contentId.Equals(Guid.Empty)) throw new ArgumentException(MC.Request_Params_InvalidError);
            var appCode = context.Request.Form["AppCode"];
            if(string.IsNullOrWhiteSpace(appCode)) throw new ArgumentException(MC.Request_Params_InvalidError);
            var userId = WebCommon.GetUserId();
            if (userId.Equals(Guid.Empty)) throw new ArgumentException(MC.L_InvalidError);
            var currTime = DateTime.Now;
            var effect = 0;

            #endregion

            HttpFileCollection files = context.Request.Files;
            if (files.Count > 0)
            {
                var ufh = new UploadFilesHelper();
                ImagesHelper ih = new ImagesHelper();
                foreach (string item in files.AllKeys)
                {
                    HttpPostedFile file = files[item];
                    if (file == null || file.ContentLength == 0) continue;
                    var ext = Path.GetExtension(file.FileName);

                    FileValidated(file);

                    string originalUrl = ufh.UploadOriginalFile(file, "ContentFiles", currTime);
                    var originalPath = UploadFilesHelper.ToFullPath(originalUrl);
                    var htmlFullPath = string.Empty;
                    if(ext == ".doc" || ext == ".docx")
                        htmlFullPath = OoxmlConvert.WordToHtml(originalPath, Path.GetFileNameWithoutExtension(file.FileName));
                    else if(ext == ".xls" || ext == ".xlsx")
                        htmlFullPath = AsposeConvert.ExcelToHtml(originalPath, Path.GetFileNameWithoutExtension(file.FileName));

                    var cfInfo = new ContentFileInfo(Guid.Empty, appCode.Trim(), userId,contentId, file.FileName,file.ContentLength,VirtualPathUtility.GetExtension(file.FileName).ToLower(), originalUrl.Trim('~'),UploadFilesHelper.ToVirtualPath(htmlFullPath), currTime);
                    var cfBll = new ContentFile();
                    effect += cfBll.Insert(cfInfo);

                    //if (ufh.IsImage(file))
                    //    CreateThumbnailImage(context, ih, context.Server.MapPath(originalUrl));

                }
            }

            if (effect > 0) context.Response.Write(ResResult.ResJsonString(true, MC.M_Save_Ok, null));
            else context.Response.Write(ResResult.ResJsonString(false, MC.M_Save_Error, ""));
        }

        #region 私有方法

        private void FileValidated(HttpPostedFile file)
        {
            int fileSize = file.ContentLength;
            int uploadFileSize = int.Parse(WebConfigurationManager.AppSettings["UploadFileSize"]);
            if (fileSize > uploadFileSize) throw new ArgumentException("文件【" + file.FileName + "】大小超出字节" + uploadFileSize + "，无法上传，请正确操作！");
            if (!UploadFilesHelper.IsFileValidated(file.InputStream, fileSize,VirtualPathUtility.GetExtension(file.FileName)))
            {
                new CustomException("上传了非法文件--" + file.FileName);
                throw new ArgumentException("文件【" + file.FileName + "】为受限制的文件，请正确操作！");
            }
        }

        private void CreateThumbnailImage(HttpContext context, ImagesHelper ih, string originalPath)
        {
            var ext = Path.GetExtension(originalPath);
            var rndFolder = Path.GetFileNameWithoutExtension(originalPath);
            string[] platformNames = Enum.GetNames(typeof(EnumData.Platform));
            var index = 0;
            foreach (string name in platformNames)
            {
                string sizeAppend = WebConfigurationManager.AppSettings[name];
                string[] sizeArr = sizeAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < sizeArr.Length; i++)
                {
                    string bmsPath = string.Format("{0}\\{1}_{2}{3}{4}", Path.GetDirectoryName(originalPath), rndFolder, index, i+1, ext);
                    string[] wh = sizeArr[i].Split('*');

                    ih.CreateThumbnailImage(originalPath, bmsPath, int.Parse(wh[0]), int.Parse(wh[1]), "DB", ext);
                }
                index++;
            }
        }

        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using Aspose.Cells;
using TygaSoft.SysHelper;
using TygaSoft.SysException;

namespace TygaSoft.Converter
{
    public class AsposeConvert
    {
        /// <summary>
        /// 测试入口
        /// </summary>
        public static void AsposeConvertTest()
        {
            var excelPath = @"D:\VSWorkspace\SoftCode\个人专用系统\源码\TygaSoft\TaskCA\TestFiles\ECM企业移动门户产品开发工作2017年下半年推进整体安排.xlsx";
            var htmlPath = ConvertFormat(excelPath, SaveFormat.Html, "ECM企业移动门户产品开发工作2017年下半年推进整体安排");
        }

        public static string ExcelToHtml(string sourcePath, string pageTitle)
        {
            return ConvertFormat(sourcePath, SaveFormat.Html, pageTitle);
        }

        private static string ConvertFormat(string sourcePath, SaveFormat saveFormat, string pageTitle)
        {
            var savePath = Path.Combine(Path.GetDirectoryName(sourcePath), string.Format("{0}.{1}", Path.GetFileNameWithoutExtension(sourcePath), saveFormat.ToString().ToLower()));
            byte[] byteSource = File.ReadAllBytes(sourcePath);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(byteSource, 0, byteSource.Length);
                Workbook wb = new Workbook(ms);
                wb.Save(savePath, saveFormat);

                if (saveFormat == SaveFormat.Html) ExcelToHtml(ms, savePath, pageTitle);
            }

            return savePath;
        }

        private static void ExcelToHtml(Stream stream, string htmlPath, string pageTitle)
        {
            var sheetNames = OoxmlHelper.GetSheetNames(stream);
            var toDir = htmlPath.Replace(".html", "_files");
            var toDirName = toDir.Replace(toDir.Substring(0, toDir.LastIndexOf("\\") + 1), "");

            var templatesDir = Path.GetDirectoryName(GlobalConfig.Templates_ExcelToHtml);
            var htmlEmptyText = File.ReadAllText(Path.Combine(templatesDir, "ExcelToHtmlEmpty.htm"));

            var sTemplate = File.ReadAllText(GlobalConfig.Templates_ExcelToHtml);
            sTemplate = sTemplate.Replace("{title}", pageTitle);
            sTemplate = sTemplate.Replace("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">", "");
            var xmlTemplate = XElement.Parse(sTemplate);
            var bodyNodes = xmlTemplate.Element("body").Elements();
            var tabs = bodyNodes.First(x => x.Attribute("class").Value == "easyui-tabs");
            var tab = tabs.Elements().First();
            tabs.Elements().Remove();

            var files = Directory.GetFiles(toDir);

            #region 逐个处理生成的文件

            foreach (var item in files)
            {
                var ext = Path.GetExtension(item);
                if (ext == ".htm")
                {
                    var name = Path.GetFileNameWithoutExtension(item);
                    if (name.StartsWith("sheet"))
                    {
                        var htmIndex = int.Parse(name.Replace("sheet", ""));
                        if (htmIndex > sheetNames.Length)
                        {
                            File.Delete(item);
                        }
                        else
                        {
                            var itemText = new StringBuilder();
                            using (StreamReader reader = new StreamReader(item))
                            {
                                var isStart = false;
                                var isEnd = false;
                                string strReadline;
                                while ((strReadline = reader.ReadLine()) != null)
                                {
                                    if (!isStart) isStart = strReadline.StartsWith("<body");
                                    if (isStart) isEnd = strReadline.StartsWith("</body>");
                                    if (isEnd) isStart = false;

                                    if (isStart && !isEnd)
                                    {
                                        if (strReadline.StartsWith("<body")) continue;
                                        if (strReadline.Contains("Aspose.Cells for .NET.Copyright")) continue;
                                        itemText.Append(strReadline);
                                    }
                                }
                            }
                            var htmlText = string.Format(htmlEmptyText, itemText.ToString());
                            File.WriteAllText(item, htmlText);
                            var sheetHtmName = Path.GetFileName(item);
                            tabs.Add(string.Format(tab.ToString(), sheetNames[htmIndex - 1], sheetHtmName));
                        }
                    }
                    else
                    {
                        File.Delete(item);
                    }
                }
                else if (ext == ".xml") File.Delete(item);
            }

            #endregion

            var text = string.Format("{0}{1}", "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">", xmlTemplate.ToString().Replace("&lt;", "<").Replace("&gt;", ">"));
            File.WriteAllText(htmlPath, text);

            files = Directory.GetFiles(toDir);
            foreach (var file in files)
            { 
                File.Move(file,file.Replace(toDirName+"\\",""));
            }
            Directory.Delete(toDir, true);
        }
    }
}

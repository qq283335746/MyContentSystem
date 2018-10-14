using System;
using System.IO;
using System.Xml.Linq;
using System.Drawing.Imaging;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;

namespace TygaSoft.Converter
{
    public class OoxmlConvert
    {
        /// <summary>
        /// 测试入口
        /// </summary>
        public static void OoxmlConvertTest()
        {
            var wordPath = @"D:\VSWorkspace\SoftCode\个人专用系统\源码\TygaSoft\TaskCA\TestFiles\系统统计功能.docx";
            var htmlPath = WordToHtml(wordPath, "系统统计功能");
        }

        public static string WordToHtml(string sourcePath, string pageTitle)
        {
            var dir = Path.GetDirectoryName(sourcePath);
            var htmlPath = Path.Combine(dir, string.Format("{0}{1}", Path.GetFileNameWithoutExtension(sourcePath), ".html"));
            byte[] byteSource = File.ReadAllBytes(sourcePath);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(byteSource, 0, byteSource.Length);
                WordToHtml(ms, htmlPath, pageTitle);
            }

            return htmlPath;
        }

        public static void WordToHtml(Stream stream, string htmlPath, string pageTitle)
        {
            using (WordprocessingDocument wDoc = WordprocessingDocument.Open(stream, true))
            {
                XElement html = HtmlConverter.ConvertToHtml(wDoc,Settings(htmlPath, pageTitle));
                File.WriteAllText(htmlPath, html.ToStringNewLineOnAttributes());
            }
        }

        public static HtmlConverterSettings Settings(string htmlPath, string pageTitle)
        {
            string imageDir = Path.Combine(Path.GetDirectoryName(htmlPath), "_files");
            var dirInfo = new DirectoryInfo(imageDir);
            if (dirInfo.Exists)
            {
                foreach (var f in dirInfo.GetFiles())
                    f.Delete();
                dirInfo.Delete();
            }
            var imageCounter = 0;
            HtmlConverterSettings settings = new HtmlConverterSettings()
            {
                AdditionalCss = "body { margin: 1cm auto; max-width: 20cm; padding: 0; }",
                PageTitle = pageTitle,
                FabricateCssClasses = true,
                CssClassPrefix = "pt-",
                RestrictToSupportedLanguages = false,
                RestrictToSupportedNumberingFormats = false,
                ImageHandler = imageInfo =>
                {
                    DirectoryInfo localDirInfo = new DirectoryInfo(imageDir);
                    if (!localDirInfo.Exists)
                        localDirInfo.Create();
                    ++imageCounter;
                    string extension = imageInfo.ContentType.Split('/')[1].ToLower();
                    ImageFormat imageFormat = null;
                    if (extension == "png")
                        imageFormat = ImageFormat.Png;
                    else if (extension == "gif")
                        imageFormat = ImageFormat.Gif;
                    else if (extension == "bmp")
                        imageFormat = ImageFormat.Bmp;
                    else if (extension == "jpeg")
                        imageFormat = ImageFormat.Jpeg;
                    else if (extension == "tiff")
                    {
                        // Convert tiff to gif.
                        extension = "gif";
                        imageFormat = ImageFormat.Gif;
                    }
                    else if (extension == "x-wmf")
                    {
                        extension = "wmf";
                        imageFormat = ImageFormat.Wmf;
                    }

                    if (imageFormat == null)
                        return null;

                    string imageFileName = imageDir + "/image" +
                        imageCounter.ToString() + "." + extension;
                    try
                    {
                        imageInfo.Bitmap.Save(imageFileName, imageFormat);
                    }
                    catch (System.Runtime.InteropServices.ExternalException)
                    {
                        return null;
                    }
                    string imageSource = localDirInfo.Name + "/image" +
                        imageCounter.ToString() + "." + extension;

                    XElement img = new XElement(Xhtml.img,
                        new XAttribute(NoNamespace.src, imageSource),
                        imageInfo.ImgStyleAttribute,
                        imageInfo.AltText != null ?
                            new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                    return img;
                }
            };

            return settings;
        }
    }
}

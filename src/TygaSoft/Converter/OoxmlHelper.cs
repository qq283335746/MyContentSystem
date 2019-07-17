using System.IO;
using System.Linq;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;

namespace TygaSoft.Converter
{
    public class OoxmlHelper
    {
        public static string[] GetSheetNames(Stream stream)
        {
            using (SpreadsheetDocument sDoc = SpreadsheetDocument.Open(stream, false))
            {
                var workbookXDoc = sDoc.WorkbookPart.GetXDocument();
                var sheetNames = workbookXDoc.Root.Elements(S.sheets).Elements(S.sheet).Attributes("name").Select(a => (string)a).ToArray();
                return sheetNames;
            }
        }
    }
}

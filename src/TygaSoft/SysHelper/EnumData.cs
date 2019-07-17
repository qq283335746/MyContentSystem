using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.SysHelper
{
    public class EnumData
    {
        public enum OperationAccess { 浏览 = 1, 新增 = 2, 编辑 = 3, 删除 = 4, 导入 = 5, 导出 = 6 };

        public enum MenuName { 首页, 禁止匿名访问, 匿名访问 };

        public enum Openness:byte { 完全公开};

        public enum ValidateAccess { IsView = 1, IsAdd = 2, IsEdit = 3, IsDelete = 4, IsImport = 5, IsExport = 6 };

        public enum ResCode { 成功 = 1000, 失败 = 1001 };

        public enum IsOk { 否, 是 };

        public enum IsDisable { 启用, 禁用 };

        public enum Status { 正常 };

        public enum Platform : byte { PC, Android, IOS }

        public enum RunQueue { BaiduMapRestApi }

        public enum FileExtension
        {
            jpg = 255216, gif = 7173, bmp = 6677, png = 13780, xls = 208207, xlsx = 8075, doc = 208207, docx = 8075, xml = 6033, pdf = 3780, txt = 210187
            // apk-8075,exe-7790,zip-8075,rar-8297,dll-7790,xml-6063,html-6033,aspx-239187,cs-117115,js-119105,txt-210187,sql-255254;
        }

    }
}

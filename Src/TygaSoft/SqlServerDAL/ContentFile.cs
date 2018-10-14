using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DBUtility;

namespace TygaSoft.SqlServerDAL
{
    public partial class ContentFile
    {
        #region IContentFile Member

        public IList<ContentFileInfo> GetListByJoin(string sqlWhere, params SqlParameter[] cmdParms)
        {
            var sb = new StringBuilder(500);
            sb.Append(@"select Id,AppCode,UserId,ContentId,FileName,FileSize,FileExt,FileUrl,ViewUrl,LastUpdatedDate
                        from ContentFile ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.Append("order by LastUpdatedDate desc ");

            var list = new List<ContentFileInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ContentFileInfo model = new ContentFileInfo();
                        model.Id = reader.IsDBNull(0) ? Guid.Empty : reader.GetGuid(0);
                        model.ContentId = reader.IsDBNull(3) ? Guid.Empty : reader.GetGuid(3);
                        model.FileName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        model.FileSize = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                        model.FileExt = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                        model.FileUrl = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                        model.ViewUrl = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        model.LastUpdatedDate = reader.IsDBNull(9) ? DateTime.Parse("1754-01-01") : reader.GetDateTime(9);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}

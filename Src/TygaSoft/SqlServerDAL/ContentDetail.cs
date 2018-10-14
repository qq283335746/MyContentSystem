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
    public partial class ContentDetail
    {
        #region IContentDetail Member

        public IList<ContentDetailInfo> GetListByContentType(int pageIndex, int pageSize, out int totalRecords, object contentTypeId, string sqlWhere, params SqlParameter[] cmdParms)
        {
            var sb = new StringBuilder(1000);
            sb.AppendFormat(@"select count(*) from ContentDetail p 
                        join
                        (
                            select c1.Id from ContentType c1 where CHARINDEX('{0}', c1.Step) > 0
                        )
                        c on c.Id = p.ContentTypeId
                      ", contentTypeId);
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ContentDetailInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.AppendFormat(@"select * from(select row_number() over(order by p.Sort) as RowNumber,
			          p.AppCode,p.Id,p.UserId,p.ContentTypeId,p.Title,p.Keyword,p.Descr,p.ContentText,p.Openness,p.Sort,p.RecordDate,p.LastUpdatedDate
                      from ContentDetail p
                        join
                        (
                          select c1.Id from ContentType c1 where CHARINDEX('{0}', c1.Step) > 0
                        )
                        c on c.Id = p.ContentTypeId
                      ", contentTypeId);

            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            var list = new List<ContentDetailInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var model = new ContentDetailInfo();
                        model.Id = reader.IsDBNull(2) ? Guid.Empty : reader.GetGuid(2);
                        model.ContentTypeId = reader.IsDBNull(4) ? Guid.Empty : reader.GetGuid(4);
                        model.Title = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        model.Keyword = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                        model.Descr = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                        model.ContentText = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        model.Openness = reader.IsDBNull(9) ? byte.MinValue : reader.GetByte(9);
                        model.Sort = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                        model.RecordDate = reader.IsDBNull(11) ? DateTime.Parse("1754-01-01") : reader.GetDateTime(11);
                        model.LastUpdatedDate = reader.IsDBNull(12) ? DateTime.Parse("1754-01-01") : reader.GetDateTime(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ContentDetailInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            var sb = new StringBuilder(500);
            sb.Append(@"select count(*) from ContentDetail p ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ContentDetailInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by p.Sort) as RowNumber,
			          p.AppCode,p.Id,p.UserId,p.ContentTypeId,p.Title,p.Keyword,p.Descr,p.ContentText,p.Openness,p.Sort,p.RecordDate,p.LastUpdatedDate
					  from ContentDetail p ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            var list = new List<ContentDetailInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var model = new ContentDetailInfo();
                        model.Id = reader.IsDBNull(2) ? Guid.Empty : reader.GetGuid(2);
                        model.ContentTypeId = reader.IsDBNull(4) ? Guid.Empty : reader.GetGuid(4);
                        model.Title = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        model.Keyword = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                        model.Descr = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                        model.ContentText = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        model.Openness = reader.IsDBNull(9) ? byte.MinValue : reader.GetByte(9);
                        model.Sort = reader.IsDBNull(10) ? 0 : reader.GetInt32(10);
                        model.RecordDate = reader.IsDBNull(11) ? DateTime.Parse("1754-01-01") : reader.GetDateTime(11);
                        model.LastUpdatedDate = reader.IsDBNull(12) ? DateTime.Parse("1754-01-01") : reader.GetDateTime(12);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}

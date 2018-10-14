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
    public partial class ContentDetail : IContentDetail
    {
        #region IContentDetail Member

        public int Insert(ContentDetailInfo model)
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"insert into ContentDetail (AppCode,UserId,ContentTypeId,Title,Keyword,Descr,ContentText,Openness,Sort,RecordDate,LastUpdatedDate)
			            values
						(@AppCode,@UserId,@ContentTypeId,@Title,@Keyword,@Descr,@ContentText,@Openness,@Sort,@RecordDate,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@AppCode",SqlDbType.Char,10),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@ContentTypeId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Title",SqlDbType.NVarChar,256),
new SqlParameter("@Keyword",SqlDbType.NVarChar,256),
new SqlParameter("@Descr",SqlDbType.NVarChar,300),
new SqlParameter("@ContentText",SqlDbType.NText,1073741823),
new SqlParameter("@Openness",SqlDbType.TinyInt),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@RecordDate",SqlDbType.DateTime),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.AppCode;
            parms[1].Value = model.UserId;
            parms[2].Value = model.ContentTypeId;
            parms[3].Value = model.Title;
            parms[4].Value = model.Keyword;
            parms[5].Value = model.Descr;
            parms[6].Value = model.ContentText;
            parms[7].Value = model.Openness;
            parms[8].Value = model.Sort;
            parms[9].Value = model.RecordDate;
            parms[10].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int InsertByOutput(ContentDetailInfo model)
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"insert into ContentDetail (Id,AppCode,UserId,ContentTypeId,Title,Keyword,Descr,ContentText,Openness,Sort,RecordDate,LastUpdatedDate)
			            values
						(@Id,@AppCode,@UserId,@ContentTypeId,@Title,@Keyword,@Descr,@ContentText,@Openness,@Sort,@RecordDate,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@AppCode",SqlDbType.Char,10),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@ContentTypeId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Title",SqlDbType.NVarChar,256),
new SqlParameter("@Keyword",SqlDbType.NVarChar,256),
new SqlParameter("@Descr",SqlDbType.NVarChar,300),
new SqlParameter("@ContentText",SqlDbType.NText,1073741823),
new SqlParameter("@Openness",SqlDbType.TinyInt),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@RecordDate",SqlDbType.DateTime),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.AppCode;
            parms[2].Value = model.UserId;
            parms[3].Value = model.ContentTypeId;
            parms[4].Value = model.Title;
            parms[5].Value = model.Keyword;
            parms[6].Value = model.Descr;
            parms[7].Value = model.ContentText;
            parms[8].Value = model.Openness;
            parms[9].Value = model.Sort;
            parms[10].Value = model.RecordDate;
            parms[11].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ContentDetailInfo model)
        {
            StringBuilder sb = new StringBuilder(500);
            sb.Append(@"update ContentDetail set AppCode = @AppCode,UserId = @UserId,ContentTypeId = @ContentTypeId,Title = @Title,Keyword = @Keyword,Descr = @Descr,ContentText = @ContentText,Openness = @Openness,Sort = @Sort,RecordDate = @RecordDate,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@AppCode",SqlDbType.Char,10),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@ContentTypeId",SqlDbType.UniqueIdentifier),
new SqlParameter("@Title",SqlDbType.NVarChar,256),
new SqlParameter("@Keyword",SqlDbType.NVarChar,256),
new SqlParameter("@Descr",SqlDbType.NVarChar,300),
new SqlParameter("@ContentText",SqlDbType.NText,1073741823),
new SqlParameter("@Openness",SqlDbType.TinyInt),
new SqlParameter("@Sort",SqlDbType.Int),
new SqlParameter("@RecordDate",SqlDbType.DateTime),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.AppCode;
            parms[2].Value = model.UserId;
            parms[3].Value = model.ContentTypeId;
            parms[4].Value = model.Title;
            parms[5].Value = model.Keyword;
            parms[6].Value = model.Descr;
            parms[7].Value = model.ContentText;
            parms[8].Value = model.Openness;
            parms[9].Value = model.Sort;
            parms[10].Value = model.RecordDate;
            parms[11].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(Guid id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ContentDetail where Id = @Id ");
            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = id;

            return SqlHelper.ExecuteNonQuery(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public bool DeleteBatch(IList<object> list)
        {
            StringBuilder sb = new StringBuilder(500);
            ParamsHelper parms = new ParamsHelper();
            int n = 0;
            foreach (string item in list)
            {
                n++;
                sb.Append(@"delete from ContentDetail where Id = @Id" + n + " ;");
                SqlParameter parm = new SqlParameter("@Id" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(item);
                parms.Add(parm);
            }

            return SqlHelper.ExecuteNonQuery(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), parms != null ? parms.ToArray() : null) > 0;
        }

        public ContentDetailInfo GetModel(Guid id)
        {
            ContentDetailInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 AppCode,Id,UserId,ContentTypeId,Title,Keyword,Descr,ContentText,Openness,Sort,RecordDate,LastUpdatedDate 
			            from ContentDetail
						where Id = @Id ");
            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier)
                                   };
            parms[0].Value = id;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), parms))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new ContentDetailInfo();
                        model.AppCode = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                        model.Id = reader.IsDBNull(1) ? Guid.Empty : reader.GetGuid(1);
                        model.UserId = reader.IsDBNull(2) ? Guid.Empty : reader.GetGuid(2);
                        model.ContentTypeId = reader.IsDBNull(3) ? Guid.Empty : reader.GetGuid(3);
                        model.Title = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        model.Keyword = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        model.Descr = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                        model.ContentText = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                        model.Openness = reader.IsDBNull(8) ? byte.MinValue : reader.GetByte(8);
                        model.Sort = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                        model.RecordDate = reader.IsDBNull(10) ? DateTime.Parse("1754-01-01") : reader.GetDateTime(10);
                        model.LastUpdatedDate = reader.IsDBNull(11) ? DateTime.Parse("1754-01-01") : reader.GetDateTime(11);
                    }
                }
            }

            return model;
        }

        public IList<ContentDetailInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(500);
            sb.Append(@"select count(*) from ContentDetail ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ContentDetailInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by Sort) as RowNumber,
			          AppCode,Id,UserId,ContentTypeId,Title,Keyword,Descr,ContentText,Openness,Sort,RecordDate,LastUpdatedDate
					  from ContentDetail ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ContentDetailInfo> list = new List<ContentDetailInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ContentDetailInfo model = new ContentDetailInfo();
                        model.AppCode = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        model.Id = reader.IsDBNull(2) ? Guid.Empty : reader.GetGuid(2);
                        model.UserId = reader.IsDBNull(3) ? Guid.Empty : reader.GetGuid(3);
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

        public IList<ContentDetailInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(500);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by Sort) as RowNumber,
			           AppCode,Id,UserId,ContentTypeId,Title,Keyword,Descr,ContentText,Openness,Sort,RecordDate,LastUpdatedDate
					   from ContentDetail ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ContentDetailInfo> list = new List<ContentDetailInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ContentDetailInfo model = new ContentDetailInfo();
                        model.AppCode = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        model.Id = reader.IsDBNull(2) ? Guid.Empty : reader.GetGuid(2);
                        model.UserId = reader.IsDBNull(3) ? Guid.Empty : reader.GetGuid(3);
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

        public IList<ContentDetailInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(500);
            sb.Append(@"select AppCode,Id,UserId,ContentTypeId,Title,Keyword,Descr,ContentText,Openness,Sort,RecordDate,LastUpdatedDate
                        from ContentDetail ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.Append("order by Sort ");

            IList<ContentDetailInfo> list = new List<ContentDetailInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ContentDetailInfo model = new ContentDetailInfo();
                        model.AppCode = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                        model.Id = reader.IsDBNull(1) ? Guid.Empty : reader.GetGuid(1);
                        model.UserId = reader.IsDBNull(2) ? Guid.Empty : reader.GetGuid(2);
                        model.ContentTypeId = reader.IsDBNull(3) ? Guid.Empty : reader.GetGuid(3);
                        model.Title = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        model.Keyword = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        model.Descr = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                        model.ContentText = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                        model.Openness = reader.IsDBNull(8) ? byte.MinValue : reader.GetByte(8);
                        model.Sort = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                        model.RecordDate = reader.IsDBNull(10) ? DateTime.Parse("1754-01-01") : reader.GetDateTime(10);
                        model.LastUpdatedDate = reader.IsDBNull(11) ? DateTime.Parse("1754-01-01") : reader.GetDateTime(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ContentDetailInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select AppCode,Id,UserId,ContentTypeId,Title,Keyword,Descr,ContentText,Openness,Sort,RecordDate,LastUpdatedDate 
			            from ContentDetail
					    order by Sort ");

            IList<ContentDetailInfo> list = new List<ContentDetailInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ContentDetailInfo model = new ContentDetailInfo();
                        model.AppCode = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                        model.Id = reader.IsDBNull(1) ? Guid.Empty : reader.GetGuid(1);
                        model.UserId = reader.IsDBNull(2) ? Guid.Empty : reader.GetGuid(2);
                        model.ContentTypeId = reader.IsDBNull(3) ? Guid.Empty : reader.GetGuid(3);
                        model.Title = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        model.Keyword = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        model.Descr = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                        model.ContentText = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                        model.Openness = reader.IsDBNull(8) ? byte.MinValue : reader.GetByte(8);
                        model.Sort = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                        model.RecordDate = reader.IsDBNull(10) ? DateTime.Parse("1754-01-01") : reader.GetDateTime(10);
                        model.LastUpdatedDate = reader.IsDBNull(11) ? DateTime.Parse("1754-01-01") : reader.GetDateTime(11);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        #endregion
    }
}

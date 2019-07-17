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
    public partial class ContentFile : IContentFile
    {
        #region IContentFile Member

        public int Insert(ContentFileInfo model)
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"insert into ContentFile (AppCode,UserId,ContentId,FileName,FileSize,FileExt,FileUrl,ViewUrl,LastUpdatedDate)
			            values
						(@AppCode,@UserId,@ContentId,@FileName,@FileSize,@FileExt,@FileUrl,@ViewUrl,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@AppCode",SqlDbType.Char,6),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@ContentId",SqlDbType.UniqueIdentifier),
new SqlParameter("@FileName",SqlDbType.NVarChar,256),
new SqlParameter("@FileSize",SqlDbType.Int),
new SqlParameter("@FileExt",SqlDbType.VarChar,50),
new SqlParameter("@FileUrl",SqlDbType.NVarChar,256),
new SqlParameter("@ViewUrl",SqlDbType.NVarChar,256),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.AppCode;
            parms[1].Value = model.UserId;
            parms[2].Value = model.ContentId;
            parms[3].Value = model.FileName;
            parms[4].Value = model.FileSize;
            parms[5].Value = model.FileExt;
            parms[6].Value = model.FileUrl;
            parms[7].Value = model.ViewUrl;
            parms[8].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int InsertByOutput(ContentFileInfo model)
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"insert into ContentFile (Id,AppCode,UserId,ContentId,FileName,FileSize,FileExt,FileUrl,ViewUrl,LastUpdatedDate)
			            values
						(@Id,@AppCode,@UserId,@ContentId,@FileName,@FileSize,@FileExt,@FileUrl,@ViewUrl,@LastUpdatedDate)
			            ");

            SqlParameter[] parms = {
                                       new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@AppCode",SqlDbType.Char,6),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@ContentId",SqlDbType.UniqueIdentifier),
new SqlParameter("@FileName",SqlDbType.NVarChar,256),
new SqlParameter("@FileSize",SqlDbType.Int),
new SqlParameter("@FileExt",SqlDbType.VarChar,50),
new SqlParameter("@FileUrl",SqlDbType.NVarChar,256),
new SqlParameter("@ViewUrl",SqlDbType.NVarChar,256),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.AppCode;
            parms[2].Value = model.UserId;
            parms[3].Value = model.ContentId;
            parms[4].Value = model.FileName;
            parms[5].Value = model.FileSize;
            parms[6].Value = model.FileExt;
            parms[7].Value = model.FileUrl;
            parms[8].Value = model.ViewUrl;
            parms[9].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Update(ContentFileInfo model)
        {
            StringBuilder sb = new StringBuilder(500);
            sb.Append(@"update ContentFile set AppCode = @AppCode,UserId = @UserId,ContentId = @ContentId,FileName = @FileName,FileSize = @FileSize,FileExt = @FileExt,FileUrl = @FileUrl,ViewUrl = @ViewUrl,LastUpdatedDate = @LastUpdatedDate 
			            where Id = @Id
					    ");

            SqlParameter[] parms = {
                                     new SqlParameter("@Id",SqlDbType.UniqueIdentifier),
new SqlParameter("@AppCode",SqlDbType.Char,6),
new SqlParameter("@UserId",SqlDbType.UniqueIdentifier),
new SqlParameter("@ContentId",SqlDbType.UniqueIdentifier),
new SqlParameter("@FileName",SqlDbType.NVarChar,256),
new SqlParameter("@FileSize",SqlDbType.Int),
new SqlParameter("@FileExt",SqlDbType.VarChar,50),
new SqlParameter("@FileUrl",SqlDbType.NVarChar,256),
new SqlParameter("@ViewUrl",SqlDbType.NVarChar,256),
new SqlParameter("@LastUpdatedDate",SqlDbType.DateTime)
                                   };
            parms[0].Value = model.Id;
            parms[1].Value = model.AppCode;
            parms[2].Value = model.UserId;
            parms[3].Value = model.ContentId;
            parms[4].Value = model.FileName;
            parms[5].Value = model.FileSize;
            parms[6].Value = model.FileExt;
            parms[7].Value = model.FileUrl;
            parms[8].Value = model.ViewUrl;
            parms[9].Value = model.LastUpdatedDate;

            return SqlHelper.ExecuteNonQuery(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), parms);
        }

        public int Delete(Guid id)
        {
            StringBuilder sb = new StringBuilder(250);
            sb.Append("delete from ContentFile where Id = @Id ");
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
                sb.Append(@"delete from ContentFile where Id = @Id" + n + " ;");
                SqlParameter parm = new SqlParameter("@Id" + n + "", SqlDbType.UniqueIdentifier);
                parm.Value = Guid.Parse(item);
                parms.Add(parm);
            }

            return SqlHelper.ExecuteNonQuery(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), parms != null ? parms.ToArray() : null) > 0;
        }

        public ContentFileInfo GetModel(Guid id)
        {
            ContentFileInfo model = null;

            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select top 1 Id,AppCode,UserId,ContentId,FileName,FileSize,FileExt,FileUrl,ViewUrl,LastUpdatedDate 
			            from ContentFile
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
                        model = new ContentFileInfo();
                        model.Id = reader.IsDBNull(0) ? Guid.Empty : reader.GetGuid(0);
                        model.AppCode = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        model.UserId = reader.IsDBNull(2) ? Guid.Empty : reader.GetGuid(2);
                        model.ContentId = reader.IsDBNull(3) ? Guid.Empty : reader.GetGuid(3);
                        model.FileName = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                        model.FileSize = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                        model.FileExt = reader.IsDBNull(6) ? string.Empty : reader.GetString(6);
                        model.FileUrl = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                        model.ViewUrl = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        model.LastUpdatedDate = reader.IsDBNull(9) ? DateTime.Parse("1754-01-01") : reader.GetDateTime(9);
                    }
                }
            }

            return model;
        }

        public IList<ContentFileInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(500);
            sb.Append(@"select count(*) from ContentFile ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            totalRecords = (int)SqlHelper.ExecuteScalar(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), cmdParms);

            if (totalRecords == 0) return new List<ContentFileInfo>();

            sb.Clear();
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			          Id,AppCode,UserId,ContentId,FileName,FileSize,FileExt,FileUrl,ViewUrl,LastUpdatedDate
					  from ContentFile ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ContentFileInfo> list = new List<ContentFileInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ContentFileInfo model = new ContentFileInfo();
                        model.Id = reader.IsDBNull(1) ? Guid.Empty : reader.GetGuid(1);
                        model.AppCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        model.UserId = reader.IsDBNull(3) ? Guid.Empty : reader.GetGuid(3);
                        model.ContentId = reader.IsDBNull(4) ? Guid.Empty : reader.GetGuid(4);
                        model.FileName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        model.FileSize = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                        model.FileExt = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                        model.FileUrl = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        model.ViewUrl = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                        model.LastUpdatedDate = reader.IsDBNull(10) ? DateTime.Parse("1754-01-01") : reader.GetDateTime(10);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ContentFileInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(500);
            int startIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;

            sb.Append(@"select * from(select row_number() over(order by LastUpdatedDate desc) as RowNumber,
			           Id,AppCode,UserId,ContentId,FileName,FileSize,FileExt,FileUrl,ViewUrl,LastUpdatedDate
					   from ContentFile ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.AppendFormat(@")as objTable where RowNumber between {0} and {1} ", startIndex, endIndex);

            IList<ContentFileInfo> list = new List<ContentFileInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ContentFileInfo model = new ContentFileInfo();
                        model.Id = reader.IsDBNull(1) ? Guid.Empty : reader.GetGuid(1);
                        model.AppCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                        model.UserId = reader.IsDBNull(3) ? Guid.Empty : reader.GetGuid(3);
                        model.ContentId = reader.IsDBNull(4) ? Guid.Empty : reader.GetGuid(4);
                        model.FileName = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);
                        model.FileSize = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                        model.FileExt = reader.IsDBNull(7) ? string.Empty : reader.GetString(7);
                        model.FileUrl = reader.IsDBNull(8) ? string.Empty : reader.GetString(8);
                        model.ViewUrl = reader.IsDBNull(9) ? string.Empty : reader.GetString(9);
                        model.LastUpdatedDate = reader.IsDBNull(10) ? DateTime.Parse("1754-01-01") : reader.GetDateTime(10);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public IList<ContentFileInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms)
        {
            StringBuilder sb = new StringBuilder(500);
            sb.Append(@"select Id,AppCode,UserId,ContentId,FileName,FileSize,FileExt,FileUrl,ViewUrl,LastUpdatedDate
                        from ContentFile ");
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);
            sb.Append("order by LastUpdatedDate desc ");

            IList<ContentFileInfo> list = new List<ContentFileInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ContentFileInfo model = new ContentFileInfo();
                        model.Id = reader.IsDBNull(0) ? Guid.Empty : reader.GetGuid(0);
                        model.AppCode = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        model.UserId = reader.IsDBNull(2) ? Guid.Empty : reader.GetGuid(2);
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

        public IList<ContentFileInfo> GetList()
        {
            StringBuilder sb = new StringBuilder(300);
            sb.Append(@"select Id,AppCode,UserId,ContentId,FileName,FileSize,FileExt,FileUrl,ViewUrl,LastUpdatedDate 
			            from ContentFile
					    order by LastUpdatedDate desc ");

            IList<ContentFileInfo> list = new List<ContentFileInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString()))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ContentFileInfo model = new ContentFileInfo();
                        model.Id = reader.IsDBNull(0) ? Guid.Empty : reader.GetGuid(0);
                        model.AppCode = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                        model.UserId = reader.IsDBNull(2) ? Guid.Empty : reader.GetGuid(2);
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

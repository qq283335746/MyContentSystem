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
    public partial class SiteRoles
    {
        #region ISiteRoles Member

        public SiteRolesInfo GetAspnetModel(string appName, string name)
        {
            SiteRolesInfo model = null;

            var sb = new StringBuilder(300);
            sb.Append(@"select top 1 ApplicationId,RoleId,RoleName,LoweredRoleName
			            from aspnet_Roles where ApplicationId = @ApplicationId and LoweredRoleName = @Named ");
            SqlParameter[] parms = {
                                     new SqlParameter("@ApplicationId",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@Named",SqlDbType.NVarChar,50)
                                   };
            parms[0].Value = new Applications().GetAspnetAppId(appName);
            parms[1].Value = name;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.AspnetDbConnString, CommandType.Text, sb.ToString(), parms))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new SiteRolesInfo();
                        model.ApplicationId = reader.GetGuid(0);
                        model.Id = reader.GetGuid(1);
                        model.Named = reader.GetString(2);
                        model.LowerName = reader.GetString(3);
                    }
                }
            }

            return model;
        }

        public List<SiteRolesInfo> GetAspnetList(string appName, string sqlWhere, params SqlParameter[] cmdParms)
        {
            var sb = new StringBuilder(500);
            sb.AppendFormat(@"select r.RoleId,r.RoleName from aspnet_Roles r
                       join aspnet_Applications a on a.ApplicationId = r.ApplicationId and a.LoweredApplicationName = '{0}'
                       ", appName.ToLower());
            if (!string.IsNullOrEmpty(sqlWhere)) sb.AppendFormat(" where 1=1 {0} ", sqlWhere);

            var list = new List<SiteRolesInfo>();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.AspnetDbConnString, CommandType.Text, sb.ToString(),cmdParms))
            {
                if (reader != null && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var model = new SiteRolesInfo();
                        model.Id = reader.GetGuid(0);
                        model.Named = reader.GetString(1);

                        list.Add(model);
                    }
                }
            }

            return list;
        }

        public SiteRolesInfo GetModel(string name)
        {
            SiteRolesInfo model = null;

            var sb = new StringBuilder(300);
            sb.Append(@"select top 1 ApplicationId,Id,Named,LowerName,LastUpdatedDate 
			            from SiteRoles
						where Named = @Named ");
            SqlParameter[] parms = {
                                     new SqlParameter("@Named",SqlDbType.NVarChar,50)
                                   };
            parms[0].Value = name;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.TygaSoftDbConnString, CommandType.Text, sb.ToString(), parms))
            {
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        model = new SiteRolesInfo();
                        model.ApplicationId = reader.GetGuid(0);
                        model.Id = reader.GetGuid(1);
                        model.Named = reader.GetString(2);
                        model.LowerName = reader.GetString(3);
                        model.LastUpdatedDate = reader.GetDateTime(4);
                    }
                }
            }

            return model;
        }

        public int UpdateAspnetRoles(SiteRolesInfo model)
        {
            string cmdText = @"update aspnet_Roles set RoleName = @RoleName,LoweredRoleName = Lower(@RoleName) where RoleId = @RoleId";

            SqlParameter[] parms = {
                                     new SqlParameter("@RoleId",SqlDbType.UniqueIdentifier),
                                     new SqlParameter("@RoleName",SqlDbType.NVarChar,256),
                                   };
            parms[0].Value = Guid.Parse(model.Id.ToString());
            parms[1].Value = model.Named;

            return SqlHelper.ExecuteNonQuery(SqlHelper.AspnetDbConnString, CommandType.Text, cmdText, parms);
        }

        #endregion
    }
}

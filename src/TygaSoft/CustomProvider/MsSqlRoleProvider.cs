using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Linq;
using System.Text;
using System.Web.Security;
using TygaSoft.BLL;
using TygaSoft.Model;

namespace TygaSoft.CustomProvider
{
    public class MsSqlRoleProvider : RoleProvider
    {
        #region 属性

        private string _AppName;
        public override string ApplicationName
        {
            get { return _AppName; }
            set
            {
                _AppName = value;
            }
        }

        #endregion

        #region 成员方法

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (String.IsNullOrEmpty(name))
                name = "SqlRoleProvider";
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", SM.GetString(SM.RoleSqlProvider_description));
            }
            base.Initialize(name, config);

            _AppName = config["applicationName"];
            if (string.IsNullOrEmpty(_AppName))
                _AppName = SC.GetDefaultAppName();

            if (_AppName.Length > 256)
            {
                throw new ProviderException(SM.GetString(SM.Provider_application_name_too_long));
            }

            config.Remove("connectionStringName");
            config.Remove("applicationName");
            config.Remove("commandTimeout");
            if (config.Count > 0)
            {
                string attribUnrecognized = config.GetKey(0);
                if (!String.IsNullOrEmpty(attribUnrecognized))
                    throw new ProviderException(SM.GetString(SM.Provider_unrecognized_attribute, attribUnrecognized));
            }
            var bll = new Applications();
            if (!(bll.GetApplicationId(_AppName) is Guid))
            {
                var appInfo = new ApplicationsInfo(Guid.Empty, "100000", _AppName, _AppName.ToLower(), "默认应用程序");
                bll.Insert(appInfo);
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            var uBll = new SiteUsers();
            var rBll = new SiteRoles();
            var urBll = new UsersInRoles();
            foreach(var uItem in usernames)
            {
                var uInfo = uBll.GetModel(uItem);
                foreach(var rItem in roleNames)
                {
                    var rInfo = rBll.GetModel(rItem);
                    var urInfo = new UsersInRolesInfo(uInfo.Id, rInfo.Id);
                    urBll.Insert(urInfo);
                }
            }
        }

        public override void CreateRole(string roleName)
        {
            var bll = new SiteRoles();
            var appId = Guid.Parse(new Applications().GetApplicationId(ApplicationName).ToString());
            var model = new SiteRolesInfo(appId, Guid.Empty, roleName, roleName.ToLower(), DateTime.Now);
            bll.Insert(model);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            var bll = new SiteRoles();
            var list = bll.GetList();
            return list.Select(m => m.Named).ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            var bll = new SiteRoles();
            var model = bll.GetModel(roleName);
            return model != null;
        }

        #endregion
    }
}

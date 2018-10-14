using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Security.Cryptography;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using TygaSoft.BLL;
using TygaSoft.Model;

namespace TygaSoft.CustomProvider
{
    public class MsSqlMembershipProvider : MembershipProvider
    {
        #region 属性

        private string _sqlConnectionString;
        private bool _EnablePasswordRetrieval;
        private bool _EnablePasswordReset;
        private bool _RequiresQuestionAndAnswer;
        private string _AppName;
        private bool _RequiresUniqueEmail;
        private int _MaxInvalidPasswordAttempts;
        private int _CommandTimeout;
        private int _PasswordAttemptWindow;
        private int _MinRequiredPasswordLength;
        private int _MinRequiredNonalphanumericCharacters;
        private string _PasswordStrengthRegularExpression;
        private int _SchemaVersionCheck;
        private MembershipPasswordFormat _PasswordFormat;

        public override string ApplicationName
        {
            get { return _AppName; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value");

                if (value.Length > 256)
                    throw new ProviderException(SM.GetString(SM.Provider_application_name_too_long));
                _AppName = value;
            }
        }
        public override bool EnablePasswordReset { get { return _EnablePasswordReset; } }
        public override bool EnablePasswordRetrieval { get { return _EnablePasswordRetrieval; } }
        public override int MaxInvalidPasswordAttempts { get { return _MaxInvalidPasswordAttempts; } }
        public override int MinRequiredNonAlphanumericCharacters { get { return _MinRequiredNonalphanumericCharacters; } }
        public override int MinRequiredPasswordLength { get { return _MinRequiredPasswordLength; } }
        public override int PasswordAttemptWindow { get { return _PasswordAttemptWindow; } }
        public override MembershipPasswordFormat PasswordFormat { get { return _PasswordFormat; } }
        public override string PasswordStrengthRegularExpression { get { return _PasswordStrengthRegularExpression; } }
        public override bool RequiresQuestionAndAnswer { get { return _RequiresQuestionAndAnswer; } }
        public override bool RequiresUniqueEmail { get { return _RequiresUniqueEmail; } }

        #endregion

        #region 成员方法

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");
            if (String.IsNullOrEmpty(name))
                name = "SqlMembershipProvider";
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", SM.GetString(SM.MembershipSqlProvider_description));
            }
            base.Initialize(name, config);

            _SchemaVersionCheck = 0;

            _EnablePasswordRetrieval = SC.GetBooleanValue(config, "enablePasswordRetrieval", false);
            _EnablePasswordReset = SC.GetBooleanValue(config, "enablePasswordReset", true);
            _RequiresQuestionAndAnswer = SC.GetBooleanValue(config, "requiresQuestionAndAnswer", true);
            _RequiresUniqueEmail = SC.GetBooleanValue(config, "requiresUniqueEmail", true);
            _MaxInvalidPasswordAttempts = SC.GetIntValue(config, "maxInvalidPasswordAttempts", 5, false, 0);
            _PasswordAttemptWindow = SC.GetIntValue(config, "passwordAttemptWindow", 10, false, 0);
            _MinRequiredPasswordLength = SC.GetIntValue(config, "minRequiredPasswordLength", 7, false, 128);
            _MinRequiredNonalphanumericCharacters = SC.GetIntValue(config, "minRequiredNonalphanumericCharacters", 1, true, 128);

            _PasswordStrengthRegularExpression = config["passwordStrengthRegularExpression"];
            if (_PasswordStrengthRegularExpression != null)
            {
                _PasswordStrengthRegularExpression = _PasswordStrengthRegularExpression.Trim();
                if (_PasswordStrengthRegularExpression.Length != 0)
                {
                    try
                    {
                        Regex regex = new Regex(_PasswordStrengthRegularExpression);
                    }
                    catch (ArgumentException e)
                    {
                        throw new ProviderException(e.Message, e);
                    }
                }
            }
            else
            {
                _PasswordStrengthRegularExpression = string.Empty;
            }
            if (_MinRequiredNonalphanumericCharacters > _MinRequiredPasswordLength)
                throw new HttpException(SM.GetString(SM.MinRequiredNonalphanumericCharacters_can_not_be_more_than_MinRequiredPasswordLength));

            _CommandTimeout = SC.GetIntValue(config, "commandTimeout", 30, true, 0);
            _AppName = config["applicationName"];
            if (string.IsNullOrEmpty(_AppName))
                _AppName = SC.GetDefaultAppName();

            if (_AppName.Length > 256)
            {
                throw new ProviderException(SM.GetString(SM.Provider_application_name_too_long));
            }

            string strTemp = config["passwordFormat"];
            if (strTemp == null) strTemp = "Hashed";

            switch (strTemp)
            {
                case "Clear":
                    _PasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                case "Encrypted":
                    _PasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Hashed":
                    _PasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                default:
                    throw new ProviderException(SM.GetString(SM.Provider_bad_password_format));
            }

            if (PasswordFormat == MembershipPasswordFormat.Hashed && EnablePasswordRetrieval)
                throw new ProviderException(SM.GetString(SM.Provider_can_not_retrieve_hashed_password));
            //if (_PasswordFormat == MembershipPasswordFormat.Encrypted && MachineKeySection.IsDecryptionKeyAutogenerated)
            //    throw new ProviderException(SecurityMessage.GetString(SecurityMessage.Can_not_use_encrypted_passwords_with_autogen_keys));

            config.Remove("connectionStringName");
            config.Remove("enablePasswordRetrieval");
            config.Remove("enablePasswordReset");
            config.Remove("requiresQuestionAndAnswer");
            config.Remove("applicationName");
            config.Remove("requiresUniqueEmail");
            config.Remove("maxInvalidPasswordAttempts");
            config.Remove("passwordAttemptWindow");
            config.Remove("commandTimeout");
            config.Remove("passwordFormat");
            config.Remove("name");
            config.Remove("minRequiredPasswordLength");
            config.Remove("minRequiredNonalphanumericCharacters");
            config.Remove("passwordStrengthRegularExpression");
            if (config.Count > 0)
            {
                string attribUnrecognized = config.GetKey(0);
                if (!String.IsNullOrEmpty(attribUnrecognized))
                    throw new ProviderException(SM.GetString(SM.Provider_unrecognized_attribute, attribUnrecognized));
            }
        }

        public override bool ValidateUser(string username, string password)
        {
            var bll = new SiteUsers();
            var model = bll.GetModelByJoin(username,null);
            if (model == null) return false;
            return EncodePassword(password, model.PasswordFormat, model.PasswordSalt) == model.Password;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            if (passwordQuestion == null) passwordQuestion = "";
            if (passwordAnswer == null) passwordAnswer = "";
            var bll = new SiteUsers();
            var oldInfo = bll.GetModelByJoin(username, providerUserKey);
            if (oldInfo != null)
            {
                status = MembershipCreateStatus.Success;
                return new MembershipUser(this.Name, oldInfo.Named, oldInfo.Id, oldInfo.Email, oldInfo.PasswordQuestion, oldInfo.Comment, oldInfo.IsApproved, oldInfo.IsLockedOut, oldInfo.CreateDate, oldInfo.LastLoginDate, oldInfo.LastActivityDate, oldInfo.LastPasswordChangedDate, oldInfo.LastLockoutDate);
            }
            var appId = Guid.Parse(new Applications().GetAspnetAppId(ApplicationName).ToString());
            var currTime = DateTime.Now;
            var mBll = new SiteMembers();
            var userId = Guid.NewGuid();
            var uInfo = new SiteUsersInfo(appId, userId, "",username,username.ToLower(),"",false, currTime, currTime);
            var salt = GenerateSalt();
            var mInfo = new SiteMembersInfo(appId, userId, EncodePassword(password, (int)PasswordFormat, salt), (int)PasswordFormat, salt, "", email, email.ToLower(), passwordQuestion, passwordAnswer, isApproved, false, currTime, currTime, currTime, currTime, 0, currTime,0, currTime, "");
            bll.InsertByOutput(uInfo);
            mBll.Insert(mInfo);
            oldInfo = bll.GetModelByJoin(username, providerUserKey);
            status = MembershipCreateStatus.Success;
            return new MembershipUser(this.Name, oldInfo.Named, oldInfo.Id, oldInfo.Email, oldInfo.PasswordQuestion, oldInfo.Comment, oldInfo.IsApproved, oldInfo.IsLockedOut, oldInfo.CreateDate, oldInfo.LastLoginDate, oldInfo.LastActivityDate, oldInfo.LastPasswordChangedDate, oldInfo.LastLockoutDate);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            if (string.IsNullOrEmpty(username))
            {
                username = HttpContext.Current.User.Identity.Name;
            }
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }
            var bll = new SiteUsers();
            var model = bll.GetModelByJoin(username,null);
            if (model == null) return null;
            return new MembershipUser(this.Name,model.Named,model.Id,model.Email,model.PasswordQuestion,model.Comment,model.IsApproved,model.IsLockedOut,model.CreateDate,model.LastLoginDate,model.LastActivityDate,model.LastPasswordChangedDate,model.LastLockoutDate);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            if (providerUserKey == null)
            {
                throw new ArgumentNullException("providerUserKey");
            }
            if (!(providerUserKey is Guid))
            {
                throw new ArgumentException(SM.GetString(SM.Membership_InvalidProviderUserKey), "providerUserKey");
            }
            var bll = new SiteUsers();
            var model = bll.GetModelByJoin(null, providerUserKey);
            if (model == null) return null;
            return new MembershipUser(this.Name, model.Named, model.Id, model.Email, model.PasswordQuestion, model.Comment, model.IsApproved, model.IsLockedOut, model.CreateDate, model.LastLoginDate, model.LastActivityDate, model.LastPasswordChangedDate, model.LastLockoutDate);
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            if (pageIndex < 1) pageIndex = 1;
            MembershipUserCollection list = new MembershipUserCollection();
            var bll = new SiteUsers();
            var ulist = bll.GetListByJoin(pageIndex, pageSize, out totalRecords, "", null);
            foreach(var item in ulist)
            {
                list.Add(new MembershipUser(this.Name, item.Named, item.Id, item.Email, item.PasswordQuestion, item.Comment, item.IsApproved, item.IsLockedOut, item.CreateDate, item.LastLoginDate, item.LastActivityDate, item.LastPasswordChangedDate, item.LastLockoutDate));
            }
            return list;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            //var userInfo = GetModel(username);
            //if (userInfo == null) throw new ArgumentException(MC.GetString(MC.Request_NotExist, username));
            //var rndPsw = new Random().Next(100000, 999999).ToString();
            //var salt = EncryptHelper.GenerateSalt();
            //userInfo.PasswordFormat = 1;
            //userInfo.Password = EncryptHelper.EncodePassword(rndPsw, userInfo.PasswordFormat, salt);
            //userInfo.PasswordSalt = salt;
            //if (dal.Update(userInfo) > 0) return rndPsw;
            //else throw new ArgumentException(MC.M_Save_Error);
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            //var userInfo = GetModel(username);
            //if (userInfo == null) throw new ArgumentException(MC.GetString(MC.Request_NotExist, username));
            //if (!ValidateUser(username, oldPassword)) throw new ArgumentException(MC.Login_InvalidAccount);
            //if (!Regex.IsMatch(newPassword, MC.PasswordStrengthRegularExpression)) throw new ArgumentException(MC.Login_InvalidPassword);
            //var salt = EncryptHelper.GenerateSalt();
            //userInfo.PasswordFormat = 1;
            //userInfo.Password = EncryptHelper.EncodePassword(newPassword, userInfo.PasswordFormat, salt);
            //userInfo.PasswordSalt = salt;
            //return dal.Update(userInfo) > 0;
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 自定义

        internal string GenerateSalt()
        {
            byte[] buf = new byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }

        internal string EncodePassword(string pass, int passwordFormat, string salt)
        {
            if (passwordFormat == 0) // MembershipPasswordFormat.Clear
                return pass;

            byte[] bIn = Encoding.Unicode.GetBytes(pass);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bAll = new byte[bSalt.Length + bIn.Length];
            byte[] bRet = null;

            Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
            Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
            if (passwordFormat == 1)
            {
                // MembershipPasswordFormat.Hashed
                HashAlgorithm s = HashAlgorithm.Create(Membership.HashAlgorithmType);
                bRet = s.ComputeHash(bAll);
            }
            else
            {
                bRet = EncryptPassword(bAll);
            }

            return Convert.ToBase64String(bRet);
        }

        internal string UnEncodePassword(string pass, int passwordFormat)
        {
            switch (passwordFormat)
            {
                case 0: // MembershipPasswordFormat.Clear:
                    return pass;
                case 1: // MembershipPasswordFormat.Hashed:
                    throw new ProviderException(SM.GetString(SM.Provider_can_not_decode_hashed_password));
                default:
                    byte[] bIn = Convert.FromBase64String(pass);
                    byte[] bRet = DecryptPassword(bIn);
                    if (bRet == null)
                        return null;
                    return Encoding.Unicode.GetString(bRet, 16, bRet.Length - 16);
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.CustomProvider
{
    internal static class SM
    {
        internal static string GetString(string strString)
        {
            return strString;
        }
        internal static string GetString(string strString, string param1)
        {
            return string.Format(strString, param1);
        }

        internal static string GetString(string strString, string param1, string param2)
        {
            return string.Format(strString, param1, param2);
        }
        internal static string GetString(string strString, string param1, string param2, string param3)
        {
            return string.Format(strString, param1, param2, param3);
        }

        internal const string Provider_can_not_decode_hashed_password = "当前密码为不可逆加密类型，无法解密！";
        internal const string Membership_InvalidProviderUserKey = "参数值不正确！";
        internal const string RoleSqlProvider_description = "SQL role provider.";
        internal const string Provider_application_name_too_long = "应用程序名称长度超过了指定值，请检查！";
        internal const string Provider_unrecognized_attribute = "配置节有错误！";

        internal const string MembershipSqlProvider_description = "SQL membership provider.";
        internal const string Provider_can_not_retrieve_hashed_password = "Configured settings are invalid: Hashed passwords cannot be retrieved. Either set the password format to different type, or set supportsPasswordRetrieval to false.";
        internal const string Provider_bad_password_format = "Web.config错误：Password format specified is invalid.";

        internal const string Value_must_be_boolean = "The value must be boolean (true or false) for property '{0}'.";
        internal const string Value_must_be_non_negative_integer = "The value must be a non-negative 32-bit integer for property '{0}'.";
        internal const string Value_must_be_positive_integer = "The value must be a positive 32-bit integer for property '{0}'.";
        internal const string Value_too_big = "The value '{0}' can not be greater than '{1}'.";
        internal const string MinRequiredNonalphanumericCharacters_can_not_be_more_than_MinRequiredPasswordLength = "The minRequiredNonalphanumericCharacters can not be greater than minRequiredPasswordLength.";
    }
}

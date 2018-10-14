using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace TygaSoft.CustomProvider
{
    internal static class SC
    {
        internal static string GetDefaultAppName()
        {
            try
            {
                string appName = HostingEnvironment.ApplicationVirtualPath;
                if (String.IsNullOrEmpty(appName))
                {
                    appName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;

                    int indexOfDot = appName.IndexOf('.');
                    if (indexOfDot != -1)
                    {
                        appName = appName.Remove(indexOfDot);
                    }
                }

                if (String.IsNullOrEmpty(appName))
                {
                    return "/";
                }
                else
                {
                    return appName;
                }
            }
            catch
            {
                return "/";
            }
        }

        internal static bool GetBooleanValue(NameValueCollection config, string valueName, bool defaultValue)
        {
            string sValue = config[valueName];
            if (sValue == null)
            {
                return defaultValue;
            }

            bool result;
            if (bool.TryParse(sValue, out result))
            {
                return result;
            }
            else
            {
                throw new ProviderException(SM.GetString(SM.Value_must_be_boolean, valueName));
            }
        }

        internal static int GetIntValue(NameValueCollection config, string valueName, int defaultValue, bool zeroAllowed, int maxValueAllowed)
        {
            string sValue = config[valueName];

            if (sValue == null)
            {
                return defaultValue;
            }

            int iValue;
            if (!Int32.TryParse(sValue, out iValue))
            {
                if (zeroAllowed)
                {
                    throw new ProviderException(SM.GetString(SM.Value_must_be_non_negative_integer, valueName));
                }

                throw new ProviderException(SM.GetString(SM.Value_must_be_positive_integer, valueName));
            }

            if (zeroAllowed && iValue < 0)
            {
                throw new ProviderException(SM.GetString(SM.Value_must_be_non_negative_integer, valueName));
            }

            if (!zeroAllowed && iValue <= 0)
            {
                throw new ProviderException(SM.GetString(SM.Value_must_be_positive_integer, valueName));
            }

            if (maxValueAllowed > 0 && iValue > maxValueAllowed)
            {
                throw new ProviderException(SM.GetString(SM.Value_too_big, valueName, maxValueAllowed.ToString(CultureInfo.InvariantCulture)));
            }

            return iValue;
        }
    }
}

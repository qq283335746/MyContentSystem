using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TygaSoft.SysException;
using TygaSoft.SysHelper;
using TygaSoft.BLL;
using TygaSoft.Model;
using TygaSoft.WcfModel;
using TygaSoft.WebHelper;

namespace TygaSoft.WcfService
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PdaService : IPda
    {
        public string GetHelloWord()
        {
            return ResResult.ResJsonString(true, "", "Hello Word");
        }

        public string ValidateUser(string username,string password)
        {
            try
            {
                return ResResult.ResJsonString(Membership.ValidateUser(username,password), "", "");
            }
            catch(Exception ex)
            {
                return ResResult.ResJsonString(false, ex.Message, "");
            }
        }
    }
}

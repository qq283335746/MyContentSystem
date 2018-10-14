using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TygaSoft.WebHelper;

namespace TygaSoft.Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SysException.Log.Info("日志测试44449999994--");
            if (!Page.IsPostBack)
            {
                if(ConfigHelper.GetValueByKey("RunMode") == "saas")
                {

                }
            }
        }
    }
}
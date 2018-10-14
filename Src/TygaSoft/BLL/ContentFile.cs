using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DALFactory;
using TygaSoft.SysHelper;

namespace TygaSoft.BLL
{
    public partial class ContentFile
    {
        #region ContentFile Member

        public IList<ContentFileInfo> GetListByJoin(string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetListByJoin(sqlWhere, cmdParms);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IContentFile
    {
        #region IContentFile Member

        IList<ContentFileInfo> GetListByJoin(string sqlWhere, params SqlParameter[] cmdParms);

        #endregion
    }
}

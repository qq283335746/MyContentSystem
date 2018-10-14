using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IContentDetail
    {
        #region IContentDetail Member

        IList<ContentDetailInfo> GetListByContentType(int pageIndex, int pageSize, out int totalRecords, object contentTypeId, string sqlWhere, params SqlParameter[] cmdParms);

        IList<ContentDetailInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms);

        #endregion
    }
}

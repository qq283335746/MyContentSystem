using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.IDAL;
using TygaSoft.Model;
using TygaSoft.DALFactory;

namespace TygaSoft.BLL
{
    public partial class ContentDetail
    {
        #region ContentDetail Member

        public IList<ContentDetailInfo> GetListByContentType(int pageIndex, int pageSize, out int totalRecords, object contentTypeId, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetListByContentType(pageIndex,pageSize,out totalRecords,contentTypeId,sqlWhere,cmdParms);
        }

        public IList<ContentDetailInfo> GetListByJoin(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms)
        {
            return dal.GetListByJoin(pageIndex, pageSize, out totalRecords, sqlWhere, cmdParms);
        }
        
        #endregion
    }
}

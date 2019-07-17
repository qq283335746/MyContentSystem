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

        int Insert(ContentDetailInfo model);

        int InsertByOutput(ContentDetailInfo model);

        int Update(ContentDetailInfo model);

        int Delete(Guid id);

        bool DeleteBatch(IList<object> list);

        ContentDetailInfo GetModel(Guid id);

        IList<ContentDetailInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms);

        IList<ContentDetailInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms);

        IList<ContentDetailInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms);

        IList<ContentDetailInfo> GetList();

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TygaSoft.Model;

namespace TygaSoft.IDAL
{
    public partial interface IContentType
    {
        #region IContentType Member

        int Insert(ContentTypeInfo model);

        int InsertByOutput(ContentTypeInfo model);

        int Update(ContentTypeInfo model);

        int Delete(string appCode, Guid id);

        bool DeleteBatch(IList<object> list);

        ContentTypeInfo GetModel(string appCode, Guid id);

        IList<ContentTypeInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms);

        IList<ContentTypeInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms);

        IList<ContentTypeInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms);

        IList<ContentTypeInfo> GetList();

        #endregion
    }
}

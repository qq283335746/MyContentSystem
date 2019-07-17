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

        int Insert(ContentFileInfo model);

        int InsertByOutput(ContentFileInfo model);

        int Update(ContentFileInfo model);

        int Delete(Guid id);

        bool DeleteBatch(IList<object> list);

        ContentFileInfo GetModel(Guid id);

        IList<ContentFileInfo> GetList(int pageIndex, int pageSize, out int totalRecords, string sqlWhere, params SqlParameter[] cmdParms);

        IList<ContentFileInfo> GetList(int pageIndex, int pageSize, string sqlWhere, params SqlParameter[] cmdParms);

        IList<ContentFileInfo> GetList(string sqlWhere, params SqlParameter[] cmdParms);

        IList<ContentFileInfo> GetList();

        #endregion
    }
}

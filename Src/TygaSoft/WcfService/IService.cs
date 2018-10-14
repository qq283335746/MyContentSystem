using System;
using System.ServiceModel;
using TygaSoft.WcfModel;

namespace TygaSoft.WcfService
{
    [ServiceContract(Namespace = "http://TygaSoft.Services.AfdService")]
    public interface IService
    {
        #region 基础数据

        #region 内容类别

        [OperationContract(Name = "GetContentTypeTree")]
        ResResultModel GetContentTypeTree();

        [OperationContract(Name = "SaveContentType")]
        ResResultModel SaveContentType(ContentTypeFmModel model);

        [OperationContract(Name = "DeleteContentType")]
        ResResultModel DeleteContentType(string appCode, Guid Id);

        #endregion

        #region 内容明细

        [OperationContract(Name = "GetContentDetailList")]
        ResResultModel GetContentDetailList(ListModel model);

        [OperationContract(Name = "SaveContentDetail")]
        ResResultModel SaveContentDetail(ContentDetailFmModel model);

        [OperationContract(Name = "DeleteContentDetail")]
        ResResultModel DeleteContentDetail(string itemAppend);

        #endregion

        #region 内容文件

        [OperationContract(Name = "GetContentFilesByContentId")]
        ResResultModel GetContentFilesByContentId(object contentId);

        [OperationContract(Name = "DeleteContentFile")]
        ResResultModel DeleteContentFile(object Id);

        #endregion

        #endregion

        #region 系统管理

        [OperationContract(Name = "GetFeatureUserInfo")]
        ResResultModel GetFeatureUserInfo(string username, string typeName);

        [OperationContract(Name = "SaveFeatureUser")]
        ResResultModel SaveFeatureUser(FeatureUserFmModel model);

        #endregion

        #region 图片、文件管理

        [OperationContract(Name = "GetSitePictureList")]
        ResResultModel GetSitePictureList(ListModel model);

        [OperationContract(Name = "DeleteSitePicture")]
        ResResultModel DeleteSitePicture(string itemAppend);

        #endregion

    }
}

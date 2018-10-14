using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TygaSoft.SysException;
using TygaSoft.SysHelper;
using TygaSoft.DBUtility;
using TygaSoft.Model;
using TygaSoft.WcfModel;
using TygaSoft.BLL;
using TygaSoft.WebHelper;
using TygaSoft.CustomProvider;

namespace TygaSoft.WcfService
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service: IService
    {
        #region 基础数据

        #region 内容类别

        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public ResResultModel GetContentTypeTree()
        {
            try
            {
                var bll = new ContentType();
                return ResResult.Response(true, "", bll.GetTreeJson());
            }
            catch (Exception ex)
            {
                return ResResult.Response(false, ex.Message, null);
            }
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public ResResultModel SaveContentType(ContentTypeFmModel model)
        {
            try
            {
                if (model == null) return ResResult.Response(false, MC.Request_Params_InvalidError, null);
                if (string.IsNullOrWhiteSpace(model.Named) || string.IsNullOrWhiteSpace(model.AppCode)) return ResResult.Response(false, MC.Request_Params_InvalidError, null);
                var Id = Guid.Empty;
                var parentId = Guid.Empty;
                if (model.Id != null && !string.IsNullOrWhiteSpace(model.Id.ToString())) Guid.TryParse(model.Id.ToString(), out Id);
                if (model.ParentId != null && !string.IsNullOrWhiteSpace(model.ParentId.ToString())) Guid.TryParse(model.ParentId.ToString(), out parentId);
                var openness = (byte)EnumData.Openness.完全公开;
                var currTime = DateTime.Now;
                var bll = new ContentType();
                int effect = 0;

                if (bll.IsExistCode(model.Coded, Id))
                {
                    return ResResult.Response(false, MC.GetString(MC.Params_CodeExistError, model.Coded), Id);
                }

                var modelInfo = new ContentTypeInfo(model.AppCode, Id, WebCommon.GetUserId(), model.Coded, model.Named, parentId, model.Step.Trim(','),model.FlagName, openness, model.Sort, model.Remark, currTime, currTime);
                if (modelInfo.Id.Equals(Guid.Empty))
                {
                    MenusDataProxy.ValidateAccess((int)EnumData.OperationAccess.新增, true);
                    modelInfo.Id = Guid.NewGuid();
                    modelInfo.Step = modelInfo.Id.ToString() + "," + modelInfo.Step;
                    effect = bll.InsertByOutput(modelInfo);
                }
                else
                {
                    MenusDataProxy.ValidateAccess((int)EnumData.OperationAccess.编辑, true);
                    effect = bll.Update(modelInfo);
                }
                if (effect < 1) return ResResult.Response(false, MC.M_Save_Error, null);

                return ResResult.Response(true, MC.M_Save_Ok, modelInfo.Id);
            }
            catch (Exception ex)
            {
                return ResResult.Response(false, ex.Message, null);
            }
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public ResResultModel DeleteContentType(string appCode, Guid Id)
        {
            try
            {
                if (Id.Equals(Guid.Empty))
                {
                    return ResResult.Response(false, MC.Request_Params_InvalidError, null);
                }

                var bll = new ContentType();
                if (bll.IsExistChild(Id)) return ResResult.Response(false, MC.M_DeleteTreeNodeError, null);

                MenusDataProxy.ValidateAccess((int)EnumData.OperationAccess.删除, true);
                return ResResult.Response(bll.Delete(appCode,Id) > 0, "", null);
            }
            catch (Exception ex)
            {
                return ResResult.Response(false, "操作异常：" + ex.Message + "", null);
            }
        }

        #endregion

        #region 内容明细

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public ResResultModel GetContentDetailList(ListModel model)
        {
            try
            {
                if (model.PageIndex < 1) model.PageIndex = 1;
                if (model.PageSize < 1) model.PageSize = 10;
                int totalRecord = 0;
                var bll = new ContentDetail();

                IList<ContentDetailInfo> list = null;

                StringBuilder sqlWhere = null;
                ParamsHelper parms = null;
                if (!string.IsNullOrWhiteSpace(model.Keyword))
                {
                    sqlWhere = new StringBuilder(1000);
                    parms = new ParamsHelper();
                    sqlWhere.Append("and (p.Title+p.Keyword+p.Descr) like @Keyword ");
                    parms.Add(new SqlParameter("@Keyword", "%" + model.Keyword + "%"));
                }
                var contentTypeId = Guid.Empty;
                if (model.ParentId != null && Guid.TryParse(model.ParentId.ToString(), out contentTypeId))
                {
                    list = bll.GetListByContentType(model.PageIndex, model.PageSize, out totalRecord, contentTypeId, sqlWhere == null ? null : sqlWhere.ToString(), parms == null ? null : parms.ToArray());
                }
                else
                {
                    list = bll.GetListByJoin(model.PageIndex, model.PageSize, out totalRecord, sqlWhere == null ? null : sqlWhere.ToString(), parms == null ? null : parms.ToArray());
                }

                return ResResult.Response(true, "", "{\"total\":" + totalRecord + ",\"rows\":" + JsonConvert.SerializeObject(list) + "}");
            }
            catch (Exception ex)
            {
                return ResResult.Response(false, ex.Message, "");
            }
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public ResResultModel SaveContentDetail(ContentDetailFmModel model)
        {
            try
            {
                if (model == null) return ResResult.Response(false, MC.Request_Params_InvalidError, null);
                if (string.IsNullOrWhiteSpace(model.AppCode) || string.IsNullOrWhiteSpace(model.Title)) return ResResult.Response(false, MC.Request_Params_InvalidError, null);
                Guid Id = Guid.Empty;
                if (model.Id != null) Guid.TryParse(model.Id.ToString(), out Id);
                Guid contentTypeId = Guid.Empty;
                if (model.ContentTypeId != null) Guid.TryParse(model.ContentTypeId.ToString(), out contentTypeId);
                var userId = WebCommon.GetUserId();
                var currTime = DateTime.Now;
                var modelInfo = new ContentDetailInfo(model.AppCode, Id, userId, contentTypeId, model.Title,model.Keyword,model.Descr,model.ContentText,model.Openness,model.Sort,currTime,currTime);

                var bll = new ContentDetail();
                int effect = -1;

                if (Id.Equals(Guid.Empty))
                {
                    modelInfo.Id = Guid.NewGuid();
                    effect = bll.InsertByOutput(modelInfo);
                }
                else
                {
                    effect = bll.Update(modelInfo);
                }
                if (effect < 1) return ResResult.Response(false, MC.M_Save_Error, "");

                return ResResult.Response(true, "", modelInfo.Id);
            }
            catch (Exception ex)
            {
                return ResResult.Response(false, ex.Message, "");
            }
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public ResResultModel DeleteContentDetail(string itemAppend)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(itemAppend)) return ResResult.Response(false, MC.Request_Params_InvalidError, "");
                var items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var bll = new ContentDetail();

                if (bll.DeleteBatch((IList<object>)items.ToList<object>()))
                {
                    foreach(var item in items)
                    {
                        DeleteContentFile(item);
                    }
                }

                return ResResult.Response(true, MC.M_Save_Ok, "");
            }
            catch (Exception ex)
            {
                return ResResult.Response(false, ex.Message, "");
            }
        }

        #endregion

        #region 内容文件

        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public ResResultModel GetContentFilesByContentId(object contentId)
        {
            try
            {
                var sContentId = Guid.Empty;
                if (contentId != null) Guid.TryParse(contentId.ToString(), out sContentId);
                if(sContentId.Equals(Guid.Empty)) return ResResult.Response(false, MC.Request_Params_InvalidError, null);
                var bll = new ContentFile();

                var list = bll.GetListByJoin(string.Format("and ContentId = '{0}'",contentId), null);

                return ResResult.Response(true, "", "{\"total\":" + list.Count + ",\"rows\":" + JsonConvert.SerializeObject(list) + "}");
            }
            catch (Exception ex)
            {
                return ResResult.Response(false, ex.Message, "");
            }
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public ResResultModel DeleteContentFile(object Id)
        {
            try
            {
                var id = Guid.Empty;
                if (Id != null) Guid.TryParse(Id.ToString(), out id);
                if(id.Equals(Guid.Empty)) return ResResult.Response(false, MC.Request_Params_InvalidError, "");

                var bll = new ContentFile();
                var model = bll.GetModel(id);
                if(model != null)
                {
                    if(bll.Delete(id) > 0)
                    {
                        var dir = Path.GetDirectoryName(FilesHelper.GetFullPath(model.FileUrl));
                        Task.Factory.StartNew(() =>
                        {
                            Directory.Delete(dir, true);
                        });
                    }
                }

                return ResResult.Response(true, MC.M_Save_Ok, "");
            }
            catch (Exception ex)
            {
                return ResResult.Response(false, ex.Message, "");
            }
        }

        #endregion

        #endregion

        #region 系统管理

        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public ResResultModel GetFeatureUserInfo(string username,string typeName)
        {
            try
            {
                var bll = new FeatureUser();
                var fuInfo = bll.GetModel(SecurityService.GetUserId(username), typeName);
                if (fuInfo == null) fuInfo = new FeatureUserInfo();
                return ResResult.Response(true, "", JsonConvert.SerializeObject(fuInfo));
            }
            catch(Exception ex)
            {
                return ResResult.Response(false, ex.Message, "");
            }
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public ResResultModel SaveFeatureUser(FeatureUserFmModel model)
        {
            try
            {
                var featureId = Guid.Empty;
                if (!string.IsNullOrWhiteSpace(model.FeatureId)) Guid.TryParse(model.FeatureId, out featureId);
                var userId = SecurityService.GetUserId(model.UserName);

                var fuBll = new FeatureUser();
                fuBll.DoFeatureUser(userId, featureId, model.TypeName, true);

                return ResResult.Response(true, "", "");
            }
            catch (Exception ex)
            {
                return ResResult.Response(false, ex.Message, "");
            }
        }

        #endregion

        #region 图片、文件管理

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public ResResultModel GetSitePictureList(ListModel model)
        {
            try
            {
                if (model.PageIndex < 1) model.PageIndex = 1;
                if (model.PageSize < 1) model.PageSize = 10;
                int totalRecord = 0;
                var bll = new SitePicture();

                var list = bll.GetCbbList(model.PageIndex, model.PageSize, out totalRecord, model.Keyword);
                return ResResult.Response(true, "", "{\"total\":" + totalRecord + ",\"rows\":" + JsonConvert.SerializeObject(list) + "}");
            }
            catch (Exception ex)
            {
                return ResResult.Response(false, ex.Message, "");
            }
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public ResResultModel DeleteSitePicture(string itemAppend)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(itemAppend)) return ResResult.Response(false, MC.Request_Params_InvalidError, "");
                var items = itemAppend.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var bll = new SitePicture();
                if (!bll.DeleteBatch((IList<object>)items.ToList<object>()))
                {
                    return ResResult.Response(false, MC.M_Save_Error, "");
                }

                return ResResult.Response(true, MC.M_Save_Ok, "");
            }
            catch (Exception ex)
            {
                return ResResult.Response(false, ex.Message, "");
            }
        }

        #endregion
    }
}

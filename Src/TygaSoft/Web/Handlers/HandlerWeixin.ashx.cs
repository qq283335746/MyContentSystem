using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Web;
using Newtonsoft.Json.Linq;
using TygaSoft.SysException;
using TygaSoft.SysHelper;

namespace TygaSoft.Web.Handlers
{
    /// <summary>
    /// 微信公众号首页
    /// </summary>
    public class HandlerWeixin : IHttpHandler
    {
        HttpContext context;
        public void ProcessRequest(HttpContext context)
        {
            this.context = context;
            context.Response.ContentType = "application/json; charset=utf-8";
            try
            {
                new CustomException("context.Request.Url--" + context.Request.Url);

                var sHttpMethod = context.Request.HttpMethod.ToUpper();

                string reqName = "";
                switch (sHttpMethod)
                {
                    case "GET":
                        reqName = context.Request.QueryString["ReqName"];
                        break;
                    case "POST":
                        reqName = context.Request.Form["ReqName"];
                        break;
                    default:
                        break;
                }
                if (!string.IsNullOrEmpty(reqName))
                {
                    switch (reqName)
                    {
                        case "GetUserInfoByOAuth":
                            GetUserInfoByOAuth(context.Request.QueryString["Code"]);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    var signature = context.Request.QueryString["signature"];
                    var timestamp = context.Request.QueryString["timestamp"];
                    var nonce = context.Request.QueryString["nonce"];

                    if (!ValidateFromWeixin(signature, timestamp, nonce)) return;

                    if (sHttpMethod == "GET")
                    {
                        var echostr = context.Request.QueryString["echostr"];
                        context.Response.Write(echostr);
                    }
                    else
                    {
                        string postString = string.Empty;
                        using (var sr = new StreamReader(context.Request.InputStream))
                        {
                            postString = sr.ReadToEnd();
                        }

                        if (!string.IsNullOrEmpty(postString))
                        {
                            new CustomException("HandlerWeixin--postString--" + postString);
                            SendMessage(postString);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new CustomException("HandlerWeixin--ProcessRequest--异常--" + ex.Message);
                context.Response.Write(ResResult.ResJsonString(false, ex.Message, ""));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region 消息管理

        /// <summary>
        /// 响应请求的信息
        /// </summary>
        /// <param name="xml">来自微信的请求XML数据包</param>
        private void SendMessage(string xml)
        {
            var xels = XElement.Parse(xml);
            string sMsgType = xels.Element("MsgType").Value;

            switch (sMsgType.Trim())
            {
                case "text":
                    break;
                case "event":
                    break;
                case "voice":
                    break;
                case "location":
                    break;
                default:
                    break;
            }
            string sToUserName = xels.Element("ToUserName").Value;
            string sFromUserName = xels.Element("FromUserName").Value;
            var msgXml = SendWelcomeMessage(sToUserName, sFromUserName);
            context.Response.ContentType = "text/xml";
            context.Response.ContentEncoding = _Encoding;
            context.Response.Write(msgXml);
            context.Response.End();
        }

        /// <summary>
        /// 发送关注时的消息
        /// </summary>
        private string SendWelcomeMessage(string fromUser,string toUser)
        {
            var xml = CreateMessageTemplate("news");
            xml = xml.Replace("{ToUserName}", toUser);
            xml = xml.Replace("{FromUserName}", fromUser);
            xml = xml.Replace("{CreateTime}", GetTimestamp());
            xml = xml.Replace("{ArticleCount}", "1");
            xml = xml.Replace("{Title}", "您好，欢迎您来到我们的地盘");
            xml = xml.Replace("{Description}", "请点击链接获取详细信息");
            xml = xml.Replace("{PicUrl}", "");
            xml = xml.Replace("{Url}", _WebIndexUrl);
            return xml;
        }

        /// <summary>
        /// 以消息类型创建消息模板
        /// </summary>
        /// <param name="msgType"></param>
        /// <returns></returns>
        private string CreateMessageTemplate(string msgType)
        {
            var xml = string.Empty;
            switch (msgType)
            {
                case "news":
                    xml = @"<xml>
                            <ToUserName><![CDATA[{ToUserName}]]></ToUserName>
                            <FromUserName><![CDATA[{FromUserName}]]></FromUserName>
                            <CreateTime>{CreateTime}</CreateTime>
                            <MsgType><![CDATA[news]]></MsgType>
                            <ArticleCount>{ArticleCount}</ArticleCount>
                            <Articles>
                            <item>
                            <Title><![CDATA[{Title}]]></Title> 
                            <Description><![CDATA[{Description}]]></Description>
                            <PicUrl><![CDATA[{PicUrl}]]></PicUrl>
                            <Url><![CDATA[{Url}]]></Url>
                            </item>
                            </Articles>
                            </xml>";
                    break;
                default:
                    break;
            }
            return xml;
        }

        #endregion

        /// <summary>
        /// 微信网页授权，OAuth2授权
        /// </summary>
        /// <param name="code"></param>
        private void GetUserInfoByOAuth(string code)
        {
            if(string.IsNullOrEmpty(code))
            {
                context.Response.Write(ResResult.ResJsonString(true, null, string.Format(_OAuth2Url, _AppID, _WebIndexUrl)));
                return;
            }
            var accessTokenResult = HttpHelper.DoGet(string.Format(_AccessTokenByCodeApi, _AppID, _AppSecret, code.Trim()));
            new CustomException("HandlerWeixin--GetUserInfoByOAuth--accessTokenResult--" + accessTokenResult);
            JObject jAccessTokenInfo = JObject.Parse(accessTokenResult);
            var sAccessToken = jAccessTokenInfo["access_token"].ToString();
            var sRefreshToken = jAccessTokenInfo["refresh_token"].ToString();
            var sOpenId = jAccessTokenInfo["openid"].ToString();
            var sScope = jAccessTokenInfo["scope"].ToString();

            var refreshTokenResult = HttpHelper.DoGet(string.Format(_RefreshTokenApi,_AppID, sRefreshToken));
            new CustomException("HandlerWeixin--GetUserInfoByOAuth--refreshTokenResult--" + refreshTokenResult);
            JObject jRefreshTokenInfo = JObject.Parse(refreshTokenResult);
            sAccessToken = jRefreshTokenInfo["access_token"].ToString();
            sRefreshToken = jRefreshTokenInfo["refresh_token"].ToString();
            sOpenId = jRefreshTokenInfo["openid"].ToString();
            sScope = jRefreshTokenInfo["scope"].ToString();

            WeixinUserInfo userInfo = null;
            if (sScope.Contains("snsapi_userinfo"))
            {
                var userInfoResult = HttpHelper.DoGet(string.Format(_SnsapiUserinfoApi, sAccessToken, sOpenId));
                new CustomException("HandlerWeixin--GetUserInfoByOAuth--userInfoResult--" + userInfoResult);
                JObject jUserInfo = JObject.Parse(userInfoResult);
                var openid = jUserInfo["openid"].ToString();
                var nickname = jUserInfo["nickname"].ToString();
                var sex = jUserInfo["sex"].ToString();
                var province = jUserInfo["province"].ToString();
                var city = jUserInfo["city"].ToString();
                var country = jUserInfo["country"].ToString();
                var headimgurl = jUserInfo["headimgurl"].ToString();
                var unionid = jUserInfo["unionid"] != null ? jUserInfo["unionid"].ToString() : "";
                userInfo = new WeixinUserInfo(openid, nickname, sex, province, city, country, headimgurl, unionid);
            }
            
            context.Response.Write(ResResult.ResJsonString(true, null, userInfo));
        }

        /// <summary>
        /// 校验请求是否来自微信
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        private bool ValidateFromWeixin(string signature, string timestamp, string nonce)
        {
            if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(timestamp) || string.IsNullOrEmpty(nonce)) return false;

            string[] keys = { _DevServerToken, timestamp, nonce };
            Array.Sort(keys);
            string signStr = string.Join("", keys);
            return signature == SignHash(signStr);
        }

        private string GetTimestamp()
        {
            return DateTime.Now.Subtract(new DateTime(1970, 1, 1, 8, 0, 0)).TotalSeconds.ToString();
        }

        private string SignHash(string inputString)
        {
            byte[] bIn = null;
            byte[] bRet = null;

            bIn = _Encoding.GetBytes(inputString);
            HashAlgorithm hashAlgo = SHA1.Create();
            bRet = hashAlgo.ComputeHash(bIn);

            return BitConverter.ToString(bRet).Replace("-", "").ToLower();
        }

        private Encoding _Encoding = Encoding.UTF8;

        private const string _AppID = "wx2e9ec5a6cd7db968";
        private const string _AppSecret = "8e8479806400e83c792f27796c5961ed";
        private const string _DevServerUrl = "http://express.tygaweb.com/me/h/weixin.html";
        private const string _DevServerToken = "TygaYesoft";
        private const string _DevServerEncodingAESKey = "BPrqmlTvsKewAJnoZgNl6frfZ0UAx6YB2qRxBr1tS0S";

        private const string _WebIndexUrl = "http://express.tygaweb.com/me/m/wx.html";
        private const string _OAuth2Url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=wx#wechat_redirect";
        private const string _AccessTokenByCodeApi = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
        private const string _RefreshTokenApi = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}";
        private const string _SnsapiUserinfoApi = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";
    }

    public class WeixinUserInfo
    {
        public WeixinUserInfo() { }
        public WeixinUserInfo(string openid,string nickname,string sex,string province,string city,string country,string headimgurl,string unionid)
        {
            this.Openid = openid;
            this.Nickname = nickname;
            this.Sex = sex;
            this.Province = province;
            this.City = city;
            this.Country = country;
            this.Headimgurl = headimgurl;
            this.Unionid = unionid;
        }

        public string Openid { get; set; }
        public string Nickname { get; set; }
        public string Sex { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Headimgurl { get; set; }
        public string Unionid { get; set; }
    }
}
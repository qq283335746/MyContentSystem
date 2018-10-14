var Weixin = {
    Init: function () {
        Weixin.ValidateOpenId();
    },
    ValidateOpenId: function () {
        var sOpenId = Common.GetCookie("OpenId");
        if (!sOpenId) {
            var sCode = Common.GetQueryString("code");
            if (!sCode) sCode = "";
            var url = Common.AppName + '/h/weixin.html';
            var data = { ReqName: 'GetUserInfoByOAuth', Code: sCode };
            Common.DoGet(url, data, true, true, function (result) {
                if (typeof (result) == "string") {
                    if (result.indexOf("https://open.weixin.qq.com") > -1) {
                        window.location = result;
                    }
                }
                else {
                    Common.SetCookie("OpenId", result.Openid, { expires: 30 });
                }
            })
        }
    },
    OnShowOpenId: function () {
        alert('Weixin--OnShowOpenId--' + Common.GetCookie("OpenId"));
        return false;
    }
}
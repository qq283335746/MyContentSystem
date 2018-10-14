<%@ Page Title="微信公众号" Language="C#" MasterPageFile="~/Masters/Shares.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="TygaSoft.Web.Shares.Weixin.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="server">

    <div style="height:40px;line-height:40px;">
        <a onclick="Weixin.OnShowOpenId()">OnShowOpenId</a>
    </div>

    <script src="/Me/Scripts/Shares/Weixin.js"></script>
    <script type="text/javascript">
        $(function () {
            Weixin.Init();
        })
    </script>

</asp:Content>

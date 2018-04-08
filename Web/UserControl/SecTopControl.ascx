<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SecTopControl.ascx.cs" Inherits="UserControl_SecTopControl" %>

<!--页头 start-->
    <div class="plat-header">
        <div class="logo fl">
            <a href='<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>'>
                <img src="../images/logo2.0.png" height="58"></a></div>
        <ul class="menu fl">
            <li><a href='<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>'>首页</a></li>
            <li><a href="/compnew.html">招商信息</a></li>
            <li><a href="/comphot.html">厂家列表</a></li>
            <li><a href="/platform.html">平台介绍</a></li>
            <li><a href="/news_1.html">新闻资讯</a></li>
			<li><a href="/financial.html">金融服务</a></li>
            <li><a href="/help/help.html">帮助中心</a></li></ul>
        <div class="p-app fr">
            <span>
                <img src="images/app-qr.png" width="60"></span><i>下载医站通</i></div>
    </div>
<!--页头 end-->
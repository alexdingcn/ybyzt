<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SSOLogin.aspx.cs" Inherits="SSOLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title><%= ConfigurationManager.AppSettings["TitleName"].ToString() %></title>
    <link href="Company/css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery.js"></script>
    <script language="javascript">
        $(function () {
            $('.error').css({ 'position': 'absolute', 'left': ($(window).width() - 490) / 2 });
            $(window).resize(function () {
                $('.error').css({ 'position': 'absolute', 'left': ($(window).width() - 490) / 2 });
            })
        }); 

    </script>
</head>
<body style="background: #edf6fa;">
    <form id="form1" runat="server">
    </form>
    <%--type='xxxxx'
        username='xxxxx'
        password='xxxxx'
        logo='xxxxx'*/
        user="{ 'type':'xxxxx','username':'xxxxx','password':'xxxxx','logo':'xxxxx'}";
        sign=""; 加密Key--%>

    <div class="place" style='display:none;'>
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="#">404错误提示</a></li>
        </ul>
    </div>
    <div class="error">
        <h2 id="pError" runat="server">
        </h2>
        <p style='display:none;'>
            看到这个提示，就自认倒霉吧!</p>
        <div class="reindex">
            <a href="javascript:history.go(-1);" target="_parent">返回</a></div>
    </div>
</body>
</html>

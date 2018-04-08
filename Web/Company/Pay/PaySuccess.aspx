<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaySuccess.aspx.cs" Inherits="Distributor_Pay_PaySuccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>支付成功</title>
    <link href="../../Distributor/css/pay.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        

        function myfunction(wait) {
            if (wait > 0) {
                $("#lblmessage").html("页面将在<i class='red'  style='font-size:20px;'>" + wait + "</i>秒内自动关闭！");
                wait--;
                setTimeout(function () { myfunction(wait) }, 1000);
            } else {
                //window.location.href = " ../OrderInfo.aspx?KeyID=<%= Common.DesEncrypt(KeyID.ToString(),Common.EncryptKey) %>";
                window.location.href = "../jsc.aspx";
            }
            window.top.opener.readyPay();
            "<%= IsRef %>" == "Y" ? window.opener.window.opener.window.history.go(0) : window.opener.window.history.go(0);
        }
    </script>
    <script type="text/javascript">        $(function () {
            if ($(window).innerWidth() <= 1024) { $("body").append('<link href="<%= ResolveUrl("../css/root.css")%>" rel="stylesheet" type="text/css" />'); }
        })
    </script>
</head>
<body onload="myfunction(5)" class="root3">
    <form id="form1" runat="server">
    <!--header start-->
    <div class="header">
        <div class="con">
            <div class="logo">
                <a href="">
                    <img src="../../images/logo.png" height="37" /></a><i>支付平台</i></div>
            <div class="topMenu">
                <a href="../../index.aspx">医站通首页</a>|<a href="../OrderList.aspx">我的订单</a> &nbsp;<i style=" font-weight:bold; color:red;">服务热线：400-8859-319</i></div>
        </div>
    </div>
    <!--header end-->
    <div class="blank20">
    </div>
    <div class="payOpt" id="checkId">
        <div class="payOk">
            <img src="../../Distributor/images/ok.png" /><%=str %><i class="red"><span id="PayedAmount" runat="server"></span></i>元</div>
        <div class="payLink">
           <%=url %></div>
        <div class="payLink"  style="padding-top:5px;">
            <%=lastmesage %></div>
        <div class="blank20">
        </div>
    </div>
    <!--快捷支付弹窗 end-->
    <div class="blank20">
    </div>
    <!--常见问题 start-->
    <div class="question">
       <%-- <div class="title">
            常见问题</div>
        <dl class="list">
            <dt>1、经常需要动态给Div添加或者删除样式，利用jquery的addClass(id)与delete</dt>
            <dd>
                在开发网页过程中常需要对A链接使用JS制作一个点击弹窗,那么在点击该A链接就不能发生跳转行为,于是我们取消A链接的跳转</dd>
            <dt>1、经常需要动态给Div添加或者删除样式，利用jquery的addClass(id)与delete</dt>
            <dd>
                在开发网页过程中常需要对A链接使用JS制作一个点击弹窗,那么在点击该A链接就不能发生跳转行为,于是我们取消A链接的跳转</dd>
            <dt>1、经常需要动态给Div添加或者删除样式，利用jquery的addClass(id)与delete</dt>
            <dd>
                在开发网页过程中常需要对A链接使用JS制作一个点击弹窗,那么在点击该A链接就不能发生跳转行为,于是我们取消A链接的跳转</dd>
        </dl>--%>
    </div>
    <!--常见问题 end-->
    <div class="footer">
      <%= ConfigurationManager.AppSettings["CompanyName"].ToString() %></div>
    </form>
</body>
</html>

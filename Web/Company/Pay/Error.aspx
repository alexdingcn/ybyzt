<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Distributor_Pay_Error" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>支付错误</title>
    <link href="../../Distributor/css/pay.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            if (window.history && window.history.pushState) {
                $(window).on('popstate', function () {
                    var hashLocation = location.hash;
                    var hashSplit = hashLocation.split("#!/");
                    var hashName = hashSplit[1];
                    if (hashName !== '') {
                        var hash = window.location.hash;
                        if (hash === '') {
                            window.history.pushState('forward', null, window.location.href);
                        }
                    }
                });
                window.history.pushState('forward', null, window.location.href);
            }
        });
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
        <!--header start-->
        <div class="header">
        <div class="con">
            <div class="logo">
                <a href="../../index.aspx">
                    <img src="../../images/logo.png" height="37" /></a><i>支付平台</i></div>
            <div class="topMenu">
                <a href="../UserIndex.aspx">我的桌面</a>|<a href="../../index.aspx">医站通首页</a>|<a href="orderPayList.aspx">我的待支付订单</a>
                &nbsp;
                <% if (ConfigurationManager.AppSettings["OrgCode"] == "SYJ")
                   {%>
                <i style="font-weight: bold; color: red;">服务热线：400-8859-319</i>
                <% } %>
            </div>
        </div>
    </div>
        <!--header end-->
        <div class="blank20"></div>

       <div class="payOpt" id="checkId">
	        <div class="payError"><img src="../images/error.png" /><%=str%><i class="red"><span id="PayedAmount" runat="server"></span></i></div>
            <div class="payLink"><label id="lblMsg" runat="server" ></label></div> 
            <div class="payLink" style="padding-top:5px;"><%=url %></div> 
            <div class="blank20"></div>
        </div> 
       <%-- <div class="payOpt" id="checkId">
	<div class="payError"><img src="../images/error.png" />无法完成付款</div>
    <div class="payLink">您可以<a href="">查看订单详情</a><a href="">返回订单管理</a></div> 
    <div class="blank20"></div>
</div> --%>
        <!--快捷支付弹窗 end-->



        <div class="blank20"></div>
        <!--常见问题 start-->
        <div class="question">
	        <%--<div class="title">常见问题</div>
            <dl class="list">
    	        <dt>1、经常需要动态给Div添加或者删除样式，利用jquery的addClass(id)与delete</dt>
                <dd>在开发网页过程中常需要对A链接使用JS制作一个点击弹窗,那么在点击该A链接就不能发生跳转行为,于是我们取消A链接的跳转</dd>
                <dt>1、经常需要动态给Div添加或者删除样式，利用jquery的addClass(id)与delete</dt>
                <dd>在开发网页过程中常需要对A链接使用JS制作一个点击弹窗,那么在点击该A链接就不能发生跳转行为,于是我们取消A链接的跳转</dd>
                <dt>1、经常需要动态给Div添加或者删除样式，利用jquery的addClass(id)与delete</dt>
                <dd>在开发网页过程中常需要对A链接使用JS制作一个点击弹窗,那么在点击该A链接就不能发生跳转行为,于是我们取消A链接的跳转</dd>
            </dl>--%>
        </div>
        <!--常见问题 end-->
        <div class="footer"> <%= ConfigurationManager.AppSettings["CompanyName"].ToString() %></div>
    </form>
</body>
</html>

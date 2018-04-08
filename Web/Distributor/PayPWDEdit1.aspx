<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayPWDEdit1.aspx.cs" Inherits="Distributor_PayPWDEdit1" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>修改密码</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../js/CommonJs.js"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $(function () {
                $("#spanpwd1").text("(注意：支付密码必须为6-16位字母与数字的组合)").css("color", "#000");
            });
        });
        function svae() {
            if ($.trim($("#Password").val()) == "") {
                $("#spanpwd1").text("请输入新密码").css({"display": "inline-block","color":"red"});
                return false;
            }
            if ($.trim($("#Password1").val()) == "") {
                $("#spanpwd2").text("请输入确认密码").css("display", "inline-block");
                return false;
            }
            if ($.trim($("#Password").val()) != $.trim($("#Password1").val())) {
                $("#spanpwd2").text("两次输入的密码不一致").css("display", "inline-block");
                return false;
            }
            if ($("#Password").val().length < 6 || $("#Password").val().length > 16) {
                $("#spanpwd1").text("-新密码长度必须大于6位，小于16位").css({ "display": "inline-block", "color": "red" });
                return false;
            }
            if (!IsPayPwd($("#Password").val())) {
                $("#spanpwd1").text("-新密码必须为字母加数字的组合").css({ "display": "inline-block", "color": "red" });
                return false;
            }
        }
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head1" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="nav-3" />
    <div class="rightCon">
    <div class="info"> <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="#" class="cur">修改密码</a></div>
    <div id="updatepay1" class="userTrend">
            <div class="uTitle">
                <b>修改支付密码</b></div>
            <ul class="ModifyData">
                <li><i class="head">登录帐号：</i><%=user.UserName %></li>
                <li><i class="head"><i class="required">*</i>新支付密码：</i><input id="Password" name="" type="password" runat="server" class="box" value="" maxlength="40" />&nbsp;&nbsp;<span id="spanpwd1" runat="server" style="color:Red;"></span></li>
                <li><i class="head"><i class="required">*</i>确认支付密码：</i><input id="Password1" name="" type="password" runat="server" class="box" value="" maxlength="40" />&nbsp;&nbsp;<span id="spanpwd2" runat="server" style="color:Red;"></span></li>
            </ul>
            <div class="mdBtn">
                <a id="A2" href="#" onclick="return svae();" onserverclick="A_Save" runat="server" class="btnYe">确定修改</a></div>
            <div class="blank10">
            </div>
        </div>
        <div class="blank10">
            </div>
    </div>

    </div>
    
    </form>
</body>
</html>

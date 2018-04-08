<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserPWDEdit.aspx.cs" Inherits="Distributor_UserPWDEdit" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>修改密码</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/CommonJs.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/CommonSha.js"></script>
    <script type="text/javascript">
        

        function btnupdate() {
            if ($.trim($("#txtpwd1").val()) == "") {
                $("#spanpwd1").text("-原密码不能为空").css("display", "inline-block");
                return false;
            }
            if ($.trim($("#txtpwd2").val()) == "123456") {
                $("#spanpwd2").text("-不能使用系统默认密码作为新密码").css("display", "inline-block");
                return false;
            }
            if ($.trim($("#txtpwd2").val()) == "") {
                $("#spanpwd2").text("-新密码不能为空").css("display", "inline-block");
                return false;
            }
            if ($.trim($("#txtpwd3").val()) == "") {
                $("#spanpwd3").text("-确认密码不能为空").css("display", "inline-block");
                return false;
            }
            if ($.trim($("#txtpwd3").val()) != $.trim($("#txtpwd2").val())) {
                $("#spanpwd3").text("-两次输入的密码不一致").css("display", "inline-block");
                return false;
            }

            if ($("#txtpwd2").val().length < 6 || $("#txtpwd2").val().length > 16) {
                $("#spanpwd3").text("-新密码长度必须大于6位，小于16位").css("display", "inline-block");
                return false;
            }
            $("#txtpwd1").val(hex_two($("#txtpwd1").val()));
            $("#txtpwd2").val(hex_two($("#txtpwd2").val()));
            $("#txtpwd3").val(hex_two($("#txtpwd3").val()));
            return true;
            
        }

    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head1" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="nav-6" />
    <div class="rightCon">
    <div class="info"><a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="/Distributor/UserPWDEdit.aspx" class="cur">修改密码</a></div>
        <!--修改登录密码 start-->
        <div class="userTrend">
            <div class="uTitle">
                <b>修改登录密码</b></div>
            <ul class="ModifyData">
                <li><i class="head">登录帐号：</i><%=user.UserName %></li>
                <li><i class="head"><i class="required">*</i>原密码：</i><input id="txtpwd1" name="" type="password" runat="server" class="box" value="" maxlength="40" />&nbsp;&nbsp;<span id="spanpwd1" runat="server" style="color:Red;"></span></li>
                <li><i class="head"><i class="required">*</i>新密码：</i><input id="txtpwd2" name="" type="password" runat="server" class="box" value="" maxlength="40" />&nbsp;&nbsp;<span id="spanpwd2" runat="server" style="color:Red;"></span></li>
                <li><i class="head"><i class="required">*</i>确认密码：</i><input id="txtpwd3" name="" type="password" runat="server" class="box" value="" maxlength="40" />&nbsp;&nbsp;<span id="spanpwd3" runat="server" style="color:Red;"></span></li>
            </ul>
            <div class="mdBtn">
                <a href="#" onclick="if(!btnupdate()){return false;}" onserverclick="Btn_Update" runat="server" class="btnYe">确定修改</a></div>
            <div class="blank10">
            </div>
        </div>
        <!--修改登录密码 end-->
    </div>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayPWDEdit.aspx.cs" Inherits="Distributor_PayPWDEdit" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>修改密码</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/GetPhoneCode.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
        <script src="../js/CommonJs.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(function () {
                $("#spanpwd2").text("(注意：支付密码必须为6-16位字母与数字的组合)").css("color", "#000");

                //忘记密码 by sjz 2015-12-11
                var type = '<%=Request["type"]+"" %>';
                if (type.toString() == "RestPWD") {
                    $("#PhonePay").css("display", "block");
                    $("#updatepay").css("display", "none");
                }
            });
            $("#ForgetPay").click(function () {
                $("#PhonePay").css("display", "block");
                $("#updatepay").css("display", "none");
            });
            $("#off").click(function () {
                if ($("#hidfh").val() != "") {
                    location.href = 'pay/Pay.aspx?KeyID=<%=Common.DesEncrypt(OrderID,Common.EncryptKey) %>';
                }
                else {
                    $("#PhonePay").css("display", "none");
                    $("#updatepay").css("display", "block");
                }
            });
        });

        function asave() {
            var str = "";

            if ($("#paypwd").length != 0) {
                if ($.trim($("#paypwd").val()) == "") {
                    $("#spanpwd1").text("-原支付密码不能为空").css("display", "inline-block");
                    return false;
                }
            }
            if ($.trim($("#paypwd1").val()) == "") {
                $("#spanpwd2").css("color", "red");
                $("#spanpwd2").text("-新支付密码不能为空").css("display", "inline-block");
                return false;
            }
            if ($.trim($("#paypwd2").val()) == "") {
                $("#spanpwd3").text("-确认支付密码不能为空").css("display", "inline-block");
                return false;
            }
            if ($.trim($("#paypwd2").val()) != $.trim($("#paypwd1").val())) {
                $("#spanpwd3").text("-两次输入的密码不一致").css("display", "inline-block");
                return false;
            }
            if (!IsPayPwd($("#paypwd1").val())) {
                $("#spanpwd2").css("color", "red");
                $("#spanpwd2").text("-新密码必须为字母加数字的组合").css("display", "inline-block");
                return false;
            }
            if (!IsPayPwd($("#paypwd2").val())) {
                $("#spanpwd3").text("-确认密码必须为字母加数字的组合").css("display", "inline-block");
                return false;
            }
            if ($("#paypwd1").val().length < 6 || $("#paypwd1").val().length > 16) {
                $("#spanpwd2").css("color", "red");
                $("#spanpwd2").text("-新密码长度必须大于6位，小于16位").css("display", "inline-block");
                return false;
            }
            return true;
        }
        function aaffirm() {
            var str = "";
            if ($.trim($("#txtcode").val()) == "") {
                $("#spancode").text("-验证码不能为空").css("display", "inline-block");
                return false;
            }
            $.ajax({
                url: 'PayPWDEdit.aspx?type=1',
                data: { code: $.trim($("#txtcode").val()) },
                dataType: 'json',
                success: function (img) {
                    if (!img.type) {
                        $("#spancode").text(img.str).css("display", "inline-block");
                        $("#txtcode").val("");
                    }
                    else {
                        location.href = "PayPWDEdit1.aspx?code=" + img.str;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("服务器异常，请稍后再试", IconOption.哭脸);
                }
            });
        }
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head1" runat="server" />
    <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="nav-3" />
        <div class="rightCon">
            <div class="info">
                 <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="#" class="cur">修改密码</a></div>
            <!--修改支付密码 start-->
            <div id="updatepay" runat="server" class="userTrend">
                <div class="uTitle">
                    <b>修改支付密码</b></div>
                <ul class="ModifyData">
                    <li><i class="head">登录帐号：</i><%=user.UserName %></li>
                    <li id="paypwdli" runat="server"><i class="head"><i class="required">*</i>原支付密码：</i><input
                        id="paypwd" name="" type="password" runat="server" class="box" value="" maxlength="40" /><a id="ForgetPay"
                            href="#" class="a2">忘记支付密码?</a></li>
                    <li><i class="head"><i class="required">*</i>新支付密码：</i><input id="paypwd1" name=""
                        type="password" runat="server" class="box" value="" maxlength="40" />&nbsp;&nbsp;<span id="spanpwd2"
                            runat="server" style="color: Red;"></span></li>
                    <li><i class="head"><i class="required">*</i>确认支付密码：</i><input id="paypwd2" name=""
                        type="password" runat="server" class="box" value="" maxlength="40" />&nbsp;&nbsp;<span id="spanpwd3"
                            style="color: Red;"></span></li>
                </ul>
                <div class="mdBtn">
                    <a href="#" onclick="return asave();" onserverclick="A_Save" runat="server" class="btnYe">
                        确定修改</a>&nbsp;&nbsp;<span id="spanpwd1" runat="server" style="color: Red;"></span></div>
                <div class="blank10">
                </div>
            </div>
            <div id="PhonePay" class="userTrend" runat="server" style="display: none;">
                <div class="uTitle">
                    <b>修改支付密码</b></div>
                <ul class="ModifyData">
                    <li><i class="head">手机号：</i><%=user.Phone %><a href="#" id="getcode" onclick='getphonecode("<%=user.Phone %>","0","支付密码找回","<%=user.ID %>","<%=user.UserName %>");'
                        class="btnBl">获取验证码</a></li>
                    <li><i class="head"><i class="required">*</i>手机验证码：</i><input id="txtcode" runat="server"
                        name="" type="text" class="box box2" value="" />&nbsp;&nbsp;<span id="spancode" style="color: Red;"></span></li>
                </ul>
                <div class="mdBtn">
                    <a href="#" onclick="aaffirm();" runat="server" style="margin: 0 0 0 100px;" class="btnOr">
                        确定</a><a href="#" id="off" style="margin: 0 0 0 10px;" class="btnBl">取消</a></div>
                <input type="hidden" id="hidfh" runat="server" />
                <div class="blank10">
                </div>
            </div>
        </div>
    </div>
    
    </form>
</body>
</html>

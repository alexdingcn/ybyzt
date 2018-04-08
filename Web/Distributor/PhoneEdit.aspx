<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PhoneEdit.aspx.cs" Inherits="Distributor_PhoneEdit" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<title>修改绑定手机</title>
<link href="css/global.css" rel="stylesheet" type="text/css" />
<script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
<script src="../js/GetPhoneCode.js" type="text/javascript"></script>
<script src="../js/layer/layer.js" type="text/javascript"></script>
<script src="../js/layerCommon.js" type="text/javascript"></script>
<script type="text/javascript">
    var isMobile = /^0?1[0-9]{10}$/;

    function aerphone() {
        if ($.trim($("#txtphone").val()) == "") {
            $("#spanphone").text("手机号码不能为空").css("display", "inline-block");
            return false;
        }
        if (!isMobile.test($.trim($("#txtphone").val()))) {
            $("#spanphone").text("手机号码格式不正确").css("display", "inline-block");
            return false;
        }
        $.ajax({
            url: "phoneedit.aspx?type=erphone",
            data: { phone: $("#txtphone").val() },
            success: function (img) {
                if (img != "") {
                    $("#spanphone").text(img).css("display", "inline-block");
                    $("#getcode").unbind("click");
                    return false;
                }
                else {
                    $("#getcode").bind("click", function () {
                        getcode();
                    });
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("服务器异常，请稍后再试", IconOption.哭脸);
            }
        });
    }


    function btnupdate() {
        var str = "";
//        if ($.trim($("#txtpaypwd").val()) == "") {
//            $("#spanpwd").text("-请输入支付密码").css("display", "inline-block");
//            return false;
//        }
        if ($.trim($("#txtphone").val()) == "") {
            $("#spanphone").text("-请输入手机号码").css("display", "inline-block");
            return false;
        }
        else if (!isMobile.test($.trim($("#txtphone").val()))) {
            $("#spanphone").text("-手机号码格式不正确").css("display", "inline-block");
            return false;
        }
        
        if ($.trim($("#txtcode").val()) == "") {
            $("#spancode").text("-请输入验证码").css("display", "inline-block");
            return false;
        }
        
        
        return true;

    }

    function getcode() {
        if ($.trim($("#spanphone").text()) != "") {
            return false;
        }
        var phone = $("#txtphone").val();
        if (phone != "") {
            $.ajax({
                url: "phoneedit.aspx?types=aphone",
                type: "POST",
                data: { phone: phone },
                success: function (phones) {
                    if (phones != "") {
                        getphonecode(phones, "0", "修改绑定手机", "<%=user.ID %>", "<%=user.UserName %>");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("服务器异常，请稍后再试", IconOption.哭脸);
                }
            })
        }
    }

    
</script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head1" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="PhoneEdit" />
    <div class="rightCon">
    <div class="info"> <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="#" class="cur">修改绑定手机</a></div>
        <!--修改绑定手机 start-->
        <div class="userTrend">
            <div class="uTitle">
                <b>修改绑定手机</b></div>
            <ul class="ModifyData">
                <li><i class="head">登录帐号：</i><%=user.UserName %></li>
                <%--<li><i class="head"><i class="required">*</i>支付密码：</i><input id="txtpaypwd" onfocus="$('#spanpwd').css('display','none');" name="" type="password" class="box" runat="server" value="" />&nbsp;&nbsp;<span id="spanpwd" style="color:Red;"></span></li>--%>
                <li><i class="head"><i class="required">*</i>新手机号码：</i><input id="txtphone" name="" onfocus="$('#spanphone').text('').css('display','none');" onblur="aerphone()" type="text" class="box" runat="server" value="" /><a id="getcode"
                    href="#" class="btnBl">获取验证码</a>&nbsp;&nbsp;<span id="spanphone" style="color:Red;"></span></li>
                <li><i class="head"><i class="required">*</i>手机验证码：</i><input id="txtcode" name="" onfocus="$('#spancode').css('display','none');" type="text" runat="server" class="box box2" value="" />&nbsp;&nbsp;<span id="spancode" runat="server" style="color:Red;"></span></li>
            </ul>
            <div class="mdBtn">
                <a href="#" onclick="if(!btnupdate()){return false;}" onserverclick="Btn_Update" runat="server" class="btnYe">确定修改</a><a id="afh" href="UserEdit.aspx" runat="server" style=" margin:0 0 0 20px; padding: 2px 15px;" class="btnBl">返回</a></div>
            <div class="blank10">
            </div>
        </div>
        <!--修改绑定手机 end-->
    </div>
    </div>
    </form>
</body>
</html>

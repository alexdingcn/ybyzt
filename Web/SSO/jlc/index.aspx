<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="jlc_login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>酒隆仓代理商订单管理系统</title>
    <script src="<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>/Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>/js/GetPhoneCode.js" type="text/javascript"></script>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script src="<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>/js/layer/layer.js" type="text/javascript"></script>
    <script src="<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>/js/layerCommon.js" type="text/javascript"></script>
<script type="text/javascript">

        function txtfocus() {
            $("#txtpassword").hide(), $("#txtpwd").show().focus(), $("#txtpwd").css('color', '#464646');
        }
        function txtblur() {
            $("#txtpwd").val() == "" && $("#txtpassword").show() && $("#txtpwd").hide();
        }
        var index = 0;
        function LayerClose() {
            layerCommon.layerClose(index);
        }
        $(document).ready(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    if ($(".mianLogin#defaultlogin").is(":visible")) {
                        $("#btnlogin").trigger("click");
                    }
                    else if ($(".mianLogin#Phonelogin").is(":visible")) {
                        $("#btnPhoneLogin").trigger("click");
                    }
                }
            });


            $("#btnlogin").on("click", function () {
                var $this = $(this);
                if ($this.data("IsEnabled") == true) {
                    return;
                }
                if ($.trim($("#txtuid").val()) == "" || $.trim($("#txtuid").val()) == "帐号") {
                    $("#txtuid").addClass("redbox");
                    return;
                } else {
                    $("#txtuid").removeClass("redbox");
                }

                if ($.trim($("#txtpassword").val()) == "") {
                    $("#txtpassword").addClass("redbox");
                    return;
                } else {
                    $("#txtpassword").removeClass("redbox");
                }

                if ($.trim($("#txtcode").val()) == "" || $.trim($("#txtcode").val()) == $("#txtcode")[0].defaultValue) {
                    $("#txtcode").addClass("redbox");
                    return;
                } else {
                    $("#txtcode").removeClass("redbox");
                }
                $this.data("IsEnabled", true);

                
            });
        })
    </script>
</head>
<body>
    <form id="form1" action="" runat="server">
    <div class="header">
        <h1 title="酒隆仓">
            <img src="images/lh-logo.png" alt=""/></h1>
    </div>
    <div class="login">
        <!--帐号登陆 start-->
        <div class="boxLogin">
            <div class="mianLogin" id="defaultlogin" runat="server">
                <div class="bt">
                    帐号登录<a id="aphonelogin" href="javascript:;" class="tw PhoneLg" style="display:none;">手机登录</a>
                </div>
                <div class="li">
                    <input id="txtuid" autocomplete="off" type="text" runat="server" class="box2" onfocus="if(value==defaultValue){value='';this.style.color='#555'}"
                        onblur="if(!value){value=defaultValue;this.style.color='#999'}" value="帐号" />
                </div>
                <div class="li">
                    <%--<input id="txtpassword" autocomplete="off" type="text" runat="server" class="box2" onfocus="if(value==defaultValue){value='';this.style.color='#555'}"
                        onblur="if(!value){value=defaultValue;this.style.color='#999'}" value="密码" />--%>
                    <input type="text" value="密码" onfocus="txtfocus()" class="box2" id="txtpassword" /><input
                        type="password" value="" onblur="txtblur()" class="box2" id="txtpwd" runat="server" style="display: none;" />
                </div>
                <div class="li">
                    <input id="txtcode" autocomplete="off" style="width: 180px" type="text" class="box2" onfocus="if(value==defaultValue){value='';this.style.color='#555'}"
                        onblur="if(!value){value=defaultValue;this.style.color='#999'}" value="请输入验证码"  />
                    <span class="captcha">
                        <img id="ckcode" style="cursor: pointer;" alt="看不清？点击更换" onclick="this.src='/UserControl/CheckCode.aspx?t='+new Date().getTime()"
                            src="/UserControl/CheckCode.aspx" width="80" title="看不清?点击更换" height="35" /></span>
                </div>
                <div class="btn">
                    <a class="btnlogin" href="javascript:;" style="cursor: pointer;" id="btnlogin" runat="server" onserverclick="btnlogin_click">登 录</a></div>
                <div class="txt2">
                    <span id="erlogin" style="padding: 0px 10px 0 0px; color: red; display: none;"><span
                        style="position: relative; top: 4px; margin: 0px 5px;">
                        <img src="/images/icoerror.gif" /></span><i id="erpwd"></i></span></div>
                <div class="bg">
                </div>
            </div>
            <!--帐号登陆 end-->
            <!--手机登录 start-->
            <div class="mianLogin" id="Phonelogin" runat="server" style="display: none;">
                <div class="bt">
                    手机登录<a id="apwdlogin" href="javascript:;" class="tw AccountLg">帐号登录</a>
                </div>
                <div class="li">
                    <input id="txtPhone" type="text" runat="server" class="box" maxlength="11" onfocus="if(value==defaultValue){value='';this.style.color='#555'}"
                        onblur="if(!value){value=defaultValue;this.style.color='#999'}" value="输入帐号绑定的手机" /></div>
                <div class="li">
                    <input id="txtPcode" type="text" class="box boxw" style="width: 180px" onfocus="if(value==defaultValue){value='';this.style.color='#555'}"
                        onblur="if(!value){value=defaultValue;this.style.color='#999'}" value="请输入验证码"
                        autocomplete="off" />
                    <span class="captcha">
                        <img id="ckcode" style="cursor: pointer;" alt="看不清？点击更换" onclick="this.src='/UserControl/CheckCode.aspx?t='+new Date().getTime()"
                            src="/UserControl/CheckCode.aspx" width="80" title="看不清?点击更换" height="35" /></span>
                </div>
                <div class="li">
                    <input id="txtPhoneCode" type="text" class="box boxw" style="width: 180px;" onfocus="if(value==defaultValue){value='';this.style.color='#555'}"
                        onblur="if(!value){value=defaultValue;this.style.color='#999'}" value="请输入短信验证码" />
                    <a id="getcode" class="btn2 captcha">获取验证码</a>
                </div>
                <div class="btn">
                    <a href="javascript:;" id="btnPhoneLogin">登 录</a></div>
                <div class="txt2">
                    <span id="erlogin" class="left" style="display: none;"><span style="position: relative;
                        top: 4px; margin: 0px 5px;">
                        <img src="/images/icoerror.gif" /></span><i id="erpwd"></i></span>
                </div>
                <div class="bg">
                    <a href="" id="aSSoLogin" runat="server">SSO登录</a>
                </div>
            </div>
        </div>
        <!--手机登录 end-->
    </div>
    <div class="blank10">
    </div>
    <div class="f-copy">
        粤ICP备17130448号 Copyright © 2016 深圳前海医伴金服信息技术有限公司版权所有</div>
   
    </form>
</body>
</html>

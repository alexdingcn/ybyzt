<%@ Page Language="C#" AutoEventWireup="true" CodeFile="indexold.aspx.cs" Inherits="SSO_jlc_indexold" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>酒隆仓代理商订单管理系统</title>
    <script src="/Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="/js/GetPhoneCode.js" type="text/javascript"></script>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script src="/js/layer/layer.js" type="text/javascript"></script>
    <script src="/js/layerCommon.js" type="text/javascript"></script>
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


            $(".PhoneLg", ".mianLogin#defaultlogin").on("click", function () {
                $(".mianLogin#defaultlogin").fadeOut(400, function () {
                    $("img#ckcode", ".mianLogin#Phonelogin").trigger("click"), $(".mianLogin#Phonelogin").fadeIn(300);
                })
            }), $(".AccountLg", ".mianLogin#Phonelogin").on("click", function () {
                $(".mianLogin#Phonelogin").fadeOut(400, function () {
                    $("img#ckcode", ".mianLogin#defaultlogin").trigger("click"), $(".mianLogin#defaultlogin").fadeIn(300);
                })
            }), $("#btnPhoneLogin", ".mianLogin#Phonelogin").on("click", function () {
                var $this = $(this);
                if ($this.data("IsEnabled") == true) {
                    return;
                };
                return (($.trim($("#txtPhone").val()) == $("#txtPhone")[0].defaultValue || $.trim($("#txtPhone").val()) == "") && $("#txtPhone").addClass("redbox").focus() ||
                ($.trim($("#txtPcode").val()) == $("#txtPcode")[0].defaultValue || $.trim($("#txtPcode").val()) == "") && $("#txtPcode").addClass("redbox").focus() ||
                ($.trim($("#txtPhoneCode").val()) == $("#txtPhoneCode")[0].defaultValue || $.trim($("#txtPhoneCode").val()) == "") && $("#txtPhoneCode").addClass("redbox").focus() ? !1 : function () {
                    $this.data("IsEnabled", true);
                    $.ajax({
                        type: "POST",
                        url: '/Controller/login.ashx',
                        data: { SubmitAcion: "PhoneLogin", Phone: $.trim($("#txtPhone").val()), PhoneCode: $.trim($("#txtPhoneCode").val()), Code: $.trim($("#txtPcode").val()) },
                        dataType: "json",
                        timeout: 5000,
                        cache: false,
                        success: function (ReturnData) {
                            if (ReturnData.Result) {
                                $("#erlogin", ".mianLogin#Phonelogin").hide();
                                !ReturnData.IsMoreUser ?
                                function () {
                                    switch (ReturnData.Type) {
                                        case 1: location.href = "/Distributor/UserIndex.aspx"; break;
                                        case 5: location.href = "/Distributor/UserIndex.aspx"; break;
                                        case 3: location.href = "/Company/jsc.aspx"; break;
                                        case 4: location.href = "/Company/jsc.aspx"; break;
                                        default: location.href = "/" + ReturnData.Compid + ".html";
                                    }
                                } () : function () {
                                    index = layerCommon.openWindow("选择角色", "/WindowLogin.aspx?Action=AcccountSwich", "360px", "300px", function () {
                                        $.ajax({
                                            type: "POST",
                                            url: '/Controller/login.ashx',
                                            data: { SubmitAcion: "CloseAccuntSwitch" },
                                            cache: false,
                                            success: function (ReturnData) {
                                            },
                                            error: function () {
                                            }
                                        });
                                    }, false);
                                    $this.data("IsEnabled", false);
                                } ();
                            }
                            else {
                                $("#erlogin", ".mianLogin#Phonelogin").show().find("#erpwd").text(ReturnData.Msg);
                                $this.data("IsEnabled", false);
                            }
                        },
                        error: function () {
                            $this.data("IsEnabled", false);
                            $("#erlogin", ".mianLogin#Phonelogin").show().find("#erpwd").text("服务器或网络异常，请重试");
                        }
                    });
                } ()
                );

            }), $(".mianLogin#defaultlogin input:text,.mianLogin#Phonelogin input:text").on("blur", function () {
                if ($.trim(this.value) != "") {
                    $(this).removeClass("redbox");
                }
                $("#erlogin", ".mianLogin#defaultlogin,.mianLogin#Phonelogin").hide();
            }), $("#getcode", ".mianLogin#Phonelogin").on("click", function () {

                return (!/^0?1[0-9]{10}$/.test($.trim($("#txtPhone").val())) && $("#erlogin", ".mianLogin#Phonelogin").show().find("#erpwd").text("手机号码格式错误")
                || $.trim($("#txtPcode").val()) == $("#txtPcode")[0].defaultValue && $("#erlogin", ".mianLogin#Phonelogin").show().find("#erpwd").text("请输入四位图文验证码") ? !1 : function () {
                    $.ajax({
                        type: "POST",
                        url: '/Controller/login.ashx',
                        data: { SubmitAcion: "CheckCode", Code: $.trim($("#txtPcode").val()) },
                        cache: false,
                        timeout: 5000,
                        dataType: "json",
                        success: function (ReturnData) {
                            if (ReturnData.Result) {
                                getCode($("#txtPhone").val(), "-1", "手机登录", 0, $("#txtPhone").val());
                            } else {
                                $("#erlogin", ".mianLogin#Phonelogin").show().find("#erpwd").text(ReturnData.Msg);
                            }
                        },
                        error: function () {
                        }
                    });
                } ()
                );
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

                if ($.trim($("#txtpwd").val()) == "") {
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
                $.ajax({
                    type: "POST",
                    url: '/Controller/login.ashx',
                    data: { SubmitAcion: "AccuntLogin", Username: $.trim($("#txtuid").val()), Password: $.trim($("#txtpwd").val()), Code: $.trim($("#txtcode").val()) },
                    timeout: 5000,
                    cache: false,
                    dataType: "json",
                    success: function (ReturnData) {
                        if (ReturnData.Result) {
                            $("#erlogin", ".mianLogin#defaultlogin").hide();
                            !ReturnData.IsMoreUser ? function () {
                                switch (ReturnData.Type) {
                                    case 1: location.href = "/Distributor/UserIndex.aspx"; break;
                                    case 5: location.href = "/Distributor/UserIndex.aspx"; break;
                                    case 3: location.href = "/Company/jsc.aspx"; break;
                                    case 4: location.href = "/Company/jsc.aspx"; break;
                                    default: location.href = "/" + ReturnData.Compid + ".html";
                                }
                            } () : function () {
                                $this.data("IsEnabled", false);
                                index = layerCommon.openWindow("选择角色", "/WindowLogin.aspx?Action=AcccountSwich", "360px", "300px", function () {
                                    $.ajax({
                                        type: "POST",
                                        url: '/Controller/login.ashx',
                                        data: { SubmitAcion: "CloseAccuntSwitch" },
                                        cache: false,
                                        success: function (ReturnData) {
                                        },
                                        error: function () {
                                        }
                                    });
                                }, false);
                            } ();
                        }
                        else {
                            $this.data("IsEnabled", false), $("#erlogin", ".mianLogin#defaultlogin").show().find("#erpwd").text(ReturnData.Msg), $("#ckcode").click(), $("#txtcode").val('');
                        }
                    },
                    error: function () {
                        $this.data("IsEnabled", false);
                        $("#erlogin", ".mianLogin#defaultlogin").show().find("#erpwd").text("服务器或网络异常，请重试");
                    }
                });

            });


        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="header">
        <h1 title="酒隆仓">
            <img src="images/lh-logo.png" /></h1>
    </div>
    <div class="login">
        <!--帐号登陆 start-->
        <div class="boxLogin">
            <div class="mianLogin" id="defaultlogin" runat="server">
                <div class="bt">
                    帐号登录<a id="aphonelogin" href="javascript:;" class="tw PhoneLg">手机登录</a>
                </div>
                <div class="li">
                    <input id="txtuid" autocomplete="off" type="text" runat="server" class="box2" onfocus="if(value==defaultValue){value='';this.style.color='#555'}"
                        onblur="if(!value){value=defaultValue;this.style.color='#999'}" value="帐号" />
                </div>
                <div class="li">
                    <input type="text" value="密码" onfocus="txtfocus()" class="box2" id="txtpassword" /><input
                        type="password" value="" onblur="txtblur()" class="box2" id="txtpwd" style="display: none;" />
                </div>
                <div class="li">
                    <input id="txtcode" style="width: 180px" type="text" class="box2" onfocus="if(value==defaultValue){value='';this.style.color='#555'}"
                        onblur="if(!value){value=defaultValue;this.style.color='#999'}" value="请输入验证码"
                        autocomplete="off" />
                    <span class="captcha">
                        <img id="ckcode" style="cursor: pointer;" alt="看不清？点击更换" onclick="this.src='/UserControl/CheckCode.aspx?t='+new Date().getTime()"
                            src="/UserControl/CheckCode.aspx" width="80" title="看不清?点击更换" height="35" /></span>
                </div>
                <div class="btn">
                    <a class="btnlogin" href="javascript:;" style="cursor: pointer;" id="btnlogin">登 录</a></div>
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
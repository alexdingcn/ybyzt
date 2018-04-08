<%@ Page Language="C#" AutoEventWireup="true" CodeFile="windowlogin.aspx.cs" Inherits="WindowLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="baidu-site-verification" content="IdU3LryeUL" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/index.css" rel="stylesheet" type="text/css" />
    <script src="Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="js/GetPhoneCode.js" type="text/javascript"></script>
    <script src="/js/layer/layer.js" type="text/javascript"></script>
    <script src="js/CommonSha.js"></script>
    <script src="/js/layerCommon.js" type="text/javascript"></script>
    <script type="text/javascript">
        function txtfocus() {
            $("#txtpassword").hide(), $("#txtpwd").show().focus(), $("#txtpwd").css('color', '#464646');
        }
        function txtblur() {
            $("#txtpwd").val() == "" && $("#txtpassword").show() && $("#txtpwd").hide();
        }

        $(document).ready(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    if ($(".pageLogin#defaultlogin").is(":visible")) {
                        $("#btnlogin").trigger("click");
                    }
                    else if ($(".pageLogin#Phonelogin").is(":visible")) {
                        $("#btnPhoneLogin").trigger("click");
                    }
                }
            });

            $("#AccountSwitch a").on("click", function () {
                $.ajax({
                    type: "POST",
                    url: '/Controller/login.ashx',
                    data: { SubmitAcion: "AccuntSwitch", Tip: $(this).attr("tip"), chklogin: ($(this).attr("type") == "0" ? window.parent.$("#chklogin").is(":checked") : window.parent.$("#chklogin2").is(":checked")) },
                    dataType: "json",
                    timeout: 5000,
                    cache: false,
                    success: function (ReturnData) {
                        if (ReturnData.Result) {
                            !ReturnData.IsRegis ? function () {
                                //switch (ReturnData.Type) {
                                //    case 1:
                                //    case 5: window.parent.location.href = "/" + ReturnData.Compid + ".html"; break;
                                //    case 3:
                                //    case 4: window.parent.location.href = "/" + ReturnData.Compid + ".html"; break;
                                //    default: window.parent.location.href = "/" + ReturnData.Compid + ".html"; break;
                                //}
                                //edit by hgh 0421 登录后直接跳转到管理中心
                                switch (ReturnData.Type) {
                                    case 1:
                                    case 5: window.parent.location.href = "/Distributor/UserIndex.aspx"; break;
                                    case 3:
                                    case 6:
                                    case 4: window.parent.location.href = "/Company/jsc.aspx"; break;
                                    default: window.parent.location.href = "/" + ReturnData.Compid + ".html";
                                }
                                window.parent.LayerClose();
                            } () : function () {
                                window.parent && window.parent.location.reload();
                            } ();
                        } else {
                            layerCommon.msg(ReturnData.Msg, IconOption.错误, 2000);
                        }
                    },
                    error: function () {
                    }
                });
            })

            $(".PhoneLg", ".pageLogin#defaultlogin").on("click", function () {
                $(".pageLogin#defaultlogin").fadeOut(400, function () {
                    $(".pageLogin#Phonelogin").fadeIn(300), $(".pageLogin#Phonelogin img#ckcode").trigger("click");
                })
            }), $(".AccountLg", ".pageLogin#Phonelogin").on("click", function () {
                $(".pageLogin#Phonelogin").fadeOut(400, function () {
                    $(".pageLogin#defaultlogin").fadeIn(300), $(".pageLogin#defaultlogin img#ckcode").trigger("click");
                })
            }), $(".pageLogin#defaultlogin input:text,.pageLogin#Phonelogin input:text").on("blur", function () {
                if ($.trim(this.value) != "") {
                    $(this).removeClass("redbox2");
                }
                $("#erlogin", ".pageLogin#Phonelogin,.pageLogin#Phonelogin").hide();
            }), $("#getcode", ".pageLogin#Phonelogin").on("click", function () {
                return (!/^0?1[0-9]{10}$/.test($.trim($("#txtPhone").val())) && $("#erlogin", ".pageLogin#Phonelogin").show().find("#erpwd").text("手机号码格式错误")
                || $.trim($("#txtPcode").val()) == $("#txtPcode")[0].defaultValue && $("#erlogin", ".pageLogin#Phonelogin").show().find("#erpwd").text("请输入四位图文验证码") ? !1 : function () {
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
                                $("#erlogin", ".pageLogin#Phonelogin").show().find("#erpwd").text(ReturnData.Msg);
                            }
                        },
                        error: function () {
                        }
                    });
                } ()
                );
            }), $("#btnPhoneLogin", ".pageLogin#Phonelogin").on("click", function () {
                var $this = $(this);
                if ($this.data("IsEnabled") == true) {
                    return;
                }
                return (($.trim($("#txtPhone").val()) == $("#txtPhone")[0].defaultValue || $.trim($("#txtPhone").val()) == "") && $("#txtPhone").addClass("redbox2").focus() ||
                ($.trim($("#txtPcode").val()) == $("#txtPcode")[0].defaultValue || $.trim($("#txtPcode").val()) == "") && $("#txtPcode").addClass("redbox2").focus() ||
                ($.trim($("#txtPhoneCode").val()) == $("#txtPhoneCode")[0].defaultValue || $.trim($("#txtPhoneCode").val()) == "") && $("#txtPhoneCode").addClass("redbox2").focus() ? !1 : function () {
                    $this.data("IsEnabled", true);
                    $.ajax({
                        type: "POST",
                        url: 'Controller/login.ashx',
                        data: { SubmitAcion: "PhoneLogin", Phone: $.trim($("#txtPhone").val()), PhoneCode: $.trim($("#txtPhoneCode").val()), Code: $.trim($("#txtPcode").val()) },
                        dataType: "json",
                        timeout: 5000,
                        cache: false,
                        success: function (ReturnData) {
                            if (ReturnData.Result) {
                                $("#erlogin", ".pageLogin#Phonelogin").hide();
                                !ReturnData.IsMoreUser ? function () {
                                    window.parent && window.parent.location.reload();
                                } () : function () {
                                    location.href = "/WindowLogin.aspx?Action=AcccountSwich";
                                } ();
                            }
                            else {
                                $("#erlogin", ".pageLogin#Phonelogin").show().find("#erpwd").text(ReturnData.Msg);
                                $this.data("IsEnabled", false);
                            }
                        },
                        error: function () {
                            $this.data("IsEnabled", false);
                            $("#erlogin", ".pageLogin#Phonelogin").show().find("#erpwd").text("服务器或网络异常，请重试");
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
                    $("#txtuid").addClass("redbox2");
                    return;
                } else {
                    $("#txtuid").removeClass("redbox2");
                }

                if ($.trim($("#txtpwd").val()) == "") {
                    $("#txtpwd").addClass("redbox2");
                    return;
                } else {
                    $("#txtpwd").removeClass("redbox2");
                }

                if ($.trim($("#txtcode").val()) == "" || $.trim($("#txtcode").val()) == $("#txtcode")[0].defaultValue) {
                    $("#txtcode").addClass("redbox2");
                    return;
                } else {
                    $("#txtcode").removeClass("redbox2");
                }
                $this.data("IsEnabled", true);
                $.ajax({
                    type: "POST",
                    url: 'Controller/login.ashx',
                    data: { SubmitAcion: "AccuntLogin", Username: $.trim($("#txtuid").val()), Password: hex_one(hex_one(hex_two($.trim($("#txtpwd").val())))) , Code: $.trim($("#txtcode").val()) },
                    timeout: 5000,
                    cache: false,
                    dataType: "json",
                    success: function (ReturnData) {
                        if (ReturnData.Result) {
                            $("#erlogin", ".pageLogin#defaultlogin").hide();
                            !ReturnData.IsMoreUser ? function () {
                                window.parent && window.parent.location.reload();
                            } () : function () {
                                location.href = "/WindowLogin.aspx?Action=AcccountSwich";
                            } ();
                        }
                        else {
                            $this.data("IsEnabled", false), $("#erlogin", ".pageLogin#defaultlogin").show().find("#erpwd").text(ReturnData.Msg), $("#ckcode").click(), $("#txtcode").val('');
                        }
                    },
                    error: function () {
                        $this.data("IsEnabled", false);
                        $("#erlogin", ".pageLogin#defaultlogin").show().find("#erpwd").text("服务器或网络异常，请重试");
                    }
                });

            });


        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <style>
        /*登录*/
        .box2Login
        {
            width: 320px;
            margin: 0px auto;
            z-index: 99;
            position: absolute;
            top: 50%;
            left: 50%;
            margin: -180px 0 0 -160px;
        }
        .pageLogin
        {
            position: relative;
            z-index: 99;
            width: 320px;
            margin-top: 10px;
        }
        .pageLogin .bg, .pageLogin2 .bg
        {
            position: absolute;
            top: -0px;
            right: 0;
            left: 0;
            bottom: 0;
            z-index: 1;
            background: #fff;
            filter: alpha(opacity=90);
            opacity: 0.9;
        }
        .pageLogin .bt
        {
            line-height: 45px;
            font-size: 17px;
            overflow: hidden;
            position: relative;
            padding: 0 15px;
            color: #555;
            z-index: 9;
        }
        .pageLogin .bt .tw
        {
            position: absolute;
            top: 0;
            right: 35px;
            color: #555;
            font-size: 12px;
        }
        /* add by kb 2016/5/20 */
        .pageLogin .bt .close
        {
            background: url(Distributor/images/icon.png) no-repeat 0px -119px;
            width: 14px;
            height: 14px;
            cursor: pointer;
            display: inline-block;
            position: absolute;
            top: 15px;
            right: 15px;
            z-index: 999;
        }
        .pageLogin .li
        {
            padding-left: 10px;
            overflow: hidden;
            padding-bottom: 12px;
            position: relative;
            z-index: 9;
        }
        .pageLogin .li i
        {
            font-style: normal;
            font-size: 14px;
        }
        .pageLogin .li .box, .pageLogin .li .box2
        {
            border: 1px solid #ddd;
            height: 35px;
            line-height: 35px;
            width: 290px;
            color: #aaa;
            font-size: 14px;
            padding-left: 10px;
            font-size: 14px;
        }
        .pageLogin .li .redbox, .pageLogin .li .redbox2
        {
            border: 1px solid red;
        }
        
        .pageLogin .captcha
        {
            width: 90px;
            height: 35px;
            overflow: hidden;
            border: 1px solid #ddd;
            overflow: hidden;
            position: absolute;
            top: 0;
            right: 10px;
        }
        .pageLogin .btn
        {
            padding: 0px 15px;
            z-index: 9;
            position: relative;
        }
        .pageLogin .btn a
        {
            width: 100%;
            height: 40px;
            background: #ff8b00;
            display: block;
            text-align: center;
            font-size: 16px;
            color: #fff;
            line-height: 40px;
            cursor: pointer;
        }
        .pageLogin .btn a:hover
        {
            text-decoration: none;
            background: #f60;
        }
        .pageLogin .txt
        {
            height: 30px;
            padding: 12px 10px 0 0;
            text-align: right;
            z-index: 9;
            position: relative;
        }
        .pageLogin .txt a
        {
            padding-right: 10px;
            color: #777;
        }
        .iconYz
        {
            width: 18px;
            height: 13px;
            background: url(../images/icon.png) no-repeat 0px -32px;
            display: inline-table;
        }
        .iconMm
        {
            background: url(../images/icon.png) no-repeat 0px -59px;
            width: 16px;
            height: 15px;
            display: inline-table;
        }
        
        .pullDown2
        {
            border: 1px solid #e5e5e5;
            width: 152px;
            background: #fff;
            position: absolute;
            z-index: 10000;
        }
        .pullDown2 .list a
        {
            padding-left: 10px;
            line-height: 26px;
            height: 26px;
            display: block;
            color: #444;
        }
        .pullDown2 .list a:hover
        {
            background: #d1d1d2;
            color: #444;
        }
        .pullDown2 .addBtn
        {
            background: #f5f5f5;
            border-top: 1px solid #ddd;
            height: 30px;
            line-height: 30px;
            position: relative;
            display: block;
            padding-left: 25px;
            color: #555;
        }
        .pageLogin .btn2
        {
            line-height: 36px;
            text-align: center;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffff', endColorstr='#f1f1f1');
            background: -webkit-linear-gradient(top, #ffffff, #f1f1f1);
            background: -moz-linear-gradient(top, #ffffff, #f1f1f1);
            background: -ms-linear-gradient(top, #ffffff, #f1f1f1);
            background: linear-gradient(top, #ffffff, #f1f1f1);
            display: inline-block;
            border: 1px solid #ddd;
        }
        .pageLogin .btn2:hover
        {
            background: #f0f6fc;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#f0f6fc', endColorstr='#f0f6fc');
            border: 1px solid #dae1e8;
            text-decoration: none;
            color: #2e88c6;
            cursor: pointer;
        }
        
        /*加盟*/
        .tip
        {
            position: absolute;
            top: 50%;
            margin-top: -140px;
            left: 50%;
            margin-left: -225px;
            background: #fff;
            z-index: 999;
            overflow: hidden;
            width: 450px;
            height: 280px;
            display: none;
        }
        .tiptop
        {
            line-height: 40px;
            font-size: 14px;
            color: #ccc;
            padding-left: 10px;
            position: relative;
            background: #888;
        }
        .tipinfo
        {
            padding-top: 20px;
        }
        .tipinfo .lb
        {
            padding-top: 10px;
            padding-left: 10px;
            font-size: 14px;
            line-height: 25px;
        }
        .tipinfo .title
        {
            display: inline-block;
            width: 100%;
            text-align: center;
            color: #606a74;
            font-size: 24px;
        }
        .tipinfo .tel
        {
            color: #999;
            font-size: 12px;
            text-align: center;
            display: block;
            padding-top: 5px;
        }
        .tipinfo .box2
        {
            border: 1px solid #c3c3c3;
            height: 48px;
            line-height: 48px;
            width: 290px;
            border-radius: 3px;
            margin-left: 75px;
            box-shadow: 2px 2px 1px 0px #eee inset;
            font-size: 30px;
            letter-spacing: 27px;
            text-indent: 25px;
        }
        .tipinfo .txt
        {
            color: #da4444;
            text-align: center;
            padding: 9px 0;
            font-size: 12px;
            display: block;
        }
        .tipinfo .txt2
        {
            border-top: 1px solid #d1d1d1;
            margin: 0px 20px;
            font-size: 12px;
        }
        .tipinfo .txt2 span
        {
            width: 260px;
            display: inline-block;
            text-align: center;
        }
        .tipinfo .txt2 .close2
        {
            margin-left: 60px;
        }
        .tipinfo .btn
        {
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffff', endColorstr='#fafafa');
            background: -webkit-linear-gradient(top, #ffffff, #fafafa);
            background: -moz-linear-gradient(top, #ffffff, #fafafa);
            background: -ms-linear-gradient(top, #ffffff, #fafafa);
            background: linear-gradient(top, #ffffff, #fafafa);
            border: 1px solid #ddd;
            border-radius: 5px;
            display: inline-block;
            line-height: 26px;
            padding: 0px 10px;
        }
        .tipinfo .btn:hover
        {
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#f57403', endColorstr='#f57403');
            background: #f57403;
            border: 1px solid #d66500;
            color: #fff;
            text-decoration: none;
        }
        .tiptop .close
        {
            background: url(Distributor/images/icon.png) no-repeat 0px -119px;
            width: 14px;
            height: 14px;
            cursor: pointer;
            display: inline-block;
            position: absolute;
            top: 12px;
            right: 12px;
            border: none;
            padding: 0;
        }
        .payOpt .li
        {
            margin-top: 2px;
        }
        .opacity
        {
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 2000px;
            background-color: #000;
            opacity: 0.3;
            z-index: 998;
            filter: alpha(opacity=30);
        }
    </style>
    <%--       <style>
.role-cur{ background:#fff; position:absolute;top:-15px; padding:5px 0 5px 6px; }
.role-cur .title{ color:#265fb4; padding:5px 0px 5px 10px; position:relative; }
.hx-i{width:17px; height:14px;background:url(images/icon2.0.png) no-repeat; position:relative; display:inline-block; background-position:-67px -267px; margin-right:5px; top:2px; }
.jx-i{width:17px; height:15px;background:url(images/icon2.0.png) no-repeat; position:relative; display:inline-block; background-position:-67px -290px; margin-right:5px; top:2px;}
.role-cur .list { overflow:hidden; color:#ddd; padding:0px 0 5px 10px; line-height:24px;}
.role-cur .list a{ float:left;width:115px;white-space:nowrap;text-overflow: ellipsis; overflow:hidden; display:inline-block;}
.role-cur .list i{ float:left; padding:0px 13px;}
    </style>--%>
    <style>
        .role
        {
            width: 190px;
        }
        .role-cur
        {
            background: #fff;
            position: absolute;
            top: -15px;
            padding: 5px 0 5px 6px;
        }
        .role-cur .title
        {
            color: #265fb4;
            padding: 5px 0px 5px 10px;
            position: relative;
        }
        .hx-i
        {
            width: 17px;
            height: 14px;
            background: url(images/icon2.0.png) no-repeat;
            position: relative;
            display: inline-block;
            background-position: -67px -267px;
            margin-right: 5px;
            top: 2px;
        }
        .jx-i
        {
            width: 17px;
            height: 15px;
            background: url(images/icon2.0.png) no-repeat;
            position: relative;
            display: inline-block;
            background-position: -67px -290px;
            margin-right: 5px;
            top: 2px;
        }
        .role-cur .list
        {
            overflow: hidden;
            color: #ddd;
            padding: 0px 0 5px 10px;
            line-height: 24px;
        }
        .role-cur .list a
        {
            white-space: nowrap;
            text-overflow: ellipsis;
            overflow: hidden;
            display: block;
        }
    </style>
    <div class="box2Login" runat="server" id="LoginBox2">
        <!--用户登录 start-->
        <div class="pageLogin" id="defaultlogin" runat="server">
            <div class="bt">
                帐号登录<%--<a id="aphonelogin" style="right: 10px;" href="javascript:;" class="tw PhoneLg">手机登录</a>--%></div>
            <div class="li">
                <input id="txtuid" autocomplet="off" type="text" runat="server" class="box" onfocus="if(value==defaultValue){value='';this.style.color='#555'}"
                    onblur="if(!value){value=defaultValue;this.style.color='#999'}" value="帐号" maxlength="40" /></div>
            <div class="li">
                <input id="txtpwd" type="password" class="box" onfocus="if(value==defaultValue){value='';this.style.color='#555'}"
                    placeholder="密码" onblur="if(!value){value=defaultValue;this.style.color='#999'}"
                    maxlength="40" /></div>
            <div class="li">
                <input id="txtcode" style="width: 190px;" type="text" class="box2" onfocus="if(value==defaultValue){value='';this.style.color='#555'}"
                    onblur="if(!value){value=defaultValue;this.style.color='#999'}" value="验证码" autocomplete="off" />
                <span class="captcha">
                    <img id="ckcode" style="cursor: pointer;" alt="看不清？点击更换" onclick="this.src='/UserControl/CheckCode.aspx?t='+new Date().getTime()"
                        src="/UserControl/CheckCode.aspx" width="90" title="看不清?点击更换" height="35" />
                </span>
            </div>
            <div class="btn">
                <a id="btnlogin" href="javascript:;" style="cursor: pointer;">登录</a></div>
            <div class="txt">
                <span id="erlogin" style="display: none; padding: 0px 10px 0 0px; color: red;"><span
                    style="position: relative; top: 4px; margin: 0px 5px;">
                    <img src="images/icoerror.gif" alt="暂无图片" /></span><i id="erpwd"></i></span>
                <a class="A_defaultRegis" style="display: none;" href="javascript:window.parent.location.href='CompRegister.aspx<%=Request["comid"]=="0"?"":("?comid="+Request["comid"])%>';window.parent.layerCommon.layerClose(1);">
                    申请合作</a><a href="javascript:window.parent.location.href='findPwd.aspx';window.parent.layerCommon.layerClose(1)">忘记密码
                        ?</a></div>
            <div class="bg">
            </div>
        </div>
        <!--用户登录 end-->
        <div class="blank10">
        </div>
        <!--手机登录 start-->
        <div class="pageLogin" id="Phonelogin" runat="server">
            <div class="bt">
                手机登录<a href="javascript:;" style="right: 10px;" class="tw AccountLg">帐号登录</a></div>
            <div class="li">
                <input id="txtPhone" type="text" runat="server" class="box" maxlength="11" onfocus="if(value==defaultValue){value='';this.style.color='#555'}"
                    onblur="if(!value){value=defaultValue;this.style.color='#999'}" value="输入帐号绑定的手机" /></div>
            <div class="li">
                <input id="txtPcode" type="text" class="box2" style="width: 190px;" onfocus="if(value==defaultValue){value='';this.style.color='#555'}"
                    onblur="if(!value){value=defaultValue;this.style.color='#999'}" value="请输入验证码"
                    autocomplete="off" />
                <span class="captcha">
                    <img id="ckcode" style="cursor: pointer;" alt="看不清？点击更换" onclick="this.src='/UserControl/CheckCode.aspx?t='+new Date().getTime()"
                        src="/UserControl/CheckCode.aspx" width="90" title="看不清?点击更换" height="35" /></span>
            </div>
            <div class="li">
                <input id="txtPhoneCode" type="text" class="box2" style="width: 190px;" onfocus="if(value==defaultValue){value='';this.style.color='#555'}"
                    onblur="if(!value){value=defaultValue;this.style.color='#999'}" value="请输入短信验证码" />
                <a id="getcode" class="btn2 captcha" onclick="SendCode()">获取验证码</a>
            </div>
            <div class="btn">
                <a id="btnPhoneLogin" href="javascript:;" style="cursor: pointer;">登录</a></div>
            <div class="txt">
                <span id="erlogin" style="padding: 0px 10px 0 0px; color: red; display: none;"><span
                    style="position: relative; top: 4px; margin: 0px 5px;">
                    <img src="images/icoerror.gif" alt="暂无图片" /></span><i id="erpwd"></i></span>
                <a class="A_PhoneRegis" style="display: none;" href="javascript:window.parent.location.href='CompRegister.aspx<%= Request["comid"]=="0"?"":("?comid="+Request["comid"])%>';window.parent.layerCommon.layerClose(1);">
                    申请合作</a></div>
            <div class="bg">
            </div>
        </div>
        <!--手机登录 end-->
        <!--申请加盟 start-->
        <%--    <div class="tip" style="display: none;">
            <div class="tiptop"><span>加盟验证 — 上海my1818企业</span><input name="" type="button" class="cancel close" value=""></div>
            <div class="tipinfo">
        	    <div class="lb"><span class="title">请输入短信验证码</span><i id="phone" class="tel">（已发送至137 **** 0677）</i></div>
                <div class="lb">
                    <input name="txtPhoneNum" type="text" id="txtPhoneNum" class="box2" autocomplete="off">
                    <span style="display:none; margin-left:195px;" id="paying">正在支付...</span>
                    <input type="submit" name="btnTx1376" value="确认"  id="btnTx1376" style="display:none;">
                </div>
                <div class="lb txt"><span>短信验证码已发送，请注意查收</span></div>
			    <div class="lb txt2">
                    <span>
                    	<i class="" id="msgone">97秒后短信验证码失效</i> 
                        <i class="" id="msgtwo" style="display: none;">验证码已失效，请关闭窗口，重新支付！</i> 
                    </span>|<a href="javascript:void(0)" class="cancel close2">关闭</a>
                </div>	
		    </div>
        </div>
        
    <div class="tip" style="display: none;">
            <div class="tiptop"><span>加盟验证 — 上海my1818企业</span><input name="" type="button" class="cancel close" value=""></div>
            <div class="tipinfo">
        	    <div class="lb" style="margin-top:60px;"><span class="title">加盟申请已提交，等待审核...</span></div>
		    </div>
        </div>--%>
        <!--申请加盟 end-->
        <%= WriteHTML %>
    </div>
    <link href="css/root.css" rel="stylesheet" type="text/css" />
    </form>
</body>
</html>

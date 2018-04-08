<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>登录页 医站通</title>
    <meta name="keywords" content="我的1818_我的1818网_医站通_网上分销_电子商务_B2B_批发_采购" />
    <meta name="description" content="我的1818网（医站通.com）,中小企业网上分销平台,为您寻找产品批发分销渠道,提供订单管理、代理商管理、资金对账、数据分析等功能,建立企业网上分销管理平台" />
    <script src="Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="js/GetPhoneCode.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        rel="stylesheet" type="text/css" />
    <link href="css/login.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        rel="stylesheet" type="text/css" />
    <link href="css/global.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        rel="stylesheet" type="text/css" />
    <link href="css/index.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/superslide.2.1.js"></script>
    <script src="js/layer/layer.js" type="text/javascript"></script>
    <script src="js/CommonSha.js" type="text/javascript"></script>
    <script src="js/layerCommon.js" type="text/javascript"></script>
    <%--<script type="text/javascript">
        var _hmt = _hmt || [];
        (function () {
            var hm = document.createElement("script");
            hm.src = "https://hm.baidu.com/hm.js?779e9b3d086d94ec0ead28ec3dd99190";
            var s = document.getElementsByTagName("script")[0];
            s.parentNode.insertBefore(hm, s);
        })();
    </script>--%>
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
                    $.ajax({
                        type: "POST",
                        url: 'Controller/login.ashx',
                        data: { SubmitAcion: "PhoneLogin", Phone: $.trim($("#txtPhone").val()), PhoneCode: $.trim($("#txtPhoneCode").val()), Code: $.trim($("#txtPcode").val()), chklogin: $.trim($("#chklogin").is(":checked")) },
                        dataType: "json",
                        beforeSend: function () { $this.data("IsEnabled", true).addClass("ebled").css({ "cursor": "default" }).text("正在登录..."); },
                        timeout: 5000,
                        cache: false,
                        success: function (ReturnData) {
                            if (ReturnData.Result) {
                                $("#erlogin", ".mianLogin#Phonelogin").hide();
                                !ReturnData.IsMoreUser ? function () {
                                    //switch (ReturnData.Type) {
                                    //    case 1:
                                    //    case 5: location.href = "/" + ReturnData.Compid + ".html"; break;
                                    //    case 3:
                                    //    case 4: location.href = "/" + ReturnData.Compid + ".html"; break;
                                    //    default: location.href = "/" + ReturnData.Compid + ".html";
                                    //}
                                    //edit by hgh 0421 登录后直接跳转到管理中心
                                    switch (ReturnData.Type) {
                                        case 1:
                                        case 5: location.href = "/Distributor/UserIndex.aspx"; break;
                                        case 3:
                                        case 6:
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
                                    $this.data("IsEnabled", false).removeClass("ebled").css("cursor", "pointer").text("登录");
                                } ();
                            }
                            else {
                                $("#erlogin", ".mianLogin#Phonelogin").show().find("#erpwd").text(ReturnData.Msg);
                                $this.data("IsEnabled", false).removeClass("ebled").css("cursor", "pointer").text("登录");
                            }
                        },
                        error: function () {
                            $this.data("IsEnabled", false).removeClass("ebled").css("cursor", "pointer").text("登录");
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
                if ($.trim($("#txtuid").val()) == "" || $.trim($("#txtuid").val()) == "帐号/手机") {
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
                $.ajax({
                    type: "POST",
                    url: 'Controller/login.ashx',
                    data: { SubmitAcion: "AccuntLogin", Username: $.trim($("#txtuid").val()), Password: hex_one(hex_one(hex_two($.trim($("#txtpwd").val())))), Code: $.trim($("#txtcode").val()), chklogin: $.trim($("#chklogin2").is(":checked")) },
                    timeout: 5000,
                    beforeSend: function () { $this.data("IsEnabled", true).addClass("ebled").css({ "cursor": "default" }).text("正在登录..."); },
                    cache: false,
                    dataType: "json",
                    success: function (ReturnData) {
                        if (ReturnData.Result) {
                            $("#erlogin", ".mianLogin#defaultlogin").hide();
                            !ReturnData.IsMoreUser ? function () {
                                //switch (ReturnData.Type) {
                                //    case 1:
                                //    case 5: location.href = "/" + ReturnData.Compid + ".html"; break;
                                //    case 3:
                                //    case 4: location.href = "/" + ReturnData.Compid + ".html"; break;
                                //    default: location.href = "/" + ReturnData.Compid + ".html";
                                //}
                                //edit by hgh 0421 登录后直接跳转到管理中心
                                switch (ReturnData.Type) {
                                    case 1:
                                    case 5: location.href = "/Distributor/UserIndex.aspx"; break;
                                    case 3:
                                    case 6:
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
                                    $this.data("IsEnabled", false).removeClass("ebled").css("cursor", "pointer").text("登录");
                                }, false);
                            } ();
                        }
                        else {
                            $this.data("IsEnabled", false).removeClass("ebled").css("cursor", "pointer").text("登录"), $("#erlogin", ".mianLogin#defaultlogin").show().find("#erpwd").text(ReturnData.Msg), $("#ckcode").click(), $("#txtcode").val('');
                        }
                    },
                    error: function () {
                        $this.data("IsEnabled", false).removeClass("ebled").css("cursor", "pointer").text("登录");
                        $("#erlogin", ".mianLogin#defaultlogin").show().find("#erpwd").text("服务器或网络异常，请重试");
                    }
                });

            });


        })
    </script>
</head>
<body class="root">
    <form id="form1" runat="server">
    <!--页头 start-->
    <div class="plat-header" style="padding-top: 0;">
        <div class="logo fl">
            <a href="<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>">
                <img src="images/logo2.0.png"  height="60" alt="登录"></a></div>
        <div class="menu fl">
        </div>
        <div class="p-app fr">
            <span>
                <img src="images/app-qr.png" width="60" height="60" alt="下载医站通"></span><i>下载医站通</i></div>
    </div>
    <!--页头 end-->
    <div class="fullSlide">
        <div class="bd">
            <ul>
                <!--<li _src="url(images/banner1.jpg)" ><a href="#"></a></li>-->
                <li _src="url(images/banner4.jpg)"><a href="#"></a></li>
                <%--<li _src="url(images/banner2.jpg)"><a href="#"></a></li>--%>
            </ul>
        </div>
        <div class="hd">
            <ul>
            </ul>
        </div>
        <span class="prev"></span><span class="next"></span>
        <!--footer start-->
        <div class="mianFooter">
            <div class="txt">
                <%--<a href="/platform.html">平台介绍</a>|<a href="/about_1.html">关于我们</a>|<a href="/help/help.html">帮助中心</a>|<a
                    href="/about_3.html">联系我们</a>|<a href="/statement_1.html">网站声明</a><br />--%>
                <a href="https://www.yibanmed.com/" target="_blank">平台介绍</a>|<a href="https://www.yibanmed.com/" target="_blank">关于我们</a>|<a href="https://www.yibanmed.com/" target="_blank">帮助中心</a>|
                <a href="https://www.yibanmed.com/" target="_blank">联系我们</a>|<a href="https://www.yibanmed.com/" target="_blank">网站声明</a><br />
                粤ICP备17130448号-2 Copyright © 2017 深圳前海医伴金服信息技术有限公司版权所有
            </div>
            <div class="bg">
            </div>
        </div>
        <!--footer end-->
    </div>
    <div class="boxLogin">
        <!--用户登录 start-->
        <div class="mianLogin" id="defaultlogin" runat="server">
            <div class="bt">
                登 录<%--<a id="aphonelogin" href="javascript:;" class="tw PhoneLg">手机登录</a>--%>
            </div>
            <div class="li">
                <input id="txtuid" autocomplete="off" type="text" runat="server" class="box2" onfocus="if(value==defaultValue){value='';this.style.color='#555'}"
                    onblur="if(!value){value=defaultValue;this.style.color='#999'}" value="帐号/手机" />
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
                        src="UserControl/CheckCode.aspx" width="80" title="看不清?点击更换" height="35" /></span>
            </div>
            <div class="btn">
                <a class="btnlogin" href="javascript:;" style="cursor: pointer;" id="btnlogin">登 录</a></div>
            <div class="txt">
                <input type="checkbox" id="chklogin2" checked="checked"  runat="server" name="chklogin2" style="float: left; margin: 2px 0 0 16px" /><label
                    for="chklogin2" style="color: #777; float: left;">&nbsp;自动登录</label>
                <a href="compOrdisRegister.html">快速注册</a> <a style="padding-right: 0px;" href="findPwd.html">
                    忘记密码</a></div>
            <div class="txt2">
                <span id="erlogin" style="padding: 0px 10px 0 0px; color: red; display: none;"><span
                    style="position: relative; top: 4px; margin: 0px 5px;">
                    <img src="images/icoerror.gif" alt="暂无图片" /></span><i id="erpwd"></i></span></div>
            <div class="bg">
            </div>
        </div>
        <!--用户登录 end-->
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
                        src="UserControl/CheckCode.aspx" width="80" title="看不清?点击更换" height="35" /></span>
            </div>
            <div class="li">
                <input id="txtPhoneCode" type="text" class="box boxw" style="width: 180px;" onfocus="if(value==defaultValue){value='';this.style.color='#555'}"
                    onblur="if(!value){value=defaultValue;this.style.color='#999'}" value="请输入短信验证码" />
                <a id="getcode" class="btn2 captcha">获取验证码</a>
            </div>
            <div class="btn">
                <a href="javascript:;" id="btnPhoneLogin">登 录</a></div>
            <div class="txt">
                <input type="checkbox" id="chklogin" checked="checked" runat="server" name="chklogin" style="float: left; margin: 2px 0 0 16px" /><label
                    for="chklogin" style="color: #777; float: left;">&nbsp;自动登录</label>
                <a href="compOrdisRegister.html">快速注册</a></div>
            <div class="txt2">
                <span id="erlogin" class="left" style="display: none;"><span style="position: relative;
                    top: 4px; margin: 0px 5px;">
                    <img src="images/icoerror.gif" alt="暂无图片" /></span><i id="erpwd"></i></span>
            </div>
            <div class="bg">
            </div>
        </div>
        <!--手机登录 end-->
    </div>
    <script type="text/javascript">
        $(".fullSlide").hover(function () {
            $(this).find(".prev,.next").stop(true, true).fadeTo("show", 0.1)
        },
	    function () {
	        $(this).find(".prev,.next").fadeOut()
	    });
        $(".prev,.next").hover(function () {
            $(this).fadeTo("show", 0.5);
        }, function () {
            $(this).fadeTo("show", 0.1);
        })
        $(".fullSlide").slide({
            titCell: ".hd ul",
            mainCell: ".bd ul",
            effect: "fold",
            autoPlay: true,
            autoPage: true,
            trigger: "click",
            startFun: function (i) {
                var curLi = jQuery(".fullSlide .bd li").eq(i);
                if (!!curLi.attr("_src")) {
                    curLi.css("background-image", curLi.attr("_src")).removeAttr("_src")
                }
            }
        });
        
          $(".fullSlide").slide({
              titCell: ".hd ul",
              mainCell: ".bd ul",
              effect: "fold",
              autoPlay: true,
              autoPage: true,
              trigger: "click",
              startFun: function (i) {
                  var curLi = jQuery(".fullSlide .bd li").eq(i);
                  if (!!curLi.attr("_src")) {
                      curLi.css("background-image", curLi.attr("_src")).removeAttr("_src")
                  }
              }
          });

	</script>
    
<!-- QQ客服 region -->

    <div id="float_service">
        <div class="float_start" style="opacity: 0.7; letter-spacing: 0.5em; border-radius: 10px 10px 0px 0px;
            font-size: 14px; width: 180px; height: 35px; line-height: 35px; cursor: pointer;
            color: rgb(255, 255, 255); text-indent: 2em; font-family: 宋体, simSun; font-weight: bold;
            bottom: 0px; visibility: visible; background-color: #2059ae">
            在线服务 <em class="float_open" style="position: absolute; top: 10px; right: 30px; width: 20px;
                height: 20px; background: url(../images/floatservice-bg-5.png) -20px -460px no-repeat;">
            </em>
        </div>
        <div class="float_contact" style="width: 192px; height: 442px; float: left; background-image: url(../images/floatservice-bg-5.png);">
            <em class="float_close" style="position: absolute; top: 50px; right: 30px; width: 20px;
                height: 20px; border-radius: 3px; cursor: pointer; opacity: 0.7; background: url(../images/floatservice-bg-5.png) 0px -460px no-repeat;">
            </em>
            <ul style="margin-top: 105px; padding-left: 25px; list-style: none; font-size: 14px;">
                <li><a target="_blank" id="service_qq1" href="http://crm2.qq.com/page/portalpage/wpa.php?uin=4009619099&f=1&ty=1&aty=0&a=&from=6"
                    style="transition-duration: 0.5s; width: 95px; height: 18px; padding: 11px 20px;
                    display: block; color: rgb(255, 255, 255); background-color: rgb(32, 89, 174);">
                    <em style="display: inline-block; background: url(../images/floatservice-bg-5.png) 0 -509px no-repeat;
                        width: 20px; height: 20px; float: left; margin-right: 5px;"></em><span style="float: left;
                            line-height: 18px;">在线客服</span> </a></li>
                <li style="margin-top: 12px;"><a id="liuyan" href="javascript:void(0)" style="transition-duration: 0.5s;
                    width: 95px; height: 18px; padding: 11px 20px; display: block; color: rgb(255, 255, 255);
                    background-color: rgb(119, 179, 108);"><em style="display: inline-block; background: url(../images/floatservice-bg-5.png) -20px -509px no-repeat;
                        width: 20px; height: 20px; float: left; margin-right: 5px;"></em><span style="float: left;
                            line-height: 18px;">留言反馈</span> </a></li>
            </ul>
        </div>
    </div>
    <!-- QQ客服 end -->
    <script type="text/javascript">
        $(document).ready(function () {

            function b() {
                var b = {
                    clientW: document.body.clientWidth,
                    clientH: document.body.clientHeight,
                    scrollTop: $(window).scrollTop()
                }
              , c = {
                  left: b.clientW / 2 - ($("#ly_content").width() / 2),
                  top: b.scrollTop + 50
              };
                $("#ly_content").css({
                    left: c.left
                }).animate({
                    top: c.top
                })
            };
            $.fn.extend({ defaultValue: function () {
                var c = function () {
                    var a = document.createElement("input");
                    return "placeholder" in a
                } ();
                c ? !1 : this.each(function (index, control) {
                    $(control).data("defaultValued", $(control).attr("placeholder")),
                $(control).val($(control).data("defaultValued")),
                $(control).undelegate("focus").delegate("", "focus", function () {
                    if ($.trim($(this).val()) == $(this).data("defaultValued")) {
                        $(this).val("");
                    }
                }),
                $(control).undelegate("blur").delegate("", "blur", function () {
                    if ($.trim($(this).val()) == "") {
                        $(this).val($(this).data("defaultValued"));
                    }
                })
                })
            }
            }),
        $("#float_service").css({ position: "fixed", right: "10px", bottom: "-442px", overflow: "hidden", zIndex: "2007" }).on({
            "mouseenter": function () {
                $("#float_service").stop().animate({
                    bottom: 0,
                    height: "426px"
                }),
                    $(".float_start").css({
                        opacity: 0,
                        visibility: "hidden"
                    });
            },
            "mouseleave": function () {
                $("#float_service").stop().animate({
                    bottom: "-392px",
                    height: "426px"
                }, function () {
                    $(".float_start").css({
                        visibility: "visible"
                    }).animate({
                        opacity: 0.7
                    })
                });
            }
        }),
        $("#liuyan").css({
            "transition-duration": "0.5s",
            width: "95px",
            height: "18px",
            padding: "11px 20px",
            display: "block",
            backgroundColor: "#77b36c",
            color: "#FFFFFF"
        }).on({
            mouseenter: function () {
                $(this).css({
                    "transition-duration": "0.5s",
                    width: "95px",
                    height: "18px",
                    padding: "11px 20px",
                    display: "block",
                    backgroundColor: "#508945",
                    color: "#FFFFFF"
                })
            },
            mouseleave: function () {
                $(this).css({
                    "transition-duration": "0.5s",
                    width: "95px",
                    height: "18px",
                    padding: "11px 20px",
                    display: "block",
                    backgroundColor: "#77b36c",
                    color: "#FFFFFF"
                })
            }
        }).on("click", function () {
            $(".float_close").trigger("click");
            var e = '<div id="ly_lockmask" style="position: fixed;filter:alpha(opacity=40); left: 0px; top: 0px; width: 100%; height: 100%; overflow: hidden; z-index: 2008;background: #000000;opacity: .4;"></div>'
                  , f = '<div id="ly_content" style="left: 581px; top: -400px; visibility: visible; position: absolute; width: 410px;height:560px;background-color:#FFFFFF; z-index: 2028;" class=""><div style="overflow:hidden;line-height:40px;"><a id="closeContent" href="javascript:void(0)" style="float:right;font-size:16px;font-family:arial;margin-right:10px;color:#555555;text-decoration:none;">X</a></div><div style="width:320px;margin:0 auto;"><div style="width:320px;height:47px;background:url(../images/floatservice-bg-5.png) 0 -529px no-repeat;"></div><div style="width:320px;height:42px;margin-top:10px;"><input id="name" maxlength="20" placeholder="怎么称呼您？" style="color:#555555;border:1px solid #cccccc;line-height:18px;height:20px;width:298px;padding:10px;" type="text" /></div><div style="width:320px;height:42px;margin-top:10px;"><input id="phone" maxlength="11" placeholder="请留下您的手机号码" style="color:#555555;border:1px solid #cccccc;line-height:18px;height:20px;width:298px;padding:10px;" type="text" /></div><div style="width:320px;height:42px;margin-top:10px;"><input id="qq" maxlength="20" placeholder="请留下您的邮箱或QQ号码" style="color:#555555;border:1px solid #cccccc;line-height:18px;height:20px;width:298px;padding:10px;" type="text" /></div><div style="width:320px;height:42px;margin-top:10px;"><img style="width:80px;height:42px;float:right;" id="verifyCodeImg" src="" alt="" /><input id="verifyCode" maxlength="4" placeholder="验证码" style="color:#555555;border:1px solid #cccccc;line-height:18px;height:20px;width:218px;padding:10px;" type="text" /></div><div style="width:320px;height:150px;margin-top:10px;"><textarea id="content" maxlength="2000" placeholder="如有产品咨询请您留言；如咨询合作代理请务必同时留下您的qq和手机" style="color:#555555;border:1px solid #cccccc;line-height:24px;height:128px;width:298px;padding:10px;" name="" id="" cols="30" rows="10"></textarea></div><div style="width:320px;height:42px;margin-top:20px;"><a id="send" style="width:320px;height:40px;line-height:40px;color:#FFFFFF;text-align:center;background-color:rgb(32, 89, 174);display:block;font-size:16px;letter-spacing:1px;" href="javascript:void(0)">提交您的留言</a></div></div></div>';
            0 == $("#ly_content").length && ($("body").append(e),
                $("body").append(f),
                $("#verifyCodeImg", "#ly_content").attr("src", "UserControl/CheckCode.aspx"),
                $("#verifyCodeImg", "#ly_content").undelegate("click").delegate("", "click", function () {
                    $(this).attr("src", '/UserControl/CheckCode.aspx?t=' + new Date().getTime());
                },
                $("#name", "#ly_content").defaultValue(),
                $("#phone", "#ly_content").defaultValue(),
                $("#qq", "#ly_content").defaultValue(),
                $("#verifyCode", "#ly_content").defaultValue(),
                $("#content", "#ly_content").defaultValue()
                ), b(),
              $("#closeContent", "#ly_content").undelegate("click").delegate("", "click", function () {
                  $("#ly_lockmask").animate({
                      opacity: 0
                  }, 300, function () {
                      $(this).remove()
                  }),
                    $("#ly_content").animate({
                        opacity: 0
                    }, 300, function () {
                        $(this).remove()
                    })
              }),
              $("#send", "#ly_content").undelegate("click").delegate("", "click", function () {
                  var Msg =
                  {
                      name: $.trim($("#name", "#ly_content").val()),
                      Phone: $.trim($("#phone", "#ly_content").val()),
                      MailQQ: $.trim($("#qq", "#ly_content").val()),
                      Code: $.trim($("#verifyCode", "#ly_content").val()),
                      Msg: $.trim($("#content", "#ly_content").val())
                  };
                  return (Msg.name == "" || Msg.name == $("#name", "#ly_content").data("defaultValued")) || (Msg.Phone == "" || Msg.Phone == $("#phone", "#ly_content").data("defaultValued")) || (Msg.Code == "" || Msg.Code == $("#verifyCode", "#ly_content").data("defaultValued")) || (Msg.Msg == "" || Msg.Msg == $("#content", "#ly_content").data("defaultValued")) ? (layerCommon.msg("请输入完整的留言信息", IconOption.哭脸)) : (void $.ajax({
                      url: "../Handler/SubmitUserMsg.ashx",
                      type: "get",
                      data: Msg,
                      dataType: 'json',
                      timeout: 4000,
                      success: function (ReturnData) {
                          if (ReturnData.Result) {
                              layerCommon.msg("留言成功，我们会尽快联系您！", IconOption.笑脸, 2500);
                              $("#ly_lockmask").animate({
                                  opacity: 0
                              }, 300, function () {
                                  $(this).remove()
                              }),
                             $("#ly_content").animate({
                                 opacity: 0
                             }, 300, function () {
                                 $(this).remove()
                             });
                          }
                          else {
                              layerCommon.msg(ReturnData.Msg, IconOption.哭脸);
                          }
                      }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                          if (XMLHttpRequest.statusText == "timeout") {
                              layerCommon.msg("提交留言超时，请稍候再试！", IconOption.哭脸);
                          } else {
                              layerCommon.msg("提交留言超时，请稍候再试！", IconOption.哭脸);
                          }
                      }
                  }));
              })
          );
        });


            $(".float_close").on("click", function () {
                $("#float_service").trigger("mouseleave");
            })
        });
    </script>
    </form>
</body>
</html>

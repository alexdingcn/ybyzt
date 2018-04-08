<%@ Page Language="C#" AutoEventWireup="true" CodeFile="findpwd.aspx.cs" Inherits="findPwd" %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/NavTopControl.ascx" TagPrefix="uc1" TagName="TopNav" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="Bottom" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>找回密码 医站通-B2B电子商务平台, 分销、批发就上医站通</title>
    <meta name="keywords" content="B2B电子商务,在线订货,手机订货,订货app,电商平台,分销系统,代理商管理,订货系统,管理系统,在线订单管理,网上订货系统,在线订货平台,订货软件,代理商系统" />
    <meta name="description" content="医站通-B2B电子商务平台是为贸易或生产企业开发的网上订单管理系统,实现厂商与代理商之间实时订货,付款,发货,收货,库存管理,收付款对帐管理,物流信息查询,安全的在线支付,在线客服等全面高效的订货流程管理,提升企业管理竞争力,分销、批发就上医站通.咨询热线:40077-40088" />
    <script src="Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="js/GetPhoneCode.js" type="text/javascript"></script>
    <script src="/js/layer/layer.js" type="text/javascript"></script>
    <script src="/js/layerCommon.js" type="text/javascript"></script>
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <link href="css/login.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <%--<script>
        var _hmt = _hmt || [];
        (function () {
            var hm = document.createElement("script");
            hm.src = "https://hm.baidu.com/hm.js?779e9b3d086d94ec0ead28ec3dd99190";
            var s = document.getElementsByTagName("script")[0];
            s.parentNode.insertBefore(hm, s);
        })();
    </script>--%>    
</head>
<body class="root">
    <form id="form1" runat="server">
    <!--topBar start-->
       <uc1:Top ID="top1" runat="server" />
      <uc1:TopNav ID="TopNav1" runat="server" ShowID="" />
    <!--nav end-->
    <div class="login">
        <script type="text/javascript">
            $(function () {
                $(document).on("keydown", function (e) {
                    if (e.keyCode == 13) {
                        if ($(".pawoOne").is(":visible")) {
                            findtel();
                        }
                        else if ($(".patwo").is(":visible")) {
                            checkphonecode();
                        }
                        else if ($(".paThree").is(":visible")) {
                            updatePwd();
                        }
                    }
                })
            })
            //验证用户，获取手机号码
            function findtel() {
                if ($.trim($("#txtuid").val()) == "") {
                    $("#txtuid").attr("class", "redbox");
                } else {
                    $("#txtuid").attr("class", "box");
                }

                if ($.trim($("#txtcode").val()) == "") {
                    $("#txtcode").attr("class", "redbox2");
                } else {
                    $("#txtcode").attr("class", "box2");
                }
                if ($.trim($("#txtuid").val()) != "" && $.trim($("#txtcode").val()) != "") {
                    $.ajax({
                        url: 'Controller/FindPwd.ashx',
                        data: {
                            username: $.trim($("#txtuid").val()),
                            code: $.trim($("#txtcode").val())
                        },
                        dataType: "json",
                        async: false,
                        success: function (usercontent) {
                            if (usercontent.type) {
                                var usertel = usercontent.usertel;
                                var userid = usercontent.userid;
                                var addd = $("#txtuid").val();
                                var usertype = usercontent.typeid;
                                $(".pawoOne").removeClass("show")
                                $(".patwo").addClass("show");
                                $("#txtusertel").text(usertel);
                                $("#hidusername").val(addd);
                                $("#hiduserid").val(userid);
                                $("#hidusertype").val(usertype);
                                $("#hidusertel").val(usertel);
                            }
                            else {
                                layerCommon.msg(usercontent.str, IconOption.错误);
                            }
                        },
                        error: function () {
                        }
                    });
                }
            }


            //获取手机验证码

            function getptelcode() {
                var username = $("#txtuid").val();
                var userid = $("#hiduserid").val();
                var usertel = $("#hidusertel").val();
                var usertype = $("#hidusertype").val();
                if ($.trim(username) == "") {
                    layerCommon.msg("请验证帐号后再获取手机验证码", IconOption.错误, 2500);
                    $(".pawoOne").removeClass("show");
                    $(".paone").addClass("show");
                }
                getCode(usertel, usertype, "修改登录密码", userid, username, function () { $("#showtext").text("校验码已发出，请注意查收短信"); });
            }
            //验证手机验证码
            function checkphonecode() {
                var userphone = $("#hidusertel").val();
                var username3 = $("#hidusername").val();
                var phonecode = $("#txtphonecode").val();
                $.ajax({
                    type: "post",
                    url: "findPwd.aspx",
                    data: { action: "checktelcode", username3: username3, userphone3: userphone, phonecode3: phonecode },
                    dataType: "json",
                    success: function (data) {
                        if (data.type) {
                            $(".pawoOne").removeClass("show")
                            $(".paThree").addClass("show");
                        }
                        else {
                            layerCommon.msg(data.str, IconOption.错误, 2500);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        var a;
                    }
                });
            }

            function ChangeClass() {
                $("#newpwd1").attr("class", "box");
                $("#newpwd2").attr("class", "box");
            }

            //修改密码
            function updatePwd() {
                if ($.trim($("#newpwd1").val()) == "") {
                    $("#newpwd1").attr("class", "redbox");
                    return;
                } else {
                    $("#newpwd1").attr("class", "box");
                }
                
                if ($.trim($("#newpwd2").val()) == "") {
                    $("#newpwd2").attr("class", "redbox");
                    return;
                } else {
                    $("#newpwd2").attr("class", "box");
                }
                if (($.trim($("#newpwd2").val()) != $.trim($("#newpwd1").val())) || ($.trim($("#newpwd1").val()) == "")) {
                    layerCommon.msg("两次密码不一致", IconOption.错误);
                    return;
                }
                if ($.trim($("#newpwd2").val()) == "123456" || $.trim($("#newpwd1").val()) == "123456") {
                    layerCommon.msg("密码过于简单", IconOption.错误);
                    return;
                }
                if (($.trim($("#newpwd2").val()) == $.trim($("#newpwd1").val())) && ($.trim($("#newpwd2").val()) != "")) {
                    $.ajax({
                        type: "post",
                        url: "findPwd.aspx",
                        data: { action: "updatepwd", pwd1: $.trim($("#newpwd1").val()), pwd2: $.trim($("#newpwd2").val()), userid: $("#hiduserid").val(), username1: $("#hidusername").val(), userphone: $.trim($("#hidusertel").val()), Phonecode: $.trim($("#txtphonecode").val()) },
                        dataType: "json",
                        success: function (data) {
                            if (data.type) {
                                $(".pawoOne").removeClass("show")
                                $(".paFour").addClass("show");
                            }
                            else {
                                if (data.code == "error") {
                                    $(".pawoOne").removeClass("show");
                                    $(".paone").addClass("show");
                                }
                                layerCommon.msg(data.str, IconOption.错误, 2500);
                            }
                        },
                        error: function () {
                        }
                    });
                }
            }

        </script>
        <div class="password">
            <div class="title">
                忘记密码</div>
            <!--填写号码 start-->
            <div class="pawoOne paone show" id="one" runat="server">
                <ul class="step">
                    <li class="cur"><span><i>...</i></span><i class="txt">填写帐号</i></li>
                    <li><span><i>2</i></span><i class="txt">验证身份</i></li>
                    <li><span><i>3</i></span><i class="txt">重置密码</i></li>
                    <li><span><i>4</i></span><i class="txt">修改成功</i></li>
                </ul>
                <div class="bg">
                </div>
                <ul class="lb">
                    <li><i class="bt">登录帐号：</i><input id="txtuid" type="text" class="box" maxlength="40" /></li>
                    <li><i class="bt">　验证码：</i><input id="txtcode" type="text" class="box2" maxlength="10" onkeydown='if(event.keyCode==13){findtel()}'/><span class="capt">
                        <img id="ckcode" style="top: 5px; left: 355px;" alt="看不清？点击更换" onclick="this.src=this.src+'?'"
                            src="UserControl/CheckCode.aspx" width="80" height="30" /></span><a onclick="$('#ckcode').trigger('click');" href="javascript:;" class="link">&nbsp;看不清？换一张</a></li>
                    <li><a class="btn" onclick="findtel()" style="cursor: pointer;">下一步</a></li>
                </ul>
            </div>
            <!--填写号码 end-->
            <!--验证身份 start-->
            <div class="pawoOne patwo" id="two" runat="server">
                <ul class="step">
                    <li><span><i>1</i></span><i class="txt">填写号码</i></li>
                    <li class="cur"><span><i>...</i></span><i class="txt">验证身份</i></li>
                    <li><span><i>3</i></span><i class="txt">重置密码</i></li>
                    <li><span><i>4</i></span><i class="txt">修改成功</i></li>
                </ul>
                <div class="bg">
                </div>
                <ul class="lb">
                    <li><i class="bt">已验证手机：</i><i class="txt2" id="txtusertel"></i></li>
                    <li><i class="bt">手机验证码：</i><input id="txtphonecode" type="text" class="box2" maxlength="10" /><a
                        class="btn2" style="cursor: pointer; text-decoration:none;" onclick="getptelcode()" id="getcode">获取验证码</a><i class="gray"
                            id="showtext"></i></li>
                    <li><a class="btn" onclick="checkphonecode()" style="cursor: pointer;">下一步</a></li>
                </ul>
            </div>
            <!--验证身份 end-->
            <!--重置密码 start-->
            <div class="pawoOne paThree" id="showthree" runat="server">
                <ul class="step">
                    <li><span><i>1</i></span><i class="txt">填写号码</i></li>
                    <li><span><i>2</i></span><i class="txt">验证身份</i></li>
                    <li class="cur"><span><i>...</i></span><i class="txt">重置密码</i></li>
                    <li><span><i>4</i></span><i class="txt">修改成功</i></li>
                </ul>
                <div class="bg">
                </div>
                <ul class="lb">
                    <li><i class="bt">新登录密码：</i><input id="newpwd1" type="password" class="box" maxlength="40" onfocus="ChangeClass()"/></li>
                    <li><i class="bt">确认新密码：</i><input id="newpwd2" type="password" class="box" maxlength="40" onfocus="ChangeClass()"/></li>
                    <li><a class="btn" style="cursor: pointer;" onclick="updatePwd()">提交</a></li>
                </ul>
            </div>
            <!--重置密码 end-->
            <!--重置密码 start-->
            <div class="pawoOne paFour" id="four" runat="server">
                <ul class="step">
                    <li><span><i>1</i></span><i class="txt">填写号码</i></li>
                    <li><span><i>2</i></span><i class="txt">验证身份</i></li>
                    <li><span><i>3</i></span><i class="txt">重置密码</i></li>
                    <li class="cur"><span><i>...</i></span><i class="txt">修改成功</i></li>
                </ul>
                <div class="bg">
                </div>
                <div class="patwoOk">
                    <span class="pic">
                        <img src="../images/ok.png" alt="暂无图片" /></span> <b>新密码设置成功！</b> <i>请牢记您新设置的密码。<a href="<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>"
                            class="link">返回登录页面</a></i>
                </div>
            </div>
            <!--重置密码 end-->
            <input type="hidden" id="hidusername" />
            <input type="hidden" id="hiduserid" />
            <input type="hidden" id="hidusertype" />
            <input type="hidden" id="hidusertel" />
            <%--  <input type="button" id="btnCheckPhone" style="display: none;" runat="server" value="验证手机验证码"
                onserverclick="btnCheckPhone_ServerClick" />--%>
        </div>
    </div>
    <!--footer start-->
   <uc1:Bottom ID="Bottom1" runat="server" />
    <!--footer end-->
    <link href="css/root.css" rel="stylesheet" type="text/css" />
    </form>
</body>
</html>

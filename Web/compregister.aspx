<%@ Page Language="C#" AutoEventWireup="true" CodeFile="compregister.aspx.cs" Inherits="CompRegister" %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/BotControl.ascx" TagPrefix="uc1" TagName="Bot" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>医站通-厂家注册</title>
    <meta name="keywords" content="B2B电子商务,在线订货,手机订货,订货app,电商平台,分销系统,代理商管理,订货系统,管理系统,在线订单管理,网上订货系统,在线订货平台,订货软件,代理商系统" />
    <meta name="description" content="医站通-B2B电子商务平台是为贸易或生产企业开发的网上订单管理系统,实现厂商与代理商之间实时订货,付款,发货,收货,库存管理,收付款对帐管理,物流信息查询,安全的在线支付,在线客服等全面高效的订货流程管理,提升企业管理竞争力,分销、批发就上医站通.咨询热线:40077-40088" />
    <link href="css/global.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <link href="css/index.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <link href="css/login.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <link href="Company/css/Enterprice.css" rel="stylesheet" type="text/css" />
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>
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
<body style="background: #f6f6f6;">
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" />
    <div class="mianDiv reg" style="width: 980px;">
        <div class="logo">
            <div class="lo" onclick="javascript:location.href='<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>';" style="cursor: pointer;">
                <img src="/images/logo2.0.png" height="55" alt="医站通"></div>
            <i class="title" runat="server" id="Title_type" title="">厂商注册</i></div>
        <!--注册信息-设置登录名 start-->
        <div class="regInfo RegisterNo1">
            <div class="regTitle">
            </div>
            <ul class="registInfo">
                <li><i class="label">手机号码：</i><div class="regBox">
                    <input id="txt_Phone" maxlength="11" autocomplete="off" runat="server" type="text"
                        class="box " /></div>
                    <label class="text" data-default="非常重要，登录，发送短信验证码使用">
                        非常重要，登录，发送短信验证码使用</label></li>
                <li class="wid"><i class="label">图文验证码：</i><div class="regBox ">
                    <input id="txt_CheckCode" autocomplete="off" maxlength="4" runat="server" type="text"
                        class="box" /></div>
                    <span class="captcha">
                        <img onclick="this.src=this.src+'?'" id="GetCode" onclick="this.src='/UserControl/CheckCode.aspx?t='+new Date().getTime()"
                            style="width: 100%; height: 100%;" src="UserControl/CheckCode.aspx" alt="验证码" /></span><label
                                class="text" data-default="验证码为4位随机数字">验证码为4位随机数字</label></li>
                <li class="wid"><i class="label">手机验证码：</i><div class="regBox ">
                    <input id="txt_PhoneCode" autocomplete="off" maxlength="6" runat="server" type="text"
                        class="box" /></div>
                    <a class="regbtn" style="float: left;" id="SendPhoneCode">获取验证码</a><label class="text"
                        data-default="请在30分钟内完成输入验证,每条验证码只可办理一笔业务">请在30分钟内完成输入验证,每条验证码只可办理一笔业务</label></li>
                <li style="width: 600px;">
                    <div class="txt" style="position: relative; height: 36px;">
                        <input id="CK_MYAgment" checked="checked" type="checkbox" class="fx">
                        <a href="javascript:;" id="OpenAgment">《医站通》平台用户服务协议</a>
                        <label class="text none" style="height: 35px; position: absolute; top: 30px; left: 170px;
                            line-height: 35px;">
                        </label>
                    </div>
                    <a id="btn_Submit" class="regbtn2" href="javascript:;">同意协议并提交</a><label class="text none"
                        id="lblSubmit" data-default="" style="float: right;"></label></li>
            </ul>
        </div>
        <!--注册信息-设置登录名 end-->
        <!--注册信息-设置账户信息 start-->
        <div class="regInfo RegisterNo2" style="display: none;">
            <div class="regTitle">
                <i class="p2"></i>
            </div>
            <ul class="registInfo">
                <li><i class="label" runat="server" id="CompDisName">企业名称：</i><div class="regBox">
                    <input id="txt_CompName" autocomplete="off" runat="server" type="text" maxlength="30"
                        class="box" /></i></div>
                    <label class="text" data-default="企业的法律名称，2-20个汉字或字母">
                        企业的法律名称，2-20个汉字或字母</label></li>

                    <!--
                    <li><i class="label" runat="server" id="Leading">法人姓名：</i><div class="regBox">
                        <input id="txt_Leading" autocomplete="off" runat="server" type="text" maxlength="30"
                            class="box" />
                        </div>
                        <label class="text" data-default="企业法人名称，2-20个汉字或字母">
                            企业法人名称，2-20个汉字或字母</label></li>
                    <li><i class="label" runat="server" id="Licence">法人身份证：</i><div class="regBox">
                        <input id="txt_Licence" autocomplete="off" runat="server" type="text" maxlength="30"
                            class="box" />
                        </div>
                        <label class="text" data-default="企业法人身份证号码">
                            企业法人身份证号码</label></li>
                    <li><i class="label" runat="server" id="creditCode">统一社会信用代码：</i><div class="regBox">
                        <input id="txt_creditCode" autocomplete="off" runat="server" type="text" maxlength="30"
                            class="box" />
                        </div>
                        <label class="text" data-default="请输入统一社会信用代码">
                            请输入统一社会信用代码</label></li>
                    -->
                    <li><i class="label" runat="server" id="TrueName">联系人姓名：</i><div class="regBox">
                        <input id="txt_TrueName" autocomplete="off" runat="server" type="text" maxlength="30"
                            class="box" />
                        </div>
                        <label class="text" data-default="请输入联系人姓名">
                            请输入联系人姓名</label></li>

                <li><i class="label">登录帐号：</i><div class="regBox">
                    <input id="txt_Account" autocomplete="off" type="text" class="box" /></div>
                    <label class="text" data-default="（包括文字，字符，英文，数字，下划线）注册后不能更改，但您可以用绑定的手机号码进行登录">
                        （包括文字，字符，英文，数字，下划线）注册后不能更改，但您可以用绑定的手机号码进行登录</label></li>
                <li><i class="label">登录密码：</i><div class="regBox">
                    <input id="txt_PassWord" autocomplete="off" maxlength="20" type="password" class="box" /></div>
                    <label class="text" data-default="6-20个字符，支持字母（区分大小写）、数字和符号">
                        6-20个字符，支持字母（区分大小写）、数字和符号</label></li>
                <li><i class="label">确认密码：</i><div class="regBox">
                    <input id="txt_CheckPassWord" autocomplete="off" maxlength="20" type="password" class="box" /></div>
                    <label class="text" data-default="请再输入确认密码">
                        请再输入确认密码</label></li>
                <li><i class="label">附件上传（可选）：</i>
                    <div class="teamR" style="height: 30px; margin-top: 7px;">
                        <div class="verFile" style="margin: 0px; display: inline; float: left;">
                            <span class="verFileCon">
                                <input id="uploadFile" runat="server" type="file" name="fileAttachment" /></span>
                            <a class="btn1" id="A3" style="text-decoration: NONE;" href="javascript:void(0)"><b
                                class="L"></b><b class="R"></b>上传附件</a> <a class="btn1" id="A4" onclick='$("#HidFfileName").val(""); $("#UpFileText").empty();'
                                    style="margin-left: 5px; text-decoration: NONE;" href="javascript:void(0)"><b class="L">
                                    </b><b class="R"></b>清除全部附件</a> <i class="a2" style="color: #777; line-height: 25px;
                                        margin-left: 5px; float: left;">可上传营业执照、税务登记证、组织机构代码证、法人身份证等附件（单个附件不可超过5M）</i>
                        </div>
                    </div>
                    <span style="color:red;">请上传清晰的营业执照</span>
                    <div id="UpFileText">
                    </div>
                    <input runat="server" id="HidFfileName" type="hidden" />
                </li>
                <li style="width: 600px;"><a id="btnRegister" href="javascript:;" class="regbtn2">确认并注册</a><label
                    class="text none" id="lblRegister" data-default="" style="float: right;"></label></li>
            </ul>
        </div>
        <!--注册信息-设置账户信息 end-->
        <!--注册信息-注册成功 start-->
        <div class="regInfo RegisterNo3" style="display: none;">
            <div class="regTitle">
                <i class="p2"></i><i class="p3"></i>
            </div>
            <div class="regOk">
                <div class="pic">
                </div>
                <div class="w1">
                    入驻申请提交成功，请等待审核</div>
                <div class="w2">
                    页面将在3秒之后跳转公共商城<a href="">立即跳转</a></div>
            </div>
        </div>
        <!--注册信息-注册成功 end-->
    </div>
    <input type="hidden" id="HidWindow" />
    <input type="hidden" id="HidCompid" runat="server" />
    <uc1:Bot ID="Bot1" runat="server" />
    <script src="js/UploadJs.js" type="text/javascript"></script>
    <script src="js/ComRegister.js?v=201712221553" type="text/javascript"></script>
    <script src="js/layer/layer.js" type="text/javascript"></script>
    <script src="js/layerCommon.js" type="text/javascript"></script>
    <script src="js/CommonJs.js?v=20160829" type="text/javascript"></script>
    <script type="text/javascript">
    <asp:Literal ID="LiteralJS" runat="server"></asp:Literal>
       $(document).ready(function () {
           $("#OpenAgment").on("click", function () {
               layerCommon.openWindow("协议查看", 'agreement1.aspx', '850px', '500px');
           });
           $("#uploadFile", "div.teamR").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText", ResultId: "HidFfileName", AjaxSrc: "Controller/Fileup.ashx", maxlength: 5 });
           $("body").delegate("#RegisLogin", "click", function (e,data) {
               layerCommon.openWindow("用户登录", "/WindowLogin.aspx?Comid=" + <%=Compid %>, "400px", "345px",function(){
                 $("#GetCode").trigger("click");
               }, false);
           })

       });
    </script>
    
    </form>
</body>
</html>

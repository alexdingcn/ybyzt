<%@ Page Language="C#" AutoEventWireup="true" CodeFile="help.aspx.cs" Inherits="help" EnableViewState="false" %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/NavTopControl.ascx" TagPrefix="uc1" TagName="TopNav" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="Bottom" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>帮助中心 医站通</title>
    <meta name="keywords" content="我的医站通网_医站通_网上分销_电子商务_B2B_批发_采购" />
    <meta name="description" content="我的医站通网,中小企业网上分销平台,为您寻找产品批发分销渠道,提供订单管理、代理商管理、资金对账、数据分析等功能,建立企业网上分销管理平台" />
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
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
<script type="text/javascript">
    $(function () {
        $("#helpList li").click(function () {
            $("#helpList li").removeClass("cur");
            $(this).addClass("cur");
        });
    }); 
</script>
<body class="root">
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" />
      <uc1:TopNav ID="TopNav1" runat="server" ShowID="" />
    <div class="help">
        <div class="helpCon">
            <div class="bt" title="常见问题">
            </div>
            <ul class="list" id="helpList">
                <li class="cur"><a>忘记登录帐号怎么办？有没有其他的登录方式？</a><div class="con">
                    可以点击右上角的“手机号码登录”，您可以直接用您注册时填写的手机号码进行登录。<i class="trigon"></i></div>
                </li>
                <li><a>线下交易如何录入系统？</a><div class="con">
                    在操作界面“代客下单”生成订单，在“订单收款补录”里面进行补录、发货或销账。<i class="trigon"></i></div>
                </li>
                <li><a>管理员帐号可以更改吗？</a><div class="con">
                    管理员帐号一经注册后不能进行更改，您也可以通过管理员手机号在“手机号码登录”中进行登录。<i class="trigon"></i></div>
                </li>
                <li><a>第一次进入该系统，我该怎么操作？</a><div class="con">
                    您可以按照系统提示步骤进行操作；也可以在界面下方或者界面左侧功能菜单栏内找到“我要开通”，并按相应步骤操作。<i class="trigon"></i></div>
                </li>
                <li><a>如何修改登录密码？</a><div class="con">
                    您可以在操作界面的“我要维护”—>“修改登录密码”菜单对您的登录密码进行修改。<i class="trigon"></i></div>
                </li>
                <li><a>为什么代客下了销售订单后不能发货？</a><div class="con">
                    您下单后需要代理商成功支付货款后，才可发货。<i class="trigon"></i></div>
                </li>
                <li><a>订单如何取消？</a><div class="con">
                    在您下单提交之后，如需取消订单，请及时与企业管理员联系，只有企业管理员才有取消订单的权限。<i class="trigon"></i></div>
                </li>
                <li><a>下单后可以修改订单吗？</a><div class="con">
                    订单在没有提交之前是可以进行修改的。<i class="trigon"></i></div>
                </li>
                <li><a>什么是特价订单？</a><div class="con">
                    您可以在收到代理商的特价订单申请或您在“代客下单”操作中选择特价订单后，通过修改商品单价，给代理商优惠。<i class="trigon"></i></div>
                </li>
                <li><a>我在哪些情况下要修改代理商的订单价格？</a><div class="con">
                    当订单类型为特价订单或此代理商的订单需要审核时可以修改商品单价，其他情况是不可以修改的。<i class="trigon"></i></div>
                </li>
                <li><a>订单收款补录是用来做什么的？</a><div class="con">
                    订单收款补录是您在代客下单后，将线下的销售订单收款信息补录进系统。<i class="trigon"></i></div>
                </li>
                <li><a>什么是预收款补录？</a><div class="con">
                    预收款补录是您将代理商通过现金或转账汇款等方式进行企业钱包充值的金额补录进系统。<i class="trigon"></i></div>
                </li>
                <li><a>什么是预收款冲正？</a><div class="con">
                    预收款冲正是指您在操作界面“预收款冲正”中录入需要从代理商预收款中支付的费用。<i class="trigon"></i></div>
                </li>
                <li><a>厂商可以修改手机号吗？</a><div class="con">
                    您可以在操作界面的“我要维护”—>“企业信息维护”菜单对您的法人及联系人手机号进行修改，但不能修改管理员的手机号码，如需修改请联系客服。<i class="trigon"></i></div>
                </li>
                <li><a>如何查看、修改自己的个人信息？</a><div class="con">
                    您可以在操作界面的“我要维护”—>“企业信息维护”里面查看、修改您的个人信息。<i class="trigon"></i></div>
                </li>
                <li><a>我要新加几个代理商到系统，该怎么进行添加？</a><div class="con">
                    在操作界面的“我要维护”—>”代理商维护”—>“代理商新增”里面新增代理商。（您可以选择“表格导入”或“界面录入”）。<i class="trigon"></i></div>
                </li>
                <li><a>我要新增加几个商品到系统，该怎么进行添加？</a><div class="con">
                    在操作界面的“我要维护”—>“商品维护”—>“商品新增”里面新增商品。（您可以选择“表格导入”或“界面录入”进行操作）。<i class="trigon"></i></div>
                </li>
                <li><a>为什么我不能在“商品信息维护”界面为商品新增一个规格属性？</a><div class="con">
                    请将该商品删除，在“商品新增”界面重新录入该商品信息。<i class="trigon"></i></div>
                </li>
                <li><a>怎么让代理商把钱打到我不同的银行卡上面去？</a><div class="con">
                    在操作界面的“我要维护”—>“收款帐号管理”菜单新增其他的收款帐号并绑定相应代理商。<i class="trigon"></i></div>
                </li>
                <li><a>上传附件时，附件的格式都是正确的，但为何始终无法成功？</a><div class="con">
                    可能是浏览器的兼容性问题，可以换其他的浏览器进行上传。<i class="trigon"></i></div>
                </li>
            </ul>
            <!-- 分页 START -->
            <%--<div class="page">
<!-- AspNetPager V7.2 for VS2005 & VS2008  Copyright:2003-2008 Webdiyer (www.webdiyer.com) -->
<div id="Pager" style="width:100%;">
<div >
	<a disabled="disabled" style="margin-right:5px;">首页</a><a disabled="disabled" style="margin-right:5px;">前页</a><span style="margin-right:5px;font-weight:Bold;color:red;">1</span><a href="javascript:__doPostBack('Pager','2')" style="margin-right:5px;">2</a><a href="javascript:__doPostBack('Pager','3')" style="margin-right:5px;">3</a><a href="javascript:__doPostBack('Pager','2')" style="margin-right:5px;">后页</a><a href="javascript:__doPostBack('Pager','3')" style="margin-right:5px;">尾页</a>
</div>
</div>
<!-- AspNetPager V7.2 for VS2005 & VS2008 End -->
</div>--%>
            <!-- 分页 END -->
        </div>
        <div class="helpCon download" title="常见问题">
            <div class="bt">
            </div>
            <ul class="list" id="helpList">
                <li><a>忘记登录帐号怎么办？有没有其他的登录方式？</a><div class="con">
                    可以点击右上角的“手机号码登录”，您可以直接用您注册时填写的手机号码进行登录。<i class="trigon"></i></div>
                </li>
                <li><a>管理员帐号可以更改吗？</a><div class="con">
                    管理员帐号一经注册后不能进行更改，您也可以通过管理员手机号在“手机号码登录”中进行登录。<i class="trigon"></i></div>
                </li>
                <li><a>如何修改登录密码？</a><div class="con">
                    您可以在操作界面的“我要维护”—>“修改登录密码”菜单对您的登录密码进行修改。<i class="trigon"></i></div>
                </li>
                <li><a>为什么我的页面选择不了赊销订单？</a><div class="con">
                    您能否赊销由您的厂商决定。<i class="trigon"></i></div>
                </li>
                <li><a>下单后可以修改订单吗？</a><div class="con">
                    订单在没有提交之前是可以进行修改的。<i class="trigon"></i></div>
                </li>
                <li><a>订单如何取消？</a><div class="con">
                    在您下单提交之后，如需取消订单，请及时与企业管理员联系，只有企业管理员才有取消订单的权限。<i class="trigon"></i></div>
                </li>
                <li><a>什么是特价订单？</a><div class="con">
                    您提交特价订单后，经厂商同意并修改商品单价，您将获得优惠。<i class="trigon"></i></div>
                </li>
                <li><a>退货款项去了哪里？</a><div class="con">
                    退货款项直接转入到您在厂商的企业钱包账户中。<i class="trigon"></i></div>
                </li>
                <li><a>支付方式有哪些？如何支付？</a><div class="con">
                    支付的方式有线上：企业钱包、转账汇款、网银支付和快捷支付等方式。线下：现金、票据等方式。<br />
                    现金、票据为现场交易，企业钱包、网银支付和快捷支付在订单交易时就可以支付，转账汇款通过“我要支付”—>“转账汇款”转给厂商。<i class="trigon"></i></div>
                </li>
                <li><a>代理商可以修改绑定手机号吗？</a><div class="con">
                    您可以在登录之后，在操作界面的“我要维护”—>“修改绑定手机”菜单对您的手机号进行修改。<i class="trigon"></i></div>
                </li>
                <li><a>如何查看、修改自己的个人信息？</a><div class="con">
                    您可以在操作界面的“我要维护”—>“基本信息维护”里面查看、修改您的个人信息。<i class="trigon"></i></div>
                </li>
                <li><a>上传附件时，附件的格式都是正确的，但为何始终无法成功？</a><div class="con">
                    可能是浏览器的兼容性问题，可以换其他的浏览器进行上传。<i class="trigon"></i></div>
                </li>
            </ul>
           
        </div>
        <div class="blank20">
        </div>
        <div class="ad1200">
            <img src="images/ad1200.jpg" alt="暂无图片"></div>
    </div>
    <div class="blank20">
    </div>
    <uc1:Bottom ID="Bottom1" runat="server" />
<script src="js/layer/layer.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
<script src="js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    </form>
</body>
</html>

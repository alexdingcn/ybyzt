<%@ Page Language="C#" AutoEventWireup="true" CodeFile="help.aspx.cs" Inherits="help_help" %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="Bottom" %>
<%@ Register Src="~/UserControl/SecTopControl.ascx" TagPrefix="uc1" TagName="TopSec" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title id="title">帮助中心-账号注册、厂家帮助、代理商帮助、常见问题-医站通</title>
    <meta name="keywords" content="B2B电子商务,在线订货,手机订货,订货app,电商平台,分销系统,代理商管理,订货系统,管理系统,在线订单管理,网上订货系统,在线订货平台,订货软件,代理商系统" />
    <meta name="description" content="医站通-B2B电子商务平台是为贸易或生产企业开发的网上订单管理系统,实现厂商与代理商之间实时订货,付款,发货,收货,库存管理,收付款对帐管理,物流信息查询,安全的在线支付,在线客服等全面高效的订货流程管理,提升企业管理竞争力,分销、批发就上医站通.咨询热线:40077-40088" />
    <link href="../css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery-1.9.1.min.js"></script>  
    <script type="text/javascript">
        $(function () {
        
            Index()
            $(".flbt").click(function () {
                $(this).addClass('cur').siblings().removeClass('cur');
                $(this).addClass('hover').siblings().removeClass('hover');
                document.title = $(this).children("a").text() + "-医站通帮助中心";
            });

            //选中厂家或厂家入门选项
         var K = "<%=K%>";
        if (K == "0") {
            $("#PK_Buyers")[0].click();
            $("#PK_Buyers").parent()[0].click();
            $(window).scrollTop(250);
        } else if (K == "1") {
            $("#PK_Seller")[0].click();
            $("#PK_Seller").parent()[0].click();
            $(window).scrollTop(250);
        }

        })
        function Index()
        {
            var index = $("#TopControl_a").val()
            $(".flbt").eq(index).addClass('cur').siblings().removeClass('cur');
            $(".flbt").eq(index).addClass('hover').siblings().removeClass('hover');
            document.title = $(".flbt").eq(index).children("a").text() + "-医站通帮助中心";
            if (index == "7" || index == 9) {
                $(".flbt").eq(index).find("li:eq(0)").children()[0].click();
            }
            else {
                $(".flbt").eq(index).children()[0].click();
            }
      
        }

        function iframeSrcA(str,obj,parent) {
            $("#parent").text(parent);
            $("#reportDis").attr("src", str)
            $("#text").text($(obj).text().replace("◇", ""));
        
        }
    </script>  
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
<body>
    <form id="form1" runat="server">

<uc1:Top ID="top1" runat="server" />

<!--页头 start-->
<uc1:TopSec ID="top2" runat="server" />
<!--页头 end-->

        <!--帮助中心头部 start-->
<div class="guideTitle"><div class="nr">
	    <div class="menu"><a class="hover">帮助中心</a></div>
</div></div>
<div class="help-ad"><img src="images/help-ad.jpg" /></div>
 <!--帮助中心头部 end--> 
<div class="blank10"></div>

<div class="wrap">
<!--帮助中心-左侧 start-->
<div class="help-sideBar fl"  id="PK_show">
	<dl class="menu">
        <dt class="title"><h2>账号注册</h2></dt>
        <dd class="flbt hover"><a href="javascript:;" class="t" onclick="iframeSrcA('账号注册登录/00_厂商注册.html',this,'账号注册')"><i class="arrw-i"></i>厂商注册</a></dd>
        <dd class="flbt"><a href="javascript:;" class="t" onclick="iframeSrcA('账号注册登录/01_代理商注册.html',this,'账号注册')"><i class="arrw-i"></i>代理商注册</a></dd>
        <dd class="flbt"><a href="javascript:;" class="t" onclick="iframeSrcA('账号注册登录/02_手机登录.html',this,'账号注册')"><i class="arrw-i"></i>手机号登录</a></dd>
        <dd class="flbt"><a href="javascript:;" class="t" onclick="iframeSrcA('账号注册登录/03_账号登录.html',this,'账号注册')"><i class="arrw-i"></i>账号登录</a></dd>
        <dd class="flbt"><a href="javascript:;" class="t" onclick="iframeSrcA('账号注册登录/04_密码修改.html',this,'账号注册')"><i class="arrw-i"></i>密码修改</a></dd>
        <dd class="flbt"><a href="javascript:;" class="t" onclick="iframeSrcA('账号注册登录/05_密码找回.html',this,'账号注册')"><i class="arrw-i"></i>密码找回</a></dd>
        <dt class="title"><h2>厂家帮助</h2></dt>
        <dd class="flbt"><a href="javascript:;" id="PK_Seller"  onclick="iframeSrcA('厂商专区/18_厂家入门.html?T=<%=Request["T"]%>',this,'厂家帮助')" class="t"><i class="arrw-i"></i>厂家入门</a></dd>
        <dd class="flbt">
            <a href="javascript:void(0);" class="t"><i class="arrw-i"></i>进阶</a>
            <ul class="help-submenu" >
                <li><a href="javascript:;" onclick="iframeSrcA('厂商专区/进阶/11_订单审核.html',this,'厂家帮助')"><i>◇</i>订单审核</a></li>
              
                           
                <li><a href="javascript:;" onclick="iframeSrcA('厂商专区/进阶/20_商品促销.html',this,'厂家帮助')"><i>◇</i>商品促销</a></li>
                <li><a href="javascript:;" onclick="iframeSrcA('厂商专区/进阶/21_订单促销.html',this,'厂家帮助')"><i>◇</i>订单促销</a></li>
                <li><a href="javascript:;" onclick="iframeSrcA('厂商专区/进阶/22_设置商品规格.html',this,'厂家帮助')"><i>◇</i>设置商品规格</a></li>
                <li><a href="javascript:;" onclick="iframeSrcA('厂商专区/进阶/23_收款账号设置.html',this,'厂家帮助')"><i>◇</i>收款账号维护</a></li>
                <li><a href="javascript:;" onclick="iframeSrcA('厂商专区/进阶/24_岗位权限管理.html',this,'厂家帮助')"><i>◇</i>岗位权限维护</a></li>
                <li><a href="javascript:;" onclick="iframeSrcA('厂商专区/进阶/25_信息发布.html',this,'厂家帮助')"><i>◇</i>信息发布</a></li>
                <li><a href="javascript:;" onclick="iframeSrcA('厂商专区/进阶/26_企业信息.html',this,'厂家帮助')"><i>◇</i>企业信息</a></li>
                <li><a href="javascript:;" onclick="iframeSrcA('厂商专区/进阶/27_留言回复.html',this,'厂家帮助')"><i>◇</i>留言回复</a></li>
                <li><a href="javascript:;" onclick="iframeSrcA('厂商专区/进阶/28_销售人员维护.html',this,'厂家帮助')"><i>◇</i>销售员维护</a></li>
                <li><a href="javascript:;" onclick="iframeSrcA('厂商专区/进阶/29_经销商管理员维护.html',this,'厂家帮助')"><i>◇</i>代理商管理员维护</a></li>
                <li><a href="javascript:;" onclick="iframeSrcA('厂商专区/进阶/30_系统设置.html',this,'厂家帮助')"><i>◇</i>系统设置</a></li>
            </ul>
        </dd>
        <dt class="title"><h2>代理商帮助</h2></dt>
        <dd class="flbt"><a href="javascript:;" id="PK_Buyers" onclick="iframeSrcA('代理商专区/18_代理商入门.html?T=<%=Request["T"]%>',this,'代理商帮助')"  class="t"><i class="arrw-i"></i>代理商入门</a></dd>
        <dd class="flbt">
            <a href="javascript:void(0);" class="t"><i class="arrw-i"></i>进阶</a>
            <ul class="help-submenu">
                <li><a href="javascript:;" onclick="iframeSrcA('代理商专区/进阶/30_收藏商品.html',this,'代理商帮助')"><i>◇</i>收藏商品</a></li>
                
                <li><a href="javascript:;" onclick="iframeSrcA('代理商专区/进阶/35_收货地址维护.html',this,'代理商帮助')"><i>◇</i>收货地址维护</a></li>
                <li><a href="javascript:;" onclick="iframeSrcA('代理商专区/进阶/36_修改快捷银行卡.html',this,'代理商帮助')"><i>◇</i>修改快捷银行卡</a></li>
                <li><a href="javascript:;" onclick="iframeSrcA('代理商专区/进阶/37_修改绑定手机.html',this,'代理商帮助')"><i>◇</i>修改绑定手机</a></li>
                <li><a href="javascript:;" onclick="iframeSrcA('代理商专区/进阶/38_岗位权限维护.html',this,'代理商帮助')"><i>◇</i>岗位权限维护</a></li>
                <li><a href="javascript:;" onclick="iframeSrcA('代理商专区/进阶/39_修改支付密码.html',this,'代理商帮助')"><i>◇</i>修改支付密码</a></li>
                <li><a href="javascript:;" onclick="iframeSrcA('代理商专区/进阶/40_我要咨询.html',this,'代理商帮助')"> <i>◇</i>我要咨询</a></li>
            </ul>
        </dd>
        <dt class="title"><h2>常见问题</h2></dt>
        <dd class="flbt"><a href="javascript:;" class="t" onclick="iframeSrcA('常见问题/44_邀请现有代理商.html',this,'常见问题')"><i class="arrw-i"></i>邀请现有代理商</a></dd>
       
        <dd class="flbt"><a href="javascript:;" class="t"  onclick="iframeSrcA('常见问题/45_管理代理商区域.html',this,'常见问题')"><i class="arrw-i"></i>管理代理商区域</a></dd>

        <dd class="flbt"><a href="javascript:;" class="t"  onclick="iframeSrcA('常见问题/46_管理分销网络.html',this,'常见问题')"><i class="arrw-i"></i>管理分销网络</a></dd>

        <dd class="flbt"><a href="javascript:;" class="t"  onclick="iframeSrcA('常见问题/47_添加合同.html',this,'常见问题')"><i class="arrw-i"></i>添加合同</a></dd>
	</dl>
</div>
<!--帮助中心-左侧 end-->
    <input type="hidden" id="TopControl_a" value="<%=index %>"/>

<!--帮助中心-右侧 end-->
<div class="help-ricon fr">
	<div class="place"><a href="javascript:;" class="t" >帮助中心</a>><a href="javascript:;" class="t" id="parent">账号注册</a>><a href="javascript:;" class="t" id="text">注册</a>
        <a href="http://crm2.qq.com/page/portalpage/wpa.php?uin=4009619099&f=1&ty=1&aty=0&a=&from=6" class="link"><i class="seek-i"></i>我有问题咨询</a></div>
    <div class="help-con">
        <iframe src="账号注册登录/00_厂商注册.html" id="reportDis"  frameborder="0" scrolling="no" onload="this.height=100" style="width:980px; margin:0; padding:0;" ></iframe>
    </div>
</div>
<!--帮助中心-右侧 end-->

</div>
<div class="blank20"></div>

<uc1:Bottom ID="Bottom1" runat="server" />
<script type="text/javascript" language="javascript">   
function reinitIframe(){
var iframe = document.getElementById("reportDis");
try{
var bHeight = iframe.contentWindow.document.body.scrollHeight;
var dHeight = iframe.contentWindow.document.documentElement.scrollHeight;
var height = Math.min(bHeight, dHeight);
iframe.height = height;
console.log(height);
}catch (ex){}
}
window.setInterval("reinitIframe()", 200);
</script>
<script type="text/javascript" src="../js/menu.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"></script>  
<script src="../js/layer/layer.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
<script src="../js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
</form>
</body>
</html>

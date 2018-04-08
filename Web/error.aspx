<%@ Page Language="C#" AutoEventWireup="true" CodeFile="error.aspx.cs" Inherits="error" %>
<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="bottom" %>
<%@ Register Src="~/UserControl/SecTopControl.ascx" TagPrefix="uc1" TagName="TopSec" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>错误页-医站通 B2B电子商务平台，分销、批发就上医站通</title>
    <meta name="keywords" content="B2B电子商务,在线订货,手机订货,订货app,电商平台,分销系统,代理商管理,订货系统,管理系统,在线订单管理,网上订货系统,在线订货平台,订货软件,代理商系统" />
    <meta name="description" content="医站通-B2B电子商务平台是为贸易或生产企业开发的网上订单管理系统,实现厂商与代理商之间实时订货,付款,发货,收货,库存管理,收付款对帐管理,物流信息查询,安全的在线支付,在线客服等全面高效的订货流程管理,提升企业管理竞争力,分销、批发就上医站通.咨询热线:40077-40088" />
    <script src="Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    
</head>
<body>
    <form id="form1" runat="server">

    <!--顶部导航栏 start-->
    <uc1:Top ID="top1" runat="server" />
    <!--顶部导航栏 end-->

    <!--页头 start-->
    <uc1:TopSec ID="top2" runat="server" />
    <!--页头 end-->

<div class="errorBox">
	<div class="p404"></div>
	<div class="text"><i class="">(╯﹏╰)</i>  哎呀，<%=strMsg%>……<br />小金喊你去<a href='<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>'>医站通首页</a>或者<a href="#" onclick="javascript:history.go(-1);">返回上一页</a></div>
</div>
<div class="blank10"></div>
<!--热门店铺 start-->
<div class="wrap">
	<div class="hotShop">
    	<h2 class="title"><i class="lou-i">1F</i>热门店铺</h2>
        <ul class="list">
            <asp:Repeater ID="hotShop_RPT" runat="server">
                <ItemTemplate>
                    <li>
                    <a href="/<%#Eval("ID") %>.html" target="_blank" title="<%#Eval("CompName")%>">
                    <span class="pic"> <img width="110" height="59" src="<%=ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/" %><%#Eval("ShopLogo").ToString()==""?Eval("CompLogo"):Eval("ShopLogo") %>" alt="<%#Eval("CompName")%>" />
                     </span>
                      <%# Eval("ShortName") %>
                    </a>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
<!--热门店铺 end-->

<div class="blank20"></div>
<!--热门商品 start-->
    <div class="hotGoods">
    	<h2 class="title"><i class="lou-i">2F</i>热门商品</h2>
        <ul class="goods-li">
          <asp:Repeater ID="hotGoods_RPT" runat="server">
           <ItemTemplate>
            <li>
            <div class="wrapper">
              <div class="pic"> <a target="_blank" href="/e<%#Eval("ID")%>_<%#Eval("CompID")%>.html"> <img alt="暂无图片" src="<%# ResolveUrl(Common.GetPicURL(Eval("Pic2").ToString(),"3")) %>" onerror="this.src='/images/Goods400x400.jpg'"></a></div>
              <div class="price"> <b class="txt"> 代理商可见 </b> </div>
              <div class="txt2"> <a target="_blank" title='<%#Eval("GoodsName") %>' href='/e<%#Eval("ID")%>_<%#Eval("CompID")%>.html'> <%#Eval("GoodsName") %></a></div>
              <div id="Ltr_98328" class="literal">
                <div class="specs"></div>
              </div>
              <div class="btn"  ><a  style="width:200px "id="GoodsBig_AddCart" href='CompRegister_<%#Eval("CompID") %>.html' class="addCart" >申请合作</a></div>
            </div>
          </li>
            
         </ItemTemplate>
        </asp:Repeater>
        </ul>
    </div>
    
</div>
<!--热门商品 ends-->

        <uc1:bottom runat="server" ID="bottom" />
        <script src="js/layer/layer.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
        <script src="js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    </form>
</body>
</html>

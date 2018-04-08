<%@ Page Language="C#" AutoEventWireup="true" CodeFile="compOrdisRegister.aspx.cs" Inherits="CompRegister" %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/BotControl.ascx" TagPrefix="uc1" TagName="Bot" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>入驻、加盟 医站通-B2B电子商务平台,手机订货平台,网上订货系统,订货app,加盟商订货软件,订单管理,代理商管理,在线订单,在线支付,分销、批发就上医站通
    </title>
    <meta name="keywords" content="B2B电子商务,在线订货,手机订货,订货app,电商平台,分销系统,代理商管理,订货系统,管理系统,在线订单管理,网上订货系统,在线订货平台,订货软件,代理商系统" />
    <meta name="description" content="医站通-B2B电子商务平台是为贸易或生产企业开发的网上订单管理系统,实现厂商与代理商之间实时订货,付款,发货,收货,库存管理,收付款对帐管理,物流信息查询,安全的在线支付,在线客服等全面高效的订货流程管理,提升企业管理竞争力,分销、批发就上医站通.咨询热线:400-961-9099" />
    <link href="css/global.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <link href="css/index.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <link href="css/login.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <link href="Company/css/Enterprice.css" rel="stylesheet" type="text/css" />
    <link href="css/global-2.0.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>
  
</head>
<body style="background: #f6f6f6;">
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" />
    <div class="mianDiv reg" style="width: 980px;">
        <div class="logo">
            <div class="lo" onclick="javascript:location.href='<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>';" style="cursor: pointer;">
                <img src="/images/logo2.0.png" height="55" alt="my1818"></div>
            <i class="title" runat="server" id="Title_type" title="">注册</i></div>
        <!--注册信息-设置登录名 start-->
        <div class="regInfo RegisterNo1">
           <div >
			   <ul class="zc-role">
				  <li><a href="compregister.aspx"><img src="images/comp.png" />
					   <h3 class="t">我是厂商</h3>
					   <p class="t2">I am a vendor</p>
					   </a></li>
				  <li><a href="disregister.aspx"><img src="images/dis.png" />
					   <h3 class="t">我是代理商</h3>
					   <p class="t2">I am an agent</p>
					   </a></li>
			   </ul>
			   
			 
			   
           </div>
		
          
            
         

          
        </div>

    </div>
  
    <uc1:Bot ID="Bot1" runat="server" />
    </form>
</body>
</html>

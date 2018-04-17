<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TopControl.ascx.cs" Inherits="Company_UserControl_TopControl" %>
<%@ Register Src="~/Company/UserControl/leftControl.ascx" TagPrefix="uc1" TagName="left" %>
<script>
    $(function () {
        var lefttype = '<%=Request["lefttype"] + "" %>';
        if (lefttype != "") {
            if (lefttype == "1") {
                $(".rightinfo").css({ "margin-top": "0px" });
            } else if (lefttype == "2") {
                $(".rightinfo").css({ "margin-left": "0px" });
            } else if (lefttype == "3") {
                $(".rightinfo").css({ "margin-top": "0px", "margin-left": "0px", " width": "auto" });
            }
        }
    });
</script>
<!--头部 start-->
<div class="topBar" id="topBar" runat="server">
    <div class="nr">
        <div class="logo">
            <span><img src='<%= bol==true?logo:"/Distributor/images/al-logo.png" %>' width="94"></span> 
            <i id="disName">
                <a target="" href='<%=ResolveUrl("../jsc.aspx")%>' style="font-size: 16px;">
                    <%=CompName %></a>
            </i>
        </div>
        <ul class="right topNav">
            <li>客服热线：40077-40088</li>
            <li class="name">
                <a href="<%=ResolveUrl("../jsc.aspx")%>" style="font-size: 14px;"><i class="me-icon"></i>
                <%=UserName %><i class="triangle"></i></a> 
                
                <span class="tgn">
                <%--<a href="<%=ResolveUrl("../SysManager/Service.aspx")%>" style="font-size: 12px;">购买服务</a> --%>
                <a href="<%=ResolveUrl("../ChangePwd.aspx")%>"  style="font-size: 12px;">修改密码</a>                    
                <a href="javascript:void(0);" style="font-size: 12px;"  target="_parent" onclick=" window.location.href ='<%=ResolveUrl("../loginout.aspx")%>?type=<%= bol==true?1:0 %>';">退出</a> 
                </span>
            </li>
            <%--<li><a target="_blank" href="/ShoppingMall.aspx" style="font-size: 14px;"><i class="qy-icon"></i>e商城</a> </li>--%>
            <li id="Head1_eDis" style='<%= bol==true?"display:none": ""%>'>
                <a target="_blank" href="/<%=compid %>.html" style="font-size: 14px;"><i class="dp-icon"></i>店铺</a> 
            </li>
            <li style='<%= bol==true?"display:none": ""%>'>
                <a target="_blank" href='<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>' style="font-size: 14px;">
                    <i class="home-icon"></i>首页
                </a>
            </li>
            <li>
                <a target="_blank" href="/help/厂商专区/18_厂家入门.html" style="font-size: 14px;"><i class="xx-icon"></i>帮助中心</a> 
            </li>
            <li><a href="javascript:void(0);" style="font-size: 14px;" onclick=" window.location.href ='<%=ResolveUrl("../loginout.aspx")%>?type=<%= bol==true?1:0 %>';">退出</a></li>
        </ul>
    </div>
</div>
<!--头部 end-->
<input type="hidden" id="Showid" runat="server" />
<uc1:left ID="left1" Visible="false" runat="server"  />

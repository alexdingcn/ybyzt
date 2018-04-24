<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompanyTop.ascx.cs" Inherits="Company_UserControl_CompanyTop" %>

<script>
    $(function () {
        var lefttype = '<%=Request["lefttype"] + "" %>';
        if (lefttype != "") {
            if (lefttype == "1") {
                $(".navbar").hide();
            } else if (lefttype == "2") {
                $(".sidebar").hide();
            } else if (lefttype == "3") {
                $(".navbar").hide();
                $(".sidebar").hide();
            }
        }
    });
</script>
<!--头部 start-->
<div class="navbar navbar-default" id="topBar" runat="server">
    <div class="navbar-container" id="navbar-container">
		<button type="button" class="navbar-toggle menu-toggler pull-left" id="menu-toggler" data-target="#sidebar">
			<span class="sr-only">切换侧边栏</span>
			<span class="icon-bar"></span>
			<span class="icon-bar"></span>
			<span class="icon-bar"></span>
		</button>

		<div class="navbar-header pull-left">
			<a href='<%=ResolveUrl("../jsc.aspx")%>' class="navbar-brand">
				<small>
                    <img src='<%= bol==true?logo:"/Distributor/images/al-logo.png" %>' width="94" />
                    <%=CompName %>
				</small>
			</a>
		</div>

		<div class="navbar-buttons navbar-header pull-right" role="navigation">
			<ul class="nav ace-nav" style="">
                <li class="light-blue" style="padding-right:5px"><i class="icon iconfont icon--kefu-xianxing"></i>客服热线：40077-40088</li>
                <li class='grey <%= bol==true?"hide": ""%>'>
                    <a target="_blank" href="/<%=compid %>.html"><i class="icon iconfont icon-dianpu"></i>店铺</a> 
                </li>
                <li class='purple <%= bol==true?"hide": ""%>'>
                    <a target="_blank" href='<%= ConfigurationManager.AppSettings["WebDomainName"].ToString() %>'>
                        <i class="icon iconfont icon-shouye"></i>首页
                    </a>
                </li>
                <li class="green">
                    <a target="_blank" href="/help/厂商专区/18_厂家入门.html"><i class="fa fa-question-circle fa-fw"></i>帮助中心</a> 
                </li>

                <li class="light-blue dropdown-modal">
                    <a href="#" data-toggle="dropdown" class="dropdown-toggle">
                        <i class="ace-icon fa fa-user"></i>
                        <span class=""><%=UserName %></span>
                        <i class="ace-icon fa fa-caret-down"></i>
                    </a>
                    <ul class="user-menu dropdown-menu-right dropdown-menu dropdown-yellow dropdown-caret dropdown-close">
						<li>
                            <a href="<%=ResolveUrl("../ChangePwd.aspx")%>"><i class="ace-icon fa fa-cog"></i>修改密码</a>
						</li>
						<li class="divider"></li>
						<li>
                            <a href="javascript:void(0);" target="_parent" onclick="window.location.href ='<%=ResolveUrl("../loginout.aspx")%>?type=<%= bol==true?1:0 %>';">
                                <i class="ace-icon fa fa-power-off"></i>退出
                            </a>
						</li>
					</ul>
                </li>
			</ul>
		</div>
	</div>
</div>
<!--头部 end-->
<input type="hidden" id="Showid" runat="server" />


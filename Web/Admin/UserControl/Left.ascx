<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Left.ascx.cs" Inherits="Admin_UserControl_Left" %>
<script type="text/javascript">
    $(function () {
        $(".submenu")
    })
</script>
<!--左侧菜单 start-->
<div class="leftNav">
	<div class="mlogo"><span title="广州农商行-赢家生意圈"><img src="../../Distributor/images/al-logo.png" width="110"title="广州农商行-赢家生意圈"> </span></div>
	<div class="li ">
    	<div class="name">系统管理</div>
    	<ul id="xtgl"  runat="server" visible="false" class="submenu">
        	<li id="xtyhjqx" runat="server" visible="false"><a href="../../Admin/Systems/UserList.aspx">系统用户及权限</a></li>
            <li id="xwfb" runat="server" visible="false"><a href="../../Admin/Systems/NewsList.aspx">新闻发布</a></li>
            <li id="hyflgl" runat="server" visible="false"><a href="../../Admin/Systems/IndustryList.aspx">商品分类管理</a></li>
            <li id="kfly" runat="server" visible="false"><a href="../../Admin/Systems/MessageList.aspx">客服留言</a></li>
            <%--<li id="ptskzh" runat="server" visible="false"><a href="../../Admin/Systems/PaybankList.aspx">平台收款账户</a></li>--%>
            <%--<li id="gmfwyh" runat="server" visible="false"><a href="../../Admin/Systems/CompService.aspx">购买服务用户</a></li>--%>
            <li id="dlrz" runat="server" visible="false"><a href="../../Admin/Systems/LoginLog.aspx">登录日志</a></li>
            <li id="ywrz" runat="server" visible="false"><a href="../../Admin/Systems/BusinessLog.aspx">业务日志</a></li>
            <li id="yyda" runat="server" visible="false"><a href="../../Admin/Systems/hospitalList.aspx">医院档案</a></li>
            <%--<li id="ssgjz" runat="server" visible="true"><a href="../../Admin/Systems/SelectList.aspx">搜索关键字列表</a></li>--%>
        </ul>
    </div>
    <div class="li">
    	<div class="name">机构管理</div>
    	<ul id="jggl" runat="server" visible="false" class="submenu">
        	<li id="jgwh" runat="server" visible="false"><a href="../../Admin/OrgManage/OrgList.aspx">机构维护</a></li>
            <li id="jgyhwh" runat="server" visible="false"><a href="../../Admin/OrgManage/OrgUserList.aspx">机构用户维护</a></li>
            <li id="jgywywh" runat="server" visible="false"><a href="../../Admin/OrgManage/SaleManList.aspx">机构业务员维护</a></li>
        </ul>
    </div>
    <div class="li">
    	<div class="name">厂商管理</div>
    	<ul id="hxqygl"  runat="server" visible="false" class="submenu">
        	<li id="hxqyxz" runat="server" visible="false"><a href="../../Admin/Systems/CompEdit.aspx?show=0&type=1">厂商新增</a></li>
            <li id="hxqysh" runat="server" visible="false"><a href="../../Admin/Systems/CompAuditList.aspx">厂商审核</a></li>
            <li id="hxqywh" runat="server" visible="false"><a href="../../Admin/Systems/CompList.aspx">厂商维护</a></li>
            <li id="qyyhwh" runat="server" visible="false"><a href="../../Admin/Systems/CompUserList.aspx">企业用户维护</a></li>
            <li id="hxqyzx" runat="server" visible="false"><a href="../../Admin/Systems/CompzxList.aspx?type=1">厂商装修</a></li>
            <li id="hxqyzxsh" runat="server" visible="false"><a href="../../Admin/Systems/CompzxList.aspx?type=2">厂商装修审核</a></li>
        </ul>
    </div>
    <div class="li">
    	<div class="name">代理商管理</div>
    	<ul id="jxsgl"  runat="server" visible="false" class="submenu">
        	<li id="jxscx" runat="server" visible="false"><a href="../../Admin/Systems/DisList.aspx">代理商查询</a></li>
            <%--<li id="jxsglycx" runat="server" visible="false"><a href="../../Admin/Systems/DisUserList.aspx">代理商管理员查询</a></li>--%>
        </ul>
    </div>
    <div class="li">
    	<div class="name">商品查询</div>
    	<ul id="spgls"  runat="server" visible="false" class="submenu">
        	<li id="spcx" runat="server" visible="false"><a href="../../Admin/Systems/GoodsInfoList.aspx">商品查询</a></li>
        </ul>
    </div>
    
    <div class="li">
    	<div class="name">订单查询</div>
    	<ul id="ddcxs"  runat="server" visible="false" class="submenu">
        	<li id="ddcx" runat="server" visible="false"><a href="../../Admin/Systems/OrderList.aspx">订单查询</a></li>
        </ul>
    </div>
    
    <div class="li li7">
    	<div class="name">报表查询</div>
    	<ul id="bbcxs"  runat="server" visible="false" class="submenu">
        	<li id="xsddcx" runat="server" visible="false"><a href="../../Admin/Systems/RepOrderList.aspx">销售订单查询</a></li>
            <li id="jxsxssj" runat="server" visible="false"><a href="../../Admin/Systems/RepCusList.aspx">代理商销售数据</a></li>
            <li id="spxssj" runat="server" visible="false"><a href="../../Admin/Systems/RepGoodsList.aspx">商品销售数据</a></li>
            <li id="xssjyfbd" runat="server" visible="false"><a href="../../Admin/Systems/RepMonthList.aspx">销售数据月份变动</a></li>
            <li id="yszk" runat="server" visible="false"><a href="../../Admin/Systems/RepArrearageList.aspx">应收账款</a></li>
            <li id="zd" runat="server" visible="false"><a href="../../Admin/Systems/RepPayList.aspx">账单</a></li>
            <li id="dpdjlcx" runat="server" visible="false"><a href="../../Admin/Systems/EShopLogReport.aspx" >店铺点击量查询</a></li>
            <li id="ipdjlcx" runat="server" visible="false"><a href="../../Admin/Systems/EShopIPReport.aspx" >IP点击量查询</a></li>
        </ul>
    </div>
</div>
<!--左侧菜单 end-->

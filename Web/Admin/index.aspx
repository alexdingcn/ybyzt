<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Admin_index" %>
<%@ Register src="UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>我的桌面</title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        //跳转
        function OpenShow(url, id) {
            window.parent.leftFrame.onlinkOrder(id);
            location.href = url; //  "Systems/CompAuditList.aspx";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <style type="text/css">
        .desktop
        {
            border: 1px solid #d7d7d7;
            overflow: hidden;
            margin: 0 0 0 10px;
            width: 980px;
            height: 140px;
            color: #494949;
        }
		.toDo{ border:none;}
        .toDo li
        {
            float: left; /*width: 194px;*/
            width: 190px;
            height: 70px;
            padding-top: 70px;
            border-left: 1px solid #d7d7d7;
            overflow: hidden;
            text-align: center;
            position: relative;
        }
        .toDo li:first-child
        {
            border-left: none;
        }
        .leftmenu dd{ width:148px;}
        .toDo .title
        {
            color: #222;
            display: block;
            font-size: 14px;
            font-weight: normal;
            line-height: 35px;
        }
        .toDo .text
        {
            color: #999;
            display: block;
        }
        .toDop1
        {
            width: 40px;
            height: 37px;
            background: url(../Company/images/toDo.png) no-repeat 0 0;
            position: absolute;
            top: 50%;
            left: 50%;
            margin: -39px 0 0 -20px;
        }
        .toDo .font
        {
            position: absolute;
            top: -5px;
            right: -5px;
            text-align: center;
            line-height: 16px;
            padding: 0px 5px;
            border-radius: 8px;
            background: #ff7100;
            color: #fff;
        }
        
        .statis
        {
            float: left;
            border-right: 1px solid #ddd;
            border-left: 1px solid #ddd;
            width: 169px;
            height: 140px;
            padding: 20px 20px 0 40px;
        }
        .statis .li
        {
            margin-top: 20px;
            font-size: 14px;
        }
        .statis .f
        {
            color: red;
            margin: 0px 5px;
            font-size: 16px;
            font-weight: normal;
        }
        .statis a:hover
        {
            text-decoration: underline;
        }
        
        .userCounts
        {
            float: right;
            width: 345px;
        }
        .userCounts .title
        {
            line-height: 30px;
            position: relative;
            height: 30px;
            font-size: 14px;
            margin-top: 3px;
            color: #494949;
        }
        .userCounts .noticeIcon
        {
            background: url(../images/notice2.png) no-repeat 0 0;
            width: 19px;
            height: 16px;
            display: inline-block;
            position: relative;
            top: 3px;
            left: 2px;
            margin-right: 5px;
        }
        .userCounts .more
        {
            position: absolute;
            top: 0;
            right: 10px;
            font-size: 12px;
            color: #494949;
        }
        .userCounts .list
        {
            line-height: 26px;
            padding-top: 1px;
        }
        .userCounts ul li.top a
        {
            color: Red;
        }
        .newIcon
        {
            width: 22px;
            height: 14px;
            background: url(../images/new.png) no-repeat 0 0;
            display: inline-block;
            position: relative;
            top: 2px;
            margin-left: 3px;
        }
        
        .tableList
        {
            width: 980px;
            border: 1px solid #ddd;
            border-bottom: none;
            margin: 10px 0 0 10px;
            color: #222;
        }
        .tableList td
        {
            line-height: 40px;
            border-bottom: 1px solid #ddd;
            border-left: 1px solid #ddd;
            text-align: center;
            width: 16%;
        }
        .tableList thead td
        {
            line-height: 45px;
            background: #e5f1f6;
        }
        .tableList tbody tr:hover
        {
            background: #f5fafc;
            cursor: pointer;
        }
        .tableList td:first-child
        {
            border-left: none;
            font-weight: bold;
        }
        .tableList tbody tr:nth-last-child(odd)
        {
            background: #f5fafc;
        }
        .tableList a:hover, .userCounts a:hover
        {
            text-decoration: underline;
            color: red;
        }
    </style>
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
        <i>位置：</i><a href="index.aspx" style="cursor: pointer;">我的桌面</a>
    </div>

    <div class="desktop">
        <!--待办事项 start-->
        <ul class="toDo left"  id="ulComp" runat="server">
            <li><a href="Systems/CompAuditList.aspx" onclick="OpenShow('Systems/CompAuditList.aspx','hxqysh')">
                <span class="toDop1"><i class="font" id="lblCompcount" runat="server"></i></span>
                <b class="title">待审核企业</b> <i class="text">请及时进行审核</i></a>
            </li>
            <li><a href="Systems/MessageList.aspx?State=0" onclick="OpenShow('Systems/MessageList.aspx?State=0','kfly')">
                <span class="toDop1"><i class="font" id="lblMessage" runat="server"></i></span>
                <b class="title">待处理留言</b> <i class="text">请及时进行处理</i></a>
            </li>
        </ul>
        <!--待办事项 end-->
        <div class="statis">
            <div class="li">
                前台显示企业<a href="Systems/CompList.aspx?IsFist=Show" class="f" id="lblShowcount" onclick="OpenShow('Systems/CompList.aspx?IsFist=Show','hxqycx')"
                    runat="server"></a>家</div>
            <div class="li">
                机构 <a href="OrgManage/OrgList.aspx" class="f" onclick="OpenShow('OrgManage/OrgList.aspx','jgwh')"
                    id="lblOrgcount" runat="server"></a>家</div>
        </div>
        <!--最新公告 start-->
        <div class="userCounts">
            <div class="title">
                <i class="noticeIcon"></i>最新公告<a href="Systems/NewsList.aspx" onclick="OpenShow('Systems/NewsList.aspx','xwgg')"
                    class="more">更多&gt;&gt;</a></div>
            <ul class="list">
                <asp:Repeater ID="rptNews" runat="server">
                    <ItemTemplate>
                        <li <%# Eval("istop").ToString()=="1"?"class='top'":"" %>><a title="<%# Eval("newsTitle") %>"
                            href="Systems/NewsInfo.aspx?KeyID=<%# Eval("id") %>" onclick="OpenShow('Systems/NewsInfo.aspx?KeyID=<%# Eval("id") %>','xwgg')">
                            【<%# GetType(Eval("newsType").ToString())%>】<%# Eval("newsTitle") %></a><i <%# Eval("istop").ToString()=="1"?"class='newIcon'":"" %>></i></li></ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <!--最新公告 end-->
    </div>
    <div class="tableList">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <thead>
                <tr>
                    <td>
                        类型
                    </td>
                    <td>
                        今日（0:00~24:00）
                    </td>
                    <td>
                        本 周
                    </td>
                    <td>
                        本 月
                    </td>
                    <td>
                        本 年
                    </td>
                    <td>
                        累 计
                    </td>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <asp:Repeater ID="rptComplist" runat="server">
                        <HeaderTemplate>
                            <td>
                                厂商
                            </td>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <td>
                                <a href="javascript:;">
                                    <%# Eval("sumcount1") %></a>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
                <tr>
                    <asp:Repeater ID="rptDisList" runat="server">
                        <HeaderTemplate>
                            <td>
                                代理商
                            </td>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <td>
                                <a href="javascript:;">
                                    <%# Eval("sumcount2") %></a>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
                <tr>
                    <asp:Repeater ID="rptOrderList" runat="server">
                        <HeaderTemplate>
                            <td>
                                订单
                            </td>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <td>
                                <a href="javascript:;">
                                    <%# Eval("sumcount3") %></a>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
                <tr>
                    <asp:Repeater ID="rptPriceList" runat="server">
                        <HeaderTemplate>
                            <td>
                                收款
                            </td>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <td>
                                <a href="javascript:;">
                                    <%# Convert.ToDecimal(Eval("sumcount4")).ToString("N")%>
                                </a>
                            </td>
                        </ItemTemplate>
                    </asp:Repeater>
                </tr>
            </tbody>
        </table>
    </div>
        </div>
    </form>
</body>
</html>

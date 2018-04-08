<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IndustryInfo.aspx.cs" Inherits="Admin_Systems_IndustryInfo" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>行业分类管理</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("li#libtnDel").on("click", function () {
                confirm("确认删除？", function () { $("#btnDel").trigger("click"); }, "提示");
            })

            $("li#libtnEdit").on("click", function () {
                location.href = "IndustryModify.aspx?KeyID="+<%= Request.QueryString["KeyID"] %>;
            })

        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">系统管理</a><i>></i>
            <a href="IndustryList.aspx">行业分类管理</a>
    </div>
        <div class="tools">
            <ul class="toolbar left">
                <li id="libtnEdit"><span>
                    <img src="../../Company/images/t02.png" /></span>编辑</li>
                <li id="libtnDel"><span>
                    <img src="../../Company/images/t03.png" /></span>删除<input type="button" runat="server"
                        id="btnDel" onserverclick="btn_Del" style="display: none;" /></li>
                <li id="lblbtnback" onclick="location.href = 'IndustryList.aspx'"><span>
                    <img src="../../Company/images/tp3.png" /></span>返回</li>
            </ul>
        </div>
        <div class="div_content">
            <table class="tb">
                <tbody>
                    <tr>
                        <td width="120">
                            <span>行业分类名称</span>
                        </td>
                        <td>
                            <label runat="server" id="lblname">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            <span>排 序</span>
                        </td>
                        <td>
                            <label runat="server" id="lblsort">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            <span>状 态</span>
                        </td>
                        <td>
                            <label runat="server" id="lblstate">
                            </label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    </form>
</body>
</html>

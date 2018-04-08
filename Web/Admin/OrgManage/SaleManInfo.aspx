<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleManInfo.aspx.cs" Inherits="Admin_OrgManage_SaleManInfo" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>机构业务员维护</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />  
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
//            $("li#libtnDel").on("click", function () {
//                confirm("确认删除？", function () { $("#btnDel").trigger("click"); }, "提示");
//            })

            $("li#libtnEdit").on("click", function () {
                location.href = "SaleManEdit.aspx?KeyID="+<%= Request.QueryString["KeyID"] %>;
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
            <a href="#">机构管理</a><i>></i>
            <a href="#">机构业务员维护</a>
    </div>
        <div class="tools">
            <ul class="toolbar left">
                <li id="libtnEdit"><span>
                    <img src="../../Company/images/t02.png" /></span>编辑</li>
                <%--<li id="libtnDel"><span>
                    <img src="../../Company/images/t03.png" /></span>删除<input type="button" runat="server"
                        id="btnDel" onserverclick="btn_Del" style="display: none;" /></li>--%>
                <li id="lblbtnback" onclick="location.href = 'SaleManList.aspx'"><span>
                    <img src="../../Company/images/tp3.png" /></span>返回</li>
            </ul>
        </div>
        <div class="div_content">
            <table class="tb">
                <tbody>
                    <tr>
                        <td width="10%">
                            <span>机构</span>
                        </td>
                        <td width="20%">
                            <label runat="server" id="lblOrg">
                            </label>
                        </td>
                        <td width="10%">
                            <span>业务员名称</span>
                        </td>
                        <td width="20%">
                            <label runat="server" id="lblSaleName">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td width="10%">
                            <span>业务员编码</span>
                        </td>
                        <td width="20%">
                            <label runat="server" id="lblSaleCode">
                            </label>
                        </td>
                        <td width="10%">
                            <span>联系手机</span>
                        </td>
                        <td width="20%">
                            <label runat="server" id="lblPhone">
                            </label>
                        </td>
                    </tr>
                    <tr> 
                        <td>
                            <span>邮箱</span>
                        </td>
                        <td>
                            <label runat="server" id="lblEmail">
                            </label>
                        </td>                       
                        <td>
                            <span>是否启用</span>
                        </td>
                        <td>
                            <label runat="server" id="lblstate">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>备注</span>
                        </td>
                        <td colspan="3">
                            <label runat="server" id="lblRemark">
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
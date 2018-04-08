<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MessageInfo.aspx.cs" Inherits="Admin_Systems_MessageInfo" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>客服留言</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />    
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $("li#libtnDel").on("click", function() {
                confirm("确认删除？", function() { $("#btnDel").trigger("click"); }, "提示");
            });

            $("li#libtnEdit").on("click", function() {
                location.href = "NewsModify.aspx?KeyID=" + <%= Request.QueryString["KeyID"] %>;
            });
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
            <a href="MessageList.aspx">客服留言</a>
    </div>
        <div class="tools">
            <ul class="toolbar left">
                
                <li id="lblbtnback" onclick="location.href = 'MessageList.aspx'"><span>
                    <img src="../../Company/images/tp3.png" /></span>返回</li>
            </ul>
        </div>
        <div class="div_content">
            <table class="tb">
                <tbody>
                    <tr>
                        <td width="160">
                            <span>姓  名</span>
                        </td>
                        <td>
                            <label runat="server" id="lblUserName">
                            </label>&nbsp;
                        </td></tr>
                    <tr>
                        <td >
                            <span>电  话</span>
                        </td>
                        <td>
                            <label runat="server" id="lblUserPhone">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>QQ/Email</span>
                        </td>
                        <td>
                            <label runat="server" id="lblUserMailQQ">
                            </label>
                        </td></tr>
                    <tr>
                        <td>
                            <span>留言内容</span>
                        </td>
                        <td>
                            <label runat="server" id="lblUserMessge">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>留言时间</span>
                        </td>
                        <td>
                            <label runat="server" id="lblCreateDate">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>处理状态</span>
                        </td>
                        <td>
                            <label runat="server" id="lblState">
                            </label>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <span>处理结果</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Height="97px" 
                                Width="320px" MaxLength="100"></asp:TextBox> <font color="red">*</font>
                        </td>
                    </tr>
                    <tr id="tr1" runat="server" visible="false">
                        <td>
                            <span>处理时间</span>
                        </td>
                        <td>
                            <label runat="server" id="lblModify">
                            </label>
                        </td>
                    </tr>

                    <tr id="tr2" runat="server" visible="false">
                        <td></td>
                        <td>
                            <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定处理" 
                                OnClick="btnAdd_Click" Width="106px" />
                        </td>
                    </tr>

                </tbody>
            </table>
        </div>
    </div>
    </form>
</body>
</html>


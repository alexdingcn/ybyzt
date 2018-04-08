<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewsInfo.aspx.cs" ValidateRequest="false" Inherits="Admin_Systems_NewsInfo" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>新闻发布</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />    
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    
    <script src="../../kindeditor/kindeditor.js" type="text/javascript"></script>
    <script src="../../kindeditor/lang/zh_CN.js" type="text/javascript"></script>
    <script src="../../kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
    <link href="../../kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
    <link href="../../kindeditor/plugins/code/prettify.css" rel="stylesheet" type="text/css" />

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
            <a href="NewsList.aspx">新闻发布</a>
    </div>
        <div class="tools">
            <ul class="toolbar left">
                <li id="libtnEdit"><span>
                    <img src="../../Company/images/t02.png" /></span>编辑</li>
                <li id="libtnDel"><span>
                    <img src="../../Company/images/t03.png" /></span>删除<input type="button" runat="server"
                        id="btnDel" onserverclick="btn_Del" style="display: none;" /></li>
                    
                <li id="lblbtnback" onclick="location.href = 'NewsList.aspx'"><span>
                    <img src="../../Company/images/tp3.png" /></span>返回</li>
            </ul>
        </div>
        <div class="div_content">
            <table class="tb">
                <tbody>
                    <tr>
                        <td width="120">
                            <span>信息标题</span>
                        </td>
                        <td>
                            &nbsp;<%=title%>&nbsp;&nbsp;
                           <a target="_blank" href="../../newsinfo.aspx?newsid=<%=KeyID %>&newstype=<%=newstype %>" style="text-decoration: underline; color:#0080b8;">信息预览</a>
                        </td></tr>
                       
                    <tr>
                        <td >
                            <span>信息类别</span>
                        </td>
                        <td>
                            <label runat="server" id="lblnewtype">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            <span>是否发布</span>
                        </td>
                        <td>
                            <label runat="server" id="lblstate">
                            </label>
                        </td></tr>
                    <tr>
                        <td width="120">
                            <span>是否置顶</span>
                        </td>
                        <td>
                            <label runat="server" id="lblistop">
                            </label>
                        </td>
                    </tr>
                    <tr class="newspan">
                        <td>
                            <span>信息内容</span>
                        </td>
                        <td>
                            <asp:TextBox ID="content7" runat="server" TextMode="MultiLine" Height="350px" Width="800px"
                                class="textBox"></asp:TextBox>
                            <script>
                                KindEditor.ready(function (K) {
                                    window.editor = K.create('#content7', {
                                        uploadJson: '../../Kindeditor/asp.net/upload_json.ashx',
                                        fileManagerJson: '../../Kindeditor/asp.net/file_manager_json.ashx',
                                        allowFileManager: true
                                    });
                                });
                            </script>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <span>Keywords</span>
                        </td>
                        <td>
                            <asp:Label ID="lblKeywords" runat="server" Text="" style="text-align:left"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <span>信息摘要</span>
                        </td>
                        <td>
                            <asp:Label ID="lblNewsInfo" runat="server" Text="" style="text-align:left"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    </form>
</body>
</html>


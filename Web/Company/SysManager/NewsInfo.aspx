<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewsInfo.aspx.cs" Inherits="NewsInfo" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>信息发布详细</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../js/OpenJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("li#libtnDel").on("click", function() {
               layerCommon.confirm("确认删除？", function() { $("#btnDel").trigger("click"); }, "提示");
            });

            $("li#libtnEdit").on("click", function() {
                location.href = "NewsModify.aspx?KeyID=" + <%= Request.QueryString["KeyID"] %>;
            });

            $("#liPreView").on("click",function(){
               layerCommon.openWindow("信息预览","NewsPreview.aspx?KeyID=<%=KeyID%>","1000px","500px");
            });

            $("#lblNewContent").find("a").attr("target","_blank");
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
    <div class="rightinfo">
    <div class="place">
       <ul class="placeul">
            <li><a href="../jsc.aspx" target="_top">我的桌面 </a></li><li>></li>
            <li><a href="../SysManager/NewsList.aspx">信息发布</a></li><li>></li>
            <li><a href="#">信息发布详细</a></li>
        </ul>
    </div>
        <div class="tools">
            <ul class="toolbar left">
                <li id="libtnEdit" runat="server"><span>
                    <img src="../../Company/images/t02.png" /></span>编辑</li>
                <li id="libtnDel" runat="server"><span>
                    <img src="../../Company/images/t03.png" /></span>删除<input type="button" runat="server"
                        id="btnDel" onserverclick="btn_Del" style="display: none;" /></li>
                <li id="liPreView" runat="server"><span><img src="../../Company/images/t17.png" /></span>预览
                </li>
                <li id="libtnback" runat="server" onclick="location.href = 'NewsList.aspx'"><span>
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
                            &nbsp;<%=title%>
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
                            <span>是否置顶</span>
                        </td>
                        <td>
                            <label runat="server" id="lblistop">
                            </label>
                        </td>
                    </tr>
                     <tr>
                        <td width="120">
                            <span>同时店铺发布</span>
                        </td>
                        <td>
                            <label runat="server" id="lblstate">
                            </label>
                        </td>
                        </tr>
                    <tr>
                     <td width="120">
                            <span>显示方式</span>
                        </td>
                        <td>
                            <label runat="server" id="lblShowType"></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>信息内容</span>
                        </td>
                        <td>
                            <label id="lblNewContent"><%=content%></label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    </form>
</body>
</html>


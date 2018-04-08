<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShopMessageInfo.aspx.cs" Inherits="Company_ShopManager_ShopMessageInfo" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Cache-Control" content="no-cache"> 
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>店铺留言详细</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script type="text/javascript">
        function GoBack() {
            window.location.href='ShopMessage.aspx';
        }
        $(function () {
            $("#libtnEdit").click(function () {
                layerCommon.confirm('确定要删除', function () {
                    $("#btnDelate").click();
                });
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
    <div class="rightinfo">


    <div class="place">
	    <ul class="placeul">
            <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
            <li><a href="ShopMessage.aspx">店铺留言</a></li><li>></li>
            <li><a href="javascript:;" runat="server" id="Atitle">店铺留言详细</a></li>
	    </ul>
     </div>
     <input type= "hidden" id="hid_Alert" />
        <div class="tools">
            <ul class="toolbar left">
                <asp:Button ID="btnDelate" runat="server" Text="Button" 
                    onclick="btnDelate_Click"  style="display:none;"/>
                <li id="libtnEdit" runat="server"><span><img src="../images/t03.png" /></span><font color="red">删除</font></li>
                <li id="lblbtnback" onclick="GoBack();"><span><img src="../images/tp3.png" /></span>返回</li>
            </ul>
        </div>
        <div class="div_content">
             <table class="tb" >
                <tbody>
                    <tr>
                        <td style="width:15%;"><span>姓名</span> </td>
                        <td style="width:30%;"><label runat="server" id="lblname"></label></td>
            
                    </tr>
                    <tr>
                        <td><span>手机</span> </td>
                        <td><label runat="server" id="lblphone"></label></td>
              
                    </tr>
                    <tr>
                        <td ><span>留言内容</span></td >
                        <td><label runat="server" id="lblremark"></label> </td>
                    </tr>
                    <tr>
                        <td ><span>状态</span></td >
                        <td ><label runat="server" id="lblread"></label> </td>
                    </tr>
                     <tr>
                        <td style="width:15%;"><span>留言时间</span> </td>
                        <td style="width:30%;"><label runat="server" id="lblDate"></label></td>
                    </tr>
                </tbody>
             </table>
        </div>
     </div>
    </form>
</body>
</html>

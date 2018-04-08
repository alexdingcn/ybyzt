<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RepSuggestInfo.aspx.cs" Inherits="Company_SysManager_RepSuggestInfo" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Cache-Control" content="no-cache"> 
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>留言回复详细</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            $("#libtnEdit").click(function() {
                //记录弹出对象
                var index = layerCommon.openWindow('留言回复', 'RepSuggest.aspx?ID=' + <%=Request.QueryString["ID"] %>, '660px', '270px');
                $("#hid_Alert").val(index); //记录弹出对象
            });
        });

        function Suggest(){
            layerCommon.layerClose($("#hid_Alert").val());
            window.location.href='RepSuggestList.aspx';
        }

        function Layerclose(){
          layerCommon.layerClose($("#hid_Alert").val());
        }

        function GoBack() {
            window.location.href='RepSuggestList.aspx?';
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
    <div class="rightinfo">


    <div class="place">
	    <ul class="placeul">
            <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
            <li><a href="../SysManager/RepSuggestList.aspx">留言回复</a></li><li>></li>
            <li><a href="#" runat="server" id="Atitle">留言回复详细</a></li>
	    </ul>
     </div>
     <input type= "hidden" id="hid_Alert" />
        <div class="tools">
            <ul class="toolbar left">
                <li id="libtnEdit" runat="server"><span><img src="../images/t02.png" /></span><font color="red">回复</font></li>
                <li id="lblbtnback" onclick="GoBack();"><span><img src="../images/tp3.png" /></span>返回</li>
            </ul>
        </div>
        <div class="div_content">
             <table class="tb" >
                <tbody>
                    <tr>
                        <td style="width:15%;"><span>代理商名称</span> </td>
                        <td style="width:30%;"><label runat="server" id="lblDisName"></label></td>
                        <td style="width:15%;"><span>留言时间</span> </td>
                        <td style="width:30%;"><label runat="server" id="lblCreateDate"></label></td>
                    </tr>
                    <tr>
                        <td><span>留言人</span> </td>
                        <td><label runat="server" id="lblCreateUser"></label></td>
                        <td><span>状态</span> </td>
                        <td><label runat="server" id="lblState"></label></td>
                    </tr>
                    <tr>
                        <td ><span>留言主题</span></td >
                        <td colspan="3"><label runat="server" id="lblTitle"></label> </td>
                    </tr>
                    <tr>
                        <td ><span>留言内容</span></td >
                        <td colspan="3"><label runat="server" id="lblContent"></label> </td>
                    </tr>
                    <tr id="CompRemark" runat="server">
                        <td ><span>回复内容</span></td >
                        <td colspan="3"><label runat="server" id="lblCompRemark"></label> </td>
                    </tr>
                    <tr id="CompRepk" runat="server" >
                        <td style="width:15%;"><span>回复人</span> </td>
                        <td style="width:30%;"><label runat="server" id="lblCompUser"></label></td>
                        <td style="width:15%;"><span>回复时间</span> </td>
                        <td style="width:30%;"><label runat="server" id="lblReplyDate"></label></td>
                    </tr>
                </tbody>
             </table>
        </div>
     </div>
    </form>
</body>
</html>

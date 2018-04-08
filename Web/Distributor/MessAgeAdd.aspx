<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MessAgeAdd.aspx.cs" Inherits="Distributor_MessAgeAdd" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>新增咨询</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script type="text/javascript">
        function addmessage() { 
            var str="";
            if ($.trim($("#txtTitle").val()) == "") {
                str += "-请输入标题<br/>";
            }
            if ($.trim($("#txtRemark").val()) == "") {
                str += "-请输入内容<br/>";
            }
            if (str != "") {
                layerCommon.alert(str);
                return false;
            }
            return true;
        }
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="nav-6" />
    <div class="rightCon">
    <div class="info"> <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="MessAgeList.aspx" class="cur">我要咨询</a>>
                <a id="navigation3" href="#" class="cur">新增咨询</a>
            </div>
        <!--功能条件 start-->
        <div class="userFun">
            <div class="left">
                <a href="MessAgeList.aspx" class="btnBl"><i class="returnIcon"></i>返回</a></div>
        </div>
        <!--功能条件 end-->
        <div class="blank10">
        </div>
        <div class="userTrend">
            <div class="uTitle">
                <b>咨询</b></div>
            <ul class="message">
                <li><i class="head"><i class="required">*</i>标题：</i><input id="txtTitle" name="" maxlength="20" runat="server" type="text" class="box" value="" style="width:240px;" /></li>
                <li><i class="head"><i class="required">*</i>内容：</i><textarea id="txtRemark" maxlength="200" name="" style="width:600px;" runat="server" class="box3"></textarea></li>
            </ul>
            <div class="mdBtn">
                <a href="#" onclick="if(!addmessage()){return false;}" onserverclick="AddMessAge" runat="server" class="btnYe"><i class="prnIcon"></i>确定</a></div>
            <div class="blank10">
            </div>
        </div>
    </div>
    </div>
    </form>
    
</body>
</html>

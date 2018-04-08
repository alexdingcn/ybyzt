<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportDis3.aspx.cs" Inherits="Company_ImportDis3" %>

<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>代理商导入</title>
    <link href="css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script>
        $(function () {
            //导出错误列表
            $(".dcerror").click(function () {
                $("#btnImport").click();
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Button ID="btnImport" runat="server" Text="Button" Style="display: none" OnClick="btnImport_Click" />
    <uc1:Top ID="top1" runat="server" ShowID="nav-4" />
    <uc1:CompRemove runat="server" ID="Remove" ModuleID="4" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="jsc.aspx">我的桌面 </a></li>
                 <li>&gt;</li>
                <li><a href="SysManager/DisList.aspx" id="a1">代理商列表</a></li>
                <li>&gt;</li>
                <li><a href="javascript:;" id="atitle">代理商导入</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <div class="blank10">
        </div>
        <!--步骤 start-->
        <ul class="imStep">
            <li class="a1 ">1、上传导入文件<i class="arr1"></i></li>
            <li class="a2">2、导入预览<i class="arr1"></i><i class="arr2"></i></li>
            <li class="a3 cur">3、导入完成<i class="arr1"></i><i class="arr2"></i></li>
        </ul>
        <!--步骤 end-->
        <!--导入规则说明 start-->
        <div class="imNo ">
            <div class="bt">
                <i class="imno-i"></i>导入失败！</div>
            <div class="text">
                共<i class="oclor oclor2">0</i>条数据，导入成功<i class="oclor oclor1">0</i>条，导入失败<i class="oclor oclor3">0</i>条</div>
            <div class="text2">
                <i class="le"><b>导入失败原因</b>：</i><ul class="le le2">
                </ul>
            </div>
            <div class="text3">
                您可以<a href="SysManager/DisList.aspx" class="bclor">代理商列表</a>|<a href="ImportDis.aspx"
                    class="bclor">再次导入</a>|<a href="JavaScript:;" class="bclor dcerror">导出错误列表</a></div>
        </div>
        <!--导入规则说明 end-->
        <!--导入规则说明 start-->
        <div class="imNo imOk none">
            <div class="bt">
                <i class="imok-i"></i>导入成功！</div>
            <div class="text">
                共<i class="oclor oclor1">0</i>条数据，全部导入成功</div>
            <div class="text3">
                您可以<a href="SysManager/DisList.aspx" class="bclor">代理商列表</a>|<a href="ImportDis.aspx"
                    class="bclor">再次导入</a></div>
        </div>
        <!--导入规则说明 end-->
    </div>
    </form>
</body>
</html>

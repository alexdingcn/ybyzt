<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Report6.aspx.cs" Inherits="Company_Report_Report6" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>应收分析</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var h = $(document).height();
            $("#reportDis").height(h - 95);
            //搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });
            ///重置
            $("#li_Reset").click(function () {
                window.location.href = window.location.href;
            });
            $("#cancel").click(function () {
                window.location.href="Reporttest.aspx";
            });
            //选择加盟商
            $("#txtDisID").click(function () {

                ///获取光标位置
                //var x = $("#txtDisID").offset().left;
                //var y = $("#txtDisID").offset().top;
                var Id = <%=this.CompID %>;

                //ChoseProductClass('/Company/UserControl/SelectDisList.aspx?compid=' + Id, 0, 0);
                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度
                var index = layerCommon.openWindow('选择代理商', '/Company/UserControl/SelectDisList.aspx?compid=' + Id, '880px', '450px'); //记录弹出对象
                $("#hid_Alert").val(index); //记录弹出对象
            });
        });
        function selectDis(id, name) {
            if (id.toString() != "") {
                $("#txtDisID").focus(); //解决 IE11 弹出层后文本框不能输入
                CloseDialog();
                $("#txtDisID").val(name); //区域名称
                $("#hidDisID").val(id); //区域id
            } else {
                $("#txtDisID").focus(); //解决 IE11 弹出层后文本框不能输入
                CloseDialog();
                $("#txtDisID").val(name); //区域名称
                $("#hidDisID").val(id); //区域id
            }
        }
        function CloseDialog() {
            //$("#btn_del").focus(); //解决 IE11 弹出层后文本框不能输入
            var showedDialog = $("#hid_Alert").val(); //获取弹出对象
            layerCommon.layerClose(showedDialog); //关闭弹出对象
            //$(".txt_count").focus();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-5" />
    <!--当前位置 end-->
    <input type="hidden" runat="server" id="hidDisID" />
    <input type="hidden" runat="server" id="hid_Alert" />
    <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
    <div class="rightinfo">
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="Reporttest.aspx">数据分析报表</a></li><li>></li>
                <li><a href="#">应收分析</a></li>
            </ul>
        </div>
        <div times="1" id="xubox_shade" class="xubox_shade" style="z-index: 19891015; background-color: #000;
            opacity: 0.3; filter: alpha(opacity=30);">
        </div>
        <!--功能按钮 end-->
        <iframe src="<%= url %>" name="reportDis" id="reportDis" title="reportDis" scrolling="no"
            style="width: 100%; height: auto;"></iframe>
    </div>

    <script type="text/javascript">
        var iframe = document.getElementById("reportDis");
        if (iframe.attachEvent) {
            iframe.attachEvent("onload", function () {
                //以下操作必须在iframe加载完后才可进行  
                $("#xubox_shade").hide();
            });
        } else {
            iframe.onload = function () {
                //以下操作必须在iframe加载完后才可进行  
                $("#xubox_shade").hide();
            };
        }  
    </script>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisPrice.aspx.cs" Inherits="Company_Goods_DisPrice" %>

<%@ Register Src="~/Company/UserControl/TreeDemo.ascx" TagPrefix="uc1" TagName="TreeDemo" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>代理商价目</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script>
        $(function () {
            if ($(".downBox option:selected").text() != "请选择") {
                $(".toolbar3 li").eq(1).show();
                $(".toolbar3 li").eq(2).show();
            } else {
                $(".toolbar3 li").eq(1).hide();
                $(".toolbar3 li").eq(2).hide();
            }
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            })
            $(".downBox").change(function () {
                if ($(".downBox option:selected").text() != "请选择") {
                    $(".toolbar3 li").eq(1).show();
                    $(".toolbar3 li").eq(2).show();
                } else {
                    $(".toolbar3 li").eq(1).hide();
                    $(".toolbar3 li").eq(2).hide();
                }
            })
        })
        // 每页显示数量非空验证
        function ChkPage() {
            if ($("#txtPager").val() == "") {
                layerCommon.msg("每页显示数量不能为空", IconOption.错误);
                return false;
            } else if (parseInt($("#txtPageSize").val()) == 0) {
                layerCommon.msg("每页显示数量必须大于0", IconOption.错误);
                return false;
            } else {
                var disId = $("#ddlDisList option:selected").val();
                if ($.trim(disId) == "") {
                    layerCommon.msg("请选择代理商", IconOption.错误);
                    return false;
                }
                $("#btnSearch").click();
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input id="hid_Alert" type="hidden" />
    <input type="hidden" id="hidCompId" runat="server" />
    <!--当前位置 end-->
    <div class="rightinfo" style="margin-top: 0px; margin-left: 0px; width: auto;">
        <!--功能按钮 start-->
        <div class="tools">
            <div class="right">
                <ul class="toolbar right ">
                    <li onclick="return ChkPage()"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                    <uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />
                </ul>
                <ul class="toolbar3">
                    <li>选择代理商:<asp:DropDownList ID="ddlDisList" runat="server" CssClass="downBox">
                    </asp:DropDownList>
                    </li>
                    <li>商品名称:<input name="txtGoodsName" type="text" id="txtGoodsName" runat="server"
                        style="width: 110px" class="textBox txtGoodsName" maxlength="50" /></li>
                    <li>商品分类:<uc1:TreeDemo runat="server" ID="txtCategory" />
                        &nbsp;&nbsp; </li>
                    <li>每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                        class="textBox" style="width: 40px;" value="50" />&nbsp;条 </li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <!--信息列表 start-->
        <table class="tablelist" id="TbList">
            <thead>
                <tr>
                    <th>
                        商品名称
                    </th>
                    <th class="t3">
                        商品规格属性
                    </th>
                    <th class="t5">
                        基础价格(元)
                    </th>
                    <th class="t5">
                        代理商价格(元)
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptDisPrice" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <div class="tcle"><%# Eval("GoodsName").ToString()%></div>
                            </td>
                            <td>
                                <div class="tcle"><%# GoodsAttr(Eval("id").ToString())%></div>
                            </td>
                            <td>
                               <div class="tc"> <%#  decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(Eval("SalePrice")).ToString())).ToString("#,##0.00")%></div>
                            </td>
                            <td>
                               <div class="tc"> <label <%# GetPrice(Eval("Id").ToString(),Eval("SalePrice").ToString())==Eval("SalePrice").ToString()?"":"style='color:red'" %>>
                                    <%#  decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(GetPrice(Eval("Id").ToString(),Eval("SalePrice").ToString())).ToString())).ToString("#,##0.00")%>
                                </label></div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <div class="pagin">
            <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                NextPageText=">" PageSize="50" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                OnPageChanged="Pager_PageChanged">
            </webdiyer:AspNetPager>
        </div>
        <!--列表分页 end-->
        <asp:Button ID="btnSearch" runat="server" Style="display: none" Text="搜索" OnClick="btnSearch_Click" />
        <!--信息列表 end-->
    </div>
    </form>
    <script type="text/javascript">
        $('.tablelist tbody tr:odd').addClass('odd');
    </script>
</body>
</html>

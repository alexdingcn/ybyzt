<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateDisPrice.aspx.cs" Inherits="Company_Goods_UpdateDisPrice" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TreeDisType.ascx" TagPrefix="uc1" TagName="TreeDisType" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>商品调价列表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="rightinfo" style="margin-top: 0px; margin-left: 0px; width: auto;">
        <!--功能按钮 start-->
        <div class="tools">
            <div class="right">
                <ul class="toolbar right ">
                    <li onclick="return ChkPage()"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                    <uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" Visible="false" />
                </ul>
                <ul class="toolbar3">
                    <li>代理商分类:
                        <uc1:TreeDisType runat="server" ID="txtDisType" />
                    </li>
                    <li>代理商名称:<input name="txtDisName" type="text" id="txtDisName" runat="server" class="textBox txtDisName" maxlength="50" /></li>
                    <li>每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                        class="textBox" style="width: 40px;" value="50" />&nbsp;条 </li>
                </ul>
            </div>
        </div>
        <ul style="float: left; margin-bottom: 10px;">
            <li>商品名称：<label id="lblGoodsName" runat="server"></label>&nbsp;&nbsp;&nbsp;&nbsp; 商品属性：<label
                id="lblAttribute" runat="server"></label>
            </li>
        </ul>
        <table class="tablelist">
            <thead>
                <tr>
                    <th>
                        代理商名称
                    </th>
                    <th>
                        最新价格（非促销）
                    </th>
                    <th>
                        调整价格
                    </th>
                    <th style="display: none">
                        操作
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptDis" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input type="hidden" name="lblDisId" value='<%# Eval("ID") %>' />
                                <%# Eval("DisName") %>
                            </td>
                            <td>
                                <%# GoodsTinkerPrice(goodsInfoId,Eval("id").ToString(),1)%>
                            </td>
                            <td>
                                <%--        <label id="lblPrice<%# Eval("ID") %>">
                                    <%# GoodsTinkerPrice(goodsInfoId,Eval("id").ToString(),1)%>
                                </label>--%>
                                <input style="margin: 0px 10px 0 0;" id="txtPrice<%# Eval("ID") %>" name="txtPrice"
                                    class="textBox" type="text" onkeyup="KeyInt2(this);" value='<%# GoodsTinkerPrice(goodsInfoId,Eval("id").ToString(),2)%>' />
                            </td>
                            <td style="display: none">
                                <img alt="编辑" title="编辑" style="cursor: pointer;" onclick="return SavePrice(<%# Eval("ID") %>)"
                                    id="img<%# Eval("ID") %>" src="../images/t10.png" />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <div class="pagin" style="margin-top: 30px;">
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
        <asp:Button ID="btnSearch" runat="server" Style="display: none" Text="搜索" OnClick="btnSearch_Click" />
        <div class="div_footer" style="padding-top: 40px; display: none;">
            <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClick="btnAdd_Click" />&nbsp;
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $('.tablelist tbody tr:odd').addClass('odd');
        $(function () {
            $(".showDiv3 .ifrClass").css("width", "155px");
            $(".showDiv3").css("width", "150px");
            $('iframe').load(function () {
                $('iframe').contents().find('.pullDown').css("width", "150px");
            })
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            })
            //确定
            $("#btnAdd").click(function () {
                var price = $("input[name='txtPrice']").val();
                if ($.trim(price) == "") {
                      layerCommon.msg("调整价格不能为空", IconOption.错误);
                    return false;
                }
                return true;
            })
            if ( <%=  num %>!=0) {
                $(".div_footer").show();
                $(".tablelist").show();
            } else {
                $(".div_footer").hide();
                $(".tablelist").hide();
            }
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
              $(".hid_Alert").val("");
                $("#btnSearch").click();
            }
            return true;
        }
        //价格调整
        function SavePrice(id) {
            if ($("#img" + id).attr("title") == "编辑") {
                $("#lblPrice" + id).hide();
                $("#txtPrice" + id).show();
                $("#img" + id).attr("src", "../images/icon_save.png");
                $("#img" + id).attr("title", "保存");
                return;
            }
            var price = $("#txtPrice" + id).val();
            if ($.trim(price) == "") {
                      layerCommon.msg("价格不能为空", IconOption.错误);
                return;
            }
            $.ajax({
                type: "post",
                url: "UpdateDisPrice.aspx",
                data: { ck: Math.random(), action: "save", id: id, price: price },
                dataType: "text",
                success: function (data) {
                    if (data != "") {
                        $("#lblPrice" + id).text(formatMoney(price, 2));
                        $("#lblPrice" + id).show();
                        $("#txtPrice" + id).hide();
                        $("#img" + id).attr("src", "../images/t10.png");
                        $("#img" + id).attr("title", "编辑");
                    } else {
                                   layerCommon.msg("保存失败", IconOption.错误);
                        return;
                    }
                }, error: function () { }
            })
        }
    </script>
</body>
</html>

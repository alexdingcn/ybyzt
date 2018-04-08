<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsAreasEdit.aspx.cs" Inherits="Company_Goods_GoodsAreasEdit" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TreeDisArea.ascx" TagPrefix="uc1" TagName="TreeDisArea" %>
<%@ Register Src="~/Company/UserControl/TreeDemo.ascx" TagPrefix="uc1" TagName="TreeDemo" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>商品不可售区域新增</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script>
        $(function () {
            var count = $(".tablelist tr").length;
            if (count > 1) {
                $(".orangeBtn").show();
            } else {
                $(".orangeBtn").hide();
            }
            //返回
            $(".cancel").click(function () {
                location.href = "GoodsAreasList.aspx";
            })
        })
        //搜索
        function ChkPage() {
            var area = $(".hid_AreaId").val();
            if ($.trim(area) == "") {
                layerCommon.msg("请选择区域", IconOption.错误);
                return false;
            }
            if ($("#txtPageSize").val() == "") {
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-3" />
    <input type="hidden" id="hid_Alert">
    <input type="hidden" name="hid_org_id" id="hid_org_id">
    <div class="rightinfo">
        <div class="place">
        <ul class="placeul">
            <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
            <li><a href="GoodsAreasList.aspx">商品不可售区域</a></li><li>></li>
            <li><a href="GoodsAreasedit.aspx">商品不可售区域新增</a></li>
        </ul>
    </div>
        <!--功能按钮 start-->
        <div class="tools">
            <div class="right">
                <ul class="toolbar right ">
                    <li onclick="return ChkPage()"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                </ul>
                <ul class="toolbar3">
                    <li>
                        <label class="required">
                            *</label>代理商区域:<uc1:TreeDisArea runat="server" ID="txtDisAreaBox" />
                    </li>
                    <li>
                        <%--<label class="required">*</label>--%>商品分类:
                        <uc1:TreeDemo runat="server" ID="txtCategory" />
                    </li>
                    <li>商品名称:<input name="txtGoodsName" type="text" id="txtGoodsName" runat="server"
                        class="textBox txtGoodsName" maxlength="50" /></li>
                    <li>每页<input name="txtPageSize" type="text" class="textBox txtPageSize" id="txtPageSize"
                        style="width: 40px" runat="server" value="100" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />条</li>
                </ul>
            </div>
        </div>
        <table class="tablelist">
            <thead>
                <tr>
                    <th>
                        <input type="checkbox" name="checkbox" id="CB_SelAll" onclick="SelectAll(this)" />
                    </th>
                    <th>
                        商品名称
                    </th>
                    <th>
                        商品分类
                    </th>
                    <th>
                        规格属性
                    </th>
                    <th>
                        价格(元)
                    </th>
                    <th>
                        计量单位
                    </th>
                    <th>
                        状态
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptGoods" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="text-indent: 6px;">
                                <asp:CheckBox ID="CB_SelItem" runat="server"></asp:CheckBox>
                                <asp:HiddenField ID="HF_Id" runat="server" Value='<%# Eval("ID") %>' />
                                <asp:HiddenField ID="HF_CateID" runat="server" Value='<%# Eval("CategoryID") %>' />
                            </td>
                            <td style="width: 300px">
                                    <%# Eval("GoodsName").ToString() %>
                            </td>
                            <td>
                                <%# GoodsCategory(Eval("categoryID").ToString())%>
                            </td>
                            <td>
                                <%# GoodsAttr2(Eval("ID").ToString())%>
                            </td>
                            <td>
                                <%#  decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(Eval("SalePrice")).ToString())).ToString("#,##0.00")%>
                            </td>
                            <td>
                                <%# Eval("Unit").ToString()%>
                            </td>
                            <td>
                                <%# Eval("isOffline").ToString()=="1"?"上架":"下架" %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <div class="pagin">
            <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                OnPageChanged="Pager_PageChanged">
            </webdiyer:AspNetPager>
        </div>
        <asp:Button ID="btnSearch" runat="server" Style="display: none" Text="搜索" OnClick="btnSearch_Click" />
        <div class="div_footer" style="padding-top: 50px;">
            <asp:Button ID="btnAdd" CssClass="orangeBtn" Style="display: none" runat="server"
                Text="确定不可售设置" OnClick="btnAdd_Click" />&nbsp;
            <input name="" type="button" class="cancel" value="返回" />
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $('.tablelist tbody tr:odd').addClass('odd');
    </script>
</body>
</html>

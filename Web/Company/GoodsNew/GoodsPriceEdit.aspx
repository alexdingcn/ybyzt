<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsPriceEdit.aspx.cs" Inherits="Company_Goods_GoodsPriceEdit" %>

<%@ Register Src="~/Company/UserControl/TreeDemo.ascx" TagPrefix="uc1" TagName="TreeDemo" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>商品选择</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script>
        $(function () {
            if ( <%=  num %>== 1) {
                $(".div_footer").show();
                $(".tablelist").show();
            } else {
                $(".div_footer").hide();
                $(".tablelist").hide();
            }
            //确定按钮
            $("#btnAdd").click(function () {
                var z = 0;
                $("input[type='checkbox']").each(function (index, obj) {
                    if (obj.checked) {
                        z++;
                    }

                })
                if (z != 0) {
                    return true;
                } else {
                          layerCommon.msg("请选择商品", IconOption.错误);
                    return false;
                }
            })
        })
        //搜索
         // 每页显示数量非空验证
        function ChkPage() {
            if ($("#txtPager").val() == "") {
                           layerCommon.msg("每页显示数量不能为空", IconOption.错误);
                return false;
            } else if (parseInt($("#txtPager").val()) == 0) {
                           layerCommon.msg("每页显示数量必须大于0", IconOption.错误);
                return false;
            } else {
                $("#btnSearch").click();
            }
            return true;
        }
        //重置
        function Fanh() {
  location=location;
        }
        
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="hid_Alert">
    <input type="hidden" name="hid_org_id" id="hid_org_id">
    <div class="rightinfo" style="margin-top: 0px; margin-left: 0px; width: auto;">
        <div class="tools">
            <div class="right">
                <ul class="toolbar right ">
                    <li onclick="return ChkPage()"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                </ul>
                <ul class="toolbar3">
                    <li style="color: red">请选择类别并搜索！ &nbsp; &nbsp; </li>
                    <li>类别名称:
                        <uc1:TreeDemo runat="server" ID="txtCategory" />
                    </li>
                    <li>商品名称:<input name="txtGoodsName" type="text" id="txtGoodsName" runat="server"
                        class="textBox txtGoodsName" maxlength="50" /></li>
                    <li>每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                        class="textBox" style="width: 40px;" value="50" />&nbsp;条 </li>
                </ul>
            </div>
        </div>
        <table class="tablelist" style="display: none;">
            <thead>
                <tr>
                    <th class="t7">
                        <input type="checkbox" name="checkbox" id="CB_SelAll" onclick="SelectAll(this)" />
                    </th>
                    <th>
                        商品名称
                    </th>
                    <th class="t3">
                        商品规格属性
                    </th>
                    <th class="t5">
                        销售价格(元)
                    </th>
                    <%--  <th width="13%">
                        调整后价格(元)
                    </th>--%>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptGoodsInfo" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                               <div class="tc"> <asp:CheckBox ID="CB_SelItem" runat="server"></asp:CheckBox>
                                <asp:HiddenField ID="HF_Id" runat="server" Value='<%# Eval("ID") %>' />
                                <input id="HF_Price<%# Eval("ID") %>" type="hidden" /></div>
                            </td>
                            <td>
                               <div class="tcle"> <a href="javascript:;" title='<%# Eval("GoodsName").ToString() %>' class="link edit"
                                    style="text-decoration: underline; width: 300px" tip='<%# Eval("ID") %>'>
                                    <%# Eval("GoodsName").ToString() %></div>
                                </a>
                            </td>
                            <td>
                               <div class="tcle"><%# Eval("ValueInfo").ToString() %></div>
                            </td>
                            <td>
                                <div class="tc"><%#  decimal.Parse(GoodsTinkerPrice( Convert.ToInt32( Eval("ID").ToString()), disId.ToString(), string.Format("{0:N2}", Convert.ToDecimal(Eval("tinkerPrice")).ToString()))).ToString("#,##0.00")%></div>
                            </td>
                            <%--    <td>
                                <asp:TextBox ID="txtPrice" CssClass='<%# "textBox txtPrice"+Eval("id")%>' Text='<%# decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(Eval("TinkerPrice")).ToString())).ToString("0.00")%>'
                                    onkeyup="KeyInt2(this);" runat="server"></asp:TextBox>
                            </td>--%>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <!--列表分页 start-->
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
        <div class="div_footer" style="display: none; padding-top: 40px;">
            <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClick="btnAdd_Click" />&nbsp;
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $('.tablelist tbody tr:odd').addClass('odd');
    </script>
</body>
</html>

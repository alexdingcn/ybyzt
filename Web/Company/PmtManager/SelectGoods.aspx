<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectGoods.aspx.cs" Inherits="Company_PmtManager_SelectGoods" %>

<%@ Register Src="~/Company/UserControl/TreeDemo.ascx" TagPrefix="uc1" TagName="TreeDemo" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>商品选择</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script>
         //禁用F12
        document.onkeydown = function (e) {
            var ev = window.event || e;
            var code = ev.keyCode || ev.which;
            var code = ev.keyCode || ev.which || ev.charCode;
            if (code == 123) {
                if (ev.preventDefault) {
                    ev.preventDefault();
                } else {
                    ev.keyCode = 505;
                    ev.returnValue = false;
                }
            }
        }

        $(function () {
            $(".showDiv .ifrClass").css("width", "155px");
            $(".showDiv").css("width", "150px");

            if('<%=Request["type"]+"" %>'=="1"){
                $(".rightinfo").css("width","auto");
            }

            $('iframe').load(function () {
                $('iframe').contents().find('.pullDown').css("width", "150px");
            });
            if ( <%=  num %>== 1) {
                $(".div_footer").show();
                $(".tablelist").show();
            } else {
                $(".div_footer").hide();
                $(".tablelist").hide();
            }
            //确定按钮
            $("#btnAdd").click(function () {
               var infoid=$("#info").val();
               var z = 0;
                $("input[type='checkbox']").each(function (index, obj) {
                    if (obj.checked) {
                        z++;
                    }

                })

                if (infoid.length+z != 0) {
                    return true;
                } else {
                     layerCommon.msg("请选择商品", IconOption.错误);
                    return false;
                }
            })
        })
        //搜索
        function ChkPage() {
            $("#btnSearch").click();
            return true;
        }
        //重置
        function Fanh() {
            $(".txt_product_class").val("");
            $(".hid_product_class").val("");
            $(".txtGoodsName").val("");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="hid_Alert" />
    <input type="hidden" id="info" runat="server" />
    <input type="hidden" name="hid_org_id" id="hid_org_id" />
    <div class="rightinfo" style="margin-top: 0px; margin-left: 0px;">
        <div class="tools">
            <div class="right">
                <ul class="toolbar right ">
                    <li onclick="return ChkPage()"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                    <li onclick="return Fanh()"><span>
                        <img src="../images/t06.png" /></span>重置</li>
                </ul>
                <ul class="toolbar3">
                    <li style="color: red"></li>
                    <li>商品分类:
                        <uc1:TreeDemo runat="server" ID="txtCategory" />
                    </li>
                    <li>商品名称/编码:<input name="txtGoodsName" type="text" id="txtGoodsName" runat="server"
                        class="textBox txtGoodsName" /></li>
                    <li>每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                        class="textBox" style="width: 40px;" />&nbsp;条 </li>
                </ul>
            </div>
        </div>
        <table class="tablelist" style="display: none">
            <thead>
                <tr>
                    <th class="t7">
                        <input type="checkbox" name="checkbox" id="CB_SelAll" onclick="SelectAll(this)" />
                    </th>
                    <th class="t6">
                        商品名称(编码)
                    </th>
                    <th class="t6">
                        商品规格属性
                    </th>
                    <th class="t2">
                        基础价格(元)
                    </th>
                    <%--<th width="13%">
                        调整后价格(元)
                    </th>--%>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptGoodsInfo" runat="server" OnItemDataBound="rptGoodsInfo_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <div class="tc">
                                    <asp:CheckBox ID="CB_SelItem" runat="server"></asp:CheckBox>
                                    <asp:HiddenField ID="HF_Id" runat="server" Value='<%# Eval("ID") %>' />
                                    <input id="HF_Price<%# Eval("ID") %>" type="hidden" />
                                </div>
                            </td>
                            <td>
                                <div class="tcle">
                                    <a href="javascript:;" title='<%# Eval("GoodsName").ToString() %>' class="link edit"
                                        style="text-decoration: underline; width: 300px" tip='<%# Eval("ID") %>'>
                                        <%# Eval("GoodsName").ToString()%>(<%# Eval("Barcode").ToString()%>)
                                    </a>
                                </div>
                            </td>
                            <td>
                                <div class="tcle">
                                    <%# Eval("ValueInfo").ToString() == "" ? Eval("memo").ToString() : Eval("ValueInfo").ToString() %></div>
                            </td>
                            <td>
                                <div class="tc">
                                    <%#  decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(Eval("SalePrice")).ToString())).ToString("#,##0.00")%></div>
                            </td>
                            <%--  <td>
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

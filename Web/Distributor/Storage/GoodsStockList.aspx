<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsStockList.aspx.cs"
    Inherits="Distributor_Storage_GoodsStockList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>商品批次库存</title>
   <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Company/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../../js/CommonJs.js"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            });

            $("#btnAdd").click(function () {
                window.location.href = 'CMerchantsAdd.aspx';
            });

            //订单生成 搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });

            ///重置
            $("#li_Reset").click(function () {
                window.location.href = 'CMerchantsList.aspx';
            });
        });

    </script>
    <style type="text/css">
        .tablelist td a
        {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="nav-7" />
        <div class="rightCon">
            <div class="info"><a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
                    <a id="navigation2" href="../Storage/GoodsStockList.aspx" class="cur">商品批次库存</a></div>

            <!--功能条件 start-->
            <div class="userFun">
                <div class="left">
                </div>
                <div class="right">
                    <ul class="term">

                        <li><label class="head">商品名称:</label><input name="txtGoodsName" type="text" id="txtGoodsName" runat="server" class="box" maxlength="50" /></li>

                        <a id="A1" href="#" class="btnBl" onserverclick="btnSearch_Click" runat="server"><i class="searchIcon"></i>搜索</a>
    
                    </ul>
                </div>
            </div>
            <div class="hidden userFun" style=" text-align:right; padding-right:160px; padding-top:10px; display:none;">
                 <div class="right">
                    <ul class="term">
                           
                    </ul>
                </div>
            </div>
            <!--功能条件 end-->
            <div class="blank10"></div>
            <div class="orderNr">
                <table class="PublicList list" id="orderBg" border="0" cellspacing="0" cellpadding="0">
                    <asp:Repeater ID="rptOrder" runat="server">
                        <HeaderTemplate>
                            <thead>
                                <tr>
                                    <%--<th>商品编码</th>--%>
                                    <th>商品名称</th>
                                    <%--<th>商品分类</th>--%>
                                    <th>规格属性</th>
                                    <th>批次号</th>
                                    <th>批次有效期</th>
                                    <th>单位</th>
                                    <th>库存</th>
                                </tr>
                            </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr id='tr_<%# Eval("Id") %>' <%# isvalidDate(Eval("validDate").ToString())=="1"?"style=\" color:Red;\"":""  %>>
                                <td>
                                    <%# Eval("GoodsName")%>
                                </td>
                                <td>
                                    <%# Eval("ValueInfo")%>    
                                </td>
                                <td>
                                    <%# Eval("BatchNO")%>
                                </td>
                                <td>
                                    <%# Eval("validDate", "{0:yyyy-MM-dd}")%>
                                </td>
                                <td>
                                    <%# Eval("Unit")%>
                                </td>
                                <td>
                                    <%# Eval("StockNum")%>
                                </td>
     
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody> </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <!--列表分页 start-->
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
            <!--列表分页 end-->
        </div>
    </div>
    </form>
</body>
</html>

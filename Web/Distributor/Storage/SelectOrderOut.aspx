<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectOrderOut.aspx.cs"
    Inherits="Distributor_Storage_SelectOrderOut" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>招商列表</title>

    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../Company/css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../Company/js/order.js" type="text/javascript"></script>
    <link href="../newOrder/css/global2.5.css?v=2015001311899" rel="stylesheet"
        type="text/css" />
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
            $("#btnCancel").click(function () {
                window.parent.closeAll();
            })

            //确定
            $("#btnConfirm").click(function () {
                var str = "";
                $("table tbody tr").each(function (item) {
                    if ($(this).find(".chkbox").is(':checked'))
                        str += $(this).find(".chkbox").val() + ",";
                });
                if (str != "") {
                    str = str.substring(0, str.length - 1);
                    window.parent.GoodsList(str);
                }
                window.parent.closeAll();
            });

        });

    </script>
    <style type="text/css">
        .tablelist td a {
            text-decoration: underline;
        }

        td {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <input id="hid_Alert" type="hidden" />
        <input type="hidden" id="hidCompId" runat="server" />
        <div class="rightinfo" style="width: 1000px; margin: 0 auto; margin-top: 10px;">
            <!--功能按钮 start-->
            <div class="tools">

                <div class="right">
                    <ul class="toolbar right">
                        <li id="Search"><span>
                            <img src="../../Company/images/t04.png" /></span>搜索</li>
                        <%--<li id="export"><span><img src="../images/tp3.png" /></span>导出</li>--%>
                    </ul>
                    <ul class="toolbar3">
                        <li>发货单号:<input name="ReceiptNo" type="text" id="ReceiptNo" runat="server" class="textBox" maxlength="50" /></li>
    
                        <li>发货日期:<input name="txtCreateDate" runat="server" onclick="var endDate = $dp.$('txtEndCreateDate'); WdatePicker({ onpicked: function () { endDate.focus(); }, maxDate: '#F{$dp.$D(\'txtEndCreateDate\')}' })"
                            style="width: 100px;" id="txtCreateDate" readonly="readonly" type="text" class="Wdate"
                            value="" />&nbsp;-&nbsp;
                        <input name="txtEndCreateDate" runat="server" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'txtCreateDate\')}' })"
                            style="width: 100px;" id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate"
                            value="" /></li>

                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->
            <table class="tablelist" id="TbList" ">
                <thead>
                    <tr>
                        <th class="t8"></th>
                        <th class="t3">发货单编号
                        </th>
                        <th class="t2">发货日期
                        </th>
                        <th class="t1">商品名称
                        </th>
                        <th class="t1">规格属性
                        </th>
                        <th class="t1">批次
                        </th>
                        <th class="t1">有效期
                        </th>
                        <th class="t1">数量
                        </th>

                    </tr>
                </thead>
                <tbody>
                    <!--信息列表 start-->
                    <asp:Repeater runat="server" ID="rptOrder">
                        <ItemTemplate>
                            <tr id='tr_<%# Eval("Id") %>'>
                                <td style="text-align: center;">
                                    <input type="checkbox" id="checkbox-<%#Eval("Detailid")%>" class="regular-checkbox chkbox" value="<%# Eval("Detailid") %>">
                                    <label for="checkbox-<%#Eval("Detailid")%>" class="checkboxLabel"></label>
                                </td>
                                <td>
                                    <div class="tc">
                                        <%# Eval("ReceiptNo")%>
                                    </div>
                                </td>
                                <td>
                                    <div class="tc">
                                        <%# Eval("SendDate", "{0:yyyy-MM-dd}")%>
                                    </div>
                                </td>
                                <td>
                                    <div class="tc">
                                        <%#  Common.MySubstring(Eval("GoodsName").ToString(),30,"...")%>
                                    </div>
                                </td>
                                <td>
                                    <div class="tc">
                                       <%# Eval("ValueInfo")%>
                                    </div>
                                </td>

                                <td>
                                    <div class="tc">
                                       <%# Eval("BatchNO")%>
                                    </div>
                                </td>

                                <td>
                                    <div class="tc">
                                        <%# Eval("validDate", "{0:yyyy-MM-dd}")%>
                                    </div>
                                </td>

                                <td>
                                    <div class="tc">
                                        <%# Eval("SignNum")%>
                                    </div>
                                </td>


                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <!--信息列表 end-->
            <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
            <%--<asp:Button ID="btnVolumeDel" Text="批量删除" runat="server" OnClick="btnVolumeDel_Click"
            Style="display: none" />--%>
            <!--列表分页 start-->
            <div class="pagin">
                <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                    NextPageText=">" PageSize="6" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                    ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                    TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                    CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                    ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                    CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                    OnPageChanged="Pager_PageChanged">
                </webdiyer:AspNetPager>
            </div>
            <!--列表分页 end-->
            <!--商品 end-->
            <div class="po-btn">
                <a href="javascript:void(0);" class="gray-btn" id="btnCancel">取消</a> <a href="javascript:void(0);"
                    class="btn-area" id="btnConfirm">确定</a>
            </div>
        </div>
    </form>
</body>
</html>

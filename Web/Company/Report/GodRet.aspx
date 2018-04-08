<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GodRet.aspx.cs" Inherits="Company_Report_GodRet" %>

<!DOCTYPE html>

<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager, Version=7.2.0.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>商品销售明细</title>
    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <style>
        .timeClass a
        {
            height: 23px;
            line-height: 23px;
            display: inline-block;
            border: 1px solid #ddd;
            padding: 0px 10px;
            margin: 0 0 0 5px;
        }
        a.hover
        {
            color: #fff;
            border: 1px solid #5e89c9;
            background: #779bd1;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var bdate = $(".txtBCreateDate").val(); //开始日期
            var edate = $(".txtECreateDate").val(); //结束日期
            if (!Date.prototype.toISOString) {
                Date.prototype.toISOString = function () {
                    function pad(n) { return n < 10 ? '0' + n : n }
                    return this.getUTCFullYear() + '-'
                    + pad(this.getUTCMonth() + 1) + '-'
                    + pad(this.getUTCDate()) + 'T'
                    + pad(this.getUTCHours()) + ':'
                    + pad(this.getUTCMinutes()) + ':'
                    + pad(this.getUTCSeconds()) + '.'
                    + pad(this.getUTCMilliseconds()) + 'Z';
                }
            }
            //页面刷新后日期的选中不能修改
            if (bdate == dateRangeUtil.getCurrentWeek()[0].toISOString().slice(0, 10) && edate == dateRangeUtil.getCurrentDate().toISOString().slice(0, 10)) {
                $(".timeClass a").eq(0).addClass("hover").siblings().removeClass("hover");
            } else if (bdate == dateRangeUtil.getCurrentMonth()[0].toISOString().slice(0, 10) && edate == dateRangeUtil.getCurrentDate().toISOString().slice(0, 10)) {
                $(".timeClass a").eq(1).addClass("hover").siblings().removeClass("hover");
            } else if (bdate == dateRangeUtil.getCurrentYear()[0].toISOString().slice(0, 10) && edate == dateRangeUtil.getCurrentDate().toISOString().slice(0, 10)) {
                $(".timeClass a").eq(2).addClass("hover").siblings().removeClass("hover");
            } else {
                $(".timeClass a").removeClass("hover");
            }
            //回车键事件
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            })
            //本周、本月、本年切换
            $(".timeClass a").click(function () {
                $(this).addClass("hover").siblings().removeClass("hover");
                switch ($.trim($(this).text())) {
                    case "本周": $(".txtBCreateDate").val(dateRangeUtil.getCurrentWeek()[0].toISOString().slice(0, 10)); //开始日期
                        break;
                    case "本月": $(".txtBCreateDate").val(dateRangeUtil.getCurrentMonth()[0].toISOString().slice(0, 10)); //开始日期
                        break;
                    default: $(".txtBCreateDate").val(dateRangeUtil.getCurrentYear()[0].toISOString().slice(0, 10)); //开始日期
                        break;
                }
                $(".txtECreateDate").val(dateRangeUtil.getCurrentDate().toISOString().slice(0, 10)); //结束日期
                $("#btnSearch").trigger("click");
            })
        })
        $(document).ready(function () {
            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');
            //搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });
            ///重置
            $("#li_Reset").click(function () {
                window.location.href = 'DisRpt.aspx';
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-5" />
    <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a href="GodRpt.aspx">商品销售明细</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li id="Search"><span>
                        <img src="../../Company/images/t04.png" /></span>搜索</li>
                    <uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />
                </ul>
                <ul class="toolbar3">
                    <li>代理商:<input name="txtDisName" type="text" id="txtDisName" runat="server" class="textBox" /> </li>
                    <li>商品:<input name="txtGoodsName" type="text" id="txtGoodsName" runat="server" class="textBox" /></li>
                    <%--  <li>订单状态：<select name="OState" runat="server" id="ddrOState" class="downBox">
                        <option value="-2">全部</option>
                        <option value="2">已审核</option>
                        <option value="4">已发货</option>
                        <option value="5">已到货</option>
                    </select>
                    </li>--%>
                    <li>下单日期：
                        <input name="txtBCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                            id="txtBCreateDate" readonly="readonly" type="text" class="Wdate txtBCreateDate"
                            value="" />&nbsp; -&nbsp;
                        <input name="txtECreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                            id="txtECreateDate" readonly="readonly" type="text" class="Wdate txtECreateDate"
                            value="" />
                    </li>
                    <li class="timeClass"><a href="javascript:;" class="hover">本周</a> <a href="javascript:;">
                        本月</a> <a href="javascript:;">本年</a> </li>
                    <li>每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                        class="textBox" style="width: 40px;" />&nbsp;条 </li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <!--信息列表 start-->
        <asp:Repeater ID="rptOrder" runat="server">
            <HeaderTemplate>
                <table class="tablelist" id="TbList">
                    <thead>
                        <tr>
                            <th class="t3">
                                订单编号
                            </th>
                            <th class="t5">
                                下单日期
                            </th>
                            
                            <th class="t3">
                                代理商名称
                            </th>
                            <th class="t5">
                                商品编码
                            </th>
                             <th class="t3">
                                商品名称
                            </th>
                            <th class="t5">
                                订单数量
                            </th>
                            <th class="t5">
                                订单金额
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr id='tr_<%# Eval("ID") %>'>
                    <td>
                        <div class="tcle">
                            <%#Eval("ReceiptNo")%></div>
                    </td>
                    <td>
                        <div class="tc">
                            <%#Convert.ToDateTime( Eval("CreateDate")).ToString("yyyy-MM-dd")%></div>
                    </td>
                     
                     <td>
                        <div class="tc">
                            <%# Eval("DisName")%></div>
                    </td>
                      <td>
                        <div class="tc">
                            <%#Eval("GoodsCode")%></div>
                    </td>
                      <td>
                        <div class="tc">
                         <%#Common.MySubstring(Convert.ToString(Eval("GoodsName")),40,"...")%></i></div>
                    </td>
                    <td>
                        <div class="tc">
                            <%#  Convert.ToInt32(Eval("GoodsNum"))%></div>
                          
                    </td>
                    <td>
                        <div class="tc">
                            <%# Convert.ToDecimal(Eval("AuditAmount") == DBNull.Value ? 0 : Eval("AuditAmount")).ToString("N")%></div>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>           
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
        <!--信息列表 end-->
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
    <div style="margin-left: 120px;">
        <span style="padding: 20px 0px 0px 20px; color: red;">统计说明：1、订单通过厂商审核后才计入统计 &nbsp;&nbsp;&nbsp;&nbsp;2、退货单通过审核后才计入统计&nbsp;&nbsp;&nbsp;&nbsp;3、作废的订/退货单不计入统计</span></div>
    </form>
</body>
</html>

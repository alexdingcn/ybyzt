<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsPriceInfo.aspx.cs" Inherits="Company_Goods_GoodsPriceInfo" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>代理商调价明细</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script>
        $(function () {
            //返回
            $(".cancel").click(function () {
                location.href = "DisPriceList.aspx";
            })
        })
    </script>
    <style>
        .tb2
        {
            width: 100%;
            height: auto;
            border-bottom: medium none;
        }
        .tb2 table
        {
            border-collapse: collapse;
            border-spacing: 0;
        }
        .tb2 .span
        {
            background: none repeat scroll 0 0 #f6f6f6;
            display: block;
            padding-right: 10px;
            text-align: right;
            white-space: nowrap;
        }
        .tb2 label
        {
            padding-left: 5px;
        }
        .tb2 td
        {
            border: 1px solid #dedede;
            font-size: 13px;
            line-height: 30px;
            text-align: left;
        }
        .tb2 span i
        {
            color: red;
            margin-right: 5px;
        }
        .dh3 td
        {
            border: 0px solid #dedede;
            font-size: 13px;
            line-height: 35px;
            text-align: left;
        }
        .textarea
        {
            /* background: rgba(0, 0, 0, 0) url("../images/inputbg.gif") repeat-x scroll 0 0;*/
            line-height: 25px;
            margin-left: 5px;
            text-indent: 5px;
            margin: 5px;
            width: 585px;
            height: 50px;
            border: 1px solid #d1d1d1;
        }
        .footerBtn
        {
            text-align: center;
            margin-top: 15px;
            padding-bottom: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-3" />
    <input id="hid_Alert" type="hidden" />
    <input type="hidden" id="hidCompId" runat="server" />
    
    <!--当前位置 end-->
    <div class="rightinfo">
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="../GoodsNew/DisPriceList.aspx" runat="server" id="btitle">代理商价格维护</a></li><li>></li>
                <li><a href="javascript:;" runat="server" id="A1">代理商调价明细</a></li>
            </ul>
        </div>
        <div class="div_content">
            <div class="div_title">
            </div>
            <table class="tb2">
                <tbody>
                    <tr class="trClass">
                        <td style="width: 20%;">
                            <span class="span">调价标题</span>
                        </td>
                        <td colspan="3">
                            <label runat="server" style="margin-left: 5px; margin-top: 0px; float: left" id="lblDisTitle">
                            </label>
                        </td>
                    </tr>
                    <tr class="trClass">
                        <td style="width: 20%;">
                            <span class="span">选择代理商</span>
                        </td>
                        <td colspan="3">
                            <label runat="server" style="margin-left: 5px; margin-top: 0px; float: left" id="lblDisID">
                            </label>
                        </td>
                    </tr>
                    <tr class="trClass">
                        <td style="background: #f6f6f6 none repeat scroll 0 0; width: 20%;">
                            <span class="span">备注</span>
                        </td>
                        <td>
                            <textarea id="txtRemark" maxlength="400" readonly="readonly" class="textarea" runat="server"></textarea>
                        </td>
                    </tr>
                </tbody>
            </table>
            <br />
            <!--功能按钮 start-->
            <!--功能按钮 end-->
            <!--信息列表 start-->
            <asp:Repeater ID="rptGoodsPrice" runat="server">
                <HeaderTemplate>
                    <table class='tablelist'>
                        <thead>
                            <tr>
                                <th class="t3">
                                    代理商名称
                                </th>
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
                                    本次调整价格(元)
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                           <div class="tcle"><%# Common.GetDis(Convert.ToInt32( Eval("disID").ToString()),"disname")%></div>
                        </td>
                        <td>
                            <div class="tcle"><%# GetGoodsName( Convert.ToInt32( Eval("GoodsinfoID").ToString()))%></div>
                        </td>
                        <td>
                            <div class="tcle"><%# GoodsAttr(Eval("GoodsInfoId").ToString())%></div>
                        </td>
                        <td>
                            <div class="tc"><%#  decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(GetPrice(Convert.ToInt32(Eval("GoodsInfoId").ToString()))))).ToString("#,##0.00")%></div>
                        </td>
                        <td>
                            <div class="tc"><%#  decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(Eval("TinkerPrice")).ToString())).ToString("#,##0.00")%></div>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </thead> </table></FooterTemplate>
            </asp:Repeater>
            <!--信息列表 end-->
            <div class="footerBtn">
                <input name="" type="button" class="cancel" value="返回" />
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $('.tablelist tbody tr:odd').addClass('odd');
    </script>
</body>
</html>

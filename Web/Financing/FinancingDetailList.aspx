<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FinancingDetailList.aspx.cs" Inherits="Financing_FinancingDetailList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>交易明细</title>
    <link href="../Distributor/css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("input[type='text']").blur();
                    location.href = $("#A1").attr("href");
                }
            })
        })
        $(document).ready(function () {
            $('.PublicList tbody tr:odd').addClass('odd');
        });
        function Goinfo(id) {
            window.location.href = '/Distributor/neworder/orderdetail.aspx?KeyID=' + id + '&type1=FinancingDetailList';
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" method="post">
    <Head:Head ID="Head1" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="FinancingDetailList" />
    
    <div class="rightCon">
    <div class="info"><span class="homeIcon"></span><a id="navigation1" href="#">基本资料</a>><a id="navigation2" href="#" class="cur">基本信息</a></div>
        <!--功能条件 start-->
        <div class="userFun">
            <div class="right">
                <ul class="term">
                    <li>
                        <label class="head">
                            交易流水号：</label>
                        <input id="txtFinancingNo" type="text" runat="server" />
                    </li>
                    
                    <li>
                        <label class="head">
                            类型：</label><select id="ddrType" name="" runat="server" class="xl">
                                <option value="-1">全部</option>
                                <option value="1">入金</option>
                                <option value="2">出金</option>
                                <option value="3">订单支付</option>
                                <option value="4">借款申请</option>
                            </select></li>
                    <li> <label class="head">每页</label><input type="text" onkeyup='KeyInt(this)' onblur="KeyInt(this);" id="txtPager" name="txtPager" runat="server"
                class="box3" /><label class="head">行</label></li>
                    
                 
                </ul>
                <a id="A1" href="" class="btnBl" onserverclick="A_Seek" runat="server"><i class="searchIcon"></i>搜索</a><a href="javascript:void(0)" style="display:none;"  class="btnBl liSenior"><i
                    class="resetIcon "></i>高级</a>
            </div>
        </div>
           <div class="hidden userFun" style=" text-align:right; padding-right:160px; padding-top:10px; display:none;">
             <div class="right">
                    <ul class="term">
                         
                    </ul>
                </div>
            </div>
        <!--功能条件 end-->
        <div class="blank10">
        </div>
        <!--订单管理 start-->
        <div class="orderNr">
            <table class="PublicList list" id="orderBg" border="0" cellspacing="0" cellpadding="0">
                    <thead>
                        <tr>
                            <th>
                                交易流水号
                            </th>
                            <th>
                                订单编号
                            </th>
                            <th>
                                发生额
                            </th>
                            <th>
                                制单日期
                            </th>
                            <th>
                                类型
                            </th>
                            <th>
                                状态
                            </th>
                            <%--<th>
                                制单人
                            </th>--%>
                            <th>
                                备注
                            </th>
                        </tr>
                    </thead>
                    <asp:Repeater ID="repfinan" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Convert.ToDateTime(Eval("ts")).ToString("yyyyMMdd") + getIDLen(Eval("ID").ToString())%>
                                </td>
                                <td>
                                    <%#GetOrderNo(Convert.ToInt32(Eval("OrderID").ToString()))%>
                                </td>
                                <td>
                                    <%#Convert.ToDecimal(Eval("AclAmt")).ToString("0.00") %>
                                </td>
                                <td>
                                    <%#Convert.ToDateTime(Eval("ts")).ToString("yyyy-MM-dd")%>
                                </td>
                                <td>
                                    <%#Common.GetFinacingType(Convert.ToInt32(Eval("Type").ToString())) %>
                                </td>
                                <td>
                                    <%#Eval("State").ToString() == "1" ? "成功" : Eval("State").ToString()=="3"?"处理中":"其他"%>
                                </td>
                                <%--<td>
                                    <%#new Hi.BLL.SYS_Users().GetModel(Convert.ToInt32(Eval("modifyuser"))).TrueName %>
                                </td>--%>
                                <td title="<%# Eval("vdef1")%>" style="cursor:pointer;">
                                    <%# GetStr(Eval("vdef1").ToString())%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
            </table>
            <div class="pagin">
                <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5" OnPageChanged="Pager_PageChanged">
            </webdiyer:AspNetPager>
            </div>
        </div>
        <!--订单管理 end-->
    </div>
    <div class="blank20">
    </div>
    </div>
    <Footer:Footer ID="Footer" runat="server" />
    </form>
</body>
</html>

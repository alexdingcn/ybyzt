<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentList.aspx.cs" Inherits="Distributor_Payment_PaymentList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>收款单列表</title>
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Company/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../../js/CommonJs.js"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <Head:Head ID="Head" runat="server" />
        <div class="w1200">
            <Left:Left ID="Left1" runat="server" ShowID="nav-3" />
            <div class="rightCon">
                <div class="info">
                    <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
                    <a id="navigation2" href="PaymentList.aspx" class="cur">收款单列表</a>
                </div>

                <!--功能条件 start-->
                <div class="userFun">
                    <div class="left">
                        <a href="/Distributor/Payment/PaymentAdd.aspx" class="btnOr" id="btnAdd" runat="server"><i class="addIcon"></i>新建收款单</a>
                    </div>
                    <div class="right">
                        <ul class="term">
                            <li>
                                <label class="head">单号:</label>
                                <input  type="text" id="PaymentNO" runat="server" class="box"/>
                            </li>
                            <li>
                                <label class="head">状态:</label><select name="State" runat="server" id="ddrState" style="width: 90px;" class="xl">
                                    <option value="">请选择</option>
                                    <option value="0">新增</option>
                                    <option value="1">审核</option>

                                </select>
                            </li>

                            <a id="A1" href="#" class="btnBl" onserverclick="btnSearch_Click" runat="server"><i class="searchIcon"></i>搜索</a>
                            <a href="javascript:void(0)" class="btnBl liSenior"><i class="resetIcon"></i>高级</a>
                        </ul>
                    </div>
                </div>
                <div class="hidden userFun" style="text-align: right; padding-right: 160px; padding-top: 10px; display: none;">
                    <div class="right">
                        <ul class="term">
                            <li>
                                <label class="head">日期:</label><input name="txtCreateDate" runat="server" onclick="var endDate = $dp.$('txtEndCreateDate'); WdatePicker({ onpicked: function () { endDate.focus(); }, maxDate: '#F{$dp.$D(\'txtEndCreateDate\')}' })"
                                    style="width: 100px;" id="txtCreateDate" readonly="readonly" type="text" class="Wdate"
                                    value="" />&nbsp;-&nbsp;
                                        <input name="txtEndCreateDate" runat="server" onclick="WdatePicker({ minDate: '#F{$dp.$D(\'txtCreateDate\')}' })"
                                            style="width: 100px;" id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate"
                                            value="" /></li>
                            <li>
                                <label class="head">每页</label><input type="text" onkeyup="KeyInt(this)" id="txtPager" name="txtPager" runat="server"
                                    class="box" style="width: 40px;" /><label class="head">条</label></li>
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
                                        <th>收款单号</th>
                                        <th>收款日期</th>
                                        <th>医院</th>
                                        <th>状态</th>
                                        <th>收款金额</th>
                                        <th>操作</th>
                                    </tr>
                                </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr id='tr_<%# Eval("Id") %>'>
                                    <td>
                                        <%# Eval("PaymentNO")%>
                                    </td>
                                    <td>
                                        <%# Eval("PaymentDate","{0:yyyy-MM-dd}")%>
                                    </td>
                                    <td>
                                        <%#Eval("hospital").ToString()%>    
                                    </td>
                                    <td>
                                        <%# Eval("IState").ToString().ToInt(0) == 0 ? "新增" :  "审核"%>
                                    </td>
                                    <td>
                                        <%# Convert.ToDecimal( Eval("PaymentAmount")).ToString("#0.00")%>    
                                    </td>
                                    <td>
                                        <a href="PaymentInfo.aspx?KeyID=<%# Eval("Id") %>">详情</a>
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

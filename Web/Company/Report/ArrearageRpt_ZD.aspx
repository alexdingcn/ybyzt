<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ArrearageRpt_ZD.aspx.cs" Inherits="Company_Report_ArrearageRpt_ZD" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager, Version=7.2.0.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>代理商账单应收</title>

    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
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
                window.location.href = 'ArrearageRpt_ZD.aspx';
            });
        });
        //转到详细页
        function GotReturnInfo(Id) {
            var sdate = $("#txtCreateDate").val();
            var edate = $("#txtECreateDate").val();
            var height = document.documentElement.clientHeight;
            var layerOffsetY = (height - 550) / 2; //计算宽度
            var index = layerCommon.openWindow('账单详情', 'Pay.aspx?type=2&KeyID=' + Id + '&Sdate=' + sdate + '&Edate=' + edate, '900px', '450px');
            $("#hid_Alert").val(index);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:top ID="top1" runat="server" ShowID="nav-5" />
        <asp:Button ID="btnSearch" Text="搜索" runat="server" onclick="btnSearch_Click" style=" display:none"  />

        <div class="rightinfo">
            <!--当前位置 start-->
            <div class="place">
	            <ul class="placeul">
                    <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                    <li><a href="../Report/ArrearageRpt_ZD.aspx">代理商账单应收</a></li>
	            </ul>
            </div>
            <!--当前位置 end--> 
            <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                </ul>
                <div class="right">
                    <ul class="toolbar right">
                        <li id="Search"><span><img src="../../Company/images/t04.png" /></span>搜索</li>
                        <uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />
                    </ul>
                    <ul class="toolbar3">
                        <li>
                            代理商名称:<input name="txtDisName" type="text" id="txtDisName" runat="server" class="textBox"/>
                        
                      </li>
                        <li>
                            账单日期:<input name="txtCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;" id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;
                                             -&nbsp;
                                     <input name="txtECreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;" id="txtECreateDate" readonly="readonly" type="text" class="Wdate" value="" />
                        </li>
                        <li>
                            每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server" class="textBox" style=" width:40px;" />&nbsp;条
                        </li>
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
                                    <th>代理商名称</th>
                                    <th clss="t2">总金额</th>
                                    <th clss="t2">账龄1年以内</th>
                                    <th clss="t2">账龄1-2年</th>
                                    <th clss="t2">账龄2年以上</th>
                                    <th clss="t2">预收款余额</th>
                                    <th clss="t2">轧差应收账款</th>
                                     
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id='tr_<%# Eval("DisID") %>' >
                            <td><div class="tcle"><a style=" text-decoration:underline;" href="javascript:void(0)" onclick='GotReturnInfo(<%# Eval("DisID") %>);'>
                            <%# Common.GetDisValue(Eval("DisID").ToString().ToInt(0), "DisName")%></a></div></td>
                            
                            <td><div class="tc"><%# Convert.ToDecimal(Eval("AuditAmount") == DBNull.Value ? 0 : Eval("AuditAmount")).ToString("N")%></div></td>
                            <td><div class="tc"><%# Convert.ToDecimal(Eval("year1")).ToString("N")%></div></td>
                            <td><div class="tc"><%# Convert.ToDecimal(Eval("year2")).ToString("N")%></div></td>
                            <td><div class="tc"><%# Convert.ToDecimal(Eval("year3")).ToString("N")%></div></td>
                            <td><div class="tc"><%#Convert.ToDecimal(new Hi.BLL.PAY_PrePayment().sums(Eval("DisID").ToString().ToInt(0), CompID)).ToString("N")%></td>
                            <td><div class="tc"><%# (decimal.Parse(Eval("AuditAmount").ToString() == "" ? "0" : Eval("AuditAmount").ToString()) - Convert.ToDecimal(new Hi.BLL.PAY_PrePayment().sums(Eval("DisID").ToString().ToInt(0), CompID))) > 0 ? (decimal.Parse(Eval("AuditAmount").ToString() == "" ? "0" : Eval("AuditAmount").ToString()) - Convert.ToDecimal(new Hi.BLL.PAY_PrePayment().sums(Eval("DisID").ToString().ToInt(0), CompID))).ToString("N") : "0.00"%></div></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <tr id="trTotal" runat="server" visible='<%#bool.Parse((Pager.PageCount == Pager.CurrentPageIndex&&rptOrder.Items.Count!=0).ToString())%>'>
                            <td><div class="tcle"><font color="red">总计</font></div></td>
                            <td>
                                <div class="tc"><asp:label ID="total" runat="server" Text="" style="color:Red;"><%=ta.ToString("N") %></asp:label></div>
                            </td>
                            <td colspan="4">&nbsp;</td>
                            <td>
                               <div class="tc"> <asp:label ID="NeTotal" runat="server" Text="" style="color:Red;"><%=tb.ToString("N") %></asp:label></div>
                            </td>
                        </tr>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            <!--信息列表 end-->

            <!--列表分页 start--> 
            <div class="pagin">
                <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                     NextPageText=">"  PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
             ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                       TextBeforePageIndexBox="<span class='jump'>跳转到:</span>"
                                    CustomInfoSectionWidth="20%"  CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;"  CustomInfoClass="message" ShowCustomInfoSection="Left"
                  CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                     CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                    OnPageChanged="Pager_PageChanged" >
                    </webdiyer:AspNetPager>
            </div>
            <!--列表分页 end--> 
        </div>
    </form>
</body>
</html>

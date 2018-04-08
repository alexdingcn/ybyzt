<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RepArrearageList.aspx.cs" Inherits="Admin_Systems_RepArrearageList" EnableEventValidation="false" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Src="~/Admin/UserControl/Org.ascx" TagPrefix="uc1" TagName="Org" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager, Version=7.2.0.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>应收账款报表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
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
            //订单生成 搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });
            ///重置
            $("#li_Reset").click(function () {
                window.location.href = 'RepArrearageList.aspx';
            });
        });
        //转到详细页
        function GotReturnInfo(Id) {
            var sdate = $("#txtCreateDate").val();
            var edate = $("#txtCreateDate1").val();
            var height = document.documentElement.clientHeight;
            var layerOffsetY = (height - 550) / 2; //计算宽度
            var index = showDialog('订单详情', 'RepPay.aspx?KeyID=' + Id + '&Sdate=' + sdate + '&Edate=' + edate, '900px', '450px', layerOffsetY);
            $("#hid_Alert").val(index);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!--当前位置 start-->
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">报表查询</a><i>></i>
            <a href="RepArrearageList.aspx">应收账款</a>
     </div>
        <!--当前位置 end--> 
        <input type="hidden" id="salemanid" runat="server" />
        <input type="hidden" id="hid" runat="server" />
        <input type="hidden" id="aspx" runat="server" value="RepArrearageList.aspx" />
        <uc1:Org runat="server" ID="txtDisArea" />
        <asp:Button ID="btnSearch" Text="搜索" runat="server" onclick="btnSearch_Click" style=" display:none"  />
            <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                </ul>
                <div class="right">
                    <ul class="toolbar right">
                        <li id="Search"><span><img src="../../Company/images/t04.png" /></span>搜索</li>
                        <li id="li_Reset"><span><img src="../../Company/images/t06.png" /></span>重置</li>
                        <uc1:ToExcel runat="server" ID="ToExcel" contect="TbRepArrearageList" Visible="true" />
                        <li class="liSenior"><span><img src="../../Company/images/t07.png" /></span>高级</li>
                    </ul>
                    <ul class="toolbar3">
                        <li>厂商名称:<input name="txtCompName" type="text" id="txtCompName" runat="server"
                            class="textBox txtCompName" />
                        </li>                       
                        <li>
                            代理商名称:<input name="txtDisName" type="text" id="txtDisName" runat="server" class="textBox"/>
                        </li>
                        <li>
                            起止日期:<input name="txtCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                                             id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;
                                             -&nbsp;
                                     <input name="txtCreateDate1" runat="server" onclick="WdatePicker()" style="width: 115px;"
                                             id="txtCreateDate1" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;&nbsp;
                        </li>
                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->
            <div class="hidden" style="display:none;">
               <ul style="">
                    <li>
                        每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server" class="textBox" style=" width:40px;" />&nbsp;条
                    </li>
                    <li>业务员:<asp:DropDownList runat="server" ID="SaleMan" CssClass="downBox" ></asp:DropDownList>&nbsp;&nbsp;</li>
                    <li>机构名称:<asp:DropDownList runat="server" ID="Org" CssClass="downBox" ></asp:DropDownList>&nbsp;&nbsp;</li>
               </ul>
             </div>
            <!--信息列表 start-->
                <asp:Repeater ID="rptOrder" runat="server" >
                    <HeaderTemplate>
                        <table class="tablelist" id="TbRepArrearageList">
                            <thead>
                                <tr>
                                    <th>厂商名称</th>
                                    <th>代理商名称</th>
                                    <th>总金额</th>
                                    <th>账龄1年以内</th>
                                    <th>账龄1-2年</th>
                                    <th>账龄2年以上</th>
                                    <th>企业钱包余额</th>
                                    <th>轧差应收账款</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id='tr_<%# Eval("DisID") %>' >
                            <td><%# Common.GetCompValue(Convert.ToInt32(Eval("CompID").ToString()), "CompName")%></td>
                            <td><a style=" text-decoration:underline;" href="javascript:void(0)" onclick='GotReturnInfo(<%# Eval("DisID") %>);'><%# Common.GetDisValue(Eval("DisID").ToString().ToInt(0), "DisName")%></a></td>
                            <td><%# Convert.ToDecimal(Eval("AuditAmount") == DBNull.Value ? 0 : Eval("AuditAmount")).ToString("N")%></td>
                            <td><%# Convert.ToDecimal(Eval("year1")).ToString("N")%></td>
                            <td><%# Convert.ToDecimal(Eval("year2")).ToString("N")%></td>
                            <td><%# Convert.ToDecimal(Eval("year3")).ToString("N")%></td>
                            <td><%#Convert.ToDecimal(new Hi.BLL.PAY_PrePayment().sums(Eval("DisID").ToString().ToInt(0), Convert.ToInt32(Eval("CompID").ToString()))).ToString("N")%></td>
                            <td><%# (decimal.Parse(Eval("AuditAmount").ToString() == "" ? "0" : Eval("AuditAmount").ToString()) - Convert.ToDecimal(new Hi.BLL.PAY_PrePayment().sums(Eval("DisID").ToString().ToInt(0), Convert.ToInt32(Eval("CompID").ToString())))) > 0 ? (decimal.Parse(Eval("AuditAmount").ToString() == "" ? "0" : Eval("AuditAmount").ToString()) - Convert.ToDecimal(new Hi.BLL.PAY_PrePayment().sums(Eval("DisID").ToString().ToInt(0), Convert.ToInt32(Eval("CompID").ToString())))).ToString("N") : "0.00"%></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            <tr id="trTotal" runat="server" visible='<%#bool.Parse((Pager.PageCount == Pager.CurrentPageIndex&&rptOrder.Items.Count!=0).ToString())%>'>
                                <td><font color="red">总计</font></td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:label ID="total" runat="server" Text="" style="color:Red;"><%=ta.ToString("N") %></asp:label>
                                </td>
                                <td colspan="4">&nbsp;</td>
                            <td>
                                <asp:label ID="NeTotal" runat="server" Text="" style="color:Red;"><%=tb.ToString("N") %></asp:label>
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
        
        <div><span style="padding: 20px 0px 0px 20px;color:red;">备注：销售统计订单为已审核、已发货、已到货状态的订单。</span></div>

    </div>
    </form>
</body>
</html>


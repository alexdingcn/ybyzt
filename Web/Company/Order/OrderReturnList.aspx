<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderReturnList.aspx.cs"
    Inherits="Company_Order_OrderReturnList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>退货审核</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/order.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            })
        });

        $(document).ready(function () {
            //订单生成 搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });

            ///重置
            $("#li_Reset").click(function () {
                window.location.href = 'OrderReturnList.aspx';
            });

            //导出
            $("#export").click(function () {

                var str = "";

                if ($.trim($("#ddrState").val()) != "-2")
                    str += " and o.ReturnState=" + $("#ddrState").val();
                if ($.trim($("#txtReceiptNo").val()) != "")
                    str += " and o.ReceiptNo like '%" + $("#txtReceiptNo").val() + "&'";
                if ($.trim($("#txtCreateDate").val()) != "")
                    str += " and o.CreateDate>='" + $("#txtCreateDate").val() + "'";
                if ($.trim($("#txtEndCreateDate").val()) != "") {
                    var dtime = $("#txtEndCreateDate").val()
                    // 转换日期格式
                    dtime = dtime.replace(/-/g, '/'); // "2010/08/01";
                    // 创建日期对象
                    var date = new Date(dtime);
                    // 加一天
                    date.setDate(date.getDate() + 1);
                    var dateTime = date.getFullYear() + "-" + (date.getMonth() + 1).toString() + "-" + (date.getDate()).toString();
                    str += "and o.CreateDate<'" + dateTime + "'";
                }
                if ($.trim($("#txtDisName").val()) != "")
                    str += "and dis.disName like '%" + $("#txtDisName").val() + "%'";

                window.location.href = '../../../ExportExcel.aspx?intype=2&searchValue=' + str + '&p=' + $("#txtPager").val() + '&c=<%=Pager.CurrentPageIndex %>';
            });
        });

        //转到详细页
        function GotReturnInfo(Id, page) {
            var type='<%=Request["type"]+"" %>';
            window.location.href = 'OrderReturnInfo.aspx?KeyID=' + Id + '&go=1&type='+type;
        }

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
    <uc1:top ID="top1" runat="server" ShowID="nav-1" />
    <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
    <input id="hid_Alert" type="hidden" />
    <input type="hidden" id="hidCompId" runat="server" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="#">退货审核</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <%--<li class="click" id="btnAdd"><span><img src="../images/t01.png" /></span>新增订单</li>--%>
                <%--<li class="click2"><span><img src="../images/t02.png" /></span>编辑</li>--%>
                <%--<li id="VolumeDel"><span><img src="../images/t03.png" /></span>批量删除</li>--%>
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li id="Search"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                    <%--<uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />--%>
                    <li id="export"><span><img src="../images/tp3.png" /></span>导出</li>
                    <li class="liSenior"><span>
                        <img src="../images/t07.png" /></span>高级</li>
                </ul>
                <ul class="toolbar3">
                    <li style=" display:none;">退单编号:<input name="txtReceiptNo" maxlength="50" type="text" id="txtReceiptNo" runat="server"
                        class="textBox" />
                    </li>
                    <li>代理商名称:<input id="txtDisName" maxlength="50" runat="server" type="text" class="textBox" />
                    </li>
                    <li>退货状态:
                        <select name="OState" runat="server" id="ddrState" class="downBox">
                            <option value="-2">全部</option>
                            <option value="-1" id='Ostate1' runat="server">已拒绝</option>
                            <option value="1" id='Ostate2' runat="server">待审核</option>
                            <option value="2">已退货</option>
                            <option value="4">已退货款</option>
                        </select>
                    </li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <div class="hidden">
            <ul style="width: 90%;">
                <li>每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                    class="textBox" style="width: 40px;" />行 </li>
                <li>退货时间:
                     <input name="txtCreateDate" runat="server" onclick="var endDate=$dp.$('txtEndCreateDate'); WdatePicker({onpicked:function(){endDate.focus();},maxDate:'#F{$dp.$D(\'txtEndCreateDate\')}'})" style="width: 115px;"
                            id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;-&nbsp;
                    <input name="txtEndCreateDate" runat="server" onclick="WdatePicker({minDate:'#F{$dp.$D(\'txtCreateDate\')}'})" style="width: 115px;"
                            id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;&nbsp;
                </li>
                <li>退货审核时间:
                     <input name="txtAuditDate" runat="server" onclick="var endDate=$dp.$('txtEndAuditDate'); WdatePicker({onpicked:function(){endDate.focus();},maxDate:'#F{$dp.$D(\'txtEndAuditDate\')}'})" style="width: 115px;"
                            id="txtAuditDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;-&nbsp;
                    <input name="txtEndAuditDate" runat="server" onclick="WdatePicker({minDate:'#F{$dp.$D(\'txtAuditDate\')}'})" style="width: 115px;"
                            id="txtEndAuditDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;&nbsp;
                </li>
            </ul>
        </div>
        <!--信息列表 start-->
        <asp:Repeater ID="rptOrder" runat="server">
            <HeaderTemplate>
                <table class="tablelist" id="TbList">
                    <thead>
                        <tr>
                            <%--<th><input type="checkbox" id="CB_SelAll" onclick="SelectAll(this)"/></th>--%>
                            <th class="t3">
                                退单编号
                            </th>
                            <th class="t3">
                                代理商名称
                            </th>
                            <th class="t1">
                                退货状态
                            </th>
                            <th class="t2">
                                退货时间
                            </th>
                            <th class="t2">
                                审核时间
                            </th>
                            <th class="t6">
                                退货说明
                            </th>
                            <th class="t1">
                                申请人
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr id='tr_<%# Eval("Id") %>'>
                    <%--<td>
                        <asp:CheckBox ID="CB_SelItem" runat="server"></asp:CheckBox>
                        <asp:HiddenField ID="Hd_Id" runat="server" Value='<%# Eval("Id") %>' />
                    </td>--%>
                    <td>
                       <div class="tcle"><a href="javascript:void(0)" onclick='GotReturnInfo("<%# Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) %>")'>
                            <%# Eval("ReceiptNo") %>
                        </a></div>
                    </td>
                    <td>
                        <div class="tcle"><%# Common.GetDis(Convert.ToInt32(Eval("DisID").ToString()), "DisName")%></div>
                    </td>
                    <td>
                       <div class="tc"><%# OrderInfoType.ReturnState(int.Parse(Eval("ReturnState").ToString())) %></div>
                    </td>
                    <td>
                        <div class="tc"><%# Eval("ReturnDate","{0:yyyy-MM-dd HH:mm}") %></div>
                    </td>
                    <td>
                        <div class="tc"><%# Eval("AuditDate").ToString().ToDateTime()!=DateTime.MinValue? Eval("AuditDate", "{0:yyyy-MM-dd HH:mm}"):""%></div>
                    </td>
                    <td title='<%# Eval("ReturnContent").ToString() %>'>
                       <div class="tcle"><%# Common.MySubstring(Eval("ReturnContent").ToString(),20,"...")%></div>
                    </td>
                    <td>
                        <div class="tc"><%# Common.GetUserName(Convert.ToInt32(Eval("CreateUserID").ToString()))%></div>
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
                CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5" OnPageChanged="Pager_PageChanged">
            </webdiyer:AspNetPager>
        </div>
        <!--列表分页 end-->
    </div>
    </form>
</body>
</html>

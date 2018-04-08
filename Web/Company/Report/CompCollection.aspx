<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompCollection.aspx.cs" Inherits="Company_Report_CompCollection" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager, Version=7.2.0.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>订单收款明细</title>
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

            //删除按钮单条选中
            $("[name = RmCheck]:checkbox").click(function () {
                var cla = $(this).attr("class")
                if (cla == "RmCheck cur") {
                    //取消选中
                    $(this).removeClass("cur");
                }
                else {
                    //选中
                    $(this).addClass("cur")
                }
                var chk = $(".chk .cur");
                if ((chk.length * 1) > 0) {
                    $("#FCheck").prop("checked", true);
                    $("#FCheck").addClass("cur")
                }
                else {
                    $("#FCheck").prop("checked", false);
                    $("#FCheck").removeClass("cur")
                }
            })
            //确认删除
            $("#DelAll").click(function () {
                var chk = $(".chk .cur");
                if ((chk.length * 1) > 0) {
                    layerCommon.confirm("该操作不可恢复！是否确定", function () {

                        var IDlist = "";
                        $.each(chk, function (index, item) {
                            IDlist += $(item).val() + ",";
                        })
                        $.ajax({
                            type: "post",
                            url: "../../Handler/orderHandle.ashx",
                            data: { ActionType: "DelAll2", IDlist: IDlist },
                            dataType: "json",
                            success: function (data) {
                                if (data.rel == "OK") {
                                    layerCommon.msg("" + data.Msg + "", IconOption.笑脸);
                                    window.location.href = 'CompCollection.aspx';
                                }
                                else {
                                    layerCommon.msg("" + data.Msg + "", IconOption.哭脸);
                                }
                            }
                        })
                    }, "批量删除")
                }
                else {
                    layerCommon.msg("请最少选择一条订单！", IconOption.错误);
                }
            })

        });
       
        $(document).ready(function () {
            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');
            //搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });

            ///重置
            $("#li_Reset").click(function () {
                window.location.href = 'CompCollection.aspx';
            });
        });

        function goAuditInfo(Id, page) {
            window.location.href = '../Order/OrderAuditInfo.aspx?KeyID=' + Id + "&page=" + page;
        }
        //转到详细页
        function GotReturnInfo(paymentId,pretype) {
            var height = document.documentElement.clientHeight;
            var layerOffsetY = (height - 550) / 2; //计算宽度
            var index = layerCommon.openWindow('收款详情', 'CompOrderInfo.aspx?KeyID=' + paymentId+'&pretype='+pretype, '900px', '450px');
            $("#hid_Alert").val(index);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:top ID="top1" runat="server" ShowID="nav-2" />
       
        <input type="hidden" id="hid_Alert" />
        <asp:Button ID="btnSearch" Text="搜索" runat="server" onclick="btnSearch_Click" style=" display:none"  />

        <div class="rightinfo">
             <!--当前位置 start-->
            <div class="place">
	            <ul class="placeul">
                    <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                    <li><a href="../Report/CompCollection.aspx">订单收款明细</a></li>
	            </ul>
            </div>
            <!--当前位置 end--> 
            <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                    <li id="DelAll"  runat="server"><span><img src="../images/t03.png" /></span>批量删除</li>
                </ul>
                <div class="right">
                    <ul class="toolbar right">
                        
                        <li id="Search"><span><img src="../images/t04.png" /></span>搜索</li>
                        <uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />
                        <li class="liSenior"><span><img src="../images/t07.png" /></span>高级</li>
                    </ul>
                    <ul class="toolbar3">
                         
                         <li>
                            订单编号：<input id="orderid" runat="server" type="text" class="textBox" />
                        </li>
                        <li>
                            代理商名称:<input id="txtDisName" runat="server" type="text" class="textBox" />
                        </li>
                        
                        <li>
                            收款日期:
                            <input name="txtCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;" id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;
                            -&nbsp;
                            <input name="txtECreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;" id="txtECreateDate" readonly="readonly" type="text" class="Wdate" value="" />
                        </li>
                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->
            <div class="hidden">
            <ul style="width: 90%;">
                <li>
                    每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server" class="textBox" style=" width:40px;" />&nbsp;条
                </li>
                <li>
                    收款来源:
                    <select name="ddrPayType" runat="server" id="ddrPayType" class="downBox">
                       <option value="-1">全部</option>
                                <option value="1">快捷支付</option>
                                <option value="2">银联支付</option>
                                <option value="3">网银支付</option>
                                <option value="4">B2B网银支付</option>
                                <option value="5">线下支付</option>
                                <option value="6">支付宝支付</option>
                                <option value="7">微信支付</option>
                                <option value="8">企业钱包支付</option>
                    </select>&nbsp;&nbsp;
                </li>
            </ul>
        </div>
            <!--信息列表 start-->
                <asp:Repeater ID="rptOrder" runat="server" >
                    <HeaderTemplate>
                        <table class="tablelist" id="TbList">
                            <thead>
                                <tr>
                                    <th><input type="checkbox" class="" id="FCheck" /></th>
                                    <th class="t3">订单编号</th>
                                    <th class="t2">支付日期</th>
                                    <th class="t3">代理商名称</th>
                                    <%--<th>金融机构</th>--%>
                                    <th class="t2">收款来源</th>
                                    <th class="t2">支付状态</th>
                                    <th class="t2">确认状态</th>
                                    <%--<th>渠道</th>--%>
                                    <th class="t5">收款金额</th>
                                    <%--<th>对账状态</th>--%>
                                    <%--<th>备注</th>--%>
                                </tr>  
                                
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id='tr_<%# Eval("ID") %>' > 
                            <td class="tcle chk"><input type="checkbox" name="RmCheck" class="RmCheck" value="<%#Eval("orderID").ToString()+"|"+Eval("paymentID")%>"/></td>                           
                            <td>
                               <div class="tc"> <a style=" text-decoration:underline;" href="javascript:void(0)" onclick='GotReturnInfo("<%#  Common.DesEncrypt(Eval("paymentID").ToString(),Common.EncryptKey)%>","<%#  Common.DesEncrypt(Eval("PreType").ToString(),Common.EncryptKey) %>")'>
                                <%# Common.GetOrderValue(Convert.ToInt32(Eval("orderID").ToString()), "ReceiptNo")%>&nbsp;</a></div>
                            </td>
                            <td><div class="tc"><%# Convert.ToDateTime(Eval("Date")).ToString("yyyy-MM-dd")%></div></td>
                            <td><div class="tcle"><%# Eval("DisName").ToString()%></div></td>
                            <%--<td><%# GetBankName(Eval("BankID").ToString())%></td>--%>
                            <td><div class="tc"><%# Eval("Source").ToString()%></div></td>
                             <td><div class="tc"><%# Convert.ToString(Eval("status"))=="1"?"成功":"失败"%></div></td>
                            <td><div class="tc"><%#Common.GetVdef9(Convert.ToString(Eval("vedf9")),Convert.ToString( Eval("Source")))%></div></td>
                            <td><div class="tc"><%# Convert.ToDecimal(Convert.ToDecimal(Eval("Price1")) - (Convert.ToDecimal(Eval("sxf1") == DBNull.Value ? "0" : Eval("sxf1")))).ToString("N")%></div></td>
                            <%--<td title="<%#Eval("Remark").ToString()%>" style="cursor:pointer;"><%# GetStr(Eval("Remark").ToString())%></td>--%>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                       
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

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="orderZDList.aspx.cs" Inherits="Distributor_Pay_orderZDList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>账单支付</title>
    <%--<link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />--%>
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("input[type='text']").blur();
                    $("#btnSearch").trigger("click");
                }
            })
        })
        $(document).ready(function () {
            $('.PublicList tbody tr:odd').addClass('odd');

            //订单生成 搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });

            //重置
            $("#li_Reset").click(function () {
                $("#txtReceiptNo").val("");
                $("#ddrPayState").val("-1");
                $("#txtArriveDate").val("");
                $("#txtArriveDate1").val("");
            });
        });

        function showPayDetail(orderid) {
            var height = document.body.clientHeight; //计算高度
            var layerOffsetY = (height - 450) / 2; //计算宽度
            var index = layerCommon.openWindow('账单支付明细', 'PayDetail.aspx?KeyID=' + orderid, '750px', '380px'); //记录弹出对象
            $("#hid_Alert").val(index); //记录弹出对象
        }

        function CloseDialog() {
            var showedDialog = $("#hid_Alert").val(); //获取弹出对象
            layerCommon.layerClose(showedDialog); //关闭弹出对象
        }


        function pay(isDBPay,Id) {
            //window.location.href = 'Pay.aspx?KeyID=' + Id;
            window.open('Pay.aspx?isDBPay='+isDBPay+'&KeyID=' + Id, "Pay");
        }       
        function info(Id) {
            window.location.href = '../OrderZDInfo.aspx?KeyID=' + Id + '&type1=orderZDList';
        }
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
        <Head:Head ID="Head2" runat="server" />
        <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="orderZDList" />
        <div class="rightCon">
        <div class="info"> 
            <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
            <a id="navigation2" href="#" class="cur">我的账单</a>
        </div>
	        <!--功能条件 start-->
	        <div class="userFun">
                <div class="left" style="display:none;">
                    <i style="font-size: 15px;">企业钱包余额:</i><i style="font-size: 15px; color: Red; font-weight:bold;">￥<%=price%>&nbsp;</i>
                    <a href="remittanceAdd.aspx" class="btnPay">充值</a>
                </div>
                <div class="right">
        	        <ul class="term">
            	        <li><label class="head">账单编号：</label><input name="txtReceiptNo" type="text" id="txtReceiptNo" runat="server" class="box" style="width:110px;"/></li>
           		       
                        <li>
                            <label class="head">账单日期：</label>
                            <input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtArriveDate"
                                readonly="readonly" type="text" class="Wdate box" style=" width:100px;" value="" />
                            <i class="txt">—</i>
                            <input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtArriveDate1"
                                readonly="readonly" type="text" class="Wdate box" style=" width:100px;" value="" />
                        </li>
            	        <li><label class="head">每页</label><input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server" class="box" style=" width:30px;" /><label class="head">条</label></li>
                    </ul>
        	        <a href="javascript:void(0)" id="Search" class="btnBl"><i class="searchIcon"></i>搜索</a>
                    <!--<a href="javascript:void(0)" id="li_Reset" class="btnBl"><i class="resetIcon"></i>重置</a>-->
                    <a href="javascript:void(0)"  class="btnBl liSenior" style="display:none;"><i class="resetIcon "></i>高级</a>
                </div>
            </div>
            <div class="hidden userFun" style=" text-align:right; padding-right:160px; padding-top:10px; display:none;">
                <div class="right">
                    <ul class="term">
                        <li>
                            <label class="head">订单来源：</label> 
                            <asp:DropDownList runat="server" ID="ddlAddtype"  CssClass="xl" style=" width:100px;">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label class="head">订单类型：</label>
                            <asp:DropDownList runat="server" ID="ddlOtype"  CssClass="xl" style=" width:100px;">
                            </asp:DropDownList>
                        </li>
                    </ul>
                </div>
            </div>
            <!--功能条件 end-->
            <asp:Button ID="btnSearch" Text="搜索" runat="server" onclick="btnSearch_Click" style=" display:none"  />
            <div class="blank10"></div>
            <!--订单管理 start-->
            <div class="orderNr">
                <!--信息列表 start-->
                <asp:Repeater runat="server" ID="rptOrder" OnItemCommand="rptOrder_ItemCommand">
                    <HeaderTemplate>
                        <table class="PublicList list">
                            <thead>
                                <tr>
                                   <%-- <th><input type="checkbox" id="CB_SelAll" onclick="SelectAll(this)" style="width:30px;"/></th>
                                   --%>
                                    <th>账单编号</th>
                                    <th>账单日期</th>
                                    <th>账单金额</th>
                                    <th>已支付金额</th>
                                    <th>费用名称</th>
                                    <%--<th>账单来源</th>
                                    <th>账单状态</th>--%>
                                    <th>支付状态</th>
                                  <th width="110" style="text-align:center;">操作</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id='tr_<%# Eval("Id") %>' >
                            <%-- <td>
                           <asp:CheckBox ID="CB_SelItem" runat="server"></asp:CheckBox>
                                <asp:HiddenField ID="Hd_Id" runat="server" Value='<%# Eval("Id") %>' />
                            </td>--%>
                            <td><a href="javascript:void(0)" onclick='info("<%# Common.DesEncrypt(Eval("Id").ToString(),Common.EncryptKey) %>")'><%# Eval("ReceiptNo")%></a></td>
                            <td><%# Convert.ToDateTime(Eval("CreateDate")).ToString("yyyy-MM-dd") %></td>
                            <td><%# (Convert.ToDecimal(Eval("AuditAmount")) + Convert.ToDecimal(Eval("OtherAmount"))).ToString("0.00")%></td>
                            <td><%# Convert.ToDecimal(Eval("PayedAmount")).ToString("0.00")%></td>
                            <td><%# Eval("vdef2").ToString()%></td>
                            
                            <%--<td><%# OrderInfoType.PayState(int.Parse(Eval("PayState").ToString()))%></td>--%>
                             <td style="width:100px;">
                                <%# OrderInfoType.PayState((int)Eval("paystate")) == "已支付" ? "<a title='查看支付清单' class='ger' href='javascript:void(0)' onclick='showPayDetail(\"" + Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) + "\")'>已支付</a>" : OrderInfoType.PayState((int)Eval("paystate")) == "部分支付" ? "<a title='查看支付清单' class='ger' href='javascript:void(0)' onclick='showPayDetail(\"" + Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) + "\")'>部分支付</a>" : OrderInfoType.PayState((int)Eval("paystate"))%>&nbsp;
                            </td>
                            <td>
                             	<span class="linkBtn">
                                	 <%# Getmessage(Eval("OState"), Convert.ToString(Eval("PayState")), Eval("ReturnState"), Convert.ToInt32(Eval("ID")))%>
                              		<a class='a-gray'  href='javascript:void(0);' onclick='info("<%# Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) %>")'>账单详情</a>
                                </span>
                      <style>                     	
						
                      </style>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <!--信息列表 end-->
                
                <!--分页 start-->
                 <%-- 
                 <div class="page"><ul class="list">
        	        <li><a href="">&lt;</a></li><li class="cur"><a href="">1</a></li><li><a href="">2</a></li><li><a href="">3</a></li>
                    <li><a href="">4</a></li><li><a>...</a></li><li><a href="">10</a></li><li><a href="">&gt;</a></li>
                </ul></div>
                --%>
		        <!--分页 end-->
            </div>
            <!--列表分页 start--> 
                <div class="pagin">
                    <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                        NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                        ShowPageIndexBox="Always" TextAfterPageIndexBox="<span style='margin-left:5px;'>页</span>"
                        TextBeforePageIndexBox="<span>跳转到: </span>" ShowCustomInfoSection="Left" CustomInfoClass="message"
                        CustomInfoStyle="padding-left:20px;" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                        CustomInfoSectionWidth="35%" CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                        Width="100%" OnPageChanged="Pager_PageChanged">
                    </webdiyer:AspNetPager>
                </div>
                <!--列表分页 end--> 
             <!--订单管理 end-->   
        </div>


        </div>
      
    </form>
</body>
</html>
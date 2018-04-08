
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReceivingList.aspx.cs" Inherits="Distributor_ReceivingList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>收货查询</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../js/CommonJs.js"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("input[type='text']").blur();
                    location.href = $("#A1").attr("href");
                }
            })
        })
        $(document).ready(function () {
            $('#orderBg tr:even').addClass("bg");
        });

        function btnSing(orderid) {
           layerCommon.confirm("确认签收订单吗？", function () {
                $.ajax({
                    url: "ReceivingList.aspx?type=sing",
                    data: { orderid: orderid },
                    dataType: 'json',
                    success: function (img) {
                        if (img.str) {
                            location.href = "ReceivingList.aspx";
                        }
                        else {
                            layerCommon.msg("-到货状态异常",IconOption.哭脸);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layerCommon.msg("服务器异常，请稍后再试",IconOption.哭脸);
                    }
                });
            });
        }
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head1" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="nav-4" />
    <div class="rightCon">
    <div class="info"> <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="ReceivingList.aspx" class="cur">收货查询</a>
        </div>
	<!--功能条件 start-->
	<div class="userFun">
        <div class="right">
        	<ul class="term">
            	<li><label class="head">订单编号：</label><input id="txtorderReceiptNo" type="text" runat="server" class="box" style="width:110px;" /></li>
                <%--<li><label class="head">签收人：</label><input id="txtname" type="text" runat="server" class="box" /></li>--%>
           		<%--<li><label class="head">签收状态：</label>--%>
                <%--<select id="IsSign" name="" runat="server" class="xl">
                <option value="-1">全部</option>
                <option value="0">未签收</option>
                <option value="1">已签收</option>
                </select>--%>
                <%--</li>--%>
                <li><label class="head">
                            下单日期：</label> <input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtOderStartDate"
                                readonly="readonly" type="text" class="Wdate box" style=" width:100px;" /><i class="txt">—</i> 
                                 <input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtOderEndDate" 
                                readonly="readonly" type="text" class="Wdate box" style=" width:100px;margin-left:0px;"  />
                </li>
                <li> <label class="head">每页</label><input type="text" onkeyup='KeyInt(this)' onblur='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                        class="box3" /><label class="head">行</label></li>
            </ul>
        	<a href="" id="A1" onserverclick="A_Seek" runat="server" class="btnBl"><i class="searchIcon"></i>搜索</a>
            <a href="javascript:void(0);" onclick="javascript:location.href='ReceivingList.aspx'" class="btnBl"><i class="resetIcon"></i>重置</a>
        </div>
    </div>
    <!--功能条件 end-->
    
    <div class="blank10"></div>
    <!--订单管理 start-->
    <div class="orderNr">
    	<table class="PublicList list" id="orderBg" border="0" cellspacing="0" cellpadding="0">
    	<asp:Repeater ID="rptving" runat="server">
        <HeaderTemplate>
        <thead><tr> <th style=" width:20%">订单编号</th><th>下单日期</th><th>订单金额</th><th>发货日期</th><th>发货人</th><th style="width:130px;">操作</th></tr></thead>
        </HeaderTemplate>
        <ItemTemplate>
         <tbody>
        	<tr><td><a href='<%#"/Distributor/newOrder/orderdetail.aspx?top=2&KeyID="+Common.DesEncrypt(Eval("orderid").ToString(), Common.EncryptKey) +"&type1=ReceivingList" %>'><%# GetOrderID((int)Eval("OrderID"))%>&nbsp;</a></td>
            <td><%# GetOrderdatetime((int)Eval("OrderID"))%></td>
            <td><%# GetOrderdatemy((int)Eval("OrderID")).ToString("N") %></td>
            <td><%# Eval("SignDate").ToString().ToDateTime() == DateTime.MinValue ? "" : Eval("SignDate", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
            <td><%# Eval("ActionUser") %></td>
            <%--<td><%#Eval("ExpressNo") %>&nbsp;</td>
            <td><%# Common.GetDateTime((DateTime)Eval("SignDate"),"yyyy-MM-dd")%>&nbsp;</td><td><%#Eval("IsSign").ToString()=="0"?"未签收":"已签收" %>&nbsp;</td><td><%#Eval("SignUser") %>&nbsp;</td>--%>
            <td><span class="linkBtn">
				<%--<%#Eval("IsSign").ToString() == "0" ? "<a onclick=\"btnSing('" + Common.DesEncrypt(Eval("orderid").ToString(), Common.EncryptKey) + "')\" id=\"SingIcon\" href=\"javascript:void(0)\">立即签收</a>" : "已签收"%>--%>
                <a href='<%#"/Distributor/newOrder/orderdetail.aspx?top=2&KeyID="+Common.DesEncrypt(Eval("orderid").ToString(), Common.EncryptKey) +"&type1=ReceivingList" %>'>详情</a>
                </span>
            </td></tr>
         </tbody>
         </ItemTemplate>
         </asp:Repeater>
         </table>
        
    </div>
     <!--分页 start-->
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
            <!--分页 end-->
     <!--订单管理 end-->   
</div>


</div>
    </form>
</body>
</html>

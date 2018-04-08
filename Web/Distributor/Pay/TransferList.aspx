<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransferList.aspx.cs" Inherits="Distributor_Pay_TransferList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>转账汇款查询</title>
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
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
                window.location.href = 'TransferList.aspx';
            });
        });
        function pay(Id) {
            window.location.href = 'Pay.aspx?KeyID=' + Id;
        }
        function info(Id) {
            window.location.href = '../neworder/orderdetail.aspx?KeyID=' + Id;
        }
        //企业钱包详细页面
        function goInfo(Id) {
            window.location.href = 'PrePayInfo.aspx?KeyID=' + Id + '&type1=TransferList';
        }
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="TransferList" />
    <div class="rightCon">
    <div class="info"><span class="homeIcon"></span><a id="navigation1" href="#">基本资料</a>><a id="navigation2" href="#" class="cur">基本信息</a></div>
        <!--功能条件 start-->
        <div class="userFun">
            <div class="left">
            
            </div>
            <div class="right">
                <ul class="term">
                    <li>
                        <label class="head">
                            流水帐号：</label>
                        <input name="txtReceiptNo" type="text" id="txtReceiptNo" runat="server" style="width:110px;"
                            class="box" maxlength="40" />
                    </li>
                    <li>
                        <label class="head">
                            汇款日期：</label>
                        <input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtArriveDate"
                                readonly="readonly" type="text" class="Wdate box" style=" width:100px;" value="" />
                        <i class="txt">—</i>
                        <input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtArriveDate1"
                                readonly="readonly" type="text" class="Wdate box" style=" width:100px;" value="" />
                    </li>
                    <!--
                    <li style="display:none">
                        <label class="head">
                            支付状态：</label>
                        <select name="ddrPayState" runat="server" id="ddrPayState" class="xl">
                            <option value="-1">全部</option>
                            <option value="1">成功</option>
                            <option value="2">失败</option>
                            <option value="3">处理中</option>
                            <option value="2">结算</option>
                        </select>
                    </li>
                    <li>
                        <label class="head">
                            款项类型：</label>
                        <select name="ddrPayType" runat="server" id="ddrPayType" class="xl">
                            <option value="-1">全部</option>
                            <option value="1">转账汇款</option>
                            <option value="2">手工录入</option>
                            <option value="3">冲正</option>
                            <option value="4">退款</option>                            
                            <option value="5">订单付款</option>
                        </select>
                    </li>
                    <li>
                        <label class="head">
                            审核状态：</label>
                        <select name="ddrAuditState" runat="server" id="ddrAuditState" class="xl">
                            <option value="-1">全部</option>
                            <option value="0">未审</option>
                            <option value="2">已审</option>
                        </select>
                    </li>
                    -->
                    <li>
                        <label class="head">
                            每页</label><input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager"
                                runat="server" class="box" style="width: 30px;" /><label class="head">条</label></li>
                </ul>
                <a href="javascript:void(0)" id="Search" class="btnBl"><i class="searchIcon"></i>搜索</a>
                <a href="javascript:void(0)" id="li_Reset" class="btnBl"><i class="resetIcon"></i>重置</a>
            </div>
        </div>
        <!--功能条件 end-->
        <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
        <div class="blank10">
        </div>
        <!--订单管理 start-->
        <div class="orderNr">
            <!--信息列表 start-->
            <asp:Repeater runat="server" ID="rptOrder" OnItemCommand="rptOrder_ItemCommand">
                <HeaderTemplate>
                    <table class="PublicList list">
                        <thead>
                            <tr>                               
                                <th>
                                    流水帐号
                                </th>
                                <th>
                                    金额（元）
                                </th>
                                
                                <th>
                                    汇款日期
                                </th>
                                <th>
                                    汇款人
                                </th>
                                <%--<th>
                                    审核状态
                                </th>--%>
                                <%--<th>
                                    支付状态
                                </th>
                                <th>
                                    款项类型
                                </th>--%>
                                
                               
                                <th>
                                    备注
                                </th>
                               <%-- <th style="text-align: center; width: 110px;">
                                    操作
                                </th>--%>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id='tr_<%# Eval("Id") %>'>                       
                        <td><a href="javascript:void(0)"  onclick='goInfo("<%# Common.DesEncrypt(Eval("ID").ToString(),Common.EncryptKey) %>")'><%#Eval("ID") %></a>
                        </td>
                        <td>
                            <%# Convert.ToDecimal(Eval("price")).ToString("N")%>
                        </td>
                        <td>
                            <%# Convert.ToDateTime(Eval("Paytime")).ToString("yyyy-MM-dd")%>
                        </td>
                        <td>
                            <%# new Hi.BLL.SYS_Users().GetModel(Convert.ToInt32(Eval("CrateUser"))).TrueName%>
                        </td>
                        <%--<td>
                            <%# Common.GetNameBYPreStart(Convert.ToInt32(Eval("AuditState")))%>
                        </td>--%>
                        <%--<td>
                            <%# Common.GetNameBYPrePayMentStart(Convert.ToInt32(Eval("Start")))%>
                        </td>
                         <td>
                            <%# Common.GetPrePayStartName(Convert.ToInt32(Eval("PreType")))%>
                        </td>--%>
                        <td title="<%# Eval("vdef1")%>" style="cursor:pointer;">
                            <%# GetStr(Eval("vdef1").ToString())%>
                        </td>
                       <%-- <td style="width: 110px" align="center">
                            <a href="javascript:void(0)" onclick='goInfo(<%# Eval("ID") %>)' class="tablelinkQx"
                                id="clickMx">查看</a>
                        </td>--%>
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
                    ShowPageIndexBox="Always" TextAfterPageIndexBox="<span style='margin-left:5px;'>页</span>"
                    TextBeforePageIndexBox="<span>跳转到: </span>" ShowCustomInfoSection="Left" CustomInfoClass="message"
                    CustomInfoStyle="padding-left:20px;" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                    CustomInfoSectionWidth="35%" CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                    Width="100%" OnPageChanged="Pager_PageChanged">
                </webdiyer:AspNetPager>
            </div>
            <!--列表分页 end-->
            <!--分页 start-->
            <%-- 
                 <div class="page"><ul class="list">
        	        <li><a href="">&lt;</a></li><li class="cur"><a href="">1</a></li><li><a href="">2</a></li><li><a href="">3</a></li>
                    <li><a href="">4</a></li><li><a>...</a></li><li><a href="">10</a></li><li><a href="">&gt;</a></li>
                </ul></div>
            --%>
            <!--分页 end-->
        </div>
        <!--订单管理 end-->
    </div>
    </div>
    </form>
</body>
</html>

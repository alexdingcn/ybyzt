<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrePayList.aspx.cs" Inherits="Distributor_Pay_PrePayList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>我的钱包</title>
    <%--<link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />--%>
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
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
                $("#txtReceiptNo").val("");
                $("#ddrPayState").val("-1");
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
            window.location.href = 'PrePayInfo.aspx?KeyID=' + Id;
        }

        //修改支付密码
        function asave() {
            var str = "";
            if ($("#paypwd").length != 0) {
                if ($.trim($("#paypwd").val()) == "") {
                    $("#spanpaypwd").text("-原支付密码不能为空").css("display", "inline-block");
                    return false;
                }
            }
            if ($.trim($("#paypwd1").val()) == "") {
                $("#spanpaypwd").text("-新支付密码不能为空").css("display", "inline-block");
                return false;
            }
            if ($.trim($("#paypwd2").val()) == "") {
                $("#spanpaypwd").text("-确认支付密码不能为空").css("display", "inline-block");
                return false;
            }
            if ($.trim($("#paypwd2").val()) != $.trim($("#paypwd1").val())) {
                $("#spanpaypwd").text("-两次输入的密码不一致").css("display", "inline-block");
                return false;
            }
            if (!IsPayPwd($("#paypwd1").val())) {
                $("#spanpaypwd").text("-新密码必须为字母加数字的组合").css("display", "inline-block");
                return false;
            }
            if (!IsPayPwd($("#paypwd2").val())) {
                $("#spanpaypwd").text("-确认密码必须为字母加数字的组合").css("display", "inline-block");
                return false;
            }
            if ($("#paypwd1").val().length < 6 || $("#paypwd1").val().length > 16) {
                $("#spanpaypwd").text("-新密码长度必须大于6位，小于16位").css("display", "inline-block");
                return false;
            }
            return true;
        }

        $(function () {
            $("#divpaypwd").css({
                'top': ($(window).height() - $("#divpaypwd").height()) / 2,
                'left': ($(window).width() - $("#divpaypwd").width()) / 2
            });
            $(window).scroll(function () {
                $("#divpaypwd").css({
                    'top': ($(window).height() - $("#divpaypwd").height()) / 2 + $(window).scrollTop(),
                    'left': (($(window).width() - $("#divpaypwd").width()) / 2) + $(window).scrollLeft()
                });
            });
            $(window).resize(function () {
                $("#divpaypwd").css({
                    'top': ($(window).height() - $("#divpaypwd").height()) / 2,
                    'left': ($(window).width() - $("#divpaypwd").width()) / 2
                });
            });
        });
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="PrePayList" />
        <div class="rightCon">
            <div class="info">
                <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
            <a id="navigation2" href="/Distributor/pay/PrePayList.aspx" class="cur">我的钱包</a></div>
            <!--功能条件 start-->
            <div class="userFun">
                <div class="left">
                    <i style="font-size: 15px;">企业钱包余额:</i><i style="font-size: 15px; color: Red; font-weight:bold;">￥<%=price%>&nbsp;</i>
                   <%--<i style="font-size: 15px;">累计收益:</i><i style="font-size: 15px; color: Red; font-weight:bold;">￥5&nbsp;</i>  --%>                 
                    <a href="remittanceAdd.aspx" id="remittanceAdd" runat="server" class="btnPay">充值</a>
                </div>
                <div class="right">
                    <ul class="term">
                        <li>
                            <label class="head">
                                流水帐号：</label><input name="txtReceiptNo" type="text" id="txtReceiptNo" runat="server" style="width:110px;"
                                    class="box" />
                        </li>
                        <li style="display: none">
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
                            <select name="ddrPayType" runat="server" id="ddrPayType" class="xl" style="width:95px;">
                                <option value="-1">全部</option>
                                <option value="1">充值</option>
                                <option value="2">企业钱包补录</option>
                                <option value="3">企业钱包冲正</option>
                                <option value="4">退款</option>
                                <option value="5">订单付款</option>
                               <option value="9">利息收益 </option>
                            </select>
                        </li>
                        <%-- <li>
                        <label class="head">
                            审核状态：</label>
                        <select name="ddrAuditState" runat="server" id="ddrAuditState" class="xl">
                            <option value="-1">全部</option>
                            <option value="0">未审</option>
                            <option value="2">已审</option>
                        </select>
                    </li>--%>
                        <li>
                            <label class="head">
                                每页</label><input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager"
                                    runat="server" class="box" style="width: 30px;" /><label class="head">条</label></li>
                    </ul>
                    <a href="javascript:void(0)" id="Search" class="btnBl"><i class="searchIcon"></i>搜索</a>
                    <a href="javascript:void(0)" id="li_Reset" class="btnBl liSenior"><i class="resetIcon"></i>高级</a>
                </div>
            </div>
            <div class="hidden userFun" style=" text-align:right; padding-right:160px; padding-top:10px; display:none;">
                <div class="right">
                    <ul class="term">
                        <li>
                            <label class="head">
                                制单日期：</label>
                            <input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtArriveDate"
                                    readonly="readonly" type="text" class="Wdate box" style=" width:100px;" value="" />
                            <i class="txt">—</i>
                            <input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtArriveDate1"
                                    readonly="readonly" type="text" class="Wdate box" style=" width:100px;" value="" />
                        </li>
                        <li>
                            <label class="head">
                                制单人：</label><input name="txtCrateUser" type="text" id="txtCrateUser" runat="server" 
                                    class="box" />
                        </li>
                    </ul>
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
                                        支付流水号
                                    </th>
                                    <th>
                                        金额（元）
                                    </th>
                                    <th>
                                        制单日期
                                    </th>
                                    <th>
                                        制单人
                                    </th>
                                    <%-- <th>
                                    审核状态
                                </th>--%>
                                    <%--<th>
                                        支付状态
                                    </th>--%>
                                    <th>
                                        款项类型
                                    </th>
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
                            <td>
                                <a href="javascript:void(0)" onclick='goInfo("<%# Common.DesEncrypt(Eval("ID").ToString(),Common.EncryptKey) %>")'>
                                    <%#Eval("ID") %></a>
                            </td>
                              <td>
                                <%# Eval("guid")%>
                            </td>
                            <td>
                                <%# Convert.ToDecimal(Eval("price")).ToString("N")%>
                            </td>
                            <td>
                                <%# Convert.ToDateTime(Eval("Paytime")).ToString("yyyy-MM-dd")%>
                            </td>
                            <td>
                              <%# new Hi.BLL.SYS_Users().GetModel(Convert.ToInt32(Eval("CrateUser"))) == null ? "" : new Hi.BLL.SYS_Users().GetModel(Convert.ToInt32(Eval("CrateUser"))).TrueName%>
                            </td>
                            <%-- <td>
                            <%# Common.GetNameBYPreStart(Convert.ToInt32(Eval("AuditState")))%>
                        </td>--%>
                            <%--<td>
                                <%# Common.GetNameBYPrePayMentStart(Convert.ToInt32(Eval("Start")))%>
                            </td>--%>
                            <td>
                                <%# Common.GetPrePayStartName(Convert.ToInt32(Eval("PreType")))%>
                            </td>
                            <td title="<%# Eval("vdef1")%>" style="cursor:pointer;">
                                <%# GetStr(Convert.ToString(Eval("vdef1")))%>
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
                        CustomInfoSectionWidth="35%" CssClass="paginList" CurrentPageButtonClass="paginItem"
                        NumericButtonCount="5" Width="100%" OnPageChanged="Pager_PageChanged">
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

    <div class="tip" style="display: none; z-index: 999;" id="divpaypwd" runat="server">
        <div style="position: absolute; top: 0; left: 0; right: 0; bottom: 0; z-index: 999;
            background: #fff;">
            <div class="tiptop">
                <span>修改支付密码</span></div>
            <div class="tipinfo">
                <ul class="ModifyData">
                    <li><i class="head"><i class="required">*</i>原支付密码：</i><asp:TextBox runat="server"
                        TextMode="Password" MaxLength="30" ReadOnly="true" CssClass="box" Enabled="false"
                        ID="paypwd"></asp:TextBox></li>
                    <li><i class="head"><i class="required">*</i>新支付密码：</i><input id="paypwd1" name=""
                        type="password" runat="server" maxlength="30" class="box" value="" /></li>
                    <li><i class="head"><i class="required">*</i>确认支付密码：</i><input id="paypwd2" name=""
                        type="password" runat="server" maxlength="30" class="box" value="" /></li>
                    <li style="text-align: center;"><span id="spanpaypwd" runat="server" style="color: Red;
                        height: 40px; width: 100%; text-align: center; display: block;"></span></li>
                </ul>
                <div class="mdBtn" style="text-align: center;">
                    <a id="A2" href="#" style="margin: 0" onclick="return asave();" onserverclick="A_Save"
                        runat="server" class="btnYe">确定修改</a></div>
            </div>
        </div>
        <div style="z-index: 998; opacity: 0.3; filter: Alpha(Opacity=30) !important; border-radius: 5px;
            top: -8px; left: -8px; right: -8px; bottom: -8px; background-color: rgb(0, 0, 0);
            position: absolute; top">
        </div>
    </div>
    <div id="zzc" class="zzc" runat="server" style="display: none;">
    </div>

    </form>
</body>
</html>

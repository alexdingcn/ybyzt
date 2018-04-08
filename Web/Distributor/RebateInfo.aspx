<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RebateInfo.aspx.cs" Inherits="Distributor_RebateInfo" %>
<%@ Register TagPrefix="Head" TagName="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagPrefix="Left" TagName="Left" Src="~/Distributor/DealerLeft.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>返利详细</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../Company/js/js.js" type="text/javascript"></script>
    <script src="../Company/js/order.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var type='<%=Request["type1"] %>';

            $("#returnIcon").click(function(){
                    location.href="RebateList.aspx";
            });
        });

        function a_addordreturn()
        {
            if($.trim($("#txtremark1").val())=="")
            {
                JScript.AlertMsgOne(this, "请填写退货理由！", JScript.IconOption.错误);
                return false;
            }
            else
            {
                $("#txtremark").val($.trim($("#txtremark1").val()));
            }
        }

    </script>
    <style>
        .addBank{ width:300px; height:150px; padding:10px; overflow:hidden; background:#fff; position:fixed; top:35%; left:40%; z-index:999; box-shadow: 0px 0px 0px 5px #333;}
        .addBank .title{ border-bottom:1px solid #d1d1d1; line-height:30px; font-size:14px; padding-left:5px;}
        .addBank .close{ background:url(images/fx.png) no-repeat 0 -52px; width:13px ; height:13px; display:block; position:absolute; top:10px; right:10px;}
        .payTis .pic{ position:absolute; top:10px; left:20px; background-image:url("../css/default/xubox_ico0.png"); background-position:-163px -75px; width:30px; height:30px;}
        .payTis i{ display:block; font-size:14px; color:#797979; line-height:28px; padding-bottom:10px;}
        .payTis{ position:relative; width:100%; padding-top:10px; padding-left:60px;}
        .payTis a{ height:30px; line-height:30px; color:#fff;}
        .opacity
        {
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 2000px;
            background-color: #000;
            opacity: 0.3;
            z-index: 998;
            filter: alpha(opacity=30);
        }
    </style>
</head>
<body class="root3">
    
    <form id="form1" runat="server" method="post">
    <Head:Head ID="Head2" runat="server" />
    <%--<textarea style="display:none" id="txtremark" runat="server"></textarea>--%>
    <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="nav-3" />
        <input type="hidden" id="hid_Alert" />
        <input id="hid_Alert2" type="hidden" />
        <div class="rightCon">
            <div class="info">
                <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" runat="server" href="RebateList.aspx" class="cur">我的返利</a>>
                <a id="navigation3" href="#" class="cur">返利订单详细</a>
            </div>
            <!--功能条件 start-->
            <div class="userFun">
                <a href="#" class="btnBl" id="returnIcon" runat="server">
                        <i class="returnIcon"></i>返回</a>
            </div>
            <!--功能条件 end-->
            <div class="blank10">
            </div>
            <!--订单管理详细 start-->
            <div class="orderNr">
                <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                    <tbody>
                        <tr>
                            <td class="head" style="width: 13%">
                                返利单编号
                            </td>
                            <td style="width: 37%">
                                <label id="lblReceiptNo" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head" style="width: 13%">
                                已用金额
                            </td>
                            <td style="width: 37%">
                                <label id="lblCreateDate" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="head">
                                返利金额
                            </td>
                            <td>
                                <label id="lblTotalPrice" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head">
                                可用金额
                            </td>
                            <td>
                                <label id="lblPayedPrice" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                             <td class="head">
                                返利类型
                            </td>
                            <td>
                                <label id="lblOtype" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head">
                                返利状态
                            </td>
                            <td>
                                <label id="lblAddType" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                             <td class="head">
                                有效期
                            </td>
                            <td colspan="3">
                                <label id="lblArriveDate" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="head">
                                备注
                            </td>
                            <td colspan="5" style="word-wrap: break-word; word-break: break-all;">
                                <label id="lblRemark" runat="server" style="line-height: 20px;">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                    </tbody>
                </table>
                
            </div>
            <div class="blank10"></div>
            <div class="orderLiv">
                    <table class="PublicList" border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th>订单编号</th>
                                <th>使用金额</th>
                                <th>使用日期</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rpDtl">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                             <div class="tc">
                                                 <%# new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(Eval("OrderID").ToString())).ReceiptNo%>
                                             </div>
                                        </td>
                                        <td>
                                            <div class="tc"> <%# Convert.ToDecimal(Eval("Amount")).ToString("N")%></div>
                                        </td>
                                        <td>
                                            <div class="tc"> <%# Convert.ToDateTime(Eval("CreateDate")).ToString("yyyy-MM-dd HH:mm:ss")%></div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr id="tr" runat="server" visible='<%# bool.Parse((rpDtl.Items.Count==0).ToString())%>' class="noordl">
                                        <td colspan="6" align="center">
                                            无匹配数据
                                        </td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
        </div>
    </div>
    <div class="opacity" id="opacityID" runat="server" style=" display:none;"></div>
    </form>
    
</body>
</html>

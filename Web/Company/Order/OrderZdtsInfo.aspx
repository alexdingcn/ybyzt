<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderZdtsInfo.aspx.cs"
    Inherits="Company_Order_OrderZdtsInfo" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>账单查询详细</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/order.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //返回
            $("#cancel").click(function () {

                   var type='<%=Request["showtype"]+"" %>';
                   var page='<%=Request["page"] %>';
//                   if(type=="PayZdblList")
//                   {
//                        window.location.href='../Pay/PayZdblList.aspx';
//                   }else if(type=="PaymentZdblcxList")
//                   {
                    window.location.href='../Pay/PayZdblList.aspx';
                     // window.location.href='../Pay/PaymentZdblcxList.aspx';
                  // }else
                   // window.location.href='OrderZdtsList.aspx?page='+page;
//                }}else{
//                    cancel();
//                }
            });

            //日志
           $("#Log").on("click", function () {
                var KeyId=<%=KeyID %>;
                var CompId=<%=this.CompID %>;
                Zd_Log(KeyId,CompId);
            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-2" />
    <input type="hidden" id="hid_Alert" />
    <div class="rightinfo" id="btnright" runat="server" >
        <!--当前位置 start-->
        <div class="place" id="btntitle" runat="server">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="../pay/PayZdblList.aspx">账单查询</a></li><li>></li>
                <li><a href="#" id="zd" runat="server">账单查询详细</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools" id="btn" runat="server" style="padding-left:5px;">
            <ul class="toolbar left">
                <li id="Remove" runat="server" style="display:none;"><span>
                    <img src="../images/t03.png" /></span>取消</li>
                <li id="Edit" runat="server"><span>
                    <img src="../images/t02.png" /></span>编辑</li>
                <li id="CopyOrder" runat="server" style="display:none;"><span>
                    <img src="../images/t14.png" /></span>复制</li>
                <li id="Log" runat="server"><span>
                    <img src="../images/tp2.png" /></span>日志</li>
                <li id="cancel" runat="server"><span>
                    <img src="../images/tp3.png" /></span>返回</li>
            </ul>
        </div>
        <!--功能按钮 end-->
        <div class="div_content" >
            <!--销售订单主体 start-->
            <div style="padding-top:5px;">
                <div class="lbtb">
                    <table class="dh">
                        <tr>
                            <td style="width:15%;">
                                <span>账单编号</span>
                            </td>
                            <td>
                                <label id="lblReceiptNo" runat="server">
                                </label>&nbsp;
                            </td>
                            <td style="width:15%;">
                                <span>代理商名称</span>
                            </td>
                            <td>
                                <label id="lblDisName" runat="server">
                                </label>&nbsp;
                                <input id="hidDisId" type="hidden" runat="server" />
                            </td>
                            <td style="width:15%;">
                               <span>账单日期</span>
                            </td>
                            <td>
                                <label id="lblCreateDate" runat="server">
                                </label>&nbsp;
                            </td>
                        </tr>
                        <tr>
                             <td>
                                <span>账单金额</span>
                            </td>
                            <td>
                                <label id="lblTotalPrice" runat="server">
                                </label>&nbsp;
                            </td>
                            <td>
                                <span>账单状态</span>
                            </td>
                            <td>
                                <label id="lblOState" style="color :Red;" runat="server">
                                </label>&nbsp;
                            </td>
                                 <td>
                                <span>有效截止日期</span>
                            </td>
                            <td>
                                <label id="lblArriveDate" runat="server">
                                </label>&nbsp;
                            </td>      
                          
                        </tr>
                       
                        <tr>
                            <td>
                                <span>支付金额</span>
                            </td>
                            <td>
                                <label id="lblPayedPrice" runat="server">
                                </label>&nbsp;
                            </td>
                            <td>
                                <span>支付状态</span>
                            </td>
                            <td >
                                <label id="lblPayState" runat="server">
                                </label>&nbsp;
                            </td>   
                                 <td>
                                <span>制单人</span>
                            </td>
                            <td>
                                <label id="lblDisUser" runat="server">
                                </label>&nbsp;
                            </td>             
                        </tr>
                       <tr>
                            <td>
                                <span>费用名称</span>
                            </td>
                            <td id="state" runat="server">
                                <label id="lblAddr" runat="server">
                                </label>&nbsp;
                                <input id="hidAddrId" type="hidden" runat="server" />
                            </td>
                            <td id="paytime1" runat="server" visible="false">
                                <span>收款日期</span>
                            </td>
                            <td id="paytime2" runat="server" visible="false" colspan="3">
                                <label id="lblpaytime" runat="server">
                                </label>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="background: #f6f6f6 none repeat scroll 0 0;">
                                <span style="height: auto;">订单备注</span>
                            </td>
                            <td colspan="5" style="word-wrap: break-word; padding-left:5px; word-break: break-all;">
                                <label id="lblRemark" runat="server" style="line-height: 20px;">
                                </label>&nbsp;
                                
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <!--销售订单主体 end-->
            <!--清除浮动-->
            <div style="clear: none;">
            </div>
        </div>
    </div>
    </form>
</body>
</html>

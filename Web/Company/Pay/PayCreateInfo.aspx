<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayCreateInfo.aspx.cs" Inherits="Company_Pay_PayCreateInfo" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>钱包查询</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../js/Pay.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            //返回
            $("#cancel").click(function () {
                //history.go(-1);
                window.location.href = 'PayDisList.aspx';
            });

            $("#Edit").click(function () {
                window.location.href = 'PayCorrectAdd.aspx?KeyID=<%=KeyID %>';
            });

            $("#btnBank").click(function () {
                window.location.href = 'PAbankEdit.aspx?paid=<%=KeyID %>';
            });
             //日志
            $("#Log").on("click", function () {
                var KeyId=<%=this.KeyID %>;
                var CompId=<%=this.CompID %>;
                Log(KeyId,CompId);
            });

            //生成订单
            $("#Audit").on("click", function () {
                $("#btnAudit").trigger("click");
            });

        });

       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-2" />
    <div class="rightinfo">

    <!--当前位置 start-->
    <div class="place">
        <ul class="placeul">
            <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
            <li><a href="PayDisList.aspx">钱包查询</a></li><li>></li>
            <li><a href="#">交易明细</a></li>
        </ul>
    </div>
    <!--当前位置 end-->
    <asp:Button ID="btnAudit" Text="审核" runat="server" OnClick="btnAudit_Click" Style="display: none;" />
    <input type="hidden" id="hid_Alert" />
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left" style="padding-left:5px;">
                <li id="Audit" runat="server"><span>
                    <img src="../images/tp1.png" /></span>审核</li>
                <%--<li id="Edit" runat="server"><span><img src="../images/t02.png" /></span>编辑</li>                
                    <li id="Del" runat="server"><span><img src="../images/t03.png" /></span>删除</li> --%>
                <li id="Log" runat="server"><span>
                    <img src="../images/tp2.png" /></span>日志</li>
                <li id="cancel" runat="server"><span><img src="../images/tp3.png" /></span>返回</li>
            </ul>
        </div>
        <!--功能按钮 end-->
        <div class="div_content">
            <!--销售订单主体 start-->
            <div>
                <div class="lbtb">
                    <table class="dh">
                        <tr>
                            <td style="width:110px;">
                                <span>代理商名称</span>
                            </td>
                            <td>
                                <label id="lbldis" runat="server">
                                </label>
                            </td>
                            <td>
                                <span>金额</span>
                            </td>
                            <td>
                                <label id="lblprice" runat="server">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>预收款来源</span>
                            </td>
                            <td>
                                <label id="lblpaytype" runat="server">
                                </label>
                            </td>
                           <td>
                                <span>付款方式</span>
                            </td>
                            <td>
                                <label id="lblpay_type" runat="server">
                                </label>
                            </td>
                        </tr>
                        <tr>
                      <td><span style=" height:100px;padding-top:45px; ">付款凭证</span></td >
                      <td colspan="3">
                    <%--   <asp:Panel runat="server" id="DFile" style=" margin-left:5px;"></asp:Panel>--%>

                       <ul class="list" id="ulAtta" runat="server"></ul>
                      </td>
                     </tr>
                        <tr>
                            <td>
                                <span>创建人</span>
                            </td>
                            <td>
                                <label id="lblcreateuser" runat="server">
                                </label>
                            </td>
                             <td style="width: 110px;">
                                <span>创建时间</span>
                            </td>
                            <td>
                                <label id="lblcreatetime" runat="server">
                                </label>
                            </td>
                           <%-- <td>
                                <span>审批状态</span>
                            </td>
                            <td>
                                <label id="Label1" runat="server">
                                </label>
                            </td>--%>
                        </tr>
                        <tr>
                            <td style="background: #f6f6f6 none repeat scroll 0 0;">
                                <span style="height: auto;">备注</span>
                            </td>
                            <td colspan="3">
                                <label id="lblRemark" runat="server">
                                </label>
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

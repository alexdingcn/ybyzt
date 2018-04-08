<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentEdit.aspx.cs" Inherits="Company_Pay_PaymentEdit" %>

<%@ Register Src="~/Company/UserControl/DisAreaTreeBox.ascx" TagPrefix="uc1" TagName="DisArea" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>订单收款补录新增</title>
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
        /*企业钱包补录 start */
        $(document).ready(function () {
            $_def.ID = "btnSave";
        });
        //验证用
        function formCheck() {
            var str = "";
            var txtcomp = $("#hidDisId").val();
            var txtprice = $("#txtPayCorrectPrice").val();
            var txtremark = $("#txtRemark").val();
            var ddltype = $("#ddltype").val();
            if (txtcomp == "") {
                str += "-请选择代理商；\r\n";
            }
//            if (txtprice == "") {
//                str += "-请填写金额；\r\n";
//            }
            if (ddltype == "-1") {
                str += "-请选择预收款来源；\r\n";
            }
//            if (txtremark == "") {
//                str += "-请填写备注；\r\n";
//            }
            if (txtremark.length > 200) {
                str += "-备注字数不能大于200个字符；\r\n";
            }


            if (str != "") {
                layerCommon.msg(str, IconOption.错误);
                return false;
            } else {
                return true;
            }

        }

        //金额只能输入正数和小数
        function KeyIntPrice(val) {
            val.value = val.value.replace(/[^\d.]/g, '');
        }
        //禁用f12
        document.onkeydown = function () {
            if (window.event && window.event.keyCode == 123) {
                //window.event.keyCode = 505;
                window.event.returnValue = false;
            }
        }
    </script>
    <style type="text/css">
        input[type='text']
        {
            width: 170px;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-2" />
    <div class="rightinfo">
    <!--当前位置 start-->
    <div class="place">
        <ul class="placeul">
            <li><a href="../../Company/jsc.aspx">我的桌面 </a></li><li>></li>
            <li><a href="../Pay/PayOrderList.aspx">订单收款补录</a></li><li>></li>
            <li><a href="#">订单收款补录新增</a></li>
        </ul>
    </div>
    <input id="hid_Alert" type="hidden" />
    <input id="hid_start" type="hidden" runat="server" />
    <input type="hidden" id="hidCompId" runat="server" />
    <!--当前位置 end-->
        <div >
            <!--收款帐号管理新增 start-->
            <div class="lbtb layoutRegist">
                <table class="dh">
                    <tr>
                        <td style="width: 15%;">
                            <span>
                                <label style="color: Red; display: inline-block;">
                                    <i class="required">*</i></label>&nbsp;代理商名称</span>
                        </td>
                        <td style="width: 30%;">
                           
                               <%-- <input name="txtdisname"  runat="server" id="txtdisname" style="cursor:pointer;" readonly="readonly" type="text" class="textBox" value="" />
                              --%> 
                               <label id="txtdisname" runat="server">
                                </label>
                               <input type="hidden" id="hidDisId" runat="server" />
                        </td>
                        <td style="width: 15%;">
                            <span>
                                <label style="color: Red; display: inline-block;">
                                    <i class="required">*</i></label>&nbsp;关联订单</span>
                        </td>
                        <td style="width: 30%;">
                            <%--<input name="txtordercode"  runat="server" id="txtordercode" style="cursor:pointer;" readonly="readonly" type="text" class="textBox" value="" />
                            --%> 
                              <label id="txtordercode" runat="server">
                                </label>
                               <input type="hidden" id="hidordid" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>
                                <label style="color: Red; display: inline-block;">
                                    <i class="required">*</i></label>&nbsp;款项来源</span>
                        </td>
                        <td>
                            <select id="ddltype" runat="server" class="textBox" style="width: 172px; margin-left: 5px;">
                                <option value="-1">请选择</option>
                                <option value="现金">现金</option>
                                <option value="转账汇款">转账汇款</option>
                                <option value="票据">票据</option>
                                <option value="其它">其它</option>
                                <%-- <option value="赊销">赊销</option>--%>
                            </select>
                        </td>
                        <td style="width: 15%;">
                            <span>
                                <label style="color: Red; display: inline-block;">
                                    <i class="required">*</i></label>&nbsp;收款金额</span>
                        </td>
                        <td style="width: 30%;">
                           <%-- <input id="txtPayCorrectPrice" onkeyup='KeyIntPrice(this)' type="text" runat="server"  readonly="readonly"
                                class="downBox" />--%>
                                 <label id="txtPayCorrectPrice" runat="server">
                                </label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>订单总价</span>
                        </td>
                        <td>
                               <label id="totalmoney" runat="server">
                                </label>
                        </td>
                        <td>
                            <span>已付金额</span>
                        </td>
                        <td>
                              <label id="paymoney" runat="server">
                                </label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="height: 60px; padding-top: 15px;"><span>
                                备注</span>
                        </td>
                        <td colspan="3">
                            <textarea id="txtRemark" maxlength="200" class="textarea" placeholder=""
                                runat="server"></textarea>
                        </td>
                    </tr>
                </table>
            </div>
            <%--</div>--%>
            <!--销售订单主体 start-->
            <div class="footerBtn">
                <asp:Button ID="btnSave" runat="server" Text="确定" CssClass="orangeBtn" OnClick="btnSave_Click"
                    OnClientClick="return formCheck()" />&nbsp;
                <input name="" type="button" onclick="javascript:window.location.href = 'PayOrderList.aspx';" class="cancel"
                    value="返回" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>

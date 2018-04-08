<%@ Page Language="C#" AutoEventWireup="true" CodeFile="payAccountAdd.aspx.cs" Inherits="Company_Pay_payAccountAdd" %>

<%@ Register Src="~/Company/UserControl/DisAreaTreeBox.ascx" TagPrefix="uc1" TagName="DisArea" %>
<%@ Register TagPrefix="uc1" TagName="SelectComp" Src="~/Admin/UserControl/TextCompList.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>收款帐号新增</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../js/order.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $_def.ID = "btnSave";
        });

        //验证用
        function formCheck() {
            var str = "";
            
            var txtpayname = $("#txtpayname").val();
            var txtpaycode = $("#txtpaycode").val();
            var txtorgcode = $("#txtorgcode").val();
            var ddltype = $("#ddltype").val();
            var txtRemark = $("#txtRemark").val();
            var txtqy = $.trim($("#<%=txtqy.Hid_ID %>").val());
            
//            if (txtqy == "") {
//                str += "-请选择区域；\r\n";
//            }
            if (ddltype=="20") {
            if (txtpayname == "") {
                str += "-请填写中金账户名称；\r\n";
            }
            if (txtpaycode == "") {
                str += "-请填写中金账户号码；\r\n";
            }
            }
            if (txtorgcode == "") {
                str += "-请填写机构代码；\r\n";
            }
            if (ddltype == "-1") {
                str += "-请选择账户类型；\r\n";
            }

            if (txtRemark.length > 200) {
                str += "-备注字数不能大于200个字符；\r\n";
            }

            if (str != "") {
                layerCommon.msg(str, IconOption.错误, 2000);
                return false;
            } else {
                return true;
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
            <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
            <li><a href="../Pay/PayAccountList.aspx" runat="server" id="atitle">收款帐号管理</a></li><li>></li>
            <li><a href="#">收款帐号新增</a></li>
        </ul>
    </div>
    <input id="hid_Alert" type="hidden" />
    <!--当前位置 end-->
        <div class="div_content">
            <!--收款帐号管理新增 start-->
            <div class="lbtb">
                <table class="dh">
                   <%-- <tr>
                        <td style="width: 15%;">
                            <span>
                                <label style="color: Red; display: inline-block;">
                                    *</label>&nbsp;厂商名称</span>
                        </td>
                        <td style="width: 30%;">
                            <%--<uc1:SelectComp runat="server" ID="txtcompID" style=" margin-left:2px;"  />
                            <input type="hidden" runat="server" id="hidtxtcompid" />
                            <input name="txtComp" maxlength="50" runat="server" id="txtComp" type="text" class="textBox" />
                        </td>
                         <td>
                            <span>
                                <label style="color: Red; display: inline-block;">
                                    *</label>&nbsp;机构代码</span>
                        </td>
                        <td>
                            <input name="txtorgcode" maxlength="50" runat="server" id="txtorgcode" type="text"
                                class="textBox" disabled="disabled" value="001520" />
                        </td>

                       
                    </tr>--%>
                   <%-- <tr>
                        <td>
                            <span>
                                <label style="color: Red; display: inline-block;">
                                    </label>&nbsp;中金账户名称</span>
                        </td>
                        <td>
                            <input name="txtpayname" maxlength="50" runat="server" id="txtpayname" type="text"
                                class="textBox" />
                        </td>
                        <td>
                            <span>
                                <label style="color: Red; display: inline-block;">
                                    </label>&nbsp;中金账户号码</span>
                        </td>
                        <td>
                            <input name="txtpaycode" maxlength="50" runat="server" id="txtpaycode" type="text"
                                class="textBox" />
                        </td>
                    </tr>--%>
                    <tr>
                       
                        <td  style="width: 15%;">
                            <span>
                                <label style="color: Red; display: inline-block;">
                                    *</label>&nbsp;账户类型</span>
                        </td>
                        <td style="width: 30%;">
                            <select id="ddltype" runat="server" class="textBox" style="width: 172px;">
                                <option value="-1">请选择</option>
                                <option value="11">个人账户</option>
                                <option value="12">企业账户</option>                               
                            </select>
                        </td>
                         <td style="width: 15%;">
                            <span>
                                <label style="color: Red; display: inline-block;">
                                    </label>&nbsp;区域</span>
                        </td>
                        <td style="width: 30%;">
                            <uc1:DisArea runat="server" Id="txtqy" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="height: 60px;">备注</span>
                        </td>
                        <td colspan="3">
                            <textarea id="txtRemark" maxlength="200" class="textarea" placeholder="订单备注不能大于200个字符"
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
                <input name="" type="button" onclick="javascript:history.go(-1);" class="cancel"
                    value="&lt&lt;返回" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AcrossTradeOutMoney.aspx.cs"
    Inherits="Company_Financing_AcrossTradeOutMoney" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>跨行出金</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script type="text/javascript">
        function goto() {
            $("#lblErr").html("");
            var txtPrice = $("#txtPrice").val();
            if (!parseFloat(txtPrice) > 0) {
                layerCommon.msg("金额必须大于0", IconOption.错误);
                return false;
            }
            return true;
        }
        function priceKeyup(obj) {
            //own.value = own.value.replace(/[^\d.]/g, '');
            var reg = /^[\d]+$/g;
            if (!reg.test(obj.value)) {
                var txt = obj.value;
                var i = 0;
                var arr = new Array();
                txt.replace(/[^\d.]/g, function (char, index, val) {//匹配第一次非数字字符
                    arr[i] = index;
                    i++;
                    obj.value = obj.value.replace(/[^\d.]/g, ""); //将非数字字符替换成""
                    var rtextRange = null;
                    if (obj.setSelectionRange) {
                        obj.setSelectionRange(arr[0], arr[0]);
                    } else {//支持ie
                        rtextRange = obj.createTextRange();
                        rtextRange.moveStart('character', arr[0]);
                        rtextRange.collapse(true);
                        rtextRange.select();
                    }
                });
            }
        }
        function priceBlur(own) {
            priceKeyup(own);
            own.value = own.value == "" ? "0" : own.value;
            own.value = parseFloat(own.value).toFixed(2);
        }
        function priceFocus(own) {
            if ($(own).val() == "0.00") {
                $(own).val("");
            }
        }
    </script>
    <style type="text/css">
        .dh
        {
            margin-top:50px;
        }
        .dh tr
        {
            height:40px;
        }
        .dh td
        {
            border: none;
        }
        .uTitle{filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffff', endColorstr='#f9f9f9'); background: -webkit-linear-gradient(top, #ffffff, #f3f3f3);    background: -moz-linear-gradient(top, #ffffff, #f3f3f3);background: -ms-linear-gradient(top, #ffffff, #f3f3f3);background: linear-gradient(top, #ffffff, #f3f3f3);border-bottom:1px solid #ddd; height:40px; line-height:40px; box-shadow: -1px 1px 1px #e6e6e6;}
        .uTitle b{ font-size:16px; color:#333; margin-left:15px; font-weight:normal;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <uc1:top ID="top1" runat="server" ShowID="" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="#">在线融资</a></li><li>></li>
                <li><a href="#">跨行出金</a></li>
            </ul>
        </div>
            <div class="uTitle">
                <b>跨行出金</b>
            </div>
            <div style=" border:1px solid #d1d1d1;">
            <div class="div_content">
                <div class="lbtb">
                    <table class="dh" style="width: 700px;">
                        <tr>
                            <td style="width: 140px; text-align:right;">
                                账户余额：
                            </td>
                            <td>
                                <label runat="server" id="SPAcBalance" style="font-size: 18px; color: Red;">
                                    ￥0.00&nbsp;</label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">
                                金额：
                            </td>
                            <td>
                                <input onfocus="priceFocus(this)" onblur="priceBlur(this)" onkeyup="priceKeyup(this)"
                                    id="txtPrice" name="txtPrice" type="text" class="textBox" runat="server" value="0.00"
                                    style="width: 200px; height: 30px; font-size: 20px; line-height: 30px;" /><i class="grayTxt">&nbsp;&nbsp;&nbsp;&nbsp;最多保留两位小数</i>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">
                                备注：
                            </td>
                            <td>
                                <textarea id="txtRemark" maxlength="200" class="textarea" runat="server" style=" width:400px;"></textarea>
                            </td>
                        </tr>
                    </table>
                    <input type="hidden" id="hidID" runat="server" />
                </div>
            </div>
            <div class="footerBtn" style="width: 800px;">
                <asp:Button ID="recharge" runat="server" Text="确定" CssClass="orangeBtn" OnClick="btn_save"
                    OnClientClick="return goto();" />&nbsp; &nbsp;&nbsp;<label id="lblErr" style="color: Red;"
                        runat="server"></label>
            </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

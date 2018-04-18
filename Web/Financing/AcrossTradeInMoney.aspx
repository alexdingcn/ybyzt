<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AcrossTradeInMoney.aspx.cs" Inherits="Financing_AcrossTradeInMoney" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>跨行入金</title>
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <link href="../Distributor/css/global.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        select
        {
            width:316px;
            height:26px;
            line-height:26px;
            padding:0 7px;
            margin-left:0px;    
            border:1px solid #d1d1d1;
            background-color:#E5E5E5;
        }
        .prepay
        {
            display:inline-block;
            float:right;
            margin-right:10px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            var aClick = function () {
                $("#lblErr").html("");
                var txtPrice = $("#txtPrice").val();
                if (!parseFloat(txtPrice) > 0) {
                    $("#recharge").one("click", aClick);
                    $("#lblErr").html("金额必须大于0");
                    return false;
                }
                if ($.trim($("#txtPayPwd").val()) == "") {
                    $("#lblErr").html("请输入支付密码");
                    $("#recharge").one("click", aClick);
                    return false;
                }
                $.ajax({
                    type: 'POST',
                    url: "../../Handler/Trd15000.ashx",
                    async: false,
                    data: { userid: "<%= Common.DesEncrypt(UserID.ToString(),Common.EncryptKey) %>", aclamt: $("#txtPrice").val(), Paypwd: $.trim($("#txtPayPwd").val()) },
                    success: function (data) {
                        var data1 = jQuery.parseJSON(data);
                        if (data1.success == "1") {
                            eval(data1.js);
                        }
                        if (data1.error == 1) {
                            $("#lblErr").html(data1.msg);
                        }
                    }
                });
                $("#recharge").one("click", aClick);
            };
            $("#recharge").one("click", aClick);
        });
        function goto() {
            $("#recharge").unbind("click",goto);    
            $("#lblErr").html("");
            var txtPrice = $("#txtPrice").val();
            if (!parseFloat(txtPrice) > 0) {
                layerCommon.alert("金额必须大于0", IconOption.错误);
                return false;
            }
            $.ajax({
                type: 'POST',
                url: "../../Handler/Trd15000.ashx",
                async: false,
                data: { userid: "<%= Common.DesEncrypt(UserID.ToString(),Common.EncryptKey) %>", aclamt: $("#txtPrice").val() },
                success: function (data) {
                    var data1 = jQuery.parseJSON(data);
                    if (data1.success == "1") {
                        eval(data1.js);
                    }
                    if (data1.error == 1) {
                        $("#lblErr").html(data1.msg);
                    }
                }
            });
            //$("#recharge").bind("click", goto);
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
</head>
<body>
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
        <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="AcrossTradeInMoney" />
        <div class="rightCon">
        <div class="info"><span class="homeIcon"></span><a id="navigation1" href="#">基本资料</a>><a id="navigation2" href="#" class="cur">基本信息</a></div>
            <!--转账汇款 start-->
            <div class="userTrend">
                <div class="uTitle">
                    <b>跨行入金</b>
                </div>
                <ul class="ModifyData">
                    <li><i class="head">账户余额：</i><span runat="server" id="SPAcBalance" style="font-size: 18px; color: Red;">￥0.00&nbsp;</span></li>
                    <li><i class="head"><i class="required">*</i>金 额：</i><input onfocus="priceFocus(this)" onblur="priceBlur(this)" onkeyup="priceKeyup(this)" id="txtPrice" name="txtPrice" type="text" class="box box2" runat="server" value="0.00" style="width:200px; height:30px; font-size:20px; line-height:30px;" /><span style="color:darkgrey;" >&nbsp;&nbsp;&nbsp;&nbsp;最多保留两位小数</span></li>
                    <li><i class="head"><i class="required">*</i>支付密码：</i><input id="txtPayPwd" name="" type="password" runat="server" class="box" value="" /> &nbsp;&nbsp;&nbsp;<a href="../Distributor/PayPWDEdit.aspx?type=RestPWD" style="text-decoration: underline;color: #999;">忘记密码?</a></li>
                    <li style="line-height:52px;"><i class="head" style=" display:table-cell; float:left;">备 注：</i><textarea id="txtRemark" maxlength="200" class="textarea" runat="server" style=" float:left;"></textarea><div class="clear"></div></li>
                </ul>
                <input type="hidden" id="hidID" runat="server" />
                <div class="mdBtn">
                    <a id="recharge" href="javascript:void(0)" onclick="" class="btnOr">充 值</a>
                    &nbsp;&nbsp;<label id="lblErr" style="color:Red;" runat="server" ></label>
                </div>
                <div class="blank10">
                </div>
            </div>
            <!--转账汇款 end-->
        </div>
        </div>
        <Footer:Footer ID="Footer" runat="server" />
    </form>
</body>
</html>

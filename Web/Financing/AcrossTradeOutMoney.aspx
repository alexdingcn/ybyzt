<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AcrossTradeOutMoney.aspx.cs" Inherits="Financing_AcrossTradeOutMoney" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>跨行出金</title>
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
        function goto() {
            $("#lblErr").html("");
            var txtPrice = $("#txtPrice").val();
            if (!parseFloat(txtPrice) > 0) {
                 $("#lblErr").html("金额必须大于0"); 
                return false;
            }
            if ($.trim($("#txtPayPwd").val()) == "") {
                $("#lblErr").html("请输入支付密码");
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
</head>
<body>
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
        <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="AcrossTradeOutMoney" />
        <div class="rightCon">
        <div class="info"><span class="homeIcon"></span><a id="navigation1" href="#">基本资料</a>><a id="navigation2" href="#" class="cur">基本信息</a></div>
            <!--转账汇款 start-->
            <div class="userTrend">
                <div class="uTitle">
                    <b>跨行出金</b>
                </div>
                <ul class="ModifyData">
                    <li><i class="head">账户余额：</i><span runat="server" id="SPAcBalance" style="font-size: 18px; color: Red;">￥0.00&nbsp;</span></li>
                    <li><i class="head"><i class="required">*</i>金 额：</i><input onfocus="priceFocus(this)" onblur="priceBlur(this)" onkeyup="priceKeyup(this)" id="txtPrice" name="txtPrice" type="text" class="box box2" runat="server" value="0.00" style="width:200px; height:30px; font-size:20px; line-height:30px;" /><span style="color:darkgrey;" >&nbsp;&nbsp;&nbsp;&nbsp;最多保留两位小数</span></li>
                    <li><i class="head"><i class="required">*</i>支付密码：</i><input id="txtPayPwd" name="" type="password" runat="server" class="box" value="" /> &nbsp;&nbsp;&nbsp;<a href="../Distributor/PayPWDEdit.aspx?type=RestPWD" style="text-decoration: underline;color: #999;">忘记密码?</a></li>
                    <li style="line-height:52px;"><i class="head" style=" display:table-cell; float:left;">备 注：</i><textarea id="txtRemark" maxlength="200" class="textarea" runat="server" style=" float:left;"></textarea><div class="clear"></div></li>
                </ul>
                <input type="hidden" id="hidID" runat="server" />
                <div class="mdBtn">
                    <a id="recharge" href="javascript:void(0)" runat="server" onclick="if(!goto())return false;" onserverclick="btn_save" class="btnOr">出 金</a>
                    &nbsp;&nbsp;<label id="lblErr" style="color:Red;" runat="server"></label>
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

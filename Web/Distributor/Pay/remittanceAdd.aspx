<%@ Page Language="C#" AutoEventWireup="true" CodeFile="remittanceAdd.aspx.cs" Inherits="Distributor_Pay_remittanceAdd" %>
<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>企业钱包充值</title>
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
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
    <script type="text/javascript" language="javascript">
        function goto() {
            var txtPrice = $("#txtPrice").val();
            if (!parseFloat(txtPrice) > 0) {
                layerCommon.alert("金额必须大于0", IconOption.哭脸);
                $("#txtPrice").val("0.00")
                //$("#lblErr").html("金额必须大于0");
                return false;
            }
//            if ($.trim($("#txtPayPwd").val()) == "") {
//                $("#lblErr").html("请输入支付密码");
//                return false;
            //            }


            //弹出新窗口
          window.open("", "transfer");
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
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
        <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="PrePayList" />
        <div class="rightCon">
        <div class="info"> <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
            <a id="navigation2" href="#" class="cur">企业钱包充值</a></div>
            <!--转账汇款 start-->
            <div class="userTrend">
                <div class="uTitle">
                    <b>企业钱包充值</b>
                </div>
                <ul class="ModifyData">
                    <li><i class="head">企业钱包余额：</i><span style="font-size: 18px; color: Red;">￥<%=PrePrice%>&nbsp;</span></li>
                    <li><i class="head">厂商名称：</i><span id="compName" runat="server"></span></li>
                    <!--<li><i class="head">转账流水号：</i><input id="txtGuid" name="txtGuid" type="text" class="box" disabled="disabled" runat="server" /></li>-->
                    <li><i class="head"><i class="required">*</i>金 额：</i><input onfocus="priceFocus(this)" onblur="priceBlur(this)" onkeyup="priceKeyup(this)" id="txtPrice" name="txtPrice" type="text" class="box box2" maxlength="15" runat="server" value="0.00" style="width:200px; height:30px; font-size:20px; line-height:30px;" /><span style="color:darkgrey;" >&nbsp;&nbsp;&nbsp;&nbsp;最多保留两位小数</span></li>
                    <%--<li><i class="head"><i class="required">*</i>支付密码：</i><input id="txtPayPwd" name="" type="password" runat="server" class="box" value="" /> &nbsp;&nbsp;&nbsp;<a href="../Distributor/PayPWDEdit.aspx?type=RestPWD" style="text-decoration: underline;color: #999;">忘记密码?</a></li>--%>
                    <li style="line-height:52px;"><i class="head" style=" display:table-cell; float:left;">备 注：</i><textarea id="txtRemark" maxlength="200" class="textarea" runat="server" style=" float:left;"></textarea><div class="clear"></div></li>
                </ul>
                <input type="hidden" id="hidID" runat="server" />
                <div class="mdBtn">
                    <a id="recharge" href="#" runat="server" onclick="if(!goto())return false;" onserverclick="Btn_Recharge" class="btnYe">充 值</a>
                    <%-- &nbsp;&nbsp;<label id="lblErr" style="color:Red;" runat="server"></label>--%>
                    <a id="ATrance" target="_blank" runat="server" style=" display:none;"  href=""></a>
                    </div>
                <div class="blank10">
                </div>
            </div>
            <!--转账汇款 end-->
        </div>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayDB.aspx.cs" Inherits="Distributor_Pay_PayDB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title> <%= ConfigurationManager.AppSettings["PhoneSendName"].ToString()%>支付平台</title>
    <link href="../../Distributor/css/pay.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/pay.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script type="text/javascript">        $(function () {
            if ($(window).innerWidth() <= 1024) { $("body").append('<link href="<%= ResolveUrl("../css/root.css")%>" rel="stylesheet" type="text/css" />'); }
        })
    </script>
    <script type="text/javascript">
        
        function change() {
            this.value = this.value.replace(/\D/g, '').replace(/....(?!$)/g, '$& '); //替换空格前4位数字为4位数字加空格  
        }
        var stop;
        $(document).ready(function () {
            $("#a2 a.check").click();

            $("#btnPay").one("click", check);

            $("#btnPayS").click(function () {
                $("#btnPaySuccess").click();
            });

            $("#btnPayF").click(function () {
                $("#btnPayFailure").click();
            });

            $("#txtBankCode").keyup(function () {
                $(this).val($(this).val().replace(/\D/g, '').replace(/....(?!$)/g, '$& '));
            });

            $("#txtPhoneNum").keyup(function () {
                if ($(this).val().length == 6 || $(this).val().length > 6) {
                    clearTimeout(stop);
                    $("#btnTx1376").click();
                    $("#txtPhoneNum").hide();
                    $("#paying").show();
                }
            });

            $("#otherBank").click(function (e) {
                e.stopPropagation();
                $(this).parent().hide();
                $(".otherBank").show();
            });

            $(".cancel").click(function () {
                $(".tip").fadeOut(100);
                $(".opacity").fadeOut(100);
                clearTimeout(stop);
                $("#btnPay").one("click", check);
            });

            $(".AccountType").click(function (e) {
                e.stopPropagation();
            });
        })

        var stopid;
        function time(own, wait, action) {
            if (wait > 0) {
                //$(own).removeAttr("onclick");
                $(own).html("重新获取验证码(" + wait + ")");
                wait--;
                stopid = setTimeout(function () { time(own, wait, action) }, 1000);
            } else {
                clearTimeout(stopid);
                $("#msg").html("");
                $(own).html("获取验证码");
                $("#submit2531").removeAttr("style");
                $(own).unbind().click(function () { onSubmit(own, action) });
                $("#isshow").hide();
            }
        }
        function closeTime(a) {
            var falg = arguments[0] ? arguments[0] : 0; 
            clearTimeout(stopid);
            $("#msg").html("");
            $("#submit2531").html("获取验证码");
            $("#submit2531").removeAttr("style");
            if(falg==0)
                $("#submit2531").unbind().click(function () { onSubmit($("#submit2531"), "2531") });
            $("#isshow").hide();
            $(".addBank .bankCard2 li").removeClass("border3"); //删除快捷支付绑定选择银行卡页面，所选择的银行卡
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
        function onSubmit(own, action) {
            var hidUserName = $("#hidUserName").val();
            var hidBankid = $("#hidBankid").val();
            var txtBankCode = $("#txtBankCode").val();
            var hidBankCode = $("#hidBankCode").val();
            var txtUserName = $("#txtUserName").val();
            var txtIDCard = $("#txtIDCard").val();
            var txtPhone = $("#txtPhone").val();
            var hidBankLogo = $("#hidBankLogo").val();
            var txtPhoneCode = $("#txtPhoneCode").val();
            var hidFas = $("#hidFas").val();
            var url = "";
            if (action == "2531") {
                $("#msg").html("");
                url = "<%=action2531 %>";
                if (txtBankCode == "") {
                   layerCommon.alert("银行卡必须填写！",IconOption.哭脸);
                    return false;
                }
                if (txtUserName == "") {
                    layerCommon.alert("姓名必须填写！",IconOption.哭脸);
                    return false;
                }
                if (txtIDCard == "") {
                    layerCommon.alert("身份证必须填写！",IconOption.哭脸);
                    return false;
                } else {
                    if (!validateIdCard(txtIDCard)) {
                        layerCommon.alert("身份证格式不正确！",IconOption.哭脸);
                        return;
                    }
                }
                if (txtPhone == "") {
                    layerCommon.alert("手机号必须填写！",IconOption.哭脸);
                    return false;
                } else {
                    var regMobile = /^1[3,5,8]\d{9}$/;
                    if (!regMobile.test(txtPhone)) {
                        layerCommon.alert("手机号码输入不正确，请重新输入！",IconOption.哭脸);
                        return false;
                    }
                }
                $("#hidBankCode").val($("#txtBankCode").val());
                $(own).html("正在发送...");
                $(own).css("color", "rgb(162, 162, 162)");
                $(own).removeAttr("onclick");
            } else if (action == "2532") {
                url = "<%=action2532 %>";
                if (txtPhoneCode == "") {
                    layerCommon.alert("手机验证码必须填写！",IconOption.哭脸);
                    return false;
                }

            }
            $.ajax({
                type: 'POST',
                url: url,
                data: { hidUserName: hidUserName, hidBankid: hidBankid, txtBankCode: txtBankCode, txtUserName: txtUserName, txtIDCard: txtIDCard, txtPhone: txtPhone, hidBankLogo: hidBankLogo, txtPhoneCode: txtPhoneCode, hidBankCode: hidBankCode, hidFas: hidFas },
                success: function (data) {
                    var data1 = jQuery.parseJSON(data);
                    if (action == "2531") {
                        if (data1.error == 1) {
                            closeTime();
                            $("#msg").html(data1.msg);
                        } else {
                            time(own, 120, action);
                            $("#hidFas").val(data1.id);
                            $("#isshow").show();
                        }
                    } else {
                        closeTime();
                        if (data1.error != 1) {
                            $("#msg").html("");
                            $('.cd-popup').click();
                            refFastBank();
                        } else {
                            $("#txtPhoneCode").val("");
                            $("#hidFas").val("");
                            $("#msg").html(data1.msg);
                        }
                    }
                }
            });
        }

        function refFastBank() {
            $.ajax({
                type: 'POST',
                url: "../../Handler/RefFastBank.ashx",
                data: { user: "<%= loguser.UserID %>" },
                success: function (data) {
                    var data1 = jQuery.parseJSON(data);
                    var html = "";
                    for (var i = 0; i < data1.length; i++) {
                        html += '<li><span><img src="' + data1[i].BankLogo + '" /></span><span style=" position:absolute; left:300px;"><i class="quick">快捷</i><i>储蓄卡</i>|<i>尾号**' + data1[i].bankcode.slice(-4) + '</i>|<i>手机号' + data1[i].phone.substr(0, 3) + '****' + data1[i].phone.slice(-4) + '</i></span><input type="hidden" class="hidFastID" value="' + data1[i].ID + '" /></li>';
                    }
                    $("#a2 ul.bankCard").html(html);
                }
            });
        }

        var payid = "";
        var prepayid = "";
        function submitPay(action) {
            var KeyID = '<%= Request.QueryString["KeyID"] %>';
            var hida1 = $("#hida1").val();
            var hida2 = $("#hida2").val();
            var hida3 = $("#hida3").val();
            var hida4 = $("#hida4").val();
            var padPaypas = $("#padPaypas").val();
            var txtPrice = $("#txtPrice").val();
            var txtPayOrder = $("#txtPayOrder").val();
            var hidFastPay = $("#hidFastPay").val();
            var hidUserName = $("#hidUserName").val();
            var txtBankCode = $("#txtBankCode").val();
            var txtPhoneNum = $("#txtPhoneNum").val();
            var AccountType = $("input[name='AccountType']:checked").val();
            var hidBank = $("#hidBank").val();
            $("#txtPhoneNum").val("");
            var url = "";
            if (action == "1375" || action == "1376") {
                if (action == "1375") {
                    url = "<%= action1375 %>";
                } else if (action == "1376") {
                    url = "<%= action1376 %>";
                }
                $.ajax({
                    type: 'POST',
                    url: url,
                    data: { KeyID: KeyID, hida1: hida1, hida2: hida2, hida3: hida3, padPaypas: padPaypas, txtPrice: txtPrice, txtPayOrder: txtPayOrder, hidFastPay: hidFastPay, hidUserName: hidUserName, txtBankCode: txtBankCode, txtPhoneNum: txtPhoneNum, payid: payid, prepayid: prepayid, isDBPay: 1},
                    success: function (data) {
                        var data1 = jQuery.parseJSON(data);
                        if (action == "1375" || action == "1376") {
                            if (data1.error == 1) {
                                $("#lblPayError").html(data1.msg);
                                if (data1.js !== null && data1.js != "") {
                                    eval(data1.js);
                                }
                                $("#btnPay").one("click", check);
                            }
                            if (data1.success == 1) {
                                payid = data1.payid;
                                prepayid = data1.prepayid;
                                eval(data1.js);
                                if (action == "1376") {
                                    payid = "";
                                    prepayid = "";
                                }
                            } else if (data1.success == 2) {
                                window.location.replace(data1.js);
                            }
                        }
                    }
                });
            } else if (action == "1311") {
                $("#divPayMsg").show();
                $(".opacity").show();
                $("#hidOid").val(KeyID);
                $("#hidPayOrder").val(txtPayOrder);
                $("#hidPrice").val("0.00");
                $("#hidIsPre").val("0");
                $("#IsDBPay").val("1");
                $("#hidBankNo").val(hidBank);
                $("#hidAccountType").val(AccountType);
                $("#hidPayPas").val(padPaypas);
                window.open("", "newWin");
                $("#Form").submit();
//                url = "<%= action1311 %>";
//                $.ajax({
//                    type: 'POST',
//                    url: url,
//                    data: { KeyID: KeyID, hida1: hida1, hida2: hida2, hida3: hida3, padPaypas: padPaypas, txtPrice: txtPrice, txtPayOrder: txtPayOrder, hidUserName: hidUserName, AccountType: AccountType, hidBank: hidBank, isDBPay: 1 },
//                    success: function (data) {
//                        var data1 = jQuery.parseJSON(data);
//                        if (data1.error == 1) {
//                            $("#lblPayError").html(data1.msg);
//                            $("#btnPay").one("click", check);
//                        }
//                        if (data1.success == 1) {
//                            $("#btnTx1311").click();
//                        } else if (data1.success == 2) {
//                            window.location.href = data1.js;
//                        }
//                    }
//                });
            } else if (action == "17000") {
                url = "<%= action17000 %>";
                $.ajax({
                    type: 'POST',
                    url: url,
                    data: { KeyID: KeyID, hida4: hida4, txtPayOrder: txtPayOrder, hidUserName: hidUserName },
                    success: function (data) {
                        var data1 = jQuery.parseJSON(data);
                        if (data1.success == "1") {
                            eval(data1.js);
                        }
                        if (data1.error == 1) {
                            $("#lblPayError").html(data1.msg);
                            $("#btnPay").one("click", check);
                        }
                    }
                });

            } else if (action == "71000") {
                url = "<%= action71000 %>";
                $.ajax({
                    type: 'POST',
                    url: url,
                    async: false,
                    data: { KeyID: KeyID, hida4: hida4, txtPayOrder: txtPayOrder, hidUserName: hidUserName },
                    success: function (data) {
                        try {
                            var data1 = jQuery.parseJSON(data);
                            if (data1.success == "1") {
                                eval(data1.js);
                            }
                            if (data1.error == 1) {
                                $("#lblPayError").html(data1.msg);
                                if (data1.js != "") {
                                    eval(data1.js);
                                }
                                $("#btnPay").one("click", check);
                            }
                        } catch (e) {
                            layerCommon.alert(e.name + ": " + e.message,IconOption.哭脸);
                        }
                    }
                });
            }
        }

        function check() {
            $("#txtPhoneNum").removeAttr("disabled");
            $("#lblPayError").html("");
            $("#FinancingA").hide();
            if ($("#a3").is("[class*='center']")) {
                if (!$("#a3").children("ul").children("li").is("[class*='border2']")) {
                    layerCommon.alert("请选择银行！",IconOption.哭脸);
                    return false;
                }
            }
            if ($("#a2").is("[class*='center']")) {
                if (!$("#a2").children("ul").children("li").is("[class*='border']")) {
                    layerCommon.alert("请绑定快捷支付银行卡！",IconOption.哭脸);
                    return false;
                }
            }
            if (!$("#a4").is("[class*='center']") && !$("#a3").is("[class*='center']") && !$("#a2").is("[class*='center']") && !$("#a1").is("[class*='center']")) {
                layerCommon.alert("请选择支付方式！",IconOption.哭脸);
                return false;
            }
            if (parseFloat($("#txtPayOrder").val()) == 0) {
                layerCommon.alert("支付金额必须大于0！",IconOption.哭脸);
                return false;
            }
            var hida1 = $("#hida1").val();
            var hida2 = $("#hida2").val();
            var hida3 = $("#hida3").val();
            var hida4 = $("#hida4").val();
            if ((hida3 == "1" && hida2 != "1" && hida4 != "1") || (hida1 == "1" && hida3 != "1" && hida2 != "1" && hida4 != "1")) {
                submitPay("1311");
            } else if (hida2 == "1" && hida3 != "1" && hida4 != "1") {
                submitPay("1375");
            } else if (hida4 == "1" && hida2 != "1" && hida3 != "1") {
                var hidType = $("#hidType").val();
                if (hidType == "1") {
                    submitPay("17000");
                } else if (hidType == "2") {
                    if (!!window.ActiveXObject || "ActiveXObject" in window) {
                        submitPay("71000");
                    } else {
                        layerCommon.alert("请使用IE浏览器！",IconOption.哭脸);
                        return false;
                    }
                }
            }
        }
        function msgTime(wait) {
            if (wait > 0) {
                $("#msgone").html(wait + "秒后短信验证码将失效！");
                wait--;
                stop = setTimeout(function () { msgTime(wait) }, 1000);
            } else {
                clearTimeout(stop);
                $("#txtPhoneNum").attr("disabled", "disabled");
                $("#msgone").hide();
                $("#msgtwo").show();
            }
        }
        function checkCode() {
            $("#paying").hide();
            $("#txtPhoneNum").show();
            if ($("#txtPhoneNum").val() == "") {
                layerCommon.alert("请填写验证码",IconOption.哭脸);
                return false;
            }
            submitPay("1376");
        }
        function setType(val) {
            $("#hidType").val(val);
            if (val == 1) {
                $(".payBtn .btn").html("立即支付");
            } else if (val == 2) {
                $(".payBtn .btn").html("申请借款");
            }
        }
        function BankCodeBlur(own) {
            $.ajax({
                type: 'POST',
                url: "../../Handler/BankCodeBlur.ashx",
                data: { BankCode: $(own).val() },
                success: function (data) {
                    if (data != "") {
                        var result = JSON.parse(data);
                        //layerCommon.alert(result.BankCode,IconOption.哭脸);
                        $("#imgBankImg").show();
                        $("#imgBankImg").attr("src", "<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>"+result.BankName+".jpg");
                        $("#hidBankid").val(result.BankCode);
                        $("#hidBankLogo").val("<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>"+result.BankName+".jpg");
                    }
                }
            });
        }
    </script>
    <style type="text/css">
        .tip
        {
            position: absolute;
            top: 50%;
            margin-top: -140px;
            left: 50%;
            margin-left: -225px;
            background: #fff;
            z-index: 999;
            border: 5px solid #888;
            overflow: hidden;
            width: 450px;
            height: 280px;
            display: none;
        }
        .tiptop
        {
            line-height: 40px;
            font-size: 16px;
            color: #fff;
            padding-left: 10px;
            position: relative;
            background: #4c5b78;
        }
        .tipinfo
        {
            padding-top: 20px;
        }
        .tipinfo .lb
        {
            padding-top: 10px;
            padding-left: 10px;
            font-size: 14px;
            line-height: 25px;
        }
        .tipinfo .title
        {
            display: inline-block;
            width: 100%;
            text-align: center;
            color: #606a74;
            font-size: 24px;
        }
        .tipinfo .tel
        {
            color: #999;
            font-size: 12px;
            text-align: center;
            display: block;
            padding-top: 5px;
        }
        .tipinfo .box2
        {
            border: 1px solid #c3c3c3;
            height: 48px;
            line-height: 48px;
            width: 290px;
            border-radius: 3px;
            margin-left: 75px;
            box-shadow: 2px 2px 1px 0px #eee inset;
            font-size: 30px;
            letter-spacing: 27px;
            text-indent: 25px;
        }
        .tipinfo .txt
        {
            color: #da4444;
            text-align: center;
            padding: 9px 0;
            font-size: 12px;
            display: block;
        }
        .tipinfo .txt2
        {
            border-top: 1px solid #d1d1d1;
            margin: 0px 20px;
            font-size: 12px;
        }
        .tipinfo .txt2 span
        {
            width: 260px;
            display: inline-block;
            text-align: center;
        }
        .tipinfo .txt2 .close2
        {
            margin-left: 60px;
        }
        .tipinfo .btn
        {
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffff', endColorstr='#fafafa');
            background: -webkit-linear-gradient(top, #ffffff, #fafafa);
            background: -moz-linear-gradient(top, #ffffff, #fafafa);
            background: -ms-linear-gradient(top, #ffffff, #fafafa);
            background: linear-gradient(top, #ffffff, #fafafa);
            border: 1px solid #ddd;
            border-radius: 5px;
            display: inline-block;
            line-height: 26px;
            padding: 0px 10px;
        }
        .tipinfo .btn:hover
        {
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#f57403', endColorstr='#f57403');
            background: #f57403;
            border: 1px solid #d66500;
            color: #fff;
            text-decoration: none;
        }
        .tiptop .close
        {
            background: url(../images/icon.png) no-repeat 0px -119px;
            width: 14px;
            height: 14px;
            cursor: pointer;
            display: inline-block;
            position: absolute;
            top: 12px;
            right: 12px;
            border: none;
            padding: 0;
        }
        .payOpt .li
        {
            margin-top: 2px;
        }
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
        #a4 .bankCard2 li
        {
            text-align:center; 
            width:150px; 
            padding-left:0;
            margin-right:50px;
        }
        #a4 .bankCard2 li img
        {
            padding-top:3px;
        }
        #a4 .bankCard2 li label
        {
            font-size:16px;
            position: relative;
            top:-5px;
        }
        .decoration:hover
        {
            text-decoration:none;
        }
        /*
		.cancel{ background:#f57403; border:1px solid #d66500; height:25px; line-height:25px; width:65px; text-align:center; color:#fff; cursor:pointer;}
		.cancel:hover{ border:1px solid #f2c69e; background:#ffebd9; color:#f57403;}
		*/
    </style>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <!--header start-->
    <div class="header">
        <div class="con">
            <div class="logo">
                <a href="../../index.aspx">
                    <img src="../../images/logo2.0.png" height="37" /></a><i>支付平台</i></div>
            <div class="topMenu">
<a href="../UserIndex.aspx">我的桌面</a>|<a href="<%=ResolveUrl("../../EShop/index.aspx") %>?Comid=<%=loguser.CompID %>">e店铺</a>|<a href="orderPayList.aspx">我的待支付订单</a> &nbsp;
<% if (ConfigurationManager.AppSettings["OrgCode"] == "SYJ")
   {%>
<i style=" font-weight:bold; color:red;">服务热线：400-8859-319</i>
<% } %>
</div>
                    
        </div>
    </div>
    <!--header end-->
    <!--隐藏域 start -->
    <input type="hidden" id="hidPay" runat="server" value="0" />
    <input type="hidden" id="hidPrepay" runat="server" value="0" />
    <input type="hidden" id="hidNoPrice" runat="server" value="0" />
    <input type="hidden" id="hidOrderid" name="hidOrderid" runat="server" value="0" />
    <input type="hidden" id="hidUserName" name="hidUserName" runat="server" value="" />
    <input type="hidden" id="hidSumPrice" name="" runat="server" value="0" />
    <input type="hidden" id="hidPricePay" runat="server" value="0.00" />
    <!--隐藏域  end  -->
    <!--支付信息 start-->
    <div class="payInfo">
        <div class="number">
            订\账单号：<a href="javascript:void(0)" id="lblOrderNO" runat="server"></a></div>
        <div class="gray">
            您的订\账单已经确认，请及时支付。</div>
        <ul class="amount">
            <li class="ab">本次支付金额：<b class="money"><input id="txtPayOrder" runat="server" onkeyup="priceKeyup(this)" type="text" class="box1" /></b>元（支付金额可修改）</li>
            <li>订\账单总金额：<b class="price" runat="server" id="lblPriceO"></b>元　未付款金额：<b class="price" runat="server" id="lblPricePay"></b>元</li>
            </ul>
    </div>
    <!--支付信息 end-->
    <div class="payOpt" id="checkId">
        <!--企业钱包支付 start-->
        <%if (false)
          { %><div class="li" id="a1" style="display:none">
            <input type="hidden" name="hida1" id="hida1" runat="server" value="" />
            <a href="javascript:void(0)" class="check cur"></a>
            <div class="title">
                企业钱包</div>
            <div class="amOpt">
                <i>可用余额：<label id="lblSumPrice" runat="server"></label>
                    元 </i>
                <input id="txtPrice" name="txtPrice" onkeyup="priceKeyup(this)" type="text" value="0.00" class="box" runat="server" style="display:none" /></div>
            <div class="amount">
                支付<b><label id="lblPrice1" runat="server">0.00</label></b>元</div>
            <input type="hidden" id="hidYorN1" value="0" />
        </div><%} %>
        <!--企业钱包支付 end-->
        <!--快捷支付 start-->
        <div class="li" id="a2">
            <input type="hidden" name="hida2" id="hida2" runat="server" value="" />
            <input type="hidden" name="hidFastPay" id="hidFastPay" runat="server" value="0" />
            <a href="javascript:void(0)" class="check cur"></a>
            <div class="title">
                快捷支付</div>
            <ul class="bankCard">
                <asp:Repeater runat="server" ID="rptQpay" onitemcommand="rptQpay_ItemCommand">
                    <ItemTemplate>
                        <li><span>
                            <img src="<%# Eval("BankLogo") %>" /></span>
                            <span style=" position:absolute; left:300px;"><i class="quick">快捷</i><i>储蓄卡</i>|<i>尾号**<%# Eval("bankcode").ToString().Replace(" ", "").Substring(Eval("bankcode").ToString().Replace(" ", "").Length - 4, 4)%></i>|<i>手机号<%# Eval("phone").ToString().Substring(0,3) %>****<%# Eval("phone").ToString().Substring(Eval("phone").ToString().Length-4,4) %></i></span><input type="hidden" class="hidFastID" value="<%# Eval("ID") %>" /></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <div class="addCard">
                <a href="javascript:void(0)" class="time cd-popup-trigger">添加银行卡</a></div>
            <div class="amount">
                支付<b><label id="lblPrice2" runat="server">0.00</label></b>元</div>
            <input type="hidden" id="hidYorN2" value="0" />
        </div>
        <!--快捷支付 end-->
        <!--网银支付 start-->
        <div class="li" id="a3">
            <input type="hidden" name="hida3" id="hida3" runat="server" value="" />
            <input type="hidden" name="hidBank" id="hidBank" runat="server" />
            <a href="javascript:void(0)" class="check cur"></a>
            <div class="title">
                网银支付</div>
            <div class="amOpt">
             &nbsp;&nbsp;账户类型：
                <input type="radio" name="AccountType" value="11" id="type11" class="AccountType" runat="server" checked="true" /><label class="AccountType" for="type11">个人账户</label>
              &nbsp;&nbsp;  <input type="radio" name="AccountType" value="12" id="type12" class="AccountType" runat="server" /><label class="AccountType" for="type12">企业账户</label>
            </div>
            <div class="amount">
                支付<b><label id="lblPrice3" runat="server">0.00</label></b>元</div>
            <ul class="bankCard2">
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国工商银行.jpg" /><input type="hidden" class="hidBank" value="102" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国农业银行.jpg" /><input type="hidden" class="hidBank" value="103" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国银行.jpg" /><input type="hidden" class="hidBank" value="104" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国建设银行.jpg" /><input type="hidden" class="hidBank" value="105" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>交通银行.jpg" /><input type="hidden" class="hidBank" value="301" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中信银行.jpg" /><input type="hidden" class="hidBank" value="302" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国光大银行.jpg" /><input type="hidden" class="hidBank" value="303" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>华夏银行.jpg" /><input type="hidden" class="hidBank" value="304" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国民生银行.jpg" /><input type="hidden" class="hidBank" value="305" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>广发银行.jpg" /><input type="hidden" class="hidBank" value="306" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>平安银行.jpg" /><input type="hidden" class="hidBank" value="307" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>招商银行.jpg" /><input type="hidden" class="hidBank" value="308" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>兴业银行.jpg" /><input type="hidden" class="hidBank" value="309" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>浦发银行.jpg" /><input type="hidden" class="hidBank" value="310" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>宁波银行.jpg" /><input type="hidden" class="hidBank" value="408" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>天津银行.jpg" /><input type="hidden" class="hidBank" value="434" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>徽商银行.jpg" /><input type="hidden" class="hidBank" value="440" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>北京银行.jpg" /><input type="hidden" class="hidBank" value="403" /></a></li>
            </ul>
            <input type="hidden" id="hidYorN3" value="0" />
        </div>
        <!--网银支付 end-->
        <% if(ConfigurationManager.AppSettings["IsFinancing"] == "1" && orderModel.Otype!=(int)Enums.OType.推送账单){ %>
        <!--在线融资 start-->
        <div class="li" id="a4" style="display:none;">
            <input type="hidden" name="hida4" id="hida4" runat="server" value="" />
            <input type="hidden" id="hidType" value="" runat="server" />
            <a href="javascript:void(0)" class="check cur"></a>
            <div class="title">
                在线融资</div>
            <div class="amOpt">
                &nbsp;&nbsp;<label id="lblBalance" style="display:none">账户余额：<label id="lblBalance1" runat="server" style="color:#ff4e02; font-size:18px;">0.00</label>&nbsp;&nbsp;元</label>
                &nbsp;&nbsp;<label id="lblBalance2" style="display:none">授信余额：<label id="lblBalance3" runat="server" style="color:#ff4e02; font-size:18px;">0.00</label>&nbsp;&nbsp;元</label>
                &nbsp;&nbsp;<label id="lblFinancingMsg" style="color: #2ea7e7;">每个订单只能借款一次</label>
                <% if (PList != null && PList.Count > 0)
                   { %>
                &nbsp;&nbsp;<label>融资处理中金额：<label id="lblBalance5" class="price" runat="server" style=" color:#666; font-size:14px; font-weight:bold;">0.00</label>&nbsp;&nbsp;元</label>
                <% } %>
            </div>
            <div class="amount">
                支付<b><label id="lblPrice4" runat="server">0.00</label></b>元</div>
            <ul class="bankCard2">
                <li onclick="setType(1)"><img src="../images/BalancePay.png" /><label>账户余额支付</label></li>
                <% if (PList != null && PList.Count == 0)
                   { %>
                <li onclick="setType(2)" title="仅支持IE浏览器操作"><img src="../images/Loan.png" /><label>申请借款</label></li>
                <% } %>
            </ul>
        </div>
        <!--在线融资 end-->
        <% } %>
        <div class="payPas" style="display: none">
            企业钱包支付密码:<input name="padPaypas" id="padPaypas" type="password" class="box3" runat="server"
                value="" />&nbsp;&nbsp;&nbsp; <a href="../PayPWDEdit.aspx?type=zhifu&ordID=<%=KeyID %>"
                    style="text-decoration: underline;">忘记密码?</a></div>
        <div class="payBtn">
            <a href="javascript:void(0)" id="btnPay" class="btn">立即支付</a>
            <%--<asp:Button ID="btnTx1311" runat="server" style="display:none;" OnClick="btnTx1311_Click" />--%>
            <%--<asp:Button ID="btnPay" runat="server" CssClass="btn" Text="立即支付" OnClick="btnPay_Click" OnClientClick="return check()" />--%>
            <i class="red">
                <label id="lblPayError" runat="server">
                </label>
            </i>
            <a id="FinancingA" href="https://111.205.98.184:80/logon/account.htm" style="display:none;" target="_blank" runat="server">融资平台登录</a>
        </div>
    </div>
    <!--快捷支付弹窗 start-->
    <div class="cd-popup" id="addBank">
        <div class="cd-popupBg">
        </div>
        <div class="hidebg">
        </div>
        <!--添加银行卡 start-->
        <div class="cd-popup-container addBank a5 noOff">
            <div class="title">
                <b>添加银行卡</b>&nbsp;<i style=" color:#2ea7e7;">(暂不支持信用卡绑定)</i></div>
            <ul class="bankCard2">
                 <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国工商银行.jpg" /><input
                        type="hidden" class="hidBankCode" value="102" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国农业银行.jpg" /><input
                        type="hidden" class="hidBankCode" value="103" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国银行.jpg" /><input
                        type="hidden" class="hidBankCode" value="104" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国建设银行.jpg" /><input
                        type="hidden" class="hidBankCode" value="105" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>交通银行.jpg" /><input
                        type="hidden" class="hidBankCode" value="301" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国光大银行.jpg" /><input
                        type="hidden" class="hidBankCode" value="303" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国民生银行.jpg" /><input
                        type="hidden" class="hidBankCode" value="305" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>广发银行.jpg" /><input
                        type="hidden" class="hidBankCode" value="306" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中信银行.jpg" /><input
                        type="hidden" class="hidBankCode" value="302" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>浦发银行.jpg" /><input
                        type="hidden" class="hidBankCode" value="310" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>兴业银行.jpg" /><input
                        type="hidden" class="hidBankCode" value="309" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>上海银行.jpg" /><input
                        type="hidden" class="hidBankCode" value="401" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>北京银行.jpg" /><input
                        type="hidden" class="hidBankCode" value="403" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>平安银行.jpg" /><input
                        type="hidden" class="hidBankCode" value="307" /></a></li>
                 <li onclick="return false;" style=" text-align:center; line-height:40px; border:0px; padding:0px; width:204px; height:40px;" ><a id="otherBank" href="javascript:void(0)" style="color:#2ea7e7; font-size:14px; width:100%;">更多银行</a></li>
                <asp:Repeater runat="server" ID="rptOtherBank">
                    <ItemTemplate>
                        <li class="otherBank"><a href="javascript:void(0)">
                    <img src="<%# Common.GetWebConfigKey("ImgViewPath") + "BankImg/" + Eval("BankName") %>.jpg" /><input type="hidden" class="hidBankCode" value="<%# Eval("BankCode") %>" /></a></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            
            <div class="payBtn">
                <a href="javascript:void(0)" class="btn btnId">下一步</a></div>
            <a href="javascript:void(0)" class="close cd-popup-close img-replace" onclick="closeTime(1)">关闭</a>
        </div>
        <!--添加银行卡 end-->
        <!--添加银行卡 start-->
        <div class="cd-popup-container addBank a6">
            <div class="title">
                <b>添加银行卡</b>&nbsp;<i style=" color:#2ea7e7;">(暂不支持信用卡绑定)</i></div>
            <ul class="addCardNr">
                <li><i class="bt">银行卡</i><input id="txtBankCode" name="txtBankCode" type="text" class="box2" onkeyup="this.value=this.value.replace(/\D/g,'');" onblur="BankCodeBlur(this)" /><input type="hidden" id="hidBankCode" /></li>
                <li><i class="bt">付款银行</i>
                    <div class="card">
                        <span><img id="imgBankImg" src="../images/payBank.jpg"></span>
                        <input type="hidden" id="hidBankid" name="hidBankID" />
                        <input type="hidden" id="hidBankLogo" name="hidBankLogo" />
                    </div>
                    <a href="javascript:void(0)" class="return decoration"><i class="icon"></i>选择其他银行</a>
                </li>
                <li><i class="bt">姓名</i><input id="txtUserName" name="txtUserName" type="text" class="box3" maxlength="40" /></li>
                <li><i class="bt">身份证</i><input id="txtIDCard" name="txtIDCard" type="text" class="box2" maxlength="40" /></li>
                <li><i class="bt">手机号</i><input id="txtPhone" name="txtPhone" type="text" class="box3" onkeyup="this.value=this.value.replace(/\D/g,'')" value="" maxlength="11" />
                    <a href="javascript:void(0)" class="btn2 decoration" id="submit2531" onclick="onSubmit(this,'2531')">获取验证码</a></li>
                <li><i class="bt">手机验证码</i><input id="txtPhoneCode" name="txtPhoneCode" type="text" onkeyup="this.value=this.value.replace(/\D/g,'')"
                    class="box3" style="width: 50px;" value="" />&nbsp;&nbsp;<label style="color:#da4444;" id="msg"></label>
                    <input type="hidden" id="hidFas" />
                    <% if (ConfigurationManager.AppSettings["Paytest_zj"] == "1")
                              { %>
                               <i id="isshow" style="display:none;">  <b class="red">（测试密码:123456）</b></i>
                                 <% }%>
                </li>
          
 </ul>
            <div class="payBtn payBtn2">
                <a href="javascript:void(0)" class="btn" onclick="onSubmit(this,'2532')">确认绑定</a></div>
            <a href="javascript:void(0)" class="close cd-popup-close img-replace" onclick="closeTime(1)">关闭</a>
        </div>
        <!--添加银行卡 end-->
    </div>
    <!--快捷支付弹窗 end-->
    <!--弹出遮罩层 Begin-->
    <div class="opacity" style="display:none;">
    </div>
    <!--弹出遮罩层 End-->
 
    <!--弹出录入层 Begin-->
    <div class="tip">
        <div class="tiptop"><span>快捷支付</span><input name="" type="button" class="cancel close" value="" /></div>
        <div class="tipinfo">
        	<div class="lb"><span class="title">请输入短信验证码</span><i class="tel" runat="server" id="phone">已发送至188****2156</i></div>
            <div class="lb">
                <input id="txtPhoneNum" class="box2" type="text" onkeypress="priceKeyup(this)" onkeyup="priceKeyup(this)" runat="server" autocomplete="off" />
                <span style="display:none; margin-left:195px;" id="paying"><b>正在支付...</b></span>
                <%--<asp:Button ID="btnTx1376" style="display:none;" runat="server" Text="确认" OnClick="btnTx1376_Click" OnClientClick="return checkCode();" />--%>
                <input id="btnTx1376" type="text" style="display:none;" value="确认" onclick="checkCode()" />
            </div>
            <div class="lb txt"><span>短信验证码已发送，请注意查收</span></div>
			<div class="lb txt2">                
                <span>
                    <%--<a href="" class="btn" style="display:none;">重新获取手机短信验证码</a>--%>
                    <i class="" id="msgone">120秒后短信验证码将失效</i> 
                    <i id="msgtwo" style="color:Red;">验证码已失效，请关闭窗口，重新支付！</i> 
                </span>
                |<a href="javascript:void(0)" class="cancel close2">关闭</a>
                
            </div>	
		</div>
    </div>
    <!--弹出录入层 End-->

    <!--网上支付提示 start-->
    <div class="addBank" id="divPayMsg" style="display:none; height:225px;">
		<div class="title"><b>网上支付提示 </b></div>
        <div class="payTis"><span class="pic"><img src="../images/payTis.gif" width="200" /></span><i>支付完成前，请不要关闭支付验证窗口<br />支付完成后，请根据您支付的情况点击下面按钮</i><a href="javascript:void(0)" id="btnPayF" class="bluBtn">支付遇到问题</a><a href="javascript:void(0)" id="btnPayS" class="redBtn">已经完成支付</a></div>
        <asp:Button ID="btnPaySuccess" runat="server" style="display:none;" OnClick="btnPaySuccess_Click" />
        <asp:Button ID="btnPayFailure" runat="server" style="display:none;" OnClick="btnPayFailure_Click" />
	</div>
    <!--网上支付提示 end-->

    <div class="blank20">
    </div>



    <!--常见问题 start-->
    <div class="question">
        <%--<div class="title">
            常见问题</div>
        <dl class="list">
            <dt>1、经常需要动态给Div添加或者删除样式，利用jquery的addClass(id)与delete</dt>
            <dd>
                在开发网页过程中常需要对A链接使用JS制作一个点击弹窗,那么在点击该A链接就不能发生跳转行为,于是我们取消A链接的跳转</dd>
            <dt>1、经常需要动态给Div添加或者删除样式，利用jquery的addClass(id)与delete</dt>
            <dd>
                在开发网页过程中常需要对A链接使用JS制作一个点击弹窗,那么在点击该A链接就不能发生跳转行为,于是我们取消A链接的跳转</dd>
            <dt>1、经常需要动态给Div添加或者删除样式，利用jquery的addClass(id)与delete</dt>
            <dd>
                在开发网页过程中常需要对A链接使用JS制作一个点击弹窗,那么在点击该A链接就不能发生跳转行为,于是我们取消A链接的跳转</dd>
        </dl>--%>
    </div>
    <!--常见问题 end-->
    <div class="footer">
       <%= ConfigurationManager.AppSettings["CompanyName"].ToString() %></div>
    </form>
    <form id="Form" action="Request.aspx" method="post" target="newWin">  
        <input type="hidden" id="hidOid" name="hidOid" runat="server" value="" />
        <input type="hidden" id="hidPayOrder" name="hidPayOrder" runat="server" value="" />
        <input type="hidden" id="hidPrice" name="hidPrice" runat="server" value="" />
        <input type="hidden" id="hidIsPre" name="hidIsPre" runat="server" value="" />
        <input type="hidden" id="hidIsDBPay" name="hidIsDBPay" runat="server" value="" />
        <input type="hidden" id="hidBankNo" name="hidBankNo" runat="server" value="" />
        <input type="hidden" id="hidAccountType" name="hidAccountType" runat="server" value="" />
        <input type="hidden" id="hidPayPas" name="hidPayPas" runat="server" value="" />
    </form>
</body>
</html>

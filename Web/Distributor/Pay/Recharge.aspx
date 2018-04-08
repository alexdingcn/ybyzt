<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Recharge.aspx.cs" Inherits="Distributor_Pay_Recharge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title> <%= ConfigurationManager.AppSettings["PhoneSendName"].ToString()%>支付平台</title>
    <link href="../../Distributor/css/pay.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/payRecharge.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script type="text/javascript">
        window.history.forward();
        $(document).ready(function () {
            $("#txtBankCode").keyup(function () {  
          <%-- 银行卡格式化 1111 2222 3333 4444--%>
                $(this).val($(this).val().replace(/\D/g, '').replace(/....(?!$)/g, '$& '));
            });

            $("#txtBankCode").keydown(function () {
             <%-- 银行卡格式化 1111 2222 3333 4444--%>
                $(this).val($(this).val().replace(/\D/g, '').replace(/....(?!$)/g, '$& '));
            });

            $("#txtBankCode").keypress(function () {
             <%-- 银行卡格式化 1111 2222 3333 4444--%>
                $(this).val($(this).val().replace(/\D/g, '').replace(/....(?!$)/g, '$& '));
            });

            $("#txtPhoneNum").keyup(function () {
         <%--快捷支付，检验验证码，输入6位数字后提交数据 --%>
                if ($(this).val().length == 6 || $(this).val().length > 6) {
                    $("#btnTx1376").click();<%--检验快捷支付验证码必填--%>
                    $("#txtPhoneNum").hide();<%--隐藏输入密码的文本框--%>
                    $("#paying").show();<%--正在支付文字显示--%>
                }
            });

            $(".AccountType").click(function (e) {  <%--网银支付，选择账户类型时，取消冒泡事件--%>
                e.stopPropagation();
                  var abc=$(this).attr("id");
                if(abc=="type11")
                {                   
                    $("#divbank1").show();
                    $("#divbank2").hide();
                     $("#divbank3").hide();
                    $("#hid_PayType").val("11");
                    
                 <%--支付手续费 -start  --%>
                    var a3_price = 0.00;
                     var a3zfje=0.00;
                      <%-- 本次支付金额--%>
                     var txtPrice = $("#lblPrice").html();

                     
                     a3zfje  =parseFloat(txtPrice);                    
                     
                     var bankCode = $("#hidBank").val() == "" ? "0" : $("#hidBank").val();
                   
               <%--网银支付 --%>
                        a3_price = (a3zfje * parseFloat($("#pay_b2cwyzfbl").val())).toFixed(3);
                         <%-- 封底 --%>
                        var pay_b2cwyzfstart=parseFloat($("#pay_b2cwyzfstart").val());
                        if(a3_price<pay_b2cwyzfstart )
                                a3_price=pay_b2cwyzfstart;

                    $("#lblPrice3_sxf").html(parseFloat(a3_price).toFixed(2)); 
                    
                    $("#sumje").html((parseFloat(a3zfje) + parseFloat(a3_price)).toFixed(2));                    
                       <%--支付手续费 -end  --%>
                }
                 else  if(abc=="type13")
                {
                   
                    $("#divbank3").show();
                    $("#divbank2").hide();
                    $("#divbank1").hide();
                    $("#hid_PayType").val("13");

                    
                    <%-- 支付手续费 -start  --%>
                    var a3_price = 0.00;
                     var a3zfje=0.00;

                       <%-- 本次支付金额--%>
                     var txtPrice = $("#lblPrice").html();
                     
                     a3zfje  =parseFloat(txtPrice);     

                     <%--网银支付 --%>
                     a3_price = (a3zfje * parseFloat($("#pay_b2cwyzfbl").val())).toFixed(3);
                      <%-- 封底 --%>
                        var pay_b2cwyzfstart=parseFloat($("#pay_b2cwyzfstart").val());
                        if(a3_price<pay_b2cwyzfstart )
                                a3_price=pay_b2cwyzfstart;

                    $("#lblPrice3_sxf").html(parseFloat(a3_price).toFixed(2)); 

                    
                    $("#sumje").html((parseFloat(a3zfje) + parseFloat(a3_price)).toFixed(2));  
                    
                    
                }
                else
                {
                     $("#divbank3").hide();
                     $("#divbank1").hide();
                     $("#divbank2").show();
                     $("#hid_PayType").val("12");     
                     <%--B2B支付手续费  --%>               
                     $("#lblPrice3_sxf").html(parseFloat($("#pay_b2bwyzf").val()).toFixed(2));


                     <%--支付手续费  -start --%>
                     
                     var txtPrice = $("#lblPrice").html();<%-- 本次支付金额--%>
                     
                     a3zfje  =parseFloat(txtPrice);  
                     $("#sumje").html((parseFloat(a3zfje) + parseFloat($("#pay_b2bwyzf").val())).toFixed(2)); 

                     

                     <%--支付手续费  -end  --%>  
                }
            });

            $(".cancel").click(function () {<%--快捷支付，检验验证码，关闭弹出框 --%>
                $(".tip").fadeOut(100);
                $(".opacity").fadeOut(100);
            });

            $("#otherBank").click(function (e) {<%--快捷支付绑定，选择银行卡，更多“银行按钮” --%>
                e.stopPropagation();<%--控制更多银行按钮，不做赋值操作。 --%>
                $(this).parent().hide();<%--隐藏按钮 --%>
                $(".otherBank").show();<%--展示更多银行信息 --%>
            });
        })

        var stopid;
        function time(own, wait, action) {<%--快捷支付绑定，计时器 --%>
            if (wait > 0) {
                $(own).html("重新获取验证码(" + wait + ")");
                wait--;
                stopid = setTimeout(function () { time(own, wait, action) }, 1000);
            } else {
                clearTimeout(stopid);<%--清空stopid值，停止技术器 --%>
                $("#msg").html("");
                $(own).html("获取验证码");
                $("#submit2531").removeAttr("style");
                $(own).unbind().one("click", function () { onSubmit(own, action) });<%--重新注册获取验证码按钮事件 --%>
                $("#isshow").hide();<%--测试接口密码显示隐藏 --%>
            }
        }
        function closeTime(a)<%--快捷支付绑定，关闭计时器 --%>
        {
            var falg = arguments[0] ? arguments[0] : 0; 
            clearTimeout(stopid);<%--停止计时器 --%>
            $("#msg").html("");<%-- 清空错误信息文本--%>
            $("#submit2531").html("获取验证码");<%-- 修改获取验证码按钮text值--%>
            $("#submit2531").removeAttr("style");<%--删除获取验证码按钮的颜色，恢复原本颜色 --%>
            if(falg==0)
                $("#submit2531").unbind().one("click", function () { onSubmit($("#submit2531"), "2531") });<%--重新绑定“获取验证码”按钮事件 --%>
            $("#isshow").hide();<%--测试用密码隐藏 --%>
            $(".addBank .bankCard2 li").removeClass("border3"); <%--删除快捷支付绑定选择银行卡页面，所选择的银行卡 --%>
        }

        function onSubmit(own, action) {<%--快捷支付绑定银行卡提交数据 --%>
          
            var hidUserName = $("#hidUserName").val();<%--记录当前用户名称 --%>
            var hidBankid = $("#hidBankid").val();<%--选择银行对应的编码 --%>
            var txtBankCode = $("#txtBankCode").val();<%--银行卡号 --%>
            var hidBankCode = $("#hidBankCode").val();<%--银行卡号 --%>
            var txtUserName = $("#txtUserName").val();<%-- 姓名--%>
            var txtIDCard = $("#txtIDCard").val();<%--身份证 --%>
            var txtPhone = $("#txtPhone").val();<%--手机号码 --%>
            var hidBankLogo = $("#hidBankLogo").val();<%--银行logo --%>
            var txtPhoneCode = $("#txtPhoneCode").val();<%--手机验证码 --%>
            var hidFas = $("#hidFas").val();<%--快捷支付表ID  （机构代码+fastpay表id =中金快捷支付交易流水号） --%>
            var url = "";
            if (action == "2531") {<%--发送验证码，检验必填项 --%>
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
                $(own).css("color","rgb(162, 162, 162)");
                $(own).removeAttr("onclick");<%--删除获取验证码事件 --%>
            } else if (action == "2532") {<%-- 检验验证码，检验必填项--%>
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
                    if (action == "2531") {<%-- 发送验证码接口，返回数据--%>
                        if (data1.error == 1) {<%--失败 --%>
                            closeTime();<%--关闭计时器 --%>
                            $("#msg").html(data1.msg);<%--显示错误信息 --%>
                        } else {//成功
                            time(own, 120, action);<%--开启计时器 --%>
                            $("#hidFas").val(data1.id);<%--快捷支付绑定记录ID --%>
                            $("#isshow").show();<%--测试接口显示密码 --%>
                        }
                    } else {<%-- 检验验证码，返回数据--%>
                        closeTime();<%-- 关闭计时器--%>
                        if (data1.error != 1) {<%--成功 --%>
                            $("#msg").html("");<%--清空错误提示 --%>
                            $('.cd-popup').click();<%--关闭输入验证码弹出框 --%>
                            refFastBank();<%--刷新快捷支付列表 --%>
                        } else {<%--失败 --%>
                            $("#txtPhoneCode").val("");<%--清空验证 --%>
                            $("#hidFas").val("");<%--清空快捷支付绑定记录ID --%>
                            $("#msg").html(data1.msg);<%--显示错误提示 --%>
                        }
                    }
                }
            });
        }

        function refFastBank() {<%--更新快捷支付 --%>
            $.ajax({
                type: 'POST',
                url: "../../Handler/RefFastBank.ashx",
                data: { user:0 },
                success: function (data) {
                    var data1 = jQuery.parseJSON(data);
                    var html = "";
                    for (var i = 0; i < data1.length; i++) {
                        html += '<li><span><img src="' + data1[i].BankLogo + '" /></span><span style=" position:absolute; left:300px;"><i class="quick">快捷</i><i>储蓄卡</i>|<i>尾号**' + data1[i].bankcode.slice(-4) + '</i>|<i>手机号' + data1[i].phone.substr(0, 3) + '****' + data1[i].phone.slice(-4) + '</i></span><input type="hidden" class="hidFastID" value="' + data1[i].ID + '" /></li>';
                    }
                    $("#a2 ul.bankCard").html(html);

                     $("#checkId .li .bankCard li").click();
                }
            });
        }


        function check() {<%-- "立即支付"按钮--%>
            if ($("#a3").is("[class*='center']")) {<%--网银支付，必须选择一家银行 --%>
                if (!$("#a3").children("div").children("ul").children("li").is("[class*='border2']")) {
                    layerCommon.alert("请选择银行！",IconOption.哭脸);
                    return false;
                }
            }
            if ($("#a2").is("[class*='center']")) {<%--快捷支付，必须选择一个快捷支付 --%>
                if (!$("#a2").children("ul").children("li").is("[class*='border']")) {
                    layerCommon.alert("请绑定快捷支付银行卡！",IconOption.哭脸);
                    return false;
                }
            }
            $("#lblErr").html("");
        }
        function msgTime(wait) {<%-- 快捷支付发送验证码，计时器--%>
            if (wait > 0) {
                $("#msgone").html(wait + "秒后短信验证码失效");
                wait--;
                setTimeout(function () { msgTime(wait) }, 1000);
            } else {
                $("#txtPhoneNum").attr("disabled", "disabled");
                $("#msgone").hide();
                $("#msgtwo").show();
            }
        }
        function checkCode() {<%-- 检验快捷支付验证码必填--%>
            if ($("#txtPhoneNum").val() == "") {
                layerCommon.alert("请填写验证码",IconOption.哭脸);
                return false;
            }
            return true;
        }
        function BankCodeBlur(own) {<%--输入银行卡号，自动对应银行 --%>
            $.ajax({
                type: 'POST',
                url: "../../Handler/BankCodeBlur.ashx",
                data: { BankCode: $(own).val() },
                success: function (data) {
                    if (data != "") {
                        var result = JSON.parse(data);
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
        .tip{position:absolute;top:50%; margin-top:-140px;left:50%; margin-left:-225px;background: #fff;z-index: 999;border:5px solid #888;overflow:hidden; width:450px; height:280px; display:none;}
        .tiptop{line-height: 40px;font-size: 16px; color:#fff;padding-left: 10px; position:relative; background:#4c5b78;}
		.tipinfo{ padding-top:20px;}
        .tipinfo .lb{padding-top:10px;padding-left: 10px; font-size:14px; line-height:25px;}
		.tipinfo .title{ display:inline-block; width:100%; text-align:center; color:#606a74; font-size:24px;}
		.tipinfo .tel{ color:#999; font-size:12px; text-align:center; display:block; padding-top:5px;}
		.tipinfo .box2{ border:1px solid #c3c3c3; height:48px; line-height:48px; width:290px; border-radius:3px; margin-left:75px;box-shadow:2px 2px 1px 0px #eee inset; font-size:30px;letter-spacing:27px;text-indent:25px;}
		.tipinfo .txt{ color:#da4444; text-align:center; padding:9px 0; font-size:12px; display:block;}
		.tipinfo .txt2{ border-top:1px solid #d1d1d1; margin:0px 20px; font-size:12px;}
		.tipinfo .txt2 span{ width:260px; display:inline-block; text-align:center;}
		.tipinfo .txt2 .close2{ margin-left:60px;}
		.tipinfo .btn{filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffff', endColorstr='#fafafa'); background: -webkit-linear-gradient(top, #ffffff, #fafafa);background: -moz-linear-gradient(top, #ffffff, #fafafa);background: -ms-linear-gradient(top, #ffffff, #fafafa);background: linear-gradient(top, #ffffff, #fafafa); border:1px solid #ddd; border-radius:5px; display:inline-block; line-height:26px; padding:0px 10px;}		
		.tipinfo .btn:hover{  filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#f57403', endColorstr='#f57403');  background: #f57403;border: 1px solid #d66500; color:#fff; text-decoration:none;}
		.tiptop .close{background: url(../images/icon.png) no-repeat 0px -119px; width:14px; height:14px; cursor:pointer; display:inline-block; position:absolute; top:12px; right:12px; border:none; padding:0;}
	    .payOpt .li{ margin-top:2px;}
        .opacity{position: absolute;top: 0%;left: 0%;width: 100%;height: 2000px;background-color: #000;opacity: 0.3;z-index: 998;filter:alpha(opacity=30)}
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
        <%--header start--%>
        <div class="header"><div class="con">
	        <div class="logo"><a href="../../index.aspx"><img src="../../images/logo2.0.png" height="37" /></a><i>支付平台</i></div>
            <div class="topMenu"><a href="../UserIndex.aspx">我的桌面</a>|<a href="../../index.aspx">医站通首页</a> &nbsp;
            <% if (ConfigurationManager.AppSettings["OrgCode"] == "SYJ")
               {%>
            <i style=" font-weight:bold; color:red;">服务热线：400-8859-319</i>
            <% } %>
            </div>
        </div></div>
       <%-- header end--%>

       <%--隐藏域 start --%>
        <input type="hidden" id="hidPay" runat="server" value="0" />
        <input type="hidden" id="hidPrepay" runat="server" value="0" />
        <input type="hidden" id="hidUserName" name="hidUserName" value="<%=this.UserName %>" />

          <%--支付手续费设置，新增隐藏域字段 start--%>
    <input type="hidden" id="hid_PayType" runat="server" value="11" />

    <input type="hidden" id="pay_sxfsq" runat="server" value="0.00" />
    <input type="hidden" id="pay_zffs" runat="server" value="0.00" />
    <input type="hidden" id="pay_kjzfbl" runat="server" value="0.00" />
    <input type="hidden" id="pay_kjzfstart" runat="server" value="0.00" />
    <input type="hidden" id="pay_kjzfend" runat="server" value="0.00" />
    <input type="hidden" id="pay_ylzfbl" runat="server" value="0.00" />
    <input type="hidden" id="pay_ylzfstart" runat="server" value="0.00" />
    <input type="hidden" id="pay_ylzfend" runat="server" value="0.00" />
    <input type="hidden" id="pay_b2cwyzfbl" runat="server" value="0.00" />
    <input type="hidden" id="pay_b2cwyzfstart" runat="server" value="0.00" />
    <input type="hidden" id="pay_b2bwyzf" runat="server" value="0.00" />
    <input type="hidden" id="Pay_mfcs" runat="server" value="0.00" />
     <%--支付手续费设置，新增隐藏域字段 end--%>
        <%--隐藏域  end  --%>

        <%--支付信息 start--%>
        <div class="payInfo">
            <div class="number">
                流水号：<a href="javascript:void(0)"><label id="lblOrderNO" runat="server"></label></a></div>
            <div class="gray">交易记录已生成，请及时支付。</div>
            <div class="amount">
                <b style="font-size:18px;color:Red; font-weight:normal;">￥<label id="lblPrice" runat="server"></label></b>  元</div>
        </div>
       <%-- 支付信息 end--%>

        <div class="payOpt" id="checkId">
            <input type="hidden" id="hidKeyID" name="hidKeyID" runat="server" value="0" />
            <%--快捷支付 start--%>
            <div class="li" id="a2">
                <input type="hidden" name="hida2" id="hida2" value="" runat="server" />
                <input type="hidden" name="hidFastPay" id="hidFastPay" runat="server" />
                <a href="javascript:void(0)" class="check cur"></a>
                <div class="title">快捷支付</div>
                <ul class="bankCard">
                    <asp:Repeater runat="server" ID="rptQpay">
                        <ItemTemplate>
                            <li><span><img src="<%# Eval("BankLogo") %>" /></span><span style=" position:absolute; left:300px;"><i class="quick">快捷</i><i>储蓄卡</i>|<i>尾号**<%# Eval("bankcode").ToString().Substring(Eval("bankcode").ToString().Length-4,4) %></i>|<i>手机号<%# Eval("phone").ToString().Substring(0,3) %>****<%# Eval("phone").ToString().Substring(Eval("phone").ToString().Length-4,4) %></i></span>
                            <input type="hidden" class="hidFastID" value="<%# Eval("ID") %>" /></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <div class="addCard"><a href="javascript:void(0)"  class="time cd-popup-trigger">添加银行卡</a></div>
                <div class="amount" id="div_kjzfsxf" runat="server"> <%-- 支付<b><label id="lblPrice1" runat="server"></label></b>元--%>
                手续费<b><label id="lblPrice2_sxf" runat="server">0.00</label></b>元
                </div>
                <input type="hidden" id="hidYorN2" value="0" />
            </div>
 	       <%-- 快捷支付 end--%>
    
           <%-- 网银支付 start--%>
  	        <div class="li" id="a3">
                <input type="hidden" name="hida3" id="hida3" value="" runat="server" />
                <input type="hidden" name="hidBank" id="hidBank" runat="server" />
    	        <a href="javascript:void(0)" class="check cur"></a>
                <div class="title">网银支付</div>
                <div class="amOpt">
                &nbsp;&nbsp;账户类型：
                    <input type="radio" name="AccountType" value="11" id="type11" class="AccountType" runat="server" checked="true" /><label class="AccountType" for="type11">个人账户（借记卡）</label>&nbsp;&nbsp;&nbsp;&nbsp;
                <input type="radio" name="AccountType" style=" display:none;" value="13" id="type13" class="AccountType" runat="server"  /> <%--<label class="AccountType" for="type13">个人账户（贷记卡)</label>&nbsp;&nbsp;&nbsp;&nbsp;--%>
                
                 <%--<i style="color: #2ea7e7;">(暂不支持信用卡)</i>&nbsp;&nbsp;--%><input type="radio" name="AccountType" value="12" id="type12" class="AccountType" runat="server" /><label class="AccountType" for="type12">企业账户</label>
                </div>
                 <div class="amount" id="div_wyzfsxf" runat="server">
               <%-- 支付<b><label id="lblPrice3" runat="server">0.00</label></b>元
                &nbsp;&nbsp;|&nbsp;&nbsp;--%>
                手续费<b><label id="lblPrice3_sxf" runat="server">0.00</label></b>元
                </div>
                <div id="divbank1" class="divbank"> <%--个人账户显示银行 --%> 
                       <ul class="bankCard2">
                       <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国工商银行.jpg" /><input
                        type="hidden" class="hidBank" value="102" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>邮储银行.jpg" /><input
                        type="hidden" class="hidBank" value="100" /></a></li>
                
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国农业银行.jpg" /><input
                        type="hidden" class="hidBank" value="103" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国银行.jpg" /><input
                        type="hidden" class="hidBank" value="104" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国建设银行.jpg" /><input
                        type="hidden" class="hidBank" value="105" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>交通银行.jpg" /><input
                        type="hidden" class="hidBank" value="301" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国光大银行.jpg" /><input
                        type="hidden" class="hidBank" value="303" /></a></li>  
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>华夏银行.jpg" /><input
                        type="hidden" class="hidBank" value="304" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国民生银行.jpg" /><input
                        type="hidden" class="hidBank" value="305" /></a></li>            
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>招商银行.jpg" /><input
                        type="hidden" class="hidBank" value="308" /></a></li>                
               <%-- <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>浦发银行.jpg" /><input
                        type="hidden" class="hidBank" value="310" /></a></li>--%>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>徽商银行.jpg" /><input
                        type="hidden" class="hidBank" value="440" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>上海银行.jpg" /><input
                        type="hidden" class="hidBank" value="401" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中信银行.jpg" /><input
                        type="hidden" class="hidBank" value="302" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>宁波银行.jpg" /><input
                        type="hidden" class="hidBank" value="408" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>青岛银行.jpg" /><input
                        type="hidden" class="hidBank" value="450" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>兰州银行.jpg" /><input
                        type="hidden" class="hidBank" value="447" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>平安银行.jpg" /><input
                        type="hidden" class="hidBank" value="307" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>北京银行.jpg" /><input
                        type="hidden" class="hidBank" value="403" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>杭州银行.jpg" /><input
                        type="hidden" class="hidBank" value="423" /></a></li>
                        <%--2016-11-03  最新更新  begin --%>                       
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>天津银行.jpg" /><input
                        type="hidden" class="hidBank" value="434" /></a></li>
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>颖淮农村商业银行.jpg" /><input
                        type="hidden" class="hidBank" value="423" /></a></li>
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>浙商银行.jpg" /><input
                        type="hidden" class="hidBank" value="316" /></a></li>
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>广州农商银行.jpg" /><input
                        type="hidden" class="hidBank" value="1405" /></a></li>
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>兴业银行.jpg" /><input
                        type="hidden" class="hidBank" value="309" /></a></li>
                           <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>广发银行.jpg" /><input
                        type="hidden" class="hidBank" value="306" /></a></li>
                        <%--2016-11-03  最新更新  end --%>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>银联在线.jpg" /><input
                        type="hidden" class="hidBank" value="888" /></a></li>
            </ul>
               </div>
                <div id="divbank3" class="divbank"  style="display:none"> <%--个人账户贷记卡--%> 
                       <ul class="bankCard2">
                 <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国工商银行.jpg" /><input
                        type="hidden" class="hidBank" value="102" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>邮储银行.jpg" /><input
                        type="hidden" class="hidBank" value="100" /></a></li>                
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国农业银行.jpg" /><input
                        type="hidden" class="hidBank" value="103" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国银行.jpg" /><input
                        type="hidden" class="hidBank" value="104" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国建设银行.jpg" /><input
                        type="hidden" class="hidBank" value="105" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>交通银行.jpg" /><input
                        type="hidden" class="hidBank" value="301" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国光大银行.jpg" /><input
                        type="hidden" class="hidBank" value="303" /></a></li> 
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>平安银行.jpg" /><input
                        type="hidden" class="hidBank" value="307" /></a></li>                
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国民生银行.jpg" /><input
                        type="hidden" class="hidBank" value="305" /></a></li>            
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>招商银行.jpg" /><input
                        type="hidden" class="hidBank" value="308" /></a></li>                
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>浦发银行.jpg" /><input
                        type="hidden" class="hidBank" value="310" /></a></li>                
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>上海银行.jpg" /><input
                        type="hidden" class="hidBank" value="401" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中信银行.jpg" /><input
                        type="hidden" class="hidBank" value="302" /></a></li>                
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>北京银行.jpg" /><input
                        type="hidden" class="hidBank" value="403" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>杭州银行.jpg" /><input
                        type="hidden" class="hidBank" value="423" /></a></li> 
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>天津银行.jpg" /><input
                        type="hidden" class="hidBank" value="434" /></a></li>                       
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>兴业银行.jpg" /><input
                        type="hidden" class="hidBank" value="309" /></a></li>
                           <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>广发银行.jpg" /><input
                        type="hidden" class="hidBank" value="306" /></a></li>

                       <%-- 新增信用卡银行  2016-11-06  begin--%>
                    <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>成都农商银行.jpg" />
                        <input type="hidden" class="hidBank" value="1528" /></a></li>
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>成都银行.jpg" />
                        <input type="hidden" class="hidBank" value="429" /></a></li>
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>东莞农村商业银行.jpg" />
                        <input type="hidden" class="hidBank" value="1415" /></a></li>
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>法兴银行（中国）.jpg" />
                        <input type="hidden" class="hidBank" value="3036" /></a></li>
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>赣州银行.jpg" />
                        <input type="hidden" class="hidBank" value="463" /></a></li>
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>广州银行.jpg" />
                        <input type="hidden" class="hidBank" value="413" /></a></li>
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>哈尔滨银行.jpg" />
                        <input type="hidden" class="hidBank" value="442" /></a></li>
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>河北银行.jpg" />
                        <input type="hidden" class="hidBank" value="422" /></a></li>
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>湖南农村信用社.jpg" />
                        <input type="hidden" class="hidBank" value="1438" /></a></li>
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>江苏银行.jpg" />
                        <input type="hidden" class="hidBank" value="1501" /></a></li>
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>莱商银行.jpg" />
                        <input type="hidden" class="hidBank" value="497" /></a></li>
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>六盘水市商业银行.jpg" />
                        <input type="hidden" class="hidBank" value="500" /></a></li>
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>齐商银行.jpg" />
                        <input type="hidden" class="hidBank" value="438" /></a></li>
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>上饶银行.jpg" />
                        <input type="hidden" class="hidBank" value="1508" /></a></li>
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>盛京银行.jpg" />
                        <input type="hidden" class="hidBank" value="417" /></a></li>
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>威海市商业银行.jpg" />
                        <input type="hidden" class="hidBank" value="481" /></a></li>
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>潍坊银行.jpg" />
                        <input type="hidden" class="hidBank" value="462" /></a></li>
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>新疆乌鲁木齐市商业银行.jpg" />
                        <input type="hidden" class="hidBank" value="427" /></a></li>
                     <%--
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>银联在线.jpg" /><input
                        type="hidden" class="hidBank" value="888" /></a></li>--%>
            </ul>
               </div>
                <div id="divbank2" class="divbank" style="display:none"><%--企业账户显示银行 --%> 
                      <ul class="bankCard2">
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国工商银行.jpg" /><input
                        type="hidden" class="hidBank" value="102" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国农业银行.jpg" /><input
                        type="hidden" class="hidBank" value="103" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国银行.jpg" /><input
                        type="hidden" class="hidBank" value="104" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国建设银行.jpg" /><input
                        type="hidden" class="hidBank" value="105" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>交通银行.jpg" /><input
                        type="hidden" class="hidBank" value="301" /></a></li>               
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国光大银行.jpg" /><input
                        type="hidden" class="hidBank" value="303" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>华夏银行.jpg" /><input
                        type="hidden" class="hidBank" value="304" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国民生银行.jpg" /><input
                        type="hidden" class="hidBank" value="305" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>平安银行.jpg" /><input
                        type="hidden" class="hidBank" value="307" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>招商银行.jpg" /><input
                        type="hidden" class="hidBank" value="308" /></a></li>

                 <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>浦发银行.jpg" /><input
                        type="hidden" class="hidBank" value="310" /></a></li>

                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>徽商银行.jpg" /><input
                        type="hidden" class="hidBank" value="440" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中信银行.jpg" /><input
                        type="hidden" class="hidBank" value="302" /></a></li>
                        
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>兴业银行.jpg" /><input
                        type="hidden" class="hidBank" value="309" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>广发银行.jpg" /><input
                        type="hidden" class="hidBank" value="306" /></a></li>
              
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>天津银行.jpg" /><input
                        type="hidden" class="hidBank" value="434" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>宁波银行.jpg" /><input
                        type="hidden" class="hidBank" value="408" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>北京银行.jpg" /><input
                        type="hidden" class="hidBank" value="403" /></a></li>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>杭州银行.jpg" /><input
                        type="hidden" class="hidBank" value="423" /></a></li>
                          <%--   2016-11-03 新增银行 begin--%>
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>青岛银行.jpg" /><input
                        type="hidden" class="hidBank" value="450" /></a></li>
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>邮储银行.jpg" /><input
                        type="hidden" class="hidBank" value="100" /></a></li>
                           <%--2016-11-03  最新更新  end --%>
            </ul>
            
            </div>
                <input type="hidden" id="hidYorN3" value="0" />
            </div>
           <%-- 网银支付 end--%>

                <%--其他平台支付 start--%>
                
            <input type="hidden" name="hida5" id="hida5" runat="server" value="" />
            <input type="hidden" name="hidWxorAplipay" id="hidWxorAplipay" runat="server" />
        <%if (Common.GetPayWxandAli(this.CompID).ali_isno == "1" || Common.GetPayWxandAli(this.CompID).wx_Isno == "1")
          {%>
        <div class="li" id="a5">
            <a href="javascript:void(0)" class="check cur"></a>
            <div class="title">其他支付</div>
            <div class="amount">手续费<b><label id="lblPrice5">0.00</label></b>元<%--<div id="div_wxzfsxf">　|　手续费<b><label id="lblPrice5_sxf">0.00</label></b>元</div>--%></div>
           
            <div class="platformPay">
            <% if (Common.GetPayWxandAli(this.CompID).ali_isno == "1"){ %>
            	<a href="javascript:void(0)"><i class="zfb-i"></i>支付宝支付<input type="hidden" class="abcd" value="zfb" /></a>
                <%}%>
               <%if (Common.GetPayWxandAli(this.CompID).wx_Isno == "1"){ %>
                <a href="javascript:void(0)"><i class="wx-i"></i>微信支付<input type="hidden" class="abcd" value="wx" /></a>
                <%} %>
            </div>
             <div class="blank20"></div><div class="blank20"></div>
          </div>  
          <%} %>
         <%--其他平台支付 end--%>
            <div class="payBtn">
                <asp:Button ID="btnPay" runat="server" CssClass="btn  right" Text="立即支付" OnClick="btnPay_Click" OnClientClick="return check()" />
                 <div class="right" runat="server" id="div_sumprice"><div class="txt">总金额(含手续费)：<b ><label id="sumje" class="red"  style="font-size:18px; color:#ff4e02;font-weight:normal" runat="server"></label></b>元<%--<br />未付款金额：<b class="red">300.00</b>元--%></div></div>
           <div class="blank10"></div>
                <i class="red tis"><label id="lblErr"></label></i>
            </div>
        </div>


        <%--快捷支付弹窗 start--%>
        <div class="cd-popup" id="addBank">
	        <div class="cd-popupBg"></div>
            <div class="hidebg"></div>
            <%--添加银行卡 start--%>
	        <div class="cd-popup-container addBank a5 noOff" >
	        <div class="title"><b>添加银行卡</b>&nbsp;<i style="color:#2ea7e7; font-size:12px;">(暂不支持信用卡绑定)</i></div>
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
            <div class="payBtn"><a href="javascript:void(0)" class="btn btnId">下一步</a></div>
            <a href="javascript:void(0)" class="close cd-popup-close img-replace" onclick="closeTime(1)">关闭</a>
            </div>
            <%--添加银行卡 end--%>
    
            <%--添加银行卡 start--%>
	        <div class="cd-popup-container addBank a6">
	        <div class="title"><b>添加银行卡 </b>&nbsp;<i style=" color:#2ea7e7; font-size:12px;">(暂不支持信用卡绑定)</i></div>
                <ul class="addCardNr">
                <li><i class="bt">银行卡</i><input id="txtBankCode" name="txtBankCode" type="text" class="box2" onkeyup="this.value=this.value.replace(/\D/g,'')" onblur="BankCodeBlur(this)"/></li>
    	        <li><i class="bt">付款银行</i><div class="card"><span><img id="imgBankImg" src="../images/payBank.jpg"></span><input type="hidden" id="hidBankid" name="hidBankID" /><input type="hidden" id="hidBankLogo" name="hidBankLogo" /></div><a href="javascript:void(0)" class="return decoration"><i class="icon"></i>选择其他银行</a></li>
                <li><i class="bt">姓名</i><input id="txtUserName" name="txtUserName" type="text" class="box3"/></li>
                <li><i class="bt">身份证</i><input id="txtIDCard" name="txtIDCard" type="text" class="box2" /></li>
                <li><i class="bt">手机号</i><input id="txtPhone" name="txtPhone" type="text" class="box3" onkeyup="this.value=this.value.replace(/\D/g,'')" value=""/><a href="javascript:void(0)" id="submit2531" class="btn2 decoration" onclick="onSubmit(this,'2531')">获取验证码</a></li>
                <li>
                    <i class="bt">手机验证码</i><input id="txtPhoneCode" name="txtPhoneCode" type="text" class="box3" style=" width:50px;" value="" onkeyup="this.value=this.value.replace(/\D/g,'')"/>
                    <input type="hidden" id="hidFas" />&nbsp;&nbsp;<label id="msg"  style="color:#da4444;" ></label>

                      <% if (ConfigurationManager.AppSettings["Paytest_zj"] == "1")
                              { %>
                               <i id="isshow" style="display:none;">  <b class="red">（测试密码:123456）</b></i>
                                 <% }%>
                </li>
       	        </ul>
            <div class="payBtn payBtn2"><a href="javascript:void(0)" class="btn" onclick="onSubmit(this,'2532')">确认绑定</a></div>
            <a href="javascript:void(0)" class="close cd-popup-close img-replace" onclick="closeTime(1)">关闭</a>
            </div>
            <%--添加银行卡 end --%>
     
        </div> 
        <%--快捷支付弹窗 end--%>

        <%--弹出遮罩层 Begin--%>
        <div class="opacity" style="display:none;"></div>
       <%--弹出遮罩层 End--%>
       <%-- 弹出录入层 Begin--%>
        <div class="tip">
            <div class="tiptop"><span>快捷支付</span><input name="" type="button" class="cancel close" value="" /></div>
            <div class="tipinfo">
        	    <div class="lb"><span class="title">请输入短信验证码</span><i class="tel" runat="server" id="phone">已发送至188****2156</i></div>
                <div class="lb">
                    <input id="txtPhoneNum" class="box2" type="text" runat="server" autocomplete="off" />
                    <span style="display:none; margin-left:195px;" id="paying">正在支付...</span>
                    <asp:Button ID="btnTx1376" style="display:none;" runat="server" Text="确认" OnClick="btnTx1376_Click" OnClientClick="return checkCode();" />
                </div>
                <div class="lb txt"><span>短信验证码已发送，请注意查收</span>
                 <% if (ConfigurationManager.AppSettings["Paytest_zj"] == "1")
                              { %>
                                 <b class="red">（测试密码:123456）</b>
                                 <% }%>
                </div>
			    <div class="lb txt2">
                    <span>
                        <%--<a href="" class="btn" style="display:none;">重新获取手机短信验证码</a>--%>
                        <i class="" id="msgone">120秒后短信验证码失效</i> 
                        <i class="" id="msgtwo">验证码已失效，请关闭窗口，重新支付！</i> 
                    </span>
                    |<a href="javascript:void(0)" class="cancel close2">关闭</a>
                </div>	
		    </div>
        </div>
        <%--弹出录入层 End--%>
        
		<%--<!--网上支付提示 start-->
        <div class="addBank" style="display:block; height:225px;">
			<div class="title"><b>网上支付提示 </b></div>
            <div class="payTis"><span class="pic"><img src="../images/payTis.gif" width="200" /></span><i>支付完成前，请不要关闭支付验证窗口<br />支付完成后，请根据您支付的情况点击下面按钮</i><a href="" class="bluBtn">支付遇到问题</a><a href="" class="redBtn">成功完成支付</a></div>
		</div>
        <!--网上支付提示 end-->--%>
     
     
        <div class="blank20"></div>
       <%-- 常见问题 start--%>
        <%--<div class="question">
	        <div class="title">常见问题</div>
            <dl class="list">
    	        <dt>1、经常需要动态给Div添加或者删除样式，利用jquery的addClass(id)与delete</dt>
                <dd>在开发网页过程中常需要对A链接使用JS制作一个点击弹窗,那么在点击该A链接就不能发生跳转行为,于是我们取消A链接的跳转</dd>
                <dt>1、经常需要动态给Div添加或者删除样式，利用jquery的addClass(id)与delete</dt>
                <dd>在开发网页过程中常需要对A链接使用JS制作一个点击弹窗,那么在点击该A链接就不能发生跳转行为,于是我们取消A链接的跳转</dd>
                <dt>1、经常需要动态给Div添加或者删除样式，利用jquery的addClass(id)与delete</dt>
                <dd>在开发网页过程中常需要对A链接使用JS制作一个点击弹窗,那么在点击该A链接就不能发生跳转行为,于是我们取消A链接的跳转</dd>
            </dl>
        </div>--%>
        <%--常见问题 end--%>
        <div class="footer"> <%= ConfigurationManager.AppSettings["CompanyName"].ToString() %></div>
    </form>
</body>
</html>

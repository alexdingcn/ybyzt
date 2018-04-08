<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pay.aspx.cs" Inherits="Distributor_Pay_Pay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>
        <%= ConfigurationManager.AppSettings["PhoneSendName"].ToString()%>支付平台</title>
    <link href="../../Distributor/css/pay.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/pay.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script type="text/javascript">        $(function () {
            if ($(window).innerWidth() <= 1024) { $("body").append('<link href="<%= ResolveUrl("../css/root.css")%>" rel="stylesheet" type="text/css" />'); }
        })
    </script>
    <script type="text/javascript">
        function readyPay(){
            //window.top.opener.readyPay();
        }

        function change() {
            this.value = this.value.replace(/\D/g, '').replace(/....(?!$)/g, '$& '); <%--替换空格前4位数字为4位数字加空格 --%>    
        }
        var stop;
        $(document).ready(function () {
            $("#btnPay").one("click", check); <%-- 支付按钮触发 --%>

            <%-- 融资申请弹出框，“申请借款成功”按钮--%>
            $("#btnS").click(function () {   
                $("#btnSuccess").click();
            });

            $("#btnF").click(function () { <%--融资申请弹出框，“借款遇到问题”按钮 --%>
                $("#btnFailure").click();
            });

             $("#btnPayS").click(function () { <%-- 网银支付弹出框，“已经完成支付”按钮 --%>
                $("#btnPaySuccess").click();
            });

            $("#btnPayF").click(function () {  <%-- 网银支付弹出框，“支付遇到问题”按钮 --%>
                $("#btnPayFailure").click();
            });

            $("#txtBankCode").keyup(function () { <%-- 银行卡号格式 1111 2222 3333 4444 --%>
                $(this).val($(this).val().replace(/\D/g, '').replace(/....(?!$)/g, '$& '));
            });

            $("#txtPhoneNum").keyup(function () { <%-- 快捷支付，检验验证码，输入6位数字后提交数据 --%>
                if ($(this).val().length == 6 || $(this).val().length > 6) {
                    clearTimeout(stop);  <%-- 清空倒计时 --%>
                    $("#btnTx1376").click();  <%-- 检验快捷支付验证码必填 --%>
                    $("#txtPhoneNum").hide();<%-- 隐藏输入密码的文本框 --%>
                    $("#paying").show(); <%-- 正在支付文字显示 --%>
                }
            });

            $("#otherBank").click(function (e) { <%-- 快捷支付绑定，选择银行卡，更多“银行按钮” --%>
                e.stopPropagation(); <%-- 控制更多银行按钮，不做赋值操作 --%>
                $(this).parent().hide(); <%-- 隐藏按钮 --%>
                $(".otherBank").show(); <%-- 展示更多银行信息 --%>
            });

            $(".cancel").click(function () {  <%-- 快捷支付，检验验证码，关闭弹出框 --%>
                $(".tip").fadeOut(100); <%-- 输入验证码弹出层隐藏 --%>
                $(".opacity").fadeOut(100);<%--遮罩层隐藏 --%>
                clearTimeout(stop); <%--  关闭计时器 --%>
                $("#btnPay").one("click", check); <%-- 重新绑定“立即支付”按钮方法 --%>
            });


            $(".AccountType").click(function (e) {<%--网银支付，选择账户类型时，取消冒泡事件 --%>
                e.stopPropagation();
                var abc=$(this).attr("id");
                if(abc=="type11")
                {
                   
                    $("#divbank1").show();
                    $("#divbank2").hide();
                     $("#divbank3").hide();
                    $("#hid_PayType").val("11");

                    
                    <%-- 支付手续费 -start  --%>
                    var a3_price = 0.00;
                     var a3zfje=0.00;
                     
                     var txtPrice = $("#txtPrice").val();<%-- 企业钱包金额 --%>
                     var txtPayOrder = $("#txtPayOrder").val();<%-- 本次支付金额 --%>

                     var hida1 = $("#hida1").val(); <%-- 是否使用企业钱包 --%>
                     if(hida1=="1")
                             a3zfje  =parseFloat(txtPayOrder)-parseFloat(txtPrice);
                     else
                             a3zfje=parseFloat(txtPayOrder);
                     
                     var bankCode = $("#hidBank").val() == "" ? "0" : $("#hidBank").val();
                      
                        a3_price = (a3zfje * parseFloat($("#pay_b2cwyzfbl").val())).toFixed(3);
                        <%-- 封底 --%>
                        var pay_b2cwyzfstart=parseFloat($("#pay_b2cwyzfstart").val());
                        if(a3_price<pay_b2cwyzfstart )
                                a3_price=pay_b2cwyzfstart;
                         

                    $("#lblPrice3_sxf").html(parseFloat(a3_price).toFixed(2)); 
                    $("#sumje").html((parseFloat(a3zfje) + parseFloat(a3_price)).toFixed(2));  
                    
                    
                     <%--隐藏支付总金额文本框 --%>
                     if(parseFloat(a3zfje)==0)
                      $("#sum_payprice").hide();
                      else
                      $("#sum_payprice").show();
                                        
                       <%--支付手续费 -end --%> 
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
                     
                     var txtPrice = $("#txtPrice").val();<%-- 企业钱包金额 --%>
                     var txtPayOrder = $("#txtPayOrder").val();<%-- 本次支付金额 --%>

                     var hida1 = $("#hida1").val(); <%-- 是否使用企业钱包 --%>
                     if(hida1=="1")
                             a3zfje  =parseFloat(txtPayOrder)-parseFloat(txtPrice);
                     else
                             a3zfje=parseFloat(txtPayOrder);
                     
                     var bankCode = $("#hidBank").val() == "" ? "0" : $("#hidBank").val();
                     
                     <%--网银支付 --%>
                        a3_price = (a3zfje * parseFloat($("#pay_b2cwyzfbl").val())).toFixed(3);
                        <%-- 封底 --%>
                        var pay_b2cwyzfstart=parseFloat($("#pay_b2cwyzfstart").val());
                        if(a3_price<pay_b2cwyzfstart )
                                a3_price=pay_b2cwyzfstart;

                    $("#lblPrice3_sxf").html(parseFloat(a3_price).toFixed(2)); 

                    
                    $("#sumje").html((parseFloat(a3zfje) + parseFloat(a3_price)).toFixed(2));  
                    
                    
                     <%--隐藏支付总金额文本框 --%>
                     if(parseFloat(a3zfje)==0)
                      $("#sum_payprice").hide();
                      else
                      $("#sum_payprice").show();
                                        
                       <%--支付手续费 -end --%> 
                }
                else
                {
                     $("#divbank1").hide();
                     $("#divbank3").hide();
                     $("#divbank2").show();
                     $("#hid_PayType").val("12");     
                     <%-- B2B支付手续费  --%>              
                     $("#lblPrice3_sxf").html(parseFloat($("#pay_b2bwyzf").val()).toFixed(2));


                     <%-- 支付手续费  -start --%>
                     var txtPrice = $("#txtPrice").val(); <%-- 企业钱包金额 --%>
                     var txtPayOrder = $("#txtPayOrder").val(); <%-- 本次支付金额 --%>

                     var hida1 = $("#hida1").val();<%-- 是否使用企业钱包 --%>
                     if(hida1=="1")
                             a3zfje  =parseFloat(txtPayOrder)-parseFloat(txtPrice);
                     else
                             a3zfje=parseFloat(txtPayOrder);

                       

                     $("#sumje").html((parseFloat(a3zfje) + parseFloat($("#pay_b2bwyzf").val())).toFixed(2)); 

                     <%-- 隐藏支付总金额文本框 --%>
                     if(parseFloat(a3zfje)==0)
                      $("#sum_payprice").hide();
                      else
                      $("#sum_payprice").show();
                     

                     <%-- 支付手续费  -end   --%>
                }

            });
           
            <%-- 阻止文本框的enter事件 --%>
            $("#padPaypas").keypress(function(event) { 
                    if (event.keyCode == 13) {                       
                    return false;
                                     }    

                    });

        })

        var stopid;
        function time(own, wait, action) {<%--快捷支付绑定，计时器--%>
            if (wait > 0) {
                $(own).html("重新获取验证码(" + wait + ")");
                wait--;
                stopid = setTimeout(function () { time(own, wait, action) }, 1000);
            } else {
                clearTimeout(stopid);<%--清空stopid值，停止技术器--%>
                $("#msg").html("");
                $(own).html("获取验证码");
                $("#submit2531").removeAttr("style");
                $(own).unbind().click(function () { onSubmit(own, action) });<%--重新注册获取验证码按钮事件--%>
                $("#isshow").hide();<%--测试接口密码显示隐藏--%>

            }
        }

        <%--关闭按钮事件 --%>
        function closeTime(a)<%--快捷支付绑定，关闭计时器 --%>
        {
            var falg = arguments[0] ? arguments[0] : 0;<%--参数赋默认值 --%>
            clearTimeout(stopid);<%--停止计时器 --%>
            $("#msg").html("");<%--清空错误信息文本 --%>
            $("#submit2531").html("获取验证码");<%--修改获取验证码按钮text值 --%>
            $("#submit2531").removeAttr("style");<%--删除获取验证码按钮的颜色，恢复原本颜色 --%>
            if(falg==0)
                $("#submit2531").unbind().click(function () { onSubmit($("#submit2531"), "2531") });<%--重新绑定“获取验证码”按钮事件 --%>
            $("#isshow").hide();<%--测试用密码隐藏--%>
            $(".addBank .bankCard2 li").removeClass("border3"); <%--删除快捷支付绑定选择银行卡页面，所选择的银行卡 --%>
        }


        function priceKeyup(obj) { <%--格式化金额格式，只能是正数字 --%>
            //own.value = own.value.replace(/[^\d.]/g, '');
            var reg = /^[\d]+$/g;
            if (!reg.test(obj.value)) {
                var txt = obj.value;
                var i = 0;
                var arr = new Array();
                txt.replace(/[^\d.]/g, function (char, index, val) { <%-- 匹配第一次非数字字符 --%>
                    arr[i] = index;
                    i++;
                    obj.value = obj.value.replace(/[^\d.]/g, "");  <%--将非数字字符替换成"" --%>
                    var rtextRange = null;
                    if (obj.setSelectionRange) {
                        obj.setSelectionRange(arr[0], arr[0]);
                    } else { <%--支持ie --%>
                        rtextRange = obj.createTextRange();
                        rtextRange.moveStart('character', arr[0]);
                        rtextRange.collapse(true);
                        rtextRange.select();
                    }
                });
            }
        }

        <%-- 快捷支付发送验证码+检验验证码  2531+2532 --%>
        function onSubmit(own, action) { <%--快捷支付绑定银行卡提交数据 --%>
        
            var hidUserName = $("#hidUserName").val(); <%-- 记录当前用户名称 --%>
            var hidBankid = $("#hidBankid").val(); <%--选择银行对应的编码 --%>
            var txtBankCode = $("#txtBankCode").val(); <%-- 银行卡号 --%>
            var hidBankCode = $("#hidBankCode").val(); <%--银行卡号 --%>
            var txtUserName = $("#txtUserName").val(); <%--姓名 --%>
            var txtIDCard = $("#txtIDCard").val(); <%--身份证 --%>
            var txtPhone = $("#txtPhone").val(); <%--手机号码 --%>
            var hidBankLogo = $("#hidBankLogo").val(); <%--银行logo --%>
            var txtPhoneCode = $("#txtPhoneCode").val(); <%--手机验证码 --%>
            var hidFas = $("#hidFas").val(); <%--快捷支付表ID  （机构代码+fastpay表id =中金快捷支付交易流水号）--%>
            var url = "";
            if (action == "2531") { <%--发送验证码，检验必填项 --%>
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
                $(own).removeAttr("onclick");<%-- 删除获取验证码事件 --%>
            } else if (action == "2532") { <%-- 检验验证码，检验必填项 --%>
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
                    if (action == "2531") { <%--获取验证码接口，返回数据 --%>
                        if (data1.error == 1) {<%--失败 --%>
                            closeTime(); <%--关闭计时器 --%>
                            $("#msg").html(data1.msg);<%--显示错误信息 --%>
                        } else { <%--成功 --%>
                         $(own).unbind("click");
                            time(own, 120, action);<%--开启计时器 --%>
                            $("#hidFas").val(data1.id); <%--快捷支付绑定记录ID --%>
                            $("#isshow").show();<%--测试接口显示密码 --%>
                        }
                    } else { <%--检验验证码，返回数据 --%>
                        closeTime(); <%-- 关闭计时器 --%>
                        if (data1.error != 1) { <%--成功 --%>
                            $("#msg").html(""); <%-- 清空错误提示 --%>
                            $('.cd-popup').click(); <%--关闭输入验证码弹出框 --%>
                            refFastBank(); <%--刷新快捷支付列表 --%>
                        } else { <%--失败 --%>
                            $("#txtPhoneCode").val("");<%--清空验证 --%>
                            $("#hidFas").val("");<%--清空快捷支付绑定记录ID --%>
                            $("#msg").html(data1.msg); <%-- 显示错误提示 --%>
                        }
                    }
                }
            });
        }

        function refFastBank() { <%--更新快捷支付 --%>
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
        <%--刷新绑定快捷支付银行 --%>

      
        }

        var payid = "";
        var prepayid = "";
        function submitPay(action) {<%--“立即支付”按钮onclick方法 --%>
            var KeyID = '<%= Request.QueryString["Orderid"] %>'; <%--订单ID --%>
            var CompID= '<%= Request.QueryString["compID"] %>'; <%-- 企业ID --%>
            var isDBPay= '<%= Common.DesDecrypt(Request.QueryString["isDBPay"], Common.EncryptKey) %>';<%--支付方式（0，支付，1，担保支付）--%>
            var hida1 = $("#hida1").val();<%--是否使用企业钱包 --%>
            var hida2 = $("#hida2").val(); <%-- 是否使用快捷支付 --%>
            var hida3 = $("#hida3").val(); <%--是否使用网银支付 --%>
            var hida4 = $("#hida4").val(); <%--是否使用融资 --%>
            var padPaypas = $("#padPaypas").val();  <%--企业钱包密码 --%>
            var txtPrice = $("#txtPrice").val(); <%--企业钱包金额 --%>
            var txtPayOrder = $("#txtPayOrder").val(); <%--本次支付金额 --%>
            var hidFastPay = $("#hidFastPay").val(); <%--快捷支付ID --%>
            var hidUserName = $("#hidUserName").val(); <%--用户名称 --%>
            var txtBankCode = $("#txtBankCode").val();<%--银行卡号 --%>
            var txtPhoneNum = $("#txtPhoneNum").val();<%--1376验证码 --%>
            var AccountType = $("input[name='AccountType']:checked").val(); <%-- 网银支付，账户类型 --%>
            
            var hidBank = $("#hidBank").val();<%--网银支付，选择银行Code --%>
            $("#txtPhoneNum").val("");<%--清空1376验证码 --%>
            var url = "";
            if (action == "1375" || action == "1376") {<%--快捷支付 --%>
                if (action == "1375") {
                    url = "<%= action1375 %>";
                } else if (action == "1376") {
                    url = "<%= action1376 %>";
                }
                $.ajax({
                    type: 'POST',
                    url: url,
                    data: { KeyID: KeyID, hida1: hida1, hida2: hida2, hida3: hida3, padPaypas: padPaypas, txtPrice: txtPrice, txtPayOrder: txtPayOrder, hidFastPay: hidFastPay, hidUserName: hidUserName, txtBankCode: txtBankCode, txtPhoneNum: txtPhoneNum, payid: payid, prepayid: prepayid, isDBPay:isDBPay,CompID:CompID },
                    success: function (data) {
                        var data1 = jQuery.parseJSON(data);
                        if (action == "1375" || action == "1376") {
                            if (data1.error == 1) {<%--失败 --%>
                                $("#lblPayError").html(data1.msg);<%--显示错误 --%>
                                if (data1.js !== null && data1.js != "") { <%-- 例：1376关闭遮罩层和弹出框 --%>
                                    eval(data1.js); <%--执行后台js，例如：关闭弹出框  --%>
                                }
                                $("#btnPay").one("click", check);<%--重新绑定“立即支付”按钮onclick事件 --%>
                            }
                            if (data1.success == 1) { <%--成功 --%>
                                payid = data1.payid;
                                prepayid = data1.prepayid;
                                eval(data1.js); <%--执行后台js，例如：弹出弹出框 --%>
                                if (action == "1376") {
                                    payid = "";<%--清空历史快捷支付记录 --%>
                                    prepayid = "";<%--清空历史企业钱包支付记录 --%>
                                } else {
                                    $("#txtPhoneNum").select();<%--获取文本框焦点 --%>
                                }
                            } else if (data1.success == 2) {
                                window.location.replace(data1.js);<%--支付成功，跳转到成功页面 --%>
                            }
                        }
                    }
                });
            } else if (action == "1311") {<%--网银支付 --%>
                $("#divPayMsg").show();<%--显示支付遇到问题按钮和支付成功按钮 --%>
                $(".opacity").show();<%--遮罩层 --%>
                $("#hidOid").val(KeyID);
                $("#hidPayOrder").val(txtPayOrder);<%--本次支付金额 --%>                
                $("#hidPrice").val(txtPrice);<%--企业钱包金额 --%>
                $("#hidIsPre").val(hida1);<%--是否使用企业钱包 --%>
                
                var isDBPay=  '<%= Common.DesDecrypt(Request.QueryString["isDBPay"], Common.EncryptKey) %>';<%--支付方式（0，支付，1，担保支付）--%>
                
                $("#hidIsDBPay").val(isDBPay);<%--非担保支付 --%>


                $("#hidBankNo").val(hidBank);<%--银行卡编码 --%>
                $("#hidAccountType").val(AccountType);<%--账户类型 --%>
                $("#hidPayPas").val(padPaypas);<%--企业钱包密码 --%>               
                window.open("","newWin");<%--打开空白页面 --%>
                $("#Form").submit();<%--提交数据 --%>
                

            } else if (action == "17000") {<%--融资余额支付 --%>
                url = "<%= action17000 %>";
                $.ajax({
                    type: 'POST',
                    url: url,
                    data: { KeyID: KeyID, hida4: hida4, txtPayOrder: txtPayOrder, hidUserName: hidUserName },
                    success: function (data) {
                        var data1 = jQuery.parseJSON(data);
                        if (data1.success == "1") {
                            eval(data1.js);<%--支付成功，跳转到成功页面 --%>
                        }
                        if (data1.error == 1) {
                            $("#lblPayError").html(data1.msg);<%--失败，显示错误提示 --%>
                            $("#btnPay").one("click", check);<%--重新绑定“立即支付”onclick事件--%>
                        }
                    }
                });

            } else if (action == "71000") {<%--融资申请借款 --%>
                $(".opacity").show();
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
                                eval(data1.js); <%--接口调用成功，弹出 #divFinaMsg 弹出框 --%>
                            }
                            if (data1.error == 1) {
                                $(".opacity").hide();
                                $("#lblPayError").html(data1.msg);<%--失败，显示错误提示 --%>
                                if (data1.js != "") {
                                    eval(data1.js);<%--执行后台js --%>
                                }
                                $("#btnPay").one("click", check);<%--重新绑定“立即支付”onclick事件 --%>
                            }
                        } catch (e) {
                             layerCommon.alert(e.name + ": " + e.message,IconOption.哭脸);
                        }
                    }
                });
            }
        }

        function check() {<%-- 立即支付按钮 --%>
            $("#txtPhoneNum").removeAttr("disabled");<%--快捷支付输入验证码框启用 --%>
            $("#lblPayError").html("");<%--立即支付后边message文本框 --%>
            $("#FinancingA").hide();<%--隐藏在线融资超链接 --%>
            if ($("#a1").is("[class*='center']")) {<%--企业钱包支付，密码不能为空 --%>
                if ($("#padPaypas").val() == null||$("#padPaypas").val() == "") {
                    
                    $("#lblPayError").html("支付密码不能为空！");<%-- 显示错误 --%>
                    $("#btnPay").one("click", check);<%-- 重新绑定立即支付按钮 --%>
                    return false;
                }
            }
            if ($("#a3").is("[class*='center']")) {<%--网银支付，必须选择一家银行 --%>
                if (!$("#a3").children("div").children("ul").children("li").is("[class*='border2']")) {
                    
                    $("#lblPayError").html("请选择银行！");<%--显示错误 --%>
                    $("#btnPay").one("click", check);<%--重新绑定立即支付按钮 --%>
                    return false;
                }
            }
            if ($("#a2").is("[class*='center']")) {<%--快捷支付，必须选择一个快捷支付 --%>
                if (!$("#a2").children("ul").children("li").is("[class*='border']")) {
                   
                    $("#lblPayError").html("请绑定快捷支付银行卡！");<%--显示错误 --%>
                    $("#btnPay").one("click", check);<%--重新绑定立即支付按钮 --%>
                    return false;
                }
            }
            <%--四种支付方式必须选择一种支付方式 --%>
            if (!$("#a4").is("[class*='center']") && !$("#a3").is("[class*='center']") && !$("#a2").is("[class*='center']") && !$("#a1").is("[class*='center']") && !$("#a5").is("[class*='center']")) {
                
                $("#lblPayError").html("请选择支付方式！");<%--显示错误 --%>
                $("#btnPay").one("click", check);
                return false;
            }
            if (!parseFloat($("#txtPayOrder").val())>0) {<%--支付金额必须大于0 --%>
                
                $("#lblPayError").html("支付金额必须大于0！"); <%--显示错误 --%>
                $("#txtPayOrder").val("0.00"); 
                $("#sumje").val("0.00");
                $("#btnPay").one("click", check);<%-- 重新绑定立即支付按钮 --%>                
                return false;
            }
            var hida1 = $("#hida1").val();<%--是否使用企业钱包 --%>
            var hida2 = $("#hida2").val();<%-- 是否使用快捷支付 --%>
            var hida3 = $("#hida3").val();<%--是否使用网银支付 --%>
            var hida4 = $("#hida4").val();<%--是否使用融资 --%>
            var hida5 = $("#hida5").val();<%--其它支付，微信、支付宝 --%>

            if (hida3 == "1" && hida2 != "1" && hida4 != "1" && hida5 != "1") {<%--网银支付 ， 网银支付+企业钱包支付 --%>
                submitPay("1311");
            }<%--微信支付、支付宝支付+企业钱包支付 --%>
            if (hida5 == "1" && hida2 != "1" && hida4 != "1" && hida3 != "1") {<%--网银支付 ， 网银支付+企业钱包支付 --%>
            var hidWxorAplipay=$("#hidWxorAplipay").val();<%--微信支付 Or  支付宝支付 --%>
            if(hidWxorAplipay=='zfb')
            {
                <%--支付宝支付 --%>
                 $("#btnApliay").trigger("click");
                 }else
                 {
                 <%--微信支付 --%>
                  $("#btnWxPay").trigger("click");

                 }
            }
             else if ((hida2 == "1" && hida3 != "1" && hida4 != "1" && hida5 != "1") || (hida1 == "1" && hida3 != "1" && hida2 != "1" && hida4 != "1" && hida5 != "1")) {
                <%--快捷支付 ， 企业钱包支付 ， 快捷支付+企业钱包支付 --%>
                submitPay("1375");
            } else if (hida4 == "1" && hida2 != "1" && hida3 != "1" && hida5!= "1") {<%--融资 --%>
                var hidType = $("#hidType").val();
                if (hidType == "1") {<%--融资余额支付 --%>
                    submitPay("17000");
                } else if (hidType == "2") {<%--申请借款 --%>
                    if (!!window.ActiveXObject || "ActiveXObject" in window) {
                        submitPay("71000");
                    } else {
                       
                        $("#lblPayError").html("支付金额必须大于0！");<%--显示错误 --%>
                        $("#btnPay").one("click", check);<%-- 重新绑定立即支付按钮 --%>
                        return false;
                    }
                }
            }
        }
        function msgTime(wait) {<%--1376快捷支付发送验证码，计时器 --%>
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
        function checkCode() {<%--检验快捷支付验证码必填 --%>
            clearTimeout(stop);
            $("#paying").hide();<%--正在支付文字隐藏 --%>
            $("#txtPhoneNum").show();<%--验证码文本框显示 --%>
            if ($("#txtPhoneNum").val() == "") {
                layerCommon.alert("请填写验证码",IconOption.哭脸);
                return false;
            }
            submitPay("1376");
        }
        function setType(val) {<%--根据在线融资的支付方式，改变支付按钮文字 --%>
            $("#hidType").val(val);<%--融资按钮值 --%>
            if (val == 1) {
                $("#btnPay").html("立即支付");
            } else if (val == 2) {
                $("#btnPay").html("申请借款");
            }
        }

        <%--关闭窗体 --%>
        function reclwindow()
        {
         this.window.close();
        }

      

        function BankCodeBlur(own) {<%--输入银行卡号，自动对应银行 --%>
            $.ajax({
                type: 'POST',
                url: "../../Handler/BankCodeBlur.ashx",
                data: { BankCode: $(own).val() },
                success: function (data) {
                    if (data != "") {
                        var result = JSON.parse(data);
                        //layerCommon.alert(result.BankCode);
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
            text-align: center;
            width: 150px;
            padding-left: 0;
            margin-right: 30px;
            line-height: 35px;
            height: 35px;
            background: #f6f6f6;
            cursor: pointer;
        }
        #a4 .bankCard2 li img
        {
            padding-top: 3px;
        }
        #a4 .bankCard2 li label
        {
            font-size: 16px;
            cursor: pointer;
        }
        .decoration:hover
        {
            text-decoration: none;
        }
    /* #div_kjzfsxf { float:right; display:inline;}
      #div_wyzfsxf { float:right; display:inline;}
      #div_wxzfsxf { float:right; display:inline;}*/
      
      #div_kjzfsxf {  display:inline;}
      #div_wyzfsxf {  display:inline;}
      #div_wxzfsxf {  display:inline;}
      
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
                    <img src="../../images/logo2.0.png" height="33" /></a><i>支付平台</i></div>
            <div class="topMenu">
                <a href="../UserIndex.aspx">我的桌面</a>|<a href="../../index.aspx">医站通首页</a>
                &nbsp;
                <% if (ConfigurationManager.AppSettings["OrgCode"] == "SYJ")
                   {%>
                <i style="font-weight: bold; color: red;">服务热线：400-8859-319</i>
                <% } %>
            </div>
        </div>
    </div>
    <!--header end-->
   <%-- 隐藏域 start--%>
    <input type="hidden" id="hidPay" runat="server" value="0" />
    <input type="hidden" id="hidPrepay" runat="server" value="0" />
    <input type="hidden" id="hidNoPrice" runat="server" value="0" />
    <input type="hidden" id="hidOrderid" name="hidOrderid" runat="server" value="0" />
    <input type="hidden" id="hidUserName" name="hidUserName" runat="server" value="" />
    <input type="hidden" id="hidSumPrice" name="" runat="server" value="0" />
    <input type="hidden" id="hidPricePay" runat="server" value="0.00" />
    
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
   <%-- 隐藏域  end  --%>
    <%--支付信息 start--%>
    <div class="payInfo">
        <div class="number">
            购买服务期限：<a href="javascript:void(0)" id="Data" runat="server"></a> ~ <a href="javascript:void(0)" id="DataEnd" runat="server"></a></div>
        <div class="gray">
          </div>
        <ul class="amount">
            <li class="ab">本次支付金额：<b class="money"><input id="txtPayOrder" runat="server" onkeyup="priceKeyup(this)"
                type="text" class="box1" readonly="readonly"/></b>元&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</li>
        </ul>
    </div>
    <%--支付信息 end--%>
    <div class="payOpt" id="checkId">
       <%-- 企业钱包支付 start--%>
       <%-- <div class="li" id="a1">
            <input type="hidden" name="hida1" id="hida1" runat="server" value="" />
            <a href="javascript:void(0)" class="check cur"></a>
            <div class="title">
                企业钱包</div>
            <div class="amOpt">
                <i>可用余额：0<label id="lblSumPrice" runat="server"></label>
                    元 </i>
                <input autocomplete="off" id="txtPrice" name="txtPrice" onkeyup="priceKeyup(this)"
                    type="text" value="0.00" class="box" runat="server" style="display: none" /></div>
            <div class="amount">
                支付<b><label id="lblPrice1" runat="server">0.00</label></b>元</div>
            <input type="hidden" id="hidYorN1" value="0" />
        </div>--%>
       <%--企业钱包支付 end--%>

       <%--快捷支付 start--%>
        <%--<div class="li" id="a2">
            <input type="hidden" name="hida2" id="hida2" runat="server" value="" />
            <input type="hidden" name="hidFastPay" id="hidFastPay" runat="server" value="0" />
            <a href="javascript:void(0)" class="check cur"></a>
            <div class="title">
                快捷支付</div>
            <ul class="bankCard">
                <asp:Repeater runat="server" ID="rptQpay" OnItemCommand="rptQpay_ItemCommand">
                    <ItemTemplate>
                        <li><span>
                            <img src="<%# Eval("BankLogo") %>" /></span> <span style="position: absolute; left: 300px;">
                                <i class="quick">快捷</i><i>储蓄卡</i>|<i>尾号**<%# Eval("bankcode").ToString().Replace(" ", "").Substring(Eval("bankcode").ToString().Replace(" ", "").Length - 4, 4)%></i>|<i>手机号<%# Eval("phone").ToString().Substring(0,3) %>****<%# Eval("phone").ToString().Substring(Eval("phone").ToString().Length-4,4) %></i>
                            </span>
                            <input type="hidden" class="hidFastID" value="<%# Eval("ID") %>" /></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <div class="addCard">
                <a href="javascript:void(0)" class="time cd-popup-trigger">添加银行卡</a></div>
            <div class="amount">
                支付<b><label id="lblPrice2" runat="server">0.00</label></b>元

                <div runat="server" id="div_kjzfsxf" >
                &nbsp;&nbsp; |&nbsp;&nbsp;手续费
            <b><label id="lblPrice2_sxf" runat="server">0.00</label></b>元
                </div></div>
            <input type="hidden" id="hidYorN2" value="0" />
        </div>--%>
       <%-- 快捷支付 end--%>
        <%--网银支付 start--%>
        <div class="li center" id="a3">
            <input type="hidden" name="hida3" id="hida3" runat="server" value="1" />
            <input type="hidden" name="hidBank" id="hidBank" runat="server" />
            <a href="javascript:void(0)" class="check cur"></a>
            <div class="title">
                网银支付</div>
            <div class="amOpt">
                &nbsp;&nbsp;账户类型：
                <input type="radio" name="AccountType" value="11" id="type11" class="AccountType"
                    runat="server" checked="true" /><label class="AccountType" for="type11">个人账户（储蓄卡）</label>&nbsp;&nbsp;&nbsp;&nbsp;
                    <input type="radio" name="AccountType" value="13" id="type13" class="AccountType"
                    runat="server" /><label class="AccountType" for="type11">个人账户（信用卡）</label>&nbsp;&nbsp;&nbsp;&nbsp;
                <%--<i style="color: #2ea7e7;">(暂不支持信用卡)</i>&nbsp;&nbsp;--%>
                <input type="radio" name="AccountType" value="12" id="type12" class="AccountType"
                    runat="server" /><label class="AccountType" for="type12">企业账户</label>
            </div>
            <div class="amount">
                支付<b><label id="lblPrice3" runat="server">0.00</label></b>元
               <div id="div_wyzfsxf" runat="server"> &nbsp;&nbsp;|&nbsp;&nbsp;
                手续费<b><label id="lblPrice3_sxf" runat="server">0.00</label></b>元
                </div></div> 
                <div id="divbank1" class="divbank"> <%--个人账户借记卡--%> 
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
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>浦发银行.jpg" /><input
                        type="hidden" class="hidBank" value="310" /></a></li>
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
               <%-- <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国农业银行.jpg" /><input
                        type="hidden" class="hidBank" value="103" /></a></li>--%>
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>中国银行.jpg" /><input
                        type="hidden" class="hidBank" value="104" /></a></li>
                           <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>广州农商银行.jpg" /><input
                        type="hidden" class="hidBank" value="1405" /></a></li>
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

             <%--   <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>杭州银行.jpg" /><input
                        type="hidden" class="hidBank" value="423" /></a></li> 
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>天津银行.jpg" /><input
                        type="hidden" class="hidBank" value="434" /></a></li>  --%>                     
                        <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>兴业银行.jpg" /><input
                        type="hidden" class="hidBank" value="309" /></a></li>
                           <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>广发银行.jpg" /><input
                        type="hidden" class="hidBank" value="306" /></a></li>

                       <%-- 新增信用卡银行  2016-11-06  begin--%>
                   <%-- <li><a href="javascript:void(0)">
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
                        <input type="hidden" class="hidBank" value="427" /></a></li>--%>

                       <%-- 新增信用卡银行  2016-11-06 end--%>
                        <%-- <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>深圳发展银行 .jpg" />
                        <input type="hidden" class="hidBank" value="427" /></a></li>--%>
                        <%--  <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>东莞银行.jpg" />
                        <input type="hidden" class="hidBank" value="425" /></a></li>
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>大连银行.jpg" />
                        <input type="hidden" class="hidBank" value="420" /></a></li>                       
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>日照银行.jpg" />
                        <input type="hidden" class="hidBank" value="455" /></a></li>
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>山西省临汾市尧都区信用合作联社.jpg" />
                        <input type="hidden" class="hidBank" value="1434" /></a></li>
                     
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>天津银行.jpg" />
                        <input type="hidden" class="hidBank" value="434" /></a></li>--%>
                        <%-- <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>天津农商银行.jpg" />
                        <input type="hidden" class="hidBank" value="427" /></a></li>--%>
                        <%-- <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>宜昌市商业银行.jpg" />
                        <input type="hidden" class="hidBank" value="427" /></a></li>
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>浙江泰隆商业银行.jpg" />
                        <input type="hidden" class="hidBank" value="473" /></a></li>
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>重庆银行.jpg" />
                        <input type="hidden" class="hidBank" value="441" /></a></li>
                         <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>浙江省农村信用社.jpg" />
                        <input type="hidden" class="hidBank" value="1429" /></a></li>--%>
                        <%--
                <li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>银联在线.jpg" /><input
                        type="hidden" class="hidBank" value="888" /></a></li>--%>
            </ul>
               </div>
                <div id="divbank2"  class="divbank" style="display:none"><%--企业账户显示银行 --%>
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
        <%--网银支付 end--%>
       
        <%--其他平台支付 start--%>
     
<%--        <div class="li" id="a5">
        <asp:Button ID="btnApliay" runat="server" OnClick="btnApliay_Click" Text="支付宝支付" Style="display: none;" />
        <asp:Button ID="btnWxPay" runat="server" OnClick="btnWxPay_Click" Text="微信支付" Style="display: none;" />
            <input type="hidden" name="hida5" id="hida5" runat="server" value="" />
            <input type="hidden" name="hidWxorAplipay" id="hidWxorAplipay" runat="server" />
            <a href="javascript:void(0)" class="check cur"></a>
            <div class="title">其他支付</div>
         <div class="amount">支付<b><label id="lblPrice5">487.50</label></b>元<div id="div_wxzfsxf">　|　手续费<b><label id="lblPrice5_sxf">0.00</label></b>元</div></div>
           
            <div class="platformPay">
          
            	<a href="javascript:void(0)"><i class="zfb-i"></i>支付宝支付<input type="hidden" class="abcd" value="zfb" /></a>
                <a href="javascript:void(0)"><i class="wx-i"></i>微信支付<input type="hidden" class="abcd" value="wx" /></a>
      
            </div>
             <div class="blank20"></div><div class="blank20"></div>
          </div>  --%>
     
        <%--其他平台支付 end--%>
     
       <%-- 在线融资 start--%>
       <%-- <div class="li" id="a4">
            <input type="hidden" name="hida4" id="hida4" runat="server" value="" />
            <input type="hidden" id="hidType" value="" runat="server" />
            <a href="javascript:void(0)" class="check cur"></a>
            <div class="title">
                在线融资</div>
            <div class="amOpt">
                &nbsp;&nbsp;<label id="lblBalance" style="display: none">账户余额：<label id="lblBalance1"
                    runat="server" style="color: #ff4e02; font-size: 18px;">0.00</label>&nbsp;&nbsp;元</label>
                &nbsp;&nbsp;<label id="lblBalance2" style="display: none">授信余额：<label id="lblBalance3"
                    runat="server" style="color: #ff4e02; font-size: 18px;">0.00</label>&nbsp;&nbsp;元</label>
                &nbsp;&nbsp;<label id="lblFinancingMsg" style="color: #2ea7e7;">每个订单只能借款一次</label>
                <% if (PList != null && PList.Count > 0)
                   { %>
                &nbsp;&nbsp;<label>融资处理中金额：<label id="lblBalance5" class="price" runat="server" style="color: #666;
                    font-size: 14px; font-weight: bold;">0.00</label>&nbsp;&nbsp;元</label>
                <% } %>
            </div>
            <div class="amount">
                支付<b><label id="lblPrice4" runat="server">0.00</label></b>元</div>
            <ul class="bankCard2">
                <li onclick="setType(1)">
                    <label>
                        账户余额支付</label></li>
                <% if (PList != null && PList.Count == 0)
                   { %>
                <li onclick="setType(2)" title="仅支持IE浏览器操作">
                    <label>
                        申请借款</label></li>
                <% } %>
            </ul>
        </div>--%>
        <%--在线融资 end--%>
       
        <div class="payPas" style="display: none">
            支付密码<input name="padPaypas" id="padPaypas" type="text" class="box3" runat="server"
                onfocus="this.type='password'" autocomplete="off" value="" />
            <% if (ConfigurationManager.AppSettings["Paytest_zj"] == "1")
               { %>
            <b class="red">测试用密码:123456a</b>
            <% }%>
            &nbsp;&nbsp;&nbsp; <%--<a href="../PayPWDEdit.aspx?type=zhifu&ordID=<%=KeyID %>" style="text-decoration: underline;">
                忘记密码?</a>--%> </div>
        <div class="payBtn">
            <a href="javascript:void(0)" id="btnPay" class="btn right">立即支付</a>
            <div class="right" id="sum_payprice" runat="server"><div class="txt">总金额(含手续费)：&nbsp;<b ><label id="sumje" class="red" style="font-size:18px; color:#ff4e02;font-weight:normal" runat="server"></label></b>&nbsp;元<%--<br />未付款金额：<b class="red">300.00</b>元--%></div></div>
           <div class="blank10"></div>
           <i class="red tis"><label id="lblPayError" runat="server"></label></i>
        </div>
      

        <div class="blank10"></div>
    </div>
    <%--快捷支付弹窗 start--%>
    <div class="cd-popup" id="addBank">
        <div class="cd-popupBg">
        </div>
        <div class="hidebg">
        </div>
        <%--添加银行卡 start--%>
        <div class="cd-popup-container addBank a5 noOff">
            <div class="title">
                <b>添加银行卡</b>&nbsp;<i style="color: #2ea7e7;">(暂不支持信用卡绑定)</i></div>
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
                <%--<li><a href="javascript:void(0)">
                    <img src="<%= Common.GetWebConfigKey("ImgViewPath") + "BankImg/" %>浦发银行.jpg" /><input
                        type="hidden" class="hidBankCode" value="310" /></a></li>--%>
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
               
                <li onclick="return false;" style="text-align: center; line-height: 40px; border: 0px;
                    padding: 0px; width: 204px; height: 40px;"><a id="otherBank" href="javascript:void(0)"
                        style="color: #2ea7e7; font-size: 14px; width: 100%;">更多银行</a></li>
                <asp:Repeater runat="server" ID="rptOtherBank">
                    <ItemTemplate>
                        <li class="otherBank"><a href="javascript:void(0)">
                            <img src="<%# Common.GetWebConfigKey("ImgViewPath") + "BankImg/" + Eval("BankName") %>.jpg" /><input
                                type="hidden" class="hidBankCode" value="<%# Eval("BankCode") %>" /></a></li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <div class="payBtn">
                <a href="javascript:void(0)" class="btn btnId">下一步</a></div>
            <a href="javascript:void(0)" class="close cd-popup-close img-replace" onclick="closeTime(1)">
                关闭</a>
        </div>
       <%-- 添加银行卡 end--%>
        <%--添加银行卡 start--%>
        <div class="cd-popup-container addBank a6">
            <div class="title">
                <b>添加银行卡</b>&nbsp;<i style="color: #2ea7e7;">(暂不支持信用卡绑定)</i></div>
            <ul class="addCardNr">
                <li><i class="bt">银行卡</i><input id="txtBankCode" maxlength="25" name="txtBankCode" type="text" class="box2"
                    onkeyup="this.value=this.value.replace(/\D/g,'');" onblur="BankCodeBlur(this)" /><input
                        type="hidden" id="hidBankCode" /></li>
                <li><i class="bt">付款银行</i>
                    <div class="card">
                        <span>
                            <img id="imgBankImg" src="../images/payBank.jpg"></span>
                        <input type="hidden" id="hidBankid" name="hidBankID" />
                        <input type="hidden" id="hidBankLogo" name="hidBankLogo" />
                    </div>
                    <a href="javascript:void(0)" class="return decoration"><i class="icon"></i>选择其他银行</a>
                </li>
                <li><i class="bt">姓名</i><input id="txtUserName" name="txtUserName" type="text" class="box3" /></li>
                <li><i class="bt">身份证</i><input id="txtIDCard" onkeyup="this.value=this.value.replace(/[^\a-\z\A-\Z0-9]/g,'')"  name="txtIDCard" type="text" maxlength="18" class="box2" /></li>
                <li><i class="bt">手机号</i><input id="txtPhone" name="txtPhone"   maxlength="11" type="text" class="box3"
                    onkeyup="this.value=this.value.replace(/\D/g,'')" value="" />
                    <a href="javascript:void(0)" class="btn2 decoration" id="submit2531" onclick="onSubmit(this,'2531')">
                        获取验证码</a></li>
                <li><i class="bt">手机验证码</i><input id="txtPhoneCode" maxlength="6" name="txtPhoneCode" type="text"
                    onkeyup="this.value=this.value.replace(/\D/g,'')" class="box3" style="width: 50px;"
                    value="" />&nbsp;&nbsp;<label style="color: #da4444;" id="msg"></label>
                    <input type="hidden" id="hidFas" />
                    <% if (ConfigurationManager.AppSettings["Paytest_zj"] == "1")
                       { %>
                    <i id="isshow" style="display: none;"><b class="red">（测试密码:123456）</b></i>
                    <% }%>
                </li>
                
            </ul>
            <div class="payBtn payBtn2">
                <a href="javascript:void(0)" class="btn" onclick="onSubmit(this,'2532')">确认绑定</a></div>
            <a href="javascript:void(0)" class="close cd-popup-close img-replace" onclick="closeTime(1)">
                关闭</a>
        </div>
       <%--添加银行卡 end--%>
    </div>
   <%--快捷支付弹窗 end--%>
    <%--弹出遮罩层 Begin--%>
    <div class="opacity" style="display: none;">
    </div>
    <%--弹出遮罩层 End--%>
    <%--弹出录入层 Begin--%>
    <div class="tip">
        <div class="tiptop">
            <span>快捷支付</span><input name="" type="button" class="cancel close" value="" /></div>
        <div class="tipinfo">
            <div class="lb">
                <span class="title">请输入短信验证码</span> <i class="tel" runat="server" id="phone">已发送至188****2156</i></div>
            <div class="lb">
                <input id="txtPhoneNum" class="box2" type="text" onkeypress="priceKeyup(this)" onkeyup="priceKeyup(this)"
                    runat="server" autocomplete="off" />
                <span style="display: none; margin-left: 195px;" id="paying"><b>正在支付...</b></span>
                <%--<asp:Button ID="btnTx1376" style="display:none;" runat="server" Text="确认" OnClick="btnTx1376_Click" OnClientClick="return checkCode();" />--%>
                <input id="btnTx1376" type="text" style="display: none;" value="确认" onclick="checkCode()" />
            </div>
            <div class="lb txt">
                <span>短信验证码已发送，请注意查收</span>
                <% if (ConfigurationManager.AppSettings["Paytest_zj"] == "1")
                   { %>
                <b class="red">（测试密码:123456）</b>
                <% }%></div>
            <div class="lb txt2">
                <span>
                    <%--<a href="" class="btn" style="display:none;">重新获取手机短信验证码</a>--%>
                    <i class="" id="msgone">120秒后短信验证码将失效</i> <i id="msgtwo" style="color: Red;">验证码已失效，请关闭窗口，重新支付！</i>
                </span>|<a href="javascript:void(0)" class="cancel close2">关闭</a>
            </div>
        </div>
    </div>
    <%--弹出录入层 End--%>
    <%--网上支付提示 start--%>
    <div class="addBank" id="divFinaMsg" style="display: none; height: 225px;">
        <div class="title">
            <b>网上支付提示 </b>
        </div>
        <div class="payTis">
            <span class="pic">
                <img src="../images/payTis.gif" width="200" /></span><i>借款完成前，请不要关闭借款验证窗口<br />
                    借款完成后，请根据您支付的情况点击下面按钮</i><a href="javascript:void(0)" id="btnF" class="bluBtn">借款遇到问题</a><a
                        href="javascript:void(0)" id="btnS" class="redBtn">申请借款成功</a></div>
        <asp:Button ID="btnSuccess" runat="server" Style="display: none;" OnClick="btnSuccess_Click" />
        <asp:Button ID="btnFailure" runat="server" Style="display: none;" OnClick="btnFailure_Click" />
    </div>
    <%--网上支付提示 end--%>
    <%--网上支付提示 start--%>
    <div class="addBank" id="divPayMsg" style="display: none; height: 225px;">
        <div class="title">
            <b>网上支付提示 </b>
        </div>
        <div class="payTis">
            <span class="pic">
                <img src="../images/payTis.gif" width="200" /></span><i>支付完成前，请不要关闭支付验证窗口<br />
                    支付完成后，请根据您支付的情况点击下面按钮</i><a href="javascript:void(0)" id="btnPayF" class="bluBtn">支付遇到问题</a><a
                        href="javascript:void(0)" id="btnPayS" class="redBtn">已经完成支付</a></div>
        <asp:Button ID="btnPaySuccess" runat="server" Style="display: none;" OnClick="btnPaySuccess_Click" />
        <asp:Button ID="btnPayFailure" runat="server" Style="display: none;" OnClick="btnPayFailure_Click" />
    </div>
    <%--网上支付提示 end--%>
    <div class="blank20">
    </div>
    <%--常见问题 start--%>
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
    <%--常见问题 end--%>
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
     <input type="hidden" id="hidCompid" name="hidCompid" runat="server" value="" />
    </form>
</body>
</html>

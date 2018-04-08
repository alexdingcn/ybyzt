<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayQuickly.aspx.cs" Inherits="Distributor_Pay_PayQuickly" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>修改快捷支付</title>
    <link href="../css/pay.css" rel="stylesheet" type="text/css" />
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/pay.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script language="C#" runat="server">
        string action2531 = "../../Handler/Tx2531.ashx";
        string action2532 = "../../Handler/Tx2532.ashx";
    </script>
    <script type="text/javascript">
    //关闭按钮事件
        function GoTopag(id) {
        
            layerCommon.confirm('确认解除绑定吗？',function(){$("#hidFastBankid").val(id);
                $("#btnClose").trigger("click");})
        }
        
        $(document).ready(function () {
            $("#txtBankCode").keyup(function () {
                $(this).val($(this).val().replace(/\D/g, '').replace(/....(?!$)/g, '$& '));
            });
            $("#otherBank").click(function (e) {
                e.stopPropagation();
                $(this).parent().hide();
                $(".otherBank").show();
            });
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    e.keyCode = 0;
                    return false;
                }
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
        function closeTime(a)
        {
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
                if (txtBankCode.length<10) {
                    layerCommon.alert("银行卡长度不符！", IconOption.哭脸);
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
                    //layerCommon.alert(data1.msg,IconOption.哭脸);
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
                data: { user:0 },
                success: function (data) {
                    var data1 = jQuery.parseJSON(data);
                    var html = "";
                    for (var i = 0; i < data1.length; i++) {
                        html += '<li>';
                        html += '<div class="left">';
                        html += '<span class="pic" style="width:200px;">';
                        html += '<img src="' + data1[i].BankLogo + '" />';
                        html += '</span><i>尾号**' + data1[i].bankcode.slice(-4) + '(储蓄卡)</i>';
                        html += '</div>';
                        html += '<div class="left line">';
                        html += '<i>手机：' + data1[i].phone.substr(0, 3) + '****' + data1[i].phone.slice(-4) + ' </i><i';
                        html += 'class="gray">(使用此号码接收验证码)</i> ';
                        html += '<a href="javascript:void(0)"  onclick=\'GoTopag(' + data1[i].ID + ')\'   class="bule">关闭</a>';
                        html += '</div>';
                        html += '</li>';
                    }
                    $(".binding ul.list").html(html);
                    $("#bankCount").html(data1.length);
                }
            });
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
        .cd-popup-trigger
        {
            display:inline-block;
            float:right;
            margin-right:10px;
        }
        /*footer*/
.footer{ text-align:left; color:#999; margin-top:0;}
.header{ width:100%;  border-bottom:1px solid #d1d1d1; overflow:hidden;}
.header .con{ width:1200px; margin:0px auto; position:relative; overflow:auto;}
.header .logo{ position:relative; top:0; left:0;}
.header .logo i{ font-size:16px; position:relative; top:0;}

    </style>
</head>
<body class="root3">
  <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="PayQuickly" />
      <asp:Button ID="btnClose" runat="server" onclick="btnClose_Click" style=" display:none"  />
    <input type="hidden" id="hidFastBankid" runat="server" />
    <div class="rightCon">
    <div class="info">
         <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
        <a id="navigation2" href="/Distributor/Pay/PayQuickly.aspx" class="cur">修改快捷支付</a></div>
        <!--绑定快捷支付 start-->
        <div class="userTrend">
            <div class="uTitle">
                <b>快捷支付</b>
                <a href="javascript:void(0)" class="time cd-popup-trigger btnYe" onclick="refFastBank()" style="color:#fff; float:none;"><i class="addIcon"></i>添加银行卡</a>
            </div>
            <div class="binding">
                <div class="title">
                    <b class="font">已开通快捷支付的银行卡：</b>已添加<b class="red" id="bankCount"><%= SumNumber %></b>张储蓄卡。</div>
                <div class="blank10">
                </div>
                <ul class="list">
                    <asp:Repeater runat="server" ID="rptQpay">
                        <ItemTemplate>
                            <li>
                                <div class="left">
                                    <span class="pic" style="width:200px;">
                                        <img src="<%# Eval("BankLogo") %>" />
                                    </span><i>尾号**<%# Eval("bankcode").ToString().Substring(Eval("bankcode").ToString().Length-4,4) %>(储蓄卡)</i>
                                </div>
                                <div class="left line">
                                    <i>手机：<%# Convert.ToString(Eval("phone")).Substring(0,3)%>****<%# Convert.ToString(Eval("phone")).Substring(Eval("phone").ToString().Length - 4, 4) %></i>
                                    <i class="gray">(使用此号码接收验证码)</i> 
                                    <a href="javascript:void(0)"  onclick='GoTopag(<%# Eval("ID") %>)'   class="bule">关闭</a>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <div class="blank10">
            </div>
        </div>
        <!--绑定快捷支付 end-->
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
                <li onclick="return false;" style=" text-align:center; line-height:40px; border:0px #000; padding:0px; width:204px; height:40px;" ><a id="otherBank" href="javascript:void(0)" style="color:#2ea7e7; font-size:14px; width:100%;">更多银行</a></li>
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
        <input type="hidden" id="hidUserName" name="hidUserName" runat="server" value="" />
        <div class="cd-popup-container addBank a6">
            <div class="title">
                <b>添加银行卡</b></div>
            <ul class="addCardNr">
                <li><i class="bt">银行卡</i><input id="txtBankCode" name="txtBankCode" type="text" class="box2" onkeyup="this.value=this.value.replace(/\D/g,'');" onblur="BankCodeBlur(this)" /><input type="hidden" id="hidBankCode" /></li>
                <li><i class="bt">付款银行</i>
                    <div class="card">
                        <span><img id="imgBankImg"></span>
                        <input type="hidden" id="hidBankid" name="hidBankid" />
                        <input type="hidden" id="hidBankLogo" name="hidBankLogo" />
                    </div>
                    <a href="javascript:void(0)" class="return"><i class="icon"></i>选择其他银行</a>
                </li>
                <li><i class="bt">姓名</i><input id="txtUserName" name="txtUserName" type="text" class="box3" maxlength="40" /></li>
                <li><i class="bt">身份证</i><input id="txtIDCard" name="txtIDCard" type="text" class="box2" maxlength="40" /></li>
                <li><i class="bt">手机号</i><input id="txtPhone" name="txtPhone" type="text" class="box3" onkeyup="this.value=this.value.replace(/\D/g,'')" value="" maxlength="11" />
                    <a href="javascript:void(0)" class="btn2" id="submit2531" onclick="onSubmit(this,'2531')">获取验证码</a></li>
                <li><i class="bt">手机验证码</i><input id="txtPhoneCode" name="txtPhoneCode" type="text" onkeyup="this.value=this.value.replace(/\D/g,'')"
                    class="box3" style="width: 50px;" value="" />&nbsp;&nbsp;<label id="msg" style="color:#da4444;" ></label>
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
    </div>
    </form>
</body>
</html>

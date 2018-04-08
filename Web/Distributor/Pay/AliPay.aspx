<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AliPay.aspx.cs" Inherits="Distributor_Pay_AliPay" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
<title><%= ConfigurationManager.AppSettings["PhoneSendName"].ToString()%>支付平台</title>
    <link href="../css/pay.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/pay.js" type="text/javascript"></script>    
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
     <script type="text/javascript">
         $(document).ready(function () {
            // window.opener.reclwindow();
         });
         var stopid;
         function time(wait) {//计时器
             if (wait >0) {
                 $(".text").html("距离二维码过期还剩<i class='red'  style='font-size:20px;'>" + wait + "</i> 秒，过期请刷新页面重新获取二维码");              
                
                 wait--;
                 stopid = setTimeout(function () { time(wait) }, 1000);

                 //查询是否已经支付成功
                 var hidguid = $("#hidguid").val();
                 var hidprice = $("#hidprice").val();
                 var hidordid = $("#hidordid").val();               

                 $.ajax({
                     type: 'POST',
                     url: "../../Handler/BehindWxPay.ashx",
                     async: false,
                     data: { hidguid: hidguid, hidprice: hidprice, hidordid: hidordid},
                     success: function (data) {
                         try {
                             var result_data = jQuery.parseJSON(data);
                             //支付状态修改成功，跳转成功页面
                             if (result_data.rel == "ok") {
                                 clearTimeout(stopid); //清空stopid值，停止技术器    
                                 window.location.href = result_data.js;
                             } //else//支付失败
                                // window.location.href = result_data.js; //支付失败，跳转到失败页面
                         } catch (e) {
                             window.location.href = '../newOrder/orderdetail.aspx?top=4&KeyID=<%=orderid%>';
                         }
                     }
                 });
             } else {
                 clearTimeout(stopid); //清空stopid值，停止技术器  
                 // window.location.href = '../newOrder/orderdetail.aspx?top=4&KeyID=<%=orderid%>';
                 $(".text").html("距离二维码过期还剩<i class='red'  style='font-size:20px;'>" + wait + "</i>秒，过期请<a id='msgtwo' onclick='receh()'  style='color: Blue;cursor: pointer;'>刷新</a>页面重新获取二维码");              
                
                 var path = '../images/wechat.jpg';
                 $("#Image").attr('src', path);
             }

         }
         //选择其他支付方式跳转js
         function pay() {
             window.location.href ='Pay.aspx?isDBPay=<%=Common.PaySetingsValue(this.CompID) %>&KeyID=<%=Common.DesEncrypt(orderid.ToString(),Common.EncryptKey) %>';
         }





         //刷新当前页面
         function receh() {
             clearTimeout(stopid);
             window.location.reload();
             //var hidjson = $("#hidjson").val();
             //$.ajax({
             //    type: 'post',
             //    url: 'WeChatPay.aspx?action=sett',
             //    data: { hidjson: hidjson },
             //    async: false, //true:同步 false:异步
             //    success: function (data) {
                     
             //        var result_data = jQuery.parseJSON(data);
             //        //支付状态修改成功，跳转成功页面
             //        if (result_data.rel == "ok") {
             //           // clearTimeout(stopid); //清空stopid值，停止技术器    
             //            var path = result_data.url;
             //            $("#Image").attr('src', path);

             //            $("#hidguid").val(result_data.Hidguid);
             //            $("#hidprice").val(result_data.Hidprice);
             //            $("#hidordid").val(result_data.Hidordid);
             //            $("#hidpid").val(result_data.Hidpid);
             //            $("#hidppid").val(result_data.Hidppid);
             //            time(60);

             //        }
             //    }
             //});
         }
        

     </script>
</head>
<body onload="time(0)">
<input type="hidden" id="hidguid" runat="server" />
<input type="hidden" id="hidprice" runat="server" />
<input type="hidden" id="hidordid" runat="server" />


<!--header start-->
    <div class="header">
        <div class="con">
            <div class="logo">
                <a href="../../index.aspx">
                    <img src="../../images/logo2.0.png" height="33" /></a><i>支付平台</i></div>
            <div class="topMenu">
                
                <a href="../UserIndex.aspx">我的桌面</a>|<a href="../../index.aspx">医站通首页</a>|<a href="orderPayList.aspx">我的待支付订单</a>
                &nbsp;
                <% if (ConfigurationManager.AppSettings["OrgCode"] == "SYJ")
                   {%>
                <i style="font-weight: bold; color: red;">服务热线：400-8859-319</i>
                <% } %>
            </div>
        </div>
    </div>
<!--header end-->

<!--支付信息 start-->
<div class="payInfo">
	<div class="number">订单号：<label id="lblOrderNO" runat="server"></label><%--<a href="javascript:void(0)" id="lblOrderNO" runat="server"></a>--%>&nbsp;&nbsp;&nbsp;收款方：<label id="fee" runat="server"></label><%--<a href="javascript:void(0)" id="fee" runat="server"></a>--%></div>
   <%-- <div class="gray">请您在提交订单后24小时内完成支付，否则订单会自动取消。</div>--%>
    <div class="amount" style=" font-size:14px; top:28px;">应付金额：<b style="color:#ff4e02"><%=txtPayOrder.ToString("0.00")%></b>元</div>
</div>
<!--支付信息 end-->

<!--快捷支付弹窗 start-->
<div class="pay-wechat">
	<div class="title">支付宝</div>
	<div class="return"><a href="javascript:" onclick="pay()">< 选择其他支付方式</a></div>
	<div class="text"><%--距离二维码过期还剩40秒，过期自动跳转页面--%></div>
	<div class="wechat-p">
   <asp:Image ID="Image" runat="server" Width="260"/>
    <%-- <img src="../images/wechat.jpg" width="260" /></div>--%>
	<div class="tis"><i class="sm-i"></i><i class="t">请使用支付宝扫一扫<br />扫描二维码支付</i></div>
    <div class="app-p2"></div>
    <div class=" clear"></div>
    </div> 
</div>
<!--快捷支付弹窗 end-->

<div class="blank20"></div>

<div class="footer"><%= ConfigurationManager.AppSettings["CompanyName"].ToString() %></div>

</body>
</html>

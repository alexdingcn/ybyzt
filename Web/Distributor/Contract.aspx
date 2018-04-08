<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Contract.aspx.cs" Inherits="Distributor_Contract" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>交易合约</title>
</head>

<body>
<style>
 /* 整站全局公共样式的设定 */
* html,* html body{background-image:url(about:blank);background-attachment:fixed}
* html .ie6fixedTL{position:absolute;left:expression(eval(document.documentElement.scrollLeft));top:expression(eval(document.documentElement.scrollTop))}
body {margin:0; padding:0; background:#fff;color:#494949; font-size:12px; font-family:"微软雅黑";}
html{-webkit-text-size-adjust: none;}
div,form,img,ul,ol,li,dl,dt,dd,p,span,b,i{margin: 0; padding: 0; border: 0; font-style:normal;}
li,dl{list-style-type:none;}
h1,h2,h3,h4,h5,h6 { margin:0; padding:0;}
a:link {color: #494949; text-decoration:none;}
a:visited {color: #494949; text-decoration:none;}
a:hover {color: #016bb5;text-decoration:none; }
a:active {color: #494949; text-decoration:none;}
a,img,input{ outline:none;star:expression(this.onFocus=this.blur());}
.left{ float:left;}
.right{ float:right}


 /* 整表格样式的设定 */
.contract{ width:570px; height:420px; position:absolute; top:50%; left:50%; margin: -230px 0 0 -290px; color:#898989}
.contract_t h2{ position:relative;width:556px; height:32px; line-height:32px; background:#f1f1f1; padding-left:14px; font-size:14px; font-weight:bold; color:#545454;}
.contract_ico{ width:9px; height:9px;background:url(images/ico.png) -13px 0px; display:block; position:absolute; top:10px; right:13px;}

.zbt{ width:564px; padding-left:14px; padding-top:14px;font-size:12px; }
.contract_b{ color:#545454;}
.orange{ color:#ff7200; font-weight:bold;}
.nr-list{width:564px; padding-left:14px; padding-top:5px;}
.li{width:540px; height:52px;border-bottom:dashed #e5e5e5 1px;  line-height:50px; position:relative;}
.li2{width:540px; height:96px;border-bottom:dashed #e5e5e5 1px;padding-top:5px;line-height:40px; position:relative;}
.li3{width:540px; height:52px;line-height:50px; position:relative;}

.ico1{ width:32px; height:30px; background:url(images/ico.png) -6px -41px; position:absolute; top:8px; left:10px;display:inline-block;}
.ico2{ width:29px; height:25px; background:url(images/ico.png) -7px -105px; position:absolute; top:8px; left:10px;display:inline-block;}
.ico3{ width:35px; height:28px; background:url(images/ico.png) -4px -160px; position:absolute; top:8px; left:10px;display:inline-block;}
.ico4{ width:29px; height:30px; background:url(images/ico.png) -7px -220px; position:absolute; top:8px; left:10px;display:inline-block;}
.ico5{ width:30px; height:34px; background:url(images/ico.png) -7px -279px; position:absolute; top:8px; left:10px;display:inline-block;}

.title{ width:80px; text-align:center; color:#545454; display:block; padding-left:45px;}
.a1{ width:380px; position:relative;}
.ico_s1{ width:34px; height:23px;background:url(images/ico.png) -10px -341px no-repeat;position:absolute; display:inline-block; top:10px; left:-30px;}
.ico_s2{ width:34px; height:27px;background:url(images/ico.png) -6px -391px no-repeat;position:absolute; display:inline-block; top:10px;left:-30px;}
.ico_s3{ width:37px; height:19px;background:url(images/ico.png) -1px -447px no-repeat;position:absolute; display:inline-block; top:10px;}
</style>


<div class="contract">
 <div class="zbt"><i class="">交易流程：</i></div> 
  <ul class="nr-list">
	<li class="li">
    	<div class="title left"><i class="ico1"></i>订单生效</div>
        <div class="left">订单提交后，厂商<b class="orange">审核后生效</b>。</div>
    </li>   
    <%--<li class="li2">
    	<div class="title left"><i class="ico2"></i>订单发货</div>
        <div class="left">
        	<div class="a1">订单金额在<b class="orange">信用额度</b>之内的，不用付款厂商即安排发货。</div>
            <div class="a1">下单后在<b class="orange"><%=PayDay%>天内</b>付款，否则订单自动作废，付款后厂商安排发货。</div>
        </div>
    </li>--%>
    <li class="li">
    	<div class="title left"><i class="ico3"></i>订单签收</div>
        <div class="left">请在<b class="orange"><%=SignDay%>天内</b> 确认收货，否则自动签收。</div>
    </li>
    
  </ul>

  <div class="zbt"><i class="">支付方式：</i></div> 
    <ul class="nr-list">
	<li class="li">
    	<div class="title left"><i class="ico4"></i>普通支付</div>
        <div class="left">代理商支付货款后，T+1天后结算到厂商账户。<i class="ico_s3 left"></i></div>
    </li>   
   
    <li class="li3">
    	<div class="title left"><i class="ico5"></i>担保支付</div>
        <div class="left">代理商支付货款，并且收货以后，T+1天后结算到厂商账户。<i class="ico_s3 left" style=" display: none"></i></div>
    </li>
    
  </ul>
</div>
</body>
</html>

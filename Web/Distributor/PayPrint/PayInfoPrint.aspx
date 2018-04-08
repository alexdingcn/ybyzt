<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayInfoPrint.aspx.cs" Inherits="PayInfoPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>电子回单打印</title>
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>

    <script>
        $(document).ready(function () {
            $(".cancel").click(function () {
                window.close();
            });

            $(".orangeBtn").click(function () {
                $(".footerBtn").css("display", "none");
                javascript:window.print();
                $(".footerBtn").css("display", "block");
            });
        });
    </script>

<style>
@charset "utf-8";
/* 整站全局公共样式的设定 */
body {margin:0; padding:0; background:#fff;color:#000; font-size:12px; font-family:"微软雅黑";}
html{-webkit-text-size-adjust: none;}
div,form,img,ul,ol,li,dl,dt,dd,p,span,b,i{margin: 0; padding: 0; border: 0; font-style:normal;}
li,dl{list-style-type:none;}h1,h2,h3,h4,h5,h6 { margin:0; padding:0;}
a:link {color: #494949; text-decoration:none;}
a:visited {color: #494949; text-decoration:none;}
a:hover {color: #2e70d3;text-decoration:none;}/*text-decoration:underline;*/
a:active {color: #494949; text-decoration:none;}
a,img,input{ outline:none;/*star:expression(this.onFocus=this.blur())\9\0;star:expression(this.onFocus=this.blur())\0;*/}
/* 其它常用样式的定义 */ 
i,em{ font-style:normal;}
::-ms-clear, ::-ms-reveal{display: none;}/*去掉文本框的叉*/
.box{outline:none;blr:expression(this.onFocus=this.blur()); border:1px solid #ddd; }/*去掉游览器自带文本框边框*/
.none{ display:none;}
table{border-spacing:0;border-collapse:0;}
.le{ float: left;}.ri{ float: right;}.clear{ clear: both;}
.blank5{height:5px; font-size:1px; clear:both;overflow:hidden;}.blank10{height:10px; font-size:1px; clear:both;overflow:hidden;}.blank20{height:20px; font-size:1px; clear:both;overflow:hidden;}
input.box,textarea.box{border:1px solid #ddd; color:#999; font-family:"微软雅黑"; font-size:12px; border-radius:5px; padding:0px 5px;}
.le10{ padding-left:10px; display:inline-block;}.le5{ padding-left:5px; display:inline-block;}
input{color:#666;}

.receiptPrint{ width:900px; margin:50px auto 0px auto; border:1px solid #999; border-bottom:none; position:relative;}
.receiptPrint .title{ text-align:center; padding:20px 0 15px 0; border-bottom:1px solid #999;}
.receiptPrint .title i{ position:relative; top:-11px; font-size:18px; margin-left:10px;}
.receiptPrint table{ width:100%; font-size:16px;}
.receiptPrint th{ height:50px; line-height:50px; border-bottom:1px solid #999; font-weight:normal;}
.receiptPrint td{ border-bottom:1px solid #999;  border-right:1px solid #999; padding:20px 0;}
.receiptPrint .tle{ text-align:left; padding-left:20px; line-height:26px;}
.receiptPrint .tc{ text-align:center;}
.receiptPrint tr td:last-child{border-right:none;}
.receiptPrint .t1{ width:120px;}
.seal{ display:inline-block; position:absolute; right:30px; bottom:20px;}
</style>

</head>
<body>
    <form id="form1" runat="server">
    
    <div class="receiptPrint">
	<div class="title"><span><img src="printlogo.png" height="35" /></span><i>网络支付电子回单</i></div>
	<table border="0" cellspacing="0" cellpadding="0">
      <thead><tr><th colspan="2"><div class="tle">回单编号：<asp:Label ID="lblNo" runat="server" Text=""></asp:Label></div></th><th colspan="2"><div class="tle">支付时间：<asp:Label ID="lblPayDate" runat="server" Text=""></asp:Label></div></th></tr></thead>
      <tbody>
        <tr>
          <td class="t1"><div class="tc">付<br />款<br />方</div></td>
          <td><div class="tc"><asp:Label ID="lblDisName" runat="server" Text=""></asp:Label><br /><%--（中国银行 123321123321）--%></div></td>
          <td class="t1"><div class="tc">收<br />款<br />方</div></td>
          <td><div class="tc"><asp:Label ID="lblCompName" runat="server" Text=""></asp:Label><br /><asp:Label ID="lblBank2" runat="server" Text=""></asp:Label></div></td>
        </tr>
        
        <tr>
          <td class="t1"><div class="tc">金额小写</div></td>
          <td><div class="tc"><asp:Label ID="lblPayPrice" runat="server" Text=""></asp:Label></div></td>
          <td class="t1"><div class="tc">金额大写</div></td>
          <td><div class="tc"><asp:Label ID="lblPayPriceDX" runat="server" Text=""></asp:Label></div></td>
        </tr>
        <tr>
          <td class="t1"><div class="tc">手续费</div></td>
          <td><div class="tc"><asp:Label ID="lblSXFPrice" runat="server" Text=""></asp:Label></div></td>
          <td class="t1"><div class="tc">币种</div></td>
          <td><div class="tc">人民币</div></td>
        </tr>
        <tr>
          <td class="t1"><div class="tc">订/帐单号</div></td>
          <td><div class="tc"><asp:Label ID="lblOrderNo" runat="server" Text=""></asp:Label></div></td>
          <td class="t1"><div class="tc">资金用途</div></td>
          <td><div class="tc">支付订单</div></td>
        </tr>
        <tr>
          <td colspan="4"><div class="tle"><p>重要提示：</p>1、此凭证用户可自行打印，但仅供参考，实际交易信息以银行盖章回单为准；<br />2、此凭证仅说明该笔汇款当前状态，不代表实际入账情况，不作为收款方发货依据；<br />3、每笔支付编号唯一，请仔细核对，避免重复打印。</div></td>    
        </tr>
        </tbody>
    </table>
    <div class="seal"><img src="seal.png" /></div>
</div>
    
    <br /><br />
    <div class="footerBtn" style="display:none;">
        <input name="" type="button" class="orangeBtn" value="打印" />
         &nbsp;
        <input name="" type="button" class="cancel" value="关闭" />
    </div>
    </form>
</body>
</html>

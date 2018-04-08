<%@ Page Language="C#" AutoEventWireup="true" CodeFile="appdown.aspx.cs" Inherits="AppDown" EnableViewState="false" %>
<%@ OutputCache Duration="180" VaryByParam="none" Location="Server"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>医站通APP下载</title>
    <style>
    #weixin_tip{display:none;position:fixed;left:0;top:0;background:rgba(0,0,0,0.8);filter:alpha(opacity=80);width:100%;height:100%;z-index:100;}
#weixin_tip p{text-align:center;margin-top:10%;position:relative;}
#weixin_tip img{ width:100%;}
#weixin_tip .close{color:#fff;padding:5px;font:bold 60px/70px simsun;text-shadow:0 1px 0 #ddd;position:absolute;top:0;left:5%;}
    </style>
    <script type="text/javascript">
    <% if(Is_Weixin) {%>
//        window.onload = function () {	
//            var winHeight = typeof window.innerHeight != 'undefined' ? window.innerHeight : document.documentElement.clientHeight; //兼容IOS，不需要的可以去掉
//            var tip = document.getElementById('weixin_tip');
//            var close = document.getElementById('close');
//            tip.style.height = winHeight + 'px'; //兼容IOS弹窗整屏
//            tip.style.display = 'block';
//            close.onclick = function () {
//                tip.style.display = 'none';
//            }
//        }
            window.location.href= "http://a.app.qq.com/o/simple.jsp?pkgname=com.yzt.app.h5";
      <% } else if(IsPhoneMobileBrower){ %>
                window.location.href= "https://itunes.apple.com/cn/app/%E5%8C%BB%E7%AB%99%E9%80%9A/id1355304363";
            <%} %>
    </script>
</head>
<body>
 <form id="form1" runat="server">
    <div id="weixin_tip" runat="server" visible="false"><p><img src="/images/Android_Weixin.png" runat="server" id="ImgMobile" alt="微信扫描打开APP下载链接提示代码优化" /><span id="close" title="关闭" class="close">×</span></p></div>
    </form>
</body>
</html>

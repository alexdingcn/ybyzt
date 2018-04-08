<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.ascx.cs" Inherits="Admin_UserControl_Header" %>

<!--header start-->
<div class="header">
  <div class="logo">平台管理后台</div>
  <div class="topright">
    <ul>
      <li><a href="#" target="_parent"><%=login_Tname%>（<%=login_name%>）</a></li>
      <li><a href="../../Admin/index.aspx" target="_parent">我的桌面</a></li>
      <li><a href="../../Admin/changePwd.aspx" target="_parent">修改密码</a></li>
      <li><a href="../../index.html" target="_blank">医站通首页</a></li>
      <li><a href="../../Admin/loginout.aspx" target="_parent" onclick="return confirm('您是否确退出本系统！');"> 退出</a></li>
    </ul>
  </div>
</div>
<!--header end-->

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisUserInfo.aspx.cs" Inherits="Admin_Systems_DisUserInfo" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>代理商管理员查询</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">代理商管理</a><i>></i>
            <a href="#">代理商管理员查询</a>
     </div>
      <div class="tools">
          <ul class="toolbar left">
               <li id="lblbtnback" onclick="javascript:history.go(-1);"><span><img src="../../Company/images/tp3.png" /></span>返回</li>
          </ul>
            </div>

            <div class="div_content">
                  <table class="tb" >
                   <tbody>
                      <tr>
                      <td style="width:15%;"><span>代理商名称</span> </td>
                      <td style="width:30%;"><label runat="server" id="lblDisName"></label></td>
                      <td style="width:15%;"><span>登录帐号</span> </td>
                      <td style="width:30%;"><label runat="server" id="lblUname"></label></td>
                      </tr>

                      <tr>
                       <td><span>姓名</span> </td>
                      <td><label runat="server" id="lblTrueName"></label></td>
                      <td><span>微信帐号</span> </td>
                      <td><label runat="server" id="lblOpenID"></label></td>
                      </tr>

                      <tr>
                      <td><span>性别</span> </td>
                      <td><label runat="server" id="lblSex"></label></td>
                      <td><span>手机号码</span> </td>
                      <td><label runat="server" id="lblPhone"></label></td>
                      </tr>

                      <tr>
                       <td><span>电话</span> </td>
                      <td><label runat="server" id="lblTel"></label></td>
                      <td><span>身份证</span> </td>
                      <td><label runat="server" id="lblIdentitys"></label></td>
                      </tr>

                      <tr>
                      <td><span>状态</span> </td>
                      <td><label runat="server" id="lblIsEnabled"></label></td>
                      <td><span>邮箱</span> </td>
                      <td><label runat="server" id="lblEmail"></label></td>
                      </tr>

                   </tbody>
                  </table>
               </div>


    </div>
    </form>
</body>
</html>

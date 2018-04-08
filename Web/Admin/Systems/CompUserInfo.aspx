<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompUserInfo.aspx.cs" Inherits="Admin_Systems_CompUserInfo" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>企业用户维护</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            var div = $('<div   Msg="True"  style="z-index:10; background-color:#000; opacity:0.5; filter:alpha(opacity=50);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');

            $("#libtnEdit").on("click", function () {
                location.href = 'CompUserEdit.aspx?KeyID=<%=KeyID %>';
            })
        })

    </script>
</head>
<body>
    <form id="form1" runat="server">
     <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">企业管理</a><i>></i>
            <a href="#" runat="server" id="Atitle">企业用户维护</a>
        </div>
                <div class="tools">
                  <ul class="toolbar left">
                   <li id="libtnEdit"><span><img src="../../Company/images/t02.png" /></span>编辑</li>
                   <li id="lblbtnback" onclick="javascript:window.location.href='CompUserList.aspx';"><span><img src="../../Company/images/tp3.png" /></span>返回</li>
                  </ul>
                 </div>

                      <div class="div_content">

                                         <table  class="tb" id="tab1">
                       <tbody>
                          <tr>
                          <td style=" width:15%;"><span>登录帐号</span> </td>
                          <td style=" width:25%;"> <label runat="server" id="lblUserName"></label>  </td>
                          <td style=" width:15%;"><span>姓名</span> </td>
                          <td style=" width:25%;"><label runat="server" id="lblTrueName"></label>   </td>
                         </tr>

                       <tr>
                       <td ><span>所属企业</span></td >
                      <td><label runat="server" id="lblCompName"></label> </td>
                      <td ><span>微信帐号</span></td >
                      <td><label runat="server" id="lblOpenID"></label> </td>
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
                      <td><span>是否启用</span> </td>
                      <td><label runat="server" id="lblIsEnabled"></label></td>
                      <td><span>邮箱</span> </td>
                      <td><label runat="server" id="lblEmail"></label></td>
                      </tr>


                      <tr>
                      <td ><span>详细地址</span></td >
                      <td colspan="3"><label runat="server" id="lblAddress"></label> </td>
                     </tr>

                       </tbody>
                      </table>
                      </div>
         </div>


    


    </form>
</body>
</html>

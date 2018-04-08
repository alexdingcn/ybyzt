<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompUserEdit.aspx.cs" Inherits="Admin_Systems_CompUserEdit" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>企业用户维护</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script>
        function formCheck() {
            var str = "";
            if ($.trim($("#txtMobil").val()) == "") {
                str = "手机号码不能为空。";
            }
            else if (!IsMobile($.trim($("#txtMobil").val()))) {
                str = "手机号码不正确。";
            }
            else if ($.trim($("#txtUpwd").val()).length < 6) {
                str = "登录密码不能少于6位。";
            }
            if (str != "") {
                errMsg("提示", str, "", "");
                return false;
            }
            return true;
        }
    
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
               <div class="div_content">

                  <table  class="tb">
                   <tbody>
                     <tr>
                       <td style=" width:15%;"><span><i class="required">*</i>登录帐号</span> </td>
                       <td style=" width:25%;"> <input runat="server"  type="text" maxlength="50" class="textBox" id="txtUname" disabled="disabled" />  </td>
                       <td style=" width:15%;"><span>姓名</span> </td>
                       <td style=" width:25%;"><input runat="server"  type="text" maxlength="50" class="textBox" id="txtTrueName" /> </td>
                     </tr>
                     <tr>
                      <td> <span>微信帐号</span></td>
                       <td> <input runat="server"  type="text" maxlength="50" class="textBox" id="txtOpenID" />  </td>
                       <td><span>性别</span> </td>
                       <td>&nbsp;  <input type="radio" runat="server"  name="Person" value="1" checked="true"  id="rdMan" />男 &nbsp;&nbsp;&nbsp;<input runat="server" id="rdWomen" name="Person" type="radio"  value="0" />女 </td>
                     </tr>
                     <tr>
                       <td><span><i class="required">*</i>手机号码</span></td>
                       <td> <input runat="server"  type="text" maxlength="11" disabled="disabled" class="textBox" id="txtMobil" />  </td>
                       <td><span>电话</span></td>
                       <td> <input runat="server"  type="text" maxlength="20" class="textBox" id="txtPhone" />  </td>
                     </tr>

                     <tr>
                       <td><span>身份证号码</span> </td>
                       <td> <input runat="server"  type="text" maxlength="50" class="textBox" id="txtPId" /> </td>
                       <td><span><i class="required">*</i>是否启用</span> </td>
                       <td>&nbsp;  <input type="radio" runat="server"  name="IsEnabled" value="1" checked="true"  id="rdEnabledYes" />是 &nbsp;&nbsp;&nbsp;<input runat="server" id="rdEnabledNo" name="IsEnabled" type="radio"  value="0" />否</td>
                     </tr>

                     <tr>
                       <td><span>邮箱</span></td>
                       <td > <input runat="server"  type="text" maxlength="50" class="textBox" id="txtEmail" /></td>
                       <td><span>登录密码</span></td>
                       <td><asp:TextBox ID="txtUpwd" TextMode="Password" maxlength="50"  runat="server"  CssClass="textBox"></asp:TextBox></td>
                     </tr>

                     <tr>
                      <td><span>详细地址</span></td >
                      <td colspan="3"><input runat="server" maxlength="100" style=" width:600px;" type="text"  class="textBox" id="txtAddress" /> </td>
                     </tr>

                   </tbody>
                  </table>

               </div>

                   <div  class="div_footer">
                    <asp:Button ID="btnEdit" CssClass="orangeBtn" runat="server" Text="确定"  OnClientClick="return formCheck()" onclick="btnEdit_Click" />&nbsp;
                    <input name="" type="button" class="cancel" onclick="javascript:history.go(-1);" value="返回" />
               </div>

               </div>

    </form>
</body>
</html>

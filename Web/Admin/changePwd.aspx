<%@ Page Language="C#" AutoEventWireup="true" CodeFile="changePwd.aspx.cs" Inherits="Admin_changePwd" %>
<%@ Register src="UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/layer.js" type="text/javascript"></script>
    <script src="../js/CommonSha.js" type="text/javascript"></script>
    <script src="../js/CommonJs.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ChkPage() {
            if ($("#txtOldPassWord").val() == "") {
                errMsg("提示", "- 原密码不能为空", "", "");
                return false;
            } 
            else {
                if ($("#txtNewPassWord").val() == "") {
                errMsg("提示", "- 新密码不能为空", "", "");
                return false;
            }
            else {
                if ($("#txtConfrimNewPassWord").val() == "") {
                    errMsg("提示", "- 确认密码不能为空", "", "");
                    return false;
                }
                else {
                        if ($.trim($("#txtNewPassWord").val()).length < 6) {
                            errMsg("提示", "- 新密码不能少于6位", "", "");
                            return false;
                        }
                        else {
                            if ($.trim($("#txtConfrimNewPassWord").val()) != $.trim($("#txtNewPassWord").val())) {
                                errMsg("提示", "- 密码填写不一致", "", "");
                                return false;
                            }
                            else {
                                confirm("您确认要修改吗？", function () {
                                    $("#txtOldPassWord").val(hex_two($("#txtOldPassWord").val()));
                                    $("#txtNewPassWord").val(hex_two($("#txtNewPassWord").val()));
                                    $("#txtConfrimNewPassWord").val(hex_two($("#txtConfrimNewPassWord").val()));
                                    $("#btnModify").trigger("click");
                                }, "提示");
                            }
                        }
                }
            }                
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
	        <a href="index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="changePwd.aspx">修改密码</a>
     </div>
     <asp:Button ID="btnModify" runat="server" Style="display: none" Text="修改密码" OnClick="btnModify_Click" />
           <div class="tools">
                    <ul class="toolbar left">
                        <li id="liModify" onclick="return ChkPage()"><span><img src="../Company/images/t02.png" /></span>确定修改</li>
                    </ul>
            </div>
               <div class="div_content">
                  <table class="tb" >
                   <tbody>
                      <tr>
                      <td style="width:20%;"><span><i class="required">*</i>原密码</span> </td>
                      <td style="width:70%;"><input type="password" id="txtOldPassWord" runat="server" maxlength="30" class="textBox" width="224"/></td>
                      </tr>
                      <tr>
                       <td><span><i class="required">*</i>新密码</span> </td>
                      <td><input type="password" id="txtNewPassWord" runat="server" class="textBox" maxlength="30" width="224"/></td>
                      </tr>
                      <tr>
                      <td><span><i class="required">*</i>确认密码</span> </td>
                      <td><input type="password" id="txtConfrimNewPassWord" runat="server" class="textBox" maxlength="30" width="224"/></td>
                      </tr>
                   </tbody>
                  </table>
               </div>
        </div>
    </form>
</body>
</html>

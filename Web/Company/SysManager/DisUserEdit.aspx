<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisUserEdit.aspx.cs" Inherits="Company_SysManager_DisUserEdit" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>代理商管理员新增</title>
   <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
   <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
   <script src="../js/js.js" type="text/javascript"></script>
   <script src="../../js/CommonJs.js" type="text/javascript"></script>
   <script src="../../js/layer/layer.js" type="text/javascript"></script>
   <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script>
        $(document).ready(function () {
            $_def.ID = "btnEdit";
        });

        function formCheck() {
            var str = "";
            if ($.trim($("#txtMobil").val()) == "") {
                str = "找回手机号不能为空。";
            }
            else if (!IsMobile($.trim($("#txtMobil").val()))) {
                str = "手机号码不正确。";
            }
            else if ($.trim($("#txtUpwd").val()).length < 6) {
                str = "登录密码不能少于6位。";
            }
            if (str != "") {
                layerCommon.msg(str, IconOption.错误);
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-4" />
        <div class="rightinfo">
            <div class="place">
	            <ul class="placeul">
                    <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                    <li><a href="../SysManager/DisUserList.aspx">代理商管理员查询</a></li><li>></li>
                    <li><a href="#">代理商管理员新增</a></li>
	            </ul>
          </div>
               <div class="div_content">

                  <div class="div_title" >
                   代理商管理员基本信息:
                  </div>

                  <table  class="tb">
                   <tbody>
                     <tr>
                       <td width="140px"><span><i class="required">*</i>登录帐号</span> </td>
                       <td> <input runat="server"  type="text" maxlength="50" class="textBox" id="txtUname" disabled="disabled" />  </td>
                       <td width="140px"><span>姓名</span> </td>
                       <td><input runat="server"  type="text" maxlength="50" class="textBox" id="txtTrueName" /> </td>
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
                      <td colspan="3"><input runat="server"  style=" width:600px;" maxlength="100" type="text"  class="textBox" id="txtAddress" /> </td>
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

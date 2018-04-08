<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgUserEdit.aspx.cs" Inherits="Admin_OrgManage_OrgUserEdit" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
          <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script>
        function formCheck() {
            var str = "";
            if ($.trim($("#txtTrueName").val()) == "") {
                str = "姓名不能为空";
            }
            else if ($.trim($("#txtPhone").val()) == "") {
                str = "手机号不能为空。";
            }
            else if (!IsMobile($("#txtPhone").val())) {
                str = "手机号不正确。";
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
            <a href="#">机构管理</a><i>></i>
            <a href="#" runat="server" id="Atitle">机构新增</a>
    </div>
          <div class="tools" runat="server" id="divshow">
            <ul class="toolbar left">
              <li id="lblbtnback" onclick="javascript:history.go(-1);" ><span><img src="../../Company/images/tp3.png" /></span>返回</li>
            </ul>
           </div>


            <div class="div_content">
               <table  class="tb">
                 <tbody>
                   <tr>
                       <td style=" width:10%;"><span><i class="required">*</i>登录帐号</span> </td>
                       <td style=" width:30%;" nowrap="nowrap"> <input runat="server"  type="text" maxlength="50"  disabled="disabled"  class="textBox" id="txtLoginName" /> </td>
                       <td style=" width:10%;"><span><i class="required">*</i>姓名</span> </td>
                       <td style=" width:30%;" nowrap="nowrap"> <input runat="server" onkeypress="KeyPress(event)"  type="text" maxlength="50"   class="textBox" id="txtTrueName" />  </td>
                    </tr>

                     <tr>
                       <td><span><i class="required">*</i>手机号</span> </td>
                       <td> <input runat="server" onkeypress="KeyPress(event)"  disabled="disabled"  type="text" maxlength="11" class="textBox" id="txtPhone" />  </td>
                       <td><span>是否启用</span> </td>
                       <td>&nbsp;  <input type="radio" runat="server" checked="true"  name="IsEbled" value="1"   id="rdEbleYes" />启用 &nbsp;&nbsp;&nbsp;<input runat="server" id="rdEbleNo" name="IsEbled" type="radio"  value="0" />禁用 </td>
                       </tr>

                    <tr>
                       <td><span>登录密码</span> </td>
                       <td> <asp:TextBox ID="txtUpwd" TextMode="Password"  maxlength="50"  runat="server"  CssClass="textBox"></asp:TextBox>   </td>
                    </tr>
                 </tbody>
                 </table>

                     <div  class="div_footer">
                <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定"  OnClientClick="return formCheck()" onclick="btnAdd_Click" />&nbsp;
                <input name="" type="button" class="cancel" onclick="javascript:history.go(-1);" value="返回" />
           </div>

           </div>
        </div>
    </form>
</body>
</html>

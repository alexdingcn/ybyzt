<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgEdit.aspx.cs" Inherits="Admin_OrgManage_OrgEdit" %>
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
              if ($.trim($("#txtOrgName").val()) == "") {
                  str = "机构名称不能为空。";
              }
              else if ($.trim($("#txtPhone").val()) != "" && !IsMobile($("#txtPhone").val())) {
                  str = "联系人手机不正确。";
              }
              else if ($.trim($("#txtUsername").val()) == "") {
                  str = "登录帐号不能为空。";
              }
              else if ($.trim($("#txtUserPhone").val()) == "") {
                  str = "请输入管理员手机号";
              }
              else if (!IsMobile($.trim($("#txtUserPhone").val()))) {
                  str = "管理员手机号码不正确。";
              }
              else if ($.trim($("#txtUpwd").val()).length < 6) {
                  str = "用户密码不能少于6位。";
              }
              else if ($.trim($("#txtUpwd").val()) != $.trim($("#txtUpwds").val())) {
                  str = "确认密码不一致";
              }
              else if ($.trim($("#txtUserTrueName").val()) == "") {
                  str = "管理员姓名不能为空。";
              }
              if (str != "") {
                  errMsg("提示", str, "", "");
                  return false;
              }
              return true;

          }

          function phoneValue(obj) {
              if (!IsMobile($.trim($("#txtUserPhone").val()))) {
                  $("#txtUserPhone").val(obj.value);
              }
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
                       <td style=" width:8%;"><span><i class="required">*</i>机构名称</span> </td>
                       <td style=" width:30%;" nowrap="nowrap" colspan="3"> <input runat="server" onkeypress="KeyPress(event)"  type="text" maxlength="50"   class="textBox" id="txtOrgName" /> </td>
                       <td style=" width:8%;"><span>联系人手机</span> </td>
                       <td style=" width:30%;" nowrap="nowrap" colspan="3"> <input runat="server" onkeypress="KeyPress(event)" onblur="phoneValue(this)"  type="text" maxlength="11" class="textBox" id="txtPhone" />  </td>
                       </tr>

                        <tr>
                       <td style=" width:8%;"><span>联系人</span> </td>
                       <td style=" width:30%;" nowrap="nowrap" colspan="3"> <input runat="server" onkeypress="KeyPress(event)"  type="text" maxlength="50"   class="textBox" id="txtPrincipal" /> </td>
                       <td style=" width:8%;"><span>排序</span> </td>
                       <td style=" width:30%;" nowrap="nowrap" colspan="3"> <input runat="server"  type="text" maxlength="50"   class="textBox" id="txtSortIndex" />  </td>
                       </tr>

                       <tr>
                       <td style=" width:8%;"><span>是否启用</span> </td>
                       <td nowrap="nowrap" colspan="6">&nbsp;  <input type="radio" runat="server" checked="true"  name="IsEbled" value="1"   id="rdEbleYes" />启用 &nbsp;&nbsp;&nbsp;<input runat="server" id="rdEbleNo" name="IsEbled" type="radio"  value="0" />禁用 </td>
                       
                       </tr>

                        <tr>
                       <td style=" width:8%;"><span>备注</span> </td>
                           <td colspan="6">  <textarea  style=" height:auto; width:600px;" rows="3" class="textBox" maxlength="2000" runat="server" id="txtRemark"></textarea> </td>
                       </tr>

                  </tbody>
                </table>

                <div class="div_title">
                  机构管理员用户:
               </div>

                 <table class="tb">
                    <tbody>
                      <tr>
                        <td style=" width:10%;"><span><i class="required">*</i>登录帐号</span></td >
                       <td style=" width:30%;" nowrap="nowrap"> <input runat="server"  type="text" maxlength="50" class="textBox" id="txtUsername" /><i class="grayTxt">（2-20个字符，支持字母、数字或下划线）</i>  </td>
                        <td style=" width:10%;"> <span><i class="required">*</i>手机号码</span></td>
                       <td style=" width:30%;" nowrap="nowrap"><input runat="server"  type="text" maxlength="11" class="textBox" id="txtUserPhone" /> <i class="grayTxt">（非常重要，登录、发送验证短信使用）</i>  </td>
                      </tr>
                      <tr>
                       <td ><span><i class="required">*</i>登录密码</span> </td >
                       <td><asp:TextBox ID="txtUpwd" TextMode="Password"  maxlength="50"  runat="server"  CssClass="textBox"></asp:TextBox> 
                        <i class="grayTxt" id="UpwTitle" runat="server">（新增时默认为123456，可更改）</i>  
                       </td>
                          <td ><span><i class="required">*</i>确认密码</span></td >
                       <td><asp:TextBox ID="txtUpwds" TextMode="Password" maxlength="50"  runat="server"  CssClass="textBox"></asp:TextBox>
                       <i class="grayTxt" id="UpwsTitle" runat="server">（新增时默认为123456，可更改）</i>  
                       </td>
                      </tr>
                      <tr>
                        <td><span><i class="required">*</i>姓名</span></td >
                       <td colspan="3">  <input runat="server"  type="text" maxlength="50" class="textBox" id="txtUserTrueName" /> <i
                                    class="grayTxt">（请填写真实姓名，以便更好地为您服务）</i> </td>
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

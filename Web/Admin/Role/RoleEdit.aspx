<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoleEdit.aspx.cs" Inherits="Admin_Role_RoleEdit" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>岗位权限维护新增</title>
   <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>


    <script>
        $(document).ready(function () {
            $_def.ID = "btnAdd";
            $('.tb tbody tr td:even').addClass('odd');
            if ('<%=Request["nextstep"] %>' == "1") {
                document.getElementById("imgmenu").style.display = "block";
            }
        })

        function formCheck() {
            var str = "";
            if ($.trim($("#txtRoleName").val()) == "") {
                str = "岗位名称不能为空！";
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
    <%--<uc1:CompRemove runat="server" ID="Remove" ModuleID="1" />
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />--%>
<uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#"  runat="server" id="atitle">岗位权限维护</a><i>></i>
            <a href="#" runat="server" id="btitle">岗位权限维护新增</a>
        </div>
               <div class="div_content">

               <div class="div_title" >
               
               </div>
                  <table  class="tb">
                   <tbody>
                     <tr>
                       <td style=" width:140px;"><span><i class="required">*</i>岗位名称</span></td>
                       <td><asp:TextBox ID="txtRoleName" type="text" maxlength="20" class="textBox" runat="server"></asp:TextBox>
                       <i class="grayTxt">（例如：录单岗、审核岗、财务岗） </i></td>
                     </tr>
                     <tr>
                       <td><span><i class="required">*</i>是否启用</span> </td>
                       <td> &nbsp;  <input type="radio" runat="server" name="audit" value="1" checked="true"  id="rdAuditYes" />启用&nbsp;&nbsp;&nbsp;
                       <input runat="server" id="rdAuditNo" name="audit" type="radio"  value="0" />禁用</td>
                     </tr>
                     <tr>
                     <td ><span>备  注</span></td >
                      <td>  <textarea  style=" height:auto; width:500px; margin:8px 6px;" rows="5" maxlength="100" class="textBox" runat="server" id="txtRemark"></textarea> </td>
                     </tr>
                     <tr>
                       <td><span>排  序</span></td>
                       <td> <input runat="server"  type="text"  class="textBox" id="txtSortIndex" maxlength="10" style=" width:35px; padding-right:5px; text-align:center;" /><i class="grayTxt">（列表顺序排列）</i></td>
                     </tr>
                   </tbody>
                  </table>
               </div>

               <div  class="div_footer" style="max-width:510px; margin-left:147px; text-align:left;">
                    <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="下一步"  OnClientClick="return formCheck()" onclick="btnAdd_Click" />&nbsp;
                    <input name="" type="button" class="cancel" onclick="javascript:history.go(-1);" value="返回" />
               </div>
             </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgUserInfo.aspx.cs" Inherits="Admin_OrgManage_OrgUserInfo" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.idTabs.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {

            $("li#libtnEdit").on("click", function () {
                location.href = 'OrgUserEdit.aspx?S=1&KeyID=<%=KeyID %>&type=<%=Request["type"] %>';
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
            <a href="#" runat="server" id="Btitle">机构管理</a><i>></i>
            <a href="#" runat="server" id="Atitle">机构用户查看</a>
      </div>
           <div class="tools">
                  <ul class="toolbar left">
                   <li id="libtnEdit" runat="server"><span><img src="../../Company/images/t02.png" /></span>编辑</li>
                   <li id="lblbtnback" onclick="javascript:history.go(-1);" runat="server"><span><img src="../../Company/images/tp3.png" /></span>返回</li>                  
                  </ul>
            </div>

            <div class="div_content">
                <table  id="tab1" class="tb">
                       <tbody>
                          <tr>
                          <td style=" width:15%;"><span>登录帐号</span> </td>
                          <td style=" width:25%;"> <label runat="server" id="lblLoginName"></label>  </td>
                          <td style=" width:15%;"><span>姓名</span> </td>
                          <td style=" width:25%;"><label runat="server" id="lblTrueName"></label>  </td>
                         </tr>

                          <tr>
                          <td ><span>所属机构</span> </td>
                          <td ><label runat="server" id="lblOrgName"></label> </td>
                           <td ><span>用户类型</span> </td>
                          <td ><label runat="server" id="lblUtype"></label> </td>
                         </tr>

                         <tr>
                          <td ><span>是否启用</span> </td>
                          <td > <label runat="server" id="lblIsEnabled"></label>  </td>
                          <td ><span>手机号</span> </td>
                          <td > <label runat="server" id="lblPhone"></label>  </td>
                         </tr>

                         <tr>
                          <td ><span>备注</span></td >
                          <td colspan="3"> <label runat="server" id="lblRemark"></label>   </td>
                        </tr>

                        </tbody>
                     </table>

            </div>



       </div>
    </form>
</body>
</html>

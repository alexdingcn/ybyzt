<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserInfo.aspx.cs" Inherits="Company_UserContro_UserInfo" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>员工帐号详情</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("li#libtnDel").on("click", function () {
                layerCommon.confirm("确认禁用？", function () { $("#btnDel").trigger("click"); }, "提示");
            })

            $("li#libtnDelete").on("click", function () {
                layerCommon.confirm("确认删除？", function () { $("#btnDelete").trigger("click"); }, "提示");
            })

            $("li#lblbtnSendOut").on("click", function () {
                layerCommon.confirm("确认发送？", function () { $("#btnSendOut").trigger("click"); }, "提示");
            })

            $("li#libtnUse").on("click", function () {
                layerCommon.confirm("确认启用？", function () { $("#btnUse").trigger("click"); }, "提示");
            })
            $("li#libtnEdit").on("click", function () {
                var keyid= $("#txt_keyid").val();
                location.href = "UserEdit.aspx?usertype=2&KeyId="+keyid;
            })
            $("li#lblbtnback").on("click", function () {
                location.href = "UserList.aspx";
            })

        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
       <input type="hidden" name="txt_keyid" id="txt_keyid" runat="server"/>
      <div class="rightinfo">
        <div class="place">
	            <ul class="placeul">
                    <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                    <li><a href="../SysManager/UserList.aspx" runat="server" id="atitle">员工帐号维护</a></li><li>></li>
                    <li><a href="#" runat="server" id="btitle">员工帐号详细</a></li>
	            </ul>
         </div>

           <div class="tools">
          <ul class="toolbar left">
              <li id="libtnEdit"><span><img src="../images/t02.png" /></span>编辑</li>
              <li id="libtnUse" runat="server" visible="false"><span><img src="../images/nlock.png" /></span>启用<input type="button" runat="server" id="btnUse" onserverclick="btn_Use" style="display:none;"  /></li>
              <li id="libtnDel" runat="server" visible="false"><span><img src="../images/lock.png" /></span>禁用<input type="button" runat="server" id="btnDel" onserverclick="btn_Del" style="display:none;"  /></li>
              <li id="libtnDelete" runat="server" visible="false"><span><img src="../images/t03.png" /></span>删除<input type="button" runat="server" id="btnDelete" onserverclick="btn_Delete" style="display:none;"  /></li>
              <li id="lblbtnSendOut"><span><img src="../images/t13.png" /></span>发送<input type="button" runat="server" id="btnSendOut" onserverclick="btn_SendOut" style="display:none;"  /></li>
              <li id="lblbtnback"><span><img src="../images/tp3.png" /></span>返回</li>
          </ul>
            </div>
               <div class="div_content">
                  <table class="tb" >
                   <tbody>
                     <tr>
                       <%--<td style="width:110px;"><span>岗位</span></td>
                       <td><label runat="server" id="lblRoleId"></label></td>--%>
                       <td style="width:110px;"><span>登录帐号</span> </td>
                       <td><label runat="server" id="lblUserName"></label></td>
                       <td><span>手机号码</span> </td>
                       <td><label runat="server" id="lblPhone"></label></td>  
                     </tr>
                     <%--<tr>
                       <td><span>登录别名</span></td>
                       <td><label runat="server" id="lblUserLoginName"></label></td>
                       <td><span>性别</span> </td>
                       <td><label runat="server" id="lblSex"></label></td>
                     </tr>--%>
                     <tr>
                       <td><span>人员姓名</span></td>
                       <td><label runat="server" id="lblTrueName"></label></td>   
                       <td><span>状态</span> </td>
                       <td><label runat="server" id="lblIsEnabled"></label></td>               
                     </tr>
                     <tr>
                       <td><span>身份证号</span> </td>
                       <td><label runat="server" id="lblIdentitys"></label></td>
                       <td><span>邮箱</span></td>
                       <td><label runat="server" id="lblEmail"></label></td>
                     </tr>
                     <tr>
                       <td><span>家庭地址</span></td>
                       <td colspan="3"><label runat="server" id="lblAddress"></label></td>
                     </tr>
                       <tr>
                       <td><span>用户类型</span></td>
                       <td colspan="3"><label runat="server" id="UserTypes"></label></td>
                     </tr>
                   </tbody>
                  </table>
               </div>
              <div class="div_content" id="MyRole">
                  <div class="div_title" style="margin-top:20px;">岗位权限：</div>
                  <table >
                            <tbody>
                            <tr>
                                <td style="font-size:14px;">
                                    <asp:Repeater ID="RepeaterRoles" runat="server" >
                                        <ItemTemplate>
                                       <input id='Menu_<%#Eval("ID")%>' <%#BindRoleSysFun(Eval("ID")+"") %>  type="checkbox" disabled="disabled" value='<%#Eval("ID")%>' />
                                        <%#Eval("RoleName")%> &nbsp; &nbsp; &nbsp; &nbsp;
                                    </ItemTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                            </tbody>
                        </table>
              </div>
     </div>
    </form>
</body>
</html>

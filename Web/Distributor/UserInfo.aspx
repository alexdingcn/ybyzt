<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserInfo.aspx.cs" Inherits="Distributor_UserInfo" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>代理商后台</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/CommonJs.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#NUseIcon").on("click", function () {
                $("#btnDel").trigger("click");
            })
            $("#UseIcon").on("click", function () {
                $("#btnUse").trigger("click");
            })
            $("#EditIcon").on("click", function () {
                location.href = "UserAdd.aspx?usertype=2&KeyID=<%=Common.DesEncrypt(KeyID.ToString(),Common.EncryptKey) %>&userID=<%=UID %>";
            })
            $("#DleteIcon").on("click", function () {
                layerCommon.confirm("确认删除？", function () { $("#btnDelete").trigger("click"); }, "提示");
            });
            $("#returnIcon").on("click", function () {
                location.href = "UsersList.aspx";
            });

        })
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
        <input type="button" runat="server" id="btnUse" onserverclick="btn_Use" style="display:none;"  />
        <input type="button" runat="server" id="btnDel" onserverclick="btn_Del" style="display:none;"  />
        <input type="button" runat="server" id="btnDelete" onserverclick="btn_Delete" style="display:none;"  />
        <div class="w1200">
            <Head:Head ID="Head2" runat="server" />
            <Left:Left ID="Left1" runat="server" ShowID="nav-6" />
            <div class="rightCon">
                <!--当前位置 start-->
                <div class="info">
                    <a href="../jsc.aspx">我的桌面 </a>>
                    <a href="UsersList.aspx" runat="server" id="atitle">员工帐号维护</a>>
                    <a runat="server" id="a1">详情页面</a>
                </div>
               <div class="userFun">
                   <div class="left">                    
                    <a href="javascript:void(0)" class="btnOr" id="EditIcon" runat="server" >
                        <i class="editIcon"></i>编辑</a>
                    <a href="javascript:void(0)" class="btnBl" id="UseIcon" runat="server">
                        <i class="prnIcon"></i>启用</a> 
                    <a href="javascript:void(0)" class="btnBl" id="NUseIcon" runat="server">
                        <i class="offIcon"></i>禁用</a>
                    <a href="javascript:void(0)" class="btnBl" id="DleteIcon" runat="server">
                        <i class="offIcon"></i>删除</a>
                    <a href="javascript:void(0)" class="btnBl" id="returnIcon" runat="server">
                        <i class="returnIcon"></i>返回</a>
                    <%--<a href="#" class="btnBl" onclick="javascript:window.parent.save();" id="CloseIcon" runat="server">
                        <i class="offIcon"></i>关闭</a>--%>
                </div>
               </div>
            <div class="blank10">
            </div>
            <div class="orderNr">
                <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
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
        </div>
    </form>
</body>
</html>

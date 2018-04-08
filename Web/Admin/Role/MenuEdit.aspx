<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MenuEdit.aspx.cs" Inherits="Admin_Role_MenuEdit" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>权限分配</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $_def.ID = "btnEdit";

            if ('<%=Request["nextstep"] %>' == "1") {
                document.getElementById("imgmenu").style.display = "block";
            }
        })
        //返回
        function btnRetutn_onclick() {
            //location.href = "RoleList.aspx";
            history.go(-1);
        }
        function CheckAll(Id) {

            var chks = document.getElementById("tab_" + Id).getElementsByTagName("input");

            for (var i = 0; i < chks.length; i++) {
                if (document.getElementById("Big_" + Id).checked == true) {
                    chks[i].checked = true;
                } else {
                    chks[i].checked = false;
                }
            }
        }
        
        function CheckMeau(Id, ParentId) {
            //没选中的时候

            var IsNone = $("input[name='Item_" + Id + "']")
            for (j = 0; j < IsNone.length; j++) {
                if (document.getElementById("Menu_" + Id).checked == true) {
                    IsNone[j].checked = true;
                }
                else {
                    IsNone[j].checked = false;
                }
            }
            var IsBig = $("#tab_" + ParentId + " input:checked").length;

            if (IsBig > 0) {
                document.getElementById("Big_" + ParentId).checked = true;
            }
            else {
                document.getElementById("Big_" + ParentId).checked = false;
            }
        }

        function CheckItem(ParentId, ParentParentId) {
            var IsNone = $("input[name='Item_" + ParentId + "']:checked").length;
            if (IsNone > 0) {
                document.getElementById("Menu_" + ParentId).checked = true;
            }
            else {
                document.getElementById("Menu_" + ParentId).checked = false;
            }
            var IsBig = $("#tab_" + ParentParentId + " input:checked").length;
            if (IsBig > 1) {
                document.getElementById("Big_" + ParentParentId).checked = true;
            }
            else {
                document.getElementById("Big_" + ParentParentId).checked = false;
            }
        }


        function MyRoleAll() {
            var chks = document.getElementById("MyRole").getElementsByTagName("input");
            var MyValue = "";
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].checked == true) {
                    MyValue += chks[i].value + ",";
                }
            }
            document.getElementById("HF_MyRole").value = MyValue;
        }
        function KeySort(val) {
            val.value = val.value.replace(/[^\d]/g, "");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
   <%-- <uc1:top ID="top1" runat="server" ShowID="nav-6" />

    <uc1:CompRemove runat="server" ID="Remove" ModuleID="1" />--%>
   <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#" runat="server" id="atitle">岗位权限维护</a><i>></i>
            <a href="#">岗位权限维护详细</a><i>></i>
            <a href="#">分配权限</a>  
    </div>
    
    <div class="main">
        <!--[if !IE]>标题搜索 start<![endif]-->
        <div style="padding-left:20px;" >
            <strong>岗位名称：<b style=" display:inline-block;"><asp:Label ID="lblRoleName" runat="server" Text=""></asp:Label></b></strong>
            <font color="red">
                &nbsp; &nbsp; <b id="b1" runat="server">[请勾选该岗位对应的功能菜单]</b>
            </font>
        </div>
        
        <!--[if !IE]>标题搜索 end<![endif]-->
    </div>
    <div class="content jzAdd"  style=" overflow:hidden;">
        <table width="100%" id="mytables" class="clear" border="0" align="left" cellpadding="6"
            cellspacing="0">
            <tr>
                <td height="25" align="right" valign="middle" colspan="2">
                    <div id="MyRole" class="content">
                        <table align="left" border="0" cellpadding="0" cellspacing="0" width="100%">
                            <asp:Repeater ID="RepeaterPMeau" runat="server" OnItemDataBound="RepeaterPMeau_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td align="left" style="height: 5px;" valign="top">
                                        </td>
                                        <td>&nbsp;
                                            
                                        </td>
                                        <td>&nbsp;
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" width="1%">&nbsp;
                                            
                                        </td>
                                        <td>
                                            <table align="left" bgcolor="#e8e8e8" border="0" cellpadding="0" cellspacing="1"
                                                width="100%">
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <table align="left" border="0" cellpadding="0" cellspacing="0" height="30px" width="100%">
                                                            <tr>
                                                                <td align="left" valign="middle">
                                                                    <table align="left" border="0" cellpadding="0" cellspacing="3" style="margin-left: 15px;
                                                                        display: inline;">
                                                                        <tr>
                                                                            <td align="left" valign="middle">
                                                                                <span class="shop">
                                                                                    <input id="Big_<%#Eval("FunCode")%>" <%#BindRoleSysFun(Eval("FunCode")+"") %> onclick="CheckAll(<%#Eval("FunCode")%>);"
                                                                                        type="checkbox" value='<%#Eval("FunCode")%>' />
                                                                                </span>
                                                                            </td>
                                                                            <td align="left" valign="middle">
                                                                                <strong style="color: #033d91;">
                                                                                    &nbsp; <%#Eval("FunName")%></strong>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr  id="tab_<%#Eval("FunCode")%>">
                                                    <td align="left" bgcolor="#ffffff" style="padding: 5px;">
                                                        <asp:Repeater ID="RepeaterItemMeau" runat="server" OnItemDataBound="RepeaterMeau_ItemDataBound">
                                                            <ItemTemplate>
                                                                <input id='Menu_<%#Eval("FunCode")%>' <%#BindRoleSysFun(Eval("FunCode")+"") %> onclick='CheckMeau(<%#Eval("FunCode")%>,<%#Eval("ParentCode")%>);'
                                                                    type="checkbox" value='<%#Eval("FunCode")%>' />
                                                                <%#Eval("FunName")%>&nbsp;&nbsp;&nbsp;
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div class="footer">
        <div class="footerBtn" style="padding-left: 22px; text-align:center;">
            <asp:Button ID="btnEdit" runat="server" Text="确定" OnClientClick="return MyRoleAll();"
                CssClass="orangeBtn" OnClick="btnEdit_Click" Width="69px" />
            <input type="button" class="cancel" value="返回" style="width:75px;" id="btnRetutn" onclick="return btnRetutn_onclick()"  />
        </div>
        <asp:HiddenField ID="HF_MyRole" runat="server" />
    </div>
    </div>
    </form>
</body>
</html>


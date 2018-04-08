<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoleList.aspx.cs" Inherits="Distributor_RoleList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>设置岗位权限</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script> 
    <script src="../Company/js/order.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#orderBg tr:even').addClass("bg");
        });

        //新增人员
        function GoInfo(Id) {
            var index = layerCommon.openWindow('岗位人员新增', 'UserAdd.aspx?usertype=1&RoleID=' + Id, '900px', '410px');
            $("#hid_Alert").val(index);
        }
        function save() {
            CloseDialog();
        }
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server" method="post">
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="nav-6" />
    <input id="hid_Alert" type="hidden" />
    <div class="rightCon">
    <div class="info"> <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="/Distributor/RoleList.aspx" class="cur">设置岗位权限</a></div>
        <!--功能条件 start-->
        <div class="userFun">
            <div class="left">
                <a href="RoleEdit.aspx" class="btnOr"><i class="addIcon"></i>新增岗位</a></div>
            <div class="right">
                <ul class="term">
                    <li>
                        <label class="head">
                            岗位名称：</label><input id="txtRoleName" runat="server" type="text" class="box" style="width:110px;" maxlength="40" /></li>
                    <li>
                        <label class="head">状态：</label>
                        <asp:DropDownList ID="sltIsAllow" runat="server" Width="72px" CssClass="downBox">
                            <asp:ListItem Value="">全部</asp:ListItem>
                            <asp:ListItem Value="1">启用</asp:ListItem>
                            <asp:ListItem Value="0">禁用</asp:ListItem>
                        </asp:DropDownList>
                    </li>   
                 <li> <label class="head">每页</label><input type="text" onkeyup='KeyInt(this)' onblur="KeyInt(this);" id="txtPageSize" name="txtPager" runat="server" class="box3" /><label class="head">行</label></li>
                </ul>
                <a id="A1" href="" class="btnBl" onserverclick="A_Seek" runat="server"><i class="searchIcon"></i>搜索</a>
            </div>
        </div>
        <!--功能条件 end-->
        <div class="blank10">
        </div>
        <!--岗位权限管理 start-->
        <div class="orderNr">
            <table class="PublicList list" id="orderBg" border="0" cellspacing="0" cellpadding="0">
                <asp:Repeater ID="Rpt_Role" runat="server">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th>
                                    岗位名称
                                </th>
                                <th>
                                    是否启用
                                </th>
                                <th>
                                    创建日期
                                </th>
                                <th>
                                    操作
                                </th>
                            </tr>
                        </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><a style=" text-decoration:underline; " href='RoleInfo.aspx?type=1&KeyId=<%# Common.DesEncrypt(Eval("Id").ToString(), Common.EncryptKey) %>'><%# Eval("RoleName") %></a></td>
                         <td><%# Eval("IsEnabled").ToString()=="1" ? "启用" : "<font color=red>禁用</font>"%></td>
                         <td><%# Eval("CreateDate","{0:yyyy-MM-dd}") %></td>
                         <td>
                             <a href='RoleInfo.aspx?KeyID=<%# Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) %>' title='查看' style="color:#fff;" class="btnYe">查看</a>
                             &nbsp;<a href='MenuEdit.aspx?type=1&RoleId=<%#Eval("Id") %>' title='分配权限' style="color:#fff;" class="btnYe">分配权限</a>&nbsp;
                             <%--<a href='#' onclick='GoInfo(<%# Eval("ID") %>);' title='新增人员' style="color:#fff;" class="btnYe">新增人员</a>--%>
                         </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            
        </div>
        <!--分页 start-->
            <div class="pagin">
                <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5" OnPageChanged="Pager_PageChanged">
            </webdiyer:AspNetPager>
            </div>
            <!--分页 end-->
        <!--岗位权限管理 end-->
    </div>
    </div>
    </form>
</body>
</html>

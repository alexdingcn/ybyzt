<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserList.aspx.cs" Inherits="Company_UserContro_UserList" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>员工帐号维护</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    ChkPage();
                }
            })
        })
        $(document).ready(function () {
            $('.tablelist tbody tr:odd').addClass('odd');
            $("li#libtnAdd").on("click", function () {
                location.href = "UserEdit.aspx";
            })
        })
        // 每页显示数量非空验证
        function ChkPage() {
            if ($("#txtPageSize").val() == "") {
                layerCommon.msg(" 每页显示数量不能为空", IconOption.错误);
                return false;
            } else {
                $("#btnSearch").click();
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:top ID="top1" runat="server" ShowID="nav-6" />
        <div class="rightinfo">

        <!--当前位置 start-->
        <div class="place">
	        <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="../SysManager/UserList.aspx" runat="server" id="atitle">员工帐号维护</a></li>
	        </ul>
        </div>
        <!--当前位置 end--> 
            <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                    <li id="libtnAdd"><span><img src="../images/t01.png" /></span>新增员工</li>
                </ul>
                <div class="right">
                    <ul class="toolbar right">
                        <li onclick="return ChkPage()"><span><img src="../images/t04.png" /></span>搜索</li>
                        <li class="liSenior"><span><img src="../images/t07.png" /></span>高级</li>
                    </ul>
                    <ul class="toolbar3">
                        <li>登录帐号:<input  runat="server" id="txtUserName" type="text" class="textBox"/></li>
                        <li> 人员姓名:<input  runat="server" id="txtTrueName" type="text" class="textBox"/></li>
                        <li> 状 态:
                        <asp:DropDownList ID="sltIsAllow" runat="server" Width="72px" CssClass="textBox">
                            <asp:ListItem Value="">全部</asp:ListItem>
                            <asp:ListItem Value="1">启用</asp:ListItem>
                            <asp:ListItem Value="0">禁用</asp:ListItem>
                        </asp:DropDownList>
                        </li>
                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->

            <div class="hidden">
               <ul >
                  <li>每页<input name="txtPageSize" type="text" class="textBox txtPageSize" id="txtPageSize"
                        style="width: 40px" runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />条 &nbsp; </li>
                  <li> &nbsp; &nbsp;  手机号码:<input  runat="server" id="txtPhone" type="text" class="textBox"/> &nbsp;  &nbsp; </li>
               </ul>
             </div>

            <!--信息列表 start-->
            <table class="tablelist">
                <thead>
                    <tr>
                        <th>登录帐号</th>
                        <th>人员姓名</th>
                        <th>手机号码</th>   
                        <th>身份证</th>                     
                        <th>邮箱</th>
                        <th>状态</th>                        
                    </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="Rpt_Company" runat="server">
                   <ItemTemplate>
                    <tr>
                         <td><div class="tcle"><a style="" href='UserInfo.aspx?KeyId=<%#Common.DesEncrypt(Eval("UserID").ToString(), Common.EncryptKey) %>' ><%# Eval("UserName") %></a></div></td>
                         <td><div class="tc"><%# Eval("TrueName")%></div></td>
                         <td><div class="tc"><%# Eval("Phone")%></div></td>
                         <td><div class="tc"><%# Eval("Identitys")%></div></td>
                         <td><div class="tc"><%# Eval("Email")%></div></td>
                         <td><div class="tc"><%# System.Convert.ToString(DataBinder.Eval(Container.DataItem, "IsEnabled")) == "0" ? "<font color=red>禁用</font>" : "启用"%></div></td>
                    </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <asp:Button ID="btnSearch" runat="server" Style="display: none" Text="搜索" OnClick="btnSearch_Click" />
            <!--信息列表 end-->
            <!--列表分页 start--> 
            <div class="pagin">
            <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                     NextPageText=">"  PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
          ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                       TextBeforePageIndexBox="<span class='jump'>跳转到:</span>"
                   CustomInfoSectionWidth="20%"  CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;"  CustomInfoClass="message" ShowCustomInfoSection="Left"
                  CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                     CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                    OnPageChanged="Pager_PageChanged" >
                </webdiyer:AspNetPager>
        </div>
            <!--列表分页 end--> 
        </div>

    </form>
</body>
</html>

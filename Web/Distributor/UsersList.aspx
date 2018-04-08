<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UsersList.aspx.cs" Inherits="Distributor_UsersList" %>


<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>员工帐号维护</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../js/CommonJs.js"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#NUseIcon").on("click", function () {
                $("#btnDel").trigger("click");
            })
            $("#UseIcon").on("click", function () {
                $("#btnUse").trigger("click");
            })
            $("#EditIcon").on("click", function () {
               <%-- location.href = "UserAdd.aspx?usertype=2&KeyID=<%=Common.DesEncrypt(KeyID.ToString(),Common.EncryptKey) %>&userID=<%=UID %>";--%>
            })
            $("#DleteIcon").on("click", function () {
                layerCommon.confirm("确认删除？", function () { $("#btnDelete").trigger("click"); }, "提示");
            })

        })
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
        <div class="w1200">
            <Head:Head ID="Head2" runat="server" />
            <Left:Left ID="Left1" runat="server" ShowID="nav-6" />
            <div class="rightCon">

            <!--当前位置 start-->
            <div class="info">
                <a href="../jsc.aspx">我的桌面 </a>>
                <a href="UsersList.aspx" runat="server" id="atitle">员工帐号维护</a>
            </div>
                <!--功能按钮 start-->
                <div class="userFun">
                    <div class="left">
                        <a href="UserAdd.aspx" class="btnOr" id="AddIcon" runat="server" >
                            <i class="addIcon"></i>新增员工</a>
                    </div>
                    <div class="right">
                        <ul class="term">
                        <li>
                            <label class="head">登录帐号：</label>
                            <input id="txtUsername" runat="server" type="text" class="box" style="width:110px;" />
                        </li>
                        <li>
                            <label class="head">
                                姓名：</label><input id="txtName" runat="server" type="text" class="box" style="width:110px;" />
                        </li>
                    </ul>
                        <a href="javascript:void(0)" class="btnBl" id="Search"><i class="searchIcon"></i>搜索</a>
                        <a href="javascript:void(0)"  class="btnBl liSenior"><i class="resetIcon "></i>高级</a>
                    </div>
                </div>
                <div class="hidden userFun"  style=" text-align:right;padding-top:10px; display:none; float:right">
                   <ul class="term">
                      <%--<li>每页<input name="txtPageSize" type="text" class="box" id="txtPageSize"
                            style="width: 40px" runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />条 &nbsp; </li>--%>
                       <li>
                           <label class="head">手机号码：</label>
                           <input id="txtPhone" runat="server" type="text" class="box" style="width:110px;" maxlength="40" />
                       </li>
                       <li> 
                           <label class="head">每页</label>
                           <input type="text" onkeyup='KeyInt(this)' onblur="KeyInt(this);" id="txtPageSize" name="txtPageSize" runat="server" class="box3" style="width: 40px" value="12"  /><label class="head">行</label>
                       </li>
                   </ul>
                 </div>
                <!--功能按钮 end-->
            <!--当前位置 end--> 
                <div class="blank10"></div>
                

                <!--信息列表 start-->
                <div class="orderNr">
                    <table class="PublicList list">
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
                                 <td><div class="tcle" style="text-align:center;"><a style="text-decoration:underline;" href='UserInfo.aspx?KeyId=<%#Common.DesEncrypt(Eval("UserID").ToString(), Common.EncryptKey) %>' ><%# Eval("UserName") %></a></div></td>
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
                </div>
                <asp:Button ID="btnSearch" runat="server" Style="display: none" Text="搜索" OnClick="btnSearch_Click" />
                <!--信息列表 end-->
                <div class="blank10"></div>
                <!--列表分页 start--> 
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
                <!--列表分页 end--> 
            </div>
        </div>
    </form>
</body>
</html>

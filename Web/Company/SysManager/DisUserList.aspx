<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisUserList.aspx.cs" Inherits="Company_SysManager_DisUserList" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>代理商管理员查询</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
        <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script>
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btn_Search").trigger("click");
                }
            })
        })
        $(document).ready(function () {
            $("li#liSearch").on("click", function () {
                $("#btn_Search").trigger("click");
            })
            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');

        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-4" />
            <div class="rightinfo">
        
       <div class="place">
	        <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="../SysManager/DisUserList.aspx">代理商管理员查询</a></li>
	        </ul>
        </div>

              <div class="tools">
             

                    <div class="right">
                    <ul class="toolbar right">
                        <li id="liSearch"><span><img src="../images/t04.png" /></span>搜索</li>
                        <%--<uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />--%>
                        <li class="liSenior"><span><img src="../images/t07.png" /></span>高级</li>
                    </ul>
                    <input type="button" runat="server" id="btn_Search" style="display:none;" onserverclick="btn_SearchClick" /> 
                    <ul class="toolbar3">
                        <li>代理商名称:<input  runat="server" id="txtDisName" type="text" class="textBox"/></li>
                        <li>登录帐号:<input  runat="server" id="txtuname" type="text" class="textBox"/></li>
                        <li>状态:<asp:DropDownList runat="server" ID="ddlState" CssClass="downBox">
                           <asp:ListItem Value="-1" Text="全部"></asp:ListItem>
                           <asp:ListItem Value="1" Text="启用"></asp:ListItem>
                           <asp:ListItem Value="0" Text="禁用"></asp:ListItem>
                          </asp:DropDownList></li>
                    </ul>
                </div>
             </div>
                <div class="hidden">
               <ul>
                    <li>每页<input name="txtPageSize" type="text" style=" width:40px;" class="textBox txtPageSize" id="txtPageSize"
                       runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />&nbsp;条&nbsp;&nbsp;</li>
                       <li>创建日期:<input name="txtCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                        id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;-&nbsp;
                        <input name="txtEndCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                        id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;&nbsp;
                    </li>
               </ul>
             </div>

                    <table class="tablelist" id="TbList">
                <thead>
                    <tr>
<%--                        <th><input  type="checkbox" id="CkAllcheck"  /></th>--%>
                        <th>代理商名称</th>
                        <th class="t2">登录帐号</th>
                        <th class="t5">姓名</th>
                        <th class="t5">联系人手机</th>
                        <th class="t5">状态</th>
                        <th class="t5">创建日期</th>
 
                        
                    </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="Rpt_Disuser" runat="server">
                   <ItemTemplate>
                    <tr>
  <%-- <td><input name="CkDis" runat="server" value='<%#Eval("ID") %>' type="checkbox" id="Disck"   /></td>--%>
                         <td><div class="tcle"> <%# Common.GetDisValue(Eval("Disid").ToString().ToInt(0), "DisName")%></div></td>
                         <td><div class="tc"> <a  href="DisUserInfo.aspx?KeyID=<%#Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) %>" style=" text-decoration:underline;"> <%# Eval("username")%></a></div></td>
                         <td><div class="tc"> <%# Eval("TrueName") %></div></td>
                         <td><div class="tc"> <%# Eval("Phone")%></div></td>
                         <td><div class="tc"> <%# Eval("IsEnabled").ToString() == "1" ? "启用" : "<span style='color:red'>禁用</span>"%></div></td>
                         <td><div class="tc"> <%# Common.GetDateTime(Eval("CreateDate").ToString().ToDateTime(), "yyyy-MM-dd")%></div></td>
                    </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>

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
          
          </div>

    </form>
</body>
</html>

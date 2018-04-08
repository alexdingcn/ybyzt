<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgList.aspx.cs" Inherits="Admin_OrgManage_OrgList" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script language="javascript">
        $(document).ready(function () {

            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btn_Search").trigger("click");
                }
            })

            $("li#libtnAdd").on("click", function () {
                location.href = "OrgEdit.aspx?type=1";
            })

            $("li#liSearch").on("click", function () {
                $("#btn_Search").trigger("click");
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
            <a href="#">机构管理</a><i>></i>
            <a href="#">机构维护</a>
        </div>
                  <div class="tools">
                     <ul class="toolbar left">
                          <li id="libtnAdd"><span><img src="../../Company/images/t01.png" /></span>新增机构</li>
                      </ul>

                    <div class="right">
                      <ul class="toolbar right">
                        <li id="liSearch"><span><img src="../../Company/images/t04.png" /></span>搜索</li>
                         <li onclick="javascript:window.location.href=window.location.href;"><span><img src="../../Company/images/t06.png" /></span>重置</li>
                         <uc1:ToExcel runat="server" ID="ToExcel" contect="TbOrgList" Visible="true" />
                        <li class="liSenior"><span><img src="../../Company/images/t07.png" /></span>高级</li>
                     </ul>
                    <input type="button" runat="server" id="btn_Search" style="display:none;" onserverclick="btn_SearchClick" /> 
                    <ul class="toolbar3">
                        <li>机构名称:<input  runat="server" id="txtOrgName" type="text" class="textBox"/></li>
                        <li>是否启用:
                            <select id="ddrOtype" runat="server" class="downBox" name="AddType">
                            <option value="-1">全部</option>
                            <option value="1">是</option>
                            <option value="0">否</option>
                           </select></li>
                    </ul>
                </div>
             </div>

             
            <div class="hidden" style="display:none;" >
               <ul>
                      <li>每页<input name="txtPageSize" type="text" style=" width:40px;" class="textBox txtPageSize" id="txtPageSize"
                       runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" /> &nbsp;条&nbsp;&nbsp;</li>
                     <li>联系人<input  runat="server" id="txtPrincipal" type="text" class="textBox"/>&nbsp;&nbsp;</li>
                      <li>联系人手机<input  runat="server" id="txtPhone" type="text" maxlength="11" class="textBox"/>&nbsp;&nbsp;</li>
               </ul>
             </div>


           <table class="tablelist" id="TbOrgList">
                <thead>
                    <tr>
                        <th>机构名称</th>
                        <%--<th>机构编号</th>--%>
                        <th>联系人</th>
                        <th>联系人手机</th>
                        <th>是否启用</th>
                    </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="Rpt_Org" runat="server">
                   <ItemTemplate>
                    <tr>
                         <td ><a style=" text-decoration:underline; " href='OrgInfo.aspx?KeyID=<%#Eval("Id") %>&type=2&page=<%=page %>' ><%# Eval("OrgName")%> </a></td>
                         <%--<td> <%# Eval("OrgCode")%></td>--%>
                          <td><%# Eval("Principal")%></td>
                         <td><%# Eval("Phone")%></td>
                           <td><%# Eval("IsEnabled").ToString() == "1" ? "是" : "<span style='color:red'>否</span>"%></td>
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
                                    CustomInfoSectionWidth="300px"  CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;"  CustomInfoClass="message" ShowCustomInfoSection="Left"
                  CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                     CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                    OnPageChanged="Pager_PageChanged" >
                </webdiyer:AspNetPager>
            </div>

            </div>


    </form>
</body>
</html>

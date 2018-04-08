<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompAuditList.aspx.cs" Inherits="Admin_Systems_CompAuditList" EnableEventValidation="false" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register TagPrefix="uc1" TagName="IndusTry" Src="~/Admin/UserControl/Tree_Industry.ascx" %>
<%@ Register Src="~/Admin/UserControl/Org.ascx" TagPrefix="uc1" TagName="Org" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> 厂商审核</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/CitysLine/JQuery-Citys-online-min.js"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
     <script>
         $(function () {
             $(document).on("keydown", function (e) {
                 if (e.keyCode == 13) {
                     ("#btn_Search").trigger("click");
                 }
             })
         })
         $(document).ready(function () {
             $('.tablelist tbody tr:odd').addClass('odd');
             $("li#liSearch").on("click", function () {
                 $("#btn_Search").trigger("click");
             })

         })

     </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="salemanid" runat="server" />
    <input type="hidden" id="hid" runat="server" />
    <input type="hidden" id="aspx" runat="server" value="CompAuditList.aspx" />
    <uc1:Org runat="server" ID="txtDisArea" />
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">厂商管理</a><i>></i>
            <a href="#">厂商审核</a>
        </div>

                  <div class="tools">
    <%--                    <input type="button" runat="server" id="btnAudit" style="display:none;" onserverclick="btn_AuditClick" />--%>
                    <div class="right">
                      <ul class="toolbar right">
                        <li id="liSearch"><span><img src="../../Company/images/t04.png" /></span>搜索</li>
                        <li onclick="javascript:window.location.href=window.location.href;"><span><img src="../../Company/images/t06.png" /></span>重置</li>
                        <uc1:ToExcel runat="server" ID="ToExcel" contect="TbCompAuditList" Visible="true" />
                        <li class="liSenior"><span><img src="../../Company/images/t07.png" /></span>高级</li>
                     </ul>
                    <input type="button" runat="server" id="btn_Search" style="display:none;" onserverclick="btn_SearchClick" /> 
                    <ul class="toolbar3">
                        <li>厂商名称/编码:<input  runat="server" id="txtCompName" type="text" class="textBox"/>&nbsp;&nbsp;</li>
                        <li>机构名称:<asp:DropDownList runat="server" ID="Org" CssClass="downBox" ></asp:DropDownList>&nbsp;&nbsp;</li>
                         <li>业务员:<asp:DropDownList runat="server" ID="SaleMan" CssClass="downBox" ></asp:DropDownList>&nbsp;&nbsp;</li>
                    
                    </ul>


                </div>
             </div>

             
            <div class="hidden" style="display:none;">
               <ul>
                      <li>每页<input name="txtPageSize" type="text" style=" width:40px;" class="textBox txtPageSize" id="txtPageSize"
                       runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" /> &nbsp;条&nbsp;&nbsp;</li>
                      <li>行业分类<asp:DropDownList class="downBox" runat="server"  ID="txtIndusName"  /> &nbsp;&nbsp;</li>
                     <li>经营范围<input  runat="server" id="txtMinfo" type="text" class="textBox"/>&nbsp;&nbsp;</li>
                      <li>企业电话<input  runat="server" id="txtTel" type="text" class="textBox"/>&nbsp;&nbsp;</li>
                    <li><div style=" float:left">所属地区：</div>
                     <div class="pullDown " style=" float:left;margin-top:5px;" >
            <input type="hidden" id="hidProvince" runat="server"  value="选择省" />
            <select  runat="server" id="ddlProvince"   class="textBox p-box prov" onchange="Change()" ></select>
		</div>
    	<div class="pullDown " style="margin-left:15px; float:left;margin-top:5px;">
			 <select runat="server" id="ddlCity" class="textBox p-box city"  onchange="Change()"></select>
             <input type="hidden" id="hidCity" runat="server" value="选择市" />
		</div>
        <div class="pullDown" style="margin-left:15px; float:left;margin-top:5px;">
			<select runat="server" id="ddlArea" class="textBox p-box dist"  onchange="Change()"></select>
            <input type="hidden" id="hidArea" runat="server" value="选择区" />
		</div>
                   </li>
               </ul>
             </div>


           <table class="tablelist" id="TbCompAuditList">
                <thead>
                    <tr>
                        <th>厂商名称</th>
                        <th>注册手机</th>
                        <th>经营范围</th>
                        <th>注册时间</th>
                        <th>法人</th>
                         <th>状态</th>
                        <th>企业电话</th>
                        <th>是否首页显示</th>
                    </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="Rpt_Comp" runat="server">
                   <ItemTemplate>
                    <tr>
                         <td ><a style=" text-decoration:underline; " href='CompInfo.aspx?KeyID=<%#Eval("Id") %>&type=1&page=<%=page%>' ><%# Eval("CompName")%> </a></td>

                         <td> <%# Eval("Phone")%></td>
                         <td><%#  Common.MySubstring(Convert.ToString(Eval("ManageInfo")), 34, "...")%></td>
                         <td><%# Common.GetDateTime(Eval("CreateDate").ToString().ToDateTime(),"yyyy-MM-dd")%></td>
                         <td><%# Eval("Legal")%></td>
                         <td><%# Eval("Auditstate").ToString() == "2" ? "已审" : "<span style='color:red'>未审</span>"%></td>
                         <td><%# Eval("Tel")%></td>
                         <td><%# Eval("FirstShow").ToString() == "1" ? "是" : "<span style='color:red'>否</span>"%></td>
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

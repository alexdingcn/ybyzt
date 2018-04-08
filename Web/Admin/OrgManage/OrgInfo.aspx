<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgInfo.aspx.cs" Inherits="Admin_OrgManage_OrgInfo" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
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
            $(".itab #yeqian").idTabs();

            $("li#libtnUse").on("click", function () {
                confirm("确认启用？", function () { $("#btnUse").trigger("click"); }, "提示");
            })
            $("li#libtnNUse").on("click", function () {
                confirm("确认禁用？", function () { $("#btnNUse").trigger("click"); }, "提示");
            })

            $("li#libtnEdit").on("click", function () {
                location.href = 'OrgEdit.aspx?S=1&KeyID=<%=KeyID %>&type=<%=Request["type"] %>';
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
               <a href="#" runat="server" id="Atitle">机构查看</a>
      </div>
           <div class="tools">
                  <ul class="toolbar left">
                   <li id="libtnEdit" runat="server"><span><img src="../../Company/images/t02.png" /></span>编辑</li>
                   <li id="libtnUse" runat="server"><span><img src="../../Company/images/nlock.png" /></span>启用</li>
                   <li id="libtnNUse" runat="server"><span><img src="../../Company/images/lock.png" /></span>禁用</li>
                   <li id="lblbtnback" onclick="javascript:history.go(-1);" runat="server"><span><img src="../../Company/images/tp3.png" /></span>返回</li>                  
                  </ul>
                      
                      <input type="button" runat="server" id="btnUse" onserverclick="btn_Use" style="display:none;"  />
                      <input type="button" runat="server" id="btnNUse" onserverclick="btn_NUse" style="display:none;"  />
            </div>

            <div class="div_content">
                <table  id="tab1" class="tb">
                       <tbody>
                          <tr>
                          <td style=" width:15%;"><span>机构名称</span> </td>
                          <td colspan="3" style=" width:25%;" > <label runat="server" id="lblOrgName"></label>  </td>
                         </tr>

                          <tr>
                          <td ><span>联系人</span> </td>
                          <td > <label runat="server" id="lblPrincipal"></label>  </td>
                           <td ><span>联系人手机</span> </td>
                          <td ><label runat="server" id="lblPhone"></label> </td>
                         </tr>

                         <tr>
                          <td ><span>是否启用</span> </td>
                          <td > <label runat="server" id="lblIsEnabled"></label>  </td>
                          <td ><span>排序</span> </td>
                          <td ><label runat="server" id="lblSortIndex"></label> </td>
                         </tr>

                         <tr>
                          <td ><span>备注</span></td >
                          <td colspan="3"> <label runat="server" id="lblRemark"></label>   </td>
                        </tr>

                        </tbody>
                     </table>

                <div id="tab4" runat="server" class="itab" style=" margin:15px 0px;">
  	                <ul id="yeqian" runat="server"> 
                    <li><a href="#tab2" class="selected">机构用户</a></li> 
                    <li><a href="#tab3">机构业务员</a></li> 
  	                </ul>
                    </div> 


              <div id="tab2" runat="server" style=" width:auto; display:block;">
                <div class="tools" id="add" runat="server">
                  <ul class="toolbar left">
                    <li id="libtnUserAdd" runat="server"  visible="false"  ><span><img src="../../Company/images/t01.png" /></span>新增机构用户</li>
                  </ul>
                </div>
                  <table class="tablelist">
                     <thead>
                    <tr>
                        <th>登录帐号</th>
                        <th>姓名</th>
                        <th>是否启用</th>
                        <th>类型</th>
                        <th>手机号码</th>
                    </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="Rpt_User" runat="server">
                   <ItemTemplate>
                    <tr>
                         <td><%# Eval("LoginName")%></td>
                         <td><%# Eval("TrueName")%></td>
                         <td><%# Eval("IsEnabled").ToString() == "1" ? "启用" : "<span style='color:red'>禁用</span>"%></td>
                         <td><%# Common.GetUTypeName(Eval("UserType").ToString())%></td>
                         <td><%# Eval("Phone")%></td>
                    </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>

                     <div class="pagin" style=" height:30px;">
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

                    <div id="tab3" runat="server" >
                       <!--信息列表 start-->
                     <table class="tablelist">
                     <thead>
                    <tr>
                        <th>业务员名称</th>
                        <th>业务员编码</th>
                        <th>联系手机</th>
                        <th>是否启用</th>
                        <th>邮箱</th>
                    </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="Rpt_SalesMan" runat="server">
                   <ItemTemplate>
                    <tr>
                         <td><a href="#" class="DisShow" id="<%#Eval("id") %>"> <%# Eval("SalesName")%></a></td>
                         <td><%# Eval("SalesCode")%></td>
                         <td><%# Eval("Phone")%></td>
                         <td><%# Eval("IsEnabled").ToString() == "1" ? "启用" : "<span style='color:red'>禁用</span>"%></td>
                         <td><%# Eval("Email")%></td>
                    </tr>
                    </ItemTemplate>
                    
                    </asp:Repeater>
                </tbody>
            </table>
            <!--信息列表 end-->

            <!--列表分页 start--> 
            <div class="pagin" style=" height:30px;">
                  <webdiyer:AspNetPager ID="Pager1" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                     NextPageText=">"  PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
             ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                       TextBeforePageIndexBox="<span class='jump'>跳转到:</span>"
                                    CustomInfoSectionWidth="20%"  CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;"  CustomInfoClass="message" ShowCustomInfoSection="Left"
                  CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                     CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                    OnPageChanged="Pager_PageChanged1" >
                </webdiyer:AspNetPager>
            </div>
            </div>


            </div>



       </div>
    </form>
</body>
</html>

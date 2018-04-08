<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisInfo.aspx.cs" Inherits="Admin_Systems_DisInfo" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>代理商查询</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <%--<link href="../../css/layer.css" rel="stylesheet" type="text/css" />--%>
    <%--<script src="../../js/CommonJs.js" type="text/javascript"></script>--%>
    <%--<script src="../../js/layer.js" type="text/javascript"></script>--%>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script>
        $(function () {
            $("li#libtnNUse").on("click", function () {
                layerCommon.confirm("确认禁用？", function () { $("#btnNUse").trigger("click"); }, "提示");
            })
            $("li#libtnUse").on("click", function () {
                layerCommon.confirm("确认启用？", function () { $("#btnUse").trigger("click"); }, "提示");
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place" id="divTitle" runat="server">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">代理商管理</a><i>></i>
           <a href="#" id="atitle" runat="server">代理商查询</a>
     </div>
           <div class="tools">
          <ul class="toolbar left">
                <li id="libtnUse" runat="server"><span>
                    <img src="../../Company/images/nlock.png" /></span>启用</li>
                <li id="libtnNUse" runat="server"><span>
                    <img src="../../Company/images/lock.png" /></span>禁用</li>
               <li id="lblbtnback" runat="server" onclick="javascript:history.go(-1);"><span><img src="../../Company/images/tp3.png" /></span>返回</li>

               <input type="button" runat="server" id="btnUse" onserverclick="btn_Use" style="display: none;" />
               <input type="button" runat="server" id="btnNUse" onserverclick="btn_NUse" style="display: none;" />
          </ul>
            </div>
               <div class="div_content">
                  <table class="tb" >
                   <tbody>
                      <tr>
                      <td style="width:15%;"><span>代理商名称</span> </td>
                      <td style="width:30%;"><label runat="server" id="lblDisName"></label></td>
                      <td style="width:15%;"><%--<span>代理商分类</span>--%> </td>
                      <td style="width:30%;"><%--<label runat="server" id="lblTyoeName"></label>--%></td>
                      </tr>
                      <%--<tr>
                       <td><span>代理商区域</span> </td>
                      <td><label runat="server" id="lblAreaName"></label></td>
                      <td><span>代理商等级</span> </td>
                      <td><label runat="server" id="lblDisLevel"></label></td>
                      </tr>--%> 

                       <tr>
                      <td><span>负责人</span> </td>
                      <td><label runat="server" id="lblLeading"></label></td>
                      <td><span>负责人身份证号码</span> </td>
                      <td><label runat="server" id="lblLicence"></label></td>
                      </tr>
                                          
                       <tr>
                      <td><span>负责人手机</span> </td>
                      <td><label runat="server" id="lblLeadingPhone"></label></td>
                       <td><span>联系人</span> </td>
                      <td><label runat="server" id="lblPerson"></label></td>
                      </tr>

                       
                        <tr>
                      <td><span>固定电话</span> </td>
                      <td><label runat="server" id="lblTel"></label></td>
                      <td><span>联系人手机</span> </td>
                      <td><label runat="server" id="lblPhone"></label></td>
                      </tr>

                      
                      <tr>
                      <td><span>传真</span> </td>
                      <td><label runat="server" id="lblFax"></label></td>    
                      <td><span>邮编</span> </td>
                      <td><label runat="server" id="lblZip"></label></td>                 
                      </tr>

                      <%--<tr>
                       <td><span>订单是否需要审核</span> </td>
                      <td><label runat="server" id="rdAuditYes"></label></td>
                      <td><span>是否可赊销</span> </td>
                      <td><label runat="server" id="rdCreditYes"></label></td>
                      </tr>--%>

                       <tr>
                      <td><span>是否启用</span> </td>
                      <td colspan="3"><label runat="server" id="lblIsEnabled"></label></td>
                      </tr>

                       <tr>
                      <td ><span>查看附件</span></td >
                      <td colspan="3">
                       <asp:Panel runat="server" id="DFile" style=" margin-left:5px;"></asp:Panel>
                      </td>
                     </tr>

                       <tr>
                      <td ><span>详细地址</span></td >
                      <td colspan="3"><label runat="server" id="lblAddress"></label> </td>
                     </tr>

                      <tr>
                      <td ><span>备注</span></td >
                      <td colspan="3"><label runat="server" id="lblRemark"></label> </td>
                     </tr>
                   </tbody>
                  </table>
                  <div>
                 <div class="div_title" >
                            代理商管理员:
                         </div>

                  <table class="tablelist">
                     <thead>
                    <tr>
                        <th class="t3">登录帐号</th>
                        <th class="t3">姓名</th>
                        <%--<th>性别</th>--%>
                        <th class="t3">厂商</th>
                        <th class="t3">是否审核</th>
                        <th class="t3">用户类型</th>
                        <th class="t3">手机号码</th>
                        <%--<th>身份证</th>--%>
                    </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="Rpt_User" runat="server">
                   <ItemTemplate>
                    <tr>
                         <td><div class="tc"><%# Eval("UserName")%></div></td>
                         <td><div class="tc"><%# Eval("TrueName")%></div></td>
                         <%--<td><%# Eval("Sex") %></td>--%>
                         <%--<td><div class="tc"><%# Eval("isaudit").ToString() == "2" ? "是" : "<span style='color:red'>否</span>"%></div></td>--%>
                         <td><div class="tc"><%# Common.GetCompValue( Eval("CompID").ToString().ToInt(0),"CompName") %></div></td>
                         <td><div class="tc">是</div></td>
                         <td><div class="tc"><%# Eval("Type").ToString() == "5" ? "管理员" : "用户"%></div></td>
                         <td><div class="tc"><%# Eval("Phone")%></div></td>
                         <%--<td><%# Eval("Identitys")%></td>--%>
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
                     CssClass="paginList" CurrentPageButtonClass="paginItem"
                    OnPageChanged="Pager_PageChanged" >
                </webdiyer:AspNetPager>
            </div>
            </div>
               </div>
     </div>
    </form>
</body>
</html>

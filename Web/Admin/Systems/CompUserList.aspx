<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompUserList.aspx.cs" Inherits="Admin_Systems_CompUserList" EnableEventValidation="false" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register TagPrefix="uc1" TagName="SelectComp" Src="~/Admin/UserControl/TextCompList.ascx" %>
<%@ Register Src="~/Admin/UserControl/Org.ascx" TagPrefix="uc1" TagName="Org" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>企业用户维护</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
         <script>
             $(function () {
                 $(document).on("keydown", function (e) {
                     if (e.keyCode == 13) {
                         $("#btn_Search").trigger("click");
                     }
                 })
             })
             $(document).ready(function () {
                 var div = $('<div   Msg="True"  style="z-index:111110; background-color:#000; opacity:0.5; filter:alpha(opacity=50);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');
                 $('.tablelist tbody tr:odd').addClass('odd');
                 $("li#liSearch").on("click", function () {
                     $("#btn_Search").trigger("click");
                 })

                 $("#ddlRoleId").on("change", function () {
                     $("#HidRoid").val($(this).val());
                 })

                 $("#libtnUserAdd").on("click", function () {
                     $("body").append(div);
                     $(".AddWindow").css("height", "360px").fadeIn(200);
                 })
                 $(".AddWindow .tiptop a,.tipbtn .cancel").on("click", function () {
                     $(div).remove();
                     $(".AddWindow").fadeOut(100);
                 })
             })
             function KeyInt(val) {
                 val.value = val.value.replace(/[^\d]/g, '');
             }

             function BindrOl() {
                 $("#HidRoid").val("");
                 $("#ddlRoleId").empty().append("<option value='-1'>请选择</option>");
                 $.ajax({
                     type: "post",
                     data: { Action: "Getrol", value: $("#<%=TextComp.Hid_Id%>").val() },
                     dataType: "text",
                     timeout: 5000,
                     success: function (data) {
                         var json = eval(data);
                         if (typeof (json) == "object") {
                             $.each(json, function (index, a) {
                                 $("#ddlRoleId").append("<option value='" + a.value + "'>" + a.name + "</option>");
                             })
                         }
                     }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                         alert("获取岗位失败");
                     }

                 })
             }

             function formCheck() {
                 var str = "";
                 if ($.trim($("#<%=TextComp.Hid_Id %>").val()) == "") {
                     str = "请选择企业。";
                 }
                 else if ($.trim($(".txtUserName").val()) == "") {
                     str = "登录帐号不能为空。";
                 }
                 else if ($.trim($(".txtUserPwd").val()).length < 6) {
                     str = "登录密码不能少于6位。";
                 }
                 else if ($.trim($(".txtUserPwds").val()) != $.trim($(".txtUserPwd").val())) {
                     str = "确认密码不一致。";
                 }
                 else if ($("#ddlRoleId").length == 0 || $("#ddlRoleId").val() == "-1" || $("#ddlRoleId").val() == "") {
                     str = "请选择岗位";
                 }
                 else if ($.trim($(".txtUserTrueName").val()) == "") {
                     str = "姓名不能为空。";
                 }
                 else if (!IsMobile($.trim($(".txtUserPhone").val()))) {
                     str = "手机号码不正确。";
                 }
                 if (str == "") {
                     str = ExisPhone($(".txtUserPhone").val());
                 }
                 if (str != "") {
                     errMsg("提示", str, "", "");
                     return false;
                 }
                 return true;
             }

             function ExisPhone(name) {
                 var str = "";
                 $.ajax({
                     type: "post",
                     data: { Action: "GetPhone", value: name },
                     dataType: 'json',
                     async: false,
                     timeout: 4000,
                     success: function (data) {
                         if (data.result) {
                             str = "该手机已被注册请重新填写！";
                         }
                     }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                         str = "手机号码效验异常";
                     }
                 })
                 return str;
             }
     </script>
     <style>
     .ddl
     {
       border:solid 1px #ced9df;
       max-width:100px; 
        height:25px;
         }
             select {
padding: 5px 0px\9;
height: 30px;
margin-left: 2px;
width: 150px;
border: 1px solid #D5D5D5;
font-family: '微软雅黑';
}
     </style>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="salemanid" runat="server" />
    <input type="hidden" id="hid" runat="server" />
    <input type="hidden" id="aspx" runat="server" value="CompUserList.aspx" />
    <uc1:Org runat="server" ID="txtDisArea" />
     <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">企业管理</a><i>></i>
            <a href="#">企业用户维护</a>
        </div>
                  <div class="tools">
          
                  <ul class="toolbar left">
                    <li id="libtnUserAdd"><span><img src="../../Company/images/t01.png" /></span>新增用户</li>
                  </ul>


                    <div class="right">
                      <ul class="toolbar right">
                        <li id="liSearch"><span><img src="../../Company/images/t04.png" /></span>搜索</li>
                        <li onclick="javascript:window.location.href=window.location.href;"><span><img src="../../Company/images/t06.png" /></span>重置</li>
                        <uc1:ToExcel runat="server" ID="ToExcel" contect="TbCompUserList" Visible="true" />
                        <li class="liSenior"><span><img src="../../Company/images/t07.png" /></span>高级</li>
                     </ul>
                    <input type="button" runat="server" id="btn_Search" style="display:none;" onserverclick="btn_SearchClick" /> 
                    <ul class="toolbar3">
                        <li>登录帐号:<input  runat="server" id="txtSUsername" type="text" class="textBox"/>&nbsp;&nbsp;</li>
                         <li>姓名:<input  runat="server" id="txtTrueName" type="text" class="textBox"/>&nbsp;&nbsp;</li>
                         <li>机构名称:<asp:DropDownList runat="server" ID="Org" CssClass="downBox" ></asp:DropDownList>&nbsp;&nbsp;</li>
                         <li>业务员:<asp:DropDownList runat="server" ID="SaleMan" CssClass="downBox" ></asp:DropDownList>&nbsp;&nbsp;</li>
                    
                    </ul>


                </div>
             </div>

             
            <div class="hidden" style="display:none;">
               <ul>
                      <li>每页<input name="txtPageSize" type="text" style=" width:40px;" class="textBox txtPageSize" id="txtPageSize"
                       runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" /> &nbsp;条&nbsp;&nbsp;</li>
                       <li>状态(是否启用)&nbsp;&nbsp;<asp:DropDownList ID="ddlDisbled" CssClass="textBox"  runat="server"><asp:ListItem Value="-1" Text="全部"></asp:ListItem><asp:ListItem Value="1" Text="启用"></asp:ListItem> <asp:ListItem Value="0" Text="禁用"></asp:ListItem></asp:DropDownList>&nbsp;&nbsp;</li>
                      
                     <li>厂商名称<input  runat="server" id="txtCompName" type="text" class="textBox"/>&nbsp;&nbsp;</li>
                      <li>手机号码<input  runat="server" id="txtPhone" type="text" onkeyup="KeyInt(this);" onblur="KeyInt(this);" class="textBox"/>&nbsp;&nbsp;</li>
               </ul>
             </div>


           <table class="tablelist" id="TbCompUserList">
                <thead>
                    <tr>
                        <th>登录帐号</th>
                        <th>厂商名称</th>
                        <th>姓名</th>
                        <th>类型</th>
                        <th>电话</th>
                        <th>状态</th>
                    </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="Rpt_Comp" runat="server">
                   <ItemTemplate>
                    <tr>
                        <td ><a style=" text-decoration:underline; " href='CompUserInfo.aspx?KeyID=<%#Eval("Id") %>' ><%# Eval("username")%> </a></td>
                         <td><a style=" text-decoration:underline; " href='CompInfo.aspx?KeyID=<%#Eval("compid") %>&type=3&page=<%=page %>' > <%# Common.GetCompValue(Eval("compid").ToString().ToInt(0),"compName") %></a></td>
                         <td ><%# Eval("trueName")%></td>
                         <td><%# Eval("Type").ToString() == "4" ? "管理员" : "普通用户"%></td>
                         <td> <%# Eval("Phone")%></td>
                         <td><%# Eval("IsEnabled").ToString() == "1" ? "启用" : "<span style='color:red'>禁用</span>"%></td>
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

                  <div class="AddWindow tip" style="display: none;">
            <div class="tiptop">
                <span>新增</span><a></a></div>
            <div class="tipinfo">
             <div class="lb">
                    <span><i class="required">*</i>厂商：</span>
                       <uc1:SelectComp runat="server" ID="TextComp"   />
                        </div>
                <div class="lb">
                    <span><i class="required">*</i>登录帐号：</span>    
                    <input name="txtUserName" id="txtUserName" style=" margin-left:2px;"  type="text" runat="server" class="textBox txtUserName" />
                        </div>
                      <div class="lb">
                    <span><i class="required">*</i>登录密码：</span><input name="txtUserPwd" id="txtUserPwd"  type="password"
                        runat="server" class="textBox txtUserPwd" /></div>
                <div class="lb">
                    <span><i class="required">*</i>确认密码：</span><input name="txtUserPwds" type="password"
                        runat="server" id="txtUserPwds"  class="textBox txtUserPwds" /></div>
                        <div class="lb">
                    <span><i class="required">*</i>岗位：</span>
                    <select id="ddlRoleId">
                    <option value="-1">请选择</option>
                    </select>
                    <input type="hidden" runat="server" id="HidRoid" />
                        </div>
                             <div class="lb">
                    <span><i class="required">*</i>姓名：</span>
                    <input name="txtUserTrueName" id="txtUserTrueName" style=" margin-left:2px;"  type="text" runat="server" class="textBox txtUserTrueName" />
                        </div>
                              <div class="lb">
                    <span><i class="required">*</i>手机号码：</span>
                     <input name="txtUserPhone" id="txtUserPhone" style=" margin-left:2px;"  type="text" runat="server" class="textBox txtUserPhone" />
                          </div>
                <div class="tipbtn">
                    <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return  formCheck();"
                        OnClick="btnAdd_Click" />&nbsp;
                    <input name="" type="button" class="cancel" value="取消" />
                </div>
            </div>
        </div>

    </form>
</body>
</html>

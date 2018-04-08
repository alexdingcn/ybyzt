<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrgUserList.aspx.cs" Inherits="Admin_OrgManage_OrgUserList" %>
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
                var div = $('<div   Msg="True"  style="z-index:111110; background-color:#000; opacity:0.5; filter:alpha(opacity=50);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');
                $(document).on("keydown", function (e) {
                    if (e.keyCode == 13) {
                        $("#btn_Search").trigger("click");
                    }
                })

                $("#libtnAdd").on("click", function () {
                    $("body").append(div);
                    $(".AddWindow").css("height", "330px").fadeIn(200);
                })

                $(".AddWindow .tiptop a,.tipbtn .cancel").on("click", function () {
                    $(div).remove();
                    $(".AddWindow").fadeOut(100);
                })

                $("li#liSearch").on("click", function () {
                    $("#btn_Search").trigger("click");
                })

            })

            function formCheck() {
                var str = "";
                if ($.trim($("#ddlOrg").val()) == "-1") {
                    str = "请选择机构。";
                }
                else if ($.trim($("#txtUserName").val()) == "") {
                  str = "登录帐号不能为空";
              }
              else if ($.trim($("#txtUserPwd").val()).length<6) {
                  str = "登录密码不能少于6位";
              }
              else if ($.trim($("#txtUserPwd").val()) != $.trim($("#txtUserPwds").val())) {
                  str = "确认密码不一致";
              }
              else if ($.trim($("#txtUserTrueName").val()) == "") {
                  str = "姓名不能为空";
              }
              else if ($.trim($("#txtUserPhone").val()) == "") {
                  str = "手机号码不能为空";
              }
              else if (!IsMobile($("#txtUserPhone").val())) {
                  str = "手机号码不正确";
              }
                 if (str != "") {
                     errMsg("提示", str, "", "");
                     return false;
                 }
                 return true;
            }
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
            <a href="#">机构用户维护</a>
        </div>
                  <div class="tools">
                     <ul class="toolbar left">
                          <li id="libtnAdd"><span><img src="../../Company/images/t01.png" /></span>新增机构用户</li>
                      </ul>

                    <div class="right">
                      <ul class="toolbar right">
                        <li id="liSearch"><span><img src="../../Company/images/t04.png" /></span>搜索</li>
                         <li onclick="javascript:window.location.href=window.location.href;"><span><img src="../../Company/images/t06.png" /></span>重置</li>
                         <%--<uc1:ToExcel runat="server" ID="ToExcel" contect="TbOrgUserList" Visible="true" />--%>
                        <li class="liSenior"><span><img src="../../Company/images/t07.png" /></span>高级</li>
                     </ul>
                    <input type="button" runat="server" id="btn_Search" style="display:none;" onserverclick="btn_SearchClick" /> 
                    <ul class="toolbar3">
                        <li>登录帐号:<input  runat="server" id="txtLoginName" type="text" class="textBox"/></li>
                        <li>姓名:<input  runat="server" id="txtTrueName" type="text" class="textBox"/></li>
                        <li>是否启用:
                            <select id="ddrOtype" runat="server" class="downBox" name="AddType">
                            <option value="-1">全部</option>
                            <option value="1">是</option>
                            <option value="0">否</option>
                           </select></li>
                        <li>机构名称:<asp:DropDownList runat="server" id="allorg" Width="72px" CssClass="textBox" /></li>
                    </ul>
                </div>
             </div>

             
            <div class="hidden" style="display:none;">
               <ul>
                      <li>每页<input name="txtPageSize" type="text" style=" width:40px;" class="textBox txtPageSize" id="txtPageSize"
                       runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" /> &nbsp;条&nbsp;&nbsp;</li>
                      <li>手机号<input runat="server" id="txtPhone" type="text" maxlength="11" class="textBox"/>&nbsp;&nbsp;</li>
               </ul>
             </div>


           <table class="tablelist" id="TbOrgUserList">
                <thead>
                    <tr>
                        <th>登录帐号</th>
                        <th>机构名称</th>
                        <th>姓名</th>
                        <th>类型</th>
                        <th>电话</th>
                        <th>是否启用</th>
                    </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="Rpt_OrgUser" runat="server">
                   <ItemTemplate>
                    <tr>
                         <td ><a style=" text-decoration:underline; " href='OrgUserInfo.aspx?KeyID=<%#Eval("id") %>&type=3&page=<%=page %>' ><%# Eval("LoginName")%> </a></td>
                         <td> <a style=" text-decoration:underline; " href='OrgInfo.aspx?KeyID=<%#Eval("orgid") %>&type=3&page=<%=page %>' ><%#  Common.GetOrgValue(Eval("Orgid").ToString().ToInt(0), "OrgName")%> </a></td>
                         <td><%# Eval("TrueName")%></td>
                         <td><%# Common.GetUTypeName(Eval("UserType").ToString())%></td>
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

              <div class="AddWindow tip" style="display: none;">
            <div class="tiptop">
                <span>新增</span><a></a></div>
            <div class="tipinfo">
             <div class="lb">
                    <span><i class="required">*</i>选择机构：</span>
                       <asp:DropDownList runat="server" ID="ddlOrg" style=" width:150px;" CssClass="downBox"></asp:DropDownList>
                        </div>
                <div class="lb">
                    <span><i class="required">*</i>登录帐号</span>    
                    <input name="txtUserName" id="txtUserName" onkeypress="KeyPress(event)" style=" margin-left:2px;"  type="text" runat="server" class="textBox txtUserName" />
                        </div>
                      <div class="lb">
                    <span><i class="required">*</i>登录密码：</span><input name="txtUserPwd" id="txtUserPwd"  type="password"
                        runat="server" class="textBox txtUserPwd" /></div>
                <div class="lb">
                    <span><i class="required">*</i>确认密码：</span><input name="txtUserPwds" type="password"
                        runat="server" id="txtUserPwds"  class="textBox txtUserPwds" /></div>
                             <div class="lb">
                    <span><i class="required">*</i>姓名：</span>
                    <input name="txtUserTrueName" id="txtUserTrueName" onkeypress="KeyPress(event)" style=" margin-left:2px;"  type="text" runat="server" class="textBox txtUserTrueName" />
                        </div>
                              <div class="lb">
                    <span><i class="required">*</i>手机号码：</span>
                     <input name="txtUserPhone" id="txtUserPhone" onkeypress="KeyPress(event)" style=" margin-left:2px;"  type="text" runat="server" class="textBox txtUserPhone" />
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

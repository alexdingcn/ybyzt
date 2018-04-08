<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoleInfo.aspx.cs" Inherits="Company_SysManager_RoleInfo" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>岗位权限维护详细</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("li#libtnDel").on("click", function () {
               layerCommon.confirm("确认禁用？", function () { $("#btnDel").trigger("click"); }, "提示");
            })
            $("li#libtnUse").on("click", function () {
                layerCommon.confirm("确认启用？", function () { $("#btnUse").trigger("click"); }, "提示");
            })

            $("li#libtnAdd").on("click", function () {
                var index = layerCommon.openWindow('岗位人员新增', 'UserEdit.aspx?usertype=1&RoleID='+<%= KeyID %>, '900px', '350px');
                $("#hid_Alert").val(index);
            })

            $("li#libtnEdit").on("click", function () {
                location.href = "RoleEdit.aspx?nextstep=<%=Request["nextstep"] %>&KeyId="+<%= KeyID %>;
            })
            $("li#libtnallot").on("click", function () {
                location.href = "MenuEdit.aspx?nextstep=<%=Request["nextstep"] %>&type=2&RoleId="+<%= KeyID %>;
            })
            $("li#lblbtnback").on("click", function () {
            if ('<%=Request["nextstep"] %>' == "1")
            {
                location.href = "RoleList.aspx?nextstep=1";
            }
            else
            {
                location.href = "RoleList.aspx?type=2&RoleId="+<%= KeyID %>;
            }
            })
             if ('<%=Request["nextstep"] %>' == "1") {
                document.getElementById("imgmenu").style.display = "block";
            }

        })

        function Layerclose(){
          layerCommon.layerClose($("#hid_Alert").val());
        }
        function save()
        {
            Layerclose();
            $("#reload").trigger("click");
        }
        //转到详细页
        function GoInfo(Id) {
            var index = layerCommon.openWindow('岗位人员详情', 'UserInfo.aspx?KeyID=' + Id, '1016px', '350px');
            $("#hid_Alert").val(index);
        }
    </script>
    <style>
    	.tablelist th{ text-align:center;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />

      <div class="rightinfo">
    <div class="place">
	        <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="../SysManager/RoleList.aspx" runat="server" id="atitle">岗位权限维护</a></li><li>></li>
                <li><a href="#" runat="server" id="btitle">岗位权限维护详细</a></li>
	        </ul>
     </div>
     <input type="hidden" id="hid_Alert" />
     <%--<asp:Button ID="reload" runat="server" OnClick="reload_Click" Style="display: none;" />--%>
           <div class="tools">
              <ul class="toolbar left">
                <li id="libtnallot"><span><img src="../images/t01.png" /></span><label>分配权限</label></li>
                <li id="libtnEdit"><span><img src="../images/t02.png" /></span>编辑</li>
                <li id="libtnUse" runat="server"><span><img src="../images/nlock.png" /></span>启用<input type="button" runat="server" id="btnUse" onserverclick="btn_Use" style="display:none;"  /></li>
                <li id="libtnDel" runat="server"><span><img src="../images/lock.png" /></span>禁用<input type="button" runat="server" id="btnDel" onserverclick="btn_Del" style="display:none;"  /></li>
                <li id="lblbtnback"><span><img src="../images/tp3.png" /></span>返回</li>
              </ul>
            </div>
               <div class="div_content">
                  <table class="tb" >
                   <tbody>
                      <tr>
                          <td style="width:110px;"><span>岗位名称</span> </td>
                          <td><label runat="server" id="lblRoleName"></label></td>
                          <td style="width:110px;"><span>创建时间</span> </td>
                          <td><label runat="server" id="lblCreateDate"></label></td>
                      </tr>
                      <tr>
                          <td><span>是否启用</span> </td>
                          <td><label runat="server" id="lblIsEnabled"></label></td>
                          <td><span>排  序</span> </td>
                          <td><label runat="server" id="lblSortIndex"></label></td>
                      </tr>
                      <tr>
                          <td ><span>备  注</span></td >
                          <td colspan="3"><label runat="server" id="lblRemark"></label> </td>
                     </tr>
                   </tbody>
                  </table>

                  <!--用户列表 start-->
                <%--<div>
                     <div class="div_title" >
                            岗位人员:
                         </div>
                         <div class="tools">
                        <ul class="toolbar left">
                            <li id="libtnAdd"><span><img src="../images/t01.png" /></span>新增人员</li>
                        </ul>
                        </div>
                    <!--用户列表 start-->
                        <div class="edittable2">
                            <asp:Repeater ID="rpDtl" runat="server">
                                <HeaderTemplate>
                                    <table class="tablelist">
                                        <tr class="list-title" style="text-align:center;">
                                            <th>序 号</th>
                                            <th>登录帐号</th>
                                            <th>真实姓名</th>
                                            <th>手机号码</th>
                                            <th>邮 箱</th>
                                            <th>状 态</th>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="list-title" style=" text-align:center;">
                                        <td>
                                            <div class="tcle"> 
                                            <asp:Label ID="ID" Text='<%# Eval("ID") %>' runat="server" Style="display: none;"></asp:Label>
                                            <%# Container.ItemIndex + 1 %>
                                            </div>
                                        </td>
                                        <td><a style=" text-decoration:underline; " href="javascript:void(0)" onclick='GoInfo(<%# Eval("ID") %>);' ><%# Eval("UserName")%></a></td>
                                        <td><%# Eval("TrueName")%></td>
                                        <td><%# Eval("Phone")%></td>
                                        <td><%# Eval("Email")%></td>
                                        <td><%# System.Convert.ToString(DataBinder.Eval(Container.DataItem, "IsEnabled")) == "0" ? "<font color=red>禁用</font>" : "启用"%></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <tr id="tr" runat="server" visible='<%#bool.Parse((rpDtl.Items.Count==0).ToString())%>'>
                                            <td colspan="6" align="center">
                                                <div class="tcle" style=" text-align:center;"> 
                                                无匹配数据</div>
                                            </td>
                                        </tr>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <div class="pagin" style=" height:30px;">
                                 <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                                      NextPageText=">"  PageSize="5" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                                      ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                                      TextBeforePageIndexBox="<span class='jump'>跳转到:</span>"
                                      CustomInfoSectionWidth="20%"  CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;"  CustomInfoClass="message" ShowCustomInfoSection="Left"
                                      CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                                      CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                                      OnPageChanged="Pager_PageChanged" >
                                 </webdiyer:AspNetPager>
                            </div>
                        </div>
                    <!--新增商品列表 end-->
                </div>--%>
                <!--销售订单明细 end-->

               </div>
        </div>

    </form>
</body>
</html>

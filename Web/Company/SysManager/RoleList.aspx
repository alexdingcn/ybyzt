<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoleList.aspx.cs" Inherits="Company_SysManager_RoleList" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>岗位权限维护</title>
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
                    $("#btn_Search").trigger("click");
                }
            })
        })

        function alinkorder() {
            $("#alinkorder").trigger("click");
        }

        $(document).ready(function () {
            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');

            $(".liSenior").on("click", function () {
                $("div.hidden").slideToggle(100);
            })

            $("li#libtnAdd").on("click", function () {
                location.href = "RoleEdit.aspx?nextstep=<%=Request["nextstep"] %>";
            })

            $("li#liSearch").on("click", function () {
                $("#btn_Search").trigger("click");
            })

            //add by hgh 
            if ('<%=Request["nextstep"] %>' == "1") {
                Orderclass("ktszgwqx");
                document.getElementById("imgmenu").style.display="block";
            }
        })
        function save() {
            Layerclose();
        }

        function Layerclose(){
          layerCommon.layerClose($("#hid_Alert").val());
        }
        //只能输入数字验证
        function KeyInt(val) {
            val.value = val.value.replace(/[^\d]/g, '');
        }
        //转到详细页
        function GoInfo(Id) {
            var index = layerCommon.openWindow('岗位人员新增', 'UserEdit.aspx?usertype=1&RoleID=' + Id, '900px', '350px');
            $("#hid_Alert").val(index);
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
                <li><a href="../SysManager/RoleList.aspx" runat="server" id="atitle">岗位权限维护</a></li>
	        </ul>
        </div>
        <input type="hidden" id="hid_Alert" />
        <!--当前位置 end--> 
            <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                    <li id="libtnAdd"><span><img src="../images/t01.png" /></span><font id="add" runat="server">新增岗位</font></li>
                    <li id="libtnNext" runat="server" style="display: none;">
                    <a href="javascript:void(0);" id="alinkorder" onclick="onlinkOrder('../SysManager/DisAdd.aspx?nextstep=1','ktxzjxs')"><span><img src="../images/t07.png" /></span><font color="red">下一步</font></a></li>
                </ul>
                <div class="right">
                    <ul class="toolbar right">
                        <li id="liSearch"><span><img src="../images/t04.png" /></span>搜索</li>
                    </ul>
                    <ul class="toolbar3">
                        <li>岗位名称:<input runat="server" id="txtRoleName" type="text" class="textBox"/></li>
                        <li>&nbsp; &nbsp; 状 态:
                          <asp:DropDownList ID="sltIsAllow" runat="server" Width="72px" CssClass="downBox">
                            <asp:ListItem Value="">全部</asp:ListItem>
                            <asp:ListItem Value="1">启用</asp:ListItem>
                            <asp:ListItem Value="0">禁用</asp:ListItem>
                        </asp:DropDownList>
                        </li>
                        <li>&nbsp; &nbsp;每页<input  runat="server" id="txtPageSize" onkeyup='KeyInt(this)' type="text" style="width:40px;" class="textBox"/>行</li>
                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->
            <uc1:CompRemove runat="server" ID="Remove" ModuleID="1" />
            <div class="hidden">
               <ul>
                  
               </ul>
             </div>

            <!--信息列表 start-->
            <table class="tablelist">
                <thead>
                    <tr>
                        <th>岗位名称</th>
                        <th>是否启用</th>
                        <th>创建日期</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="Rpt_Role" runat="server">
                   <ItemTemplate>
                    <tr>
                         <td><div class="tcle"><a style=" text-decoration:underline; " href='RoleInfo.aspx?type=1&KeyId=<%#Eval("Id") %>&nextstep=<%=Request["nextstep"] %>' ><%# Eval("RoleName") %></a></div></td>
                         <td><div class="tc"><%# Eval("IsEnabled").ToString()=="1" ? "启用" : "<font color=red>禁用</font>"%> </div></td>
                         <td><div class="tc"><%# Eval("CreateDate","{0:yyyy-MM-dd}") %></div></td>
                         <td>
                             <div class="tcle"><a href='RoleInfo.aspx?KeyId=<%#Eval("Id") %>&nextstep=<%=Request["nextstep"] %>' title='查看' class="tablelink grayBtn">查看</a>
                             &nbsp;<a href='MenuEdit.aspx?RoleId=<%#Eval("Id") %>&type=1&nextstep=<%=Request["nextstep"] %>' title='分配权限' class="tablelink">分配权限</a>&nbsp;
                             <%--<a href='#' onclick='GoInfo(<%# Eval("ID") %>);' title='新增人员' class="tablelink">新增人员</a>--%></div>
                         </td>
                    </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
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
        <input type="button" runat="server" id="btn_Search" style="display:none;" onserverclick="btn_SearchClick" />
    </form>
</body>
</html>

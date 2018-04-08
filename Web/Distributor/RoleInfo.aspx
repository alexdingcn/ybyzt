<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoleInfo.aspx.cs" Inherits="Distributor_RoleInfo" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>岗位信息</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../Company/js/js.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script> 
    <script src="../Company/js/order.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#NUseIcon").on("click", function () {
               layerCommon.confirm("确认禁用？", function () { $("#btnNUse").trigger("click"); });
            })
            $("#UseIcon").on("click", function () {
               layerCommon.confirm("确认启用？", function () { $("#btnUse").trigger("click"); });
            })

            //编辑
            $("#EditIcon").click(function () {
                window.location.href = 'RoleEdit.aspx?KeyID=<%=Common.DesEncrypt(KeyID.ToString(),Common.EncryptKey) %>';
            });
            $("#returnIcon").click(function () {
                location.href = "RoleList.aspx";
            });
            $("#FPIcon").click(function () {
                location.href = "MenuEdit.aspx?type=2&RoleID="+<%= KeyID %>;
            });
            $("#btnAdd").on("click", function () {
                var index = layerCommon.openWindow('岗位人员新增', 'UserAdd.aspx?usertype=1&RoleID='+<%= KeyID %>, '910px', '410px');
                $("#hid_Alert").val(index);
            })
        });
        //转到详细页
        function GoInfo(Id,UserID) {
            var index = layerCommon.openWindow('岗位人员详情', 'UserInfo.aspx?KeyID=' + Id+"&userID="+UserID, '910px', '410px');
            $("#hid_Alert").val(index);
        }
        function save()
        {
            CloseDialog();
            $("#reload").trigger("click");
        }
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server" method="post">
    <Head:Head ID="Head2" runat="server" />
    <input type="button" runat="server" id="btnUse" onserverclick="btn_Use" style="display:none;"  />
    <input type="button" runat="server" id="btnNUse" onserverclick="btn_NUse" style="display:none;"  />
    <%--<asp:Button ID="reload" runat="server" OnClick="reload_Click" Style="display: none;" />--%>
    <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="nav-6" />
        <input type="hidden" id="hid_Alert" />
        <div class="rightCon">
            <div class="info">
                 <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="/Distributor/RoleList.aspx" class="cur">设置岗位权限</a>>
                <a href="/Distributor/RoleInfo.aspx" class="cur">岗位信息</a></div>
            <!--功能条件 start-->
            <div class="userFun">
                <div class="left">
                    <a href="javascript:void(0)" class="btnOr" id="FPIcon" runat="server">
                        <i class="prnIcon"></i>分配权限</a>
                    <a href="javascript:void(0)" class="btnBl" id="EditIcon" runat="server" >
                        <i class="editIcon"></i>编辑</a>
                    <a href="javascript:void(0)" class="btnBl" id="UseIcon" runat="server">
                        <i class="prnIcon"></i>启用</a> 
                    <a href="javascript:void(0)" class="btnBl" id="NUseIcon" runat="server">
                        <i class="offIcon"></i>禁用</a>
                    <a href="#" class="btnBl" id="returnIcon" runat="server">
                        <i class="returnIcon"></i>返回</a>
                </div>
            </div>
            <!--功能条件 end-->
            <div class="blank10">
            </div>
            <!--岗位权限详细 start-->
            <div class="orderNr">
                <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                    <tbody>
                        <tr>
                            <td class="head" style="width: 13%">
                                岗位名称
                            </td>
                            <td style="width: 37%">
                                <label id="lblRoleName" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head" style="width: 13%">
                                创建时间
                            </td>
                            <td style="width: 37%">
                                <label id="lblCreateDate" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="head">
                                是否启用
                            </td>
                            <td>
                                <label id="lblIsEnabled" runat="server">
                                </label>
                                &nbsp;
                            </td>
                            <td class="head">
                                排序
                            </td>
                            <td>
                                <label id="lblSortIndex" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="head">
                                备注
                            </td>
                            <td colspan="3">
                                <label id="lblRemark" runat="server">
                                </label>
                                &nbsp;
                            </td>
                        </tr>
                    </tbody>
                </table>
               <%-- <div>
                	<div class="blank20"></div>
                    <div class="userFun" style="padding-left:5px;">
                        <div class="left">
                        <a href="javascript:void(0)" class="btnBl" id="btnAdd" runat="server">
                            <i class="addIcon"></i>新增岗位人员</a>
                        </div>
                    </div>
                <div class="orderLiv">
                    <table class="PublicList" border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th>
                                    序号
                                </th>
                                <th>
                                    登录帐号
                                </th>
                                <th>
                                    真实姓名
                                </th>
                                <th>
                                    手机号码
                                </th>
                                <th>
                                    邮箱
                                </th>
                                <th>
                                    状态
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rpDtl">
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:Label ID="ID" Text='<%# Eval("ID") %>' runat="server" Style="display: none;"></asp:Label>
                                            <%# Container.ItemIndex + 1 %>
                                        </td>
                                        <td><a style=" text-decoration:underline; " href="javascript:void(0)" onclick='GoInfo("<%# Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) %>","<%# Common.DesEncrypt(Eval("UserID").ToString(), Common.EncryptKey) %>");' ><%# Eval("UserName")%></a></td>
                                        <td><%# Eval("TrueName")%></td>
                                        <td><%# Eval("Phone")%></td>
                                        <td><%# Eval("Email")%></td>
                                        <td><%# System.Convert.ToString(DataBinder.Eval(Container.DataItem, "IsEnabled")) == "0" ? "<font color=red>禁用</font>" : "启用"%></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <tr id="tr" runat="server" visible='<%#bool.Parse((rpDtl.Items.Count==0).ToString())%>'>
                                            <td colspan="6" align="center">
                                                无匹配数据
                                            </td>
                                        </tr>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
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
                        </tbody>
                    </table>
                </div>
                </div>--%>
            </div>
        </div>
    </div>
    <div class="opacity" style=" display:none;"></div>
    </form>
    
</body>
</html>

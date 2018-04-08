<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompGoodsList.aspx.cs" Inherits="Company_SysManager_CompGoodsList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>企业展示产品</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
        <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script>
        // 每页显示数量非空验证
        function ChkPage() {
            if ($("#txtPageSize").val() == "") {
                layerCommon.msg(" 每页显示数量不能为空", IconOption.错误);
                return false;
            } else {
                $("#btnSearch").click();
            }
            return true;
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
                <li><a href="#">信息发布</a></li><li>></li>
                <li><a href="#">企业展示产品</a></li>
            </ul>
        </div>
        <!--当前位置 end-->

        <!--功能按钮 start-->
        <div class="tools">
            <div class="right">
                <ul class="toolbar right ">
                    <li onclick="return ChkPage()"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                </ul>
                <ul class="toolbar3">
                    <li>商品名称:<input name="txtGoodsName" type="text" id="txtGoodsName" runat="server"
                        class="textBox txtGoodsName" /></li>
                    <!--<li>订单金额(元):	<input name="" type="text" class="textBox2"/>-<input name="" type="text" class="textBox2"/></li>-->
                    <li>状态:<asp:DropDownList ID="ddlState" runat="server" CssClass="textBox">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="0">上架</asp:ListItem>
                        <asp:ListItem Value="1">下架</asp:ListItem>
                    </asp:DropDownList>
                    </li>
                    <li>每页<input name="txtPageSize" type="text" class="textBox txtPageSize" id="txtPageSize"
                        style="width: 40px" runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />条</li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <!--信息列表 start-->
        <table class="tablelist">
            <thead>
                <tr>
                    <th>
                        商品名称
                    </th>
                    
                    <th>
                        类别
                    </th>
                    <th>
                        属性
                    </th>
                    <th>
                        价格(元)
                    </th>
                    <th>
                        单 位
                    </th>
                    <th>
                        状态
                    </th>
                    <th>
                        创建人
                    </th>
                    <%--   <th>
                        操作
                    </th>--%>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptGoods" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#  Eval("GoodsName") %>
                            </td>
                            
                            <td>
                                <%# GoodsCategory(Eval("categoryID").ToString())%>
                            </td>
                            <td>
                                <%# GoodsAttr2(Eval("ID").ToString())%>
                            </td>
                            <td>
                                <%#  decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(Eval("SalePrice")).ToString())).ToString("#,##0.00")%>
                            </td>
                            <td>
                                <%# Eval("Unit").ToString()%>
                            </td>
                            <td>
                                <%# Eval("isOffline").ToString()=="1"?"上架":"下架" %>
                            </td>
                            <td>
                                <%# Common.GetUserName( Convert.ToInt32( Eval("CreateUserID")))%>
                            </td>
                            <%--        <td style="width: 100px">
                                <a href="javascript:;" class="link" onclick="EditData(<%# Eval("ID") %>);" tip='<%# Eval("ID") %>'>
                                    编辑</a> <a href="javascript:;" class="link" onclick="InfoData(<%# Eval("ID") %>)"
                                        tip='<%# Eval("ID") %>'>详情</a>
                            </td>--%>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <asp:Button ID="btnSearch" runat="server" Style="display: none" Text="搜索" OnClick="btnSearch_Click" />
        <!--信息列表 end-->
        <!--列表分页 start-->
        <div class="pagin">
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
        <!--列表分页 end-->
    </div>
    </form>
    <script type="text/javascript">
        $('.tablelist tbody tr:odd').addClass('odd');
    </script>
</body>
</html>

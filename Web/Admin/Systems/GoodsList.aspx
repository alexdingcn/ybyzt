<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsList.aspx.cs" Inherits="Admin_Systems_GoodsList" EnableEventValidation="false" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Admin/UserControl/Org.ascx" TagPrefix="uc1" TagName="Org" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>商品信息管理</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/ImgAmplify.js" type="text/javascript"></script>
    <script src="../../js/MoveLayer.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/ajaxfileupload.js" type="text/javascript"></script>
    <style type="text/css">
        /*鼠标移动：图片预览*/
        .aImg
        {
            width: 45px;
            height: 40px;
        }
        /*border: 1px solid #00adf2;*/
        .pic
        {
            width: 45px;
            height: 40px;
        }
        #tooltip
        {
            position: absolute;
            border: 1px solid #ccc;
            background: #333;
            padding: 2px;
            display: none;
            color: #fff;
            max-width: 452px;
            max-height: 424px;
        }
    </style>
    <script>
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    ChkPage();
                }
            });
            $($(".tiptop")[1]).LockMove({ MoveWindow: "#DLodIMG" });
            //移动图片展示
            $("a.tooltip").ImgAmplify();
        })
        $(document).ready(function () {
            $('.tablelist tbody tr:odd').addClass('odd');
        });
        // 每页显示数量非空验证
        function ChkPage() {
            if ($("#txtPageSize").val() == "") {
                errMsg("提示", "- 每页显示数量不能为空", "", "");
                return false;
            } else {
                $("#btnSearch").click();
            }
            return true;
        }
        function InfoData(id) {
            location.href = "GoodsInfo.aspx?KeyID=" + id;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <!--当前位置 start-->
    <input type="hidden" id="salemanid" runat="server" />
    <input type="hidden" id="hid" runat="server" />
    <input type="hidden" id="aspx" runat="server" value="GoodsList.aspx" />
    <uc1:Org runat="server" ID="txtDisArea" />
        <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">商品查询</a><i>></i>
            <a href="GoodsList.aspx">商品查询</a>
    </div>
    <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools">
            <div class="right">
                <ul class="toolbar right ">
                    <li onclick="return ChkPage()"><span>
                        <img src="../../Company/images/t04.png" /></span>搜索</li>
                    <li onclick="javascript:location.href='GoodsList.aspx'"><span>
                        <img src="../../Company/images/t06.png" /></span>重置</li>
                        <uc1:ToExcel runat="server" ID="ToExcel" contect="TbGoodsList" Visible="true" />
                    <li class="liSenior"><span><img src="../../Company/images/t07.png" /></span>高级</li>
                </ul>
                <ul class="toolbar3">
                    <li>厂商名称:<input name="txtCompName" type="text" id="txtCompName" runat="server"
                        class="textBox txtCompName" /></li>
                    <li>商品上架名称:<input name="txtGoodsName" type="text" id="txtGoodsName" runat="server"
                        class="textBox txtGoodsName" /></li>
                    <li>状态:<asp:DropDownList ID="ddlState" runat="server" CssClass="textBox">
                        <asp:ListItem Value="">全部</asp:ListItem>
                        <asp:ListItem Value="1">上架</asp:ListItem>
                        <asp:ListItem Value="0">下架</asp:ListItem>
                    </asp:DropDownList>
                    </li>
                    
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <div class="hidden">
               <ul style="">
                   <li>每页<input name="txtPageSize" type="text" class="textBox txtPageSize" id="txtPageSize"
                        style="width: 40px" runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />条&nbsp;</li>
                    <li>业务员:<asp:DropDownList runat="server" ID="SaleMan" CssClass="downBox" ></asp:DropDownList>&nbsp;&nbsp;</li>
                    <li>机构名称:<asp:DropDownList runat="server" ID="Org" CssClass="downBox" ></asp:DropDownList>&nbsp;&nbsp;</li>
               </ul>
             </div>
        <!--信息列表 start-->
        <table class="tablelist" id="TbGoodsList">
            <thead>
                <tr>
                    <th>
                        商品上架名称
                    </th>
                    <th>
                        厂商名称
                    </th>
                    <th>
                        图片
                    </th>
                    <th>
                        商品分类
                    </th>
                    <th>
                        规格属性
                    </th>
                    <th>
                        商品描述
                    </th>
                    <th>
                        计量单位
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
                                <a href="javascript:;" style="text-decoration: underline;" onclick="InfoData(<%# Eval("ID") %>)" title="<%#Eval("GoodsName") %>"
                                    tip='<%# Eval("ID") %>'>
                                    <%#  Eval("GoodsName").ToString().Length > 10 ? Eval("GoodsName").ToString().Substring(0, 10) : Eval("GoodsName").ToString()%></a>
                            </td>
                            <td><a style=" text-decoration:underline; " href='CompInfo.aspx?KeyID=<%#Eval("CompID") %>&type=4&atitle=商品查询&btitle=商品查询' ><%# Common.GetCompValue(Convert.ToInt32(Eval("CompID").ToString()), "CompName")%> </a></td>
                            <td style="height: 42px; width: 42px;">
                                <a class="tooltip" href='#' id='<%# Common.GetPicURL(Eval("Pic").ToString(), "", Eval("CompID").ToString()) %>' style="display: inline-block;">
                                    <img id='GoodsImg' class='pic' alt="暂无" runat="server" src='<%# Common.GetPicURL(Eval("Pic").ToString(), "resize200", Eval("CompID").ToString()) %>' />
                                </a>
                            </td>
                            <td>
                                <%# GoodsCategory(Eval("categoryID").ToString())%>
                            </td>
                            <td>
                                <%# GoodsAttr2(Eval("ID").ToString(), Eval("CompID").ToString())%>
                            </td>
                            <td style="line-height: 20px; width: 160px">
                                <!-- description -->
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
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <asp:Button ID="btnSearch" runat="server" Style="display: none" Text="搜索" OnClick="btnSearch_Click" />
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
    </form>
    <script type="text/javascript">
        $('.tablelist tbody tr:odd').addClass('odd');
    </script>
</body>
</html>

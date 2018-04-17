<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CMerchantsList.aspx.cs"
    Inherits="Company_CMerchants_CMerchantsList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>招商列表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/order.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            });

            $("#btnAdd").click(function () {
                window.location.href = 'CMerchantsAdd.aspx';
            });

            //订单生成 搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });

            ///重置
            $("#li_Reset").click(function () {
                window.location.href = 'CMerchantsList.aspx';
            });
        });

    </script>
    <style type="text/css">
        .tablelist td a
        {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-2" />
    <input id="hid_Alert" type="hidden" />
    <input type="hidden" id="hidCompId" runat="server" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a href="#">招商列表</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <li class="click" id="btnAdd" runat="server"><span>
                    <img src="../images/t01.png" /></span><font>新 增</font></li>
                
                
                
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li id="Search"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                    <%--<li id="export"><span><img src="../images/tp3.png" /></span>导出</li>--%>
                    <li class="liSenior"><span>
                        <img src="../images/t07.png" /></span>高级</li>
                </ul>
                <ul class="toolbar3">
                    <li>状态:<select name="State" runat="server" id="ddrState" style="  width:90px; " class="downBox">
                        <option value="">请选择</option>
                        <option value="0">已下架</option>
                        <option value="1">已上架</option>
                    </select>
                    </li>
                    <li>招商名称:<input name="txtCMName" type="text" id="txtCMName" runat="server" class="textBox" maxlength="50" /></li>
                    <li>商品名称:<input name="txtGoodsName" type="text" id="txtGoodsName" runat="server" class="textBox" maxlength="50" />
                    </li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <div class="hidden">
            <%--<ul class="toolbar right" style="width: 130px;">
                 <li id="liexcel"><span><img src="../images/tp3.png" /></span>订单详情导出</li>
            </ul>--%>
            <ul class="left" style="width: 80%;">
                <li>每页<input type="text" onkeyup="KeyInt(this)" id="txtPager" name="txtPager" runat="server"
                    class="textBox" style="width: 40px;" />&nbsp;条 </li>
                <li>生效日期:<input name="txtCreateDate" runat="server" onclick="var endDate=$dp.$('txtEndCreateDate'); WdatePicker({onpicked:function(){endDate.focus();},maxDate:'#F{$dp.$D(\'txtEndCreateDate\')}'})"
                        style="width: 100px;" id="txtCreateDate" readonly="readonly" type="text" class="Wdate"
                        value="" />&nbsp;-&nbsp;
                        <input name="txtEndCreateDate" runat="server" onclick="WdatePicker({minDate:'#F{$dp.$D(\'txtCreateDate\')}'})"
                            style="width: 100px;" id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate"
                            value="" /></li>
            </ul>
        </div>
        <!--信息列表 start-->
        <asp:Repeater runat="server" ID="rptOrder">
            <HeaderTemplate>
                <table class="tablelist" id="TbList">
                    <thead>
                        <tr>
                            <th class="t3">
                                招商名称
                            </th>
                            <th class="t3">
                                商品分类
                            </th>
                            <th>
                                商品名称
                            </th>
                            <th class="t1">
                                生效日期
                            </th>
                            <th class="t1">
                                失效日期
                            </th>
                            <th class="t1">
                                状态
                            </th>
                            <th class="t1">
                                操作
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr id='tr_<%# Eval("Id") %>'>
                   
                    <td>
                        <div class="tcle">
                             <a href="CMerchantsInfo.aspx?KeyID=<%# Common.DesEncrypt(Eval("Id").ToString(),Common.EncryptKey) %>"><%# Eval("CMName")%></a>
                        </div>
                    </td>
                    <td>
                        <div class="tcle">
                           <%# Common.GetCategoryName(Common.GetGoodsName(Eval("GoodsID").ToString(), "CategoryID"))%>    
                        </div>
                    </td>
                    <td>
                        <div class="tcle">
                           <%# Common.GetGoodsName(Eval("GoodsID").ToString(), "GoodsName") %>    
                        </div>
                    </td>
                    <td>
                        <div class="tc">
                            <%# Eval("ForceDate", "{0:yyyy-MM-dd}")%>
                        </div>
                    </td>
                    <td>
                        <div class="tc">
                            <%# Eval("InvalidDate", "{0:yyyy-MM-dd}")%>
                        </div>
                    </td>
                    <td>
                        <div class="tc">
                            <%# Eval("IsEnabled").ToString().ToInt(0) == 1 ? "上架" : "<i style=\"color:#ccc;\">下架</i>"%>
                        </div>
                    </td>
                    <td>
                        <div class="tc">
                            <a href="CMerchantsInfo.aspx?KeyID=<%# Common.DesEncrypt(Eval("Id").ToString(),Common.EncryptKey) %>">详情</a>
                        </div>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
        <!--信息列表 end-->
        <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
        <%--<asp:Button ID="btnVolumeDel" Text="批量删除" runat="server" OnClick="btnVolumeDel_Click"
            Style="display: none" />--%>
        <!--列表分页 start-->
        <div class="pagin">
            <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                OnPageChanged="Pager_PageChanged">
            </webdiyer:AspNetPager>
        </div>
        <!--列表分页 end-->
    </div>
    </form>
</body>
</html>

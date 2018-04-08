<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CMerchantsList.aspx.cs"
    Inherits="Distributor_CMerchants_CMerchantsList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>招商列表</title>
   <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Company/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../../js/CommonJs.js"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

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
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="nav-7" />
        <div class="rightCon">
            <div class="info"><a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
                    <a id="navigation2" href="/CMerchants/CMerchantsList.aspx" class="cur">招商列表</a></div>

            <!--功能条件 start-->
            <div class="userFun">
                <div class="left">
                </div>
                <div class="right">
                    <ul class="term">
                        <li><label class="head">选择厂商：</label>
                            <select id="ddrComp" name="" style=" width:120px;" runat="server" class="xl">
                            </select>
                        </li>
                        <%--<li><label class="head">状态:</label><select name="State" runat="server" id="ddrState" style="  width:90px; " class="xl">
                            <option value="">请选择</option>
                            <option value="0">已下架</option>
                            <option value="1">已上架</option>
                        </select>
                        </li>--%>
                        <li><label class="head">招商名称:</label><input name="txtCMName" type="text" id="txtCMName" runat="server" class="box" maxlength="50" /></li>
                        <li><label class="head">商品名称:</label><input name="txtGoodsName" type="text" id="txtGoodsName" runat="server" class="box" maxlength="50" />
                        </li>

                        <a id="A1" href="#" class="btnBl" onserverclick="btnSearch_Click" runat="server"><i class="searchIcon"></i>搜索</a>
                        <a href="javascript:void(0);" onclick="javascript:location.href='CMerchantsList.aspx'" class="btnBl"><i class="resetIcon"></i>重置</a>
                        <a href="javascript:void(0)"  class="btnBl liSenior"><i class="resetIcon"></i>高级</a>
                    </ul>
                </div>
            </div>
            <div class="hidden userFun" style=" text-align:right; padding-right:160px; padding-top:10px; display:none;">
                 <div class="right">
                    <ul class="term">
                            <li><label class="head">生效日期:</label><input name="txtCreateDate" runat="server" onclick="var endDate=$dp.$('txtEndCreateDate'); WdatePicker({onpicked:function(){endDate.focus();},maxDate:'#F{$dp.$D(\'txtEndCreateDate\')}'})"
                                    style="width: 100px;" id="txtCreateDate" readonly="readonly" type="text" class="Wdate"
                                    value="" />&nbsp;-&nbsp;
                                    <input name="txtEndCreateDate" runat="server" onclick="WdatePicker({minDate:'#F{$dp.$D(\'txtCreateDate\')}'})"
                                        style="width: 100px;" id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate"
                                        value="" /></li>
                                <li><label class="head">每页</label><input type="text" onkeyup="KeyInt(this)" id="txtPager" name="txtPager" runat="server"
                        class="box" style="width: 40px;" /><label class="head">条</label></li>
                    </ul>
                </div>
            </div>
            <!--功能条件 end-->
            <div class="blank10"></div>
            <div class="orderNr">
                <table class="PublicList list" id="orderBg" border="0" cellspacing="0" cellpadding="0">
                    <asp:Repeater ID="rptOrder" runat="server">
                        <HeaderTemplate>
                            <thead>
                                <tr>
                                    <th>厂商</th>
                                    <th>招商名称</th>
                                    <th>商品分类</th>
                                    <th>商品名称</th>
                                    <th>生效日期</th>
                                    <th>失效日期</th>
                                    <%--<th>状态</th>--%>
                                    <th>操作</th>
                                </tr>
                            </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr id='tr_<%# Eval("Id") %>'>
                                <td>
                                    <%# Common.GetCompValue(Eval("CompID").ToString().ToInt(0),"CompName") %>
                                </td>
                                <td>
                                    <%# Eval("CMName")%>
                                </td>
                                <td>
                                    <%# Common.GetCategoryName(Common.GetGoodsName(Eval("GoodsID").ToString(), "CategoryID"))%>    
                                </td>
                                <td>
                                    <%# Common.GetGoodsName(Eval("GoodsID").ToString(), "GoodsName") %>    
                                </td>
                                <td>
                                    <%# Eval("ForceDate", "{0:yyyy-MM-dd}")%>
                                </td>
                                <td>
                                    <%# Eval("InvalidDate", "{0:yyyy-MM-dd}")%>
                                </td>
                                <%--<td>
                                     <%# Eval("IsEnabled").ToString().ToInt(0) == 1 ? "上架" : "<i style=\"color:#ccc;\">下架</i>"%> 
                                </td>--%>
                                <td>
                                     <a href="CMerchantsInfo.aspx?KeyID=<%# Common.DesEncrypt(Eval("Id").ToString(),Common.EncryptKey) %>"> 详情 </a> 
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody> </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
            </div>
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
    </div>
    </form>
</body>
</html>

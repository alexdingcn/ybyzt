<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsList.aspx.cs" Inherits="Company_ShopManager_GoodsList" %>

<%@ Register Src="~/Company/UserControl/TreeDemo.ascx" TagPrefix="uc1" TagName="TreeDemo" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>商品选择</title>
    <link href="/Company/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/Company/css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="/Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="/Company/js/js.js" type="text/javascript"></script>
    <script src="/js/layer/layer.js" type="text/javascript"></script>
    <script src="/js/layerCommon.js" type="text/javascript"></script>
    <script>
        $(function () {
            $(".showDiv .ifrClass").css("width", "155px");
            $(".showDiv").css("width", "150px");

            $('iframe').load(function () {
                $('iframe').contents().find('.pullDown').css("width", "150px");
            });
            //选择商品
            $(".tablelist tbody tr").click(function () {
                var Type = '<%=Request["Type"] %>';
                var goodsId = $.trim($(this).find("a").attr("tip")), goodsName = $.trim($(this).find("a").text());
                if (Type == "Five") {
                    window.parent.EditParam.EditSetFiveImgGoodsValue(goodsId, goodsName);
                } else {
                    window.parent.EditParam.EditSetTopGoodsValue(goodsId, goodsName);
                }
            })
        })
        //搜索
         // 每页显示数量非空验证
        function ChkPage() {
            if ($("#txtPager").val() == "") {
                      layerCommon.msg("每页显示数量不能为空", IconOption.错误);
                return false;
            } else if (parseInt($("#txtPager").val()) == 0) {
                      layerCommon.msg("每页显示数量必须大于0", IconOption.错误);
                return false;
            } else {
                $("#btnSearch").click();
            }
            return true;
        }
        //重置
        function Fanh() {
          location = location;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="hid_Alert">
    <input type="hidden" name="hid_org_id" id="hid_org_id">
    <div class="rightinfo" style="margin: 0px 0px; width: auto">
        <div class="tools">
            <div class="right">
                <ul class="toolbar right ">
                    <li onclick="return ChkPage()"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                </ul>
                <ul class="toolbar3">
                    <li>类别名称:
                        <uc1:TreeDemo runat="server" ID="txtCategory" />
                    </li>
                    <li>商品名称:<input name="txtGoodsName" type="text" id="txtGoodsName" runat="server"
                        class="textBox txtGoodsName" /></li>
                    <li>每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                        class="textBox" style="width: 40px;" value="50" />&nbsp;条 </li>
                </ul>
            </div>
        </div>
        <table class="tablelist">
            <thead>
                <tr>
                    <th width="35%">
                        商品名称
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptGoods" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                            <div class="tc" style="text-align:left; cursor:pointer;" title="点击选择商品：<%# Eval("GoodsName").ToString() %>">
                                <a href="javascript:;" title='<%# Eval("GoodsName").ToString() %>' class="link edit"
                                    style="text-decoration: underline; width: 300px" tip='<%# Eval("ID") %>'>
                                    <%# Eval("GoodsName").ToString() %>
                                </a>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <!--列表分页 start-->
        <div class="pagin">
            <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                NextPageText=">" PageSize="50" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                OnPageChanged="Pager_PageChanged">
            </webdiyer:AspNetPager>
        </div>
        <!--列表分页 end-->
        <asp:Button ID="btnSearch" runat="server" Style="display: none" Text="搜索" OnClick="btnSearch_Click" />
    </div>
    </form>
    <script type="text/javascript">
        $('.tablelist tbody tr:odd').addClass('odd');
    </script>
</body>
</html>

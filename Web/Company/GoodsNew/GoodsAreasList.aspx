<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsAreasList.aspx.cs" Inherits="Company_Goods_GoodsAreasList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TreeDisArea.ascx" TagPrefix="uc1" TagName="TreeDisArea" %>
<%@ Register Src="~/Company/UserControl/TreeDemo.ascx" TagPrefix="uc1" TagName="TreeDemo" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>商品不可售区域</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script>
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            })
        })
        //搜索验证
        function ChkPage() {
            if ($("#txtPageSize").val() == "") {
                layerCommon.msg("每页显示数量不能为空", IconOption.错误);
                return false;
            } else if (parseInt($("#txtPageSize").val()) == 0) {
                layerCommon.msg("每页显示数量必须大于0", IconOption.错误);
                return false;
            } else {
                $(".hid_Alert").val("");
                $("#btnSearch").click();
            }
            return true;
        }
        //新增
        function addData() {
            location.href = "GoodsAreasEdit.aspx";
        }
        //删除
        function Delete(type) {
            var bol = false;
            var chklist = $(".tablelist tbody input[type='checkbox']");
            $(chklist).each(function (index, obj) {
                if (obj.checked) {
                    bol = true;
                    return false;
                }
            })
            if (type == 1) {
                if (bol) {
                    layerCommon.confirm('确定要删除数据', function () {
                        $("#btnDel").click();
                    });

                } else {
                    layerCommon.msg("请勾选需要删除的数据", IconOption.错误);
                    return false;
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-3" />
    <input type="hidden" id="hid_Alert">
    <input type="hidden" name="hid_org_id" id="hid_org_id">
    <div class="rightinfo">
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="../GoodsNew/GoodsAreaList.aspx">商品不可售区域</a></li>
            </ul>
        </div>
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <li onclick="addData()"><span>
                    <img src="../images/t01.png" /></span><font>新增</font></li>
                <li onclick="Delete(1)"><span>
                    <img src="../images/t03.png" /></span>删除</li>
            </ul>
            <div class="right">
                <ul class="toolbar right ">
                    <li onclick="return ChkPage()"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                </ul>
                <ul class="toolbar3">
                    <li>代理商区域:<uc1:TreeDisArea runat="server" ID="txtDisAreaBox" />
                    </li>
                    <li>商品分类:
                        <uc1:TreeDemo runat="server" ID="txtCategory" />
                    </li>
                    <li>商品名称:<input name="txtGoodsName" type="text" id="txtGoodsName" runat="server"
                        class="textBox txtGoodsName" style="width: 90px" maxlength="50" /></li>
                    <li>每页<input name="txtPageSize" type="text" class="textBox txtPageSize" id="txtPageSize"
                        style="width: 40px" runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />条</li>
                </ul>
            </div>
        </div>
        <table class="tablelist">
            <thead>
                <tr>
                    <th class="t7">
                        <input type="checkbox" name="checkbox" id="CB_SelAll" onclick="SelectAll(this)" />
                    </th>
                    <th class="t6">
                        代理商区域
                    </th>
                    <th class="t6">
                        商品名称
                    </th>
                    <th class="t6">
                        商品分类
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptGoodsAreas" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <div class="tc"> <asp:CheckBox ID="CB_SelItem" runat="server"></asp:CheckBox>
                                <asp:HiddenField ID="HF_Id" runat="server" Value='<%# Eval("ID") %>' /></div>
                            </td>
                            <td>
                                <div class="tcle"><%#  Common.GetDisAreaNameById( Convert.ToInt32( Eval("areaid")))%></div>
                            </td>
                            <td title='<%# Common.GetGoodsName(Eval("GoodsID").ToString()) %>' style="width: 300px">
                                <div class="tcle"><%# Common.GetGoodsName(Eval("GoodsID").ToString())%></div>
                            </td>
                            <td>
                                <div class="tcle"><%# Common.GetCategoryName(Eval("categoryid").ToString()) %></div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <asp:Button ID="btnDel" runat="server" Text="删除" Style="display: none" OnClick="btnDel_Click" />
        <asp:Button ID="btnSearch" runat="server" Style="display: none" Text="搜索" OnClick="btnSearch_Click" />
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
    </div>
    </form>
    <script type="text/javascript">
        $('.tablelist tbody tr:odd').addClass('odd');
    </script>
</body>
</html>

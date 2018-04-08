<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsTemplateList.aspx.cs"
    Inherits="Company_Goods_GoodsTemplateList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>商品规格模板</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script>
        $(function () {
            //新增
            $(".click").click(function () {
                var height = document.documentElement.clientHeight; //计算高度
                var layerOffsetY = (height - 340) / 2; //计算宽度
              //  var index = showDialog('规格模板维护', 'GoodsTemplateEdit.aspx', '850px', '433px', layerOffsetY); //记录弹出对象
                var index = layerCommon.openWindow('规格模板维护', 'GoodsTemplateEdit.aspx', '850px', '433px');  //记录弹出对象
                $("#hid_Alert").val(index); //记录弹出对象
            })
            //编辑
            $(".edit").click(function () {
                var height = document.documentElement.clientHeight; //计算高度
                var layerOffsetY = (height - 340) / 2; //计算宽度
                var id = $(this).attr("tip"); //模板id
                var index = layerCommon.openWindow('规格模板维护', 'GoodsTemplateEdit.aspx?tempId=' + id, '850px', '433px');  //记录弹出对象
              //  var index = showDialog('规格模板维护', 'GoodsTemplateEdit.aspx?tempId=' + id, '850px', '433px', layerOffsetY); //记录弹出对象
                $("#hid_Alert").val(index); //记录弹出对象
            })
        })
        // 每页显示数量非空验证
        function ChkPage() {
            if ($("#txtPageSize").val() == "") {
                layerCommon.msg("每页显示数量不能为空", IconOption.错误);
                return false;
            } else if (parseInt($("#txtPageSize").val()) == 0) {
                layerCommon.msg("每页显示数量必须大于0", IconOption.错误);
                return false;
            } else {
                $("#btnSearch").click();
            }
            return true;
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
                    layerCommon.confirm('确定要删除模板', function () {
                        $("#btnDel").click()
                    });

                } else {
                    layerCommon.msg("请勾选需要删除的模板", IconOption.错误);
                    return false;
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-3" />
    <input type="hidden" id="hid_Alert" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="../GoodsNew/GoodsTemplatelist.aspx">商品规格模板</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <li class="click"><span>
                    <img src="../images/t01.png" /></span>新增</li>
                <li onclick="return Delete(1)"><span>
                    <img src="../images/t03.png" /></span>删除</li>
            </ul>
            <div class="right">
                <ul class="toolbar right ">
                    <li onclick="return ChkPage()"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                </ul>
                <ul class="toolbar3">
                    <li>模板名称:<input name="txtTemplate" runat="server" type="text" id="txtTemplate" class="textBox txtTemplate" maxlength="50" /></li>
                    <li>每页显示<input name="txtPageSize" type="text" class="textBox txtPageSize" id="txtPageSize"
                        style="width: 40px" runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />条</li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <!--信息列表 start-->
        <table class="tablelist">
            <thead>
                <tr>
                    <th class="t7">
                        <input type="checkbox" name="checkbox" id="CB_SelAll" onclick="SelectAll(this)" />
                    </th>
                    <th class="t4">
                        操作
                    </th>
                    <th class="t3">
                        模板名称
                    </th>
                    <th>
                        属性
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptTemplate" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <div class="tc"><asp:CheckBox ID="CB_SelItem" runat="server"></asp:CheckBox>
                                <asp:HiddenField ID="HF_Id" runat="server" Value='<%# Eval("ID") %>' /></div>
                            </td>
                            <td>
                               <div class="tc"> <a href="javascript:;" class="link edit" tip='<%# Eval("ID") %>'>编辑</a></div>
                            </td>
                            <td>
                               <div class="tcle"> <%# Eval("TemplateName")%></div>
                            </td>
                            <td>
                                <div class="tcle"><%# GetTemplateList(Convert.ToInt32(Eval("ID")))%></div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <asp:Button ID="btnDel" runat="server" Text="删除" Style="display: none" OnClick="btnDel_Click" />
        <asp:Button ID="btnSearch" runat="server" Style="display: none" Text="搜索" OnClick="btnSearch_Click" />
        <!--信息列表 end-->
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
    <!--遮照层-->
    <div class='Layer'>
    </div>
    <script type="text/javascript">
        $('.tablelist tbody tr:odd').addClass('odd');
    </script>
    </form>
</body>
</html>

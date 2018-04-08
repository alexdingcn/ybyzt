<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportGoods2.aspx.cs" Inherits="Company_ImportGoods2" %>

<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>商品导入</title>
    <link href="css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <style>
        .t6
        {
            width: 50px;
        }
        .t7
        {
            width: auto;
        }
        .t77
        {
            width: 100px;
        }
        .t8
        {
            width: auto;
        }
        tr th
        {
            line-height: 15px;
        }
    </style>
    <script>
        $(function () {
            //上一步
            $("#Search").click(function () {
                location.href = "ImportGoods.aspx";
            })
            //数据检查
            $("#lblbtnToExcel").click(function () {
                $(".liSenior").attr("style", "cursor:pointer");
                // $(".tabLine table th:last").show();
                $(".tabLine table tr").each(function (index, obj) {
                    $(this).find("td").find(".tle").show();
                })
            })
            //确定导入
            $(".liSenior").click(function () {
                if ($(this).css("cursor") != "not-allowed") {
                    $("#btnImport").click();
                } else {
                    layerCommon.msg("请先数据检查", IconOption.错误);
                    return false;
                }
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Button ID="btnImport" runat="server" Text="Button" Style="display: none" OnClick="btnImport_Click" />
    <uc1:Top ID="top1" runat="server" ShowID="nav-3" />
    <uc1:CompRemove runat="server" ID="Remove" ModuleID="3" />
    <div class="rightinfo" style="width:auto;">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="jsc.aspx">我的桌面 </a></li>
                <li>&gt;</li>
                <li><a href="GoodsNew/GoodsList.aspx" id="a1">商品列表</a></li>
                <li>&gt;</li>
                <li><a href="javascript:;" id="atitle">商品导入</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <div class="blank10">
        </div>
        <!--步骤 start-->
        <ul class="imStep">
            <li class="a1">1、上传导入文件<i class="arr1"></i></li>
            <li class="a2 cur">2、导入预览<i class="arr1"></i><i class="arr2"></i></li>
            <li class="a3">3、导入完成<i class="arr1"></i><i class="arr2"></i></li>
        </ul>
        <!--步骤 end-->
        <ul class="toolbar">
            <li id="Search"><span>
                <img src="images/tp3.png"></span>上一步</li>
            <li id="lblbtnToExcel"><span>
                <img src="images/t04.png"></span>数据检查</li>
            <li class="liSenior" style="cursor: not-allowed"><span>
                <img src="images/t15.png"></span>确认导入</li>
        </ul>
        <div class="blank10">
        </div>
        <div class="tabLine ">
            <table border="0" cellspacing="0" cellpadding="0">
                <thead>
                    <tr>
                        <th class="t7">
                            分类
                        </th>
                        <th class="t7">
                            商品名称
                        </th>
                        <th class="t7">
                            商品编码
                        </th>
                        <th class="t6">
                            计量<br />
                            单位
                        </th>
                        <th class="t77">
                            价格
                        </th>
                        <th class="t77">
                            库存
                        </th>
                        <th class="t7">
                            多规格字段设置
                        </th>
                        <th class="t6">
                            店铺<br />
                            显示
                        </th>
                        <th class="t6">
                            上架
                        </th>
                        <th class="t7">
                            商品详情描述
                        </th>
                        <th class="t7">
                            检查结果
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptGoods" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <div class="tc">
                                        <%#Eval("category") %></div>
                                </td>
                                <td>
                                    <ul class="tle">
                                        <li>
                                            <%#Eval("goodsname") %></li></ul>
                                </td>
                                <td>
                                    <div class="tc">
                                        <%#Eval("barcode") %></div>
                                </td>
                                <td>
                                    <div class="tc">
                                        <%#Eval("unit") %></div>
                                </td>
                                <td>
                                    <div class="tc">
                                        <%# Eval("price").ToString().ToDecimal(0) == 0 ? Eval("price").ToString() : Eval("price").ToString().ToDecimal(0).ToString("#0.00")%></div>
                                </td>
                                <td>
                                    <div class="tc">
                                        <%#Eval("inventory", "{0:F2}")%></div>
                                </td>
                                <td>
                                    <div class="tc">
                                        <%# string.Join("/",((string[])Eval("spec"))) %>
                                    </div>
                                </td>
                                <td>
                                    <div class="tc">
                                        <%#Eval("isrecommended")%></div>
                                </td>
                                <td>
                                    <div class="tc">
                                        <%#Eval("isoffline") %></div>
                                </td>
                                <td>
                                    <div class="tc" title=' <%#Eval("details") %>'>
                                        <%#Eval("details").ToString().Length > 10 ? Eval("details").ToString().Substring(0, 10) + "..." : Eval("details").ToString()%></div>
                                </td>
                                <td>
                                    <ul class="tle" style="display: none;">
                                        <li <%#Eval("chkstr").ToString() != "数据正确！"?"style='color:Red'":"" %>>
                                            <%#Eval("chkstr")%></li></ul>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>
    </form>
</body>
</html>

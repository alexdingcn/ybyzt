<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Shop.aspx.cs" Inherits="Distributor_Shop" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>代理商后台-购物车</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/shop.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../Company/js/js.js" type="text/javascript"></script>
    <script src="../js/shopcart.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <Head:Head ID="Head1" runat="server" />
    <input type="hidden" id="hidsDigits" runat="server" />
    <input type="hidden" id="hidIsInve" runat="server" />
    <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="nav-1" />
        <div class="rightCon">
            <div class="info">
                <a id="navigation1" href="UserIndex.aspx">我的桌面</a>> <a id="navigation2" href="Shop.aspx"
                    class="cur">购物车</a>
            </div>
            <div class="whiteBg">
                <!--[if !IE]>购物车功能区 start<![endif]-->
                <div class="goods-gn">
                    <div class="left">
                        <a href="/Distributor/GoodsList.aspx" class="bule-btn">选购商品</a> <a href="javascript:void(0);"
                            class="gray-btn btnEmpty">清除购物车</a>
                    </div>
                    <div class="userFun left" style=" margin-top: 4px;">
                        <label class="head">选择厂商：</label>
                        <select id="ddrComp" name="" style=" width:150px; margin-right: 10px;" runat="server" class="xl" onserverchange="comp_ServerChange"></select>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <!--[if !IE]>购物车功能区 end<![endif]-->
                <!--[if !IE]>商品展示区 start<![endif]-->
                <div class="goods-zs goods-shop">
                    <table border="0" cellspacing="0" cellpadding="0" class="tabNr">
                        <thead>
                            <tr>
                                <th class="t1">
                                    商品描述
                                </th>
                                <th class="t2">
                                </th>
                                <th class="t3">
                                    单价
                                </th>
                                <th style='<%= IsInve==0?"": "display:none"  %>'>
                                    商品库存
                                </th>
                                <th class="t3">
                                    数量
                                </th>
                                <th class="t3">
                                    小计
                                </th>
                                <th class="t4">
                                    操作
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rprCart" runat="server">
                                <ItemTemplate>
                                    <tr id='tr_<%# Eval("ID") %>'>
                                        <td>
                                            <div class="sPic">
                                                <span><a target="_blank" href="GoodsInfo.aspx?goodsId=<%# Eval("GoodsID") %>&goodsInfoId=<%# Eval("GoodsInfoID") %>">
                                                    <img src="<%# Common.GetPicURL(Eval("pic").ToString(), "resize200") %>" width="70" height="70">
                                                </a></span><a target="_blank" href="GoodsInfo.aspx?goodsId=<%# Eval("GoodsID") %>&goodsInfoId=<%# Eval("GoodsInfoID") %>">
                                                    <%# Common.MySubstring(Eval("GoodsName").ToString(),30,"...") %></a>
                                                <%# SelectGoods.ProType(Eval("ProID"))%>
                                                <br />
                                                <%# Eval("BarCode")%>
                                            </div>
                                        </td>
                                        <td valign="top">
                                            <ul class="specs2">
                                                <%# SelectGoods.Viewinfos(Eval("GoodsInfos"))%>
                                            </ul>
                                        </td>
                                        <td>
                                            <div class="tc tprice">
                                                <%#  BLL.Common.GetGoodsPrice( Convert.ToInt32( Eval("CompID")), Convert.ToInt32(Eval("disid")), Convert.ToInt32(Eval("goodsinfoid"))).ToString("0.00")%>
                                            </div>
                                        </td>
                                        <td style='<%# IsInve==0?"": "display:none"  %>'>
                                            <div class="tc">
                                                <!--商品库存-->
                                                <span id="tr_Inve_<%# Eval("Id") %>">
                                                    <%# Eval("Inventory", "{0:F2}")%>
                                                </span>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="sl" inventory='<%# Eval("Inventory", "{0:F2}")%>'
                                                data-goodsinfoid='<%# Eval("GoodsInfoID") %>' data-proid='<%# Eval("ProID") %>'>
                                                <a href="javascript:void(0);" class="minus" id="minus">-</a>
                                                <input type="text" onclick='InputFocus(this)' id="txtGoodsNum" class="box txtGoodsNum"
                                                    value="<%# OrderInfoType.GetNum(Eval("GoodsNum").ToString().ToDecimal(0)) %>" />
                                                <a href="javascript:void(0);" class="add" id="add">+</a>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="tc sumAmount">
                                                ￥
                                                <%# Totalprice(Convert.ToInt32(Eval("CompID")), Convert.ToInt32(Eval("disid")), Convert.ToInt32(Eval("goodsinfoid")), OrderInfoType.GetNum(Convert.ToDecimal(Eval("GoodsNum")))).ToString("#,##0.00")%>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="tc alink">
                                                <a tip_id="<%# Eval("ID") %>" tip="<%# Eval("GoodsInfoID") %>" href="javascript:void(0);"
                                                    class="delcart">删除</a>
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                    <!--[if !IE]>购物车金额 start<![endif]-->
                    <div class="options-box">
                        <div class="right">
                            <div class="price-sum">
                                合计：<i class="price">￥</i> <i class="price" id="sumPrice" runat="server">0</i>元</div>
                            <a href="javascript:void(0);" class="btn-area right btnSave">立即下单</a>
                        </div>
                    </div>
                    <!--[if !IE]>购物车金额 end<![endif]-->
                    <div class="blank20">
                    </div>
                </div>
                <!--[if !IE]>商品展示区 end<![endif]-->
            </div>
        </div>
        <div class="blank20">
        </div>
    </div>
    </form>
</body>
</html>

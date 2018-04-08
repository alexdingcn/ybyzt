<%@ Page Language="C#" AutoEventWireup="true" CodeFile="selectgoods.aspx.cs" Inherits="Distributor_newOrder_selectgoods" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择商品</title>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=2015001311899" rel="stylesheet"
        type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
</head>
<body>
    <style>
        .goods-gn
        {
            padding: 15px 20px 12px 20px;
        }
        .myPagination
        {
            text-align: right;
            padding: 4px 40px 10px 40px;
        }
        .goods-see .nr
        {
            height: 387px;
            overflow: hidden;
            overflow-y: scroll;
            position: relative;
        }
        .goods-zs .sPic
        {
            padding: 6px 10px 5px 50px;
            min-height: 38px;
            position: relative;
            line-height: 22px;
        }
        .goods-zs .sPic span
        {
            border: 1px solid #ddd;
            overflow: hidden;
            position: absolute;
            top: 8px;
            left: 0px;
        }
        
        .goods-see .title
        {
            padding-right: 18px;
            background: #f7f7f7;
        }
       @media \0screen\,screen\9
       {
        #checkbox3
        {
             padding-left:20px;
        }}
    </style>
    <form id="form1" runat="server">
    <!--选择商品 start-->
    <div class="popup po-goods">
        <input type="hidden" id="hidsDigits" runat="server" />
        <input type="hidden" id="hidCompId" runat="server" value="1028" />
        <input type="hidden" id="hidDisId" runat="server" value="1113" />
        <input type="hidden" id="hidIsInve" runat="server" value="0" />
        <input type="hidden" id="hidImgViewPath" runat="server" value="" />
        <input type="hidden" id="hidgoodsInfoId" runat="server" value="" />
        <input type="hidden" id="hidtype" runat="server" value=""/>
        <input type="hidden" id="hidgoodsInfoIdList" runat="server" value="" />
        <input type="hidden" id="hidIndex" runat="server" value="" />
        <input type="hidden" id="hidSelectGoods" value="" />
        <input type="hidden" id="hidUtype" value="" runat="server" />
        <input type="hidden" id="hidStock" value="" runat="server" />
        <%--<div class="po-title">选择商品<a href="" class="close"></a></div>--%>
        <!--功能 start-->
        <div class="goods-gn">
            <div class="fl left">
                <div class="bt">
                    商品分类：</div>
                <div class="ca-box left">
                    <i class="dx">
                        <label class="category">
                        </label>
                        <i class="arrow"></i></i>
                    <div class="menu2 goodscat" runat="server" id="menu2" style="display: none;">

                    </div>
                </div>
            </div>
            <div class="s-box left">
                <div class="bt">
                    商品编码/名称：</div>
                <div class="s left">
                    <input name="" id="txtGoods" type="text" maxlength="50" autocomplete="off" value=""
                        class="box">
                    <a href="javascript:void(0);" class="searchBtn"></a>
                </div>
            </div>
            <ul class="fn left">
                <li id="goodspro"><a href="javascript:void(0);" id="agoodspro" class="k-icon" tip="">
                </a><a href="javascript:void(0);">促销商品</a></li>
                <%--<li id="pro"><a href="javascript:void(0);" id="apro" class="k-icon" tip=""></a><a
                    href="javascript:void(0);">特价商品</a></li>--%>
            </ul>
            <div class="clear">
            </div>
        </div>
        <!--功能 end-->
        <!--商品 start-->
        <div class="goods-zs goods-see">
            <div class="tabLine">
                <div class="title">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th class="t5">
                                    <input type="checkbox" id="checkbox3" class="regular-checkbox selectAll" />
                                    <label class="selectAll" for="checkbox3" style="line-height: 20px;"></label>
                                </th>
                                <th class="">
                                    商品名称
                                </th>
                                <th class="t2" align="left">
                                    规格属性
                                </th>
                                <th class="t5">
                                    单位
                                </th>
                                <th class="t3">
                                    单价
                                </th>
                                <th class="t3" runat="server" id="tdIsInve">
                                    库存
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div class="nr">
                    <table class="goods" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <%--<asp:Repeater ID="rpGoodsInfo" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td class="t5" align="center">
                                        <input type="checkbox" id="checkbox-3-1" class="regular-checkbox" />
                                        <label for="checkbox-3-1"></label>
                                        </td>
                                        <td>
                                        <div class="sPic">
                                            <span>
                                                <a href="">
                                                    <img src="../../Distributor/newOrder/images/sPic.jpg" width="40" height="40">
                                                </a>
                                            </span>
                                            <a href="" class="code">商品编码：P1046000017
                                                <div class="sale-box">
                                                    <i class="sale">促销</i>
                                                    <div class="sale-txt">
                                                        <i class="arrow"></i>满2件，总价打8.00折，参与订单满减活动
                                                    </div>
                                                </div>
                                            </a>
                                            <a href="" class="name">
                                                <%# Eval("GoodsName") %>
                                                <i>华硕（ASUS）碉堡K30DA台式电脑华硕（ASUS）碉堡K30DA台式电脑</i>
                                            </a>
                                        </div>
                                        </td>
                                        <td class="t2"><div class="tc">颜色：红，尺码：175</div></td>
                                        <td class="t5"><div class="tc">件</div></td>
                                        <td class="t4"><div class="tc">￥800.00</div></td>
                                        <td class="t4"><div class="tc">50</div></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>--%>
                        </tbody>
                    </table>
                </div>
            </div>
            <!--分页 start-->
            <div class="blank10">
            </div>
            <div class="paging">
                <%--<a href="" class="tf">上一页</a>
                    <a class="cur">1</a>
                    <a href="" class="tf">2</a>
                    <a href="" class="tf">3</a>
                    <a href="" class="tf">...</a>
                    <a href="" class="tf">下一页</a>
                    <i class="tf2">共6页</i>
                    <i class="tf2">到第<input name="" type="text" class="box" value="1">页</i>
                    <a href="" class="tf">确定</a>--%>
                <%--<webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true"  ShowFirstLast="false" PageIndexBoxType="TextBox" PageSize="12"  PrevPageText="上一页" NextPageText="下一页"  PagingButtonsStyle="border-right: 1px solid #e5e5e5;"
                            PagingButtonsClass="tf" PagingButtonClass="tf" AlwaysShow="True" UrlPaging="false" 
                            ShowPageIndexBox="Always"  TextAfterPageIndexBox="&nbsp;页&nbsp;"
                            PageIndexBoxClass="tf" TextBeforePageIndexBox="<i class='tf2'>到第</i>" 
                            SubmitButtonClass="tf" SubmitButtonStyle="" SubmitButtonText="确定" 
                            ShowCustomInfoSection="Never" CustomInfoSectionWidth="20%"
                            CustomInfoHTML="<i class='tf2'>共%PageCount%页</i>" CurrentPageButtonClass="cur" 
                            CssClass="page" NumericButtonCount="5" OnPageChanged="Pager_PageChanged">
                        </webdiyer:AspNetPager>--%>
            </div>
            <!--分页 end-->
        </div>
        <!--商品 end-->
        <div class="po-btn">
            <a href="javascript:void(0);" class="gray-btn" id="btnCancel">取消</a> <a href="javascript:void(0);"
                class="btn-area" id="btnConfirm">确定</a>
        </div>
    </div>
    <!--选择商品 end-->
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/MyClassPaging.js" type="text/javascript"></script>
    <script src="js/order_pla.js?v=2018011700001001" type="text/javascript"></script>
    </form>
</body>
</html>

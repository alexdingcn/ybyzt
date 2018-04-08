<%@ Page Language="C#" AutoEventWireup="true" CodeFile="selectgoods.aspx.cs" Inherits="Company_GoodsNew_selectgoods" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
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
        <input type="hidden" id="hidgoodsInfoIdList" runat="server" value="" />
        <input type="hidden" id="hidIndex" runat="server" value="" />
        <input type="hidden" id="hidSelectGoods" value="" />
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
                        <div class="sorts">
                            <div class="sorts1">
                                <a href="javascript:;"><i class="arrow2"></i>衣服</a></div>
                        </div>
                        <div class="sorts">
                            <div class="sorts1">
                                <a href="javascript:;"><i class="arrow2"></i>衣服</a></div>
                            <ul class="sorts2">
                                <li><a href="javascript:;"><i class="arrow3"></i>针织衫</a>
                                    <ul class="sorts3">
                                        <li><a href="javascript:;">针织衫</a></li>
                                        <li><a href="javascript:;">短袖</a></li>
                                    </ul>
                                </li>
                                <li><a href="javascript:;"><i class="arrow2"></i>短袖</a></li>
                            </ul>
                        </div>
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
                                    <input type="checkbox" id="checkbox3" class="regular-checkbox " /><label class="selectAll"
                                        for="checkbox3" style="line-height: 20px;"></label>
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
                            </tr>
                        </thead>
                    </table>
                </div>
                <div class="nr">
                    <table class="goods" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
            <!--分页 start-->
            <div class="blank10">
            </div>
            <div class="paging">
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
    <script src="order_pla.js?v=201500131157" type="text/javascript"></script>
    </form>
</body>
</html>

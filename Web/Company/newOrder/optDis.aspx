<%@ Page Language="C#" AutoEventWireup="true" CodeFile="optDis.aspx.cs" Inherits="Company_newOrder_optDis" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择代理商</title>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=201507261156" rel="stylesheet"
        type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="hidCompID" value="" runat="server" />
    <!--选择代理商 start-->
    <div class="popup po-goods optJss">
        <!--功能 start-->
        <div class="goods-gn">
            <div class="fl left">
                <div class="bt">
                    代理商分类：</div>
                <div class="ca-box left">
                    <div class="dx">
                        <label class="distype">
                        </label>
                        <i class="arrow"></i>
                    </div>
                    <div class="menu2 disopt" runat="server" id="menu2" style="display: none;">
                        <div class="sorts">
                            <div class="sorts1">
                                <a href=""><i class="arrow2"></i>衣服</a></div>
                        </div>
                        <div class="sorts">
                            <div class="sorts1">
                                <a href=""><i class="arrow2"></i>衣服</a></div>
                            <ul class="sorts2">
                                <li><a href=""><i class="arrow3"></i>针织衫</a>
                                    <ul class="sorts3">
                                        <li><a href="">针织衫</a></li>
                                        <li><a href="">短袖</a></li>
                                    </ul>
                                </li>
                                <li><a href=""><i class="arrow2"></i>短袖</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div class="s-box left">
                <div class="s left" style="width: 300px;">
                    <input name="" type="text" id="disnc" value="" placeholder="代理商编码/名称" autocomplete="off"
                        maxlength="50" class="box" style="width: 280px;">
                    <a href="javascript:;" class="searchBtn"></a>
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
                                <%--<th class="t5">
                                </th>--%>
                                <th class="">
                                    代理商名称
                                </th>
                                <th class="t2">
                                    分类
                                </th>
                                <th class="t2">
                                    地区
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div class="nr">
                    <table border="0" class="optdis" cellspacing="0" cellpadding="0">
                        <tbody>
                            <%--<tr>
                                <td class="t5" align="center">
                                    <input type="checkbox" id="checkbox-6-1" class="regular-checkbox" /><label for="checkbox-6-1"></label>
                                </td>
                                <td>
                                    <div class="tc">
                                        上海协强网络有限公司</div>
                                </td>
                                <td class="t2">
                                    <div class="tc">
                                        华东地区</div>
                                </td>
                                <td class="t2">
                                    <div class="tc">
                                        上海</div>
                                </td>
                            </tr>--%>
                        </tbody>
                    </table>
                </div>
            </div>
            <!--分页 start-->
            <div class="blank10">
            </div>
            <div class="paging">
                <%-- <a href="" class="tf">上一页</a>
                    <a class="cur">1</a>
                    <a href="" class="tf">2</a>
                    <a href="" class="tf">3</a>
                    <a href="" class="tf">...</a>
                    <a href="" class="tf">下一页</a>
                    <i class="tf2">共6页</i>
                    <i class="tf2">到第<input name="" type="text" class="box" value="1">页</i>
                    <a href="" class="tf">确定</a>--%>
            </div>
            <!--分页 end-->
        </div>
        <!--商品 end-->
        <div class="po-btn">
            <a href="javascript:;" class="gray-btn" id="btnCancel">取消</a> 
            <%--<a href="javascript:;" class="btn-area none" id="btnConfirm">确定</a>--%>
        </div>
    </div>
    <!--选择代理商 end-->
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/MyClassPaging.js" type="text/javascript"></script>
    <script src="../../Distributor/newOrder/js/ordercommon.js?v=201608170930" type="text/javascript"></script>
    <script>

        var SaleFunction = {
            BindSaleData: function (data) {
                var json = data.rows;
                var disHTML = "";

                if (json.length > 0) {
                    $.each(json, function (index, item) {
                        disHTML += "<tr val=\"" + item.ID + "\" tipname=\"" + item.DisName + "\">";
                        ////                        disHTML += " <td class=\"t5\" align=\"center\"><input type=\"radio\" name=\"dis\" id=\"checkbox-6-" + +index + "\" class=\"regular-checkbox chkbox\" value=\"" + item.ID + "\" tipname=\"" + item.DisName + "\" /><label for=\"checkbox-6-" + index + "\"></label></td>";
                        disHTML += "<td><div class=\"tc\">" + item.DisName + "</div></td>";
                        disHTML += "<td class=\"t2\"><div class=\"tc\">" + item.TypeName + "</div></td>";
                        disHTML += "<td class=\"t2\"><div class=\"tc\">" + item.AreaName + "</div></td>";
                        disHTML += "</tr>";
                    });

                } else {
                    disHTML += '<tr><td colspan="4" class="t2"><div class="tc">暂无数据</div></td></tr>';
                }
                $("table.optdis tbody").empty().append(disHTML);

                return this;
            },
            GetSalePaginData: function () {
                $(".paging").myPagination({
                    currPage: 1,
                    pageCount: 1,
                    pageSize: 12,
                    btnsize: 5,
                    IsShowOnePaging: true,
                    cssStyle: 'myPagination',
                    info: {
                        msg_on: false,
                        first_on: false,
                        last_on: false,
                        prev: "上一页",
                        next: "下一页"
                    },
                    ajax: {
                        on: true,
                        callback: 'SaleFunction.BindSaleData',
                        url: "../../Handler/GetPageDataSource.ashx?PageAction=optdis",
                        dataType: 'json',
                        params: { "CompId": '<%=Request["compid"] %>', "disnc": $.trim($("#disnc").val()), "distype": $.trim($(".distype").attr("tip")) },
                        ajaxStart: function () {
                            //$("table.optdis tbody").empty().append('<tr><td colspan="9" style="width:100%;">数据正在加载中请稍候....</td></tr>');
                        }, ajaxStop: function () {
                        },
                        ajaxError: function () {

                            $("table.optdis tbody").empty().append('<tr><td colspan="4" class="t2"><div class="tc">暂无数据</div></td></tr>');
                        }
                    }
                });
                return this;
            },
            BindSaleBtnEvent: function () {
                $(".searchBtn").on("click", function () {
                    SaleFunction.GetSalePaginData();
                })

                $(document).on("click", "div.disopt  .sorts a,.sorts .sorts1 a", function () {
                    var tip = $(this).attr("tip");
                    var tipvla = $(this).text();

                    $(".distype").text(tipvla);
                    $(".distype").attr("tip", tip);
                    $(this).parents().parents("div.disopt").css("display", "none");
                    SaleFunction.GetSalePaginData();
                });
                return this;
            }
        }

        $(function () {
            SaleFunction.GetSalePaginData().BindSaleBtnEvent();

            //取消
            $(document).on("click", "#btnCancel", function () {
                window.parent.CloseGoods();
            });

            $("#btnConfirm").click(function () {
                var str = "";
                var disn = "";
                $("table.optdis tbody tr").each(function (item) {
                    //if ($(this).find(".chkbox").is(':checked')) {
                    str += $(this).attr("val");
                    disn += $(this).attr("tipname");
                    //}
                });
                //alert(str + ":" + disn);
                window.parent.$(".add2").trigger("click"); //删除原有的商品数据行
                window.parent.$(".txtRebate").val("0.00"); //清空使用返利
                window.parent.$("#lblFanl").text("0.00"); //清空返利折扣
                window.parent.$("#hrOrderInv").val(""); //开票id
                window.parent.$("#hidLookUp").val(""); //清空抬头
                window.parent.$("#hidContext").val(""); //清空发票内容
                window.parent.$("#hidBank").val(""); //清空银行
                window.parent.$("#hidAccount").val(""); //清空账户
                window.parent.$("#hidRegNo").val(""); //清空登记号
                window.parent.$("#OrderNote").val(""); //清空订单备注
                window.parent.disBind(str, disn);
                window.parent.CloseGoods();
            });

            //tr选中代理商
            $(document).on("click", "table.optdis tbody tr", function () {
                var str = "";
                var disn = "";
                str += $(this).attr("val");
                disn += $(this).attr("tipname");
                if (str == window.parent.$("#hidDisID").val()) {
                    window.parent.CloseGoods();
                    return;
                }
                var html = "<tr><td class=\"t8\"><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td><div class=\"search\"><input name=\"\" type=\"text\" class=\"box project2\" /><a class=\"opt-i\"></a><div class=\"search-opt\" style=\"display: none;\"><ul class=\"list\"></ul><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt2-i\"></i>选择商品</a></div></div></div></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
                window.parent.$(".tabLine table tbody").append(html);
                window.parent.$("#lblTotalAmount").text("0.00"); //清空商品总额
                window.parent.$("#lblCux").text("0.00"); //清空促销优惠
                window.parent.$("#lblPostFee").text("0.00"); //清空运费
                window.parent.$("#lblYFPrice").text("0.00"); //清空应付总额
                window.parent.$(".txtRebate").val("0.00"); //清空使用返利
                window.parent.$("#lblFanl").text("0.00"); //清空返利折扣
                window.parent.$("#hrOrderInv").val(""); //开票id
                window.parent.$("#hidLookUp").val(""); //清空抬头
                window.parent.$("#hidContext").val(""); //清空发票内容
                window.parent.$("#hidBank").val(""); //清空银行
                window.parent.$("#hidAccount").val(""); //清空账户
                window.parent.$("#hidRegNo").val(""); //清空登记号
                window.parent.disBind(str, disn);
                window.parent.CloseGoods();
            });

            //代理商分类折叠开
            $(document).on("click", "div.disopt .sorts1 .arrow2,div.disopt .sorts2 .arrow3", function () {
                var arrow = $(this).attr("class");
                if (arrow == "arrow2") {
                    $(".sorts2").css("display", "none");
                    var tipdis = $(this).parent().siblings("ul[class=\"sorts2\"]").attr("tipdis");
                    if (tipdis == "no") {
                        $(this).parent().siblings("ul[class=\"sorts2\"]").attr("tipdis", "ok");
                        $(this).parent().siblings("ul[class=\"sorts2\"]").css("display", "block");
                    }
                    else {
                        $(this).parent().siblings("ul[class=\"sorts2\"]").attr("tipdis", "no");
                        $(this).parent().siblings("ul[class=\"sorts2\"]").css("display", "none");
                    }
                } else {
                    $(".sorts3").css("display", "none");
                    var tipdis = $(this).siblings("ul[class=\"sorts3\"]").attr("tipdis");
                    if (tipdis == "no") {
                        $(this).siblings("ul[class=\"sorts3\"]").attr("tipdis", "ok");
                        $(this).siblings("ul[class=\"sorts3\"]").css("display", "block");
                    }
                    else {
                        $(this).siblings("ul[class=\"sorts3\"]").attr("tipdis", "no");
                        $(this).siblings("ul[class=\"sorts3\"]").css("display", "none");
                    }
                }
            });


            //代理商分类
            $(document).on({
                "mouseover": function (e) {
                    $(".menu2").css("display", "block");
                },
                "mouseout": function (e) {
                    $(".menu2").css("display", "none");
                }
            }, ".ca-box");
        });
    </script>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="orderBuy.aspx.cs" Inherits="Company_newOrder_orderBuy" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>新建订单</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        rel="stylesheet" type="text/css" />
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/ajaxfileupload.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
    <script src="../js/js.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
    <script src="../../js/autoTextarea.js" type="text/javascript"></script>
    <script src="js/order2.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
    <script>
        //显示的商品 strtext=="" focus 事件 !="" keyup事件 strtext文本框输入的内容
        function showGoods(strtext, inindex, disId) {
            if (disId == undefined || disId == "" && $("#hidDisID").val() == "") {
                layerCommon.msg("请选择代理商", IconOption.错误);
                return false;
            } else {
                if (disId == undefined || disId == "") {
                    disId = $("#hidDisID").val();
                }
            }
            var hid = ""; //筛选下拉的商品
            //绑定下拉商品
            //第一次赋值没完成时
            if ($.trim($(".divGoodsName").text()) == "") {
                var compId = $("#hidCompId").val();
                $.ajax({
                    type: "post",
                    data: { ck: Math.random(), action: "dislist", compId: compId, disId: disId },
                    dataType: "text",
                    async: false,
                    success: function (data) {
                        hid = eval('(' + data + ')'); //筛选下拉的商品
                    }
                })
            } else {
                hid = eval('(' + $(".divGoodsName").text() + ')'); //筛选下拉的商品
            }
            //先清空下拉商品 再绑定
            $(".tabLine table tbody tr").eq(inindex).find(".search-opt .list").html("");
            //当前行下拉商品显示
            $(".tabLine table tbody tr").eq(inindex).find(".search-opt").show();
            $(hid).each(function (index, obj) {
                //下拉商品最多只显示5条
                if ($(".tabLine table tbody tr").eq(inindex).find(".search-opt .list li").length < 5) {
                    if (strtext != "") {
                        //根据商品名称和商品编码筛选商品
                        if (obj.GoodsName.indexOf(strtext) != -1 || obj.BarCode.toLocaleLowerCase().indexOf(strtext.toLocaleLowerCase()) != -1) {
                            $(".tabLine table tbody tr").eq(inindex).find(".search-opt .list").append("<li tip=\"" + obj.ID + "\"><span><a href=\"javascript:;\"><img src=\"" + ($.trim(obj.Pic) != "" ? '<%=Common.GetWebConfigKey("ImgViewPath")  %>' + "GoodsImg/" + obj.Pic : "../../images/pic.jpg") + "\" width=\"40\" height=\"40\"></a></span><i href=\"javascript:;\" class=\"code\">商品编码：" + obj.BarCode + ($.trim(obj.proTypes) != "" ? obj.Cux : "") + "</i><i href=\"javascript:;\" class=\"name\">" + GetGoodsName(obj.GoodsName, $.trim(obj.ValueInfo), 1) + "<i>" + GetGoodsName(obj.GoodsName, $.trim(obj.ValueInfo), 2) + "</i></i></li>");
                        }
                    } else {
                        $(".tabLine table tbody tr").eq(inindex).find(".search-opt .list").append("<li tip=\"" + obj.ID + "\"><span><a href=\"javascript:;\"><img src=\"" + ($.trim(obj.Pic) != "" ? '<%=Common.GetWebConfigKey("ImgViewPath")  %>' + "GoodsImg/" + obj.Pic : "../../images/pic.jpg") + "\" width=\"40\" height=\"40\"></a></span><i href=\"javascript:;\" class=\"code\">商品编码：" + obj.BarCode + ($.trim(obj.proTypes) != "" ? obj.Cux : "") + "</i><i href=\"javascript:;\" class=\"name\">" + GetGoodsName(obj.GoodsName, $.trim(obj.ValueInfo), 1) + "<i>" + GetGoodsName(obj.GoodsName, $.trim(obj.ValueInfo), 2) + "</i></i></li>");
                    }
                }
            })
        }
        //绑定数据（商品id，厂商id,当前选择行的索引,代理商id）
        function GoodsList(goodsInfoId, compId, inindex, disId) {
            var Utype=$.trim($("hidUtype").val());
            $("#hidDisID").val(disId);
            var Digits = '<%=OrderInfoType.rdoOrderAudit("订单下单数量是否取整", this.CompID) %>'; //小数位数
            var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;
            if (goodsInfoId != "") {
                var xyindex = 0; //最大的索引
                //冒泡排序获取最大的索引
                //如果间隔行去选择商品 则对应不到对应的价格 所以采用了下面这个方法
                $(".tabLine table tbody tr").each(function (x, indexy) {
                    if ($(".tabLine table tbody tr").eq(x).attr("trindex") != undefined) {
                        $(".tabLine table tbody tr").each(function (y, indexyz) {
                            y = x + 1;
                            if ($(".tabLine table tbody tr").eq(y).attr("trindex") != undefined) {
                                if (parseInt($(".tabLine table tbody tr").eq(y).attr("trindex")) < parseInt($(".tabLine table tbody tr").eq(x).attr("trindex"))) {
                                    xyindex = $(".tabLine table tbody tr").eq(y).attr("trindex");
                                    $(".tabLine table tbody tr").eq(y).attr("trindex", $(".tabLine table tbody tr").eq(x).attr("trindex"));
                                    $(".tabLine table tbody tr").eq(x).attr("trindex", xyindex);
                                }
                            } else {
                                xyindex = $(".tabLine table tbody tr").eq(x).attr("trindex");
                            }
                        })
                    }
                })
                $.ajax({
                    type: "post",
                    url: "../../Handler/GetPageDataSource.ashx?PageAction=GetGoodsInfo",
                    data: { ck: Math.random(), CompId: compId, DisId: disId, goodsInfoId: goodsInfoId, Utype: Utype },
                    dataType: "json",
                    success: function (data) {
                        var html = ""; //绑定的商品数据
                        //var z = 0;
                        var indexxy = 0; //获取选中过来的商品数量 便于相加索引
                        $(data).each(function (indexs, obj) {
                            // z = goodsInfoId.split(",").length - 1;
                            indexxy = goodsInfoId.split(",").length; //获取选中过来的商品数量 便于相加索引
                            var index = parseInt(parseInt(xyindex) + parseInt(indexs) + parseInt(indexxy)); //绑定商品数据行的索引
                            var str = '<%= IsInve==0?"":"display:none"  %>'; //是否显示库存
                            html += "<tr trindex=\"" + index + "\" trindex2=\"" + index + "\" id=\"\" tip=\"" + obj.ID + "\"><td class=\"t8\"><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td><div class=\"sPic\"><a class=\"opt-i2\"></a><span><a href=\"javascript:;\"><img src=\"" + ($.trim(obj.Pic) != "" ? '<%=Common.GetWebConfigKey("ImgViewPath")  %>' + "GoodsImg/" + obj.Pic : "../../images/pic.jpg") + "\" width=\"60\" height=\"60\"></a></span><a href=\"javascript:;\" class=\"code\">商品编码：" + obj.BarCode + ($.trim(obj.proTypes) != "" ? obj.Cux : "") + "</a><a href=\"javascript:;\" class=\"name\">" + GetGoodsName(obj.GoodsName, $.trim(obj.ValueInfo), 1) + "<i>" + GetGoodsName(obj.GoodsName, $.trim(obj.ValueInfo), 2) + "</i></a></div></td><td><div class=\"tc\">" + obj.Unit + "</div></td><td><input type=\"text\" tip2=\"" + index + "\" class=\"boxs divprice" + index + "\"   value=\"" + parseFloat($.trim(obj.pr)).toFixed(2) + "\" onfocus=\"InputFocus(this)\" onblur=\"priceBlur(this)\"  onkeyup=\"KeyInt2(this)\" maxlength=\"9\" ><input type=\"hidden\" class=\"hidPrice\" value=\"" + parseFloat($.trim(obj.pr)).toFixed(2) + "\" /></td><td style=\"" + str + "\"><div class=\"tc\"><input type=\"hidden\" id=\"hidInventory_" + index + "\" value=\"" + parseFloat(obj.Inventory).toFixed(sDigits) + "\" />" + parseFloat(obj.Inventory).toFixed(sDigits) + "</div></td><td><div class=\"sl divnum\" tip=\"" + goodsInfoId + "\" tip2=\"" + index + "\"><a href=\"javascript:void(0);\"  class=\"minus\">-</a><input type=\"text\" class=\"box txtGoodsNum txtGoodsNum" + index + "\" onfocus=\"InputFocus(this)\"  onkeyup='KeyInt2(this)' maxlength=\"9\"  value=\"" + (parseFloat(obj.Inventory).toFixed(sDigits) < 1 ? parseFloat(obj.Inventory).toFixed(sDigits) : parseFloat(1).toFixed(sDigits)) + "\"><a href=\"javascript:void(0);\"  class=\"add\">+</a></div></td><td><div class=\"tc lblTotal_" + index + "\">￥" + parseFloat($.trim(obj.pr)).toFixed(2) + "</div></td><td><div class=\"tc alink\"><a href=\"javascript:;\" class=\"aremark" + index + "\">添加</a></div></td></tr>";
                        })
                        $(".tabLine table tbody tr").eq(inindex).replaceWith(html); //替换当前选择时的行
                    }, complete: function () {
                        //重新计算价格
                        $(".tabLine table tbody tr").each(function (index, obj) {
                            if ($(".tabLine table tbody tr").eq(index).attr("tip") != undefined && $(".tabLine table tbody tr").eq(index).attr("tip") != "") {
                                onchengSum($(".tabLine table tbody tr").eq(index).attr("tip"), $(".tabLine table tbody tr").eq(index).attr("trindex2"), 0, disId);
                            }
                        })
                    }
                })
            }
        }

        //价格计算（商品id，当前行的索引,（-1 减 0 文本输入 1 加）数量,代理商id）
        function onchengSum(id, index, type, disId) {
            var Price;  //商品单价
            var snum;   //商品数量
            var SumTotal = 0;  //商品小计
            var R;   //备注
            // var disId; // 代理商Id
            if (disId == undefined || disId == "") {
                disId = $("#hidDisID").val();
            }
            var CompId = $("#hidCompId").val();
            var AddrId = ""; // $("#hidAddrID").val();
            var Digits = '<%=OrderInfoType.rdoOrderAudit("订单下单数量是否取整", this.CompID) %>'; //小数位数
            var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;
            var IsInve = '<%=IsInve %>'; //是否启用商品库存，默认0、启用库存
            snum = $(".txtGoodsNum" + index).val(); //当前行的数量
            var Num = parseFloat(snum).toFixed(sDigits);
            var batePrice = $(".txtRebate").val();
            if (parseFloat(type) == -1) {//减数量
                Num = parseFloat(Num) - parseFloat(1);
                if (parseFloat(Num) <= parseFloat(0)) {
                    Num = parseFloat(1).toFixed(sDigits)
                }
            } else if (parseFloat(type) == 1) {//加数量
                Num = parseFloat(Num) + parseFloat(1);
                if (Num > 999999999) {
                    Num = parseFloat(999999999).toFixed(sDigits)
                }
            } else {//手动输入数量
                if (parseFloat(Num) <= parseFloat(0)) {
                    Num = parseFloat(1).toFixed(sDigits)
                }
            }
            //如果数量为空或者 不为数字的时候默认赋值1
            if (Num.toString() == "" || isNaN(Num) == true) {
                Num = parseFloat(1).toFixed(sDigits)
            }
            Num = parseFloat(Num).toFixed(sDigits);
            Price = $.trim($(".divprice" + index).val()); //商品单价
            if ($(".divprice" + index).val() == "") {
                $(".divprice" + index).val(parseFloat(0).toFixed(2));
            }
            R = $(".divremark" + index).next(".cur").text(); //备注
            Price = Price.replace(/,/gm, '');
            if (isNaN(Price)) {
                Price = "0.00";
            }
            if (parseFloat(Price) < 0 || Price.toString() == "") {
                Price = 0.00;
            }
            Price = parseFloat(Price).toFixed(2); //商品单价
            SumTotal = parseFloat(parseFloat(Price) * parseFloat(Num)).toFixed(2); //商品小计
            var SumTotal2 = $.trim($(".lblTotal_" + index).text()).replace(/,/gm, ''); //原始商品小计
            SumTotal2 = SumTotal2.substring(1);
            var SumPrice = parseFloat(parseFloat($.trim($("#lblTotalAmount").text()).replace(/,/gm, '')) + parseFloat(SumTotal) - parseFloat(SumTotal2)).toFixed(2);
            SumPrice = (SumPrice == "0.00" ? SumTotal : SumPrice);
            //商品总价
            // var SumPrice = $.trim($("#lblTotalAmount").text()) == "0.00" ? SumTotals : $.trim($("#lblTotalAmount").text()).replace(/,/gm, '');
            $.ajax({
                type: "post",
                url: "../../Handler/orderHandle.ashx",
                data: { ck: Math.random(), ActionType: "GoodsInfo", disId: disId, compId: CompId, SumTotal: SumPrice },
                async: false,
                success: function (data, status) {
                    var result = eval('(' + data + ')');
                    var ds = result["ds"];
                    if (ds == "True") {
                        $(".txtGoodsNum" + index).val(Num); //商品购买数量
                        //商品小计计算
                        $(".lblTotal_" + index).text("￥" + formatMoney(SumTotal, 2));
                        //商品总价计算
                        var trnum = $(".tabLine tbody tr").length;
                        var pricestr = "0";
                        for (var i = 0; i < trnum; i++) {
                            if ($(".tabLine tbody tr").eq(i).attr("tip") != undefined) {
                                var z = $(".tabLine tbody tr").eq(i).attr("trindex2");
                                pricestr = parseFloat(pricestr) + parseFloat($.trim($(".tabLine tbody tr").eq(i).find(".lblTotal_" + z).html()).replace(/,/gm, '').substring(1));
                            }
                        }
                        var ZJSumTotal = formatMoney(parseFloat(pricestr).toFixed(2), 2);
                        $("#lblTotalAmount").text(ZJSumTotal);  //商品总价
                        var psf = $.trim($("#lblPostFee").text().replace(/,/gm, '')); //运费
                        if (result["proPrice"].toString() != "") {
                            //有促销优惠
                            $("#lblCux").text(formatMoney(result["proPrice"].replace(/,/gm, ''), 2));
                            //商品总价
                            $("#lblYFPrice").text(formatMoney(parseFloat(pricestr) - parseFloat(result["proPrice"].replace(/,/gm, '')) - parseFloat(batePrice.replace(/,/gm, '')) + parseFloat(psf), 2));
                        }
                        else {
                            //无促销优惠
                            $("#lblCux").text(parseFloat(0).toFixed(2));
                            //商品总价
                            $("#lblYFPrice").text(formatMoney(parseFloat(pricestr) - parseFloat(batePrice.replace(/,/gm, '')) + parseFloat(psf), 2));
                        }
                    } else {
                        $(".lblTotal_" + index).text(parseFloat(0).toFixed(2));
                    }
                },
//                complete: function () {
//                   $(".txtRebate").trigger("blur");
//                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("服务器异常，请稍后再试", IconOption.错误);
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-1" />
    <input id="hid_Alert" type="hidden" />
    <input id="hidType" type="hidden" runat="server" />
    <!-- 判断是否再次购买-->
    <input id="hidType2" type="hidden" runat="server" />
    <input type="hidden" id="hidKeyId" runat="server" />
    <input type="hidden" id="hidDisID" runat="server" />
    <input type="hidden" id="hidCompId" runat="server" />
    <!-- 联系人-->
    <input type="hidden" id="hidAddName" runat="server" />
    <!-- 联系人手机-->
    <input type="hidden" id="hidAddPhone" runat="server" />
    <!-- 发货地址-->
    <input type="hidden" id="hidAdder" runat="server" />
    <!--   判断开的什么发票 0 不开 1 普通 2 增值税-->
    <input type="hidden" id="hidVal" runat="server" value="0" />
    <!-- 抬头-->
    <input type="hidden" id="hidLookUp" runat="server" />
    <!-- 发票内容-->
    <input type="hidden" id="hidContext" runat="server" />
    <!-- 开户银行-->
    <input type="hidden" id="hidBank" runat="server" />
    <!--开户帐号-->
    <input type="hidden" id="hidAccount" runat="server" />
    <!-- 登记号-->
    <input type="hidden" id="hidRegNo" runat="server" />
    <!--配送方式-->
    <input type="hidden" id="hidPsType" runat="server" value="送货" />
    <!--商品id-->
    <input type="hidden" id="hidGoodsInfoId" runat="server" value="" />
    <!--运费-->
    <input type="hidden" id="hidPostFree" runat="server" value="0.00" />
    <!--时间戳-->
    <input type="hidden" id="hidts" runat="server" value="" />
    <!-- 返利-->
    <input type="hidden" id="hidFanl" runat="server" value="" />
    <input type="hidden" id="hidUtype" runat="server" value="2" />
    <div class="rightCon">
        <div class="info">
            <a href="../jsc.aspx">我的桌面</a>><a href="javascript:;">新建订单</a></div>
        <!--[if !IE]>商品展示区 start<![endif]-->
        <div class="goods-zs">
            <!--[if !IE]>选择代理商 start<![endif]-->
            <div class="jxs-box left">
                <div class="bt">
                    代理商：</div>
                <div class="s left">
                    <div class="boxBig">
                        <input name="" type="text" id="txtDisName" runat="server" autocomplete="off" placeholder="请先选择代理商"
                            class="box txtDisName" /><a class="opt-i"></a></div>
                    <div class="search-opt none">
                        <ul class="name">
                            <asp:Repeater ID="rptDisList" runat="server">
                                <ItemTemplate>
                                    <li><a tip="<%#Eval("id") %>" href="javascript:;">
                                        <%#Eval("disname") %></a></li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                        <div class="opt">
                            <a href="javascript:;"><i class="opt2-i"></i>选择代理商</a></div>
                    </div>
                </div><span runat="server" id="Msg"  style="margin-left:30px;margin-top:10px;color:red;"></span>
            </div>
            <!--[if !IE]>选择代理商 end<![endif]-->
            <!--商品 start -->
            <div class="tabLine">
                <table border="0" cellspacing="0" cellpadding="0">
                    <thead>
                        <tr>
                            <th class="t5">
                            </th>
                            <th class="">
                                商品名称
                            </th>
                            <th class="t5">
                                单位
                            </th>
                            <th class="t3">
                                价格
                            </th>
                            <th class="t3" style='<%= IsInve==0?"": "display:none"  %>'>
                                库存
                            </th>
                            <th style="width: 15%">
                                数量
                            </th>
                            <th class="t3">
                                小计
                            </th>
                            <th class="t3">
                                备注
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td class="t8">
                                <div class="addg">
                                    <a href="javascript:;" class="minus2"></a><a href="javascript:;" class="add2" tip="alast">
                                    </a>
                                </div>
                            </td>
                            <td>
                                <div class="search">
                                    <input name="" type="text" class="box project2" /><a class="opt-i"></a>
                                    <!--[if !IE]>搜索弹窗 start<![endif]-->
                                    <div class="search-opt none">
                                        <ul class="list">
                                        </ul>
                                        <div class="opt">
                                            <a href="javascript:;"><i class="opt2-i"></i>选择商品</a></div>
                                    </div>
                                    <!--[if !IE]>搜索弹窗 end<![endif]-->
                                </div>
                            </td>
                            <td style='<%= IsInve==0?"": "display:none"  %>'>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <!--商品 end -->
            <!--[if !IE]>订单金额 start<![endif]-->
            <div class="options-box price-box">
                <div class="right">
                    <table>
                        <tr>
                            <td class="li">
                                商品总额：
                            </td>
                            <td>
                                ¥<label id="lblTotalAmount" runat="server">0.00</label>
                            </td>
                        </tr>
                        <tr>
                            <td class="li">
                                促销优惠：
                            </td>
                            <td>
                                ¥<label id="lblCux" runat="server">0.00</label>
                            </td>
                        </tr>
                        <tr class="isbate">
                            <td class="li">
                                返利抵扣：
                            </td>
                            <td>
                                ¥<label id="lblFanl" runat="server">0.00</label>
                            </td>
                        </tr>
                        <tr>
                            <td class="li">
                                运费：
                            </td>
                            <td>
                                <a href="javascript:;" class="edit-i postfee"></a>¥<label id="lblPostFee" runat="server">0.00</label>
                            </td>
                        </tr>
                        <tr>
                            <td class="li">
                                应付总额：
                            </td>
                            <td>
                                <div class="price-sum li">
                                    <i class="price">￥<label id="lblYFPrice" runat="server">0.00</label></i></div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="rebate isbate">
                    <i class="bt2 left">使用返利：</i>
                    <div class="edit-ok left ">
                        <input name="txtRebate" runat="server" onblur="bate()" onfocus="InputFocus(this)"
                            onkeyup="KeyInt2(this)" maxlength="9" type="text" class="box txtRebate" id="txtRebate"
                            value="0.00" autocomplete="off" /><i class="txt">可用返利￥ 0.00<i class="sus-i seebate"></i></i></div>
                    <i class="sum ok none">￥0.00<i class="sus-i"></i></i>
                </div>
            </div>
            <!--[if !IE]>订单金额 end<![endif]-->
        </div>
        <!--[if !IE]>商品展示区end<![endif]-->
        <div class="blank20">
        </div>
        <!--[if !IE]>下单信息 start<![endif]-->
        <div class="goods-info">
            <div class="bh">
                <div class="left deli">
                    <i class="bt2">交货日期：</i>
                    <input type="text" class="Wdate" id="txtDate" runat="server" readonly="readonly"
                        onclick="WdatePicker({minDate:'%y-%M-%d'})" />
                </div>
                <div class="left carry">
                    <i class="bt2">配送方式：</i><div class="ca-box left">
                        <i class="dx">
                            <label id="lblPsType" runat="server">
                                送货</label><i class="arrow"></i></i><div class="menu">
                                    <a href="JavaScript:;">
                                        <label id="lblPsType2" runat="server">
                                            自提</label></a></div>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="bz remark">
                <i class="bt">订单备注：</i><div class="txt_box">
                    <textarea id="OrderNote" runat="server" name="OrderNote" maxlength="200" class="box"
                        placeholder="订单备注不能超过200个字"></textarea></div>
            </div>
            <div class="bz site">
                <i class="bt">收货信息：</i><div class="txtbox">
                    <a href="javascript:;" class="edit-i"></a><i class="site-if">&nbsp;</i>
                    <asp:HiddenField ID="hrAdder" runat="server" />
                </div>
            </div>
            <div class="bz invoice">
                <i class="bt">开票信息：</i><div class="txtbox">
                    <a href="javascript:;" class="edit-i"></a><i class="in-if">不开发票</i>
                    <asp:HiddenField ID="hrOrderInv" runat="server" />
                </div>
            </div>
            <div class="bz attach">
                <i class="bt">附件：</i>
                <ul class="list">
                </ul>
                <div class="add">
                    <a href="javascript:;" class="a-upload bule">
                        <input type="file" name="AddBanner" id="AddBanner" class="AddBanner" onchange="uploadAvatar(this,'<%= Common.GetWebConfigKey("ImgViewPath") %>','');" />+新增附件</a><i
                            class="txt">（附件最大20M，支持格式：PDF、Word、Excel、Txt、JPG、PNG、BMP、GIF、RAR、ZIP）</i>
                    <asp:HiddenField ID="hrOrderFj" runat="server" />
                </div>
            </div>
        </div>
        <!--[if !IE]>下单信息 end<![endif]-->
        <div class="blank20">
        </div>
        <div class="btn-box">
            <div class="btn">
                <a href="javascript:;" class="btn-area" tip="1" >提交</a>
                <a href="javascript:;" class="gray-btn">取消</a></div>
            <div class="bg">
            </div>
        </div>
    </div>
    <div id="divGoodsName" class="divGoodsName" runat="server" style="display: none">
    </div>
    <div id="divDisList" class="divDisList" runat="server" style="display: none">
    </div>
    <div class="po-bg2 none" style="z-index: 999999;">
    </div>
    <div id="p-delete" class="popup2 p-delete2 none" style="z-index: 9999999">
        <img src="../../js/layer/skin/default/loading-0.gif" />
    </div>
    </form>
</body>
</html>

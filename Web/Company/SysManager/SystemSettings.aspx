<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SystemSettings.aspx.cs" Inherits="Company_SysManager_SystemSettings" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv = "X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>系统设置</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js"></script>
    <script>
        $(document).ready(function () {
            $_def.ID = "btnAdd";

            $('.tb tbody tr td').each(function (index, obj) {
                if ($(obj).find("span").length > 0) {
                    $(obj).addClass('odd');
                }
            });

            $(".txtgoods").attr("placeholder", "添加新的标签");
            $(".txtgoodsC").attr("placeholder", "自定义字段名称");

            //维护物流信息
            $("._logistics").click(function () {
                var url = 'ComLogisticsAdd.aspx';          //转向网页的地址; 
                var name = '维护物流';                     //网页名称，可为空; 
                var iWidth = 980;                          //弹出窗口的宽度; 
                var iHeight = 600;                         //弹出窗口的高度; 
                //获得窗口的垂直位置 
                var iTop = (window.screen.availHeight - 30 - iHeight) / 2;
                //获得窗口的水平位置 
                var iLeft = (window.screen.availWidth - 10 - iWidth) / 2;
                //var index = layerCommon.openWindow(name, url, iWidth + "px", iHeight + "px");
                window.open(url, name, 'height=' + iHeight + ', width=' + iWidth + ',top=' + iTop + ',left=' + iLeft + ',toolbar=no,menubar=no,location=no,resizable=no,titlebar=no,scrollbars=yes');
                //$("#hid_Alert").val(index); //记录弹出对象
            });
            //订单完成节点设置
            $(".quan1").click(function () {
                var typeid = $(this).parent().attr("tip"); //完成节点的标识
                if (typeid == 2) {
                    if ($.trim($(this).find(".quan4").text()) == "√") {
                        $(".quan4").text("");
                    } else {
                        $(this).find(".quan4").text("√");
                        $(this).parent().nextAll().find(".quan4").text("");
                    }
                } else if (typeid == 3) {
                    if ($.trim($(this).find(".quan4").text()) == "√") {
                        $(this).find(".quan4").text("");
                        if ($.trim($(this).parent().prev().find(".quan4").text()) == "") {
                            $(this).parent().prev().find(".quan4").text("");
                        }
                        $(this).parent().next().find(".quan4").text("");
                    } else {
                        $(this).find(".quan4").text("√");
                        $(this).parent().prev().find(".quan4").text("√");
                        $(this).parent().next().find(".quan4").text("");
                    }
                } else if (typeid == 0) {
                    if ($.trim($(this).find(".quan4").text()) == "√") {
                        $(this).find(".quan4").text("");
                    } else {
                        $(".quan4").text("√");
                    }
                }
            })
            //保存
            $("#btnAdd").click(function () {

                if (!formCheck()) {
                    return;
                }
                var json = "{";
                $(".tb  tbody tr[class!=\"szhi\"]").each(function (index, obj) {
                    var val = "";
                    var key = $.trim($(obj).find("td").find("span").text().replace("*", ""));

                    if ($(obj).find("td").find("input").attr("type") == "text") {
                        if (key.toString() == "商品标签管理") {
                            $(obj).find("td").find("input").each(function (i, n) {
                                if ($(n).val() != "") {
                                    val += $(n).val() + ",";
                                }
                            });
                            val = val.substring(0, val.length - 1);
                        } else if (key.toString() == "商品自定义字段") {
                            $(obj).find("td").find("input").each(function (i, n) {
                                if ($(n).val() != "") {
                                    val += $(n).val() + ",";
                                }
                            });
                            val = val.substring(0, val.length - 1);
                        }
                        else
                            val = $(obj).find("td").find("input").val();
                    }
                    else if ($(obj).find("td").find("input").attr("type") == "radio") {
                        val = $(obj).find("td").find('input:radio[class="rdo"]:checked').val();
                    }
                    else if ($(obj).find("td").find("div").attr("class") == "ddlist") {
                        if ($.trim($(obj).find("td").find("div>li").eq(4).find(".quan4").text()) == "√") {
                            val = 0; //默认选中收货
                        } else if ($.trim($(obj).find("td").find("div>li").eq(3).find(".quan4").text()) == "√") {
                            val = 3; //选中发货
                        } else if ($.trim($(obj).find("td").find("div>li").eq(2).find(".quan4").text()) == "√") {
                            val = 2; //选中审核
                        } else {
                            val = 1; //都不选
                        }
                    } else {
                        return true; //其他的控件
                    }
                    json += ",\"" + index + "\":{\"key\":\"" + key + "\",\"val\":\"" + val + "\"}";
                });
                json += "}";
                json = json.replace("{,\"0\"", "{\"0\"");
                $.ajax({
                    type: 'post',
                    url: 'SystemSettings.aspx?action=sett',
                    data: { json: json },
                    async: false, //true:同步 false:异步
                    success: function (result) {
                        var data = eval('(' + result + ')');
                        //if (data["ds"].toString() = "0") 

                        setInterval(function () {
                            location.reload();
                        }, 3000);
                        layerCommon.alert(data["prompt"].toString(), IconOption.不显示图标);
                        return false;
                    }
                });

                setInterval(function () {
                    location.reload();
                }, 1000);
            });
        });

        //返回维护的物流信息
        function ComLogisticsAdd(json) {
            //关闭弹出层
            //CloseDialog();

            var obj = eval("(" + json + ")");
            var str = "";
            //清空
            $("#divlogistics").empty();
            for (var js2 in obj) {
                //if ($("#divlogistics i[title=\"" + obj[js2] + "\"]").text() != obj[js2])
                str += "<i style=\"margin-left:10px;  font-size: 13px; \" title=\"" + obj[js2] + "\">" + obj[js2] + "</i>";
            }
            $("#divlogistics").append(str);
        }

        function formCheck() {
            var str = "";

            //var bolgoods = "";
            //$('.txtgoods').each(function (index, obj) {
            //if ($(this).val() != "") {
            //    bolgoods = "True";
            //}
            //});
            //if (bolgoods != "True")
            //str += "- 商品标签不能全部为空。\r\n";

            if ($.trim($("#txtSinceSign").val()) == "") {
                str += "- 订单自动签收不能为空。\r\n";
            }
            if ($.trim($("#txtSinceOff").val()) == "") {
                str += "- 超时未付款自动作废订单不能为空。\r\n";
            }
            $('.txtgoodsC').each(function (index, obj) {
                if ($(obj).val() != "添加新的标签") {
                    $(this).val(stripscript($(obj).val()))
                }
            });
            $('.txtgoods').each(function (index, obj) {
                if ($(obj).val() != "自定义字段名称") {
                    $(this).val(stripscript($(obj).val()))
                }
            });
            if (str.toString() != "") {
                layerCommon.msg(str, IconOption.错误, 2500);
                return false;
            }
            else
                return true;
        }

        function binddata(json) {
            var result = eval("(" + json + ")");
            $.each(result, function (i, item) {
                $(".tb  tbody tr[class!=\"szhi\"]").each(function (index, obj) {
                    var key = $.trim($(obj).find("td").find("span").text().replace("*", ""));

                    if (item["Name"].toString() == key) {
                        if ($(obj).find("td").find("input").attr("type") == "text") {
                            if (key == "商品标签管理")
                                GoodsL(item["Value"].toString());
                            else if (key == "商品自定义字段")
                                GoodsC(item["Value"].toString());
                            else
                                $(obj).find("td").find("input").val(item["Value"].toString());
                        }
                        else if ($(obj).find("td").find("input").attr("type") == "radio") {
                            $('input[class="rdo"][value=' + item["Value"].toString() + ']').attr("checked", true);
                        }
                    }
                });
            });
        }

        //绑定自定义字段
        function GoodsC(c) {
            var goods = c.split(',');
            $('.txtgoodsC').each(function (index, obj) {
                if (goods.length >= index)
                    $(this).val(goods[index]);
            });
        }

        //绑定商品标签
        function GoodsL(lbl) {
            var goods = lbl.split(',');
            $('.txtgoods').each(function (index, obj) {
                if (goods.length >= index)
                    $(this).val(goods[index]);
            });
        }
        function Layerclose() {
            layerCommon.layerClose($("#hid_Alert").val());
        } 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-6" />
    <div class="rightinfo">
        <input type="hidden" id="hid_Alert" />
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面</a></li>
                <li>></li>
                <li><a href="../SysManager/SystemSettings.aspx" runat="server" id="atitle">系统设置</a></li>
            </ul>
        </div>
        <div class="tools">
            <ul class="toolbar left">
                <%--<li id="libtnEdit"><span><img src="../images/t02.png" /></span>编辑</li>--%>
            </ul>
        </div>
        <div class="div_content">
            <table class="tb">
                <tbody>
                    <tr class="szhi" style="display:none;">
                        <td align="right">
                            <i style="color: #333;">代理商设置</i>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td style="width: 200px;" align="right">
                            <span><i class="required">*</i>代理商加盟是否需要审核</span>
                        </td>
                        <td>
                            <input type="radio" id="rdodisOk" style="margin-left: 10px;" class="rdo" name="rdodis"
                                value="0" runat="server" />
                            需要审核
                            <input type="radio" id="rdodisNo" style="margin-left: 10px;" class="rdo" name="rdodis"
                                value="1" runat="server" />
                            不需要审核
                        </td>
                    </tr>
                    <tr class="szhi" style="display:none;">
                        <td align="right">
                            <i style="color: #333;">代理商支付设置</i>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td style="width: 200px;" align="right">
                            <span><i class="required">*</i>支付方式</span>
                        </td>
                        <td>
                            <input type="radio" id="payDBOk" style="margin-left: 10px;" class="rdo" name="rdopay"
                                value="0" runat="server" />
                            普通支付
                            <input type="radio" id="payDBNo" style="margin-left: 10px;" class="rdo" name="rdopay"
                                value="1" runat="server" />
                            担保支付
                        </td>
                    </tr>
                    <tr class="szhi">
                        <td align="right">
                            <i style="color: #333;">订单设置</i>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td style="width: 200px;" align="right">
                            <span>订单完成节点设置</span>
                        </td>
                        <td>
                            <div class="ddlist">
                                <li class="line"></li>
                                <li class="li"><i class="quan1"><i class="quan2"></i><i class="quan4">√</i></i><i
                                    ckass="name">订单提交</i></li>
                                <li class="li cur" tip="2"><i class="quan1"><i class="quan2"></i><i class="quan4">√</i></i><i
                                    ckass="name">订单审核</i></li>
                                <li class="li cur" tip="3"><i class="quan1"><i class="quan2"></i><i class="quan4">√</i></i><i
                                    ckass="name">订单发货</i></li>
                                <li class="li cur" tip="0"><i class="quan1"><i class="quan2"></i><i class="quan4">√</i></i><i
                                    ckass="name">订单收货</i></li>
                                <li class="li"><i class="quan3"></i><i ckass="name">完成</i></li>
                            </div>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td style="width: 200px;" align="right">
                            <span><i class="required">*</i>订单自动签收</span>
                        </td>
                        <td>
                            <input type="text" id="txtSinceSign" onkeyup="KeyInt(this);" onblur="KeyInt(this);"
                                class="textBox" maxlength="4" runat="server" />
                            <i id="ComSinceSign" class="grayTxt">天（默认15天）</i> <i class="grayTxt" style="padding-left: 10px;">
                                您的订单超出了订单签收期限，系统自动签收订单</i>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <%--edit by hgh  060909  暂时屏蔽--%>
                        <td style="width: 200px;" align="right">
                            <span style="width: 150px;"><i class="required">*</i>超时未付款自动作废订单</span>
                        </td>
                        <td>
                            <input type="text" id="txtSinceOff" onkeyup="KeyInt(this);" onblur="KeyInt(this);"
                                class="textBox" maxlength="4" runat="server" />
                            <i id="ComSinceOff" class="grayTxt">天（默认30天）</i> <i class="grayTxt" style="padding-left: 10px;">
                                您的订单超出了订单超时未付款保留期限，系统自动作废订单</i>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px;" align="right">
                            <span style="width: 150px;"><i class="required">*</i>代客下单是否需要审核</span>
                        </td>
                        <td>
                            <input type="radio" id="rdoOrderAddOK" style="margin-left: 10px;" class="rdo" name="rdoOrderAdd"
                                value="1" runat="server" />
                            是
                            <input type="radio" id="rdoOrderAddNO" style="margin-left: 10px;" class="rdo" name="rdoOrderAdd"
                                value="0" runat="server" />
                            否 <i id="IOrderAdd" class="grayTxt">（默认否）</i> <i class="grayTxt" style="padding-left: 30px;">
                                代客下单后，订单不需要审核</i>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px;" align="right">
                            <span style="width: 150px;"><i class="required">*</i>订单下单数量是否取整</span>
                        </td>
                        <td>
                            <input type="radio" id="rdoDigits0" style="margin-left: 10px;" class="rdo" name="rdoDigits"
                                value="0" runat="server" />
                            取整
                            <input type="radio" id="rdoDigits2" style="margin-left: 10px;" class="rdo" name="rdoDigits"
                                value="0.00" runat="server" />
                            保留2位小数 <i class="grayTxt" style="padding-left: 25px;">（默认取整，最多显示2位小数）</i>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td style="width: 200px;" align="right">
                            <span style="width: 150px;"><i class="required">*</i>订单支付返利是否启用</span>
                        </td>
                        <td>
                            <input type="radio" id="Rebate1" style="margin-left: 10px;" class="rdo" name="rdoRebate"
                                value="0" runat="server" />
                            不启用
                            <input type="radio" id="Rebate2" style="margin-left: 10px;" class="rdo" name="rdoRebate"
                                value="1" runat="server" />
                            启用 <i class="grayTxt" style="padding-left: 25px;">（默认不启用）</i>
                        </td>
                    </tr>
                    <tr class="szhi">
                        <td align="right">
                            <i style="color: #333;">商品设置</i>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr style="display:none;" >
                        <td style="width: 200px;" align="right">
                            <span style="width: 150px;"><i class="required">*</i>商品是否启用库存</span>
                        </td>
                        <td>
                            <input type="radio" id="rdoStockOk" style="margin-left: 10px;" class="rdo" name="rdoStock"
                                value="0" runat="server" />
                            是
                            <input type="radio" id="rdoStockNo" style="margin-left: 10px;" class="rdo" name="rdoStock"
                                value="1" runat="server" />
                            否 <i id="I1" class="grayTxt">（默认是）</i>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px;" align="right">
                            <span style="width: 150px;"><i class="required">*</i>商品分类选择是否折叠</span>
                        </td>
                        <td>
                            <input type="radio" id="rdoFoldingOk" style="margin-left: 10px;" class="rdo" name="rdoFolding"
                                value="1" runat="server" />
                            是
                            <input type="radio" id="rdoFoldingNo" style="margin-left: 10px;" class="rdo" name="rdoFolding"
                                value="0" runat="server" />
                            否 <i id="Folding" class="grayTxt">（默认是）</i>
                        </td>
                    </tr>
                      <tr>
                        <td style="width: 200px;" align="right">
                            <span style="width: 150px;"><i class="required">*</i>是否店铺开放价格</span>
                        </td>
                        <td>
                            <input type="radio" id="kfmoney1" style="margin-left: 10px;" class="rdo" name="kfmoney"
                                value="1" runat="server" />
                            是
                            <input type="radio" id="kfmoney0" style="margin-left: 10px;" class="rdo" name="kfmoney"
                                value="0" runat="server" />
                            否 <i id="Foldings" class="grayTxt">（默认否）</i>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px;" align="right">
                            <span style="width: 150px;">商品标签管理</span>
                        </td>
                        <td>
                            <input type="text" id="txtGoodsLable1" style="width: 100px; font-size: 13px;" class="textBox txtgoods"
                                maxlength="20" runat="server" />
                            <input type="text" id="txtGoodsLable2" style="width: 100px; font-size: 13px;" class="textBox txtgoods"
                                maxlength="20" runat="server" />
                            <input type="text" id="txtGoodsLable3" style="width: 100px; font-size: 13px;" class="textBox txtgoods"
                                maxlength="20" runat="server" />
                            <input type="text" id="txtGoodsLable4" style="width: 100px; font-size: 13px;" class="textBox txtgoods"
                                maxlength="20" runat="server" />
                            <input type="text" id="txtGoodsLable5" style="width: 100px; font-size: 13px;" class="textBox txtgoods"
                                maxlength="20" runat="server" />
                            <i class="grayTxt">（管理您的商品标签，最多支持五个标签）</i>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px;" align="right">
                            <span style="width: 150px;">商品自定义字段</span>
                        </td>
                        <td>
                            <input type="text" id="txtGoodsC1" style="width: 100px; font-size: 13px;" class="textBox txtgoodsC"
                                maxlength="20" runat="server" />
                            <input type="text" id="txtGoodsC2" style="width: 100px; font-size: 13px;" class="textBox txtgoodsC"
                                maxlength="20" runat="server" />
                            <input type="text" id="txtGoodsC3" style="width: 100px; font-size: 13px;" class="textBox txtgoodsC"
                                maxlength="20" runat="server" />
                            <input type="text" id="txtGoodsC4" style="width: 100px; font-size: 13px;" class="textBox txtgoodsC"
                                maxlength="20" runat="server" />
                            <input type="text" id="txtGoodsC5" style="width: 100px; font-size: 13px;" class="textBox txtgoodsC"
                                maxlength="20" runat="server" />
                            <i class="grayTxt">（管理您的商品自定义字段，最多支持五个字段）</i>
                        </td>
                    </tr>
                    <%--<tr class="szhi">
                        <td align="right">
                            <i style="color: #333;">计息设置</i>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px;" align="right">
                            <span style="width: 150px;">是否计息</span>
                        </td>
                        <td>
                            <input type="radio" id="rdoDrawYes" style="margin-left: 10px;" class="rdo" name="rdoDraw"
                                value="1" runat="server" />
                            是
                            <input type="radio" id="rdoDrawNo" style="margin-left: 10px;" checked="true" class="rdo"
                                name="rdoDraw" value="0" runat="server" />
                            否 <i id="IDraw" class="grayTxt">（默认否）</i>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px;" align="right">
                            <span>利息率设置</span>
                        </td>
                        <td>
                            <input type="text" id="txtInterest" onkeyup="KeyInt2(this);" onblur="KeyInt2(this);"
                                class="textBox" maxlength="10" runat="server" />
                            <i id="IInterest " class="grayTxt">利息率（0--100%）</i>
                        </td>
                    </tr>--%>
                    <tr class="szhi">
                        <td align="right">
                            <i style="color: #333;">物流设置</i>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px;" align="right">
                            <span style="width: 150px;">常用物流公司</span>
                        </td>
                        <td>
                            <div id="divlogistics" runat="server" style="display: inline-block; min-width: 170px;">
                            </div>
                            <a style="color: #1a8fc2" href="javascript:;" class="_logistics">&nbsp; 维护物流公司</a>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="div_footer">
            <input type="button" id="btnAdd" class="orangeBtn" runat="server" value="确定" />
            &nbsp;
            <%--<input name="" type="button" class="cancel" onclick="javascript:history.go(-1);" value="取消">--%>
        </div>
    </div>
    </form>
</body>
</html>

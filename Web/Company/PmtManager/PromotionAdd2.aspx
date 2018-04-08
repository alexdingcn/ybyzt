<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PromotionAdd2.aspx.cs" Inherits="Company_PmtManager_PromotionAdd2" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%if (KeyID == 0)
                      { %>新增促销<%}
                      else
                      {%>编辑促销<%}%></title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <style type="text/css">
        input[type='text']
        {
            width: 150px;
        }
        .send
        {
            border-bottom: 1px solid #d6dee3;
            outline: 0;
            border: 0;
            height: 14px;
            line-height: 18px;
            border-bottom: 1px solid #d6dee3;
            color: #555;
            padding: 2px 12px;
            font-size: 12px;
        }
        .theme-color, a.theme-color:hover
        {
            color: #1596ad !important;
        }
        .ml20
        {
            margin-left: 20px !important;
        }
    </style>
    <script>
        $(function () {
            $_def.ID = "btnSave";

            if ($('input[name="IsEnabled2"]:checked').val() == "1") {
                $(".addItem").show();
            } else {
                $(".addItem").hide();
            }
            if ($('input[name="promotionType"]:checked').val() == "5") {
                $(".SendFull").css("display", "inline-block");
                $(".Discount").css("display", "none");
            } else {
                $(".SendFull").css("display", "none");
                $(".Discount").css("display", "inline-block");
            }
            //促销方式
            $('input[name="promotionType"]').click(function () {
                if ($.trim($(this).val()) == "5") {
                    //满减
                    $(".SendFull").css("display", "inline-block");
                    $(".Discount").css("display", "none");
                    $(".SendFull label:not(:first)").remove();
                    $(".SendFull br").remove();
                } else if ($.trim($(this).val()) == "6") {
                    //满折
                    $(".SendFull").css("display", "none");
                    $(".Discount").css("display", "inline-block");
                    $(".Discount label:not(:first)").remove();
                    $(".Discount br").remove();
                }
            });
            //返回
            $(".cancel").click(function () {
                window.location.href = 'PromotionList.aspx?type=<%=Request["Type"] %>';
            });
            //是否启用阶梯
            $('input[name="IsEnabled2"]').click(function () {
                if ($.trim($(this).val()) == "1") {
                    $(".addItem").show();
                } else {
                    $(".addItem").hide();
                    if ($.trim($('input[name="promotionType"]:checked').val()) == "5") {
                        //满减
                        $(".SendFull label:not(:first)").remove();
                        $(".SendFull br").remove();
                    } else if ($.trim($('input[name="promotionType"]:checked').val()) == "6") {
                        //满折
                        $(".Discount label:not(:first)").remove();
                        $(".Discount br").remove();
                    }
                }
            })
            //添加区间
            $(".addItem").click(function () {
                var type = $('input[name="promotionType"]:checked').val();
                if (type == "5") {
                    var count = $(".SendFull label").length;
                    count++;
                    var html = "<br /><label>订单金额满￥<input type=\"text\" onkeyup='KeyInt2(this)'  id=\"txtPrice" + count + "\" class=\"send txtPrice\"  style=\"width: 50px;\" name=\"txtPrice\"/>，立减￥<input type=\"text\"  id=\"txtSendFull" + count + "\" onkeyup='KeyInt2(this)' class=\"send txtSendFull\" style=\"width: 50px;\" name=\"txtSendFull\"/><a class=\"theme-color ml20 deleteItem\" href=\"javascript:;\">删除</a></label>";
                    $(".SendFull").append(html);
                } else if (type == "6") {
                    var count = $(".Discount label").length;
                    count++;
                    var html = "<br /><label>订单金额满￥<input type=\"text\" onkeyup='KeyInt2(this)'  id=\"txtPrices" + count + "\" class=\"send txtPrices\" style=\"width: 50px;\" name=\"txtPrices\"/>，打折（<input type=\"text\"  id=\"txtDiscount" + count + "\" onkeyup='KeyInt2(this)' class=\"send txtDiscount\" style=\"width: 20px;\" name=\"txtDiscount\"/>）%<a class=\"theme-color ml20 deleteItem\" href=\"javascript:;\">删除</a></label>";
                    $(".Discount").append(html);
                }

            })
            //删除当前行
            $(document).on("click", ".deleteItem", function () {
                $(this).parent().prev("br").remove();
                $(this).parent().remove();
            })

        });
        //验证
        function formCheck() {
            var str = "";
            if ($.trim($("#txtPromotionDate").val()) == "") {
                str += "--促销开始日期不能为空。</br>";
            }

            if ($.trim($("#txtPromotionDate1").val()) == "") {
                str += "--促销结束日期不能为空。</br>";
            }

            var protypeval = $('input[name="promotionType"]:checked').val();
            if (protypeval.toString() == "6") {
                $(".txtPrices").each(function (index, ibj) {
                    if ($(this).val() == "") {
                        str += "--满减的金额不能为空。</br>";
                        return false;
                    }
                })
                $(".txtDiscount").each(function (index, ibj) {
                    if ($(this).val() == "") {
                        str += "--打折不能为空。</br>";
                        return false;
                    } else {
                        if (parseFloat($(this).val()) < 0 || parseFloat($(this).val()) > 100) {
                            str += "--打折请输入0—100的数。</br>";
                            return false;
                        }
                    }
                })

            } else if (protypeval.toString() == "5") {
                $(".txtPrice").each(function (index, ibj) {
                    if ($(this).val() == "") {
                        str += "--满减的金额不能为空。</br>";
                        return false;
                    }
                })
                $(".txtSendFull").each(function (index, ibj) {
                    if ($(this).val() == "") {
                        str += "--立减的金额不能为空。</br>";
                        return false;
                    } else {
                        if (parseFloat($.trim($("#txtPrice" + parseInt(parseInt(index) + 1)).val())) <= parseFloat($.trim($(this).val()))) {
                            str += "--立减金额要小于订单金额。</br>";
                            return false;
                        }
                    }
                })
            }
            if (str != "") {
                layerCommon.msg(str, IconOption.错误, 2000);
                return false;
            } else {
                return true;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-3" />
    <input type="hidden" id="hid_Alert" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a id="protitle" runat="server"></a></li>
                <li>></li>
                <li><a href="javascript:void(0);"><%if (KeyID == 0)
                      { %>新增促销<%}
                      else
                      {%>编辑促销<%}%></a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <div class="div_content">
            <!--促销主体 start-->
            <div class="lbtb">
                <table class="dh">
                    <tr>
                        <td style="width: 140px;">
                            <span>
                                <label class="required">
                                    *</label>促销开始日期</span>
                        </td>
                        <td>
                            <input name="txtPromotionDate" runat="server" onclick="var endDate=$dp.$('txtPromotionDate1'); WdatePicker({onpicked:function(){endDate.focus();},maxDate:'#F{$dp.$D(\'txtPromotionDate1\')}'})"
                                id="txtPromotionDate" readonly="readonly" type="text" class="Wdate" value=""
                                style="width: 150px; margin-left: 5px;" /><label>促销开始时间是00:00:00</label>
                            <b class="hint">1</b>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 140px;">
                            <span>
                                <label class="required">
                                    *</label>促销结束日期</span>
                        </td>
                        <td>
                            <input name="txtPromotionDate1" runat="server" onclick="WdatePicker({minDate:'#F{$dp.$D(\'txtPromotionDate\')}'})"
                                id="txtPromotionDate1" readonly="readonly" type="text" class="Wdate" value=""
                                style="width: 150px; margin-left: 5px;" /><label>促销结束时间是23:59:59</label>
                            <b class="hint">2</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>是否发送促销信息</span>
                        </td>
                        <td>
                            <input style="margin-left: 5px;" type="radio" id="isOkComNews" name="ComNews" value="1"
                                checked="true" runat="server" />
                            <label for="isOkComNews">
                                是</label>
                            <input style="margin-left: 5px;" type="radio" id="isNoComNews" name="ComNews" value="0"
                                runat="server" />
                            <label for="isNoComNews">
                                否</label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 140px;">
                            <span>是否启用</span>
                        </td>
                        <td>
                            <input style="margin-left: 5px;" type="radio" name='IsEnabled' id="IsEnabled1" value='1'
                                checked="true" runat="server" /><label for='IsEnabled1'>启用</label>
                            <input style="margin-left: 5px;" type="radio" name='IsEnabled' id="IsEnabled0" value='0'
                                runat="server" /><label for='IsEnabled0'>停用</label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 140px;">
                            <span>是否启用阶梯促销</span>
                        </td>
                        <td>
                            <input style="margin-left: 5px;" type="radio" name='IsEnabled2' id="Radio1" value='1'
                                runat="server" /><label for='Radio1'>启用</label>
                            <input style="margin-left: 5px;" type="radio" name='IsEnabled2' id="Radio2" value='0'
                                checked="true" runat="server" /><label for='Radio2'>停用</label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 140px;">
                            <span>
                                <label class="required">
                                    *</label>促销方式</span>
                        </td>
                        <td>
                            <div class="GoodsSales" style="display: inline-block;">
                                <input style="margin-left: 5px;" type="radio" name='promotionType' id="promotionType3"
                                    checked="true" value='5' runat="server" /><label for='promotionType3'>满减</label>
                                <input style="margin-left: 5px;" type="radio" name='promotionType' id="promotionType4"
                                    value='6' runat="server" /><label for='promotionType4'>满折</label><label style="color: #AAAAAA;">（应付订单总额=订单金额X折扣）</label>
                            </div>
                            <b class="hint">3</b>
                        </td>
                    </tr>
                    <tr id="tr_pro">
                        <td style="background: #f6f6f6 none repeat scroll 0 0;">
                            <span>
                                <label class="required">
                                    *</label>促销条件</span>
                        </td>
                        <td>
                            <div class="SendFull" id="SendFull" runat="server" style="display: inline-block;">
                                <label>
                                    订单金额满￥<input type="text" onkeyup='KeyInt2(this)' id="txtPrice1" class="send txtPrice"
                                        style="width: 50px;" name="txtPrice" />，立减￥<input type="text" id="txtSendFull1" onkeyup='KeyInt2(this)'
                                            class="send txtSendFull" name="txtSendFull" style="width: 50px;" /></label>
                            </div>
                            <div class="Discount" id="Discount" runat="server" style="display: none;">
                                <label>
                                    订单金额满￥<input type="text" onkeyup='KeyInt2(this)' id="txtPrices1" class="send txtPrices"
                                        style="width: 50px;" name="txtPrices" />，打折（<input type="text" id="txtDiscount1"
                                            onkeyup='KeyInt2(this)' class="send txtDiscount" name="txtDiscount" style="width: 20px;" />）%</label>
                            </div>
                            <a class="theme-color addItem" id="addItem" runat="server" href="javascript:;">+添加区间</a>
                            <b class="hint">4</b>
                        </td>
                    </tr>
                    <tr>
                        <td style="background: #f6f6f6 none repeat scroll 0 0;">
                            <span>促销描述</span>
                        </td>
                        <td>
                            <textarea id="txtProInfos" maxlength="400" placeholder="订单备注不能大于400个字符" runat="server"
                                class='textarea'></textarea>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
            <!--促销主体 end-->
            <!--清除浮动-->
            <div style="clear: none;">
            </div>
            <!--促销商品明细 end-->
            <div class="footerBtn">
                <asp:Button ID="btnSave" runat="server" Text="确定" CssClass="orangeBtn" OnClick="btnSave_Click"
                    OnClientClick="return formCheck();" />&nbsp;
                <input name="" type="button" class="cancel" value="返回" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>

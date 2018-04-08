<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PromotionAdd.aspx.cs" Inherits="Company_PmtManager_PromotionAdd" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%if (KeyID == 0)
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
    <script type="text/javascript">
        TypeLoad(<%=KeyID %>);
        function TypeLoad(KeyId) {
            var type = '<%=Type %>';
            if (KeyId.toString() == "0") {//新增
                    //商品促销默认满送
                    $("input[value='3']").attr("checked", true);
                    //促销商品价格不能修改
                    $(".list td .textBox").attr("disabled", "false");
                    $(".SendFull").show();
                    $(".Discount").hide();
                
            } else {
                //修改
                //促销商品价格不能修改
                $(".list td .textBox").attr("disabled", "false");

                if ($('input[name="promotionType"]:checked').val() == "3") {
                    //满送
                    $(".SendFull").show();
                    $(".Discount").hide();
                } else {
                    //打折
                    $(".SendFull").hide();
                    $(".Discount").show();
                }
            }
        }

        function Protype(val) {
            //选中
            $('input[value="' + val + '"]').attr("checked", true);

            if (val.toString() == "3") {
                //促销商品价格不能修改
                $(".list td .textBox").attr("disabled", "false");
                //满送
                $(".SendFull").show();
                $(".Discount").hide();
            } else {
                //促销商品价格不能修改
                $(".list td .textBox").attr("disabled", "false");

                //打折
                $(".SendFull").hide();
                $(".Discount").show();
             }
        }

        $(function () {
            $_def.ID = "btnSave";

            $('input[name="promotionType"]').click(function () {
                if ($.trim($(this).val()) == "3") {
                    //满送
                    $(".SendFull").show();
                    $(".Discount").hide();
                } else if ($.trim($(this).val()) == "4") {
                    //打折
                    $(".SendFull").hide();
                    $(".Discount").show();
                }
            });

            //返回
            $(".cancel").click(function () {
                window.location.href = 'PromotionList.aspx?type=<%=Request["Type"] %>';
            });

            //选择促销商品
            $("#btnGoods").click(function () {
                if ($("#txtPromotiontitle").val() != "") {
                    $("#txtPromotiontitle").val(stripscript($("#txtPromotiontitle").val()))
                }
                if ($("#txtProInfos").val() != "订单备注不能大于400个字符") {
                    $("#txtProInfos").val(stripscript($("#txtProInfos").val()))
                }
                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度
                var index = layerCommon.openWindow('选购促销商品', 'SelectGoods.aspx?type=1&CompId=<%=this.CompID %>', '880px', '600px');
                $("#hid_Alert").val(index); //记录弹出对象
            });
        });

        //关闭选择商品区域
        function GbGoods() {
            layerCommon.layerClose($("#hid_Alert").val());
            $("#btnGoodsInfo").trigger("click");
        }

        function delGoods(Id) {
            $("#hiddelgoodsid").val(Id);
            layerCommon.confirm("确认删除吗？", function () { $("#btnDelGoods").trigger("click"); });
        }

        function onchengPrice(Id, q) {

            var Price = $(q).val();

            $.ajax({
                type: 'post',
                url: 'PromotionAdd.aspx?action=GoodsInfo',
                data: { goodsInfoId: Id, Price: Price },
                async: false,
                success: function (data, status) {

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("服务器异常，请稍后再试", IconOption.错误);
                }
            });

        }

        //验证
        function formCheck() {
            var str = "";
            if ($.trim($("#txtPromotiontitle").val()) == "") {
                str += "--促销标题不能为空。</br>";
            }

            if ($.trim($("#txtPromotionDate").val()) == "") {
                str += "--促销开始日期不能为空。</br>";
            }

            if ($.trim($("#txtPromotionDate1").val()) == "") {
                str += "--促销结束日期不能为空。</br>";
            }
            var protypeval = $('input[name="promotionType"]:checked').val();
            if (protypeval.toString() == "4") {
                var duscoutval = $("#txtDiscount").val();
                if (duscoutval != "") {
                    if (parseFloat(duscoutval) < 0 || parseFloat(duscoutval) > 100) {
                        str += "--打折请输入0—100的数。</br>";
                    }
                } else {
                    str += "--打折不能为空。\r\n";
                }
            } else if (protypeval.toString() == "3") {
                var disCount = $("#txtSendFull").val();
                var SendNum = $("#txtSendNum").val();
                if (disCount.toString() == "" || SendNum.toString() == "") {
                    if (disCount.toString() == "")
                        str += "--满送订购数量不能为空。</br>";
                    else
                        str += "--获赠商品数量不能为空。</br>";
                }
            }

            if (str != "") {
                layerCommon.msg(str, IconOption.错误,1000);
                return false;
            } else {
                return true;
            }
        }
    </script>
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-3" />
    <input type="hidden" id="hid_Alert" />
    <asp:Button ID="btnGoodsInfo" runat="server" OnClick="btnGoodsInfo_Click" Text="选择商品"
        Style="display: none" />
    <asp:Button ID="btnDelGoods" runat="server" Text="删除商品" Style="display: none;" OnClick="btnDelGoods_Click" />
    <input type="hidden" id='hiddelgoodsid' value='' runat="server" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a id="protitle" runat="server"></a></li>
                <li>></li>
                <li><a href="javascript:void(0);">
                    <%if (KeyID == 0)
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
                                    *</label>促销标题</span>
                        </td>
                        <td colspan="3">
                            <input type="text" id="txtPromotiontitle" style="width: 500px;" class="textBox" runat="server"
                                maxlength="50" />
                            <b class="hint">1</b>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 140px;">
                            <span>
                                <label class="required">
                                    *</label>促销开始日期</span>
                        </td>
                        <td colspan="3">
                            <input name="txtPromotionDate" runat="server" onclick="var endDate=$dp.$('txtPromotionDate1'); WdatePicker({onpicked:function(){endDate.focus();},maxDate:'#F{$dp.$D(\'txtPromotionDate1\')}'})"
                                id="txtPromotionDate" readonly="readonly" type="text" class="Wdate" value=""
                                style="width: 150px; margin-left: 5px;" /><label>促销开始时间是00:00:00</label>
                            <b class="hint">2</b>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 140px;">
                            <span>
                                <label class="required">
                                    *</label>促销结束日期</span>
                        </td>
                        <td colspan="3">
                            <input name="txtPromotionDate1" runat="server" onclick="WdatePicker({minDate:'#F{$dp.$D(\'txtPromotionDate\')}'})"
                                id="txtPromotionDate1" readonly="readonly" type="text" class="Wdate" value=""
                                style="width: 150px; margin-left: 5px;" /><label>促销结束时间是23:59:59</label>
                            <b class="hint">3</b>
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
                            <span>
                                <label class="required">
                                    *</label>促销方式 </span>
                        </td>
                        <td colspan="3">
                            <input style="margin-left: 5px;" type="radio" name='promotionType' id="promotionType3"
                                value='3' runat="server" /><label for='promotionType3'>满送</label>
                            <input style="margin-left: 5px;" type="radio" name='promotionType' id="promotionType4"
                                value='4' runat="server" /><label for='promotionType4'>打折</label>
                            <%--<div class="doubt left">
                                <i class="doubt-i"></i>
                                <div class="txt">
                                    <i class="trian-i"></i><i class="trian-i trian-i2"></i>
                                    <p>
                                        1、</p>
                                    <p>
                                        2、</p>
                                </div>
                            </div>--%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>促销条件</span>
                        </td>
                        <td colspan="3">
                            <div class="SendFull" style="display: inline-block;">
                                <label>
                                    订购数量每满<input type="text" onkeyup='KeyInt2(this)' runat="server" id="txtSendFull"
                                        class="send" style="width: 50px;" />，获赠商品（<input type="text" runat="server" id="txtSendNum"
                                            onkeyup='KeyInt2(this)' class="send" style="width: 36px;" />）个</label>
                            </div>
                            <div class="Discount" style="display: inline-block;">
                                <label>
                                    在原订货价基础上打折（<input type="text" onkeyup='KeyInt2(this)' style="width: 36px;" runat="server"
                                        id="txtDiscount" class="send" />）%</label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="background: #f6f6f6 none repeat scroll 0 0;">
                            <span>促销描述</span>
                        </td>
                        <td colspan='3'>
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
            <!--促销商品明细 start-->
            <div style="padding: 15px 0 0 5px;">
                <!--选择促销商品按钮 start-->
                <div class="tools">
                    <div class="click" id="btnGoods">
                        <ul class="toolbar left">
                            <li><span>
                                <img src="../images/t01.png" /></span>选购促销商品 </li>
                            <b class="hint" style="position: relative; top: 6px;">4</b>
                        </ul>
                    </div>
                </div>
                <!--选择促销商品按钮 end-->
                <!--新增促销商品列表 start-->
                <div>
                    <div class="tablelist">
                        <asp:Repeater ID="rpDtl" runat="server">
                            <HeaderTemplate>
                                <table>
                                    <tr>
                                        <th class="t7">
                                            序 号
                                        </th>
                                        <th>
                                            商品名称
                                        </th>
                                        <th class="t6">
                                            商品描述
                                        </th>
                                        <th class="t8">
                                            单 位
                                        </th>
                                        <th class="t5">
                                            基础价格
                                        </th>
                                        <% if (Type == "0")
                                           { %>
                                        <th class="t5">
                                            促销价格
                                        </th>
                                        <% } %>
                                        <th class="t8">
                                            删 除
                                        </th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <div class="tc">
                                            <%# Container.ItemIndex+1 %></div>
                                    </td>
                                    <td>
                                        <div class="tcle">
                                            <%# GoodsName(Eval("ID").ToString()) %></div>
                                    </td>
                                    <td>
                                        <div class="tcle">
                                            <%# Goodsmemo(Eval("ID").ToString())%></div>
                                    </td>
                                    <td>
                                        <div class="tc">
                                            <%# GoodsUnit(Eval("ID").ToString())%></div>
                                    </td>
                                    <td>
                                        <div class="tc">
                                            <%# string.Format("{0:N2}", Convert.ToDecimal(Eval("SalePrice").ToString()))%></div>
                                    </td>
                                    <% if (Type == "0")
                                       { %>
                                    <td align="center">
                                        <input type='text' class='textBox' style="width: 80px;" onfocus="InputFocus(this)"
                                            id='txtPrice_<%# Eval("ID") %>' name='txtPrice' value='<%# string.Format("{0:N2}", Convert.ToDecimal(Eval("TinkerPrice").ToString()))%>'
                                            onkeyup='KeyInt2(this)' onchange='onchengPrice(<%# Eval("ID") %>,this)' />
                                    </td>
                                    <% } %>
                                    <td>
                                        <div class="tc">
                                            <img src='../../images/del.gif' style="width: 16px; height: 16px; border: 0px; cursor: pointer;"
                                                onclick='delGoods(<%# Eval("ID") %>);'></div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <!--新增促销商品列表 end-->
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

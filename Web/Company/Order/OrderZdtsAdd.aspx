<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderZdtsAdd.aspx.cs" Inherits="Company_Order_OrderZdtsAdd" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TextDisList.ascx" TagPrefix="uc1" TagName="DisList" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>账单新增</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $_def.ID = "btnSave";

            $(".showDiv4 .ifrClass").css("width", "355px");
            $(".showDiv4").css("width", "350px");
            $(".txt_product_class").css("width", "350px");
            $('iframe').load(function () {
                $('iframe').contents().find('.pullDown').css("width", "350px");
            });
            var type = '<%=Request["type"]+"" %>';
            if (type == "") {
                //$(".cancel").css("display", "none");
            }

            $("#btncancel2").click(function () {

                //$(window.parent.leftFrame.document).find(".menuson li.active").removeClass("active");
                //window.parent.leftFrame.document.getElementById("ddcx").className = "active";

                window.location.href = 'OrderZdtsList.aspx';
            });
            //计量单位
            $(".txtunit").click(function () {
                if ($.trim($(".pullDown2").attr("class")) == "pullDown2 xy") {
                    $(".pullDown2").hide();
                    $(".pullDown2").removeClass("xy");
                } else {
                    $(".pullDown2").show();
                    $(".pullDown2").addClass("xy");
                    $(".pullDown2").css("display", "block");
                }
                var x = $(".txtunit").offset().left;
                var y = $(".txtunit").offset().top;
                // alert(x + "," + y);
                $(".pullDown2").css("position", "absolute");
                $(".pullDown2").css("left", x - 5 + "px");
                $(".pullDown2").css("top", y + 30 + "px");
            })
            $(document).on("click", ".pullDown2 .list li", function () {
                $(".txtunit").val($.trim($(this).text()));
                $(".pullDown2").hide();
                $(".pullDown2").removeClass("xy");
            })
            //新增计量单位
            $(".addBtn").click(function () {
                $(".tip").fadeIn(200);
                $(".txtunits").focus();
                $(".txtunits").val("");
                $(".Layer").fadeIn(200);
            });
            $(document.body).click(function (e) {
                var xx = e.pageX; //鼠标X轴
                var yy = e.pageY; //鼠标Y轴
                var x = parseInt($(".txtunit").offset().left) - 5; //文本框坐标
                var y = parseInt($(".txtunit").offset().top) + 25; //文本框坐标

                if (xx > x && xx < x + 170 && yy + 25 > y && yy < y) {
                    //  alert(x + "," + xx + ";;" + y + "," + yy)
                } else {
                    if ($(".pullDown2").is(":visible")) {
                        $(".pullDown2").hide();
                        $(".pullDown2").removeClass("xy");
                    }
                }
            });
            //取消计量单位
            $("#btncancel1").click(function () {
                $(".tip").fadeOut(100);
                $(".Layer").fadeOut(100);
            });
            if ($(".pullDown2 .list li").length > 6) {//超过6个单位计量，则出现滚轴
                $(".pullDown2 .list").css("height", "156px");
            }
            //验证新增计量单位
            $(".btnAddUnit").click(function () {
                var unit = $(".txtunits").val();
                if (unit == "") {
                    layerCommon.msg("计量单位不能为空", IconOption.哭脸, 2000);
                    return false;
                }
                $.ajax({
                    type: "post",
                    url: "OrderZdtsAdd.aspx",
                    data: { ck: Math.random(), action: "AddFykm", unit: unit },
                    dataType: "json",
                    success: function (data) {
                        var html = "";
                        $(data).each(function (index, obj) {
                            if (obj.AtVal == "sb") {
                                layerCommon.msg("费用名称添加失败", IconOption.哭脸, 2000);
                            } else if (obj.AtVal == "ycz") {
                                layerCommon.msg("费用名称已存在", IconOption.哭脸, 2000);
                            } else if (obj.AtVal == "cc") {
                                layerCommon.msg("费用名称下拉加载失败", IconOption.哭脸, 2000);
                            } else {
                                html += "<li><a href=\"javascript:;\">" + obj.AtVal + "</a></li>";
                                if (index == 0) {
                                    $(".txtunit").val(obj.AtVal);
                                }
                            }
                        })
                        if (html != "") {
                            $(".pullDown2 .list").html(html);
                            $(".tip").fadeOut(100);
                            $(".Layer").fadeOut(100);
                        }
                    }, error: function () { }
                })
                return false;
            })
        });



        //禁用Enter键表单自动提交  
        document.onkeydown = function (event) {
            var target, code, tag;
            if (!event) {
                event = window.event; //针对ie浏览器  
                target = event.srcElement;
                code = event.keyCode;
                if (code == 13) {
                    tag = target.tagName;
                    if (tag == "TEXTAREA") { return true; }
                    else { return false; }
                }
            }
            else {
                target = event.target; //针对遵循w3c标准的浏览器，如Firefox  
                code = event.keyCode;
                if (code == 13) {
                    tag = target.tagName;
                    if (tag == "INPUT") { return false; }
                    else { return true; }
                }
            }
        }

        function DeliveryAdd() {
            var height = document.body.clientHeight; //计算高度
            var layerOffsetY = (height - 450) / 2; //计算宽度

            var disId = $("#hidDisId").val();
            if (disId.toString() == "") {
                layerCommon.msg("请选择代理商", IconOption.哭脸, 2000);
                return false;
            }

            var index = layerCommon.openWindow('新增地址', 'DeliveryAdd.aspx?disId=' + disId, '710px', '300px'); 
            $("#hid_Alert2").val(index); //记录弹出对象 
        }

        //金额只能输入正数和小数
        function KeyIntPrice(val) {
            val.value = val.value.replace(/[^\d.]/g, '');
        }

        function KeyIn(val) {
            val.value = "";
         }
    </script>
    <style type="text/css">
        input[type='text']
        {
            width:90%; margin:0px auto;
        }
        .dh td
        {
            line-height: 35px;
        }
       .number1{ display:inline-block; border:1px solid #ddd; overflow:hidden; height:23px; line-height:23px;}
       .number1 a{ /*background:#ededed;*/ width:23px; height:23px; text-align:center; display:inline-block; color:#000; font-size:14px;font-family:"微软雅黑";}
	   .number1 .add{border-right: 1px solid #ddd;}
	   .number1 a:hover{ text-decoration:none;}
	   .number1 .minus{border-left: 1px solid #ddd;}
	   .number1 .num{ width:40px; height:23px; line-height:23px;line-height:17px\9;*line-height:16px; background:#fff;text-align:center; border:none; display:inline; padding:0; margin:0;}
       .pricea{ display: inline-block; height: 24px; line-height: 30px; overflow: hidden; }
    </style>
    <style type="text/css">
         input[type='text']
        {
            width:200px;
            margin-left:5px;
        }
        .txtunit
        {
            width: 172px;
        }
        .ke-container
        {
            margin-left: 6px;
            margin-top: 5px;
            margin-bottom: 6px;
        }
        .ke-toolbar span
        {
            padding-right: 0px;
        }
        .tb2
        {
            width: 100%;
            height: auto;
            border-bottom: medium none;
        }
        .tb2 table
        {
            border-collapse: collapse;
            border-spacing: 0;
        }
        .tb2 .span
        {
            background: none repeat scroll 0 0 #f6f6f6;
            display: block;
            padding-right: 10px;
            text-align: right;
            white-space: nowrap;
        }
        .tb2 label
        {
            padding-left: 5px;
        }
        .tb2 td
        {
            border: 1px solid #dedede;
            font-size: 13px;
            line-height: 30px;
            text-align: left;
        }
        .tb2 span i
        {
            color: red;
            margin-right: 5px;
        }
        .dh3 td
        {
            border: 0px solid #dedede;
            font-size: 13px;
            line-height: 35px;
            text-align: left;
        }
        .pullDown2
        {
            margin: -5px 0px 0px 5px;
            border: 1px solid #e5e5e5;
            width: 200px;
            background: #fff;
            position: relative;
        }
        .pullDown2 .list
        {
            overflow-y: scroll;
        }
        .pullDown2 .list a
        {
            padding-left: 10px;
            line-height: 26px;
            height: 26px;
            display: block;
            color: #444;
        }
        .pullDown2 .list a:hover
        {
            background: #d1d1d2;
            color: #444;
        }
        .pullDown2 .addBtn
        {
            background: #f5f5f5;
            border-top: 1px solid #ddd;
            height: 30px;
            line-height: 30px;
            position: relative;
            display: block;
            padding-left: 25px;
            color: #555;
        }
        .pullDown2 .addIcon
        {
            width: 12px;
            height: 14px;
            background: url(../images/t05.png) no-repeat 0 0;
            display: inline-block;
            position: absolute;
            top: 8px;
            left: 8px;
        }
        .xy
        {
            z-index: 999999px;
            position: absolute;
        }
        .tipinfo
        {
            padding-top: 60px;
            margin-left: 0;
            height: 400px;
        }
        .abc
        {
            margin-left: 5px;
            }
    </style>
    <%--<link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>--%>
    <script src="../js/order.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-2" />
    <input id="hid_Alert" type="hidden" />
    <input id="hid_Alert1" type="hidden" />
    <input id="hid_Alert2" type="hidden" />
    <input type="hidden" id="hidCompId" runat="server" />
    <input type="hidden" id="hidgoodsInfo" runat="server" />
    <input type="hidden" id="hiddelgoodsid" runat="server" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="../pay/PayZdblList.aspx">账单查询</a></li><li>></li>
                <li><a href="#" id="zd" runat="server">账单新增</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <div class="div_content">
            <!--销售订单主体 start-->
            <%--<div style="padding-left: 70px; ">--%>
            <div class="lbtb">
                <table class="dh">
                    <tr>
                        <td style="width: 140px;">
                            <span>
                                <label class="required">
                                    *</label>选择代理商</span>
                        </td>
                        <td>
                            <uc1:DisList runat="server" ID="DisListID"/>
                        </td>
                        <td style="width: 140px;">
                            <span>账单编号</span>
                        </td>
                        <td>
                            <label id="lblReceiptNo" runat="server" style="width: 155px; display: inline-block;">
                                （自动生成）</label>
                        </td>
                    </tr>
                    <tr>
                        <td style="background: #f6f6f6 none repeat scroll 0 0;">
                            <span>
                                <label class="required">
                                    *</label>费用名称</span>
                        </td>
                        <td>
                            <div style="display: none;">
                                <select id="ddlOtype" runat="server" onchange="Otypechange1()" class="textBox" style="width: 150px;
                                    margin-top: 2px;">
                                </select>
                            </div>
                            <input name="txtunit" maxlength="10" type="text" id="txtunit" readonly="readonly"
                                runat="server" class="textBox txtunit" style="width: 200px; cursor: pointer;
                                margin-left: 5px;" />
                            <div class="pullDown2" style="display: none;">
                                <ul class="list">
                                    <asp:Repeater ID="rptUnit" runat="server">
                                        <ItemTemplate>
                                            <li><a href="javascript:;">
                                                <%# Eval("AtVal")%></a></li></ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                                <a href="javascript:;" class="addBtn"><i class="addIcon"></i>新增</a>
                            </div>
                        </td>
                        <td style="background: #f6f6f6 none repeat scroll 0 0;">
                            <span>账单金额</span>
                        </td>
                        <td>
                            <input name="txtOtherAmount" maxlength="50" onfocus="KeyIn(this)" onkeyup='KeyIntPrice(this)' runat="server" id="txtOtherAmount" type="text"
                                class="textBox" style="width: 200px; cursor: pointer; margin-left: 5px;" value="0.00" />
                            <input type="hidden" id="hidTotalAmount" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>有效截止日期</span>
                        </td>
                        <td>
                            <input name="txtArriveDate" runat="server" onclick="WdatePicker({minDate:'%y-%M-%d'})"
                                id="txtArriveDate" readonly="readonly" type="text" class="Wdate" value="" style="width: 150px;
                                margin-left: 5px;" />
                        </td>
                        <td>
                            <span>制单人</span>
                        </td>
                        <td>
                            <label id="txtDisUser" runat="server">
                            </label>
                            <input type="hidden" id="hidDisUserId" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="background: #f6f6f6 none repeat scroll 0 0;">
                            <span style="height: auto;">账单备注</span>
                        </td>
                        <td colspan="3">
                            <textarea id="txtRemark" maxlength="400" class="textarea" placeholder="账单备注不能超过400个字"
                                runat="server"></textarea>
                        </td>
                    </tr>
                </table>
            </div>
            <%--</div>--%>
            <!--销售订单主体 start-->
            <!--清除浮动-->
            <div style="clear: none;">
            </div>
            <!--销售订单明细 start-->
            <!--销售订单明细 start-->
            <!--遮照层-->
            <div class='Layer'>
            </div>
            <!--新增 start-->
            <div class="tip" style="display: none;">
                <div style="position: absolute; top: 0; left: 0; right: 0; bottom: 0; z-index: 999;
                    background: #fff;">
                    <div class="tiptop">
                        <span>新增</span><a></a></div>
                    <div class="tipinfo">
                        <div class="lb">
                            <span><i class="required">*</i>费用名称：</span><input name="txtunits" id="txtunits" type="text"
                                runat="server" class="textBox txtunits" />
                        </div>
                        <div class="tipbtn">
                            <input name="" type="button" class="orangeBtn btnAddUnit" value="确定" />
                            &nbsp;
                            <input name="" type="button" class="cancel" id="btncancel1" value="取消" />
                        </div>
                    </div>
                </div>
                <div id="xubox_border1" class="xubox_border" style="z-index: 11; opacity: 0.3; filter: Alpha(Opacity=30) !important;
                    border-radius: 5px; top: -8px; left: -8px; right: -8px; bottom: -8px; background-color: rgb(0, 0, 0);
                    position: absolute; top">
                </div>
            </div>
            <!--新增 end-->
            <div class="footerBtn">
                <asp:Button ID="btnSave" runat="server" Text="确定" CssClass="orangeBtn" OnClick="btnSave_Click"
                    OnClientClick="return formCheck();" />&nbsp;
                <input name="" type="button" class="cancel" id="btncancel2" value="返回" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>

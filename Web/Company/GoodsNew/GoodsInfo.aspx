<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsInfo.aspx.cs" Inherits="Company_Goods_GoodsInfo" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>商品详细</title>

    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <link href="../../css/lanrenzhijia.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <script>
        $(function () {

            $("img").each(function (index, obj) {
                var width = $(window).width() * 0.7;
                var attewidth = $(obj).width() * 1;
                if (attewidth >= width) {
                    var style = $(obj).attr("style");
                    if (style == undefined) {
                        $(obj).attr("style", "width:100%;")
                    }
                    else {
                        $(obj).attr("style", $(obj).attr("style").replace("width:" + attewidth + "px;", ""))
                        $(obj).width("100%");
                    }
                }
            })
        })
    </script>
    <style>
        table {
            border-collapse: collapse;
            white-space: normal;
            line-height: normal;
            font-weight: normal;
            font-size: medium;
            font-variant: normal;
            font-style: normal;
            color: -webkit-text;
        }

        tr {
            display: table-row;
            vertical-align: inherit;
            border-color: inherit;
        }

        .control-input {
            float: none;
            line-height: 29px;
            margin-left: 5px;
            overflow: hidden;
            padding-left: 0;
        }

        .div_content .tb td label {
            display: inline-block;
            line-height: 25px;
            margin-left: 0;
            min-width: 0;
            padding: 0;
        }

        label.checked {
            background-color: #648bc6;
            border: 1px solid #648bc6;
            color: #fff;
        }

        input[type="checkbox"] {
            margin: 3px 3px 3px 4px;
            vertical-align: middle;
        }

        .control-input label {
            border: 1px solid #d6dee3;
            cursor: pointer;
            display: block;
            float: left;
            height: 26px;
            margin-right: 10px;
            text-align: center;
            width: 80px;
        }

        .ProdView .btns .bule {
            background: #959595 none repeat scroll 0 0;
            border: 1px solid #959595;
        }

        .ProdView .btns a {
            color: #fff;
            border: 1px solid #a91c1d;
            display: inline-block;
            font-size: 18px;
            height: 40px;
            line-height: 40px;
            margin-right: 15px;
            text-align: center;
            width: 150px;
        }

        .btns {
            margin-top: 20px;
        }

        .ProdView .btns .red {
            background: #648bc6 none repeat scroll 0 0;
            border: 1px solid #648bc6;
        }

        .sale-txt {
            background: #fffbfb none repeat scroll 0 0;
            border: 1px solid #f7d2d3;
            color: #333;
            display: none;
            left: 0;
            padding: 8px 11px;
            position: absolute;
            top: 33px;
            width: 200px;
            z-index: 99;
        }

            .sale-txt .arrow {
                background: rgba(0, 0, 0, 0) url("../images/icon-1.png") no-repeat scroll -112px -225px;
                display: inline-block;
                height: 4px;
                left: 10px;
                position: absolute;
                top: -5px;
                width: 7px;
            }

        .sale {
            background: #e4393c none repeat scroll 0 0;
            border-radius: 5px;
            color: #fff;
            cursor: pointer;
            display: inline-block;
            height: 18px;
            line-height: 17px;
            margin-right: 2px;
            padding: 0 7px;
        }

        .sale-box {
            font-size: 12px;
            padding-top: 0;
            display: inline-block;
            margin-left: 5px;
            padding-top: 5px;
            position: relative;
        }

            .sale-box:hover .sale-txt {
                display: block;
            }
        /*right.html*/
        .place {
            height: 30px;
            color: #999;
            z-index: 999;
            padding-bottom: 3px;
        }

            .place a {
                color: #888;
                cursor: default;
                cursor: pointer;
            }

        .placeul li {
            float: left;
            line-height: 30px;
            padding: 0 3px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:Top ID="top1" runat="server" ShowID="nav-3" />
        <input id="hid_Alert" type="hidden" />
        <div class="rightinfo">
            <div class="place">
                <ul class="placeul">
                    <li><a href="../jsc.aspx">我的桌面</a></li>
                    <li>></li>
                    <li><a href="GoodsList.aspx" runat="server" id="atitle">商品列表</a></li>
                    <li>></li>
                    <li><a href="javascript:;" runat="server" id="ctitle">商品详细</a></li>
                </ul>
            </div>
            <div class="ProdView">
                <div class="pic" style="display: none">
                    <img src="../../images/havenopicmax.gif" width="350" height="350" />
                </div>
                <div class="lanrenzhijia" style="position: absolute;">
                    <!-- 大图begin -->
                    <div id="preview" class="spec-preview">
                        <span class="jqzoom">
                            <img jqimg="" src="../../images/havenopicmax.gif" id="imgPic" runat="server" width="350"
                                height="350" /></span>
                    </div>
                    <!-- 大图end -->
                    <!-- 缩略图begin -->
                    <div class="spec-scroll">
                        <a class="prev">&lt;</a> <a class="next">&gt;</a>
                        <div class="items">
                            <ul>
                                <asp:Repeater ID="rptImg" runat="server">
                                    <ItemTemplate>
                                        <li>
                                            <img bimg="<%# Eval("pic").ToString()=="" ? "../images/havenopicmax.gif" : Common.GetWebConfigKey("OssImgPath") + "company/"+  (Session["UserModel"] as LoginModel).CompID + "/" + Eval("pic") %>"
                                                src="<%# Eval("pic").ToString()=="" ? "../images/havenopicmax.gif" : Common.GetWebConfigKey("OssImgPath") + "company/"+  (Session["UserModel"] as LoginModel).CompID + "/" + Eval("pic") + "?x-oss-process=style/resize400" %>"
                                                onmousemove="preview(this);">
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <li id="ImgShow" runat="server"><img bimg="" src="../../images/havenopicmax.gif" id="imgPic2" runat="server" onmousemove="preview(this);"></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div class="bt" id="divTitle" runat="server">
                    商品名称
                </div>
                <lable id="divMemo" class="txt" runat="server"></lable>
                <lable id="lblHideInfo1" class="txt" runat="server"></lable>
                <lable id="lblHideInfo2" class="txt" runat="server"></lable>
                <div class="li">
                    <i class="bt2">分类：</i><i id="lblCategory" runat="server"></i>&nbsp;&nbsp;&nbsp;&nbsp;
                <i class="bt2">编码：</i><i id="lblCode" runat="server"></i>
                </div>
                <asp:Literal ID="litZiDingYi" runat="server"></asp:Literal>
                <div class="li">
                    <i class="bt2">销售价格：</i><i class="red">代理商可见</i>
                </div>
                <asp:Literal ID="litAttrVaue" runat="server"></asp:Literal>
                <div class="li" style='<%= OrderInfoType.rdoOrderAudit("商品是否启用库存",this.CompID)=="0"?"": "display:none" %>'>
                    <i class="bt2">商品库存：</i><i id="lblInventory" runat="server">代理商可见</i>
                </div>
                <div class="li">
                    <i class="bt2">计量单位：</i><i id="lblUnit" runat="server">代理商可见</i>
                </div>
                <div class="li">
                    <i class="bt2">店铺显示：</i><i id="lblShow" runat="server">显示</i>
                </div>
                <div class="li" id="isls" runat="server">
                    <i class="bt2">零售价格：</i><i id="lbllsprice" runat="server"></i>
                </div>
                <div class="li" style="display: none">
                    <i class="bt2">店铺推荐：</i><i id="lblRecommend" runat="server">推荐</i>
                </div>


                <div class="li"  style="width:400px;height:25px;">
                    <i class="bt2" style="float: left">商品标签：</i><i><div class="control-input" id="DivLabel"
                        runat="server">
                    </div>
                    </i>
                </div>


                <%--<div class="li" style="width:400px;height:25px;">
                    <i class="bt2" style="float: left">注册证：</i>
                    <i>
                        <div id="UpFileText2" runat="server" style="margin-left: 10px; width: 340px;">
                        </div>
                    </i>
                </div>--%>



                <div class="blank10">
                </div>
                <div class="btns">
                    <a href="javascript:;" class="red aprice" style="text-decoration: none; display: none;">代理商价目</a> 
                    <a href="javascript:;" class="red aedit" style="text-decoration: none;">编辑</a>
                    <a href="javascript:;" id="bule" class="bule fanh" style="text-decoration: none;">返回</a>
                </div>
            </div>
            <div class="blank10">
            </div>
            <div class="nr-deta">
                <div class="t-deta">
                    <b tip="DivShow" class="cur">商品介绍</b>
                    <b tip="DivShow1" >注册证</b>
                </div>
                <span syle="">
                    <div class="nr" id="DivShow" runat="server" style=" display:block;">
                    </div>
                    <div class="nr" id="DivShow1" runat="server"  style=" display:none;">
                    </div>
                </span>
            </div>
            <div class="blank20">
            </div>
        </div>
    </form>
    <script>        $(function () { $.fn.jqueryzoom = function (options) { } })</script>
    <script src="../../js/lanrenzhijia.js" type="text/javascript"></script>
    <%--    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>--%>
    <script>
        $(function () {

            GetPrice();
            $(".fun a").click(function () {
                $(this).addClass("hover").siblings().removeClass("hover");
                GetPrice();
            })
            //编辑
            $(".aedit").click(function () {
                if ('<%=Request["nextstep"] %>' == "1") {
                    location.href = "GoodsEdit.aspx?nextstep=1&KeyID=<%=Common.DesEncrypt(goodsId.ToString(), Common.EncryptKey) %>";
                }
                else {
                    location.href = "GoodsEdit.aspx?KeyID=<%=Common.DesEncrypt(goodsId.ToString(), Common.EncryptKey) %>";
                }
            })
            //代理商价目
            $(".aprice").click(function () {
                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度
                // var index = showDialog('代理商价目', 'UpdateDisPrice.aspx?goodsInfoId=<%=goodsInfoId %>', '880px', '500px', layerOffsetY); //记录弹出对象
                var index = layerCommon.openWindow('代理商价目', 'UpdateDisPrice.aspx?goodsInfoId=<%=goodsInfoId %>', '880px', '500px');

                $("#hid_Alert").val(index); //记录弹出对象
            })
            //返回
            $(".fanh").click(function () {
                location.href = "<%= Request["rtype"]=="1"?"GoodsInfoList.aspx":"GoodsList.aspx"%> ";
            })
        })
        //获取价格
        function GetPrice() {
            var valuelist = ""; //属性值id
            $(".ProdView .fun a").each(function (index, obj) {
                if ($.trim($(".ProdView .fun a").eq(index).attr("class")) == "hover") {
                    var attr = $(".ProdView .fun a").eq(index).parent().siblings(".bt2").attr("tip");
                    valuelist += attr + ":" + $(".ProdView .fun a").eq(index).text() + ";";
                }
            });

            $.ajax({
                type: "post",
                data: { ck: Math.random(), action: "price", value: valuelist },
                dataType: "text",
                success: function (data) {
                    var reg = /^[\u4e00-\u9fa5]{0,}$/; //中文正则表达式
                    if (reg.exec(data)) {
                        $(".li .red").text(data);
                    } else {
                        var price = formatMoney(data, 2);
                        $(".li .red").text("￥" + price);
                    }

                    $.ajax({
                        type: "post",
                        data: { ck: Math.random(), action: "code", value: valuelist },
                        dataType: "text",
                        success: function (data) {
                            if (data != "") {
                                $("#lblCode").text(data.split(',')[0]);
                                $("#lblInventory").text(data.split(',')[1]);
                            }
                        }, error: function () { }
                    })
                }, error: function () { }
            })
        }
        //价格格式
        function formatMoney(s, type) {
            if (/[^0-9\.]/.test(s))
                return "0";
            if (s == null || s == "")
                return "0";
            s = s.toString().replace(/^(\d*)$/, "$1.");
            s = (s + "00").replace(/(\d*\.\d\d)\d*/, "$1");
            s = s.replace(".", ",");
            var re = /(\d)(\d{3},)/;
            while (re.test(s))
                s = s.replace(re, "$1,$2");
            s = s.replace(/,(\d\d)$/, ".$1");
            if (type == 0) {// 不带小数位(默认是有小数位) 
                var a = s.split(".");
                if (a[1] == "00") {
                    s = a[0];
                }
            }
            return s;
        }

        $(document).on("click",".t-deta b",function(){
           $(this).siblings("b").removeClass("cur");
           $(this).addClass("cur");
           var tip=$.trim($(this).attr("tip"));
           $("#"+tip).siblings("div").hide();
           $("#"+tip).show();
        });
    </script>
</body>
</html>

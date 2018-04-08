<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsInfo.aspx.cs" Inherits="Admin_Systems_GoodsInfo" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>商品详细</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/shop.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs1.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <link href="../../css/lanrenzhijia.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .control-input
        {
            display: inline;
            float: none;
            line-height: 29px;
            margin-left: 5px;
            overflow: hidden;
            padding-left: 0;
        }
        
        .div_content .tb td label
        {
            display: inline-block;
            line-height: 25px;
            margin-left: 0;
            min-width: 0;
            padding: 0;
        }
        label.checked
        {
            background-color: #003c9d;
            border: 1px solid #003c9d;
            color: #fff;
        }
        input[type="checkbox"]
        {
            margin: 3px 3px 3px 4px;
            vertical-align: middle;
        }
        .control-input label
        {
            border: 1px solid #d6dee3;
            cursor: pointer;
            display: block;
            float: left;
            height: 26px;
            margin-right: 10px;
            text-align: center;
            width: 80px;
        }
        
        .ProdView .btns .bule
        {
            background: #6694b3 none repeat scroll 0 0;
            border: 1px solid #becbd5;
        }
        .ProdView .btns a
        {
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
        .btns
        {
            margin-top: 20px;
        }
        .ProdView .btns .red
        {
            background: #c81e1f none repeat scroll 0 0;
            border: 1px solid #a91c1d;
        }
		.header{ padding:0; padding-left:150px; height:60px; background:#f5f5f5; border-bottom:1px solid #ddd; width:auto;}
    </style>
    <script>
        var LayserIndex = 0;
        $(document).ready(function () {
            $("#ASetFirstShow").on("click", function () {
                var height = document.documentElement.clientHeight;
                var layerOffsetY = (height - 500) / 2; //计算宽度
                LayserIndex = showDialog('设置商品是否首页显示', '<%=ResolveUrl("SetGoodsShow.aspx")%>?KeyID=' + '<%=Request["goodsId"] %>', '500px', '350px', layerOffsetY);
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input id="hid_Alert" type="hidden" />
        <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="javascript:;" runat="server" id="atitle" style="outline: medium none;
                text-decoration: none;">我要维护</a><i>></i>
            <a href="javascript:;" runat="server" id="btitle" style="outline: medium none;
                text-decoration: none;">商品列表</a><i>></i>
            <a href="javascript:;" runat="server" id="ctitle" style="outline: medium none;
                text-decoration: none;">商品详细</a>
    </div>
    <div class="mianDiv" style="width: 1100px; margin-left: 10px">
        <div class="ProdView">
            <div class="pic" style="display: none">
                <img src="../../images/havenopicmax.gif" width="350" height="350" /></div>
            <div class="lanrenzhijia" style="position: absolute;">
                <!-- 大图begin -->
                <div id="preview" class="spec-preview">
                    <span class="jqzoom">
                        <img jqimg="" src="../../images/havenopicmax.gif"
                            id="imgPic" runat="server" width="350" height="350" /></span>
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
                                        <img bimg="<%# Eval("pic3").ToString()=="D" || Eval("pic3").ToString()==""?"../../images/havenopicmax.gif": Common.GetWebConfigKey("ImgViewPath") + "/GoodsImg/"+ Eval("pic3") %>"
                                            src="<%# Eval("pic2").ToString()=="X" || Eval("pic2").ToString()==""?"../../images/havenopicmax.gif": Common.GetWebConfigKey("ImgViewPath") + "/GoodsImg/"+ Eval("pic2") %>"
                                            onmousemove="preview(this);"></li>
                                </ItemTemplate>
                            </asp:Repeater>
                            <li id="ImgShow" runat="server">
                                <img bimg="" src="../../images/havenopicmax.gif"
                                    id="imgPic2" runat="server" onmousemove="preview(this);"></li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="bt" id="divTitle" runat="server">
                商品名称
            </div>
            <div class="txt" id="divMemo" runat="server">
            <lable id="lblHideInfo1" class="txt" runat="server"></lable>
            <lable id="lblHideInfo2" class="txt" runat="server"></lable>
            </div>
            <div class="li">
                <i class="bt2">分类：</i><i id="lblCategory" runat="server">代理商可见</i>&nbsp;&nbsp;&nbsp;&nbsp;
                <i class="bt2">编码：</i><i id="lblCode" runat="server">代理商可见</i></div>
            <asp:Literal ID="litZiDingYi" runat="server"></asp:Literal>
            <div class="li">
                <i class="bt2">销售价格：</i><i class="red"></i></div>
            <asp:Literal ID="litAttrVaue" runat="server"></asp:Literal>
                <div class="li" style='<%= OrderInfoType.rdoOrderAudit("商品是否启用库存",compId)=="0"?"": "display:none" %>'>
                <i class="bt2">库存：</i><i id="lblInventory" runat="server">代理商可见</i></div>
            <div class="li">
                <i class="bt2">计量单位：</i><i id="lblUnit" runat="server">代理商可见</i></div>
            <div class="li">
                <i class="bt2" style="float: left">商品标签：</i><i><div class="control-input" id="DivLabel"
                    runat="server">
                </div>
                </i>
            </div>
            <div class="blank10">
            </div>
            <div class="btns">
                <a href="javascript:history.go(-1);" id="bule" class="bule" style="text-decoration: none;">
                    返 回</a>
                <a href="javascript:;" id="ASetFirstShow" class="bule" style="text-decoration: none; padding-left:5px;padding-right:5px;">
                    设置首页显示</a>（<asp:Literal ID="labText" runat="server"></asp:Literal>）
                    </div>
        </div>
        <div class="blank10">
        </div>
        <div class="hotProd ProdNr">
            <div class="zbt">
                <b>商品介绍</b></div>
            <div class="detailed" id="DivShow" runat="server">
            </div>
        </div>
        <div class="blank20">
        </div>
    </div>
     </div>
    </form>
    <script src="../../js/jquery.jqzoom.js" type="text/javascript"></script>
    <script src="../../js/lanrenzhijia.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script>
        $(function() {
            GetPrice();
            $(".fun a").click(function() {
                $(this).addClass("hover").siblings().removeClass("hover");
                GetPrice();
            });
        });
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
                url: "goodsInfo.aspx",
                data: { ck: Math.random(), action: "price", value: valuelist },
                dataType: "text",
                success: function(data) {
                    var reg = /^[\u4e00-\u9fa5]{0,}$/; //中文正则表达式
                    if (reg.exec(data)) {
                        $(".li .red").text(data);
                    } else {
                        var price = formatMoney(data, 2);
                        $(".li .red").text("￥" + price);
                    }
                },
                error: function() {}
            });
            $.ajax({
                type: "post",
                data: { ck: Math.random(), action: "code", value: valuelist },
                dataType: "text",
                success: function(data) {
                    if (data != "") {
                        $("#lblCode").text(data.split(',')[0]);
                        $("#lblInventory").text(data.split(',')[1]);
                    }
                },
                error: function() {}
            });
        }
        //关闭代理商调价区域
        function GbGoods() {
            CloseDialog();
        }
    </script>
</body>
</html>

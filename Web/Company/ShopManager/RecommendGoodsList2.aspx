<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecommendGoodsList2.aspx.cs"
    Inherits="Company_ShopManager_RecommendGoodsList2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>主推商品维护</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <%--    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>--%>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <style>
        .place2 .placeul li
        {
            float: left;
            line-height: 30px;
            padding-left: 7px;
            padding-right: 12px;
            background: url(../images/rlist.gif) no-repeat right;
        }
        .place2 span
        {
            line-height: 30px;
            font-weight: bold;
            float: left;
            margin-left: 12px;
        }
        .place2
        {
            height: 30px;
            color: #fff;
            z-index: 999;
            background: #1d4a8d;
            border-left: 1px solid #103a78;
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
        }
        .place2 a
        {
            color: #fff;
            cursor: default;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="hid_Alert">
    <div class="place2">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="../../admin/main.aspx" target="_top">首页</a></li>
            <li><a href="#" runat="server" id="Btitle">厂商管理</a></li>
            <li><a href="#" runat="server" id="Atitle">店铺推荐</a></li>
        </ul>
    </div>
    <div class="rightinfo" style="margin-left: 0px; margin-top: 30px; width: auto;">
        <!--广告区 start-->
        <div class="adbox">
            <!--店铺推荐 start-->
            <div class="adMenu">
                <div class="title">
                    店铺推荐 <i class="addCate" title="新增分类">新增</i></div>
                <asp:Literal ID="lblHtml" runat="server"></asp:Literal>
                <%--    <div class="bt" style="cursor: pointer;">
                分类标题<i title="编辑" class="edit_">编辑</i></div>
            <ul class="list">
                <li><a href="JavaScript:;">商品名称</a></li>
            </ul>
             <div class="line">
            </div>--%>
            </div>
            <!--店铺推荐 end-->
            <div class="adInfo" style="margin-right: 400px; width: 400px; display: none;">
                <div class="title">
                    <a href="" class="hover">主推商品信息</a></div>
                <ul class="list" style="height: 400px; line-height: 30px; overflow-y: auto;">
                    <div class="ullist">
                        <li><i style="color: Red">*</i>分类标题：<input type="text" class="textBox txtTitle" name="txtTitle" /></li>
                        <li>
                            <input type="hidden" class="hidGoodsId" name="hidGoodsId" />
                        </li>
                        <li><i style="color: Red">*</i>选择商品：<input type="text" class="textBox txtGoodsName"
                            name="txtGoodsName" readonly="readonly" />
                            <i class="add_" title="添加">添加</i><i class="del_" title="删除" style="display: none;">删除</i></li>
                        <li><i>*</i>显示名称：<input type="text" class="textBox txtShowName" name="txtShowName" /></li>
                    </div>
                    <div style="padding-left: 70px; margin-top: 10px;">
                        <input type="hidden" id="hidtitle" runat="server" />
                        <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="保存" OnClientClick="return formCheck()"
                            OnClick="btnAdd_Click" />
                        <input name="" type="button" class="orangeBtn btnDel" onclick="return delCheck()"
                            value="删除" />
                        <asp:Button ID="btnDel" CssClass="orangeBtn" Style="display: none;" runat="server"
                            OnClientClick="return delCheck()" Text="删除" OnClick="btnDel_Click" />
                    </div>
                </ul>
            </div>
        </div>
        <!--广告区 end-->
    </div>
         <div class="div_footer">
            <input name="" type="button" class="cancel" onclick="javascript:history.go(-1);"
                value="返回" />
        </div>
    </form>
    <script>
        $(function () {
            //新增分类
            $(".addCate").click(function () {
                $("#hidtitle").val("");
                $(".adInfo").show();
                $(".btnDel").hide();
                $(".ullist").html("<li><i style=\"color: Red\">*</i>分类标题：<input type=\"text\" class=\"textBox txtTitle\" name=\"txtTitle\" /></li><li><input type=\"hidden\" class=\"hidGoodsId\" name=\"hidGoodsId\" /></li><li><i style=\"color: Red\">*</i>选择商品：<input type=\"text\" class=\"textBox txtGoodsName\"  name=\"txtGoodsName\" readonly=\"readonly\" /><i class=\"add_\" title=\"添加\">添加</i><i class=\"del_\" title=\"删除\" style=\"display: none;\">删除</i></li><li><i>*</i>显示名称：<input type=\"text\" class=\"textBox txtShowName\" name=\"txtShowName\" /></li>");
            })
            //商品新增
            $(document).on("click", ".add_", function () {
                $(this).next().show();
                $(this).hide();
                $(".adInfo .ullist").append(" <li><input type=\"hidden\" class=\"hidGoodsId\" name=\"hidGoodsId\" /></li><li><i style=\"color: Red\">*</i>选择商品：<input type=\"text\" class=\"textBox txtGoodsName\" name=\"txtGoodsName\" readonly=\"readonly\" /><i class=\"add_\" title=\"添加\">添加</i><i class=\"del_\" title=\"删除\" style=\"display: none;\">删除</i></li><li><i>*</i>显示名称：<input type=\"text\" class=\"textBox txtShowName\" name=\"txtShowName\" /></li>");
            })
            //删除单个商品
            $(document).on("click", ".del_", function () {
                $(this).parent().prev().remove(); //隐藏id删除
                $(this).parent().next().remove(); //显示名称删除
                $(this).parent().remove(); //商品名称删除
            })
            //编辑商品
            $(document).on("click", ".edit_", function () {
                $(".adInfo").show();
                $(".btnDel").show();
                $("#hidtitle").val($.trim($(this).attr("tip")));
                $.ajax({
                    type: "post",
                    data: { ck: Math.random(), action: "edit", title: $.trim($(this).attr("tip")) },
                    dataType: "text",
                    success: function (data) {
                        if (data != "") {
                            $(".ullist").html(data);
                        } else {
                            layerCommon.msg("数据有误", IconOption.错误);
                            return;
                        }
                    }
                })
            })
            //选择商品
            $(document).on("click", ".txtGoodsName", function () {
                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度
                var index = $(this).parent("li").index()
                // var index = showDialog('选择商品', 'goodslist.aspx?index=' + index, '880px', '450px', layerOffsetY); //记录弹出对象
                var index = layerCommon.openWindow('选择商品', 'goodslist.aspx?index=' + index+"&comPid="+<%=comPid %>, '880px', '450px');  //记录弹出对象
                $("#hid_Alert").val(index); //记录弹出对象
            })
        })
        //验证
        function formCheck() {
            var bol = false;
            var title = $.trim($(".txtTitle").val()); //分类标题
            var goodsId = $.trim($(".hidGoodsId").val()); //商品id
            if (title == "") {
                layerCommon.msg("请输入分类标题", IconOption.错误);
                return false;
            }
            if (goodsId == "") {
                layerCommon.msg("请选择商品", IconOption.错误);
                return false;
            }
            return true;
        }
        //删除验证
        function delCheck() {
            layerCommon.confirm("确定删除？", function () { $("#btnDel").click(); });
        }
    </script>
</body>
</html>

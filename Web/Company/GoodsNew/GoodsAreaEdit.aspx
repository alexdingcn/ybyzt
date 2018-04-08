<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsAreaEdit.aspx.cs" Inherits="Company_GoodsNew_GoodsAreaEdit" %>

<%@ Register Src="~/Company/UserControl/TreeDisArea.ascx" TagPrefix="uc1" TagName="TreeDisArea" %>
<%@ Register Src="~/Company/UserControl/TreeDemo.ascx" TagPrefix="uc1" TagName="TreeDemo" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>商品不可售区域新增</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <style>
        .col-xs-6
        {
            float: left;
            min-height: 1px;
            padding-left: 15px;
            padding-right: 15px;
            position: relative;
        }
        .border
        {
            border: 1px solid #ddd;
        }
        .space5
        {
            margin-top: 5px;
        }
        .row
        {
            margin-left: -15px;
            margin-right: -15px;
        }
        .modal-body
        {
            padding: 15px;
            position: relative;
        }
        .p-b-6
        {
            padding-top: 6px;
        }
        .m-l-10
        {
            margin-left: 10px;
        }
        .pointer
        {
            cursor: pointer;
        }
        .ng-scope span
        {
            display: inline-block;
        }
        input[type="checkbox"], input[type="radio"]
        {
            line-height: normal;
            margin: 3px 5px 5px 5px;
        }
        input, select
        {
            display: inline-block;
        }
        .btn, img, input
        {
            outline: 0 none !important;
            vertical-align: middle;
        }
        .space5 li:hover
        {
            background: #e5ebee;
        }
    </style>
    <script>
        $(function () {
            $('.showDiv2  iframe').load(function () {
                $('.showDiv2 iframe').contents().find('.pullDown').css("width", "130px");
            })
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            })
            //返回
            $(".cancel").click(function () {
                location.href = "GoodsAreaList.aspx";
            })
            //删除
            $(document).on("click", "#divDis img,#divGoods img", function () {
                $(this).parent().parent().remove();
            })
        })
        //代理商区域搜索验证
        function ChkPage() {
            if ($.trim($(".hid_AreaId").val()) == "") {
                layerCommon.msg("请选择代理商区域", IconOption.错误);
                return false;
            }
            $.ajax({
                type: "post",
                data: { ck: Math.random(), action: "dislist", id: $.trim($(".hid_AreaId").val()) },
                dataType: "json",
                success: function (data) {
                    var html = "<ul class=\"ng-scope\">";
                    var json = eval(data);
                    $(json).each(function (index, obj) {
                        if (obj.DisName != undefined) {
                            html += "<li class=\"pointer proHover p-t-6 p-b-6 ng-scope\"><span class=\"customerManager m-l-10\"></span><span class=\"ng-binding\" >" + obj.DisName + "</span><input type=\"hidden\" name=\"disId\" value=\"" + obj.ID + "\" /><span style=\"float:right; margin-right:50px;\"><img src=\"../../images/icon_del.png\" title=\"删除\" /></span></li>";
                        } else {
                            return false;
                        }
                    })
                    html += "</ul>";
                    $("#divDis").html(html);
                }
            })
        }
        //重置代理商区域
        function Chongz() {
            $(".hid_AreaId").val("");
            $(".txt_txtAreaname").val("");
        }
        //商品搜索验证
        function ChkPage2() {
            if ($.trim($(".hid_product_class").val()) == "" && $.trim($(".txtGoodsName").val() == "")) {
                layerCommon.msg("请选择商品分类或者填写商品名称", IconOption.错误);
                return false;
            }
            $.ajax({
                type: "post",
                data: { ck: Math.random(), action: "goodslist", id: $.trim($(".hid_product_class").val()), name: $.trim($(".txtGoodsName").val()) },
                dataType: "json",
                success: function (data) {
                    var html = "<ul class=\"ng-scope\">";
                    var json = eval(data);
                    $(json).each(function (index, obj) {
                        if (obj.GoodsName != undefined) {
                            var strgoods = obj.GoodsName;
                            if (obj.GoodsName.length > 50) {
                                strgoods = obj.GoodsName.substring(0, 50) + "...";
                            }
                            html += "<li class=\"pointer proHover p-t-6 p-b-6 ng-scope\"><span class=\"customerManager m-l-10\"></span><span class=\"ng-binding\" name=\"disName\" title=\""+obj.GoodsName+"\">" + strgoods + "</span><input type=\"hidden\" name=\"GoodsId\" value=\"" + obj.ID + "\" /><span style=\"float:right; margin-right:50px;\"><img src=\"../../images/icon_del.png\" title=\"删除\" /></span></li>";
                        } else {
                            return false;
                        }
                    })
                    html += "</ul>";
                    $("#divGoods").html(html);
                }
            })
        }
        //商品代理商区域
        function Chongz2() {
            $(".hid_product_class").val("");
            $(".txt_product_class").val("");
            $(".txtGoodsName").val("");
        }
        //验证
        function formCheck() {
            var dis = $("#divDis li").length;
            var goods = $("#divGoods li").length;
            if (dis == 0) {
                layerCommon.msg("请选择代理商", IconOption.错误);
                return false;
            }
            if (goods == 0) {
                layerCommon.msg("请选择商品", IconOption.错误);
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-3" />
    <div class="rightinfo">
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a href="GoodsAreasList.aspx">商品不可售区域</a></li><li>></li>
                <li><a href="GoodsAreaEdit.aspx">商品不可售区域新增</a></li>
            </ul>
        </div>
        <div class="div_content">
            <div class="lbtb">
                <div class="modal-body">
                    <div class="row">
                        <div class="col-xs-6" style="width: 375px;">
                            <div>
                                <span style="display: inline-block;">
                                    <ul class="toolbar right ">
                                        <li onclick="return ChkPage()"><span>
                                            <img src="../images/t04.png" /></span>搜索</li>
                                        <li onclick="return Chongz()"><span>
                                            <img src="../images/t06.png" /></span>重置</li>
                                    </ul>
                                    <ul class="toolbar3">
                                        <li><i style="color: Red">*</i>代理商区域:<uc1:TreeDisArea runat="server" ID="txtDisAreaBox" />
                                        </li>
                                    </ul>
                                </span>
                            </div>
                            <div class="border space5" id="divDis" runat="server" style="height: 440px; overflow: auto;">
                            </div>
                        </div>
                        <div class="col-xs-6" style="width: 610px;">
                            <div>
                                <span style="display: inline-block;">
                                    <ul class="toolbar right ">
                                        <li onclick="return ChkPage2()"><span>
                                            <img src="../images/t04.png" /></span>搜索</li>
                                        <li onclick="return Chongz2()"><span>
                                            <img src="../images/t06.png" /></span>重置</li>
                                    </ul>
                                    <ul class="toolbar3">
                                        <li>商品分类:
                                            <uc1:TreeDemo runat="server" ID="txtCategory" />
                                        </li>
                                        <li>商品名称:<input name="txtGoodsName" type="text" id="txtGoodsName" runat="server"
                                            class="textBox txtGoodsName" style="width: 90px" maxlength="50" /></li>
                                    </ul>
                                </span>
                            </div>
                            <div class="border space5" id="divGoods" style="height: 440px; overflow: auto;">
                            </div>
                        </div>
                    </div>
                </div>
                <div>
                </div>
                <div class="div_footer" style="float: left; margin-left: 200px;">
                    <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定不可售" OnClientClick="return formCheck()"
                        OnClick="btnAdd_Click" />&nbsp;
                    <input name="" type="button" class="cancel" value="返回" />
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

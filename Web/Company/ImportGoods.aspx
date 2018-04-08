<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImportGoods.aspx.cs" Inherits="Company_ImportGoods" %>

<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>商品导入</title>
    <link href="css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script>
        $(function () {
            //验证
            $(".btn-o").click(function () {
                var str = $(".file").val();
                if (str == "") {
                    layerCommon.msg("请选择要导入商品的Excel文件", IconOption.错误);
                    return false;
                }
                var suffix = $.trim(str.substring(str.lastIndexOf(".")));
                if (suffix == ".xlsx" || suffix == ".xls") {
                    $("#btnAddList").click();
                    return true;
                } else {
                    layerCommon.msg("请选择Excel文件", IconOption.错误);
                    return false;
                }
            })
            //上传click事件
            $(".file,.file1").change(function () {
                var ua = navigator.userAgent.toLowerCase(); //浏览器信息
                var info = {
                    ie: /msie/.test(ua) && !/opera/.test(ua),        //匹配IE浏览器    
                    op: /opera/.test(ua),     //匹配Opera浏览器    
                    sa: /version.*safari/.test(ua),     //匹配Safari浏览器    
                    ch: /chrome/.test(ua),     //匹配Chrome浏览器    
                    ff: /gecko/.test(ua) && !/webkit/.test(ua)     //匹配Firefox浏览器
                };
                if (!info.ie) {
                    if (this.files[0].size > 2 * 1024 * 1024) {
                        layerCommon.msg("只能导入2M以下的Excel文件", IconOption.错误);
                        return false;
                    }
                }
                $(".inputstyle").val($(this).val());
            })
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-3" />
    <uc1:CompRemove runat="server" ID="Remove" ModuleID="3" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="jsc.aspx">我的桌面 </a></li>
                <li>&gt;</li>
                <li><a href="GoodsNew/GoodsList.aspx" id="a1">商品列表</a></li>
                <li>&gt;</li>
                <li><a href="javascript:;" id="atitle">商品导入</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <div class="blank10">
        </div>
        <!--步骤 start-->
        <ul class="imStep">
            <li class="a1 cur">1、上传导入文件<i class="arr1"></i></li>
            <li class="a2">2、导入预览<i class="arr1"></i><i class="arr2"></i></li>
            <li class="a3">3、导入完成<i class="arr1"></i><i class="arr2"></i></li>
        </ul>
        <!--步骤 end-->
        <!--导入规则说明 start-->
        <div class="imExplain">
            <b class="bt">导入规则说明：</b>
            <p>
                1、数据导入格式可能会发生改变，请及时下载最新导入模板。</p>
            <p>
                2、文件后缀名必须为：xls或xlsx（Excel标准格式），文件大小<2M。</p>
            <p>
                3、商品编码允许为空，为空则系统为其生成编码。</p>
        </div>
        <!--导入规则说明 end-->
        <!--上传模板 start-->
        <div class="imBox">
            <div class="title">
                <a href="GoodsNew/TemplateFile/商品表格导入模版.xls" target="_blank"><i class="dow-i"></i>下载模板</a></div>
            <div class="b-upload fl">
                <input name="" type="text" id="viewfile" class="inputstyle" />
                <label for="unload" class="file1">
                    +</label>
                <asp:FileUpload ID="upload" runat="server" CssClass="file" Style="width: 200px;" />
                <%--<input type="file" class="file" id="upload" runat="server" style="width: auto;" />--%>
            </div>
            <a href="javascript:;" class="btn-o fl">上传</a>
        </div>
        <!--上传模板 end-->
        <div class="imBox">
            <a href="GoodsNew/UploadPic.aspx">图片批量导入>></a>
        </div>
    </div>
    <asp:Button ID="btnAddList" runat="server" Text="Button" OnClick="btnAddList_Click"
        Style="display: none;" />
    </form>
    <style>
        .imBox .btn-o2
        {
            background: #3b6dbb none repeat scroll 0 0;
            border-radius: 0;
            color: #fff;
            display: inline-block;
            height: 30px;
            line-height: 30px;
            margin: 10px 0 0 10px;
            text-align: center;
            width: 100px;
        }
    </style>
</body>
</html>

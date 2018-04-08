<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PicSpaceEdit.aspx.cs" Inherits="Company_GoodsNew_PicSpaceEdit" %>

<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>图片上传</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/ImgAmplify.js" type="text/javascript"></script>
    <script src="../../js/MoveLayer.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <link rel="stylesheet" href="http://cdn.bootcss.com/bootstrap/3.3.5/css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="../js/webuploader/webuploader.css">
    <link rel="stylesheet" type="text/css" href="../js/webuploader/style.css">
    <script type="text/javascript" src="../js/webuploader/jquery.min.js"></script>
    <script type="text/javascript" src="../js/webuploader/webuploader.min.js"></script>
    <script type="text/javascript" src="../js/webuploader/upload.js"></script>
    <style>
        .demo
        {
            min-width: 360px;
        }
        .file-item
        {
            float: left;
            position: relative;
            width: 110px;
            height: 110px;
            margin: 0 20px 20px 0;
            padding: 4px;
        }
        .file-item .info
        {
            overflow: hidden;
        }
        .rightinfo
        {
            margin-left: 120px;
            margin-top: 60px;
            padding: 8px;
        }
        .container
        {
            width: 1100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-3" />
    <uc1:CompRemove runat="server" ID="Remove" ModuleID="3" />
    <div class="rightinfo">
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a href="../GoodsNew/GoodsList.aspx" runat="server" id="atitle">商品列表</a></li><li>
                    ></li>
                <li><a href="../GoodsNew/PicSpaceList.aspx" runat="server" id="btitle">图片空间</a></li><li>
                    ></li>
                <li><a href="javascript:;" runat="server" id="A1">上传图片</a></li>
            </ul>
        </div>
        <div class="container">
            <div class="row main">
                <div class="demo">
                    <div id="uploader">
                        <div class="queueList">
                            <div id="dndArea" class="placeholder" style="min-height: 220px">
                                <div id="filePicker" >
                                </div>
                                <p>
                                    或将照片拖到这里，单次最多可选30张
                                    <br />
                                    提示：仅支持<strong>JPG、JPEG、PNG</strong>格式；<br>
                                    建议上传无线详情图片宽度<strong>750px</strong>以上，效果更佳
                                </p>
                            </div>
                        </div>
                        <div class="fanhui fanhui2" style="background: #aaaaaa  none repeat scroll 0 0; border-color: transparent;
                            color: #fff; border-radius: 3px; cursor: pointer; display: inline-block; float: right;
                            font-size: 14px; margin-right: 20px; padding: 10px 18px; height: 40px;">
                            返回列表
                        </div>
                        <div class="statusBar" style="display: none;">
                            <div class="progress">
                                <span class="text">0%</span> <span class="percentage"></span>
                            </div>
                            <div class="info">
                            </div>
                            <div class="btns">
                                <div class="fanhui" style="background: #aaaaaa  none repeat scroll 0 0; border-color: transparent;
                                    color: #fff; border-radius: 3px; cursor: pointer; display: inline-block; float: left;
                                    font-size: 14px; margin-left: 10px; padding: 0 18px;">
                                    返回列表
                                </div>
                                <div id="filePicker2">
                                </div>
                                <div class="uploadBtn">
                                    开始上传</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="g.aspx.cs" Inherits="g" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<meta name="data-spm" content="a215s"/>
<meta content="yes" name="apple-mobile-web-app-capable"/>
<meta content="yes" name="apple-touch-fullscreen"/>
<meta name="viewport" content="initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=0, width=device-width"/>
    <script src="js/jquery-1.9.1.min.js"></script>
    <script>
        //商品详情自适应图片大小
        window.onload = function () {
            $("div,td,tr,li,span,table,p").each(function (index, obj) {
                var winwidth = $(window).width() * 0.98;
                var attewidth = $(obj).width() * 1;

                var style = $(obj).attr("style");
                if (attewidth >= winwidth) {
                    var style = $(obj).attr("style");
                    if (style == undefined) {
                        $(obj).attr("style", "width:" + winwidth + "px;")
                    }
                    else {
                        $(obj).attr("style", $(obj).attr("style").replace("width:" + attewidth + "px;", ""))
                        $(obj).width("" + winwidth + "px");
                    }
                }

            })
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



            $("div,td,tr,li,span,table,p,strong").each(function (index, obj) {
                var html = $(obj).html();
                var px = $(obj).css('fontSize');
                if (px == "9px") {
                    fontsize(obj, px, 1);
                }
                if (px == "10px") {
                    fontsize(obj, px, 1);
                }
                if (px == "12px") {
                    fontsize(obj, px, 1);
                }
                if (px == "14px") {
                    fontsize(obj, px, 1.3);
                }
                if (px == "16px") {
                    fontsize(obj, px, 1.4);
                }
                if (px == "18px") {
                    fontsize(obj, px, 1.5);
                }
                if (px == "24px") {
                    fontsize(obj, px, 1.6);
                }
                if (px == "32px") {
                    fontsize(obj, px, 1.7);
                }
            })

        }
        function fontsize(obj, px, size) {
            var html = $(obj).html();
            var style = $(obj).attr("style")
            if (style==undefined)
            {
                return;
            }
            if (style == "font-size:" + px + ";") {
                $(obj).attr("style", "font-size:" + size + "em;")
            }
            else {
                $(obj).attr("style", $(obj).attr("style").replace("font-size:" + px + "px;", "font-size:" + size + "em;"))
            }
        }
    </script>
    <style>
        .img {
        width:100%;
        }
        body{
         font:normal100%Helvetica,Arial,sans-serif;
         }
        body {
            font-size:10px;
        }
    </style>
    <title>商品详情</title>
</head>
<body>
    <form id="form1" runat="server">
     <div class="blank10">
        </div>
        <div class="nr-deta">
            <div class="nr" id="DivShow" runat="server" style="width:100%;margin:0 auto;">
            </div>
        </div>
    </form>
</body>
</html>

(function ($) {
    //    显示位置：鼠标左上角
    //    var x = -410;
    //    var y = -300;
    //    显示位置：鼠标右侧
    //    var x = 10;
    //    var y = -130;
    $.fn.ImgAmplify = function () {
        var x = 10;
        var y = -180;
        return this.each(function (i) {
            $(this).on({
                "mouseover": function (e) {
                    if ($(this).children("img").attr("src") == undefined ||
                        $(this).children("img").attr("src") == "" ||
                        $(this).children("img").attr("src") == "../../images/havenopicsmallest.gif")
                    {
                        return;
                    }
                    $(this).addClass("aImg");

                    //拼装图片的url
                    var src = this.id;

                    this.myTitle = this.title;
                    this.title = "";
                    var imgTitle = this.myTitle ? "<br/>" + this.myTitle : "";
                    var tooltip = "<div id='tooltip'><img style=' max-width:450px;max-height:422px;' src='" + src + "' alt='产品预览图'/>" + imgTitle + "<\/div>"; //创建 div 元素
                    $("body").append(tooltip); //把它追加到文档中
                    
                    var Y = e.pageY + y;
                    if (Y < 40) {
                        Y = 40;
                    } else if ($(document).height() - Y < 430) {
                        Y = $(document).height()-435;
                    } 
                    $("#tooltip").css({ "top": Y + "px", "left": (e.pageX + x) + "px" }).show("fast"); //设置x坐标和y坐标，并且显示
                },
                "mouseout": function () {
                    if ($(this).children("img").attr("src") == undefined || $(this).children("img").attr("src") == ""||
                    $(this).children("img").attr("src") == "../../images/havenopicsmallest.gif") {
                        return;
                    }
                    this.title = this.myTitle;
                    $("#tooltip").remove(); //移除
                    $(this).removeClass("aImg");
                }
                ,
               
                "mousemove": function (e) {
                    if ($(this).children("img").attr("src") == undefined || $(this).children("img").attr("src") == ""||
                    $(this).children("img").attr("src") == "../../images/havenopicsmallest.gif") {
                        return;
                    }
                    
                    var YY = e.pageY + y;
                    if (YY < 38) {
                        YY = 38;
                    } else if ($(document).height() - YY < 430) {
                        YY = $(document).height() -435;
                    }
                    $("#tooltip").css({ "top": YY + "px", "left": (e.pageX + x) + "px" });
                }
            });

        });
    };


})(jQuery);
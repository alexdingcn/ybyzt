(function ($) {

    var $D = "Isopen";
    var Browser = new Object();

    Browser.isMozilla = (typeof document.implementation != 'undefined') && (typeof document.implementation.createDocument != 'undefined') && (typeof HTMLDocument != 'undefined');
    Browser.isIE = window.ActiveXObject ? true : false;
    Browser.isFirefox = (navigator.userAgent.toLowerCase().indexOf("firefox") != -1);
    Browser.isSafari = (navigator.userAgent.toLowerCase().indexOf("safari") != -1);
    Browser.isOpera = (navigator.userAgent.toLowerCase().indexOf("opera") != -1);

    $.fn.TreeViewOpen = function (option) {
        var options = $.extend({}, $.fn.TreeViewOpen.defaults, option);
        var obj = this;

        return this.each(function () {
            //不能上移的tr
            var up = $("#HiddenUp").val().split(",");
            for (var i = 0; i < up.length; i++) {
                var objup = $("a[tip=" + up[i] + "][class=TypeIndex]");
                objup.css("color", "#c0c0c0");
                objup.off("click");
                objup.css("cursor", "not-allowed");
            }
            //不能下移的tr
            var down = $("#HiddenDown").val().split(",");
            for (var i = 0; i < down.length; i++) {
                var objdown = $("a[tip=" + down[i] + "][class=TypeIndexDown]");
                objdown.css("color", "#c0c0c0");
                objdown.off("click");
                objdown.css("cursor", "not-allowed");
            }

            obj.find("tr[level=3]").find("a[class=TypeChildAdd]").removeAttr("class").css({ color: "#c0c0c0", cursor: "not-allowed" });
            obj.find("img").each(function () {
                //默认的tr打开
                if ($(this).attr("src") == "../images/menu_plus.gif") {
                    $(this).parents("tr:eq(0)").attr($D, "0");
                }
            });

            //一次性加载50个以下、全部展开，商品分类大于50个展开一级分类
            if (obj.find("tbody tr").length <= 50) {
                obj.find("tr").each(function () {
                    $(this).find("img#" + options.imgID).attr("src", "../images/menu_minus.gif");
                    $(this).attr($D, "1");
                    if ($(this).attr("level") == "2") {
                        $(this).find("img#" + options.imgID).css("margin-left", "39px");
                    } else if ($(this).attr("level") == "3") {
                        $(this).find("img#" + options.imgID).css("margin-left", "78px");
                        $(this).find("a[class=TypeChildAdd]").css("visibility", "hidden");
                    }
                });
            } else {
                obj.find("tr").each(function () {
                    if ($(this).attr("level") == "1") {
                        $(this).find("img#" + options.imgID).attr("src", "../images/menu_minus.gif");
                        $(this).attr($D, "1");
                    }else if ($(this).attr("level") == "2") {
                        $(this).attr($D, $(this).find("img#" + options.imgID).attr("src") == "../images/menu_plus.gif" ? "0" : "1");
                        $(this).find("img#" + options.imgID).css("margin-left", "39px");
                    } else if ($(this).attr("level") == "3") {
                        $(this).find("img#" + options.imgID).attr("src", "../images/menu_minus.gif");
                        $(this).find("img#" + options.imgID).css("margin-left", "78px");
                        $(this).attr($D, "1");
                        $(this).css("display", "none");
                    }
                });
            }

            $(document).on("click", "#" + this.id + " tr img#" + options.imgID, function () {
                var $P = $(this).parents("tr:eq(0)");
                var Left = $(this).css("margin-left").replace("px", "");
                var thisImg = this;
                if ($P.attr($D) == "0") { //打开子类
                    if (obj.find("tr[ParentId=" + $P.attr("id") + "]").length == 0) {
                        $.ajax({
                            type: "post",
                            url: "../../Handler/CategoryHandler.ashx",
                            data: { TableType: "1", ParentID: $P.attr("Id"), Level: $P.attr("level") },
                            dataType: "text",
                            success: function (data) {
                                if (data != "") {
                                    $(thisImg).attr("src", options.DownSrc);
                                    $P.attr($D, "1");
                                    $($P).after(data);
                                    $.each(obj.find("tr[ParentId=" + $P.attr("id") + "]"), function (index, tr) {
                                        $(tr).css("display", (Browser.isIE) ? '' : "table-row");
                                        $(tr).find("img#" + options.imgID).css("margin-left", (parseInt(Left) + 39).toString() + "px");
                                    });
                                }
                            }
                        });
                    } else {
                        $(this).attr("src", options.DownSrc);
                        $P.attr($D, "1");
                        $.each(obj.find("tr[ParentId=" + $P.attr("id") + "]"), function (index, tr) {
                            $(tr).css("display", (Browser.isIE) ? '' : "table-row");
                            $(tr).find("img#" + options.imgID).css("margin-left", (parseInt(Left) + 39).toString() + "px");
                        });
                    }
                }
                else { //关闭子类
                    if (obj.find("tr[ParentId=" + $P.attr("id") + "]").length != 0) {
                        $(this).attr("src", options.UpSrc);
                        $P.attr($D, "0");
                        $.each(obj.find("tr[ParentId=" + $P.attr("id") + "]"), function (index, tr) {
                            $(tr).hide();
                            if ($(tr).attr($D) == "1") {
                                $(tr).find(" img#" + options.imgID).trigger("click");
                            }
                        });
                    }
                }
            });
        });
    };

    $.fn.TreeViewOpen.defaults = {
        imgID: "",
        UpSrc: "",
        DownSrc: ""
    };

})(jQuery);
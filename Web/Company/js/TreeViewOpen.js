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
            $(this).find("tr").attr($D, "0");
            $(this).find("tr[ParentId=0] img#" + options.imgID).css("margin-left", "13px");
            $(this).find("img#" + options.imgID).css("cursor", "pointer");
            $("#" + this.id + " tr img#" + options.imgID).on("click", function () {
                var $P = $(this).parents("tr:eq(0)");
                if (obj.find("tr[ParentId=" + $P.attr("id") + "]").length == 0) {
                    return;
                }
                var Left = $(this).css("margin-left").replace("px", "");
                if ($P.attr($D) == "0") {
                    $(this).attr("src", options.DownSrc);
                    $P.attr($D, "1");
                    $.each(obj.find("tr[ParentId=" + $P.attr("id") + "]"), function (index, tr) {
                        $(tr).css("display", (Browser.isIE) ? '' : "table-row");
                        $(tr).find("img#" + options.imgID).css("margin-left", (parseInt(Left) + 39).toString() + "px");
                    });
                }
                else {
                    $(this).attr("src", options.UpSrc);
                    $P.attr($D, "0");
                    $.each(obj.find("tr[ParentId=" + $P.attr("id") + "]"), function (index, tr) {
                        $(tr).css("display", "none");
                        if ($(tr).attr($D) == "1") {
                            $(tr).find(" img#" + options.imgID).trigger("click");
                        }
                    });

                }
            });
        });
    }


    $.fn.TreeViewOpen.defaults = {
        imgID: "",
        UpSrc: "",
        DownSrc: ""
    }

})(jQuery);
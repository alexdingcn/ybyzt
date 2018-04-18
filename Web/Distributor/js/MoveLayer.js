(function ($) {
    var conf = {
        Ismove: false,
        IsParent: true,
        MoveWindow: function (Mwindow) {
            var lefts = parseInt(conf.SEMove.css('left'));
            var tops = parseInt(conf.SEMove.css('top'));
            if (conf.IsParent) {
                if ($(Mwindow).parent("div").length > 0) {
                    lefts = lefts - parseInt($(Mwindow).parent("div").offset().left);
                    tops = tops - parseInt($(Mwindow).parent("div").offset().top);
                }
            }
            Mwindow.css({ left: lefts, top: tops });
        }

    };
    var Winid;
    $.fn.LockMove = function (options, IsParent) {
        var timeid;
        IsParent != undefined && (conf.IsParent = IsParent);
        var option = $.extend({}, $.fn.config, options);
        $(this).data("Winid", option.MoveWindow);
        return this.each(function () {
            $(this).on("mousedown", function (M) {
                M.preventDefault();
                Winid = $(this).data("Winid");
                timeid = setTimeout(function () {
                    try {
                        conf.Ismove = true;
                        var MwinDow = $("" + Winid + "");
                        var xx = MwinDow.offset().left, yy = MwinDow.offset().top + 1, ww = MwinDow.width() - 6, hh = MwinDow.height();
                        if (!$('#SEbox_moves')[0]) {
                            $('body').append("<div id='SEbox_moves' class='SEbox_moves' style='left:" + xx + "px;top:" + yy + "px;width:" + ww + "px;height:" + hh + "px; border: 3px solid #333;cursor: move;opacity: 0.6; z-index:2147483584;position:absolute; '  ></div>");
                        }
                        conf.SEMove = $("#SEbox_moves");
                        conf.moveX = M.pageX - conf.SEMove.position().left;
                        conf.moveY = M.pageY - conf.SEMove.position().top;

                    }
                    catch (e) {
                        conf.Ismove = false;
                    }
                }, 150);

            });

            $(document).on("mousemove", function (M) {
                M.preventDefault();
                if (conf.Ismove) {
                    var offsetX = M.pageX - conf.moveX, offsetY = M.pageY - conf.moveY;

                    if (true) {
                        var win = $(window);
                        conf.setY = win.scrollTop();
                        var setRig = win.width() - conf.SEMove.outerWidth() - 5, setTop = 5 + conf.setY;
                        offsetX < 5 && (offsetX = 5);
                        offsetX > setRig && (offsetX = setRig);
                        offsetY < setTop && (offsetY = setTop);
                        offsetY > win.height() - conf.SEMove.outerHeight() - 5 + conf.setY && (offsetY = win.height() - conf.SEMove.outerHeight() - 5 + conf.setY);
                    }

                    conf.SEMove.css({ left: offsetX, top: offsetY });
                    offsetX = null;
                    offsetY = null;
                }
            }).on("mouseup", function () {
                try {
                    if (conf.Ismove) {
                        conf.MoveWindow($("" + Winid + ""));
                        conf.SEMove.remove();
                        conf.Ismove = false;
                    }
                } catch (e) {
                    conf.Ismove = false;
                }
                clearTimeout(timeid);
            });



        })



    }

    $.fn.config = {
        MoveWindow: ""
    }

})(jQuery);
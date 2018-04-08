(function ($) {

    $.fn.InputSearchData = function (options) {
        var ua = navigator.userAgent.toLowerCase(); //浏览器信息
        var info = {
            ie: /msie/.test(ua) && !/opera/.test(ua),        //匹配IE浏览器    
            op: /opera/.test(ua),     //匹配Opera浏览器    
            sa: /version.*safari/.test(ua),     //匹配Safari浏览器    
            ch: /chrome/.test(ua),     //匹配Chrome浏览器    
            ff: /gecko/.test(ua) && !/webkit/.test(ua)     //匹配Firefox浏览器
        };
        var option = $.extend({}, $.fn.Deaultconfig, options);
        var IsSetInput = false;
        var keycode = 0;
        return this.each(function () {
            var elment = this;
            $(document.body).click(function (e) {
                var xx = e.pageX; //鼠标X轴
                var yy = e.pageY; //鼠标Y轴
                var x = parseInt($(elment).offset().left); //文本框坐标
                var y = parseInt($(elment).offset().top) + 25; //文本框坐标
                if (xx > x && xx < x + $(elment)[0].clientWidth && yy + $(elment)[0].clientHeight > y && yy < y) {
                    //  alert(x + "," + xx + ";;" + y + "," + yy)
                } else {
                    if ($(".pullDown2").is(":visible")) {
                        $(".pullDown2").hide();
                        $(".pullDown2").removeClass("xy");
                    }
                }
            });


            $(this).bind({ "input propertychange focus": function (e) {
                if (IsSetInput || (e.type == "propertychange" && keycode != 8 && $(this).val() == "")) {
                    return;
                }
                if (option.callBackData) {
                    if (e.type != "focus") {
                        ShowDIV.empty();
                        ShowDIV.remove();
                    } else {
                        ShowDIV.empty();
                    }
                    ShowUL.empty();
                    option.callBackData($(this).val(), function (json) {
                        if ($("div.ShowSearchData").length == 0) {
                        } else {
                            ShowDIV = $("div.ShowSearchData");
                        }
                        ShowDIV.css("width", $(elment)[0].clientWidth);
                        //显示的div定位 start
                        if ($(elment).offsetParent().length > 0) {
                            ShowDIV.css({ left: "" + ($(elment).offset().left - $(elment).offsetParent().offset().left).toFixed(0) + "px", top: "" + ($(elment).offset().top - $(elment).offsetParent().offset().top + $(elment).height() + option.AddShowHeight).toFixed(0) + "px" });
                        }
                        else {
                            ShowDIV.css({ left: "" + ($(elment).offset().left).toFixed(0) + "px", top: "" + ($(elment).offset().top + $(elment).height()).toFixed(0) + "px" });
                        }
                        //显示的div定位 end
                        ShowUL.css("max-height", option.ShowMaxHeight).empty();
                        $.each(json, function (index, data) {
                            var Showli = $("<li></li>").append($("<a href='javascript:;'>" + data[option.JsonName] + (option.JsonType != null ? "【" + data[option.JsonType] + "】" : "") + "</a>")).data("Vtext", data[option.JsonName]);
                            if (option.JsonID) {
                                Showli.data("Vid", data[option.JsonID]);
                            }
                            Showli.appendTo(ShowUL);
                        });
                        ShowDIV.append(ShowUL).css("display", "block").insertAfter(elment);
                        $(".ShowSearchData .list li").click(function (M) {
                            IsSetInput = true;
                            $(elment).val($(this).data("Vtext"));
                            if (option.JsonID) {
                                $("#" + option.SetValueID + "").val($(this).data("Vid"));
                            }
                            IsSetInput = false;
                        });
                        return false;
                    });
                }
            }, "blur": function () {
                if (option.IsReset) {
                    var IsFind = false;
                    var FindA = undefined;
                    var Itext = this;
                    $.each(ShowUL.find("li"), function (index, obj) {
                        if ($(obj).data("Vtext") == $(Itext).val()) {
                            IsFind = true;
                            FindA = obj;
                        }
                    })
                    if (IsFind) {
                        $("#" + option.SetValueID + "").val($(FindA).data("Vid"));
                    }
                    else {
                        IsSetInput = true;
                        $(this).val("");
                        IsSetInput = false;
                        $("#" + option.SetValueID + "").val("");
                    }
                }
            }, "keydown": function (event) {
                if (window.event) {
                    keycode = window.event.keyCode;
                } else {
                    keycode = event.which;
                }
            }
            });
        });
    }
    var ShowDIV = $("<div class=\"pullDown2 ShowSearchData\"></div>");
    var ShowUL = $("<ul class=\"list\" style='overflow-y:scroll'></ul>");
    $.fn.Deaultconfig =
    {
        callBackData: function (value, callback) { },
        SetValueID: "HidSM",
        JsonName: "name",
        JsonID: undefined,
        IsReset: false,
        ShowMaxHeight: "150px",
        JsonType: null,
        AddShowHeight: 0
    }
})(jQuery);
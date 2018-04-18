/*
* CitysLinkage 1.0.0.0 
* Copyright (c) 2009 ClickCai www.ahszcc.com 
* Date: 2014-11-21
* 使用CitysLinkage 达到省市区联动的效果
* 调用方法
* $("[ssq]").CitysLinkage({
*     ddlP: "#ddlP",//省控件
*     ddlC: "#ddlC",//市控件
*     ddlD: "#ddlD",//区空间
*     path: "content/",//内容库相对路径（ps：针对当前使用页面）
*     ACode: "152522"//获得的code
* });
*
*/

(function ($) {
    var o = { "min": "max" };
    $.fn.CitysLinkage = function (options) {
        var opts = $.extend({}, $.fn.CitysLinkage.defaults, options);
        return this.each(function (i) {
            var $this = $(this);
            var op = $.meta ? $.extend({}, opts, $this.data()) : opts;
            if (typeof ($(op.ddlP)[0]) == "undefined" || typeof ($(op.ddlC)[0]) == "undefined" || typeof ($(op.ddlD)[0]) == "undefined") {
                return;
            }
            $(op.ddlP).attr("range", op.range.toString());
            $(op.ddlC).attr("range", op.range.toString());
            $(op.ddlD).attr("range", op.range.toString());
            var name = op.range.toString();
            o[name] = op;

            if (typeof (provinces) == "undefined")
                $("<script type = 'text/javascript' src='" + o[name].path + "json-array-of-province.js'><\/script>").appendTo("head");
            $.each(provinces, function () {
                $this.find(o[name].ddlP).append("<p tet=\"" + this.name + "\" value=\"" + this.code + "\" >" + this.name + "</p>")
            });
            if ($.trim(o[name].ACode[i]).length == 6) {
                var pCode = $.trim(o[name].ACode[i]).substring(0, 2);
                var cCode = $.trim(o[name].ACode[i]).substring(2, 4);
                var dCode = $.trim(o[name].ACode[i]).substring(4, 6);
                if ($.trim(pCode).length == 2 && $.trim(pCode) != "00") {
                    $this.find(o[name].ddlP).val($.trim(pCode) + "0000");
                    $.fn.CitysLinkage.BindCity($this, $.trim(pCode) + "0000", name)
                }
                if ($.trim(cCode).length == 2 && $.trim(cCode) != "00") {
                    $this.find(o[name].ddlC).val($.trim(pCode) + $.trim(cCode) + "00");
                    $.fn.CitysLinkage.BindDistribute($this, $.trim(pCode) + $.trim(cCode) + "00", name)
                }
                if ($.trim(dCode).length == 2 && $.trim(dCode) != "00") {
                    $this.find(o[name].ddlD).val($.trim(o.ACode[i]))
                }
            }
            $(o[name].ddlP).find("p").on("click", function () {
                var name = $(this).parents().attr("range");
                if (name == "undefined")
                    return;
                $(o[name].txtAddr).val("");
                $this.find(o[name].ddlC).siblings("li").find("input[type='button']").val("");
                $this.find(o[name].ddlD).siblings("li").find("input[type='button']").val("");
                $this.find(o[name].ddlC).empty();
                $this.find(o[name].ddlD).empty();

                if ($.trim($(this).parents().val()).length > 0) {
                    if (typeof (citys) == "undefined")
                        $("<script type = 'text/javascript' src='" + o[name].path + "json-array-of-city.js'><\/script>").appendTo("head");

                    var pcCode = $.trim($(this).attr("value"));
                    for (var i = 0; i < citys.length; i++) {
                        if ($.trim(citys[i].code).substring(0, 2) == pcCode.substring(0, 2))
                            $this.find(o[name].ddlC).append("<p tet=\"" + citys[i].name + "\" value=\"" + citys[i].code + "\" >" + citys[i].name + "</p>")
                    }

                    $_Area();
                }
            });
            $_Area = function () {
                $(o[name].ddlC).find("p").on("click", function () {
                    var name = $(this).parents().attr("range");
                    if (name == "undefined")
                        return;
                    $(o[name].txtAddr).val("");
                    $this.find(o[name].ddlD).siblings("li").find("input[type='button']").val("");
                    $this.find(o[name].ddlD).empty();

                    if ($.trim($(this).parents().val()).length > 0) {
                        if (typeof (districts) == "undefined")
                            $("<script type='text/javascript' src='" + o[name].path + "json-array-of-district.js'><\/script>").appendTo("head");

                        var ccCode = $.trim($(this).attr("value"));
                        for (var i = 0; i < districts.length; i++) {
                            if ($.trim(districts[i].code).substring(0, 4) == ccCode.substring(0, 4))
                                $this.find(o[name].ddlD).append("<p tet=\"" + districts[i].name + "\" value=\"" + districts[i].code + "\" >" + districts[i].name + "</p>")
                        }

                    }
                    $_AddAddr();
                });
            }

            //选择时，赋值
            $_AddAddr = function () {
                $(o[name].ddlD).find("p").on("click", function () {
                    var dArea = $(o[name].ddlD).find("p[class='hover']").text();
                    var p = $(o[name].txtP).val();
                    var c = $(o[name].txtC).val();
                    var Address = p;
                    if (p.toString() == "北京市" || p.toString() == "上海市" || p.toString() == "天津市" || p.toString() == "重庆市") {
                        Address += dArea;
                    } else {
                        Address += c + dArea;
                    }
                    $(o[name].txtAddr).val(Address);
                });
            }

            //伪下拉地址赋值时使用
            $_Addr = function (Province, City, Area, Address) {
                $(o[name].ddlP).children("p[tet='" + Province + "']").trigger("click");
                $(o[name].ddlC).children("p[tet='" + City + "']").trigger("click");

                $(o[name].txtP).val(Province);
                $(o[name].txtC).val(City);
                $(o[name].txtD).val(Area);
                $(o[name].txtAddr).val(Address);
            }
        })
    };

    function debug($obj) {
        if (window.console && window.console.log) window.console.log('hilight selection count: ' + $obj.size())
    };
    $.fn.CitysLinkage.BindCity = function ($this, Pval, Name) {
        if (Name == "undefined")
            return;
        $this.find(o[Name].ddlD).empty();
        $this.find(o[Name].ddlC).empty();
        if ($.trim(Pval).length > 0) {
            if (typeof (citys) == "undefined")
                $("<script type = 'text/javascript' src='" + o[Name].path + "json-array-of-city.js'><\/script>").appendTo("head");
            var pCode = $.trim(Pval);
            $.each(citys, function () {
                if ($.trim(this.code).substring(0, 2) == pCode.substring(0, 2))
                    $this.find(o[Name].ddlC).append("<p tet=\"" + this.name + "\" value=\"" + this.code + "\" >" + this.name + "</p>")
            })
        }
    };
    $.fn.CitysLinkage.BindDistribute = function ($this, Cval, Name) {
        if (Name == "undefined")
            return;
        $this.find(o[Name].ddlD).empty();
        if ($.trim(Cval).length > 0) {
            if (typeof (districts) == "undefined")
                $("<script type='text/javascript' src='" + o[Name].path + "json-array-of-district.js'><\/script>").appendTo("head");
            var cCode = $.trim(Cval); $.each(districts, function () {
                if ($.trim(this.code).substring(0, 4) == cCode.substring(0, 4))
                    $this.find(o[Name].ddlD).append("<p tet=\"" + this.name + "\" value=\"" + this.code + "\" >" + this.name + "</p>")
            })
        }
    };
    $.fn.CitysLinkage.defaults = {
        ddlP: '#ddlP', ddlC: '#ddlC', ddlD: '#ddlD', path: "content/", txtP: '#txtP', txtC: '#txtC', txtD: '#txtD', txtAddr: '#txtAddr', ACode: [], range: "reg"
    }
})(jQuery);



$(function () {

    //省市区选择器 容器
    $("#form1").CitysLinkage({
        ddlP: ".prov", //省选择器
        ddlC: ".city", //市选择器
        ddlD: ".dist", //区选择器
        path: "../../js/CitysLine/", //插件相对当前页面路径
        txtP: "#txtProvince",
        txtC: "#txtCity",
        txtD: "#txtArea",
        txtAddr: "#txtAddress",
        ACode: [""],  //你获取到的cityCode，只需要把code写在这就好，插件内会自动判断
        range: "self"
    });
});
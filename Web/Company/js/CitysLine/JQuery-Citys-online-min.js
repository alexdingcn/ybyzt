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
            $this.find(o[name].ddlP).append("<option value=\" \" >" + "选择省" + "</option>");
            $this.find(o[name].ddlC).append("<option value=\"\" >" + "选择市" + "</option>");
            $this.find(o[name].ddlD).append("<option value=\"\" >" + "选择区" + "</option>");
            if (typeof (provinces) == "undefined")
                $("<script type = 'text/javascript' src='" + o[name].path + "json-array-of-province.js'><\/script>").appendTo("head");
            $.each(provinces, function () {
                $this.find(o[name].ddlP).append("<option value=\"" + this.code + "\" >" + this.name + "</option>")
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
            $(o[name].ddlP).on("change", function () {
                var name = $(this).attr("range");
                if (name == "undefined")
                    return;
                $this.find(o[name].ddlC).empty();
                $this.find(o[name].ddlD).empty();
                if ($.trim($(this).val()).length > 0) {
                    if (typeof (citys) == "undefined")
                        $("<script type = 'text/javascript' src='" + o[name].path + "json-array-of-city.js'><\/script>").appendTo("head");
                    $this.find(o[name].ddlC).append("<option value=\"\" >选择市</option>");
                    $this.find(o[name].ddlD).append("<option value=\"\" >选择区</option>");
                    var pcCode = $.trim($(this).val());
                    $.each(citys, function () {
                        if ($.trim(this.code).substring(0, 2) == pcCode.substring(0, 2))
                            $this.find(o[name].ddlC).append("<option value=\"" + this.code + "\" >" + this.name + "</option>")
                    })
                } 
                //else {
                //    $this.find(o[name].ddlC).append("<option value=\"\" >请选择市</option>");
                //    $this.find(o[name].ddlD).append("<option value=\"\" >" + "-请选择区" + "</option>");                
                // }
            });
            $(o[name].ddlC).on("change", function () {
                var name = $(this).attr("range");
                if (name == "undefined")
                    return;
                $this.find(o[name].ddlD).empty();
                if ($.trim($(this).val()).length > 0) {
                    if (typeof (districts) == "undefined")
                        $("<script type='text/javascript' src='" + o[name].path + "json-array-of-district.js'><\/script>").appendTo("head");
                    $this.find(o[name].ddlD).append("<option value=\"\" >选择区</option>");
                    var ccCode = $.trim($(this).val());
                    $.each(districts, function () {
                        if ($.trim(this.code).substring(0, 4) == ccCode.substring(0, 4))
                            $this.find(o[name].ddlD).append("<option value=\"" + this.code + "\" >" + this.name + "</option>")
                    })
                }
            })
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
            $this.find(o[Name].ddlC).append("<option value=\"\" >===请选择市===</option>");
            var pCode = $.trim(Pval);
            $.each(citys, function () {
                if ($.trim(this.code).substring(0, 2) == pCode.substring(0, 2))
                    $this.find(o[Name].ddlC).append("<option value=\"" + this.code + "\" >" + this.name + "</option>")
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
            $this.find(o[Name].ddlD).append("<option value=\"\" >===请选择区===</option>");
            var cCode = $.trim(Cval); $.each(districts, function () {
                if ($.trim(this.code).substring(0, 4) == cCode.substring(0, 4))
                    $this.find(o[Name].ddlD).append("<option value=\"" + this.code + "\" >" + this.name + "</option>")
            })
        }
    };
    $.fn.CitysLinkage.defaults = {
        ddlP: '#ddlP', ddlC: '#ddlC', ddlD: '#ddlD', path: "content/", ACode: [], range: "reg"
    }
})(jQuery);

(function (JQ) {
    JQ.fn.addFavorite = function (sURL, sTitle) {

        return this.each(function (i) {
            JQ(this).click(function () {
                AddFavorite(sURL, sTitle);
            })
        })

    }

    function AddFavorite(sURL, sTitle) {
        try {
            window.external.addFavorite(sURL, sTitle);
        }
        catch (e) {
            try {
                window.sidebar.addPanel(sTitle, sURL, "");
            }
            catch (e) {
                alert("加入收藏失败，请使用Ctrl+D进行添加");
            }
        }
    }
})(jQuery);



$(function () {

    //省市区选择器 容器
    $("#form1").CitysLinkage({
        ddlP: ".prov", //省选择器
        ddlC: ".city", //市选择器
        ddlD: ".dist", //区选择器
        path: "../js/CitysLine/", //插件相对当前页面路径
        ACode: [""],  //你获取到的cityCode，只需要把code写在这就好，插件内会自动判断
        range: "self"
    });

    var myprovince = $("#hidProvince").val();
    var mycity = $("#hidCity").val();
    var mydistrict = $("#hidArea").val();
    //省份下拉选中
    Selected(".prov", myprovince);
    //市下拉选中
    Selected(".city", mycity);
    //区县下拉选中     
    Selected(".dist", mydistrict);


    $(".conInfo").CitysLinkage({
        ddlP: ".prov1", //省选择器
        ddlC: ".city1", //市选择器
        ddlD: ".dist1", //区选择器
        path: "js/CitysLine/", //插件相对当前页面路径
        ACode: [""],  //你获取到的cityCode，只需要把code写在这就好，插件内会自动判断
        range: "self"
    });

    var myprovince = $("#hidProvince1").val();
    var mycity = $("#hidCity1").val();
    var mydistrict = $("#hidArea1").val();
    //省份下拉选中
    Selected(".prov1", myprovince);
    //市下拉选中
    Selected(".city1", mycity);
    //区县下拉选中     
    Selected(".dist1", mydistrict);
});
//count  下拉数量  classid  下拉class  text 需要选中的值
function Selected(classid, text) {
    if (text != undefined && classid != undefined) {
        if ($(classid + " option:contains(" + text + ")").length > 0) {
            $(classid + " option:contains(" + text + ")")[0].selected = true;
            $(classid).trigger("change")
        }
    }
}

function Change() {
    var provchange = $(".prov option:selected").text();
    var citychange = $(".city option:selected").text();
    var distchange = $(".dist option:selected").text();
    var distchange2 = $(".dist option:selected").val();
    $("#hidProvince").val(provchange);
    $("#hidCity").val(citychange);
    $("#hidArea").val(distchange);
    $("#hidCode").val(distchange2);
};


function Change1() {
    var provchange = $(".prov1 option:selected").text();
    var citychange = $(".city1 option:selected").text();
    var distchange = $(".dist1 option:selected").text();
    var distchange2 = $(".dist1 option:selected").val();
    $("#hidProvince1").val(provchange);
    $("#hidCity1").val(citychange);
    $("#hidArea1").val(distchange);
    $("#hidCode1").val(distchange2);
};

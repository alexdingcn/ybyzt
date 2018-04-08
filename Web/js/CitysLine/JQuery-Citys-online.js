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
// 创建一个闭包
(function ($) {
    //全局变量
    var $P;
    var $C;
    var $D;
    var o;
    // 插件的定义    
    $.fn.CitysLinkage = function (options) {
        // build main options before element iteration    
        var opts = $.extend({}, $.fn.CitysLinkage.defaults, options);
        // iterate and reformat each matched element    
        return this.each(function () {
            var $this = $(this);
            // build element specific options    
            o = $.meta ? $.extend({}, opts, $this.data()) : opts;
            $P = $this.find(o.ddlP);
            $C = $this.find(o.ddlC);
            $D = $this.find(o.ddlD);
            // update element styles    
            //$this.css({
            //    backgroundColor: o.background,
            //    color: o.foreground
            //});
            //var markup = $this.html();
            //// call our format function    
            //markup = $.fn.CitysLinkage.format(markup);
            //$this.html(markup);
            $P.append("<option value=\" \" >" + "===请选择省份===" + "</option>"); //.appendTo(); //(o.ddlP);
            if (typeof (provinces) == "undefined")
                $("<script type = 'text/javascript' src='" + o.path + "json-array-of-province.js'><\/script>").appendTo("head");
            $.each(provinces, function () {
                $P.append("<option value=\"" + this.code + "\" >" + this.name + "</option>");
            });

            if ($.trim(o.ACode).length == 6) {
                var pCode = $.trim(o.ACode).substring(0, 2);
                var cCode = $.trim(o.ACode).substring(2, 4);
                var dCode = $.trim(o.ACode).substring(4, 6);
                if ($.trim(pCode).length == 2 && $.trim(pCode) != "00") {
                    $P.val($.trim(pCode) + "0000");
                    $.fn.CitysLinkage.BindCity($.trim(pCode) + "0000");
                }
                if ($.trim(cCode).length == 2 && $.trim(cCode) != "00") {
                    $C.val($.trim(pCode) + $.trim(cCode) + "00");
                    $.fn.CitysLinkage.BindDistribute($.trim(pCode) + $.trim(cCode) + "00");
                }
                if ($.trim(dCode).length == 2 && $.trim(dCode) != "00") {
                    $D.val($.trim(o.ACode));
                }
            }

            $P.on("change", function () {
                //清空 市信息
                $C.empty();
                //清空区信息
                $D.empty();
                if ($.trim($(this).val()).length > 0) {
                    //更新 市 信息
                    if (typeof (citys) == "undefined")
                        $("<script type = 'text/javascript' src='" + o.path + "json-array-of-city.js'><\/script>").appendTo("head");
                    $C.append("<option value=\"\" >===请选择市===</option>");
                    var pcCode = $.trim($(this).val());
                    $.each(citys, function () {
                        if ($.trim(this.code).substring(0, 2) == pcCode.substring(0, 2))
                            $C.append("<option value=\"" + this.code + "\" >" + this.name + "</option>");
                    });
                }
            });
            $C.on("change", function () {
                //清空 区 信息
                $D.empty();
                if ($.trim($(this).val()).length > 0) {
                    //更新 区 信息
                    if (typeof (districts) == "undefined")
                        $("<script type='text/javascript' src='" + o.path + "json-array-of-district.js'><\/script>").appendTo("head");
                    $D.append("<option value=\"\" >===请选择区===</option>");
                    var ccCode = $.trim($(this).val());
                    $.each(districts, function () {
                        if ($.trim(this.code).substring(0, 4) == ccCode.substring(0, 4))
                            $D.append("<option value=\"" + this.code + "\" >" + this.name + "</option>");
                    });
                }
            });
        });
    };
    // 私有函数：debugging    
    function debug($obj) {
        if (window.console && window.console.log)
            window.console.log('hilight selection count: ' + $obj.size());
    };
    // 定义暴露format函数    
    //$.fn.CitysLinkage.format = function (txt) {
    //    return '<strong>' + txt + '</strong>';
    //};
    $.fn.CitysLinkage.BindCity = function (Pval) {
        $D.empty();
        $C.empty();
        if ($.trim(Pval).length > 0) {
            //更新 市 信息
            if (typeof (citys) == "undefined")
                $("<script type = 'text/javascript' src='" + o.path + "json-array-of-city.js'><\/script>").appendTo("head");
            $C.append("<option value=\"\" >===请选择市===</option>");
            var pCode = $.trim(Pval);
            $.each(citys, function () {
                if ($.trim(this.code).substring(0, 2) == pCode.substring(0, 2))
                    $C.append("<option value=\"" + this.code + "\" >" + this.name + "</option>");
            });
        }
    };
    $.fn.CitysLinkage.BindDistribute = function (Cval) {
        $D.empty();
        if ($.trim(Cval).length > 0) {
            //更新 区 信息
            if (typeof (districts) == "undefined")
                $("<script type='text/javascript' src='" + o.path + "json-array-of-district.js'><\/script>").appendTo("head");
            $D.append("<option value=\"\" >===请选择区===</option>");
            var cCode = $.trim(Cval);
            $.each(districts, function () {
                if ($.trim(this.code).substring(0, 4) == cCode.substring(0, 4))
                    $D.append("<option value=\"" + this.code + "\" >" + this.name + "</option>");
            });
        }
    };
    // 插件的defaults    
    $.fn.CitysLinkage.defaults = {
        ddlP: '#ddlP',
        ddlC: '#ddlC',
        ddlD: '#ddlD',
        path: "content/",
        ACode: ""
    };
    // 闭包结束    
})(jQuery);
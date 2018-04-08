(function ($) {
    $defaults = { ID: "myChart", title: "", xData: [], ItemData: [], MaxData: 1000, xNumber: 6, yNumber: 6 }
    $.fn.$myChartFun = function (options) {
        var $op = $.extend({}, $defaults, options);

        var dom = document.getElementById($op.ID);
        var myChart = echarts.init(dom);
        myChart.clear();

        option = {
            title: {
                show: false,
                text: $op.title,
                left: 'center',
                subtext: ''
            },
            tooltip: {
                trigger: 'axis'
            },
            grid: {
                /*height:'400',*/
                top: '30',
                bottom: '20'
            },
            toolbox: {
                show: true,
                orient: 'vertical',
                itemGap: 18,
                right: '60',
                feature: {
                    /*dataZoom: { show:false },
                    dataView: { readOnly: false },*/
                    magicType: { type: ['line', 'bar'] },
                    restore: {},
                    saveAsImage: {}
                }
            },
            xAxis: {
                type: 'category',
                boundaryGap: false,
                splitNumber: $op.xNumber,
                axisLine: {
                    show: true,
                    onZero: false,
                    lineStyle: {
                        color: '#ccc'
                    }
                },
                axisTick: {
                    show: true,
                    lineStyle: {
                        color: '#ccc'
                    }
                },
                splitLine: {
                    show: false,
                    lineStyle: {
                        color: ['#eee']
                    }
                },
                axisLabel: {
                    show: true,
                    textStyle: {
                        color: '#666',
                        fontStyle: 'normal',
                        fontWeight: 0,
                        fontFamily: '微软雅黑'
                    },
                    //X轴刻度配置
                    interval: 'auto' //0：表示全部显示不间隔；auto:表示自动根据刻度个数和宽度自动设置间隔个数
                },
                data: $op.xData
            },
            yAxis: {
                type: 'value',
                splitNumber: $op.yNumber,
                max: function () {
                    Math.ceil(parseFloat($op.MaxData));
                } ()
                , /*'dataMax',*/
                min: 'dataMin',
                /*interval: parseInt(parseFloat($op.MaxData)/parseFloat($op.yNumber)),*/
                axisLine: {
                    show: false,
                    onZero: false,
                    lineStyle: {
                        color: '#ccc'
                    }
                },
                axisTick: {
                    show: false,
                    lineStyle: {
                        color: '#ccc'
                    }
                },
                splitLine: {
                    show: true,
                    lineStyle: {
                        color: ['#eee'],
                        type: 'solid'
                    }
                },
                axisLabel: {
                    show: true,
                    interval: 'auto',
                    textStyle: {
                        color: '#666',
                        fontStyle: 'normal',
                        fontWeight: 0,
                        fontFamily: '微软雅黑'
                    },
                    formatter: '￥ {value}'
                }
            },
            series: [{
                name: '订单金额',
                type: 'line',
                itemStyle: {
                    normal: {
                        color: '#729bdb',
                        opacity: 1,
                        borderColor: '#729bdb',
                        borderWidth: 3,
                        shadowBlur: {
                            shadowColor: '#729bdb',
                            shadowBlur: 10
                        },
                        shadowColor: '#729bdb',
                        shadowOffsetX: 0,
                        shadowOffsetY: 0
                    }
                },
                data: $op.ItemData
            }]
        };
        myChart.setOption(option, true);
    }
})(jQuery);

$(function () {
    //首次加载当月分析
    $.ajax({
        type: 'post',
        url: '../Handler/OrderChart.ashx?ActionType=Month',
        async: true, //false:同步 true: 异步
        dataType: 'json',
        success: function (data) {
            data = eval("(" + data + ")");
            if (data.result == "true") {
                var MaxData = data.MaxData;
                $.fn.$myChartFun({ ID: "myChart", title: "", xData: eval(data.day), ItemData: eval(data.yData), MaxData: MaxData, xNumber: 10, yNumber: 5 });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert("请求错误或超时,请重试");
        }
    });

    //简报 、待处理、商品信息
    $.ajax({
        type: 'post',
        url: '../Handler/jsc.ashx?ActionType=Count',
        async: true, //false:同步 true: 异步
        dataType: 'json',
        success: function (data) {
            if (data.Result) {
                //待审核
                $("#NotCount").html(data.NotCount);
                $("#NotCount").attr("href", "Order/OrderCreateList.aspx?type=1");

                //待发货
                $("#DeliveryCount").html(data.DeliveryCount);
                $("#DeliveryCount").attr("href", "Order/OrderCreateList.aspx?type=3");

                //合作待审
                $("#ReturnCount").html(data.ReturnCount);
                $("#ReturnCount").attr("href", "CMerchants/FirstCampList.aspx?type=1");

                //证件到期
                $("#disCount").html(data.disCount);
                $("#disCount").attr("href", "SysManager/DisFCmaterialsList.aspx");

                //当日订单数
                $("#DayOrderCount").html(data.DayOrderCount);
                //当日收款金额
                $("#dayPaggerSum").html(data.dayPaggerSum);
                //当日订单金额
                $("#DaySum").html(data.DaySum);

                //本月订单数
                $("#OrderCount").html(data.OrderCount);
                //本月订单金额
                $("#MonthSum").html(data.MonthSum);
                //本月收款金额
                $("#paggerSum").html(data.paggerSum);

                //本周订单数
                $("#WeekOrderCount").html(data.WeekOrderCount);
                //本周订单金额
                $("#WeekSum").html(data.WeekSum);
                //本周收款金额
                $("#WeekPaggerSum").html(data.WeekPaggerSum);

                //当日退货信息
                $("#ReturnCountDay").html(data.ReturnCountDay);
                $("#ReturnMoneyDay").html(data.ReturnMoneyDay);
                //本周退货信息
                $("#ReturnCountWeek").html(data.ReturnCountWeek);
                $("#ReturnMoneyWeek").html(data.ReturnMoneyWeek);
                //当日退货信息
                $("#ReturnCountMonth").html(data.ReturnCountMonth);
                $("#ReturnMoneyMonth").html(data.ReturnMoneyMonth);

                //招商
                $("#DayCMCount").html(data.DayCMCount);
                $("#WeekCMCount").html(data.WeekCMCount);
                $("#MonthCMCount").html(data.MonthCMCount);

                //已上架商品
                $("#IsOffLineOk").html(data.IsOffLineOk);
                $("#IsOffLineOk").attr("href", "GoodsNew/GoodsList.aspx?isoffline=1");
                //已下架商品
                $("#IsOffLineNO").html(data.IsOffLineNO);
                $("#IsOffLineNO").attr("href", "GoodsNew/GoodsList.aspx?isoffline=0");
                //促销商品数
                //$("#proCount").html(data.proCount);
                //未读留言
                $("#shopmsgCount").html(data.shopmsgCount);
                $("#shopmsgCount").attr("href", "ShopManager/ShopMessage.aspx?isread=0");



            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert("请求错误或超时,请重试");
        }
    });

    $(".xx li").on("click", function () {
        $(this).siblings("li").removeClass("hover");
        $(this).addClass("hover");
        var tip = $(this).attr("tip");

        var DEFAULT_VERSION = "8.0";
        var ua = navigator.userAgent.toLowerCase();
        var isIE = ua.indexOf("msie") > -1;
        var safariVersion;
        if (isIE) {
            safariVersion = ua.match(/msie ([\d.]+)/)[1];
            if (safariVersion <= DEFAULT_VERSION) {
                $("#myChart").children().remove("div");
            }
        }

        $.ajax({
            type: 'post',
            url: '../Handler/OrderChart.ashx?ActionType=' + tip,
            async: true, //false:同步 true: 异步
            dataType: 'json',
            success: function (data) {
                data = eval("(" + data + ")");
                if (data.result == "true") {
                    var MaxData = data.MaxData;

                    $.fn.$myChartFun({ ID: "myChart", title: "", xData: eval(data.day), ItemData: eval(data.yData), MaxData: MaxData, xNumber: 10, yNumber: 5 });

                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //alert("请求错误或超时,请重试");
            }
        });

    });


}); 
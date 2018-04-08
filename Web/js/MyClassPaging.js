/*!
* myPagination Jquery Pagination Plug-in Library v4.0
* Date: 2011/7/18 19:47
* data:2012/03/06  by liz
2 当前页码为1时 首页不可用
3 当前页码为最后一页时 尾页不可用
4 添加 ajaxStop 方法
*/
(function ($) {
    $.fn.myPagination = function (param) {
        init(param, $(this));
        return $(this)
    };
    function init(param, obj) {
        if (param && param instanceof Object) {
            var options;
            var currPage;
            var pageCount;
            var pageSize;
            var btnsize = 5;
            var MyPaginCssSrc = "/css/PageStyle.css";
            var PageResourceCount = 0;
            var tempPage;
            var RequstParam;
            var defaults = new Object({
                currPage: 1,
                pageCount: 10,
                pageSize: 5,
                IsShowOnePaging: false,
                cssStyle: 'badoo',
                ajax: {
                    on: false,
                    pageCountId: 'pageCount',
                    param: {
                        on: false,
                        page: 1,
                        pagesize: 10
                    },
                    ajaxStart: function () {
                        return false
                    },
                    ajaxStop: function () {
                        return false;
                    }
                },
                info: {
                    first: '首页',
                    last: '尾页',
                    next: '下一页',
                    prev: '上一页',
                    first_on: true,
                    last_on: true,
                    next_on: true,
                    prev_on: true,
                    msg_on: true,
                    link: location.href, // + '&#goodsAttr',
                    msg: '<span>&nbsp;&nbsp;跳{currText}/{sumPage}页</span>',
                    text: {
                        width: '22px'
                    }
                }
            });
            function getCurrPage() {
                if (options.info && options.info.cookie_currPageKey && options.info.cookie_currPage) {
                    var cookie_currPage = $.cookie(options.info.cookie_currPageKey + "_currPage");
                    if (cookie_currPage != "" && cookie_currPage != null) {
                        return cookie_currPage
                    }
                }
                if (options.currPage) {
                    return options.currPage
                } else {
                    return defaults.currPage
                }
            }
            function getPageCount() {
                if (options.pageCount) {
                    return options.pageCount
                } else {
                    return defaults.pageCount
                }
            }
            function getPageSize() {
                if (options.pageSize) {
                    return options.pageSize
                } else {
                    return defaults.pageSize
                }
            }
            function getCssStyle() {
                if (options.cssStyle) {
                    return options.cssStyle
                } else {
                    return defaults.cssStyle
                }
            }
            function getAjax() {
                if (options.ajax && options.ajax.on) {
                    return options.ajax
                } else {
                    return defaults.ajax
                }
            }
            function getParam() {
                if (options.ajax.param && options.ajax.param.on) {
                    options.ajax.param.page = currPage;
                    return options.ajax.param
                } else {
                    defaults.ajax.param.page = currPage;
                    defaults.ajax.param.pagesize = pageSize;
                    return defaults.ajax.param
                }
            }

            function getPrev() {
                if (options.info && options.info.prev_on == false) {
                    return ""
                }
                if (options.info && options.info.prev) {
                    return options.info.prev
                } else {
                    return defaults.info.prev
                }
            }
            function getNext() {
                if (options.info && options.info.next_on == false) {
                    return ""
                }
                if (options.info && options.info.next) {
                    return options.info.next
                } else {
                    return defaults.info.next
                }
            }
            function getLink() {
                if (options.info && options.info.link) {
                    return options.info.link
                } else {
                    return defaults.info.link
                }
            }
            function getMsg() {
                var input = "<input type='text' value='" + currPage + "' >";
                if (options.info && options.info.msg_on == false) {
                    return false
                }
                if (options.info && options.info.msg) {
                    var str = options.info.msg;
                    str = str.replace("{currText}", input);
                    str = str.replace("{currPage}", currPage);
                    str = str.replace("{sumPage}", pageCount);
                    return str
                } else {
                    var str = defaults.info.msg;
                    str = str.replace("{currText}", input);
                    str = str.replace("{currPage}", currPage);
                    str = str.replace("{sumPage}", pageCount);
                    return str
                }
            }
            function getText() {
                var msg = getMsg();
                if (msg) {
                    msg = $(msg)
                } else {
                    return ""
                }
                var input = msg.children(":text");
                if (options.info && options.info.text) {
                    var css = options.info.text;
                    for (temp in css) {
                        var val = eval("css." + temp);
                        input.css(temp, val)
                    }
                    return msg.html()
                } else {
                    var css = defaults.info.text;
                    for (temp in css) {
                        var val = eval("css." + temp);
                        input.css(temp, val)
                    }
                    return msg.html()
                }
            }
            function getPageCountId() {
                if (options.ajax && options.ajax.pageCountId) {
                    return options.ajax.pageCountId
                } else {
                    return defaults.ajax.pageCountId
                }
            }
            function getAjaxStart() {
                if (options.ajax && options.ajax.ajaxStart) {
                    options.ajax.ajaxStart();
                } else {
                    defaults.ajax.ajaxStart;
                }
            }
            //请求完成执行方法
            function getAjaxStop() {
                if (options.ajax && options.ajax.ajaxStop) {
                    options.ajax.ajaxStop();
                } else {
                    defaults.ajax.ajaxStop;
                }
            }

            //请求失败执行方法
            function getAjaxError() {
                if (options.ajax && options.ajax.ajaxStop) {
                    options.ajax.ajaxError();
                } else {
                    defaults.ajax.ajaxError();
                }
            }

            function saveCurrPage(page) {
                if (options.info && options.info.cookie_currPageKey && options.info.cookie_currPage) {
                    var key = options.info.cookie_currPageKey + "_currPage";
                    $.cookie(key, page)
                }
            }
            function getInt(val) {
                return parseInt(val)
            }


            function updateView() {
                currPage = getInt(currPage);
                pageCount = getInt(pageCount);

                var link = getLink();
                var firstPage = lastPage = 1;
                if (currPage - tempPage > 0) {
                    firstPage = currPage - tempPage
                } else {
                    firstPage = 1
                }
                if (firstPage + btnsize > pageCount) {
                    lastPage = pageCount + 1;
                    firstPage = lastPage - btnsize
                } else {
                    lastPage = firstPage + btnsize
                }
                var content = "";
                content += getFirst();
                if (currPage == 1) {
                    content += "<span class=\"preNo\" title=\"" + getPrev() + "\">" + getPrev() + " </span>"
                } else {
                    content += "<a  href='javascript:;'  class='pre' title='" + (currPage - 1) + "'>" + getPrev() + " </a>"
                }
                if (firstPage <= 0) {
                    firstPage = 1
                }
                if (firstPage > 1) {
                    if (firstPage <= btnsize) {
                        content += "<a href='javascript:;' class='cur_dft' title='1'>...</a>";
                    } else {
                        content += "<a href='javascript:;' class='cur_dft' title='" + (firstPage - btnsize) + "'>...</a>";
                    }
                }
                for (firstPage; firstPage < lastPage; firstPage++) {
                    if (firstPage == currPage) {
                        content += "<span class=\"cur_index\" title=\"" + firstPage + "\">" + firstPage + "</span>"
                    } else {
                        content += "<a href='javascript:;' class='cur_dft' title='" + firstPage + "'>" + firstPage + "</a>"
                    }
                }

                if (pageCount > lastPage - 1) {
                    content += "<a href='javascript:;' class='cur_dft' title='" + (lastPage) + "'>...</a>";
                }

                if (currPage == pageCount) {
                    content += "<span class=\"nextNo\" title=\"" + getNext() + "\">" + getNext() + " </span>"
                } else {
                    content += "<a href='javascript:;' class='next' title='" + (currPage + 1) + "'>" + getNext() + " </a>"
                }
                content += getLast(pageCount);
                content += getText();
                content += " <span>共 " + pageCount + " 页 </span> <span>&nbsp;到第&nbsp;<input type='text' " + (PageResourceCount > 0 && pageCount > 1 ? "" : "disabled=\"disabled\"") + " maxlength='6' class='pageBox' />&nbsp;页&nbsp;<a href='javascript:void(0)'  class='goto'>确定</a></span>";
                if (!options.IsShowOnePaging) {
                    content = "";
                }
                //----------------   
                obj.html(content);
                obj.find("input[type=text]:enabled").on("keyup", function (event) {
                    var keycode = event.which;
                    if (keycode != 8) {
                        PagingKeyInt(this);
                        if (this.value != "" && PageResourceCount > 0) {
                            $(obj).find(".goto").unbind().on("click", function () {
                                //得到页码
                                //获取那个文本框的页面
                                var tpage = $(".pageBox", obj).val();
                                if (parseInt(tpage) > pageCount) {
                                    layerCommon.msg("输入值不能大于总页数", IconOption.错误, 2000);
                                    $(obj).find(".goto").unbind("click");
                                    $(".pageBox", obj).val("");
                                    return false;
                                }
                                createView(tpage);
                                $(this).focus();
                                return false

                            });
                        } else {
                            $(obj).find(".goto").unbind("click");
                        }
                    }
                });

                function PagingKeyInt(val) {
                    if (val.value == "0")
                        val.value = "";
                    else
                        val.value = val.value.replace(/[^\d]/g, '');
                };
                $("a.cur_dft", obj).on("click", function () {
                    var page = $(this).attr("title");
                    createView(page);
                    return false;
                });

                $("a.pre", obj).on("click", function () {
                    var ThitCurPage = parseInt(currPage);
                    if (ThitCurPage > 1) {
                        createView(ThitCurPage - 1);
                    }
                    return false;
                });

                $("a.next", obj).on("click", function () {
                    var ThitCurPage = parseInt(currPage);
                    if (ThitCurPage < parseInt(pageCount)) {
                        createView(ThitCurPage + 1);
                    }
                    return false;
                });

            };
            //得到首页
            function getFirst() {
                currPage = getInt(currPage);
                if (options.info && options.info.first_on == false) {
                    return ""
                }

                //---------修改如果当前页码为1  首页不可用-----------/
                var str = "";
                if (options.info && options.info.first_on && options.info.first) {
                    if (currPage == 1) {
                        str = "<span class=\"disabled\" title='1'>" + options.info.first + " </span>";
                    } else {
                        str = "<a href='" + getLink() + "' title='1'>" + options.info.first + "</a>";
                    }
                    return str;
                } else {
                    if (currPage == 1) {
                        str = "<span class=\"disabled\" title='1'>" + defaults.info.first + " </span>";
                    } else {
                        str = "<a href='" + getLink() + "' title='1'>" + defaults.info.first + "</a>";
                    }
                    return str;
                }
            }

            //得到尾页
            function getLast(pageCount) {
                currPage = getInt(currPage);
                if (options.info && options.info.last_on == false) {
                    return ""
                }
                //---------修改如果当前页码为最后一页  尾页不可用-----------/
                var str = "";
                if (options.info && options.info.last_on && options.info.last) {
                    if (currPage == pageCount) {
                        str = "<span class=\"disabled\" title='" + pageCount + "'>" + options.info.first + " </span>";
                    } else {
                        str = "<a href='" + getLink() + "' title='" + pageCount + "'>" + options.info.last + "</a>";
                    }

                    return str
                } else {
                    if (currPage == pageCount) {
                        str = "<span class=\"disabled\" title='" + pageCount + "'>" + defaults.info.first + " </span>";
                    } else {
                        var str = "<a href='" + getLink() + "' title='" + pageCount + "'>" + defaults.info.last + "</a>";
                    }
                    return str
                }
            }
            function createView(page) {
                currPage = page;
                saveCurrPage(page);
                var ajax = getAjax();
                if (ajax.on) {
                    var varUrl = ajax.url;
                    var param = getParam();
                    RequstParam && function () {
                        for (var Item in RequstParam) {
                            param[Item] = RequstParam[Item];
                        }
                    }();
                    $.ajax({
                        url: varUrl,
                        type: 'GET',
                        data: param,
                        contentType: "application/x-www-form-urlencoded;utf-8",
                        async: true,
                        cache: false,
                        timeout: 5000,
                        error: function () {
                            getAjaxError();
                        },
                        success: function (data) {
                            loadPageCount({
                                dataType: ajax.dataType,
                                callback: ajax.callback,
                                data: data
                            });
                            updateView();
                        }, beforeSend: function () {   //请求前
                            getAjaxStart();
                        }, complete: function () {  //请求后
                            getAjaxStop();
                        }
                    })
                } else {
                    updateView()
                }
            }
            function checkParam() {
                if (currPage < 1) {
                    alert("配置参数错误\n错误代码:-1");
                    return false
                }
                if (currPage > pageCount) {
                    alert("配置参数错误\n错误代码:-2");
                    return false
                }
                return true
            }


            function loadPageCount(options) {
                if (options.dataType) {
                    var data = options.data;
                    var resultPageCount = false;
                    var isB = true;
                    var pageCountId = getPageCountId();
                    switch (options.dataType) {
                        case "json":
                            data = eval("(" + data + ")");
                            resultPageCount = eval("data." + pageCountId);
                            PageResourceCount = parseInt(eval("data.totalCount"));
                            break;
                        case "xml":
                            resultPageCount = $(data).find(pageCountId).text();
                            break;
                        default:
                            isB = false;
                            var callback = options.callback + "(data)";
                            eval(callback);
                            resultPageCount = $("#" + pageCountId).val();
                            break
                    }
                    if (resultPageCount) {
                        pageCount = resultPageCount;
                    }
                    if (isB) {
                        var callback = options.callback + "(data)";
                        eval(callback)
                    }
                }
            }
            options = param;
            currPage = getCurrPage();
            pageCount = getPageCount();
            pageSize = getPageSize();
            tempPage = 0;
            var cssStyle = getCssStyle();
            var link = document.createElement("link");
            if (options.MyPaginCssSrc) {
                MyPaginCssSrc = options.MyPaginCssSrc;
            }
            link["href"] = MyPaginCssSrc;
            link["rel"] = "stylesheet";
            link["type"] = "text/css";
            $("head")[0].appendChild(link);
            obj.addClass(cssStyle);
            if (options.ajax.params) {
                RequstParam = options.ajax.params;
            }
            if (options.btnsize) {
                btnsize = options.btnsize;
            } else {
                btnsize = pageSize;
            }
            if (checkParam()) {
                createView(currPage);
            }
        }
    }
})(jQuery);
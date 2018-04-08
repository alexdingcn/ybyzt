$(function () {
    //增加选择商品行
    $(document).on("click", ".minus2", function () {
        //新增行追加一个空的html
        var html = "<tr><td class=\"t8\"><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td><div class=\"search\"><input name=\"\" type=\"text\" class=\"box project2\" /><a class=\"opt-i\"></a><div class=\"search-opt none\"><ul class=\"list\"></ul><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt2-i\"></i>选择商品</a></div></div></div></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
        $(".tabLine table tbody").append(html);
    })
    //删除当前商品行
    $(document).on("click", ".add2", function () {
        if ($(".tabLine table tbody tr").length > 1) {
            $(this).parent().parent().parent().remove(); //大于1行时直接删除
        } else {
            $(this).parent().parent().parent().remove(); //小于等于1时 先删除 再添加一个空的html
            var html = "<tr><td class=\"t8\"><div class=\"addg\"><a href=\"javascript:;\" class=\"minus2\"></a><a href=\"javascript:;\" class=\"add2\"></a></div></td><td><div class=\"search\"><input name=\"\" type=\"text\" class=\"box project2\" /><a class=\"opt-i\"></a><div class=\"search-opt none\"><ul class=\"list\"></ul><div class=\"opt\"><a href=\"javascript:;\"><i class=\"opt2-i\"></i>选择商品</a></div></div></div></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>";
            $(".tabLine table tbody").append(html);
        }
    })

    //失去焦点，延迟加载
    var t = '';
    $(document).on("blur", ".project2", function () {
        clearTimeout(t);
        t = setTimeout("send_data()", 200); //选中了商品 延迟后关闭
    });
    //商品筛选
    $(document).on("keyup", ".project2", function () {
        var strtext = $.trim($(this).val()); //输入的商品关键字或者商品编码
        var inindex = $(this).parent().parent().parent().index(); //当前行索引
        showGoods(strtext, inindex); //筛选商品
    })
    //商品显示top5
    $(document).on("focus", ".project2", function () {
        var inindex = $(this).parent().parent().parent().index(); //当前行索引
        //setTimeout("showGoods('', " + inindex + ", " + disId + ")", 1000);
        showGoods("", inindex); //筛选商品
    })
    //更多商品3
    $(document).on("click", ".sPic", function () {
        var indexs = $(this).parent().parent().index(); //当前行索引
        var goodsInfoId = $(this).parent().parent().attr("tip"); //当前行商品id
        var goodsInfoIdList = ""; //拼接所有行的商品id 用于查询时排除
        $(".tabLine table tbody tr").each(function (x, indexy) {
            if ($(".tabLine table tbody tr").eq(x).attr("trindex") != undefined) {
                goodsInfoIdList += $(".tabLine table tbody tr").eq(x).attr("tip") + ",";
            }
        })
        if (goodsInfoIdList != "") {
            goodsInfoIdList = goodsInfoIdList.substring(0, goodsInfoIdList.length - 1);
        }
        if (goodsInfoId != undefined) {
            gengduo(indexs, goodsInfoId, goodsInfoIdList); //(当前行的索引，当前行的商品id，所有行的商品id拼接，代理商id)
        } else {
            gengduo(indexs, "", ""); //(当前行的索引，当前行的商品id，所有行的商品id拼接，代理商id)
        }
    })
    //更多商品2
    $(document).on("click", ".tabLine .opt-i", function () {
        var indexs = $(this).parent().parent().parent().index(); //当前行索引
        var goodsInfoIdList = ""; //拼接所有行的商品id 用于查询时排除
        $(".tabLine table tbody tr").each(function (x, indexy) {
            if ($(".tabLine table tbody tr").eq(x).attr("trindex") != undefined) {
                goodsInfoIdList += $(".tabLine table tbody tr").eq(x).attr("tip") + ",";
            }
        })
        if (goodsInfoIdList != "") {
            goodsInfoIdList = goodsInfoIdList.substring(0, goodsInfoIdList.length - 1);
        }
        gengduo(indexs, "", goodsInfoIdList); //(当前行的索引，当前行的商品id，所有行的商品id拼接)
    })
    //更多商品1
    $(document).on("click", ".tabLine .search-opt .opt", function () {
        var indexs = $(this).parent().parent().parent().parent().index(); //当前行索引
        var goodsInfoIdList = ""; //拼接所有行的商品id 用于查询时排除
        $(".tabLine table tbody tr").each(function (x, indexy) {
            if ($(".tabLine table tbody tr").eq(x).attr("trindex") != undefined) {
                goodsInfoIdList += $(".tabLine table tbody tr").eq(x).attr("tip") + ",";
            }
        })
        if (goodsInfoIdList != "") {
            goodsInfoIdList = goodsInfoIdList.substring(0, goodsInfoIdList.length - 1);
        }
        gengduo(indexs, "", goodsInfoIdList); //(当前行的索引，当前行的商品id，所有行的商品id拼接，代理商id)
    })
    //选中筛选商品
    $(document).on("click", ".search-opt .list li", function () {
        var goodsInfoId = $(this).attr("tip"); //goodsInfoid
        var bol = false;
        var inindex = $(this).parent().parent().parent().parent().parent().index();
        //判断是否存在相同的商品
        $(".tabLine table tbody tr").each(function (indexss, objs) {
            if ($(".tabLine table tbody tr").eq([indexss]).attr("tip") != undefined) {
                //根据商品id与每行商品数据的tip比较
                if (goodsInfoId == $(".tabLine table tbody tr").eq([indexss]).attr("tip")) {
                    bol = true;
                    return false;
                } else {
                    bol = false;
                }
            }
        })
        if (bol) { layerCommon.msg("商品已存在", IconOption.错误); } else {
            GoodsList(goodsInfoId,"",inindex,"");
        }
    })
    //商品数量减
    $(document).on("click", ".divnum .minus", function () {
        var id = $(this).parent().parent().parent().attr("tip"); //goodsinfoId
        var index = $(this).parent().attr("tip2"); //index
        onchengSum(id, index, -1);
    })
    //商品数量框
    $(document).on("change", ".divnum .txtGoodsNum", function () {
        var id = $(this).parent().parent().parent().attr("tip"); //goodsinfoId
        var index = $(this).parent().attr("tip2"); //index
        onchengSum(id, index, 0);
    })
    //商品数量增
    $(document).on("click", ".divnum .add", function () {
        var id = $(this).parent().parent().parent().attr("tip"); //goodsinfoId
        var index = $(this).parent().attr("tip2"); //index
        onchengSum(id, index, 1);
    })
})
//（商品id，当前行的索引,（-1 减 0 文本输入 1 加）数量）
function onchengSum(id, index, type) {
    var snum;   //商品数量
    var Digits = '<%=OrderInfoType.rdoOrderAudit("订单下单数量是否取整", this.CompID) %>'; //小数位数
    var sDigits = Digits.substring(2, Digits.indexOf(".") + 5).length;
    var IsInve = '<%=IsInve %>'; //是否启用商品库存，默认0、启用库存
    snum = $(".txtGoodsNum" + index).val(); //当前行的数量
    var Num = parseFloat(snum).toFixed(sDigits);
    var batePrice = $(".txtRebate").val();
    if (parseFloat(type) == -1) {//减数量
        Num = parseFloat(Num) - parseFloat(1);
        if (parseFloat(Num) <= parseFloat(0)) {
            Num = parseFloat(1).toFixed(sDigits)
        }
    } else if (parseFloat(type) == 1) {//加数量
        Num = parseFloat(Num) + parseFloat(1);
        if (Num > 999999999) {
            Num = parseFloat(999999999).toFixed(sDigits)
        }
    } else {//手动输入数量
        if (parseFloat(Num) <= parseFloat(0)) {
            Num = parseFloat(1).toFixed(sDigits)
        }
    }
    //如果数量为空或者 不为数字的时候默认赋值1
    if (Num.toString() == "" || isNaN(Num) == true) {
        Num = parseFloat(1).toFixed(sDigits)
    }
    Num = parseFloat(Num).toFixed(sDigits);
    $(".txtGoodsNum" + index).val(formatMoney(Num, 2)); //商品购买数量
}
//更多商品（当前行索引，当前行商品id，每一行的商品id拼接,代理商id）
function gengduo(indexs, goodsInfoId, goodsInfoIdList) {
    var compId = $("#hidCompId").val();
    var type = $("#hid_Type").val();
    if (type=="1") {
        var index = layerCommon.openWindow("选择商品", "../../Distributor/newOrder/selectgoods.aspx?stock=1&CompId=" + compId + "&index=" + indexs + "&goodsInfoId=" + goodsInfoId + "&type=1&goodsInfoIdList=" + goodsInfoIdList, "985px", "630px");  //记录弹出对象
    }
    else {
        var index = layerCommon.openWindow("选择商品", "../../Distributor/newOrder/selectgoods.aspx?stock=1&CompId=" + compId + "&index=" + indexs + "&goodsInfoId=" + goodsInfoId + "&goodsInfoIdList=" + goodsInfoIdList, "985px", "630px");  //记录弹出对象
    }
    $("#hid_Alert").val(index); //记录弹出对象
}
function send_data() {
    $(".tabLine .search-opt").hide(); //隐藏商品列表
}
//截取字符串
//商品名称，属性值，是否需要截取
function GetGoodsName(goodsName, valueInfo, type) {
    goodsName = stripscript(goodsName)
    var str = "";
    var str2 = "";
    if (valueInfo != "") {
        str2 = valueInfo.toString().substring(0, valueInfo.length - 1).toString().replace('；', ',');
    } else {
        str2 = valueInfo;
    }
    str = goodsName + "&nbsp;" + str2;
    if (type == "1") {
        if (str.length > 30) {
            str = str.substring(0, 30) + "...";
        }
    }
    return str;
};
//过滤非法字符
function stripscript(strHtlm) {
    strHtlm = strHtlm + "";
    //var pattern = new RegExp("exec|insert|delete|drop|truncate|update|declare|frame|or|style|expression|and|select|create|script|img|alert|href|1=1|<(.[^>]*)>.*?<(.[^>]*)>|<(.[^>]*)>|&(lt|#60)|&(gt|#62))","g")
    //s.replace(pattern,"");
    //var pattern = new RegExp("[%--`~!@#$^&*()=|{}':;',\\[\\].<>/?~！@#￥……&*（）——|{}【】‘；：”“'。，、？]")        //格式 RegExp("[在中间定义特殊过滤字符]")
    var pattern = /(insert|delete|truncate|update|declare|frame|style|expression|select|create|script|alert|<(.[^>]*)>.*?<(.[^>]*)>|<(.[^>]*)>|&(lt|#60)|&(gt|#62)) /ig;
    return strHtlm.replace(pattern, "");
}
//关闭弹出窗口
function CloseGoods() {
    layerCommon.layerClose("hid_Alert");
}
//禁用Enter
document.onkeydown = function (event) {
    var target, code, tag;
    if (!event) {
        event = window.event; //针对ie浏览器  
        target = event.srcElement;
        code = event.keyCode;
        if (code == 13) {
            tag = target.tagName;
            if (tag == "TEXTAREA") { return true; }
            else { return false; }
        }
    }
    else {
        target = event.target; //针对遵循w3c标准的浏览器，如Firefox  
        code = event.keyCode;
        if (code == 13) {
            tag = target.tagName;
            if (tag == "INPUT") { return false; }
            else { return true; }
        }
    }
}
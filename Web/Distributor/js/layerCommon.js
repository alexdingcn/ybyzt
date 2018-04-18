
var ClearMillisecond = 3000;
var IconOption = {
    "不显示图标": -1,
    "感叹": 0,
    "正确": 1,
    "错误": 2,
    "询问": 3,
    "锁定": 4,
    "哭脸": 5,
    "笑脸": 6
};
var layerCommon = {
    alert: function (Msg, Icon, CMillisecond, CallBack, closeBtn) {
        var Time = ClearMillisecond;
        if (CMillisecond != undefined) {
            Time = CMillisecond;
        }
        if (Icon == undefined) {
            Icon = IconOption.笑脸;
        }
        if (closeBtn == undefined) {
            closeBtn = 1;
        }
        var index = layer.alert(Msg, { icon: Icon, closeBtn: closeBtn }, function () {
            layerCommon.layerClose(index);
            if (typeof CallBack == "function")
                CallBack();
        });
    },
    msg: function (Msg, Icon, CMillisecond, EndCallback) {
        var Time = ClearMillisecond;
        if (CMillisecond != undefined) {
            Time = CMillisecond;
        }
        if (Icon == undefined) {
            Icon = IconOption.笑脸;
        }
        layer.msg(Msg, { icon: Icon, time: Time, end: function () {
            if (typeof EndCallback == "function")
                EndCallback();
        } 
        });
    },
    confirm: function (Msg, CallBack, title, CancelCallBack) {
        if (title == undefined) {
            title = "提示";
        }
        var index = layer.confirm(Msg, { icon: 3,
            btn: ['确定', '取消'] //按钮
            , title: title
        }, function () {
            layerCommon.layerClose(index);
            if (typeof CallBack == "function")
                CallBack();
        }, function () {
            if (typeof CancelCallBack == "function")
                CancelCallBack();
        });
    },
    openWindow: function (title, url, width, height, CloseCallBack, Ismaxmin) {
        Ismaxmin = Ismaxmin == undefined ? true : Ismaxmin;
        var index = layer.open({
            type: 2,
            title: title,
            shade: 0.7,
            area: [width, height],
            maxmin: Ismaxmin,
            content: url, //iframe的url
            end: function () {
                if (typeof CloseCallBack == "function")
                    CloseCallBack();
            }
        });
        return index;
    },
    layerClose: function (index) {
        $("#" + index).length > 0 ? layer.close($("#" + index).val()) : layer.close(index);
    },
    layerCloseAll: function (type) {
        type == undefined ? layer.closeAll() : layer.closeAll(type);
    },
    tip: function (Content, FixControl, Options) {
        var index = layer.tips(Content, FixControl, Options);
    }
}


function KeyInt(val,defaultValue) {
    if (val.value == "0" || val.value == "") {
        if (defaultValue != undefined)
            val.value = defaultValue;
        else 
        val.value = "";
    }
    else
        val.value = val.value.replace(/[^\d]/g, '');
};

    $(document).ready(function () {
        $("input:text.txtKeyInt").on({
            "keyup": function () {
                KeyInt(this, "");
            }, "blur": function () {
                KeyInt(this, "1");
            }
        })
    });

    $.fn.extend({ PositionParent: function (ParentSelector) {
        var Top = { top: 0 }, $this = $(this), $Parent = $(ParentSelector);
        Top.top = $this.offset().top - $Parent.offset().top;
        return Top;
    }
    });


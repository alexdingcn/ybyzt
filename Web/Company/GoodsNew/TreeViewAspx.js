$(document).ready(function () {
    var div = $('<div   Msg="True"  style="z-index:1000; background-color:#000; opacity:0.3; filter:alpha(opacity=30);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');
    




    //移除
    $(document).on("click", ".tablelist .TypeDel", function () {
        var obj = $(this);
        layerCommon.confirm("确认删除?", function () {
            var id = obj.attr("tip");
            $.ajax({
                type: "post",
                url: "GoodsCategory.aspx",
                dataType: "text",
                async: false,
                data: { Action: "Del", Id: id },
                success: function (data) {
                    var json = eval('(' + data + ')');
                    if (json.result != undefined) {
                        if (json.result == true) {
                            layerCommon.msg("移除成功！", IconOption.笑脸, 3000);
                            obj.parents("tr:eq(0)").remove();
                        } else {
                            if (json.code != "操作成功") {
                                layerCommon.msg(json.code, IconOption.哭脸, 3000);
                            }
                        }
                    } else {
                        layerCommon.msg("操作失败", IconOption.错误, 3000);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
            });
        }, "提示", function () {
        });
    });

    //上移
    $(document).on("click", ".tablelist .TypeIndex", function () {
        var obj = $(this);
        var id = obj.attr("tip");
        var previd = obj.parents().parents().prev().attr("Id");
        $.ajax({
            type: "post",
            url: "GoodsCategory.aspx",
            dataType: "text",
            async: false,
            data: { Action: "Sort", Id: id },
            success: function (data) {
                var json = eval('(' + data + ')');
                if (json.result != undefined) {
                    if (json.result == true) {
                        layerCommon.msg("上移成功！", IconOption.笑脸, 3000);
                        window.location.reload();
                    } else {
                        if (json.code != "操作成功") {
                            layerCommon.msg(json.code, IconOption.哭脸, 3000);
                        }
                    }
                } else {
                    layerCommon.msg("上移失败！", IconOption.错误, 3000);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("上移失败！", IconOption.错误, 2000);
            }
        });
    });

    //下移
    $(document).on("click", ".tablelist .TypeIndexDown", function () {
        var obj = $(this);
        var id = obj.attr("tip");
        $.ajax({
            type: "post",
            url: "GoodsCategory.aspx",
            dataType: "text",
            async: false,
            data: { Action: "SortDown", Id: id },
            success: function (data) {
                var json = eval('(' + data + ')');
                if (json.result != undefined) {
                    if (json.result == true) {
                        layerCommon.msg("下移成功！", IconOption.笑脸, 2000);
                        window.location.reload();
                    } else {
                        if (json.code != "操作成功") {
                            layerCommon.msg(json.code, IconOption.哭脸, 2000);
                        }
                    }
                } else {
                    layerCommon.msg("下移失败！", IconOption.错误, 2000);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layerCommon.msg("下移失败！", IconOption.错误, 2000);
            }
        });
    });
});
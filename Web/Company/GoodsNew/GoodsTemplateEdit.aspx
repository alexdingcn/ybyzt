<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsTemplateEdit.aspx.cs"
    Inherits="Company_Goods_GoodsTemplateEdit" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>规格模板维护</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <style>
        .col-xs-6
        {
            width: 46%;
            float: left;
            min-height: 1px;
            padding-left: 15px;
            padding-right: 15px;
            position: relative;
        }
        .border
        {
            border: 1px solid #ddd;
        }
        .space5
        {
            margin-top: 5px;
        }
        .row
        {
            margin-left: -15px;
            margin-right: -15px;
        }
        .modal-body
        {
            padding: 15px;
            position: relative;
        }
        .p-b-6
        {
            padding-top: 6px;
        }
        .m-l-10
        {
            margin-left: 10px;
        }
        .pointer
        {
            cursor: pointer;
        }
        .ng-scope span
        {
            display: inline-block;
        }
        input[type="checkbox"], input[type="radio"]
        {
            line-height: normal;
            margin: 3px 5px 5px 5px;
        }
        input, select
        {
            display: inline-block;
        }
        .btn, img, input
        {
            outline: 0 none !important;
            vertical-align: middle;
        }
        .col-xs-6 li:hover
        {
            background: #e5ebee;
        }
    </style>
    <script>
        $(function () {
            //选中属性得到属性值列表
            $(document).on("click", "#divAttr li", function () {
                var id = $(this).attr("tip"); //属性id
                var bol = false;
                $("#divAttrValue ul").each(function (index, obj) {
                    if ($(this).attr("tip") == id) {
                        $(this).show().siblings().hide();
                        bol = true;
                    }
                })
                if (!bol) {
                    layerCommon.msg("改属性没有维护属性值", IconOption.错误);
                    return false;
                }
                //                $.ajax({
                //                    type: "post",
                //                    url: "GoodsTemplateEdit.aspx",
                //                    data: { ck: Math.random(), action: "AttrValueShow", id: id },
                //                    dataType: "json",
                //                    success: function (data) {
                //                        var html = "<ul class=\"ng-scope\">";
                //                        var json = eval(data);
                //                        $(json).each(function (index, obj) {
                //                            if (obj.AttrValue != undefined) {
                //                                html += "<li class=\"pointer proHover p-t-6 p-b-6 ng-scope\"><span class=\"customerManager m-l-10\"><input class=\"ng-pristine ng-untouched ng-valid\"  type=\"checkbox\" name=\"chekAttrValue\" value=\"" + obj.ID + "\"/></span><span class=\"ng-binding\">" + obj.AttrValue + "</span></li>";
                //                            } else {
                //                                return false;
                //                            }
                //                        })
                //                        html += "</ul>";
                //                        $("#divAttrValue").html(html);
                //                    }
                //                })
            })
            //属性的勾选事件
            $("#divAttr input[type='checkbox']").click(function () {
                var id = $(this).val();
                if (!$(this).prop("checked")) {
                    $("#divAttrValue ul").each(function (index, obj) {
                        if ($(this).attr("tip") == id) {
                            $(this).find("input[type='checkbox']").prop("checked", false);
                        }
                    })
                } else {
                    $("#divAttrValue ul").each(function (index, obj) {
                        if ($(this).attr("tip") == id) {
                            $(this).find("input[type='checkbox']").prop("checked", true);
                        }
                    })
                }
                var count = 0;
                var chklist = $("#divAttr input[type='checkbox']");
                $(chklist).each(function (index, obj) {
                    if (obj.checked) {
                        count++;
                    }
                })
                if (count > 3) {
                    layerCommon.msg("最多勾选三个属性", IconOption.错误);
                    return false;
                }
            })
            //属性值的勾选事件
            $("#divAttrValue input[type='checkbox']").click(function () {
                var count = 0;
                $(this).parent().parent().parent().find("input[type='checkbox']").each(function (index, obj) {
                    if (obj.checked) {
                        count++;
                    }
                })
                var tip = $(this).parent().parent().parent().attr("tip");
                if (count > 0) {
                    $("#divAttr li").each(function (index, obj) {
                        if ($(this).attr("tip") == tip) {
                            $("#divAttr li").eq(index).find("input[type='checkbox']").prop("checked", true);
                        }
                    })
                } else {
                    $("#divAttr li").each(function (index, obj) {
                        if ($(this).attr("tip") == tip) {
                            $("#divAttr li").eq(index).find("input[type='checkbox']").prop("checked", false);
                        }
                    })
                }
            })
        })
        //验证
        function formCheck() {
            var template = $(".txtTemplate").val(); //模板名称
            if (template == "") {
                layerCommon.msg("模板名称不能为空", IconOption.错误);
                return false;
            }
            var bol = false;
            var chklist = $("#divAttr input[type='checkbox']");
            $(chklist).each(function (index, obj) {
                if (obj.checked) {
                    bol = true;
                    return false; 
                }
            })
            if (!bol) {
                layerCommon.msg("请勾选属性", IconOption.错误);
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="rightinfo" style="margin-top: 20px; margin-left: 0px; width: auto;">
        <div class="div_content">
            <div class="lbtb">
                <table class="dh">
                    <tr>
                        <td>
                            <span>
                                <label class="required">
                                    *</label>模板名称</span>
                        </td>
                        <td>
                            <input name="txtTemplate" runat="server" id="txtTemplate" type="text" class="textBox txtTemplate"
                                style="width: 150px; margin-left: 5px;" />
                        </td>
                    </tr>
                </table>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-xs-6">
                            <div>
                                选择属性<span style="color: Red; display: inline-block;">(注：一次最多勾选三个属性)</span></div>
                            <div class="border space5" id="divAttr" runat="server" style="height: 240px; overflow: auto;">
                            </div>
                        </div>
                        <div class="col-xs-6">
                            <div style="height: 17px">
                                属性值
                            </div>
                            <div class="border space5" id="divAttrValue" runat="server" style="height: 240px;
                                overflow: auto;">
                            </div>
                        </div>
                    </div>
                </div>
                <div>
                </div>
                <div class="div_footer" style="float: left; margin-left: 380px;">
                    <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return formCheck()"
                        OnClick="btnAdd_Click" />&nbsp;
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

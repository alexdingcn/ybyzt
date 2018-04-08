<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsAttributeList.aspx.cs"
    Inherits="Company_Goods_GoodsAttributeList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>商品属性维护</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            })
        })
        $(document).ready(function () {
            //新增
            $(".click").click(function () {
                $(".tip").fadeIn(200);
                $(".Layer").fadeIn(200);
                $(".tip").css("height", "400px");
                $(".divedit").html("");
            });
            $(".tiptop a").click(function () {
                $(".tip").fadeOut(200);
                $(".tip2").fadeOut(200);
                $(".Layer").fadeOut(200);
                $("input[type='text']").val("");
            });
            $(".cancel").click(function () {
                $(".tip").fadeOut(200);
                $(".tip2").fadeOut(200);
                $(".Layer").fadeOut(200);
                $("input[type='text']").val("");
            });
            //编辑
            $(".edit").click(function () {
                $(".tip2").fadeIn(200);
                $(".Layer").fadeIn(200);
                $(".tip2").css("height", "400px");
                var id = $(this).attr("tip");
                $("#hideAttrId").val(id);
                ShowAttributeValue(id); //显示属性值列表
            })
            //删除属性值
            $(document).on("click", ".Del", function () {
                var obj = $(this);
                var id = obj.attr("tip"); //属性值ID
                var AttributeID = obj.attr("tips"); //属性ID
                $.ajax({
                    type: "post",
                    url: "GoodsAttributeList.aspx",
                    data: { ck: Math.random(), action: "Del", id: id },
                    dataType: "text",
                    success: function (data) {
                        if ($.trim(data) == "ycz") {
                            layerCommon.msg("属性值已被使用，不能删除", IconOption.错误);
                            return;
                        }
                        ShowAttributeValue(AttributeID); //显示属性值列表
                    }, error: function () { }
                })
            })
            //禁用属性值
            $(document).on("click", ".Enable", function () {
                var id = $(this).attr("tip"); //属性值ID
                var AttributeID = $(this).attr("tips"); //属性ID
                $.ajax({
                    type: "post",
                    url: "GoodsAttributeList.aspx",
                    data: { ck: Math.random(), action: "Enable", id: id },
                    dataType: "text",
                    success: function (data) {
                        if ($.trim(data) == "jycg") {//禁用
                            $("#txtAttrValue" + id).attr("disabled", "disabled");
                        } else if ($.trim(data) == "qycg") {//启用
                            $("#txtAttrValue" + id).removeAttr("disabled");
                        }
                        ShowAttributeValue(AttributeID); //显示属性值列表
                    }, error: function () { }
                })
            })
            //触发
            $(document).on("change", ".txtAttrValue", function () {
                var id = $(this).attr("tip");
                $("#txtAttrValue" + id).val($(this).val());
            })
            //编辑属性值
            $(document).on("click", ".Save", function () {
                var id = $(this).attr("tip"); //属性值ID
                var AttributeID = $(this).attr("tips"); //属性ID
                var value = $("#txtAttrValue" + id).val(); //属性值
                if ($.trim(id) != "") {
                    $.ajax({
                        type: "post",
                        url: "GoodsAttributeList.aspx",
                        data: { ck: Math.random(), action: "Save", id: id, value: value, attributeId: AttributeID },
                        dataType: "text",
                        async: false,
                        success: function (data) {
                            if ($.trim(data) == "ycz") {
                                layerCommon.msg("属性值已存在", IconOption.错误);
                            } else if ($.trim(data) == "cg") {
                                layerCommon.msg("保存成功", IconOption.正确); return false; // $(".cancel").trigger("click"); location.replace(location.href); 
                            }
                        }, error: function () { }
                    })
                }
            })

            //显示属性值列表
            function ShowAttributeValue(id) {
                $(".btnBg2").attr("tip", id); //隐藏当前属性的id，用于新增属性值
                $.ajax({
                    type: "post",
                    url: "GoodsAttributeList.aspx",
                    data: { ck: Math.random(), action: "show", id: id },
                    dataType: "json",
                    success: function (data) {
                        var html = "<table style=\"width:100%;\">";
                        var json = eval(data);
                        $(json).each(function (index, obj) {
                            $(".txtAttributeNames").val(obj.AttributeName);
                            $(".txtMemos").val(obj.Memo);
                            if ($.trim(obj.AttrValue) != "") {
                                var strhtml = "";
                                if (obj.IsEnabled == "0") {
                                    strhtml = "disabled=\"disabled\"";
                                }
                                html += "<tr><td style=\"width: 20px;\">" + parseInt(index + 1) + "、</td>" +
                                                               "<td styel=\"width: 190px; height:24px; vertical-align:middle; text-align:left;\">" +
                                                               "<input onkeyup=\"priceKeyup(this)\"  style=\"width:100px\" type=\"text\" " + strhtml + " id=\"txtAttrValue" + obj.ID + "\"" +
                                    "class=\"textBox txtAttrValue\" value=\"" + obj.AttrValue + "\" tip=\"" + obj.ID + "\"/></td>" +
                                                               "<td>&nbsp;&nbsp;</td><td style=\"width: 20px;\"><img class=\"Save\" tip=\"" + obj.ID + "\"  tips=\"" + obj.AttributeID + "\" title=\"保存属性值\" src=\"../images/icon_save.png\" style=\"cursor: pointer;\"" +
                                                               " /></td><td>&nbsp;&nbsp;</td><td style=\"width: 20px;\"><img class=\"Enable\" tip=\"" + obj.ID + "\"  tips=\"" + obj.AttributeID + "\" title=\"禁用属性值\" src=\"../images/icon_enable.png\" style=\"cursor: pointer;\"" +
                                                               " /></td><td>&nbsp;&nbsp;</td><td style=\"width: 20px;\"><img class=\"Del\" tip=\"" + obj.ID + "\"  tips=\"" + obj.AttributeID + "\" title=\"删除属性值\" src=\"../images/icon_del.png\" style=\"cursor: pointer;\"" +
                                                               " /></td></tr>";
                            }
                        })
                        html += "</table>";
                        $(".divedit").html(html);
                    }, error: function () {

                    }
                })
            }
            //(编辑)新增属性值
            $(".btnBg2").click(function () {
                var str = "";
                var attrs = $.trim($(".txtAttributeNames").val());
                var values = $.trim($(".txtAddValues").val());
                if (attrs == "") {
                    str = str + "- 属性不能为空。\r\n";
                }
                if (values == "") {
                    str = str + "- 属性值不能为空。\r\n";
                }
                if (str != "") {
                    layerCommon.msg(str, IconOption.错误);
                    return false;
                } else {
                    var id = $(this).attr("tip");
                    ShowAddValue(1, id, "", values);
                }
            })
            //(添加)新增属性值
            $(".btnBg1").click(function () {
                var str = "";
                var attr =stripscript( $.trim($(".txtAttributeName").val())).replace("<","");
                var value = stripscript($.trim($(".txtAddValue").val())).replace("<", "");
                if (attr == "") {
                    str = str + "- 属性不能为空。\r\n";
                }
                if (value == "") {
                    $(".txtAddValue").val(value)
                    str = str + "- 属性值不能为空。\r\n";
                }
                if (str != "") {
                    layerCommon.msg(str, IconOption.错误);
                    return false;
                } else {
                    ShowAddValue(2, $("#hideadd").val(), attr, value);
                }
            })
            //取消
            $(".tip .cancel").click(function () {
                var id = $("#hideadd").val();
                if (id != "") {
                    $.ajax({
                        type: "post",
                        data: { ck: Math.random(), action: "delattr", id: id },
                        dataType: "text",
                        success: function (data) {
                            location = location;
                        }
                    })
                }
            })
            //显示添加后的属性以及属性值
            //1代表编辑  ，2代表添加
            function ShowAddValue(type, id, attrName, value) {
                $.ajax({
                    type: "post",
                    url: "GoodsAttributeList.aspx",
                    data: { ck: Math.random(), action: "addValue", attrName: attrName, value: value, id: id, type: type },
                    dataType: "text",
                    success: function (data) {
                        if (data != "") {
                            if (data == "ycz") {
                                layerCommon.msg("属性值已存在", IconOption.错误);
                                return false;
                            } else if (data == "sxycz") {
                                layerCommon.msg("属性已存在", IconOption.错误);
                                return false;
                            } else {
                                if (type == 1) {
                                    ShowAttributeValue(id); //编辑属性的ID
                                } else {
                                    ShowAttributeValue(data); //添加属性的id
                                    $("#hideadd").val(data);
                                    if ($(".divedit tr").length > 6) {
                                        $(".addattr").css("overflow", "hidden");
                                    }
                                }
                            }
                            $(".txtAddValues").val("");
                            $(".txtAddValue").val("");
                        }
                    }, error: function () {
                    }
                })
            }
        });
    </script>
    <script type="text/javascript">
        //验证用
        function formCheck() {
            var Name = $.trim($(".txtAttributeName").val());
            var Names = $.trim($(".txtAttributeNames").val());
            var attrValue = $.trim($(".tip .divedit").html());
            var attrValues = $.trim($(".tip2 .divedit").html());
            var str = "";
            if (Name == "" && Names == "") {
                str = str + "- 属性不能为空。\r\n";
            }
            if ((attrValue == "" || attrValue == "<table style=\"width:100%;\"></table>") && (attrValues == "" || attrValues == "<table style=\"width:100%;\"></table>")) {
                str = str + "- 属性值不能为空。\r\n";
            }
            if (str == "") {
                $(".tip").fadeOut(100);
                $(".tip2").fadeOut(100);
                $(".Layer").fadeOut(200);
                return true;
            }
            else {
                layerCommon.msg(str, IconOption.错误);
                return false;
            }
        }
        // 每页显示数量非空验证
        function ChkPage() {
            if ($("#txtPageSize").val() == "") {
                layerCommon.msg("每页显示数量不能为空", IconOption.错误);
                return false;
            } else if (parseInt($("#txtPageSize").val()) == 0) {
                layerCommon.msg("每页显示数量必须大于0", IconOption.错误);
                return false;
            } else {
                $("#btnSearch").click();
            }
            return true;
        }
        //删除
        function Delete(type) {
            var bol = false;
            var chklist = $(".tablelist tbody input[type='checkbox']");
            $(chklist).each(function (index, obj) {
                if (obj.checked) {
                    bol = true;
                    return false;
                }
            })
            if (type == 1) {
                if (bol) {
                    layerCommon.confirm('确定要删除属性', function () {
                        $("#btnDel").click()
                    });

                } else {
                    layerCommon.msg("请勾选需要删除的属性", IconOption.错误);
                    return false;
                }
            }
        }
        function priceKeyup(val) {
            val.value = val.value.replace(/:/gm, '').replace(/；/gm, '');
        }
    </script>
    <style type="text/css">
        .btnBg
        {
            background: url(../../images/toolbg.gif) repeat-x;
            line-height: 26px;
            height: 26px;
            padding: 0px 7px 0px 7px;
            border: solid 1px #d3dbde;
            border-radius: 3px;
            behavior: url(../js/pie.htc);
            cursor: pointer;
            margin-left: 5px;
        }
        .btnBg { display: inline-block }
        .btnBg { *display: inline }
        .btnBg .symbol
        {
            position: relative;
            top: 4px;
            right: 3px;
        }
        .btnBg:hover
        {
            background: #fffdc3;
            border: 1px solid #f1eea2;
            color: #ff7800;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-3" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a href="../GoodsNew/GoodsAttributelist.aspx">商品属性维护</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <li class="click"><span>
                    <img src="../images/t01.png" /></span>新增</li>
                <li onclick="return Delete(1)"><span>
                    <img src="../images/t03.png" /></span>删除</li>
            </ul>
            <div class="right">
                <ul class="toolbar right ">
                    <li onclick="return ChkPage()"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                </ul>
                <ul class="toolbar3">
                    <li>属性名称:<input name="txtAttribute" runat="server" type="text" id="txtAttribute"
                        class="textBox txtAttribute" onkeyup="priceKeyup(this)" /></li>
                    <li>每页显示<input name="txtPageSize" type="text" class="textBox txtPageSize" id="txtPageSize"
                        style="width: 40px" runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />条</li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <!--信息列表 start-->
        <table class="tablelist">
            <thead>
                <tr>
                    <th class="t7">
                        <input type="checkbox" name="checkbox" id="CB_SelAll" onclick="SelectAll(this)" />
                    </th>
                    <th class="t4">
                        操作
                    </th>
                    <th class="t3">
                        属性名称
                    </th>
                    <th>
                        属性值
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptAttribute" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <div class="tc">
                                    <asp:CheckBox ID="CB_SelItem" runat="server"></asp:CheckBox>
                                    <asp:HiddenField ID="HF_Id" runat="server" Value='<%# Eval("ID") %>' />
                                </div>
                            </td>
                            <td>
                                <div class="tc">
                                    <a href="javascript:;" class="link edit" tip='<%# Eval("ID") %>'>编辑</a></div>
                            </td>
                            <td>
                                <div class="tcle">
                                    <%# Eval("AttributeName") %>
                                    <%# GetMemo(Eval("Memo"))%></div>
                            </td>
                            <td>
                                <div class="tcle">
                                    <%# GetAttributeValueList( Convert.ToInt32( Eval("ID")))%></div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <asp:Button ID="btnDel" runat="server" Text="删除" Style="display: none" OnClick="btnDel_Click" />
        <asp:Button ID="btnSearch" runat="server" Style="display: none" Text="搜索" OnClick="btnSearch_Click" />
        <!--信息列表 end-->
        <!--列表分页 start-->
        <div class="pagin">
            <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                OnPageChanged="Pager_PageChanged">
            </webdiyer:AspNetPager>
        </div>
        <!--列表分页 end-->
    </div>
    <!--遮照层-->
    <div class='Layer'>
    </div>
    <!--新增 start-->
    <div class="tip" style="display: none; width: 550px;">
        <div class="tiptop">
            <span>新增</span><a></a></div>
        <div class="tipinfo">
            <div class="lb">
                <span><i class="required">*</i>属性名称：</span><input name="txtAttributeName" runat="server"
                    maxlength="4" type="text" id="txtAttributeName" class="textBox txtAttributeName" onkeyup="priceKeyup(this)"
                    style="width: 130px;" />
                <label style="color: #aaaaaa">
                    (例如：颜色、尺码、材质；最多4个汉字)</label>
            </div>
            <div class="lb">
                <span>属性备注：</span><input name="txtMemo" runat="server" id="txtMemo" type="text" maxlength="50"
                    class="textBox txtMemo" style="width: 130px;" />
                <label style="color: #aaaaaa">
                    (备注：该属性用于什么类别)</label></div>
            <div class="lb">
                <span><i class="required">*</i>添加属性值：</span><input name="txtAddValue" id="txtAddValue" onkeyup="priceKeyup(this)"
                    maxlength="15" runat="server" type="text" class="textBox txtAddValue" style="width: 86px;" /><li
                        class="btnBg btnBg1"><i class="symbol">
                            <img src="../images/t01.png"></i>新增</li>
                <label style="color: #aaaaaa">
                    (例如：白色、黑色、40、41、PU、塑胶)</label></div>
            <%--    <div class="lb" style="height: 143px">
                <span><i class="required">*</i>属性值：</span>
                <div style="border: 1px solid #eee; width: 200px; height: 150px; margin-left: 155px;
                    margin-top: -28px;" class="divedit">
                </div>
            </div>--%>
            <div class="lb addattr" style="height: 155px;">
                <span><i class="required">*</i>属性值：</span>
                <div style="border: 1px solid #ddd; background: #fdfdfd; padding: 7px 10px; width: 190px;
                    padding-right: 55px; height: 145px; overflow-y: auto; margin-left: 155px; margin-top: -28px;"
                    class="divedit">
                </div>
            </div>
            <div class="tipbtn">
                <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return formCheck()"
                    OnClick="btnAdd_Click" />&nbsp;
                <input name="" type="button" class="cancel" value="取消" /><asp:HiddenField ID="hideadd"
                    runat="server" />
            </div>
        </div>
    </div>
    <!--新增 end-->
    <!--编辑 start-->
    <div class="tip2" style="display: none; width: 550px">
        <div class="tiptop">
            <span>编辑</span><a></a></div>
        <div class="tipinfo">
            <div class="lb">
                <span><i class="required">*</i>属性名称：</span><input name="txtAttributeNames" runat="server" onkeyup="priceKeyup(this)"
                    maxlength="4" type="text" id="txtAttributeNames" class="textBox txtAttributeNames" />
                <label style="color: #aaaaaa">
                    (例如：颜色、尺码、材质)</label>
            </div>
            <div class="lb">
                <span>属性备注：</span><input name="txtMemos" runat="server" id="txtMemos" type="text"
                    maxlength="50" class="textBox txtMemos" />
                <label style="color: #aaaaaa">
                    (备注：该属性用于什么类别)</label></div>
            <div class="lb">
                <span><i class="required">*</i>添加属性值：</span><input name="txtAddValues" type="text" onkeyup="priceKeyup(this)"
                    maxlength="50" class="textBox txtAddValues" id="txtAddValues" runat="server"
                    style="width: 86px;" /><li class="btnBg btnBg2"><i class="symbol">
                        <img src="../images/t01.png"></i>新增</li>
                <label style="color: #aaaaaa">
                    (例如：白色、黑色、40、41、PU、塑胶)</label></div>
            <div class="lb" style="height: 155px;">
                <span><i class="required">*</i>属性值：</span>
                <div style="border: 1px solid #ddd; background: #fdfdfd; padding: 7px 10px; width: 190px;
                    padding-right: 55px; height: 145px; margin-left: 155px; margin-top: -28px; overflow-y: auto;"
                    class="divedit">
                </div>
            </div>
            <div class="tipbtn">
                <asp:Button ID="btnEdit" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return formCheck()"
                    OnClick="btnEdit_Click" />&nbsp;
                <asp:HiddenField ID="hideAttrId" runat="server" />
                <input name="" type="button" class="cancel" value="取消" />
            </div>
        </div>
    </div>
    <!--编辑 end-->
    <script type="text/javascript">
        $('.tablelist tbody tr:odd').addClass('odd');
    </script>
    </form>
</body>
</html>

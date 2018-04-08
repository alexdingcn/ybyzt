<%@ Page Language="C#" AutoEventWireup="true" CodeFile="addrinfo.aspx.cs" Inherits="Distributor_newOrder_addrinfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>收货信息</title>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=201507261156" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CitysLine/JQuery-Citys-online-min.js" type="text/javascript"></script>
    <%--<script src="../../js/CitysLine/New_JQuery-Citys-online-min.js" type="text/javascript"></script>--%>
    <%--<script src="js/order.js" type="text/javascript"></script>--%>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../Company/newOrder/js/order2.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <!--收货信息 start-->
    <div class="popup po-site">
        <%--<div class="po-title">收货信息<a href="" class="close"></a></div>--%>
        <input type="hidden"  id="hidkeyID" runat="server" />
        <input type="hidden"  id="hidDisID" runat="server" />

        <div style=" height:480px; overflow: hidden;overflow-y: auto;">
        <!--收货地址 start-->
        <ul class="site-list">
            <asp:Repeater ID="rpt_addr" runat="server">
                <ItemTemplate>
                    <li><i class="dx">
                        <input type="radio" name="addr" id="checkbox-1-<%# Eval("ID") %>" value="<%# Eval("ID") %>"
                            class="regular-checkbox" />
                            <label for="checkbox-1-<%# Eval("ID") %>">
                                </label>
                        </i>
                        <label class="principal">
                            <%# Eval("principal")%></label>
                        <label class="Address">
                            <%# Eval("Address") %>
                        </label>
                        <%# Eval("isdefault").ToString() == "1" ? "<label class=\"isdeflt\">（默认地址）</label>" : ""%>
                        <label class="phone">
                            <%# Eval("phone") %></label>
                        <div class="btn">
                            <a href="javascript:;" class="bule upAddr" tip="<%# Eval("ID") %>">修改</a> 
                            <a href="javascript:;" class="bule btaddrdel" tip="<%# Eval("ID") %>">删除</a>
                            <%# Eval("isdefault").ToString() == "1" ? "<a href=\"javascript:;\" tip=\"" + Eval("ID") + "\" class=\"gray\">默认地址</a>" : "<a href=\"javascript:;\" tip=\"" + Eval("ID") + "\" class=\"bule btnisdef\">设为默认</a>"%>
                            <input type="hidden" class="pr" value="<%# Eval("Province") %>" />
                            <input type="hidden" class="ci" value="<%# Eval("City") %>" />
                            <input type="hidden" class="ar" value="<%# Eval("Area") %>" />
                        </div>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
            <li><i class="dx">
                <input type="radio" name="addr" value="addaddr" id="checkbox-1" class="regular-checkbox" />
                <label for="checkbox-1">
                </label>
            </i>使用新地址</li>
        </ul>
        <!--收货地址 end-->
        <!--添加收货地址 start-->
        <div id="addaddr" class="site-add none">
            <div class="li">
                <div class="bt">
                    收货人</div>
                <input name="" id="txtprincipal" autocomplete="off" maxlength="50" type="text" class="box" />
                <label style=" color:Red;" id="lblprincapal"></label>
            </div>
            <div class="li">
                <div class="bt">
                    手机</div>
                <input name="" id="txtphone" onkeyup="KeyInt(this)" autocomplete="off" maxlength="20" type="text" class="box" />
                <label style=" color:Red;" id="lblphone"></label>
            </div>
            <div class="li">
                <div class="bt">
                    地址</div>
                <div class="address">
                    <ul>
                        <li>
                            <%--<input type="button" id="txtProvince" class="text" onclick="beginSelect(this);" value="" />--%>
                            <%--<span class="arrow" onmousedown="beginSelect(this)"></span>--%>
                            <input type="hidden" id="hidProvince" runat="server" value="省" />
                            <select runat="server" id="ddlProvince" class="prov text" onchange="Change()"></select>
                        </li>
                        <%--<li class="prov select"></li>--%>
                    </ul>
                    <ul>
                        <li>
                            <%--<input type="button" id="txtCity" class="text" onclick="beginSelect(this);" value="" />--%>
                            <%--<span class="arrow" onmousedown="beginSelect(this)"></span>--%>
                             <input type="hidden" id="hidCity" runat="server" value="市" />
                            <select runat="server" id="ddlCity" class="city text" onchange="Change1()"></select>
                        </li>
                        <%--<li class="city select"></li>--%>
                    </ul>
                    <ul>
                        <li>
                            <%--<input type="button" id="txtArea" class="text" onclick="beginSelect(this);" value="" />--%>
                            <%--<span class="arrow" onmousedown="beginSelect(this)"></span>--%>
                            <select runat="server" id="ddlArea" class="dist text" onchange="Change4()"></select>
                            <input type="hidden" id="hidArea" runat="server" value="区" />
                        </li>
                        <%--<li class="dist select"></li>--%>
                    </ul>
                </div>
            </div>
            <div class="li">
                <div class="bt">
                    详细地址</div>
                <input name="" id="txtAddress" autocomplete="off" maxlength="100" type="text" class="box" />
                <label style=" color:Red;" id="lblAddrress"></label>
            </div>
            <div class="li">
                <a href="javascript:;" class="bule btn" id="btnSave">保存</a></div>
            <div class="blank20">
            </div>
        </div>
        <!--添加收货地址 end-->
        </div>

        <div class="po-btn">
             <span style="margin-left:400px;height:60px;line-height:50px;color:red;font-size:14px" id="addMsg"></span>
            <a href="javascript:;" class="gray-btn" id="btnCancel">取消</a> <a href="javascript:;"
                class="btn-area" id="btnConfirm">确定</a>
        </div>
    </div>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="js/ordercommon.js?v=201608170930" type="text/javascript"></script>
    <script>


        $(function () {
            //取消
            $(document).on("click", "#btnCancel", function () {
                window.parent.CloseGoods();
            });

            //确定
            $("#btnConfirm").click(function () {
                //获取radio已经选中的ID
                var rdo = $(".site-list").find("li input[name=\"addr\"]:checked");
                var id = $(rdo).val();
                if (id == undefined || id == "addaddr") {
                    $("#addMsg").html("请选择一个有效地址")
                    return false;
                }
                var principal = $.trim($(rdo).parent().siblings("label[class=\"principal\"]").text());
                var phone = $.trim($(rdo).parent().siblings("label[class=\"phone\"]").text());
                var Address = $.trim($(rdo).parent().siblings("label[class=\"Address\"]").text());

                if (id == undefined) {
                    window.parent.CloseGoods();
                    return;
                } else {
                    window.parent.addr_info(id, principal, phone, Address);
                    window.parent.CloseGoods();
                }
            });

            $(document).on("click", "input:radio[name=\"addr\"]", function () {
                var add = $(this).val();
                if (add == "addaddr") {
                    $("#addaddr").attr("class", "site-add");
                    $("#txtprincipal").val("");
                    $("#txtprincipal").attr("tip_id", "");
                    $("#txtphone").val("");
                    //$_Addr("", "", "", "");

                    //省份下拉选中
                    Selected(".prov", "");
                    //市下拉选中
                    Selected(".city", "");
                    //区县下拉选中     
                    Selected(".dist", "");
                    $("#txtAddress").val("");
                }
                else
                    $("#addaddr").attr("class", "site-add none");
            });

            //修改地址
            $(document).on("click", ".upAddr", function () {
                $("#addaddr").attr("class", "site-add");

                var ID = $.trim($(this).attr("tip"));
                var principal = $.trim($(this).parents().siblings("label[class=\"principal\"]").text());
                var phone = $.trim($(this).parents().siblings("label[class=\"phone\"]").text());
                var Address = $.trim($(this).parents().siblings("label[class=\"Address\"]").text());

                var Province = $.trim($(this).siblings("input[class=\"pr\"]").val());
                var City = $.trim($(this).siblings("input[class=\"ci\"]").val());
                var Area = $.trim($(this).siblings("input[class=\"ar\"]").val());

                $(this).parents().siblings("i[class=\"dx\"]").find("input[type=\"radio\"]").prop("checked", true);

                $("#txtprincipal").val(principal);
                $("#txtprincipal").attr("tip_id", ID);
                $("#txtphone").val(phone);
                //$_Addr(Province, City, Area, Address);

                //省份下拉选中
                Selected(".prov", Province);
                //市下拉选中
                Selected(".city", City);
                //区县下拉选中     
                Selected(".dist", Area);
                $("#txtAddress").val(Address);

                $(".site-add div.li input[type=\"text\"]").siblings("label").html("");
            });
        });

        //删除
        $(document).on("click", ".btaddrdel", function () {
            var ID = $(this).attr("tip");
            var th = this;
            
            $.ajax({
                type: 'post',
                url: '../../Handler/ShopCart.ashx',
                data: { ck: Math.random(), ActionType: "deladdr", id: ID, DisID: $.trim($("#hidDisID").val()) },
                dataType: 'json',
                success: function (data) {
                    if (data.Result) {
                        layerCommon.msg("删除地址成功", IconOption.笑脸);
                        $(th).parents().parents().remove("li");
                    } else
                        layerCommon.msg(data.Msg, IconOption.错误);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
                }
            });
        });

        //设为默认地址
        $(document).on("click", ".btnisdef", function () {
            var ID = $(this).attr("tip");
            var th = this;

            $.ajax({
                type: 'post',
                url: '../../Handler/ShopCart.ashx',
                data: { ck: Math.random(), ActionType: "isdef", id: ID, DisID: $.trim($("#hidDisID").val()) },
                dataType: 'json',
                success: function (data) {
                    if (data.Result) {
                        $(".isdeflt").remove("label");
                        $(".gray").text("设为默认");
                        $(".gray").attr("class", "bule btnisdef");

                        var ID = $.trim($(th).attr("tip"));
                        var principal = $.trim($(th).parents().siblings("label[class=\"principal\"]").text());
                        var phone = $.trim($(th).parents().siblings("label[class=\"phone\"]").text());
                        var Address = $.trim($(th).parents().siblings("label[class=\"Address\"]").text());
                        var Province = $.trim($(th).siblings("input[class=\"pr\"]").val());
                        var City = $.trim($(th).siblings("input[class=\"ci\"]").val());
                        var Area = $.trim($(th).siblings("input[class=\"ar\"]").val());

                        var str = "<li><i class=\"dx\"><input type=\"radio\" name=\"addr\" id=\"checkbox-1-" + ID + "\" value=\"" + ID + "\" class=\"regular-checkbox\" /><label for=\"checkbox-1-" + ID + "\"></label></i><label class=\"principal\">" + principal + "</label>&nbsp;<label class=\"Address\">" + Address + "</label><label class=\"isdeflt\">（默认地址）</label>&nbsp;<label class=\"phone\">" + phone + "</label><div class=\"btn\"><a href=\"javascript:;\" class=\"bule upAddr\" tip=\"" + ID + "\">修改</a><a href=\"javascript:;\" class=\"bule btaddrdel\" tip=\"" + ID + "\">删除</a><a href=\"javascript:;\" tip=\"" + ID + "\" class=\"gray btnisdef\">默认地址</a><input type=\"hidden\" class=\"pr\" value=\"" + Province + "\" /><input type=\"hidden\" class=\"ci\" value=\"" + City + "\" /><input type=\"hidden\" class=\"ar\" value=\"" + Area + "\" /></div></li>";

                        $(th).parents().parents().remove("li");
                        $("ul.site-list li:first").before(str);
                        layerCommon.msg("设置默认地址成功", IconOption.笑脸);

                    } else
                        layerCommon.msg(data.Msg, IconOption.错误);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
                }
            });
        });

        //收货地址验证
        $(".site-add div.li input[type=\"text\"]").on({
            "focus": function () {
                $(this).siblings("label").html("");
            },
            "blur": function () {
                var ltxt = "- " + $(this).siblings("div[class=\"bt\"]").text();
                ltxt += "不能为空";
                var val = $(this).val();
                if (val == "") {
                    $(this).siblings("label").html(ltxt);
               }
//              else {
//                    var tip = $(this).attr("id");
//                    if (tip == "txtphone") {
//                        var isMobile = /^0?(13[0-9]|15[012356789]|18[0-9]|14[57]|17[7])[0-9]{8}$/;
//                        if (!isMobile.test(val)) {
//                            $("#lblphone").html("- 手机号码格式不正确");
//                        }
//                    }
//                }
            }
        });

        //保存地址
        $(document).on("click", "#btnSave", function () {
            var id = $.trim($("#txtprincipal").attr("tip_id"));
            var principal =stripscript( $.trim($("#txtprincipal").val()));
            var phone = stripscript($.trim($("#txtphone").val()));
            var Address =stripscript( $.trim($("#txtAddress").val()));
            var Province = stripscript($.trim($("#hidProvince").val()));
            var City = stripscript($.trim($("#hidCity").val()));
            var Area = stripscript($.trim($("#hidArea").val()));
            var str = "";
            //判断是修改、新增地址
            var ActionType = "AddAddr";
            if (id != "") {
                ActionType = "UpAddr";
            }
            //非空判断
            if (principal == "") {
                str += "- 收货人不能为空。<br/>";
                $("#lblprincapal").html("- 收货人不能为空");
            }
            if (phone == "") {
                str += "- 手机不能为空。<br/>";
                $("#lblphone").html("- 手机不能为空");
            } 
//            else {
//                var isMobile = /^0?(13[0-9]|15[012356789]|18[0-9]|14[57]|17[7])[0-9]{8}$/;
//                if (!isMobile.test(phone)) {
//                    $("#lblphone").html("- 手机号码格式不正确");
//                    str += "- 手机号码格式不正确。<br/>";
//                }
//            }
            if (Address == "") {
                str += "- 详细地址不能为空。<br/>";
                $("#lblAddrress").html("- 详细地址不能为空");
            }
            if (str != "") {
                return false;
            }

            $.ajax({
                type: 'post',
                url: '../../Handler/ShopCart.ashx',
                data: { ck: Math.random(), ActionType: ActionType, principal: principal, phone: phone, Address: Address, Province: Province, City: City, Area: Area, id: id, DisID: $.trim($("#hidDisID").val()) },
                dataType: 'json',
                success: function (data) {
                    if (data.Result) {
                        if (id != "") {
                            //修改完成
                            var th = $("input:radio[name=\"addr\"][value=\"" + id + "\"]");
                            $(th).parent().siblings("label[class=\"principal\"]").text(principal);
                            $(th).parent().siblings("label[class=\"phone\"]").text(phone);
                            $(th).parent().siblings("label[class=\"Address\"]").text(Address);
                            $(th).parents().find("div[class=\"btn\"]").find("input[class=\"pr\"]").val(Province);
                            $(th).parents().find("div[class=\"btn\"]").find("input[class=\"ci\"]").val(City);
                            $(th).parents().find("div[class=\"btn\"]").find("input[class=\"ar\"]").val(Area);
                            $("#addaddr").attr("class", "site-add none");
                        } else {
                            //新增完成
                            var th = $("input:radio[name=\"addr\"]:checked")

                            var str = "<li><i class=\"dx\"><input type=\"radio\" name=\"addr\" id=\"checkbox-1-" + data.Code + "\" value=\"" + data.Code + "\" class=\"regular-checkbox\" /><label for=\"checkbox-1-" + data.Code + "\"></label></i><label class=\"principal\">" + principal + "</label>&nbsp;<label class=\"Address\">" + Address + "</label>&nbsp;<label class=\"phone\">" + phone + "</label><div class=\"btn\"><a href=\"javascript:;\" class=\"bule upAddr\" tip=\"" + data.Code + "\">修改</a><a href=\"javascript:;\" class=\"bule btaddrdel\" tip=\"" + data.Code + "\">删除</a><a href=\"javascript:;\" tip=\"" + data.Code + "\" class=\"bule btnisdef\">设为默认</a><input type=\"hidden\" class=\"pr\" value=\"" + Province + "\" /><input type=\"hidden\" class=\"ci\" value=\"" + City + "\" /><input type=\"hidden\" class=\"ar\" value=\"" + Area + "\" /></div></li>";

                            $(th).parents("li").eq(0).before(str);

                            $("#addaddr").attr("class", "site-add");
                            $("#txtprincipal").val("");
                            $("#txtprincipal").attr("tip_id", "");
                            $("#txtphone").val("");
                            //省份下拉选中
                            Selected(".prov", "");
                            //市下拉选中
                            Selected(".city", "");
                            //区县下拉选中     
                            Selected(".dist", "");
                            $("#txtAddress").val("");
                        }
                    } else {
                        layerCommon.msg(data.Msg, IconOption.错误);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
                }
            });

        });
    </script>
    </form>
</body>
</html>

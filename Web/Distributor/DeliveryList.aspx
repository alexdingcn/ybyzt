<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="DeliveryList.aspx.cs"
    Inherits="Distributor_DeliveryList" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title>收货地址</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/GetPhoneCode.js" type="text/javascript"></script>
    <script src="../js/CitysLine/JQuery-Citys-online-min.js" type="text/javascript"></script>
    <script src="../Company/js/CitysLine/json-array-of-city.js" type="text/javascript"></script>
    <script src="../Company/js/CitysLine/json-array-of-province.js" type="text/javascript"></script>
    <script src="../Company/js/CitysLine/json-array-of-district.js" type="text/javascript"></script>
    <script type="text/javascript">
        var isMobile = /^0?1[0-9]{10}$/;
        $(function () {
            $("#aadd").click(function () {
                $(".li1").slideToggle(500);
                $("#txtusername").val("<%=Principal %>");
                $("#txtuserphone").val("<%=Phone %>");
                $("#txtaddress").val("");
                $("#hidID").val("");
                $("#txtcode").val("");
                $.ajax({
                    url: "DeliveryList.aspx?type=add",
                    data: { disid: "<%=ID %>" },
                    success: function (result) {
                        updatecon(result, "add");
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layerCommon.msg("服务器异常，请稍后再试", IconOption.哭脸);
                    }
                });
            });
            $("#btnoff").click(function () {
                $(".li1").slideUp(500);
                $("#txtusername").val("");
                $("#txtuserphone").val("");
                $("#txtaddress").val("");
                $("#hidID").val("");
                $("#txtcode").val("");
            });
        });

        function update(id) {
            $(".li1").slideDown(500);
            document.documentElement.scrollTop = 0;
            $("#text").val("");
            $.ajax({
                url: "../Controller/DisDelivery.ashx?type=up",
                data: { updateid: id },
                success: function (result) {
                    updatecon(result);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layerCommon.msg("服务器异常，请稍后再试", IconOption.哭脸);
                }
            });
        }

        function addorupdate() {

            if ($.trim($("#txtusername").val()) == "") {
                $("#spanusername").text("-收货人不能为空").css("display", "inline-block");
                return;
            }
            if ($.trim($("#txtuserphone").val()) == "") {
                $("#spanphone").text("-收货人手机不能为空").css("display", "inline-block");
                return;
            }
            else if (!isMobile.test($.trim($("#txtuserphone").val()))) {
                $("#spanphone").text("-手机号码格式不正确").css("display", "inline-block");
                return;
            }
            if ($.trim($("#hidProvince").val()) == "" || $.trim($("#hidProvince").val()) == "省") {
                $("#spanProvince").text("-请选择省份").css("display", "inline-block");
                return;
            }
            if ($.trim($("#hidCity").val()) == "" || $.trim($("#hidCity").val()) == "市") {
                $("#spanProvince").text("-请选择市").css("display", "inline-block");
                return;
            }
            if ($.trim($("#hidArea").val()) == "" || $.trim($("#hidArea").val()) == "区") {
                $("#spanProvince").text("-请选择县").css("display", "inline-block");
                return;
            }
            if ($.trim($("#txtaddress").val()) == "") {
                $("#spanaddress").text("-详细地址不能为空").css("display", "inline-block");
                return;
            }
            if ($.trim($("#txtcode").val()) == "") {
                $("#spancode").text("-请输入手机验证码(当前手机为代理商注册手机)").css("display", "inline-block");
                return;
            }
            var address1 = $.trim($("#lbladdr1").text()) + $.trim($("#txtaddress").val());
            if ($("#hidID").val() == "") {

                $.ajax({
                    url: "../Controller/DisDelivery.ashx?type=add",
                    data: {
                        username: $.trim($("#txtusername").val()),
                        userphone: $.trim($("#txtuserphone").val()),
                        address: address1,
                        Province: $.trim($("#hidProvince").val()),
                        City: $.trim($("#hidCity").val()),
                        Area: $.trim($("#hidArea").val()),
                        code: $.trim($("#txtcode").val()),
                        user: "<%=this.UserID %>",
                        disId: "<%=this.DisID %>",
                        disphone: "<%=phones %>"
                    },
                    dataType: 'json',
                    success: function (img) {
                        if (!img.type) {
                            $("#spancode").text("-" + img.str).css("display", "inline-block");
                        }
                        else {
                            location.reload();
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layerCommon.msg("服务器异常，请稍后再试", IconOption.哭脸);
                    }
                });
            }
            else {
                $.ajax({
                    url: "../Controller/DisDelivery.ashx?type=savedis",
                    data: {
                        updateid: $("#hidID").val(),
                        username: $.trim($("#txtusername").val()),
                        userphone: $.trim($("#txtuserphone").val()),
                        address: address1,
                        Province: $.trim($("#hidProvince").val()),
                        City: $.trim($("#hidCity").val()),
                        Area: $.trim($("#hidArea").val()),
                        code: $.trim($("#txtcode").val()),
                        user: "<%=this.UserID %>",
                        disId: "<%=this.DisID %>",
                        disphone: "<%=phones %>"
                    },
                    dataType: 'json',
                    success: function (img) {
                        if (!img.type) {
                            $("#spancode").text("-" + img.str).css("display", "inline-block");
                        }
                        else {
                            location.reload();
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layerCommon.msg("服务器异常，请稍后再试", IconOption.哭脸);
                    }
                });
            }

        }


        function updatecon(result, type) {
            var update = eval('(' + result + ')');
            $("#txtusername").val($.trim(update["Principal"]));
            $("#txtuserphone").val($.trim(update["Phone"]));
            if (update["Province"] == "北京市" || update["Province"] == "上海市" || update["Province"] == "天津市" || update["Province"] == "重庆市") {
                var Value = update["Province"] + update["Area"];
            } else {
                Value = update["Province"] + update["City"] + update["Area"];
            }
            var address = update["Address"].replace(Value, "");
            $("#txtaddress").val(address);
            type != "add" && $("#hidID").val(update["ID"]);

            for (var i = 0; i < provinces.length; i++) {
                if (provinces[i].name == update["Province"]) {
                    $("#ddlProvince").val(provinces[i].code);
                    break;
                }
            }
            if (update["Province"] == "") {
                $("#ddlProvince").val(" ");
            }
            Change();
            $("#ddlProvince").trigger("change");

            $("#ddlCity option").each(function () {
                if ($(this).text() == update["City"]) {
                    $("#ddlCity").val($(this).val());
                }
            });
            Change1();
            $("#ddlCity").trigger("change");


            $("#ddlArea option").each(function () {
                if ($(this).text() == update["Area"]) {
                    $("#ddlArea").val($(this).val());
                }
            });
            Change2();
        }

    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head runat="server" />
    <div class="w1200">
        <Left:Left runat="server" ShowID="DeliveryList" />
        <div class="rightCon">
            <div class="info">
                <a id="navigation1" href="UserIndex.aspx">我的桌面</a>> <a id="navigation2" href="#"
                    class="cur">收货地址</a></div>
            <!--收货地址 start-->
            <div class="address" style="border: none;">
                <div class="btnBox">
                    <a id="aadd" href="#" class="btnOr" style="padding: 3px 15px;"><i class="addIcon"></i>
                        新增收货地址</a></div>
                <div class="li1" style="display: none">
                    <div class="zbt">
                    </div>
                    <ul class="list">
                        <li><i class="a1"><i class="required">*</i>收货人：</i><input id="txtusername" onfocus="this.value='';$('#spanusername').css('display','none');"
                            runat="server" name="" type="text" class="box" />&nbsp;<span id="spanusername" style="color: Red;"></span></li>
                        <li><i class="a1"><i class="required">*</i>手机：</i><input id="txtuserphone" onfocus="this.value='';$('#spanphone').css('display','none');"
                            runat="server" name="" type="text" class="box" />&nbsp;<span id="spanphone" style="color: Red;"></span></li>
                        <li><i class="a1"><i class="required">*</i>地址：</i><select runat="server" id="ddlProvince"
                            class="prov xl" onchange="$('#spanProvince').css('display','none');Change()">
                        </select>
                            <input type="hidden" id="hidProvince" runat="server" value="省" />
                            <select runat="server" id="ddlCity" class="city xl" onchange="$('#spanProvince').css('display','none');Change1()">
                            </select>
                            <input type="hidden" id="hidCity" runat="server" value="市" />
                            <select runat="server" id="ddlArea" class="dist xl" onchange="$('#spanProvince').css('display','none');Change2()">
                            </select>
                            <input type="hidden" id="hidArea" runat="server" value="区" />
                            &nbsp;<span id="spanProvince" style="color: Red;"></span> </li>
                        <li><i class="a1 left"><i class="required">*</i>详细地址：</i><label id="lbladdr1" class="left"
                            style="display: none; line-height: 26px; margin-right: 5px;"></label><input id="txtaddress"
                                style="width: 208px;" maxlength="100" onfocus="$('#spanaddress').css('display','none');"
                                runat="server" name="" type="text" class="box" />&nbsp;<span id="spanaddress" style="color: Red;"></span></li>
                        <li><i class="a1"><i class="required">*</i>手机验证码：</i><input style="width: 50px;"
                            id="txtcode" onfocus="$('#spancode').css('display','none');"  maxlength="6" runat="server" name=""
                            type="text" class="box2" /><i id="disphone" class="a2"><%=userphone %></i><a id="getcode"
                                href="#" style="margin-left: 20px;" onclick='getphonecode("<%=phones %>","0","修改地址","<%=this.UserID %>","<%=this.UserName %>");'
                                class="btnBl">获取验证码</a>&nbsp;<span id="spancode" style="color: Red;"></span></li>
                        <li><a href="#" onclick="addorupdate()" class="btnOr">确定</a><a id="btnoff" href="#"
                            class="btnGrey">取消</a><input id="hidID" type="hidden" value="" /></li>
                    </ul>
                </div>
                <asp:Repeater ID="rptdelivery" runat="server">
                    <ItemTemplate>
                        <div class="li2">
                            <div class="zbt">
                                <a id="update" onclick='<%# "update(\""+Eval("id")+"\")" %>' href="#">编辑地址</a><a
                                    id="defat" defatid='<%#Eval("id") %>' onserverclick='A_Defat' runat="server"
                                    href="#"><%#Eval("IsDefault").ToString()=="0"?"设为默认":"(默认地址)" %></a><a id="del" 
                                        deleteid='<%#Eval("id") %>' onserverclick="A_DLT" href="#" runat="server" class="del"></a></div>
                            <ul class="list">
                                <li><i class="a1">收货人：</i><%#Eval("Principal")%></li>
                                <li><i class="a1">手机：</i><%#Eval("Phone")%></li>
                                <li><i class="a1">省市区：</i><%#Eval("Province").ToString() + Eval("City").ToString() + Eval("Area").ToString()%></li>
                                <li><i class="a1">详细地址：</i><%#Eval("Address")%></li>
                            </ul>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <!--收货地址 end-->
        </div>
    </div>
    </form>
</body>
</html>

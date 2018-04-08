<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeliveryAdd.aspx.cs" Inherits="Company_Order_DeliveryAdd" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>新增收货地址</title>
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <link href="../Company/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <%--<link href="../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../js/layer.js" type="text/javascript"></script>
    <script src="../js/CommonJs.js" type="text/javascript"></script>--%>
     <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/CitysLine/JQuery-Citys-online-min.js" type="text/javascript"></script>
    <script src="../js/GetPhoneCode.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            //取消
            $(".cancel").click(function () {
                parent.layerCommon.layerClose("hid_Alert2");
            });

            $(".sure").click(function () {
                $(".sure").hide();
                addorupdate();
            });

            $(".dh input:text").on("blur", function () {
                switch (this.id) {
                    case "txtusername": if ($.trim($("#txtusername").val()) == "") { $("#lblusername").text("收货人不能为空"); } else { $("#lblusername").text(""); } break;
                    case "txtuserphone":
                        if ($.trim($("#txtuserphone").val()) == "") {
                            $("#lbluserphone").text("收货人手机不能为空");
                        } else {
                            var isMobile = /^0?1[0-9]{10}$/;
                            if (!isMobile.test($.trim($("#txtuserphone").val()))) {
                                $("#lbluserphone").text("手机号码格式不正确");
                            } else {
                                $("#lbluserphone").text("");
                            }
                        } break;
                    case "txtaddress": if ($.trim($("#txtaddress").val()) == "") {
                            $("#lbladdress").text("详细地址不能为空");
                        } else {
                            var addressName = $.trim($("#hidProvince").val()) + $.trim($("#hidCity").val()) + $.trim($("#hidArea").val())
                            if ($.trim($("#txtaddress").val()).toString() == addressName) {
                                $("#lbladdress").text("请修改详细地址");
                             } else {
                                $("#lbladdress").text("");
                            }
                        } break;
                    case "txtcode":
                        if ($.trim($("#txtcode").val()) == "") {
                             $("#spancode").text("请输入手机验证码(当前手机为代理商注册手机)");
                         }else{
                            $("#spancode").text("");
                         }
                        break;
                }
            });
        });

        function addorupdate() {
            var isMobile = /^0?1[0-9]{10}$/;
            var str = "";
            var lusername = "";
            var luserphone = "";
            var laddress = "";
            var lProvince = "";
            //if ($.trim($("#txtdlename").val()) == "") {
            //    str = "-别名不能为空(给地址一个别名,方便下单等操作选择).\r\n";
            //}
            if ($.trim($("#txtusername").val()) == "") {
                str += "-收货人不能为空.\r\n";
                lusername = "收货人不能为空";
            }
            if ($.trim($("#txtuserphone").val()) == "") {
                str += "-收货人手机不能为空.\r\n";
                luserphone = "收货人手机不能为空";
            }
            else if (!isMobile.test($.trim($("#txtuserphone").val()))) {
                str += "-手机号码格式不正确.\r\n";
                luserphone = "手机号码格式不正确";
            }
            if ($.trim($("#hidProvince").val()) == "" || $.trim($("#hidProvince").val()) == "选择省") {
                str += "-请选择省份.\r\n";
                lProvince += "请选择省份";
            }
            if ($.trim($("#hidCity").val()) == "" || $.trim($("#hidCity").val()) == "选择市") {
                str += "-请选择市.\r\n";
                lProvince += "-请选择市";
            }
            if ($.trim($("#hidArea").val()) == "" || $.trim($("#hidArea").val()) == "选择县") {
                str += "-请选择县.\r\n";
                lProvince += "-请选择县";
            }
            var address1="";
            var addressName = $.trim($("#hidProvince").val()) + $.trim($("#hidCity").val()) + $.trim($("#hidArea").val())
            if ($.trim($("#txtaddress").val()).toString() == addressName || $.trim($("#txtaddress").val()) == "") {
                if ($.trim($("#txtaddress").val()) == "") {
                    str += "-详细地址不能为空.\r\n";
                    laddress = "详细地址不能为空";
                } else {
                    str += "-请修改详细地址.\r\n";
                    laddress = "请修改详细地址";
                 }
                 
            }
            if($.trim($("#lbladdress1").text())!="")
            {
                address1=$.trim($("#lbladdress1").text())+$.trim($("#txtaddress").val());
            }
             if ($.trim($("#txtcode").val()) == "") {
                $("#spancode").text("请输入手机验证码(当前手机为代理商注册手机)");
                str += "-请输入手机验证码.\r\n";
            }

            $("#lblusername").text(lusername);
            $("#lbluserphone").text(luserphone);
            $("#lblProvince").text(lProvince);
            $("#lbladdress").text(laddress);

            if (str != "") {
                //errMsg("提示", str, "", "");
                $(".sure").show();
                return false;
            } else {
                $.ajax({
                    url: "../Controller/DisDelivery.ashx?type=add",
                    data: {
                        userid: <%=this.UserID %>,
                        username:$("#txtusername").val(),
                        userphone:$("#txtuserphone").val(),
                        Province:$("#hidProvince").val(),
                        City:$("#hidCity").val(),
                        Area:$("#hidArea").val(),
                        address:address1,
                        code: $.trim($("#txtcode").val()),
                        user: <%=this.UserID %>,
                        disId: <%=this.DisID%>,
                        disphone: "<%=this.Phone %>"
                    },
                    dataType: 'json',
                    success: function (img) {
                        if (!img.type) {
                            $("#spancode").text(img.str).css("display","inline-block");
                            $(".sure").show();
                        }
                        else {
                            //location.reload();
                            //window.parent.CloseDialogAddAddr();
                            var address = address1;
                            var id=img.str;
                            window.parent.getAddress(address,id);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                         layerCommon.msg("服务器异常，请稍后再试", IconOption.错误);
                        $(".sure").show();
                    }
                });
            }
        }

        function Change2() {
            var provchange = $(".prov option:selected").text();
            var citychange = $(".city option:selected").text();
            var distchange = $(".dist option:selected").text();
            var distchange2 = $(".dist option:selected").val();
            $("#hidProvince").val(provchange);
            $("#hidCity").val(citychange);
            $("#hidArea").val(distchange);
            $("#hidCode").val(distchange2);
            if ($("#hidProvince").val() == "北京市" || $("#hidProvince").val() == "上海市" || $("#hidProvince").val() == "天津市" || $("#hidProvince").val() == "重庆市") {
                $("#lbladdress1").text(provchange + distchange).css("display","block");
                $("#lbladdress").text("");
            }
            else {
                $("#lbladdress1").text(provchange + citychange + distchange).css("display","block");
                $("#lbladdress").text("");
            }
            $("#lblProvince").text("");
        }
    </script>
    <style>
        body
        {
            font-family: "微软雅黑";
            margin: 0 auto;
            min-width: 540px;
        }
        .dh td label
        {
            color: red;
            display: inline-block;
            font-style: normal;
            padding-right: 5px;
            position: relative;
            top: 3px;      
        }
        .btnBl
        {
            min-height: 22px;
            min-width: 20px;
            background: #33a3cd;
            border-radius: 8px;
            height: 22px;
            line-height: 22px;
            margin-top: 4px;
            color: #fff;
            display: inline-block;
            padding: 0px 15px;
            text-decoration: none;
        }
        .btnBl:visited {
            background: #33a3cd none repeat scroll 0 0;
        }
        .btnBl:hover
        {
            background: #ff8106;
            text-decoration: none;
            color: #fff;    
        }
    </style>
</head>
<body class="root3">
    <form id="form1" runat="server" style="width: 680px; height: 245px; ">
    <div class="rightinfo" style=" width:680px; height:245px; margin: 0px 0px; width:auto; ">
        <div class="div_content">
             <div class="lbtb">
                <table class="dh">
                    <tr>
                        <td>
                            <span><label class="required">*</label>收货人</span>
                        </td>
                        <td>
                            <input id="txtusername" runat="server" name="" type="text" class="textBox" />
                            <label id="lblusername"></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><label class="required">*</label>手机</span>
                        </td>
                        <td>
                            <input id="txtuserphone" runat="server" name="" readonly="readonly" type="text" class="textBox" />
                            <label id="lbluserphone"></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><label class="required">*</label>地址</span>
                        </td>
                        <td>
                            <select  runat="server" id="ddlProvince" class="prov xl textBox" style=" width:120px;" onchange="Change()" >
					        </select>
                                <input type="hidden" id="hidProvince" runat="server"  value="" />
					        <select runat="server" id="ddlCity" class="city xl textBox" style=" width:120px;"  onchange="Change1()">
					        </select>
                                <input type="hidden" id="hidCity" runat="server" value="" />
					        <select runat="server" id="ddlArea" class="dist xl textBox" style=" width:120px;"  onchange="Change2()">
					        </select>
                            <input type="hidden" id="hidArea" runat="server" value="" />

                            <label id="lblProvince"></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><label class="required">*</label>详细地址</span>
                        </td>
                        <td>
                            <label id="lbladdress1" style="display:none; color:#000; float:left;"></label>
                            <input id="txtaddress" style="width:208px; float:left;" runat="server" maxlength="100" name="" type="text" class="textBox" />
                            <label id="lbladdress"></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><label class="required">*</label>验证码</span>
                        </td>
                        <td>
                            <input style="width:50px;" id="txtcode" onfocus="$('#spancode').css('display','none');" runat="server" name="" type="text" class="textBox" />
                            <i id="disphone" class="a2"><%=this.Phone %></i>
                            <a id="getcode" href="#" style="margin-left:20px;" onclick='getphonecode("<%=this.Phone %>","0","修改地址","<%=this.ID %>","<%=this.UserName %>");' class="btnBl">发送验证码</a>
                            <label id="spancode"></label>
                        </td>
                    </tr>
                </table>
             </div>

             <!--清除浮动-->
            <div style="clear: none;"></div>

             <div class="footerBtn">
                <input type="button" class="sure" value="确定"/>
                <input name=""  id="btnoff" type="button" class="cancel" value="取消" />
                <input id="hidID" type="hidden" runat="server" value="" />
             </div>
        </div>
    </div>
    <!--收货地址 start-->
    <%--<div class="address">
        <div class="li1">
            <ul class="list">
                <li><i class="a1"><span class="required">*</span>地址别名：</i><input id="txtdlename"
                    runat="server" name="" type="text" class="box" />&nbsp;<label id="dlenamestr"></label>（例如：默认地址）</li>
                <li><i class="a1"><span class="required">*</span>收货人：</i><input id="txtusername"
                    runat="server" name="" type="text" class="box" />&nbsp;<label id="usernamestr"></label>（默认代理商联系人）</li>
                <li><i class="a1"><span class="required">*</span>手机：</i><input id="txtuserphone"
                    runat="server" name="" type="text" class="box" />（默认代理商联系人手机）</li>
                <li><i class="a1"><span class="required">*</span>地址：</i><select runat="server" id="ddlProvince"
                    class="prov xl" onchange="Change()">
                </select>
                    <input type="hidden" id="hidProvince" runat="server" value="" />
                    <select runat="server" id="ddlCity" class="city xl" onchange="Change1()">
                    </select>
                    <input type="hidden" id="hidCity" runat="server" value="" />
                    <select runat="server" id="ddlArea" class="dist xl" onchange="Change2()">
                    </select>
                    <input type="hidden" id="hidArea" runat="server" value="" />
                </li>
                <li><i class="a1"><span class="required">*</span>详细地址：</i><input id="txtaddress"
                    style="width: 308px;" runat="server" name="" type="text" class="box" />
                    （详细发货地址）</li>
            </ul>
        </div>
        <div style=" margin-left:120px;">
            <a href="javascript:void(0);" class="sure">保存</a> 
            <a id="btnoff" href="javascript:void(0);" class="cancel">取消</a>
            <input id="hidID" type="hidden" value="" runat="server" />
        </div>
    </div>--%>
    </form>
</body>
</html>

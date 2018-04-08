<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayAccountList.aspx.cs" Inherits="Company_Pay_PayAccountList" %>

<%@ Register TagPrefix="uc1" TagName="SelectComp" Src="~/Admin/UserControl/TextCompList.ascx" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>收款账户</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../js/Pay.js" type="text/javascript"></script>
    <script src="../js/CitysLine/JQuery-Citys-online-min.js" type="text/javascript"></script>
    <script type="text/javascript">
        //回车事件
        //        $(function () {
        //            $(document).on("keydown", function (e) {
        //                if (e.keyCode == 13) {
        //                    $("#btnSearch").trigger("click");
        //                }
        //            })
        //        })


        //保存方法
        function BtnSave() {

            var str = "";

            /* 银行账户设置  start ***********************************/
            var chkIsno = -1;

            var ddltype = stripscript($.trim($("#ddltype").val())); //账户类型
            var txtDisUser = stripscript($.trim($("#txtDisUser").val())); //户名
            var ddlbank = stripscript($.trim($("#ddlbank").val())); //开户银行
            var txtbankcode =stripscript( $.trim($("#txtbankcode").val())); //帐号
            var SltPesontype =stripscript( $.trim($("#SltPesontype").val())); //证件类型
            var txtpesoncode =stripscript( $.trim($("#txtpesoncode").val())); //证件号码
            var hidProvince =stripscript( $("#hidProvince").val()); //省
            var hidCity = stripscript($("#hidCity").val()); //市
            var hidArea = stripscript($("#hidArea").val()); //区
            var txtbankAddress = stripscript($.trim($("#txtbankAddress").val())); //开户行地址

            //是否启用
            var al = $('#chkIsno').is(':checked');
            if ($('#chkIsno').is(':checked')) {
                chkIsno = 1;

                if (ddltype == "-1")
                    str += "请选择账户类型！";
                else {
                    if (ddltype == "11") {//个人帐号
                        if (SltPesontype == "-1") {
                            str += "请填写证件类型！";
                        }
                        if (txtpesoncode == "") {
                            str += "请填写证件号码！";
                        }
                    } else {//企业账户
                        if (txtDisUser.length < 6) {
                            str += "-户名不正确，请检查！";
                        }
                    }
                }
                if (txtDisUser == "")
                    str += "请填写户名！";
                if (ddlbank == "-1")
                    str += "请选择开户银行！";
                if (txtbankcode == "")
                    str += "请填写帐号！";
                if (hidProvince == "")
                    str += "请填写省！";
                if (hidCity == "")
                    str += "请填写市！";
                if (hidArea == "")
                    str += "请填写区！";
                if (txtbankAddress == "")
                    str += "请填写开户行地址！";

                if (str != "") {
                    //errMsg("提示", str, "", "");
                    layerCommon.msg(str, IconOption.错误);
                    return false;

                }

                // ajax  ---begin
                //            var mes = 0;
                //            $.ajax({
                //                type: 'POST',
                //                async: false,
                //                url: '../../Handler/Tx2310.ashx',
                //                data: { ddltype: ddltype, ddlbank: ddlbank, SltPesontype: SltPesontype, txtpesoncode: txtpesoncode, txtDisUser: txtDisUser, txtbankcode: txtbankcode, chkisno: 0 },
                //                success: function (data) {
                //                    var resdata = jQuery.parseJSON(data);

                //                    if (resdata.ID != "2000") {
                //                        str = resdata.msg;
                //                    }

                //                }
                //            });

                //ajax   end
            }
            /* 银行账户设置  end *************************************/

            /* 微信支付账户设置  start***********************************/
            var wx_chisno = -1;
            //手续费比例
            var appid = stripscript($.trim($("#appid").val())); //appid
            var appsecret = stripscript($.trim($("#appsecret").val())); //appsecret
            var mchid = stripscript($.trim($("#mchid").val())); //mchid
            var key = stripscript($.trim($("#key").val())); //key
            //是否启用
            if (document.getElementById("wx_chisno").checked) {
                wx_chisno = 1;

                if (document.getElementById("wx_chisno").checked) {
                    if (appid == "") {
                        str += "请填写微信AppID！";
                    }
                    if (mchid == "") {
                        str += "请填写微信MCHID！";
                    }
                    if (key == "") {
                        str += "请填写微信支付APPKey！";
                    }
                }
            }
            /* 微信支付账户设置  end *****************************/

            /* 支付宝支付账户设置 start *************************/
            var ali_chkisno = -1;
            //手续费比例
            var seller_email =stripscript( $.trim($("#seller_email").val())); //appid
            var partner =stripscript( $.trim($("#partner").val())); //appsecret
            var PayKey =stripscript( $.trim($("#PayKey").val())); //mchid
            var alirsa =stripscript( $.trim($("#alirsa").val()));
            //是否启用
            if (document.getElementById("ali_chisno").checked) {
                ali_chkisno = 1;



                if (document.getElementById("ali_chisno").checked) {
                    if (seller_email == "") {
                        str += "请填写支付宝企业账户！";
                    }
                    if (partner == "") {
                        str += "请填写支付宝合作者身份！";
                    }
                    if (PayKey == "") {
                        str += "请填写支付宝安全验证码！";
                    }
                    // if (alirsa == "")
                    //    str += "请填写RSA秘钥！";
                }
            }
            /* 支付宝支付账户设置 start *************************/

            if (str != "") {
                //errMsg("提示", str, "", "");
                layerCommon.msg(str, IconOption.错误);
                return false;

            }

            $.ajax({
                type: 'post',
                url: 'PayAccountList.aspx?action=sett',
                data: { chkIsno: chkIsno, ddltype: ddltype, txtDisUser: txtDisUser, ddlbank: ddlbank, txtbankcode: txtbankcode, SltPesontype: SltPesontype, txtpesoncode: txtpesoncode, hidProvince: hidProvince, hidCity: hidCity, hidArea: hidArea, txtbankAddress: txtbankAddress, wx_chisno: wx_chisno, appid: appid, appsecret: appsecret, mchid: mchid, key: key, ali_chkisno: ali_chkisno, seller_email: seller_email, partner: partner, PayKey: PayKey, alirsa: alirsa },
                async: false, //true:同步 false:异步
                success: function (result) {
                    var data = eval('(' + result + ')');

                    layerCommon.msg(data["prompt"].toString(), IconOption.笑脸);
                    location.reload();
                }
            });
        }



        //只能输入数字验证
        function KeyInt(val) {
            val.value = val.value.replace(/[^\d]/g, '');
        }

        //选择银行卡返回
        function CloseBank() {
            // layerCommon.layerClose();
            layerCommon.layerCloseAll();
        }

        //选择银行卡返回
        function SelectBankReturn(materialCodes, bankName) {
            if (materialCodes != "") {
                form1.txtbankcodes.value = materialCodes;
                form1.txtbandname.value = bankName;
                //document.getElementById("btnSelectBank").click();
                var objselect = document.getElementById("ddlbank");
                jsAddItemToSelect(objselect, materialCodes, bankName);
                CloseBank();
            }
        }

        //选择银行
        function Otypechange(ower) {
            if (ower == "20000") {
                $("#ddlbank").val("-1");
                var dealerId = 1;
                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度
                var index = layerCommon.openWindow('选择银行', 'PaySelectBank.aspx', '1000px', '480px'); //记录弹出对象
                $("#hid_alerts").val(index); //记录弹出对象
            }
        }

        //账户类型显示
        function Mackchange(ower) {
            if (ower == "11") {
                $("#tbdis").show();
                $("#SltPesontype").val(0);
            } else {
                $("#tbdis").hide();
            }
        }

        //输入汉字自动替换为空,允许输入字母或者数字
        function KeyIntCheck(val) {
            val.value = val.value.replace(/([^\u0000-\u00FF])/g, '');
        }
        //修改按钮事件
        function btnUpdate() {
            $("#btnSave").show();
            $("#btnUpdate").hide();
            $("#btnEsc").show();

            $('input').removeAttr("disabled");
            $("input").removeClass("noBox");

            $('select').removeAttr("disabled");
            $("select").removeClass("noBox");

        }
        //取消
        function btnEsc() {
            location.reload();
        }

        // 1.判断select选项中 是否存在Value="paraValue"的Item 
        function jsSelectIsExitItem(objSelect, objItemValue) {
            var isExit = false;
            for (var i = 0; i < objSelect.options.length; i++) {
                if (objSelect.options[i].value == objItemValue) {
                    isExit = true;
                    break;
                }
            }
            return isExit;
        }
        // 2.向select选项中 加入一个Item 
        function jsAddItemToSelect(objSelect, objItemText, objItemValue) {
            //判断是否存在 
            if (jsSelectIsExitItem(objSelect, objItemValue)) {
                alert("该Item的Value值已经存在");
            } else {
                var varItem = new Option(objItemValue, objItemText);
                objSelect.options.add(varItem);              
                //$(objSelect).find("option:first").insertBefore(str);
                $("#ddlbank").find("option[value='" + objItemText + "']").attr("selected", true);
              //  alert("成功加入");
            }
        }
    </script>
    <style type="text/css"></style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:CompRemove runat="server" ID="Remove" ModuleID="4" />
    <uc1:Top ID="top1" runat="server" ShowID="nav-6" />
    <!--代理商搜索 Begin-->
    <input id="hid_Alerts" type="hidden" />
    <!--代理商搜索 End-->
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a href="../Pay/PayAccountList.aspx" runat="server" id="atitle">收款账户</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <div id="DisBoot" class="tools">
            <ul class="toolbar left">
                <li id="btnUpdate" runat="server" onclick="btnUpdate()"><span>
                    <img src="../images/t02.png" /></span><font>修改</font></li>
                <li id="btnSave" runat="server" onclick="BtnSave()"><span>
                    <img src="../images/t15.png" /></span><font>确定</font></li>
                <li id="btnEsc" style="display: none" runat="server" onclick="btnEsc()"><span>
                    <img src="../images/t03.png" /></span><font>取消</font></li>
            </ul>
        </div>
        <div class="receivables">
            <!--银行账户 start-->
            <div class="recd-title">
                <h3 class="left">
                    银行账户</h3>
                <div class="doubt left">
                    <i class="doubt-i"></i>
                    <div class="txt">
                        <i class="trian-i"></i><i class="trian-i trian-i2"></i>
                        <p>
                            1、企业账户是针对企业而言，是凭借公司工商登记证、税务登记证和组织机构代码证等在银行开设的账户；个人帐户是针对私人而言，是凭借个人身份证在银行开设的账户。</p>
                        <p>
                            2、请务必校验收款账户信息，如果填错，可能导致账款不能及时到账。</p>
                        <p>
                            3、若账户填错，系统会自动原路退回，或者平台工作人员会在T+1工作日后通知收款方修改账户后重新结算。</p>
                    </div>
                </div>
                <div class="text">
                    <input name="chkIsno" id="chkIsno" runat="server" type="checkbox" value="" class="fx" />是否启用&nbsp;&nbsp;&nbsp;&nbsp;<i class="rcolor">(使用银行卡做为收款账户，避免使用存折账户)</i></div>
            </div>
            <div class="recd-box">
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="recd-box">
                    <tr>
                        <td class="t1">
                            <div class="name">
                                <i class="rcolor">*</i>账户类型</div>
                        </td>
                        <td>
                            <%--<select name="" class="xz noBox"><option>账户类型</option></select>--%>
                            <select id="ddltype" runat="server" class="xz" onchange="Mackchange(this.options[this.selectedIndex].value)">
                                <option value="-1">请选择</option>
                                <option value="11">个人账户</option>
                                <option value="12">企业账户</option>
                            </select>
                        </td>
                        <td class="t1">
                            <div class="name">
                                <i class="rcolor">*</i> 户名</div>
                        </td>
                        <td>
                            <input name="txtDisUser" id="txtDisUser" runat="server" type="text" class="box" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="name">
                                <i class="rcolor">*</i>开户行</div>
                        </td>
                        <td>
                            <%--<select name="" class="xz noBox"></select>--%>
                            <select id="ddlbank" runat="server" onchange="Otypechange(this.options[this.selectedIndex].value)"
                                class="xz">
                                <option value="-1">请选择</option>
                                <option value="1405">广州农村商业银行</option>
                                <option value="102">中国工商银行</option>
                                <option value="103">中国农业银行</option>
                                <option value="104">中国银行</option>
                                <option value="105">中国建设银行</option>
                                <option value="301">交通银行</option>
                                <option value="100">邮储银行</option>
                                <option value="303">光大银行</option>
                                <option value="305">民生银行</option>
                                <option value="306">广发银行</option>
                                <option value="302">中信银行</option>
                                <option value="310">浦发银行</option>
                                <option value="309">兴业银行</option>
                                <option value="401">上海银行</option>
                                <option value="403">北京银行</option>
                                <option value="307">平安银行</option>
                                <option value="308">招商银行</option>
                                <option value="20000" style="color: Blue;">更多银行</option>
                            </select>
                        </td>
                        <td>
                            <div class="name">
                                <i class="rcolor">*</i> 账 号</div>
                        </td>
                        <td>
                            <input name="txtbankcode" id="txtbankcode" runat="server" type="text" class="box"
                                onkeyup="this.value=this.value.replace(/\D/g,'');" />
                        </td>
                    </tr>
                    <tr id="tbdis" runat="server">
                        <td>
                            <div class="name">
                                <i class="rcolor">*</i>证件类型</div>
                        </td>
                        <td>
                            <%--<select name="" class="xz"></select>--%>
                            <select id="SltPesontype" runat="server" class="xz">
                                <option value="-1">请选择</option>
                                <option value="0">身份证</option>
                                <option value="1">户口薄</option>
                                <option value="2">护照</option>
                                <option value="3">军官证</option>
                                <option value="4">士兵证</option>
                                <option value="5">港澳居民来往内地通行证</option>
                                <option value="6">台湾同胞来往内地通行证</option>
                                <option value="7">临时身份证</option>
                                <option value="8">外国人居留证</option>
                                <option value="9">警官证</option>
                                <option value="x">其他证件</option>
                            </select>
                        </td>
                        <td>
                            <div class="name">
                                <i class="rcolor">*</i> 证件号码</div>
                        </td>
                        <td>
                            <input name="txtpesoncode" id="txtpesoncode" runat="server" maxlength="18" type="text"
                                class="box" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="name">
                                <i class="rcolor">*</i>开户所在省/市/区</div>
                        </td>
                        <td colspan="3">
                            <%--<div class="left">--%>
                            <%--<select name="" class="xz2"></select></div><div class="left">
            <select name="" class="xz2"></select></div><div class="left">
            <select name="" class="xz2"></select></div>--%>
                            <select runat="server" id="ddlProvince" class="prov select1 l xz2" onchange="Change()"
                                style="margin-right: 0px;">
                            </select>
                            <input type="hidden" id="hidProvince" runat="server" value="请选择省份" />
                            <select runat="server" id="ddlCity" class="city select xz2" onchange="Change()">
                            </select>
                            <input type="hidden" id="hidCity" runat="server" value="请选择市" />
                            <select runat="server" id="ddlArea" class="dist select xz2" onchange="Change()">
                            </select>
                            <input type="hidden" id="hidArea" runat="server" value="请选择区" />
                            <input type="hidden" id="hidCode" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="name">
                                <i class="rcolor">*</i>开户行地址</div>
                        </td>
                        <td colspan="3">
                            <input name="txtbankAddress" id="txtbankAddress" runat="server" type="text" class="box box2" />
                        </td>
                    </tr>
                </table>
            </div>
            <!--银行账户 end-->
            <div class="blank20">
            </div>
            <!--微信账户 start-->
            <div class="recd-title none">
                <h3 class="left">
                    微信账户</h3>
                <div class="doubt left">
                    <i class="doubt-i"></i>
                    <div class="txt">
                        <i class="trian-i"></i><i class="trian-i trian-i2"></i>
                        <p>
                            APPID（应用ID）：获取路径：公众平台-开发者中心。<br />
                            APPSecret（应用秘钥）：获取路径：公众平台-开发者中心。<br />
                            Mchid（商户号）：获取路径：公众平台-微信支付。<br />
                            Appkey（API秘钥）：获取路径：公众平台-API安全-API秘钥。<br />
                            <i style="color: Blue">以上信息在“在微信支付开通成功的邮件中可获取到”</i></p>
                        <p>
                            为避免因微信支付收 款帐号设置错误影响 您正常收款甚至导致 不必要的损失。 请在设置好收款帐号后，第一时间使用订货方帐号登录医站通, 提交一笔小额的订单，
                            并使用微信支付。</p>
                    </div>
                </div>
                <div class="text">
                    <input name="chisno" id="wx_chisno" runat="server" type="checkbox" value="" class="fx" />是否启用</div>
            </div>
            <div class="recd-box none">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="t1">
                            <div class="name">
                                <i class="rcolor">*</i> AppID(应用ID)</div>
                        </td>
                        <td>
                            <input type="text" id="appid" onkeyup="KeyIntCheck(this);" onblur="KeyIntCheck(this);"
                                class="box" maxlength="40" runat="server" />
                        </td>
                        <td class="t1">
                            <div class="name ">
                                AppSecrect(应用秘钥)</div>
                        </td>
                        <td>
                            <input type="text" id="appsecret" onkeyup="KeyIntCheck(this);" onblur="KeyIntCheck(this);"
                                class="box" maxlength="60" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="name">
                                <i class="rcolor">*</i> Mchid(商户号)</div>
                        </td>
                        <td>
                            <input type="text" id="mchid" onkeyup="KeyIntCheck(this);" onblur="KeyIntCheck(this);"
                                class="box" maxlength="40" runat="server" />
                        </td>
                        <td>
                            <div class="name">
                                <i class="rcolor">*</i> APPKey(API秘钥)</div>
                        </td>
                        <td>
                            <input type="text" id="key" onkeyup="KeyIntCheck(this);" onblur="KeyIntCheck(this);"
                                class="box" maxlength="60" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <!--微信账户 end-->
            <%--<div class="blank20">
            </div>
            <div class="blank10">
            </div>--%>
            <!--支付宝帐号 start-->
            <div class="recd-title none">
                <h3 class="left">
                    支付宝帐号</h3>
                <div class="doubt left">
                    <i class="doubt-i"></i>
                    <div class="txt">
                        <i class="trian-i"></i><i class="trian-i trian-i2"></i>
                        <p>
                            1、支付宝企业帐号：帐号必须是企业支付宝账户、并通过企业实名认证。<br />
                            2、在访问支付宝商户服务中心（b.alipay.com）,用您的签约支付宝帐号登陆。<br />
                            在“我的商家服务”中，点击“查询PID、Key、RSA秘钥（RSA秘钥手机app支付时必填，电脑扫描支付不需要此秘钥）”，将相应信息填写到设置收款帐号的，对应输入框中。<br />
                        </p>
                        <p>
                            为避免因支付宝支付收款帐号设置错误影响您正常收款甚至导致不必要的损失。请在设置好收款帐号后，第一时间使用订货方帐号登录my1818.com, 提交一笔小额的订单，并使用支付宝支付。</p>
                    </div>
                </div>
                <div class="text">
                    <input name="ali_chisno" id="ali_chisno" runat="server" type="checkbox" value=""
                        class="fx" />是否启用</div>
            </div>
            <div class="recd-box none">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="t1">
                            <div class="name">
                                <i class="rcolor">*</i> 支付宝企业账户</div>
                        </td>
                        <td>
                            <input type="text" id="seller_email" onkeyup="KeyIntCheck(this);" onblur="KeyIntCheck(this);"
                                class="box" maxlength="40" runat="server" />
                        </td>
                        <td class="t1">
                            <div class="name">
                                <i class="rcolor">*</i> 合作者身(Partner ID)</div>
                        </td>
                        <td>
                            <input type="text" id="partner" onkeyup="KeyIntCheck(this);" onblur="KeyIntCheck(this);"
                                class="box" maxlength="40" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="name">
                                <i class="rcolor">*</i>安全校验码（Key）</div>
                        </td>
                        <td>
                            <input type="text" id="PayKey" onkeyup="KeyIntCheck(this);" onblur="KeyIntCheck(this);"
                                class="box" maxlength="40" runat="server" />
                        </td>
                        <td>
                            <div class="name">
                                RSA秘钥(app支付必填)</div>
                        </td>
                        <td>
                            <input type="text" id="alirsa" onkeyup="KeyIntCheck(this);" onblur="KeyIntCheck(this);"
                                class="box" maxlength="300" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <!--支付宝帐号 end-->

            <div class="recd-title">
                <h3 class="left" style=" width:230px;"> 手续费（收款方付手续费）：</h3>
            </div>
            <div class="recd-box ">
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="pay-box">
                    <tr>
                        <td>
                            <div class="name">支付方式</div>
                        </td>
                        <td>
                            <div class="name">手续费</div>
                        </td>
                        
                        
                    </tr>
                    <tr>
                        <td>
                            <div class="name">B2B</div>
                        </td>
                        <td>
                            <div class="name"><b>10元/笔</b></div>
                        </td>
                       
                        
                    </tr>
                    <tr>
                        <td>
                            <div class="name">B2C网银</div>
                        </td>
                        <td>
                            <div class="name"><b>2‰</b></div>
                        </td>
                       
                        
                    </tr>
                    <tr>
                        <td>
                            <div class="name">B2C快捷</div>
                        </td>
                        <td>
                            <div class="name"><b>5‰</b></div>
                        </td>
                       
                        
                    </tr>
                    <tr>
                        <td>
                            <div class="name">扫码支付</div>
                        </td>
                        <td>
                            <div class="name"><b>5.5‰</b></div>
                        </td>
                       
                        
                    </tr>
                    <tr>
                        <td>
                            <div class="name">APP跳转支付</div>
                        </td>
                        <td>
                            <div class="name"><b>7‰</b></div>
                        </td>
                        
                        
                    </tr>
                </table>
            </div>
	    <div class="blank10"></div>
	    <div class="recd-title"><span class="t left">结算方式：</span><span class="t2">T+1 节假日不算，一般是付款后第二个工作日15：00 到账</span></div>
        </div>
        <input type="button" id="btnSelectBank" runat="server" onserverclick="btnSelectBankReturn_ServerClick"
            style="display: none;" />
        <input type="button" id="btnUpdateclik" runat="server" onserverclick="btnUpdate_ServerClick"
            style="display: none;" />
        <input id="txtbankcodes" type="hidden" name="txtbankcodes" runat="server" />
        <input id="txtbandname" type="hidden" name="txtbandname" runat="server" />
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PAbankEdit.aspx.cs" Inherits="Company_Pay_PAbankEdit" %>

<%@ Register Src="~/Company/UserControl/DisAreaTreeBox.ascx" TagPrefix="uc1" TagName="DisArea" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>销售管理</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" /> 
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs1.js" type="text/javascript"></script>
   <script src="../../js/pay.js" type="text/javascript"></script>
    <script src="../js/CitysLine/JQuery-Citys-online-min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
             $_def.ID = "btnSave";

            if ('<%=Request["nextstep"] %>' == "1") {
                document.getElementById("imgmenu").style.display = "block";
            }
            $("#btnDis").click(function () {
                //$(".tip2").fadeIn(200);
                //$(".opacity").fadeIn(200);
                var dealerId = 1;
                var code = $("#txtGoodsCodes").val(); //查询条件

                var name = $("#txtDisUser").val();
                var bankcode = $("#txtbankcode").val();
                var index=layerCommon.openWindow('选择代理商', 'PaySelectDisList.aspx?ReturnFunc=SelectMaterialsReturn&Store=Store&name=' + name + '&bankcode=' + bankcode + '&code=' + code, '750px', '380px');
                $("#hid_Alert").val(index); //记录弹出对象
            });

            $("#txtbankcode").keyup(function () {
                $(this).val($(this).val().replace(/\D/g, '').replace(/....(?!$)/g, '$& '));
            });
        });

        //选择代理商返回
        function SelectMaterialsReturn(materialCodes) {
            Close();
            if (materialCodes != "") {
                form1.txtMaterialCodes.value = materialCodes;
                document.getElementById("btnSelectMaterialsReturn").click();

            }
        }

        //选择代理商返回
        function Close() {
           layerCommon.layerClose($("#hid_Alert").val());
        }

        //选择银行卡返回
        function CloseBank() {
              layerCommon.layerClose($("#hid_alerts").val());
        }

        //选择银行卡返回
        function SelectBankReturn(materialCodes, bankName) {
            if (materialCodes != "") {
                form1.txtbankcodes.value = materialCodes;
                form1.txtbandname.value = bankName;
                document.getElementById("btnSelectBank").click();
                CloseBank();
            }
        }
        //是否默认事件
        function change() {
            if (document.getElementById("chkIsno").checked) {
                $("#btnDis").hide();
                $("#div_grid").hide();
            } else {
                $("#btnDis").show();
                $("#div_grid").show();
            }
        }

        //获取短信验证码代码 js  begin---------------------------------------------------


        //获取短信验证码
        function getpcode() {
         <% if (ConfigurationManager.AppSettings["Paytest_zj"] != "1")
          { %>
                  
            $("#msg").html("");
            if ($("#hid_tel").val() == "") {
                layerCommon.msg("手机号码不能为空！",IconOption.错误);
                return false;
            } else {
                getphonecode($("#hid_tel").val(), "0", "收款帐号绑定", $("#hid_userid").val(), $("#hid_username").val());
            }

             <% } else { %>
             $("#hid_phone").val("1");
              $("#isshow").show();
             <% } %>
        }

        var i = 0;
        function getphonecode(phone1, type1, Module1, userid1, username1) {
            var isMobile = /^0?1[0-9]{10}$/;
            if (!isMobile.test(phone1)) {
                layerCommon.msg("手机号码格式不正确！",IconOption.错误);
                return false;
            }
            if (i == 0) {
                $.ajax({
                    url: "../../Controller/GetPhoneCode_ggh.ashx",
                    async: true,
                    cache: false,
                    data: { phone: phone1, type: type1, Module: Module1, userid: userid1, username: username1 },
                    dataType: 'json',
                    success: function (gim) {
                        if (!gim.type) {
                              layerCommon.msg("-" + gim.str,IconOption.错误,2000);
                        }
                        else {
                            $("#getcode").css("background", "#ccc");
                            i = 60;
                            timeout();
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        layerCommon.msg("加载错误！",IconOption.错误,2000);
                    }
                });
            }
            else {
                  layerCommon.msg("1分钟内请勿重复发送验证码！",IconOption.错误);
            }
        }

        var ti;
        function timeout() {
            if (i != 0) {
                $("#getcode").text(i.toString() + "秒后重新获取");
                ti = setTimeout("timeout()", 1000);
                i--;
            }
            else {
                $("#getcode").text("获取验证码").css("background", "rgb(63,151,201)");
                clearTimeout(ti);
            }
        }


        //获取短信验证码代码 js  end---------------------------------------------------



        //返回按钮事件
        function goInfo() {
            window.location.href = 'PayAccountList.aspx?nextstep=<%=Request["nextstep"] %>';

        }
        //验证用
        function formCheck() {

            var str = "";
            var id=<%=KeyID %>;
            var txtDisUser = $.trim($("#txtDisUser").val());
            var ddlbank = $("#ddlbank").val();
            var txtbankcode = $("#txtbankcode").val();
            var txtbankAddress = $("#txtbankAddress").val();
            var ddltype = $("#ddltype").val();
            var ddlbank = $("#ddlbank").val();

            var ddlProvince = parseFloat($("#ddlProvince").val());
            var ddlCity = parseFloat($("#ddlCity").val());

            var SltPesontype = $("#SltPesontype").val();
            var txtpesoncode = $("#txtpesoncode").val();
            var chkisno = 0;


            if (document.getElementById("chkIsno").checked) {
                chkisno = 1;
            }

            var hid_phone = $("#hid_phone").val();
            var txtphpcode = $("#txtphpcode").val();

            var gvDtl =$(".tablelist tr.tr_Order").length;// document.getElementById("gvDtl").value;

            var txtcompId = $("#txtcompId").val();



            if (ddltype == "-1") {
                str += "-请选择账户类型！；\r\n";
            }
            if (ddlbank == "-1") {
                str += "-请选择开户银行！；\r\n";
            }
            if (ddlbank == "20000") {
                str += "-开户银行不能选更多银行！；\r\n";
            }


            if (ddltype == "11") {

                if (SltPesontype == "-1") {
                    str += "-请选择证件类型！；\r\n";
                }

                if (txtpesoncode == "") {
                    str += "-证件号码不能为空！；\r\n";
                }
                else {
                    if (SltPesontype == "0") {
                        if (!validateIdCard(txtpesoncode)) {
                            str += "证件号码不正确！；\r\n";

                        }
                    }
                }

            }
            if (txtDisUser == "") {
                str += "-账户名称不能为空！；\r\n";
            }
            if (txtbankcode == "") {
                str += "-账户号码不能为空！；\r\n";
            }
            if (txtbankAddress == "") {
                str += "-开户行地址不能为空！；\r\n";
            }
            if (isNaN(ddlProvince)) {
                str += "-请选择开户所在省份！；\r\n";
            }
            if (isNaN(ddlCity)) {
                str += "-请选择开户所在市！；\r\n";
            }
            if (txtphone == "") {
                str += "-手机号码不能为空！；\r\n";
            }

            if (txtphpcode == "") {
                str += "-手机验证码不能为空！；\r\n";
            } else {
                if (hid_phone == "0") {
                    str += "-手机验证码有误！；\r\n";
                }
            }

            if (chkisno == 0 && gvDtl ==0) {
                str += "-非第一收款账户，请选择要关联的代理商！；\r\n";
            }

            if (str != "") {
                layerCommon.msg(str,IconOption.错误,2500);
                return false;
            } else {
            
                // ajax  ---begin
                var mes = 0;
                $.ajax({
                    type: 'POST',
                    async: false,
                    url: '../../Handler/Tx2310.ashx',
                    data: { ddltype: ddltype, ddlbank: ddlbank, SltPesontype: SltPesontype, txtpesoncode: txtpesoncode, txtDisUser: txtDisUser, txtbankcode: txtbankcode, txtcompId: txtcompId, chkisno: chkisno,id:id },
                    success: function (data) {
                        var resdata = jQuery.parseJSON(data);
                       
                        if (resdata.ID == "2000") {
                            mes = 1;
                        }
                        else if (resdata.ID == "1qaz") {
                            mes = 2;
                            str = resdata.msg;
                        }
                        else {                           
                            str = resdata.msg;

                        }

                    }, error: function (a, b, c) {

                    }
                });

                //ajax   end

                if (mes == 1) {
                    return true;
                }
                else if (mes == 2) {
                    if (confirm(str)) {
                        $("#hid_isno").val("1");
                    } else {
                        $("#hid_isno").val("0");
                        if (gvDtl == 0) {
                            layerCommon.msg("-非第一收款账户，请选择要关联的代理商！",IconOption.错误);
                            document.getElementById("chkIsno").checked = false;
                            $("#btnDis").show();
                            $("#div_grid").show();                        
                            return false;
                        }
                    }
                    return true;

                } else {
                     layerCommon.msg(str,IconOption.错误,2500);
                    return false;
                }
            }

        }

        //验证手机验证码是否正确
        function VphoneCode() {

          <% if (ConfigurationManager.AppSettings["Paytest_zj"] != "1")
                              { %>
        if($("#hid_phone").val()=="0" && $("#txtphpcode").val()!=""){
            var phonecode = $("#txtphpcode").val();
            var phone = $("#hid_tel").val();
            var username = $("#hid_username").val();
            if (phonecode != "") {
                $.ajax({                   
                    type: 'POST',
                    url: '../../Handler/VphoneCode.ashx',
                    data: { username: username, phone: phone, phonecode: phonecode },
                    success: function (data) {
                        var resdata = jQuery.parseJSON(data);

                        if (resdata.ID == "2000") {
                            $("#msg").html(resdata.msg);
                            $("#hid_phone").val("1");

                        } else {
                            $("#msg").html(resdata.msg);
                            $("#hid_phone").val("0");

                        }


                    }, error: function (a, b, c) {

                    }
                });
            }
          }
          <%} %>
        }


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


        function Mackchange(ower) {
            if (ower == "11") {
                $("#tbdis").show();
                $("#SltPesontype").val(0);
            } else {
                $("#tbdis").hide();
            }
        }
        //删除代理商
          function delGoods(Id) {
            $("#hiddelgoodsid").val(Id);
            layerCommon.confirm("确认删除吗？", function () { $("#btnDelGoods").trigger("click"); });
        }
      
    </script>
    <style type="text/css">
        input[type='text']
        {
            width: 170px;
        }
        .layoutRegist select
        {
            width: 100px;
            display: inline-block;
            height: 23px;
            border: 2px;
            line-height: 23px;
            border: 1px solid #ddd;
        }
        .layoutRegist p
        {
            color: #1b2b3b;
            font-size: 9px;
            border: 2px;
            padding-left: 30px;
            position: relative;
        }
        .layoutRegist p span
        {
            width: 60px;
            text-align: left;
            position: absolute;
            left: 50px;
            top: 10px;
        }
        .select
        {
            line-height: 23px;
            display: inline-block;
            height: 23px;
            width: 85px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:CompRemove runat="server" ID="Remove" ModuleID="4" />
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
     <input type="hidden" id="hiddelgoodsid" runat="server" />
      <asp:Button ID="btnDelGoods" runat="server" Text="删除商品" Style="display: none;" OnClick="btnDelGoods_Click" />

   
    <input id="hid_Alert" type="hidden" />
    <input id="hid_alerts" type="hidden" />
    <input id="hid_isno" type="hidden" value="0" runat="server" />
    <input id="hid_phone" type="hidden" value="0" runat="server" />
    <input id="hid_tel" type="hidden" value="0" runat="server" />
    <input id="hid_username" type="hidden" runat="server" />
    <input id="hid_userid" type="hidden" runat="server" />
    <div class="rightinfo">
         <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面</a></li><li>></li>
                <li><a href="../Pay/PayAccountList.aspx" runat="server" id="atitle">收款帐号管理</a></li><li>></li>
                <li><a href="#" runat="server" id="btitle">收款帐号新增</a></li>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <li style="font-size: 12px; color:Red;">
                    <label style="color:Red; display: inline-block;" class="required">
                        *</label>为必填项，确保填写账户、地址真实有效，否则会导致收款失败！</li>
            </ul>
        </div>
        <!--当前位置 end-->

        <div class="div_content">
            <!--销售订单主体 start-->
            <%--<div style="padding-left: 70px; ">--%>
            <div class="lbtb layoutRegist">
                <table class="dh">
                    <tr>
                        <td style="width: 15%;">
                            <span>
                                <label style="color: Red; display: inline-block;" class="required">
                                    *</label>&nbsp;账户类型</span>
                        </td>
                        <td style="width: 30%;">
                            <select id="ddltype" runat="server" class="select" onchange="Mackchange(this.options[this.selectedIndex].value)"
                                style="width: 172px; margin-left: 5px;">
                                <option value="-1">请选择</option>
                                <option value="11">个人账户</option>
                                <option value="12">企业账户</option>
                            </select>&nbsp;
                        </td>
                        <%-- <td style="width: 15%;">
                            <span>
                                <label style="color: Red; display: inline-block;">
                                </label>
                                &nbsp;区域</span>
                        </td>
                        <td style="width: 30%;">
                            <uc1:DisArea runat="server" Id="txtqy" />
                        </td>--%>
                        <td style="width: 15%;">
                            <span>
                                <label style="color: Red; display: inline-block;" class="required">
                                    *</label>&nbsp;开户银行</span>
                        </td>
                        <td style="width: 30%;">
                            <select id="ddlbank" runat="server" onchange="Otypechange(this.options[this.selectedIndex].value)"
                                class="textBox"  style="width: 200px;" >
                                <option value="-1">请选择</option>
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
                             <a style="color: #1a8fc2" href="javascript:Otypechange('20000')">
                                更多银行</a>
                        </td>
                    </tr>
                    <tr id="tbdis" runat="server">
                        <td style="width: 15%;">
                            <span>
                                <label style="color: Red; display: inline-block;" class="required">
                                    *</label>&nbsp;证件类型</span>
                        </td>
                        <td style="width: 30%;">
                            <select id="SltPesontype" runat="server" class="textBox" style="width: 172px;">
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
                            <span>
                                <label style="color: Red; display: inline-block;" class="required">
                                    *</label>&nbsp;证件号码</span>
                        </td>
                        <td>
                            <input name="txtpesoncode" maxlength="18" runat="server" id="txtpesoncode" type="text"
                                class="textBox"  style="width: 200px;" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%;">
                            <span>
                                <label style="color: Red; display: inline-block;" class="required">
                                    *</label>&nbsp;银行账户名称</span>
                        </td>
                        <td style="width: 30%;">
                            <input name="txtDisUser" maxlength="50" runat="server" id="txtDisUser" type="text"
                                class="textBox" value="" />&nbsp;
                        </td>
                        <td>
                            <span>
                                <label style="color: Red; display: inline-block;" class="required">
                                    *</label>&nbsp;银行账户号码</span>
                        </td>
                        <td>
                            <input name="txtbankcode" maxlength="50"  style="width: 200px;"  runat="server" onkeyup="this.value=this.value.replace(/\D/g,'');"
                                id="txtbankcode" type="text" class="textBox" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>
                                <label style="color: Red; display: inline-block;" class="required">
                                    *</label>&nbsp;开户行所在省/市/区</span>
                        </td>
                        <td colspan="3">
                            <p>
                                <select runat="server" id="ddlProvince" class="prov select1 l" onchange="Change()"
                                    style="margin-left: -24px;">
                                </select>
                                <input type="hidden" id="hidProvince" runat="server" value="请选择省份" />
                                <select runat="server" id="ddlCity" class="city select" onchange="Change()">
                                </select>
                                <input type="hidden" id="hidCity" runat="server" value="请选择市" />
                                <select runat="server" id="ddlArea" class="dist select" onchange="Change()">
                                </select>
                                <input type="hidden" id="hidArea" runat="server" value="请选择区" />
                                <input type="hidden" id="hidCode" runat="server" /><%--
                                <span id="spandiqu" style="color: #da5132; margin: 0 0 0 350px; width: 80px;"></span>--%>
                                &nbsp;
                                <p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>
                                <label style="color: Red; display: inline-block;" class="required">
                                    *</label>&nbsp;开户行地址</span>
                        </td>
                        <td colspan="3">
                            <input name="txtbankAddress" maxlength="80" runat="server" id="txtbankAddress" type="text"
                                class="textBox" value="" style="width: 450px;" />&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%;">
                            <span>
                                <label style="color: Red; display: inline-block;" class="required">
                                </label>
                                &nbsp;手机号码</span>
                        </td>
                        <td colspan="3" style="width: 30%;">
                            <%--  <input name="txtphone" maxlength="50" runat="server" id="txtphone" type="text"
                                class="textBox" value="" />--%>
                            <label id="txtphone" runat="server">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>
                                <label style="color: Red; display: inline-block;" class="required">
                                    *</label>&nbsp;手机验证码</span>
                        </td>
                        <td colspan="3">
                            <input name="txtphpcode" maxlength="50" runat="server" onkeyup="this.value=this.value.replace(/\D/g,'');"
                                id="txtphpcode" type="text" class="textBox" onblur="VphoneCode()" />&nbsp;<a id="getcode"
                                    href="#" onclick='getpcode();' class="tablelink">获取验证码</a>&nbsp;
                            <label style="color: #da4444;" id="msg">
                              <% if (ConfigurationManager.AppSettings["Paytest_zj"] == "1")
                                 {%>
                               <i id="isshow" style="display:none;">  <b class="red">（测试密码:123456）</b></i>
                                 <% }%>
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>是否默认为第一收款账户</span>
                        </td>
                        <td colspan="3">
                            <input name="chkIsno" runat="server" id="chkIsno" type="checkbox" checked="checked"
                                onchange="change()" class="textBox" value="" style="width: 30px; border: none;
                                float: left; margin-top: 4px;" />
                            <label style="color: #aaaaaa; font-size: 9pt;">
                                默认所有未关联的代理商货款均收入此户，如否，则选择和此账户相关联的代理商。
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span style="height: 60px; padding-top: 15px;">备注</span>
                        </td>
                        <td colspan="3">
                            <textarea id="txtRemark" maxlength="200" class="textarea" placeholder="备注不能大于200个字符"
                                runat="server"></textarea>
                        </td>
                    </tr>
                </table>
            </div>
            <%--</div>--%>
            <!--销售订单主体 start-->
            <!--清除浮动-->
            <div style="clear: none;">
            </div>
            <!--代理商明细 start-->
            <div style="padding-top: 10px;">
                <!--选择代理商按钮 start-->
                <div class="tools">
                    <div class="click" id="btnDis" runat="server">
                        <ul class="toolbar left">
                            <li><span>
                                <img src="../images/t01.png" /></span>选择要和上面收款账户关联的代理商 </li>
                            <%-- (<a href="#" style="color: #aaaaaa">如果不选择代理商，默认第一收款帐号</a>)--%>
                        </ul>
                    </div>
                </div>
                
                <div>
                    <div class="tablelist" runat="server" id="div_grid">
                        <asp:Repeater ID="gvDtl" runat="server" >
                            <HeaderTemplate>
                                <table>
                                    <tr>
                                        <th class="t8">序号</th>
                                        <th>代理商名称</th>
                                        <th class="t6">代理商区域 </th>
                                        <th class="t8">代理商等级</th>
                                        <th class="t5">详细地址</th>
                                        <th class="t5">联系人</th>
                                        <th class="t8">删 除</th>

                                    </tr>
                                    
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class="tr_Order">
                                    <td>
                                       <div class="tc"> <asp:Label ID="lblGoodsId" Text='<%# Eval("ID") %>' runat="server" Style="display: none;"></asp:Label>
                                        <%# Container.ItemIndex + 1 %></div>
                                    </td>
                                    <td>
                                       <div class="tcle"><%# Eval("DisName")%></div>
                                    </td>
                                    <td>
                                        <div class="tcle"> <%# Common.GetDisAreaNameById(Eval("AreaID").ToString().ToInt(0))%></div>
                                    </td>
                                    <td>
                                       <div class="tc"> <%# Eval("DisLevel")%></div>
                                    </td>
                                    <td>
                                       <div class="tc"> <%# Eval("Address")%></div>
                                    </td>
                                    <td>
                                       <div class="tc"> <%# Eval("Principal")%>  </div>
                                    </td>  
                                    <td>
                                        <%--<asp:LinkButton ID="btnDelete" runat="server" Text="<img src=../../images/del.gif width=16 height=16 border=0>"
                                                CausesValidation="false" CommandName="del" CommandArgument='<%# Eval("GoodsinfoID") %>'></asp:LinkButton>--%>
                                        <div class="tc"> <img src='../../images/del.gif' style="width: 16px; height: 16px; border: 0px; cursor: pointer;"
                                            onclick='delGoods(<%# Eval("ID") %>);'></div>
                                    </td>
                                </tr>
                                
                            </ItemTemplate>    
                           <FooterTemplate></table></FooterTemplate>            
                        </asp:Repeater>

                    </div>
                </div>
                <!--新增商品列表 end-->
            </div>
            <!--销售订单明细 start-->
            <div class="footerBtn">
                <asp:Button ID="btnSave" runat="server" Text="确定" CssClass="orangeBtn" OnClick="btnSave_Click"
                    OnClientClick="return formCheck()" />&nbsp;
                <input name="" type="button" class="cancel" onclick="javascript:goInfo();" value="返回" />
                <input type="button" id="btnSelectMaterialsReturn" runat="server" onserverclick="btnSelectMaterialsReturn_ServerClick"
                    style="display: none;" />
                <input type="button" id="btnSelectBank" runat="server" onserverclick="btnSelectBankReturn_ServerClick"
                    style="display: none;" />
                <input id="txtMaterialCodes" type="hidden" name="txtMaterialCodes" runat="server" />
                <input id="txtbankcodes" type="hidden" name="txtbankcodes" runat="server" />
                <input id="txtbandname" type="hidden" name="txtbandname" runat="server" />
                <input id="txtGoodsCodes" type="hidden" name="txtGoodsCodes" runat="server" />
                <input id="txtcompId" type="hidden" name="txtcompId" runat="server" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>

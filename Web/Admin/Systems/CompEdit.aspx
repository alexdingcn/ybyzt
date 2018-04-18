<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompEdit.aspx.cs" Inherits="Admin_Systems_CompEdit" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="IndusTry" Src="~/Admin/UserControl/Tree_Industry.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>厂商编辑</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/CitysLine/JQuery-Citys-online-min.js"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>
    <link href="../../Company/css/Enterprice.css" rel="stylesheet" type="text/css" />
    <script>

        $(document).ready(function () {
        $("#uploadFile").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText", ResultId: "HidFfileName", AjaxSrc: "../../Controller/Fileup.ashx",maxlength:5,DownSrc:"../../" });
            $("#libtnFile").on("click", function () {
                $("#uploadFile").trigger("click");
            })
          
        })

        function blurCName(obj){
          if($.trim($("#txtShotName").val())=="" && $.trim($(obj).val()).length<=12){
           $("#txtShotName").val($.trim($(obj).val()));
          }
        }

        function formCheck() {
            var str = "";
            if ($.trim($("#txtCompName").val()) == "") {
                str = "厂商名称不能为空。";
            }
             else if ($.trim($("#txtCompName").val()).length < 2 || $.trim($("#txtCompName").val()).length > 20){
                 str = "厂商名称必须在2-20字符之间。";
            }
            else if($.trim($("#txtShotName").val()) == ""){
               str = "企业简称不能为空。";
            }
//            else if ($("#txtIndusName").val() == "-1") {
//                str = "请选择产品所属分类。";
//            }
            if ($.trim($("#Capital").val()) == "") {
                str = "企业注册资金不能为空";
            }
//            else if($.trim($("#txtLegalTel").val())==""){
//                 str = "请输入法人手机";
//            }
//            else if (!IsLegalTel($.trim($("#txtLegalTel").val()))) {
//                str = "法人联系电话不正确。";
//            }
//            else if (!IsEmail($.trim($("#txtZip").val()))) {
//                str = "邮箱格式不正确。";
//            }
            //edit by hgh 
//            else if ($.trim($("#txtZip").val())=="") {
//                str = "邮箱格式不能为空。";
//            }
            //else if ($("#txtZip").val().toString().indexOf('@')<=0) {
            //    str = "邮箱格式不正确。";
            //}
            else if($.trim($("#txtPhone").val())==""){
                str = "联系人手机不能为空。";
            }
            else if($.trim($("#txtPhone").val())!="" && !IsMobile($.trim($("#txtPhone").val())))
            {
                 str ="联系人手机不正确";
            }
            else if ($.trim($("#txtInfo").val()) == "") {
                str = "主要经营范围不能为空。";
            }
             else if ($.trim($("#txtAddress").val()) == "") {
                str = "详细地址不能为空。";
            }
            else if ($.trim($("#txtUsername").val()) == "") {
                str = "登录帐号不能为空。";
            }
            else if ($.trim($("#txtUsername").val()).length < 2 || $.trim($("#txtUsername").val()).length > 20){
                str = "登录帐号必须在2-20字符之间。";
            }
            else if($.trim($("#txtUserPhone").val())=="")
            {
                str="请输入管理员手机号";
            }
            else if (!IsMobile($.trim($("#txtUserPhone").val()))) {
                str = "管理员手机号码不正确。";
            }
            else if ($.trim($("#txtUpwd").val()).length < 6) {
                str = "用户密码不能少于6位。";
            }
            else if ($.trim($("#txtUpwd").val()) != $.trim($("#txtUpwds").val())) {
                str = "确认密码不一致";
            }
            else if ($.trim($("#txtUserTrueName").val()) == "") {
                str = "管理员姓名不能为空。";
            }
            else if ($.trim($("#hidProvince").val()) == "" || $.trim($("#hidProvince").val()) == "选择省") {
                str = "请选择所属地区省份。";
            }

            <% if(KeyID == 0) { %>
            else if (IsMobile($.trim($("#txtUserPhone").val()))) {
                str= ExisPhone($.trim($("#txtUserPhone").val()));
            }
            <% } %>
            if (str != "") {
                errMsg("提示", str, "", "");
                return false;
            }
            return true;
        }

        function getFileName(o) {
            var pos = o.lastIndexOf("\\");
            return o.substring(pos + 1);
        }

          function phoneValue(obj){
         if(!IsMobile($.trim($("#txtUserPhone").val()))){
            $("#txtUserPhone").val(obj.value);
        }
    }

        function change(obj, showid) {
            var FileName = getFileName(obj.value.toString());
            $("#" + showid + "").text(FileName);
        }

        function ExisPhone(name) {
            var str="";
            $.ajax({
                type: "post",
                data: { Action: "GetPhone", Value: name },
                dataType: 'json',
                async: false,
                timeout: 4000,
                success: function (data) {
                    if (data.result) {
                       str= "该手机已被注册请重新填写！";
                    }
                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                      str="手机号码效验异常";
                }
            })
            return str;
        }

    </script>
    <style type="text/css">
        .style1
        {
            height: 36px;
        }
        .fl {
            float:left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">厂商管理</a><i>></i>
            <a href="#" runat="server" id="Atitle">厂商新增</a>
    </div>
        <div class="tools" runat="server" id="divshow">
            <ul class="toolbar left">
                <li id="lblbtnback" onclick="javascript:history.go(-1);"><span>
                    <img src="../../Company/images/tp3.png" /></span>返回</li>
            </ul>
        </div>
        <div class="div_content">
            <table class="tb">
                <tbody>
                    <tr>
                        <td style="width: 10%;">
                            <span><i class="required">*</i>厂商名称</span>
                        </td>
                        <td style="width: 30%;" nowrap="nowrap">
                            <input runat="server" onkeypress="KeyPress(event)" type="text" maxlength="50" onblur="blurCName(this)"
                                class="textBox" id="txtCompName" /><i class="grayTxt">（企业的法律名称，2-20个汉字或字母）</i>
                        </td>
                        <td style="width: 10%;">
                            <span>产品所属分类</span>
                        </td>
                        <td style="width: 30%;" nowrap="nowrap">
                            <asp:DropDownList runat="server" ID="txtIndusName" />
                        </td>
                    </tr>
                                    
                    <tr>
                        <td>
                            <span><i class="required">*</i>企业简称</span>
                        </td>
                        <td colspan="3">
                            <input runat="server" onkeypress="KeyPress(event)" type="text" maxlength="12" class="textBox"
                                id="txtShotName" /><i class="grayTxt">（不得超过12个汉字或字母）</i>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>企业编号</span>
                        </td>
                        <td align="left">
                            <label style="text-align: left;" id="lblCompCode" runat="server">
                                （自动生成）</label>
                        </td>
                        <td>
                        <span>     法人</span>
                        </td>
                        <td>
                            <input runat="server" type="text" maxlength="50" onkeypress="KeyPress(event)" class="textBox"
                                id="txtLegal" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                         <span>   法人身份证</span>
                        </td>
                        <td>
                            <input runat="server" type="text" maxlength="50" class="textBox" id="txtIdentitys" />
                        </td>
                        <td>
                            <span>法人联系电话</span>
                        </td>
                        <td>
                            <input runat="server" type="text" maxlength="11" class="textBox" id="txtLegalTel" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>营业执照号码</span>
                        </td>
                        <td>
                            <input runat="server" type="text" maxlength="50" class="textBox" id="txtLicence" />
                        </td>
                        <td class="style1">
                            <span>组织机构代码证号码</span>
                        </td>
                        <td class="style1">
                            <input runat="server" type="text" maxlength="100" class="textBox" id="txtOrcode" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <span>税务登记证号码</span>
                        </td>
                        <td class="style1">
                            <input runat="server" type="text" maxlength="100" class="textBox" id="txtAccount" />
                        </td>
                        <td>
                            <span>联系人</span>
                        </td>
                        <td>
                            <input runat="server" onkeypress="KeyPress(event)" type="text" maxlength="50" class="textBox"
                                id="txtPrincipal" onblur="javascript:if($.trim($('#txtUserTrueName').val())==''){$('#txtUserTrueName').val(this.value);}" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>固定电话</span>
                        </td>
                        <td>
                            <input runat="server" type="text" maxlength="50" class="textBox" id="txtTel" />
                        </td>
                        <td>
                            <span><i class="required">*</i>联系人手机</span>
                        </td>
                        <td>
                            <input runat="server" type="text" maxlength="11" class="textBox" id="txtPhone" onblur="phoneValue(this)" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>传真</span>
                        </td>
                        <td>
                            <input runat="server" type="text" maxlength="50" class="textBox" id="txtFax" />
                        </td>
                        <td>
                            <span>邮箱</span>
                        </td>
                        <td>
                            <input runat="server" type="text" maxlength="50" class="textBox" id="txtZip" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><i class="required">*</i>是否启用</span>
                        </td>
                        <td>
                            &nbsp;
                            <input type="radio" runat="server" checked name="IsEbled" value="1" id="rdEbleYes" />启用
                            &nbsp;&nbsp;&nbsp;<input runat="server" id="rdEbleNo" name="IsEbled" type="radio"
                                value="0" />禁用
                        </td>
                        <td>
                            <span><i class="required">*</i>是否允许代理商加盟</span>
                        </td>
                        <td>
                            &nbsp;
                            <input type="radio" runat="server" name="Hot" value="1" id="rdHotShowYes" checked="true"/>是 &nbsp;&nbsp;&nbsp;<input runat="server" id="rdHotShowNo" name="Hot" type="radio"  value="0" />否
                            <i class="grayTxt">（默认是）</i>
                        </td>
                    </tr>
                    <tr>
                       
                       <td>
                            <span><i class="required">*</i>首页显示</span>
                        </td>
                        <td>  
                            <asp:DropDownList ID="ddlChkShow" runat="server">
                                <asp:ListItem Value="1">首页显示</asp:ListItem>
                                <asp:ListItem Value="2">只搜索显示</asp:ListItem>
                                <asp:ListItem Value="0">不显示</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <span>企业来源</span>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlErptype" Enabled="false" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>附件上传</span>
                        </td>
                        <td colspan="3">
                            <div class="teamR" style="margin: 5px 0px 0px 0px;">
                                <div class="verFile" style="margin: 0px; width: 100%;">
                                    <span class="verFileCon" style="min-width: 60px;">
                                        <input id="uploadFile" runat="server" type="file" name="fileAttachment" /></span>
                                    <a class="btn1" id="A1" style="margin-left: 5px; text-decoration: NONE;" href="javascript:void(0)">
                                        <b class="L"></b><b class="R"></b>上传附件</a>
                                </div>
                                <asp:Panel runat="server" ID="DFile" Style="margin: 5px;">
                                </asp:Panel>
                            </div>
                            <div id="UpFileText" style="margin: 5px 5px;">
                            </div>
                            <input runat="server" id="HidFfileName" type="hidden" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><i class="required">*</i>主要经营范围</span>
                        </td>
                        <td colspan="3">
                            <input runat="server" maxlength="200" style="width: 600px;" type="text" class="textBox"
                                id="txtInfo" />
                        </td>
                    </tr>
                        <tr>
                        <td style="width: 10%;">
                            <span><i class="required">*</i>注册资金(万)</span>
                        </td>
                        <td style="width: 30%;" nowrap="nowrap">
                            <input runat="server" onkeyup="MoneyYZ(this)" type="text" maxlength="50" class="textBox" id="Capital" /><i class="grayTxt">（单位：万）</i>
                        </td>
                        <td style="width: 10%;">
                            <span><i class="required">*</i>企业机构类型</span>
                        </td>
                        <td style="width: 30%;" nowrap="nowrap">
                                <asp:DropDownList ID="CompType" runat="server">
                                <asp:ListItem Value="1">个人</asp:ListItem>
                                <asp:ListItem Value="2">股份</asp:ListItem>
                               
                            </asp:DropDownList>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <span><i class="required">*</i>厂商地区</span>
                        </td>
                        <td colspan="3">
                            <div class="pullDown fl" style="margin-left:5px;">
			
            <input type="hidden" id="hidProvince" runat="server"  value="选择省" />
            <select  runat="server" id="ddlProvince"   class="box1 p-box prov" onchange="Change()" ></select>
		</div>
    	<div class="pullDown fl">
			 <select runat="server" id="ddlCity" class="box1 p-box city"  onchange="Change()"></select>
             <input type="hidden" id="hidCity" runat="server" value="选择市" />
		</div>
        <div class="pullDown fl">
			<select runat="server" id="ddlArea" class="box1 p-box dist"  onchange="Change()"></select>
            <input type="hidden" id="hidArea" runat="server" value="选择区" />
		</div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><i class="required">*</i>详细地址</span>
                        </td>
                        <td colspan="3">
                            <input runat="server" maxlength="200" style="width: 600px;" type="text" class="textBox"
                                id="txtAddress" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>备注</span>
                        </td>
                        <td colspan="3">
                            <textarea style="height: auto; width: 600px;" rows="3" class="textBox" maxlength="2000"
                                runat="server" id="txtRemark"></textarea>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="div_title">
                企业管理员用户:
            </div>
            <table class="tb">
                <tbody>
                    <tr>
                        <td style="width: 10%;">
                            <span><i class="required">*</i>登录帐号</span>
                        </td>
                        <td style="width: 30%;">
                            <input runat="server" type="text" maxlength="50" class="textBox" id="txtUsername" /><i
                                class="grayTxt">（2-20个字符，支持字母、数字或下划线）</i>
                        </td>
                        <td style="width: 10%;">
                            <span><i class="required">*</i>手机号码</span>
                        </td>
                        <td style="width: 30%;">
                            <input runat="server" type="text" maxlength="11" class="textBox" id="txtUserPhone" />
                            <i class="grayTxt">（非常重要，登录、发送验证短信使用）</i>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><i class="required">*</i>登录密码</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUpwd" TextMode="Password" MaxLength="50" runat="server" CssClass="textBox"></asp:TextBox>
                            <i class="grayTxt" id="UpwTitle" runat="server">（新增时默认为123456，可更改）</i>
                        </td>
                        <td>
                            <span><i class="required">*</i>确认密码</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUpwds" TextMode="Password" MaxLength="50" runat="server" CssClass="textBox"></asp:TextBox>
                            <i class="grayTxt" id="UpwsTitle" runat="server">（新增时默认为123456，可更改）</i>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><i class="required">*</i>姓名</span>
                        </td>
                        <td colspan="3">
                            <input runat="server" type="text" maxlength="50" class="textBox" id="txtUserTrueName" />
                            <i class="grayTxt">（请填写真实姓名，以便更好地为您服务）</i>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="div_footer">
            <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return formCheck()"
                OnClick="btnAdd_Click" />&nbsp;
            <input name="" type="button" class="cancel" onclick="javascript:history.go(-1);"
                value="返回" />
        </div>
    </div>
    </form>
</body>
</html>

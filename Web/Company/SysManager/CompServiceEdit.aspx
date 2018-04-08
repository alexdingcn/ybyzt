<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompServiceEdit.aspx.cs" Inherits="Company_SysManager_CompServiceEdit" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>企业信息维护</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>
    <link href="../css/Enterprice.css" rel="stylesheet" type="text/css" />
    <script>

        $(document).ready(function () {
            $_def.ID = "btnSave";
        $("#uploadFile").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText", ResultId: "HidFfileName", AjaxSrc: "../../Controller/Fileup.ashx",maxlength:5 ,DownSrc:"../../"});
            $("#libtnFile").on("click", function () {
                $("#uploadFile").trigger("click");
            })
            $("li#libtnSave").on("click", function () {
                $("#btnSave").trigger("click");
            })

            //仓单信息
            $("#libtnEreceipt").on("click", function () {
                //window.top.open("CompEreceipt.aspx");

                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度

                var index = layerCommon.openWindow('仓单信息', 'CompEreceipt.aspx', '850px', '450px');  //记录弹出对象
                $("#hid_Alert").val(index); //记录弹出对象
            });
        })

        function formCheck() {
            var str = "";
            if ($.trim($("#txtCompName").val()) == "") {
                str = "厂商名称不能为空。";
            }
            else if ($.trim($("#txtCompName").val()).length < 2 || $.trim($("#txtCompName").val()).length > 20) {
                str = "厂商名称必须在2-20字符之间。";
            }
            else if ($("#txtIndusName").val() == "-1") {
                str = "请选择行业类别。";
            }
            else if ($.trim($("#txtShortName").val()) == "") {
                str = "企业简称不能为空。";
            }
//            else if ($.trim($("#txtLegalTel").val()) == "") {
//                str = "法人手机不能为空。";
//            }
//            else if (!IsLegalTel($.trim($("#txtLegalTel").val()))) {
//                str = "法人联系电话不正确。";
//            }
//            else if (!IsEmail($.trim($("#txtZip").val()))) {
//                str = "邮箱格式不正确。";
            //            }
            //edit by hgh 
//            else if ($.trim($("#txtZip").val()) == "") {
//                str = "邮箱格式不能为空。";
//            }
//            else if ($("#txtZip").val().toString().indexOf('@') <= 0) {
//                str = "邮箱格式不正确。";
//            }

            else if ($.trim($("#txtPhone").val()) != "" && !IsMobile($.trim($("#txtPhone").val()))) {
                str = "联系人手机不正确。";
            }
//            else if ($.trim($("#txtFinanceCode").val()) == "") {
//                str = "融资账户号不能为空。";
//            }
//            else if ($.trim($("#txtFinanceName").val()) == "") {
//                str = "融资账户名称不能为空。";
//            }
            else if ($.trim($("#txtInfo").val()) == "") {
                str = "主要经营范围不能为空。";
            }
            else if ($.trim($("#txtAddress").val()) == "") {
                str = "详细地址不能为空。";
            }
            else if ($.trim($("#txtUserTrueName").val()) == "") {
                str = "管理员姓名不能为空。";
            }
            else if ($.trim($("#QQ").val()) != "")
            {
             var k = $.trim($("#QQ").val())
             var reg=/^\d{5,12}$/;    
             if (!reg.test(k)) {
                 str = "请输入正确的QQ号 并确保无误。";
            }
            }

            if (str != "") {
                layerCommon.msg(str, IconOption.错误);
                return false;
            }
            return true;
        }
        function addname() {
            if ($.trim($("#txtShortName").val()) == "" && $.trim($("#txtCompName").val()).length <= 12) {
                $("#txtShortName").val($.trim($("#txtCompName").val()));
            }
        }
        function getFileName(o) {
            var pos = o.lastIndexOf("\\");
            return o.substring(pos + 1);
        }

        function txtCompName_onclick() {

        }

        function layerClose() {
            layerCommon.layerClose($("#hid_Alert").val());
        }

    </script>
    <style type="text/css">
        .style1
        {
            height: 36px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
    <input id="hid_Alert"  type="hidden"/>
           <div class="rightinfo">
            <div class="place">
	            <ul class="placeul">
                    <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                    <li><a href="../SysManager/CompServiceEdit.aspx" runat="server" id="a2">企业信息维护</a></li>
	            </ul>
            </div>
               <div class="tools" id="DisBoot" runat="server">
                    <ul class="toolbar left">
                        <li id="libtnSave" runat="server" ><span><img src="../images/t15.png" /></span><font>确定</font></li>
                        <% if (ConfigurationManager.AppSettings["IsFinancing"] == "1")
                           { %>
                        <li id="libtnEreceipt" runat="server" ><span><img src="../images/t01.png" /></span>仓单信息</li>
                        <%} %>
                    </ul>
               </div>
               <asp:Button ID="btnSave" runat="server" Text="确定"  OnClientClick="return formCheck()" onclick="btn_Save" style="display:none;" />
               <div class="div_content">
                    <table  class="tb">
                      <tbody>
                       <tr>
                       <td style=" width:10%;"><span><i class="required">*</i>厂商名称</span> </td>
                       <td style=" width:30%;"> <input runat="server" onblur="addname()" onkeypress="KeyPress(event)"  type="text" maxlength="50"  class="textBox" id="txtCompName" onclick="return txtCompName_onclick()" /><i class="grayTxt">（企业法律名称，2-20个汉字或字母）</i>  </td>
                       <td style=" width:10%;" ><span><i class="required">*</i>行业类别</span> </td>
                       <td style=" width:30%;" nowrap="nowrap"><asp:DropDownList runat="server"  ID="txtIndusName"  />  </td>
                       </tr>

                       <tr>
                       <td ><span><i class="required">*</i>企业简称</span></td>
                       <td><input runat="server" onkeypress="KeyPress(event)"  type="text" maxlength="24"  class="textBox" id="txtShortName" /><i class="grayTxt">（不得超过24个字符）</i> </td> 
                       <td>
                           <span><i class="required">*</i>是否允许代理商加盟</span>
                       </td>     
                       <td>
                            &nbsp;
                            <input type="radio" runat="server" name="Hot" value="1" id="rdHotShowYes" checked="true"  />是 &nbsp;&nbsp;&nbsp;<input runat="server" id="rdHotShowNo" name="Hot" type="radio" value="0" />否
                            <i class="grayTxt">（默认是）</i>
                       </td>                                   
                       </tr>

                       <tr>
                       <td ><span>企业编号</span></td>
                       <td ><label style=" text-align:left;" id="lblCompCode" runat="server">（自动生成）</label></td>   
                       <td><span>法人</span> </td>
                       <td><input runat="server"  type="text" maxlength="50" onkeypress="KeyPress(event)"  class="textBox" id="txtLegal" /> </td>                                         
                       </tr>

                       <tr>                       
                       <td><span>法人身份证</span> </td>
                       <td><input runat="server"  type="text" maxlength="50"  class="textBox" id="txtIdentitys" /></td>
                       <td><span>法人联系电话</span> </td>
                       <td><input runat="server"  type="text" maxlength="11"  class="textBox" id="txtLegalTel" /></td>
                       </tr>
                       <tr>
                       
                       <td><span>营业执照号码</span> </td>
                       <td><input runat="server"  type="text" maxlength="50"  class="textBox" id="txtLicence" /></td>
                       <td class="style1"><span>组织机构代码证号码</span> </td>
                       <td class="style1" > <input runat="server"  type="text" maxlength="100" class="textBox" id="txtOrcode" />  </td>                                               
                      </tr>
                       <tr>
                       <td class="style1"><span>税务登记证号码</span> </td>
                       <td class="style1" > <input runat="server"  type="text" maxlength="100" class="textBox" id="txtAccount" />  </td>
                        <td><span>联系人</span> </td>
                       <td><input runat="server"  type="text" maxlength="50" onkeypress="KeyPress(event)" class="textBox" id="txtPrincipal" onblur="javascript:if($.trim($('#txtUserTrueName').val())==''){$('#txtUserTrueName').val(this.value);}" /> </td>
                       </tr>
                      
                       <tr>
                       <td><span>固定电话</span> </td>
                       <td> <input runat="server"  type="text" maxlength="20"  class="textBox" id="txtTel" />  </td> 
                       <td><span>联系人手机</span> </td>
                       <td> <input runat="server"  type="text" maxlength="11" class="textBox" id="txtPhone"  onblur="phoneValue(this)" />  </td>                       
                       </tr>
                       <tr>
                       <td><span>传真</span> </td>
                       <td><input runat="server"  type="text" maxlength="50" class="textBox" id="txtFax" /> </td>
                        <td><span>邮箱</span> </td>
                        <td><input runat="server"  type="text" maxlength="50"  class="textBox" id="txtZip" /> </td>
                       </tr>

                       <%--<tr>
                       <td><span><i class="required">*</i>融资账户号</span></td>
                       <td><input runat="server"  type="text" maxlength="100" class="textBox" id="txtFinanceCode" /> </td>
                        <td><span><i class="required">*</i>融资账户名称</span> </td>
                        <td><input runat="server"  type="text" maxlength="100"  class="textBox" id="txtFinanceName" /> </td>
                       </tr>--%>

                        <tr>
                       <td><span>附件上传</span> </td>
                       <td > 
                       
                            <div class="teamR" style=" margin:5px 0px 0px 0px;" >
                                <div class="verFile" style="margin: 0px; width:100%; ">
                                 <span class="verFileCon" style=" min-width:60px;" > <input id="uploadFile" runat="server" type="file"  name="fileAttachment"   /></span>
                                 <a class="btn1" id="A1" 
                                      style="margin-left:5px; text-decoration: NONE; " 
                                      href="javascript:void(0)"><b class="L"></b><b class="R"></b>上传附件</a>
                                 </div>
                                                          <asp:Panel runat="server" id="DFile" style=" margin:5px;"></asp:Panel>
                            </div>

                        <div id="UpFileText" style=" margin:5px 5px;">
                         </div>
                           <input runat="server" id="HidFfileName" type="hidden" />
                        </td>
                            <td><span>店主QQ</span> </td>
                       <td> <input runat="server"  type="text" maxlength="11" class="textBox" id="QQ"  />&nbsp;请先<a href="http://shang.qq.com/v3/widget.html" target="_blank" style="text-decoration:underline;"><font color="red">开通允许陌生人给我发消息</font></a> </td>                       
                       </tr>
                       <tr>
                       <td><span><i class="required">*</i>主要经营范围</span> </td>
                       <td colspan="3"> <input runat="server" maxlength="200"  style=" width:600px;"  type="text"  class="textBox" id="txtInfo" />  </td>
                       </tr>
                        <tr>
                       <td><span><i class="required">*</i>详细地址</span> </td>
                       <td colspan="3"> <input runat="server" maxlength="200"  style=" width:600px;"  type="text"  class="textBox" id="txtAddress" />  </td>
                       </tr>
                     <tr>
                        <td><span><i class="required">*</i>管理员登录帐号</span></td >
                       <td> <input runat="server"  type="text" maxlength="50" class="textBox" id="txtUsername" /></td>
                        <td> <span><i class="required">*</i>管理员手机号</span></td>
                       <td><input runat="server"  type="text" maxlength="11" class="textBox" id="txtUserPhone" /></td>
                      </tr>
                      <tr>
                      <td><span><i class="required">*</i>管理员姓名</span></td >
                       <td colspan="3"><input runat="server"  type="text" maxlength="50" class="textBox" id="txtUserTrueName" />  </td>
                      </tr>
                      </tbody>
                    </table>              
           </div>
        </div>

    </form>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SpendingUserAdd.aspx.cs" Inherits="Company_Financing_SpendingUserAdd" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>开销户维护</title>
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script>
        $(function () {
            $(window.parent.leftFrame.document).find(".menuson li.active").removeClass("active");
            window.parent.leftFrame.document.getElementById("kxhwh").className = "active";

        });
        
        function formCheck() {
            var str = "";
            if ($.trim($("#txtAccName").val()) == "") {
                str = "帐户名称不能为空";
            }
            else if ($.trim($("#txtAccountName").val()) == "") {
                str = "代理商名称不能为空";
            }
            else if ($("#ddlAccountNature").val() == null) {
                str = "请选择代理商性质";
            }
            else if ($("#ddlDocumentType").val() == null) {
                str = "请选择证件类型";
            }
            else if ($("#ddlDocumentType").val() == '<%=(int)Enums.CertificatesNature.身份证%>' && !validateIdCard($.trim($("#txtDocumentCode").val()))) {
                str = "身份证号码不正确";
            }
            else if ($.trim($("#txtOrgCode").val()) == "") {
                str = "组织机构代码证不能为空";
            }
            else if ($.trim($("#txtBusinessLicense").val()) == "") {
                str = "营业执照不能为空";
            }
            else if ($.trim($("#txtPhone").val()) != "" && !IsMobile($.trim($("#txtPhone").val()))) {
                str = "手机号码不正确";
            }
            else if ($.trim($("#txtEmail").val()) != "" && !IsEmail($.trim($("#txtEmail").val()))) {
                str = "邮箱不正确";
            }


            if (str != "") {
                errMsg("提示", str, "", "");
                return false;
            }
            return true;
        }
    
    </script>
    <style>
      input[type='text']
      {
          width:150px;
          
      }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="" />

    <div class="rightinfo">

    <!--当前位置 start-->
    <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="../jsc.aspx">我的桌面</a></li><li>></li>
            <li><a href="#">在线融资</a></li><li>></li>
            <li><a href="#">开销户维护</a></li>
        </ul>
    </div>
        <div class="div_content">
            <div class="lbtb">
            <table class="dh">
                <tr>
                    <td style="width:10%;">
                        <span><label class="required">*</label>账户号</span>
                    </td>
                    <td style=" width:30%;">
                        <input runat="server" type="text" style=" border-right:none; padding-right:0px;" class="textBox"  id="txtOgCode" />
                        <i class="grayTxt">(不允许修改，<%=ConfigurationManager.AppSettings["OrgCode"]%>为系统默认编码)</i>
                    </td>
                    <td style="width:10%;">
                        <span><label class="required">*</label>账户名称</span>
                    </td>
                    <td style=" width:30%;">
                        <input runat="server" type="text" class="textBox" maxlength="25" id="txtAccName" />
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;">
                        <span><label class="required">*</label>代理商名称</span>
                    </td>
                    <td style=" width:30%;">
                        <input runat="server" type="text" class="textBox" maxlength="25" id="txtAccountName" />
                    </td>
                    <td style="width:10%;">
                        <span><label class="required">*</label>代理商性质</span>
                    </td>
                    <td style=" width:30%;">
                        <asp:DropDownList runat="server" ID="ddlAccountNature" style="width: 152px; height:25px;" CssClass="textBox">
                        <asp:ListItem Value="2"  Text="企业"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;">
                        <span><label class="required">*</label>证件类型</span>
                    </td>
                    <td style=" width:30%;">
                        <asp:DropDownList style="width: 152px; height:25px;" runat="server" ID="ddlDocumentType" CssClass="textBox">
                        
                        </asp:DropDownList>
                    </td>
                    <td style="width:10%;">
                        <span><label class="required">*</label>证件号</span>
                    </td>
                    <td style=" width:30%;">
                        <input runat="server"  type="text" maxlength="25"  class="textBox" id="txtDocumentCode" />
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;">
                        <span><label class="required">*</label>组织机构代码证</span>
                    </td>
                    <td style=" width:30%;">
                        <input runat="server"  type="text" maxlength="25" class="textBox" id="txtOrgCode" />
                    </td>
                    <td style="width:10%;">
                        <span><label class="required">*</label>营业执照</span>
                    </td>
                    <td style=" width:30%;">
                        <input runat="server"  type="text" maxlength="25" class="textBox" id="txtBusinessLicense" />
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;">
                        <span>代理商地址</span>
                    </td>
                    <td style=" width:30%;">
                        <input runat="server"  type="text" maxlength="25" class="textBox" id="txtAccAddress" />
                    </td>
                    <td style="width:10%;">
                        <span>性别</span>
                    </td>
                    <td style=" width:30%;">
                        <asp:DropDownList runat="server" ID="ddlSex" style="width: 152px; height:25px;" CssClass="textBox">
                        <asp:ListItem Value="0"  Text="男"></asp:ListItem>
                        <asp:ListItem Value="1"  Text="女"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;">
                        <span>国籍</span>
                    </td>
                    <td style=" width:30%;">
                        <input runat="server"  type="text" maxlength="25" class="textBox" id="txtNationality" />
                    </td>
                    <td style="width:10%;">
                        <span>电话号码</span>
                    </td>
                    <td style=" width:30%;">
                        <input runat="server"  type="text" maxlength="25" class="textBox" id="txtPhoneNumbe" />
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;">
                        <span>传真</span>
                    </td>
                    <td style=" width:30%;">
                        <input runat="server"  type="text" maxlength="25" class="textBox" id="txtFax" />
                    </td>
                    <td style="width:10%;">
                        <span>手机</span>
                    </td>
                    <td style=" width:30%;">
                        <input runat="server"  type="text" maxlength="25" class="textBox" id="txtPhone" />
                    </td>
                </tr>
                <tr>
                    <td style="width:10%;">
                        <span>邮箱</span>
                    </td>
                    <td style=" width:30%;">
                        <input runat="server"  type="text" maxlength="25" class="textBox" id="txtEmail" />
                    </td>
                    <td style="width:10%;">
                        <span>邮政编码</span>
                    </td>
                    <td style=" width:30%;">
                        <input runat="server"  type="text" maxlength="25" class="textBox" id="txtPostcode" />
                    </td>
                </tr>
            </table>
            </div>
            <div class="footerBtn">
                <asp:Button ID="btnupdate" runat="server" Text="确定" CssClass="orangeBtn" OnClick="Btn_Save"
                    OnClientClick="return formCheck();" />&nbsp;
                <label runat="server" style=" color:Red;" id="lblMsg" visible="false"></label>
            </div>
        </div>
    </div>
    <!--当前位置 end-->
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SpendingUserAdd.aspx.cs" Inherits="Financing_SpendingUserAdd" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>开销户</title>
    <link href="../Distributor/css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>

    <style type="text/css">
        .teamList{line-height:28px; padding:5px 0 0 0;}
        .teamList dd{ background:#f9f9f9; margin-top:2px; width:305px; padding-left:10px; position:relative;}
        .teamList .red{ color:red; margin-left:10px; position:absolute; top:1px; right:7px; display:inline-block; height:26px; line-height:26px; cursor:pointer;}
        .teamList .speed{ position:absolute; top:0; right:45px; color:#999;}
        .teamList .green{ color:#14ab00;}
    </style>
    <script>
        function formCheck() {
            var str = "";
            if ($.trim($("#txtAccNumver").val()) == "") {
                str = "帐户号不能为空";
            }
            else if ($.trim($("#txtAccName").val()) == "") {
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
                layerCommon.alert(str, IconOption.错误);
                return false;
            }
            return true;
        }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="SpendingUserAdd" />
    <div class="rightCon">
    <div class="info"><span class="homeIcon"></span><a id="navigation1" href="#">基本资料</a>><a id="navigation2" href="#" class="cur">基本信息</a></div>

        <div class="userTrend">
            <div class="uTitle">
                <b>开销户</b></div>
            <div class="orderNr">
            <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                <tbody>
                    <tr>                      
                        <td class="head" style="width: 10%">
                            <span><i class="required">*</i></span>账户号
                        </td>
                        <td style="width: 40%">
                           <input runat="server" type="text" style=" border-right:none; padding-right:0px;" class="box" maxlength="25" id="txtOgCode" /> <input runat="server"  style=" margin-left:-6px;border-left:none; padding-left:0px; width:135px;" type="text" class="box" maxlength="25" id="txtAccNumver" />
                           <span style="color:Red;">(不允许修改，<%=ConfigurationManager.AppSettings["OrgCode"]%>为系统默认编码)</span>
                        </td>
                        <td class="head" style="width: 10%">
                             <span><i class="required">*</i></span>账户名称
                        </td>
                        <td style="width: 40%">
                              <input runat="server" type="text" class="box" maxlength="25" id="txtAccName" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><i class="required">*</i></span>代理商名称
                        </td>
                        <td>
                           <input runat="server" type="text" class="box" maxlength="25" id="txtAccountName" />
                        </td>
                        <td class="head">
                             <span><i class="required">*</i></span>代理商性质
                        </td>
                        <td>
                          <asp:DropDownList runat="server" ID="ddlAccountNature" style="width: 166px; font-size:12px;" CssClass="xl">
                          <asp:ListItem Value="0"  Text="个人"></asp:ListItem>
                          <asp:ListItem Value="1"  Text="公司"></asp:ListItem>
                          </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="head">
                             <span><i class="required">*</i></span>证件类型
                        </td>
                        <td >
                        <asp:DropDownList style="width: 166px; font-size:12px;" runat="server" ID="ddlDocumentType" CssClass="xl">
                        
                        </asp:DropDownList>
                        </td>
                        <td class="head">
                          <span><i class="required">*</i></span>证件号
                        </td>
                        <td >
                            <input runat="server"  type="text" maxlength="25"  class="box" id="txtDocumentCode" />
                        </td>
                    </tr>
                    <tr>
                        <td class="head"><span><i class="required">*</i></span>组织机构代码证</td >
                       <td><input runat="server"  type="text" maxlength="25" class="box" id="txtOrgCode" /> </td>
                        <td class="head"><span><i class="required">*</i></span>营业执照</td>
                       <td><input runat="server"  type="text" maxlength="25" class="box" id="txtBusinessLicense" /> </td>
                      </tr>
                      <tr>
                        <td class="head">代理商地址</td >
                       <td> <input runat="server"  type="text" maxlength="25" class="box" id="txtAccAddress" />  </td>
                       <td class="head">性别</td >
                       <td>  <asp:DropDownList runat="server" ID="ddlSex" style="width: 166px; font-size:12px;" CssClass="xl">
                          <asp:ListItem Value="0"  Text="男"></asp:ListItem>
                          <asp:ListItem Value="1"  Text="女"></asp:ListItem>
                          </asp:DropDownList> </td>
                      </tr>
                      <tr>
                        <td class="head">国籍</td >
                       <td>  <input runat="server"  type="text" maxlength="25" class="box" id="txtNationality" />  </td>
                       <td class="head">电话号码</td >
                       <td>  <input runat="server"  type="text" maxlength="25" class="box" id="txtPhoneNumbe" />  </td>
                      </tr>
                      <tr>
                        <td class="head">传真</td >
                       <td>  <input runat="server"  type="text" maxlength="25" class="box" id="txtFax" />  </td>
                       <td class="head">手机</td >
                       <td>  <input runat="server"  type="text" maxlength="25" class="box" id="txtPhone" />  </td>
                      </tr>
                      <tr>
                        <td class="head">邮箱</td >
                       <td>  <input runat="server"  type="text" maxlength="25" class="box" id="txtEmail" />  </td>
                       <td class="head">邮政编码</td >
                       <td>  <input runat="server"  type="text" maxlength="25" class="box" id="txtPostcode" />  </td>
                      </tr>
                </tbody>
            </table>
            </div>
              <div class="mdBtn" style="text-align:center;">
                <a href="#" id="btnupdate"  onclick="return formCheck()" onserverclick="Btn_Save" runat="server" class="btnOr" style=" margin:15px 0 ;"><i class="prnIcon"></i>保存</a>
                <label runat="server" style=" color:Red;" id="lblMsg" visible="false"></label>
              </div>
        </div>
            <div class="blank10">
            </div>
        </div>
        <!--修改资料 end-->
    </div>
    <div class="blank20">
    </div>
    <Footer:Footer ID="Footer" runat="server" />
    </form>
</body>
</html>

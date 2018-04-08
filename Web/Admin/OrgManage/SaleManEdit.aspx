<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleManEdit.aspx.cs" Inherits="Admin_OrgManage_SaleManEdit" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>销售管理</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.tb tbody tr td:even').addClass('odd');
        })

        function formCheck() {
            var str = "";
            if ($.trim($("#txtOrg").val()) == "-1") {
                str = "请选择机构。";
            }
            else if ($.trim($("#txtSaleName").val()) == "") {
                str = "业务员名称不能为空。";
            }
            else if ($.trim($("#txtPhone").val()) == "") {
                str = "联系手机不能为空。";
            }
            else if (!IsMobile($.trim($("#txtPhone").val()))) {
                str = "联系手机不正确。";
            }
            if (str != "") {
                errMsg("提示", str, "", "");
                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <!--当前位置 start-->
   <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">机构管理</a><i>></i>
            <a href="#">机构业务员维护</a>
    </div>
    <!--当前位置 end-->
        <div class="div_content">
            <table class="tb">
                <tbody>
                    <tr>
                        <td width="120">
                            <span><i class="required">*</i>机构</span>
                        </td>
                        <td>
                            <asp:DropDownList runat="server"  ID="txtOrg"  />
                        </td>
                        <td width="120">
                            <span><i class="required">*</i>业务员名称</span>
                        </td>
                        <td>
                            <input type="text" id="txtSaleName" runat="server" class="textBox" maxlength="50"/>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            <span>业务员编码</span>
                        </td>
                        <td>
                            <input type="text" id="txtSaleCode" runat="server" class="textBox" maxlength="50"/>
                        </td>
                        <td width="120">
                            <span><i class="required">*</i>联系手机</span>
                        </td>
                        <td>
                            <input type="text" id="txtPhone" runat="server" class="textBox" maxlength="50"/>
                        </td>
                    </tr>
                    <tr> 
                        <td width="120">
                            <span>邮箱</span>
                        </td>
                        <td>
                            <input type="text" id="txtEmail" runat="server" class="textBox" maxlength="50"/>
                        </td>                       
                        <td>
                            <span>状 态</span>
                        </td>
                        <td colspan="3">
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="rdoStatus0" name="rdoStatus" runat="server" value="1" checked="true" />&nbsp;&nbsp;启
                            用&nbsp;&nbsp;&nbsp;&nbsp;<input type="radio" id="rdoStatus1" name="rdoStatus" runat="server"
                                value="0" />&nbsp;&nbsp;禁 用
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>备注</span>
                        </td>
                        <td colspan="3">
                            <textarea style="height: auto; width: 500px;" rows="3" class="textBox" runat="server" maxlength="1000"
                                id="txtRemark"></textarea>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="div_footer">
            <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return formCheck()"
                OnClick="btnAdd_Click" />&nbsp;
            <input name="" type="button" class="cancel" onclick="javascript:history.go(-1);"
                value="取消" />
        </div>
    </div>
    </form>
</body>
</html>

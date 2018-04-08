<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpPhone.aspx.cs" Inherits="Admin_Systems_UpPhone" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改企业手机号码</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script>

        function formCheck() {
            var str = "";
            if ($.trim($("#txt_NewPhone").val()) == "") {
                str = "新手机号码不能为空。";
            }
            else if (!IsMobile($.trim($("#txt_NewPhone").val()))) {
                str = "新手机号码格式不正确。"
            }
            if (str != "") {
                errMsg("提示", str, "", "");
                return false;
            }
            return true;
        }
    </script>
</head>
<body  style=" overflow:hidden;" >
    <form id="form1" runat="server">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">厂商管理</a><i>></i>
            <a href="UpPhone.aspx" runat="server" id="Atitle">修改企业手机号</a>
    </div>
       <div class="div_content">
          <table class="tb" style=" max-width:430px">
                <tbody>
                    <tr>
                    <td width="20%"> <span>原手机号码</span></td>
                     <td>
                         <input runat="server" type="text" disabled="disabled" readonly="readonly" maxlength="11" class="textBox" id="txt_OldPhone" />
                     </td>
                    </tr>
                    <tr>
                    <td> <span>新手机号码</span></td>
                     <td>
                         <input runat="server" type="text"  maxlength="11" class="textBox" autocomplete="off" id="txt_NewPhone" />
                     </td>
                    </tr>
                </tbody>
            </table>
        </div>

    <div class="div_footer" style=" max-width:430px">
        <input type="button" runat="server" id="btnSubMit" class="orangeBtn"  value="确定" onclick="if(!formCheck()){return false;} " onserverclick="btnSubMit_Click" />
    </div>

    </form>
</body>
</html>

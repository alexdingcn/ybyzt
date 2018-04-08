<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePwd.aspx.cs" Inherits="Company_ChangePwd" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>修改登录密码</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/CommonSha.js"></script>
    <script src="../js/CommonJs.js" type="text/javascript"></script>
    <script>
        function ChkPage() {
            if ($("#txtOldPassWord").val() == "") {
                layerCommon.msg("原密码不能为空", IconOption.错误);
                return false;
            }
            else {
                if ($("#txtNewPassWord").val() == "") {
                    layerCommon.msg("新密码不能为空", IconOption.错误);
                    return false;
                }
                else {
                    if ($("#txtConfrimNewPassWord").val() == "") {
                        layerCommon.msg("确认密码不能为空", IconOption.错误);
                        return false;
                    }
                    else {
                            if ($.trim($("#txtNewPassWord").val()).length < 6) {
                                layerCommon.msg("新密码不能少于6位", IconOption.错误);
                                return false;
                            }
                            else {
                                if ($.trim($("#txtConfrimNewPassWord").val()) != $.trim($("#txtNewPassWord").val())) {
                                    layerCommon.msg("确认密码填写不一致", IconOption.错误);
                                    return false;
                                }
                                else {
                                    if ($.trim($("#txtNewPassWord").val()) == "123456") {
                                        layerCommon.msg("不能使用系统默认密码作为新密码", IconOption.错误);
                                        return false;
                                    }
                                    else {
                                        layerCommon.confirm("您确认要修改吗？", function () {
                                            $("#txtOldPassWord").val(hex_two($("#txtOldPassWord").val()));
                                            $("#txtNewPassWord").val(hex_two($("#txtNewPassWord").val()));
                                            $("#txtConfrimNewPassWord").val(hex_two($("#txtConfrimNewPassWord").val()));
                                            $("#btnModify").trigger("click");
                                        }, "提示");
                                    } 
                                }
                            }
                    }
                }
            }
            return true;
        }

        $(document).ready(function () {
            
            //add by hgh 
            if ('<%=Request["IsDpwd"] %>' == "1") {
                Orderclass("xgdlmm");
            }
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
     <asp:Button ID="btnModify" runat="server" Style="display: none" Text="修改密码" OnClick="btnModify_Click" />
     <div class="rightinfo">
        <div class="place">
	        <ul class="placeul">
                <li><a href="jsc.aspx">我的桌面</a></li><li>></li>
                <li><a href="ChangePwd.aspx">修改登录密码</a></li>
	        </ul>
     </div>
           <div class="tools">
                    <ul class="toolbar left">
                        <li id="liModify" onclick="return ChkPage()"><span><img src="images/t02.png" /></span><font>确定修改</font></li>
                    </ul>
            </div>
               <div class="div_content">
                  <table class="tb" >
                   <tbody>
                      <tr>
                      <td style="width:20%;"><span><i class="required">*</i>原密码</span> </td>
                      <td style="width:70%;"><input type="password" id="txtOldPassWord" runat="server" class="textBox" maxlength="30" width="224"/></td>
                      </tr>
                      <tr>
                       <td><span><i class="required">*</i>新密码</span> </td>
                      <td><input type="password" id="txtNewPassWord" runat="server" class="textBox" maxlength="30" width="224"/></td>
                      </tr>
                      <tr>
                      <td><span><i class="required">*</i>确认密码</span> </td>
                      <td><input type="password" id="txtConfrimNewPassWord" runat="server" class="textBox" maxlength="30" width="224"/></td>
                      </tr>
                   </tbody>
                  </table>
               </div>
     </div>
    </form>
</body>
</html>

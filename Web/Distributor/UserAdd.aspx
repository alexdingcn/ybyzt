<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserAdd.aspx.cs" Inherits="Distributor_UserAdd" %>
<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <title>代理商后台</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../Company/js/js.js" type="text/javascript"></script>
     <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script> 
    <script src="../js/CommonJs.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#orderBg tr:even').addClass("bg");
            $("#btnupdate").click(function () {
                if (formCheck()) {
                    $("#btnSave").trigger("click");
                }
            });

        });
        function Close() {
            location.href = "UsersList.aspx";
        }
        function cancel() {
            window.parent.save();
        }
        function formCheck() {
            var str = "";
            if ($.trim($("#txtPhone").val())=="") {
                str = "手机号码不能为空";
            }
            else if (!IsMobile($.trim($("#txtPhone").val()))) {
                str = "手机号码不正确";
            }
            else if ($.trim($("#txtUserName").val()) == "") {
                str = "登录帐号不能为空";
            }
            else if ($.trim($("#txtUserPwd").val()) == "") {
                str = "密码不能为空";
            }
            else if ($.trim($("#txtPwd").val()) == "") {
                str = "确认密码不能为空";
            }
            else if ($.trim($("#txtTrueName").val()) == "") {
                str = "真实姓名不能为空";
            }
            else if ($.trim($("#txtUserPwd").val()).length < 6) {
                str = "密码不能少于6位";
            }
            else if ($.trim($("#txtPwd").val()) != $.trim($("#txtUserPwd").val())) {
                str = "密码填写不一致";
            }
            
            if (str != "") {
                layerCommon.alert( str, IconOption.错误);
                return false;
            }
            //获取权限
            var chks = document.getElementById("MyRole").getElementsByTagName("input");
            var MyValue = "";
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].checked == true) {
                    MyValue += chks[i].value + ",";
                }
            }
            document.getElementById("hidMyRole").value = MyValue;

            return true;
        }
         function ExisPhone(name) {
            var str = "";
            $.ajax({
                type: "post",
                data: { Action: "GetPhone", Value: name },
                dataType: 'json',
                async: false,
                timeout: 4000,
                success: function (data) {
                    if (data.result) {
                        str = "该手机已被注册请重新填写！";
                    }
                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                    CheckTitle(obj, false, "校验失败，服务器或网络异常");
                }
            })
            return str;
         }

         function keyint() {
             var sss = $("#txtPhone").val();
             keyint(sss)
         }
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
        <div class="w1200">
            <Head:Head ID="Head2" runat="server" />
            <Left:Left ID="Left1" runat="server" ShowID="nav-6" />
            <div class="rightCon">
                <!--当前位置 start-->
                <div class="info">
                    <a href="../jsc.aspx">我的桌面 </a>>
                    <a href="UsersList.aspx" runat="server" id="atitle">员工帐号维护</a>>
                    <a runat="server" id="a1">编辑页面</a>
                </div>
                <div class="blank10"></div>
                <div class="orderNr">
                    <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                    <tbody>
                        <tr>
                       <%--<td style=" width:110px;" class="head"><span>岗位</span> </td>
                       <td ><label style=" text-align:left;min-width:135px; display:inline-block;" runat="server"  ID="ddlRoleId"></label><i class="grayTxt">（对应岗位权限）</i></td>--%>
                       <td class="head"><span><i class="required">*</i>登录帐号</span></td>
                       <td><input runat="server"  type="text"  class="xl" maxlength="50" id="txtUserName" /><i class="grayTxt">（登录系统使用）</i></td>
                       <td style=" width:110px;" class="head"> <span><i class="required">*</i>密码</span></td>
                       <td> <asp:TextBox runat="server" TextMode="Password" maxlength="50" id="txtUserPwd" CssClass="xl"></asp:TextBox></td>
                     </tr>
                     <tr>
                       <td class="head"> <span><i class="required">*</i>人员姓名</span></td>
                       <td> <input runat="server"  type="text" maxlength="50" class="xl" id="txtTrueName" /><i class="grayTxt">（人员真实姓名）</i></td>
                       <td class="head"> <span><i class="required">*</i>确认密码</span></td>
                       <td> <asp:TextBox runat="server" TextMode="Password" maxlength="50" id="txtPwd" CssClass="xl"></asp:TextBox></td>                       
                     </tr>
                     <tr>
                         <td class="head"><span><i class="required">*</i>手机号码</span></td>
                       <td> <input runat="server"  type="text" maxlength="11" class="xl" id="txtPhone" /><font color="red">（登录、发送消息）</font></td>
                       <td class="head"> <span><i class="required">*</i>状态</span> </td>
                       <td>&nbsp;  <input type="radio" runat="server"  name="IsEnabled" value="1" checked="true"  id="rdEnabledYes" />启用&nbsp;&nbsp;<input runat="server" id="rdEnabledNo" name="IsEnabled" type="radio"  value="0" />禁用</td>                       
                     </tr>
                      <tr>
                       <td class="head"><span>身份证号</span></td>
                       <td>   <input runat="server"  type="text" maxlength="50" class="xl" id="txtIdentitys" /><i class="grayTxt">（人员身份证号）</i></td>     
                       <td class="head"><span>邮箱</span></td >
                       <td > <input runat="server"  type="text" maxlength="50" class="xl" id="txtEmail" /><i class="grayTxt">（人员邮箱地址）</i></td>           
                     </tr>
                     <tr>
                       <td class="head"><span>家庭地址</span></td>
                       <td colspan="3"> <input runat="server" style=" width:600px;" maxlength="100" type="text"  class="xl" id="txtAddress" />  </td>
                     </tr>
                    </tbody>
                </table>
                </div>
                <div class="div_content" id="MyRole">
                    <asp:HiddenField ID="hidMyRole" runat="server" />
                  <div class="div_title" style="margin-top:20px;font-size:16px;">岗位权限：</div>
                  <table style="margin-top:10px;" >
                       <tbody>
                         <tr>
                             <td style="font-size:14px;">
                                  <asp:Repeater ID="RepeaterRoles" runat="server" >
                                      <ItemTemplate>
                                           <input id='Menu_<%#Eval("ID")%>' <%#BindRoleSysFun(Eval("ID")+"") %>  type="checkbox"  value='<%#Eval("ID")%>' />
                                            <%#Eval("RoleName")%> &nbsp; &nbsp; &nbsp; &nbsp;
                                      </ItemTemplate>
                                  </asp:Repeater>
                             </td>
                         </tr>
                      </tbody>
                  </table>        
              </div>
                <div class="blank10"></div>
                <div class="mdBtn">
                </br><a href="#" id="btnupdate" runat="server" class="btnYe">确定</a><a href="#" id="btnClose" runat="server" onclick="Close()" class="btnYe" style="margin-left:20px;">返回</a></div>
            </div>
            <asp:Button ID="btnSave" runat="server" Text="确定"  OnClick="btnSave_Click" />
        </div>
    </form>
</body>
</html>

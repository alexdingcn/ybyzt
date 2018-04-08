<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserEdit.aspx.cs" Inherits="Company_SysManager_UserEdit" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>员工帐号编辑</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $_def.ID = "btnAdd";

            if('<%=Request["usertype"]%>'==1)
            {
                $(".rightinfo").css("width","auto");
            }
        });

        function formCheck() {
            var str = "";
            if (stripscript($.trim($("#txtPhone").val())) == "") {
                $("#txtPhone").val(stripscript($.trim($("#txtPhone").val())))
                str = "手机号码不能为空";
            }
            else if (!IsMobile($.trim($("#txtPhone").val()))) {
                str = "手机号码不正确";
            }
            else if (stripscript($.trim($("#txtUserName").val())) == "") {
                $("#txtUserName").val(stripscript($.trim($("#txtUserName").val())))
                str = "登录帐号不能为空";
            }
            else if ($.trim($("#txtUserPwd").val()) == "") {
               
                str = "密码不能为空";
            }
            else if ($.trim($("#txtPwd").val()) == "") {
                str = "确认密码不能为空";
            }
            else if (stripscript($.trim($("#txtTrueName").val())) == "") {
                $("#txtTrueName").val(stripscript($.trim($("#txtTrueName").val())))
                str = "真实姓名不能为空";
            }
            else if ($.trim($("#txtUserPwd").val()).length < 6) {
                str = "密码不能少于6位";
            }
            else if ($.trim($("#txtPwd").val()) != $.trim($("#txtUserPwd").val())) {
                str = "确认密码不一致";
            }
        
            //if(str==""){
            //  str= ExisPhone($.trim($("#txtPhone").val()));
            //}

            if (str != "") {
                 layerCommon.msg(str, IconOption.错误);
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

            //if (MyValue == '')
            //    return confirm('您确定不勾选权限吗？');
            return true;
        }

        function Close() {
            window.location.href = "UserList.aspx";
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
        var div = $('<div   Msg="True"  style="z-index:10; background-color:#000; opacity:0.5; filter:alpha(opacity=50);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');
        $(function () {

            $("#Radio2").click(function () {
                $("body").append(div);
                $(".AddWindow2").css("height", "180px").fadeIn(200);
            });
            $("#Radio1").click(function () {
                $("#SalesManNames").html("");
                $("#DisSalesManID").val("0")
            })
            $(".AddWindow2 .tiptop a,.AddWindow2 .tipbtn .cancel").on("click", function () {
                $(div).remove();
                $(".AddWindow2").fadeOut(100);
                $("#Radio2").prop("checked", true)
                if ($("#DisSalesManID").val()=="0")
                {
                    $("#Radio1")[0].click();
                }
               
            })

            $("#btn").click(function () {
                var str = "";
                if ($("select#DisSalesMan").val() == "-1" || $("select#DisSalesMan option").length == 0) {
                    str = "请选择业务员";
                    layerCommon.msg(str, IconOption.错误);
                    return false;
                }
                var SalesMan = $("#DisSalesMan").val()
                var SalesManName = $("#DisSalesMan").find("option:selected").text();
                $("#SalesManNames").html("(业务员：" + SalesManName + ")");
                $("#DisSalesManID").val(SalesMan)
                $(div).remove();
                $(".AddWindow2").fadeOut(100);

            })

        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
        <div class="rightinfo">
            
            <div class="place">
	            <ul class="placeul">
                    <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                    <li><a href="../SysManager/UserList.aspx" runat="server" id="atitle">员工帐号维护</a></li><li>></li>
                    <li><a href="#" runat="server" id="btitle">员工帐号编辑</a></li>
	            </ul>
            </div>

            <div class="div_content">
               <div class="div_title" style="margin-top:20px;">
               帐号信息：</div>
                  <table class="tb">
                   <tbody>
                     <tr>
                       <td style="width:110px;"><span><i class="required">*</i>登录帐号</span> </td>
                       <td><input runat="server"  type="text"  class="textBox" maxlength="50" id="txtUserName" /><i class="grayTxt">（登录系统使用）</i></td>
                       <td style=" width:110px;"> <span><i class="required">*</i>密码</span></td>
                       <td> <asp:TextBox runat="server" TextMode="Password" maxlength="50" id="txtUserPwd" CssClass="textBox"></asp:TextBox></td>
                     </tr>
                     <tr>
                       <td><span><i class="required">*</i>员工姓名</span></td>
                       <td> <input runat="server"  type="text" maxlength="50" class="textBox" id="txtTrueName" /><i class="grayTxt">（人员真实姓名）</i></td>
                       <td> <span><i class="required">*</i>确认密码</span></td>
                       <td> <asp:TextBox runat="server" TextMode="Password" maxlength="50" id="txtPwd" CssClass="textBox"></asp:TextBox></td>                       
                     </tr>
                    
                     <tr>
                       <td> <span><i class="required">*</i>手机号码</span></td>
                       <td> <input runat="server"  type="text" maxlength="11" class="textBox" id="txtPhone" /><font color="red">（登录、发送消息）</font></td>
                       <td> <span><i class="required">*</i>状态</span> </td>
                       <td>&nbsp;  <input type="radio" runat="server"  name="IsEnabled" value="1" checked="true"  id="rdEnabledYes" />启用&nbsp;&nbsp;<input runat="server" id="rdEnabledNo" name="IsEnabled" type="radio"  value="0" />禁用</td>                       
                     </tr>
                    <tr>
                      <td><span>身份证号</span></td>
                       <td>   <input runat="server"  type="text" maxlength="20" class="textBox" id="txtIdentitys" /><i class="grayTxt">（人员身份证号）</i></td>
                      <td><span>邮箱</span></td>
                       <td > <input runat="server"  type="text" maxlength="50" class="textBox" id="txtEmail" /><i class="grayTxt">（人员邮箱地址）</i></td>
                     </tr>
                     <tr>
                       <td><span>家庭地址</span></td>
                       <td colspan="3"> <input runat="server" style=" width:600px;" maxlength="100" type="text"  class="textBox" id="txtAddress" />  </td>
                     </tr>
                      <tr>
                           <td> <span><i class="required">*</i>用户类型</span> </td>
                       <td colspan="3">
                          <i>&nbsp;  <input type="radio" runat="server"  name="UserType" value="1" checked="true"  id="Radio1" />业务用户&nbsp;&nbsp;
                           <input runat="server" id="Radio2" name="UserType" type="radio"  value="0" />销售用户</i> 
                           <i id="SalesManNames"  runat="server"></i>
                       </td>                       
                      </tr>
                   </tbody>
                  </table>
               </div>

               <div class="div_content" id="MyRole">
                   <div class="div_title" style="margin-top:20px;">
                   岗位权限：</div>
                   <table >
                        <tbody>
                        <tr>
                            <td style="font-size:14px;">
                                <asp:Repeater ID="RepeaterRoles" runat="server" >
                                    <ItemTemplate>
                                       <input id='Menu_<%#Eval("ID")%>' <%#BindRoleSysFun(Eval("ID")+"") %>  type="checkbox" value='<%#Eval("ID")%>' />
                                        <%#Eval("RoleName")%> &nbsp; &nbsp; &nbsp; &nbsp;
                                    </ItemTemplate>
                                </asp:Repeater>
                            </td>
                        </tr>
                        </tbody>
                    </table>
               </div>

               <div  class="div_footer">
                    <br /><br />
                    <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="保存"  OnClientClick="return formCheck()" onclick="btnAdd_Click" />&nbsp;
                    <input id="btnback" runat="server" name="" type="button" class="cancel" onclick="Close()" value="取消" />
               </div>
                <asp:HiddenField ID="hidMyRole" runat="server" />
             </div>

           <div class="AddWindow2 tip" style="display: none;">
         <div style=" position:absolute;top:0; left:0; right:0; bottom:0;z-index:999; background:#fff;">
            <div class="tiptop">
               <span>绑定销售业务员</span><a></a>
            </div>
           <div class="tipinfo">
               <div class="lb">
                    <span><i class="required">*</i>选择业务员：</span>
                    <asp:DropDownList runat="server" ID="DisSalesMan" CssClass="XlCity" ></asp:DropDownList>
                </div>
               <div class="tipbtn"> 
                   <a href="#" id="btn" class="orangeBtn" style="color:#ffffff;font-size: 14px;background-color:#ff6a00;height:30px;">确定</a>&nbsp;
                    <input name="" type="button" class="cancel" value="取消" />
                     <input type="hidden" id="DisSalesManID" runat="server" value="0"/>
                </div>
           </div>
           
       </div>
       </div>
    </form>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserModify.aspx.cs" Inherits="Admin_Role_UserModify"  EnableEventValidation="false"%>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>销售管理</title>
       <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            $('.tb tbody tr td:even').addClass('odd');
            $("#cancel").on("click", function () {
                location.href = "../Systems/UserInfo.aspx?RoleID=<%=RoleID%>&KeyID="+<%=KeyID%>;
            });
        });

        function Close() {
            if ('<%=Request["ntype"]%>'=='1')
            {
                history.go(-1);
            }
            if('<%=Request["ntype"]%>'=='2')
            {
                window.parent.Layerclose();
            }
        }

        function formCheck() {
            var str = "";
            if ($.trim($("#txtusername").val()) == "" ) {
                str = "登录帐号不能为空";
            }
            else if ($.trim($("#txtpwd").val()) == "") {
                str = "密码不能为空";
            } else if ($.trim($("#txtpwd2").val()) == "") {
                str = "确认密码不能为空";
            }
            else if ($.trim($("#txtpwd").val()).length<6) {
                str = "密码不能少于6位";
            }
            else if ($.trim($("#txtpwd").val()) != $.trim($("#txtpwd2").val())) {
                str = "两次密码不一致，请确认";
            }

            if (str != "") {
                errMsg("提示", str, "", "");
                return false;
            }
            return true;
        }
        function GetSaleMan() {
            $("#SaleMan").empty();
            var Org = $("#Org").val();
            $("<option></option>")
                .val("-1")
                .text("全部")
                .appendTo($("#SaleMan"));
            $.ajax({
                type: "post",
                data: { Action: "Action", OrgID: Org },
                success: function (data) {
                    data = eval("(" + data + ")");
                    $.each(data, function (i, item) {
                        $("<option></option>")
                        .val(item["ID"])
                        .text(item["SalesName"])
                        .appendTo($("#SaleMan"));
                    });

                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                    CheckTitle(obj, false, "服务器或网络异常");
                }
            })
        }
              var div = $('<div   Msg="True"  style="z-index:10; background-color:#000; opacity:0.5; filter:alpha(opacity=50);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');
        $(function () {
            GetSaleMan()
            $("#rdotype3").click(function () {
                $("body").append(div);
                $(".AddWindow2").css("height", "180px").fadeIn(200);
            });
               
            $(".AddWindow2 .tiptop a,.AddWindow2 .tipbtn .cancel").on("click", function () {
                $(div).remove();
                $(".AddWindow2").fadeOut(100);
                $("#rdotype2").prop("checked",true)

            })

            $("#Org").change(function () {
                GetSaleMan();
            })

            $("#btn").click(function () {
                var str = "";
                if ($("select#Org").val() == "-1" || $("select#Org option").length == 0) {
                    str = "请选择机构";
                    layerCommon.msg(str, IconOption.错误);
                    return false;
                }
                if ($("select#SaleMan").val() == "-1" || $("select#SaleMan option").length == 0) {
                    str = "请选择机构业务员";
                    layerCommon.msg(str, IconOption.错误);
                    return false;
                }
                var SalesMan = $("#SaleMan").val()
                var orgids = $("#Org").val()
                var SalesManName = $("#SaleMan").find("option:selected").text();
                $("#SalesManNames").html("(机构业务员：" + SalesManName + ")");
                $("#salemanid").val(SalesMan)
                $("#orgids").val(orgids)
                $(div).remove();
                $(".AddWindow2").fadeOut(100);

            })
        })
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
            <a href="#">岗位权限维护</a><i>></i>
            <a href="#">用户管理</a>
    </div>
    <!--当前位置 end-->
        <div class="div_content">
            <table class="tb">
                <tbody>
                    <tr>
                        <td width="120">
                            <span><i class="required">*</i>登录帐号</span>
                        </td>
                        <td>
                            <input type="text" id="txtusername" runat="server" class="textBox" maxlength="50" />
                            <label runat="server" id="lblusername"></label>
                        </td>
                        <td width="120">
                            <span>真实姓名</span>
                        </td>
                        <td>
                            <input type="text" id="txtturename" runat="server" class="textBox" maxlength="50" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><i class="required">*</i>密 码</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpwd" TextMode="Password"  runat="server"  CssClass="textBox" maxlength="50"></asp:TextBox>
                        </td>
                        <td>
                            <span><i class="required">*</i>确认密码</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpwd2" TextMode="Password"  runat="server"  CssClass="textBox" maxlength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <span>电 话</span>
                        </td>
                        <td>
                            <input type="text" id="txttel" runat="server" class="textBox" maxlength="50"/>
                        </td>
                        <td>
                            <span>状 态</span>
                        </td>
                        <td>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="rdoStatus0" name="rdoStatus" runat="server" value="1" checked="true" />&nbsp;&nbsp;启
                            用&nbsp;&nbsp;&nbsp;&nbsp;<input type="radio" id="rdoStatus1" name="rdoStatus" runat="server"
                                value="0" />&nbsp;&nbsp;禁 用
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>类 型</span>
                        </td>
                        <td>
                            <i>
                            <%--&nbsp;&nbsp;&nbsp;&nbsp;
                            <input type="radio" id="rdotype1" name="rdotype" runat="server" value="1"/>&nbsp;&nbsp;系统管理员
                            --%>&nbsp;&nbsp;&nbsp;&nbsp;<input type="radio" id="rdotype2" name="rdotype" runat="server"
                                value="2"  />&nbsp;&nbsp;系统用户
                               <input type="radio" id="rdotype3" name="rdotype" runat="server"
                                value="4"   />&nbsp;&nbsp;机构业务员</i><i id="SalesManNames" runat="server"></i>
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
            <input id="cancel" name="qx" type="button" runat="server" class="cancel"  value="取消" />
        </div>
    </div>

          

         <div class="AddWindow2 tip" style="display:none;">
         <div style=" position:absolute;top:0; left:0; right:0; bottom:0;z-index:999; background:#fff;">
            <div class="tiptop">
               <span>绑定机构业务员</span><a></a>
            </div>
           <div class="tipinfo">
               <div class="lb">
                    <span><i class="required">*</i>机构：</span>
                    <asp:DropDownList runat="server" ID="Org" CssClass="XlCity" ></asp:DropDownList>

                </div>
               <div class="lb">
                    <span><i class="required">*</i>业务员：</span>
                    <asp:DropDownList runat="server" ID="SaleMan" CssClass="XlCity" ></asp:DropDownList>
               </div>
               <div class="tipbtn"> 
                   <a href="#" id="btn" class="orangeBtn" style="color:#ffffff;font-size: 14px;background-color:#ff6a00;height:30px;">确定</a>&nbsp;
                    <input name="" type="button" class="cancel" value="取消" />
                     <input type="hidden" id="salemanid" runat="server" value="0"/>
                    <input type="hidden" id="orgids" runat="server" value="0"/>
                </div>
           </div>
           
       </div>
       </div>
    </form>
</body>
</html>

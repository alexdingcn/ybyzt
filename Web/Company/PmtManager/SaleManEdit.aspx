<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleManEdit.aspx.cs" Inherits="Admin_OrgManage_SaleManEdit" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>销售人员维护修改</title>
    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $_def.ID = "btnAdd";

            if('<%=Request["lefttype"]+"" %>'!=""){
                $(".rightinfo").css("width","850px");
            }

            $('.tb tbody tr td:even').addClass('odd');

            if ($("#txtSMType").val() == '<%=(int)Enums.DisSMType.业务员%>') {
                $("#txtSMType").parent("td").removeAttr("colspan");
                $("td.TDSMPrent").show();
            } else {
                $("#txtSMType").parent("td").attr("colspan", "3");
                $("td.TDSMPrent").hide();
            }


            $("#txtSMType").on("change", function () {
                if ($(this).val() == '<%=(int)Enums.DisSMType.业务员%>') {
                    $("#txtSMType").parent("td").removeAttr("colspan");
                    $("td.TDSMPrent").show();
                } else {
                    $("#txtSMType").parent("td").attr("colspan","3");
                    $("td.TDSMPrent").hide();
                }
            });

        })

        function formCheck() {
            var str = "";
            if ($.trim($("#txtSaleName").val()) == "") {
                str = "销售人员名称不能为空。";
            }
            else if ($("#txtSMType").val() == '<%=(int)Enums.DisSMType.业务员%>' && ($("#ddlSMParent option").length==1 && $("#ddlSMParent").val()=="-1") ) {
                str = "无业务经理请先添加业务经理";
            }
            else if ($("#txtSMType").val() == '<%=(int)Enums.DisSMType.业务员%>' && $("#ddlSMParent").val() == "-1") {
                str = "请选择业务经理";
            }
            else if ($.trim($("#txtPhone").val()) == "") {
                str = "联系手机不能为空。";
            }
            else if (!IsMobile($.trim($("#txtPhone").val()))) {
                str = "联系手机不正确。";
            }
            if (str != "") {
                layerCommon.msg(str, IconOption.错误, 3000);
                return false;
            }
            return true;
        }
        function Close() {
            if ('<%=Request["posttype"]%>'=="1")
            {
                window.parent.LayerClose();
            }
            else
            {
                history.go(-1);
            }
        }
        function cancel(name,id) {
            window.parent.save(name,id);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
    <div class="rightinfo" id="right" runat="server">
        <!--当前位置 start-->
        <div class="place" runat="server" id="first">
               <ul class="placeul">
                <li><a href="../jsc.aspx" target="_top">我的桌面 </a></li><li>></li>
                <li><a href="../PmtManager/SaleManList.aspx">销售人员维护</a></li><li>></li>
                <li><a href="#">销售人员维护修改</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <div class="div_content">
            <table class="tb">
                <tbody>
                    <tr>
                        <td width="120">
                            <span><i class="required">*</i>销售人员类型</span>
                        </td>
                        <td colspan="3">
                            <asp:DropDownList runat="server"  ID="txtSMType"  />
                        </td>
                        <td width="120" class="TDSMPrent" style=" display:none;">
                            <span><i class="required">*</i>选择业务经理</span>
                        </td>
                        <td  class="TDSMPrent" style=" display:none;">
                            <asp:DropDownList runat="server"  ID="ddlSMParent"  />
                        </td>
              
                    </tr>
                    <tr>
                      <td width="120">
                            <span><i class="required">*</i>销售人员名称</span>
                        </td>
                        <td>
                            <input type="text" id="txtSaleName" runat="server" class="textBox" maxlength="50"/>
                        </td>
                        <td width="120">
                            <span>销售人员编码</span>
                        </td>
                        <td>
                            <input type="text" id="txtSaleCode" runat="server" class="textBox" maxlength="50"/>
                        </td>
                    </tr>
                    <tr> 
                       <td width="120">
                            <span><i class="required">*</i>联系手机</span>
                        </td>
                        <td>
                            <input type="text" id="txtPhone" runat="server" class="textBox" maxlength="11"/>
                        </td>
                        <td width="120">
                            <span>邮箱</span>
                        </td>
                        <td>
                            <input type="text" id="txtEmail" runat="server" class="textBox" maxlength="50"/>
                        </td>                       
                    </tr>
                     <tr>
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
            <input name="" type="button" class="cancel" onclick="Close()"
                value="取消" />
        </div>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SettlementAdd.aspx.cs" Inherits="Company_Financing_SettlementAdd" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/Admin/UserControl/TextBankList.ascx" TagPrefix="uc1" TagName="BankSelect" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>结算账户维护</title>
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script type="text/jscript">
        $(function () {
            $(window.parent.leftFrame.document).find(".menuson li.active").removeClass("active");
            window.parent.leftFrame.document.getElementById("jszhwh").className = "active";

        });
        $(document).ready(function () {
            $("select#selePrcCd").on("change", function () {
                $("select#seleCityCd").empty();
                $.ajax({
                    type: "post",
                    data: { action: "GetCity", Pcode: $(this).val() },
                    dataType: 'text',
                    success: function (data) {
                        if (data != "") {
                            $("select#seleCityCd").html(data);
                            if ($("#hdCityCd").val() != "" && $("#hdCityCd").val() != "-1" && $("select#seleCityCd option[value=" + $("#hdCityCd").val() + "]").length > 0) {
                                $("select#seleCityCd").val($("#hdCityCd").val());
                            } else {
                                $("#hdCityCd").val($("select#seleCityCd").val());
                            }
                        } else {
                            $("select#seleCityCd").html("<option value=\"-1\">请选择</option>")
                        }
                    }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                        $("select#seleCityCd").html("<option value=\"-1\">请选择</option>");
                    }
                })
            })
            $("select#seleCityCd").on("change", function () {
                $("#hdCityCd").val($(this).val());
            })

            if ($("#hdCityCd").val() != "" && $("#hdCityCd").val()) {
                $("select#selePrcCd").trigger("change");
            }

        })


        function SetBank(name) {
            $("input#txtBkNm").val(name);
        }

        function verify() {
            var str = "";
            if ($.trim($("#txtAccNo").val()) == "") {
                str = "银行帐号不能为空";
            }
            else if ($.trim($("#txtBkNm").val()) == "") {
                str = "开户网点名称不能为空";
            }
            else if ($("#seleCrsMk").val() == '<%=(int)Enums.Pay_CrsMk.跨行%>') {
                if ($.trim($("#<%=Boxbank.txtBankID %>").val()) == "") {
                    str = "分支行清算行号不能为空";
                }
                else if ($.trim($("#selePrcCd").val()) == "" || $.trim($("#selePrcCd").val()) == "-1") {
                    str = "请选择分支行省份";
                }
                else if ($.trim($("#seleCityCd").val()) == "" || $.trim($("#seleCityCd").val()) == "-1") {
                    str = "请选择分支行城市编号";
                }
                else if ($.trim($("#txtBkAddr").val()) == "") {
                    str = "开户行网点地址";
                }
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
    <uc1:top ID="top1" runat="server" ShowID="" />

    <div class="rightinfo">
    <!--当前位置 start-->
    <div class="place">
        <ul class="placeul">
            <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
            <li><a href="#">在线融资</a></li><li>></li>
            <li><a href="#">结算账户维护</a></li>
        </ul>
    </div>
        <div class="div_content">
            <div class="lbtb">
                <table class="dh">
                    <tr>
                        <td style="width:10%;">
                            <span><label class="required">*</label>银行帐号</span>
                        </td>
                        <td style=" width:30%;">
                            <input runat="server" style=" width:200px;" onkeyup="$(this).val($(this).val().replace(/\D/g, '').replace(/....(?!$)/g, '$& '))" type="text" class="textBox" maxlength="50" id="txtAccNo" />
                        </td>
                        <td style="width:10%;">
                            <span><label class="required">*</label>开户名称</span>
                        </td>
                        <td style=" width:30%;">
                            <label id="lblAccNm" runat="server">
                                </label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%;">
                            <span><label class="required">*</label>账户类别</span>
                        </td>
                        <td style=" width:30%;">
                            <select id="seleAccTp" runat="server" style="width:202px;" class="textBox">
                                <option value="1">公司</option>
                                <option value="2" selected="selected">个人</option>
                            </select>
                        </td>
                        <td style="width:10%;">
                            <span><label class="required">*</label>跨行标示</span>
                        </td>
                        <td style=" width:30%;">
                            <select id="seleCrsMk" runat="server" style="width:202px;" class="textBox">
                                <option value="1">本行</option>
                                <option value="2" selected="selected">跨行</option>
                            </select>
                            <i  style='margin-left:5px;color: #aaaaaa;font-style: normal;'>暂只支持跨行</i>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%;">
                            <span><label class="required">*</label>分支行清算行号</span>
                        </td>
                        <td style=" width:30%;">
                           <%-- <input runat="server"  type="text" style=" width:200px;" maxlength="50" class="textBox" id="txtOpenBkCd" />--%>
                            <uc1:BankSelect runat="server" SetNameFc="SetBank(BankName)" ID="Boxbank"></uc1:BankSelect>
                        </td>
                        <td style="width:10%;">
                            <span><label class="required">*</label>开户行网点名称</span>
                        </td>
                        <td style=" width:30%;">
                            <input runat="server"  type="text" style=" width:200px;" maxlength="50" class="textBox" id="txtBkNm" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%;">
                            <span><label class="required">*</label>分支行省份</span>
                        </td>
                        <td style=" width:30%;">
                            <select id="selePrcCd" runat="server" style="width:202px;" class="textBox">
                            </select>
                        </td>
                        <td style="width:10%;">
                            <span><label class="required">*</label>分支行城市</span>
                        </td>
                        <td style=" width:30%;">
                            <select id="seleCityCd" name="seleCityCd" style="width:202px;" class="textBox">
                            <option  value="-1">请选择</option>
                            </select>
                            <input type="hidden" runat="server" id="hdCityCd" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:10%;">
                            <span><label class="required">*</label>开户行网点地址</span>
                        </td>
                        <td colspan="3">
                            <input runat="server" style=" width:200px;"  type="text" maxlength="50" class="textBox" id="txtBkAddr" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="footerBtn">
                <asp:Button ID="btnupdate" runat="server" Text="确定" CssClass="orangeBtn" OnClick="Btn_Save"
                    OnClientClick="return verify();" />&nbsp;
                <label runat="server" style=" color:Red;" id="lblMsg" visible="false"></label>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

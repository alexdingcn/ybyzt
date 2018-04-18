<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SettlementAdd.aspx.cs" Inherits="Financing_SettlementAdd" %>
<%@ Register Src="~/Admin/UserControl/TextBankList.ascx" TagPrefix="uc1" TagName="BankSelect" %>
<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>结算账户新增</title>
    <link href="../Distributor/css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>

    <script type="text/jscript">
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
            else if ($("#seleCrsMk").val() == '<%=(int)Enums.Pay_CrsMk.跨行%>') 
            {
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
    <Left:Left ID="Left1" runat="server" ShowID="SettlementAdd" />
    <div class="rightCon">
    <div class="info"><span class="homeIcon"></span><a id="navigation1" href="#">基本资料</a>><a id="navigation2" href="#" class="cur">基本信息</a></div>

        <div class="userTrend">
            <div class="uTitle">
                <b>结算账户新增</b></div>
                <div class="">
        <div  class="orderNr">
            <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                <tbody>
                    
                    <tr>                      
                        <td class="head" style="width: 10%">
                            <span><i class="required">*</i></span>银行帐号
                        </td>
                        <td style="width: 23%">
                            <input runat="server" style=" width:200px;" onkeyup="$(this).val($(this).val().replace(/\D/g, '').replace(/....(?!$)/g, '$& '))" type="text" class="box" maxlength="50" id="txtAccNo" />
                        </td>
                        <td class="head" style="width: 10%">
                            <span>开户名称
                        </td>
                        <td style="width: 23%">
                             <label id="lblAccNm" runat="server">
                                </label>
                        </td>
                    </tr>
                    <tr>
                        <td class="head">
                            <span><i class="required">*</i></span>账户类别
                        </td>
                        <td >
                            <select id="seleAccTp" runat="server" style="width:217px; font-size:12px;" class="xl">
                                <option value="1">公司</option>
                                <option value="2" selected="selected">个人</option>
                            </select>
                        </td>
                        <td class="head">
                            <span><i class="required">*</i></span>跨行标示
                        </td>
                        <td >
                            <select id="seleCrsMk" runat="server" style="width:217px; font-size:12px;" class="xl">
                                <option value="1">本行</option>
                                <option value="2" selected="selected">跨行</option>
                            </select>
                            <i  style='margin-left:5px;color: #aaaaaa;font-style: normal;'>暂只支持跨行</i>
                        </td>
                    </tr>
                    <tr>
                        <td class="head"><span><i class="required">*</i></span>分支行清算行号</td >
                       <td> <%--<input runat="server"  type="text" style=" width:200px;" maxlength="50" class="box" id="txtOpenBkCd" />--%>
                        <uc1:BankSelect runat="server" SetNameFc="SetBank(BankName)"  Class="box" ID="Boxbank"></uc1:BankSelect>
                       </td>
                         <td class="head"><span><i class="required">*</i></span>开户行网点名称</td >
                       <td>  <input runat="server"  type="text" style=" width:200px;" maxlength="50" class="box" id="txtBkNm" />  </td>
                      </tr>
                      <tr>
                      <td class="head"><span><i class="required">*</i></span>分支行省份</td>
                       <td>
                       <select id="selePrcCd" runat="server" style="width:217px; font-size:12px;" class="xl">
                       </select>
                       </td>
                        <td class="head"><span><i class="required">*</i></span>分支行城市</td >
                       <td>  
                       <select id="seleCityCd" name="seleCityCd" style="width:217px; font-size:12px;" class="xl">
                       <option  value="-1">请选择</option>
                       </select>
                       <input type="hidden" runat="server" id="hdCityCd" />
                       </td>
                      </tr>
                      <tr>
                        <td class="head"><span><i class="required">*</i></span>开户行网点地址</td >
                       <td colspan="3">  <input runat="server" style=" width:200px;"  type="text" maxlength="50" class="box" id="txtBkAddr" />  </td>
                      </tr>
                </tbody>
            </table>
      </div>
            </div>
        
            <div class="mdBtn">
                <br /><a href="#" id="btnupdate" onclick="return verify()" onserverclick="Btn_Save" runat="server" class="btnOr">保存</a>
                <label runat="server" style=" color:Red;" id="lblMsg" visible="false"></label>
                </div>
            <div class="blank10">
            </div>
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

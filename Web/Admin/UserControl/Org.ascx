<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Org.ascx.cs" Inherits="Admin_UserControl_Org" %>
<script type="text/javascript">
    $(document).ready(function () {
        GetSaleMan();
        $("#Org").change(function () { $("#salemanid").val(""); GetSaleMan(); });
        $("#SaleMan").change(function () { GetOrg() });
    })
    function GetOrg() {
        $("#salemanid").val($("#SaleMan").val());
        $("#hid").val($("#SaleMan").val());
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
                if ($("#salemanid").val() == "") {
                    $("#SaleMan option[value='" + $("#hid").val() + "']").attr("selected", true);
                    $("#salemanid").val($("#SaleMan").val());
                } else {
                    $("#SaleMan").val($("#salemanid").val());
                }
            }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                CheckTitle(obj, false, "服务器或网络异常");
            }
        })
    }
    </script>
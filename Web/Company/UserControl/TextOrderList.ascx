<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TextOrderList.ascx.cs" Inherits="Disany_UserControl_TextOrderList" %>
<script>
    $(document).ready(function () {
        $("#<%=txt_OrderCoder.ClientID %>").on("click", function () {
            //            var height = document.body.clientHeight; //计算高度
            var height = document.documentElement.clientHeight;
            var layerOffsetY = (height - 500) / 2; //计算宽度
            var index = layerCommon.openWindow('选择订单', '<%=ResolveUrl("SelectOrderList.aspx")%>?compid='+<%=CompID %>, '900px', '500px', layerOffsetY);
            $("#<%=hid_Alertorder.ClientID %>").val(index);
        })
    })

    function selectOrder(Disid, DisName) {
        $("#<%=txt_OrderCoder.ClientID %>").val(DisName);
        $("#<%=hid_OrderID.ClientID %>").val(Disid);
        CloseDialogTo_order();
    }

    function CloseDialogTo_order() {
        layerCommon.layerClose($("#<%=hid_Alertorder.ClientID %>").val());
    }

</script>
  <input type="hidden" id="hid_Alertorder" runat="server" />
<input runat="server" type="text" id="txt_OrderCoder" readonly="readonly" class="textBox txt_OrderCoder" style="cursor: pointer; " />
<input runat="server" name="hid_product_class"  type="hidden" id="hid_OrderID" />
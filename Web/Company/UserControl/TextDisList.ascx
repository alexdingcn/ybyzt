<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TextDisList.ascx.cs" Inherits="Disany_UserControl_TextDisList" %>
<script>
    $(document).ready(function () {
        $("#<%=txt_txtDisName.ClientID %>").on("click", function () {
            //            var height = document.body.clientHeight; //计算高度
            var height = document.documentElement.clientHeight;
            var layerOffsetY = (height - 500) / 2; //计算宽度
            var index = layerCommon.openWindow('选择代理商', '<%=ResolveUrl("SelectDisList.aspx")%>?compid='+<%=CompID %>, '900px', '500px');
            $("#<%=hid_Alert.ClientID %>").val(index);
        })
    })

    function selectDis(Disid, DisName) {
        $("#<%=txt_txtDisName.ClientID %>").val(DisName);
        $("#<%=hid_DisId.ClientID %>").val(Disid);
        CloseDialogTo();
    }

    function CloseDialogTo() {
          layerCommon.layerClose($("#<%=hid_Alert.ClientID %>").val());
    }

</script>
  <input type="hidden" id="hid_Alert" runat="server" />
<input runat="server" type="text" id="txt_txtDisName" readonly="readonly" class="textBox txtDisName" style="cursor: pointer; " />
<input runat="server" name="hid_product_class"  type="hidden" id="hid_DisId" />
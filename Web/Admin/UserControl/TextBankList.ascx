<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TextBankList.ascx.cs" Inherits="Admin_UserControl_TextBankList" %>
<script>
    $(document).ready(function () {
        $("#<%=txt_txtBankid.ClientID %>").on("click", function () {
            //            var height = document.body.clientHeight; //计算高度
            var height = document.documentElement.clientHeight;
            var layerOffsetY = (height - 500) / 2; //计算宽度
            var index = showDialog('选择结算银行', '<%=ResolveUrl("SelectBankList.aspx")%>', '950px', '500px', layerOffsetY);
            $("#<%=hid_Alert.ClientID %>").val(index);
        })
    })

    function selectBank(Bankid, BankName) {
        $("#<%=txt_txtBankid.ClientID %>").val(Bankid);
        CloseDialogTo();
        <%  if(!string.IsNullOrWhiteSpace(SetNameFc)) {%>
          <%=SetNameFc %>;
        <%} %>
    }

    function CloseDialogTo() {
        CloseDialogRe("<%=hid_Alert.ClientID %>");
    }

</script>
  <input type="hidden" id="hid_Alert" runat="server" />
<input runat="server" type="text" id="txt_txtBankid" readonly="readonly" style=" width:200px;cursor: pointer; " class="textBox txtBankid" />
<%--<input runat="server" name="hid_product_class"  type="hidden" id="hid_BankId" />--%>
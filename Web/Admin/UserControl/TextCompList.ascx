<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TextCompList.ascx.cs" Inherits="Admin_UserControl_TextCompList" %>
<script>
    $(document).ready(function () {
        $("#<%=txt_txtCompName.ClientID %>").on("click", function () {
            //            var height = document.body.clientHeight; //计算高度
            var height = document.documentElement.clientHeight;
            var layerOffsetY = (height - 500) / 2; //计算宽度
            var index = showDialog('选择企业', '<%=ResolveUrl("SelectCompList.aspx")%>', '900px', '500px', layerOffsetY);
            $("#<%=hid_Alert.ClientID %>").val(index);
        })
    })

    function selectComp(compid, compName) {
        $("#<%=txt_txtCompName.ClientID %>").val(compName);
        $("#<%=hid_CompId.ClientID %>").val(compid);
        CloseDialogTo();
            if ($("#BoxComp_txt_txtCompName").length > 0) {
                    CheckTitle($("#BoxComp_txt_txtCompName"), true);
                    ExisDis($("#txtDisName"), $.trim($("#txtDisName").val()), false);
                }
         <% if(RTFunc!=""){ %>
           <%=RTFunc%>();
         <%} %>
           if (typeof (BindrOl) == "function") {
                    BindrOl();
           }
    }

    function CloseDialogTo() {
        CloseDialogRe("<%=hid_Alert.ClientID %>");
    }

</script>
  <input type="hidden" id="hid_Alert" runat="server" />
<input runat="server" type="text" id="txt_txtCompName" readonly="readonly" class="textBox txtCompName" style="cursor: pointer; " />
<label runat="server" visible="false" id="lblIndusName"></label>
<input runat="server" name="hid_product_class"  type="hidden" id="hid_CompId" />
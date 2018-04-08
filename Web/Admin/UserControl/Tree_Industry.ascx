<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Tree_Industry.ascx.cs" Inherits="Admin_UserControl_Tree_Industry" %>
<link href="<%=ResolveUrl("../../css/zTreeStyle/zTreeStyle.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=ResolveUrl("../../js/jquery.ztree.core-3.5.js")%>" type="text/javascript" ></script>

<script>
    $(document).ready(function () {
        $("#<%=txt_txtIndusname.ClientID %>").on("click", function () {
            var x = $(this).offset().left;
            var y = $(this).offset().top;
            ChoseProductClassRe('<%=ResolveUrl("Tree_IndusTryPage.aspx") %>', x, y, "<%=hid_Alert.ClientID %>");
        });
    })

    function celar() {
        $("#<%=txt_txtIndusname.ClientID %>").val("");
        $("#<%=hid_IndusId.ClientID %>").val("");
        CloseDialogRe("<%=hid_Alert.ClientID %>");
    }

    function CloseProductClassIndus(id, name) {
        $("#<%=txt_txtIndusname.ClientID %>").focus(); //解决 IE11 弹出层后文本框不能输入
        CloseDialogRe("<%=hid_Alert.ClientID %>");
        $("#<%=txt_txtIndusname.ClientID %>").val(name); //区域名称
        $("#<%=hid_IndusId.ClientID %>").val(id); //区域id
    }
</script>
  <input type="hidden" id="hid_Alert" runat="server" />
<input runat="server" type="text" id="txt_txtIndusname" readonly="readonly" class="textBox txtIndusname" style="cursor: pointer; " />
<label runat="server" visible="false" id="lblIndusName"></label>
<input runat="server" name="hid_product_class"  type="hidden" id="hid_IndusId" />
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DisTypeTreeBox.ascx.cs" Inherits="Company_UserContro_DisTypeTreeBox" %>
<link href="<%=ResolveUrl("../../css/zTreeStyle/zTreeStyle.css") %>?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
<script src="<%=ResolveUrl("../../js/jquery.ztree.core-3.5.js")%>?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript" ></script>

<script>
    $(document).ready(function () {
        $("#<%=txt_txtTypename.ClientID %>").on("click", function () {
            var x = $(this).offset().left;
            var y = $(this).offset().top;
             <% if(CompHDID=="") {%>
            ChoseProductClassRe('<%=ResolveUrl("../SysManager/TreeDisType.aspx") %>?compid=<%=CompID %>', x, y, "<%=hid_Alert.ClientID %>");
             <% } else {%>
               if($.trim($("#<%=CompHDID %>").val())==""){
                errMsg("提示","请选择企业","","");
              }else{
               ChoseProductClassRe('<%=ResolveUrl("../SysManager/TreeDisType.aspx") %>?compid='+$("#<%=CompHDID %>").val(), x, y, "<%=hid_Alert.ClientID %>");
              }

             <%} %>
        });
    })

    function TypeLoad(){
        $("#<%=txt_txtTypename.ClientID %>").trigger("click");
    }

    function Typecelar() {
        $("#<%=txt_txtTypename.ClientID %>").val("");
        $("#<%=hid_TypeId.ClientID %>").val("");
        CloseDialogRe("<%=hid_Alert.ClientID %>");
    }

    function TypeClose(){
    CloseDialogRe("<%=hid_Alert.ClientID %>");
    }

    function CloseProductClassId(id, name) {
        $("#<%=txt_txtTypename.ClientID %>").focus(); //解决 IE11 弹出层后文本框不能输入
        CloseDialogRe("<%=hid_Alert.ClientID %>");
        $("#<%=txt_txtTypename.ClientID %>").val(name); //分类名称
        $("#<%=hid_TypeId.ClientID %>").val(id); //分类id
    }
</script>
  <input type="hidden" id="hid_Alert" runat="server" class="hid_Alert" />
<input runat="server" type="text" id="txt_txtTypename" readonly="readonly" class="textBox txt_txtTypename" style="cursor: pointer;" />
<input runat="server" name="hid_product_class"  type="hidden" id="hid_TypeId" />

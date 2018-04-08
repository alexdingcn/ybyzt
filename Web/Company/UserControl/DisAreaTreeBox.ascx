<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DisAreaTreeBox.ascx.cs" Inherits="Company_UserContro_DisAreaTreeBox" %>
<link href="<%=ResolveUrl("../../css/zTreeStyle/zTreeStyle.css") %>?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
<script src="<%=ResolveUrl("../../js/jquery.ztree.core-3.5.js")%>?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript" ></script>

<script>
    $(document).ready(function () {
        $("#<%=txt_txtAreaname.ClientID %>").on("click", function () {
            var x = $(this).offset().left;
            var y = $(this).offset().top;
            <% if(CompHDID=="") {%>
            ChoseProductClassRe('<%=ResolveUrl("../SysManager/TreeDisArea.aspx") %>?compid=<%=CompID %>', x, y, "<%=hid_Alert.ClientID %>");
              <% } else {%>
              if($.trim($("#<%=CompHDID %>").val())==""){
                errMsg("提示","请选择企业","","");
              }
              else{
               ChoseProductClassRe('<%=ResolveUrl("../SysManager/TreeDisArea.aspx") %>?compid='+$("#<%=CompHDID %>").val(), x, y, "<%=hid_Alert.ClientID %>");
              }

              <%} %>
        });
    })

    function AreaLoad(){
        $("#<%=txt_txtAreaname.ClientID %>").trigger("click");
    }

    function Areacelar() {
        $("#<%=txt_txtAreaname.ClientID %>").val("");
        $("#<%=hid_AreaId.ClientID %>").val("");
        CloseDialogRe("<%=hid_Alert.ClientID %>");
    }

    function AreaClose(){
     CloseDialogRe("<%=hid_Alert.ClientID %>");
    }

    function CloseProductClassArea(id, name) {
        $("#<%=txt_txtAreaname.ClientID %>").focus(); //解决 IE11 弹出层后文本框不能输入
        CloseDialogRe("<%=hid_Alert.ClientID %>");
        $("#<%=txt_txtAreaname.ClientID %>").val(name); //区域名称
        $("#<%=hid_AreaId.ClientID %>").val(id); //区域id
    }
</script>
  <input type="hidden" id="hid_Alert" runat="server" class="hid_Alert"/>
<input runat="server" type="text" id="txt_txtAreaname" readonly="readonly" class="textBox txt_txtAreaname" style="cursor: pointer;" />
<input runat="server" name="hid_product_class"  type="hidden" id="hid_AreaId" />
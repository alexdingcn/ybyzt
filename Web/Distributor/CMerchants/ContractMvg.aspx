<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ContractMvg.aspx.cs" Inherits="Distributor_CMerchants_ContractMvg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>申请合作</title>
    
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../Company/js/order.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#btnCancel").on("click", function () {
                window.parent.layerCommon.layerClose("hid_Alert");
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="orderNr">
            <input type="hidden" id="hidProvideData" runat="server" />
            <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                <tbody>
                    <tr runat="server" id="UpFile1" visible="false"> 
                        <td class="head" style="width: 15%"><i class="red">*</i>附件</td>
                        <td style="width: 37%">
                            <div class="con upload">
                                 <div style="float:left">
                                     <a href="javascript:;" class="a-upload bclor le"> 
                                     <input id="uploadFile" runat="server" type="file" 
                                         onclick="return uploadFileClick()"
                                         name="fileAttachment"  class="AddBanner"/>上传附件</a></div>
                                 <div style="float:left">
                                  <div id="UpFileText" style="margin-left:10px;" runat="server">
                                  </div>
                                 </div>     
                               </div>
                               <input runat="server" id="HidFfileName" type="hidden"  />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <a href="javascript:void(0);" class="btnBl" id="btnCancel">取消</a>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>

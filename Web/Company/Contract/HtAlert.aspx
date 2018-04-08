<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HtAlert.aspx.cs" Inherits="Distributor_newOrder_remarkview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>备注</title>
    <link href="../../Distributor/newOrder/css/global2.5.css" rel="stylesheet" />
    <style type="text/css">
            .coreInfo .box1 {
                width: 360px;
            }
            .box1, .box2 {
                border: 1px solid #ddd;
                border-radius: 5px;
                height: 34px;
                line-height: 34px;
                padding: 0px 10px;
                color: #555;
                font-size: 12px;
            }
            .box2 {
                width: 120px;
            }
        </style>
</head>
<body>
    <form id="form1" runat="server" >
        <!--备注 start-->
        <div class="popup po-remark" style="width:550px;height:100px;">
            <input type="hidden" id="txtHtDrop" runat="server" value="" />
            <input type="hidden" id="hidTrId" runat="server" value="" />
            <input type="hidden" id="hidIndex" runat="server" value="" />
             <input type="hidden" id="txtHtDropID" runat="server" value="" />
           

            <div style="width:500px;height:100px; margin: 25px 30px 0px; padding: 5px 10px;">
               
                <span style="margin-top:30px;">选择医院:</span> <asp:DropDownList ID="HtDrop" runat="server" AutoPostBack="True" 
                    style="margin-left:15px;width:380px;margin-top:30px;"
                    CssClass="box1" OnSelectedIndexChanged="HtDrop_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="po-btn" style="width:100%;">
                <a href="javascript:void(0);" class="gray-btn" id="btnCancel">取消</a> <a href="#"
                    runat="server" class="btn-area" id="btnConfirm">确定</a>
            </div>
        </div>
        <!--备注 end-->
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
        
        <script type="text/javascript">
            $(function () {
                //取消
                $(document).on("click", "#btnCancel", function () {
                    window.parent.CloseGoods();
                });
                //确定
                $("#btnConfirm").click(function () {
               
              
                var HtName = $.trim($("#txtHtDrop").val());
                var Htid = $.trim($("#txtHtDropID").val());
               
                var hidTrId = $("#hidTrId").val();
                window.parent.Htinfo(HtName, Htid, hidTrId);
               
            });
        });

       
        </script>
    </form>
</body>
</html>

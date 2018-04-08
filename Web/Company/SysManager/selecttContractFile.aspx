<%@ Page Language="C#" AutoEventWireup="true" CodeFile="selecttContractFile.aspx.cs" Inherits="Distributor_newOrder_remarkview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>查看附件</title>
    <link href="../../Distributor/newOrder/css/global2.5.css" rel="stylesheet" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>


    <script type="text/javascript">

       
        </script>


    <style type="text/css">
        .coreInfo .box1,li {
            width: 160px;
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
    <form id="form1" runat="server">
        <!--备注 start-->
        <div class="popup po-remark" style="height:150px;">
            <input type="hidden" id="txtHtDrop" runat="server" value="" />
            <input type="hidden" id="hidTrId" runat="server" value="" />
            <input type="hidden" id="hidIndex" runat="server" value="" />
            <input type="hidden" id="txtHtDropID" runat="server" value="" />


            <div class="remark-box" >

                <ul class="coreInfo">

                    <li class="lb clear" style="width:560px;">
                        <div class="con upload" style="width:560px;">
                          <div id="UpFileText" style="margin-left: 10px; width: 240px;" runat="server">
                          </div>
                        </div>
                    </li>
                </ul>
            </div>
            <div class="po-btn" style="width: 100%;">
                <a href="javascript:void(0);" class="gray-btn" id="btnCancel">取消</a> <a href="#"
                    runat="server" class="btn-area" id="btnConfirm">确定</a>
            </div>
        </div>
        <!--备注 end-->

        <script type="text/javascript">
            $(function () {
                //取消
                $(document).on("click", "#btnCancel", function () {
                    window.parent.LayerClose();
                });
                //确定
                $("#btnConfirm").click(function () {
                    window.parent.LayerClose();
               
            });
        });

       
        </script>
    </form>
</body>
</html>

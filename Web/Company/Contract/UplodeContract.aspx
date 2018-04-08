<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UplodeContract.aspx.cs" Inherits="Distributor_newOrder_remarkview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>上传合同</title>
    <link href="../../Distributor/newOrder/css/global2.5.css" rel="stylesheet" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>

    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            $("#uploadFile").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText", ResultId: "HidFfileName", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
           

        })
        //  重复上传判断
        function uploadFileClick() {
            if ($("#HidFfileName").val() != "" && $("#HidFfileName").val() != undefined) {
                layerCommon.msg("请勿重复上传！", IconOption.哭脸);
                return false;
            }
            else
                return true;
        }
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
                  <li class=" fl" style="width:310px;"><i class="name"><i class="red">*</i>生效日期</i>
                        <input name="txtForceDate" runat="server" onclick="WdatePicker()"
                            id="txtForceDate" readonly="readonly" type="text" class="box1" value="" />
                    </li>
                    <li class=" fl" style="width:310px;"><i class="name"><i class="red">*</i>失效日期</i>
                        <input name="txtInvalidDate" runat="server" onclick="WdatePicker()"
                            id="txtInvalidDate" readonly="readonly" type="text" class="box1" value="" />
                    </li>

                    <li class="lb clear" style="width:560px;">
                        <div class="con upload" style="width:560px;">
                            <div style="float: left">
                                <a href="javascript:;" class="a-upload bclor le">
                                    <input id="uploadFile" runat="server" type="file"
                                        onclick="return uploadFileClick()"
                                        name="fileAttachment" class="AddBanner" />上传合同</a>
                            </div>
                            <div style="float: left">
                                <div id="UpFileText" style="margin-left: 10px; width: 240px;" runat="server">
                                </div>
                            </div>
                        </div>
                        <asp:Panel runat="server" ID="DFile" Style="margin: 5px 5px;">
                        </asp:Panel>
                        <input runat="server" id="HidFfileName" type="hidden" />
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
                    var FileName = $.trim($("#HidFfileName").val());
                    var txtForceDate = $.trim($("#txtForceDate").val());
                    var txtInvalidDate = $.trim($("#txtInvalidDate").val());
                    if (FileName == "" || FileName == undefined)
                    {
                        layerCommon.msg("合同附件不能为空！", IconOption.哭脸);
                        return false;
                    }
                    if (txtForceDate == "" || txtForceDate == undefined) {
                        layerCommon.msg("生效日期不能为空！", IconOption.哭脸);
                        return false;
                    }
                    if (txtInvalidDate == "" || txtInvalidDate == undefined) {
                        layerCommon.msg("失效日期不能为空！", IconOption.哭脸);
                        return false;
                    }
                    window.parent.insertContract(FileName,txtForceDate,txtInvalidDate);
                    window.parent.LayerClose();
               
            });
        });

       
        </script>
    </form>
</body>
</html>

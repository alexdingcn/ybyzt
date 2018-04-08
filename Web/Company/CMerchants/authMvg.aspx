<%@ Page Language="C#" AutoEventWireup="true" CodeFile="authMvg.aspx.cs" Inherits="Company_CMerchants_authMvg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>代理商资料</title>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=201507261156" rel="stylesheet"
        type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#uploadFile").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText", ResultId: "HidFfileName", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });


            //取消
            $(document).on("click", "#btnCancel", function () {
                window.parent.layerCommon.layerClose("hid_Alert");
            });


            //确定
            $("#btnConfirm").click(function () {

                var KeyID = '<%=Request["KeyID"] %>';
                var HidFfileName = $("#HidFfileName").val();
                $.ajax({
                    type: "Post",
                    url: "authMvg.aspx/Edit",
                    data: "{ 'KeyID':'" + KeyID + "', 'HidFfileName':'" + HidFfileName + "', 'UserID':'" + <%= this.UserID %> + "'}",
                    dataType: "json",
                    timeout: 5000,
                    contentType: "application/json; charset=utf-8",
                    success: function (ReturnData) {
                        var Json = eval('(' + ReturnData.d + ')');
                        if (Json.result) {
                            window.parent.Refresh();
                            window.parent.layerCommon.layerClose("hid_Alert");
                        } else {
                            window.parent.layerCommon.msg(Json.code, window.parent.IconOption.错误);
                        }
                    }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                        var a;
                    }
                });
            });

        })

        function uploadFileClick() {
            if ($("#HidFfileName").val() != "" && $("#HidFfileName").val() != undefined) {
                window.parent.layerCommon.msg("请勿重复上传！", window.parent.IconOption.哭脸);
                return false;
            }
            else
                return true;
        }

        function Cancel(){
           $("#UpFileText").html("");
           $("#HidFfileName").val(""); 
        }
    </script>
    <style>
        .updt
        {
            text-align: right;
            line-height: 30px;    
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="popup po-invoice">
            <div class="invoice-li">
                <div class="li">
                    <div class="bt">授权书：</div>
                    <div class="con upload updt">
                        <div style="float:left">
                            <a href="javascript:;" class="a-upload bclor le"> 
                            <input id="uploadFile" runat="server" type="file" 
                                onclick="return uploadFileClick()"
                                name="fileAttachment"  class="AddBanner"/>上传附件</a></div>
                        <div style="float:left"">
                            <div id="UpFileText" style="margin-left:10px;" runat="server">
                            </div>
                        </div>     
                    </div>
                    <input runat="server" id="HidFfileName" type="hidden"  />
                </div>

                <div class="po-btn">
                    <a href="javascript:void(0);" class="gray-btn" id="btnCancel">取消</a>

                    <a href="javascript:void(0);" class="btn-area" id="btnConfirm">确定</a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

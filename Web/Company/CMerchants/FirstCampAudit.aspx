<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FirstCampAudit.aspx.cs" Inherits="Company_CMerchants_FirstCampAudit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>代理商资料</title>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=201507261156" rel="stylesheet"
        type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/classifyview.js" type="text/javascript"></script>

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
                <div class="li" runat="server" id="liDisArea">
                    <div class="bt">代理商区域：</div>
                    <input type="text" class="box1" id="txtDisArea" readonly="readonly" runat="server"/>
                    <%--<span class="arrow"></span>--%>
                    <input type="hidden" class="box1" id="hid_txtDisArea" readonly="readonly" runat="server"/>               
                     <a id="aDisArea" style="color: #1a8fc2" href="javascript:void(0);"></a>
    	            <div class="pop-menu" style="width:605px; display:none;">
    	            </div>
                    <a id="addArea" href='javascript:void(0);' style=" cursor:pointer">新增区域</a>
                </div>
                <div class="li">
                    <div class="bt">处理备注：</div>
                    <textarea id="txtRemark" runat="server" name="txtRemark" style=" height:200px;" maxlength="200" class="box"
                        placeholder="处理备注不能超过200个字"></textarea>
                </div>
            </div>
            <div class="po-btn">
                <a href="javascript:void(0);" class="gray-btn" id="btnCancel">取消</a>
                <a href="javascript:void(0);" class="btn-area" id="btnReject" style="background:#ff8106;">驳回</a>
                <a href="javascript:void(0);" class="btn-area" runat="server" id="btnConfirm">通过</a>
            </div>
        </div>
    
        <script src="../../js/layer/layer.js" type="text/javascript"></script>
        <script src="../../js/layerCommon.js" type="text/javascript"></script>
        <script>
            $(function () {
                //取消
                $(document).on("click", "#btnCancel", function () {
                    window.parent.layerCommon.layerClose("hid_Alert");
                });

                //新增区域
                $(document).on("click","#addArea",function(){
                    //window.parent.location.href='../SysManager/DisAreaList.aspx';
                    window.open('../SysManager/DisAreaList.aspx');
                    window.parent.layerCommon.layerClose("hid_Alert");
                });

                //绑定代理商区域
                $("#txtDisArea").click(function () {
                    handleChange(this, "<%=DisArea%>");
                });

                //确定
                $("#btnConfirm").click(function () {
                    var Remark = $.trim($("#txtRemark").val());
                    var DisAreaID=typeof($("#hid_txtDisArea").val())=="undefined"?"":$.trim($("#hid_txtDisArea").val()); 
                    
                    if(DisAreaID === ""){
                        layerCommon.msg("请选择代理商地区", IconOption.错误);
                        return;
                    }
                    /*if (Remark === "") {
                        layerCommon.msg("请输入处理备注", IconOption.错误);
                        return;
                    }*/
                    var KeyID = '<%=Request["KeyID"] %>';
                   
                    $.ajax({
                        type: "Post",
                        url: "FirstCampAudit.aspx/Edit",
                        data: "{ 'KeyID':'" + KeyID + "', 'Remark':'" + Remark + "', 'DisAreaID':'" + DisAreaID + "', 'CompID':'" + <%=this.CompID %> +"', 'UserID':'" + <%=this.UserID %> +  "'}",
                        dataType: "json",
                        timeout: 5000,
                        contentType: "application/json; charset=utf-8",
                        success: function (ReturnData) {
                            var Json = eval('(' + ReturnData.d + ')');
                            if (Json.result) {
                                layerCommon.msg("首营申请通过，请尽快签定合同", IconOption.笑脸,2000,function(){
                                    window.parent.Refresh();
                                    window.parent.layerCommon.layerClose("hid_Alert"); 
                                });
                            } else {
                                layerCommon.msg(Json.code, IconOption.错误);
                            }
                        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                            var a;
                        }
                    });
                });


                //驳回
                $("#btnReject").click(function () {
                    var Remark = $.trim($("#txtRemark").val());
                    /*if (Remark === "") {
                        layerCommon.msg("请输入处理备注", IconOption.错误);
                        return;
                    }*/
                    var KeyID = '<%=Request["KeyID"] %>';

                    $.ajax({
                        type: "Post",
                        url: "FirstCampAudit.aspx/RejectEdit",
                        data: "{ 'KeyID':'" + KeyID + "', 'Remark':'" + Remark + "'}",
                        dataType: "json",
                        timeout: 5000,
                        contentType: "application/json; charset=utf-8",
                        success: function (ReturnData) {
                            var Json = eval('(' + ReturnData.d + ')');
                            if (Json.result) {
                                window.parent.Refresh();
                                window.parent.layerCommon.layerClose("hid_Alert");
                            } else {
                                layerCommon.msg(Json.code, IconOption.错误);
                            }
                        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                            var a;
                        }
                    });
                });
            });
        </script>


    </form>
</body>
</html>

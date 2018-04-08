<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FirstCampEdit.aspx.cs" Inherits="Distributor_CMerchants_FirstCampEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>申请合作</title>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=201507261156" rel="stylesheet"
        type="text/css" />
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../Company/js/order.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {

            $("#uploadFile").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText1", ResultId: "HidFfileName1", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
            $("#uploadFile2").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText2", ResultId: "HidFfileName2", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
            $("#uploadFile3").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText3", ResultId: "HidFfileName3", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
            $("#uploadFile4").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText4", ResultId: "HidFfileName4", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });

            $("#btnCancel").on("click", function () {
                window.parent.layerCommon.layerClose("hid_Alert");
            });

            //提交按钮单机事件
            $("#btnSaves").click(function () {
                    if(!formCheck()){
                        return false;
                    }

                    var HidFfileName1=typeof($("#HidFfileName1").val())=="undefined"?"":$("#HidFfileName1").val();
                    var validDate1=typeof($("#txtvalidDate1").val())=="undefined"?"":$("#txtvalidDate1").val();

                    var HidFfileName2=typeof($("#HidFfileName2").val())=="undefined"?"":$("#HidFfileName2").val();
                    var validDate2=typeof($("#txtvalidDate2").val())=="undefined"?"":$("#txtvalidDate2").val();

                    var HidFfileName3=typeof($("#HidFfileName3").val())=="undefined"?"":$("#HidFfileName3").val();
                    var validDate3=typeof($("#txtvalidDate3").val())=="undefined"?"":$("#txtvalidDate3").val();

                    var HidFfileName4=typeof($("#HidFfileName4").val())=="undefined"?"":$("#HidFfileName4").val();
                    var validDate4=typeof($("#txtvalidDate4").val())=="undefined"?"":$("#txtvalidDate4").val();
                    var ApplyRemark=$.trim($("#txtApplyRemark").val());

                    $.ajax({
                        type: "Post",
                        url: "FirstCampEdit.aspx/Edit",
                        data: "{ 'KeyID':'" + <%=this.KeyID %> + "', 'UserID':'" + <%= this.UserID %> + "', 'ForceDate':'" +$("#txtForceDate").val()+ "', 'InvalidDate':'" + $("#txtInvalidDate").val() + "', 'HidFfileName1':'" + HidFfileName1 +"', 'validDate1':'" + validDate1 + "', 'HidFfileName2':'" + HidFfileName2 +"', 'validDate2':'" + validDate2 + "', 'HidFfileName3':'" + HidFfileName3 +"', 'validDate3':'" + validDate3 + "', 'HidFfileName4':'" + HidFfileName4 +"', 'validDate4':'" + validDate4 +"', 'ApplyRemark':'" + ApplyRemark + "'}",
                        dataType: "json",
                        timeout: 5000,
                        contentType: "application/json; charset=utf-8",
                        success: function (ReturnData) {
                            var Json = eval('(' + ReturnData.d + ')');
                            if (Json.result) {
                                window.parent.Close();
                            } else {
                                window.parent.layerCommon.msg(Json.code, window.parent.IconOption.错误);
                            }
                        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                            var a;
                        }
                });

             
            });

        });

        //营业执照  重复上传判断
        function uploadFileClick() {
            if ($("#HidFfileName1").val() != "" && $("#HidFfileName").val() != undefined) {
                window.parent.layerCommon.msg("请勿重复上传！", window.parent.IconOption.哭脸);
                return false;
            }
            else
                return true;
        }

        //医疗器械经营许可证  重复上传判断
        function uploadFileClick2() {
            if ($("#HidFfileName2").val() != "" && $("#HidFfileName2").val() != undefined) {
                window.parent.layerCommon.msg("请勿重复上传！", window.parent.IconOption.哭脸);
                return false;
            }
            else
                return true;
        }
        //用户许可证  重复上传判断
        function uploadFileClick3() {
            if ($("#HidFfileName3").val() != "" && $("#HidFfileName3").val() != undefined) {
                window.parent.layerCommon.msg("请勿重复上传！", window.parent.IconOption.哭脸);
                return false;
            }
            else
                return true;
        }
        //医疗器械备案  重复上传判断
        function uploadFileClick4() {
            if ($("#HidFfileName4").val() != "" && $("#HidFfileName4").val() != undefined) {
                window.parent.layerCommon.msg("请勿重复上传！", window.parent.IconOption.哭脸);
                return false;
            }
            else
                return true;
        }

        function Cancel(c) {
            $("#UpFileText"+c).html("");
            $("#HidFfileName" + c).val("");
        }

        function formCheck() {
            var str = "";

            //需要上传的资料
            var ProvideData = $.trim($("#hidProvideData").val());

            if (ProvideData.indexOf('1') > -1) {
                if ($("#HidFfileName1").val() == "" || typeof ($("#HidFfileName1").val()) == "undefined") {
                    str += "营业执照不能为空 <br/>";
                }
            } 
             if (ProvideData.indexOf('2') > -1) {
                if ($("#HidFfileName2").val() == "" || typeof ($("#HidFfileName2").val()) == "undefined") {
                    str += "医疗器械经营许可证不能为空<br/>";
                }
            } 
             if (ProvideData.indexOf('3') > -1) {
                if ($("#HidFfileName3").val() == "" || typeof ($("#HidFfileName3").val()) == "undefined") {
                    str += "开户许可证不能为空<br/>";
                }
            }
             if (ProvideData.indexOf('4') > -1) {
                if ($("#HidFfileName4").val() == "" || typeof ($("#HidFfileName4").val()) == "undefined") {
                    str += "医疗器械备案不能为空<br/>";
                }
            }

            if (str !== "") {
                window.parent.layerCommon.msg(str, window.parent.IconOption.哭脸);
                return false;
            } else {
                return true
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="orderNr">
            <input type="hidden" id="hidcompID" runat="server" />

            <input type="hidden" id="hidProvideData" runat="server" />
            <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                <tbody>
                    <tr>
                        <td class="head" style="width: 15%">医院</td>
                        <td colspan="3">
                            <input type="text" class="box" placeholder="" runat="server" maxlength="50" id="txtHtname" readonly="readonly"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="head" style="width: 15%">生效日期</td>
                        <td style="width: 37%">
                           <input type="text" class="box" placeholder="" runat="server" maxlength="50" id="txtForceDate" readonly="readonly" onclick="WdatePicker({})"/>
                        </td>
                        <td class="head" style="width: 15%">失效日期</td>
                        <td style="width: 37%">
                            <input type="text" maxlength="50" style="margin-left:2px" class="box" id="txtInvalidDate" runat="server" readonly="readonly" onclick="WdatePicker({})"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="head" style="width: 15%">申请备注</td>
                        <td colspan="3">
                            <textarea id="txtApplyRemark" runat="server" class="box1" style="margin: 0px; width: 669px; height: 124px;"></textarea>
                        </td>
                    </tr>
                    <tr id="UpFile1" runat="server" visible="false"> 
                        <td class="head" style="width: 15%"><i class="red">*</i>营业执照</td>
                        <td style="width: 37%">
                            <div class="con upload">
                                 <div style="float:left">
                                     <a href="javascript:;" class="a-upload bclor le"> 
                                     <input id="uploadFile" runat="server" type="file" 
                                         onclick="return uploadFileClick()"
                                         name="fileAttachment"  class="AddBanner"/>上传附件</a></div>
                                 <div style="float:left">
                                  <div id="UpFileText1" style="margin-left:10px;" runat="server">
                                  </div>
                                 </div>     
                               </div>
                               <input runat="server" id="HidFfileName1" type="hidden"  />
                        </td>
                        <td colspan="2">
                             <input type="text" maxlength="50" placeholder="有效期" style="margin-left:2px" class="box" id="txtvalidDate1" runat="server" readonly="readonly" onclick="WdatePicker({})"/>
                        </td>
                    </tr>
                    <tr id="UpFile2" runat="server" visible="false">
                        <td class="head" style="width: 15%"><i class="red">*</i>医疗器械经营许可证</td>
                        <td style="width: 37%">
                            <div class="con upload">
                                 <div style="float:left">
                                     <a href="javascript:;" class="a-upload bclor le"> 
                                     <input id="uploadFile2" runat="server" type="file" 
                                         onclick="return uploadFileClick2()"
                                         name="fileAttachment"  class="AddBanner"/>上传附件</a></div>
                                 <div style="float:left">
                                  <div id="UpFileText2" style="margin-left:10px;" runat="server">
                                  </div>
                                 </div>     
                               </div>
                               <input runat="server" id="HidFfileName2" type="hidden"  />
                        </td>
                        <td colspan="2">
                             <input type="text" maxlength="50" placeholder="有效期" style="margin-left:2px" class="box" id="txtvalidDate2" runat="server" readonly="readonly" onclick="WdatePicker({})"/>
                        </td>
                    </tr>
                    <tr id="UpFile3" runat="server" visible="false">
                        <td class="head" style="width: 15%"><i class="red">*</i>开户许可证</td>
                        <td style="width: 37%">
                            <div class="con upload">
                                <div style="float:left">
                                    <a href="javascript:;" class="a-upload bclor le"> 
                                    <input id="uploadFile3" runat="server" type="file" 
                                        onclick="return uploadFileClick3()"
                                        name="fileAttachment"  class="AddBanner"/>上传附件</a></div>
                                <div style="float:left">
                                <div id="UpFileText3" style="margin-left:10px;" runat="server">
                                </div>
                                </div>     
                            </div>
                            <input runat="server" id="HidFfileName3" type="hidden"  />
                        </td>
                        <td colspan="2">
                             <input type="text" maxlength="50" placeholder="有效期" style="margin-left:2px" class="box" id="txtvalidDate3" runat="server" readonly="readonly" onclick="WdatePicker({})"/>
                        </td>
                    </tr>
                    <tr id="UpFile4" runat="server" visible="false">
                        <td class="head" style="width: 13%"><i class="red">*</i>医疗器械备案</td>
                        <td style="width: 37%">
                            <div class="con upload">
                                <div style="float:left">
                                    <a href="javascript:;" class="a-upload bclor le"> 
                                    <input id="uploadFile4" runat="server" type="file" 
                                        onclick="return uploadFileClick4()"
                                        name="fileAttachment"  class="AddBanner"/>上传附件</a></div>
                                <div style="float:left">
                                <div id="UpFileText4" style="margin-left:10px;" runat="server">
                                </div>
                                </div>     
                            </div>
                            <input runat="server" id="HidFfileName4" type="hidden"  />
                        </td>
                        <td colspan="2">
                             <input type="text" maxlength="50" placeholder="有效期" style="margin-left:2px" class="box" id="txtvalidDate4" runat="server" readonly="readonly" onclick="WdatePicker({})"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <a href="javascript:void(0);" class="btnOr" id="btnSaves">确定</a>
                            <%--<asp:Button ID="btnSave" CssClass="" runat="server"  OnClientClick="return formCheck()" OnClick="btnSave_Click" />--%>
                            <a href="javascript:void(0);" class="btnBl" id="btnCancel">取消</a>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FCmaterials.aspx.cs" Inherits="Company_SysManager_FCMaterialEdit" %>

<%@ Register Src="~/Company/UserControl/TreeDisArea.ascx" TagPrefix="uc1" TagName="TreeDisArea" %>
<%@ Register Src="~/Company/UserControl/TreeDisType.ascx" TagPrefix="uc1" TagName="TreeDisType" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/CompanyTop.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/Company/UserControl/CompanyLeft.ascx" TagPrefix="uc1" TagName="left" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>我的首营资料</title>

    <link href="/assets/css/bootstrap.min.css" rel="Stylesheet" type="text/css" />
    <link href="/assets/font-awesome/4.5.0/css/font-awesome.min.css" rel="Stylesheet" type="text/css" />
    <link href="/assets/css/iconfont.css"  rel="stylesheet" />
    <link href="/assets/css/ace.min.css" rel="Stylesheet" type="text/css" />
    <link href="/assets/css/ace-skins.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="/assets/css/bootstrap-datepicker3.min.css" />

    <script src="/assets/js/jquery-2.1.4.min.js" type="text/javascript"></script>
    <script src="/assets/js/bootstrap.min.js" type="text/javascript" ></script>
    <script src="/assets/js/bootbox.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="/assets/js/bootstrap-datepicker.min.js" type="text/javascript" ></script>
    <script src="/assets/js/ace-elements.min.js" type="text/javascript" ></script>
    <script src="/assets/js/ace.min.js" type="text/javascript" ></script>
    <!--[if lte IE 8]>
		<script src="/assets/js/html5shiv.min.js"></script>
		<script src="/assets/js/respond.min.js"></script>
	<![endif]-->


    <script type="text/javascript"> 

        $(document).ready(function () {

            $('.file-input-ctrl').each(function() {
                var resultId = $(this).data("result-id");

                $(this).ace_file_input({
                    no_file: '请选择文件...',
                    btn_choose: '选择文件',
                    btn_change: '更换文件',
                    droppable: false,
                    onchange: null,
                    thumbnail: true,
                    maxSize: 5242880,
                    // whitelist: 'gif|png|jpg|jpeg',
                    // allowExt: ['gif', 'png', 'jpg', 'jpeg', "bmp", "pdf"],
                    // allowMime: ["image/jpg", "image/jpeg", "image/png", "image/gif", "image/bmp", "pdf"],
                    before_remove: function () {
                        var resultId = $(this).data("result-id");
                        $("#" + resultId).val('');
                        $(this).parents(".form-group").find(".date-picker").val("");
                        return true;
                    },
                    before_change: function (files, dropped) {
                        var fd = new FormData();
                        fd.append("file", files[0]);
                        var resultId = $(this).data("result-id");
                        var deferred = $.ajax({
                            url: '../../Controller/Fileup.ashx?UploadFiles=UploadFile/',
                            type: "post",
                            processData: false,
                            contentType: false,
                            data: fd
                        });
                        deferred.done(function (result) {
                            if (result) {
                                var value = result;
                                if (value.indexOf("@returnstart@") === 0) {
                                    value = value.substring(13, result.indexOf("@returnend@"));
                                }
                                var obj = JSON.parse(value);
                                if (obj.result) {
                                    $("#" + resultId).val(obj.name);
                                }
                            }
                        }).fail(function (result) {
                            console.log(result);
                        });
                        return true;
                    }
                });

                var existingVal = $("#" + resultId).val();
                if (existingVal !== '') {
                    $(this).ace_file_input('show_file_list', [{ type: 'image', name: existingVal }]);
                }
            });

            $('.date-picker').datepicker({
                autoclose: true,
                todayHighlight: true,
                language: 'cn'
            })
            //提交按钮单击事件
            $("#btnAdds").click(function () {
                $("#btnAdd").trigger("click")
            })
        });

        //选择银行卡返回
        function CloseBank() {
            // layerCommon.layerClose();
            layerCommon.layerCloseAll();
        }

        //选择银行卡返回
        function SelectBankReturn(materialCodes, bankName) {
            if (materialCodes != "") {
                form1.txtbankcodes.value = materialCodes;
                form1.txtbandname.value = bankName;
                //document.getElementById("btnSelectBank").click();
                var objselect = document.getElementById("ddlbank");
                jsAddItemToSelect(objselect, materialCodes, bankName);
                CloseBank();
            }
        }
  
        //选择银行
        function Otypechange(ower) {
            if (ower == "20000") {
                $("#ddlbank").val("-1");
                var dealerId = 1;
                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度
                var index = layerCommon.openWindow('选择银行', '../Pay/PaySelectBank.aspx', '1100px', '500px'); //记录弹出对象
                $("#hid_alerts").val(index); //记录弹出对象
            }
        }

        // 1.判断select选项中 是否存在Value="paraValue"的Item 
        function jsSelectIsExitItem(objSelect, objItemValue) {
            var isExit = false;
            for (var i = 0; i < objSelect.options.length; i++) {
                if (objSelect.options[i].value == objItemValue) {
                    isExit = true;
                    break;
                }
            }
            return isExit;
        }
        // 2.向select选项中 加入一个Item 
        function jsAddItemToSelect(objSelect, objItemText, objItemValue) {
            //判断是否存在 
            if (jsSelectIsExitItem(objSelect, objItemValue)) {
                alert("该Item的Value值已经存在");
            } else {
                var varItem = new Option(objItemValue, objItemText);
                objSelect.options.add(varItem);
                //$(objSelect).find("option:first").insertBefore(str);
                $("#ddlbank").find("option[value='" + objItemText + "']").attr("selected", true);
                //  alert("成功加入");
            }
        }

        //确认提交  非空验证
        function formCheck() {
            if ($("#HidFfileName").val() == "") {
                bootbox.alert({
                    size: 'small',
                    message: "请选择营业执照",
                    buttons: {
                        ok: {
                            label: "确认",
                            className: "btn-primary btn-sm",
                        },
                    },
                });
                return false;
            }
            if ($("#HidFfileName").val() != "" && $("#HidFfileName").val() != undefined) {
                if ($("#validDate").val() == "" || $("#validDate").val() == undefined) {
                    bootbox.alert({
                        size: 'small',
                        message: "请选择营业执照有效期",
                        buttons: {
                            ok: {
                                label: "确认",
                                className: "btn-primary btn-sm",
                            },
                        },
                    });
                    return false;
                }
            }
            if ($("#HidFfileName2").val() == "") {
                bootbox.alert({
                    size: 'small',
                    message: "请选择生产许可证",
                    buttons: {
                        ok: {
                            label: "确认",
                            className: "btn-primary btn-sm",
                        },
                    },
                });
                return false;
            }
             if ($("#HidFfileName2").val() != "" && $("#HidFfileName2").val() != undefined) {
                 if ($("#validDate2").val() == "" || $("#validDate2").val() == undefined) {
                     bootbox.alert({
                         size: 'small',
                         message: "请选择生产许可证有效期",
                         buttons: {
                             ok: {
                                 label: "确认",
                                 className: "btn-primary btn-sm",
                             },
                         },
                     });
                    return false;
                }
            }

        }

    </script>
</head>
<body class="skin-3">


    <uc1:top ID="top1" runat="server" ShowID="nav-2" />

<div class="main-container" id="main-container">
    <uc1:left ID="left1" runat="server"  />

    <div class="main-content">
        <div class="main-content-inner">
            <div class="breadcrumbs">
                <ul class="breadcrumb">
                    <li>
					    <i class="icon iconfont icon-shouye"></i>
					    <a href="../jsc.aspx">我的桌面</a>
				    </li>
                    <li>
                        <a href="../SysManager/FCmaterialsInfo.aspx">我的首营资料</a>
                    </li>
                    <li class="active">我的首营资料编辑</li>
                </ul>
            </div>

            <uc1:CompRemove runat="server" ID="Remove" ModuleID="2" />

            <div class="page-content">
                <form id="form1" runat="server" class="form-horizontal" role="form">
                    <div class="page-header">
					    <h1>
						    基本资料
                            <small>
						    <i class="ace-icon fa fa-angle-double-right"></i>
							    首营资料和证件管理
						    </small>

                            <span class="pull-right">
                                <a href="javascript:;" class="btn btn-sm btn-success" id="btnAdds">提交</a>
                                <a href="javascript:;" class="btn btn-link" onclick="javascript:history.go(-1);">取消</a>
                    
                                <asp:Button ID="btnAdd" CssClass="hide" runat="server"  OnClientClick="return formCheck()"  OnClick="btnAdd_Click" />
                            </span>
					    </h1>
				    </div>

                    <div class="row">
                        <div class="col-xs-12">
                                <input type="hidden"  id="hid_Alert"/> 
                                <input id="hid_Alerts" type="hidden" />
                                <input id="txtbankcodes" type="hidden" name="txtbankcodes" runat="server" />
                                <input id="txtbandname" type="hidden" name="txtbandname" runat="server" />

                                <div class="form-group">
								    <label class="col-sm-2 control-label " for="form-field-1"> <i class="red">*</i> 营业执照 </label>

								    <div class="col-sm-5">
                                        <input type="file" class="file-input-ctrl" data-result-id="HidFfileName"/>
                                    </div>
                                    <label class="col-sm-1 control-label " >有效期 </label>
                                    <div class="col-sm-3">
                                        <div class="input-group">
										    <input class="form-control date-picker" id="validDate" name="validDate" runat="server" type="text" data-date-format="yyyy/mm/dd" />
										    <span class="input-group-addon">
											    <i class="fa fa-calendar bigger-110"></i>
										    </span>
									    </div>
								    </div>
							    </div>

                                <div class="form-group">
								    <label class="col-sm-2 control-label " for="form-field-1"> <i class="red">*</i> 生产/许可证 </label>

								    <div class="col-sm-5">
								        <label class="ace-file-input">
                                             <input type="file" class="file-input-ctrl" data-result-id="HidFfileName2"/>
                                         </label>
                                    </div>
                                    <label class="col-sm-1 control-label " >有效期 </label>
                                    <div class="col-sm-3">
                                        <div class="input-group">
										    <input class="form-control date-picker" id="validDate2" name="validDate2" runat="server" type="text" data-date-format="yyyy/mm/dd" />
										    <span class="input-group-addon">
											    <i class="fa fa-calendar bigger-110"></i>
										    </span>
									    </div>
								    </div>
							    </div>

                                <div class="form-group">
								    <label class="col-sm-2 control-label " for="form-field-1">税务登记证</label>

								    <div class="col-sm-5">
								        <input type="file" class="file-input-ctrl" data-result-id="HidFfileName3"/>
                                    </div>
                                    <label class="col-sm-1 control-label " >有效期 </label>
                                    <div class="col-sm-3">
                                        <div class="input-group">
										    <input class="form-control date-picker" id="validDate3" name="validDate3" runat="server" type="text" data-date-format="yyyy/mm/dd" />
										    <span class="input-group-addon">
											    <i class="fa fa-calendar bigger-110"></i>
										    </span>
									    </div>
								    </div>
							    </div>


                                <div class="form-group">
								    <label class="col-sm-2 control-label " for="form-field-1">开户银行许可证</label>

								    <div class="col-sm-5">
								        <input type="file" class="file-input-ctrl" data-result-id="HidFfileName4"/>
                                    </div>
                                    <label class="col-sm-1 control-label " >有效期 </label>
                                    <div class="col-sm-3">
                                        <div class="input-group">
										    <input class="form-control date-picker" id="validDate4" name="validDate4" runat="server" type="text" data-date-format="yyyy/mm/dd" />
										    <span class="input-group-addon">
											    <i class="fa fa-calendar bigger-110"></i>
										    </span>
									    </div>
								    </div>
							    </div>


                                <div class="form-group">
								    <label class="col-sm-2 control-label " for="form-field-1">质量管理体系调查表</label>

								    <div class="col-sm-5">
								        <input type="file" class="file-input-ctrl" data-result-id="HidFfileName5"/>
                                    </div>
                                    <label class="col-sm-1 control-label " >有效期 </label>
                                    <div class="col-sm-3">
                                        <div class="input-group">
										    <input class="form-control date-picker" id="validDate5" name="validDate5" runat="server" type="text" data-date-format="yyyy/mm/dd" />
										    <span class="input-group-addon">
											    <i class="fa fa-calendar bigger-110"></i>
										    </span>
									    </div>
								    </div>
							    </div>


                                <div class="form-group">
								    <label class="col-sm-2 control-label " for="form-field-1">GSP/GMP证书</label>

								    <div class="col-sm-5">
								        <input type="file" class="file-input-ctrl" data-result-id="HidFfileName6"/>
                                    </div>
                                    <label class="col-sm-1 control-label " >有效期 </label>
                                    <div class="col-sm-3">
                                        <div class="input-group">
										    <input class="form-control date-picker" id="validDate6" name="validDate6" runat="server" type="text" data-date-format="yyyy/mm/dd" />
										    <span class="input-group-addon">
											    <i class="fa fa-calendar bigger-110"></i>
										    </span>
									    </div>
								    </div>
							    </div>


                                <div class="form-group">
								    <label class="col-sm-2 control-label " for="form-field-1">开票信息 </label>

								    <div class="col-sm-5">
								        <input type="file" class="file-input-ctrl" data-result-id="HidFfileName7"/>
                                    </div>
                                    <label class="col-sm-1 control-label " >有效期 </label>
                                    <div class="col-sm-3">
                                        <div class="input-group">
										    <input class="form-control date-picker" id="validDate7" name="validDate7" runat="server" type="text" data-date-format="yyyy/mm/dd" />
										    <span class="input-group-addon">
											    <i class="fa fa-calendar bigger-110"></i>
										    </span>
									    </div>
								    </div>
							    </div>

                                <div class="form-group">
								    <label class="col-sm-2 control-label " for="form-field-1">企业年报</label>

								    <div class="col-sm-5">
								       <input type="file" class="file-input-ctrl" data-result-id="HidFfileName8"/>
                                    </div>
                                    <label class="col-sm-1 control-label " >有效期 </label>
                                    <div class="col-sm-3">
                                        <div class="input-group">
										    <input class="form-control date-picker" id="validDate8" name="validDate8" runat="server" type="text" data-date-format="yyyy/mm/dd" />
										    <span class="input-group-addon">
											    <i class="fa fa-calendar bigger-110"></i>
										    </span>
									    </div>
								    </div>
							    </div>


                                <div class="form-group">
								    <label class="col-sm-2 control-label " for="form-field-1">银行收付款帐号资料</label>

								    <div class="col-sm-5">
								        <input type="file" class="file-input-ctrl" data-result-id="HidFfileName9"/>
                                    </div>
                                    <label class="col-sm-1 control-label " >有效期 </label>
                                    <div class="col-sm-3">
                                        <div class="input-group">
										    <input class="form-control date-picker" id="validDate9" name="validDate9" runat="server" type="text" data-date-format="yyyy/mm/dd" />
										    <span class="input-group-addon">
											    <i class="fa fa-calendar bigger-110"></i>
										    </span>
									    </div>
								    </div>
							    </div>


                                <div class="form-group">
								    <label class="col-sm-2 control-label " for="form-field-1">购销合同</label>

								    <div class="col-sm-5">
								        <input type="file" class="file-input-ctrl" data-result-id="HidFfileName10"/>
                                    </div>
                                    <label class="col-sm-1 control-label " >有效期 </label>
                                    <div class="col-sm-3">
                                        <div class="input-group">
										    <input class="form-control date-picker" id="validDate10" name="validDate10" runat="server" type="text" data-date-format="yyyy/mm/dd" />
										    <span class="input-group-addon">
											    <i class="fa fa-calendar bigger-110"></i>
										    </span>
									    </div>
								    </div>
							    </div>
                        </div>
                    </div>
                    
                    <input runat="server" id="HidFfileName" type="hidden" />
                    <input runat="server" id="HidFfileName2" type="hidden" />
                    <input runat="server" id="HidFfileName3" type="hidden" />
                    <input runat="server" id="HidFfileName4" type="hidden" />
                    <input runat="server" id="HidFfileName5" type="hidden" />
                    <input runat="server" id="HidFfileName6" type="hidden" />
                    <input runat="server" id="HidFfileName7" type="hidden" />
                    <input runat="server" id="HidFfileName8" type="hidden" />
                    <input runat="server" id="HidFfileName9" type="hidden" />
                    <input runat="server" id="HidFfileName10" type="hidden" />

                    <h4 class="header green">开票信息</h4>
                    <div class="row">
                        <div class="col-xs-12">
                                                <div class="form-group">
						    <label class="col-sm-2 control-label " for="txtRise"> 发票抬头 </label>

						    <div class="col-sm-4">
							    <input type="text" placeholder="发票抬头" class="col-xs-10 col-sm-10" runat="server" maxlength="50" id="txtRise"/>
						    </div>
 
						    <label class="col-sm-2 control-label " for="txtContent"> 发票内容 </label>

						    <div class="col-sm-4">
							    <input type="text" placeholder="发票内容" class="col-xs-10 col-sm-10" runat="server" maxlength="50" id="txtContent"/>
						    </div>
					    </div>

                        <div class="form-group">
						    <label class="col-sm-2 control-label " for="txtContent"> 开户银行 </label>

						    <div class="col-sm-4">
                                <select id="txtOBank" runat="server" onchange="Otypechange(this.options[this.selectedIndex].value)"
                                    class="col-sm-10">
                                    <option value="-1">请选择</option>
                                    <option value="1405">广州农村商业银行</option>
                                    <option value="102">中国工商银行</option>
                                    <option value="103">中国农业银行</option>
                                    <option value="104">中国银行</option>
                                    <option value="105">中国建设银行</option>
                                    <option value="301">交通银行</option>
                                    <option value="100">邮储银行</option>
                                    <option value="303">光大银行</option>
                                    <option value="305">民生银行</option>
                                    <option value="306">广发银行</option>
                                    <option value="302">中信银行</option>
                                    <option value="310">浦发银行</option>
                                    <option value="309">兴业银行</option>
                                    <option value="401">上海银行</option>
                                    <option value="403">北京银行</option>
                                    <option value="307">平安银行</option>
                                    <option value="308">招商银行</option>
                                    <option value="20000" style="color: Blue;">更多银行</option>
                                </select>
						    </div>

						    <label class="col-sm-2 control-label " for="txtContent"> 开户账户 </label>

						    <div class="col-sm-4">
							    <input type="text" placeholder="开户账户" class="col-xs-10 col-sm-10" runat="server" maxlength="50" id="txtOAccount"/>
						    </div>
					    </div>
    
                        <div class="form-group">
						    <label class="col-sm-2 control-label " for="txtContent"> 纳税人登记号 </label>

						    <div class="col-sm-4">
							    <input type="text" placeholder="纳税人登记号" class="col-xs-10 col-sm-10" runat="server" maxlength="50" id="txtTRNumber"/>
						    </div>
					    </div>
                        </div>

                    </div>

            
                </form>



            </div>
        </div>
    </div>

</div>


</body>
</html>

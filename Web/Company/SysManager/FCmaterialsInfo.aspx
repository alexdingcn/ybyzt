<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FCmaterialsInfo.aspx.cs" Inherits="Company_SysManager_DisEdit" %>

<%@ Register Src="~/Company/UserControl/TreeDisArea.ascx" TagPrefix="uc1" TagName="TreeDisArea" %>
<%@ Register Src="~/Company/UserControl/TreeDisType.ascx" TagPrefix="uc1" TagName="TreeDisType" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/CompanyTop.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/Company/UserControl/CompanyLeft.ascx" TagPrefix="uc1" TagName="left" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head lang="zh-cn">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>我的首营资料</title>

    <!--#include file="common_js_css.inc"-->
    <script type="text/javascript">
        $(document).ready(function () {
            $("#form-groupachment-form .form-group").each(function () {
                var obj = $(this).find(".form-groupachment");
                var imgFile = obj.text();
                if ($(this).find(".form-groupachment").text() == '') {
                    $(this).hide();
                }
                obj.html("<a class='imagelink' href=\"<%= Common.GetWebConfigKey("OssImgPath") %>UploadFile/" + imgFile + "\">点击查看</a>");
            });

            $(".imagelink").on('click', function (event) {
                var link = $(this).attr('href');
                if (link && (link.match(/\.(jpeg|jpg|gif|png|bmp|tif|tiff)$/) != null)) {
                    event.preventDefault();
                    $('.imagepreview').attr('src', link);
                    $('#imagemodal').modal('show');
                }
            })
        });
    </script>
</head>
<body class="skin-3">

<uc1:top ID="top2" runat="server" ShowID="nav-2" />

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
                    <li>我的首营资料</li>
                </ul>
            </div>

            <div class="page-content">
                <div class="page-header">
					<h1>
						基本资料
                        <small>
						<i class="ace-icon fa fa-angle-double-right"></i>
							首营资料和证件管理
						</small>

                        <span class="pull-right">
                            <a href="FCMaterials.aspx?id=<%= fid%>" class="btn btn-sm btn-success">修改</a>
                        </span>
					</h1>
				</div>

                <form runat="server">
                    <div class="row" id="form-groupachment-form">
                        <div class="col-xs-12 form-group">
						    <label class="col-sm-2 control-label " for="form-field-1">营业执照</label>
						    <div class="col-sm-2">
                                <span class="form-groupachment" id="HidFfileName" runat="server"/>
                            </div>
                            <label class="col-sm-1 control-label ">有效期 </label>
                            <div class="col-sm-3">
                                <div class="input-group">
								    <input class="form-control date-picker" id="validDate" name="validDate" runat="server" disabled="disabled" type="text" data-date-format="yyyy/mm/dd" />
								    <span class="input-group-addon">
									    <i class="fa fa-calendar bigger-110"></i>
								    </span>
							    </div>
						    </div>
                        </div>
                        <div class="col-xs-12 form-group">
						    <label class="col-sm-2 control-label " for="form-field-1">生产/许可证</label>
						    <div class="col-sm-2">
                                <span class="form-groupachment" id="HidFfileName2" runat="server"/>
                            </div>
                            <label class="col-sm-1 control-label ">有效期 </label>
                            <div class="col-sm-3">
                                <div class="input-group">
								    <input class="form-control date-picker" id="validDate2" name="validDate2" runat="server" disabled="disabled" type="text" data-date-format="yyyy/mm/dd" />
								    <span class="input-group-addon">
									    <i class="fa fa-calendar bigger-110"></i>
								    </span>
							    </div>
						    </div>
                        </div>
                        <div class="col-xs-12 form-group">
						    <label class="col-sm-2 control-label " for="form-field-1">税务登记证</label>
						    <div class="col-sm-2">
                                <span class="form-groupachment" id="HidFfileName3" runat="server"/>
                            </div>
                            <label class="col-sm-1 control-label ">有效期 </label>
                            <div class="col-sm-3">
                                <div class="input-group">
								    <input class="form-control date-picker" id="validDate3" name="validDate3" runat="server" disabled="disabled" type="text" data-date-format="yyyy/mm/dd" />
								    <span class="input-group-addon">
									    <i class="fa fa-calendar bigger-110"></i>
								    </span>
							    </div>
						    </div>
                        </div>
                        <div class="col-xs-12 form-group">
						    <label class="col-sm-2 control-label " for="form-field-1">开户银行许可证</label>
						    <div class="col-sm-2">
                                <span class="form-groupachment" id="HidFfileName4" runat="server"/>
                            </div>
                            <label class="col-sm-1 control-label ">有效期 </label>
                            <div class="col-sm-3">
                                <div class="input-group">
								    <input class="form-control date-picker" id="validDate4" name="validDate4" runat="server" disabled="disabled" type="text" data-date-format="yyyy/mm/dd" />
								    <span class="input-group-addon">
									    <i class="fa fa-calendar bigger-110"></i>
								    </span>
							    </div>
						    </div>
                        </div>
                        <div class="col-xs-12 form-group">
						    <label class="col-sm-2 control-label " for="form-field-1">质量管理体系调查表</label>
						    <div class="col-sm-2">
                                <span class="form-groupachment" id="HidFfileName5" runat="server"/>
                            </div>
                            <label class="col-sm-1 control-label ">有效期 </label>
                            <div class="col-sm-3">
                                <div class="input-group">
								    <input class="form-control date-picker" id="validDate5" name="validDate5" runat="server" disabled="disabled" type="text" data-date-format="yyyy/mm/dd" />
								    <span class="input-group-addon">
									    <i class="fa fa-calendar bigger-110"></i>
								    </span>
							    </div>
						    </div>
                        </div>
                        <div class="col-xs-12 form-group">
						    <label class="col-sm-2 control-label " for="form-field-1">GSP/GMP证书</label>
						    <div class="col-sm-2">
                                <span class="form-groupachment" id="HidFfileName6" runat="server"/>
                            </div>
                            <label class="col-sm-1 control-label ">有效期 </label>
                            <div class="col-sm-3">
                                <div class="input-group">
								    <input class="form-control date-picker" id="validDate6" name="validDate6" runat="server" disabled="disabled" type="text" data-date-format="yyyy/mm/dd" />
								    <span class="input-group-addon">
									    <i class="fa fa-calendar bigger-110"></i>
								    </span>
							    </div>
						    </div>
                        </div>
                        <div class="col-xs-12 form-group">
						    <label class="col-sm-2 control-label " for="form-field-1">开票信息</label>
						    <div class="col-sm-2">
                                <span class="form-groupachment" id="HidFfileName7" runat="server"/>
                            </div>
                            <label class="col-sm-1 control-label ">有效期</label>
                            <div class="col-sm-3">
                                <div class="input-group">
								    <input class="form-control date-picker" id="validDate7" name="validDate7" runat="server" disabled="disabled" type="text" data-date-format="yyyy/mm/dd" />
								    <span class="input-group-addon">
									    <i class="fa fa-calendar bigger-110"></i>
								    </span>
							    </div>
						    </div>
                        </div>
                        <div class="col-xs-12 form-group">
						    <label class="col-sm-2 control-label " for="form-field-1">企业年报</label>
						    <div class="col-sm-2">
                                <span class="form-groupachment" id="HidFfileName8" runat="server"/>
                            </div>
                            <label class="col-sm-1 control-label ">有效期</label>
                            <div class="col-sm-3">
                                <div class="input-group">
								    <input class="form-control date-picker" id="validDate8" name="validDate8" runat="server" disabled="disabled" type="text" data-date-format="yyyy/mm/dd" />
								    <span class="input-group-addon">
									    <i class="fa fa-calendar bigger-110"></i>
								    </span>
							    </div>
						    </div>
                        </div>
                        <div class="col-xs-12 form-group">
						    <label class="col-sm-2 control-label " for="form-field-1">银行收付款帐号资料</label>
						    <div class="col-sm-2">
                                <span class="form-groupachment" id="HidFfileName9" runat="server"/>
                            </div>
                            <label class="col-sm-1 control-label ">有效期</label>
                            <div class="col-sm-3">
                                <div class="input-group">
								    <input class="form-control date-picker" id="validDate9" name="validDate9" runat="server" disabled="disabled" type="text" data-date-format="yyyy/mm/dd" />
								    <span class="input-group-addon">
									    <i class="fa fa-calendar bigger-110"></i>
								    </span>
							    </div>
						    </div>
                        </div>
                        <div class="col-xs-12 form-group">
						    <label class="col-sm-2 control-label " for="form-field-1">购销合同</label>
						    <div class="col-sm-2">
                                <span class="form-groupachment" id="HidFfileName10" runat="server"/>
                            </div>
                            <label class="col-sm-1 control-label ">有效期</label>
                            <div class="col-sm-3">
                                <div class="input-group">
								    <input class="form-control date-picker" id="validDate10" name="validDate10" runat="server" disabled="disabled" type="text" data-date-format="yyyy/mm/dd" />
								    <span class="input-group-addon">
									    <i class="fa fa-calendar bigger-110"></i>
								    </span>
							    </div>
						    </div>
                        </div>
                    </div>

                    <h4 class="header green">开票信息</h4>
                    <div class="row">
                        <div class="col-xs-12 form-group">
                            <label class="col-sm-2 control-label " for="form-field-1">发票抬头</label>
						    <div class="col-sm-4">
                                <input type="text" runat="server" disabled="disabled" id="txtRise"/>
                            </div>
                            <label class="col-sm-2 control-label " for="form-field-1">发票内容</label>
						    <div class="col-sm-4">
                                <input type="text" runat="server" disabled="disabled" id="txtContent"/>
                            </div>
                        </div>
                        <div class="col-xs-12 form-group">
                            <label class="col-sm-2 control-label " for="form-field-1">开户银行</label>
						    <div class="col-sm-4">
                            <select id="txtOBank" runat="server" disabled="disabled">
                                <option value=""></option>
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
                            <label class="col-sm-2 control-label " for="form-field-1">开户账户</label>
						    <div class="col-sm-4">
                                <input type="text" runat="server" disabled="disabled" id="txtOAccount"/>
                            </div>
                        </div>
                        <div class="col-xs-12 form-group">
                            <label class="col-sm-2 control-label " for="form-field-1">纳税人登记号</label>
						    <div class="col-sm-4">
                                <input type="text" runat="server" disabled="disabled" id="txtTRNumber"/>
                            </div>
                        </div>
                    </div>

                </form>
            </div>
        </div>
    </div>

    <div class="modal fade" id="imagemodal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">              
          <div class="modal-body">
      	    <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
            <img src="" class="imagepreview" style="width: 100%;" >
          </div>
        </div>
      </div>
    </div>
</div>


</body>
</html>

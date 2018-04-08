<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShopManager.aspx.cs"  EnableViewState="false" Inherits="Company_ShopManager_ShopManager" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>店铺装修</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="baidu-site-verification" content="IdU3LryeUL" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/Company/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/Company/css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="/eshop/css/global.css" rel="stylesheet" type="text/css" />
    <script src="/Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="/js/layer/layer.js" type="text/javascript"></script>
    <script src="/js/layerCommon.js" type="text/javascript"></script>
    <script type="text/javascript" src="/js/superslide.2.1.js"></script>
    <script src="/js/MoveLayer.js" type="text/javascript"></script>
    <script src="/js/ajaxfileupload.js" type="text/javascript"></script>
    <script src="/eshop/js/json2.js" type="text/javascript"></script>
    <script  type="text/javascript">
           (function ($) {
               $.extend({ BindEditShopManager: function (EditSelctor, ShowOption, MaskSelector) {
                   var EditShopVoid = {
                       //ajax获取/提交数据 start
                       AjaxForm: function (ShowControl, Url, Data, LoadingC, AjaxRequesType, CallBack, IsShowMsg) {
                           if (IsShowMsg == undefined) {
                               IsShowMsg = true;
                           }
                           Data.CompKey = $.trim($("#Hid_UserCompKey").val());
                           $.ajax({
                               type: 'POST',
                               url: Url,
                               data: Data,
                               dataType: "json",
                               timeout: 5000,
                               cache: false,
                               beforeSend: function () {
                                   AjaxRequesType == RequesType.Query && function () { ShowControl.find(".ShowSucessBtn").remove(); ShowControl.find(LoadingC).html("<div class=\"EditLoding\" style=\" width:100%; height:100px;line-height:100px;font-size:16px; text-align:center;\">数据正在加载中...</div>"); EditParam.EditDivCenter(ShowControl); return true; } () || AjaxRequesType == RequesType.Submit && function () {
                                       ShowControl.data("IsSucess", false).addClass("ebled").text("正在提交请稍候..."); return true;
                                   } ();
                               },
                               success: function (ReturnData) {
                                   if (ReturnData.Result) {
                                       AjaxRequesType == RequesType.Action && function () { CallBack(ReturnData); return true } () || setTimeout(function () { AjaxRequesType == RequesType.Query && $(".EditLoding").remove(); CallBack(ReturnData); }, 500);
                                   } else {
                                       AjaxRequesType == RequesType.Query && function () { $(".EditLoding").html(ReturnData.Msg); return true; } () || AjaxRequesType == RequesType.Submit && function () {
                                           ShowControl.data("IsSucess", true); layerCommon.msg(ReturnData.Msg, IconOption.错误, 2000), ShowControl.removeClass("ebled").text("确认装修"); return true;
                                       } () || function () { IsShowMsg && layerCommon.msg(ReturnData.Msg, IconOption.错误, 2000); } ();
                                   }
                               }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                                   AjaxRequesType == RequesType.Query && function () {
                                       $(".EditLoding").text("数据加载失败，服务器或网络异常"); return true;
                                   } () || AjaxRequesType == RequesType.Submit && function () {
                                       ShowControl.data("IsSucess", true); layerCommon.msg("数据提交失败，服务器或网络异常请重试。", IconOption.错误, 2000), ShowControl.removeClass("ebled").text("确认装修"); return true;
                                   } () || function () { IsShowMsg && layerCommon.msg("请求失败，服务器或网络异常请重试。", IconOption.错误, 2000); } ();
                               }
                           });
                       },
                       //ajax获取/提交数据 end
                       //绑定事件 start
                       BindEditShopEvent: function () {
                           $(".edit-title .close").on("click", function () {
                               $(this).parent().parent().fadeOut(500);
                               $(MaskSelector).fadeOut(500);
                           });

                           $(EditSelctor).on("click", function () {
                               var $id = this.id;
                               $(ShowOption[$id]).fadeIn();
                               EditShopVoid.BindEditData(ShowOption[$id]);
                               $(MaskSelector).fadeIn(500);
                           });
                       },
                       BindBannerEvent: function () {
                           $("input:file", Showoption.btnBanner).unbind().on("change", function (e, data) {
                               var btnFile = this, str = $(btnFile).val(), Suffix = $.trim(str.substring(str.lastIndexOf("."))), $Li = $(btnFile).closest("li"), BanerLength = $(e.delegateTarget).closest(".ShowSucessBtn").prev().find("li").length;
                               if (Suffix != ".gif" && Suffix != ".jpeg" && Suffix != ".jpg" && Suffix != ".png") {
                                   layerCommon.msg("请上传图片类型", IconOption.错误, 2500);
                                   return;
                               }
                               if (BanerLength >= 5 && $(btnFile).hasClass("AddBanner")) {
                                   return layerCommon.msg("最多只能上传5张Bnaner图", IconOption.错误, 2500);
                               }
                               $.ajaxFileUpload({
                                   type: 'post',
                                   url: "/Controller/CompImg.ashx?FlileID=" + btnFile.id + "&Fileype=Banner&CompKey=" + $.trim($("#Hid_UserCompKey").val()) + "",
                                   secureuri: false,
                                   fileElementId: btnFile.id,
                                   dataType: 'text',
                                   success: function (msg, status) {
                                       msg == "0" ? function () { layerCommon.msg("Banner图上传失败", IconOption.哭脸, 2500) } () : function () {
                                           !$(btnFile).hasClass("AddBanner") ? $Li.attr("type", "new").find("img").attr("src", "" + EditParam.EditImgPath + "CompImage/" + msg + "").data("ImgUrl", msg) : function () {
                                               var Container = $(Showoption.btnBanner).find("ul.list"), index = parseInt(Container.children("li:last").attr("data-index")), index = isNaN(index) ? 0 : index,
                                                   Li = $('<li data-index="' + (index + 1) + '" type="new" ><div class="name">图片' + (index + 1) + '：</div><div class="pic"><img style="width:250px;" src="' + EditParam.EditImgPath + '/CompImage/' + msg + '" height="60" /></div><div class="link"><a href="javascript:;" class="a-upload"><input  name="FileUpBanner' + (index + 1) + '" class="UpBanner' + (index + 1) + '" id="FileUpBanner' + (index + 1) + '" type="file" accept="image/gif,image/jpeg,image/jpg,image/png">重新上传</a><a class="btnDelBanner" href="javascript:;">删除</a></div></li>');
                                               Li.find("img").data("ImgUrl", msg).data("type", "Add"), Container.append(Li);
                                               if (BanerLength >= 4) {
                                                   $(".ShowSucessBtn .add-pic", Showoption.btnBanner).hide();
                                               }
                                           } ();
                                       } ();
                                       EditShopVoid.BindBannerEvent();
                                   },
                                   error: function (msg, status, e) {
                                       layerCommon.msg(msg, IconOption.哭脸, 2500);
                                   }
                               });
                           });

                           $("li .btnDelBanner", Showoption.btnBanner).unbind().on("click", function (e, data) {
                               var btnDel = $(this), ImgControl = btnDel.parent().prev().children("img");
                               if (ImgControl && ImgControl.length > 0) {
                                   if (ImgControl.data("type") == "Add") {
                                       EditShopVoid.AjaxForm($(this), "/Controller/AddUpDataSource.ashx", { action: "DelCompBanner", Value: ImgControl.data("ImgUrl") }, "", RequesType.Action, function (ReturnData) {
                                           btnDel.closest("li").animate({ opacity: 0 }, 400, function () {
                                               $(".ShowSucessBtn .add-pic", Showoption.btnBanner).show();
                                               btnDel.closest("li").remove();
                                               $.each($("li", Showoption.btnBanner), function (index, LiControl) {
                                                   $(LiControl).attr("data-index", ++index), $(LiControl).children("div.name").text('图片' + (index) + '：');
                                               });
                                           });
                                       });
                                   }
                                   else {
                                       btnDel.closest("li").animate({ opacity: 0 }, 400, function () {
                                           $(".ShowSucessBtn .add-pic", Showoption.btnBanner).show();
                                           btnDel.closest("li").remove();
                                           $.each($("li", Showoption.btnBanner), function (index, LiControl) {
                                               $(LiControl).attr("data-index", ++index), $(LiControl).children("div.name").text('图片' + (index) + '：');
                                           });
                                       });
                                   }
                               }
                           });

                           $(".edit-btn .okbtn", Showoption.btnBanner).unbind().on("click", function (e, data) {
                               if ($(this).data("IsSucess") != false) {
                                   var ImgUrls = new Array();
                                   $.each($("li .pic img", Showoption.btnBanner), function (index, ImgControl) {
                                       ImgUrls.push($(ImgControl).data("ImgUrl"));
                                   });
                                   ImgUrls = ImgUrls.join(",");
                                   EditShopVoid.AjaxForm($(this), "/Controller/AddUpDataSource.ashx", { action: "UpCompBanner", Value: ImgUrls }, "", RequesType.Submit, function (ReturnData) {
                                       location.reload();
                                   });
                               }
                           });
                       },
                       BindRecommendEvent: function () {
                           $(".adMenu-height .show dt  .DelRecommend", Showoption.btnRecommend).unbind().on("click", function (e, data) {
                               var ControlDl = $(this).closest("dl");
                               ControlDl.animate({ opacity: 0 }, 400, function () {
                                   ControlDl.remove();
                                   $.each($(".adMenu-height dl", Showoption.btnRecommend), function (index, DlControl) {
                                       $(DlControl).attr("data-index", ++index), $(DlControl).children("dt").children("div.name").html('<i class="red">*</i>分类标题' + index + '：'), $(DlControl).find("dt .link .DelRecommend").attr("title", "点击删除：分类标题" + index + "");
                                   });
                               })
                           });

                           $(".adMenu-height .show dd  .DelGoods", Showoption.btnRecommend).unbind().on("click", function (e, data) {
                               var ControlDD = $(this).closest("dd");
                               if (ControlDD.siblings("dd").length == 0) {
                                   layerCommon.msg("至少保留一项商品", IconOption.错误, 2000); return;
                               }
                               ControlDD.siblings("dd").length == 1 && ControlDD.siblings("dd").children("div.dele").hide(), ControlDD.animate({ opacity: 0 }, 400, function () { ControlDD.remove(); });
                           });

                           $(".adMenu-height .show dd  .SelectGoods", Showoption.btnRecommend).unbind().on("click", function (e, data) {
                               EditParam.EditSelectParentControl = $(this).closest("dd");
                               var index = layerCommon.openWindow('选择商品', "../goodslist.aspx?KeyID=<%=KeyID %>", '880px', '450px');
                               $("#hid_Alert").val(index); //记录弹出对象
                           });

                           $(".adMenu-height .add-pic .add", Showoption.btnRecommend).unbind().on("click", function (e, data) {
                               var ControlDL = $(this).closest("dl");
                               ControlDL.children(".add-pic").remove();
                               $("dd div.dele:hidden", ControlDL).removeAttr("style");
                               ControlDL.append(function (index) {
                                   var OutHtml = "";
                                   OutHtml += '<dd class="li" data-Key="">';
                                   OutHtml += '<div class="a1"><div class="name"><i class="red">*</i>商品名称：</div><input  type="text" class="box txt_GoodsName" readonly="readonly"/><div class="link"><a class="SelectGoods" href="javascript:;">选择商品</a></div></div>';
                                   OutHtml += '<div class="a1"><div class="name">显示名称：</div><input  type="text" class="box txt_ShowName" /><div class="txt2">自定义商品显示名称</div></div>';
                                   OutHtml += '<div class="dele"><a href="javascript:;" class="DelGoods">删除</a></div><input type="hidden"  class="HidGoodsID" >';
                                   OutHtml += '</dd>';
                                   return OutHtml;
                               });
                               ControlDL.append('<div class="add-pic add-btn2" Control-Key="' + ControlDL.attr("data-index") + '"><a href="javascript:;" class="add"><i class="add-i"></i>添加</a></div>');
                               $(".adMenu-height", Showoption.btnRecommend).scrollTop($(".adMenu-height", Showoption.btnRecommend)[0].scrollHeight);
                               EditShopVoid.BindRecommendEvent();
                           });

                           $(".ShowSucessBtn .add-pic .add", Showoption.btnRecommend).unbind().on("click", function (e, data) {
                               var ControlDiv = $(".adMenu-height", Showoption.btnRecommend), Dataindex = 0;
                               ControlDiv.children("dl:last").length > 0 && (Dataindex = ControlDiv.children("dl:last").attr("data-index"));
                               ControlDiv.append(function () {
                                   var OutHtml = '<dl class="show" data-index="' + (++Dataindex) + '">';
                                   OutHtml += '<dt><div class="name"><i class="red">*</i>分类标题' + Dataindex + '：</div><input  type="text" maxlength="20" class="box txt_Title" /><div class="link" style="float:right"><a href="javascript:;" title="点击删除：分类标题' + Dataindex + '" class="DelRecommend" >删除</a></div></dt>';
                                   OutHtml += '<dd class="li" data-Key="">';
                                   OutHtml += '<div class="a1"><div class="name"><i class="red">*</i>商品名称：</div><input  type="text" class="box txt_GoodsName" readonly="readonly"/><div class="link"><a class="SelectGoods" href="javascript:;">选择商品</a></div></div>';
                                   OutHtml += '<div class="a1"><div class="name">显示名称：</div><input  type="text" maxlength="30" class="box txt_ShowName" /><div class="txt2">自定义商品显示名称</div></div>';
                                   OutHtml += '<div class="dele" style="display:none;"><a href="javascript:;" class="DelGoods">删除</a></div><input type="hidden"  class="HidGoodsID" >';
                                   OutHtml += '</dd>';
                                   OutHtml += '<div class="add-pic add-btn2" Control-Key="' + Dataindex + '"><a href="javascript:;" class="add"><i class="add-i"></i>添加</a></div>';
                                   OutHtml += '</dl>';
                                   return OutHtml;
                               });
                               $(".adMenu-height", Showoption.btnRecommend).scrollTop($(".adMenu-height", Showoption.btnRecommend)[0].scrollHeight);
                               EditShopVoid.BindRecommendEvent();
                           });


                           $(".edit-btn .okbtn", Showoption.btnRecommend).unbind().on("click", function (e, data) {
                               if ($(this).data("IsSucess") != false) {
                                   var JsonArryData = [], validator = true; $This = $(this), ItemValidator = true, txtControl = null, ParentControl = $(".adMenu-height", Showoption.btnRecommend);
                                   //数据验证 start
                                   $.each($(".adMenu-height dl", Showoption.btnRecommend), function (inex, DL) {
                                       var JsonData = { Title: "", Data: [] };
                                       txtControl = $(DL).find("dt input.txt_Title");
                                       ItemValidator = ($.trim(txtControl.val()) != "");
                                       validator = validator ? ItemValidator : validator;
                                       if (!validator) {
                                           EditParam.EditShowTip(txtControl, ParentControl, "请输入标题内容");
                                           return false;
                                       };
                                       JsonData.Title = $.trim(txtControl.val());
                                       $.each($(DL).find("dd"), function (inex, DD) {
                                           var HidC = $(DD).children(".HidGoodsID"), DataKey = "", txtControl = $(DD).find("input.txt_GoodsName"), JsonData2 = { GoodsID: "", ShoaName: "", Key: "" };
                                           ItemValidator = ($.trim(HidC.val()) != "");
                                           validator = validator ? ItemValidator : validator;
                                           if (!validator) {
                                               EditParam.EditShowTip(txtControl, ParentControl, "请选择商品", 1);
                                               return false;
                                           }
                                           txtControl = $(DD).find("input.txt_ShowName");
                                           ItemValidator = ($.trim(txtControl.val()) != "");
                                           validator = validator ? ItemValidator : validator;
                                           if (!validator) {
                                               EditParam.EditShowTip(txtControl, ParentControl, "请输入显示名称");
                                               return false;
                                           }
                                           $(DD).attr("data-key") != undefined && (DataKey = $(DD).attr("data-key"));
                                           JsonData2.GoodsID = $.trim(HidC.val()), JsonData2.ShoaName = $.trim(txtControl.val()), JsonData2.Key = DataKey;
                                           JsonData.Data.push(JsonData2);
                                       })
                                       if (!validator) {
                                           return false;
                                       }
                                       JsonArryData.push(JsonData);
                                   });
                                   //数据验证 end
                                   validator && function () {
                                       EditParam.EditRecommendCount > 0 && JsonArryData.length == 0 ? function () {
                                           layerCommon.confirm("确认删除所有分类？", function () {
                                               EditShopVoid.AjaxForm($This, "/Controller/AddUpDataSource.ashx", { action: "UpRecommend", Value: JSON.stringify(JsonArryData) }, "", RequesType.Submit, function (ReturnData) {
                                                   location.reload();
                                               });
                                           })
                                       } () : function () {
                                           EditShopVoid.AjaxForm($This, "/Controller/AddUpDataSource.ashx", { action: "UpRecommend", Value: JSON.stringify(JsonArryData) }, "", RequesType.Submit, function (ReturnData) {
                                               location.reload();
                                           });
                                       } ();
                                   } ();
                               }
                           });
                       },
                       BindFiveImgEvent: function () {
                           $("li .a-upload input:file", Showoption.btnFiveImg).unbind().on("change", function (e, data) {
                               var btnFile = this, str = $(btnFile).val(), Suffix = $.trim(str.substring(str.lastIndexOf("."))), $LI = $(btnFile).closest("li");
                               if (Suffix != ".gif" && Suffix != ".jpeg" && Suffix != ".jpg" && Suffix != ".png") {
                                   layerCommon.msg("请上传图片类型", IconOption.错误, 2500);
                                   return;
                               }
                               $.ajaxFileUpload({
                                   type: 'post',
                                   url: "/Handler/HandleImg3.ashx?FlileID=" + btnFile.id + "&Compid=<%=KeyID %>",
                                   secureuri: false,
                                   fileElementId: btnFile.id,
                                   dataType: 'text',
                                   success: function (msg, status) {
                                       msg == "0" ? function () {
                                           layerCommon.msg("图上传失败", IconOption.哭脸, 2500)
                                       } () : function () {
                                           var jdata = eval('(' + msg + ')');
                                           if (jdata.result) {
                                               $(".pic img", $LI).attr("src", EditParam.EditImgPath + "CompFiveImg/" + jdata.msg + "").data("ImgUrl", jdata.msg);
                                               $(".title .a-upload span", $LI).text("重新上传图片");
                                           } else {
                                               layerCommon.msg(jdata.msg, IconOption.哭脸, 2500);
                                           }
                                       } ();
                                       EditShopVoid.BindFiveImgEvent();
                                   },
                                   error: function (msg, status, e) {
                                       layerCommon.msg(msg, IconOption.哭脸, 2500);
                                   }
                               });

                           });

                           $("li div.dele > .DelFiveImg", Showoption.btnFiveImg).unbind().on("click", function (e, data) {
                               var $this = $(this), $LI = $this.closest("li"), $Img = $(".pic img", $LI);
                               $Img.data("type") == "Add" && function () {
                                   EditShopVoid.AjaxForm($(this), "/Controller/AddUpDataSource.ashx", { action: "DelFiveImg", Value: $Img.data("ImgUrl") }, "", RequesType.Action, function (ReturnData) {

                                   }, false);
                               } ();
                               $Img.attr("src", "/images/Goods200x200.jpg").data("ImgUrl", ""), $(".website .HidGoodsID", $LI).val(""), $(".website .HidGoodsName", $LI).val("")
                               , $(".website img", $LI).val(""), $(".title .a-upload span", $LI).text("上传图片"), $(".website .txt_GoodsUrl", $LI).val(""), $(".website .SelectGoods", $LI).text("选择商品地址");
                           });

                           $("li .website .SelectGoods", Showoption.btnFiveImg).unbind().on("click", function (e, data) {
                               var $this = $(this), $LI = EditParam.EditSelectParentControl = $this.closest("li"), $Img = $(".pic img", $LI);
                               $Img.data("ImgUrl") == "" || $Img.data("ImgUrl") == undefined ? function () {
                                   layerCommon.msg("请先上传商品图片", IconOption.哭脸, 2000);
                               } () : function () {
                                   var index = layerCommon.openWindow('选择商品', "../goodslist.aspx?Type=Five&KeyID=<%=KeyID %>", '880px', '450px');
                                   $("#hid_Alert").val(index); //记录弹出对象  
                               } ();
                           });

                           $(".edit-btn .okbtn", Showoption.btnFiveImg).unbind().on("click", function (e, data) {
                               if ($(this).data("IsSucess") != false) {
                                   var JsonArryData = [], $This = $(this), index = 0, txtControl = null, ParentControl = $(".adImg-height", Showoption.btnFiveImg);
                                   $.each($("li", ParentControl), function (inex, LI) {
                                       if (++index <= 5) {
                                           var JsonData = { GoodsUrl: "", GoodsID: "", ImgUrl: "", Key: "", GoodsName: "" };
                                           var DataKey = $(LI).attr("data-key") == undefined ? "" : $(LI).attr("data-key");
                                           JsonData.GoodsUrl = $.trim($(".website .txt_GoodsUrl", LI).val());
                                           JsonData.GoodsID = $.trim($(".website .HidGoodsID", LI).val());
                                           JsonData.GoodsName = $.trim($(".website .HidGoodsName", LI).val());
                                           JsonData.Key = $.trim(DataKey);
                                           JsonData.ImgUrl = $.trim($(".pic img", LI).data("ImgUrl"));
                                           JsonArryData.push(JsonData);
                                       }
                                   });
                                   EditShopVoid.AjaxForm($This, "/Controller/AddUpDataSource.ashx", { action: "UpFiveImg", Value: JSON.stringify(JsonArryData) }, "", RequesType.Submit, function (ReturnData) {
                                       location.reload();
                                   });
                               }
                           });

                       },
                       BindContactEvent: function () {
                           $(".edit-btn .okbtn", Showoption.btnContact).unbind().on("click", function (e, data) {
                               var JsonData = { Principal: "", Phone: "", Address: "" }, validator = true; $This = $(this), ItemValidator = true, txtControl = null, index = 0, ParentControl = $(".DivMsg", Showoption.btnContact);
                               $.each($("li", ParentControl), function (index, LI) {
                                   if (++index == 1) {
                                       txtControl = $(".txt_Principal", LI);
                                       ItemValidator = ($.trim(txtControl.val()) != "");
                                       validator = validator ? ItemValidator : validator;
                                       if (!ItemValidator) {
                                           EditParam.EditShowTip(txtControl, ParentControl, "请输入联系人", 1);
                                           return false;
                                       }
                                       JsonData.Principal = $.trim(txtControl.val());
                                   } else if (index == 2) {
                                       txtControl = $(".txt_Phone", LI);
                                       ItemValidator = ($.trim(txtControl.val()) != "");
                                       validator = validator ? ItemValidator : validator;
                                       if (!ItemValidator) {
                                           EditParam.EditShowTip(txtControl, ParentControl, "请输入电话", 1);
                                           return false;
                                       }
                                       ItemValidator = (EditParam.IsPhone($.trim(txtControl.val())));
                                       validator = validator ? ItemValidator : validator;
                                       if (!validator) {
                                           EditParam.EditShowTip(txtControl, ParentControl, "联系电话不正确", 1);
                                           return false;
                                       }
                                       JsonData.Phone = $.trim(txtControl.val());
                                   } else if (index == 3) {
                                       txtControl = $(".txt_Address", LI);
                                       ItemValidator = ($.trim(txtControl.val()) != "");
                                       validator = validator ? ItemValidator : validator;
                                       if (!ItemValidator) {
                                           EditParam.EditShowTip(txtControl, ParentControl, "请输入地址", 1);
                                           return false;
                                       }
                                       JsonData.Address = $.trim(txtControl.val());
                                   }

                               });
                               validator && function () {
                                   EditShopVoid.AjaxForm($This, "/Controller/AddUpDataSource.ashx", { action: "UpCompContact", Value: JSON.stringify(JsonData) }, "", RequesType.Submit, function (ReturnData) {
                                       location.reload();
                                   });
                               } ();
                           });
                       },
                       //绑定事件 end
                       //加载数据 start
                       BindEditData: function (ShowClass) {
                           var LoadVoid;
                           switch (ShowClass) {
                               case Showoption.btnBanner: LoadVoid = function () {
                                   EditShopVoid.AjaxForm($(ShowClass), "/Controller/GetDataSource.ashx", { action: "GetCompBanner" }, "ul.list", RequesType.Query, function (ReturnData) {
                                       var BannerArry = [];
                                       if (ReturnData.Code != "") {
                                           BannerArry = ReturnData.Code.split(",");
                                           var Container = $(ShowClass).find("ul.list");
                                           $.each(BannerArry, function (index, data) {
                                               var Li = $('<li data-index="' + (index + 1) + '"><div class="name">图片' + (index + 1) + '：</div><div class="pic"><img style="width:250px;" src="' + EditParam.EditImgPath + 'CompImage/' + data + '" height="60" /></div><div class="link"><a href="javascript:;" class="a-upload"><input  name="FileUpBanner' + (index + 1) + '" class="UpBanner' + (index + 1) + '" id="FileUpBanner' + (index + 1) + '" type="file" accept="image/gif,image/jpeg,image/jpg,image/png">重新上传</a><a class="btnDelBanner" href="javascript:;">删除</a></div></li>');
                                               Li.find("img").data("ImgUrl", data), Container.append(Li);
                                           })
                                       }
                                       $(ShowClass).append('<div class="ShowSucessBtn"><div class="add-pic" ' + (BannerArry.length >= 5 ? "style='display:none;' " : "") + ' ><a href="javascript:;" class="a-upload add"><input type="file" name="AddBanner" id="AddBanner" class="AddBanner" ><i class="add-i"></i>添加图片</a></div>  <div class="edit-btn"><a href="javascript:;" class="okbtn">确认装修</a><i class="txt">最多添加5张广告图片 ，广告图片 默认大小：1920*460</i></div></div>');
                                       EditParam.EditDivCenter($(ShowClass));
                                       EditShopVoid.BindBannerEvent();
                                   });
                               }
                               ; break;
                               case Showoption.btnRecommend: LoadVoid = function () {
                                   EditShopVoid.AjaxForm($(ShowClass), "/Controller/GetDataSource.ashx", { action: "GetRecommend" }, ".adMenu-height", RequesType.Query, function (ReturnData) {
                                       try {
                                           if (ReturnData.Code != "") {
                                               var JsonData = eval('(' + ReturnData.Code + ')');
                                               EditParam.EditRecommendCount = JsonData.length;
                                               var JsonArry = {}, OutHtml = "", index = 0, JsonArryData = [], JsonName = null;
                                               $.each(JsonData, function (index, DataValue) {
                                                   var JsonD = {};
                                                   JsonArry[DataValue.Title] == undefined && (JsonArry[DataValue.Title] = new Array(), (index > 0 && JsonArryData.push(JsonName)));
                                                   JsonD["Title"] = DataValue.Title, JsonD["GoodsName"] = DataValue.GoodsName, JsonD["ShowName"] = DataValue.ShowName, JsonD["GoodsID"] = DataValue.GoodsID, JsonD["Key"] = DataValue.ID;
                                                   JsonArry[DataValue.Title].push(JsonD);
                                                   JsonName = JsonArry[DataValue.Title];
                                                   ++index;
                                               });
                                               if (JsonName != null) {
                                                   JsonArryData.push(JsonName);
                                               }
                                               index = 0;
                                               for (var item in JsonArryData) {
                                                   var ItemName = JsonArryData[item][0].Title;
                                                   OutHtml += '<dl class="show" data-index="' + (++index) + '">';
                                                   OutHtml += '<dt><div class="name"><i class="red">*</i>分类标题' + index + '：</div><input  type="text" maxlength="20" class="box txt_Title" value="' + ItemName + '"/><div class="link" style="float:right"><a href="javascript:;" title="点击删除：分类标题' + index + '" class="DelRecommend" >删除</a></div></dt>';
                                                   $.each(JsonArryData[item], function (index, DataValue) {
                                                       OutHtml += '<dd class="li" data-Key="' + DataValue.Key + '">';
                                                       OutHtml += '<div class="a1"><div class="name"><i class="red">*</i>商品名称：</div><input  type="text" class="box txt_GoodsName" readonly="readonly" value="' + DataValue.GoodsName + '"/><div class="link"><a class="SelectGoods" href="javascript:;">重新选择</a></div></div>';
                                                       OutHtml += '<div class="a1"><div class="name">显示名称：</div><input  type="text" class="box txt_ShowName" maxlength="30" value="' + DataValue.ShowName + '"/><div class="txt2">自定义商品显示名称</div></div>';
                                                       OutHtml += '<div class="dele" ' + (JsonArryData[item].length > 1 ? "" : "style='display:none;'") + ' ><a href="javascript:;" class="DelGoods">删除</a></div><input type="hidden" value="' + DataValue.GoodsID + '" class="HidGoodsID" >';
                                                       OutHtml += '</dd>';
                                                   })
                                                   OutHtml += '<div class="add-pic add-btn2" Control-Key="' + index + '"><a href="javascript:;" class="add"><i class="add-i"></i>添加</a></div>';
                                                   OutHtml += '</dl>';
                                               }
                                           }
                                           $(".adMenu-height", ShowClass).html(OutHtml);
                                           $(ShowClass).append('<div class="ShowSucessBtn"><div class="add-pic add-btn"><a href="javascript:;" class="add"><i class="add-i"></i>添加分类标题</a></div><div class="blank10"></div><div class="edit-btn"><a href="javascript:;" class="okbtn">确认装修</a><i class="txt">点击添加分类标题,就可以添加新的分类和商品！</i></div></div>');
                                           EditParam.EditDivCenter($(ShowClass));
                                           EditShopVoid.BindRecommendEvent();
                                       }
                                       catch (ex) {

                                       }
                                   });
                               }
                           ; break;
                               case Showoption.btnFiveImg: LoadVoid = function () {
                                   EditShopVoid.AjaxForm($(ShowClass), "/Controller/GetDataSource.ashx", { action: "GetFiveImg" }, ".adImg-height", RequesType.Query, function (ReturnData) {
                                       try {
                                           var ULDemo = null, LIDemo = null, OutHTML = "", index = 0;
                                           var JsonData = eval('(' + ReturnData.Code + ')');
                                           ULDemo = $('<ul class="list"><ul>');
                                           if (ReturnData.Code != "" && JsonData.length > 0) {
                                               $.each(JsonData, function (index, Data) {
                                                   var Imgurl = Data.ImageUrl == "" ? "/images/Goods200x200.jpg" : EditParam.EditImgPath + "CompFiveImg/" + Data.ImageUrl + "", Uptext = Data.ImageUrl == "" ? "上传图片" : "重新上传图片", text = Data.GoodsUrl == "" ? "选择商品地址" : "重新选择商品地址";
                                                   if (++index == 1) {
                                                       OutHTML = '<li data-inex="' + index + '" data-key="' + Data.ID + '" > <div class="box-bg">';
                                                       OutHTML += '<div class="pic"><img src="' + Imgurl + '"   width="70" height="77" /></div>';
                                                       OutHTML += '<div class="title"><b class="c6">左侧大图</b><i class="c9">最佳尺寸（410*450）</i><a href="javascript:;" class="a-upload cbule"><input type="file" name="FileUpFiveImg' + index + '" id="FileUpFiveImg' + index + '" accept="image/gif,image/jpeg,image/jpg,image/png"  ><span>' + Uptext + '</span></a></div>';
                                                       OutHTML += '<div class="website"><input type="hidden" class="HidGoodsID" value="' + Data.GoodsID + '" /><input type="hidden" class="HidGoodsName" value="' + Data.ImageName + '" /><input  type="text" class="box txt_GoodsUrl" value="' + Data.GoodsUrl + '"/><div class="link"><a href="javascript:;" class="SelectGoods" >' + text + ' </a></div></div>';
                                                       OutHTML += '<div class="dele"><a href="javascript:;" class="DelFiveImg" >删除</a></div>';
                                                       OutHTML += '</div></li>';
                                                       LIDemo = $(OutHTML);
                                                       LIDemo.find("div.pic > img").data("ImgUrl", Data.ImageUrl);
                                                       ULDemo.append(LIDemo);
                                                   } else {
                                                       OutHTML = '<li data-inex="' + index + '" data-key="' + Data.ID + '" ><div class="box-bg">';
                                                       OutHTML += '<div class="pic"><img src="' + Imgurl + '"   width="70" height="77" /></div>';
                                                       OutHTML += '<div class="title"><b class="c6">右侧小图' + (index - 1) + '</b><i class="c9">最佳尺寸（180*220）</i><a href="javascript:;" class="a-upload cbule"><input type="file" name="FileUpFiveImg' + index + '" id="FileUpFiveImg' + index + '" accept="image/gif,image/jpeg,image/jpg,image/png" ><span>' + Uptext + '</span></a></div>';
                                                       OutHTML += '<div class="website"><input type="hidden" class="HidGoodsID" value="' + Data.GoodsID + '" /><input type="hidden" class="HidGoodsName" value="' + Data.ImageName + '" /><input  type="text" class="box txt_GoodsUrl" value="' + Data.GoodsUrl + '"/><div class="link"><a href="javascript:;" class="SelectGoods" >' + text + '</a></div></div>';
                                                       OutHTML += '<div class="dele"><a href="javascript:;" class="DelFiveImg" >删除</a></div>';
                                                       OutHTML += '</div></li>';
                                                       LIDemo = $(OutHTML);
                                                       LIDemo.find("div.pic > img").data("ImgUrl", Data.ImageUrl);
                                                       ULDemo.append(LIDemo);
                                                   }
                                               });
                                               for (var i = JsonData.length; i < 5; i++) {
                                                   OutHTML = '<li data-inex="' + (i + 1) + '" data-key="" type="new" ><div class="box-bg">';
                                                   OutHTML += '<div class="pic"><img src="/images/Goods200x200.jpg"   width="70" height="77" /></div>';
                                                   OutHTML += '<div class="title"><b class="c6">右侧小图' + (i) + '</b><i class="c9">最佳尺寸（180*220）</i><a href="javascript:;" class="a-upload cbule"><input type="file" name="FileUpFiveImg' + (i + 1) + '" id="FileUpFiveImg' + (i + 1) + '" accept="image/gif,image/jpeg,image/jpg,image/png" ><span>上传图片</span></a></div>';
                                                   OutHTML += '<div class="website"><input type="hidden" class="HidGoodsID"  /><input type="hidden" class="HidGoodsName"/><input  type="text" class="box txt_GoodsUrl" value=""/><div class="link"><a href="javascript:;" class="SelectGoods" >选择商品地址 </a></div></div>';
                                                   OutHTML += '<div class="dele"><a href="javascript:;" class="DelFiveImg" >删除</a></div>';
                                                   OutHTML += '</div></li>';
                                                   LIDemo = $(OutHTML);
                                                   LIDemo.find("div.pic > img").data("ImgUrl", "").data("type", "Add");
                                                   ULDemo.append(LIDemo);
                                               }
                                           }
                                           else {
                                               for (var i = 1; i <= 5; i++) {
                                                   if (i == 1) {
                                                       OutHTML = '<li data-inex="' + i + '" data-key="" type="new" ><div class="box-bg">';
                                                       OutHTML += '<div class="pic"><img src="/images/Goods200x200.jpg"   width="70" height="77" /></div>';
                                                       OutHTML += '<div class="title"><b class="c6">左侧大图</b><i class="c9">最佳尺寸（410*450）</i><a href="javascript:;" class="a-upload cbule"><input type="file" name="FileUpFiveImg' + i + '" id="FileUpFiveImg' + i + '" accept="image/gif,image/jpeg,image/jpg,image/png"  ><span>上传图片</span></a></div>';
                                                       OutHTML += '<div class="website"><input type="hidden" class="HidGoodsID"  /><input type="hidden" class="HidGoodsName" /><input  type="text" class="box txt_GoodsUrl" value=""/><div class="link"><a href="javascript:;" class="SelectGoods" >选择商品地址 </a></div></div>';
                                                       OutHTML += '<div class="dele"><a href="javascript:;" class="DelFiveImg" >删除</a></div>';
                                                       OutHTML += '</div></li>';
                                                       LIDemo = $(OutHTML);
                                                       LIDemo.find("div.pic > img").data("ImgUrl", "").data("type", "Add");
                                                       ULDemo.append(LIDemo);
                                                   } else {
                                                       OutHTML = '<li data-inex="' + i + '" data-key="" type="new" ><div class="box-bg">';
                                                       OutHTML += '<div class="pic"><img src="/images/Goods200x200.jpg"   width="70" height="77" /></div>';
                                                       OutHTML += '<div class="title"><b class="c6">右侧小图' + (i - 1) + '</b><i class="c9">最佳尺寸（180*220）</i><a href="javascript:;" class="a-upload cbule"><input type="file" name="FileUpFiveImg' + i + '" id="FileUpFiveImg' + i + '" accept="image/gif,image/jpeg,image/jpg,image/png" ><span>上传图片</span></a></div>';
                                                       OutHTML += '<div class="website"><input type="hidden" class="HidGoodsID"  /><input  type="text" class="box txt_GoodsUrl" value=""/><div class="link"><a href="javascript:;" class="SelectGoods" >选择商品地址 </a></div></div>';
                                                       OutHTML += '<div class="dele"><a href="javascript:;" class="DelFiveImg" >删除</a></div>';
                                                       OutHTML += '</div></li>';
                                                       LIDemo = $(OutHTML);
                                                       LIDemo.find("div.pic > img").data("ImgUrl", "").data("type", "Add");
                                                       ULDemo.append(LIDemo);
                                                   }
                                               }
                                           }
                                           $(".adImg-height", ShowClass).append(ULDemo);
                                           $(ShowClass).append('<div class="ShowSucessBtn"> <div class="blank10"></div><div class="edit-btn"><a href="javascript:;" class="okbtn">确认装修</a><i class="txt">点击上传图片，即可上传推荐商品图片</i></div><divs>');
                                           EditParam.EditDivCenter($(ShowClass));
                                           EditShopVoid.BindFiveImgEvent();
                                       }
                                       catch (e) {
                                           var a;
                                       }
                                   });
                               }; break;
                               case Showoption.btnContact: LoadVoid = function () {
                                   EditShopVoid.AjaxForm($(ShowClass), "/Controller/GetDataSource.ashx", { action: "GetCompContact" }, ".DivMsg", RequesType.Query, function (ReturnData) {
                                       try {
                                           if (ReturnData.Code != "") {
                                               var JsonData = eval('(' + ReturnData.Code + ')');
                                               if (JsonData.length > 0) {
                                                   var OutHTML = "";
                                                   $.each(JsonData, function (index, DataValue) {
                                                       OutHTML += '<ul class="list">';
                                                       OutHTML += '<li><div class="name">联系人：</div><input  type="text" class="box txt_Principal" value="' + DataValue.Principal + '"/></li>';
                                                       OutHTML += ' <li><div class="name">电话：</div><input  type="text" class="box txt_Phone" value="' + DataValue.Phone + '"/></li>';
                                                       OutHTML += '   <li><div class="name">地址：</div><input type="text" class="box txt_Address" value="' + DataValue.Address + '"/></li>';
                                                       OutHTML += '</ul>';
                                                   });
                                                   $(".DivMsg", ShowClass).append(OutHTML);
                                                   $(ShowClass).append('<div class="ShowSucessBtn"><div class="edit-btn"><a href="javascript:;" class="okbtn">确认装修</a></div></div>');
                                                   EditParam.EditDivCenter($(ShowClass));
                                                   EditShopVoid.BindContactEvent();
                                               }
                                           }
                                       }
                                       catch (e) {

                                       }
                                   });
                               }; break;
                           }
                           LoadVoid();
                       }
                       //加载数据 end
                   };
                   EditShopVoid.BindEditShopEvent();
               }
               });
           })(jQuery);
           var EditParam = {
               EditRecommendCount: 0,
               EditImgPath: '<%=ConfigurationManager.AppSettings["ImgViewPath"] %>',
               EditDivCenter: function (ThisControl) {
                   var Screenheight = (document.documentElement.clientHeight + $(window).scrollTop()), height = $(ThisControl).outerHeight(), Top = 0, Bodyheight = document.body.clientHeight; //计算高度
                   Top = (Screenheight + $(window).scrollTop() - height) / 2;
                   ThisControl.css("top", Top);
               },
               WebDomainName: '<%=ConfigurationManager.AppSettings["WebDomainName"]%>',
               EditSelectParentControl: null,
               EditSetTopGoodsValue: function (Id, Name) {
                   EditParam.EditSelectParentControl != null && function () {
                       $(".link .SelectGoods", EditParam.EditSelectParentControl).text("重新选择"), $(".txt_GoodsName", EditParam.EditSelectParentControl).val(Name), $(".txt_ShowName", EditParam.EditSelectParentControl).val(Name), $(".HidGoodsID", EditParam.EditSelectParentControl).val(Id);
                       layerCommon.layerClose("hid_Alert");
                   } ();
               },
               EditSetFiveImgGoodsValue: function (Id, Name) {
                   EditParam.EditSelectParentControl != null && function () {
                       $(".website .HidGoodsName", EditParam.EditSelectParentControl).val(Name), $(".website .SelectGoods", EditParam.EditSelectParentControl).text("重新选择商品地址"), $(".website .txt_GoodsUrl", EditParam.EditSelectParentControl).val(EditParam.WebDomainName + "/e" + Id + "_<%=KeyID%>.html"), $(".website .HidGoodsID", EditParam.EditSelectParentControl).val(Id);
                       layerCommon.layerClose("hid_Alert");
                   } ();
               },
               EditShowTip: function (txtControl, ParentControl, Msg, TipPosion) {
                   var Top, TipPosion = TipPosion == undefined ? 2 : TipPosion, txtTop = $(txtControl).PositionParent(ParentControl).top;
                   txtTop > 0 ? function () {
                       Top = txtTop + 50 > ParentControl[0].clientHeight ? (txtTop + 50 - ParentControl[0].clientHeight) : 0;
                       Top > 1 && ParentControl.scrollTop(ParentControl.scrollTop() + Top);
                   } () : function () {
                       ParentControl.scrollTop(ParentControl.scrollTop() + txtTop - 40);
                   } ();
                   layerCommon.tip(Msg, txtControl, { tips: [TipPosion, "#F90"], time: 3000 });
               },
               IsPhone: function IsMobile(value) {
                   var isMobile = /^0?1[0-9]{10}$/;
                   return isMobile.test(value);
               }
           };
   </script>
    <script type="text/javascript">
        <%=BindShowJson %>
        $(document).ready(function () {
            $(".edit-title:eq(0)").LockMove({ MoveWindow: ".banner-edit" },false);
            $(".edit-title:eq(1)").LockMove({ MoveWindow: ".adMenu-edit" },false);
            $(".edit-title:eq(2)").LockMove({ MoveWindow: ".adImg-edit" },false);
            $(".edit-title:eq(3)").LockMove({ MoveWindow: ".contact-edit" },false);
            $.BindEditShopManager("#btnBanner,#btnRecommend,#btnFiveImg,#btnContact", Showoption, ".edit-mask");
            $(".fullSlide").hover(function () {
                $(this).find(".prev,.next").stop(true, true).fadeTo("show", 0.1);
            },
	function () {
	    $(this).find(".prev,.next").fadeOut()
	});
            $(".prev,.next").hover(function () {
                $(this).fadeTo("show", 0.5);
            }, function () {
                $(this).fadeTo("show", 0.1);
            })

            $(".fullSlide").slide({
                titCell: ".hd ul",
                mainCell: ".bd ul",
                effect: "fold",
                autoPlay: true,
                autoPage: true,
                trigger: "click",
                startFun: function (i) {
                    var curLi = jQuery(".fullSlide .bd li").eq(i);
                    if (!!curLi.attr("_src")) {
                        curLi.css("background-image", curLi.attr("_src")).removeAttr("_src")
                    }
                }
            });
        });
    </script>
    <style>
  .ebled {
    background: rgb(129,129,129) !important;
}
    </style>
</head>
<body >
    <form id="form1" runat="server">
    <div runat="server" id="DIvBodyHTML">
    <input type="hidden" runat="server" id="Hid_UserCompKey" />
    <input type="hidden" id="hid_Alert">

    <!--banner编辑 start-->
    <div class="banner-edit" style=" display:none;">
    	<div class="edit-title"><h3>店铺广告区</h3><a href="javascript:;" class="close"></a></div>
        <ul class="list">
        </ul>
    </div>
    <!--banner编辑 end-->
    
    
    <!--店铺推荐编辑 start-->
    <div class="adMenu-edit" style=" display:none;">
    	<div class="edit-title"><h3>店铺推荐</h3><a href="javascript:;" class="close"></a></div>
    	<div class="adMenu-height" style="min-height:0px;">    

     	</div> 
    </div>
    <!--店铺推荐编辑 end-->
    
    
    <!--图片4+1 start-->
	<div class="adImg-edit" style=" display:none;">
    	<div class="edit-title"><h3>主推图片维护</h3><a href="javascript:;" class="close"></a></div>
        <!--图片列表维护 start-->
        <div class="adImg-height" style="min-height:50px;">
        </div>
	</div>
    <!--图片4+1 end-->
    
    
    <!--联系方式编辑 start-->
	<div class="contact-edit" style="display:none;  height:auto; " >
    <div class="edit-title"><h3>联系方式</h3><a href="javascript:;" class="close"></a></div>
    <div class="DivMsg">
    </div>
<%--		<div class="edit-btn"><a href="javascript:;" class="okbtn">确认装修</a></div>--%>
	</div>
    <!--联系方式编辑 end-->
    
    <!--遮罩层 start-->
    <div class="edit-mask" style="display:none;"></div>
    <!--遮罩层 end-->
    
    
    
    <div class="edit_title" style=" margin-top:0px;"><div class="left bt"><i class="shops-i"></i>店铺装修</div><div class="right"><a href="/<%=KeyID %>.html" class="okBtn">完成发布</a></div></div>
    <!--banner start-->
        <div class="fullSlide" runat="server"  id="Top_Banner">
        	<div class="edit-box"><div class="mask"></div><a href="javascript:;" id="btnBanner" class="btn">编辑</a></div>
            <div class="bd">
                <ul runat="server" id="BannerUl">
                </ul>
            </div>
            <div class="hd">
                <ul>
                </ul>
            </div>
            <span class="prev"></span><span class="next"></span>
        </div>
    <!--banner end-->

      <div class="boxa11" style="position:relative;">   
      
      <!--广告区 start-->
<div class="adbox" runat="server"  id="Top_Advertisement">
	<!--店铺推荐 start-->
	<div class="adMenu">
    	<div class="edit-box"><div class="mask"></div><a href="javascript:;" id="btnRecommend" class="btn">编辑</a></div>
    	<div class="title">店铺推荐 <i class="sale" style=" font-size:12px; height:15px; margin-left:0px;">Hot</i></div>
        <asp:Literal ID="lblHtml" runat="server"></asp:Literal>
    </div>
	<!--店铺推荐 end-->
    <!--产品图片推荐 start-->
    <div class="adImg-box">
       <div class="edit-box"><div class="mask"></div><a href="javascript:;" id="btnFiveImg" class="btn">编辑</a></div>
        <div class="adImg"><a target="_blank" href="<%=GetBannerTopImg("GoodsUrl",1) %>"><img height="450" width="410" src="<%=GetBannerTopImg("Img",1) %>" /></a></div>
        <ul class="adImg2">
            <li><a target="_blank" href="<%=GetBannerTopImg("GoodsUrl",2) %>"><img width="180" height="220" src="<%=GetBannerTopImg("Img",2) %>" /></a></li>
            <li><a target="_blank" href="<%=GetBannerTopImg("GoodsUrl",3) %>"><img  width="180" height="220" src="<%=GetBannerTopImg("Img",3) %>" /></a></li>
            <li><a target="_blank" href="<%=GetBannerTopImg("GoodsUrl",4) %>"><img  width="180" height="220" src="<%=GetBannerTopImg("Img",4) %>" /></a></li>
            <li><a target="_blank" href="<%=GetBannerTopImg("GoodsUrl",5) %>"><img  width="180" height="220" src="<%=GetBannerTopImg("Img",5) %>" /></a></li>
        </ul>
    </div>
    <!--产品图片推荐 end-->
    <!--信息联系方式 start-->
    <div class="adInfo">
    	<div class="title"><a href="" class="hover">消息</a><a href="" style="display:none;">通知</a><a href="" style="display:none;">公告</a></div>
        <ul class="list" runat="server" id="NewsList">
        </ul>
        <div class="d-contact">
            <div class="edit-box"><div class="mask"></div><a href="javascript:;" id="btnContact" class="btn">编辑</a></div>
            <div class="title"><a class="hover">联系方式</a></div>
            <ul class="cut">
                <li><p runat="server" id="lblPrincipal" /></li>
                <li><p runat="server" id="lblPhone" /></li>
                <li><p runat="server" id="lblAddress" /></li>
            </ul>
         </div>
    </div>
    <!--信息联系方式 end-->
    
</div>
<!--广告区 end-->
      </div>
       <div class="blank10">
       </div>

    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FCmaterials.aspx.cs" Inherits="Company_SysManager_DisEdit" %>

<%@ Register Src="~/Company/UserControl/TreeDisArea.ascx" TagPrefix="uc1" TagName="TreeDisArea" %>
<%@ Register Src="~/Company/UserControl/TreeDisType.ascx" TagPrefix="uc1" TagName="TreeDisType" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>我的首营资料</title>

    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript"> 
      
        $(document).ready(function () {
            
            $("#uploadFile").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText", ResultId: "HidFfileName", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
            $("#uploadFile2").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText2", ResultId: "HidFfileName2", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
            
            $("#uploadFile3").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText3", ResultId: "HidFfileName3", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
            $("#uploadFile4").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText4", ResultId: "HidFfileName4", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
            $("#uploadFile5").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText5", ResultId: "HidFfileName5", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
            $("#uploadFile6").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText6", ResultId: "HidFfileName6", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
            $("#uploadFile7").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText7", ResultId: "HidFfileName7", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
            $("#uploadFile8").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText8", ResultId: "HidFfileName8", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
            $("#uploadFile9").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText9", ResultId: "HidFfileName9", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
            $("#uploadFile10").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText10", ResultId: "HidFfileName10", AjaxSrc: "../../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../../" });
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


        //营业执照  重复上传判断
        function uploadFileClick() {
            if ($("#HidFfileName").val() != "" && $("#HidFfileName").val() != undefined)
            {
                layerCommon.msg("请勿重复上传！", IconOption.哭脸);
                return false;
            }
              
            else
                return true;
        }
        //生产许可证  重复上传判断
        function uploadFile2Click() {
            if ($("#HidFfileName2").val() != "" && $("#HidFfileName2").val() != undefined)
            {
                layerCommon.msg("请勿重复上传！", IconOption.哭脸);
                return false;
            }
            else
                return true;
        }
        //税务登记证  重复上传判断
        function uploadFile3Click() {
            if ($("#HidFfileName3").val() != "" && $("#HidFfileName3").val() != undefined) {
                layerCommon.msg("请勿重复上传！", IconOption.哭脸);
                return false;
            }
            else
                return true;
        }

        //开户银行许可证  重复上传判断
        function uploadFile4Click() {
            if ($("#HidFfileName4").val() != "" && $("#HidFfileName4").val() != undefined) {
                layerCommon.msg("请勿重复上传！", IconOption.哭脸);
                return false;
            }
            else
                return true;
        }

        //质量管理体系调查表  重复上传判断
        function uploadFile5Click() {
            if ($("#HidFfileName5").val() != "" && $("#HidFfileName5").val() != undefined) {
                layerCommon.msg("请勿重复上传！", IconOption.哭脸);
                return false;
            }
            else
                return true;
        }

        //GSP/GMP证书  重复上传判断
        function uploadFile6Click() {
            if ($("#HidFfileName6").val() != "" && $("#HidFfileName6").val() != undefined) {
                layerCommon.msg("请勿重复上传！", IconOption.哭脸);
                return false;
            }
            else
                return true;
        }


        //开票信息  重复上传判断
        function uploadFile7Click() {
            if ($("#HidFfileName7").val() != "" && $("#HidFfileName7").val() != undefined) {
                layerCommon.msg("请勿重复上传！", IconOption.哭脸);
                return false;
            }
            else
                return true;
        }


        //企业年报  重复上传判断
        function uploadFile8Click() {
            if ($("#HidFfileName8").val() != "" && $("#HidFfileName8").val() != undefined) {
                layerCommon.msg("请勿重复上传！", IconOption.哭脸);
                return false;
            }
            else
                return true;
        }

        //银行收付款帐号资料  重复上传判断
        function uploadFile9Click() {
            if ($("#HidFfileName9").val() != "" && $("#HidFfileName9").val() != undefined) {
                layerCommon.msg("请勿重复上传！", IconOption.哭脸);
                return false;
            }
            else
                return true;
        }


        //购销合同 重复上传判断
        function uploadFile10Click() {
            if ($("#HidFfileName10").val() != "" && $("#HidFfileName10").val() != undefined) {
                layerCommon.msg("请勿重复上传！", IconOption.哭脸);
                return false;
            }
            else
                return true;
        }

        //确认提交  非空验证
        function formCheck() {
            if ($("#HidFfileName").val() == "") {
                layerCommon.msg("请选择营业执照", IconOption.哭脸);
                return false;
            }
            if ($("#HidFfileName").val() != "" && $("#HidFfileName").val() != undefined) {
                if ($("#validDate").val() == "" || $("#validDate").val() == undefined) {
                    layerCommon.msg("请选择营业执照有效期！", IconOption.哭脸);
                    return false;
                }
            }
            if ($("#HidFfileName2").val() == "") {
                layerCommon.msg("请选择生产许可证", IconOption.哭脸);
                return false;
            }
             if ($("#HidFfileName2").val() != "" && $("#HidFfileName2").val() != undefined) {
                if ($("#validDate2").val() == "" || $("#validDate2").val() == undefined) {
                    layerCommon.msg("请选择生产许可证有效期！", IconOption.哭脸);
                    return false;
                }
            }

        }
        
        //营业执照附件删除
        function AnnexDelete() {
            $("#UpFileText").html("");
            $("#HidFfileName").val("");

        }

        //生产许可证附件删除
        function AnnexDelete2() {
            $("#UpFileText2").html("");
            $("#HidFfileName2").val("");
        }
        function AnnexDelete3() {
            $("#UpFileText3").html("");
            $("#HidFfileName3").val("");
        }
        function AnnexDelete4() {
            $("#UpFileText4").html("");
            $("#HidFfileName4").val("");
        }
        function AnnexDelete5() {
            $("#UpFileText5").html("");
            $("#HidFfileName5").val("");
        }
        function AnnexDelete6() {
            $("#UpFileText6").html("");
            $("#HidFfileName6").val("");
        }
        function AnnexDelete7() {
            $("#UpFileText7").html("");
            $("#HidFfileName7").val("");
        }
        function AnnexDelete8() {
            $("#UpFileText8").html("");
            $("#HidFfileName8").val("");
        }
        function AnnexDelete9() {
            $("#UpFileText9").html("");
            $("#HidFfileName9").val("");
        }
        function AnnexDelete10() {
            $("#UpFileText10").html("");
            $("#HidFfileName10").val("");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-2" />
    <uc1:CompRemove runat="server" ID="Remove" ModuleID="2" />
    <input type="hidden"  id="hid_Alert"/> 
    <input id="hid_Alerts" type="hidden" />
<div class="rightinfo">
<!--当前位置 start-->
<div class="info">
    <a href="../jsc.aspx">我的桌面 </a>>
    <a href="../SysManager/FCmaterialsInfo.aspx" runat="server" id="atitle">我的首营资料</a>>
    <a href="#" runat="server" id="btitle">我的首营资料编辑</a>
</div>
<!--当前位置 end-->


    <div class="c-n-title">基本资料</div>
<ul class="coreInfo">
	
     <li class="lb clear">
    	<i class="name fl" style="width:150px;"><i class="red">*</i>营业执照</i>
    	 <div class="con upload">
         <div style="float:left">
             <a href="javascript:;" class="a-upload bclor le"> 
             <input id="uploadFile" runat="server" type="file" 
                 onclick="return uploadFileClick()"
                 name="fileAttachment"  class="AddBanner"/>上传附件</a></div>
         <div style="float:left">
          <div id="UpFileText" style="margin-left:10px;width:240px;" runat="server" >
          </div>
         </div>     
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate" runat="server"  onclick="WdatePicker()" style="width: 115px;"
                    id="validDate" readonly="readonly" type="text" class="Wdate" value="" /></i>
       </div>
              <asp:Panel runat="server" ID="DFile" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName" type="hidden"  />
     </li>


     <li class="lb clear">
    	<i class="name fl" style="width:150px;"><i class="red">*</i>生产/许可证</i>
    	 <div class="con upload">
         <div style="float:left">
             <a href="javascript:;" class="a-upload bclor le">
         <input id="uploadFile2" runat="server" type="file" 
              onclick="return uploadFile2Click()"
             name="fileAttachment2"  class="AddBanner"/>上传附件</a></div>
         <div style="float:left">
          <div id="UpFileText2" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate2" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate2" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile2" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName2" type="hidden" />
     </li>

     <li class="lb clear">
    	<i class="name fl" style="width:150px;">税务登记证</i>
    	 <div class="con upload">
         <div style="float:left">
             <a href="javascript:;" class="a-upload bclor le">
         <input id="uploadFile3" runat="server" type="file" 
              onclick="return uploadFile3Click()"
             name="fileAttachment3"  class="AddBanner"/>上传附件</a></div>
         <div style="float:left">
          <div id="UpFileText3" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate3" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate3" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile3" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName3" type="hidden" />
     </li>


     <li class="lb clear">
    	<i class="name fl" style="width:150px;">开户银行许可证</i>
    	 <div class="con upload">
         <div style="float:left">
             <a href="javascript:;" class="a-upload bclor le">
         <input id="uploadFile4" runat="server" type="file" 
              onclick="return uploadFile4Click()"
             name="fileAttachment3"  class="AddBanner"/>上传附件</a></div>
         <div style="float:left">
          <div id="UpFileText4" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate4" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate4" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile4" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName4" type="hidden" />
     </li>

     <li class="lb clear">
    	<i class="name fl" style="width:150px;">质量管理体系调查表</i>
    	 <div class="con upload">
         <div style="float:left">
             <a href="javascript:;" class="a-upload bclor le">
         <input id="uploadFile5" runat="server" type="file" 
              onclick="return uploadFile5Click()"
             name="fileAttachment3"  class="AddBanner"/>上传附件</a></div>
         <div style="float:left">
          <div id="UpFileText5" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate5" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate5" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile5" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName5" type="hidden" />
     </li>

     <li class="lb clear">
    	<i class="name fl" style="width:150px;">GSP/GMP证书</i>
    	 <div class="con upload">
         <div style="float:left">
             <a href="javascript:;" class="a-upload bclor le">
         <input id="uploadFile6" runat="server" type="file" 
              onclick="return uploadFile6Click()"
             name="fileAttachment3"  class="AddBanner"/>上传附件</a></div>
         <div style="float:left">
          <div id="UpFileText6" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate6" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate6" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile6" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName6" type="hidden" />
     </li>

     <li class="lb clear">
    	<i class="name fl" style="width:150px;">开票信息</i>
    	 <div class="con upload">
         <div style="float:left">
             <a href="javascript:;" class="a-upload bclor le">
         <input id="uploadFile7" runat="server" type="file" 
              onclick="return uploadFile7Click()"
             name="fileAttachment3"  class="AddBanner"/>上传附件</a></div>
         <div style="float:left">
          <div id="UpFileText7" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate7" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate7" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile7" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName7" type="hidden" />
     </li>


     <li class="lb clear">
    	<i class="name fl" style="width:150px;">企业年报</i>
    	 <div class="con upload">
         <div style="float:left">
             <a href="javascript:;" class="a-upload bclor le">
         <input id="uploadFile8" runat="server" type="file" 
              onclick="return uploadFile8Click()"
             name="fileAttachment3"  class="AddBanner"/>上传附件</a></div>
         <div style="float:left">
          <div id="UpFileText8" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate8" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate8" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile8" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName8" type="hidden" />
     </li>

     <li class="lb clear">
    	<i class="name fl" style="width:150px;">银行收付款帐号资料</i>
    	 <div class="con upload">
         <div style="float:left">
             <a href="javascript:;" class="a-upload bclor le">
         <input id="uploadFile9" runat="server" type="file" 
              onclick="return uploadFile9Click()"
             name="fileAttachment3"  class="AddBanner"/>上传附件</a></div>
         <div style="float:left">
          <div id="UpFileText9" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate9" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate9" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile9" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName9" type="hidden" />
     </li>

     <li class="lb clear">
    	<i class="name fl" style="width:150px;">购销合同</i>
    	 <div class="con upload">
         <div style="float:left">
             <a href="javascript:;" class="a-upload bclor le">
         <input id="uploadFile10" runat="server" type="file" 
              onclick="return uploadFile10Click()"
             name="fileAttachment3"  class="AddBanner"/>上传附件</a></div>
         <div style="float:left">
          <div id="UpFileText10" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate10" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate10" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile10" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName10" type="hidden" />
     </li>
</ul>
         


<div class="c-n-title">开票信息</div>
<ul class="coreInfo">
	<li class="lb fl"><i class="name">发票抬头</i>
     <input type="text" class="box1" placeholder="" runat="server" maxlength="50" id="txtRise"/></li>

    	<li class="lb fl"><i class="name">发票内容</i>
     <input type="text" class="box1" placeholder="" runat="server" maxlength="50" id="txtContent"/></li>
    	<li class="lb fl"><i class="name"><i class="red"></i>开户银行</i>
     
             <select id="txtOBank" runat="server" onchange="Otypechange(this.options[this.selectedIndex].value)"
                               class="box1" style="width:382px;">
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



    	</li>

    
    	<li class="lb fl"><i class="name"><i class="red"></i>开户账户</i>
     <input type="text" class="box1" placeholder="" runat="server" maxlength="50" id="txtOAccount"/></li>

    	<li class="lb fl"><i class="name"><i class="red"></i>纳税人登记号</i>
     <input type="text" class="box1" placeholder="" runat="server" maxlength="50" id="txtTRNumber"/></li>
	
</ul>


<div class="blank60"></div>
<div class="btn-box">
	<div class="btn">
    
             
    <a href="javascript:;" class="btn-area" id="btnAdds">提交</a>
    <a href="javascript:;" class="gray-btn" onclick="javascript:history.go(-1);">取消</a></div>
     <asp:Button ID="btnAdd" CssClass="" runat="server"  OnClientClick="return formCheck()"  OnClick="btnAdd_Click" />
	<div class="bg"></div>
</div>

</div>



        <input id="txtbankcodes" type="hidden" name="txtbankcodes" runat="server" />
        <input id="txtbandname" type="hidden" name="txtbandname" runat="server" />
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserEdit.aspx.cs" Inherits="Distributor_UserEdit" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>基本信息</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Company/js/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../js/CommonJs.js"></script>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>
    <link href="../Company/css/Enterprice.css" rel="stylesheet" type="text/css" />
    <script src="../js/CommonJs.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script> 
    <script src="../Company/js/CitysLine/JQuery-Citys-online-min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function getFileName(o) {
            var pos = o.lastIndexOf("\\");
            return o.substring(pos + 1);
        }

        function formCheck() {
            var str = "";
            if ($.trim($("#txtDisName").val()) == "") {
                str = "名称不能为空";
            }
            else if ($.trim($("#ddlProvince").val()) == "") {
                str = "请选择省";
            }
            else if ($.trim($("#ddlCity").val()) == "") {
                str = "请选择市";
            }
            else if ($.trim($("#ddlArea").val()) == "") {
                str = "请选择区";
            }
            else if ($.trim($("#txtAddress").val()) == "") {
                str = "详细地址不能为空";
            }
            else if ($.trim($("#txtUserTrueName").val())=="") {
                str = "管理员姓名不能为空";
            }
            else if (!$.trim($("#txtLicence").val()) == "") {
                if (!validateIdCard($.trim($("#txtLicence").val()))) {
                    str = "负责人身份证号码不正确。";
                }
            }
            if (str != "") {
                layerCommon.alert(str, IconOption.错误);
                return false;
            }
            return true;
        }
        $(document).ready(function () {
            $("#uploadFile").AjaxUploadFile({ Src: "UploadFile/", ShowDiv: "UpFileText", ResultId: "HidFfileName", AjaxSrc: "../Controller/Fileup.ashx", maxlength: 5, DownSrc: "../" });
            $("#btnupdate").on("click", function () {
                $("#btnSave").trigger("click");
            })
        });

        function phoneedit() {
            $("#btnSave1").trigger("click");  
        }
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <Head:Head ID="Head1" runat="server" />
    <div class="w1200">
    <Left:Left ID="Left1" runat="server" ShowID="nav-6" />
    <div class="rightCon">
    <div class="info"> <a id="navigation1" href="UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="#" class="cur">基本信息</a></div>
        <!--修改资料 start-->
        <div class="userTrend">
            <div class="uTitle">
                <b>基本信息维护</b></div>
            <div class="orderNr" style=" border:none;">
            <asp:Button ID="btnSave" runat="server" Text="确定"  OnClientClick="return formCheck()" onclick="btn_Save" style="display:none;" />
            <asp:Button ID="btnSave1" runat="server" Text="确定" onclick="btn_Save1" style="display:none;" />
            <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                <tbody>
                    <tr>
                        <td class="head">
                            <span><i class="required">*</i></span>名  称
                        </td>
                        <td colspan="3">
                            <input runat="server" type="text" class="box" maxlength="50" id="txtDisName" /><i
                                class="grayTxt">（2-20个汉字或字母，推荐使用中文名称）</i>
                        </td>
                    </tr>
                    <tr>
                        <td class="head" style="width: 10%">
                            负责人
                        </td>
                        <td style="width: 23%">
                            <input runat="server" type="text" class="box" maxlength="50" id="txtLeading" />
                        </td>
                        <td class="head" style="width: 10%">
                            负责人身份证号码
                        </td>
                        <td style="width: 23%">
                            <input runat="server" type="text" class="box" maxlength="50" id="txtLicence" />
                        </td>
                    </tr>
                    <tr>                      
                        <td class="head">
                            负责人手机
                        </td>
                        <td>
                            <input runat="server" type="text" class="box" maxlength="50" id="txtLeadingPhone" />
                        </td>
                        <td class="head">
                            联系人
                        </td>
                        <td>
                            <input runat="server" type="text" class="box" maxlength="50" id="txtname" />
                        </td>
                    </tr>
                    <tr>
                        <td class="head" style="width: 10%">
                            固定电话
                        </td>
                        <td style="width: 30%;">
                            <input runat="server" type="text" class="box" maxlength="50" id="txtTel" />
                        </td>
                        <td class="head">
                            联系人手机
                        </td>
                        <td>
                            <input runat="server" type="text" class="box" maxlength="50" id="txtnamephone" />
                        </td>
                    </tr>
                    <tr>
                        <td class="head">
                            传  真
                        </td>
                        <td >
                            <input runat="server" type="text" maxlength="50" class="box" id="txtFax" />
                        </td>
                        <td class="head">
                            邮  编
                        </td>
                        <td >
                            <input runat="server" type="text" maxlength="50" class="box" id="txtZip" />
                        </td>
                    </tr>
                    <tr>
                        <td class="head" >
                            <span>附件上传</span>
                        </td>
                        <td colspan="3" style="padding-top:2px;padding-bottom:2px;">
                            <div class="teamR" style=" margin: 5px 0px 0px 0px;">
                                <div class="verFile" style="margin: 0px; width:100%;padding-bottom:4px; "><span class="verFileCon" style=" min-width:60px;">
                                    <input id="uploadFile" runat="server" type="file" name="fileAttachment"  /></span>
                                    <a class="btn1" id="A1" style="margin-left: 5px; text-decoration: NONE;" href="javascript:void(0)">
                                        <b class="L"></b><b class="R"></b>上传附件</a></div>
                                               <asp:Panel runat="server" id="DFile" style=" margin-left:5px; "></asp:Panel>

                            </div>
                             <div id="UpFileText" style=" margin:5px 5px;">
                            </div>
                            <input runat="server" id="HidFfileName" type="hidden" />
                        </td>
                    </tr>
                    <tr>
                        <td class="head">
                            <span><i class="required">*</i></span>详细地址
                        </td>
                        <td colspan="3">
                       &nbsp; <select  runat="server" id="ddlProvince"   class="prov xl" onchange="Change()" ></select>
                        <input type="hidden" id="hidProvince" runat="server"  value="选择省" />
                        <select runat="server" id="ddlCity" class="city xl"  onchange="Change()">
					</select>
                      <input type="hidden" id="hidCity" runat="server" value="选择市" />
					<select runat="server" id="ddlArea" class="dist xl"  onchange="Change()">        
					</select>
                    <input type="hidden" id="hidArea" runat="server" value="选择区" />
                            <input runat="server" style="width: 300px;" type="text" maxlength="100" class="box"
                                id="txtAddress" />
                            <i class="grayTxt">（常用收货地址）</i>
                        </td>
                    </tr>
                    <tr>
                        <td class="head"><span><i class="required">*</i></span>管理员登录帐号</td >
                       <td> <input runat="server"  type="text" maxlength="50" class="box" id="txtUsername" /></td>
                        <td class="head"> <span><i class="required">*</i></span>管理员手机号</td>
                       <td><input runat="server"  type="text" maxlength="11" class="box" id="txtUserPhone" /><a style="color:Red;" onclick="phoneedit()" href="#">(如需修改,请点击这里)</a></td>
                      </tr>
                      <tr>
                        <td class="head"><span><i class="required">*</i></span>管理员姓名</td >
                       <td colspan="3">  <input runat="server"  type="text" maxlength="50" class="box" id="txtUserTrueName" />  </td>
                      </tr>
                </tbody>
            </table>
        </div>
            <div class="mdBtn">
                </br><a href="#" id="btnupdate" runat="server" class="btnYe">确定修改</a></div>
            <div class="blank10">
            </div>
        </div>
        <!--修改资料 end-->
    </div>
    </div>

    </form>
</body>
</html>

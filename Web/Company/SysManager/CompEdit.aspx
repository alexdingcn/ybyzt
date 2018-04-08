<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompEdit.aspx.cs" Inherits="Company_SysManager_CompEdit"
    ValidateRequest="false" %>

<%@ Register TagPrefix="uc1" TagName="IndusTry" Src="~/Admin/UserControl/Tree_Industry.ascx" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>店铺信息</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/Enterprice.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/ajaxfileupload.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/OpenJs.js" type="text/javascript"></script>
    <link href="../../kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
    <link href="../../kindeditor/plugins/code/prettify.css" rel="stylesheet" type="text/css" />
    <script src="../../kindeditor/kindeditor.js" type="text/javascript"></script>
    <script src="../../kindeditor/lang/zh_CN.js" type="text/javascript"></script>
    <script src="../../js/xss.js"></script>
    <script src="../../kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
    <script>
        var laterIndex = 0;
        var ImgPath = '<%=ConfigurationManager.AppSettings["ImgViewPath"] %>';
        $(document).ready(function () {
            $_def.ID = "btnAdd";
            //add by hgh 
            //            if ('<%=Request["nextstep"] %>' == "1") {
            //                Orderclass("ktzxsc");
            //                document.getElementById("imgmenu").style.display = "block";
            //            }
<%--            $("#A1", ".verFile").click(function () {
                laterIndex = layerCommon.openWindow("企业LOGO", '../GoodsNew/Jcrop.aspx?Compid=<%=CompID%>&picfile=' + $("#HDCompPath").val(), '818px', '390px');
            });--%>

            $(".delImg2", ".e-banner").each(function (index, Control) {
                $(Control).data("ImgPath", $(Control).attr("tip"));
            });
            $(".e-banner").delegate(".delImg2", "click", function () {
                var ImgPathValue = "";
                $(this).parent().remove(),
                 $(".delImg2", ".e-banner").each(function (index, Control) {
                     $.trim(ImgPathValue) == "" ? (ImgPathValue = $(Control).data("ImgPath")) : (ImgPathValue += "," + $(Control).data("ImgPath") + "")
                 }), $("#HDBannerPath").val(ImgPathValue);
                ;
            })
        });
        var ImgPathValueone = "";

        function getFileName(o) {
            var pos = o.lastIndexOf("\\");
            return o.substring(pos + 1);
        }

        function CloseLayer() {
            layerCommon.layerClose(laterIndex);
        }
        function uploadAvatar2(obj, imgID, hidID, spanID) {
            var FileName = getFileName(obj.value.toString());
            $.ajaxFileUpload({
                type: 'post',
                url: "/Controller/CompImg.ashx?Compid=<%=KeyID%>&FlileID=" + obj.id + "",            //需要链接到服务器地址
                secureuri: false,
                fileElementId: obj.id,                        //文件选择框的id属性
                dataType: 'text',
                //服务器返回的格式，可以是json
                success: function (msg, status)            //相当于java中try语句块的用法
                {
                    if (msg == "0") {
                        layerCommon.msg("图片上传失败", IconOption.哭脸, 2500);
                        return false;
                    }
                    else {
                        var temp = '';
                        for (var i = 0; i < 4; i++) {
                            temp += parseInt(Math.random() * 10);
                        }
                        //                        var src = msg + "?temp=" + temp;
                        $("#" + imgID + "").attr("src", ImgPath + "CompImage/" + msg).css("display", "block");
                        if (imgID == "ImgCompLOGO") {
                            $("#ImgCompName").hide();
                        }
                        $("#" + hidID + "").val(msg);
                        $("#" + spanID + "").html(FileName);
                    }
                },
                error: function (msg, status, e) {
                    layerCommon.msg(msg + "," + status, IconOption.哭脸, 2500);
                    return false;
                }
            });
        }

        function formCheck() {
            var str = "";
            if ($.trim($("#txtAddress").val()) == "") {
                str = "详细地址不能为空。";
            }
            if (str == "") {
                if ($.trim($("#txtPhone").val()) != "") {
                    if (!IsMobile($.trim($("#txtPhone").val()))) {
                        str = "联系人手机不正确";
                    }
                }
            }
            if ($.trim($("#txtCustomInfo").val()) != "") {
                var html = filterXSS($.trim($("#txtCustomInfo").val()));
                $("#txtCustomInfo").val(html);
            }
            if (str != "") {
                layerCommon.msg(str, IconOption.哭脸, 2500);
                return false;
            }
        }

        //显示效果图
        function showPic(pic) {
            layerCommon.openWindow('效果图展示', "../GoodsNew/ShowPic.aspx?pic=../images/" + pic, '750px', '500px');
        }
    </script>
    <style>
        input[type=file]
        {
            left: -49px !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
    <input type="hidden" id="hid_Alert" />
    <uc1:CompRemove runat="server" ID="Remove" ModuleID="5" />
    
    <div class="rightinfo">
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="../SysManager/CompEdit.aspx?back=0" runat="server" id="btitle">店铺信息</a></li>
            </ul>
        </div>
        <div class="div_content">
            <table class="tb">
                <tbody>
                    <tr>
                        <td>
                            <span>联系人</span>
                        </td>
                        <td>
                            <input runat="server" type="text" onkeypress="KeyPress(event)" maxlength="50" class="textBox"
                                id="txtPrincipal" />
                        </td>
                        <td>
                            <span>固定电话</span>
                        </td>
                        <td>
                            <input runat="server" type="text" class="textBox" id="txtTel" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>联系人手机</span>
                        </td>
                        <td >
                            <input runat="server" type="text" maxlength="11" class="textBox" id="txtPhone" />
                        </td>
                          <td>
                            <span>传真</span>
                        </td>
                        <td>
                            <input runat="server" type="text" maxlength="50" class="textBox" id="txtFax" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span><i class="required">*</i>详细地址</span>
                        </td>
                        <td colspan="3">
                            <input runat="server" maxlength="50" style="width: 628px;" type="text" class="textBox"
                                id="txtAddress" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>企业LOGO
                                <br />
                                （企业标识、标志、徽标)</span>
                        </td>
                        <td colspan="3">
                            <div runat="server" id="ImgCompName">
                                <div runat="server" id="spImgName" style="min-width: 90px; max-width: 628px; border: 1px solid #ccc;
                                    margin: 5px 0 0 5px; text-align: center; color: #047dc6; font-size: 22px; padding: 0px 30px;
                                    line-height: 105px; display: inline-block;">
                                </div>
                            </div>
                            <img runat="server" style="margin: 5px 0 0 5px; width: 185px; height: 105px;"
                                id="ImgCompLOGO" />
                            <div class="teamR">
                                <div class="verFile" style="width: 125px; float: left;">
                                    <span class="verFileCon" style="margin-left: 5px; width:120px;">
                                        <input id="uploadCompLOGO" style="cursor: pointer;" runat="server" type="file" name="fileAttachment"
                                            onchange="uploadAvatar2(this,'ImgCompLOGO','HDCompPath','ScompImgName')" /></span>
                                    <a class="btn1" id="A1" style="margin-left: 5px; margin-top: 3px; text-decoration: NONE;"
                                        href="javascript:void(0)"><b class="L"></b><b class="R"></b>企业LOGO上传</a>
                                </div>
                                <span id="ScompImgName" style="text-align: left; padding-top: 3px; display: none;">
                                </span>
                                <div style="line-height: 37px;">
                                    <i class="grayTxt" runat="server" visible="false" id="CompLogoI">(标准版本，可自行修改)</i>
                                    <i style="color: #999; padding: 3px 5px 0 0px; display: inline-block;">企业LOGO默认大小：185*105</i>
                                    <label style="color: #aaaaaa">
                                        (例如：<a href="javascript:;" onclick="showPic('CompLOGO.png')" style="color: #aaaaaa;
                                            cursor: pointer;">点击查看</a>)</label>
                                </div>
                            </div>
                            <input type="hidden" id="HDCompPath" runat="server" />
                        </td>
                    </tr>
                       <tr>
                        <td>
                            <span>店铺搜索关键字</span>
                        </td>
                        <td colspan="3">
                            <input runat="server" maxlength="100" style="width: 628px;" type="text" class="textBox"
                                id="BrandIfon" />
                        </td>
                    </tr>

                    <tr class="newspan">
                        <td>
                            <span>企业详细介绍 &nbsp; </span>
                        </td>
                        <script>
                            KindEditor.ready(function (K) {
                                window.editor = K.create('#txtCustomInfo', {
                                    uploadJson: '../../Kindeditor/asp.net/upload_json.ashx',
                                    fileManagerJson: '../../Kindeditor/asp.net/file_manager_json.ashx',
                                    allowFileManager: true
                                });
                            });
                        </script>
                        <td colspan="3">
                            <asp:TextBox ID="txtCustomInfo" runat="server" TextMode="MultiLine" Height="270px"
                                Width="600px" class="textBox"></asp:TextBox>
                            <label style="color: #aaaaaa">
                                (例如：<a href="javascript:;" onclick="showPic('CustomInfo.png')" style="color: #aaaaaa;
                                    cursor: pointer;">点击查看</a>)</label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="div_footer">
            
            <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return formCheck()"
                OnClick="btnSave_Click" />&nbsp;
            <input name="" runat="server" id="btnback" type="button" class="cancel" onclick="javascript:history.go(-1);"
                value="返回" /> &nbsp; 
            <a target="_blank" href="/<%=CompID%>.html">商城预览</a>
        </div>
    </div>
    </form>
</body>
</html>

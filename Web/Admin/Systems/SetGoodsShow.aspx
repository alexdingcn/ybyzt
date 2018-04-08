<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SetGoodsShow.aspx.cs" Inherits="Admin_SetGoodsShow" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>设置商品是否首页显示</title>
     <link href="../../Company/css/Enterprice.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/ajaxfileupload.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $("input:radio").on("click", function () {
                $(this).val() == "1" ? function () {
                    $("tr.tdShow").show();
                } () : function () {
                    $("tr.tdShow").hide();
                } ();
            });
            $("input:checked").val() == "0" && function () {
                $("tr.tdShow").hide();
            } ();


        })

        var ImgPath = '<%=ConfigurationManager.AppSettings["ImgViewPath"] %>';
        function uploadAvatar2(obj, imgID, hidID, spanID) {
            var str = obj.value, Suffix = $.trim(str.substring(str.lastIndexOf(".")));
            if (Suffix != ".gif" && Suffix != ".jpeg" && Suffix != ".jpg" && Suffix != ".png") {
                errMsgMo('提示', "请上传图片类型");
                return;
            }
            $.ajaxFileUpload({
                type: 'post',
                url: "/Controller/CompImg.ashx?Compid=<%=KeyID%>&FlileID=" + obj.id + "",            //需要链接到服务器地址
                data: { Fileype: "GFirst" },
                secureuri: false,
                fileElementId: obj.id,                        //文件选择框的id属性
                dataType: 'text',
                //服务器返回的格式，可以是json
                success: function (msg, status)            //相当于java中try语句块的用法
                {
                    if (msg == "0") {
                        errMsg("提示", "图片上传失败", "", "");
                        return false;
                    }
                    else {
                        $("#" + imgID + "").attr("src", ImgPath + "CompImage/" + msg).show();
                        $("#" + hidID + "").val(msg);
                    }
                },
                error: function (msg, status, e) {
                    errMsg("提示", msg + "," + status, "", "");
                    return false;
                }
            });
        }
    </script>
</head>
<body style=" overflow:hidden;">
    <form id="form1" runat="server">
    <%--<uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />--%>
    <%--<div class="m-con">--%>
       <div class="div_content">
          <table class="tb" style=" max-width:500px">
                <tbody>

                    <tr>
                    <td width="20%"> <span>商品名称</span></td>
                     <td>
                       <label id="lblGoodsName" runat="server"></label>
                     </td>
                    </tr>
                    <tr>
                    <td width="20%"> <span>是否首页显示</span></td>
                     <td>
                        &nbsp;
                        <input type="radio" runat="server" name="IsShow" value="1" id="rdShowYes" />显示 &nbsp;&nbsp;&nbsp;
                        <input runat="server" id="rdShowNo" checked name="IsShow" type="radio" value="0" />不显示
                     </td>
                    </tr>
                    <tr class="tdShow">
                    <td> <span>首页显示顺序</span></td>
                     <td>
                         <input runat="server" type="text"  onkeyup="KeyInt(this)" maxlength="10" class="textBox" autocomplete="off" id="txt_SortIndex" />
                            <i class="grayTxt">（数值越大显示越靠前）</i>
                     </td>
                    </tr>
                     <tr class="tdShow">
                    <td> <span>首页显示名称</span></td>
                     <td>
                       <input runat="server" type="text"   maxlength="100" class="textBox" autocomplete="off" id="txt_ShowName" />
                     </td>
                    </tr>
                    <tr class="tdShow">
                    <td> <span>首页显示图片</span></td>
                     <td>
                        <img  runat="server" style=" margin:5px 0 0 5px; display:none;  width:140px; height:75px;"  id="ImgNewPic" />

                         <div class="teamR" >
                             <div class="verFile" style=" width:145px; float:left;" >
                             <span class="verFileCon" style=" margin-left:5px;width:100%; height:30px;"> <input id="uploadNewPic" style="cursor:pointer;font-size:1000px; width:300px;" runat="server" type="file"  name="uploadNewPic"  onchange="uploadAvatar2(this,'ImgNewPic','HdNewPicName','ScNewPicName')"/></span>
                             <a class="btn1" id="A3"  style="margin-left:5px; margin-top :3px; text-decoration: NONE; " href="javascript:void(0)"><b class="L"></b><b class="R"></b>首页显示图片上传</a>
                             </div>
                                <span id="ScNewPicName" style="text-align:left; padding-top:3px; display:none;"></span>
                                <div style=" line-height:37px;">
                                </div>
                                </div>
                            <input type="hidden" id="HdNewPicName" runat="server" />
                           
                     </td>
                    </tr>


                </tbody>
            </table>
        </div>
    <%--</div>--%>

    <div class="div_footer" style=" max-width:500px;position:fixed;top: 250px;left: 200px;">
        <input type="button" runat="server" id="btnSubMit" class="orangeBtn"  value="确定"  onserverclick="btnSubMit_Click" />
        
    </div>
    </form>
</body>
</html>

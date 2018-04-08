<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecommendGoodsPic.aspx.cs"
    Inherits="Company_ShopManager_RecommendGoodsPic" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>主推图片维护</title>
    <%--<link href="../../css/layer.css" rel="stylesheet" type="text/css" />--%>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/ajaxfileupload.js" type="text/javascript"></script>
    <%--    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>--%>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
    <div class="rightinfo">
    <div class="place">
        <ul class="placeul">
            <li><a href="../index.aspx" >我的桌面</a></li><li>></li>
            <li><a href="RecommendGoodsPic.aspx" runat="server" id="atitle">主推图片维护</a></li>
        </ul>
    </div>

    <div class="adbox">
        <div class="adImg" style="margin-top:82px;">
            <img src="../../Images/Goods400x400.jpg" id="imgAvatar1" runat="server" class="imgWrap"
                style="cursor: pointer; width: 410px; height: 450px;" alt="图片" />
            <input type="file" id="upLoadImg1" name="upLoadImg1" onchange="uploadAvatar(this,1)" style="width: 410px;
                height: 450px; cursor: pointer; float: left; margin: -450px 5px 0 0; opacity: 0;
                filter: alpha(opacity=0); font-size: 100px" />
            <input type="hidden" id="hrImgPath1" name="hrImgPath" class="hrImgPath" value='<%=pic1 %>' />
            <div class="txt">
                最佳尺寸（410*450）
                <a class="delImg" tip="<%=id1 %>" style="color:red;" href="JavaScript:;">删除</a>            
            </div>
            <div class="link"><%--URL：<input type="text" id="txtGoodsId1" class="txtGoodsId textBox" name="txtGoodsId" value='<%=goodsId1 %>' onkeyup="KeyInt(this)" style="width: 355px"/>--%>
            <input type="text" id="txtUrl1" class="txtUrl  textBox" name="txtUrl" value='<%= string.IsNullOrEmpty(url1)?"请输入商品网址":url1 %>' /></div>
        </div>
        <ul class="adImg2">
            <li>
                <img src="../../Images/Goods400x400.jpg" id="imgAvatar2" runat="server" class="imgWrap"
                    style="cursor: pointer; width: 180px; height: 220px;" alt="图片" />
                <input type="file" id="upLoadImg2" name="upLoadImg2" onchange="uploadAvatar(this,2)" style="width: 180px;
                    height: 220px; cursor: pointer; float: left; margin: -220px 5px 0 0px; opacity: 0;
                    filter: alpha(opacity=0); font-size: 40px" />
                <input id="hrImgPath2" type="hidden" name="hrImgPath" class="hrImgPath" value='<%=pic2 %>' />
                <div class="txt">最佳尺寸（180*220）
                    <a class="delImg" tip="<%=id2 %>" style="color:red;" href="JavaScript:;">删除</a>
                </div>
               	<div class="link"><%--URL：<input type="text" id="txtGoodsId2" name="txtGoodsId"class="txtGoodsId  textBox" onkeyup="KeyInt(this)" value='<%=goodsId2 %>' style="width: 130px" />--%>
                <input type="text" id="txtUrl2" class="txtUrl textBox" value='<%= string.IsNullOrEmpty(url2)?"请输入商品网址":url2 %>' name="txtUrl"  <%--value='<%=url2 %>'--%> /></div>
             </li>
            <li>
                <img src="../../Images/Goods400x400.jpg" id="imgAvatar3" runat="server" class="imgWrap"
                    style="cursor: pointer; width: 180px; height: 220px;" alt="图片" />
                <input type="file" id="upLoadImg3" name="upLoadImg3" onchange="uploadAvatar(this,3)" style="width: 180px;
                    height: 220px; cursor: pointer; float: left; margin: -220px 5px 0 0; opacity: 0;
                    filter: alpha(opacity=0); font-size: 40px" />
                <input id="hrImgPath3" type="hidden" name="hrImgPath" class="hrImgPath" value='<%=pic3 %>' />
               	<div class="txt">
                    最佳尺寸（180*220）
                    <a class="delImg" tip="<%=id3 %>" style="color:red;" href="JavaScript:;">删除</a>
                </div>
				<div class="link"><%--URL：<input type="text" id="txtGoodsId3" name="txtGoodsId"class="txtGoodsId textBox" onkeyup="KeyInt(this)" value='<%=goodsId3 %>' style="width: 130px"/>--%>
                <input type="text" id="txtUrl3" class="txtUrl textBox" name="txtUrl" value='<%= string.IsNullOrEmpty(url3)?"请输入商品网址":url3 %>' <%--value='<%=url3 %>' --%>/></div>
             </li>
                
            <li style="margin-top:10px;">
                <img src="../../Images/Goods400x400.jpg" id="imgAvatar4" runat="server" class="imgWrap"
                    style="cursor: pointer; width: 180px; height: 220px;" alt="图片" />
                <input type="file" id="upLoadImg4" name="upLoadImg4" onchange="uploadAvatar(this,4)" style="width: 180px;
                    height: 220px; cursor: pointer; float: left; margin: -220px 5px 0 0; opacity: 0;
                    filter: alpha(opacity=0); font-size: 40px" />
                <input id="hrImgPath4" type="hidden" name="hrImgPath" class="hrImgPath" value='<%=pic4 %>' />
               <div class="txt">
                    最佳尺寸（180*220）
                    <a class="delImg" tip="<%=id4 %>" style="color:red;" href="JavaScript:;">删除</a>
               </div>
               <div class="link"><%--URL：<input type="text" id="txtGoodsId4" name="txtGoodsId"class="txtGoodsId textBox" onkeyup="KeyInt(this)" value='<%=goodsId4 %>' style="width: 130px"/>--%>
                <input type="text" id="txtUrl4" class="txtUrl textBox" name="txtUrl" value='<%= string.IsNullOrEmpty(url4)?"请输入商品网址":url4 %>' <%-- value='<%=url4 %>'--%> /></div>
            </li>
            <li style="margin-top:10px;">
                <img src="../../Images/Goods400x400.jpg" id="imgAvatar5" runat="server" class="imgWrap"
                    style="cursor: pointer; width: 180px; height: 220px;" alt="图片" />
                <input type="file" id="upLoadImg5" name="upLoadImg5" onchange="uploadAvatar(this,5)" style="width: 180px;
                    height: 220px; cursor: pointer; float: left; margin: -220px 5px 0 0; opacity: 0;
                    filter: alpha(opacity=0); font-size: 40px" />
                <input id="hrImgPath5" type="hidden" name="hrImgPath" class="hrImgPath" value='<%=pic5 %>' />
                <div class="txt">
                    最佳尺寸（180*220）
                    <a class="delImg" tip="<%=id5 %>" style="color:red;" href="JavaScript:;">删除</a>
                </div>
                <div class="link"><%--URL：<input type="text" id="txtGoodsId5" name="txtGoodsId" class="txtGoodsId textBox" onkeyup="KeyInt(this)" value='<%=goodsId5 %>' style="width: 130px"/>--%>
                <input type="text" id="txtUrl5" class="txtUrl textBox" name="txtUrl" value='<%= string.IsNullOrEmpty(url5)?"请输入商品网址":url5 %>' <%--value='<%=url5 %>'--%> /></div>
             </li>
        </ul>
        <div class="blank10"></div>
       <div class="zytext"><strong style="color:red;">注：</strong><br /><i> 1、商品图片网址必须填写，网址跳转指定的商品详情页面地址</i><br /><i>2、举例说明效果图（<a href="javascript:;"style="color: Blue;" onclick="showPic('../images/showUrl.png')">点击查看</a>）</i></i></div>
        <div style=" width:800px; text-align:center;">
            <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return formCheck()" OnClick="btnAdd_Click" />
        </div>
    </div>
    </div>
    <script>
        $(function () {
            $(".txtUrl").focus(function () {
                var url = $.trim($(this).val());
                if (url == "请输入商品网址") {
                    $(this).val("");
                }
            })
            $(".txtUrl").blur(function () {
                var url = $.trim($(this).val());
                if (url == "") {
                    $(this).val("请输入商品网址");
                }
            })
        })

        $(document).on("click", ".delImg", function () {
            var tip = $(this).attr("tip");

            if (tip == "")
                return;

            var th = this;
            $.ajax({
                type: 'post',
                url: '../ShopManager/RecommendGoodsPic.aspx?action=del',
                data: { tip: tip },
                async: false, //true:同步 false:异步
                success: function (result) {
                    var data = eval('(' + result + ')');
                    var falg = data["ds"];
                    if (falg == "True") {
                        $(th).attr("tip", "");
                        $(th).parents().siblings("img").attr("src", "../../Images/Goods400x400.jpg");
                        $(th).parents().siblings("div[class=\"link\"]").find("input[type=\"text\"]").val("请输入商品网址");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    parent.layerCommon.msg("请求错误或超时,请重试", parent.IconOption.错误);
                }
            });
        });

        //显示效果图
        function showPic(pic) {
            var height = document.documentElement.clientHeight; //计算高度
            var layerOffsetY = (height - 340) / 2; //计算宽度
            var wd = "625px";
            var hi = "435px";
            var index = layerCommon.openWindow('效果图展示', "../GoodsNew/ShowPic.aspx?pic=../images/" + pic, wd, hi);  //记录弹出对象
        }
        //验证
        function formCheck() {
            var bol = false;
            $(".hrImgPath").each(function (index, obj) {
                if ($(".hrImgPath").eq(index).val() == "") {
                    layerCommon.msg("请上传图片", IconOption.错误);
                    bol = true;
                    return false;
                }
            })
            if (bol) {
                return false;
            }
            var goodsIds = $.trim($(".txtUrl").val());
            if ($.trim(goodsIds)!= "请输入商品网址") {
//                layerCommon.msg("请填写URL", IconOption.错误);
//                return false;
//            } else {
                if (goodsIds.indexOf("?") == -1 || goodsIds.indexOf("&") == -1 || goodsIds.indexOf("=") == -1 || (goodsIds.indexOf("Eshop") == -1 && goodsIds.indexOf("eshop") == -1 && goodsIds.indexOf("EShop") == -1)) {
                    layerCommon.msg("填写的URL有误,不能连接到正确的商品详细页面", IconOption.错误);
                    return false;
                }
            }
            return true;
        }
        //图片上传uploadAvatar
        function uploadAvatar(obj,id) {
            $.ajaxFileUpload(
        {
            type: "post",
            url: "../../Handler/HandleImg3.ashx?FlileID=" + obj.id,            //需要链接到服务器地址
            secureuri: false,
            fileElementId:  obj.id,                        //文件选择框的id属性
            dataType: "text",
            //服务器返回的格式，可以是json
            success: function (msg, status)            //相当于java中try语句块的用法
            {
                if (msg == "0") {
                    layerCommon.msg("图片上传失败", IconOption.错误);
                    return false;
                } else if (msg == "1") {
                    layerCommon.msg("只能上传2M以下的图片", IconOption.错误);
                    return false;
                } else {
                    var temp = '';
                    for (var i = 0; i < 4; i++) {
                        temp += parseInt(Math.random() * 10);
                    }
                    var jdata = eval('(' + msg + ')');
                    var src = jdata.msg + "?temp=" + temp;
                    $("#imgAvatar" + id).attr("src", "Goodsimages/" + src);
                    $("#hrImgPath" + id).val(jdata.msg);
                    return true;
                }
            },
            error: function (msg, status, e)            //相当于java中catch语句块的用法
            {
                layerCommon.msg(msg + "," + status, IconOption.错误);
                return false;
            }
        }
    )
        }
    </script>
    </form>
</body>
</html>

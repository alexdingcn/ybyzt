<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoodsSpace.aspx.cs" Inherits="Company_GoodsNew_GoodsSpace" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>图片空间</title>
    <link rel="stylesheet" href="//g.alicdn.com/sj/dpl/1.4.3/css/sui.min.css">
    <link rel="stylesheet" href="//g.alicdn.com/cm/media/1.0.5/css/app.css" type="text/css"
        media="screen" charset="utf-8">
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/ajaxfileupload.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <style>
        .mod-container .search-result
        {
            -moz-border-bottom-colors: none;
            -moz-border-left-colors: none;
            -moz-border-right-colors: none;
            -moz-border-top-colors: none;
            background: #fff none repeat scroll 0 0;
            border-color: -moz-use-text-color #ccc #ccc;
            border-image: none;
            border-style: none solid solid;
            border-width: 0 1px 1px;
            max-height: 180px;
            overflow-x: hidden;
            overflow-y: auto;
        }
        .mod-container.list .search-result
        {
            max-height: 180px;
        }
        .mod-img
        {
            margin: 5px 0 5px 5px;
        }
        .mod-img img
        {
           width:80px;
           height:80px;
        }
    </style>
    <script>
        $(function () {
            //上传新图片
            $(".upload-tab-title").click(function () {
                $(".area-tab-title").removeClass("active");
                $(this).addClass("active");
                $(".area-container").addClass("none");
                $(".upload-container").removeClass("none");
            })
            //从空间选择
            $(".area-tab-title").click(function () {
                $(".upload-tab-title").removeClass("active");
                $(this).addClass("active");
                $(".upload-container").addClass("none");
                $(".area-container").removeClass("none");
            })
            //大图
            $(".category-thumb").click(function () {
                $(this).addClass("selected");
                $(".category-list").removeClass("selected");
                $(".mod-list-title").addClass("none");
                $(".search-result li").removeClass("list");
                $(".mod-container").removeClass("list");
                     Bind($.trim($("#keyword").val()),1);
            })
            //列表
            $(".category-list").click(function () {
                $(this).addClass("selected");
                $(".category-thumb").removeClass("selected");
                $(".mod-list-title").removeClass("none");
                $(".search-result li").addClass("list");
                $(".mod-container").addClass("list");
                     Bind($.trim($("#keyword").val()),2);
            })
            //模糊查询
            $("#keyword").keyup(function () {
              if($(".mod-category a").eq(0).attr("class").indexOf("selected")==-1){
                 Bind($.trim($(this).val()),2);
              }else
              {
                 Bind($.trim($(this).val()),1);
              }
            })
            //列表li选中
            $(document).on("click",".search-result li",function(){
                $(this).addClass("selected").siblings().removeClass("selected");
                var $this=this;
                $.ajax({
                type:"post",
                url: "../../Handler/HandleImg4.ashx",
                data:{ck:Math.random(),upLoadImg:"/"+$($this).find("a").attr("title") },
                dataType:"text",
                success:function(data){
                    if(data=="0"){
                       layerCommon.msg("图片上传失败", IconOption.错误);
                        return false;
                    }else
                    {
                var count = window.parent.$(".ImgList p").length;
               count++;
               window.parent.$(".ImgList").append("<div><p  draggable=\"true\" style=\"margin:0 5px 5px 0; float: left; cursor: move;\" class=\"p" + count + "\"><img  src=\"" +$($this).find("img").attr("src") + "\" id=\"img" + count + "\" width=\"150\" height=\"150\" class=\"imgWrap\"  alt=\"图片\" /></p><a href=\"JavaScript:;\" class=\"delImg\" tip=\"" + data  + "\" style=\"color:red; cursor: pointer; float: left; margin: 120px 0 0 -90px;display:none;\">删除</a><input type=\"hidden\" name=\"hidImg\" value=\"" + data  + "\" id=\"hidImg" + count + "\" /></div>");
               if(count>=5){
                    window.parent.$(".hint-tip").css("top","824px");
               }else
               {
                    window.parent.$(".hint-tip").css("top","669px");
               }
               if (count == 1) {
                        window.parent.$("#hrImgPath").val(data);
                    } else if (count >= 10) {   
                        window.parent.$(".AddImg").hide();
                        window.parent.$(".hint-tip").hide();
                    } else {
                        window.parent.$(".AddImg").show();
                    }
                    }
                },error:function(){
                    layerCommon.msg("图片上传出错了", IconOption.错误);
                            return false;
                    }
                })
            })
            //排序下拉条件搜素
            $("#order").change(function(){
                  if($(".mod-category a").eq(0).attr("class").indexOf("selected")==-1){
                     Bind($.trim($("#keyword").val()),2);
                  }else
                  {
                     Bind($.trim($("#keyword").val()),1);
                  }
            })
            //上传图片
            $("#pickfiles").click(function(){
            $("#upLoadImg").click();
            })
        })
        //can 模糊查询的值 can2 1大图2列表
        function Bind(can,can2) { 
         $(".search-result").html("");
         $.ajax({
                    type: "post",
                    data: { ck: Math.random(), action: "mohu", name:can ,soft:$("#order option:selected").val()},
                    dataType: "json",
                    success: function (data) {
                        var html = "";
                        $(data).each(function (index, obj) {
                        if(can2==1){
                            html += "<li class=\"mod-img\"><a title=\"" + obj.Name + "\"><img src=\"<%=Common.GetWebConfigKey("ImgViewPath") %>/PicSpace/<%=this.CompID %>/"+obj.Name+"\"><i class=\"icon-selected media-iconfont media-iconfont-selected\"></i><span class=\"pixel\">" + obj.Pixel + "</span></a></li>"
                            }else
                            {
                            html+="<li  class=\"mod-img list\"><a title=\"" + obj.Name + "\"><img src=\"<%=Common.GetWebConfigKey("ImgViewPath") %>/PicSpace/<%=this.CompID %>/"+obj.Name+"\"><span class=\"file-preview\"><img src=\"<%=Common.GetWebConfigKey("ImgViewPath") %>/PicSpace/<%=this.CompID %>/"+obj.Name+"\"></span></a><span class=\"file-name\">" + obj.Name + "</span><span class=\"file-size\">" + obj.Size + "KB</span><span class=\"file-checkbox\">"+obj.Time+"</span><i class=\"icon-selected media-iconfont media-iconfont-selected\"></i></li>";
                            }
                        })
                        $(".search-result").html(html);
                    }, error: function () {
                        $(".search-result").html("");
                        return false;
                    }
                })
        }
         //图片上传uploadAvatar
        function uploadAvatar(ele) {
           var ua = navigator.userAgent.toLowerCase(); //浏览器信息
    var info = {
        ie: /msie/.test(ua) && !/opera/.test(ua),        //匹配IE浏览器    
        op: /opera/.test(ua),     //匹配Opera浏览器    
        sa: /version.*safari/.test(ua),     //匹配Safari浏览器    
        ch: /chrome/.test(ua),     //匹配Chrome浏览器    
        ff: /gecko/.test(ua) && !/webkit/.test(ua)     //匹配Firefox浏览器
    };
    if (!info.ie) {
      if (ele.files[0].size > 3 * 1024 * 1024) {
                layerCommon.msg("只能上传3M以下的图片", IconOption.错误);
                return false;
            }
            }
            $.ajaxFileUpload(
        {
            type: "post",
            url: "../../Handler/HandleImg2.ashx",            //需要链接到服务器地址
            secureuri: false,
            fileElementId: "upLoadImg",                        //文件选择框的id属性
            dataType: "text",
            //服务器返回的格式，可以是json
            success: function (msg, status)            //相当于java中try语句块的用法
            {
                if (msg == "0") {
                         layerCommon.msg("图片上传失败", IconOption.错误);
                    return false;
                } else if (msg == "1") {
                         layerCommon.msg("只能上传3M以下的图片", IconOption.错误);
                    return false;
                }else if(msg=="2"){
                       layerCommon.msg("只能上传jpg、jpeg、png格式图片", IconOption.错误);
                    return false;
                } else {
                    var temp = '';
                    for (var i = 0; i < 4; i++) {
                        temp += parseInt(Math.random() * 10);
                    }
                    var src = msg + "?temp=" + temp;
//                    var count = $(".ImgList p").length;
//                    count++;
//                    // $("#imgAvatar").attr("src", '<%= Common.GetWebConfigKey("ImgViewPath") %>' + "GoodsImg/" + src);
//                    $(".ImgList").append("<div><p  draggable=\"true\" style=\"margin:0 5px 5px 0; float: left; cursor: move;\" class=\"p" + count + "\"><img  src=\"" + '<%= Common.GetWebConfigKey("ImgViewPath") %>' + "GoodsImg/" + src + "\" id=\"img" + count + "\" width=\"150\" height=\"150\" class=\"imgWrap\"  alt=\"图片\" /></p><a href=\"JavaScript:;\" class=\"delImg\" tip=\"" + msg + "\" style=\"color:red; cursor: pointer; float: left; margin: 120px 0 0 -90px;display:none;\">删除</a><input type=\"hidden\" name=\"hidImg\" value=\"" + msg + "\" id=\"hidImg" + count + "\" /></div>");
//                    if (count == 1) {
//                        $("#hrImgPath").val(msg);
//                    } else if (count >= 10) {
//                        $(".AddImg").hide();
//                    } else {
//                        $(".AddImg").show();
//                    }
                  var count = window.parent.$(".ImgList p").length;
               count++;
               window.parent.$(".ImgList").append("<div><p  draggable=\"true\" style=\"margin:0 5px 5px 0; float: left; cursor: move;\" class=\"p" + count + "\"><img  src=\"" + '<%= Common.GetPicBaseUrl() %>' + src + "?x-oss-process=style/resize200\" id=\"img" + count + "\" width=\"150\" height=\"150\" class=\"imgWrap\"  alt=\"图片\" /></p><a href=\"JavaScript:;\" class=\"delImg\" tip=\"" + msg + "\" style=\"color:red; cursor: pointer; float: left; margin: 120px 0 0 -90px;display:none;\">删除</a><input type=\"hidden\" name=\"hidImg\" value=\"" + msg + "\" id=\"hidImg" + count + "\" /></div>");
               if(count>=5){
                    window.parent.$(".hint-tip").css("top","824px");
               }else
               {
                    window.parent.$(".hint-tip").css("top","669px");
               }
               if (count == 1) {
                        window.parent.$("#hrImgPath").val($(this).find("a").attr("title"));
                    } else if (count >= 10) {   
                        window.parent.$(".AddImg").hide();
                        window.parent.$(".hint-tip").hide();
                    } else {
                        window.parent.$(".AddImg").show();
                    }
                      Bind($.trim($("#keyword").val()),1);
                    return true;
                }
            },
            error: function (msg, status, e)            //相当于java中catch语句块的用法
            {
                 layerCommon.msg(msg + "," + status, IconOption.错误);
                return false;
            }
        })
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="app-page list" style="margin-left: 16px;">
        <div class="sui-msg msg-stop size-restrict none">
            <div class="msg-con">
            </div>
            <s class="msg-icon"></s>
        </div>
        <ul class="sui-nav nav-tabs nav-primary mod-tabs">
            <li class="upload-tab-title"><a href="javascript:;" onclick="">上传新图片</a></li>
            <li class="area-tab-title active file-tab"><a href="javascript:;" onclick="">从图片空间选择</a></li>
        </ul>
        <div class="tab-content">
            <div class="upload-container none" style="min-height: 250px;">
                <div class="upload-table-container" style="height: auto;">
                    <i class="media-iconfont media-iconfont-upload"></i>
                    <p style="position: relative;">
                        <a id="pickfiles" class="sui-btn btn-xlarge" href="javascript:void(0);" style="position: relative;
                            z-index: 1;">点击上传</a>
                        <input type="file" id="upLoadImg" name="upLoadImg" style="width: 150px; height: 150px;
                            font-size: 100px; cursor: pointer; float: left; margin: 0px 5px 5px -150px; opacity: 0;
                            filter: alpha(opacity=0); display: none;" onchange="uploadAvatar(this);" />
                    </p>
                    <p class="info">
                        提示：仅支持<strong>JPG、JPEG、PNG</strong>格式；<br>
                        建议上传无线详情图片宽度<strong>750px</strong>以上，效果更佳
                    </p>
                </div>
            </div>
            <div class="area-container" style="min-height: 250px;">
                <div class="mod-container" style="margin-left: 15px;">
                    <div class="search-container">
                        <span class="mod-category"><a class="category-thumb media-iconfont media-iconfont-thumb selected"
                            data-type="thumb" href="javascript:;" title=""></a><a class="category-list media-iconfont media-iconfont-list"
                                data-type="list" href="javascript:;" title=""></a></span>
                        <select id="order" name="order" style=" height:auto;">
                            <option value="1" selected="selected">按时间从晚到早</option>
                            <option value="2">按时间从早到晚</option>
                            <option value="7">按图片从大到小序</option>
                            <option value="8">按图片从小到大序</option>
                            <option value="6">按图片名升序</option>
                            <option value="5">按图片名降序</option>
                        </select>
                        <input name="keyword" id="keyword" type="text" style=" height:auto;">
                    </div>
                    <ul class="mod-list-title none">
                        <li class="mod-img list"><a>预览</a> <span class="file-name">文件名</span> <span class="file-size">
                            大小</span> <span class="file-checkbox">选择</span> </li>
                    </ul>
                    <ul class="search-result">
                        <asp:Repeater ID="rptImg" runat="server">
                            <ItemTemplate>
                                <li class="mod-img"><a title="<%# Eval("Name")%>">
                                    <img src="<%# Common.GetWebConfigKey("ImgViewPath")+"PicSpace/"+this.CompID+"/" +Eval("Name")%>"><i
                                        class="icon-selected media-iconfont media-iconfont-selected"></i><span class="pixel"><%#Eval("Pixel") %></span></a></li></ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

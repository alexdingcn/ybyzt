<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PicSpaceList.aspx.cs" Inherits="Company_GoodsNew_PicSpaceList" %>

<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>图片空间</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/ImgAmplify.js" type="text/javascript"></script>
    <script src="../../js/MoveLayer.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <link href="../css/picspace/common.css" rel="stylesheet" type="text/css" />
    <link href="../css/picspace/manage.css" rel="stylesheet" type="text/css" />
    <script>

        $(function () {
            //选中图片
            $(document).on("click", "#J_Picture >.item", function () {
                if($(this).attr("class").toString().indexOf("ui-selected")!=-1){
                   $(this).removeClass("ui-selected");
              
                }else
                {
                   $(this).addClass("ui-selected");
                
                }
                var str="";
                 $("#J_Picture .item").each(function(index,obj){
                     if($(this).attr("class").toString().indexOf("ui-selected")!=-1){
                        str=$(this).find("input[type='text']").val();
                        return false;
                     }
                  })
                  if(str!=""){
                      $(".delete").css("display","list-item");
                  }else
                  {
                      $(".delete").css("display","none");
                  }
            })
            //全选文字
            $(".selected-msg").click(function () {
                if ($("#J_SelectAll").prop("checked") == undefined) {
                    $("#J_SelectAll").prop("checked", true);
                    Checked(1);
                } else {
                    if ($("#J_SelectAll").prop("checked")) {
                        $("#J_SelectAll").prop("checked", false);
                        Checked(2);
                    } else {
                        $("#J_SelectAll").prop("checked", true);
                        Checked(1);
                    }
                }
            })
              //全选
             $("#J_SelectAll").click(function () {
                    if ($(this).prop("checked")) {
                        $(this).prop("checked", true);
                        Checked(1);
                    } else {
                        $(this).prop("checked", false);
                        Checked(2);
                    }
            })
            //模糊查询
            $(".keyword").keyup(function () {
             if($("#J_ShowPic").css("background-color").toString().indexOf("rgb(204")!=-1){
                    Bind($.trim($(this).val()), 1);
                }else
                {
                    Bind($.trim($(this).val()), 2);
                }
            })
            //列表
            $("#J_ShowList").click(function(){
                $(this).css("background-color","rgb(204, 204, 204)");
                $("#J_ShowPic").css("background-color","#fff");
                $(".pic-container").addClass("list-show");
            })
            //大图
            $("#J_ShowPic").click(function(){
                $(this).css("background-color","rgb(204, 204, 204)");
                $("#J_ShowList").css("background-color","#fff");
                  $(".pic-container").removeClass("list-show");
            })
            //删除
            $(".delete").click(function(){
                 layerCommon.confirm('确定删除选中图片', function () {
                 var str="";
                        $("#J_Picture .item").each(function(index,obj){
                           if($(this).attr("class").toString().indexOf("ui-selected")!=-1){
                              str+=$(this).find("input[type='text']").val()+",";
                           }
                        })
                        if(str!=""){
                        str=str.substring(0,str.length-1);
                        }
                              $.ajax({
                                type: "post",
                                data: { ck: Math.random(), action: "delImg", filepath:str},
                                dataType: "text",
                                async: false,
                                success: function (data) {
                                    if(data=="cg"){
                                        if($("#J_ShowPic").css("background-color").toString().indexOf("rgb(204")!=-1){
                                            Bind($.trim($(".keyword").val()), 1);
                                        }else
                                        {
                                            Bind($.trim($(".keyword").val()), 2);
                                        }
                                         $(".delete").hide();
                                    }else
                                    {
                                           layerCommon.msg("删除图片失败", IconOption.哭脸);
                                           return false;
                                    }
                                }
                                })
                 });
            })
      
            //排序
            $(".dropdown-menu a").click(function(){
                if($(this).attr("class").indexOf("down")!=-1){
                    $(".dropdown-toggle").addClass("down");
                    $(".dropdown-toggle").removeClass("up");
                }else
                {
                    $(".dropdown-toggle").removeClass("down");
                    $(".dropdown-toggle").addClass("up");
                }
                    $(".dropdown-toggle").text($(this).text());
                  if($("#J_ShowPic").css("background-color").toString().indexOf("rgb(204")!=-1){
                    Bind($.trim($(".keyword").val()), 1);
                }else
                {
                    Bind($.trim($(".keyword").val()), 2);
                }
            })
            //上传图片
            $(".btn-primary").click(function(){
                location.href="PicSpaceEdit.aspx";
            })
        })
        //全选、反选
        function Checked(can) {
            if (can == 1) {
                $("#J_Picture .item").addClass("ui-selected");
                $(".delete").css("display","list-item");
            } else {
                $("#J_Picture .item").removeClass("ui-selected");
                 $(".delete").css("display","none");
            }
        }
         //can 模糊查询的值 can2 1大图2列表
        function Bind(can,can2) { 
            if(can2==1){
                $(".pic-container").removeClass("list-show");
            }else
            {
                $(".pic-container").addClass("list-show");
            }
         $("#J_Picture").html("<div class=\"list-head clearfix\"><div class=\"span1\">名称</div><div class=\"span2\">类型</div><div class=\"span2\">尺寸</div><div class=\"span2\">大小</div><div class=\"span2\">更新时间</div></div>");
         $.ajax({
                    type: "post",
                    data: { ck: Math.random(), action: "mohu", name:can ,soft:$(".dropdown-toggle").text(),soft2:$(".dropdown-toggle").attr("class")},
                    dataType: "json",
                    success: function (data) {
                        var html = "<div class=\"list-head clearfix\"><div class=\"span1\">名称</div><div class=\"span2\">类型</div><div class=\"span2\">尺寸</div><div class=\"span2\">大小</div><div class=\"span2\">更新时间</div></div>";
                        $(data).each(function (index, obj) {
                          html += "<div class=\"item ui-widget-content\"><div class=\"image\"><div class=\"base-msg\"><div class=\"img-container\"><img src=\"<%= Common.GetWebConfigKey("ImgViewPath")%>/PicSpace/<%=this.CompID %>/" + obj.Name + "\" alt=\"\"  width=\"160px\" height=\"120px\"></div><div class=\"img-name\" title=\"" + obj.Name + "\">" + obj.Name + "</div><input type=\"text\" value=\"" + obj.Name + "\" /><div class=\"qout icon\"></div></div><div class=\"out\">" + obj.Name.toString().substring(1) + "</div><div class=\"out\">" + obj.Pixel + "</div><div class=\"out\">" + obj.Size + "KB</div><div class=\"out\">" + obj.Time + "</div></div></div>";
                        })
                         $("#J_Picture").html(html);
//                    },ajaxStart:function(){
//                        $("#J_Picture").html("<img src=\"../js/layer/skin/default/loading-0.gif\" />");   
                    },error: function () {
                         $("#J_Picture").html("<div class=\"list-head clearfix\"><div class=\"span1\">名称</div><div class=\"span2\">类型</div><div class=\"span2\">尺寸</div><div class=\"span2\">大小</div><div class=\"span2\">更新时间</div></div>");
                        return false;
                    }
                })
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-3" />
    <uc1:CompRemove runat="server" ID="Remove" ModuleID="3" />
    <div class="rightinfo">
       <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a href="../GoodsNew/GoodsList.aspx" runat="server" id="atitle">商品列表</a></li><li>
                    ></li>
                <li><a href="javascript:;" runat="server" id="btitle">图片空间</a></li>
            </ul>
        </div>
        <div id="wrap">
            <div class="main-pannel">
                <div class="all-control-bar">
                    <ol class="breadcrumb" id="J_Crumbs">
                        <li class="active home"><i class="icon"></i>我的图片</li></ol>
                    <div class="control">
                        <div class="control-buttons" id="J_UpAndNew">
                            <button type="button" class="btn2 btn-primary up" data-spm-click="gostr=/tbimage;locaid=d4916817"
                                onclick="javascript:goldlog.record('/tu.1.1','','','H1673809')">
                                <span class="up-icon"></span>上传图片</button>
                        </div>
                        <div class="search-form" style="margin-left: 10px;">
                            <input type="text" class="form-control search-input keyword" placeholder="图片名称搜索">
                        </div>
                    </div>
                </div>
                <div class="page-bar clearfix" style="width: 1046px; box-shadow: none;">
                    <div class="select-bar clearfix" id="J_SelectBar">
                        <div class="select-all">
                            <input type="checkbox" id="J_SelectAll" title="全选/反选"></div>
                        <div class="selected-msg" style="cursor: pointer;">
                            全选</div>
                        <ul class="controlBar selected-controls" id="J_ControlBar">
                            <li class="delete" style="display: none;"><a href="javascript:;"><i class="icon"></i>
                                删除</a></li>
                        </ul>
                        <ul class="right-menu selected-controls" id="J_PicRightmenu" style="display: none;">
                            <li class="delete" style="display: list-item;"><a href="javascript:;"><i class="icon">
                            </i>删除</a></li>
                        </ul>
                    </div>
                    <div class="sort-bar" id="J_SortBar" style="display: block;">
                        <div class="sort my-dropdown">
                            <div class="drop-label">
                                排序:</div>
                            <div class="dropdown" id="J_Sort">
                                <a class="dropdown-toggle down" data-type="0">时间</a>
                                <ul class="dropdown-menu" role="menu" aria-labelledby="dropdownMenu1">
                                    <li role="presentation"><a role="menuitem" data-type="0" class="time down" data-spm-click="gostr=/tbimage;locaid=d4916857">
                                        时间</a> </li>
                                    <li role="presentation"><a role="menuitem" data-type="1" class="time up" data-spm-click="gostr=/tbimage;locaid=d4916853">
                                        时间</a> </li>
                                    <li role="presentation"><a role="menuitem" data-type="2" class="big down" data-spm-click="gostr=/tbimage;locaid=d4916865">
                                        大小</a> </li>
                                    <li role="presentation"><a role="menuitem" data-type="3" class="big up" data-spm-click="gostr=/tbimage;locaid=d4916861">
                                        大小</a> </li>
                                    <li role="presentation"><a role="menuitem" data-type="6" class="name down" data-spm-click="gostr=/tbimage;locaid=d4916869">
                                        名称</a> </li>
                                    <li role="presentation"><a role="menuitem" data-type="7" class="name up" data-spm-click="gostr=/tbimage;locaid=d4916873">
                                        名称</a> </li>
                                </ul>
                            </div>
                        </div>
                        <div class="btn-group show-type">
                            <button type="button" id="J_ShowList" class="btn2 btn-default" title="列表模式" data-spm-click="gostr=/tbimage;locaid=d4916845">
                                <span class="list icon "></span>
                            </button>
                            <button type="button" id="J_ShowPic" class="btn2 btn-default" title="大图模式" data-spm-click="gostr=/tbimage;locaid=d4916849"
                                style="background-color:rgb(204, 204, 204)">
                                <span class="big-pic icon "></span>
                            </button>
                        </div>
                        <div class="page-msg" id="J_TopPagination">
                        </div>
                    </div>
                </div>
                <div class="pic-container" id="J_PicContainer" style="width: 1047px;">
                    <div id="J_Picture" class="clearfix ui-selectable" style="min-height: 86px;">
                        <div class="list-head clearfix">
                            <div class="span1">
                                名称</div>
                            <div class="span2">
                                类型</div>
                            <div class="span2">
                                尺寸</div>
                            <div class="span2">
                                大小</div>
                            <div class="span2">
                                更新时间</div>
                        </div>
                        <asp:Repeater ID="rptImg" runat="server">
                            <ItemTemplate>
                                <div class="item ui-widget-content">
                                    <div class="image">
                                        <div class="base-msg">
                                            <div class="img-container">
                                                <img src="<%# Common.GetWebConfigKey("ImgViewPath")+"PicSpace/"+this.CompID+"/" +Eval("Name")%>"
                                                    alt="" width="160px" height="120px"></div>
                                            <div class="img-name" title="<%# Eval("Name")%>">
                                                <%# Eval("Name")%></div>
                                            <input type="text" value="<%# Eval("Name")%>" />
                                            <div class="qout icon">
                                            </div>
                                        </div>
                                        <div class="out">
                                            <%#Eval("Type").ToString().Substring(1) %></div>
                                        <div class="out">
                                            <%#Eval("Pixel") %></div>
                                        <div class="out">
                                            <%#Eval("Size") %>KB</div>
                                        <div class="out">
                                            <%#Eval("Time") %></div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

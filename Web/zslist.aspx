<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zslist.aspx.cs" Inherits="zslist" %>

<%@ Register Src="~/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/UserControl/NavTopControl.ascx" TagPrefix="uc1" TagName="TopNav" %>
<%@ Register Src="~/UserControl/BottomControl.ascx" TagPrefix="uc1" TagName="Bottom" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
<meta charset="utf-8">
<title>医站通-招商列表</title>
<link href="css/global-2.0.css?v=2.7.8.1" rel="stylesheet" type="text/css"/>
<script type="text/javascript" src="js/jquery-1.9.1.min.js"></script>
<script src="js/menu.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
<script src="/js/MyClassPaging.js" type="text/javascript"></script>
<script type="text/javascript" src="/js/Underscore-1.7.0.js"></script>
<script src="/js/layer/layer.js" type="text/javascript"></script>
<script src="/js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
<script src="../../js/layer/layer.js" type="text/javascript"></script>
<script src="../../js/layerCommon.js" type="text/javascript"></script> 
<link href="/css/layer.css" rel="stylesheet" type="text/css" />
<style  type="text/css">
    .none {
        display:none;
    }

.footer-loading{ text-align:center; font-size:0.16rem; color:#999; height:0.5rem; line-height:0.5rem;padding:0.3rem 0 0 0;}
.footer-loading .loading {
	   display: inline-block;
	   height: 15px;
	   width: 15px;
	   border-radius: 100%;
	   margin: 6px;
	   border: 2px solid #666;
	   border-bottom-color: transparent;
	   vertical-align: middle;
	   -webkit-animation: rotate 0.75s linear infinite;
	   animation: rotate 0.75s linear infinite;
     }
  @-webkit-keyframes rotate {
    0% {
        -webkit-transform: rotate(0deg);
    }
    50% {
        -webkit-transform: rotate(180deg);
    }
    100% {
        -webkit-transform: rotate(360deg);
    }
}
  @keyframes rotate {
         0% {
             transform: rotate(0deg);
         }

         50% {
             transform: rotate(180deg);
         }

         100% {
             transform: rotate(360deg);
         }
     }
</style>
</head>

<body>
    <form runat="server" id="form1">
<!--顶部导航栏开始-->
 <uc1:Top ID="top1" runat="server" />


<!--顶部导航结束-->	
<!--页头开始-->
<!--顶部导航栏 end-->
<uc1:TopNav ID="TopNav1" runat="server" />
<!--页头结束-->


<!--内容start-->	
<div class="bgf5">
	<div class="w1200">
		<div class="m-crumbs" id="TitleDiv"><a href="index.html">首页</a> > <a href="zslist.html">招商信息</a></div>
		<div class="list-con">
			<ul class="category">
			<li><span class="name fl">生产厂家：</span>
			<div class="changjia fl" id="productions">
                <asp:Repeater runat="server" ID="Rp_Production">
                    <ItemTemplate>
                         <a href="javascript:void(0)" tip="<%#Eval("id") %>" ><%#Eval("CompName") %></a>
                    </ItemTemplate>
                </asp:Repeater>
			</div>
			</li>
			<li><span class="name fl">商品分类：</span>
			<div class="changjia fl" id="category1">
                <asp:Repeater runat="server" ID="Rp_Category1">
                    <ItemTemplate>
                         <a href="javascript:void(0)" tip="<%#Eval("id") %>" ><%#Eval("TypeName") %></a>
                    </ItemTemplate>
                </asp:Repeater>
			</div>
			</li>
			<li class="none"><span class="name fl">二级分类：</span>
			<div class="changjia fl" id="category2">

			</div>
			</li>
          <li class="none"><span class="name fl">三级分类：</span>
			<div class="changjia fl" id="category3">

			</div>
			</li>
			</ul>

           <%-- 商品列表数据--%>
			<ul class="pro ulGoodsList">
			
			</ul>
			<div id="Pager_List" class="page paging" >
              
             </div>
		</div>
		<div class="blank25"></div>
	</div>	
</div>
<!--内容 end-->		
	
 
<!--foot start-->
   <uc1:Bottom ID="Bottom1" runat="server" />
<!--foot end-->			
         <input runat="server" value="" id="HdProduction" />
         <input runat="server" value="" id="HdCategory1" />
         <input runat="server" value="" id="HdCategory2" />
         <input runat="server" value="" id="HdCategory3" />
         <input id="hid_Alert" type="hidden" />
  <script type="text/html" id="goodsTemplate">
    {{ _.each(rows, function(item)  {  }}  
      	<li>
			<div class="pic fl"><a target="_blank" href="/e{{= item.ID}}_{{= item.CompID}}_{{=item.ycId }}.html"><img src="{{= item.Pic }}" width="178" height="128" alt=""/></a></div>
			<div class="tex fl"><p class="t1"><a target="_blank" href="/e{{= item.ID}}_{{= item.CompID}}_{{=item.ycId }}.html">{{= item.GoodsName}}</a></p><p class="t2">{{= item.ShowName }}天前发布（{{= item.CreateDate2 }}）</p>
				<p>{{= item.Title}}</p>
				<p class="tex_remark">{{= item.Remark  }}</p>
			</div>
	        <div class="btn fr"><a target="_blank" class="btn-red" href="../eshop/about_{{= item.CompID}}.html">厂家介绍</a><a class="btn-blue btn-apply" tip="{{item.CompID}}" yc-tip="{{item.ycCompID}}" yc-id="{{=item.ycId}}" href="javascript:void(0)">申请合作</a>
	        </div>
	</li>
    {{ }) }}
</script>
</form>
</body>
<script type="text/javascript" src="js/jquery.SuperSlide.2.1.1.js"></script>
<script type="text/javascript">
    "use strict"
    $(document).ready(function () {
        let categoryCommon = {
            GoodsClassexPand: function () {
                // 一级分类点击事件
                $("#category1 a").on("click", function () {
                    if (!$(this).hasClass("cur")) {
                        $("#category1 a").removeClass("cur");
                        $(this).addClass("cur");
                        $("#HdCategory1").val($(this).attr("tip"));
                        $("#category3").empty().closest("li").hide();
                        $("#HdCategory2,#HdCategory3").val("");
                        //获取二级分类数据
                        categoryCommon.GetCategoryResource($.trim($("#HdCategory1").val()), function (data) {
                            $("#category2").empty().closest("li").fadeOut(100);
                            if (data.constructor === Array && data.length > 0) {
                                 $("#category2").empty().closest("li").fadeIn(100);
                                $.each(data, function (index, item) {
                                    $("#category2").append("<a href=\"javascript:void(0)\" tip='" + item.ID + "' >" + item.TypeName + "</a>");
                                })
                            }
                        })
                    } else {
                        $(this).removeClass("cur");
                        $("#HdCategory1,#HdCategory2,#HdCategory3").val("");
                        $("#category2").empty().closest("li").fadeOut(100);
                        $("#category3").empty().closest("li").fadeOut(100);
                    }
                    MaperFunction.GetPaginData();

                });


                // 二级分类点击事件
                $(document).on("click","#category2 a", function () {
                    if (!$(this).hasClass("cur")) {
                        $("#category2 a").removeClass("cur");
                        $(this).addClass("cur");
                        $("#HdCategory2").val($(this).attr("tip"));
                        $("#HdCategory3").val("");
                        //获取二级分类数据
                        categoryCommon.GetCategoryResource($.trim($("#HdCategory2").val()), function (data) {
                            if (data.constructor === Array && data.length > 0) {
                                $("#category3").empty().closest("li").fadeIn(100);
                                $.each(data, function (index, item) {
                                    $("#category3").append("<a href=\"javascript:void(0)\" tip='" + item.ID + "' >" + item.TypeName + "</a>");
                                })
                            }
                        })
                    } else {
                        $(this).removeClass("cur");
                        $("#HdCategory2,#HdCategory3").val("");
                        $("#category3").empty().closest("li").fadeOut(100);
                    }
                    MaperFunction.GetPaginData();

                });

                // 三级分类点击事件
                $(document).on("click", "#category3 a", function () {
                    if (!$(this).hasClass("cur")) {
                        $("#category3 a").removeClass("cur");
                        $(this).addClass("cur");
                        $("#HdCategory3").val($(this).attr("tip"));
                    } else {
                        $(this).removeClass("cur");
                        $("#HdCategory3").val("");
                    }
                    MaperFunction.GetPaginData();

                });


                // 生产厂家点击事件
                $(document).on("click", "#productions a", function () {
                    if (!$(this).hasClass("cur")) {
                        $("#productions a").removeClass("cur");
                        $(this).addClass("cur");
                        $("#HdProduction").val($(this).attr("tip"));
                    } else {
                        $(this).removeClass("cur");
                        $("#HdProduction").val("");
                    }
                    MaperFunction.GetPaginData();

                });

                //申请合作
                $(document).on("click", "div.btn .btn-apply", function () {
                    let _this = this;
                    $.ajax({
                        type: 'post',
                        data: { action: "applyCooperation", CompId: $(this).attr("tip"),ycCompID:$(this).attr("yc-tip") },
                        dataType: "json",
                        success: function (ReturnData) {
                            if (ReturnData.Result) {
                                 //转向网页的地址; 
                                var url = '/Distributor/CMerchants/FirstCampAdd.aspx?KeyID=' + $(_this).attr("yc-id");
                                var index = layerCommon.openWindow("申请合作", url, '950px', '615px'); //记录弹出对象
                                $("#hid_Alert").val(index);
                            } else {
                                if (ReturnData.Code == "Login") {
                                    layerCommon.openWindow("用户登录", "/WindowLogin.aspx", "400px", "345px", function () {
                                        $.ajax({
                                            type: "POST",
                                            url: '/Controller/login.ashx',
                                            data: { SubmitAcion: "CloseAccuntSwitch" },
                                            cache: false,
                                            success: function (ReturnData) {
                                                
                                            },
                                            error: function () {
                                            }
                                        });
                                    }, false);
                                } else {
                                    layerCommon.msg(ReturnData.Msg, IconOption.错误);
                                }
                            }
                        },
                    })

                });

                //初始化加载数据
                MaperFunction.GetPaginData();

            },
            GetCategoryResource: function (cateId,callback) {
                $.ajax({
                    type: 'post',
                    data: { action: "getCategoryResource", cateId: cateId },
                    dataType: "json",
                    success: function (data) {
                        callback && callback(data);
                    },
                })
            }
        };

        categoryCommon.GoodsClassexPand();

    })


    //绑定商品分页数据
    var MaperFunction = {
        GetPaginData: function () {
            $(".paging").myPagination({
                currPage: 1,
                pageCount: 1,
                pageSize: 8,
                btnsize: 5,
                IsShowOnePaging: true,
                cssStyle: 'myPagination',
                info: {
                    msg_on: false,
                    first_on: false,
                    last_on: false,
                    prev: "上一页",
                    next: "下一页"
                },
                ajax: {
                    on: true,
                    callback: 'MaperFunction.BindGoodsData',
                    url: "zslist.aspx",
                    dataType: 'json',
                    params: {
                        action: "getGoodsResource",
                        compId: $.trim($("#HdProduction").val()),
                        cate1: $.trim($("#HdCategory1").val()),
                        cate2: $.trim($("#HdCategory2").val()),
                        cate3: $.trim($("#HdCategory3").val()),
                    },
                    ajaxStart: function () {
                        $(".ulGoodsList").empty().append('<li><footer class="footer-loading" style="font-size:12px;height:25px;"  id="footer"><span class="loading"></span><span>正在加载数据请稍候...</span></footer></li>');
                    }, ajaxStop: function () {
                    },
                    ajaxError: function () {
                        $(".ulGoodsList").empty().append('<li><div style="text-align:center;font-size:12px;height:30px;">暂无数据</div></li>');
                    }
                }
            });
        },
        BindGoodsData: function (data) {
            $(".ulGoodsList").empty();
            console.log(data)
            if (data.rows.constructor === Array && data.rows.length > 0) {
                var template = $('#goodsTemplate').html(), tempMap = _.template(template);
                $(".ulGoodsList").html(tempMap(data))
            } else {
                $(".ulGoodsList").append('<li><div style="text-align:center;font-size:12px;height:30px;">暂无数据</div></li>');
            }
            console.log(data);
        },
    }
</script>
</html>
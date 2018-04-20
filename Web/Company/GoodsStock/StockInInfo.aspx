<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StockInInfo.aspx.cs" Inherits="Company_GoodsStock_StockInInfo" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/Company/UserControl/TreeDemo.ascx" TagPrefix="uc1" TagName="TreeDemo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>商品入库明细</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../Distributor/newOrder/css/global2.5.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#rmk").hover(function () {
                $("#txtbox").show();;
            }, function () {
                $("#txtbox").hide();
            });
            //修改
            $("#Update").click(function () {
                var type = $("#hid_Type").val();
                var no = $.trim($("#hid_No").val());
                window.location = "StockInEdit.aspx?type=" + type + "&no="+no+""
            })
            //返回
            $("#reture").click(function () {
                var type = $("#hid_Type").val();
                window.location = "StockInList.aspx?type=" + type + "";
            })

            //删除
            $("#del").on("click", function () {
                layerCommon.confirm("确认删除？", function () {
                    var no = $.trim($("#hid_No").val());
                    var type = $("#hid_Type").val();
                    $.ajax({
                        url: '../../Handler/orderHandle.ashx',
                        type: 'Post',
                        data: { ActionType: "DeleteOoder", no: no, type: type },
                        success: function (data) {
                            if (data == "true") {
                                window.location = "StockInList.aspx?type=" + type + "";
                            } else {
                                layerCommon.msg("删除失败", IconOption.错误)
                            }
                        },
                        error: function (data) {
                            layerCommon.msg("操作失败", IconOption.错误)
                        }
                    });
                }, "提示");
            });

          //审核
            $("#Sh").click(function () {
                var no = $.trim($("#hid_No").val());
                var type = $("#hid_Type").val();
                $.ajax({
                    type: "post",
                    url: "../../Handler/orderHandle.ashx",
                    data: { ActionType: "ShengHe", type: type, no: no },
                    dataType: "text",
                    success: function (data) {
                        if (data == "OK") {
                            layerCommon.msg("审核成功", IconOption.笑脸);
                            window.location = "StockInInfo.aspx?no="+no+"&type=" + type + "";
                        }
                        else {
                            layerCommon.msg("操作失败：" + data + "", IconOption.哭脸);
                        }

                        return false;
                    }, error: function () {
                        layerCommon.msg("提交失败", IconOption.错误);
                        return false;
                    }
                })
            })


            //打印订单
            $(document).on("click", "#orderprint", function () {
                var oID = $.trim($("#hidOrderID").val());

                //转向网页的地址; 
                var url = 'printStock.aspx?KeyID=' + oID;
                //var index = layerCommon.openWindow("打印订单", url, '1000px', '600px'); //记录弹出对象
                //$("#hid_Alert").val(index); //记录弹出对象

                var name = '订单打印';                     //网页名称，可为空; 
                var iWidth = 850;                          //弹出窗口的宽度; 
                var iHeight = 600;                    //弹出窗口的高度; 
                //获得窗口的垂直位置 
                var iTop = (window.screen.availHeight - 30 - iHeight) / 2;
                //获得窗口的水平位置 
                var iLeft = (window.screen.availWidth - 10 - iWidth) / 2;
                window.open(url, name, 'height=' + iHeight + ', width=' + iWidth + ',top=' + iTop + ',left=' + iLeft);
            });

        })
    </script>
        <style type="text/css">
        .box
        {
            padding: 0px 10px;
            border: 1px solid #ddd;
            border-left: 0;
            border-top: 0;
            border-right: 0;
            width: 190px;
            height: 27px;
            line-height: 27px;
            color: #999;
            padding-left: 5px;
            font-family: "微软雅黑";
            font-size: 12px;
        }
        .goods-zs > .tabLine
        {
            position: initial;
        }
            .StockOrder {
                margin-left:100px;
            }
            #txtbox{
               width:400px;
               height:auto;
               background-color:#f5f7fa;
               border:1px solid #e3e7ee;
               position:absolute;
               resize:none;
               line-height:30px;
               vertical-align:middle;
               text-align: inherit;
               word-wrap:break-word; 
               word-break:break-all;
               display:none;
               z-index:100;
           }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-3" />
    <input id="hid_Alert" type="hidden" />  
    <input type="hidden" id="hid_No" runat="server" value="0"/>
    <input type="hidden" id="hid_Type" runat="server" value="1"/>
    <input type="hidden" id="hidCompId" runat="server" />
    <input type="hidden" runat="server" id="hidOrderID" value="" />

    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a href="#" runat="server" id="title1">商品入库列表</a></li>
                <li>></li>
                <li><a href="javascript:;" runat="server" id="title2">商品入库明细</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
         <div class="goods-if">
             <div class="alink">
               <label id="edit" runat="server" visible="false">
               <a href="javascript:;" class="bule" id="Update">修改</a>|</label>
               <label id="del" runat="server" visible="false">
               <a href="javascript:;" class="bule" runat="server" id="delete">删除</a>|</label>
               <label id="orderaudit" runat="server" visible="false">
               <a href="javascript:;" class="bule" id="Sh" runat="server">审核</a>|</label>
               <label id="orderprint" runat="server">
               <a href="javascript:;" class="bule" id="btnprint">打印</a></label>
               <label id="returns" runat="server">
               <a href="javascript:;" class="bule" id="reture">返回</a></label>
         </div>
        <div class="cancel ordercancel" style="display:none;"></div>
         <div class="title">
         <i>单号：<label id="lblNo" runat="server"></label>
         </i> 
         <i class="StockOrder"><%=TitleType %>类型：<label id="lblType" runat="server"></label></i> 
         <i class="StockOrder">制单时间：<label id="lbldate" runat="server"></label></i> <br />
         <i id="rmk" class="">备注：<label id="lblRmk" runat="server"></label></i>
          <div id="txtbox" runat="server"></div>
       </div></div>
         <div class="goods-zs">
      <div class="tabLine" >
                <table border="0" cellspacing="0" cellpadding="0">
                    <thead>
                        <tr>
                            <th class="">
                                商品名称
                            </th>
                            <th class="">
                                规格属性
                            </th>
                            <th class="t5">
                                单位
                            </th>
                            <th>
                                批次号
                            </th>
                            <th>
                                有效期
                            </th>
                            <th style="width: 16%">
                                <%=TitleType %>数量
                            </th>
                            <th class="t3">
                                备注
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr runat="server" id="tbodyTR">
                       <td> <div class="search">
                       <input name="" type="text" class="box project2"/><a class="opt-i"></a>
                       <!--[if !IE]>搜索弹窗 start<![endif]-->
                       <div class="search-opt none"> <ul class="list"></ul><div class="opt">
                       <a href="javascript:;"><i class="opt2-i"></i>选择商品</a></div> </div>
                       <!--[if !IE]>搜索弹窗 end<![endif]-->  </div> </td>  <td> </td> <td> </td> <td>  </td>
                       </tr>
                        <asp:Repeater ID="Repeater1" runat="server">
                            <ItemTemplate>
                           <tr trindex="<%#Eval("ID")%>" trindex2="<%#Eval("ID")%>" id="<%#Eval("GoodsID") %>" tip="<%#Eval("GoodsinfoID") %>">
                            
                               <td>
                                   <div class="sPic"><a class="opt-i2"></a>
                                   <span>
                                       <a onclick="return false;" target="_blank" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html">
                                            <img src="<%#Common.GetPicURL(Eval("pic").ToString(), "resize200") %>" width="60" height="60">
                                       </a>
                                    </span>
                                       <a  onclick="return false;" target="_blank" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html" class="code">商品编码：<%#Eval("BarCode") %> </a>
                                       <a onclick="return false;" style=" width:250px;" target="_blank"  href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html" class="name"> <%#Eval("GoodsName")%>  <i><%#Eval("GoodsName")%></i></a>
                                    </div>
                               </td>
                             <td class="tc"><%#Eval("ValueInfo") %></td>
                            <td><div class="tc"><%#Eval("Unit") %>
                          </div></td>
                          <td>
                                <div class="tc"><%#Eval("BatchNO") %></div>
                            </td>
                            <td>
                                <div class="tc"><%# Eval("validDate","{0:yyyy-MM-dd}") %></div>
                            </td>
                          <td class="tc"><div class="" tip="<%#Eval("GoodsinfoID") %>" tip2="<%#Eval("ID")%>"><%#Eval("StockNum") %></div></td><td><div class="tc alink">
                         <a href="javascript:;" class="aremark<%#Eval("ID")%>"><%#Eval("Remark").ToString()!=""?"<div class=\"tc alink\"><a href=\"javascript:;\" class=\"aremark29\"></a><div class=\"divremark29\">"+(Eval("Remark").ToString().Length <6 ? Eval("Remark").ToString(): Eval("Remark").ToString().Substring(0,6)+"...")+"..."+"</div><div class=\"cur\">"+Eval("Remark")+"</div></div>":"" %></a></div></td></tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div></div>

    </div>
    </form>
</body>
</html>

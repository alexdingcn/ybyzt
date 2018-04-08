<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryInfo.aspx.cs" Inherits="Company_GoodsStock_InventoryInfo" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/Company/UserControl/TreeDemo.ascx" TagPrefix="uc1" TagName="TreeDemo" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>商品盘点明细列表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"rel="stylesheet" type="text/css" />
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
     <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#rmk").hover(function () {
                $("#txtbox").show();;
            }, function () {
                $("#txtbox").hide();
            });
            $("#cancel").click(function () {
                location.href = "InventoryList.aspx"; 
            })
            $("#edits").click(function () {
                var no = $("#hid_No").val();
                window.location = "InventoryEdit.aspx?type=1&no=" + no + "";
            });

            //删除
            $("#del").on("click", function () {
                layerCommon.confirm("确认删除？", function () {
                    var no = $("#hid_No").val();
                    var type = 3;
                    $.ajax({
                        url: '../../Handler/orderHandle.ashx',
                        type: 'Post',
                        data: { ActionType: "DeleteOoder", no: no ,type:type},
                        success: function (data) {
                            if (data == "true") {
                                window.location = "InventoryList.aspx";
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
            $("#SH").click(function () {
                var no = $("#hid_No").val();
                $.ajax({
                    type: "post",
                    url: "../../Handler/orderHandle.ashx",
                    data: { ActionType: "ShengHe1", no: no },
                    dataType: "text",
                    success: function (data) {
                        var jsons = JSON.parse(data);
                        if (jsons.returns == "True") {
                            window.location = "InventoryInfo.aspx?no=" + jsons.no + "";
                            //$("#del").attr("Visible", false);
                        }
                        else {
                            layerCommon.msg("操作失败：" + jsons.msg + "", IconOption.哭脸);
                        }
                        return false;
                    }, error: function () {
                        layerCommon.msg("提交失败", IconOption.错误);
                        return false;
                    }
                })
            })
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
                margin-left:140px;
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
    <input type="hidden" id="hid_No" name="hid_No" runat="server"/>
    <input type="hidden" id="hidCompId" runat="server" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a href="InventoryList.aspx">库存盘点列表</a></li>
                <li>></li>
                <li><a href="javascript:;">库存盘点明细列表</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--信息列表 start-->
         <div class="goods-if">
              <div class="alink">
                   <label id="edit" runat="server" visible="false">
                   <a href="javascript:;" id="edits" class="bule">修改</a>|</label>
                   <label id="del" runat="server" visible="false">
                   <a href="javascript:;" class="bule" runat="server" id="delete">删除</a>|</label>
                   <label id="orderaudit" runat="server" visible="false">
                   <a href="javascript:;" class="bule" runat="server" id="SH">审核</a>|</label>
                   <label id="returns" runat="server">
                   <a href="InventoryList.aspx" class="bule">返回</a></label>
              </div>
              <div class="cancel ordercancel" style="display:none;"></div>
              <div class="title">
                 <i>单号：<label id="lblNo" runat="server">xdfg2334234</label></i> 
                 <i class="StockOrder">制单日期：<label id="lblType" runat="server">是防守打法三分毒</label></i><br />
                 <i id="rmk">备注：<label id="lblRmk" runat="server">水电费水电费水电费水电费</label></i>
                  <div id="txtbox" runat="server"></div>
              </div>
         </div>
         <div class="goods-zs">
             <div class="tabLine" >
                <table border="0" cellspacing="0" cellpadding="0">
                    <thead>
                        <tr>
                            <th class="">
                                商品名称
                            </th>
                            <th class="">
                                商品规格属性
                            </th>
                            <th class="t5">
                                单位
                            </th>
                            <th style="width: 16%">
                                当前库存
                            </th>
                            <th style="width: 16%">
                                盘点库存
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
                                   <div class="sPic">
                               <span style="width:60px;height:60px;"><a onclick="return fafalse;" target="_blank" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html">
                            <img src="<%#Eval("pic").ToString()!= "" ?Common.GetWebConfigKey("ImgViewPath")+"GoodsImg" +Eval("pic").ToString():"../../images/pic.jpg"%>" width="60" height="60"></a>
                            </span><a onclick="return fafalse;" target="_blank" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html" class="code">商品编码：<%#Eval("BarCode") %> </a><a onclick="return fafalse;" target="_blank" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html" class="name"> <%#Eval("GoodsName")%>  <i><%#Eval("GoodsName")%></i></a>
                                   </div>
                               </td>
                               <td class="tc"><%#Eval("ValueInfo") %></td>
                               <td>
                                   <div class="tc"><%#Eval("Unit") %></div>
                               </td>
                               <td class="tc"><%# Eval("StockOldNum") %></td>
                               <td><div style="text-align:center;" tip="<%#Eval("GoodsinfoID") %>" tip2="<%#Eval("ID")%>"><%#Eval("StockNum") %></div>
                               </td>
                               <td>
                                   <div class="tc alink">
                         <%#Eval("Remark").ToString()!=""?"<div class=\"tc alink\"><div class=\"divremark29\">"+(Eval("Remark").ToString().Length <6 ? Eval("Remark").ToString(): Eval("Remark").ToString().Substring(0,6)+"...")+"..."+"</div><div class=\"cur\">"+Eval("Remark")+"</div></div>":"" %>
                                   </div>
                               </td>
                           </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
         </div>
        <!--信息列表 end-->
    </div>
    </form>
</body>
</html>

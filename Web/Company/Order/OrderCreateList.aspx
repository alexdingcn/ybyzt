<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderCreateList.aspx.cs"
    Inherits="Company_Order_OrderCreateList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>订单列表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/order.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            });

            //删除多选按钮全选
            $("#FCheck").click(function () {
                var cla = $(this).attr("class")
                if (cla == "cur") {
                    //取消全选
                    $(".RmCheck").removeClass("cur");
                    $("[name = RmCheck]:checkbox").prop("checked",false);
                    $(this).removeClass("cur");
                }
                else {
                    //全选
                    $(".RmCheck").addClass("cur");
                    $("[name = RmCheck]:checkbox").prop("checked", true);
                    $(this).addClass("cur")
                }

            })

            //删除按钮单条选中
            $("[name = RmCheck]:checkbox").click(function () {
                var cla = $(this).attr("class")
                if (cla == "RmCheck cur") {
                    //取消选中
                    $(this).removeClass("cur");
                }
                else {
                    //选中
                    $(this).addClass("cur")
                }
                var chk = $(".chk .cur");
                if ((chk.length * 1) > 0) {
                    $("#FCheck").prop("checked", true);
                    $("#FCheck").addClass("cur")
                }
                else {
                    $("#FCheck").prop("checked", false);
                    $("#FCheck").removeClass("cur")
                }
            })
            //确认删除
            $("#DelAll").click(function () {
                var chk = $(".chk .cur");
                if ((chk.length * 1) > 0) {
                    layerCommon.confirm("该操作不可恢复！是否确定", function () {

                        var IDlist = "";
                        $.each(chk, function (index, item) {
                            IDlist += $(item).val() + ",";
                        })
                        $.ajax({
                            type: "post",
                            url: "../../Handler/orderHandle.ashx",
                            data: { ActionType: "DelAll", IDlist: IDlist },
                            dataType: "json",
                            success: function (data) {
                                if (data.rel == "OK") {
                                    layerCommon.msg("" + data.Msg + "", IconOption.笑脸);
                                    window.location.href = 'OrderCreateList.aspx';
                                }
                                else {
                                    layerCommon.msg("" + data.Msg + "", IconOption.哭脸);
                                }
                            }
                        })
                    }, "批量删除")
                }
                else {
                    layerCommon.msg("请最少选择一条订单！", IconOption.错误);
                }
            })

        });

        $(document).ready(function () {

            $("#btnAdd").click(function () {

                window.location.href = '../neworder/orderBuy.aspx';
            });

            //导出
            $("#export").click(function () {

                var str = "";
                var OState = $("#ddrOState").val();;
               
                if ($.trim($("#txtReceiptNo").val()) != "")
                    str += " and o.ReceiptNo like '%" + $("#txtReceiptNo").val() + "&'";
                if ($.trim($("#ddrPayState").val()) != "-1")
                    str += " and o.PayState=" + $("#ddrPayState").val();
                if ($.trim($("#txtTotalAmount1").val()) != "")
                    str += " and o.AuditAmount>=" + $("#txtTotalAmount1").val();
                if ($.trim($("#txtTotalAmount2").val()) != "")
                    str += " and o.AuditAmount<=" + $("#txtTotalAmount2").val();
                if ($.trim($("#txtCreateDate").val()) != "")
                    str += " and o.CreateDate>='" + $("#txtCreateDate").val() + "'";
                if ($.trim($("#txtEndCreateDate").val()) != "") {
                    var dtime = $("#txtEndCreateDate").val()
                    // 转换日期格式
                    dtime = dtime.replace(/-/g, '/'); // "2010/08/01";
                    // 创建日期对象
                    var date = new Date(dtime);
                    // 加一天
                    date.setDate(date.getDate() + 1);
                    var dateTime = date.getFullYear() +"-"+ (date.getMonth() + 1).toString() +"-"+ (date.getDate()).toString();
                    str += "and o.CreateDate<'" + dateTime + "'";
                }
                if ($.trim($("#txtDisName").val()) != "")
                    str += "and dis.disName like '%" + $("#txtDisName").val() + "%'";

                window.location.href = '../../../ExportExcel.aspx?intype=1&searchValue=' + str + '&p=' + $("#txtPager").val() + '&c=<%=Pager.CurrentPageIndex %> &OState=' + OState + '';

                return false;
            });

            //订单生成 搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });

            //设置cookie  
            function setCookie(name, value, seconds) {  
                 seconds = seconds || 0;   //seconds有值就直接赋值，没有为0，这个根php不一样。  
                 var expires = "";  
                 if (seconds != 0 ) {      //设置cookie生存时间  
                 var date = new Date();  
                 date.setTime(date.getTime()+(seconds*1000));  
                 expires = "; expires="+date.toGMTString();  
                 }  
                 document.cookie = name+"="+value+expires+"; path=/";   //转码并赋值  
            }

            //订单详情导出
            $("#liexcel").on("click", function () {
                if (parseInt(<%=PageSize %>) <= parseInt(50)) {
                    setCookie("oID","<%=oId %>",1800);
                    window.location.href = '../../../ExportExcel.aspx';
                } else {
                    layerCommon.msg("订单详情导出订单条数不得大于50条！", IconOption.笑脸);
                }
                return false;
            });

            //批量删除
            //$("#VolumeDel").on("click", function () {
            //$("#btnVolumeDel").trigger("click");
            //    fromDel('提示', '确认删除吗？', Del);
            //});

            ///重置
            $("#li_Reset").click(function () {
                window.location.href = 'OrderCreateList.aspx';
            });
        });

        //审核
        function RegionAudit(OrderID) {
            var height = document.body.clientHeight; //计算高度
            var layerOffsetY = (height - 450) / 2; //计算宽度

            //记录弹出对象
            var index = layerCommon.openWindow('订单审核', 'OrderAudit.aspx?KeyID=' + OrderID, '650px', '350px');
            $("#hid_Alert").val(index); //记录弹出对象

            return false;
        }
        //审核后关闭
        function Audit(Id) {
            //关闭选择商品窗口
            CloseDialog();
            window.location.href = 'OrderCreateInfo.aspx?KeyID=' + Id + '&type=5&go=2';
        }
        //审核 --退回
        function RAudit(Id) {
            //关闭选择商品窗口
            CloseDialog();
            //window.location.herf = '../Order/OrderAuditInfo.aspx?KeyID=' + Id;
            $("#btnRAudit").trigger("click");
        }
        //发货
        function ship(Oid, JuOrder) {
            var height = document.body.clientHeight; //计算高度
            var layerOffsetY = (height - 450) / 2; //计算宽度
            // 记录弹出对象
            var index = layerCommon.openWindow('物流发货', 'LogisticsEdit.aspx?orderId=' + Oid + '&orderOutId=&type=0', '550px', '280px');
            $("#hid_Alert").val(index); //记录弹出对象
            return false;
        }
        //关闭物流发货
        function LogisticsCancel(Oid, outID) {
            window.location.href = 'OrderOutInfo.aspx?OrderId=' + Oid + '&KeyID=' + outID + '&go=2';
        }

    </script>
    <style type="text/css">
        .tablelist td a
        {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-1" />
    <input id="hid_Alert" type="hidden" />
    <input type="hidden" id="hidCompId" runat="server" />
    <div class="rightinfo">
        <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>></li>
                <li><a href="#">订单列表</a></li>
            </ul>
        </div>
        <!--当前位置 end-->
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <li class="click" id="btnAdd" runat="server"><span>
                    <img src="../images/t01.png" /></span><font>代客下单</font></li>
                <asp:Button ID="btnRAudit" Text="审核退回" runat="server" OnClick="btnRAudit_Click" Style="display: none;" />
                <%--<li class="click2"><span><img src="../images/t02.png" /></span>编辑</li>--%>
                <li id="DelAll"  runat="server"><span><img src="../images/t03.png" /></span>批量删除</li>
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li id="Search"><span>
                        <img src="../images/t04.png" /></span>搜索</li>
                    <%--<uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />--%>
                    <li id="export"><span><img src="../images/tp3.png" /></span>导出</li>
                    <li class="liSenior"><span>
                        <img src="../images/t07.png" /></span>高级</li>
                </ul>
                <ul class="toolbar3">
                    <li>订单状态:<select name="OState" runat="server" id="ddrOState" style="  width:90px; " class="downBox">
                        <option value="-2">全部订单</option>
                        <option value="0">待处理订单</option>
                        <option value="1">已完成订单</option>
                        <option value="2">已作废订单</option>
                    </select>
                    </li>
                    <li>订单编号:<input name="txtReceiptNo" type="text" id="txtReceiptNo"
                        runat="server" class="textBox" maxlength="50" /></li>
                    <li>下单时间:<input name="txtCreateDate" runat="server" onclick="var endDate=$dp.$('txtEndCreateDate'); WdatePicker({onpicked:function(){endDate.focus();},maxDate:'#F{$dp.$D(\'txtEndCreateDate\')}'})"
                        style="width: 100px;" id="txtCreateDate" readonly="readonly" type="text" class="Wdate"
                        value="" />&nbsp;-&nbsp;
                        <input name="txtEndCreateDate" runat="server" onclick="WdatePicker({minDate:'#F{$dp.$D(\'txtCreateDate\')}'})"
                            style="width: 100px;" id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate"
                            value="" />
                    </li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <div class="hidden">
            <ul class="toolbar right" style="width: 130px;">
                 <li id="liexcel"><span><img src="../images/tp3.png" /></span>订单详情导出</li>
            </ul>
            <ul class="left" style="width: 80%;">
                <li>每页<input type="text" onkeyup="KeyInt(this)" id="txtPager" name="txtPager" runat="server"
                    class="textBox" style="width: 40px;" />&nbsp;条 </li>
                <li>总价区间:<input name="txtTotalAmount1" id="txtTotalAmount1" onkeyup="KeyInt2(this)"
                    runat="server" type="text" class="textBox2" maxlength="50" />
                    -
                    <input name="txtTotalAmount2" id="txtTotalAmount2" onkeyup="KeyInt2(this)" runat="server"
                        type="text" class="textBox2" maxlength="50" />&nbsp;&nbsp; </li>
                <li>支付状态:<select name="PayState" runat="server" id="ddrPayState" class="downBox">
                    <option value="-1">全部</option>
                    <option value="0">未支付</option>
                    <option value="1">部分支付</option>
                    <option value="2">已支付</option>
                </select>&nbsp;&nbsp; </li>
                <li>代理商名称:<input id="txtDisName" runat="server" type="text" class="textBox" maxlength="50" />&nbsp;&nbsp;
                </li>
            </ul>
        </div>
        <!--信息列表 start-->
        <asp:Repeater runat="server" ID="rptOrder">
            <HeaderTemplate>
                <table class="tablelist" id="TbList">
                    <thead>
                        <tr>
                            <%--<th><input type="checkbox" id="CB_SelAll" onclick="SelectAll(this)"/></th>--%>
                            <th><input type="checkbox" class="" id="FCheck" /></th>
                            <th class="t3">
                                订单编号
                            </th>
                            <th class="t3">
                                代理商名称
                            </th>
                            <th class="t2">
                                下单时间
                            </th>
                            <th class="t1">
                                订单状态
                            </th>
                            <th class="t1">
                                支付状态
                            </th>
                            <th class="t5">
                                订单总价(元)
                            </th>
                            <%--<th class="t1">
                                订单类型
                            </th>--%>
                            <th>
                                订单来源
                            </th>
                            <%--<th>
                                制单人
                            </th>--%>
                            <th class="t5">
                                操作
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr id='tr_<%# Eval("Id") %>'>
                    <%--<td>
                            <asp:CheckBox ID="CB_SelItem" runat="server"></asp:CheckBox>
                            <asp:HiddenField ID="Hd_Id" runat="server" Value='<%# Eval("Id") %>' />
                        </td>--%>
                    <td class="tcle chk"><input type="checkbox" name="RmCheck" class="RmCheck" value="<%#Eval("Id").ToString()%>"/></td>
                    <td>
                        <div class="tcle">
                            <a href='../newOrder/orderdetail.aspx?top=1&KeyID=<%# Common.DesEncrypt(Eval("Id").ToString(), Common.EncryptKey) %>' />
                            <%# Eval("ReceiptNo") %></a>
                        </div>
                    </td>
                    <td>
                        <div class="tcle">
                            <%# Common.GetDis(Convert.ToInt32(Eval("DisID").ToString()),"DisName") %></div>
                    </td>
                    <td>
                        <div class="tc">
                            <%# Eval("CreateDate", "{0:yyyy-MM-dd HH:mm}")%></div>
                    </td>
                    <td>
                        <div class="tc order-state order-state-<%# Eval("OState").ToString() %>">
                            <%# OrderType.GetOState(Eval("OState").ToString(), Eval("IsOutState").ToString())%></div>
                    </td>
                    <td>
                        <div class="tc pay-state pay-state-<%# Eval("PayState").ToString() %>">
                            <%# OrderInfoType.PayState(int.Parse(Eval("PayState").ToString()))%></div>
                    </td>
                    <td>
                        <div class="tc">
                            <%# Convert.ToDecimal(Eval("AuditAmount")).ToString("N")%></div>
                    </td>
                  <%--<td>
                        <div class="tc">
                            <%# OrderInfoType.OType(int.Parse(Eval("OType").ToString()))%></div>
                    </td>--%>
                    <td>
                        <div class="tc">
                            <%# OrderInfoType.AddType(int.Parse(Eval("AddType").ToString()))%></div>
                    </td>
                    <%--<td>
                        <%# Common.GetUserName(Convert.ToInt32(Eval("CreateUserID").ToString()))%>
                    </td>--%>
                    <td align="center">
                        <div class="tc">
                            <%# OrderMeau(Common.DesEncrypt(Eval("Id").ToString(),Common.EncryptKey)) %></div>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
        <!--信息列表 end-->
        <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
        <%--<asp:Button ID="btnVolumeDel" Text="批量删除" runat="server" OnClick="btnVolumeDel_Click"
            Style="display: none" />--%>
        <!--列表分页 start-->
        <div class="pagin">
            <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                NextPageText=">" PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
                ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                TextBeforePageIndexBox="<span class='jump'>跳转到:</span>" CustomInfoSectionWidth="20%"
                CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;" CustomInfoClass="message"
                ShowCustomInfoSection="Left" CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                OnPageChanged="Pager_PageChanged">
            </webdiyer:AspNetPager>
        </div>
        <!--列表分页 end-->
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentList.aspx.cs" Inherits="Company_Pay_PaymentList" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>订单收款补录查询</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                }
            })
        })
        $(document).ready(function () {
            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');

            //录入明细 搜索
            $("#Search").on("click", function () {
               
                    $("#btnSearch").trigger("click");
               
            });

            //新增按钮事件
            $("#btnAdd").click(function () {
                //$(".tip").fadeIn(200);
                window.location.href = 'PaymentEdit.aspx';
            });

        });
        //企业钱包录入详细页面
        function goInfo(Id) {
            window.location.href = 'PaymentInfo.aspx?KeyID=' + Id;
        } //跳入订单详情页面
        function goInfoorder(Id) {
            window.location.href = 'OrderCreateInfo.aspx?KeyID=' + Id + "&go=1";
        }
    </script>
    <script type="text/javascript">
        //只能输入数字验证
        function KeyInt(val) {
            val.value = val.value.replace(/[^\d]/g, '');
        }
    </script>
    <style type="text/css">
         
         </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-2" />
        <div class="rightinfo">
         <!--当前位置 start-->
        <div class="place">
	        <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="#">我要管账</a></li><li>></li>
                <li><a href="#">订单收款补录查询</a></li>
	        </ul>
        </div>
        <!--当前位置 end--> 
        <!--补录明细搜索 Begin-->
        <input id="hid_Alert" type="hidden" />
          <asp:Button ID="btnSearch" Text="搜索" runat="server" onclick="btnSearch_Click" style=" display:none"  />
        <!--补录明细搜索 End-->
        <!--补录明细审核 Begin-->
        <input id="hidShId" type="hidden" runat="server" />
        <input id="hidShIds" type="hidden" runat="server" />
       
        <!--补录明细审核 End-->
            <!--功能按钮 start-->
            <div class="tools">
              <ul class="toolbar left">
                    <li class="click" id="btnAdd" style="display:none;"><span><img src="../images/t01.png" /></span>新增</li>
                    <%--<li class="click2"><span><img src="../images/t02.png" /></span>编辑</li>
                    <li id="VolumeDel"><span><img src="../images/t03.png" /></span>批量删除</li>--%>
                </ul>
                <div class="right">
                    <ul class="toolbar right">
                        <li id="Search"><span><img src="../images/t04.png" alt="" /></span>搜索</li>
                        <uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />
                        <li class="liSenior"><span>
                        <img src="../images/t07.png" /></span>高级</li>
                    </ul>
                    <ul class="toolbar3">
                     <li>代理商名称:
                    <%--<select id="ddldisname" runat="server" class="downBox">
                        <option value="-1">请选择</option>                        
                    </select>--%>
                    
                        <input id="txtDisName" type="text" runat="server" class="textBox"/>   
                    </li>
                      <li>流水帐号:<input id="txtPayCreateNo" type="text" runat="server" class="textBox"/></li>
                        <li>入账时间:
                        <input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtPayCreateDate" readonly="readonly" type="text" class="Wdate" value="" />
                          -
                        <input name="txtPayEnddate" runat="server" onclick="WdatePicker()" id="txtPayEnddate" readonly="readonly" type="text" class="Wdate" value="" />
                       
                        </li>
                    </ul>
                   
                </div>
            </div>
            <!--功能按钮 end-->
            <div class="hidden">
            <ul style="width: 90%;">
                <li>
                    每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server" class="textBox" style=" width:40px;" />&nbsp;行 
                </li>
                <li>款项来源:<select id="ddltype" runat="server" class="downBox" >
                                <option value="-1">请选择</option>
                                <option value="现金">现金</option>
                                <option value="转账汇款">转账汇款</option>
                                <option value="票据">票据</option>
                               <option value="其它">其它</option>
                                </select>&nbsp;&nbsp;</li>                        
            </ul>
        </div>
            <!--信息列表 start-->
            <table class="tablelist" id="TbList">
            <asp:Repeater runat="server" ID="rptPay">
            <HeaderTemplate>
                <thead>
                    <tr>
                     <th>流水帐号</th>
                        <th>代理商名称</th>                       
                        <th>入账金额（元）</th>
                        <th>款项来源</th>
                        <th>入账时间</th>
                        <th>关联订单编号</th>
                       <%-- <th>审核状态</th>--%>
                        <th>备注</th> 
                       <%-- <th style="text-align:center; width:110px">操作</th> --%>
                    </tr>
                </thead>
             </HeaderTemplate>
            <ItemTemplate>
                <tbody>
                    <tr>
                         <td> <a style=" text-decoration:underline;" href="javascript:void(0)"  onclick='goInfo(<%# Eval("ID") %>)'><%# Eval("ID")%> </a></td>
                         <td><%# Common.GetDis(Convert.ToInt32(Eval("DisID")),"DisName")%></td>
                         <td><%# Convert.ToDecimal(Eval("price")).ToString("N")%></td>
                         <td><%# Eval("vdef3")%></td>
                         <td><%# Convert.ToDateTime(Eval("Paytime")).ToString("yyyy-MM-dd")%></td>
                        <%-- <td><%# Common.GetNameBYPreStart(Convert.ToInt32(Eval("AuditState")))%></td>--%>
                        <td>  <a href="javascript:void(0)" onclick='goInfoorder("<%# Common.DesEncrypt(Eval("OrderID").ToString(), Common.EncryptKey) %>")'>                         
                        
                        <%# OrderNO(Eval("OrderID"))%> </a></td>
                         <td><%#  Common.MySubstring(Convert.ToString(Eval("vdef1")), 7, "...")%></td>
                         <%-- <td style="width:110px" align="center">
                           <a href="javascript:void(0)"  onclick='goInfo(<%# Eval("ID") %>)' class="tablelinkQx" id="clickMx"> 查看</a>
                        </td>--%>
                        </tr>
                </tbody>
                </ItemTemplate>
             </asp:Repeater>
            </table>
            <!--信息列表 end-->

            <!--列表分页 start--> 
            <div class="pagin">
                     <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                     NextPageText=">"  PageSize="12" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
          ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                       TextBeforePageIndexBox="<span class='jump'>跳转到:</span>"
                   CustomInfoSectionWidth="20%"  CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;"  CustomInfoClass="message" ShowCustomInfoSection="Left"
                  CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                     CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                    OnPageChanged="Pager_PageChanged" >
                </webdiyer:AspNetPager>
            </div>
            <!--列表分页 end--> 
        </div>

    </form>
</body>
</html>

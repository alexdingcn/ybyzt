<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayExamineList.aspx.cs" Inherits="Company_Pay_PayExamineList" %>
<%@ Register Src="~/Company/UserControl/TextDisList.ascx" TagPrefix="uc1" TagName="DisList" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>预收款审核</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>

    <script src="../js/Pay.js" type="text/javascript"></script>
    <script type="text/javascript">
        //回车事件
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

            //代理商 搜索
            $("#Search").on("click", function () {
                if ($("#txtPager").val() == "" || $("#txtPager").val() == 0) {
                    layerCommon.msg(" 每页显示数量不能为空", IconOption.错误);
                    return false;
                }
                else {
                    $("#btnSearch").trigger("click");
                }
            });
             
            //重置
            $("#li_Reset").click(function () {               
                $("#txtPayCreateDate").val("");
            });
            //审核
            $(".tablelink").click(function () {
                
                        var keyId = $(this).attr("tip");
                        var keyIds = $(this).attr("ptip");
                        //alert(keyId);
                        $("#hidShId").val(keyId);
                        $("#hidShIds").val(keyIds);
                        $("#btnSh").trigger("click");
                    
                    
            });
                });
                //企业钱包录入详细页面
                function goInfo(Id) {
                    window.location.href = 'PayExamineInfo.aspx?KeyID=' + Id;
                }
    </script>
    <script type="text/javascript">
        //只能输入数字验证
        function KeyInt(val) {
            val.value = val.value.replace(/[^\d]/g, '');
        }
    </script>
    <style type="text/css">
        .tip,.tip2{width:400px; height:350px; position:absolute;top:10%; left:30%;background:#fcfdfd;box-shadow:1px 8px 10px 1px #9b9b9b;border-radius:1px;behavior:url(js/pie.htc); display:none; z-index:999;}
        .downBox{ width:160px; height:28px; border:1px solid #eee; margin-left:5px; color:#999; padding-left:2px; }
    .tablelinkQx{background:#056cad; border:1px solid #056cad; color:#fff; padding:1px 7px;}
               .tablelist td a { text-decoration: underline; }
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
                <li><a href="#">预收款审核</a></li>
	        </ul>
        </div>
        <!--当前位置 end--> 
        <!--代理商搜索 Begin-->
        <input id="hid_Alert" type="hidden" />
        <asp:Button ID="btnSearch" Text="搜索" runat="server" onclick="btnSearch_Click" style=" display:none"  />
        <!--代理商搜索 End-->
        <asp:Button ID="btnSh" Text="审核" runat="server" onclick="btnSh_Click" style=" display:none"  />
            <!--功能按钮 start-->
            <div class="tools">
                <div class="right">
                    <ul class="toolbar right">
                        <li id="Search"><span><img src="../images/t04.png" alt="" /></span>搜索</li>
                    </ul>
                    <ul class="toolbar3">
                        <li>转账汇款时间:&nbsp;&nbsp;
                        <input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtPayCreateDate" readonly="readonly" type="text" class="Wdate" value="" />
                        </li>
                        <li>
                            每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server" class="textBox" style=" width:40px;" />行
                        </li>
                    </ul>
                    <ul class="toolbar3">
                        <li>代理商名称:
                         <uc1:DisList runat="server" ID="DisListID" />
                         <input type="hidden" id="hidDisId" runat="server" />
                        <input id="hidShId" type="hidden" runat="server" />
                        <input id="hidShIds" type="hidden" runat="server" />
                        </li>
                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->

            <!--信息列表 start-->
            <table class="tablelist">
            <asp:Repeater runat="server" ID="rptExa">
            <HeaderTemplate>
                <thead>
                    <tr>
                  <th>流水帐号</th>
                    <%--<th>代理商代码</th>--%>
                        <th>代理商名称</th>                        
                        <th>转账汇款金额(元)</th>
                        <th>转账汇款日期</th>
                        <th>款项类型</th>
                        <th>审批状态</th>
                        <%--<th style="width:110px; text-align:center;">操作</th>--%>
                    </tr>
                </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tbody>
                    <tr>
                    <td> <a href="javascript:void(0)"  onclick='goInfo(<%# Eval("ID") %>)'><%#Eval("ID") %></a></td>
                         <td><%# Common.GetDis(Convert.ToInt32(Eval("DisID")), "DisName")%></td>
                         <%--<td><%# Common.GetDis(Convert.ToInt32(Eval("DisID")), "DisCode")%></td>--%>
                         <td><%# Convert.ToDecimal(Eval("Price")).ToString("0.00")%></td>
                         <td><%# Convert.ToDateTime(Eval("CreatDate")).ToString("yyyy-MM-dd")%></td>
                          <td><%# Common.GetPrePayStartName(Convert.ToInt32(Eval("PreType")))%></td>
                         <td><%# Common.GetNameBYPreStart(Convert.ToInt32(Eval("AuditState")))%></td>
                         <%--<td style="width:110px;" align="center">  
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

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleManInfo.aspx.cs" Inherits="Admin_OrgManage_SaleManInfo" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>销售人员维护详细</title>
    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
     <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
//            $("li#libtnDel").on("click", function () {
//                confirm("确认删除？", function () { $("#btnDel").trigger("click"); }, "提示");
//            })

            $("li#libtnEdit").on("click", function () {
                location.href = "SaleManEdit.aspx?KeyID="+<%= Request.QueryString["KeyID"] %>;
            });

            if('<%=SMtype %>'=='<%=(int)Enums.DisSMType.业务员%>') {  
                    $("#lblSMType").parent("td").removeAttr("colspan");
                    $("td.TDSMPrent").show();
             } 
             else 
             {
                    $("#lblSMType").parent("td").attr("colspan","3");
                    $("td.TDSMPrent").hide();
             }
             $("#btnbd").click(function () {
                var Id = $("#hidCompId").val();
                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度
                var index = layerCommon.openWindow('选择代理商', '/Company/UserControl/SelectDisList3.aspx?compid=' + Id+'&disid='+$("#hidselectDis").val()+'&keyid='+ <%= Request.QueryString["KeyID"] %>, '880px', '450px'); //记录弹出对象
                $("#hid_Alert").val(index); //记录弹出对象
            });
        });

        function selectDis(id) {
            if (id.toString() != "") {
                layerCommon.layerClose($("#hid_Alert").val())
                if ($("#hidselectDis").val() == "") {
                    $("#hidselectDis").val(id);
                }
                else {
                    $("#hidselectDis").val($("#hidselectDis").val() + "," + id);
                }
                $("#btndis").click();
            }
        }

        function Adel(showid) {
            $("#hiddel").val(showid);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />
   
    <div class="rightinfo">
         <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx" target="_top">我的桌面 </a></li><li>></li>
                <li><a href="../PmtManager/SaleManList.aspx">销售人员维护</a></li><li>></li>
                <li><a href="#">销售人员维护详细</a></li>
            </ul>
        </div>
        <div class="tools">
            <ul class="toolbar left">
                <li id="libtnEdit"><span>
                    <img src="../../Company/images/t02.png" /></span>编辑</li>
                    <li id="btnbd" runat="server"><span><img src="../images/t01.png"></span>绑定代理商</li>
                <%--<li id="libtnDel"><span>
                    <img src="../../Company/images/t03.png" /></span>删除<input type="button" runat="server"
                        id="btnDel" onserverclick="btn_Del" style="display: none;" /></li>--%>
                <li id="lblbtnback" onclick="location.href = 'SaleManList.aspx'"><span>
                    <img src="../../Company/images/tp3.png" /></span>返回</li>
            </ul>
        </div>
        <div class="div_content">
            <table class="tb">
                <tbody>
                    <tr>
                        <td width="10%">
                            <span>销售人员类型</span>
                        </td>
                        <td width="20%" colspan="3">
                            <label runat="server" id="lblSMType">
                            </label>
                        </td>
                        <td width="10%" class="TDSMPrent" style=" display:none;">
                            <span>销售人员经理</span>
                        </td>
                        <td width="20%" class="TDSMPrent" style=" display:none;">
                            <label runat="server" id="lblSMParent">
                            </label>
                        </td>

                    </tr>
                    <tr>
                     <td width="10%">
                            <span>销售人员名称</span>
                        </td>
                        <td width="20%">
                            <label runat="server" id="lblSaleName">
                            </label>
                        </td>
                        <td width="10%">
                            <span>销售人员编码</span>
                        </td>
                        <td width="20%">
                            <label runat="server" id="lblSaleCode">
                            </label>
                        </td>
                    </tr>
                    <tr> 
                        <td width="10%">
                            <span>联系手机</span>
                        </td>
                        <td width="20%">
                            <label runat="server" id="lblPhone">
                            </label>
                        </td>
                        <td>
                            <span>邮箱</span>
                        </td>
                        <td>
                            <label runat="server" id="lblEmail">
                            </label>
                        </td>                       
                    </tr>
                    <tr>
                        <td>
                            <span>是否启用</span>
                        </td>
                        <td colspan="3">
                            <label runat="server" id="lblstate">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>备注</span>
                        </td>
                        <td colspan="3">
                            <label runat="server" id="lblRemark">
                            </label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div id="tbdis" runat="server" style=" display:none" >
            <table class="tablelist">
                <thead>
                    <tr>
                        <th>
                            序号
                        </th>
                        <th>
                            代理商名称
                        </th>
                        <%--<th>
                        代理商简称
                    </th>--%>
                        <th>
                            代理商分类
                        </th>
                        <th>
                            代理商区域
                        </th>
                        <th>
                            联系人
                        </th>
                        <th>
                            联系人手机
                        </th>
                        <th>
                            操作
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="Rpt_Dis" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Container.ItemIndex + 1%>
                                </td>
                                <td>
                                    <%# Eval("DisName")%>
                                    <input type="hidden" value="<%#Eval("DisName")%>" id="Hid_DisName" />
                                </td>
                                <%--<td>
                                <%# Eval("ShortName")%>
                            </td>--%>
                                <td>
                                    <%# Common.GetDisTypeNameById(Eval("DisTypeID").ToString().ToInt(0))%>
                                </td>
                                <td>
                                    <%# Common.GetDisAreaNameById(Eval("AreaID").ToString().ToInt(0))%>
                                </td>
                                <td>
                                    <%# Eval("principal") %>
                                </td>
                                <td>
                                    <%# Eval("phone") %>
                                </td>
                                <td>
                                    <a id="A1" onclick='<%# "Adel(\""+Eval("id")+"\")"  %>' onserverclick="A_Del" runat="server"
                                        style="cursor: pointer;">
                                        <img src="../../images/del.gif" alt="" /></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <input type="hidden" id="hidCompId" runat="server" />
            <input type="hidden" id="hiddel" runat="server" />
            <input id="hid_Alert" type="hidden" />
            <input id="hidselectDis" type="hidden" runat="server" />
            <input id="btndis" type="button" onserverclick="Btn_Dis" style="display: none;" runat="server" />
            
        </div>
        

    </div>
    </form>
</body>
</html>
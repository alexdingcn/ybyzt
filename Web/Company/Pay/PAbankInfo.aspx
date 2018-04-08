<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PAbankInfo.aspx.cs" Inherits="Company_Pay_PAbankInfo" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>收款帐号管理</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../js/Pay.js" type="text/javascript"></script>
    <script type="text/javascript">
    $(document).ready(function () {
        //返回
        $("#cancel").click(function () {
            history.go(-1);
            //goInfo(<%# Eval("ID") %>)'
            window.location.href = 'PayAccountList.aspx';
        });
        $("#Edit").click(function () {
            window.location.href = 'PAbankEdit.aspx?paid=<%=Paid %>&KeyID=<%=KeyID %>';
        });

    });

    //选择代理商返回
    function SelectMaterialsReturn(materialCodes) {
        if (materialCodes != "") {
            form1.txtMaterialCodes.value = materialCodes;
            document.getElementById("btnSelectMaterialsReturn").click();
            var th = $("#hid_Alert").val();
            this.CloseDialog(th);
        } 
    }
    </script>
    <style type="text/css">
        input[type='text']
        {
            width: 170px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-6" />

    <div class="rightinfo">
    <!--当前位置 start-->
    <div class="place">
        <ul class="placeul">
            <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
            <li><a href="../Pay/PayAccountList.aspx" runat="server" id="atitle">基本设置</a></li><li>></li>
            <li><a href="#">收款帐号管理</a></li>
        </ul>
    </div>
    <input id="hid_Alert" type="hidden" />
    <!--当前位置 end-->
     <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                    <li id="Edit" runat="server"><span><img src="../images/t02.png" /></span>编辑</li>
                    <%--<li id="Del" runat="server"><span><img src="../images/t03.png" /></span>删除</li> --%>  
                    <li id="cancel" runat="server"><span><img src="../images/tp3.png" /></span>返回</li>
                </ul>
            </div>
            <!--功能按钮 end-->
        <div class="div_content">
            <!--销售订单主体 start-->
            <%--<div style="padding-left: 70px; ">--%>
            <div class="lbtb">
                <table class="dh">
                    <tr>
                     <td width="110">
                            <span>
                                账户类型</span>
                        </td>
                        <td>
                            <label id="lblType" runat="server"></label>
                        </td>
                    
                        <td width="110">
                            <span>
                                开户银行</span>
                        </td>
                        <td colspan="3">                            
                            <label id="lblddlbank" runat="server"></label>
                        </td>
                       
                    </tr>
                    <tr>
                     <td width="110">
                            <span>
                                银行账户名称</span>
                        </td>
                        <td>
                            <label id="lblDisUser" runat="server"></label>
                        </td>
                        <td>
                            <span>
                                银行账户号码</span>
                        </td>
                        <td>
                                <label id="lblbankcode" runat="server"></label>
                        </td>
                       
                    </tr>
                    <tr>
                     <td>
                            <span>
                               开户行地址</span>
                        </td>
                        <td>
                          
                             <label id="lblbankAddress" runat="server"></label>   
                        </td>
                        <td>
                            <span>开户行所在省/市</span>
                        </td>
                        <td>
                           
                            <label id="lblprivateCity" runat="server"></label>   
                        </td>
                       
                    </tr>
                    <tr> <td>
                            <span>是否默认</span>
                        </td>
                        <td>
                            
                                 <label id="lblisno" runat="server"></label>   
                        </td>
                       <td>
                            <span>是否复核</span>
                        </td>
                        <td>
                            
                                 <label id="lblstart" runat="server"></label>   
                        </td>
                        </tr>
                    <tr>
                        <td>
                            <span style="height: 60px;">备注</span>
                        </td>
                        <td colspan="3">
                            <label id="lblremake" runat="server"></label>   
                        </td>
                    </tr>
                </table>
            </div>
            <%--</div>--%>
            <!--销售订单主体 start-->
            <!--清除浮动-->
            <div style="clear: none;">
            </div>
            <!--销售订单明细 start-->
            <div style="padding-top: 10px;">
                <!--选择商品按钮 start-->
                <%--<div class="tools">
                    <div class="click" id="btnDis">
                        <ul class="toolbar left">
                            <li><span>
                                <img src="../images/t04.png" /></span>关联代理商 </li>
                        </ul>
                    </div>
                </div>--%>
                <!--选择商品按钮 start-->
                <!--新增商品列表 start-->
                <div>
                    <div class="edittable2">
                        <asp:GridView ID="gvDtl" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                            GridLines="Horizontal" CellPadding="0" Width="100%" CssClass="list" AllowSorting="false"
                            ShowFooter="false" OnRowCommand="gvDtl_RowCommand" EmptyDataRowStyle-HorizontalAlign="Center"
                            OnRowDataBound="gvDtl_RowDataBound" OnDataBound="gvDtl_DataBound">
                            <FooterStyle CssClass="list-title" />
                            <Columns>
                                <asp:TemplateField HeaderText="序号">
                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGoodsId" Text='<%# Eval("ID") %>' runat="server" Style="display: none;"></asp:Label>
                                        <asp:Label ID="lblIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                  
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="代理商名称">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDisName" Text=' <%# Eval("DisName")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                                <asp:TemplateField HeaderText="代理商区域">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblAreaID" Text=' <%# Eval("AreaID")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               
                                <asp:TemplateField HeaderText="代理商等级">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                    <ItemTemplate>
                                             <asp:Label ID="lblDisLevel" Text=' <%# Eval("DisLevel")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                <asp:TemplateField HeaderText="详细地址">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                    <ItemTemplate>
                                             <asp:Label ID="lblAddress" Text=' <%# Eval("Address")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                 <asp:TemplateField HeaderText="联系人">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle HorizontalAlign="Center" Wrap="True" />
                                    <ItemTemplate>
                                             <asp:Label ID="lblPrincipal" Text=' <%# Eval("Principal")%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                                 
                               <%-- <asp:TemplateField HeaderText="删除">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" />
                                    <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnDelete" runat="server" Text="<img src=../../images/del.gif width=16 height=16 border=0>"
                                            CausesValidation="false" CommandName="del" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                            <PagerStyle CssClass="list-title" />
                            <HeaderStyle CssClass="list-title" />
                            <EmptyDataTemplate>
                                此账户未关联代理商
                            </EmptyDataTemplate>
                            <RowStyle HorizontalAlign="Center" />
                        </asp:GridView>
                    </div>
                </div>
                <!--新增商品列表 end-->
            </div>
            <!--销售订单明细 start-->
            <div class="footerBtn">
            <input type="button" id="btnSelectMaterialsReturn" runat="server" onserverclick="btnSelectMaterialsReturn_ServerClick" />
           <input id="txtMaterialCodes" type="hidden" name="txtMaterialCodes" runat="server" />
           <input id="txtGoodsCodes" type="hidden" name="txtGoodsCodes" runat="server" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayAccountInfo.aspx.cs" Inherits="Company_Order_PayAccountInfo" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>绑定收款详细</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>

    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            //返回
            $("#cancel").click(function () {
                // history.go(-1);
                window.location.href = 'PayAccountList.aspx';
            });

            $("#Edit").click(function () {
                window.location.href = 'payAccountAdd.aspx?KeyID=<%=KeyID %>';
            });

            $("#btnBank").click(function () {
                window.location.href = 'PAbankEdit.aspx?paid=<%=KeyID %>';
            });
          

             //生成订单
            $("#CopyOrder").on("click", function () {
                $("#btnCopyOrder").trigger("click");
            });
        });

        //进入银行卡详细页面 （paid 收款帐号表Id，keyid 是银行卡表ID）
        function goInfoBank(Id) {
            window.location.href = 'PAbankInfo.aspx?paid=<%=KeyID %>&KeyID=' + Id;
        }
    </script>
    <style type="text/css">
    .list td a { text-decoration: underline; }
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
                <li><a href="../Pay/PayAccountList.aspx" runat="server" id="atitle">绑定收款帐号</a></li><li>></li>
                <li><a href="#">绑定收款详细</a></li>
	        </ul>
        </div>
        <!--当前位置 end-->
        <input type="hidden" id="hid_Alert" />

            <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                    <%--<li id="Edit" runat="server"><span><img src="../images/t02.png" /></span>编辑</li>
                    <li id="Del" runat="server"><span><img src="../images/t03.png" /></span>删除</li> --%>  
                    <li id="cancel" runat="server"><span><img src="../images/tp3.png" /></span>返回</li>
                </ul>
            </div>
            <!--功能按钮 end-->
            <div class="div_content">
                <!--销售订单主体 start-->
                <div style="padding-left: 10px; ">
                    <div class="lbtb">
                        <table class="dh">
                            <tr>
                               <td style="width:15%;"><span>厂商名称</span></td>
                               <td style="width:30%;"><label id="lblcomp" runat="server"></label></td>
                               
                                 <td><span>区域</span></td>
                                <td><label id="lblqy" runat="server"></label></td>
                            </tr>
                            <tr>
                                <td><span>中金账户名称</span></td>
                                <td>
                                    <label id="lblpayname" runat="server"></label>
                                </td>

                                <td style="width:15%;"><span>中金账户号码</span></td>
                               <td style="width:30%;">
                                    <label id="lblpaycode" runat="server"></label>
                                </td>
                               
                            </tr>  
                             <tr>
                                <td><span>机构代码</span></td>
                                <td>
                                    <label id="lblorgcode" runat="server"></label>
                                </td>

                                <td><span>账户类型</span></td>
                                <td><label id="lbltype" runat="server"></label></td>
                            </tr>                               
                            <tr>
                                <td style=" background: #f6f6f6 none repeat scroll 0 0;"><span style=" height:auto;">备注</span></td>
                                <td colspan="3"><label id="lblRemark" runat="server"></label></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <!--销售订单主体 end-->

                <!--清除浮动-->
                <div style="clear: none;"></div>
                <!--关联银行卡明细 start-->
                <div style="padding-top: 10px;">
                 <!--新增银行卡按钮 begin-->
                <div class="tools" >
                    <div class="click" id="btnBank">
                        <ul class="toolbar left">
                            <li>
                                <span><img src="../images/t04.png" /></span>添加绑定银行卡
                            </li>
                        </ul>
                    </div>
                </div>
                <!--新增银行卡按钮 end-->
                    <!--新增商品列表 start-->
                    <div style="padding-left: 10px;">
                        <div class="edittable2">
                            <asp:Repeater ID="rpDtl" runat="server"   OnItemCommand="rptPAcount_ItemCommand">
                                <HeaderTemplate>
                                    <table class="list">
                                        <tr class="list-title">
                                        <th>序号</th>
                                            <th>银行名称</th>
                                            <th>持卡人名称</th>
                                            <th>银行卡号</th>
                                            <th>开户行地址</th>
                                            <th>开户所在省/市</th>
                                            <th>是否默认</th>   
                                              <th style="text-align:center;">操作</th>                                        
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="list-title" style=" text-align:center;">
                                        <td>
                                            <asp:Label ID="lblGoodsId" Text='<%# Eval("ID") %>' runat="server" Style="display: none;"></asp:Label>
                                            <%# Container.ItemIndex + 1 %>
                                        </td>
                                        <td>   <a href="javascript:void(0)"  onclick='goInfoBank(<%# Eval("ID") %>)' id="A1"><%# new Hi.BLL.PAY_PrePayment().GetBankNameBYbankID(Convert.ToInt32(Eval("BankID")).ToString())%></a></td>
                                        <td><%# Eval("AccountName")%></td>
                                        <td><%# Eval("bankcode")%></td>
                                        <td><%# Eval("bankAddress")%></td>
                                        <td><%# Eval("bankPrivate")%>/<%# Eval("bankCity")%></td>
                                        <td><%#Convert.ToInt32(Eval("Isno"))==1?"是":"否"%></td>
                                        <td style="width:150px;" align="center">                          
                           <%-- <a href="javascript:void(0)"  onclick='goInfoBank(<%# Eval("ID") %>)' class="tablelink" id="clickMx"> 查看</a>
                           --%> <asp:LinkButton ID="btnDel" runat="server" CommandName="del" CssClass="tablelink" CommandArgument='<%# Eval("Id") %>'>删除</asp:LinkButton>
                      
                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                        <tr id="tr" runat="server" visible='<%#bool.Parse((rpDtl.Items.Count==0).ToString())%>'>
                                            <td colspan="9" align="center">
                                                无匹配数据
                                            </td>
                                        </tr>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <!--新增商品列表 end-->
                </div>
                <!--销售订单明细 end-->
            </div>
        </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RepGoodsList.aspx.cs" Inherits="Admin_Systems_RepGoodsList" EnableEventValidation="false" %>
<%@ Register TagPrefix="webdiyer" Namespace="Wuqi.Webdiyer" Assembly="AspNetPager, Version=7.2.0.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Src="~/Admin/UserControl/Org.ascx" TagPrefix="uc1" TagName="Org" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>商品销售报表</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
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
            //订单生成 搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });
            ///重置
            $("#li_Reset").click(function () {
                window.location.href = 'RepGoodsList.aspx';
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="hid_Alert">
    <input type="hidden" id="hidCompId" runat="server"/>
    <input type="hidden" id="salemanid" runat="server" />
    <input type="hidden" id="aspx" runat="server" value="RepGoodsList.aspx" />
    <uc1:Org runat="server" ID="txtDisArea" />
    <input type="hidden" id="hid" runat="server" />
        <!--当前位置 start-->
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">报表查询</a><i>></i>
            <a href="RepGoodsList.aspx">商品销售数据</a>
        </div>
        <!--当前位置 end--> 

        <asp:Button ID="btnSearch" Text="搜索" runat="server" onclick="btnSearch_Click" style=" display:none"  />
            <!--功能按钮 start-->
            <div class="tools">
                <ul class="toolbar left">
                </ul>
                <div class="right">
                    <ul class="toolbar right">
                        <li id="Search"><span><img src="../../Company/images/t04.png" /></span>搜索</li>
                        <li id="li_Reset"><span><img src="../../Company/images/t06.png" /></span>重置</li>
                        <uc1:ToExcel runat="server" ID="ToExcel" contect="TbRepGoodsList" Visible="true" />
                        <li class="liSenior"><span><img src="../../Company/images/t07.png" /></span>高级</li>
                    </ul>
                    <ul class="toolbar3">
                        <li>厂商名称:<input name="txtCompName" type="text" id="txtCompName" runat="server"
                            class="textBox txtCompName" />
                        </li>                       
                        <li>
                            商品名称:<input name="txtGoodsName" type="text" id="txtGoodsName" runat="server" class="textBox"/>
                        </li>
                        <li>起止日期：
                        <input name="txtBCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;" id="txtBCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;
                        -&nbsp;
                        <input name="txtECreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;" id="txtECreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;&nbsp;
                        </li>
                        <%--<li>商品类别:
                            <input name="txt_product_class" readonly="readonly" type="text" class="textBox txt_product_class"
                                id="txt_product_class" runat="server" style="cursor: pointer;" />
                            <input type="hidden" name="hid_product_class" class="hid_product_class" runat="server"
                                id="hid_product_class" />
                        </li>--%>
                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->
            <div class="hidden" style="display:none;">
               <ul style=" width:900px;">
                  <li>
                     每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server" class="textBox" style=" width:40px;" />&nbsp;条
                  </li>
                  <li>业务员:<asp:DropDownList runat="server" ID="SaleMan" CssClass="downBox" ></asp:DropDownList>&nbsp;&nbsp;</li>
                  <li>机构名称:<asp:DropDownList runat="server" ID="Org" CssClass="downBox" ></asp:DropDownList>&nbsp;&nbsp;</li>
                  
               </ul>
             </div>
            <!--信息列表 start-->
                <asp:Repeater ID="rptOrder" runat="server" >
                    <HeaderTemplate>
                        <table class="tablelist" id="TbRepGoodsList">
                            <thead>
                                <tr>
                                    <th>商品名称</th>
                                    <th>厂商名称</th>
                                    <%--<th>商品代码</th>--%>
                                    <th>商品分类</th>
                                    <th>商品销售数量</th>
                                    <th>商品销售金额</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id='tr_<%# Eval("goodsCode") %>' >
                            <td><%# Eval("goodsName").ToString()%></td>
                            <td><%# Common.GetCompValue(Convert.ToInt32(Eval("CompID").ToString()), "CompName")%></td>
                            <%--<td><%# Eval("goodsCode").ToString()%></td>--%>
                            <td><%# Eval("categoryName").ToString()%></td>
                            <td><%# Convert.ToDecimal(Eval("goodsNum") == DBNull.Value ? 0 : Eval("goodsNum")).ToString("N")%></td>
                            <td><%# Convert.ToDecimal(Eval("sumAmount") == DBNull.Value ? 0 : Eval("sumAmount")).ToString("N")%></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            <tr id="trTotal" runat="server" visible='<%#bool.Parse((Pager.PageCount == Pager.CurrentPageIndex&&rptOrder.Items.Count!=0).ToString())%>'>
                                <td><font color="red">总计</font></td>
                                <td colspan="3">&nbsp;</td>
                                <td>
                                    <asp:label ID="total" runat="server" Text="" style="color:Red;"><%=ta.ToString("N") %></asp:label>
                                </td>
                            </tr>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
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
        <div><span style="padding: 20px 0px 0px 20px;color:red;">备注：销售统计订单为已审核、已发货、已到货状态的订单。</span></div>
    </form>
</body>
</html>

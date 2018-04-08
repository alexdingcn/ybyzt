<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayAccountList.aspx.cs" Inherits="Admin_Systems_PayAccountList" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>收款帐号管理</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../Company/css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../Company/js/Pay.js" type="text/javascript"></script>

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

            $(".tablelink").click(function () {
                //弹出录入添加层和遮罩层
                $(".tip").fadeIn(200);
                $(".opacity").fadeIn(200);

                //录入代理商ID
                var id = $(this).attr("tip");
                var name = $(this).attr("Pname");
                //alert(id);
                //alert(name);
                $("#txtPayCreateDis").val(id);
                $("#txtPayCreateDisName").val(name);
                $("#txtPayCreateDisCode").val(code);
            });
            $(".tiptop a").click(function () {
                $(".tip").fadeOut(200);
                $(".opacity").fadeOut(200);
                $("input[type='text']").val("");
            });

            $(".cancel").click(function () {

                $(".tip").fadeOut(100);
                $(".opacity").fadeOut(100);
                $("input[type='text']").val("");
            });
           
            //新增按钮事件
            $("#btnAdd").click(function () {
                //$(".tip").fadeIn(200);
                window.location.href = 'PAbankEdit.aspx?KeyID=<%=KeyID %>';
            });
         
            //批量删除
            $("#VolumeDel").on("click", function () {
                //$("#btnVolumeDel").trigger("click");
                fromDel('提示', '确认删除吗？', Del);

            });

            //返回
            $("#back").click(function () {
                window.location.href = 'CompInfo.aspx?KeyID=<%=KeyID %>';
            });
        });

        //批量删除
        function Del() {
            $("#btnVolumeDel").trigger("click");
        }
        //收款账户管理详细页面
        function goInfo(Id) {
            window.location.href = 'PayBankAuditInfo.aspx?KeyID='+Id;
        }
         
        //js 判断是否选中checkbox
        function fromDel(title, msg, conOK) {
            var delId = { Id: "" };
            var chck = $(".tablelist > tbody > tr > td > .tc >input[type=checkbox]:checked");

            $.each(chck, function (index, chebox) {

                var $Id = $(chebox).siblings("input[type=hidden]:eq(0)");

                if ($Id.length > 0) {
                    delId.Id += $Id.val() + ",";
                }
            });

            if (delId.Id.length <= 0) {
                layerCommon.msg("请选择要删除的选项", IconOption.错误);
                return false;
            } else {
                layerCommon.confirm(msg, conOK);
            }
        }

    </script>
    <style type="text/css">
      
    </style>
</head>
<body>
    <form id="form1" runat="server">

    <uc1:CompRemove runat="server" ID="Remove" ModuleID="4" />
            <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
           <a href="#" runat="server" id="Btitle">厂商管理</a><i>></i>
           <a href="#" runat="server" id="Atitle" >收款帐号维护</a>
        </div>
        <!--代理商搜索 Begin-->
        <input id="hid_Alert" type="hidden" />
        <asp:Button ID="btnSearch" Text="搜索" runat="server" onclick="btnSearch_Click" style=" display:none"  />
        <asp:Button ID="btnVolumeDel" Text="批量删除" runat="server" onclick="btnVolumeDel_Click" style=" display:none"  />
        
        <!--代理商搜索 End-->
        
            <!--功能按钮 start-->
            <div class="tools">
             <ul class="toolbar left">
                    <li class="click" id="btnAdd"><span><img src="../../Company/images/t01.png"/></span><font color="red">新增</font></li>
                    <%--<li class="click2" id="btnEdit"><span><img src="../../Company/images/t02.png" /></span>编辑</li>--%>
                    <li id="VolumeDel"><span><img src="../../Company/images/t03.png" /></span>删除</li>
                    <li id="back" ><span><img src="../../Company/images/tp3.png" /></span>返回</li>

                </ul>
            <div class="right">
                    <ul class="toolbar right">
                        <li id="Search"><span><img src="../../Company/images/t04.png" alt="" /></span>搜索</li>
                        <uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />
                    </ul>
                    <ul class="toolbar3">
                        <li>开户银行:
                         <input name="txtbankname" type="text" id="txtbankname" runat="server" class="textBox" />
                           <%-- <uc1:SelectComp runat="server" ID="txtcompID" style=" margin-left:2px;"  />--%>
                        </li>
                        <li>
                            每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server" class="textBox" style=" width:40px;" />行
                        </li>
                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->

            <!--信息列表 start-->
            <table class="tablelist" id="TbList">
            <asp:Repeater runat="server" ID="rptPAcount"  OnItemCommand="rptPAcount_ItemCommand">
            <HeaderTemplate>
                <thead>
                    <tr>
                        <th class="t7"><input type="checkbox" id="CB_SelAll" onclick="SelectAll(this)"/></th>
                        <th class="t8">序号</th>
                        <th class="t3">开户银行</th>
                        <th class="t3">厂商名称</th>
                        <th class="t1">账户名称</th>
                        <th class="t3">账户号码</th>
                        <th class="t6">开户行地址</th>
                        <th class="t5">开户所在省/市</th>
                        <th class="t4">第一收款账户</th>  
                    </tr>
                </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr id='tr_<%# Eval("Id") %>' >
                        <td>
                            <div class="tc"><asp:CheckBox ID="CB_SelItem" runat="server"></asp:CheckBox>
                            <asp:HiddenField ID="Hd_Id" runat="server" Value='<%# Eval("Id") %>' /></div>
                        </td>
                        <td>
                           <div class="tc"> <asp:Label ID="lblGoodsId" Text='<%# Eval("ID") %>' runat="server" Style="display: none;"></asp:Label>
                            <%# Container.ItemIndex + 1 %></div>
                        </td>
                        <td><div class="tcle"><a href="javascript:void(0)"  onclick='goInfo(<%# Eval("ID") %>)' id="A1"><%# new Hi.BLL.PAY_PrePayment().GetBankNameBYbankID(Eval("BankID").ToString()) %></a></div></td>
                                      
                        <td><div class="tcle"><%# Common.GetCompValue(Convert .ToInt32(Eval("CompID")),"CompName")%></div></td>
                        <td><div class="tc"><%# Eval("AccountName")%></div></td>
                        <td><div class="tcle"><%# Eval("bankcode")%></div></td>
                        <td><div class="tcle"><%# Eval("bankAddress")%></div></td>
                        <td><div class="tc"><%# Eval("bankPrivate")%>/<%# Eval("bankCity")%></div></td>
                        <td><div class="tc"><%#Convert.ToInt32(Eval("Isno"))==1?"是":"否"%></div></td>
                       
                    </tr>
               
                </ItemTemplate>
                 <FooterTemplate>
                                        <tr id="tr" runat="server" visible='<%#bool.Parse((rptPAcount.Items.Count==0).ToString())%>'>
                                            <td colspan="9" align="center">
                                                无匹配数据
                                            </td>
                                        </tr>
                                    </table>
                                </FooterTemplate>
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
       
        
        <input id="Key_compID" runat="server" type="hidden"/>
    </form>
</body>
</html>

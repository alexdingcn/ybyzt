<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayBankAuditList.aspx.cs"
    Inherits="Company_Pay_PayBankAuditList" %>

<%@ Register TagPrefix="uc1" TagName="SelectComp" Src="~/Admin/UserControl/TextCompList.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>收款帐号管理</title>
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
                window.location.href = 'PAbankEdit.aspx';
            });
            //批量删除
            $("#VolumeDel").on("click", function () {
                //$("#btnVolumeDel").trigger("click");
                fromDel('提示', '确认删除吗？', Del);
            });
            ///重置
            $("#li_Reset").click(function () {
                window.location.href = 'PayBankAuditList.aspx';
            });
            //代理商 搜索
            $("#Search").on("click", function () {
                if ($("#txtPager").val() == "" || $("#txtPager").val() == 0) {
                    layerCommon.msg(" 每页显示数量不能为空", IconOption.错误);
                    return false;
                }
                else {
                    $("#btnSearch").trigger("click");
                    $("#pageNoo").trigger("click");
                }
            });



            //重置
            $("#li_Reset").click(function () {
                $("#txtDisID").val("");
                $("#hidDisId").val("");
            });
        });


        //批量删除
        function Del() {
            $("#btnVolumeDel").trigger("click");
        }
        //收款账户管理详细页面
        function goInfo(Id) {
            window.location.href = 'PayBankAuditInfo.aspx?KeyID=' + Id;
        }

        //只能输入数字验证
        function KeyInt(val) {
            val.value = val.value.replace(/[^\d]/g, '');
        }

        //js 判断是否选中checkbox
        function fromDel(title, msg, conOK) {
            var delId = { Id: "" };
            var chck = $(".tablelist > tbody > tr > td > input[type=checkbox]:checked");

            $.each(chck, function (index, chebox) {

                var $Id = $(chebox).siblings("input[type=hidden]:eq(0)");

                if ($Id.length > 0) {
                    delId.Id += $Id.val() + ",";
                }
            });

            if (delId.Id.length <= 0) {
                layerCommon.msg(" 请选择要删除的选项", IconOption.错误);
                return false;
            } else {
                layerCommon.confirm(msg, conOK);
            }
        }
    </script>
    <style type="text/css">
        /*tip样式*/
        .tip
        {
            width: 400px;
            height: 350px;
            position: absolute;
            top: 10%;
            left: 30%;
            background: #fcfdfd;
            display: none;
            z-index: 999;
        }
        .opacity
        {
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: #949595;
            -moz-opacity: 0.8;
            z-index: 998;
        }
        .downBox
        {
            width: 150px;
            height: 28px;
            border: 1px solid #A9BAC9;
            margin-left: 5px;
            color: red;
            padding-left: 2px;
        }
        .tablelinkQx
        {
            background: #056cad;
            border: 1px solid #056cad;
            color: #fff;
            padding: 1px 7px;
        }
        .tipinfo .lbarea
        {
            height: 80px;
            line-height: 30px;
            padding-top: 10px;
            overflow: hidden;
        }
        .tipinfo .lbarea span
        {
            display: inline-block;
            text-align: right;
            width: 150px;
            position: relative;
            top: -30px;
        }
        .tablelist td a
        {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-2" />
   
    <!--代理商搜索 Begin-->
    <input id="hid_Alert" type="hidden" />
    <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
    <asp:Button ID="btnVolumeDel" Text="批量删除" runat="server" OnClick="btnVolumeDel_Click"
        Style="display: none" />
    <!--代理商搜索 End-->
    <div class="rightinfo">
         <!--当前位置 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
                <li><a href="../Pay/PayAccountList.aspx" runat="server" id="atitle">基本设置</a></li><li>></li>
                <li><a href="#">收款帐号管理</a></li>
            </ul>
        </div>
        <!--当前位置 end-->

        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <%--<li class="click" id="btnAdd"><span><img src="../images/t01.png" /></span>新增</li>
                    <li class="click2"><span><img src="../images/t02.png" /></span>编辑</li>
                    <li id="VolumeDel"><span><img src="../images/t03.png" /></span>批量删除</li>--%>
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li id="Search"><span>
                        <img src="../images/t04.png" alt="" /></span>搜索</li>
                </ul>
                <ul class="toolbar3">
                    <li>厂商名称:
                        <uc1:SelectComp runat="server" ID="txtcompID" style="margin-left: 2px;" />
                    </li>
                    <li>每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                        class="textBox" style="width: 40px;" />行 </li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <!--信息列表 start-->
        <table class="tablelist">
            <asp:Repeater runat="server" ID="rptPAcount" OnItemCommand="rptPAcount_ItemCommand">
                <HeaderTemplate>
                    <thead>
                        <tr>
                            <th>
                                序号
                            </th>
                            <th>
                                开户银行
                            </th>
                            <th>
                                厂商名称
                            </th>
                            
                            <th>
                                账户名称
                            </th>
                            <th>
                                账户号码
                            </th>
                            <th>
                                开户行地址
                            </th>
                            <th>
                                开户所在省/市
                            </th>
                            <th>
                                是否默认
                            </th>
                             <th>复核状态</th>  
                            <%-- <th style="text-align:center;">操作</th> --%>
                        </tr>
                    </thead>
                    <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id='tr_<%# Eval("Id") %>'>
                        <td>
                            <asp:Label ID="lblGoodsId" Text='<%# Eval("ID") %>' runat="server" Style="display: none;"></asp:Label>
                            <%# Container.ItemIndex + 1 %>
                        </td>
                         <td>
                            <a href="javascript:void(0)" onclick='goInfo(<%# Eval("ID") %>)' id="A1">
                                <%# new Hi.BLL.PAY_PrePayment().GetBankNameBYbankID(Eval("BankID").ToString())%></a>
                        </td>
                        <td><%# Common.GetCompValue(Convert .ToInt32(Eval("CompID")),"CompName")%></td>
                       
                         
                        <td>
                            <%# Eval("AccountName")%>
                        </td>
                        <td>
                            <%# Eval("bankcode")%>
                        </td>
                        <td>
                            <%# Eval("bankAddress")%>
                        </td>
                        <td>
                            <%# Eval("bankPrivate")%>/<%# Eval("bankCity")%>
                        </td>
                        <td>
                            <%#Convert.ToInt32(Eval("Isno"))==1?"是":"否"%>
                        </td>
                        <td><%#Convert.ToInt32(Eval("Start")) == 1 ? "已复核" : "未复核"%></td>
                        <%--<td style="width:150px;" align="center">                          
                           <%-- <a href="javascript:void(0)"  onclick='goInfoBank(<%# Eval("ID") %>)' class="tablelink" id="clickMx"> 查看</a>
                            <asp:LinkButton ID="btnDel" runat="server" CommandName="del" CssClass="tablelink" CommandArgument='<%# Eval("Id") %>'   >删除</asp:LinkButton>
                      
                        </td>--%>
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

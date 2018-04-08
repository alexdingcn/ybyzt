<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayDisList.aspx.cs" Inherits="Company_Pay_PayDisList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/UserControl/ButtonToExcel.ascx" TagPrefix="uc1" TagName="ToExcel" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>钱包查询</title>
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

            //录入明细查看
//            $(".tablelinkQx").click(function () {

//                var id = $(this).attr("tip");
//                var height = document.documentElement.clientHeight; //计算高度
//                var layerOffsetY = (height - 500) / 2; //计算宽度
//                window.showDialog("补录明细查看", "PayCreateDetails.aspx?keyId=" + id, 1000, 500, layerOffsetY);
//                $(".xubox_close0").click(function () {
//                    window.location.reload();
//                });

//            });

            //重置
            $("#li_Reset").click(function () {
                window.location.href = 'PayDisList.aspx';
            });
        });
    </script>
    <script type="text/javascript">
        //验证用
        function formCheck() {
            var str = "";
            var txtPayCreateDis = $("#txtPayCreateDis").val();
            var txtPayCreatePrice = $("#txtPayCreatePrice").val();
            //var txtPayCreateDate = $("#txtPayCreateDate").val();
            var txtPayCreateRemark = $("#txtPayCreateRemark").val();

            if (txtPayCreateDis == "") {
                str += "-请选择代理商；\r\n";
            }
            if (txtPayCreatePrice == "") {
                str += "-请填写入账金额；\r\n";
            }
            //if (txtPayCreateDate == "") {
            //    str += "-请填写入账时间；\r\n";
            //}
            if (txtPayCreateRemark == "") {
                str += "-请填写入备注；\r\n";
                if (txtPayCreateRemark.length > 800) {
                    str += "-备注字数不能大于800个字符；\r\n";
                }
            }
            if (str != "") {
                layerCommon.msg(str, IconOption.错误);
                return false;
            } else {
                return true;
            }

        }
        //企业钱包详细页面
        function goInfo(Id) {
            window.location.href = 'PayPreList.aspx?KeyID=' + Id;
        }
        //补录新增页面
        function goInBl(Id) {
            window.location.href = 'PayCreateAdd.aspx?UID=' + Id;
        }
        //冲正新增页面
        function goIncz(Id) {
            window.location.href = 'PayCorrectAdd.aspx?UID=' + Id;
        }

        //只能输入数字验证
        function KeyInt(val) {
            val.value = val.value.replace(/[^\d]/g, '');
        }
        //金额只能输入正数和小数
        function KeyIntPrice(val) {
            val.value = val.value.replace(/[^\d.]/g, '');
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
    <uc1:top ID="top1" runat="server" ShowID="nav-2" />

    <div class="rightinfo">

    <!--当前位置 start-->
    <div class="place">
        <ul class="placeul">
            <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
            <li><a href="../pay/PayDisList.aspx">钱包查询</a></li>
        </ul>
    </div>
    <!--当前位置 end-->
    <!--代理商搜索 Begin-->
    <input id="hid_Alert" type="hidden" />
    <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
    <!--代理商搜索 End-->
        <!--功能按钮 start-->
        <div class="tools">
            <div class="right">
                <ul class="toolbar right">
                    <li id="Search"><span>
                        <img src="../images/t04.png" alt="" /></span>搜索</li>
                    
                    <uc1:ToExcel runat="server" ID="ToExcel" contect="TbList" />
                </ul>
                <ul class="toolbar3">
                    <li>代理商名称:<input  runat="server" id="txtDisName" type="text" class="textBox"/></li>
                    
                    <li>每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                        class="textBox" style="width: 40px;" />行 </li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <!--信息列表 start-->
        <table class="tablelist" id="TbList">
            <asp:Repeater runat="server" ID="rptDis">
                <HeaderTemplate>
                    <thead>
                        <tr>
                            <th class="t6">
                                代理商名称
                            </th>
                            <th class="t1">
                                代理商区域
                            </th>
                            <th class="t1">
                                代理商等级
                            </th>
                            <th>
                                详细地址
                            </th>
                            <th class="t1">
                                联系人
                            </th>
                            <th class="t5">
                                账户余额(元)
                            </th>
                            <th  class="t2">操作</th>

                        </tr>
                    </thead>
                </HeaderTemplate>
                <ItemTemplate>
                    <tbody>
                        <tr>
                            <td>
                                <div class="tcle"><a href="PayPreList.aspx?KeyID=<%# Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) %>"  style="text-decoration: underline;">
                                    <%# Eval("DisName")%></a></div>
                            </td>
                            <td>
                              <div class="tc"> <%# Common.GetDisAreaNameById(Convert.ToInt32(Eval("AreaID")))%></div>
                            </td>
                            <td>
                              <div class="tc"> <%# Eval("DisLevel ")%></div>
                            </td>
                            <td>
                               <div class="tcle"> <%# Eval("Address")%></div>
                            </td>
                            <td>
                               <div class="tc"> <%# Eval("Principal")%></div>
                            </td>
                            <td>
                               <div class="tc"> <%# new Hi.BLL.PAY_PrePayment().sums(Convert.ToInt32(Eval("ID")), Convert.ToInt32(Eval("CompID"))).ToString("N")%></div>
                            </td>
                             <td>
                         <div class="tc">  <%--  <a href="javascript:void(0)" tip="<%# Eval("ID") %>" Pname="<%# Eval("DisName") %>" class="tablelink">补录</a> --%>
                         <% if (Common.HasRight(this.CompID, this.UserID, "1116"))
                            { %>
                            <a href="PayCreateAdd.aspx?UID=<%# Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) %>"   class="tablelinkQx" id="clikcbl"> 补录</a> &nbsp;
                            <% } %>
                             <% if (Common.HasRight(this.CompID, this.UserID, "1117"))
                            { %>
                            <a href="PayCorrectAdd.aspx?UID=<%# Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) %>"     class="tablelinkQx" id="clikccz"> 冲正</a>&nbsp;<% } %>
                            <a href="PayPreList.aspx?KeyID=<%# Common.DesEncrypt(Eval("ID").ToString(), Common.EncryptKey) %>"   class="tablelinkQx" id="A1"> 流水</a>
                      </div>
                       </td>
                        </tr>
                    </tbody>
                </ItemTemplate>
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
    <!--弹出遮罩层 Begin-->
    <div class="opacity" style="display: none;">
    </div>
    <!--弹出遮罩层 End-->
    <!--弹出录入层 Begin-->
    <div class="tip" style="display: none;">
        <div class="tiptop">
            <span>预收款补录</span><a></a></div>
        <div class="tipinfo">
            <div class="lb">
                <span>
                    <label style="color: Red; display: inline-block;">
                        *</label>&nbsp;代理商名称：</span>
                <input name="txtPayCreateDisName" disabled="disabled" id="txtPayCreateDisName" type="text"
                    runat="server" class="textBox lblCategoryName" />
                <input type="hidden" id="txtPayCreateDis" runat="server" />
            </div>
            <div class="lb">
                <span>
                    <label style="color: Red; display: inline-block;">
                        *</label>&nbsp;入账金额：</span>
                <input id="txtPayCreatePrice" onkeyup='KeyIntPrice(this)' type="text" runat="server"
                    class="downBox" />
            </div>
            <!--<div class="lb">
                    <span><label style=" color:Red; display:inline-block;">*</label>&nbsp;入账时间：</span>&nbsp;&nbsp;
                    <input name="txtArriveDate" runat="server" onclick="WdatePicker()" id="txtPayCreateDate" readonly="readonly" type="text" class="Wdate" value="" />
                </div>-->
            <div class="lb">
                <span>
                    <label style="color: Red; display: inline-block;">
                        *</label>&nbsp;入账方式：</span>&nbsp;&nbsp;补录<input type="hidden" id="txtPayCreateType"
                            value="2" runat="server" />
            </div>
            <div class="lbarea">
                <span>
                    <label style="color: Red; display: inline-block;">
                        *</label>&nbsp;备注：</span>
                <textarea id="txtPayCreateRemark" maxlength="800" rows="3" cols="30" style="border: 1px solid #A9BAC9;"
                    placeholder="订单备注不能大于800个字符" runat="server"></textarea>
                <!--<input id="txtPayCreateRemark2" type="text" runat="server" class="downBox" />-->
            </div>
            <div class="tipbtn">
                <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return formCheck()"
                    OnClick="btnAdd_Click" />&nbsp;
                <input name="" type="button" class="cancel" value="取消" />
            </div>
        </div>
    </div>
    <!--弹出录入层 End-->
    </form>
</body>
</html>

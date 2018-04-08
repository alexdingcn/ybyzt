<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayReportList.aspx.cs" Inherits="Company_Pay_PayReportList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>收款明细报表</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
        <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

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
            $("#click").click(function () {
                $(".tip").fadeIn(200);
            });
            $(".edit").click(function () {
                $(".tip2").fadeIn(200);
            });
            $(".tiptop a").click(function () {
                $(".tip").fadeOut(200);
                $("input[type='text']").val("");
            });

            $(".click2").click(function () {
                $(".tip2").fadeIn(200);
            });
            $(".tiptop a").click(function () {
                $(".tip2").fadeOut(200);
                $("input[type='text']").val("");
            });

            //            $(".sure").click(function () {
            //                $(".tip").fadeOut(100);
            //                $(".tip2").fadeOut(100);
            //            });

            $(".cancel").click(function () {
                $(".tip").fadeOut(100);
                $(".tip2").fadeOut(100);
                $("input[type='text']").val("");
            });
        });
    </script>
    <script type="text/javascript">
        //验证用
        function formCheck() {
            var Name = $.trim($(".txtUnit").val());
            var Names = $.trim($(".txtUnits").val());
            var str = "";
            if (Name == "" && Names == "") {
                str = str + "- 单位不能为空。";
            }
            if (str == "") {
                $(".tip").fadeOut(100);
                $(".tip2").fadeOut(100);
                return true;
            }
            else {
                layerCommon.msg(str, IconOption.错误);
                return false;
            }
        }
        // 每页显示数量非空验证
        function ChkPage() {
            if ($("#txtPageSize").val() == "") {
                layerCommon.msg(" 每页显示数量不能为空", IconOption.错误);
                return false;
            } else {
                $("#btnSearch").click();
            }
            return true;
        }

        //重载
        function Reset() {
            $(".txtUnitName").val("");
        }
        //只能输入数字验证
        function KeyInt(val) {
            val.value = val.value.replace(/[^\d]/g, '');
        }
    </script>
    <style type="text/css">
        .tip,.tip2{width:400px; height:350px; position:absolute;top:10%; left:30%;background:#fff;/*box-shadow:1px 8px 10px 1px #9b9b9b;border-radius:1px;behavior:url(js/pie.htc); display:none; z-index:999;*/}
        .downBox{ width:160px; height:28px; border:1px solid #eee; margin-left:5px; color:#999; padding-left:2px; }
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
                <li><a href="#">收款明细报表</a></li>
	        </ul>
        </div>
        <!--当前位置 end--> 
            <!--功能按钮 start-->
            <div class="tools">
                <div class="left">
                    <ul class="toolbar right">
                        <li><span><img src="../images/t04.png" /></span>搜索</li>
                    </ul>
                     <ul class="toolbar3">
                     <li>备注:<input name="" type="text" class="textBox"/></li>
                    </ul>
                    <ul class="toolbar3">
                        <li>付款方式:
                        <select name="" class="downBox">
                                <option value="-1">请选择付款方式</option>
                        </select>
                        </li>
                    </ul>
                    <ul class="toolbar3">
                        <li>付款方（代理商名称）:<input name="" type="text" class="textBox"/></li>
                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->

            <!--信息列表 start-->
            <table class="tablelist">
                <thead>
                    <tr>
                        <th>收款日期</th>
                        <th>付款方（代理商名称）</th>
                        <th>付款方式</th>
                        <th>金额(元)</th>
                        <th>审核状态</th>
                        <th>备注</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                         <td>2015-05-11 15:05:00</td>
                         <td>行业金融</td>
                         <td>转账汇款</td>
                         <td>10000.00</td>
                         <td>已审批</td>
                         <td></td>
                    </tr>
                    <tr class="odd">
                         <td>2015-05-11 15:05:00</td>
                         <td>行业金融</td>
                         <td>转账汇款</td>
                         <td>10000.00</td>
                         <td>已审批</td>
                         <td></td>
                    </tr>
                    <tr>
                         <td>2015-05-11 15:05:00</td>
                         <td>行业金融</td>
                         <td>转账汇款</td>
                         <td>10000.00</td>
                         <td>已审批</td>
                         <td></td>
                    </tr>
                </tbody>
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
        <!--新增 start-->
        <div class="tip" style="display: none;">
            <div style=" position:absolute;top:0; left:0; right:0; bottom:0;z-index:999; background:#fff;">
            <div class="tiptop">
                <span>录入</span><a></a></div>
            <div class="tipinfo">
                <div class="lb">
                    <span>*代理商名称：</span>
                    <select name="" class="downBox">
                                <option value="-1">请选择代理商</option>
                            </select>
                    </div>
                <div class="lb">
                    <span>*入账金额：</span><input name="txtSortIndex" type="text" class="textBox txtSortIndex"
                        runat="server" id="txtSortIndex" /></div>
                <div class="lb">
                    <span>*入账时间：</span>
                    <asp:TextBox ID="txtArriveDateRegion" Style="width: 100px;" runat="server" class="Wdate" onFocus="WdatePicker({isShowWeek:true,onpicked:function() {$dp.$('ReporDays').value=$dp.cal.getP('W','W');$dp.$('HiddenField_Days').value=$dp.cal.getP('W','W');}})"></asp:TextBox>
                    </div>
                <div class="lb">
                    <span>*入账方式：</span>&nbsp;&nbsp;手工录入</div>
                <div class="lb">
                    <span>备注：</span><input name="txtSortIndex" maxlength="200" type="text" class="textBox txtSortIndex"
                        runat="server" id="Text3" /></div>
                <div class="tipbtn">
                    <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return formCheck()"
                        OnClick="btnAdd_Click" />&nbsp;
                    <input name="" type="button" class="cancel" value="取消" />
                </div>
            </div>
            </div>
            <div id="xubox_border1" class="xubox_border" style="z-index:11; opacity: 0.3;filter: Alpha(Opacity=30) !important; border-radius:5px;top: -8px; left: -8px; right: -8px; bottom: -8px;  background-color: rgb(0, 0, 0); position:absolute; top"></div>
        </div>
        <!--新增 end-->

    </form>
</body>
</html>

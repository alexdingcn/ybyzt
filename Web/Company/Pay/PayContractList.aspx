<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayContractList.aspx.cs" Inherits="Company_Pay_PayContractList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>销售订单收款</title>
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
                str = str + "- 单位不能为空。\r\n";
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
        //删除
        function Delete(type) {
            var bol = false;
            var chklist = $(".tablelist input[type='checkbox']");
            $(chklist).each(function (index, obj) {
                if (obj.checked) {
                    bol = true;
                    return false;
                }
            })
            if (type == 1) {
                if (bol) {
                    layerCommon.confirm('确定要删除单位?', function () { $("#btnDel").click(); });
                } else {
                    layerCommon.msg(" 请勾选需要删除的单位", IconOption.错误);
                    return false;
                }
            }
        }
    </script>
    <style type="text/css">
        .tip,.tip2{width:400px; height:350px; position:absolute;top:10%; left:30%;background:#fcfdfd;box-shadow:1px 8px 10px 1px #9b9b9b;border-radius:1px;behavior:url(js/pie.htc); display:none; z-index:999;}
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
                <li><a href="../jsc.aspx">我的桌面</a></li><li>></li>
                <li><a href="PayDisList.aspx">企业钱包查询</a></li><li>></li>
                <li><a href="#">销售订单收款</a></li>
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
                        <li>代理商名称:<input name="" type="text" class="textBox" maxlength="40" /></li>
                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->

            <!--信息列表 start-->
            <table class="tablelist">
                <thead>
                    <tr>
                        <th>代理商名称</th>
                        <th>代理商代码</th>
                        <th>订单编号</th>
                        <th>订单金额(元)</th>
                        <th>订单状态</th>
                        <th width="110" style="text-align:center;">操作</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                         <td>行业金融</td>
                         <td>001</td>
                         <td>2015050600000058</td>
                         <td>10000.00</td>
                         <td>待支付</td>
                         <td width="110" align="center">
                            <a href="#" class="tablelink" id="click">收款</a>
                        </td>
                    </tr>
                    <tr class="odd">
                         <td>行业金融</td>
                         <td>001</td>
                         <td>2015050600000058</td>
                         <td>10000.00</td>
                         <td>待支付</td>
                         <td width="110" align="center">
                            <a href="#" class="tablelink" id="A1">收款</a>
                        </td>
                    </tr>
                    <tr>
                         <td>行业金融</td>
                         <td>001</td>
                         <td>2015050600000058</td>
                         <td>10000.00</td>
                         <td>待支付</td>
                         <td width="110" align="center">
                            <a href="#" class="tablelink" id="A2">收款</a>
                        </td>
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

    </form>
</body>
</html>

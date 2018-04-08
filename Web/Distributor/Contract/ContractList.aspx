<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ContractList.aspx.cs" Inherits="Company_Contract_ContractList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>合同列表</title>
    <link href="../../Company/css/style.css" rel="stylesheet" />
    <link href="../css/global.css" rel="stylesheet" />

    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/MoveLayer.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script>
        $(function () {
            $(".showDiv2 .ifrClass").css("width", "155px");
            $(".showDiv2").css("width", "150px");

            $('iframe').load(function () {
                $('iframe').contents().find('.pullDown').css("width", "150px");
            });
            $(document).on("keydown", function (e) {
                if (e.keyCode == 13) {
                    $("#btn_Search").trigger("click");
                }
            })
        })
        var div = $('<div   Msg="True"  style="z-index:10; background-color:#000; opacity:0.3; filter:alpha(opacity=50);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');

        $(document).ready(function () {

            //table 行样式
            $('.tablelist tbody tr:odd').addClass('odd');

            if ('<%=Request["nextstep"] %>' == "1") {
                Orderclass("ktxzjxs");
                document.getElementById("imgmenu").style.display = "block";
            }
            $("li#liSearch").on("click", function () {
                $("#btn_Search").trigger("click");
            })


            $("a.bulk").on("click", function () {
                $("body").append(div);
                $("div#DisImport").css("width", "500px").fadeIn(200);
            })
            $("input.cancel,.tiptop a").bind("click", function (event) {
                $(div).remove();
                $("div#DisImport").fadeOut(200);
            })
            $("div.tiptop").LockMove({ MoveWindow: "#DisImport" });
            //add by hgh 
            if ('<%=Request["nextstep"] %>' == "1") {
                Orderclass("ktxzjxs");
                document.getElementById("imgmenu").style.display = "block";
            }
        });



        function addList() {
            // $("body").append(div);
            //  $("div#DisImport").css("width", "500px").fadeIn(200);
            location.href = "../../Company/ImportDis.aspx";
        }

    </script>
    <style>
        i[error] {
            color: Red;
            font-weight: bold;
            font-family: '微软雅黑';
            font-size: 16px;
        }

        .batch i {
            font-style: normal;
        }

        .batch {
            margin: 0px auto;
            width: 1000px;
            height: 275px;
            position: absolute;
            top: 50%;
            left: 50%;
            margin: -120px 0 0 -500px;
            overflow: hidden;
        }

            .batch a {
                width: 490px;
                height: 275px;
                background: url(../../Company/images/batchImages1.jpg) no-repeat 0 0;
                position: relative;
                cursor: pointer;
                display: block;
                float: left;
            }

        .bulk i {
            position: absolute;
            top: 133px;
            color: #999;
            left: 215px;
        }

        .batch .alone {
            background: url(../../Company/images/batchImages2.jpg) no-repeat 0 0;
            float: right;
        }

        .alone i {
            position: absolute;
            top: 133px;
            color: #999;
            left: 227px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <Head:Head ID="Head2" runat="server" />
        <Left:Left ID="Left1" runat="server" ShowID="nav-1" />
        <div class="rightinfo">
            <!--当前位置 start-->
            <div class="place">
                <ul class="placeul">
                    <li><a href="/Distributor/UserIndex.aspx">我的桌面 </a></li>
                    <li>></li>
                    <li><a href="javascript:;" runat="server" id="atitle">合同列表</a></li>
                </ul>
            </div>
            <!--当前位置 end-->
            <!--功能按钮 start-->
            <div class="tools">
                <div class="right">
                    <ul class="toolbar right">
                        <li id="liSearch"><span>
                            <img src="../../Company/images/t04.png" /></span>搜索</li>
                      
                        <li class="liSenior"><span>
                        <img src="../../Company/images/t07.png" /></span>高级</li>
                    </ul>
                    <input type="button" runat="server" id="btn_Search" style="display: none;" onserverclick="btn_SearchClick" />
                    <ul class="toolbar3">
                        <li>厂商名称:<input runat="server" id="txtComName" type="text" class="textBox" />&nbsp;&nbsp;</li>
                        <li>合同编号:<input runat="server" id="txtContractNO" type="text" class="textBox" />&nbsp;&nbsp;</li>

                    </ul>
                </div>
            </div>
            <!--功能按钮 end-->
            <div class="hidden">
                <ul style="">
                    <li>每页<input name="txtPageSize" type="text" style="width: 40px;" class="textBox txtPageSize"
                        id="txtPageSize" runat="server" value="12" onkeyup="KeyInt(this);" onblur="KeyInt(this);" />
                        &nbsp;条</li>
                    <li>有效期:<input name="txtCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                        id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;-&nbsp;
                    <input name="txtEndCreateDate" runat="server" onclick="WdatePicker()" style="width: 115px;"
                        id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate" value="" />&nbsp;&nbsp;
                    </li>
                </ul>
            </div>
            <!--信息列表 start-->
            <table class="tablelist" id="TbDisList">
                <thead runat="server">
                    <tr>
                        <th class="t5">合同号
                        </th>
                        <th class="t5">厂商
                        </th>
                        <th class="t5">生效日期
                        </th>
                        <th class="t1">状态
                        </th>
                        <th class="t5">操作
                        </th>

                    </tr>
                </thead>
                <tbody runat="server">
                    <asp:Repeater ID="Rpt_Distribute" runat="server">
                        <ItemTemplate>
                            <tr style="text-align:center;height:40px;">
                                <td>
                                    <%# Eval("contractNO")%>
                                   
                                </td>
                                <td>

                                    <%# Eval("DisName")%></div>
                                </td>
                                <td>
                                    <%# Convert.ToDateTime( Eval("ForceDate")).ToString("yyyy-MM-dd")%>
                                </td>
                                <td>

                                    <%# Eval("CState").ToString() == "1" ? "启用" :"停用" %>
                                </td>
                                <td>
                                    <a style="text-decoration: underline;" href='ContractInfo.aspx?cid=<%# Eval("Id")%>'>详情</a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
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

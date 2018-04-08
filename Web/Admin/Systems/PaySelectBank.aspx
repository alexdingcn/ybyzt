<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaySelectBank.aspx.cs" Inherits="Admin_Systems_PaySelectBank" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选 择 银 行</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../Distributor/css/pay.css" rel="stylesheet" type="text/css" />
    <script src="../../js/pay.js" type="text/javascript"></script>
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        
        function CheckWindow(win)
        {
            try
            {
                var s = win.location.href;
                return true;
            }
            catch(e)
            {
                return false;
            }
        }

        //选择进货单
        function doSelectMaterials(codes,bankName)
        {
            window.parent.SelectBankReturn(codes,bankName);               
        }
        //添加按钮，监测事件
        $(document).ready(function () {
            //返回
            $("#cancel").click(function () {
                window.parent.CloseBank();
            });
            //添加
            $("#Add").on("click", function () {

                $("#btnAdd").trigger("click");
            });
            //搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });
            //重置
            $("#li_Reset").on("click", function () {

                $("#txtbankname").val("");
            });
        });

        //选择图片，单选按钮选择
        function Check(radio) {           
            $("input[name='Selectbank'][value=" + radio + "]").click();
           
         }

    </script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $("a.tooltip").ImgAmplify();
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <!--当前位置 start-->
   <%-- <div class="place">
        <span>位置：</span>
        <ul class="placeul">
            <li><a href="../index.aspx">首页</a></li>
            <li><a href="#">我要管账</a></li>
            <li><a href="#">收款帐号管理</a></li>
        </ul>
    </div>--%>
    <!--当前位置 end-->
    <!--代理商搜索 Begin-->
    <input id="hid_Alert" type="hidden" />
    <input id="hid_alerts" type="hidden" />
     <asp:Button ID="btnSearch" Text="搜索" runat="server" onclick="btnSearch_Click" style=" display:none"  />
        <asp:Button ID="btnAdd" runat="server" CssClass="fxLsBtn" OnClick="SubOk_Click" Text=" 添加 " style=" display:none" />
   
    <!--代理商搜索 End-->
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <li class="click" id="Add"><span>
                    <img src="../../Company/images/t15.png" /></span>确认</li>
                      <li id="cancel" runat="server"><span><img src="../../Company/images/tp3.png" /></span>返回</li>
            </ul>
           <div class="right">
            <ul class="toolbar right">
                        <li id="Search"><span><img src="../../Company/images/t04.png" alt="" /></span>搜索</li>
                        <li id="li_Reset"><span><img src="../../Company/images/t06.png" alt="" /></span>重置</li>
                    </ul>
                    <ul class="toolbar3">
                        <li>银行名称:
                        <input name="txtbankname"  runat="server" id="txtbankname" style="cursor:pointer;" type="text" class="textBox" value="" />
                        
                        </li>                       
                    </ul>
                </div>
        </div>
        <!--功能按钮 end-->
        <!--信息列表 start-->
         <ul class="bankCard3" >
          <asp:Repeater runat="server" ID="rptOtherBank">
                    <ItemTemplate>
                     <li>
                     	<input type="radio"  name="Selectbank" class="dx" value="<%# Eval("Id") %>"  />
                     	<span ><a href="javascript:void(0)"><img   height="30px" onclick="Check(<%# Eval("Id") %>)" src="<%# Common.GetWebConfigKey("ImgViewPath") + "/BankImg/"+Eval("BankName") %>.jpg" /><input type="hidden" class="hidBankCode" value="<%# Eval("BankCode") %>" /></a></span>
                     </li>
                    </ItemTemplate>
                </asp:Repeater>
                </ul>
               
        <!--信息列表 end-->
        <!--列表分页 start-->
        <div class="pagin">
            <span runat="server" id="ScriptManage"></span>
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
        <div class="blank10 blank20">
        </div>
    </div>
    </form>
</body>
</html>

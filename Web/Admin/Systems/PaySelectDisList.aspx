<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaySelectDisList.aspx.cs" Inherits="Admin_Systems_PaySelectDisList" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择代理商</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
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
        function doSelectMaterials(codes)
        {
           window.parent.<%=ViewState["ReturnFunc"]%>(codes);               
        }
        //添加按钮，监测事件
         $(document).ready(function () {

            //返回
            $("#cancel").click(function () {
                //history.go(-1);
                window.parent.Close();

            });
            //重置
            $("#li_Reset").on("click", function () {

                $("#txtDisID").val("");
            });
             //添加
            $("#Add").on("click", function () {
            var i=0;
           $("input:checkbox").each(function() {   
               
                    if ($(this).is(":checked"))
                     {     

                       i++;
                     }
                });
                if(i==0)
                {
                 layerCommon.msg("请选择代理商！");
                 return false;
                }
           
                $("#btnAdd").trigger("click");
              
            });

              //搜索
            $("#Search").on("click", function () {
                $("#btnSearch").trigger("click");
            });
        });


       

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
    <!--代理商搜索 Begin-->
    <input id="hid_Alert" type="hidden" />
    <asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click" Style="display: none" />
    <asp:Button ID="btnAdd" runat="server" CssClass="fxLsBtn" OnClick="SubOk_Click" Text=" 添加 "
        Style="display: none" />
    <!--代理商搜索 End-->
        <!--功能按钮 start-->
        <div class="tools">
            <ul class="toolbar left">
                <li class="click" id="Add"><span>
                    <img src="../../Company/images/t15.png" /></span>确认</li>
                <li id="cancel" runat="server"><span>
                    <img src="../../Company/images/tp3.png" /></span>返回</li>
            </ul>
            <div class="right">
                <ul class="toolbar right">
                    <li id="Search"><span>
                        <img src="../../Company/images/t04.png" alt="" /></span>搜索</li>
                    
                </ul>
                <ul class="toolbar3">
                    <li>代理商名称:
                        <input name="txtDisID" runat="server" id="txtDisID" style="cursor: pointer;" type="text"
                            class="textBox" value="" />
                        <input type="hidden" id="hidDisId" runat="server" />
                    </li>
                    <li>每页<input type="text" onkeyup='KeyInt(this)' id="txtPager" name="txtPager" runat="server"
                        class="textBox" style="width: 40px;" />行 </li>
                </ul>
            </div>
        </div>
        <!--功能按钮 end-->
        <!--信息列表 start-->
        <table class="tablelist">
            <asp:Repeater runat="server" ID="rpStockApplyList" OnItemDataBound="rpStockApplyList_ItemDataBound">
                <HeaderTemplate>
                    <thead>
                        <tr>
                            <th class="t7">
                                <input type="checkbox" id="CB_SelAll" onclick="SelectAll(this)" />
                            </th>
                            <th class="t3">
                                代理商名称
                            </th>
                            
                            <th class="t2">
                                代理商区域
                            </th>
                            <th class="t2">
                                代理商等级
                            </th>
                            <th class="t6">
                                详细地址
                            </th>
                            <th class="t5">
                                联系人
                            </th>
                            <%--  <th style="text-align:center;">操作</th>--%>
                        </tr>
                    </thead>
                    <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id='tr_<%# Eval("Id") %>'>
                        <td>
                            <div class="tc"><asp:CheckBox ID="CB_SelItem" CssClass="test"  runat="server"></asp:CheckBox>
                            <asp:HiddenField ID="Hd_Ids" runat="server" Value='<%# Eval("Id") %>' /></div>
                        </td>
                        <td>
                           <div class="tcle" ><%# Eval("DisName")%></div>
                        </td>
                        <td>
                            <div class="tc"><%# Common.GetDisAreaNameById(Convert.ToInt32(Eval("AreaID")))%></div>
                        </td>
                        <td>
                            <div class="tc"> <%# Eval("DisLevel ")%></div>
                        </td>
                        <td>
                            <div class="tcle"> <%# Eval("Address")%></div>
                        </td>
                        <td>
                           <div class="tc">  <%# Eval("Principal")%></div>
                        </td>
                        <%-- <td style="width:150px;" align="center">                          
                            <a href="javascript:void(0)"  onclick='goInfo(<%# Eval("ID") %>)' class="tablelinkQx" id="clickMx"> 查看</a>
                            <asp:LinkButton ID="btnDel" runat="server" CommandName="del" CssClass="tablelink" CommandArgument='<%# Eval("Id") %>'>删除</asp:LinkButton>
                      
                        </td>--%>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        </tbody>
        <!--信息列表 end-->
        <!--列表分页 start-->
        <div class="pagin">
            <span runat="server" id="ScriptManage"></span>
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
        <div class="blank10 blank20">
        </div>
    </div>
    </form>
</body>
</html>


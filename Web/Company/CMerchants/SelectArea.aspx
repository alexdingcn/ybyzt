<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectArea.aspx.cs" Inherits="Company_CMerchants_SelectArea" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>代理商区域</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/TreeViewOpen.js" type="text/javascript"></script>
    <script src="../js/CitysLine/JQuery-Citys-online-min.js" type="text/javascript"></script>

    <style>
        .span
        {
            margin: 0;
            padding: 0;
            display: inline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <input type="hidden" id="hidPageAction" runat="server" />
        <input type="hidden" id="hidCompID" runat="server"/>        
            <div class="rightinfo" style="margin-top: 0px; margin-left: 0px;">
                <div class="tools" style=" display:none">
                    <div class="right">
                        <ul class="toolbar right ">
                            <li onclick="return firstMvg()"><span><img src="../images/t04.png" /></span>搜索</li>
                        </ul>
                        <ul class="toolbar3">
                            <li>名称:<input name="txtName" type="text" id="txtName" runat="server" class="textBox txtName" /></li>
                        </ul>
                    </div>
                </div>
                <table class="tablelist" id="tablelist" style=" display:none">
                        <thead>
                            <tr>
                                <th class="t7">
                                    <input type="checkbox" class=""/>
                                </th>
                                <th>
                                    区域名称
                                </th>
                            </tr>
                        </thead>
                        <tbody class="tbodyFc">
                            <asp:Repeater ID="rptDisAreaList" runat="server">
                                <ItemTemplate>
                                    <tr id='<%#Eval("ID") %>' parentid='<%# Eval("ParentId")%>' bgcolor='#fcfeff' style='height: 26px;
                                        width: 100%;'>
                                        <td><div class="tc"><input type="checkbox" class="" value="<%# Eval("ID") %>"/></div></td>
                                        <td>
                                        <div class="tcle"> 
                                            <img id="Openimg" height='9' src='<%# Simage(Eval("Id")) %>' width='9' border='0' />&nbsp;<span
                                                class="span">
                                                <%# Eval("AreaName")%></span></div>
                                        </td>
                                    </tr>
                                    <%# FindChild(Eval("Id").ToString())%>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                    
                    <ul class="coreInfo">
                        <li class="lb fl">
                            <i class="name">区域名称</i>
                            <input type="hidden" id="hidProvince" runat="server"  value="选择省" />
                            <select  runat="server" id="ddlProvince" style=" width:180px;" class="box1 p-box prov" onchange="Change()" ></select>
                            <input type="hidden" id="hidCity" runat="server" value="选择市" />
                            <select runat="server" id="ddlCity" style=" width:180px;" class="box1 p-box city"  onchange="Change1()"></select>
                            <input type="hidden" id="hidArea" runat="server" value="选择区" />
                            <select runat="server" id="ddlArea" style=" width:180px;" class="box1 p-box dist"  onchange="Change2()"></select>
                        </li>
                    </ul>
                    
                    
                </div>
                <div class="div_footer" style=" padding-top: 40px;">
                    <input type="button" class="orangeBtn" id="btnAdd" value="确定"/>&nbsp;
                </div>

            <script type="text/jscript">
                $(function () {
                    $(".showDiv .ifrClass").css("width", "155px");
                    $(".showDiv").css("width", "150px");
                    $(".rightinfo").css("width", "auto");

                    $('.tablelist tbody tr:odd').addClass('odd');

                    //                 $(document).on("click", "#btnAdd", function () {
                    //                     var id = "";
                    //                     $(".tbodyFc tr").each(function (index, obj) {
                    //                         if ($(this).find("input[type='checkbox']").is(":checked")) {
                    //                             var aa = $(this).find("input[type='checkbox']").val();
                    //                             id += id === "" ? aa : "," + aa;
                    //                         }
                    //                     });
                    //                     window.parent.Fc(id);
                    //                     window.parent.layerCommon.layerClose("hid_Alert");
                    //                 });

                    $(document).on("click", "#btnAdd", function () {
                        var Province = $.trim($("#hidProvince").val());
                        var City = $.trim($("#hidCity").val());
                        var Area = $.trim($("#hidArea").val());
                        window.parent.Fcs(Province, City, Area);
                        window.parent.layerCommon.layerClose("hid_Alert");
                    });
                });

             //收缩插件
             $(document).ready(function () {
                 $("#tablelist").TreeViewOpen({ imgID: "Openimg", UpSrc: "../images/menu_plus.gif", DownSrc: "../images/menu_minus.gif" });
             })
            
         </script>
    </form>
</body>
</html>

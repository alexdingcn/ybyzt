<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectDisList3.aspx.cs" Inherits="Company_UserControl_SelectDisList3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="../../Company/css/style.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="../../js/layerCommon.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="../../js/CommonJs.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script src="../js/js.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" type="text/javascript"></script>
    <script type="text/javascript">

        function btnseve() {
            var z = 0;
            var name = "";
            var id = "";
            var chklist = $(".tablelist input[type='checkbox']");
            $(chklist).each(function (index, obj) {
                if (obj.checked) {
                    z++;
                    var str = $(this).next().val();
                    if (str != undefined) {
                        name += str.split(',')[1] + ",";
                        id += str.split(',')[0] + ",";
                    }
                }
            })
            if (z == 0) {
                layerCommon.msg(" 请选择代理商", IconOption.错误);
                return false;
            }
            if (id != "") {
                id = id.substring(0, id.length - 1);
            }
            else {
                layerCommon.msg(" 请选择代理商", IconOption.错误);
                return false;
            }
            if (name != "") {
                name = name.substring(0, name.length - 1);
            }
            $("#hiddisid").val(id);
            return true;
        }

        function btnhidparent(id) {
            window.parent.selectDis(id);
        }

        $(document).ready(function () {
            $(".showDiv3 .ifrClass,.showDiv2 .ifrClass").css("width", "175px");
            $(".showDiv3,.showDiv2").css("width", "170px");
            //            $('iframe').load(function () {
            //                $('iframe').contents().find('.pullDown').css("width", "170px");
            //            });
            $(".tablelist tbody a#A_DisSelect").on("click", function () {
                var id = $(this).attr("disid");
                var name = $(this).siblings("#Hid_DisName").val();
                window.parent.selectDis(id, name);
            });

            //            $(".tablelist tbody tr").on("click", function () {
            //                var id = $(this).find("a#A_DisSelect").attr("disid");
            //                var name = $(this).find("a#A_DisSelect").siblings("#Hid_DisName").val();
            //                window.parent.selectDis(id, name);
            //            });

            //            $("#inpqx").on("click", function () {
            //                window.parent.CloseDialogTo();
            //            })
            //            //清空
            //            $("#inpqk").on("click", function () {
            //                var id = "";
            //                var name = "";
            //                window.parent.selectDis(id, name);
            //            })
            

            

            ///重置
            $("#li_Reset").click(function () {
                //window.location.href = 'OrderCreateList.aspx';
                location = location;
            });

            $(document).ready(function () {
                $('.tablelist tbody tr:odd').addClass('odd');
                $("li#liSearch").on("click", function () {
                    $("#btn_Search").trigger("click");
                })
                
            })

        })

        
        
    </script>
    <style>
        .clears
        {
            background: url(../images/btnbg2.png) repeat-x;
            color: #000;
            font-weight: normal;
        }
        .rightinfo
        {
            margin-top: 0px;
        }
    </style>
</head>
<body style="min-width: 800px;">
    <form id="form1" runat="server">
    <div class="rightinfo" style=" margin-left:0px; width:820px;">
        <div class="tools">
            <%--<input name="" type="button" class="sure" id="inpqk" value="清空选择" />--%>
            <input id="btnInsert" type="button" class="orangeBtn" onclick="if(!btnseve()){return false;}" onserverclick="Btn_seve" runat="server" value="确定" />
            <input id="hiddisid" type="hidden" runat="server" />
            <div class="right">
                <ul class="toolbar right">
                    <li id="liSearch"><span>
                        <img src="../../Company/images/t04.png" /></span>搜索</li>
                   
                </ul>
                <input type="button" runat="server" id="btn_Search" style="display: none;" onserverclick="btn_SearchClick" />
                <ul class="toolbar3">
                    <li>代理商名称：<input runat="server" id="txtDisName" type="text" class="textBox" />&nbsp;&nbsp;</li>
                    <%--<li>代理商简称:<input runat="server" id="txtDisSname" type="text" class="textBox" />&nbsp;&nbsp;</li>--%>
                    <li>联系人：<input id="txtname" runat="server" type="text" class="textBox" />
                    </li>
                    <li>手机号：<input id="txtphone" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" runat="server" type="text" class="textBox" />
                    </li>
                </ul>
            </div>
        </div>
        <table class="tablelist">
            <thead>
                <tr>
                    <th width="25">
                        <input type="checkbox" checked="" name="checkbox" id="CB_SelAll" onclick="SelectAll(this)" />
                    </th>
                    <th>
                        代理商名称
                    </th>
                    <%--<th>
                        代理商简称
                    </th>--%>
                    <th>
                        代理商分类
                    </th>
                    <th>
                        代理商区域
                    </th>
                    <th>
                        联系人
                    </th>
                    <th>
                        联系人手机
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="Rpt_Dis" runat="server">
                    <ItemTemplate>
                        <tr style="cursor: pointer;">
                            <td>
                               <div class="tc">  <asp:CheckBox ID="CB_SelItem" runat="server"></asp:CheckBox>
                                <asp:HiddenField ID="HF_Id" runat="server" Value='<%# Eval("ID") +","+ Eval("DisName")%>' /></div>
                            </td>
                            <td>
                             <div class="tcle">      <a id="A_DisSelect" style="cursor: pointer; text-decoration: underline; display: block"
                                    disid="<%#Eval("ID")%>"></a>
                                <%# Eval("DisName")%>
                                <input type="hidden" value="<%#Eval("DisName")%>" id="Hid_DisName" /></div>
                            </td>
                            <%--<td>
                                <%# Eval("ShortName")%>
                            </td>--%>
                            <td>
                             <div class="tc">   <%# Common.GetDisTypeNameById(Eval("DisTypeID").ToString().ToInt(0))%></div>
                            </td>
                            <td>
                              <div class="tc">  <%# Common.GetDisAreaNameById(Eval("AreaID").ToString().ToInt(0))%></div>
                            </td>
                            <td>
                              <div class="tc">  <%# Eval("principal") %></div>
                            </td>
                            <td>
                               <div class="tc"> <%# Eval("phone") %></div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectFC.aspx.cs" Inherits="Company_CMerchants_SelectFC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>指定选择</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
     <script>
         $(function () {
             $(".showDiv .ifrClass").css("width", "155px");
             $(".showDiv").css("width", "150px");
             $(".rightinfo").css("width", "auto");

             $('.tablelist tbody tr:odd').addClass('odd');
             firstMvg();

             $(document).on("click", "#btnAdd", function () {
                 var id = "";
                 $(".tbodyFc tr").each(function (index, obj) {
                     if ($(this).find("input[type='checkbox']").is(":checked")) {
                         var aa = $(this).find("input[type='checkbox']").val();
                         id += id === "" ? aa : "," + aa;
                     }
                 });
                 window.parent.Fc(id);
                 window.parent.layerCommon.layerClose("hid_Alert");
             });
         });

         function firstMvg() {
             var PageAction = '<%=Request["PageAction"] + "" %>';
             var Name = $("#txtName").val();
             $.ajax({
                 type: 'post',
                 url: '../../Handler/CMHandler.ashx',
                 data: { ck: Math.random(), PageAction: PageAction, compid: '<%=this.CompID %>', name: Name },
                 dataType: 'json',
                 success: function (data) {
                     if (data.length > 0) {
                         var OutHTML = "";
                         $.each(data, function (index, item) {
                             OutHTML += '<tr>';
                             OutHTML += '<td><div class="tc"><input type="checkbox" class="" value="' + item.ID + '"/></div></td>';
                             if (PageAction === "2")
                                 OutHTML += '<td><div class="tc">' + item.AreaName + '</div></td>';
                             else
                                 OutHTML += '<td><div class="tc">' + item.DisName + '</div></td>';
                             OutHTML += '</tr>';
                         });
                         $(".tbodyFc").empty().append(OutHTML);
                     }
                 },
                 error: function (XMLHttpRequest, textStatus, errorThrown) {
                     layerCommon.msg("请求错误或超时,请重试", IconOption.错误);
                 }
             });
         }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" id="hidPageAction" runat="server" />
        <input type="hidden" id="hidCompID" runat="server"/>        
        <div class="rightinfo" style="margin-top: 0px; margin-left: 0px;">
            <div class="tools">
                <div class="right">
                    <ul class="toolbar right ">
                        <li onclick="return firstMvg()"><span><img src="../images/t04.png" /></span>搜索</li>
                    </ul>
                    <ul class="toolbar3">
                        <li>名称:<input name="txtName" type="text" id="txtName" runat="server" class="textBox txtName" /></li>
                    </ul>
                </div>
            </div>
            <table class="tablelist">
                <thead>
                    <tr>
                        <th class="t7">
                            <input type="checkbox" class=""/>
                        </th>
                        <th class="t6">
                            <%=(Request["PageAction"] + "") == "2" ? "区域" : "代理商"%>名称
                        </th>
                    </tr>
                </thead>
                 <tbody class="tbodyFc">
                    <tr>
                        <td></td>
                        <td><div class="tc"></div></td>
                    </tr>
                 </tbody>
            </table>
            <div class="div_footer" style=" padding-top: 40px;">
                <input type="button" class="orangeBtn" id="btnAdd" value="确定"/>&nbsp;
            </div>
        </div>
    </form>
</body>
</html>

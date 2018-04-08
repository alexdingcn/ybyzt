<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoleEdit.aspx.cs" Inherits="Distributor_RoleEdit" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>新增岗位</title>
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/js.js" type="text/javascript"></script>
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script> 
    <script type="text/javascript">
        $(document).ready(function () {
            $('#orderBg tr:even').addClass("bg");
            $(".btnOr").click(function () {
                if (formCheck()) {
                    $("#btnSave").trigger("click");
                }
            });

        });
        function formCheck() {
            var str = "";
            if ($.trim($("#txtRoleName").val()) == "") {
                str = "岗位名称不能为空";
            }

            if (str != "") {
                layerCommon.alert(str, IconOption.错误);
                return false;
            }
            return true;
        }
    </script>
</head>
<body class="root3">
    <form id="form1" runat="server">
    <asp:Button ID="btnSave" runat="server" Text="确定" Style="display: none;" OnClick="btnSave_Click" />
    <Head:Head ID="Head2" runat="server" />
    <div class="w1200">
        <Left:Left ID="Left1" runat="server" ShowID="nav-6" />
        <div class="rightCon">
            <div class="info">
                 <a id="navigation1" href="/Distributor/UserIndex.aspx">我的桌面</a>>
                <a id="navigation2" href="/Distributor/RoleList.aspx" class="cur">设置岗位权限</a>>
                <a  href="/Distributor/RoleList.aspx" class="cur">新增岗位</a>
                </div>
            <!--功能条件 start-->
            <div class="userFun">
                <div class="left">
                	<a href="javascript:void(0);" class="btnOr"><i class="prnIcon"></i>确定</a> 
                    <a id="areturn" href=" javascript:void(0);" onclick="javascript:history.go(-1);" class="btnBl"><i class="returnIcon"></i>返回</a>
                </div>
            </div>
            <!--功能条件 end-->
            <div class="blank10">
            </div>
            <div class="orderNr">
                <table class="orderDet" border="0" cellspacing="0" cellpadding="0" width="100%">
                    <tbody>
                        <tr>
                       <td class="head"><span><i class="required">*</i>岗位名称</span></td>
                       <td><asp:TextBox ID="txtRoleName" type="text" maxlength="50" class="xl" runat="server"></asp:TextBox>
                       <i class="grayTxt">（例如：录单岗、财务岗） </i></td>
                     </tr>
                     <tr>
                       <td class="head"><span><i class="required">*</i>是否启用</span> </td>
                       <td> &nbsp;  <input type="radio" runat="server" name="audit" value="1" checked="true"  id="rdAuditYes" />启用&nbsp;&nbsp;&nbsp;
                       <input runat="server" id="rdAuditNo" name="audit" type="radio"  value="0" />禁用</td>
                     </tr>
                     <tr>
                     <td class="head"><span>备  注</span></td >
                      <td>  <textarea  style=" height:auto; width:500px; margin:8px 6px;" rows="5" maxlength="100" class="xl" runat="server" id="txtRemark"></textarea> </td>
                     </tr>
                     <tr>
                       <td class="head"><span>排  序</span></td>
                       <td> <input runat="server"  type="text"  class="xl" id="txtSortIndex" maxlength="50" style=" width:35px; padding-right:5px; text-align:center;" /><i class="grayTxt">（列表顺序排列）</i></td>
                     </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
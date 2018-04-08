<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpService.aspx.cs" Inherits="Admin_Systems_UpPhone" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改企业手机号码</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script>

        function formCheck() {
            var str = "";
            if ($.trim($("#txtCreateDate").val()) == "") {
                str = "起始日期不能为空";
            }
            else if ($.trim($("#txtEndCreateDate").val()) == "") {
                str = "结束日期不能为空";
            }
            if (str != "") {
                errMsg("提示", str, "", "");
                return false;
            }
            return true;
        }
    </script>
        <style type="text/css">
.tablelist{border:solid 1px #cbcbcb; width:100%; clear:both;}
.tablelist th{background:#f1f1f1; height:34px; line-height:34px; border:1px solid #ddd; padding-left:10px;text-align:left; min-width:55px;}
.tablelist th:first-child{ min-width:0;}
.tablelist td{line-height:35px;border-right: solid 1px #ddd;border-bottom: solid 1px #ddd;  padding-left:10px;}
.tablelist td a{ color:#0080b8;}
.tablelink{background:#3f97c9; border:1px solid #287fb1; color:#fff;border-radius:4px; padding:1px 7px;}
.tablelink:hover,.tablelist .grayBtn:hover{ background:#ff4e02; border:1px solid #ea5211; color:#fff; border-radius:4px; padding:1px 7px;}
.tablelist tbody tr.odd{background:#f9f9f9;}
.tablelist .tablelink{ color:#fff; text-decoration:none;}
.tablelist .grayBtn{ background:#efefef; color:#666; border:1px solid #e0e0e0;}

/*.tablelist tbody tr:nth-child(odd){background:#f5f8fa;}*/
.tablelist tbody tr:hover{background:#e5ebee;}

    </style>
</head>
<body  style=" overflow:hidden;" >
    <form id="form1" runat="server">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="#">厂商管理</a><i>></i>
            <a href="UpService.aspx" runat="server" id="Atitle">修改企业服务期限</a>
    </div>
       <div class="div_content">
          <table class="tb" style="width:95%;margin:0 auto;">
                <tbody>
                    <tr>
                    <td > <span>起始日期</span></td>
                     <td>
                        <input name="txtCreateDate" runat="server" onclick="WdatePicker()" style="width: 215px;"
                                   id="txtCreateDate" readonly="readonly" type="text" class="Wdate" value="" />
                     </td>
                        <td> <span>结束日期</span></td>
                     <td>
                       <input name="txtEndCreateDate" runat="server" onclick="WdatePicker()" style="width: 215px;"
                                   id="txtEndCreateDate" readonly="readonly" type="text" class="Wdate" value="" />
                     </td>
                        <td>
                            
        <input type="button" runat="server" id="btnSubMit" class="orangeBtn"  value="确定" onclick="if(!formCheck()){return false;} " onserverclick="btnSubMit_Click" />
 
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

   

            <table class="tablelist" style="width:95%;margin:0 auto;">
            <thead>
                <tr>
                    <th>
                        厂商名称
                    </th>
                    <th>
                        服务类型
                    </th>
                    <th>
                        服务金额
                    </th>
                    <th>
                        到期时间
                    </th>
                    <th>
                        开通日期
                    </th>
                    <th>
                        处理状态
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="ServiceList" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                
                                    <%# Eval("CompName").ToString()%>
                               
                            </td>
                            <td>
                                <%# Eval("ServiceType").ToString()=="1"?"年费":"月费"%>
                            </td>
                            <td>
                                <%# Eval("Price")%>
                            </td>
                            <td>
                                <%#Eval("OutData","{0:yyyy-MM-dd}")%>
                            </td>
                            <td>
                                <%# Eval("CreateDate", "{0:yyyy-MM-dd}")%>
                            </td>
                            <td>
                                已支付
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AttrManager.aspx.cs" Inherits="SysManager_AttrManager" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>基础数据</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <link href="../css/shop.css" rel="stylesheet" type="text/css" />
        <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <style type="text/css">
         .list
        {
            border-bottom : #a6c6dd 1px solid;
            background-color : #ffffff;
            border-collapse : collapse;
            margin-bottom : 9px;
            border-top : #a6c6dd 3px solid;
			line-height:30px;
			
        }
        .list-title
        {
            background-color:#ecf4fa;
            color:#003366;
            border-top:#a6c6dd 2px solid;
        }
        .list-selected
        {
            background-color:#ffffc9;
        }
    </style>
    <script type="text/javascript">
        //用于验证
        function formCheck() {
            var attrVal = $("#txtAtVal").val();
            var sortInde = $("#txtSortIndex").val();

            var str = "";
            if (attrVal == "") {
                str = str + "- 属性值不能为空。\r\n";
            }
//            if (sortInde == "") {
//                str = str + "- 排序不能为空。\r\n";
//            }

            if (str == "") {
                return true;
            } else {
                layerCommon.msg(str, IconOption.错误);
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <div class="main">
        <div class="right_top">
            <div class="fx_jh">
                <div class="title">
                    <strong>位置：基础数据</strong>
                    <div class="xzjh">
                    </div>
                </div>
            </div>
        </div>
        
    </div>
    <div class="blank10"></div>
    <div class="content jhdAdd">
        <table style="border-collapse: collapse; width:80%; height:auto; border:1px solid #a6c6dd;"  cellspacing="0" cellpadding="0"
                          align="center" border="0">
            <tr style=" height:40px;" class="list-title">
                <td align="center">
                    <b>属性名</b>
                </td>
                <td align="center">
                    <b>属性值</b></br>
                    <asp:Label ID="lblReMark" runat="server"></asp:Label>
                </td>
            </tr>
            <tr bgcolor="#fdfdfd" valign="top" style=" height:450px;">
                <td valign="top" align="center" width="25%">
                    <div style="overflow: auto; height:450px; text-align:center;border-right:1px solid #a6c6dd;">
                        <asp:DataGrid ID="DgAttrName" runat="server" CssClass="list" CellPadding="2" CellSpacing="2" BorderStyle="None"
                            BorderWidth="0" GridLines="Horizontal" AutoGenerateColumns="false" PageSize="15" 
                            Width="100%" AllowPaging="False" OnSelectedIndexChanged="DgAttrName_SelectedIndexChanged">
                            <SelectedItemStyle CssClass="list-selected" />
                            <Columns>
                                <asp:BoundColumn Visible="False" DataField="Id"></asp:BoundColumn>                                            
                                <asp:BoundColumn Visible="False" DataField="AtType"></asp:BoundColumn>
                                <asp:BoundColumn Visible="False" DataField="AtName"></asp:BoundColumn>
                                <asp:ButtonColumn DataTextField="AtName" HeaderText="" CommandName="Select" DataTextFormatString="{0}">
                                    <HeaderStyle Wrap="False" Width="40px"></HeaderStyle>
						            <ItemStyle Wrap="False"></ItemStyle>
                                </asp:ButtonColumn>
                            </Columns>                                       
                        </asp:DataGrid>                                    
                    </div>
                </td>
                <td valign="top" align="left" bgcolor="#fdfdfd">
                    <br>
                    <table cellpadding="0" cellspacing="0" height="100%" width="100%">
                        <tr>
                            <td>
                                <table height="20" cellspacing="0" cellpadding="5" width="95%" border="0">
                                    <tr align="center">
                                        <td>
                                            <table id="table" cellpadding="0" cellspacing="0" align="center" border="0"> 
                                               <tr>
                                                    <td align="center"><span id="val" runat="server">属性值 <font color="red">*</font></span></td>
                                                    <td align="center"><span id="Sorh" runat="server">排序</span></td>
                                                    <td></td>
                                               </tr>
                                               <tr>
                                                    <td align="center">
                                                         <asp:TextBox ID="txtAtVal" runat="server" class="textL" Visible="false" MaxLength="50" Width="150px"></asp:TextBox>
                                                    </td>
                                                    <td align="center">
                                                        &nbsp;&nbsp;
                                                        <asp:TextBox ID="txtSortIndex" runat="server" class="textL" Visible="false" MaxLength="50" Width="150px"></asp:TextBox>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblMsg" runat="server" Text="请选择左边的字典名"></asp:Label>&nbsp;
                                                        &nbsp;&nbsp;<asp:ImageButton ID="btnAddNew" runat="server" ImageUrl="../../images/Add.gif" BorderWidth="0"
                                                            BorderStyle="None" AlternateText="新增" Visible="False" OnClick="btnAddNew_Click" OnClientClick="return formCheck()"></asp:ImageButton>
                                                        &nbsp;&nbsp;<asp:ImageButton ID="btnSave" runat="server" ImageUrl="../../images/ToolsItemSave.gif"
                                                            BorderWidth="0" BorderStyle="None" AlternateText="保存" Visible="False" OnClick="btnSave_Click" OnClientClick="return formCheck()"></asp:ImageButton>
                                                        &nbsp;&nbsp;<asp:ImageButton ID="btnUp" runat="server" ImageUrl="../../images/Arrow_up.gif"
                                                            BorderWidth="0" BorderStyle="None" AlternateText="升序" Visible="False" OnClick="btnUp_Click"></asp:ImageButton>
                                                        &nbsp;&nbsp;<asp:ImageButton ID="btnDown" runat="server" ImageUrl="../../images/Arrow_down.gif"
                                                            BorderWidth="0" BorderStyle="None" AlternateText="降序" Visible="False" OnClick="btnDown_Click"></asp:ImageButton>
                                                        &nbsp;&nbsp;<asp:ImageButton ID="btnDelete" runat="server" ImageUrl="../../images/Del.gif"
                                                            BorderWidth="0" BorderStyle="None" AlternateText="删除" Visible="False" OnClientClick="return confirm('确实要删除吗?');" OnClick="btnDelete_Click">
                                                        </asp:ImageButton>
                                                        <asp:Label ID="lblAttrValMsg" runat="server" Text="" ForeColor="red"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style=" height:10px;">
                            <td></td>
                        </tr>
                        <tr height="100%" align="center">
                            <td>
                                <div style="height: 100%">
                                    <asp:DataGrid ID="dgAttrVal" runat="server" CssClass="list" CellPadding="0" GridLines="Horizontal"
                                        AllowSorting="True" AutoGenerateColumns="False" PageSize="15" Width="100%" OnItemCommand="dgAttrVal_ItemCommand" >
                                        <HeaderStyle CssClass="list-title"></HeaderStyle>
                                        <FooterStyle CssClass="list-title"></FooterStyle>
                                        <SelectedItemStyle CssClass="list-selected"></SelectedItemStyle>
                                        <Columns>
                                            <asp:BoundColumn Visible="False" DataField="Id"></asp:BoundColumn>
                                            <asp:ButtonColumn DataTextField="AtVal" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="属性值" CommandName="Modify">
                                            </asp:ButtonColumn>
                                            <asp:ButtonColumn DataTextField="SortIndex" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="排序" CommandName="Modify">
                                            </asp:ButtonColumn>                                     
                                        </Columns>      
                                    </asp:DataGrid>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

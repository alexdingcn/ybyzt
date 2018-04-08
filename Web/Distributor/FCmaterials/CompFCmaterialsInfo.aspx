<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompFCmaterialsInfo.aspx.cs" Inherits="Company_SysManager_DisEdit" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<%@ Register TagName="Footer" TagPrefix="Footer" Src="~/Distributor/DealerFooter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>厂商首营资料详情</title>

    <link href="../../Company/css/global-2.0.css" rel="stylesheet" type="text/css" />
        <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />
     <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript"> 
      
        $(document).ready(function () {
            //table 行样式
            $('.PublicList tbody tr:odd').addClass('odd');
        });

        //商品附件查看
        function OutFile(fileName) {
            $("#FileName").val(fileName);
            $("#FileOut").trigger("click");
        };

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <Head:Head ID="Head2" runat="server" />
     <Left:Left ID="Left1" runat="server" ShowID="nav-7" />
    <input type="hidden"  id="hid_Alert"/> 
    <input id="hid_Alerts" type="hidden" />
<div class="rightinfo">
<!--当前位置 start-->
<div class="info">
    <a href="/Distributor/UserIndex.aspx">我的桌面 </a>>
    <a href="#" runat="server" id="atitle">厂商首营资料详情</a>
</div>
<!--当前位置 end-->

<ul class="coreInfo">
    	<li class="lb fl"><i class="name"><i class="red"></i>厂商编码</i>
     <input type="text" class="box1" placeholder="" runat="server" maxlength="50" id="txtComCode" disabled="disabled" /></li>

    	<li class="lb fl"><i class="name"><i class="red"></i>厂商资料</i>
     <input type="text" class="box1" placeholder="" runat="server" maxlength="50" id="txtComName" disabled="disabled" /></li>
	
</ul>

    <div class="blank20"></div>



    <div class="c-n-title">基本资料</div>
<ul class="coreInfo">
	
     <li class="lb clear" runat="server" id="li1">
    	<i class="name fl" style="width:150px;"><i class="red">*</i>营业执照</i>
    	 <div class="con upload">
         <div style="float:left">
            </div>
         <div style="float:left">
          <div id="UpFileText" runat="server" style="margin-left:10px;width:240px;">
          </div>
         </div>     
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate"   runat="server"  style="width: 115px;"
                    id="validDate" readonly="readonly" type="text" class="Wdate" value="" /></i>
       </div>
              <asp:Panel runat="server" ID="DFile" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName" type="hidden"  />
     </li>


     <li class="lb clear" runat="server" id="li2">
    	<i class="name fl" style="width:150px;"><i class="red">*</i>生产许可证</i>
    	 <div class="con upload">
         <div style="float:left">
            </div>
         <div style="float:left">
          <div id="UpFileText2" runat="server" style="margin-left:10px;width:240px;">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate2" runat="server"  style="width: 115px;"
                    id="validDate2" readonly="readonly"  disabled="disabled" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile2" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName2" type="hidden" />
     </li>
     <li class="lb clear" runat="server" id="li3">
    	<i class="name fl" style="width:150px;">税务登记证</i>
    	 <div class="con upload">
         <div style="float:left">
             </div>
         <div style="float:left">
          <div id="UpFileText3" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate3" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate3" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile3" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName3" type="hidden" />
     </li>


     <li class="lb clear" runat="server" id="li4">
    	<i class="name fl" style="width:150px;">开户银行许可证</i>
    	 <div class="con upload">
         <div style="float:left">
             </div>
         <div style="float:left">
          <div id="UpFileText4" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate4" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate4" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile4" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName4" type="hidden" />
     </li>

     <li class="lb clear" runat="server" id="li5">
    	<i class="name fl" style="width:150px;">质量管理体系调查表</i>
    	 <div class="con upload">
         <div style="float:left">
             </div>
         <div style="float:left">
          <div id="UpFileText5" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate5" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate5" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile5" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName5" type="hidden" />
     </li>

     <li class="lb clear" runat="server" id="li6">
    	<i class="name fl" style="width:150px;">GSP/GMP证书</i>
    	 <div class="con upload">
         <div style="float:left">
             </div>
         <div style="float:left">
          <div id="UpFileText6" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate6" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate6" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile6" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName6" type="hidden" />
     </li>

     <li class="lb clear" runat="server" id="li7">
    	<i class="name fl" style="width:150px;">开票信息</i>
    	 <div class="con upload">
         <div style="float:left">
             </div>
         <div style="float:left">
          <div id="UpFileText7" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate7" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate7" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile7" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName7" type="hidden" />
     </li>


     <li class="lb clear" runat="server" id="li8">
    	<i class="name fl" style="width:150px;">企业年报</i>
    	 <div class="con upload">
         <div style="float:left">
             </div>
         <div style="float:left">
          <div id="UpFileText8" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate8" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate8" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile8" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName8" type="hidden" />
     </li>

     <li class="lb clear" runat="server" id="li9">
    	<i class="name fl" style="width:150px;">银行收付款帐号资料</i>
    	 <div class="con upload">
         <div style="float:left">
             </div>
         <div style="float:left">
          <div id="UpFileText9" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate9" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate9" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile9" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName9" type="hidden" />
     </li>

     <li class="lb clear" runat="server" id="li10">
    	<i class="name fl" style="width:150px;">购销合同</i>
    	 <div class="con upload">
         <div style="float:left">
             </div>
         <div style="float:left">
          <div id="UpFileText10" style="margin-left:10px;width:240px;" runat="server">
          </div>
         </div> 
             <i class="gclor9" style="margin-left:30px;">有效期<input name="validDate10" runat="server" onclick="WdatePicker()"  style="width: 115px;"
                    id="validDate10" readonly="readonly" type="text" class="Wdate" value="" /></i>
             
       </div>
              <asp:Panel runat="server" ID="DFile10" Style="margin: 5px 5px;">
              </asp:Panel>
            <input runat="server" id="HidFfileName10" type="hidden" />
     </li>

</ul>
         


<div class="c-n-title">开票信息</div>
<ul class="coreInfo">
	<li class="lb fl"><i class="name">发票抬头</i>
     <input type="text" class="box1" placeholder="" runat="server" maxlength="50"  disabled="disabled" id="txtRise"/></li>

    	<li class="lb fl"><i class="name">发票内容</i>
     <input type="text" class="box1" placeholder="" runat="server" maxlength="50"  disabled="disabled" id="txtContent"/></li>
    	<li class="lb fl"><i class="name"><i class="red"></i>开户银行</i>
     
             <select id="txtOBank" runat="server" disabled="disabled"  onchange="Otypechange(this.options[this.selectedIndex].value)"
                               class="box1" style="width:382px;background-color:#e5e0e0;">
                                <option value=""></option>
                                <option value="1405">广州农村商业银行</option>
                                <option value="102">中国工商银行</option>
                                <option value="103">中国农业银行</option>
                                <option value="104">中国银行</option>
                                <option value="105">中国建设银行</option>
                                <option value="301">交通银行</option>
                                <option value="100">邮储银行</option>
                                <option value="303">光大银行</option>
                                <option value="305">民生银行</option>
                                <option value="306">广发银行</option>
                                <option value="302">中信银行</option>
                                <option value="310">浦发银行</option>
                                <option value="309">兴业银行</option>
                                <option value="401">上海银行</option>
                                <option value="403">北京银行</option>
                                <option value="307">平安银行</option>
                                <option value="308">招商银行</option>
                                <option value="20000" style="color: Blue;">更多银行</option>
                            </select>



    	</li>

    
    	<li class="lb fl"><i class="name"><i class="red"></i>开户账户</i>
     <input type="text" class="box1" placeholder="" runat="server" maxlength="50" id="txtOAccount" disabled="disabled" /></li>

    	<li class="lb fl"><i class="name"><i class="red"></i>纳税人登记号</i>
     <input type="text" class="box1" placeholder="" runat="server" maxlength="50" id="txtTRNumber" disabled="disabled" /></li>
	
</ul>

    <div class="blank20"></div>

    <div class="c-n-title">产品注册证</div>

        <div class="blank20"></div>

        <div class="orderNr">
                <!--信息列表 start-->
                <asp:Repeater runat="server" ID="rptGoods" >
                    <HeaderTemplate>
                        <table class="PublicList list">
                            <thead>
                                <tr>
                                    <th>
                                        商品名称
                                    </th>
                                      <th>
                                        商品注册证查看
                                    </th>
                                    <th style="text-align: center; width: 110px;">
                                    有效期
                                </th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr id='tr_<%# Eval("ID") %>'>
                            <td>
                                <%# Eval("goodsName")%>
                            </td>
                            <td>
                                <a href="javascript:;" onclick="OutFile('<%# Eval("registeredCertificate")%>')">
                                     <%# getFileName(Eval("registeredCertificate").ToString())%>
                                </a>
                            </td>
                            <td style="width: 110px" >
                            <%# Convert.ToDateTime(Eval("validity"))==DateTime.MinValue?"":Eval("validity","{0:yyyy-MM-dd}") %>
                        </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody> </table>
                    </FooterTemplate>
                </asp:Repeater>
                <!--信息列表 end-->
            </div>


</div>

         <div class="blank20"></div>

        <%--<asp:LinkButton ID="FileOut" runat="server" OnClick="FileOut_Click">
        </asp:LinkButton>--%>

        <asp:Button ID="FileOut" Text="" runat="server" OnClick="FileOut_Click" Style="display: none" />
        <input id="FileName" type="hidden" name="FileName" runat="server" />
       
    </form>
</body>
</html>

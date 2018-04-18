

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CompInfo.aspx.cs" EnableEventValidation="false" Inherits="Admin_Systems_CompInfo" %>
<%@ Register TagPrefix="uc1" TagName="IndusTry" Src="~/Admin/UserControl/Tree_Industry.ascx" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Admin/UserControl/Org.ascx" TagPrefix="uc1" TagName="Org" %>
<%@ Register src="../../Admin/UserControl/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register src="../../Admin/UserControl/Left.ascx" tagname="Left" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>厂商信息</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/layer.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/jquery.idTabs.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/layer.js" type="text/javascript"></script>
    
    
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script type="text/javascript">
        
        $(document).ready(function () {
             //日志
            $("#Log").on("click", function () {
                var height = document.body.clientHeight; //计算高度
                var layerOffsetY = (height - 450) / 2; //计算宽度
                var index = layerCommon.openWindow('收款帐号维护日志', '../../BusinessLog.aspx?LogClass=paymentbank&ApplicationId=0&CompId=' + <%=KeyID%>, '750px', '450px'); //记录弹出对象
                $("#hid_Alert").val(index);//记录弹出对象
            });


            var div = $('<div   Msg="True"  style="z-index:10; background-color:#000; opacity:0.5; filter:alpha(opacity=50);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');

            $(".itab #yeqian").idTabs();
            $('.tablelist tbody tr:odd').addClass('odd');
            //            $("li#libtnAudit").on("click", function () {
            //                confirm("确认审核？", function () { $("#btnAudit").trigger("click") }, "提示");
            //            })

            $("li#libtnUserAdd").on("click", function () {
                $("body").append(div);
                $(".AddWindow").css("height", "340px").fadeIn(200);
            })
            $("li#liOrgBind").on("click", function () {
                $("body").append(div);
                $(".AddWindow2").css("height", "180px").fadeIn(200);
            })

            $("li#libtnDel").on("click", function () {
                confirm("确认删除？", function () { $("#btnDel").trigger("click"); }, "提示");
            })
            $("li#lizxiu").on("click", function () {
                window.location.href = 'CompFixEdit.aspx?KeyID=<%=KeyID %>';

            })
            $("li#lidp").on("click", function () {
                $(".HrefShop")[0].click();
            })
            $("li#libtnUse").on("click", function () {
                confirm("确认启用？", function () { $("#btnUse").trigger("click"); }, "提示");
            })
            $("li#libtnNUse").on("click", function () {
                confirm("确认禁用？", function () { $("#btnNUse").trigger("click"); }, "提示");
            })

            $("li#lizx").on("click", function () {
                confirm("确认装修？", function () { $("#btnzx").trigger("click"); }, "提示");
            })

            $(".tablelist tbody tr td a.DisShow").on("click", function () {
                var height = document.documentElement.clientHeight;
                var layerOffsetY = (height - 500) / 2; //计算宽度
                showDialog('代理商查看', '<%=ResolveUrl("DisInfo.aspx")%>?KeyID=' + this.id + '&type=3', '950px', '500px', layerOffsetY);
            })

            $("li#libtnAudit").on("click", function () {
                location.href = 'CompEdit.aspx?KeyID=<%=KeyID %>&type=<%=Request["type"] %>';
            })
            //编辑 libtnEdit
            $("li#libtnEdit").on("click", function () {
                location.href = 'CompEdit.aspx?S=1&KeyID=<%=KeyID %>&type=<%=Request["type"] %>';
            })

            //修改手机号码 libtnEdit
            $("li#liUpPhone").on("click", function () {
                var height = document.documentElement.clientHeight;
                var layerOffsetY = (height - 500) / 2; //计算宽度
                showDialog('修改企业手机号码', 'UpPhone.aspx?KeyID=<%=KeyID %>', '450px', '300px', layerOffsetY);
            });
            //修改企业服务期限 liUpCompService
            $("li#liUpCompService").on("click", function () {
                var height = document.documentElement.clientHeight;
                var layerOffsetY = (height - 500) / 2; //计算宽度
                showDialog('修改企业服务期限', 'UpService.aspx?KeyID=<%=KeyID %>', '1000px', '400px', layerOffsetY);
            });
            //收款帐号管理
            $("li#li1").on("click", function () {
                window.location.href = 'PayAccountList.aspx?KeyID=<%=KeyID %>&type=<%=Request["type"] %>';
            });
            // 二维码
            $("li#liQrcode").on("click", function () {
                var str = generateQrcode(<%=KeyID %>);
                if (str != "") {
                    errMsg("提示", str, "", "");
                    return false;
                }
            });
            
            //支付设置
            $("li#payset").on("click", function () {
                window.location.href = 'PayMentSettings.aspx?KeyID=<%=KeyID %>';
            })

            //微信收款帐号设置
            $("li#LiWx").on("click", function () {
                window.location.href = 'PayWxSettings.aspx?KeyID=<%=KeyID %>';
            })
            //支付宝收款帐号设置
            $("li#Lialipay").on("click", function () {
                window.location.href = 'PayAliSettings.aspx?KeyID=<%=KeyID %>';
            })

            //关闭
            $("#lblbtnback").click(function () {
                var go = '<%=Request["go"] %>';
                if (go == "1") {
                    location.href = 'CompList.aspx';
                } else
                    history.go(-1);
            });

            $("#CompName").on("click", function () {
                var url = '/<%= KeyID.ToString() %>.html';
                var tempwindow = window.open('', "pay");
                tempwindow.location = url;
            });

            $(".AddWindow .tiptop a,.AddWindow .tipbtn .cancel").on("click", function () {
                $(div).remove();
                $(".AddWindow").fadeOut(100);
            })
            $(".AddWindow2 .tiptop a,.AddWindow2 .tipbtn .cancel").on("click", function () {
                $(div).remove();
                $(".AddWindow2").fadeOut(100);
            })

        })

        function formCheck() {
            var str = "";
            if ($.trim($(".txtUserName").val()) == "") {
                str = "登录帐号不能为空。";
            }
            else if ($.trim($(".txtUserPwd").val()).length < 6) {
                str = "登录密码不能少于6位。";
            }
            else if ($.trim($(".txtUserPwds").val()) != $.trim($(".txtUserPwd").val())) {
                str = "确认密码不一致。";
            }
            //else if ($("#ddlRoleId option").length == 0) {
            //str = "该企业未维护岗位";
            //}
            else if ($.trim($(".txtUserTrueName").val()) == "") {
                str = "姓名不能为空。";
            }
            else if (!IsMobile($.trim($(".txtUserPhone").val()))) {
                str = "手机号码不正确。";
            }
            if (str == "") {
                str = ExisPhone($(".txtUserPhone").val());
            }

            if (str != "") {
                errMsg("提示", str, "", "");
                return false;
            }
            return true;
        }

        function formCheck2() {
            var str = "";
            if ($("select#Org").val() == "-1" || $("select#Org option").length == 0) 
            {
                str = "请选择机构";
            }
            else if ($("#salemanid").val() == "" || $("#salemanid").val() == "-1") 
            {
                str = "请选择业务员";
            }
            if (str != "") {
                errMsg("提示", str, "", "");
                return false;
            }
            return true;
        }

        function generateQrcode(keyId) {
            var str = "";
            $.ajax({
                type: "post",
                data: { Action: "GetQrcode", KeyID: keyId },
                dataType: 'json',
                async: false,
                timeout: 4000,
                success: function (data) {
                    if (data.result) {
                        str = "生成二维码成功！";
                    }
                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                    str = "生成二维码错误";
                }
            })
            return str;
        }

        function ExisPhone(name) {
            var str = "";
            $.ajax({
                type: "post",
                data: { Action: "GetPhone", value: name },
                dataType: 'json',
                async: false,
                timeout: 4000,
                success: function (data) {
                    if (data.result) {
                        str = "该手机已被注册请重新填写！";
                    }
                }, error: function (XMLHttpRequest, textStatus, errorThrown) {
                    str = "手机号码效验异常";
                }
            })
            return str;
        }
    
    </script>
    <style>
    select {
padding: 5px 0px\9;
height: 30px;
margin-left: 2px;
width: 150px;
border: 1px solid #D5D5D5;
font-family: '微软雅黑';
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <a href="ShopManager.aspx?KeyID=<%=KeyID %>" class="HrefShop" target="_blank" style=" display:none"></a>
     <uc1:Org runat="server" ID="HidOrgJs" />
       <uc1:Header ID="Header1" runat="server" />
    <uc2:Left ID="Left1" runat="server" />
    <div class="m-con">
    <div class="m-place">
	        <i>位置：</i>
            <a href="../index.aspx" target="_top">我的桌面</a><i>></i>
            <a href="CompList.aspx" runat="server" id="Btitle">厂商管理</a><i>></i>
            <a href="#" runat="server" id="Atitle">厂商查看</a>
        </div>
        <%#Eval("IsEnabled")%>
                <div class="tools">
                    <ul class="toolbar left">
                         <li id="lizx" runat="server"><span><img src="../../Company/images/t17.png" runat="server" id="img4" /></span>确认装修</li>
                        <li id="lizxiu" runat="server"><span><img src="../../Company/images/t17.png" runat="server" id="imgs" /></span>代为装修</li>
                        <li id="lidp" runat="server"><span><img src="../../Company/images/t17.png" runat="server" id="img2" /></span>店铺装修</li>
                        <li id="libtnAudit" runat="server" ><span><img src="../../Company/images/t15.png" /></span>审核</li>
                        <li id="libtnEdit" runat="server"><span><img src="../../Company/images/t02.png" /></span>编辑</li>
                        <li id="libtnDel" runat="server"><span><img src="../../Company/images/t03.png" /></span>删除</li>
                        <li id="libtnUse" runat="server"><span><img src="../../Company/images/nlock.png" /></span>启用</li>
                        <li id="libtnNUse" runat="server"><span><img src="../../Company/images/lock.png" /></span>禁用</li>
                        <li id="li1" runat="server"><span><img src="../../Company/images/t17.png" /></span>收款帐号管理</li>
                        <li id="liOrgBind" runat="server"><span><img src="../../Company/images/t17.png" /></span>业务员绑定</li>
                        <li id="liUpPhone" runat="server"><span><img src="../../Company/images/t02.png" /></span>修改企业手机号</li>
                        <li id="liUpCompService" runat="server"><span><img src="../../Company/images/t02.png" /></span>修改企业服务期限</li>
                        <li id="liQrcode"><span><img src="../../Company/images/leftico11.png" /></span>生成二维码</li>
                      <%--  <li id="payset" runat="server"><span><img src="../../Company/images/t17.png" runat="server" id="img1s" /></span>支付手续费设置</li>
                        <li id="LiWx" runat="server"><span><img src="../../Company/images/t17.png" runat="server" id="img1" /></span>微信收款设置</li>
                        <li id="Lialipay" runat="server"><span><img src="../../Company/images/t17.png" runat="server" id="img3" /></span>支付宝收款设置</li> --%>
                        <li id="Log" runat="server"><span><img src="../../Company/images/t14.png" runat="server"/></span>收款账户日志</li>
                        <li id="lblbtnback" runat="server"><span><img src="../../Company/images/tp3.png" /></span>返回</li>                  
                    </ul>
                      <input type="button" runat="server" id="btnDel" onserverclick="btn_Del" style="display:none;"  />
                      <%--<input type="button" runat="server" id="btnAudit" onserverclick="btn_Audit" style="display:none;"/>--%>
                      <input type="button" runat="server" id="btnUse" onserverclick="btn_Use" style="display:none;"  />
                      <input type="button" runat="server" id="btnNUse" onserverclick="btn_NUse" style="display:none;"  />
                     <input type="button" runat="server" id="btnzx" onserverclick="btn_zx" style="display:none;"  />
                 </div>

             <div class="div_content">
                 <%--<%# Eval("IsEnabled").ToString() == "1" ? "是" : "(style='color:red')"%> --%>
                     
                     <table  id="tab1" class="tb">
                       <tbody>
                          <tr>
                          <td style=" width:15%;"><span>厂商名称</span> </td>
                          <td style=" width:25%;"> 
                            <label runat="server" id="lblCompName"></label> 
                            <a id="CompName" style="cursor:pointer; text-decoration: underline;color: #0080b8;" >企业预览</a> 
                         </td>
                          <td style=" width:15%;"><span>行业类别</span> </td>
                          <td style=" width:25%;"><uc1:IndusTry runat="server"  UType="label" Id="lblIndusName" />  </td>
                         </tr>
                      
                         <tr>
                         <td><span>企业简称</span> </td>
                          <td  colspan="3"> <label runat="server" id="lblShotName"></label>  </td>
                         </tr>

                          <tr>
                          <td ><span>企业编号</span> </td>
                          <td > <label runat="server" id="lblCompCode"></label>  </td>
                           <td ><span>法人</span> </td>
                          <td ><label runat="server" id="lblLegal"></label> </td>
                         </tr>

                          <tr runat="server" id="TRORG">
                          <td ><span>机构</span> </td>
                          <td > <label runat="server" id="lblOrg"></label>  </td>
                          <td ><span>业务员</span> </td>
                          <td ><label runat="server" id="lblSaleMan"></label> </td>
                         </tr>

                         <tr>
                          <td ><span>法人身份证</span> </td>
                          <td > <label runat="server" id="lblIdentitys"></label>  </td>
                          <td ><span>法人手机</span> </td>
                          <td ><label runat="server" id="lblLegalTel"></label> </td>
                         </tr>

                         <tr>
                          <td ><span>营业执照号码</span> </td>
                          <td ><label runat="server" id="lblLicence"></label> </td>
                          <td><span>组织机构代码证号码</span> </td>
                          <td > <label runat="server" id="lblOrCode"></label>   </td>                          
                         </tr>

                         <tr>
                          <td><span>税务登记证号码</span> </td>
                          <td > <label runat="server" id="lblAccount"></label>   </td>
                            <td><span>联系人</span> </td>
                          <td><label runat="server" id="lblPrincipal"></label> </td>
                         </tr>

                         <tr>
                          <td ><span>固定电话</span> </td>
                          <td > <label runat="server" id="lblTel"></label>  </td>
                          <td ><span>联系人手机</span> </td>
                          <td > <label runat="server" id="lblPhone"></label>  </td>
                         </tr>

                         <tr>
                          <td><span>传真</span> </td>
                          <td><label runat="server" id="lblFax"></label> </td>  
                           <td ><span>邮箱</span> </td>
                          <td ><label runat="server" id="lblZip"></label> </td>                  
                         </tr>
                          <tr>
                          <td><span>是否启用</span> </td>
                          <td> <label runat="server" id="lblIsEbled"  ></label>   </td>
                          <td><span>是否已审核</span> </td>
                          <td> <label runat="server" id="lblAudit" style="width:100%"></label> 
                          </td>
                         </tr>
                       

                          <tr>
                          <td ><span>首页显示</span> </td>
                          <td ><label runat="server" id="lblIsFirst"></label> </td>
                          <td ><span>是否允许代理商加盟</span> </td>
                          <td > <label runat="server" id="lblIsHot"></label>  </td>
                         </tr>
                        
                         <tr>
                          <td><span>企业来源</span> </td>
                          <td colspan="3" id="Erptype" runat="server"> <label runat="server" id="lblErptype"></label>   </td>
                          <td id="Sort" runat="server" visible="false"><span>首页显示排序</span> </td>
                          <td id="tSort" runat="server" visible="false">
                          <input runat="server" style="width:60px;" type="text" maxlength="50" class="textBox" id="txtSort" />&nbsp;&nbsp;
                          <asp:ImageButton ID="btnSave" runat="server" ImageUrl="../../images/ToolsItemSave.gif"
                          BorderWidth="0" BorderStyle="None" AlternateText="保存" OnClick="btnSaveSort_click"></asp:ImageButton><i class="grayTxt">（默认为999，升序排列）</i></td>
                         </tr>
                           <tr>
                          <td ><span>服务起始日期</span> </td>
                          <td ><label runat="server" id="EnabledStartDate"></label> </td>
                          <td ><span>服务结束日期</span> </td>
                          <td > <label runat="server" id="EnabledEndDate"></label>  </td>
                         </tr>
                         <tr>
                       <td><span>下载查看附件</span> </td>
                       <td colspan="3">
                       <asp:Panel runat="server" id="DFile" style=" margin-left:5px;"></asp:Panel>
                       </td>
                       </tr>

                       <tr>
                           <td><span>主要经营范围</span> </td>
                          <td colspan="3"> <label runat="server" id="lblInfo"></label>  </td>
                       </tr>
                                   <tr>
                          <td style=" width:15%;"><span>注册资金(万)</span> </td>
                          <td style=" width:25%;"> 
                            <label runat="server" id="lblCapital"></label>
                         </td>
                          <td style=" width:15%;"><span>企业机构类型</span> </td>
                          <td style=" width:25%;"> <label runat="server" id="lblCompType"></label>  </td>
                         </tr>
                       <tr>
                       <td><span>所属区域</span> </td>
                       <td colspan="3"> <label runat="server" id="lblCompAddr"></label>   </td>
                       </tr>
                          <tr>
                       <td><span>详细地址</span> </td>
                       <td colspan="3"> <label runat="server" id="lblAddress"></label>   </td>
                       </tr>


                       <tr>
                     <td ><span>备注</span></td >
                      <td colspan="3"> <label runat="server" id="lblRemark"></label>   </td>
                     </tr>

                       </tbody>
                      </table>

                                         <div id="tab4" runat="server" class="itab" style=" margin:15px 0px;">
  	                <ul id="yeqian" runat="server"> 
                    <li><a href="#tab3" class="selected">企业用户</a></li> 
                    <li><a href="#tab2">代理商</a></li> 
  	                </ul>
                    </div> 


                    <div id="tab2" runat="server" style=" width:auto; display:block;">
                       <!--信息列表 start-->
                     <table class="tablelist">
                     <thead>
                    <tr>
                        <th>代理商名称</th>
                        <th>代理商分类</th>
                        <th>代理商区域</th>
                        <th>是否审核</th>
                        <th>入驻时间</th>
                        <th>联系人</th>
                        <th>联系人手机</th>
                    </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="Rpt_Distribute" runat="server">
                   <ItemTemplate>
                    <tr>
                         <td><a href="#" class="DisShow" id="<%#Eval("id") %>"> <%# Eval("DisName") %></a></td>
                         <td><%# Common.GetDisTypeNameById(Eval("DisTypeID").ToString().ToInt(0))%></td>
                         <td><%# Common.GetDisAreaNameById(Eval("AreaID").ToString().ToInt(0))%></td>
                         <td><%# Eval("AuditState").ToString() == "2" ? "是" : "<span style='color:red'>否</span>"%></td>
                         <td><%# Eval("CreateDate").ToString().ToDateTime().ToString("yyyy-MM-dd")%></td>
                         <td><%# Eval("Principal")%></td>
                         <td><%# Eval("phone")%></td>
                    </tr>
                    </ItemTemplate>
                    
                    </asp:Repeater>
                </tbody>
            </table>
            <!--信息列表 end-->

            <!--列表分页 start--> 
            <div class="pagin" style=" height:30px;">
                  <webdiyer:AspNetPager ID="Pager" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                     NextPageText=">"  PageSize="5" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
             ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                       TextBeforePageIndexBox="<span class='jump'>跳转到:</span>"
                                    CustomInfoSectionWidth="20%"  CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;"  CustomInfoClass="message" ShowCustomInfoSection="Left"
                  CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                     CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                    OnPageChanged="Pager_PageChanged" >
                </webdiyer:AspNetPager>
            </div>
            </div>

            <div id="tab3" runat="server">
                <div class="tools" id="add" runat="server">
                  <ul class="toolbar left">
                    <li id="libtnUserAdd" runat="server"  visible="false"  ><span><img src="../../Company/images/t01.png" /></span>新增用户</li>
                  </ul>
                </div>
                  <table class="tablelist">
                     <thead>
                    <tr>
                        <th>登录帐号</th>
                        <th>姓名</th>
                   <%--     <th>性别</th>--%>
                        <th>是否审核</th>
                        <th>类型</th>
                        <th>手机号码</th>
                      <%--  <th>身份证</th>--%>
                    </tr>
                </thead>
                <tbody>
                  <asp:Repeater ID="Rpt_User" runat="server">
                   <ItemTemplate>
                    <tr>
                         <td><%# Eval("UserName")%></td>
                         <td><%# Eval("TrueName")%></td>
                     <%--    <td><%# Eval("Sex") %></td>--%>
                         <td><%# Eval("isaudit").ToString() == "2" ? "是" : "<span style='color:red'>否</span>"%></td>
                         <td><%# Eval("Type").ToString() == "4" ? "管理员" : "普通用户"%></td>
                         <td><%# Eval("Phone")%></td>
                        <%-- <td><%# Eval("Identitys")%></td>--%>
                    </tr>
                    </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>

                     <div class="pagin" style=" height:30px;">
                     <webdiyer:AspNetPager ID="Pager1" runat="server" EnableTheming="true" PageIndexBoxType="TextBox"
                     NextPageText=">"  PageSize="5" PrevPageText="<" AlwaysShow="True" UrlPaging="false"
             ShowPageIndexBox="Always" TextAfterPageIndexBox="<span class='pagiPage'>页</span>"
                       TextBeforePageIndexBox="<span class='jump'>跳转到:</span>"
                                    CustomInfoSectionWidth="20%"  CustomInfoStyle="padding:5px 0 0 20px;cursor: default;color: #737373;"  CustomInfoClass="message" ShowCustomInfoSection="Left"
                  CustomInfoHTML="共%PageCount%页，每页%PageSize%条，共%RecordCount%条"
                     CssClass="paginList" CurrentPageButtonClass="paginItem" NumericButtonCount="5"
                    OnPageChanged="Pager_PageChanged1" >
                </webdiyer:AspNetPager>
            </div>
            </div>

               </div>
             </div>

         <div class="AddWindow tip" style="display: none;">
            <div style=" position:absolute;top:0; left:0; right:0; bottom:0;z-index:999; background:#fff;">
            <div class="tiptop">
                <span>新增</span><a></a></div>
            <div class="tipinfo">
                <div class="lb">
                    <span><i class="required">*</i>登录帐号：</span>
                    <input name="txtUserName" id="txtUserName" style=" margin-left:2px;"  type="text" runat="server" class="textBox txtUserName" />
                        </div>
                          <div class="lb">
                    <span><i class="required">*</i>登录密码：</span><input name="txtUserPwd" id="txtUserPwd" type="password"
                        runat="server" class="textBox txtUserPwd" /></div>
                        <div class="lb">
                    <span><i class="required">*</i>确认密码：</span><input name="txtUserPwds" type="password"
                                runat="server" id="txtUserPwds"  class="textBox txtUserPwds" /></div>
                                <div class="lb">
                    <span><i class="required">*</i>角色：</span>
                                <input type="text" id="txtrole" name="txtrole" runat="server" class="textBox txtUserName" readonly="readonly" value="厂商用户" style="margin-left:2px;"/>
                   <%-- <asp:DropDownList runat="server"  ID="ddlRoleId"></asp:DropDownList>--%>
                        </div>
                        <div class="lb">
                    <span><i class="required">*</i>姓名：</span>
                    <input name="txtUserTrueName" id="txtUserTrueName" style=" margin-left:2px;"  type="text" runat="server" class="textBox txtUserTrueName" />
                        </div>
                              <div class="lb">
                    <span><i class="required">*</i>手机号码：</span><input name="txtUserPhone" id="txtUserPhone" type="text"
                        runat="server" maxlength="11" class="textBox txtUserPhone" /></div>
                        <div class="tipbtn">
                    <asp:Button ID="btnAdd" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return  formCheck();"
                        OnClick="btnAdd_Click" />&nbsp;
                    <input name="" type="button" class="cancel" value="取消" />
                </div>
            </div>
            </div>
            <div  style="z-index:11; opacity: 0.3;filter: Alpha(Opacity=30) !important; border-radius:5px;top: -8px; left: -8px; right: -8px; bottom: -8px;  background-color: rgb(0, 0, 0); position:absolute; top"></div>
        </div>

       <div class="AddWindow2 tip" style="display: none;">
         <div style=" position:absolute;top:0; left:0; right:0; bottom:0;z-index:999; background:#fff;">
            <div class="tiptop">
               <span>绑定业务员</span><a></a>
            </div>
           <div class="tipinfo">
               <div class="lb">
                    <span><i class="required">*</i>机构：</span>
                    <asp:DropDownList runat="server" ID="Org" CssClass="XlCity" ></asp:DropDownList>

                </div>
               <div class="lb">
                    <span><i class="required">*</i>业务员：</span>
                    <asp:DropDownList runat="server" ID="SaleMan" CssClass="XlCity" ></asp:DropDownList>
               </div>
               <div class="tipbtn">
                    <asp:Button ID="btnOrgBind" CssClass="orangeBtn" runat="server" Text="确定" OnClientClick="return  formCheck2();"
                        OnClick="btnBind_Click" />&nbsp;
                    <input name="" type="button" class="cancel" value="取消" />
                     <input type="hidden" id="salemanid" runat="server" />
                </div>
           </div>
           <input type="hidden" id="hid_Alert" runat="server" />
       </div>
       </div>
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FirstCampInfo.aspx.cs" Inherits="Distributor_CMerchants_FirstCampInfo" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>首营详细</title>
     <meta http-equiv = "X-UA-Compatible" content="IE=edge,chrome=1" />
     <link href="../../Distributor/newOrder/css/global2.5.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>" rel="stylesheet" type="text/css" />
    <link href="../../Company/css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../Company/css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>

    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <style>
         .divText
        {
            width:180px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!--头部 start-->
        <Head:Head ID="Head1" runat="server" />
        <!--头部 end-->
        <!--左侧 start-->
        <Left:Left ID="Left1" runat="server" ShowID="nav-7" />
        <input type="hidden" id="hid_Alert" />
        <input type="hidden" id="hidid" runat="server" />

        <!--左侧 end-->
        <div class="rightinfo">
            <div class="info">
                <a id="navigation1" href="../UserIndex.aspx">我的桌面</a>> <a id="navigation2" runat="server" href="../CMerchants/FirstCampList.aspx">首营列表</a>> <a id="navigation3" href="#">首营详情</a>
            </div>
            <div class="tools">
              <ul class="toolbar left">
                    <li id="libtnEdit" visible="false" runat="server"><span><img src="../../Company/images/t02.png" /></span>编辑</li>
                    <li id="libtnSubmit" visible="false" runat="server"><span><img src="../../Company/images/t02.png" /></span>提交</li>
                    <li id="liReturn"><span><img src="../../Company/images/tp3.png" /></span>返回</li>
                    <input type="button" runat="server" id="btnEdit" onserverclick="btn_Edit" style="display:none;"  />
              </ul>
            </div>

            <div class="c-n-title">基本信息</div>
            <ul class="coreInfo">
                <li class="lb fl">
                    <i class="name">招商编码</i>
                    <input type="text" class="box1" placeholder="" readonly="readonly" runat="server" maxlength="50" id="txtCMCode"/>
                </li>
                <li class="lb fl">
                    <i class="name">招商名称</i>
                    <input type="text" maxlength="50" style="margin-left:2px" readonly="readonly" class="box1" id="txtCMName" runat="server" />
                </li>
                <li class="lb fl">
                    <i class="name">厂商名称</i>
                    <input type="text" class="box1" placeholder="" readonly="readonly" runat="server" maxlength="50" id="txtCompName"/>
                </li>
                <li class="lb fl">
                    <i class="name">商品编码</i>
                    <input type="text" maxlength="50" style="margin-left:2px" readonly="readonly" class="box1" id="txtGoodsCode" runat="server" />
                </li>
                 <li class="lb fl">
                    <i class="name">商品名称</i>
                    <input type="text" class="box1" placeholder="" readonly="readonly" runat="server" maxlength="50" id="txtGoodsName"/>
                </li>
                <li class="lb fl">
                    <i class="name">规格型号</i>
                    <input type="text" maxlength="50" style="margin-left:2px" readonly="readonly" class="box1" id="txtValueInfo" runat="server" />
                </li>
                <li class="lb fl">
                    <i class="name">生效日期</i>
                    <input type="text" class="box1" placeholder="" readonly="readonly" runat="server" maxlength="50" id="txtForceDate"/>
                </li>
                <li class="lb fl">
                    <i class="name">失效日期</i>
                    <input type="text" maxlength="50" style="margin-left:2px" readonly="readonly" class="box1" id="txtInvalidDate" runat="server" />
                </li>
                <li class="lb fl">
                    <i class="name">医院</i>
                    <input type="text" class="box1" placeholder="" readonly="readonly" runat="server" maxlength="50" id="txtHtName"/>
                </li>
                <li class="lb fl">
                    <i class="name">状态</i>
                    <input type="text" maxlength="50" style="margin-left:2px" readonly="readonly" class="box1" id="txtState" runat="server" />
                </li>
                <li class="lb fl">
                    <i class="name">处理备注</i>
                    <input type="text" maxlength="50" style="margin-left:2px" readonly="readonly" class="box1" id="txtRemark" runat="server" />
                </li>
                <li class="lb fl">
                    <i class="name">申请备注</i>
                    <input type="text" maxlength="50" style="margin-left:2px" readonly="readonly" class="box1" id="txtApplyRemark" runat="server" />
                </li>
             </ul>
            <div class="blank10"></div>
            <div class="c-n-title">需提供资料</div>
            <ul class="coreInfo">
                <li class="lb clear" id="UpFile1" runat="server" visible="false">
    	            <i class="name fl" style=" width:120px;">营业执照</i>
    	             <div class="con upload">
                         <div style="float:left" class="divText">
                            <div id="UpFileText" runat="server">
                            </div>
                         </div>     
                             <i class="gclor9" style="margin-left:30px;">有效期
                                <input runat="server"  style="width: 130px;" id="validDate1" readonly="readonly" type="text" class="Wdate" value="" />
                                </i>
                       </div>
                 </li>

                 <li class="lb clear" id="UpFile2" runat="server" visible="false">
    	            <i class="name fl" style=" width:120px;">医疗器械经营许可证</i>
    	             <div class="con upload">
                         <div style="float:left" class="divText">
                            <div id="UpFileText2" runat="server">
                            </div>
                         </div>     
                             <i class="gclor9" style="margin-left:30px;">有效期
                                <input runat="server"  style="width: 130px;" id="validDate2" readonly="readonly" type="text" class="Wdate" value="" />
                                </i>
                       </div>
                 </li>
                 <li class="lb clear" id="UpFile3" runat="server" visible="false">
    	            <i class="name fl" style=" width:120px;">开户许可证</i>
    	             <div class="con upload">
                         <div style="float:left" class="divText">
                            <div id="UpFileText3" runat="server">
                            </div>
                         </div>     
                             <i class="gclor9" style="margin-left:30px;">有效期
                                <input runat="server"  style="width: 130px;" id="validDate3" readonly="readonly" type="text" class="Wdate" value="" />
                                </i>
                       </div>
                 </li>
                 <li class="lb clear" id="UpFile4" runat="server" visible="false">
    	            <i class="name fl" style=" width:120px;">医疗器械备案</i>
    	             <div class="con upload">
                         <div style="float:left" class="divText">
                            <div id="UpFileText4" runat="server">
                            </div>
                         </div>     
                             <i class="gclor9" style="margin-left:30px;">有效期
                                <input runat="server"  style="width: 130px;" id="validDate4" readonly="readonly" type="text" class="Wdate" value="" />
                                </i>
                       </div>
                 </li>
            </ul>

            <div class="blank10"></div>
            <div class="c-n-title">授权书</div>
             <ul class="coreInfo">
                 <li class="lb clear" id="UpFile5" runat="server" visible="false">
                    <i class="name fl" style=" width:120px;">授权书</i>
    	             <div class="con upload">
                         <div style="float:left" class="divText">
                            <div id="UpFileText5" runat="server">
                            </div>
                         </div>     
                       </div>
                 </li>
             </ul>
             <div class="blank10"></div>
             <div class="c-n-title">合 同</div>
             <ul class="coreInfo">
                <div class="tabLine" id="GoodsUL">
                    <div class="productListBox table-wrap-lite" style="float: left; margin-bottom: 5px;">
                        <table border="0" cellspacing="0" cellpadding="0" style="width:1050px;margin:0 auto;">
                            <thead>
                                <tr>
                                    <th class="t3">合同编号</th>
                                    <th class="t3">生效日期</th>
                                    <th class="t3">失效日期</th>
                                    <th class="t3">附件</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptCon" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                             <td class="trOp"><div class="tc"><%# Eval("contractNO")%></div></td>
                                             <td class="trOp"><div class="tc"><%# Eval("ForceDate", "{0:yyyy-MM-dd}")%></div></td>
                                             <td class="trOp"><div class="tc"><%# Eval("InvalidDate", "{0:yyyy-MM-dd}")%></div></td>
                                             <td class="trOp"><div class="tc"><a  class="bclor SeeCont" tip="<%# Eval("ContID")%>">查看</a></div></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
             </ul>
             <div class="blank10"></div>
        </div>

        <script>
            $(function () {
                //返回
                $("li#liReturn").on("click", function () {
                    window.location.href = 'FirstCampList.aspx';
                });
                //提交
                $("li#libtnSubmit").on("click", function () {
                    layerCommon.confirm("确认提交？", function () { $("#btnEdit").trigger("click"); }, "提示");
                })

                //编辑
                $("li#libtnEdit").on("click", function () {
                    var url = 'FirstCampEdit.aspx?KeyID=' + $("#hidid").val();
                    var index = layerCommon.openWindow("编辑", url, '950px', '615px'); //记录弹出对象
                    $("#hid_Alert").val(index); //记录弹出对象
                })

                //查看合同附件
                $(document).on("click", ".SeeCont", function () {
                    var id = $.trim($(this).attr("tip"));

                    var url = 'ContractMvg.aspx?KeyID=' + id;
                    var index = layerCommon.openWindow("合同资料", url, '575px', '415px'); //记录弹出对象
                    $("#hid_Alert").val(index); //记录弹出对象
                });
            });


            function Close() {
                layerCommon.msg("编辑成功", window.parent.IconOption.笑脸);
                layerCommon.layerClose("hid_Alert");
                window.location.reload();
            }
        </script>
    </form>
</body>
</html>

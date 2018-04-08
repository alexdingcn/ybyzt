<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayCreateAdd.aspx.cs" Inherits="Company_Pay_PayCreateAdd" %>
<%@ Register Src="~/Company/UserControl/DisAreaTreeBox.ascx" TagPrefix="uc1" TagName="DisArea" %>
<%--<%@ Register Src="~/Company/UserControl/TextDisList.ascx" TagPrefix="uc1" TagName="DisList" %>--%>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>钱包补录</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/odrerstyle.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <link href="../../css/login.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../js/OpenJs.js"></script>
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../js/Pay.js" type="text/javascript"></script>

    <link href="../css/Enterprice.css" rel="Stylesheet" type="text/css" />
    <%-- 附件凭证上传样式  end--%>   
    <style>
            .attach .bule {
        margin: 0px 3px;
        }
        .bule, a.bule {
        color: #2670de;
        }
        .a-upload {
        position: relative;
        cursor: pointer;
        overflow: hidden;
        z-index: 1;
        }
        .a-upload input {
        position: absolute;
        font-size: 100px;
        right: 0;
        top: 0;
        opacity: 0;
        filter: alpha(opacity=0);
        cursor: pointer;
        z-index: 999;
        }
        .attach .add .txt {
        color: #999;
        }
        .add{display: inline-flex;}
            </style>
    <%-- 附件凭证上传样式  end--%>    
    <%-- 附件新增 begin--%>

    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/ajaxfileupload.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>

    <%--附件新增  end--%>

    <script type="text/javascript">
        /*企业钱包补录 start */
        $(document).ready(function () {
            $_def.ID = "btnSave";
                       
            //选择加盟商
            $("#txtDisID").click(function () {

                ///获取光标位置
                var x = $("#txtDisID").offset().left;
                var y = $("#txtDisID").offset().top;
                var Id = $("#hidCompId").val();

                ChoseProductClass('Tree_Dis.aspx?Id=' + Id, x, y);

            });


        });
        //验证用
        function formCheck() {
            var str = "";
            // 代理商ID
            var txtcomp = $("#hidDisUserID").val(); 
            
            var txtprice = $("#txtPayCorrectPrice").val();
            var txtremark = $("#txtRemark").val();
            var ddltype = $("#ddltype").val();
            var ddlPaytype = $("#ddlPaytype").val();
            if (txtcomp == "") {
                str += "-请选择代理商；\r\n";
            }
            if (txtprice == "") {
                str += "-请填写补录金额；\r\n";               
            } else if (parseFloat(txtprice) <= 0)
                str += "-补录金额不能小于或等于零；\r\n";

            if (ddltype == "-1") {
                str += "-请选择预收款来源；\r\n";
            }
            if (ddlPaytype == "-1") {
                str += "-请选择支付方式；\r\n";
            }
//            if (txtremark == "") {
//                str += "-请填写备注；\r\n";
//            }
            if (txtremark.length > 200) {
                str += "-备注字数不能大于200个字符；\r\n";
            }


            if (str != "") {
                layerCommon.msg(str, IconOption.错误);
                return false;
            } else {
                return true;
            }

        }

        function getFileName(o) {
            var pos = o.lastIndexOf("\\");
            return o.substring(pos + 1);
        }

        //金额只能输入正数和小数
        function KeyIntPrice(val) {
            val.value = val.value.replace(/[^\d.]/g, '');
        }
    </script>

    <style type="text/css">
        input[type='text']
        {
            width:170px;
            margin-left:10px;
        }
       
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:top ID="top1" runat="server" ShowID="nav-2" />
    <div class="rightinfo">
    <!--当前位置 start-->
    <div class="place">
        <ul class="placeul">
            <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
            <li><a href="../pay/PayDisList.aspx">钱包查询</a></li><li>></li>
            <li><a href="#">钱包补录</a></li>  
        </ul>
    </div>
    <input id="hid_Alert" type="hidden" />
    <input type="hidden" id="hidCompId" runat="server"/>
    <!--当前位置 end-->
        <div >
            <!--收款帐号管理新增 start-->
            
                <div class="lbtb layoutRegist">
                    <table class="tb dh">
                        <tr>
                            <td style=" width:15%;">
                                <span><label style=" color:Red; display:inline-block;"><i class="required">*</i></label>&nbsp;代理商名称</span>
                            </td>
                            <td style=" width:30%;">
                                <%--<uc1:DisList runat="server" ID="DisListID" />--%>
                                 <label id="txtDisUser" runat="server"> </label>
                                 <input type="hidden" id="hidDisUserID" runat="server" />
                            </td>
                             <td style=" width:15%;">
                                <span><label style=" color:Red; display:inline-block;"><i class="required">*</i></label>&nbsp;补录金额</span>
                            </td>
                            <td style=" width:30%;">
                                <input id="txtPayCorrectPrice" onkeyup='KeyIntPrice(this)'  onblur="KeyIntPrice(this)" maxlength="10" type="text" runat="server" class="downBox" />               
                             </td>
                        </tr>
                      
                        <tr>
                           <td>
                                 <span><label style=" color:Red; display:inline-block;"><i class="required">*</i></label>&nbsp;预收款来源</span>
                            </td>
                            <td >
                                <select id="ddltype" runat="server" class="textBox" style="width: 150px; margin-top:2px;">
                                <option value="-1">请选择</option>
                                <option value="代理商充值">代理商充值</option>
                                <option value="奖励">奖励</option>
                                <option value="返利">返利</option>
                                <option value="调价补偿">调价补偿</option>
                                <option value="积分兑换">积分兑换</option>
                                <option value="其它">其它</option>
                                </select>
                            </td>  
                             <td>
                                 <span><label style=" color:Red; display:inline-block;"><i class="required">*</i></label>&nbsp;支付方式</span>
                            </td>
                            <td >
                                <select id="ddlPaytype" runat="server" class="textBox" style="width: 150px; margin-top:2px;">
                                <option value="-1">请选择</option>
                                <option value="现金">现金</option>
                                <option value="支票">支票</option>
                                <option value="本票">本票</option>
                                <option value="汇票">汇票</option>
                                <option value="电汇">电汇</option>
                                <option value="赠送">赠送</option>
                                </select>
                            </td>                           
                        </tr>
                         <tr>
                        <td>
                            <span>上传付款凭证</span>
                        </td>
                        <td colspan="3">     
                      <%-- 附件凭证上传  start--%>    
                               <div class="bz attach">
                                <ul class="list" style="margin-left: 5px;">
                                </ul>
                                <div class="add">
                                    <a href="javascript:;" class="a-upload bule" >
                                        <input type="file" name="AddBanner" id="AddBanner" class="AddBanner" onchange="uploadAvatar(this,'<%= Common.GetWebConfigKey("ImgViewPath") %>','');" />+新增附件</a><i
                                            class="txt">（附件最大20M，支持格式：PDF、Word、Excel、Txt、JPG、PNG、BMP、GIF、RAR、ZIP）</i>
                                    <asp:HiddenField ID="hrOrderFj" runat="server" />
                                </div>
                            </div>
                      <%-- 附件凭证上传  end--%>
                           
                        </td>
                    </tr>
                        
                        <tr>
                            <td><span style=" height:60px;padding-top: 15px;""><span>&nbsp;备注</span></td>
                            <td colspan="3">
                                <textarea id="txtRemark" maxlength="200" class="textarea" placeholder="订单备注不能大于200个字符" runat="server"></textarea>
                            </td>
                        </tr>
                    </table>
                </div>
            <%--</div>--%>

            <!--销售订单主体 start-->

          
            <div class="footerBtn">
                <asp:Button ID="btnSave" runat="server" Text="确定" CssClass="orangeBtn" OnClick="btnSave_Click" OnClientClick="return formCheck()"/>&nbsp;
                <input name="" type="button" onclick="javascript:history.go(-1);" class="cancel" value="返回" />
            </div>
        </div>
    </div>
    
    </form>
</body>
</html>

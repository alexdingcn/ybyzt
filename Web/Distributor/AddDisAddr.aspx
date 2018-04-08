<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddDisAddr.aspx.cs" Inherits="AddDisAddr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改收货地址</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <link href="css/global.css" rel="stylesheet" type="text/css" />
    <link href="css/shop.css" rel="stylesheet" type="text/css" />
    <script src="../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../js/CitysLine/New_JQuery-Citys-online-min.js" type="text/javascript"></script>
    <script src="../js/layer/layer.js" type="text/javascript"></script>
    <script src="../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/shopcart.js" type="text/javascript"></script>
    <script src="../js/GetPhoneCode.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <!--[if !IE]>修改地址 start<![endif]-->
    <%--<div class="modifyBg"></div>--%>
    <div class="modify">
        <!--<div class="title">修改收货地址<a href="" class="close"></a></div>-->
        <div class="n">
            <input type="hidden" id="hidKeyID" runat="server" />
            <input type="hidden" id="hidType" runat="server"/>
            <ul class="list">
                <asp:Repeater ID="rpt_Addr" runat="server">
                    <ItemTemplate>
                        <li>
                            <input name="addr" type="radio" value="<%# Eval("ID") %>" class="dx" />
                            <label class="principal">
                                <%# Eval("principal") %></label>
                            <label class="Address">
                                <%# Eval("Address")%></label>
                            <%# Eval("isdefault").ToString() == "1" ? "(默认地址)" : ""%>
                            <label class="phone">
                                <%# Eval("phone") %></label>
                            <a href="javascript:void(0);" class="link upAddr">修改</a>
                            <input type="hidden" class="pr" value="<%# Eval("Province") %>" />
                            <input type="hidden" class="ci" value="<%# Eval("City") %>" />
                            <input type="hidden" class="ar" value="<%# Eval("Area") %>" />
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
                <li>
                    <input name="addr" type="radio" value="" class="dx" />
                    <a href="javascript:void(0);" class="upAddr">使用新地址</a> </li>
            </ul>
            <div class="add">
                <div class="li">
                    <div class="bt">收货人</div>
                    <input name="" id="txtprincipal" tip_id="" type="text" class="box" />
                    <label style=" color:Red;" id="lblprincapal"></label>
                </div>
                <div class="li">
                    <div class="bt">手机</div>
                    <input name="" id="txtphone" type="text" class="box" />
                    <label style=" color:Red;" id="lblphone"></label>
                </div>
                <div class="li">
                    <div class="bt">
                        地址</div>
                    <div class="address">
                        <ul>
                            <li>
                                <input type="button" id="txtProvince" class="text" onclick="beginSelect(this);" value="" />
                                <span class="arrow" onmousedown="beginSelect(this)"></span></li>
                            <li class="prov select"></li>
                        </ul>
                        <ul>
                            <li>
                                <input type="button" id="txtCity" class="text" onclick="beginSelect(this);" value="" />
                                <span class="arrow" onmousedown="beginSelect(this)"></span></li>
                            <li class="city select"></li>
                        </ul>
                        <ul>
                            <li>
                                <input type="button" id="txtArea" class="text" onclick="beginSelect(this);" value="" />
                                <span class="arrow" onmousedown="beginSelect(this)"></span></li>
                            <li class="dist select"></li>
                        </ul>
                    </div>
                </div>
                <div class="li">
                    <div class="bt">详细地址</div>
                    <input name="" id="txtAddress" type="text" class="box" />
                    <label style=" color:Red;" id="lblAddrress"></label>
                </div>
                <div class="li">
                    <div class="bt">手机验证码</div>
                    <input name="" id="txtPhoneCode" type="text" class="box wid" />
                    <i class="tel">手机：<label id="lblDisPhone" runat="server"></label></i> 
                    <a id="getcode" href="javascript:void(0);" class="link" onclick='getphonecode("<%=user.Phone %>","0","修改地址","<%=this.UserID %>","<%=this.UserName %>");'>获取验证码</a>
                    <label style=" color:Red;" id="spancode"></label>
                </div>
                <div class="li">
                    <a href="javascript:void(0);" class="link btnSaveAddr">保存</a>
                </div>
                <div class="blank20">
                </div>
            </div>
        </div>
        <div class="btn">
            <a href="javascript:void(0);" class="bule-btn btn_AddrSave">确定</a> 
            <a href="javascript:void(0);" class="gray-btn btn_Cancel">取消</a>
        </div>
    </div>
    <!--[if !IE]>修改地址 end<![endif]-->
    </form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ContractInfo.aspx.cs" Inherits="Company_Contract_ContractEdit" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>合同编辑</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />


    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        rel="stylesheet" type="text/css" />
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>
   
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .rightinfo {
            margin-left: 130px;
            margin-top: 60px;
            width: 1060px;
            padding: 8px;
        }
        /*公共-标题*/
        .c-n-title {
            line-height: 36px;
            height: 36px;
            /*background: #f2f4f6;*/
            font-size: 13px;
            padding-left: 13px;
            border-radius: 5px;
            margin: 10px 0 5px 0;
        }

            .c-n-title .set {
                font-size: 12px;
                margin-left: 15px;
                padding-top: 1px;
            }

                .c-n-title .set .r-check + label {
                    top: 5px;
                    margin-right: 5px;
                }
        /*公共-文本框*/
        .coreInfo .box1 {
            width: 360px;
        }

        .box1, .box2 {
            border: 1px solid #ddd;
            border-radius: 5px;
            height: 34px;
            line-height: 34px;
            padding: 0px 10px;
            color: #555;
            font-size: 12px;
        }

        .box2 {
            width: 120px;
        }

        .coreInfo .name {
            width: 90px;
            text-align: right;
            display: inline-block;
            padding-right: 10px;
            line-height: 35px;
            color: #666;
        }

        i {
            font-style: normal;
        }

        .coreInfo .lb {
            padding-top: 10px;
            min-width: 482px;
            color: #666;
            position: relative;
            margin-right: 30px;
            min-height: 36px;
        }

        .fl {
            float: left;
        }

        li, dl {
            list-style-type: none;
        }

        .box {
            padding: 0px 10px;
            border: 1px solid #ddd;
            border-left: 0;
            border-top: 0;
            border-right: 0;
            width: 190px;
            height: 27px;
            line-height: 27px;
            color: #999;
            padding-left: 5px;
            font-family: "微软雅黑";
            font-size: 12px;
        }

        .goods-zs > .tabLine {
            position: initial;
        }

        #DropDownList1 {
            border: 1px #999 solid;
            width: 150px;
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        $(function () {
           


        })
        function btnUpdate() {
            window.location = "ContractEdit.aspx?cid=<%=cid%>";
        }
    </script>
    <style type="text/css">
        .center {
            text-align: center;
            
        }
        select {
            background-color:#ddd
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <input id="hid_Alert" type="hidden" />
        <input id="hid_Type" type="hidden" value="1" runat="server" />
        <input id="hid_No" type="hidden" value="0" runat="server" />
        <input type="hidden" id="hidCompId" runat="server" />
        <uc1:Top ID="top1" runat="server" ShowID="nav-8" />
        <div class="rightCon">
            <div class="info">
                 <a href="../jsc.aspx">我的桌面</a>>
                 <a href="ContractList.aspx" runat="server" id="A1">合同列表</a>>
                 <a href="#" runat="server" id="title">合同详情</a>
            </div>
            <!--[if !IE]>商品展示区 start<![endif]-->
            <div class="goods-zs">


                    <div id="DisBoot" class="tools">
            <ul class="toolbar left">
                <li id="btnUpdate" runat="server" onclick="btnUpdate()"><span>
                    <img src="../images/t02.png" /></span><font>修改</font></li>
            </ul>
        </div>


                <!--[if !IE]>选择代理商 start<![endif]-->
                <div class="c-n-title">基本信息</div>
                <ul class="coreInfo">
                    <li class="lb fl"><i class="name"><i class="red">*</i>合同号</i>
                        <input type="text" class="box1 contractNO" id="txtcontractNO" runat="server" name="txtcontractNO" maxlength="100" disabled="disabled" />
                    </li>
                    <li class="lb fl"><i class="name"><i class="red">*</i>日期</i>
                        <input name="contractDate" runat="server" onclick="WdatePicker()" disabled="disabled"
                            id="txtcontractDate" readonly="readonly" type="text" class="box1" value="" />
                    </li>



                    <li class="lb fl">
                        <i class="name "><i class="red">*</i>代理商</i>


                        <asp:DropDownList ID="DropDis" runat="server" CssClass="box1" Width="382" disabled="disabled"></asp:DropDownList>

                    </li>
                    <li class="lb fl"><i class="name"><i class="red">*</i>状态</i>
                        <select name="CState" runat="server" id="CState" style="width: 382px;" class="box1" disabled="disabled">
                            <option value="1">启用</option>
                            <option value="2">停用</option>
                        </select>
                    </li>

                    <li class="lb fl"><i class="name"><i class="red">*</i>生效日期</i>
                        <input name="txtForceDate" runat="server" onclick="WdatePicker()" disabled="disabled"
                            id="txtForceDate" readonly="readonly" type="text" class="box1" value="" />
                    </li>
                    <li class="lb fl"><i class="name"><i class="red">*</i>失效日期</i>
                        <input name="txtInvalidDate" runat="server" onclick="WdatePicker()" disabled="disabled"
                            id="txtInvalidDate" readonly="readonly" type="text" class="box1" value="" />
                    </li>

                    <li class="lb fl"><div   style="width:90px;float:left;height:82px;line-height:82px;text-align:right">备注</div>
                        <textarea style="width: 890px; height: 80px;margin-left:15px;" rows="6" runat="server" id="txtRemark" disabled="disabled" class="box1"></textarea>
                    </li>

                </ul>

              
                <div class="clear"></div>
                <div class="c-n-title">商品</div>
                <!--[if !IE]>选择代理商 end<![endif]-->
                <!--商品 start -->
                <div class="tabLine">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th class="t5"></th>
                                <th class="t1">商品名称
                                </th>
                                <th class="t3">规格属性
                                </th>
                                <th class="">医院
                                </th>
                                 <th class="">区域
                                </th>
                                <th class="t3">零售价
                                </th>
                                <th class="t5">折扣(%)
                                </th>
                                <th class="t5">价格
                                </th>
                                <th class="t5">年指标(元)
                                </th>
                                <th class="t3">备注
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="RepContractDetail" runat="server">
                                <ItemTemplate>
                                    <tr trindex="<%#Eval("ID")%>" trindex2="<%#Eval("ID")%>" id="<%#Eval("GoodsID") %>" tip="<%#Eval("GoodsInfoID") %>">
                                        <td  class="t3">
                                            <div class="addg">
                                                <a href="javascript:;" class="minus2"></a>
                                                <a href="javascript:;" class="add2"></a>
                                            </div>
                                        </td>
                                        <td class="t2" >
                                            <div class="sPic" style="width:270px;">
                                                <a class="opt-i2"></a>
                                                <span><a target="_blank" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html" onclick="return false;">
                                                    <img src="<%# Common.GetPicURL(Convert.ToString(Eval("pic")), "resize200") %>" width="60" height="60"></a>
                                                </span><a target="_blank" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html" onclick="return false;" class="code">商品编码：<%#Eval("GoodsCode") %> </a>
                                                <a target="_blank" style="width: 200px" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html" onclick="return false;" class="name"><%#Eval("GoodsName")%>  <i><%#Eval("GoodsName")%></i></a>
                                            </div>
                                        </td>
                                        <td class="t4 center"><%#Eval("ValueInfo")%></td>
                                        <td class="t3 center"><%#getHtName(Eval("HtID").ToString())%></td>
                                        <td class="t3 center"><%# Eval("AreaID").ToString() == "" ? "" : Common.GetDisAreaNameById(Eval("AreaID").ToString().ToInt(0))%></td>
                                        <td class="t3 center"><%# Convert.ToDecimal( Eval("SalePrice")).ToString("#.00")%></td>
                                        <td class="t5 center"><%# Eval("discount").ToString()=="0.0000"?"0": Convert.ToDecimal( Eval("discount")).ToString("#.00")%></td>
                                        <td class="t5 center"><%# Convert.ToDecimal( Eval("TinkerPrice ")).ToString("#.00")%></td>
                                        <td class="t5 center"><%# Eval("target").ToString()=="0.0000"?"0": Convert.ToDecimal( Eval("target")).ToString("#.00")%></td>
                                        <td class="t3 center"><%#Eval("Remark")%></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
				  <div class="clear"></div>


                <div class="c-n-title">附件</div>
                <ul class="coreInfo">
                    <li class="lb clear">
                        <div class="con upload">
                            <asp:Panel runat="server" ID="DFile" Style="margin: 5px 5px;">
                            </asp:Panel>
                        </div>

                        <div id="UpFileText" style="margin-left: 100px;">
                        </div>
                        <input runat="server" id="HidFfileName" type="hidden" />
                    </li>
                </ul>	
            </div>
            <div class="blank20">
            </div>
            <div class="blank20">
            </div>
            <div class="blank20">
            </div>
            <div class="blank20">
            </div>
            <div class="blank20">
            </div>
            <div class="blank20">
            </div>

            <div id="divGoodsName" class="divGoodsName" runat="server" style="display: none">
            </div>
        </div>

    </form>
</body>
</html>

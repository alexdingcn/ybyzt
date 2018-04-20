<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ContractInfo.aspx.cs" Inherits="Company_Contract_ContractEdit" %>

<%@ Register TagName="Head" TagPrefix="Head" Src="~/Distributor/DealerHead.ascx" %>
<%@ Register TagName="Left" TagPrefix="Left" Src="~/Distributor/DealerLeft.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>合同详情</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

 
  
    
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../Company/js/js.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../../js/UploadJs.js" type="text/javascript"></script>
    <link href="../css/global.css" rel="stylesheet" />
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

        #DFile {
            width:800px;
        }




        




        .goods-zs .sPic {
    padding: 17px 10px 10px 85px;
    min-height: 58px;
    position: relative;
    line-height: 25px;
}
        .tabLine table {
    width: 100%;
    border: 1px solid #eee;
}
       .tabLine td {
            border-left: 1px solid #eee;
        }
.goods-zs thead {
    background: #f7f7f7;
    height: 40px;
    line-height: 40px;
    font-size: 12px;
}.opt-i2 {
    width: 18px;
    height: 23px;
    background: url(../images/icon.png) no-repeat -28px 12px;
    display: inline-block;
    position: absolute;
    top: 30px;
    right: 10px;
    cursor: pointer;
}.goods-zs .sPic span {
    border: 1px solid #ddd;
    overflow: hidden;
    position: absolute;
    top: 12px;
    left: 10px;
}
 .goods-zs .sPic .name {
    display: block;
    color: #999;
    position: relative;
}
 .goods-zs .sPic .name i {
    position: absolute;
    top: 25px;
    left: -10px;
    background: #f5f7fa;
    border: 1px solid #e3e7ee;
    display: none;
    white-space: nowrap;
    padding: 5px 8px;
    z-index: 99;
}
i {
    font-style: normal;
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
        <Head:Head ID="Head2" runat="server" />
        <Left:Left ID="Left1" runat="server" ShowID="nav-8" />
        <div class="rightCon">
            <div class="info">
                 <a href="#">我的桌面</a>>
                 <a href="ContractList.aspx" runat="server" id="A1">合同列表</a>>
                 <a href="#" runat="server" id="title">合同详情</a>
            </div>
            <!--[if !IE]>商品展示区 start<![endif]-->
            <div class="goods-zs">

                <!--[if !IE]>选择代理商 start<![endif]-->
                <div class="c-n-title">基本信息</div>
                <ul class="coreInfo">
                    <li class="lb fl"><i class="name">合同号</i>
                        <input type="text" class="box1 contractNO" id="txtcontractNO" runat="server" name="txtcontractNO" maxlength="100" disabled="disabled" />
                    </li>
                    <li class="lb fl"><i class="name">日期</i>
                        <input name="contractDate" runat="server" onclick="WdatePicker()" disabled="disabled"
                            id="txtcontractDate" readonly="readonly" type="text" class="box1" value="" />
                    </li>
                    <li class="lb fl">
                        <i class="name ">厂商</i>
                          <input name="txtCompName" runat="server"  disabled="disabled"
                            id="txtCompName" readonly="readonly" type="text" class="box1" value="" />
                    </li>
                    <li class="lb fl"><i class="name">状态</i>
                        <input name="txtState" runat="server"  disabled="disabled"
                            id="txtState" readonly="readonly" type="text" class="box1" value="" />
                    </li>

                    <li class="lb fl"><i class="name">生效日期</i>
                        <input name="txtForceDate" runat="server" onclick="WdatePicker()" disabled="disabled"
                            id="txtForceDate" readonly="readonly" type="text" class="box1" value="" />
                    </li>
                    <li class="lb fl"><i class="name">失效日期</i>
                        <input name="txtInvalidDate" runat="server" onclick="WdatePicker()" disabled="disabled"
                            id="txtInvalidDate" readonly="readonly" type="text" class="box1" value="" />
                    </li>

                    <li class="lb fl"><i class="name">备注</i>
                        <textarea style="width: 890px; height: 80px;" rows="6" runat="server" id="txtRemark" disabled="disabled" class="box1"></textarea>
                    </li>

                </ul>

                <div class="blank20"></div>


                <div class="c-n-title">附件</div>
                <ul class="coreInfo">
                    <li class="lb clear">
                        <i class="name fl">上传三证</i>
                        <div class="con upload">
                            <asp:Panel runat="server" ID="DFile" Style="margin: 5px 5px;">
                            </asp:Panel>
                        </div>

                        <div id="UpFileText" style="margin-left: 100px;">
                        </div>
                        <input runat="server" id="HidFfileName" type="hidden" />
                    </li>
                </ul>



                <div class="blank20"></div>
                <div class="blank20"></div>

                <!--[if !IE]>选择代理商 end<![endif]-->
                <!--商品 start -->
                <div class="tabLine">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <thead>
                            <tr>
                                <th class="t1">商品名称
                                </th>
                                <th class="t3">商品规格属性
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
                                        <td class="t2" >
                                            <div class="sPic" style="width:270px;">
                                                
                                                <span><a target="_blank" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html" onclick="return false;">
                                                    <img src="<%# Common.GetPicURL(Eval("pic").ToString(), "resize200") %>" width="60" height="60"></a>
                                                </span><a target="_blank" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html" onclick="return false;" class="code">商品编码：<%#Eval("GoodsCode") %> </a>
                                                <a target="_blank" style="width: 200px" href="../../e<%# Eval("GoodsInfoID") %>_<%=this.CompID %>_.html" onclick="return false;" class="name"><%#Eval("GoodsName")%>  <i><%#Eval("GoodsName")%></i></a>
                                            </div>
                                        </td>
                                        <td class=" center" style="width:220px;"><%#Eval("ValueInfo")%></td>
                                        <td class="t3 center"><%#getHtName(Eval("HtID").ToString())%></td>
                                        <td class="t3 center"><%# Eval("AreaID").ToString() == "" ? "" : Common.GetDisAreaNameById(Eval("AreaID").ToString().ToInt(0))%></td>
                                        <td class="t3 center"><%# Convert.ToDecimal( Eval("SalePrice")).ToString("#.00")%></td>
                                        
                                        <td class="t5 center"><%# Convert.ToDecimal( Eval("discount")).ToString("#.00")%></td>
                                        <td class="t5 center" style="width:100px;"><%# Convert.ToDecimal( Eval("TinkerPrice ")).ToString("#.00")%></td>
                                        <td class="t5 center"><%# Convert.ToDecimal( Eval("target")).ToString("#.00")%></td>
                                        <td class="t3 center"><%#Eval("Remark")%></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
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


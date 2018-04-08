<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FirstCampMvg.aspx.cs" Inherits="Company_CMerchants_FirstCampMvg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>代理商资料</title>
    <link href="../../Distributor/newOrder/css/global2.5.css?v=201507261156" rel="stylesheet"
        type="text/css" />
    <script src="../../Company/js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>

    <style>
        .updt
        {
            text-align: right;
            line-height: 30px;    
        }
        .invoice-li .bt
        {
            width:120px;
        }
        .divText
        {
            width:150px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="popup po-invoice">
            <div class="invoice-li">
                <div class="li">
                    <div class="bt">代理商编码：</div>
                    <input type="text" id="txtDisCode" runat="server" readonly="readonly" autocomplete="off" class="box" />
                </div>
                <div class="li">
                    <div class="bt">代理商名称：</div>
                    <input type="text" id="txtDisName" runat="server" readonly="readonly" autocomplete="off" class="box" />
                </div>
                <div class="li">
                    <div class="bt">生效日期：</div>
                    <input type="text" id="txtForceDate" runat="server" readonly="readonly" autocomplete="off" class="box" />
                </div>
                <div class="li">
                    <div class="bt">失效日期：</div>
                    <input type="text" id="txtInvalidDate" runat="server" readonly="readonly" autocomplete="off" class="box" />
                </div>

                <div class="li" id="UpFile1" runat="server" visible="false">
                    <div class="bt">营业执照：</div>
                    <div class="con upload updt">
                        <div style="float:left" class="divText">
                            <div id="UpFileText" runat="server">
                            </div>  
                        </div>  
                        <div style="float:left">
                             <i class="gclor9" style="margin-left:30px;">有效期
                                <i id="validDate1" runat="server"></i>
                            </i> 
                        </div>
                    </div>
                </div>
                <div class="li" id="UpFile2" runat="server" visible="false">
                    <div class="bt">医疗器械经营许可证：</div>
                    <div class="con upload updt">
                        <div style="float:left" class="divText">
                            <div id="UpFileText2" runat="server">
                            </div>  
                        </div>  
                        <div style="float:left">
                             <i class="gclor9" style="margin-left:30px;">有效期
                                <i id="validDate2" runat="server"></i>
                            </i> 
                        </div>
                    </div>
                </div>
                <div class="li" id="UpFile3" runat="server" visible="false">
                    <div class="bt">开户许可证：</div>
                    <div class="con upload updt">
                        <div style="float:left" class="divText">
                            <div id="UpFileText3" runat="server">
                            </div>  
                        </div>  
                        <div style="float:left">
                             <i class="gclor9" style="margin-left:30px;">有效期
                                <i id="validDate3" runat="server"></i>
                            </i> 
                        </div>
                    </div>
                </div>
                <div class="li" id="UpFile4" runat="server" visible="false">
                    <div class="bt">医疗器械备案：</div>
                    <div class="con upload updt">
                        <div style="float:left" class="divText">
                            <div id="UpFileText4" runat="server">
                            </div>  
                        </div>  
                        <div style="float:left">
                             <i class="gclor9" style="margin-left:30px;">有效期
                                <i id="validDate4" runat="server"></i>
                            </i> 
                        </div>
                    </div>
                </div>
            </div>
            <div class="po-btn">
                <a href="javascript:void(0);" class="gray-btn" id="btnCancel">取消</a>
            </div>
        </div>
    
        
        <script>
            $(function () {
                //取消
                $(document).on("click", "#btnCancel", function () {
                    window.parent.layerCommon.layerClose("hid_Alert");
                });
            });
        </script>


    </form>
</body>
</html>

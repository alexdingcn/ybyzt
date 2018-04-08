<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AreaMvg.aspx.cs" Inherits="Company_Contract_AreaMvg" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>备注</title>
    <link href="../../Distributor/newOrder/css/global2.5.css" rel="stylesheet" />

    <style type="text/css">
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
            
            /*公共-联级选择*/
            .pop-menu{ position:absolute; z-index:99; left:100px; top:50px;box-shadow: 0px 0px 5px 0px #ccc; border:1px solid #ddd\0; border-radius:5px; background:#fff;}
            .menu-item{ float:left;width:150px; height:300px; overflow-y:auto; border-right:1px solid #ddd; padding:5px 0; position:relative; }
            .menu-item:last-child{border-right:none;}
            .menu-item li{ line-height:36px; height:36px; padding-left:12px; cursor:pointer; position:relative;}
            .menu-item li.cur,.menu-item li.cur:hover{background:#528fe9; color:#fff;}
            .menu-item li:hover{ background:#e4e8f1;}
            .menu-item li:after{ content:"";position:absolute; top:12px; right:5px;display: inline-block;width:0;height:0;line-height:0;border-width:5px;border-style:solid;border-color:transparent transparent transparent #bfcbd9 ;}
            /*.menu-item:last-child>li:after{ display:none;}*/
            .menu-item li.no:after{ display:none;}
            /* 设置滚动条的样式 */::-webkit-scrollbar {    width: 12px;}/* 滚动槽 */::-webkit-scrollbar-track {  background: rgba(0,0,0,0.1);   border-radius: 0px;}/* 滚动条滑块 */::-webkit-scrollbar-thumb {    border-radius: 10px;    background: rgba(0,0,0,0.1);    -webkit-box-shadow: inset 0 0 3px rgba(0,0,0,0.1);}::-webkit-scrollbar-thumb:window-inactive {    background: rgba(0,0,0,0.4);}
            .lb .arrow{right:12px; top:25px; cursor:pointer;}
            .lb .arrow.cur{right:12px;transform:rotate(180deg); top:21px;transition: transform .5s;}
            .lb .pullDown .arrow.cur{right:12px;transform:rotate(180deg); top:10px;transition: transform .5s;}
        </style>
</head>
<body style=" height:480px;">
    <form id="form1" runat="server" >
        <!--备注 start-->
        <div class="popup po-remark" style="width:550px;height:100px;">
            <input type="hidden" id="txtHtDrop" runat="server" value="" />
            <input type="hidden" id="hidTrId" runat="server" value="" />
            <input type="hidden" id="hidIndex" runat="server" value="" />
            <div style="width:500px;height:100px; padding: 30px 50px;">
                   <ul class="coreInfo">
                    <li class="lb fl">
    	                <i class="name">代理商区域</i>
 
                        <input type="text" class="box1" id="txtDisArea" readonly="readonly" runat="server"/>
                        <%--<span class="arrow"></span>--%>
                        <input type="hidden" class="box1" id="hid_txtDisArea" readonly="readonly" runat="server"/>               
                         <a id="aDisArea" style="color: #1a8fc2" href="javascript:void(0);"></a>
    	                <div class="pop-menu" style="width:605px; display:none;">
    	                </div>
                    </li>
                   </ul>
            </div>
            <div class="po-btn" style="width:100%;">
                <a href="javascript:void(0);" class="gray-btn" id="btnCancel">取消</a> <a href="#"
                    runat="server" class="btn-area" id="btnConfirm">确定</a>
            </div>
        </div>
        <!--备注 end-->
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/classifyview.js" type="text/javascript"></script>
        
        <script type="text/javascript">
            $(function () {
                //绑定代理商区域
                $("#txtDisArea").click(function () {
                    handleChange(this, "<%=DisArea%>");
                });

                //取消
                $(document).on("click", "#btnCancel", function () {
                    window.parent.CloseGoods();
                });
                //确定
                $("#btnConfirm").click(function () {
                    var Name = $.trim($("#txtDisArea").val());
                    var id = $.trim($("#hid_txtDisArea").val());
               
                    var hidTrId = $("#hidTrId").val();
                    window.parent.Areainfo(Name, id, hidTrId);
               
            });
        });

       
        </script>
    </form>
</body>
</html>

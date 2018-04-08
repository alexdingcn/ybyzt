<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ButtonToExcel.ascx.cs" Inherits="UserControl_ButtonToExcel" %>
<script>

    $(document).ready(function () {
        $("#lblbtnToExcel").on("click", function () {
            if ($("#<%=contect %>").length == 0) {
                alert("没有需要导出的excel数据");
                return;
            }
            $("#HidExcelId").val(document.getElementById("<%=contect %>").outerHTML.replace(/<A.*?>(.*?)<\/A>/ig, "$1").replace("<table", "<table border='1' cellSpacing='0' cellPadding='5' ").replace(/(<img[^>]*>)|(<a[^>]*>)|(<\/a>)|(<input[^>]*>)/ig, '').replace(/<th[^>(ead)]*>/ig, '<th style="background:#f0f5f7;height:50px;">'));
            window.open('<%=ResolveUrl("../toExcel.aspx") %>?cid=HidExcelId');
        })
    })
</script>
<li id="lblbtnToExcel" ><span><img src="<%=ResolveUrl("../Company/images/tp3.png") %>" /></span>导出</li>
<input type="hidden" id="HidExcelId" />
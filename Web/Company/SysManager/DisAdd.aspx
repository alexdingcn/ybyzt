<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisAdd.aspx.cs" Inherits="Company_SysManager_DisAdd" %>

<%@ Register Src="~/Company/UserControl/CompRemove.ascx" TagPrefix="uc1" TagName="CompRemove" %>
<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>代理商新增</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/CommonJs.js" type="text/javascript"></script>
    <script src="../../js/MoveLayer.js" type="text/javascript"></script>
      <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <style>
        i[error]{ color:Red; font-weight:bold; font-family:'微软雅黑'; font-size:16px;}
.batch i{ font-style:normal;}
.batch{ margin:0px auto; width:1000px; height:275px; position:absolute; top:50%; left:50%; margin:-120px 0 0 -500px; overflow:hidden;}
.batch a{ width:490px; height:275px;background:url(../images/batchImages1.jpg) no-repeat 0 0;position:relative; cursor:pointer; display:block;  float:left; }
.bulk i{ position:absolute; top:133px; color:#999; left:215px;}
.batch .alone{ background:url(../images/batchImages2.jpg) no-repeat 0 0; float:right;}
.alone i{ position:absolute; top:133px; color:#999; left:227px;}
</style>

<script>
    $(document).ready(function () {
        var div = $('<div   Msg="True"  style="z-index:10; background-color:#000; opacity:0.3; filter:alpha(opacity=50);position: fixed;width:100%;height:100%;top:0px;left:0px;"></div>');
        $("a.bulk").on("click", function () {
            $("body").append(div);
            $("div#DisImport").css("width", "500px").fadeIn(200);
        })
        $("input.cancel,.tiptop a").bind("click", function (event) {
            $(div).remove();
            $("div#DisImport").fadeOut(200);
        })
        $("div.tiptop").LockMove({ MoveWindow: "#DisImport" });
        //add by hgh 
        if ('<%=Request["nextstep"] %>' == "1") {
            Orderclass("ktxzjxs");
            document.getElementById("imgmenu").style.display = "block";
        }
    });

    function formChecks(obj) {
        var str = $("#FileUpload1").val();
        if (str == "") {
            layerCommon.msg("- 请选择要导入代理商Excel的文件",IconOption.错误);
            return false;
        }
        var suffix = $.trim(str.substring(str.lastIndexOf(".")));
        if (suffix == ".xlsx" || suffix == ".xls") {
            $(obj).attr("disabled", "disabled");
            return true;
        } else {
            layerCommon.msg("- 请选择ExcelL文件",IconOption.错误);
            return false;
        }
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:CompRemove runat="server" ID="Remove" ModuleID="2" />
    <uc1:top ID="top1" runat="server" ShowID="nav-4" />
  
 <div class="rightinfo">
    <div class="place">
        <ul class="placeul">
            <li><a href="../jsc.aspx">我的桌面 </a></li><li>></li>
            <li><a href="#" runat="server" id="atitle">代理商列表</a></li><li>></li>
            <li><a href="#" runat="server" id="btitle">代理商新增</a></li>
        </ul>
    </div>
 <div class="batch">
	<a href="#" title="表格导入" class="bulk"><i>先下载Excel导入模版， 填写完成后上传</i></a>
    <a href='DisEdit.aspx?nextstep=<%=Request["nextstep"] %>' title="界面录入"  class="alone"><i>添加少数代理商时，可以选择界面录入</i></a>
</div>

  </div>

     <div class="tip" style="display: none;" id="DisImport">
        <div style=" position:absolute;top:0; left:0; right:0; bottom:0;z-index:999; background:#fff;">
        <div class="tiptop">
            <span>代理商表格导入</span><a></a></div>
        <div class="tipinfo">
            <div class="lb">
                <span><b class="hint">1</b> 模版下载： </span><a href="TemplateFile/代理商表格导入模版.xls" style="text-decoration: underline"
                    target="_blank">代理商表格导入模版.xls</a> <font color="red">（另存到本地电脑进行编辑并保存）</font>
            </div>
            <div class="lb">
                <span><b class="hint">2</b> 上传文件：</span>
                <asp:FileUpload ID="FileUpload1" runat="server" style=" width:150px;" />
                <font color="red">（选择刚下载并完成编辑的模版）</font>
            </div>
            <div class="tipbtn" style="margin-left: 155px">
                <input type="button" id="btnAddList" class="orangeBtn" runat="server" value="确定" onclick="if(!formChecks(this)){return false;}"
                    onserverclick="btnAddList_Click" />&nbsp;
                <input name="" type="button" class="cancel" value="取消" />
            </div>
        </div>
        </div>
        <div  style="z-index:11; opacity: 0.3;filter: Alpha(Opacity=30) !important; border-radius:5px;top: -8px; left: -8px; right: -8px; bottom: -8px;  background-color: rgb(0, 0, 0); position:absolute; top"></div>
    </div>

    </form>
</body>
</html>

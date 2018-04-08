<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Tree_Addr.aspx.cs" Inherits="Company_Order_Tree_Addr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../css/demo.css" rel="stylesheet" type="text/css" />
    <link href="../../css/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.ztree.core-3.5.js" type="text/javascript"></script>
    <script type="text/javascript">
        var setting = {
            callback: {
                onDblClick: CloseTree, //双击事件
                //onClick: OpenTree, //单机事件
                onClick: CloseTree,
                onAsyncSuccess: zTreeOnAsyncSuccess, //成功事件
                onAsyncError: zTreeOnAsyncError//失败事件
            },
            view: {
                dblClickExpand: false//双击打开屏蔽
            }, data: {
                simpleData: {
                    enable: true
                }
            }
        };

        $(document).ready(function () {
            $.fn.zTree.init($("#tree"), setting, zNodes);
        });

        //选择事件
        function CloseTree(event, treeId, treeNode) {
            if (treeNode != "" && treeNode != null) {
                if (parseInt(treeNode.id) != 0) {
                    window.parent.CloseProductClass("Addr", "txtAddrID", "hidAddrID", treeNode.id, treeNode.name);
                }
            }
        }

        //异常处理
        function zTreeOnAsyncError(event, treeId, treeNode, XMLHttpRequest, textStatus, errorThrown) {
            //alert(errorThrown);
        };

        //成功方法
        function zTreeOnAsyncSuccess(event, treeId, treeNode, msg) {
            var treeObj = $.fn.zTree.getZTreeObj("tree");
            var nodes = treeObj.getNodes();
            treeObj.expandNode(nodes[0], true, false, false);
        };

        //加载树
        function reloadTree() {
            $.fn.zTree.init($("#tree"), setting);
        }

        //打开文件夹
        function OpenTree(e, treeId, treeNode) {
            var zTree = $.fn.zTree.getZTreeObj("tree");
            zTree.expandNode(treeNode);
        }

        function celar() {
            $("#txtAddrID", window.parent.document).val("");
            $("#hidAddrID", window.parent.document).val("");
            //$("#hid_Alert1", window.parent.document).val()
            window.parent.CloseDialogAddr();
        }

        function close1() {
            //$("#hid_Alert1", window.parent.document).val()
            window.parent.CloseDialogAddr();
         }
	</script>
    <style>
    .tablelink
    {
        background: #167dbd;
        border: 1px solid #056cad;
        color: #fff;
        border-radius: 4px;
        float: left;
        line-height: 22px;
        width: 49%;
        height: 100%;
        text-align: center;
        margin-right: 2px;
        cursor:pointer;
    }
    .cancel
    {
        background: #167dbd;
        border: 1px solid #056cad;
        color: #fff;
        float: left;
        border-radius: 4px;
        line-height: 22px;
        width: 49%;
        height: 100%;
        cursor:pointer;
        text-align: center;
    }
        .xubox_layer
        {
            height: 200px;
            left: 209px;
            margin-left: 0;
            position: absolute;
            top: 120px;
            width: 400px;
            z-index: 19891021;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="zTreeDemoBackground left">
            <ul id="tree" class="ztree">
            </ul>
        </div>
        <div style="position: fixed; left: 0px; right: 0px; bottom: 0px;">
            <input name="" type="button" class="tablelink" onclick="celar();" value="清除" />
            <input name="" type="button" class="cancel" onclick="close1();" value="关闭" />
        </div>
    </div>
    </form>
</body>
</html>

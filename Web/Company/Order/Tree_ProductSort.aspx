<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Tree_ProductSort.aspx.cs"
    Inherits="Company_Order_Tree_ProductSort" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../css/demo.css" rel="stylesheet" type="text/css" />
    <link href="../../css/zTreeStyle/zTreeStyle.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/jquery.ztree.core-3.5.js" type="text/javascript"></script>
</head>
<script type="text/javascript">
		<!--
    var setting = {
        callback: {
            onDblClick: CloseTree, //双击事件
            //onClick: OpenTree, //单机事件
            onClick:CloseTree,
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

    //    var zNodes = [
    //    { name: "商品类别", open: true,
    //        children: [
    //			{ name: "父节点1 - 展开", open: true,
    //			    children: [
    //					{ name: "父节点11 - 折叠",
    //					    children: [
    //							{ name: "叶子节点111" },
    //							{ name: "叶子节点112" },
    //							{ name: "叶子节点113" },
    //							{ name: "叶子节点114" }
    //						]
    //					}
    //				]
    //			},
    //			{ name: "父节点3 - 没有子节点", isParent: true }
    //            ]
    //    }
    //		];
    $(document).ready(function () {
        $.fn.zTree.init($("#tree"), setting, zNodes);
    });





    //获取树
    //    var setting = {
    //        async: {
    //            enable: true,
    //            chkStyle: "checkbox",
    //            url: "../provider/ProductTree.ashx"
    //        },
    //        callback: {
    //            onDblClick: CloseTree, //双击事件
    //            onClick: OpenTree, //单机事件
    //            onAsyncSuccess: zTreeOnAsyncSuccess, //成功事件
    //            onAsyncError: zTreeOnAsyncError//失败事件
    //        },
    //        view: {
    //            dblClickExpand: false//双击打开屏蔽
    //        }
    //    };


    //选择事件
    function CloseTree(event, treeId, treeNode) {

        if (treeNode != "" && treeNode != null) {
            var inde = window.parent.location.pathname.lastIndexOf("/");
            var str = window.parent.location.pathname.substring(parseInt(inde) + 1);
            if ($.trim(str) == "GoodsEdit.aspx") {
                window.parent.CloseProductClass2("txt_product_class", "hid_product_class", treeNode.id, treeNode.name, <%= CompId %>, "lblAttr", "divheight");
            } else {
                window.parent.CloseProductClass("txt_product_class", "hid_product_class", treeNode.id, treeNode.name);
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
           $("#txt_product_class",window.parent.document).val("");
           $("#hid_product_class",window.parent.document).val("");
           window.parent.CloseDialog("2");
        }
         
    function close1() {
        window.parent.CloseDialog("2");
    }

    //加载树
    //    $(document).ready(function () {
    //        reloadTree();
    //    });
    //    
		//-->
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
        text-align: center;
    }
</style>
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

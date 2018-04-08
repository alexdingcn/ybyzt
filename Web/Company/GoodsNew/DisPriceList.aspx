<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DisPriceList.aspx.cs" Inherits="Company_GoodsNew_DisPriceList" %>

<%@ Register Src="~/Company/UserControl/TopControl.ascx" TagPrefix="uc1" TagName="Top" %>
<%@ Register Src="~/Company/UserControl/TreeDisType.ascx" TagPrefix="uc2" TagName="DisType" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <title>代理商调价</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global-2.0.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../js/layer/layer.js" type="text/javascript"></script>
    <script src="../../js/layerCommon.js" type="text/javascript"></script>
    <script src="../js/js.js" type="text/javascript"></script>
    <script src="../js/disPrice.js?v=<%=ConfigCommon.GetNodeValue("Version.xml","Version")%>"
        type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <uc1:Top ID="top1" runat="server" ShowID="nav-3" />
    <input id="hid_Alert" type="hidden" />
    <input id="hidCompId" type="hidden" runat="server" class="hidCompId" />
    <div class="rightinfo">
        <!--面包屑 start-->
        <div class="place">
            <ul class="placeul">
                <li><a href="../jsc.aspx">我的桌面 </a></li>
                <li>&gt;</li>
                <li><a href="../GoodsNew/DisPriceList.aspx" id="btitle">代理商价格</a></li>
            </ul>
        </div>
        <!--面包屑 end-->
        <div class="disprice">
            <a href="javascript:;">代理商分类价格</a>|<a href="javascript:;">代理商区域价格</a>|<a href="javascript:;"
                class="cur">代理商价格</a></div>
        <!--代理商价格 start-->
        <div class="DealerPrice1">
            <!--筛选 start-->
            <ul class="dis-filter">
                <li class="li none" liindex="0">
                    <div class="name">
                        <i>一级分类</i></div>
                    <div class="nr">
                        <a href="javascript:;"></a>
                    </div>
                </li>
                <li class="li none" liindex="1">
                    <div class="name">
                        <i>二级分类</i></div>
                    <div class="nr">
                        <a href="javascript:;"></a>
                    </div>
                </li>
                <li class="li none" liindex="2">
                    <div class="name">
                        <i>三级分类</i></div>
                    <div class="nr">
                        <a href="javascript:;"></a>
                    </div>
                </li>
                <li class="li" liindex="3">
                    <div class="name">
                        <i>名称/编码</i></div>
                    <div class="seaBox">
                        <input name="txtDis" placeholder="不输入默认显示10条代理商数据" type="text" class="box txtDis"
                            value="" maxlength="20" />&nbsp;&nbsp;代理商分类<uc2:DisType runat="server" ID="txtDisType" />
                        <a href="javascript:;" class="btn btnSelect">查询</a>&nbsp;&nbsp;<a href="javascript:;"
                            style="width: 85px" class="btn btnqk">清空筛选条件</a></div>
                </li>
                <li class="li" liindex="4">
                    <div class="name">
                        <i>代理商</i></div>
                    <div class="nr">
                        <a href="javascript:;"></a>
                    </div>
                </li>
            </ul>
            <!--筛选 end-->
            <div class="blank10">
            </div>
            <!--按钮 start-->
            <div class="tools" style="overflow: inherit;">
                <ul class="toolbar left">
                    <li><span>
                        <img src="../images/t15.png"></span>保存</li>
                    <li class="none"><span>
                        <img src="../images/t14.png"></span>Excel导入</li>
                    <li class="none"><span>
                        <img src="../images/t19.png"></span>启用</li>
                    <li class="none"><span>
                        <img src="../images/t20.png"></span>禁用</li>
                </ul>
                <div class="doubt-nr left">
                    <i class="doubt-i"></i>
                    <div class="text">
                        <i class="dp-i"></i><i class="dp-arr"></i>
                        <p>
                            1、代理商分类价格和代理商区域价格不得同时生效</p>
                        <p>
                            2、代理商分类或者代理商区域不得上下级同时定价</p>
                        <p>
                            3、取价逻辑：先取代理商价格；未取到再取销商分类价格或者代理商区域价格；未取到再取商品定价</p>
                    </div>
                </div>
            </div>
            <div class="blank10">
            </div>
            <!--按钮 end-->
            <div class="tabLine">
                <table border="0" cellspacing="0" cellpadding="0">
                    <thead>
                        <tr>
                            <th class="t5">
                            </th>
                            <th class="">
                                商品名称/编码
                            </th>
                            <th class="t1">
                                规格属性
                            </th>
                            <th class="t5">
                                单位
                            </th>
                               <th class="t4">
                                原价格
                            </th>
                            <th class="t4">
                                价格
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <div class="addg">
                                    <a href="javascript:;" class="minus2"></a><a href="javascript:;" class="add2"></a>
                                </div>
                            </td>
                            <td class="c1">
                                <div class="search">
                                    <input name="txtGoods" type="text" class="box txtGoods">
                                    <!--[if !IE]>搜索弹窗 start<![endif]-->
                                    <div class="search-opt none">
                                        <div class="opt">
                                            <a href="javascript:;"><i class="opt-i"></i>选择商品</a></div>
                                    </div>
                                    <!--[if !IE]>搜索弹窗 end<![endif]-->
                                </div>
                                <a class="opt-i"></a>
                            </td>
                            <td>
                                <div class="tc">
                                </div>
                            </td>
                            <td>
                                <div class="tc">
                                </div>
                            </td>
                            <td>
                                <div class="search">
                                    <input name="txtDisPrice" type="text" class="box box2 txtDisPrice none" onfocus="InputFocus(this)"
                                        onkeyup="KeyInt2(this)" maxlength="9" value=""></div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div id="Pager_List" class="page none">
                <a href="javascript:;" class="tf">上页</a><a href="javascript:;" class="tf">下页</a>
            </div>
            <div id="divGoodsName" class="divGoodsName" runat="server" style="display: none">
            </div>
        </div>
        <!--代理商价格 end-->
    </div>
    </form>
</body>
</html>

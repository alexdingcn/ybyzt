﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class Admin_Systems_GoodsInfo : AdminPageBase
{
    public static int goodsId = 0;
    public static int goodsInfoId = 0;
    public static int compId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        object obje = Request["action"];
        if (obje != null)
        {
            if (obje.ToString() == "price")
            {
                string valuelist = Request["value"];//属性值列表
                Response.Write(GetProPrice(valuelist));
                Response.End();
            }
            if (obje.ToString() == "code")//商品编码
            {
                string valuelist = Request["value"];//属性值列表
                Response.Write(GetGoodsCode(valuelist));
                Response.End();
            }
        }
        if (!IsPostBack)
        {
            object obj2 = Request.QueryString["goodsId"];
            if (obj2 != null)
            {
                goodsId = Convert.ToInt32(obj2);
            }
            object obj3 = Request.QueryString["goodsInfoId"];
            if (obj3 != null)
            {
                goodsInfoId = Convert.ToInt32(obj3);
            }
            object obj4 = Request.QueryString["compId"];
            if (obj4 != null)
            {
                compId = Convert.ToInt32(obj4);
            }
            if (goodsId == 0 || goodsInfoId == 0 || compId == 0)
            {
                JScript.AlertAndRedirect("商品信息不存在", "GoodsInfoList.aspx");
                return;
            }
            List<Hi.Model.BD_ImageList> Imgl = new Hi.BLL.BD_ImageList().GetList("", "isnull(dr,0)=0 and compid=" + compId + " and goodsId=" + goodsId, "");
            if (Imgl.Count > 0)
            {
                ImgShow.Visible = false;
                rptImg.DataSource = Imgl;
                rptImg.DataBind();
            }
            else
            {
                ImgShow.Visible = true;
            }
            List<Hi.Model.BD_GoodsInfo> ll = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(dr,0)=0 and isenabled=1 and compid=" + compId + " and goodsid=" + goodsId, ""); //Common.GetGoodsPrice(comPid, goodsId);//商品价格列表
            if (ll.Count > 0)
            {
                Session["price"] = Common.FillDataTable(ll);
            }
            GetGoodsLabels(goodsId, compId);//商品标签
            Bind(goodsId, goodsInfoId, compId);
        }
    }
    /// <summary>
    /// 绑定产品信息
    /// </summary>
    /// <param name="id"></param>
    public void Bind(int id, int goodsInfoId, int compId)
    {
        Hi.Model.BD_Goods model = new Hi.BLL.BD_Goods().GetModel(id);
        if (model != null && model.dr == 0 && model.IsEnabled == 1)
        {
            Hi.Model.BD_GoodsInfo model2 = new Hi.BLL.BD_GoodsInfo().GetModel(goodsInfoId);
            if (model2 != null)
            {
                this.lblCode.InnerText = model2.BarCode.Trim();//商品编号
                this.lblInventory.InnerText = model2.Inventory.ToString();//库存
            }
            Hi.Model.BD_GoodsCategory model3 = new Hi.BLL.BD_GoodsCategory().GetModel(model.CategoryID);
            if (model3 != null)
            {
                this.lblCategory.InnerText = model3.CategoryName.Trim();//分类
            }
            GetZiDingYi(model);//自定义字段
            this.divTitle.InnerText = model.GoodsName.Trim();//biaot
            this.divMemo.InnerHtml = model.Title.Trim();//描述
            this.lblHideInfo1.InnerText = model.HideInfo1;
            this.lblHideInfo2.InnerText = model.HideInfo2;//描述
            this.lblUnit.InnerText = model.Unit.Trim();//计量单位
            if (model.Details == "")
            {
                this.DivShow.InnerHtml = "<p style=\"padding-top: 20px; line-height: 40px; padding-left: 20px\">暂无数据</p>";
            }
            else
            {
                this.DivShow.InnerHtml = model.Details;
            }
            if (!string.IsNullOrEmpty(model.Pic))//有图片
            {
                this.imgPic.Src = Common.GetPicURL(model.Pic, "resize400");
                this.imgPic.Attributes.Add("jqimg", Common.GetPicURL(model.Pic));

                this.imgPic2.Src = Common.GetPicURL(model.Pic, "resize400");
                this.imgPic2.Attributes.Add("bimg", Common.GetPicURL(model.Pic));
            }
            else
            {
                this.imgPic.Src = "../../images/havenopicmax.gif";//无图片
                this.imgPic2.Src = "../../images/havenopicmax.gif";//无图片
                this.imgPic2.Attributes.Add("bimg", "../../images/havenopicmax.gif");
                this.imgPic.Attributes.Add("jqimg", "../../images/havenopicmax.gif");
            }

            //add by hgh 设置商品是否首页显示
            this.labText.Text = model.IsFirstShow == true ? "是" : "否";
        }
        else
        {
            JScript.AlertMsg(this, "商品不存在或者已经被删除", "GoodsInfoList.aspx");
            return;
        }
        string html = string.Empty;
        List<Hi.Model.BD_GoodsAttrs> ll = new Hi.BLL.BD_GoodsAttrs().GetList("", "isnull(dr,0)=0 and compId=" + compId + " and goodsId=" + id.ToString(), "");
        if (ll.Count > 0)
        {
            int xy = 0;
            foreach (Hi.Model.BD_GoodsAttrs item in ll)
            {
                List<Hi.Model.BD_GoodsAttrsInfo> lll = new Hi.BLL.BD_GoodsAttrsInfo().GetList("", "isnull(dr,0)=0 and compId=" + compId + " and attrsId=" + item.ID, "");
                if (lll.Count > 0)
                {
                    html += "<div class=\"li\"><i class=\"bt2\" tip=\"" + item.AttrsName + "\">" + item.AttrsName + "：</i><div class=\"fun\">";
                    foreach (Hi.Model.BD_GoodsAttrsInfo item2 in lll)
                    {
                        Hi.Model.BD_GoodsInfo model4 = new Hi.BLL.BD_GoodsInfo().GetModel(goodsInfoId);
                        if (xy == 0)
                        {
                            html += "<a href=\"javascript:;\" class=\"" + (model4.Value1 == item2.AttrsInfoName.Trim() ? "hover" : "") + "\" tip=\"" + item2.AttrsInfoName + "\">" + item2.AttrsInfoName + "</a>";
                        }
                        else if (xy == 1)
                        {
                            html += "<a href=\"javascript:;\" class=\"" + (model4.Value2 == item2.AttrsInfoName.Trim() ? "hover" : "") + "\" tip=\"" + item2.AttrsInfoName + "\">" + item2.AttrsInfoName + "</a>";
                        }
                        else if (xy == 2)
                        {
                            html += "<a href=\"javascript:;\" class=\"" + (model4.Value3 == item2.AttrsInfoName.Trim() ? "hover" : "") + "\" tip=\"" + item2.AttrsInfoName + "\">" + item2.AttrsInfoName + "</a>";
                        }
                    }
                    html += "</div></div>";
                }
                xy++;
            }
        }
        litAttrVaue.Text = html;
    }
    /// <summary>
    /// //自定义字段
    /// </summary>
    private void GetZiDingYi(Hi.Model.BD_Goods model)
    {
        string html = string.Empty;
        List<Hi.Model.SYS_SysName> l = new Hi.BLL.SYS_SysName().GetList("", "isnull(dr,0)=0 and compId=" + model.CompID + " and name='商品自定义字段'", "");
        if (l.Count > 0)
        {
            if (!Util.IsEmpty(l[0].Value.Trim()))
            {
                string[] valuelist = l[0].Value.Trim().Split(',');
                if (valuelist.Length == 5)
                {
                    html = "<div class=\"li\"><i class=\"bt2\">" + valuelist[0] + "：</i><i >" + model.Value1.Trim() + "</i>&nbsp;&nbsp;&nbsp;&nbsp; <i class=\"bt2\">" + valuelist[1] + "：</i><i >" + model.Value2.Trim() + "</i><br /> <i class=\"bt2\">" + valuelist[2] + "：</i><i >" + model.Value3.Trim() + "</i>&nbsp;&nbsp;&nbsp;&nbsp; <i class=\"bt2\">" + valuelist[3] + "：</i><i >" + model.Value4.Trim() + "</i><br /><i class=\"bt2\">" + valuelist[4] + "：</i><i >" + model.Value5.Trim() + "</i></div>";
                }
                else if (valuelist.Length == 4)
                {
                    html = "<div class=\"li\"><i class=\"bt2\">" + valuelist[0] + "：</i><i >" + model.Value1.Trim() + "</i>&nbsp;&nbsp;&nbsp;&nbsp; <i class=\"bt2\">" + valuelist[1] + "：</i><i >" + model.Value2.Trim() + "</i><br /> <i class=\"bt2\">" + valuelist[2] + "：</i><i >" + model.Value3.Trim() + "</i>&nbsp;&nbsp;&nbsp;&nbsp; <i class=\"bt2\">" + valuelist[3] + "：</i><i >" + model.Value4.Trim() + "</i></div>";
                }
                else if (valuelist.Length == 3)
                {
                    html = "<div class=\"li\"><i class=\"bt2\">" + valuelist[0] + "：</i><i >" + model.Value1.Trim() + "</i>&nbsp;&nbsp;&nbsp;&nbsp; <i class=\"bt2\">" + valuelist[1] + "：</i><i >" + model.Value3.Trim() + "</i><br /> <i class=\"bt2\">" + valuelist[2] + "：</i><i >" + model.Value3.Trim() + "</i></div>";
                }
                else if (valuelist.Length == 2)
                {
                    html = "<div class=\"li\"><i class=\"bt2\">" + valuelist[0] + "：</i><i >" + model.Value1.Trim() + "</i>&nbsp;&nbsp;&nbsp;&nbsp; <i class=\"bt2\">" + valuelist[1] + "：</i><i >" + model.Value2.Trim() + "</i></div>";
                }
                else if (valuelist.Length == 1)
                {
                    html = "<div class=\"li\"><i class=\"bt2\">" + valuelist[0] + "：</i><i >" + model.Value1.Trim() + "</i></div>";
                }
            }
        }
        this.litZiDingYi.Text = html;
    }
    /// <summary>
    /// 获取价格
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetProPrice(string values)
    {
        string str = "暂无价格";
        DataTable dt = Session["price"] as DataTable;

        if (dt.Rows.Count != 0)
        {
            if (dt.Select("isnull(valueinfo,'')='" + values + "'").Length == 0)
            {
                str = "商品已删除";
            }
            else
            {
                if (dt.Select("isnull(valueinfo,'')='" + values + "'")[0]["isoffline"].ToString() == "0")
                {
                    str = "商品已下架";
                }
                else
                {
                    str = dt.Select("isnull(valueinfo,'')='" + values + "'")[0]["TinkerPrice"].ToString();
                }
            }
        }
        else
        {
            str = "0.00";
        }
        return str;
    }
    /// <summary>
    /// 商品标签
    /// </summary>
    /// <returns></returns>
    public void GetGoodsLabels(int keyId, int compId)
    {
        string[] list = { };
        StringBuilder str = new StringBuilder();
        List<Hi.Model.SYS_SysName> l = new Hi.BLL.SYS_SysName().GetList("", "isnull(dr,0)=0 and compid=" + compId + " and name='商品标签管理'", "");
        if (l.Count > 0)
        {
            foreach (Hi.Model.SYS_SysName item in l)
            {
                string labelname = item.Value;
                if (labelname.Trim() != "")
                {
                    list = labelname.Split(',');
                }
            }
            List<Hi.Model.BD_GoodsLabels> ll = new Hi.BLL.BD_GoodsLabels().GetList("", "isnull(dr,0)=0 and compid=" + compId + " and goodsId=" + keyId, "");
            for (int i = 0; i < list.Length; i++)
            {
                if (keyId != 0)
                {
                    if (ll.Count > 0)
                    {
                        int z = 0;//不相同的情况
                        foreach (Hi.Model.BD_GoodsLabels item in ll)
                        {
                            if (list[i] == item.LabelName)
                            {
                                str.Append("<label class=\"productLabelItem checked\"><input type=\"checkbox\" name=\"labelcheckbox\" value=\"" + list[i] + "\" style=\"display: none\" checked=\"checked\" />" + list[i] + "</label></ItemTemplate>");
                                break;
                            }
                            z++;
                        }
                        if (z == ll.Count)
                        {
                            str.Append("<label class=\"productLabelItem\"><input type=\"checkbox\" name=\"labelcheckbox\" value=\"" + list[i] + "\" style=\"display: none\" />" + list[i] + "</label></ItemTemplate>");
                        }
                    }
                }
                else
                {
                    str.Append("<label class=\"productLabelItem\"><input type=\"checkbox\" name=\"labelcheckbox\" value=\"" + list[i] + "\" style=\"display: none\" />" + list[i] + "</label></ItemTemplate>");
                }
            }

        }
        this.DivLabel.InnerHtml = str.ToString();
    }

    /// <summary>
    /// 获取商品编码
    /// </summary>
    /// <returns></returns>
    public string GetGoodsCode(string values)
    {
        Hi.Model.BD_Goods goodsModel = new Hi.BLL.BD_Goods().GetModel(goodsId);

        DataTable dt = Session["price"] as DataTable;

        if (dt.Rows.Count != 0)
        {
            if (dt.Select("isnull(valueinfo,'')='" + values + "'").Length != 0)
            {
                for (int i = 0; i < dt.Select("isnull(valueinfo,'')='" + values + "'").Length; i++)
                {
                    return dt.Select("isnull(valueinfo,'')='" + values + "'")[i]["barcode"].ToString() + "," + dt.Select("isnull(valueinfo,'')='" + values + "'")[i]["Inventory"].ToString();
                }
            }
        }
        return "";
    }

}
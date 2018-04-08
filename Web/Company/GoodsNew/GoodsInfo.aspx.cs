using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class Company_Goods_GoodsInfo : CompPageBase
{
    public static int goodsId = 0;
    public static int goodsInfoId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        object obje = Request["action"];
        if (obje != null)
        {
            if (obje.ToString() == "price")//获取价格
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
            object obj2 = Common.DesDecrypt(Request["goodsId"], Common.EncryptKey); 
            if (obj2 != null)
            {
                goodsId = Convert.ToInt32(obj2);
            }
            object obj3 = Request.QueryString["goodsInfoId"];
            if (obj3 != null)
            {
                goodsInfoId = Convert.ToInt32(obj3);
            }
            if (goodsId == 0)// || goodsInfoId == 0)
            {
                JScript.AlertMethod(this, "商品信息不存在！", JScript.IconOption.正确, "function (){ location.replace('" + ("GoodsInfoList.aspx") + "'); }");
                return;
            }
            List<Hi.Model.BD_ImageList> Imgl = new Hi.BLL.BD_ImageList().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and goodsId=" + goodsId, "");
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
            List<Hi.Model.BD_GoodsInfo> ll = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(dr,0)=0 and isenabled=1 and compid=" + this.CompID + " and goodsid=" + goodsId, ""); //Common.GetGoodsPrice(comPid, goodsId);//商品价格列表
            if (ll.Count > 0)
            {
                Session["price"] = Common.FillDataTable(ll);
            }
            GetGoodsLabels(goodsId);//商品标签
            Bind(goodsId);//, goodsInfoId);
            if (Request["rtype"] + "" == "1")
            {
                atitle.InnerText = "商品库存";
                atitle.HRef = "GoodsInfoList.aspx";
            }
        }
        DataBindLink();
    }

    /// <summary>
    /// 附件绑定
    /// </summary>
    public void DataBindLink()
    {
        string goodsId = Request.QueryString["goodsId"];
        if (!string.IsNullOrWhiteSpace(goodsId))
        {
          string gid=  Common.DesDecrypt(Request["goodsId"], Common.EncryptKey);
            Hi.Model.BD_Goods model = new Hi.BLL.BD_Goods().GetModel(Convert.ToInt32(gid));
            if (model != null && model.CompID == this.CompID)
            {
                if (!string.IsNullOrEmpty(model.registeredCertificate))
                {
                    //LinkButton linkFile = new LinkButton();
                    //linkFile.Click += new EventHandler(Download_Click);

                    //if (model.registeredCertificate.LastIndexOf("_") != -1)
                    //{
                    //    string text = model.registeredCertificate.Substring(0, model.registeredCertificate.LastIndexOf("_")) + Path.GetExtension(model.registeredCertificate);
                    //    if (text.Length < 15)
                    //        linkFile.Text = text;
                    //    else
                    //    {
                    //        linkFile.Text = text.Substring(0, 15) + "...";
                    //    }
                    //    linkFile.Attributes.Add("title", text);
                    //}
                    //else
                    //{
                    //    string text = model.registeredCertificate.Substring(0, model.registeredCertificate.LastIndexOf("-")) + Path.GetExtension(model.registeredCertificate);
                    //    if (text.Length < 15)
                    //        linkFile.Text = text;
                    //    else
                    //    {
                    //        linkFile.Text = text.Substring(0, 15) + "...";
                    //    }
                    //    linkFile.Attributes.Add("title", text);
                    //}
                    //linkFile.Style.Add("text-decoration", "underline");
                    //linkFile.Attributes.Add("fileName", model.registeredCertificate);
                    //HtmlGenericControl div = new HtmlGenericControl("div");
                    //div.Controls.Add(linkFile);
                   
                    //UpFileText2.Controls.Add(div);


                    string url = "../../UploadFile/" + model.registeredCertificate;
                    this.DivShow1.InnerHtml = "<img width=\"600\" src=\"" + url + "\"/>";
                }
            }

        }
    }





    /// <summary>
    /// 绑定产品信息
    /// </summary>
    /// <param name="id"></param>
    public void Bind(int id)//, int goodsInfoId)
    {
        Hi.Model.BD_Goods model = new Hi.BLL.BD_Goods().GetModel(id);
        if (model != null && model.dr == 0 && model.IsEnabled == 1)
        {
            if (goodsInfoId == 0)
            {
                List<Hi.Model.BD_GoodsInfo> l = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and goodsid=" + id, "");
                if (l.Count > 0)
                {
                    this.lblCode.InnerText = l[0].BarCode.Trim();//商品编号
                    this.lblInventory.InnerText = l[0].Inventory.ToString();//商品库存.
                    goodsInfoId = l[0].ID;
                }
            }
            else
            {
                Hi.Model.BD_GoodsInfo model2 = new Hi.BLL.BD_GoodsInfo().GetModel(goodsInfoId);
                if (model2 != null)
                {
                    this.lblCode.InnerText = model2.BarCode.Trim();//商品编号
                    this.lblInventory.InnerText = model2.Inventory.ToString();//商品库存.
                }
            }
            Hi.Model.SYS_GType model3 = new Hi.BLL.SYS_GType().GetModel(model.CategoryID);
            if (model3 != null)
            {
                this.lblCategory.InnerText = model3.TypeName.Trim();//分类
            }
            GetZiDingYi(model);//自定义字段
            // this.divTitle.InnerText = model.GoodsName.Trim();//biaot
            // this.divTitle.InnerHtml = model.GoodsName.Trim() + SelectGoods.ProType(zhek);//商品名称

            string zhek = Cuxiao(goodsInfoId.ToString());
            if (!Util.IsEmpty(zhek))
            {
                this.divTitle.InnerHtml = model.GoodsName + SelectGoods.ProType(zhek);//商品名称
            }
            else
            {
                this.divTitle.InnerHtml = model.GoodsName;
            }
            if (model.IsLS == 1)
            {
                this.isls.Visible = true;
                this.lbllsprice.InnerText = "￥" + model.LSPrice.ToString("0.00");
            }
            else
            {
                this.isls.Visible = false;
            }
            this.divMemo.InnerHtml = model.Title.Trim();//描述
            this.lblHideInfo1.InnerText = model.HideInfo1;
            this.lblHideInfo2.InnerText = model.HideInfo2;//描述
            this.lblUnit.InnerText = model.Unit.Trim();//计量单位
            // this.lblRecommend.InnerText = model.IsRecommended == 2 ? "推荐" : "否";//是否推荐
            if (model.IsRecommended == 2)
            {
                this.lblShow.InnerText = "显示";
                this.lblRecommend.InnerText = "推荐";
            }
            else if (model.IsRecommended == 1)
            {
                this.lblShow.InnerText = "显示";
                this.lblRecommend.InnerText = "不推荐";
            }
            else
            {
                this.lblShow.InnerText = "不显示";
                this.lblRecommend.InnerText = "不推荐";
            }
            if (model.Details == "")
            {
                this.DivShow.InnerHtml = "<p style=\"padding-top: 20px; line-height: 40px; padding-left: 20px\">暂无数据</p>";
            }
            else
            {
                this.DivShow.InnerHtml = model.Details;
            }
            if (model.Pic2.ToString().Trim() != "X" && model.Pic2.ToString().Trim() != "")//有图片
            {
                this.imgPic.Src = Common.GetWebConfigKey("ImgViewPath") + "GoodsImg/" + model.Pic2;
                this.imgPic2.Src = Common.GetWebConfigKey("ImgViewPath") + "GoodsImg/" + model.Pic2;
                if (model.Pic3.ToString().Trim() == "" || model.Pic3.ToString().Trim() == "X")
                {
                    this.imgPic.Attributes.Add("jqimg", Common.GetWebConfigKey("ImgViewPath") + "GoodsImg/" + model.Pic2);
                    this.imgPic2.Attributes.Add("bimg", Common.GetWebConfigKey("ImgViewPath") + "GoodsImg/" + model.Pic2);
                }
                else
                {
                    this.imgPic.Attributes.Add("jqimg", Common.GetWebConfigKey("ImgViewPath") + "GoodsImg/" + model.Pic3);
                    this.imgPic2.Attributes.Add("bimg", Common.GetWebConfigKey("ImgViewPath") + "GoodsImg/" + model.Pic3);
                }
            }
            else
            {
                this.imgPic.Src = "../../images/havenopicmax.gif";//无图片
                this.imgPic2.Src = "../../images/havenopicmax.gif";//无图片
                this.imgPic2.Attributes.Add("bimg", "../../images/havenopicmax.gif");
                this.imgPic.Attributes.Add("jqimg", "../../images/havenopicmax.gif");
            }
        }
        else
        {
            JScript.AlertMethod(this, "商品不存在或者已经被删除", JScript.IconOption.错误, "function(){location.href='GoodsInfoList.aspx';}");
            return;
        }
        string html = string.Empty;
        List<Hi.Model.BD_GoodsAttrs> ll = new Hi.BLL.BD_GoodsAttrs().GetList("", "isnull(dr,0)=0 and compId=" + this.CompID + " and goodsId=" + id.ToString(), "");
        if (ll.Count > 0)
        {
            int xy = 0;
            foreach (Hi.Model.BD_GoodsAttrs item in ll)
            {
                List<Hi.Model.BD_GoodsAttrsInfo> lll = new Hi.BLL.BD_GoodsAttrsInfo().GetList("", "isnull(dr,0)=0 and compId=" + this.CompID + " and attrsId=" + item.ID, "");
                if (lll.Count > 0)
                {
                    html += "<div class=\"li\"><i class=\"bt2\" tip=\"" + item.AttrsName + "\">" + item.AttrsName + "：</i><div class=\"fun\" style=\"display: inline;\">";
                    int count = 1;
                    foreach (Hi.Model.BD_GoodsAttrsInfo item2 in lll)
                    {
                        //Hi.Model.BD_GoodsInfo model4 = new Hi.BLL.BD_GoodsInfo().GetModel(goodsInfoId);
                        //if (xy == 0)
                        //{
                        //    html += "<a href=\"javascript:;\" class=\"" + (model4.Value1 == item2.AttrsInfoName.Trim() ? "hover" : "") + "\" tip=\"" + item2.AttrsInfoName + "\">" + item2.AttrsInfoName + "</a>";
                        //}
                        //else if (xy == 1)
                        //{
                        //    html += "<a href=\"javascript:;\" class=\"" + (model4.Value2 == item2.AttrsInfoName.Trim() ? "hover" : "") + "\" tip=\"" + item2.AttrsInfoName + "\">" + item2.AttrsInfoName + "</a>";
                        //}
                        //else if (xy == 2)
                        //{
                        //    html += "<a href=\"javascript:;\" class=\"" + (model4.Value3 == item2.AttrsInfoName.Trim() ? "hover" : "") + "\" tip=\"" + item2.AttrsInfoName + "\">" + item2.AttrsInfoName + "</a>";
                        //}
                        if (count == 1)
                        {
                            html += "<a href=\"javascript:;\" class=\"hover\" tip=\"" + item2.AttrsInfoName + "\">" + item2.AttrsInfoName + "</a>";
                        }
                        else
                        {
                            html += "<a href=\"javascript:;\" class=\"\" tip=\"" + item2.AttrsInfoName + "\">" + item2.AttrsInfoName + "</a>";
                        }
                        count++;
                    }
                    html += "</div></div>";
                }
                xy++;
            }
        }
        litAttrVaue.Text = html;
    }
    /// <summary>
    /// 促销
    /// </summary>
    /// <returns></returns>
    public string Cuxiao(string goodsInfoId)
    {
        string str = string.Empty;
        List<Hi.Model.BD_PromotionDetail> pd = new Hi.BLL.BD_PromotionDetail().GetList("", "isnull(dr,0)=0 and Goodinfoid=" + goodsInfoId, " id  desc");
        if (pd.Count > 0)
        {
            for (int i = 0; i < pd.Count; i++)
            {
                List<Hi.Model.BD_Promotion> pt = new Hi.BLL.BD_Promotion().GetList("", "isnull(IsEnabled,0)=1 and isnull(dr,0)=0 and ('" + DateTime.Now.Date + "' between  ProStartTime and ProEndTime ) and id=" + pd[i].ProID, "");
                if (pt.Count > 0)
                {
                    //if (pt[0].ProType.ToString() != "3")
                    //{
                    return pt[0].ID.ToString();
                    // }
                }
            }
        }
        return str;
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
        LoginModel model = HttpContext.Current.Session["UserModel"] as LoginModel;
        //if (model != null)
        //{
        //    if (this.CompID != model.CompID)
        //    {
        //        return "代理商可见";
        //    }
        //}
        //else
        //{
        //    return "代理商可见";
        //}
        Hi.Model.BD_Goods goodsModel = new Hi.BLL.BD_Goods().GetModel(goodsId);

        DataTable dt = Session["price"] as DataTable;

        string values2 = Common.NoHTML(values);

        if (dt.Rows.Count != 0)
        {
            if (dt.Select("isnull(valueinfo,'')='" + values2 + "'").Length == 0)
            {
                return "商品已删除";
            }
            else
            {
                if (dt.Select("isnull(valueinfo,'')='" + values2 + "'")[0]["Isoffline"].ToString() == "0")
                {
                    return "商品已下架";
                }
                string id = dt.Select("isnull(valueinfo,'')='" + values2 + "'")[0]["Id"].ToString();
                //List<Hi.Model.BD_PromotionDetail> pd = new Hi.BLL.BD_PromotionDetail().GetList("", "isnull(dr,0)=0 and Goodinfoid=" + id, "ProID desc");

                //if (pd.Count > 0)
                //{
                //    for (int i = 0; i < pd.Count; i++)
                //    {
                //        List<Hi.Model.BD_Promotion> pt = new Hi.BLL.BD_Promotion().GetList("", "isnull(IsEnabled,0)=1 and isnull(dr,0)=0 and ('" + DateTime.Now.Date + "' between  ProStartTime and ProEndTime ) and id=" + pd[i].ProID, "");
                //        if (pt.Count > 0)
                //        {
                //            if (pt[0].ProType.ToString() != "3")
                //            {
                //                str = pd[i].GoodsPrice.ToString();
                //                return str;
                //            }
                //            else
                //            {
                //                break;
                //            }
                //        }
                //    }
                //}
                //List<Hi.Model.BD_GoodsPrice> ll = new Hi.BLL.BD_GoodsPrice().GetList("", "DisID=" + model.DisID + " and isnull(dr,0)=0 and isenabled=1 and compid=" + this.CompID + " and goodsinfoid=" + id, "");
                //if (ll.Count > 0)
                //{
                //    str = ll[0].TinkerPrice.ToString();
                //}
                //else
                //{
                //    str = dt.Select("isnull(valueinfo,'')='" + values + "'")[0]["TinkerPrice"].ToString();
                //}
                str = BLL.Common.GetGoodsPrice(this.CompID, model.DisID, Convert.ToInt32(id)).ToString("0.00");
            }

        }
        else
        {
            if (goodsModel != null)
            {
                str = goodsModel.SalePrice.ToString("0.00");
            }
        }
        return str;
    }
    /// <summary>
    /// 商品标签
    /// </summary>
    /// <returns></returns>
    public void GetGoodsLabels(int keyId)
    {
        string[] list = { };
        StringBuilder str = new StringBuilder();
        List<Hi.Model.SYS_SysName> l = new Hi.BLL.SYS_SysName().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and name='商品标签管理'", "");
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
            List<Hi.Model.BD_GoodsLabels> ll = new Hi.BLL.BD_GoodsLabels().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and goodsId=" + keyId, "");
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class Distributor_GoodsInfo : DisPageBase
{
    public static int goodsId = 0;
    public static int goodsInfoId = 0;
    public string hideInfo1 = string.Empty;
    public string hideInfo2 = string.Empty;
    public string tiele_name = string.Empty;
    public int compID=0;
    protected void Page_Load(object sender, EventArgs e)
    {
        object obje = Request["action"];
        if (obje != null)
        {
            if (obje.ToString() == "price")
            {
                string valuelist = Request["value"];//属性值列表
                string goodsId = Request["goodsId"];
                Response.Write(GetProPrice(valuelist.Trim(), goodsId.Trim()));
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
            if (Request["CompId"] + "" != "")
            {
                compID = (Request["CompId"] + "").ToInt(0);
            }
            object obj3 = Request.QueryString["goodsInfoId"];
            if (obj3 != null)
            {
                goodsInfoId = Convert.ToInt32(obj3);
            }
            object obj4 = Request.QueryString["sc"];
            if (obj4 != null)
            {
                this.A1.InnerText = "收藏商品";
                this.A1.HRef = "GoodsList.aspx?sc=sc";
            }
            List<Hi.Model.BD_ImageList> Imgl = new Hi.BLL.BD_ImageList().GetList("", "isnull(dr,0)=0 and compid=" + compID + " and goodsId=" + goodsId, "");
            if (Imgl.Count > 0)
            {
                ImgShow.Visible = false;
                rptImg.DataSource = Imgl;
                rptImg.DataBind();
            }
            else
            {
                ImgShow.Visible = true;
                Hi.Model.BD_Goods model = new Hi.BLL.BD_Goods().GetModel(goodsId);
                if (model != null && !string.IsNullOrEmpty(model.Pic))
                {
                    this.imgPic2.Src = Common.GetPicURL(model.Pic, "resize400");
                    this.imgPic2.Attributes.Add("bimg", Common.GetPicURL(model.Pic));
                }
            }
            List<Hi.Model.BD_GoodsInfo> l = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(dr,0)=0 and isenabled=1 and compid=" + compID + " and goodsid=" + goodsId, ""); //Common.GetGoodsPrice(comPid, goodsId);//商品价格列表
            if (l.Count > 0)
            {
                Session["price"] = Common.FillDataTable(l);
                hidGoodsInfoId.Value = l[0].ID.ToString();
                if (goodsInfoId == 0)
                {
                    goodsInfoId = Convert.ToInt32(l[0].ID.ToString());
                }
            }
            else
            {
                JScript.AlertMethod(this, "商品不存在", JScript.IconOption.错误, "function(){location.href='index.aspx?Compid=" + compID + "'}");
                return;
            }
            Bind();
        }
        //DataBindLink();
    }

    //public void Download_Click(object sender, EventArgs e)
    //{
    //    LinkButton bt = sender as LinkButton;
    //    string fileName = bt.Attributes["fileName"];
    //    string filePath = Server.MapPath("../UploadFile/") + fileName;
    //    if (File.Exists(filePath))
    //    {
    //        FileInfo file = new FileInfo(filePath);
    //        Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8"); //解决中文乱码
    //        Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name.Substring(0, file.Name.LastIndexOf("_")) + Path.GetExtension(file.Name))); //解决中文文件名乱码    
    //        Response.AddHeader("Content-length", file.Length.ToString());
    //        Response.ContentType = "appliction/octet-stream";
    //        Response.WriteFile(file.FullName);
    //        Response.Flush();
    //        Response.End();
    //    }
    //    else
    //    {
    //        JScript.AlertMsgOne(this, "附件不存在！", JScript.IconOption.错误);
    //    }
    //}


    /// <summary>
    /// 附件绑定
    /// </summary>
    //public void DataBindLink()
    //{
    //    string goodsId = Request.QueryString["goodsId"];
    //    if (!string.IsNullOrWhiteSpace(goodsId))
    //    {
    //        //string gid = Common.DesDecrypt(Request["goodsId"], Common.EncryptKey);
    //        Hi.Model.BD_Goods model = new Hi.BLL.BD_Goods().GetModel(Convert.ToInt32(goodsId));
    //        if (model != null && model.CompID == this.CompID)
    //        {
    //            if (!string.IsNullOrEmpty(model.registeredCertificate))
    //            {
    //                LinkButton linkFile = new LinkButton();
    //                linkFile.Click += new EventHandler(Download_Click);

    //                if (model.registeredCertificate.LastIndexOf("_") != -1)
    //                {
    //                    string text = model.registeredCertificate.Substring(0, model.registeredCertificate.LastIndexOf("_")) + Path.GetExtension(model.registeredCertificate);
    //                    if (text.Length < 15)
    //                        linkFile.Text = text;
    //                    else
    //                    {
    //                        linkFile.Text = text.Substring(0, 15) + "...";
    //                    }
    //                    linkFile.Attributes.Add("title", text);
    //                }
    //                else
    //                {
    //                    string text = model.registeredCertificate.Substring(0, model.registeredCertificate.LastIndexOf("-")) + Path.GetExtension(model.registeredCertificate);
    //                    if (text.Length < 15)
    //                        linkFile.Text = text;
    //                    else
    //                    {
    //                        linkFile.Text = text.Substring(0, 15) + "...";
    //                    }
    //                    linkFile.Attributes.Add("title", text);
    //                }
    //                linkFile.Style.Add("text-decoration", "underline");
    //                linkFile.Attributes.Add("fileName", model.registeredCertificate);
    //                HtmlGenericControl div = new HtmlGenericControl("div");
    //                div.Controls.Add(linkFile);
    //                UpFileText2.Controls.Add(div);

    //            }
    //        }

    //    }
    //}






    /// <summary>
    /// 商品信息绑定
    /// </summary>
    public void Bind()
    {
        Hi.Model.BD_Goods model = new Hi.BLL.BD_Goods().GetModel(goodsId);
        if (model != null)
        {
            GetZiDingYi(model);
            string zhek = Cuxiao();
            if (!Util.IsEmpty(zhek))
            {
                this.lblGoodsName.InnerHtml = model.GoodsName + SelectGoods.ProType(zhek);//商品名称
                tiele_name = model.GoodsName;//前台title显示
                Hi.Model.BD_GoodsInfo godosInfomodel = new Hi.BLL.BD_GoodsInfo().GetModel(goodsInfoId);
                if (this.CompID.ToString() == model.CompID.ToString())
                {
                    this.YuanPrice.InnerHtml = "<s><i>原价：</i><b class=\"red\" id=\"B1\">￥" + decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(godosInfomodel.TinkerPrice).ToString())).ToString("#,##0.00") + "</b></s>";
                }
            }
            else
            {
                this.lblGoodsName.InnerHtml = model.GoodsName;
                tiele_name = model.GoodsName;//前台title显示
                this.YuanPrice.InnerHtml = "";
            }
            this.lblunit.InnerText = model.Unit;
            this.lblGoodsTitle.InnerText = model.Title;//商品卖点
            hideInfo1 = model.HideInfo1;
            hideInfo2 = model.HideInfo2;
            if (!Util.IsEmpty(model.Details.Trim()))
            {
                this.lblGoodsDetali.InnerHtml = model.Details;//商品描述
            }
            if (!Util.IsEmpty(model.registeredCertificate.Trim()))
            {
                string url = Common.GetWebConfigKey("OssImgPath") + "/UploadFile/" + model.registeredCertificate;
                this.lblGoodsDetali1.InnerHtml = "<img width=\"600\" src=\"" + url + "\"/>";
            }

            if (model != null && !string.IsNullOrEmpty(model.Pic))
            {
                this.imgPic.Src = Common.GetPicURL(model.Pic, "resize400");
                this.imgPic.Attributes.Add("jqimg", Common.GetPicURL(model.Pic));
            }

            //this.lblPrice.InnerText = "¥" + decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(model.SalePrice.ToString()).ToString())).ToString("#,##0.00"); ;//商品价格

            string html = string.Empty;
            List<Hi.Model.BD_GoodsAttrs> ll = new Hi.BLL.BD_GoodsAttrs().GetList("", "isnull(dr,0)=0 and compId=" + compID + " and goodsId=" + goodsId.ToString(), "");
            if (ll.Count > 0)
            {
                foreach (Hi.Model.BD_GoodsAttrs item in ll)
                {
                    int xy = 0;
                    List<Hi.Model.BD_GoodsAttrsInfo> lll = new Hi.BLL.BD_GoodsAttrsInfo().GetList("", "isnull(dr,0)=0 and compId=" + compID + " and attrsId=" + item.ID, "");
                    if (lll.Count > 0)
                    {
                        html += "<div class=\"li\"><div class=\"t\" tip=\"" + item.AttrsName + "\">" + item.AttrsName + "：</div><div class=\"n\">";
                        foreach (Hi.Model.BD_GoodsAttrsInfo item2 in lll)
                        {
                            if (goodsInfoId == 0)
                            {
                                html += "<a href=\"javascript:;\" class=\"" + (xy == 0 ? "hover" : "") + "\" tip=\"" + item2.AttrsInfoName + "\">" + item2.AttrsInfoName + "<i class=\"xz-icon\"></i></a>";
                            }
                            else
                            {
                                Hi.Model.BD_GoodsInfo model2 = new Hi.BLL.BD_GoodsInfo().GetModel(goodsInfoId);
                                if (model2 != null)
                                {
                                    html += "<a href=\"javascript:;\" class=\"" + (model2.Value1 == item2.AttrsInfoName || model2.Value2 == item2.AttrsInfoName || model2.Value3 == item2.AttrsInfoName ? "hover" : "") + "\" tip=\"" + item2.AttrsInfoName + "\">" + item2.AttrsInfoName + "<i class=\"xz-icon\"></i></a>";
                                }
                            }
                            xy++;
                        }
                        html += "</div></div>";
                    }

                }
            }
            litAttrVaue.InnerHtml = html;
            List<Hi.Model.BD_DisCollect> llll = new Hi.BLL.BD_DisCollect().GetList("", "isnull(dr,0)=0 and comPid=" + model.CompID + " and goodsId=" + goodsId + " and disId=" + this.DisID, "");
            if (llll.Count > 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "showsc", "<script>$(function(){$(\".btns .keep\").html('<i class=\"sc-icon\" style=\"background-position:0 -73px;\"></i>取消收藏\');})</script>");
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "showsc", "<script>$(function(){$(\".btns .keep\").html('<i class=\"sc-icon\" ></i>加入收藏\');})</script>");
            }
        }
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
                for (int i = 0; i < valuelist.Length; i++)
                {
                    string strvalue = string.Empty;
                    if (i == 0)
                    {
                        strvalue = model.Value1.Trim();
                    }
                    else if (i == 1)
                    {
                        strvalue = model.Value2.Trim();
                    }
                    else if (i == 2)
                    {
                        strvalue = model.Value3.Trim();
                    }
                    else if (i == 3)
                    {
                        strvalue = model.Value4.Trim();
                    }
                    else if (i == 4)
                    {
                        strvalue = model.Value5.Trim();
                    }
                    if (Util.IsEmpty(strvalue))
                    {
                        continue;
                    }
                    html += "<div class=\"li\"><div class=\"t\" style=\"margin-top: -7px;\">" + valuelist[i] + "：</div><div class=\"n zidingyi\">" + strvalue + "</div></div>";
                }

                // }
            }
        }
        this.litZiDingYi.InnerHtml = html;
    }
    /// <summary>
    /// 获取价格
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetProPrice(string values, string goodsId)
    {
        string str = "暂无价格";

        DataTable dt = Session["price"] as DataTable;

        string values2 = Common.NoHTML(values);
        if (dt.Rows.Count != 0)
        {
            if (dt.Select("isnull(valueinfo,'')='" + values2 + "'").Length != 0)
            {
                if (dt.Select("isnull(valueinfo,'')='" + values2 + "'")[0]["isoffline"].ToString() == "0")
                {
                    return "商品已下架";
                }
                string id = dt.Select("isnull(valueinfo,'')='" + values2 + "'")[0]["Id"].ToString();
                str = BLL.Common.GetGoodsPrice(dt.Select("isnull(valueinfo,'')='" + values2 + "'")[0]["CompID"].ToString().ToInt(0), this.DisID, Convert.ToInt32(id)).ToString("0.00") + "," + id;
            }
            else
            {
                str = "商品已删除";
            }
        }
        else
        {
            Hi.Model.BD_Goods goodsModel = new Hi.BLL.BD_Goods().GetModel(Convert.ToInt32(goodsId));
            if (goodsModel != null)
            {
                str = goodsModel.SalePrice.ToString("0.00");
            }
        }
        return str;
    }
    /// <summary>
    /// 获取价格
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetProPrice2(string values, string goodsId)
    {
        string str = "暂无价格";
        Hi.Model.BD_Goods goodsModel = new Hi.BLL.BD_Goods().GetModel(Convert.ToInt32(goodsId));
        List<Hi.Model.BD_GoodsInfo> lll = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(dr,0)=0 and isoffline=1 and compId=" + this.CompID + " and goodsid=" + goodsId, "");
        if (lll.Count != 0)
        {
            return BLL.Common.GetGoodsPrice(Convert.ToInt32(this.CompID), this.DisID, Convert.ToInt32(lll[0].ID)).ToString("0.00");
        }
        return goodsModel.SalePrice.ToString("0.00");
    }
    /// <summary>
    /// 促销
    /// </summary>
    /// <returns></returns>
    public string Cuxiao()
    {
        string str = string.Empty;
        List<Hi.Model.BD_PromotionDetail> pd = new Hi.BLL.BD_PromotionDetail().GetList("", "isnull(dr,0)=0 and Goodinfoid=" + goodsInfoId, "ProID  desc");
        if (pd.Count > 0)
        {
            for (int i = 0; i < pd.Count; i++)
            {
                List<Hi.Model.BD_Promotion> pt = new Hi.BLL.BD_Promotion().GetList("", "isnull(IsEnabled,0)=1 and isnull(dr,0)=0 and ('" + DateTime.Now.Date + "' between  ProStartTime and ProEndTime ) and id=" + pd[i].ProID, "");
                if (pt.Count > 0)
                {
                    //i//f (pt[0].ProType.ToString() != "3")
                    //{
                    return pt[0].ID.ToString();
                   // }
                }
            }
        }
        return str;
    }
    /// <summary>
    /// 获取商品编码
    /// </summary>
    /// <returns></returns>
    public string GetGoodsCode(string values)
    {
        Hi.Model.BD_Goods goodsModel = new Hi.BLL.BD_Goods().GetModel(goodsId);

        DataTable dt = Session["price"] as DataTable;
        string isInv = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", this.DisID); //是否取整
        if (dt.Rows.Count != 0)
        {
            if (dt.Select("isnull(valueinfo,'')='" + values + "'").Length != 0)
            {
                for (int i = 0; i < dt.Select("isnull(valueinfo,'')='" + values + "'").Length; i++)
                {
                    return dt.Select("isnull(valueinfo,'')='" + values + "'")[i]["barcode"].ToString() + "," + decimal.Parse(string.Format("{0:N2}", dt.Select("isnull(valueinfo,'')='" + values + "'")[i]["inventory"].ToString())).ToString(isInv);
                }
            }
        }
        return "";
    }
    public string GetTitle()
    {
        return tiele_name + "【详情、图片、加盟、购买】-医站通";
    }

}
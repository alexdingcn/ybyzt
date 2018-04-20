using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.Script.Serialization;

public partial class productsview : LoginPageBase
{
    public int goodsInfoId = 0;
    public int goodsId = 0;
    public int compId = 0;
    public int userCompId = 0;
    public static int type = 0;
    public int page = 1;
    public bool islogin = false;
    public string hideInfo1 = string.Empty;
    public string hideInfo2 = string.Empty;
    public string tiele_name = string.Empty;
    public string yc_CompT = "0";
    protected override void OnInit(EventArgs e)
    {
        List<Hi.Model.BD_Company> ComList = Header.ComList;
        compId = ComList[0].ID;
        if (string.IsNullOrWhiteSpace(Request["goodsId"]) && string.IsNullOrWhiteSpace(Request["goodsInfoId"]))
        {
            Response.Redirect("~/index.aspx", true);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(Request["goodsId"]))
            {
                goodsId = Convert.ToInt32(Request["goodsId"]);
            }
            if (!string.IsNullOrWhiteSpace(Request["goodsInfoId"]))
            {
                goodsInfoId = Convert.ToInt32(Request["goodsInfoId"]);
            }
            if (!string.IsNullOrWhiteSpace(Request["ycid"]))
            {
                this.hidycId.Value = Request["ycid"].ToString();

                Hi.Model.YZT_CMerchants cmModel = new Hi.BLL.YZT_CMerchants().GetModel(Request["ycid"].ToString().ToInt(0));
                if (cmModel != null){
                    this.hidycId.Attributes.Add("yc-tip", cmModel.CompID.ToString());

                    // 判断是否厂商自己申请自己
                    LoginModel uModel = null;
                    if (HttpContext.Current.Session["UserModel"] is LoginModel)
                    {
                        uModel = HttpContext.Current.Session["UserModel"] as LoginModel;
                        if (LoginModel.GetUserCompID(uModel.UserID.ToString()) == cmModel.CompID)
                        {
                            yc_CompT = "1";
                        }
                    }
                }
                this.hidCompId.Value = Request["comid"].ToString();
            }
            if (goodsInfoId != 0)
            {
                Hi.Model.BD_GoodsInfo info = new Hi.BLL.BD_GoodsInfo().GetModel(goodsInfoId);
                goodsId = info == null ? 0 : info.GoodsID;
            }
            if (!string.IsNullOrWhiteSpace(Request["page"]))
            {
                page = Convert.ToInt32(Request["page"]);
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        LoginModel logUser = HttpContext.Current.Session["UserModel"] as LoginModel;
        if (logUser != null)
        {
            islogin = true;
            if (logUser.CompID == Request["Comid"].ToInt(0))
            {
                type = logUser.TypeID;
                userCompId = logUser.CompID;
            }
        }
        else
        {
            type = 0;
            userCompId = 0;
        }
        object obje = Request["action"];
        if (obje != null)
        {
            if (obje.ToString() == "price")
            {
                if (!string.IsNullOrWhiteSpace(Request["Comid"]))
                {
                    string valuelist = Request["value"];//属性值列表
                    string compId = Request["compId"];//属性值列表
                    string goodsId = Request["goodsId"];
                    Response.Write(GetProPrice(valuelist.Trim(), compId.Trim(), goodsId.Trim()));
                    Response.End();
                }
            }
            else if (obje.ToString() == "price2")
            {
                string valuelist = Request["value"];//属性值列表
                string compId = Request["compId"];//属性值列表
                string goodsId = Request["goodsId2"];
                Response.Write(GetProPrice2(valuelist.Trim(), compId.Trim(), goodsId.Trim()));
                Response.End();
            }
            else if (obje.ToString() == "code")//商品编码
            {
                string valuelist = "";//属性值列表
                if (Request["value"] != null)
                    valuelist = Request["value"];
                Response.Write(GetGoodsCodes(valuelist));
                Response.End();
            }
            else if (obje.ToString() == "applyCooperation")
            {
                string ycCompID = Request["ycCompID"] + "";
                Response.Write(applyCooperation(ycCompID));
                Response.End();
            }
        }
        if (!IsPostBack)
        {
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
                Hi.Model.BD_Goods model = new Hi.BLL.BD_Goods().GetModel(goodsId);
                if (model != null && !string.IsNullOrEmpty(model.Pic))
                {
                    this.img1.Src = Common.GetPicURL(model.Pic, "resize400", compId.ToString());
                    this.img1.Attributes.Add("bimg", Common.GetPicURL(model.Pic, "", compId.ToString()));
                }
            }
            List<Hi.Model.BD_GoodsInfo> l = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(dr,0)=0 and isenabled=1 and compid=" + compId + " and goodsid=" + goodsId, ""); //Common.GetGoodsPrice(comPid, goodsId);//商品价格列表
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
                JScript.AlertMethod(this, "商品不存在", JScript.IconOption.错误, "function(){location.href='index.aspx?Compid=" + compId + "'}");
                return;
            }
            Bind();
            GetGoodsLabels();
        }
    }
    /// <summary>
    /// 商品信息绑定
    /// </summary>
    public void Bind()
    {
        LoginModel logmodel = HttpContext.Current.Session["UserModel"] as LoginModel;
        Hi.Model.BD_Goods model = new Hi.BLL.BD_Goods().GetModel(goodsId);
        if (model != null)
        {
            GetZiDingYi(model);
            mKeyword.Content = string.IsNullOrWhiteSpace(model.Title) ? model.GoodsName : model.Title;
            string zhek = Cuxiao();
            if (!Util.IsEmpty(zhek))
            {
                this.lblGoodsName.InnerHtml = model.GoodsName + SelectGoods.ProType(zhek);//商品名称
                tiele_name = model.GoodsName;//前台title显示
                if (hidGoodsInfoId.Value != null && hidGoodsInfoId.Value != "")
                {
                    Hi.Model.BD_GoodsInfo godosInfomodel = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(hidGoodsInfoId.Value));
                    //add by hgh 未登录，原价不可见
                    //if (logmodel != null)
                    //{
                  

                    if (Request["Comid"].ToString() == model.CompID.ToString()  )
                    {
                        this.YuanPrice.InnerHtml = "<s><i>原价：</i><b class=\"red\" id=\"B1\">" + decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(godosInfomodel.TinkerPrice).ToString())).ToString("#,##0.00") + "</b></s>";
                    }
                    else {
                        string sysNameWhere = string.Format(" CompID={0} and Name='是否店铺开放价格'", Request["Comid"].ToInt(0));
                        List<Hi.Model.SYS_SysName> Sysl = new Hi.BLL.SYS_SysName().GetList("", sysNameWhere, "");
                        if (Sysl.Count>0)
                        {
                            if (Sysl[0].Value == "1")
                            {
                                this.YuanPrice.InnerHtml = "<s><i>原价：</i><b class=\"red\" id=\"B1\">" + decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(godosInfomodel.TinkerPrice).ToString())).ToString("#,##0.00") + "</b></s>";
                            }
                        }
                    }
                    //}
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
                this.lblGoodsDetali.InnerHtml = model.Details.Replace("<pre>", "<p>").Replace("</pre>", "</p>");//商品描述
            }
            if (!Util.IsEmpty(model.registeredCertificate.Trim()))
            {
                string url = Common.GetWebConfigKey("ImgViewPath") + "UploadFile/" + model.registeredCertificate;
                this.lblGoodsDetali1.InnerHtml = "<img width=\"600\" src=\"" + url + "\"/>";
            }
            //this.lblPrice.InnerText = "¥" + decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(model.SalePrice.ToString()).ToString())).ToString("#,##0.00"); ;//商品价格
            if (model != null && !string.IsNullOrEmpty(model.Pic))
            {
                this.imgPic.Src = Common.GetPicURL(model.Pic, "resize400", compId.ToString());
                this.imgPic.Attributes.Add("jqimg", Common.GetPicURL(model.Pic, "", compId.ToString()));
            }
            else
            {
                imgPic.Src = "../images/Goods400x400.jpg";//无图片
                imgPic.Attributes.Add("jqimg", "../images/Goods400x400.jpg");
            }
            string html = string.Empty;
            List<Hi.Model.BD_GoodsAttrs> ll = new Hi.BLL.BD_GoodsAttrs().GetList("", "isnull(dr,0)=0 and compId=" + compId + " and goodsId=" + goodsId.ToString(), "");
            if (ll.Count > 0)
            {
                foreach (Hi.Model.BD_GoodsAttrs item in ll)
                {
                    int xy = 0;
                    List<Hi.Model.BD_GoodsAttrsInfo> lll = new Hi.BLL.BD_GoodsAttrsInfo().GetList("", "isnull(dr,0)=0 and compId=" + compId + " and attrsId=" + item.ID, "");
                    if (lll.Count > 0)
                    {
                        //add by hgh
                        if (item.AttrsName.Length > 4)
                            item.AttrsName = item.AttrsName.Substring(0, 4);
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
            if (logmodel != null)
            {
                List<Hi.Model.BD_DisCollect> llll = new Hi.BLL.BD_DisCollect().GetList("", "isnull(dr,0)=0 and comPid=" + model.CompID + " and goodsId=" + goodsId + " and disId=" + logmodel.DisID, "");
                if (llll.Count > 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "showsc", "<script>$(function(){$(\".btn .keep\").html('<i class=\"sc-icon\" style=\"background-position:0 -73px;\"></i>取消收藏\');})</script>");
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "showsc", "<script>$(function(){$(\".btn .keep\").html('<i class=\"sc-icon\" ></i>加入收藏\');})</script>");
                }
            }
        }
    }

    public string applyCooperation(string ycCompID)
    {
        Information.ResultMsg msg = new Information.ResultMsg();

        if (HttpContext.Current.Session["UserModel"] is LoginModel)
        {
           
            // 判断是否厂商自己申请自己
            LoginModel uModel = null;
            if (HttpContext.Current.Session["UserModel"] is LoginModel)
            {
                uModel = HttpContext.Current.Session["UserModel"] as LoginModel;
                if (LoginModel.GetUserCompID(uModel.UserID.ToString()) == ycCompID.ToInt(0))
                {
                    msg.Msg = "当前用户不是代理商，无法申请合作";
                }
                else
                {
                    msg.Result = true;
                }
            }
            else
            {
                msg.Result = true;
            }
        }
        else
        {
            msg.Code = "Login";
        }
        return (new JavaScriptSerializer().Serialize(msg));
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
    public string GetProPrice(string values, string comPid, string goodsId)
    {
        string sysNameWhere = string.Format(" CompID={0} and Name='是否店铺开放价格'", Request["Comid"].ToInt(0));
        List<Hi.Model.SYS_SysName> Sysl = new Hi.BLL.SYS_SysName().GetList("", sysNameWhere, "");

        string str = "暂无价格";
        LoginModel model = HttpContext.Current.Session["UserModel"] as LoginModel;
        Hi.Model.BD_Goods goodsModel = new Hi.BLL.BD_Goods().GetModel(Convert.ToInt32(goodsId));
        if (goodsModel == null) {
            return "商品已删除";
        }
        if (model == null|| model.CompID != comPid.ToInt(0))
        {
            if (Sysl.Count > 0)
            {
                if (Sysl[0].Value != "1")
                {
                    if (goodsModel.IsLS != 0)
                    {
                        return goodsModel.LSPrice.ToString("#0.00");
                    }
                    return "代理商可见";
                }
            }
            else
            {
                if (goodsModel.IsLS != 0)
                {
                    return goodsModel.LSPrice.ToString("#0.00");
                }
                return "代理商可见";
            }
        }
        string values2 = Common.NoHTML(values);
        DataTable dt = Session["price"] as DataTable;
            if (dt.Rows.Count != 0)
            {
                if (dt.Select("isnull(valueinfo,'')='" + values2 + "'").Length != 0)
                {
                    if (dt.Select("isnull(valueinfo,'')='" + values2 + "'")[0]["isoffline"].ToString() == "0")
                    {
                        return "商品已下架";
                    }

                    string id = dt.Select("isnull(valueinfo,'')='" + values2 + "'")[0]["Id"].ToString();
                    str = BLL.Common.GetGoodsPrice(comPid.ToInt(0), comPid.ToInt(0), id.ToInt(0)).ToString("0.00") + "," + id;
                }
                else
                {
                    str = "商品已删除";
                }
            }
            else
            {
                str = goodsModel.SalePrice.ToString("0.00");
            }
       
        //else
        //{
        //    if (goodsModel.IsLS == 0)
        //    {
        //        string sysNameWhere = string.Format(" CompID={0} and Name='是否店铺开放价格'", Request["Comid"].ToInt(0));
        //        List<Hi.Model.SYS_SysName> Sysl = new Hi.BLL.SYS_SysName().GetList("", sysNameWhere, "");
        //        if (Sysl.Count > 0)
        //        {
        //            if (Sysl[0].Value == "1")
        //            {
        //                return goodsModel.SalePrice.ToString("#0.00");
        //            }

        //        }
        //        return "代理商可见";
        //    }
        //    return goodsModel.LSPrice.ToString("#0.00");
        //}
        return str;
    }
    /// <summary>
    /// 获取价格
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetProPrice2(string values, string comPid, string goodsId)
    {
        string sysNameWhere = string.Format(" CompID={0} and Name='是否店铺开放价格'", Request["Comid"].ToInt(0));
        List<Hi.Model.SYS_SysName> Sysl = new Hi.BLL.SYS_SysName().GetList("", sysNameWhere, "");

        string str = "暂无价格";
        LoginModel model = HttpContext.Current.Session["UserModel"] as LoginModel;
        Hi.Model.BD_Goods goodsModel = new Hi.BLL.BD_Goods().GetModel(Convert.ToInt32(goodsId));
        if (model == null|| goodsModel.CompID != model.CompID)
        {
            if (Sysl.Count > 0)
            {
                if (Sysl[0].Value != "1")
                {
                    if (goodsModel.IsLS != 0)
                    {
                        return goodsModel.LSPrice.ToString("#0.00");
                    }
                    return "代理商可见";
                }
            }
            else
            {
                if (goodsModel.IsLS != 0)
                {
                    return goodsModel.LSPrice.ToString("#0.00");
                }
                return "代理商可见";
            }

        }
        List<Hi.Model.BD_GoodsInfo> lll = new Hi.BLL.BD_GoodsInfo().GetList("", "isnull(dr,0)=0 and isoffline=1 and compId=" + comPid + " and goodsid=" + goodsId, "");
        if (lll.Count != 0)
        {
            return BLL.Common.GetGoodsPrice(Convert.ToInt32(comPid), Convert.ToInt32(comPid), Convert.ToInt32(lll[0].ID)).ToString("0.00");
        }
        return "站务价格";
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
                  //  if (pt[0].ProType.ToString() != "3")
                  //  {
                    return pt[0].ID.ToString();
                  //  }
                }
            }
        }
        return str;
    }
    /// <summary>
    /// 获取商品编码
    /// </summary>
    /// <returns></returns>
    public string GetGoodsCodes(string values)
    {
       if(values==null)
            return "";

        Hi.Model.BD_Goods goodsModel = new Hi.BLL.BD_Goods().GetModel(goodsId);

        if (Session["price"] == null) return "";
        if (goodsModel == null) return "";
        DataTable dt = Session["price"] as DataTable;

        if (dt.Rows.Count != 0)
        {
            if (dt.Select("isnull(valueinfo,'')='" + values + "'").Length != 0)
            {
                for (int i = 0; i < dt.Select("isnull(valueinfo,'')='" + values + "'").Length; i++)
                {
                    return dt.Select("isnull(valueinfo,'')='" + values + "'")[i]["barcode"].ToString() + "," + dt.Select("isnull(valueinfo,'')='" + values + "'")[i]["inventory"].ToString();
                }
            }
        }
        return "";
    }

    public string GetTitle()
    {
        return tiele_name + "【详情、图片、加盟、购买】-医站通";
    }

    /// <summary>
    /// 商品标签
    /// </summary>
    /// <returns></returns>
    public void GetGoodsLabels()
    {
        string CompID = Request["comid"];//企业ID
        string goodsid = !string.IsNullOrEmpty(goodsId.ToString()) ? goodsId.ToString() : Request["goodsId"];//商品ID
        string[] list = { };

            List<Hi.Model.BD_GoodsLabels> ll = new Hi.BLL.BD_GoodsLabels().GetList("", "isnull(dr,0)=0 and compid=" + CompID + " and goodsId=" + goodsid, "");
            string html = "";
            foreach (Hi.Model.BD_GoodsLabels item in ll)
             {
             html+=  "<i class=\"t productLabelItem cur\">" + item.LabelName + "</i>";
              
              }
        this.DivLabel.InnerHtml = html;
    }

}
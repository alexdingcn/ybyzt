using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebReference;

public partial class Company_PmtManager_PromotionAdd : CompPageBase
{
    Hi.BLL.BD_Promotion ProBll = new Hi.BLL.BD_Promotion();
    Hi.BLL.BD_PromotionDetail ProDBll = new Hi.BLL.BD_PromotionDetail();
    //促销类型：0、特价促销  1、商品促销
    public string Type = string.Empty;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["Type"] + "" != "")
        {
            Type = Request["Type"] + "";
            this.protitle.Attributes.Add("href", "../PmtManager/PromotionList.aspx?type=" + Type);
            this.protitle.InnerText = "商品促销";
        }
        if (Request["action"] + "" == "GoodsInfo")
        {
            int Id = (Request["goodsInfoId"] + "").ToInt(0);
            decimal Price = (Request["Price"] + "").ToDecimal(0);

            UpdatePrice(Id, Price);

            return;
        }

        if (!IsPostBack)
        {
           ClientScript.RegisterStartupScript(this.GetType(), "TypeLoad", "<script>TypeLoad(" + KeyID + ");</script>");
            Bind();
        }
    }

    public void Bind()
    {
        //首次进行页面清除Session
        Session.Remove("GoodsPrice");
        if (KeyID != 0)
        {
            string Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", CompID);
            Hi.Model.BD_Promotion ProModel = ProBll.GetModel(KeyID);

            //促销公告
            List<Hi.Model.BD_CompNews> newsl = new Hi.BLL.BD_CompNews().GetList("", " PMID=" + KeyID + "and isnull(IsEnabled,0)=1", "");

            if (ProModel != null)
            {
                this.txtPromotiontitle.Value = ProModel.ProTitle;
                this.txtPromotionDate.Value = ProModel.ProStartTime == DateTime.MinValue ? "" : ProModel.ProStartTime.ToString("yyyy-MM-dd");
                this.txtPromotionDate1.Value = ProModel.ProEndTime == DateTime.MinValue ? "" : ProModel.ProEndTime.ToString("yyyy-MM-dd");
                this.txtProInfos.Value = ProModel.ProInfos;

                if (newsl != null && newsl.Count > 0)
                {
                    this.isOkComNews.Checked = true;
                    this.isNoComNews.Checked = false;
                }
                else
                {
                    this.isOkComNews.Checked = false;
                    this.isNoComNews.Checked = true;
                }

                //促销方式
                if (ProModel.ProType == 3)
                {
                    this.promotionType3.Checked = true;
                    this.txtSendFull.Value = string.Format("{0:N4}", ProModel.Discount.ToString("#,####" + Digits));
                }
                else
                {
                    this.promotionType4.Checked = true;
                    this.txtDiscount.Value = string.Format("{0:N4}", ProModel.Discount.ToString("#,####" + Digits));
                }
                //促销禁用
                if (ProModel.IsEnabled == 0)
                {
                    this.IsEnabled0.Checked = true;
                }
                else
                {
                    this.IsEnabled1.Checked = true;
                }

                decimal count = 0;
                //促销活动商品明细
                List<Hi.Model.BD_PromotionDetail> gl = ProDBll.GetList("", " CompId=" + this.CompID + " and ProID=" + KeyID, "");
                if (gl != null && gl.Count > 0)
                {
                    List<Hi.Model.BD_GoodsInfo> ll = new List<Hi.Model.BD_GoodsInfo>();

                    Hi.Model.BD_GoodsInfo model = null;
                    foreach (var item in gl)
                    {
                        model = new Hi.Model.BD_GoodsInfo();
                        model.CompID = this.CompID;
                        model.IsEnabled = true;
                        model.CreateUserID = this.UserID;
                        model.CreateDate = DateTime.Now;
                        model.ts = DateTime.Now;
                        model.modifyuser = this.UserID;
                        model.ID = item.GoodInfoID;
                        model.GoodsID = item.GoodsID;
                        model.SalePrice = GoodsPrice(item.GoodInfoID.ToString());
                        model.TinkerPrice = item.GoodsPrice;  //促销价
                        count = item.GoodsPrice;
                        ll.Add(model);
                    }
                    if (ProModel.ProType == 3)
                    {
                        //满送
                        this.txtSendNum.Value = string.Format("{0:N4}", count.ToString("#,####" + Digits));
                    }
                    Session["GoodsPrice"] = ll;
                    this.rpDtl.DataSource = ll;
                    this.rpDtl.DataBind();
                }
            }
        }
        else
        {
            this.rpDtl.DataSource = "";
            this.rpDtl.DataBind();
        }
    }

    /// <summary>
    /// 修改促销价格
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Price"></param>
    public void UpdatePrice(int Id, decimal Price)
    {
        List<Hi.Model.BD_GoodsInfo> ll = Session["GoodsPrice"] as List<Hi.Model.BD_GoodsInfo>;

        if (ll != null && ll.Count > 0)
        {
            foreach (Hi.Model.BD_GoodsInfo item in ll)
            {
                if (item.ID == Id)
                {
                    item.TinkerPrice = Price;
                }
            }
        }
        Session["GoodsPrice"] = ll as List<Hi.Model.BD_GoodsInfo>;
    }

    /// <summary>
    /// 选中的商品
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnGoodsInfo_Click(object sender, EventArgs e)
    {
        string protype = Request.Form["promotionType"];

        object obj = Session["GoodsPrice"];
        if (obj != null)
        {
            List<Hi.Model.BD_GoodsInfo> lll = obj as List<Hi.Model.BD_GoodsInfo>;
            this.rpDtl.DataSource = lll;
            this.rpDtl.DataBind();
        }
        ClientScript.RegisterStartupScript(this.GetType(), "Protype", "<script>Protype('" + protype + "');</script>");
    }

    /// <summary>
    /// 删除商品
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelGoods_Click(object sender, EventArgs e)
    {
        string id = this.hiddelgoodsid.Value;

        List<Hi.Model.BD_GoodsInfo> llll = new List<Hi.Model.BD_GoodsInfo>();
        object lll = Session["GoodsPrice"];
        if (lll != null)
        {
            List<Hi.Model.BD_GoodsInfo> ll = lll as List<Hi.Model.BD_GoodsInfo>;
            foreach (Hi.Model.BD_GoodsInfo item in ll)
            {
                if (item.ID.ToString() == id)
                {
                    continue;
                }
                llll.Add(item);
            }
        }
        string protype = Request.Form["promotionType"];
        ClientScript.RegisterStartupScript(this.GetType(), "Protype", "<script>Protype('" + protype + "');</script>");
        if (llll.Count == 0)
        {
            Session["GoodsPrice"] = null;
            this.rpDtl.DataSource = "";
            this.rpDtl.DataBind();
        }
        else
        {
            Session["GoodsPrice"] = llll;
            this.rpDtl.DataSource = llll;
            this.rpDtl.DataBind();
        }

       
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string str = string.Empty;
        string ProTitle = string.Empty;
        string ProType = string.Empty;
        string ProIsEnabled = string.Empty;
        string ProInfos = string.Empty;
        string PromotionDate = string.Empty;
        string PromotionEndDate = string.Empty;
        decimal disCount = 0;  //折扣率
        decimal SendNum = 0;  //满送数量
        int isComNew = 0;

        #region 判断值是否为空，取值
        //促销标题
        if (this.txtPromotiontitle.Value == "")
        {
            str += " -- 促销标题不能为空。</br>";
        }
        else
        {
            ProTitle =Common.NoHTML( this.txtPromotiontitle.Value.Trim());
        }
        //促销方式
        ProType = Request["promotionType"].ToString();
        //是否启用
        ProIsEnabled = Request["IsEnabled"].ToString();

        //促销开始日期
        if (this.txtPromotionDate.Value.Trim() == "")
        {
            str += " -- 促销开始日期不能为空。</br>";
        }
        else
        {
            PromotionDate = this.txtPromotionDate.Value.Trim();
        }
        //促销结束日期
        if (this.txtPromotionDate1.Value.Trim() == "")
        {
            str += " -- 促销结束日期不能为空。</br>";
        }
        else
        {
            PromotionEndDate = this.txtPromotionDate1.Value.Trim();
        }
        ProInfos =Common.NoHTML( this.txtProInfos.Value.Trim().ToString());

        List<Hi.Model.BD_GoodsInfo> gl = Session["GoodsPrice"] as List<Hi.Model.BD_GoodsInfo>;

        if (gl == null || gl.Count <= 0)
        {
            str += " -- 促销商品信息不能为空。</br>";
        }
        // 促销类型判断取值
        if (Type == "1")
        {
            if (ProType == "3")
            {
                //满送
                disCount = this.txtSendFull.Value.Trim().ToString().ToDecimal(0);
                SendNum = this.txtSendNum.Value.Trim().ToString().ToDecimal(0);

                if (disCount.ToString() == "" || SendNum.ToString() == "") {
                    if (disCount.ToString() == "")
                        str += " -- 满送订购数量不能为空。</br>";
                    else
                        str += " -- 获赠商品数量不能为空。</br>";
                }
            }
            else
            {
                //打折
                disCount = this.txtDiscount.Value.Trim().ToString().ToDecimal(0);
                if (disCount.ToString() != "")
                {
                    if ((int)disCount < 0 || (int)disCount > 100)
                    {
                        str += " -- 打折请输入0—100的数。</br>";
                    }
                }
                else
                {
                    str += " -- 打折不能为空。</br>";
                }
            }
        }

        if (str != "")
        {
            JScript.AlertMsgOne(this, str, JScript.IconOption.错误, 2500);
            ClientScript.RegisterStartupScript(this.GetType(), "Protype", "<script>Protype('" + ProType + "');</script>");
            return;
        }

        #endregion

        int ProId = 0;

        //是否发布促销公告
        isComNew = this.isOkComNews.Checked ? 1 : 0;

        string NewsContents = string.Empty;


        NewsContents = "<font size=\"3\" style=\"line-height:30px;\">";
        try
        {
            

            if (KeyID != 0)
            {
                #region 修改促销
                Hi.Model.BD_Promotion proModel = ProBll.GetModel(KeyID);

                if (proModel != null)
                {
                    proModel.Type = Type.ToInt(0);
                    proModel.ProType = ProType.ToInt(0);
                    proModel.ProTitle = ProTitle;
                    proModel.IsEnabled = ProIsEnabled.ToInt(0);
                    proModel.Discount = disCount;
                    proModel.ProStartTime = PromotionDate.ToDateTime();
                    proModel.ProEndTime = PromotionEndDate.ToDateTime();
                    proModel.ProInfos = ProInfos;
                    proModel.modifyuser = this.UserID;
                    proModel.ts = DateTime.Now.ToString();

                    List<Hi.Model.BD_PromotionDetail> l = new List<Hi.Model.BD_PromotionDetail>();

                    Hi.Model.BD_PromotionDetail proDmodel = null;
                    foreach (var item in gl)
                    {
                        proDmodel = new Hi.Model.BD_PromotionDetail();
                        proDmodel.CompID = this.CompID;
                        proDmodel.GoodInfoID = item.ID;
                        proDmodel.GoodsID = item.GoodsID;

                        if (ProType == "3")
                        {
                            //满送
                            proDmodel.SendGoodsinfoID = item.ID;
                            proDmodel.GoodsPrice = SendNum;
                        }
                        else
                        {
                            proDmodel.SendGoodsinfoID = 0;

                            if (ProType == "4")
                                //打折
                                proDmodel.GoodsPrice = (item.SalePrice * (disCount / 100));
                            else
                                proDmodel.GoodsPrice = item.TinkerPrice;
                        }

                        proDmodel.GoodsName = GoodsName(item.ID.ToString());
                        proDmodel.GoodsUnit = GoodsUnit(item.ID.ToString());
                        proDmodel.Goodsmemo = Goodsmemo(item.ID.ToString());
                        proDmodel.ts = DateTime.Now.ToString();
                        proDmodel.modifyuser = this.UserID;
                        proDmodel.CreateUserID = this.UserID;
                        proDmodel.CreateDate = DateTime.Now;

                        string memo = proDmodel.Goodsmemo == "" ? "" : proDmodel.Goodsmemo.Substring(0, proDmodel.Goodsmemo.Length - 1);

                        NewsContents += proDmodel.GoodsName + "  " + memo + " /" + proDmodel.GoodsUnit + "  原价 " + item.SalePrice.ToString("N") + "   促销价  " + proDmodel.GoodsPrice.ToString("N") + "！ </br>";

                        l.Add(proDmodel);
                    }
                    int Id = ProBll.ProUpdate(proModel, l);

                    NewsContents += "先到先得！";
                    ProInfos = ProInfos == "" ? "" : "（" + ProInfos + "）";
                    NewsContents += ProInfos;

                    NewsContents += "</br> 活动时间：" + PromotionDate.ToDateTime().ToString("yyyy-MM-dd") + "至" + PromotionEndDate.ToDateTime().ToString("yyyy-MM-dd");

                    NewsContents += "</font>";

                    //促销公告
                    List<Hi.Model.BD_CompNews> newsl = new Hi.BLL.BD_CompNews().GetList("", " PMID=" + KeyID, "");
                    Hi.Model.BD_CompNews news = null;
                    if (newsl != null && newsl.Count > 0)
                    {
                        foreach (Hi.Model.BD_CompNews item in newsl)
                        {
                            news = new Hi.Model.BD_CompNews();
                            news.ID = item.ID;
                            news.PmID = KeyID;
                            news.CompID = item.CompID;
                            news.CreateDate = DateTime.Now;
                            news.CreateUserID = UserID;
                            news.dr = item.dr;
                            news.ts = DateTime.Now;
                            if (ProIsEnabled.ToInt(0) == 1)
                            {
                                news.IsTop = 1;
                                news.IsEnabled = 0;
                                news.NewsType = 4;
                                news.ShowType = "2";
                            }
                            else
                            {
                                news.IsTop = item.IsTop;
                                news.IsEnabled = 0;
                                news.NewsType = item.NewsType;
                                news.ShowType = item.ShowType;
                            }
                            news.NewsTitle = ProTitle;
                            news.NewsContents = NewsContents;
                            news.modifyuser = UserID;
                        }
                    }
                    else
                    {
                        news = new Hi.Model.BD_CompNews();
                        news.PmID = KeyID;
                        news.CompID = CompID;
                        news.CreateDate = DateTime.Now;
                        news.CreateUserID = UserID;
                        news.dr = 0;
                        news.ts = DateTime.Now;
                        news.IsTop = 1;
                        news.IsEnabled = 1;
                        news.NewsType = 4;
                        news.ShowType = "2";
                        news.NewsTitle = ProTitle;
                        news.NewsContents = NewsContents;
                        news.modifyuser = UserID;
                    }

                    if (Id != 0)
                    {
                        if (isComNew == 1)
                        {
                            if (newsl != null && newsl.Count > 0)
                            {
                                //存在促销公告
                                new Hi.BLL.BD_CompNews().Update(news);
                            }
                            else
                            {
                                //不存在促销公告
                                new Hi.BLL.BD_CompNews().Add(news);
                            }
                        }
                        else
                        {
                            if (newsl != null && newsl.Count > 0)
                            {
                                news.IsEnabled = 0;
                                //存在促销公告
                                new Hi.BLL.BD_CompNews().Update(news);
                            }
                            else
                            {
                                news.IsEnabled = 0;
                                //不存在促销公告
                                new Hi.BLL.BD_CompNews().Add(news);
                            }
                        }

                        Response.Write("<script>window.location.href='../PmtManager/PromotionInfo.aspx?KeyId=" + Common.DesEncrypt(this.KeyID.ToString(), Common.EncryptKey) + "&type=" + Type + "';</script>");
                    }
                }
                #endregion
            }
            else
            {
                #region 新增促销
                Hi.Model.BD_Promotion proModel = new Hi.Model.BD_Promotion();

                proModel.Type = Type.ToInt(0);
                proModel.CompID = this.CompID;
                proModel.ProType = ProType.ToInt(0);
                proModel.ProTitle = ProTitle;
                proModel.IsEnabled = ProIsEnabled.ToInt(0);
                proModel.Discount = disCount;
                proModel.ProStartTime = PromotionDate.ToDateTime();
                proModel.ProEndTime = PromotionEndDate.ToDateTime();
                proModel.ProInfos = ProInfos;
                proModel.CreateUserID = this.UserID;
                proModel.CreateDate = DateTime.Now;
                proModel.modifyuser = this.UserID;
                proModel.ts = DateTime.Now.ToString();

                List<Hi.Model.BD_PromotionDetail> l = new List<Hi.Model.BD_PromotionDetail>();

                Hi.Model.BD_PromotionDetail proDmodel = null;
                foreach (var item in gl)
                {
                    proDmodel = new Hi.Model.BD_PromotionDetail();
                    proDmodel.CompID = this.CompID;
                    proDmodel.GoodInfoID = item.ID;
                    proDmodel.GoodsID = item.GoodsID;

                    if (ProType == "3")
                    {
                        //满送
                        proDmodel.SendGoodsinfoID = item.ID;
                        proDmodel.GoodsPrice = SendNum;
                    }
                    else
                    {
                        proDmodel.SendGoodsinfoID = 0;
                        if (ProType == "4")
                            //打折
                            proDmodel.GoodsPrice = (item.SalePrice * (disCount / 100));
                        else
                            proDmodel.GoodsPrice = item.TinkerPrice;
                    }

                    proDmodel.GoodsName = GoodsName(item.ID.ToString());
                    proDmodel.GoodsUnit = GoodsUnit(item.ID.ToString());
                    proDmodel.Goodsmemo = Goodsmemo(item.ID.ToString());
                    proDmodel.ts = DateTime.Now.ToString();
                    proDmodel.modifyuser = this.UserID;
                    proDmodel.CreateUserID = this.UserID;
                    proDmodel.CreateDate = DateTime.Now;

                    string memo = proDmodel.Goodsmemo == "" ? "" : proDmodel.Goodsmemo.Substring(0, proDmodel.Goodsmemo.Length - 1);

                    NewsContents += proDmodel.GoodsName + "  " + memo + " /" + proDmodel.GoodsUnit + "  原价 " + item.SalePrice.ToString("N") + "   促销价  " + proDmodel.GoodsPrice.ToString("N") + "！ </br>";
                    l.Add(proDmodel);
                }

                NewsContents += "先到先得！";
                ProInfos = ProInfos == "" ? "" : "（" + ProInfos + "）";
                NewsContents += ProInfos;

                NewsContents += " </br> 活动时间：" + PromotionDate.ToDateTime().ToString("yyyy-MM-dd") + "至" + PromotionEndDate.ToDateTime().ToString("yyyy-MM-dd");

                NewsContents += "</font>";

                Hi.Model.BD_CompNews news = new Hi.Model.BD_CompNews();

                news.CompID = CompID;
                news.CreateDate = DateTime.Now;
                news.CreateUserID = UserID;
                news.dr = 0;
                news.ts = DateTime.Now;
                news.IsTop = 1;
                news.IsEnabled = 1;
                news.NewsType = 4;
                news.ShowType = "2";
                news.NewsTitle = ProTitle;
                news.NewsContents = NewsContents;
                news.modifyuser = UserID;

                int Id = ProBll.AddPro(proModel, l);

                if (Id != 0)
                {
                    news.PmID = Id;

                    if (isComNew == 1)
                    {
                        int count = new Hi.BLL.BD_CompNews().Add(news);

                        WebReference.AppService app = new AppService();
                        try
                        {
                            app.MsgPush(count.ToString(), "1");
                        }
                        catch
                        {
                            app.Abort();
                        }
                    }
                    Response.Write("<script>window.location.href='../PmtManager/PromotionInfo.aspx?KeyId=" + Common.DesEncrypt(Id.ToString(),Common.EncryptKey) + "&type=" + Type + "';</script>");
                }

                #endregion
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// 得到商品名称
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GoodsName(string id)
    {
        string name = string.Empty;
        Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(id));
        if (model != null)
        {
            Hi.Model.BD_Goods model2 = new Hi.BLL.BD_Goods().GetModel(model.GoodsID);
            if (model2 != null)
            {
                name = model2.GoodsName;
            }
        }
        return name;
    }

    /// <summary>
    /// 得到商品单位
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GoodsUnit(string id)
    {
        string name = string.Empty;
        Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(id));
        if (model != null)
        {
            Hi.Model.BD_Goods model2 = new Hi.BLL.BD_Goods().GetModel(model.GoodsID);
            if (model2 != null)
            {
                name = model2.Unit;
            }
        }
        return name;
    }

    /// <summary>
    /// 得到商品描述
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string Goodsmemo(string id)
    {
        string name = string.Empty;
        Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(id));
        if (model != null)
        {
            if (model.ValueInfo != "")
            {
                name = model.ValueInfo;
            }
            else
            {
                Hi.Model.BD_Goods model2 = new Hi.BLL.BD_Goods().GetModel(model.GoodsID);
                if (model2 != null)
                {
                    name = model2.memo;
                }
            }
        }
        return name;
    }

    /// <summary>
    /// 商品基本价格
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public decimal GoodsPrice(string id)
    {
        decimal name = 0;
        Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(id));
        if (model != null)
        {
            name = model.TinkerPrice;
        }
        return name;
    }
}
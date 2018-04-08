using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_PmtManager_PromotionInfo : CompPageBase
{
    Hi.BLL.BD_Promotion ProBll = new Hi.BLL.BD_Promotion();
    Hi.BLL.BD_PromotionDetail ProDBll = new Hi.BLL.BD_PromotionDetail();
    //促销类型：0、特价促销  1、商品促销
    public string Type = string.Empty;
    public string pro = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Type"] + "" != "")
        {
            Type = Request.QueryString["Type"] + "";
            this.protitle.Attributes.Add("href", "../PmtManager/PromotionList.aspx?type=" + Type);
            if (Type == "0")
                this.protitle.InnerText = "特价促销";
            else
                this.protitle.InnerText = "商品促销";
        }

        if (!IsPostBack)
        {
            Bind();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        if (KeyID != 0)
        {
            string Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", CompID);
            Hi.Model.BD_Promotion ProModel = ProBll.GetModel(KeyID);

            if (ProModel != null)
            {
                this.lblProTitle.InnerText = ProModel.ProTitle;
                this.lblProType.InnerText = Common.GetProType(ProModel.ProType.ToString());
                pro = ProModel.ProType.ToString();

                this.lblProIsEnabled.InnerText = Common.GetIsEnabled(ProModel.IsEnabled.ToString());
                this.lblProStartDate.InnerText = ProModel.ProStartTime == DateTime.MinValue ? "" : ProModel.ProStartTime.ToString("yyyy-MM-dd");
                this.lblProEndDate.InnerText = ProModel.ProEndTime == DateTime.MinValue ? "" : ProModel.ProEndTime.ToString("yyyy-MM-dd");
                this.lblProInfos.InnerText = ProModel.ProInfos;

                if (ProModel.ProStartTime <= DateTime.Now && ProModel.ProEndTime.AddDays(1) > DateTime.Now)
                {
                    //促销活动中
                    //判断是否禁用
                    if (ProModel.IsEnabled == 0)
                    {
                        //未禁用
                        this.liEdit.Visible = true;  //编辑
                        this.liDel.Visible = true;  //删除
                        this.liNo.Visible = false; //禁用
                        this.liOk.Visible = true;  //启用
                    }
                    else
                    {
                        //已禁用
                        this.liEdit.Visible = false;  //编辑
                        this.liDel.Visible = false;   //删除
                        this.liNo.Visible = true;   //禁用
                        this.liOk.Visible = false;  //启用
                    }
                }
                else if (ProModel.ProStartTime > DateTime.Now)
                {
                    //判断是否禁用
                    if (ProModel.IsEnabled == 0)
                    {
                        //未禁用
                        this.liEdit.Visible = true;  //编辑
                        this.liDel.Visible = true;  //删除
                        this.liNo.Visible = false; //禁用
                        this.liOk.Visible = true;  //启用
                    }
                    else
                    {
                        //已禁用
                        this.liEdit.Visible = false;  //编辑
                        this.liDel.Visible = false;  //删除
                        this.liNo.Visible = true;   //禁用
                        this.liOk.Visible = false;  //启用
                    }
                }
                else
                {
                    this.liEdit.Visible = true;  //编辑
                    this.liDel.Visible = true;  //删除
                    this.liNo.Visible = false; //禁用
                    this.liOk.Visible = false;  //启用
                }

                //促销活动商品明细
                List<Hi.Model.BD_PromotionDetail> gl = ProDBll.GetList("", " CompId=" + this.CompID + " and ProID=" + KeyID, "");
                if (gl != null && gl.Count > 0)
                {
                    if (ProModel.ProType == 3)
                    {
                        lblDisCount.InnerText = "订购数量每满（" + (int)ProModel.Discount + "），获赠品（" + gl[0].GoodsPrice.ToString("#,####" + Digits) + "）个";
                    }
                    else if (ProModel.ProType == 4)
                        lblDisCount.InnerText = "在原订货价基础上再打折（" + (int)ProModel.Discount + "）%";

                    this.rpDtl.DataSource = gl;
                    this.rpDtl.DataBind();
                }
            }
        }
    }

    /// <summary>
    /// 禁用
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNo_Click(object sender, EventArgs e)
    {
        if (KeyID != 0)
        {
            Hi.Model.BD_Promotion proModel = ProBll.GetModel(KeyID);

            //促销公告
            List<Hi.Model.BD_CompNews> newsl = new Hi.BLL.BD_CompNews().GetList("", " PMID=" + KeyID, "");

            if (proModel != null)
            {
                proModel.IsEnabled = 0;

                if (ProBll.Update(proModel))
                {
                    Hi.Model.BD_CompNews newsModel = new Hi.Model.BD_CompNews();
                    if (newsl != null && newsl.Count > 0)
                    {
                        foreach (Hi.Model.BD_CompNews item in newsl)
                        {
                            item.IsEnabled = 0;
                            newsModel = item;
                        }
                        new Hi.BLL.BD_CompNews().Update(newsModel);
                    }

                    Response.Write("<script>window.location.href='../PmtManager/PromotionInfo.aspx?KeyId=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type=" + Request["type"] + "';</script>");
                }
            }
        }
    }


    /// <summary>
    /// 启用
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (KeyID != 0)
        {
            Hi.Model.BD_Promotion proModel = ProBll.GetModel(KeyID);
            //促销公告
            List<Hi.Model.BD_CompNews> newsl = new Hi.BLL.BD_CompNews().GetList("", " PMID=" + KeyID, "");

            if (proModel != null)
            {
                proModel.IsEnabled = 1;
                if (ProBll.Update(proModel))
                {
                    if (newsl != null && newsl.Count > 0)
                    {
                        Hi.Model.BD_CompNews newsModel = new Hi.Model.BD_CompNews();
                        foreach (Hi.Model.BD_CompNews item in newsl)
                        {
                            item.IsEnabled = 0;
                            newsModel = item;
                        }
                        new Hi.BLL.BD_CompNews().Update(newsModel);
                    }

                    Response.Write("<script>window.location.href='../PmtManager/PromotionInfo.aspx?KeyId=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "&type=" + Request["type"] + "';</script>");
                }
            }
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDel_Click(object sender, EventArgs e)
    {
        if (KeyID != 0)
        {
            Hi.Model.BD_Promotion proModel = ProBll.GetModel(KeyID);
            //促销公告
            List<Hi.Model.BD_CompNews> newsl = new Hi.BLL.BD_CompNews().GetList("", " PMID=" + KeyID, "");

            if (proModel != null)
            {
                proModel.IsEnabled = 1;
                proModel.dr = 1;
                if (ProBll.Update(proModel))
                {
                    if (newsl != null && newsl.Count > 0)
                    {
                        Hi.Model.BD_CompNews newsModel = new Hi.Model.BD_CompNews();
                        foreach (Hi.Model.BD_CompNews item in newsl)
                        {
                            item.IsEnabled = 0;
                            item.dr = 1;
                            newsModel = item;
                        }
                        new Hi.BLL.BD_CompNews().Update(newsModel);
                    }
                    Response.Write("<script>window.location.href='../PmtManager/PromotionList.aspx?type=" + Request["type"] + "';</script>");
                }
            }
        }
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
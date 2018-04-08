using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_PmtManager_PromotionInfo2 : CompPageBase
{
    //促销类型：0、特价促销  1、商品促销 2、订单促销
    public string Type = string.Empty;
    public string pro = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Type"] + "" != "")
        {
            Type = Request.QueryString["Type"] + "";
            this.protitle.Attributes.Add("href", "../PmtManager/PromotionList.aspx?type=" + Type);
            if (Type == "2")
            {
                this.protitle.InnerText = "订单促销";
            }
            else
            {
                this.protitle.InnerText = "促销明细";
            }
        }

        if (!IsPostBack)
        {
            Bind();
        }
    }
    /// <summary>
    /// 数据绑定
    /// </summary>
    private void Bind()
    {
        if (KeyID != 0)
        {
            Hi.Model.BD_Promotion ProModel = new Hi.BLL.BD_Promotion().GetModel(KeyID);
            if (ProModel != null)
            {
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
                List<Hi.Model.BD_PromotionDetail2> ll = new Hi.BLL.BD_PromotionDetail2().GetList("", "isnull(dr,0)=0 and proId=" + ProModel.ID + " and compId=" + this.CompID, "");
                if (ll.Count > 1)
                {
                    this.lblProIsJieTi.InnerText = "是";
                }
                else
                {
                    this.lblProIsJieTi.InnerText = "否";
                }
                string html = "";
                if (ProModel.ProType == 5)
                {
                    foreach (Hi.Model.BD_PromotionDetail2 item in ll)
                    {
                        html += "<label>订单金额满￥<input type=\"text\" onkeyup='KeyInt2(this)' id=\"txtPrice1\" class=\"send txtPrice\" style=\"width: 50px;\" name=\"txtPrice\" value=\"" + item.OrderPrice.ToString("f2") + "\" />，立减￥<input type=\"text\" id=\"txtSendFull1\" onkeyup='KeyInt2(this)' class=\"send txtSendFull\" name=\"txtSendFull\" style=\"width: 50px;\" value=\"" + item.Discount.ToString("f2") + "\"/></label><br>";
                    }
                }
                else if (ProModel.ProType == 6)
                {

                    foreach (Hi.Model.BD_PromotionDetail2 item in ll)
                    {
                        html += "<label>订单金额满￥<input type=\"text\" onkeyup='KeyInt2(this)' id=\"txtPrices1\" class=\"send txtPrices\" style=\"width: 50px;\" name=\"txtPrices\"  value=\"" + item.OrderPrice.ToString("f2") + "\"/>，打折（<input type=\"text\" id=\"txtDiscount1\" onkeyup='KeyInt2(this)' class=\"send txtDiscount\" name=\"txtDiscount\" style=\"width: 20px;\"  value=\"" + Convert.ToInt32(item.Discount) + "\"/>）%</label><br>";
                    }
                }
                this.SendFull.InnerHtml = html;
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
            Hi.Model.BD_Promotion proModel = new Hi.BLL.BD_Promotion().GetModel(KeyID);
            //促销公告
            List<Hi.Model.BD_CompNews> newsl = new Hi.BLL.BD_CompNews().GetList("", " PMID=" + KeyID, "");

            if (proModel != null)
            {
                proModel.IsEnabled = 1;
                proModel.dr = 1;
                if (new Hi.BLL.BD_Promotion().Update(proModel))
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
                    Response.Write("<script>window.location.href='PromotionList.aspx?type=" + Request["type"] + "';</script>");
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
            Hi.Model.BD_Promotion proModel = new Hi.BLL.BD_Promotion().GetModel(KeyID);
            //促销公告
            List<Hi.Model.BD_CompNews> newsl = new Hi.BLL.BD_CompNews().GetList("", " PMID=" + KeyID, "");

            if (proModel != null)
            {
                proModel.IsEnabled = 1;
                if (new Hi.BLL.BD_Promotion().Update(proModel))
                {
                    if (newsl != null && newsl.Count > 0)
                    {
                        Hi.Model.BD_CompNews newsModel = new Hi.Model.BD_CompNews();
                        foreach (Hi.Model.BD_CompNews item in newsl)
                        {
                            item.IsEnabled = 1;
                            newsModel = item;
                        }
                        new Hi.BLL.BD_CompNews().Update(newsModel);
                    }

                    Response.Write("<script>window.location.href='PromotionInfo2.aspx?KeyId=" + Common.DesEncrypt(this.KeyID.ToString(), Common.EncryptKey) + "&type=" + Request["type"] + "';</script>");
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
            Hi.Model.BD_Promotion proModel = new Hi.BLL.BD_Promotion().GetModel(KeyID);

            //促销公告
            List<Hi.Model.BD_CompNews> newsl = new Hi.BLL.BD_CompNews().GetList("", " PMID=" + KeyID, "");

            if (proModel != null)
            {
                proModel.IsEnabled = 0;

                if (new Hi.BLL.BD_Promotion().Update(proModel))
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

                    Response.Write("<script>window.location.href='PromotionInfo2.aspx?KeyId=" + Common.DesEncrypt(this.KeyID.ToString(), Common.EncryptKey) + "&type=" + Request["type"] + "';</script>");
                }
            }
        }
    }
}
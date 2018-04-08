using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Company_PmtManager_PromotionAdd2 : CompPageBase
{
    //促销类型：0、特价促销  1、商品促销 2、订单促销
    public string Type = string.Empty;
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
                this.protitle.InnerText = "新增促销";
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
            //促销公告
            List<Hi.Model.BD_CompNews> newsl = new Hi.BLL.BD_CompNews().GetList("", " PMID=" + KeyID + "and isnull(IsEnabled,0)=1", "");

            if (ProModel != null)
            {
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
                this.txtPromotionDate.Value = ProModel.ProStartTime == DateTime.MinValue ? "" : ProModel.ProStartTime.ToString("yyyy-MM-dd");
                this.txtPromotionDate1.Value = ProModel.ProEndTime == DateTime.MinValue ? "" : ProModel.ProEndTime.ToString("yyyy-MM-dd");
                this.txtProInfos.Value = ProModel.ProInfos;
                //促销禁用
                if (ProModel.IsEnabled == 0)
                {
                    this.IsEnabled0.Checked = true;
                    this.IsEnabled1.Checked = false;
                }
                else
                {
                    this.IsEnabled0.Checked = false;
                    this.IsEnabled1.Checked = true;
                }
                List<Hi.Model.BD_PromotionDetail2> ll = new Hi.BLL.BD_PromotionDetail2().GetList("", "isnull(dr,0)=0 and proId=" + KeyID + " and compId=" + this.CompID, "");
                if (ll.Count > 1)
                {
                    this.Radio1.Checked = true;
                    this.Radio2.Checked = false;
                }
                else
                {
                    this.Radio2.Checked = true;
                    this.Radio1.Checked = false;
                }
                string html = "";
                if (ProModel.ProType == 5)
                {
                    this.promotionType3.Checked = true;
                    this.promotionType4.Checked = false;
                    int z = 0;
                    foreach (Hi.Model.BD_PromotionDetail2 item in ll)
                    {
                        z++;

                        html += "<label>订单金额满￥<input type=\"text\" onkeyup='KeyInt2(this)' id=\"txtPrice" + z + "\" class=\"send txtPrice\" style=\"width: 50px;\" name=\"txtPrice\" value=\"" + item.OrderPrice.ToString("f2") + "\" />，立减￥<input type=\"text\" id=\"txtSendFull" + z + "\" onkeyup='KeyInt2(this)' class=\"send txtSendFull\" name=\"txtSendFull\" style=\"width: 50px;\" value=\"" + item.Discount.ToString("f2") + "\"/>";
                        if (z > 1)
                        {
                            html += "<a class=\"theme-color ml20 deleteItem\" href=\"javascript:;\">删除</a>";
                        }
                        html += "</label><br>";
                    }
                    this.SendFull.InnerHtml = html.Substring(0, html.LastIndexOf("<"));
                }
                else if (ProModel.ProType == 6)
                {
                    this.promotionType4.Checked = true;
                    this.promotionType3.Checked = false;
                    int x = 0;
                    foreach (Hi.Model.BD_PromotionDetail2 item in ll)
                    {
                        x++;
                        html += "<label>订单金额满￥<input type=\"text\" onkeyup='KeyInt2(this)' id=\"txtPrices" + x + "\" class=\"send txtPrices\" style=\"width: 50px;\" name=\"txtPrices\"  value=\"" + item.OrderPrice.ToString("f2") + "\"/>，打折（<input type=\"text\" id=\"txtDiscount" + x + "\" onkeyup='KeyInt2(this)' class=\"send txtDiscount\" name=\"txtDiscount\" style=\"width: 20px;\"  value=\"" + Convert.ToInt32(item.Discount) + "\"/>）%";
                        if (x > 1)
                        {
                            html += "<a class=\"theme-color ml20 deleteItem\" href=\"javascript:;\">删除</a>";
                        }
                        html += "</label><br>";
                    }
                    this.Discount.InnerHtml = html.Substring(0, html.LastIndexOf("<"));
                }
            }
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
        string ProType = string.Empty;  //促销方式
        string ProIsEnabled = string.Empty;    //是否启用
        string ProInfos = string.Empty;//促销描述
        string ProIsJieTi = string.Empty;//是否启用阶梯
        string PromotionDate = string.Empty;     //促销开始日期
        string PromotionEndDate = string.Empty;     //促销结束日期
        int isComNew = 0;       //是否发布促销公告

        #region 判断值是否为空，取值
        //促销方式
        ProType = Request["promotionType"].ToString();
        //是否启用
        ProIsEnabled = Request["IsEnabled"].ToString();
        //是否启用阶梯
        ProIsJieTi = Request["IsEnabled2"].ToString();
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
        if (str != "")
        {
            JScript.AlertMsgOne(this, str, JScript.IconOption.错误, 2500);
            return;
        }

        #endregion

        //是否发布促销公告
        isComNew = this.isOkComNews.Checked ? 1 : 0;
        string NewsContents = string.Empty;
        NewsContents = "<font size=\"3\" style=\"line-height:30px;\">";
        SqlTransaction Tran = null;
        try
        {
            Hi.Model.BD_Promotion proModel = null;
            int ProId = 0;
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            if (KeyID != 0)//修改
            {
                ProId = KeyID;
                List<Hi.Model.BD_PromotionDetail2> l = new Hi.BLL.BD_PromotionDetail2().GetList("", "isnull(dr,0)=0 and proId=" + KeyID + " and compId=" + this.CompID, "", Tran);
                if (l.Count > 0)
                {
                    foreach (Hi.Model.BD_PromotionDetail2 item in l)
                    {
                        new Hi.BLL.BD_PromotionDetail2().Delete(item.ID, Tran);
                    }
                }
                proModel = new Hi.BLL.BD_Promotion().GetModel(KeyID, Tran);
            }
            else
            {
                proModel = new Hi.Model.BD_Promotion();
            }
            //else
            //{
            proModel.Type = Type.ToInt(0);
            proModel.CompID = this.CompID;
            proModel.ProType = ProType.ToInt(0);
            if (ProType == "5")
            {
                proModel.ProTitle = "订单满减促销";
            }
            else if (ProType == "6")
            {
                proModel.ProTitle = "订单满折促销";
            }
            proModel.IsEnabled = ProIsEnabled.ToInt(0);
            proModel.Discount = 0;
            proModel.ProStartTime = PromotionDate.ToDateTime();
            proModel.ProEndTime = PromotionEndDate.ToDateTime();
            proModel.ProInfos = ProInfos;
            proModel.CreateUserID = this.UserID;
            proModel.CreateDate = DateTime.Now;
            proModel.modifyuser = this.UserID;
            proModel.ts = DateTime.Now.ToString();
            if (KeyID != 0)//修改
            {
                ProId = KeyID;
                proModel.ID = KeyID;
                new Hi.BLL.BD_Promotion().Update(proModel, Tran);

            }
            else
            {
                ProId = new Hi.BLL.BD_Promotion().Add(proModel, Tran);
            }
            if (ProType == "5")
            {
                string[] price = Request["txtPrice"].ToString().Split(',');//需要满金额
                string[] price2 = Request["txtSendFull"].ToString().Split(',');//满后需要减得金额
                for (int i = 0; i < price.Length; i++)
                {
                    if (price[i] != "")
                    {
                        Hi.Model.BD_PromotionDetail2 modelDeta = new Hi.Model.BD_PromotionDetail2();
                        modelDeta.CompID = this.CompID;
                        modelDeta.ProID = ProId;
                        modelDeta.OrderPrice = Convert.ToDecimal(price[i]);
                        modelDeta.Discount = Convert.ToDecimal(price2[i]);
                        modelDeta.modifyuser = this.UserID;
                        modelDeta.ts = DateTime.Now;
                        modelDeta.CreateUserID = this.UserID;
                        modelDeta.CreateDate = DateTime.Now;
                        new Hi.BLL.BD_PromotionDetail2().Add(modelDeta, Tran);
                        NewsContents += "订单金额满￥" + price[i] + "，立减￥" + price2[i] + "<br>";
                    }
                }
            }
            else if (ProType == "6")
            {
                string[] prices = Request["txtPrices"].ToString().Split(',');//需要满金额
                string[] prices2 = Request["txtDiscount"].ToString().Split(',');//满后折扣
                for (int i = 0; i < prices.Length; i++)
                {
                    if (prices[i] != "")
                    {
                        Hi.Model.BD_PromotionDetail2 modelDeta = new Hi.Model.BD_PromotionDetail2();
                        modelDeta.CompID = this.CompID;
                        modelDeta.ProID = ProId;
                        modelDeta.OrderPrice = Convert.ToDecimal(prices[i]);
                        modelDeta.Discount = Convert.ToDecimal(prices2[i]);
                        modelDeta.modifyuser = this.UserID;
                        modelDeta.ts = DateTime.Now;
                        modelDeta.CreateUserID = this.UserID;
                        modelDeta.CreateDate = DateTime.Now;
                        new Hi.BLL.BD_PromotionDetail2().Add(modelDeta, Tran);
                        NewsContents += "订单金额满￥" + prices[i] + "，打折（" + prices2[i] + "）%<br>";
                    }
                }
            }
            //}
            Hi.Model.BD_CompNews news = new Hi.Model.BD_CompNews();

            news.CompID = CompID;
            news.CreateDate = DateTime.Now;
            news.CreateUserID = UserID;
            news.dr = 0;
            news.ts = DateTime.Now;
            news.IsTop = 1;
            news.IsEnabled = 0;
            news.NewsType = 4;
            news.ShowType = "2";
            if (ProType == "5")
            {
                news.NewsTitle = "订单满减促销";
            }
            else if (ProType == "6")
            {
                news.NewsTitle = "订单满折促销";
            }

            NewsContents += "先到先得！";
            ProInfos = ProInfos == "" ? "" : "（" + ProInfos + "）";
            NewsContents += ProInfos;

            NewsContents += " </br> 活动时间：" + PromotionDate.ToDateTime().ToString("yyyy-MM-dd") + "至" + PromotionEndDate.ToDateTime().ToString("yyyy-MM-dd");

            NewsContents += "</font>";
            news.NewsContents = NewsContents;
            news.modifyuser = UserID;
            news.PmID = ProId;
            if (isComNew == 1)
            {
                new Hi.BLL.BD_CompNews().Add(news, Tran);
            }
            Tran.Commit();
            Response.Write("<script>window.location.href='PromotionInfo2.aspx?KeyId=" + Common.DesEncrypt(ProId.ToString(), Common.EncryptKey) + "&type=" + Type + "';</script>");
        }
        catch (Exception ex)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                    Tran.Rollback();
            }
            JScript.AlertMsgOne(this, "保存失败了", JScript.IconOption.错误, 2500);
            return;
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }
    }
}
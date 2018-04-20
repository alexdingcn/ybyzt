

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Distributor_Shop : DisPageBase
{
    //是否启用商品库存，默认启用库存
    public int IsInve = 0;
    public int comp = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ddrComp.Attributes.Add("onchange", base.Page.GetPostBackEventReference(this.ddrComp));  
        if (!IsPostBack)
        {
            IsInve = OrderInfoType.rdoOrderAudit("商品是否启用库存", CompID).ToInt(0);
            this.hidIsInve.Value = IsInve.ToString();
            //小数位数
            this.hidsDigits.Value = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", this.CompID);

            Common.ListComps(this.ddrComp, this.UserID.ToString(), this.CompID.ToString());
            databind();
            ToTalPrice2();//购物车总价
        }
    }

    protected void databind()
    {
        DataTable dt = new Hi.BLL.DIS_ShopCart().GetGoodsCart(" sc.[CompID]=" +  this.ddrComp.Value + " and sc.[DisID]=" + this.DisID + "and sc.dr=0", "sc.[CreateDate] desc ");

        if (dt != null && dt.Rows.Count > 0)
        {
            this.rprCart.DataSource = dt;
            this.rprCart.DataBind();
        }
        else
        {
            Clear();
            this.rprCart.DataSource = null;
            this.rprCart.DataBind();
        }
    }

    /// <summary>
    /// 清除DataTable数据
    /// </summary>
    public void Clear()
    {
        if (Session["GoodsCart"] != null)
        {
            ////清空购物车
            //string str = " CompID=" + user.CompID + " and DisID=" + user.DisID;
            //new Hi.BLL.DIS_ShopCart().CartEmpty(str);

            //清除全部Session
            Session["GoodsCart"] = null;
            Session.Remove("GoodsCart");
        }
    }

    /// <summary>
    /// 获取最新的库存
    /// </summary>
    /// <returns></returns>
    public decimal NewInventory(string compid, string goodsinfoid)
    {
        string inv = string.Empty;
        bool bol = BLL.Common.GetInevntory(Convert.ToInt32(compid), Convert.ToInt32(goodsinfoid), 0, 0, out inv);

        return Convert.ToDecimal(inv);

    }
    /// <summary>
    /// 最新的商品小计
    /// </summary>
    /// <returns></returns>
    public decimal Totalprice(int compid, int disid, int goodsinfoid, string num)
    {
        decimal price = BLL.Common.GetGoodsPrice(compid, disid, goodsinfoid);
        return Convert.ToDecimal(Convert.ToDecimal(price).ToString("0.00")) * Convert.ToDecimal(Convert.ToDecimal(num).ToString("0.00"));
    }
    /// <summary>
    /// 商品总价
    /// </summary>
    /// <returns></returns>
    public void ToTalPrice2()
    {
        decimal price = 0;
        List<int> list = new List<int>();
        Dictionary<int, decimal> num = new Dictionary<int, decimal>();
        List<Hi.Model.DIS_ShopCart> l = new Hi.BLL.DIS_ShopCart().GetList("", "isnull(dr,0)=0 and compid=" + this.ddrComp.Value + " and disid=" + this.DisID, "");

        if (l.Count > 0)
        {
            for (int i = 0; i < l.Count; i++)
            {
                list.Add(l[i].GoodsinfoID);
                num.Add(l[i].GoodsinfoID, l[i].GoodsNum);
            }
            List<BLL.gDprice> lsit = BLL.Common.GetPrice(this.CompID, this.DisID, list);
            if (lsit.Count > 0)
            {
                foreach (BLL.gDprice item in lsit)
                {
                    price += item.FinalPrice * num[item.goodsInfoId];
                }
            }
        }
        this.sumPrice.InnerHtml = price.ToString().ToDecimal(0).ToString("N");
    }

    /// <summary>
    /// 选择厂商
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void comp_ServerChange(object sender, EventArgs e)
    {
        databind();
        ToTalPrice2();//购物车总价
    } 
}
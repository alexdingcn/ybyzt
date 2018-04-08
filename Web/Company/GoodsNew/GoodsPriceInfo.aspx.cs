using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class Company_Goods_GoodsPriceInfo : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (KeyID != 0)
            {

                try
                {
                    Hi.Model.BD_DisPrice model = new Hi.BLL.BD_DisPrice().GetModel(KeyID);
                    if (model != null)
                    {
                        //this.lblDisTitle.InnerText = model.Title.ToString();
                        //this.lblDisID.InnerText = model.DisNames.ToString();
                        //this.txtRemark.Value = model.Remark;
                    }
                    Bind(KeyID.ToString());
                }
                catch (Exception ex)
                {
                    JScript.AlertMethod(this.Page, "数据有误", JScript.IconOption.错误, "function(){location.href='GoodsPriceList.aspx';}");
                    return;
                }

            }
        }
    }
    /// <summary>
    /// 绑定
    /// </summary>
    public void Bind(string id)
    {
        string strWhere = string.Format("isnull(dr,0)=0 and dispriceid=" + id + " and compid=" + this.CompID, this.CompID);
        string sql = string.Format(@"select MAX(id) As id,  GoodsInfoID,tinkerprice,disId from bd_dispriceinfo where {0}
                        group by GoodsInfoID,tinkerprice,disId", strWhere);
        this.rptGoodsPrice.DataSource = SqlHelper.Query(SqlHelper.LocalSqlServer, sql);
        this.rptGoodsPrice.DataBind();
    }
    /// <summary>
    /// 得到商品名称
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetGoodsName(int id)
    {
        Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(id);
        if (model != null)
        {
            Hi.Model.BD_Goods model2 = new Hi.BLL.BD_Goods().GetModel(model.GoodsID);
            return model2.GoodsName;
        }
        return "";
    }
    /// <summary>
    /// 得到属性
    /// </summary>
    /// <returns></returns>
    public string GoodsAttr(string id)
    {
        string str = string.Empty;
        string str2 = string.Empty;
        Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(id));
        if (model != null)
        {
            str = model.ValueInfo;
            if (!Util.IsEmpty(str))
            {
                string[] lsit = { };
                string[] lsit2 = { };
                lsit = str.Replace(';', '；').Split('；');
                for (int i = 0; i < lsit.Length; i++)
                {
                    if (lsit[i] != "")
                    {
                        lsit2 = lsit[i].Split(':');
                        str2 += lsit2[0] + "：" + "<label style='color:#0080b8'>" + lsit2[1] + "</label>" + "；";
                    }
                }
            }
            else
            {
                str2 = Common.GetGoodsMemo(Convert.ToInt32(model.GoodsID));
            }
        }
        return str2;
    }
    /// <summary>
    /// 获取基础价格
    /// </summary>
    /// <returns></returns>
    public string GetPrice(int id)
    {
        Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(id);
        if (model != null)
        {
            return model.SalePrice.ToString();
        }
        return "";
    }
}
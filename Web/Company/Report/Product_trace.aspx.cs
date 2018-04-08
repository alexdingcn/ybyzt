using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Company_Product_trace : CompPageBase
{
    public string goodsid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            string action = (Request["ActionType"] + "").Trim();

            if (action == "GetDisHT")
            {
                Response.Write(GetDisHT((Request["goodsid"] + ""), (Request["DisName"] + ""), (Request["HtName"] + "")));
                Response.End();
            }

            bind();
        }
    }


    public void bind()
    {
        //string sql = string.Format(@"");

        string strwhere = string.Empty;//代理商、商品搜索条件
        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }

        List<Hi.Model.BD_Goods> list = new Hi.BLL.BD_Goods().GetList("", " dr=0 and IsEnabled=1 and IsOffline=1 and CompID=" + this.CompID + strwhere, "");

        if (list != null && list.Count > 0)
            goodsid = list[0].ID.ToString();

        rpt_Goods.DataSource = list;
        rpt_Goods.DataBind();
    }


    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["strwhere"] = Where();
        bind();
    }

    /// <summary>
    /// 商品名称条件
    /// </summary>
    /// <returns></returns>
    protected string Where()
    {
        string strWhere = string.Empty;

        if (this.txtGoodsName.Value.Trim() != "")
        {
            strWhere += " and  GoodsName like '%" + this.txtGoodsName.Value.Trim().ToString().Replace("'", "''") + "%'";
        }
        return strWhere;
    }


    public string GetDisHT(string goodsinfoID,string DisName,string HtName)
    {
        string str = string.Empty;
        if (DisName != "") {
            str += "and dis.DisName like '%" + DisName + "%'";
        }
        if (HtName != "")
        {
            str += "and HospitalName like '%" + HtName + "%'";
        }

        string sql = string.Format(@"select o.DisID,dis.DisName,HtID,HospitalName HtName,o.CompID,sum(od.GoodsNum) GoodsNum,sum(od.GoodsPrice) GoodsPrice from 
DIS_OrderDetail od left join DIS_Order o on od.OrderID=o.ID
left join BD_GoodsInfo info on info.ID=od.GoodsinfoID 
left join BD_Goods g on info.GoodsID=g.ID
left join BD_Distributor dis on dis.ID=o.DisID
left join 
(select GoodsID,HtID,ht.HospitalName from YZT_FirstCamp fc left join YZT_CMerchants cm on cm.ID=fc.CMID
left join SYS_Hospital ht on ht.ID=fc.HtID
where fc.State=2 and fc.dr=0 and cm.dr=0 and cm.IsEnabled=1) CMFC
on od.GoodsinfoID=CMFC.GoodsID
where o.OState in(2,4,5) and o.CompID={0} and info.GoodsID={1} {2}
group by o.DisID,dis.DisName,HtID,HospitalName,o.CompID order by o.DisID ", this.CompID, goodsinfoID, str);

        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        return ConvertJson.ToJson(ds); 
    }
}
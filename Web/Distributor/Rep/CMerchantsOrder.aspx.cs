using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class Distributor_Rep_CMerchantsOrder : DisPageBase
{
    public string page = "1";//默认初始页
    //Hi.Model.SYS_Users user = null;
    Hi.Model.BD_Distributor dis = null;
    public decimal ta = 0;
    public decimal tb = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";



            //this.txtArriveDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
            //this.txtArriveDate1.Value = DateTime.Now.ToString("yyyy-MM-dd");

            Common.ListFMComps(this.ddrComp, this.UserID.ToString(), this.DisID.ToString());
            ViewState["strwhere"] = Where();
            Bind();
        }
    }


    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        string date = string.Empty;
        string strwhere = string.Empty;
        string sqldate = string.Empty;

        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
        if (this.txtPager.Value.Trim().ToString() != "")
        {
            if (this.txtPager.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPager.Value = "100";
            }
            else
            {
                Pager.PageSize = this.txtPager.Value.Trim().ToInt(0);
            }
        }
        //if (this.txtArriveDate.Value.Trim() == "" && this.txtArriveDate1.Value.Trim() == "")
        //{
        //    this.txtArriveDate.Value = Convert.ToDateTime(DateTime.Now.Date.ToString().Substring(0, 4) + "/1/1").ToString("yyyy-MM-dd");
        //    this.txtArriveDate1.Value = DateTime.Now.ToString("yyyy-MM-dd");
        //    date = " and cm.InvalidDate>='" + DateTime.Now.Date.ToString().Substring(0, 4) + "/1/1 0:0:0' ";
        //}

        if (this.ddrComp.Value != "")
        {
            strwhere += "and cm.CompID=" + this.ddrComp.Value;
        }

        if (this.txtArriveDate.Value.Trim() != "")
        {
            sqldate += " and l.LibraryDate>='" + Convert.ToDateTime(this.txtArriveDate.Value.Trim()) + "'";
        }
        if (this.txtArriveDate1.Value.Trim() != "")
        {
            sqldate += " and l.LibraryDate<'" + Convert.ToDateTime(this.txtArriveDate1.Value.Trim()).AddDays(1) + "'";
        }

//        string sql = string.Format(@"select info.ID,g.GoodsName,info.ValueInfo,cm.ForceDate,cm.InvalidDate,
//com.CompName,SUM(od.GoodsNum) num,SUM(od.sumAmount) GoodsPrice from 
//(select cm.GoodsID,cm.ForceDate,cm.InvalidDate,cm.CompID,fc.DisID from YZT_FirstCamp fc left join YZT_CMerchants cm on fc.CMID=cm.ID where
//cm.IsEnabled=1 and cm.dr=0 and fc.State=2
//union
//select cd.GoodsID,c.ForceDate,c.InvalidDate,c.CompID,c.DisID from YZT_ContractDetail cd left join YZT_Contract c on cd.ContID=c.ID
//where c.CState<>2 and c.dr=0) cm
//left join BD_GoodsInfo info on cm.GoodsID=info.ID
//left join BD_Goods g on g.ID=info.GoodsID
//left join BD_Company com on com.ID=cm.CompID
//left join DIS_OrderDetail od on od.GoodsinfoID=info.ID join DIS_Order o on o.ID=od.OrderID
//where o.OState in(2,4,5) and cm.DisID={0} {1} group by info.ID,
//g.GoodsName,info.ValueInfo,cm.ForceDate,cm.InvalidDate,com.CompName order by info.ID", this.DisID, strwhere);

        string sql = string.Format(@"select cm.id,cm.GoodsName,cm.ForceDate,cm.InvalidDate,cm.CompName,
SUM(cm.num) num,SUM(cm.GoodsPrice) GoodsPrice,SUM(cm.LdAmount) LdAmount
 from (
select g.id,g.GoodsName,cm.ForceDate,cm.InvalidDate,
com.CompName,SUM(od.GoodsNum) num,SUM(od.sumAmount) GoodsPrice,ld.sumAmount LdAmount from 

(select * from (select cm.GoodsID,fc.ForceDate,fc.InvalidDate,cm.CompID,fc.DisID from YZT_FirstCamp fc left join YZT_CMerchants cm on fc.CMID=cm.ID where cm.dr=0 and fc.State=2
 union
select cd.GoodsID,c.ForceDate,c.InvalidDate,c.CompID,c.DisID from YZT_ContractDetail cd left join YZT_Contract c on cd.ContID=c.ID
where not exists (select cm.GoodsID from YZT_FirstCamp fc left join YZT_CMerchants cm on fc.CMID=cm.ID where cm.dr=0 and fc.State=2 and cm.GoodsID=cd.GoodsID) and c.CState<>2 and c.dr=0
) cm) cm
left join
(
select ID,GoodsID from BD_GoodsInfo where GoodsID in(
select info.GoodsID from (select cm.GoodsID,fc.ForceDate,fc.InvalidDate,cm.CompID,fc.DisID from YZT_FirstCamp fc left join YZT_CMerchants cm on fc.CMID=cm.ID where cm.dr=0 and fc.State=2
 union
select cd.GoodsID,c.ForceDate,c.InvalidDate,c.CompID,c.DisID from YZT_ContractDetail cd left join YZT_Contract c on cd.ContID=c.ID
where not exists (select cm.GoodsID from YZT_FirstCamp fc left join YZT_CMerchants cm on fc.CMID=cm.ID where cm.dr=0 and fc.State=2 and cm.GoodsID=cd.GoodsID) and c.CState<>2 and c.dr=0
) fcm 
left join BD_GoodsInfo info on fcm.GoodsID=info.ID
)

) info on cm.GoodsID=info.ID

left join BD_Goods g on g.ID=info.GoodsID
left join DIS_OrderDetail od on od.GoodsinfoID=cm.GoodsID  left join DIS_Order o on o.ID=od.OrderID and o.OState in(2,4,5)
left join BD_Company com on com.ID=cm.CompID
left join (select isnull(sum(sumAmount),0) sumAmount, gs.GoodsID goodsinfoID,info.GoodsID from YZT_LibraryDetail ld left join YZT_Library l
on ld.LibraryID=l.ID
left join YZT_GoodsStock gs on ld.StockID=gs.ID
left join BD_GoodsInfo info on gs.GoodsID=info.ID
where ISNULL(l.dr,0)=0 and isnull(l.IState,0)=1 {2}
group by gs.GoodsID,info.GoodsID) ld on 
ld.GoodsID=info.GoodsID
where cm.DisID={0} {1}
group by g.id,g.GoodsName,cm.ForceDate,cm.InvalidDate,com.CompName,ld.sumAmount)
cm group by cm.id,cm.GoodsName,cm.ForceDate,cm.InvalidDate,cm.CompName
order by cm.GoodsName ", this.DisID, strwhere, sqldate);

        Pagger pagger = new Pagger(sql);
        Pager.RecordCount = pagger.GetDataCount(sql);
        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        for (int i = 0; i < ds.Rows.Count; i++)
        {
           
        }
        this.rptOrder.DataSource = dt;
        this.rptOrder.DataBind();
        page = Pager.CurrentPageIndex.ToString();
    }

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        ViewState["strwhere"] = Where();
        Bind();
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void A_Seek(object sender, EventArgs e)
    {
        ViewState["strwhere"] = Where();
        Bind();
    }

    protected string Where()
    {
        string strWhere = string.Empty;
        string sqldate = string.Empty;
        
        if (this.txtArriveDate.Value.Trim() != "")
        {
            strWhere += " and o.CreateDate>='" + Convert.ToDateTime(this.txtArriveDate.Value.Trim()) + "'";
        }
        if (this.txtArriveDate1.Value.Trim() != "")
        {
            strWhere += " and o.CreateDate<'" + Convert.ToDateTime(this.txtArriveDate1.Value.Trim()).AddDays(1) + "'";
        }
        if(this.txtGoodsName.Value.Trim()!=""){
            strWhere += " and g.GoodsName like '%" + this.txtGoodsName.Value.Trim() + "%'";
        }

        return strWhere;

    }


    public string LibraryDetail(string goodsid)
    {

        string msg = "0";
        string sql = string.Format(@"select isnull(sum(sumAmount),0) sumAmount from YZT_LibraryDetail ld left join YZT_Library l
on ld.LibraryID=l.ID
left join YZT_GoodsStock gs on ld.StockID=gs.ID
where ISNULL(l.dr,0)=0 and isnull(l.IState,0)=1 and l.DisID={0} and gs.GoodsID={1}", this.DisID, goodsid);


        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (ds != null && ds.Rows.Count > 0)
        {
            msg = Convert.ToDecimal(ds.Rows[0]["sumAmount"]).ToString("0");
        }

        return msg;


    }
}
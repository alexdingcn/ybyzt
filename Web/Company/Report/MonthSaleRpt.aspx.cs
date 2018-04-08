using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Company_Report_MonthSaleRpt : CompPageBase
{
    Hi.BLL.DIS_Order OrderInfoBLL = new Hi.BLL.DIS_Order();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public decimal ta = 0;
    public decimal tb = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>$(\".txt_txtTypename,.txt_txtAreaname\").css(\"width\", \"150px\");</script>");
        //txtDisArea.CompID = CompID.ToString();
        //txtDisType.CompID = CompID.ToString();
        if (!IsPostBack)
        {
            this.txtPager.Value = Common.PageSize;           
            if (Request["type"] == "1")
            {
                this.txtBCreateDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
                this.txtECreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            }
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

        if (this.txtBCreateDate.Value.Trim() == "" && this.txtECreateDate.Value.Trim() == "")
        {
            this.txtBCreateDate.Value = Convert.ToDateTime(DateTime.Now.Date.ToString().Substring(0, 4) + "/1/1").ToString("yyyy-MM-dd");
            this.txtECreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            date = " and CreateDate>='" + DateTime.Now.Date.ToString().Substring(0,4) + "/1/1 0:0:0' ";
        }
        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }

        string sql = "select CompID,DisID,YEAR([CreateDate]) Years,MONTH([CreateDate]) as Months,SUM([sumAmount]) as [TotalAmount],sum( CASE WHEN Otype=9 THEN AuditAmount ELSE 0 END) as [zdAmount] " +
              " from ( SELECT * FROM  [dbo].[MonthSaleRpt_view] where compID=" + this.CompID + 
              date + strwhere +
              ")M " +
              "where compID=" + this.CompID +
              " group by YEAR([CreateDate]), MONTH([CreateDate]),CompID,disID order by DisID,YEAR([CreateDate]),MONTH([CreateDate])";
        
        Pagger pagger = new Pagger(sql);

        Pager.RecordCount = pagger.GetDataCount(sql);

        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        for (int i = 0; i < ds.Rows.Count; i++)
        {
            ta += Convert.ToDecimal(ds.Rows[i]["TotalAmount"].ToString() == "" ? "0" : ds.Rows[i]["TotalAmount"].ToString());
            tb += Convert.ToDecimal(ds.Rows[i]["zdAmount"].ToString() == "" ? "0" : ds.Rows[i]["zdAmount"].ToString());
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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["strwhere"] = Where();
        Bind();
    }
    protected string Where()
    {
        string strWhere = string.Empty;

        if (this.ddrOState.Value != "-2")
        {
            strWhere += " and OState=" + this.ddrOState.Value;
        }
        else
        {
            strWhere += " and OState in(2,4,5)";
        }

        if (this.txtDisName.Value.Trim() != "")
        {
            strWhere += " and  DisName like '%" + this.txtDisName.Value.Trim().ToString().Replace("'", "''") + "%'";
        }

        //商品类别 ：根据GoodsID查询
        //string hideID = this.txtCategory.treeId.Trim();
        //if (this.txtCategory.treeId.Trim() != "")
        //{
        //    string idlist = string.Empty;
        //    if (!Util.IsEmpty(hideID))
        //    {
        //        string cateID = Common.CategoryId(Convert.ToInt32(hideID), this.CompID);//商品分类递归
        //        strWhere += " and categoryID in(" + cateID + ") ";
        //    }
        //}

        //if (txtDisArea.areaId != "") //areaID
        //{
        //    string disarea = Common.DisAreaId(Convert.ToInt32(txtDisArea.areaId), this.CompID);
        //    strWhere += " and AreaID in (" + disarea + ")";
        //}

        //if (txtDisType.typeId != "") //areaIDDisType
        //{
        //    string disarea = Common.DisTypeId(Convert.ToInt32(txtDisType.typeId), this.CompID);
        //    strWhere += " and DisTypeID in (" + disarea + ")";
        //}

        if (this.txtBCreateDate.Value.Trim() != "")
        {
            strWhere += " and CreateDate>='" + Convert.ToDateTime(this.txtBCreateDate.Value.Trim()) + "'";
        }
        if (this.txtECreateDate.Value.Trim() != "")
        {
            strWhere += " and CreateDate<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        return strWhere;
    }
}
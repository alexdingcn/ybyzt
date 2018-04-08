using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_PmtManager_PromotionList : CompPageBase
{
    public string page = "1";  //默认初始页
    Hi.BLL.BD_Promotion ProBll = new Hi.BLL.BD_Promotion(); //促销
    //促销类型：  1、商品促销  2、订单促销
    public string Type = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Type"] + "" != "")
        {
            Type = Request.QueryString["Type"] + "";
            this.protitle.Attributes.Add("href", "../PmtManager/PromotionList.aspx?type=" + Type);
            if (Type == "1")
                this.protitle.InnerText = "商品促销";
            else
                this.protitle.InnerText = "订单促销";
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
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;

        if (ViewState["strwhere"] != null)
            strwhere += ViewState["strwhere"].ToString();

        strwhere += " and CompID=" + CompID + " and type=" +Common.NoHTML( Type) + " and isnull(dr,0)=0";

        //每页显示的数据设置
        if (this.txtPageSize.Value.ToString() != "")
        {
            if (this.txtPageSize.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPageSize.Value = "100";
            }
            else
                Pager.PageSize = this.txtPageSize.Value.Trim().ToInt(0);
        }

        List<Hi.Model.BD_Promotion> l = ProBll.GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, strwhere, out pageCount, out Counts);

        this.rpt_Promotion.DataSource = l;
        this.rpt_Promotion.DataBind();

        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Pager_PageChanged(object sender, EventArgs e)
    {
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

    /// <summary>
    /// 搜索条件
    /// </summary>
    /// <returns></returns>
    public string Where()
    {
        string strWhere = string.Empty;

        if (this.txtProtitle.Value.ToString() != "")
        {
            strWhere += " and ProTitle like '%" +Common.NoHTML( this.txtProtitle.Value.Trim().ToString().Replace("'", "''")) + "%'";
        }
        if (Request.Form["ddrProType"].ToString() != "-1")
        {
            strWhere += " and ProType=" +Common.NoHTML( Request.Form["ddrProType"].ToString());
        }
        if (this.txtProDate.Value != "")
        {
            strWhere += " and ProStartTime>='" +this.txtProDate.Value.Trim().ToDateTime() + "'";
        }
        if (this.txtEndProDate.Value != "")
        {
            strWhere += "and ProEndTime<='" + this.txtEndProDate.Value.Trim().ToDateTime().AddDays(1) + "'";
        }

        return strWhere;
    }
}
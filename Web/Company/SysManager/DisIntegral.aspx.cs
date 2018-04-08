using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_SysManager_DisIntegral : CompPageBase
{
    public string page = "1";//默认初始页
    public int DisID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        DisID = (Request["DisID"] + "").ToInt(0);

        if (!IsPostBack)
        {
            this.txtPager.Value = Common.PageSize;

            Bind();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    protected void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;
        strwhere += " and isnull(dr,0)=0 and DisID=" + DisID + "and CompID=" + CompID;

        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }

        if (this.txtPager.Value.Trim().ToString() != "" && this.txtPager.Value.Trim().ToString() != "0")
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

        List<Hi.Model.DIS_Integral> Integral = new Hi.BLL.DIS_Integral().GetList(Pager.PageSize, Pager.CurrentPageIndex, "ts", true, strwhere, out pageCount, out Counts);

        this.Rpt_Dis.DataSource = Integral;
        this.Rpt_Dis.DataBind();

        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        Bind();
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_SearchClick(object sender, EventArgs e)
    {
        Where();
        Bind();
    }

    /// <summary>
    /// 搜索条件
    /// </summary>
    public void Where()
    {

        string str = string.Empty;

        if (this.txtReceiptNo.Value.ToString().Trim() != "")
        {
            str += "and OrderId in (select Id from DIS_order where ReceiptNo like '%" + this.txtReceiptNo.Value.ToString().Trim().Replace("'", "''") + "%')";
        }
        if (this.txtCreateDate.Value.ToString().Trim() != "")
        {
            str += " and CreateDate>='" + this.txtCreateDate.Value.ToString().Trim() + "'";
        }
        if (this.txtEndCreateDate.Value != "")
        {
            str += "and CreateDate<'" + this.txtEndCreateDate.Value.Trim().ToDateTime().AddDays(1) + "'";
        }

        ViewState["strwhere"] = str;

       
    }
}
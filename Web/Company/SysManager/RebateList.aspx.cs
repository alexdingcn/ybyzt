using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_SysManager_RebateList : CompPageBase
{
    public string page = "1";//默认初始页
    public decimal ta = 0;
    public decimal tb = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<Hi.Model.BD_Rebate> rebateList = new Hi.BLL.BD_Rebate().GetList("", "compID='" + CompID + "' and RebateState = 1 and dr = 0", "");
            {
                try
                {
                    if (rebateList != null && rebateList.Count > 0)
                    {
                        foreach (var item in rebateList.Where(p => p.EndDate.AddDays(1) < DateTime.Now))
                        {
                            item.RebateState = 2;
                            item.ts = DateTime.Now;
                            item.modifyuser = UserID;
                            new Hi.BLL.BD_Rebate().Update(item);
                        }
                    }
                }
                catch
                {
                }
            }

            Bind();
            if (!Common.HasRight(this.CompID, this.UserID, "1119"))
                this.libtnAdd.Visible = false;
        }
    }

    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;

        //每页显示的数据设置
        if (this.txtPageSize.Value.ToString() != "")
        {
            if (this.txtPageSize.Value.Trim().ToInt(0) >= 10000)
            {
                Pager.PageSize = 100;
                this.txtPageSize.Value = "100";
            }
            else
            {
                Pager.PageSize = this.txtPageSize.Value.Trim().ToInt(0);
            }
        }
        List<Hi.Model.BD_Rebate> CompNewsNotice = new Hi.BLL.BD_Rebate().GetList(Pager.PageSize, Pager.CurrentPageIndex, "EndDate", true, SearchWhere(), out pageCount, out Counts);
        //计算总计 begin

        foreach(Hi.Model.BD_Rebate  rebate in CompNewsNotice)
        {
            ta += Convert.ToDecimal(rebate.RebateAmount.ToString() == "" ? "0" : rebate.RebateAmount.ToString());
            tb += Convert.ToDecimal(rebate.EnableAmount.ToString() == "" ? "0" : rebate.EnableAmount.ToString());
        
        }
        //计算总计  end

        
        
        this.Rpt_Distribute.DataSource = CompNewsNotice;
        this.Rpt_Distribute.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    public void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }

    public void btn_SearchClick(object sender, EventArgs e)
    {
        Bind();
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        
        string where = "  and isnull(dr,0)=0 and Compid=" + CompID + "";
        if (DisSalesManID != 0)
        {
            where += "and DisID in(select ID from BD_Distributor where smid = " + DisSalesManID + ")";
        }
        if (!string.IsNullOrEmpty(txtRebateCode.Value.Trim()))
        {
            where += " and (ReceiptNo like '%" + Common.NoHTML(txtRebateCode.Value.Trim().Replace("'", "''")) + "%' )";
        }

        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            where += " and DisID in (select ID from BD_Distributor where DisName like '%" + Common.NoHTML(txtDisName.Value.Trim()) + "%')";
        }

        if (!string.IsNullOrEmpty(ddlRebateState.SelectedValue) && ddlRebateState.SelectedValue != "0")
        {
            where += " and RebateState='" + Common.NoHTML(ddlRebateState.SelectedValue) + "'";
        }
        
        return where;
    }
}
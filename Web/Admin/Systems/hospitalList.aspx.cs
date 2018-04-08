using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Systems_PaybankList : AdminPageBase
{
    public int Id = 0;  //订单Id
    Hi.BLL.SYS_PaymentBank PAbll = new Hi.BLL.SYS_PaymentBank();
    public string page = "1";//默认初始页

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            Bind();

        }
    }

    /// <summary>
    /// 列表显示
    /// </summary>
    public void Bind()
    {
        //查询
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;

        string txtHospitalName = Common.NoHTML(this.txtHospitalName.Value.Trim().Replace("'", ""));
        if (txtHospitalName != "")
        {
            strwhere += " and  HospitalName like '%" + txtHospitalName + "%' ";
        }
        strwhere += " and isnull(dr,0)=0"; //IsDel=1  订单已删除
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

        List<Hi.Model.SYS_Hospital> hospital = new Hi.BLL.SYS_Hospital().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, strwhere, out pageCount, out Counts);
        this.rptPAcount.DataSource = hospital;
        this.rptPAcount.DataBind();

        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Bind();
    }


    /// <summary>
    /// 分页控件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }
  
   
}


using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_ReturnOrderList1 : DisPageBase
{
    public string page = "1";//默认初始页
    //Hi.Model.SYS_Users user = null;
    Hi.Model.BD_Distributor dis = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);
        dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (!IsPostBack)
        {
            txtPager.Value = "12";
            Bind();
        }
    }

    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;
        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
        strwhere += "and Otype!=9 and isnull(dr,0)=0 and OState=5 and returnstate=0 and CreateDate>'" + DateTime.Now.AddMonths(-3) + "' and disid=" + this.DisID;

        if (this.txtPager.Value.Trim().ToString() != "")
        {
            if (this.txtPager.Value.Trim().Length < 4)
            {
                Pager.PageSize = int.Parse(this.txtPager.Value.Trim());
            }
            else
            {
                this.txtPager.Value = "100";
                Pager.PageSize = 100;
            }
        }

        List<Hi.Model.DIS_Order> orders = new Hi.BLL.DIS_Order().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, strwhere, out pageCount, out Counts);
        this.rptOrder.DataSource = orders;
        this.rptOrder.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    public void A_Seek(object sender, EventArgs e)
    {
        string strwhere = string.Empty;
        if (!string.IsNullOrEmpty(orderid.Value.Trim()))
        {
            strwhere += " and receiptno like ('%" + Common.NoHTML(orderid.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (dllotype.Value != "-1")
        {
            strwhere += " and otype=" + Common.NoHTML(dllotype.Value);
        }
        ViewState["strwhere"] = strwhere;
        Bind();
    }

    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }
}
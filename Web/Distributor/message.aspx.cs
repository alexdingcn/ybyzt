
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_message :DisPageBase
{
    public string username = "hainan";
    public string page = "1";//默认初始页
    //Hi.Model.SYS_Users user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        //user = new Hi.BLL.SYS_Users().GetModel(username);
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
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
        strwhere += " and isnull(dr,0)=0 and Stype=0 and DisUserID=" + this.UserID + " and not exists(select 1 from DIS_Suggest s where s.suggest=DIS_Suggest.suggest and DIS_Suggest.id<s.id)";

        if (this.txtPager.Value.Trim().ToString() != "")
        {
            Pager.PageSize = int.Parse(this.txtPager.Value.Trim());
        }

        List<Hi.Model.DIS_Suggest> suggest = new Hi.BLL.DIS_Suggest().GetList(Pager.PageSize, Pager.CurrentPageIndex, "ts", true, strwhere, out pageCount, out Counts);
        this.rptmessage.DataSource = suggest;
        this.rptmessage.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }
}
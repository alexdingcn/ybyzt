

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Distributor_MessAgeList : DisPageBase
{
    public string page = "1";//默认初始页
    //Hi.Model.SYS_Users user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);
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
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["IsAnswer"]) && Request["IsAnswer"].ToString() == "1")
            {
                strwhere += " and IsAnswer=1";
                dllisanswer.Value = "1";
            }
        }
        strwhere += " and isnull(dr,0)=0 and Stype=0 and DisUserID=" + this.UserID;
        
        if (this.txtPager.Value.Trim().ToString() != "")
        {
            if (this.txtPager.Value.Trim().Length < 4)
            {
                Pager.PageSize = int.Parse(this.txtPager.Value.Trim());
            }
            else
            {
                this.txtPager.Value = "12";
                this.Pager.PageSize = 12;
            }
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

    public void A_Seek(object sender, EventArgs e)
    {
        string strwhere = string.Empty;
        if (!string.IsNullOrEmpty(txttitle.Value))
        {
            strwhere += " and title like ('%" + Common.NoHTML(txttitle.Value.Replace("'", "''")) + "%')";
        }
        if (dllisanswer.Value != "-1")
        {
            if (dllisanswer.Value == "1")
            {
                strwhere += " and isanswer in (1,2)";
            }
            else
            {
                strwhere += " and isanswer=" + Common.NoHTML(dllisanswer.Value);
            }
        }
        ViewState["strwhere"] = strwhere;
        Bind();
    }

    public void A_Del(object sender, EventArgs e)
    {
        HtmlAnchor a = sender as HtmlAnchor;
        string delid = a.Attributes["delid"];
        if (new Hi.BLL.DIS_Suggest().Delete(int.Parse(delid)))
        {
            Bind();//  ClientScript.RegisterStartupScript(this.GetType(), "Reslut", "<script>location.href=location.href</script>");
        }
    }
}
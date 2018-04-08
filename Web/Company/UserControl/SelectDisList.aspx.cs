using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_UserControl_SelectDisList : System.Web.UI.Page
{
    public string page = "1";//默认初始页
    string Compid
    {
        get { return Request["compid"] + ""; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>$(\".txt_txtTypename,.txt_txtAreaname\").css(\"width\", \"130px\");</script>");
        if (!IsPostBack)
        {
            txtDisAreaBox.CompID = Compid;
            txtDisType.CompID = Compid;
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            DataBinds();
        }
    }

    public void DataBinds()
    {
        txtDisAreaBox.CompID = Compid;
        txtDisType.CompID = Compid;

        int pageCount = 0;
        int Counts = 0;
        List<Hi.Model.BD_Distributor> LDis = new Hi.BLL.BD_Distributor().GetList(Pager.PageSize, Pager.CurrentPageIndex, "Createdate", false, SearchWhere(), out pageCount, out Counts);
        this.Rpt_Dis.DataSource = LDis;
        this.Rpt_Dis.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string areaId = this.txtDisAreaBox.areaId;//区域
        string type = this.txtDisType.typeId;  //分类
        string where = "  and auditstate=2 and isnull(IsEnabled,0)=1  and isnull(dr,0)=0 and CompID=" + Compid.ToInt(0) + "";
        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            where += " and ( DisName like '%" + txtDisName.Value.Trim() + "%')";
        }
        if (areaId != "")
        {
            where += "and AreaID=" + areaId;
        }
        if (type != "")
        {
            where += "and DisTypeID=" + type;
        }

        //if (!string.IsNullOrEmpty(txtDisSname.Value.Trim()))
        //{
        //    where += " and ShortName like'%" + txtDisSname.Value.Trim() + "%'";
        //}
        return where;
    }

    protected void btn_SearchClick(object sender, EventArgs e)
    {
        DataBinds();
    }

    public void Pager_PageChanged(object sender, EventArgs e)
    {
        DataBinds();
        page = Pager.CurrentPageIndex.ToString();
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DBUtility;
using System.Data;

public partial class Distributor_Rep_RepOrderList : DisPageBase
{
    public string page = "1";//默认初始页
    //Hi.Model.SYS_Users user = null;
    Hi.Model.BD_Distributor dis = null;
    public decimal ta = 0;
    public decimal tb = 0;
    public int pageCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);
        //dis = new Hi.BLL.BD_Distributor().GetModel(user.DisID);
        //if (!IsPostBack)
        //    user = LoginModel.IsLogined(this);
        //if (user != null)
        //{
        dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";

            if (Request.QueryString["type"] == null)
            {

                this.txtCreateDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
                this.txtCreateDate1.Value = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                if (Request.QueryString["type"] + "" == "1")
                {
                    this.txtCreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                    this.txtCreateDate1.Value = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            Common.ListComps(this.ddrComp, this.UserID.ToString(), this.CompID.ToString());
            ViewState["strwhere"] = Where();
            Bind();
        }
        //}
    }

    /// <summary>
    /// 绑定销售订单信息列表
    /// </summary>
    public void Bind()
    {
        int Counts = 0;
        string strwhere = string.Empty;
        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
        strwhere += "and Otype!=9 and isnull(dr,0)=0 and OState in(2,4,5,3,7) and disid=" + this.DisID + " and CompID=" + this.ddrComp.Value;

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
        //if (this.txtCreateDate.Value.Trim() == "" && this.txtCreateDate1.Value.Trim() == "")
        //{
        //    strwhere += " and CreateDate>='" + DateTime.Now.AddDays(1 - DateTime.Now.Day).ToShortDateString() + " 0:0:0' ";
        //}
        List<Hi.Model.DIS_Order> orders = new Hi.BLL.DIS_Order().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, strwhere, out pageCount, out Counts);
        //List<Hi.Model.DIS_Order> orders1 = new Hi.BLL.DIS_Order().GetList("", " isnull(dr,0)=0 and Otype!=9 and OState in(2,4,5,7,3) and disid=" + this.DisID + Where(), "");
        for (int i = 0; i < orders.Count; i++)
        {
            ta += orders[i].AuditAmount;
            tb += orders[i].PayedAmount;
        }
        this.rptOrder.DataSource = orders;
        this.rptOrder.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        ViewState["strwhere"] = Where();
        Bind();
    }
    public void A_Seek(object sender, EventArgs e)
    {
        ViewState["strwhere"] = Where();
        Bind();
    }
    public string Where()
    {
        string strwhere = string.Empty;
        if (!string.IsNullOrEmpty(orderid.Value.Trim()))
        {
            strwhere += " and receiptno like ('%" + Common.NoHTML(orderid.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (this.txtCreateDate.Value.Trim() != "")
        {
            strwhere += " and CreateDate>'" + this.txtCreateDate.Value.Trim().ToDateTime() + "'";
        }
        if (this.txtCreateDate1.Value.Trim() != "")
        {
            strwhere += " and CreateDate<'" + this.txtCreateDate1.Value.Trim().ToDateTime().AddDays(1) + "'";
        }
        if (ddrOState.Value != "-2")
        {
            strwhere += " and OState=" + Common.NoHTML(ddrOState.Value.Trim());
        }
        //if (ddrOState.Value == "9")
        //{
        //    strwhere += " and OState=" + (int)Enums.OrderState.已到货 + " and ReturnState not in (" + (int)Enums.ReturnState.未退货 + "," + (int)Enums.ReturnState.新增退货 + ")";
        //}
        //if (ddrOState.Value == "5")
        //{
        //    strwhere += " and OState=" + (int)Enums.OrderState.已到货 + " and ReturnState in (" + (int)Enums.ReturnState.未退货 + "," + (int)Enums.ReturnState.新增退货 + ")";
        //}
        return strwhere;
    }
    //public void rptOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    if (e.Item.ItemType == ListItemType.Footer)
    //    {
    //        Repeater rpt = (Repeater)sender;
    //        Hi.Model.DIS_Order ds = (Hi.Model.DIS_Order)rpt.DataSource;
    //        List<Hi.Model.DIS_Order> orders = new Hi.BLL.DIS_Order().GetList("", " isnull(dr,0)=0 and disid=" + user.DisID + Where(), "");
    //        for (int i = 0; i < orders.Count; i++)
    //        {
    //            ta += orders[i].AuditAmount;
    //            tb += orders[i].PayedAmount;
    //        }
    //        if (e.Item.FindControl("total1") != null)
    //        {
    //            Label tol1 = (Label)e.Item.FindControl("total1");
    //            tol1.Text = string.Format("{0}", ta.ToString("N"));
    //        }
    //        if (e.Item.FindControl("total2") != null)
    //        {
    //            Label tol2 = (Label)e.Item.FindControl("total2");
    //            tol2.Text = string.Format("{0}", tb.ToString("N"));
    //        }
    //    }
    //}
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

public partial class Distributor_Pay_orderZDList :DisPageBase
{
    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public string price = "0";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {           
            this.txtPager.Value = "12";
            if (Request["S"] == "1")
            {
                this.txtArriveDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
                this.txtArriveDate1.Value = DateTime.Now.ToString("yyyy-MM-dd");
                ViewState["strwhere"] = " and CreateDate>='" + Convert.ToDateTime(this.txtArriveDate.Value.Trim()) + "' and CreateDate<'" + Convert.ToDateTime(this.txtArriveDate1.Value.Trim()).AddDays(1) + "'";
            }
            Bind();
            AddTypeBind();
            OTypeBind();
        }
    }

    public void AddTypeBind()
    {
        ddlAddtype.Items.Add(new ListItem("全部", "-1"));
        foreach (Enums.AddType a in Enum.GetValues(typeof(Enums.AddType)))
        {
            ddlAddtype.Items.Add(new ListItem(a.ToString(), ((int)a).ToString()));
        }
    }

    public void OTypeBind()
    {
        ddlOtype.Items.Add(new ListItem("全部","-1"));
        foreach (Enums.OType a in Enum.GetValues(typeof(Enums.OType)))
        {
            ddlOtype.Items.Add(new ListItem(a.ToString(),((int)a).ToString()));
        }
    }

    /// <summary>
    /// 绑定销售订单信息列表
    /// </summary>
    public void Bind()
    {
        price = new Hi.BLL.PAY_PrePayment().sums(this.DisID, this.CompID).ToString("0.00");
        int pageCount = 0;
        int Counts = 0;
        //string ordStatr=this.ddrPayState.Value;//订单状态
        string strwhere = string.Empty;

        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
        //if (ordStatr!="-1")
        //{
        //    strwhere += " and PayState="+ordStatr;
        //}


        int disid =this.DisID;
        //该代理商的，没有删除标准的订单
        strwhere += " and Otype=9   and OState<>6   and DisID='" + disid + "' and ReturnState in(0,1) and isnull(dr,0)=0"; //IsDel=1  订单已删除
        
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

        List<Hi.Model.DIS_Order> l = new Hi.BLL.DIS_Order().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, strwhere, out pageCount, out Counts);

        this.rptOrder.DataSource = l;
        this.rptOrder.DataBind();

        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rptOrder_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strWhere = string.Empty;

        if (this.txtReceiptNo.Value.Trim() != "")
        {
            strWhere += "and ReceiptNo like '%" + Common.NoHTML(this.txtReceiptNo.Value.Trim().Replace("'", "''")) + "%'";
        }
        //if (this.ddrPayState.Value != "-1")
        //{
        //    strWhere += " and PayState=" + this.ddrPayState.Value;
        //}
        if (this.txtArriveDate.Value != "")
        {
            strWhere += " and CreateDate>='" + Convert.ToDateTime(this.txtArriveDate.Value.Trim()) + "'";
        }
        if (this.txtArriveDate1.Value != "")
        {
            strWhere += " and CreateDate<'" + Convert.ToDateTime(this.txtArriveDate1.Value.Trim()).AddDays(1) + "'";
        }
        if (ddlAddtype.Items.Count > 0)
        {
            if (ddlAddtype.SelectedValue != "-1")
            {
                strWhere += " and addtype='" + Common.NoHTML(ddlAddtype.SelectedValue) + "'";
            }
        }
        if (ddlOtype.Items.Count > 0)
        {
            if (ddlOtype.SelectedValue != "-1")
            {
                strWhere += " and Otype='" + Common.NoHTML(ddlOtype.SelectedValue) + "'";
            }
        }
        ViewState["strwhere"] = strWhere;
        Bind();
    }

    /// <summary>
    /// 根据不同的状态，显示不同的操作按钮
    /// </summary>
    /// <param name="state"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public string Getmessage(int state, int id)
    {
        string str = string.Empty;
        if (state == 0)
        {
            str = string.Format("<a href=\'javascript:void(0)\' onclick=\'pay(\"{0}\")\' class=\"a-red\">立即支付</a>", Common.DesEncrypt(id.ToString(), Common.EncryptKey));
        }
        else if (state == 1)
        {
            str = string.Format("<a href=\'javascript:void(0)\' onclick=\'pay(\"{0}\")\' class=\"a-red\">立即支付</a>", Common.DesEncrypt(id.ToString(), Common.EncryptKey));
        }
        else if (state == 2)
        {
            str = "已支付";
        }
        else if (state == 5)
        {
            str = "已退款";
        }
        //else if (state == 6)
        //{
        //    str = "已结算";
        //}

        return str;

    }
    /// <summary>
    /// 根据不同的状态，显示不同的操作按钮
    /// </summary>
    /// <param name="state"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public string Getmessage(object Ostate, string  paystate, object ReturnState, int id)
    {
        string str = string.Empty;
        string strDB = string.Empty;

        if (Ostate.ToString() == "2" || Ostate.ToString() == "4" || (Ostate.ToString() == "5" && (ReturnState.ToString() == "0" || ReturnState.ToString() == "1")))
        {           
            if (!paystate.Equals("2"))
            {
                if (Common.HasRight(this.CompID, this.UserID, "2213", this.DisID))
                {
                    str = string.Format("<a href='javascript:void(0)' onclick=\'pay(\"{0}\",\"{1}\")\' class=\"a-red\">立即支付</a>", Common.PaySetingsValue(this.CompID), Common.DesEncrypt(id.ToString(), Common.EncryptKey));
                }
            }


        }
        return str + strDB;
    }
}
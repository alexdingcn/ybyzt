using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Company_Pay_PayCorrectDetails : CompPageBase
{
    Hi.BLL.PAY_PrePayment PayMeauModel = new Hi.BLL.PAY_PrePayment();
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            Bind();
            //判断查询条件
           // DropListBind();
        }
    }


    //public void DropListBind()
    //{
    //    DataTable dt = new Hi.BLL.PAY_PrePayment().GetDate("id,disname", "BD_Distributor", "compid=" + this.CompID);
    //    int i = 1;
    //    foreach (DataRow dr in dt.Rows)
    //    {
    //        string disid = Convert.ToString(dr["id"]);
    //        string turename = Convert.ToString(dr["disname"]);
    //        this.ddldisname.Items.Insert(i, new ListItem(turename, disid));
    //        i++;
    //    }
    //}
    /// <summary>
    /// 查询录入明细
    /// </summary>
    public void Bind()
    {
        //查询录入明细
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;
        //查询全部手动录入冲正
        strwhere += " and PreType=3";
        //有效数据显示
       // strwhere += " and IsEnabled = 1";
        //审核状态是已审的
        //strwhere += " and AuditState = 2";
        //付款状态是成功的
        strwhere += " and Start=1";

        //所属企业
        strwhere += " and CompID=" + this.CompID;
        //int disId = Convert.ToInt32(Request.QueryString["keyId"]);
        //if (disId != 0)
        //{
        //    strwhere += " and DisID=" + disId + "";
        //}
        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
        if (this.txtPager.Value.Trim().ToString() != "")
        {
            Pager.PageSize = this.txtPager.Value.Trim().ToInt();
        }
        List<Hi.Model.PAY_PrePayment> pay = new Hi.BLL.PAY_PrePayment().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreatDate", true, strwhere, out pageCount, out Counts);

        this.rptPay.DataSource = pay;
        this.rptPay.DataBind();


        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }
    /// <summary>
    /// 代理商搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strWhere = string.Empty;

        if (this.txtPayCreateNo.Value != "")
        {
            strWhere += " and ID like '%" + this.txtPayCreateNo.Value.Trim().Replace("'", "") + "%'";
        }
        if (this.txtPayCreateDate.Value != "")
        {
            strWhere += " and Paytime >= '" + this.txtPayCreateDate.Value + "'";
        }

        if (this.txtPayEnddate.Value != "") 
        {
            strWhere += " and Convert(varchar(10),paytime,120) <='" + this.txtPayEnddate.Value + "'";
        }

        if (this.txtPayCreateDate.Value != "" && this.txtPayEnddate.Value != "")
        {
            strWhere += " and Convert(varchar(10),paytime,120) between '" + this.txtPayCreateDate.Value + "' and   '" + this.txtPayEnddate.Value + "'";
        }
        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            strWhere += " and disid  in (select id from BD_Distributor  where ISNULL(dr,0)=0 and CompID=" + this.CompID + "  and DisName like '%" + txtDisName.Value.Trim().Replace("'", "") + "%')";
            //strWhere += " and disid=" + this.DisListID.Disid;
        }

        ViewState["strwhere"] = strWhere;
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

    #region 审核按钮 --作废
    /// <summary>
    /// 企业钱包审核
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected object PayMeau(object Pid)
    //{
    //    string Str_Meau = "";

    //    Hi.Model.PAY_PrePayment PayModel = PayMeauModel.GetModel(Convert.ToInt32(Pid));

    //    if (PayModel != null)
    //    {
    //        if (PayModel.AuditState == (int)Enums.AuditState.已审)
    //        {
    //            Str_Meau += "已审";
    //        }
    //        else if (PayModel.AuditState == (int)Enums.AuditState.未审)
    //        {
    //            Str_Meau += "<asp:LinkButton ID='linkbtnShipping' class='tablelink' style='cursor: pointer;' runat='server' CommandName='ship' CommandArgument='" + PayModel.ID + "' DisIdCs='" + PayModel.DisID + "'>审核</asp:LinkButton>";

    //        }
    //    }

    //    return Str_Meau;

    //}

    #endregion
    /// <summary>
    /// 补录审核
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSh_Click(object sender, EventArgs e)
    {
        

    }
}
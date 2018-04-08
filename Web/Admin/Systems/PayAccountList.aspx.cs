using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.Formula.Functions;

public partial class Admin_Systems_PayAccountList : AdminPageBase
{
    public int Id = 0;  //订单Id
    Hi.BLL.PAY_PaymentBank PAbll = new Hi.BLL.PAY_PaymentBank();
    public string page = "1";//默认初始页

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            if (Request.QueryString["KeyID"] != "" || Request.QueryString["KeyID"] != "0")
            {
                KeyID = Convert.ToInt32(Request.QueryString["KeyID"]);

            }
            else
                KeyID = Convert.ToInt32(Key_compID.Value);

            Bind();
            
        }
    }

    /// <summary>
    /// 代理商列表显示
    /// </summary>
    public void Bind()
    {
        //查询代理商
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;
        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
        strwhere += " and CompID=" + KeyID + "  and isnull(dr,0)=0"; //IsDel=1  订单已删除
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

        List<Hi.Model.PAY_PaymentBank> PayAccount = new Hi.BLL.PAY_PaymentBank().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, strwhere, out pageCount, out Counts);
        this.rptPAcount.DataSource = PayAccount;
        this.rptPAcount.DataBind();

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

        string txtbankname = Common.NoHTML(this.txtbankname.Value.Trim().Replace("'", ""));
        if (txtbankname != "")
        {
            strWhere += " and bankid in ( select bankcode from PAY_BankInfo where BankName like '%" + txtbankname + "%' )";
        }


        ViewState["strwhere"] = strWhere;
        Bind();
    }

    //删除一行记录
    //modify  by ggh
    //
    protected void rptPAcount_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string type = e.CommandName;
        Id = Convert.ToInt32(e.CommandArgument);

        if (type == "del")
        {
            Hi.Model.PAY_PaymentBank PAmodel = PAbll.GetModel(Id);

            if (PAmodel != null)
            {

                PAmodel.dr = 1;
                bool falg = PAbll.Update(PAmodel);
                if (falg)
                {
                    falg = new Hi.BLL.PAY_PaymentAccountdtl().DeldtlBYpbID(Id);
                    Bind();
                }

            }
        }
    }

    /// <summary>
    /// 分页控件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();

        string strWhere = string.Empty;

        string txtbankname = Common.NoHTML(this.txtbankname.Value.Trim().Replace("'", ""));
        if (txtbankname != "")
        {
            strWhere += " and bankid in ( select bankcode from PAY_BankInfo where BankName like '%" + txtbankname + "%' )";
        }

        ViewState["strwhere"] = strWhere;


        Bind();
    }
   
    /// <summary>
    /// 获取选中
    /// </summary>
    /// <returns></returns>
    public string CB_SelAll()
    {
        string strId = string.Empty;

        foreach (RepeaterItem item in rptPAcount.Items)
        {
            CheckBox cb = item.FindControl("CB_SelItem") as CheckBox;
            if (cb != null && cb.Checked == true)
            {
                HiddenField fld = item.FindControl("Hd_Id") as HiddenField;

                if (fld != null)
                {
                    int id = Convert.ToInt32(fld.Value);
                    strId += id + ",";
                }
            }
        }
        if (strId != "")
        {
            strId = strId.Substring(0, strId.Length - 1);
        }
        return strId;
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnVolumeDel_Click(object sender, EventArgs e)
    {
        string Id = CB_SelAll();
        string msg = string.Empty;


        if (Id != "")
        {
            string[] strarry = Id.Split(',');
            List<Hi.Model.PAY_PaymentBank> paymentList = new List<Hi.Model.PAY_PaymentBank>();
            foreach (string str in strarry)
            {
                //删除关联的代理商
                bool result = new Hi.BLL.PAY_PaymentAccountdtl().DeldtlBYpbID(Convert.ToInt32(str));


                Hi.Model.PAY_PaymentBank paymentmodel = PAbll.GetModel(Convert.ToInt32(str));
                paymentmodel.dr = 1;
                paymentList.Add(paymentmodel);

            }
            PAbll.Update(paymentList);
            // JScript.ShowAlert(this, "操作成功！");
            Bind();
           
        }
    }
}
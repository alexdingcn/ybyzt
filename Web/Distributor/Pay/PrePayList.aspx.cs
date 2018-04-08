using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class Distributor_Pay_PrePayList :DisPageBase
{
    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public string price = string.Empty;//企业钱包余额

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            Bind();

            //判断是否修改支付密码
            PayPwd();

            if (!Common.HasRight(this.CompID, this.UserID, "2216", this.DisID))
                this.remittanceAdd.Visible = false;
        }
    }

    public string GetStr(string str)
    {
        if (str.Length == 0)
        {
            str = "无";
        }
        else if (str.Length > 5)
        {
            str = str.Substring(0, 4) + "...";
        }
        return str;
    }

    /// <summary>
    /// 绑定销售订单信息列表
    /// </summary>
    public void Bind()
    {
        price = new Hi.BLL.PAY_PrePayment().sums(this.DisID,this.CompID).ToString("0.00");
        //查询录入明细
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;
        //查询全部手动录入冲正
        // strwhere += " and PreType=3";
        //有效数据显示
        // strwhere += " and IsEnabled = 1";
        //审核状态是已审的
        //strwhere += " and AuditState = 2";
        //付款状态是成功的
        strwhere += " and Start=1";

        //所属代理商
        strwhere += "  and PreType in (1,2,3,4,5,6,7,8,9)  and DisID=" + this.DisID;
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
        List<Hi.Model.PAY_PrePayment> pay = new Hi.BLL.PAY_PrePayment().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreatDate", true, strwhere, out pageCount, out Counts);

        this.rptOrder.DataSource = pay;
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

        string IDNO = Common.NoHTML(this.txtReceiptNo.Value.Trim().ToString().Replace("'", "''"));
        string ddrAuditState = "-1";// this.ddrAuditState.Value;
        string ddrPayState = Common.NoHTML(this.ddrPayState.Value);
        string ddrPayType = Common.NoHTML(this.ddrPayType.Value);
        if (IDNO != "")
        {
            strWhere += " and ID like '%" + IDNO + "%'";
        }
        if (ddrAuditState != "-1")
        {
            strWhere += " and AuditState =" + ddrAuditState;
        }
        if (ddrPayState != "-1")
        {
            strWhere += " and Start =" + ddrPayState;
        }
        if (ddrPayType != "-1")
        {
            strWhere += " and PreType =" + ddrPayType;
        }
        if (this.txtArriveDate.Value != "")
        {
            strWhere += " and Paytime>='" + Convert.ToDateTime(this.txtArriveDate.Value.Trim()) + "'";
        }
        if (this.txtArriveDate1.Value != "")
        {
            strWhere += " and Paytime<'" + Convert.ToDateTime(this.txtArriveDate1.Value.Trim()).AddDays(1) + "'";
        }
        if (this.txtCrateUser.Value != "")
        {
            string UserIds = GetUserIDs(this.DisID,this.CompID,this.txtCrateUser.Value);
            strWhere += " and CrateUser in(" + UserIds + ")";
        }
        ViewState["strwhere"] = strWhere;
        Bind();
    }

    /// <summary>
    /// 获取所有可能制单的制单人ID
    /// </summary>
    /// <param name="DisID"></param>
    /// <param name="ComID"></param>
    /// <param name="Name"></param>
    /// <returns></returns>
    public string GetUserIDs(int DisID,int ComID,string Name)
    {
        string Ids = "";
        string sql = "select ID from SYS_Users where DisID=" + DisID + " and TrueName like '%" + Common.NoHTML(this.txtCrateUser.Value) + "%' and AuditState=2 and isnull(dr,0)=0 union all select ID from SYS_Users where DisID=0 and CompID=" + ComID + " and TrueName like '%" + Common.NoHTML(this.txtCrateUser.Value) + "%' and AuditState=2 and isnull(dr,0)=0";
        DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer,sql);
        foreach (DataRow r in ds.Tables[0].Rows)
        {
            Ids += r["ID"]+",";
        }
        if (Ids.Length > 0)
        {
            Ids = Ids.Substring(0, Ids.Length - 1);
        }
        else
        {
            Ids = "0";
        }
        return Ids;
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
            str = string.Format("<a href=\'javascript:void(0)\' onclick=\'pay({0})\' class=\"btnBl\">支付</a>", id);
        }
        else if (state ==2)
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

    public void PayPwd()
    {
        if (IsDisAdmin(this.UserID))
        {
            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
            if (dis.Paypwd == Util.md5("123456"))
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "", "<script>layerCommon.alert('经核查，目前您的支付密码还未修改，为了您的账户安全，请修改支付密码！', IconOption.笑脸);</script>");
                paypwd.Attributes.Add("value", Util.md5(dis.Paypwd));
                divpaypwd.Attributes.Add("style", "display:block");
                zzc.Attributes.Add("style", "display:block");
                return;
            }
        }
    }

    //修改支付密码
    protected void A_Save(object sender, EventArgs e)
    {
        if (paypwd1.Value == "123456")
        {
            ClientScript.RegisterStartupScript(GetType(), "", "<script>layerCommon.alert('新支付密码不能为默认密码', IconOption.错误);</script>");
            return;
        }
        if (paypwd1.Value == paypwd2.Value)
        {
            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
            dis.Paypwd = Util.md5(paypwd1.Value);
            dis.ts = DateTime.Now;
            dis.modifyuser = this.UserID;
            if (new Hi.BLL.BD_Distributor().Update(dis))
            {
                spanpaypwd.Attributes.CssStyle.Value = "display:none;";
                ClientScript.RegisterStartupScript(GetType(), "", "<script>layerCommon.alert('恭喜您，您的支付密码已经修改成功！', IconOption.笑脸);</script>");
                divpaypwd.Attributes.Add("style", "display:none");
                zzc.Attributes.Add("style", "display:none");
                return;
            }
            else
            {
                spanpaypwd.Attributes.CssStyle.Value = "display:inline-block;color:Red;";
                spanpaypwd.InnerText = "修改失败请重试！";
                return;
            }
        }
        else
        {
            spanpaypwd.Attributes.CssStyle.Value = "display:inline-block;color:Red;";
            spanpaypwd.InnerText = "两次输入的密码不一致！";
            return;
        }
    }


    //判断是否为企业管理员登录
    public bool IsDisAdmin(int ID)
    {
        string sql = "select id from Sys_Compuser where isnull(dr,0)=0 and utype=5 and userid=" + ID;
        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (dt != null && dt.Rows.Count > 0)
            return true;
        else
            return false;
    }


}
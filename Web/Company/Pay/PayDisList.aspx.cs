using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_Pay_PayDisList : CompPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string CmopId= CompID.ToString();
            //txtDisArea.CompID = CmopId;
            this.txtPager.Value = "12";
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
        string IDlist = string.Empty;//销售经理下属 员工ID集合
        if (DisSalesManID != 0)
        {
            if (Common.GetDisSalesManType(DisSalesManID.ToString(), out IDlist))
            {
                //销售经理
                strwhere = "and ID in(select ID from BD_Distributor where smid in(" + IDlist + "))";
            }
            else
            {
                strwhere = "and ID in(select ID from BD_Distributor where smid = " + DisSalesManID + ")";
            }
        }

        strwhere += " and dr=0 and AuditState=2 and CompID =" + CompID;

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

      

        List<Hi.Model.BD_Distributor> dis = new Hi.BLL.BD_Distributor().GetList(Pager.PageSize, Pager.CurrentPageIndex, "shortindex", true, strwhere, out pageCount, out Counts);

        this.rptDis.DataSource = dis;
        this.rptDis.DataBind();

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

        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            strWhere += " and  DisName like '%" +Common.NoHTML( txtDisName.Value.Trim().Replace("'","")) + "%'";
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
    /// <summary>
    /// 新建补录企业钱包
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //定义变量
        string txtPayCreateDis = string.Empty;
        string txtPayCreatePrice = string.Empty;
        //string txtPayCreateDate = string.Empty;
        string txtPayCreateType = string.Empty;
        string txtPayCreateRemark = string.Empty;
        //获取输入的值
        txtPayCreateDis = this.txtPayCreateDis.Value.Trim().ToString();
        txtPayCreatePrice = this.txtPayCreatePrice.Value.Trim().ToString();
        //txtPayCreateDate = this.txtPayCreateDate.Value.Trim().ToString();
        txtPayCreateType = this.txtPayCreateType.Value.Trim().ToString();
        txtPayCreateRemark = this.txtPayCreateRemark.Value.Trim().ToString();
        //调用model,对属性进行赋值
        Hi.Model.PAY_PrePayment prepaymentmodel = new Hi.Model.PAY_PrePayment();
        prepaymentmodel.CompID = 1;
        prepaymentmodel.DisID = Convert.ToInt32(txtPayCreateDis);
        prepaymentmodel.Start = 1;
        prepaymentmodel.PreType = Convert.ToInt32(txtPayCreateType);
        prepaymentmodel.price = Convert.ToDecimal(txtPayCreatePrice);
        prepaymentmodel.Paytime = DateTime.Now;
        prepaymentmodel.CreatDate = DateTime.Now;
        prepaymentmodel.CrateUser = 1;
        prepaymentmodel.AuditState = 0;
        prepaymentmodel.IsEnabled = 1;
        prepaymentmodel.ts = DateTime.Now;
        prepaymentmodel.dr = 1;
        prepaymentmodel.modifyuser = 1;
        prepaymentmodel.vdef1 = txtPayCreateRemark;
        //调用保存方法
        Hi.BLL.PAY_PrePayment prepaymentsave = new Hi.BLL.PAY_PrePayment();
        int reslut = prepaymentsave.Add(prepaymentmodel);
        //判断返回值int
        if (reslut > 0)
        {
            //sum代理商全部补录，冲正金额
            //decimal sums = prepaymentsave.sums(txtPayCreateDis);
            //修改代理商的企业钱包金额
            //调用model,对属性进行赋值
            //Hi.Model.BD_Distributor dismodel = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(txtPayCreateDis));
            //dismodel.DisAccount = sums;
            //调用修改方法    
            //Hi.BLL.BD_Distributor disupdate = new Hi.BLL.BD_Distributor();
            //bool disup = disupdate.Update(dismodel);

            JScript.AlertMethod(this, "补录成功！", JScript.IconOption.正确, "function (){ location.replace('" + ("PayCreateList.aspx") + "'); }");

            
        }
        else
        {
            JScript.AlertMethod(this, "补录失败！", JScript.IconOption.错误, "function (){ location.replace('" + ("PayCreateList.aspx") + "'); }");
        }





    }
}
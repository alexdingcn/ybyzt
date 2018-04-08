using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

public partial class Admin_Systems_OrderList : AdminPageBase
{
    Hi.BLL.DIS_Order OrderInfoBLL = new Hi.BLL.DIS_Order();
    List<Hi.Model.SYS_Users> userList = null; //制单人列表

    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        string Action = Request["Action"] + "";
        string OrgID = Request["OrgID"] + "";
        if (Action == "Action")
        {
            Response.Write(Common.getsaleman(OrgID));
            Response.End();
        }
        if (!IsPostBack)
        {
            Common.BindOrgSale(Org, SaleMan, "全部");
            this.txtPager.Value = Common.PageSize;
            ViewState["strwhere"] = Where();
            Bind();
        }
    }
    /// <summary>
    /// 绑定销售订单信息列表
    /// </summary>
    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strwhere = " and isnull(dr,0)=0 ";
        //if (SalesManID != null && SalesManID != 0)
        //{
        //    strwhere += " and CompID in (select ID from [dbo].[BD_Company] where orgid=" + OrgID + " and SalesManID=" + SalesManID + " and isnull(dr,0)=0)";
        //}

        if (SalesManID > 0 || OrgID > 0)
        {
            string whereIn = string.Empty;
            if (OrgID > 0)
            {
                whereIn += "  and OrgID=" + OrgID + "";
            }
            if (SalesManID > 0)
            {
                whereIn += " and SalesManID=" + SalesManID + "";
            }
            strwhere += " and CompID in (select ID from [dbo].[BD_Company] where  isnull(dr,0)=0 "+ whereIn + ")";
        }
        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
        strwhere += "and Otype!=9 and isnull(dr,0)=0"; //IsDel=1  订单已删除

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

        List<Hi.Model.DIS_Order> l = OrderInfoBLL.GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, strwhere, out pageCount, out Counts);

        this.rptOrder.DataSource = l;

        //查询制单人列表
        List<int> ids = l.Select(T => T.CreateUserID).Distinct().ToList();
        if (ids != null && ids.Count > 0)
        {
            userList = new Hi.BLL.SYS_Users().GetList("TrueName,ID", " ID in(" + string.Join(",", ids) + ") ", "");
        }

        this.rptOrder.DataBind();

        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
        this.Org.SelectedValue = (this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString());
    }
    /// <summary>
    /// 分页控件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        ViewState["strwhere"] = Where();
        Bind();
    }
    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["strwhere"] = Where();
        Bind();
    }
    protected string Where()
    {
        string strWhere = string.Empty;
        if (this.txtReceiptNo.Value.Trim() != "")
        {
            strWhere += "and ReceiptNo like '%" + Common.NoHTML(this.txtReceiptNo.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (ddrOState.Value != "-2")
        {
            if (this.ddrOState.Value == "33")
            {
                strWhere += " and OState=5 and ReturnState=" + (int)Enums.ReturnState.申请退货;
            }
            else if (this.ddrOState.Value == "71")
            {
                strWhere += "and OState=5 and ReturnState=" + (int)Enums.ReturnState.拒绝退货;
            }
            else
            {
                strWhere += " and OState=" + this.ddrOState.Value + "and ReturnState in (" + (int)Enums.ReturnState.未退货 + "," + (int)Enums.ReturnState.新增退货 + ")";
            }
        }
        if (this.ddrPayState.Value.Trim() != "-1")
        {
            strWhere += " and PayState=" + Common.NoHTML(this.ddrPayState.Value.Trim());
        }
        if (this.ddrAddType.Value.Trim() != "-1")
        {
            strWhere += " and AddType=" + Common.NoHTML(this.ddrAddType.Value.Trim());
        }
        //if (this.ddrOtype.Value.Trim() != "-1")
        //{
        //    strWhere += " and Otype=" + this.ddrOtype.Value.Trim();
        //}
        if (this.txtTotalAmount1.Value.Trim() != "")
        {
            strWhere += " and TotalAmount>=" + Common.NoHTML(this.txtTotalAmount1.Value.Trim());
        }
        if (this.txtTotalAmount2.Value.Trim() != "")
        {
            strWhere += " and TotalAmount<" + Common.NoHTML(this.txtTotalAmount2.Value.Trim());
        }
        if (this.txtCreateDate.Value.Trim() != "")
        {
            strWhere += " and CreateDate>'" + this.txtCreateDate.Value.Trim().ToDateTime() +  "'";
        }
        if (this.txtEndCreateDate.Value.Trim() != "")
        {
            strWhere += " and CreateDate<='" + this.txtEndCreateDate.Value.Trim().ToDateTime().AddDays(1) + "'";
        }
        if (!string.IsNullOrEmpty(txtCompName.Value.Trim()))
        {
            strWhere += " and exists(select 1 from BD_Company  where id=CompID and  CompName like '%" + Common.NoHTML(txtCompName.Value.Trim()) + "%')";
        }
        if (Org.SelectedValue != "-1")
        {
            string org = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
            strWhere += " and exists(select 1 from BD_Company  where id=CompID and OrgID='" + Common.NoHTML(org) + "')";
        }
        if (salemanid.Value != "-1" && salemanid.Value != "")
        {
            strWhere += " and exists(select 1 from BD_Company  where id=CompID and SalesManID='" + Common.NoHTML(salemanid.Value) + "')";
        }
        return strWhere;      
    }

    public string getUserTName(int uID)
    {
        if (userList != null)
        {
            List<Hi.Model.SYS_Users> findUser = userList.Where(T => T.ID == uID).ToList();
            if (findUser.Count > 0)
            {
                return findUser[0].TrueName;
            }
        }

        return string.Empty;
    }

}
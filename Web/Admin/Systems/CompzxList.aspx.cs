using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class Admin_Systems_CompzxList : AdminPageBase
{
    public string page = "1";//默认初始页
    public string type = "1";
    protected void Page_Load(object sender, EventArgs e)
    {
        string Action = Request["Action"] + "";
        string OrgID =Common.NoHTML( Request["OrgID"]) + "";
        if (Action == "Action")
        {
            Response.Write(Common.getsaleman(OrgID));
            Response.End();
        }
        if (!IsPostBack)
        {
            Common.BindOrgSale(Org, SaleMan,"全部");
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            if (Request["type"] != null)
            {
                type = Request["type"] + "";
            }
            Common.BindIndDDL(txtIndusName);
            
            DataBinds();
        }
    }


    public void DataBinds()
    {
        try
        {
            int pageCount = 0;
            int Counts = 0;
            if (this.txtPageSize.Value.ToString() != "")
            {
                if (this.txtPageSize.Value.Trim().Length >= 5)
                {
                    Pager.PageSize = 100;
                    this.txtPageSize.Value = "100";
                }
                else
                {
                    Pager.PageSize = this.txtPageSize.Value.Trim().ToInt(0);
                }
            }
            List<Hi.Model.BD_Company> LDis = new Hi.BLL.BD_Company().GetList(Pager.PageSize, Pager.CurrentPageIndex, "Createdate", true, SearchWhere(), out pageCount, out Counts);
            this.Rpt_Comp.DataSource = LDis;
            this.Rpt_Comp.DataBind();
            Pager.RecordCount = Counts;
            page = Pager.CurrentPageIndex.ToString();
            this.Org.SelectedValue = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
        }
        catch (Exception ex)
        {
            Tiannuo.LogHelper.LogHelper.Error("Error", ex);
        }
    }


    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string where = "  and isnull(dr,0)=0 and AuditState=2 ";
        if (OrgID > 0)
        {
            where += "  and OrgID=" + OrgID + "";
        }
        if (SalesManID > 0)
        {
            if (Request["type"] != null && Request["type"] != "1")
            {
                //判断是否存在装修审核权限,不加业务员ID查询
                string sql = "select rf.* from SYS_RoleSysFun rf join SYS_AdminUser u on u.RoleID=rf.RoleID where rf.FunCode='3215' and u.ID=" + UserID;
                DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
                if (dt != null && dt.Rows.Count < 0)
                {
                    where += " and SalesManID=" + SalesManID + "";
                }
            }
            else
            {
                where += " and SalesManID=" + SalesManID + "";
            }
        }
        if ("1".Equals(type))
        {
            //业务员确认装修
            where += " and isnull(IsZXAudit,0)<>1";
        }
        else
        {
            //业务员确认装修,装修待审核的厂商
            where += " and isnull(IsOrgZX,0)=1 and isnull(IsZXAudit,0)=0";
        }

        if (!string.IsNullOrEmpty(txtCompName.Value.Trim()))
        {
            where += " and (CompCode like '%" + Common.NoHTML(txtCompName.Value.Trim().Replace("'", "''")) + "%' or CompName like '%" + Common.NoHTML(txtCompName.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (this.chk_ywy.Checked==true)
        {
            where += "and SalesManID=0";
        }
        if (!string.IsNullOrEmpty(txtPhone.Value.Trim()))
        {
            where += " and Phone like'%" + Common.NoHTML(txtPhone.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (!string.IsNullOrEmpty(txtMinfo.Value.Trim()))
        {
            where += " and ManageInfo like'%" + Common.NoHTML(txtMinfo.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (txtIndusName.SelectedValue!="-1")
        {
            where += " and IndID ='" + txtIndusName.SelectedValue + "'";
        }
        if (this.txtCreateDate.Value.Trim() != "")
        {
            where += " and CreateDate>='" + Convert.ToDateTime(this.txtCreateDate.Value.Trim()) + "'";
        }
        if (this.txtEndCreateDate.Value.Trim() != "")
        {
            where += " and CreateDate<'" + Convert.ToDateTime(this.txtEndCreateDate.Value.Trim()).AddDays(1) + "'";
        }
        if (this.ddrOtype.Value != "-1")
        {
            where += " and IsEnabled=" + this.ddrOtype.Value;
        }
        if (Org.SelectedValue != "-1")
        {
            string org = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
            where += " and OrgID='" + org + "'";
        }
        if (salemanid.Value != "-1" && salemanid.Value != "")
        {
            where += " and SalesManID='" + salemanid.Value + "'";
        }
        if (Request["IsFist"] == "Show")
        {
            where += " and FirstShow=1 and Auditstate=2  and isenabled=1 ";
        }
        if (!string.IsNullOrWhiteSpace(hidProvince.Value.Trim()) && hidProvince.Value.Trim() != "选择省")
            where += " and CompAddr like'%" + hidProvince.Value.Trim() + "%'";
        if (!string.IsNullOrWhiteSpace(hidCity.Value.Trim()) && hidCity.Value.Trim() != "选择市")
            where += " and CompAddr like'%" + hidCity.Value.Trim() + "%'";
        if (!string.IsNullOrWhiteSpace(hidArea.Value.Trim()) && hidArea.Value.Trim() != "选择区")
            where += " and CompAddr like'%" + hidArea.Value.Trim() + "%'";

        //add by hgh  是否显示
        if (drpSYXS.SelectedValue != "")
        {
            if (drpSYXS.SelectedValue == "2")
                where += " and FirstShow='2'";
            else if (drpSYXS.SelectedValue == "1")
                where += " and FirstShow='1'";
            else if (drpSYXS.SelectedValue == "0")
                where += " and FirstShow='0'";
        }
        if (!string.IsNullOrEmpty(Capitalcrea.Value.Trim()))
        {
            where += " and Capital >='" + Capitalcrea.Value.Trim() + "'";
        }
        if (!string.IsNullOrEmpty(Capitalend.Value.Trim()))
        {
            where += " and Capital <='" + Capitalend.Value.Trim() + "'";
        }
        if (CompType.SelectedValue!="0")
        {
            where += " and CompType ='" + CompType.SelectedValue + "'";
        }

        if (this.txtCreateDatesh.Value.Trim() != "")
        {
            where += " and AuditDate>='" + Convert.ToDateTime(this.txtCreateDatesh.Value.Trim()) + "'";
        }
        if (this.txtEndCreateDatesh.Value.Trim() != "")
        {
            where += " and AuditDate<'" + Convert.ToDateTime(this.txtEndCreateDatesh.Value.Trim()).AddDays(1) + "'";
        }

        if (this.txtCreateDatezx.Value.Trim() != "")
        {
            where += " and ZXAuditDate>='" + Convert.ToDateTime(this.txtCreateDatezx.Value.Trim()) + "'";
        }
        if (this.txtEndCreateDatezx.Value.Trim() != "")
        {
            where += " and ZXAuditDate<'" + Convert.ToDateTime(this.txtEndCreateDatezx.Value.Trim()).AddDays(1) + "'";
        }
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

    public string GetName(object id)
    {
        Hi.Model.SYS_Industry type = new Hi.BLL.SYS_Industry().GetModel(Convert.ToInt16(id));
        if (type != null)
        {
            return type.InduName;
        }
        return "";
    }


    /// <summary>
    /// 根据业务员id找出对应的业务员
    /// </summary>
    /// <param name="id">业务员id</param>
    /// <returns></returns>
    public string GetSalesName(object id)
    {
        string str = "";
        int SalesManID = Convert.ToInt32(id);
        if (SalesManID != 0)
        {
            str = Common.GetSaleManValue(SalesManID, "SalesName") == null ? "" : Common.GetSaleManValue(SalesManID, "SalesName").ToString();
        }
        return str;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Company_UserControl_SelectDisList3 : System.Web.UI.Page
{
    
    public string page = "1";//默认初始页
    string Compid
    {
        get { return Request["compid"] + ""; }
    }

    string strDISID
    {
        get { return Request["disid"] + ""; }
    }

    string KeyID
    {
        get { return Request["keyid"] + ""; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>$(\".txt_txtTypename,.txt_txtAreaname\").css(\"width\", \"170px\");</script>");
        
        if (!IsPostBack)
        {
            DataBinds();
        }
    }

    public void DataBinds()
    {
        List<Hi.Model.BD_Distributor> LDis = new Hi.BLL.BD_Distributor().GetList("", SearchWhere(), "Createdate desc");
        this.Rpt_Dis.DataSource = LDis;
        this.Rpt_Dis.DataBind();
        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>$(function(){     $(\"#CB_SelAll\").trigger(\"click\");})</script>");
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string where = "";
        if (string.IsNullOrEmpty(strDISID))
        {
            where = "auditstate=2 and isnull(IsEnabled,0)=1  and isnull(dr,0)=0 and (SMID is null or SMID = 0) and CompID=" + Compid.ToInt(0) + "";
        }
        else
        {
            where = "auditstate=2 and isnull(IsEnabled,0)=1 and id not in (" + strDISID + ") and isnull(dr,0)=0 and (SMID is null or SMID = 0 or SMID=" + KeyID + ") and CompID=" + Compid.ToInt(0) + "";
        }
        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            where += " and ( DisName like '%" + txtDisName.Value.Trim() + "%')";
        }
        if (!string.IsNullOrEmpty(txtname.Value.Trim()))
        {
            where += " and (principal like '%" + txtname.Value.Trim() + "%')";
        }
        if (!string.IsNullOrEmpty(txtphone.Value.Trim()))
        {
            where += " and (phone like '%" + txtphone.Value.Trim() + "%')";
        }
        return where;
    }

    protected void btn_SearchClick(object sender, EventArgs e)
    {
        DataBinds();
    }

    protected void Btn_seve(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hiddisid.Value))
        {
            string strsql = "update BD_Distributor set SMID=" + KeyID + " where id in (" + hiddisid.Value + ")";
            SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strsql);
            ClientScript.RegisterClientScriptBlock(ClientScript.GetType(), "msg", "<script>btnhidparent('" + hiddisid.Value + "');</script>");
        }
        else
        {
            JScript.AlertMsgOne(this, "请先勾选需要添加的代理商！", JScript.IconOption.错误,2500);
        }
    }
}
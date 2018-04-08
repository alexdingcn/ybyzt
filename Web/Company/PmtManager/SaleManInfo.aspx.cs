using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Admin_OrgManage_SaleManInfo : CompPageBase
{

   public string SMtype = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        hidCompId.Value = CompID.ToString();
        if (!IsPostBack)
        {
            DataBinds();
            if (KeyID != 0)
            {
                List<Hi.Model.BD_Distributor> Ldis = new Hi.BLL.BD_Distributor().GetList("", "auditstate=2 and isnull(IsEnabled,0)=1  and isnull(dr,0)=0 and SMID = " + KeyID + " and CompID=" + CompID, "");
                if (Ldis.Count > 0)
                {
                    tbdis.Attributes["style"] = "display:block;";
                }
                else
                {
                    tbdis.Attributes["style"] = "display:none;";
                }
                Rpt_Dis.DataSource = Ldis;
                Rpt_Dis.DataBind();
                for (int i = 0; i < Ldis.Count; i++)
                {
                    hidselectDis.Value += Ldis[i].ID + ",";
                }
                hidselectDis.Value = hidselectDis.Value.TrimEnd(',');
            }
        }
    }

    protected void Btn_Dis(object sender, EventArgs e)
    {
        List<Hi.Model.BD_Distributor> Ldis = new Hi.BLL.BD_Distributor().GetList("", "isnull(dr,0)=0 and id in (" + hidselectDis.Value + ")", "");
        if (Ldis.Count > 0)
        {
            tbdis.Attributes["style"] = "display:block;";
        }
        else
        {
            tbdis.Attributes["style"] = "display:none;";
        }
        Rpt_Dis.DataSource = Ldis;
        Rpt_Dis.DataBind();


    }

    protected void A_Del(object sender, EventArgs e)
    {
        if (hidselectDis.Value.IndexOf(',') < 0)
        {
            hidselectDis.Value = "0";
        }
        string strdis = hidselectDis.Value.Replace((hiddel.Value + ","), "");
        if (strdis.Length == hidselectDis.Value.Length)
        {
            strdis = hidselectDis.Value.Replace("," + hiddel.Value, "");
        }
        string strsql = "update BD_Distributor set SMID=0 where id=" + hiddel.Value;
        SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strsql);
        hidselectDis.Value = strdis;
        Btn_Dis(null, null);
    }


    public void DataBinds()
    {
        Hi.Model.BD_DisSalesMan sale = new Hi.BLL.BD_DisSalesMan().GetModel(KeyID);
        if (sale != null)
        {
            if (sale.CompID != CompID)
            {
                Response.Write("你无权限访问。");
                Response.End();
            }
            if (sale.ParentID > 0)
            {
                lblSMParent.InnerText = Common.GetDisSMValue(sale.ParentID, "SalesName").ToString();
            }
            SMtype = sale.SalesType.ToString();
            lblSMType.InnerText = Enum.GetName(typeof(Enums.DisSMType), sale.SalesType);
            lblSaleName.InnerText = sale.SalesName;
            lblSaleCode.InnerText = sale.SalesCode;
            lblPhone.InnerText = sale.Phone;
            lblEmail.InnerText = sale.Email;
            lblRemark.InnerText = sale.Remark;
            lblstate.InnerHtml = sale.IsEnabled == 1 ? "启用" : "<i style='color:red'>禁用</i>";
            if (sale.IsEnabled == 0)
            {
                btnbd.Visible = false;
            }
        }
        else
        {
            Response.Write("业务员不存在。");
            Response.End();
        }
    }

    protected void btn_Del(object sender, EventArgs e)
    {
        Hi.Model.BD_DisSalesMan sale = new Hi.BLL.BD_DisSalesMan().GetModel(KeyID);
        if (sale != null)
        {
            sale.dr = 1;
            sale.ts = DateTime.Now;
            sale.modifyuser = UserID;
            if (new Hi.BLL.BD_DisSalesMan().Update(sale))
            {
                //JScript.AlertMsgMo(this, "操作成功", "function(){ window.location.href='SaleManList.aspx'; }");
                Response.Redirect("SaleManList.aspx");
            }
        }
    }

    public string GetOrgName(int OrgID)
    {
        Hi.Model.BD_Org org = new Hi.BLL.BD_Org().GetModel(OrgID);
        if (org != null)
        {
            return org.OrgName;
        }
        else
        {
            return "";
        }
    }
}
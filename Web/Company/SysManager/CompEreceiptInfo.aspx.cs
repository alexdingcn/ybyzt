using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_SysManager_CompEreceiptInfo : CompPageBase
{
    public List<Hi.Model.BD_Ereceipt> erl = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            Bind();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    protected void Bind()
    {
        erl = new Hi.BLL.BD_Ereceipt().GetList("", " CompID=" + this.CompID + " and isnull(dr,0)=0", "");

        if (erl != null && erl.Count > 0)
        {
            foreach (Hi.Model.BD_Ereceipt item in erl)
            {
                this.lblereceipt_batchno.InnerText = item.ereceipt_batchno.ToString();
                //this.lblereceipt_brd.InnerText = item.ereceipt_brd.ToString();
                //this.lblereceipt_chkbill.InnerText = item.ereceipt_chkbill.ToString();
                this.lblereceipt_duedate.InnerText = item.ereceipt_duedate.ToString("yyyy-MM-dd");
                //this.lblereceipt_gds.InnerText = item.ereceipt_gds.ToString();
                //this.lblereceipt_gdsdic.InnerText = item.ereceipt_gdsdic.ToString();
                //this.lblereceipt_grd.InnerText = item.ereceipt_grd.ToString();
                //this.lblereceipt_hder.InnerText = item.ereceipt_hder.ToString();
                //this.lblereceipt_kd.InnerText = item.ereceipt_kd.ToString();
                this.lblereceipt_mfters.InnerText = item.ereceipt_mfters.ToString();
                //this.lblereceipt_nm.InnerText = item.ereceipt_nm.ToString();
                //this.lblereceipt_num.InnerText = item.ereceipt_num.ToString();
                //this.lblereceipt_price.InnerText = item.ereceipt_price.ToString("0.00");
                //this.lblereceipt_rtbill.InnerText = item.ereceipt_rtbill.ToString();
                //this.lblereceipt_sgndt.InnerText = item.ereceipt_sgndt.ToString("yyyy-MM-dd");
                this.lblereceipt_std.InnerText = item.ereceipt_std.ToString();
                //this.lblereceipt_unit.InnerText = item.ereceipt_unit.ToString();
                //this.lblereceipt_value.InnerText = item.ereceipt_value.ToString();
                this.lblereceipt_whnm.InnerText = item.ereceipt_whnm.ToString();
                this.lblereceipt_whno.InnerText = item.ereceipt_whno.ToString();
            }
        }
    }
}
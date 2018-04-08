using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Distributor_newOrder_addrinfo : System.Web.UI.Page
{
    //代理商ID
    public int DisID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["DisID"]))
                DisID = Request["DisID"].ToString().ToInt(0);

            this.hidDisID.Value = DisID.ToString();
            datatbind();
        }
    }

    public void datatbind()
    {
        DataTable dt = new Hi.BLL.BD_DisAddr().GetList("addr.[ID],addr.[principal],addr.[phone],addr.[Province],addr.[City],addr.[Area],addr.[Address],addr.[isdefault],dis.phone as disPhone", "BD_DisAddr as addr left join BD_Distributor as dis on addr.DisID=dis.ID", " addr.DisID=" + this.DisID + " and addr.dr=0", "addr.isdefault desc");

        if (dt != null && dt.Rows.Count > 0)
        {
            this.rpt_addr.DataSource = dt;
            this.rpt_addr.DataBind();
        }
    }
}
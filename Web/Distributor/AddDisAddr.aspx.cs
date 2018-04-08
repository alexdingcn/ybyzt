using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class AddDisAddr : DisPageBase
{
    public Hi.Model.SYS_Users user = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        user = new Hi.BLL.SYS_Users().GetModel(this.UserID);
        //if (user != null)
        //{
            if (!IsPostBack)
            {
                this.hidKeyID.Value = Request["KeyID"] + "";
                datatbind();
            }
        //}
    }

    public void datatbind()
    {
        DataTable dt = new Hi.BLL.BD_DisAddr().GetList("addr.[ID],addr.[principal],addr.[phone],addr.[Province],addr.[City],addr.[Area],addr.[Address],addr.[isdefault],dis.phone as disPhone", "BD_DisAddr as addr left join BD_Distributor as dis on addr.DisID=dis.ID", " addr.DisID=" + this.DisID + " and addr.dr=0", "addr.isdefault desc");

        if (dt != null && dt.Rows.Count > 0)
        {
            this.rpt_Addr.DataSource = dt;
            this.rpt_Addr.DataBind();

            this.lblDisPhone.InnerText = dt.Rows[0]["disPhone"].ToString();
        }
    }
}
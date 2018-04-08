using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_Order_DeliveryAdd : DisPageBase
{
    //public Hi.Model.SYS_Users user = null;
    public Hi.Model.BD_Distributor dis = null;
    public string disphone = string.Empty;
    public string Phone = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);
        //string DisId = Request["disId"] + "";
        this.hidID.Value =this.DisID.ToString();
        Phone = new Hi.BLL.SYS_Users().GetModel(this.UserID).Phone;
        dis = new Hi.BLL.BD_Distributor().GetModel(DisID);
        if (dis != null)
        {
            disphone = dis.Phone;
        }
        else
        {
            disphone = "该代理商不存在";
        }

        Bind(DisID.ToString());
    }

    public void Bind(string Id)
    {
        Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(Id.ToInt(0));

        if (disModel != null)
        {
            this.txtuserphone.Value = Phone;
            this.txtusername.Value = disModel.Principal;
        }
    }
}
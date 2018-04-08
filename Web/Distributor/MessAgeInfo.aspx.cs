

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_MessAgeInfo : DisPageBase
{
    public Hi.Model.DIS_Suggest suggest = null;
    public Hi.Model.SYS_Users compuser = null;
    //public Hi.Model.SYS_Users user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);
        string MessAgeID = Request["id"].ToString();
        if (!string.IsNullOrEmpty(MessAgeID))
        {
            if (!Common.PageDisOperable("Message", Convert.ToInt32(MessAgeID), this.DisID))
            {
                Response.Redirect("~/NoOperable.aspx");
                return;
            }
            suggest = new Hi.BLL.DIS_Suggest().GetModel(int.Parse(MessAgeID));
            if (suggest.IsAnswer != 0)
            {
                compuser = new Hi.BLL.SYS_Users().GetModel(suggest.CompUserID);
                if (suggest.IsAnswer == 1)
                {
                    suggest.IsAnswer = 2;
                    suggest.modifyuser =this.DisID;
                    suggest.ts = DateTime.Now;
                    new Hi.BLL.DIS_Suggest().Update(suggest);
                }
            }
            else
            {
                reply.Visible = false;
            }
        }
    }
}
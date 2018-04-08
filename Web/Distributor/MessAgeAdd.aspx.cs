

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_MessAgeAdd : DisPageBase
{
    //public Hi.Model.SYS_Users user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);
    }
    protected void AddMessAge(object sender, EventArgs e)
    {
        Hi.Model.DIS_Suggest suggest = new Hi.Model.DIS_Suggest();
        Hi.Model.BD_Distributor dis=new Hi.BLL.BD_Distributor().GetModel(DisID);
        if (dis != null)
        {
            suggest.Title = Common.NoHTML(txtTitle.Value);
            suggest.DisUserID = this.UserID;
            suggest.Remark = Common.NoHTML(txtRemark.Value);
            suggest.CreateDate = DateTime.Now;
            suggest.CompID = this.CompID;
            suggest.Stype = 0;
            suggest.DisID = this.DisID;
            suggest.IsAnswer = 0;
            suggest.ts = DateTime.Now;
            suggest.dr = 0;
            suggest.modifyuser = this.UserID;
            if (new Hi.BLL.DIS_Suggest().Add(suggest) > 0)
            {
                Response.Redirect("MessAgeList.aspx");
            }
            else
            {
                JScript.AlertMsgOne(this, "添加失败！", JScript.IconOption.错误);
            }
        }
    }
}
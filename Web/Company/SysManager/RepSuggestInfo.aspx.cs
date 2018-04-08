using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_SysManager_RepSuggestInfo : CompPageBase
{
    Hi.BLL.DIS_Suggest OrderBll = new Hi.BLL.DIS_Suggest();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && Request.QueryString["ID"] != "")
        {
            Bind();
        }
    }

    /// <summary>
    /// 绑定
    /// </summary>
    public void Bind()
    {
        Hi.Model.DIS_Suggest model = OrderBll.GetModel(int.Parse(Request.QueryString["ID"].ToString()));
        
        this.lblDisName.InnerText = Common.GetDis(model.DisID,"DisName");
        this.lblCreateUser.InnerText = Common.GetUserName(model.DisUserID);
        this.lblState.InnerText = model.IsAnswer == 1 ? "已回复" : "未回复";
        this.lblCreateDate.InnerText = model.CreateDate.ToString("yyyy-MM-dd");
        this.lblTitle.InnerText = model.Title;
        this.lblContent.InnerText = model.Remark;
        this.lblCompRemark.InnerText = model.CompRemark;
        this.lblCompUser.InnerText = Common.GetUserName(model.CompUserID);
        this.lblReplyDate.InnerText = model.ReplyDate.ToString("yyyy-MM-dd");

        if (model.IsAnswer == 0)
        {
            this.CompRemark.Visible = false;
            this.lblCompUser.Visible = false;
            this.CompRepk.Visible = false;
        }
        else
        {
            this.libtnEdit.Attributes.Add("style", "display:none;");
        }
    }
}
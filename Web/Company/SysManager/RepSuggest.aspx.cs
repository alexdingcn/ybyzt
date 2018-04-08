using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_SysManager_RepSuggest : CompPageBase
{
    Hi.BLL.DIS_Suggest OrderBll = new Hi.BLL.DIS_Suggest();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && Request.QueryString["ID"] != "")
        {
           Binds();
        }
    }

    /// <summary>
    /// 绑定
    /// </summary>
    public void Binds()
    {
        Hi.Model.DIS_Suggest OrderModel = OrderBll.GetModel(int.Parse(Request.QueryString["ID"].ToString()));
        this.lblAuditUserID.InnerText = Common.GetUserName(this.UserID);
        this.hidAuditUserID.Value = this.UserID.ToString();
        this.lblContent.InnerText = OrderModel.Remark;
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Hi.Model.DIS_Suggest OrderModel = OrderBll.GetModel(int.Parse(Request.QueryString["ID"].ToString()));
        string AuditRemark = string.Empty;

        if (!string.IsNullOrEmpty(this.txtRemark.Value.Trim().ToString()))
        {
            AuditRemark =Common.NoHTML( this.txtRemark.Value.Trim().ToString());
        }
        else
        {
            JScript.AlertMsgOne(this, "回复内容不能为空！", JScript.IconOption.错误);
            return;
        }

        if (OrderModel != null)
        {
            if (OrderModel.IsAnswer == 0)
            {
                OrderModel.IsAnswer = 1;
                OrderModel.CompUserID = this.UserID;
                OrderModel.ReplyDate = DateTime.Now;
                OrderModel.CompRemark = AuditRemark;
                OrderModel.ts = DateTime.Now;
                OrderModel.modifyuser = this.UserID;

                if (OrderBll.Update(OrderModel))
                {
                    Response.Write("<script language=\"javascript\">window.parent.Suggest(" + OrderModel.ID + ");</script>");
                }
            }
        }
        else
        {
            JScript.AlertMsgOne(this, "数据不存在！", JScript.IconOption.错误);

        }
    }

}
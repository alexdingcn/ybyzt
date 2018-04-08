using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Systems_MessageInfo : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBinds();
        }
    }

    public void DataBinds()
    {
        if (KeyID != 0)
        {
            Hi.Model.SYS_UserMessage Message = new Hi.BLL.SYS_UserMessage().GetModel(KeyID);

            this.lblUserName.InnerText = Message.UserName;
            this.lblUserPhone.InnerText = Message.UserPhone;
            this.lblUserMailQQ.InnerText = Message.UserMailQQ;
            this.lblUserMessge.InnerText = Message.UserMessge;
            this.lblCreateDate.InnerText = Convert.ToDateTime(Message.CreateDate).ToString("yyyy-MM-dd HH:mm");

            //处理
            string strState = "";
            if (Message.State == 1)
            {
                strState = "已处理";
                this.tr1.Visible = true;
                this.tr2.Visible = false;
                this.txtRemark.Text = Message.Remark;
                this.lblModify.InnerText = Convert.ToDateTime(Message.ModifyDate).ToString("yyyy-MM-dd HH:mm");
            }
            else
            {
                strState = "未处理";
                this.tr2.Visible = true;
                this.tr1.Visible = false;
            }
            this.lblState.InnerText = strState;
        }
        else
        {
            Response.Write("信息不存在。");
            Response.End();
        }
    }


    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtRemark.Text.Trim()))
        {
            JScript.AlertMsg(this, "处理结果不能为空!");
            return;
        }

        Hi.Model.SYS_UserMessage Message = null;

        if (KeyID != 0)
        {
            Message = new Hi.BLL.SYS_UserMessage().GetModel(KeyID);
            Message.State = 1;
            Message.ModifyDate = DateTime.Now;
            Message.ModifyUser = UserID;
            Message.Remark = Common.NoHTML(txtRemark.Text.Trim());

            if (new Hi.BLL.SYS_UserMessage().Update(Message))
            {
                Response.Redirect("MessageList.aspx");
            }
        }
        
    }


    protected void btn_Del(object sender, EventArgs e)
    {
        Hi.Model.SYS_UserMessage Message = new Hi.BLL.SYS_UserMessage().GetModel(KeyID);
        if (Message != null)
        {
            Message.dr = 1;
            if (new Hi.BLL.SYS_UserMessage().Update(Message))
            {
                Response.Redirect("MessageList.aspx");
            }
        }
    }
}
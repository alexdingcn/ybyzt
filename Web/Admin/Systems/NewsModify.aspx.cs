using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Systems_NewsModify : AdminPageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Databind();
        }
    }

    public void Databind()
    {
        if (KeyID != 0)
        {
            Hi.Model.SYS_NewsNotice NewsNotice = new Hi.BLL.SYS_NewsNotice().GetModel(KeyID);
            try
            {
                txtNewsTitle.Value = NewsNotice.NewsTitle;
                this.textNewsInfo.Text= NewsNotice.NewsInfo;
                this.textKeywords.Value = NewsNotice.KeyWords;
                content7.Text = NewsNotice.NewsContents;
               
                //是否启用
                int status = NewsNotice.IsEnabled;
                this.rdoStatus1.Checked = (status != 1);
                this.rdoStatus0.Checked = (status == 1);

                //是否置顶
                int istop = NewsNotice.IsTop;
                this.Radio4.Checked = (istop != 1);
                this.Radio3.Checked = (istop == 1);

                //类型   1：新闻 2：公告  3：资讯
                int newstype = NewsNotice.NewsType;
                this.Radio1.Checked = (newstype == 1);
                this.Radio2.Checked = (newstype == 2);
                this.Radio5.Checked = (newstype == 3);
                this.Radio6.Checked = (newstype == 4);

            }
            catch (Exception ex)
            {

            }
        }


    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty(Common.NoHTML( txtNewsTitle.Value.Trim())))
        {
            JScript.AlertMsg(this, "消息标题不能为空!");
            return;
        }

        Hi.Model.SYS_NewsNotice NewsNotice = null;

        if (KeyID != 0)
        {
            NewsNotice = new Hi.BLL.SYS_NewsNotice().GetModel(KeyID);
            NewsNotice.NewsTitle = Common.NoHTML(txtNewsTitle.Value.Trim());//去除非法字符
            NewsNotice.NewsInfo = Common.NoHTML(textNewsInfo.Text.Trim());
            NewsNotice.KeyWords = Common.NoHTML(textKeywords.Value.Trim());
            NewsNotice.NewsContents = content7.Text.Trim();
            //是否启用
            if (this.rdoStatus1.Checked)
                NewsNotice.IsEnabled = 0;
            else
                NewsNotice.IsEnabled = 1;
            //是否置顶
            if (this.Radio3.Checked)
                NewsNotice.IsTop = 1;
            else
                NewsNotice.IsTop = 0;
            //类别
            if (this.Radio1.Checked)
            {
                NewsNotice.NewsType = 1;
            }
            else if (this.Radio2.Checked)
            {
                NewsNotice.NewsType = 2;
            }
            else if (this.Radio5.Checked)
            {
                NewsNotice.NewsType = 3;
            }
            else
            {
                NewsNotice.NewsType = 4;
            }
            NewsNotice.ts = DateTime.Now;
            NewsNotice.modifyuser = UserID;
            if (new Hi.BLL.SYS_NewsNotice().Update(NewsNotice))
            {
                //JScript.AlertMsgMo(this, "操作成功", "function(){ window.location.href='NewsInfo.aspx?KeyID=" + KeyID + "'; }");
                Response.Redirect("NewsInfo.aspx?KeyID=" + KeyID);
            }
        }
        else
        {
            NewsNotice = new Hi.Model.SYS_NewsNotice();
            NewsNotice.NewsTitle = Common.NoHTML(txtNewsTitle.Value.Trim());//去除非法字符
            NewsNotice.NewsInfo = Common.NoHTML(textNewsInfo.Text.Trim());
            NewsNotice.KeyWords = Common.NoHTML(textKeywords.Value.Trim());
            NewsNotice.NewsContents = content7.Text.Trim();
            //是否启用
            if (this.rdoStatus1.Checked)
                NewsNotice.IsEnabled = 0;
            else
                NewsNotice.IsEnabled = 1;
            //是否置顶
            if (this.Radio3.Checked)
                NewsNotice.IsTop = 1;
            else
                NewsNotice.IsTop = 0;
            //类别
            if (this.Radio1.Checked)
            {
                NewsNotice.NewsType = 1;
            }
            else if (this.Radio2.Checked)
            {
                NewsNotice.NewsType = 2;
            }
            else if (this.Radio5.Checked)
            {
                NewsNotice.NewsType = 3;
            }
            else
            {
                NewsNotice.NewsType = 4;
            }

            //标准参数
            NewsNotice.CreateDate = DateTime.Now;
            NewsNotice.CreateUserID = UserID;
            NewsNotice.ts = DateTime.Now;
            NewsNotice.modifyuser = UserID;
            int newsrid = 0;
            newsrid = new Hi.BLL.SYS_NewsNotice().Add(NewsNotice);
            if (newsrid > 0)
            {
                Response.Redirect("NewsInfo.aspx?KeyID=" + newsrid);
            }
        }
    }

}
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebReference;

public partial class NewsInfo : CompPageBase
{
    public string title = "";
    public string content = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBinds();
        }
    }

    public void DataBinds()
    {
        if (KeyID > 0)
        {
            Hi.Model.BD_CompNews NewsNotice = new Hi.BLL.BD_CompNews().GetModel(KeyID);
            if (NewsNotice.CompID!=this.CompID)
            {
                Response.Write("你无权限访问。");
                Response.End();
            }
            if (NewsNotice != null)
            {
                if (Request["Type"] == "3") {
                    libtnEdit.Visible = false;
                    libtnDel.Visible = false;
                    libtnback.Visible = false;
                    liPreView.Visible = false;
                }
                //lblnewstitle.InnerText = NewsNotice.NewsTitle;
                title = NewsNotice.NewsTitle;
                //lblcontent.InnerText = NewsNotice.NewsContents;
                string pat = "<[^<>]+>";
                Regex regex = new Regex(pat, RegexOptions.None);
                content = NewsNotice.NewsContents.Replace("<span>", "<p>").Replace("<span", "<p").Replace("</span>", "</p>");
                lblstate.InnerHtml = NewsNotice.IsEnabled == 1 ? "是" : "<i style='color:red'>否</i>";
                lblistop.InnerHtml = NewsNotice.IsTop == 1 ? "置顶" : "非置顶";
                lblnewtype.InnerHtml = Common.GetCPNewStateName(NewsNotice.NewsType);
                string[] showType = NewsNotice.ShowType.Split(new char[]{','});
                foreach (string s in showType)
                {
                    if (s == "1")
                    {
                        lblShowType.InnerText = "New";
                    }
                    if (s == "2")
                    {
                        if (lblShowType.InnerText == "")
                        {
                            lblShowType.InnerText = "标红";
                        }
                        else
                        {
                            lblShowType.InnerText += "，标红";
                        }
                    }
                }
            }
            else
            {
                Response.Write("信息不存在。");
                Response.End();
            }
        }
    }

    protected void btn_Del(object sender, EventArgs e)
    {
        Hi.Model.BD_CompNews NewsNotice = new Hi.BLL.BD_CompNews().GetModel(KeyID);
        if (NewsNotice != null)
        {
            NewsNotice.dr = 1;
            NewsNotice.ts = DateTime.Now;
            NewsNotice.modifyuser = UserID;
            if (new Hi.BLL.BD_CompNews().Update(NewsNotice))
            {
                WebReference.AppService app = new AppService();
                try
                {
                    app.MsgPush(NewsNotice.ID.ToString(), "-1");
                }
                catch
                {
                    app.Abort();
                }

                if (Request["Type"] == "3")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Result", "<script>window.close();</script>");
                }
                else
                {
                    //JScript.AlertMsgMo(this, "操作成功", "function(){ window.location.href='NewsList.aspx'; }");
                    Response.Redirect("NewsList.aspx");
                }
            }
        }
    }
}
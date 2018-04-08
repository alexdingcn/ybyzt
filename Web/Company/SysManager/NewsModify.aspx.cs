using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebReference;

public partial class NewsModify : CompPageBase
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
        if (KeyID > 0)
        {
            try
            {
                Hi.Model.BD_CompNews NewsNotice = new Hi.BLL.BD_CompNews().GetModel(KeyID);
                if (NewsNotice != null)
                {
                    if (NewsNotice.CompID != CompID)
                    {
                        Response.Write("你无权访问。");
                        Response.End();
                    }
                    txtNewsTitle.Value = NewsNotice.NewsTitle;
                    content7.Text = NewsNotice.NewsContents;
                    //是否启用
                    int status = NewsNotice.IsEnabled;
                    this.RdIsbled1.Checked = (status == 1);
                    this.RdIsbled2.Checked = (status != 1);

                    string[] ShowType = NewsNotice.ShowType.Split(new char[] { ',' });
                    foreach (string s in ShowType)
                    {
                        switch (s)
                        {
                            case "1": CkShowType1.Checked = true; break;
                            case "2": CkShowType2.Checked = true; break;
                        }
                    }

                    //是否置顶
                    int istop = NewsNotice.IsTop;
                    this.RdTop3.Checked = (istop == 1);
                    this.RdTop4.Checked = (istop != 1);

                    //类型   1：新闻 2：公告 3：活动
                    int newstype = NewsNotice.NewsType;
                    this.RdType1.Checked = (newstype == 1);
                    this.RdType2.Checked = (newstype == 2);
                    this.RdType3.Checked = (newstype == 3);
                    this.RdType4.Checked = (newstype == 4);
                    this.RdType5.Checked = (newstype == 5);
                }
                else
                {
                    Response.Write("信息不存在。");
                    Response.End();
                }

            }
            catch (Exception ex)
            {

            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtNewsTitle.Value.Trim()))
        {
            JScript.AlertMsgOne(this, "消息标题不能为空！", JScript.IconOption.错误);
            return;
        }

        Hi.Model.BD_CompNews NewsNotice = null;

        if (KeyID != 0)
        {
            NewsNotice = new Hi.BLL.BD_CompNews().GetModel(KeyID);
            NewsNotice.NewsTitle = Common.NoHTML(txtNewsTitle.Value.Trim());
            NewsNotice.NewsContents = content7.Text.Trim();
            //是否启用
            if (this.RdIsbled1.Checked)
                NewsNotice.IsEnabled = 1;
            else
                NewsNotice.IsEnabled = 0;
            //是否置顶
            if (this.RdTop3.Checked)
                NewsNotice.IsTop = 1;
            else
                NewsNotice.IsTop = 0;
            //类别
            if (this.RdType1.Checked)
            {
                NewsNotice.NewsType = 1;
            }
            else if (this.RdType2.Checked)
            {
                NewsNotice.NewsType = 2;
            }
            else if (this.RdType3.Checked)
            {
                NewsNotice.NewsType = 3;
            }
            else if (this.RdType4.Checked)
            {
                NewsNotice.NewsType = 4;
            }
            else if (this.RdType5.Checked)
            {
                NewsNotice.NewsType = 5;
            }
            string ShowType = "";
            if (CkShowType1.Checked)
            {
                ShowType = "1";
            }
            if (CkShowType2.Checked)
            {
                if (ShowType == "")
                {
                    ShowType = "2";
                }
                else
                {
                    ShowType += "," + "2";
                }
            }
            NewsNotice.ShowType = ShowType;
            NewsNotice.ts = DateTime.Now;
            NewsNotice.modifyuser = UserID;
            if (new Hi.BLL.BD_CompNews().Update(NewsNotice))
            {
                if (this.RdTop3.Checked)
                {
                    List<Hi.Model.BD_CompNews> CompNew = new Hi.BLL.BD_CompNews().GetList("", " isnull(dr,0)=0 and Compid=" + CompID + " and IsEnabled=1 and istop=1", " CreateDate desc");
                    if (CompNew.Count > 3)
                    {
                        for (int i = 0; i < CompNew.Count; i++)
                        {
                            if (i > 1 && CompNew[i].ID != NewsNotice.ID)
                            {
                                CompNew[i].IsTop = 0;
                                new Hi.BLL.BD_CompNews().Update(CompNew[i]);
                            }
                        }
                    }
                }
                    //JScript.AlertMsgMo(this, "操作成功", "function(){ window.location.href='NewsInfo.aspx?KeyID=" + KeyID + "'; }");
                Response.Redirect("NewsInfo.aspx?KeyID=" + KeyID);
            }
        }
        else
        {
            NewsNotice = new Hi.Model.BD_CompNews();
            NewsNotice.CompID = CompID;
            NewsNotice.NewsTitle = Common.NoHTML(txtNewsTitle.Value.Trim());
            NewsNotice.NewsContents = content7.Text.Trim();
            //是否启用
            if (this.RdIsbled1.Checked)
                NewsNotice.IsEnabled = 1;
            else
                NewsNotice.IsEnabled = 0;
            //是否置顶
            if (this.RdTop3.Checked)
                NewsNotice.IsTop = 1;
            else
                NewsNotice.IsTop = 0;
            //类别
            if (this.RdType1.Checked)
            {
                NewsNotice.NewsType = 1;
            }
            else if (this.RdType2.Checked)
            {
                NewsNotice.NewsType = 2;
            }
            else if (this.RdType3.Checked)
            {
                NewsNotice.NewsType = 3;
            }
            else if (this.RdType4.Checked)
            {
                NewsNotice.NewsType = 4;
            }
            else if (this.RdType5.Checked)
            {
                NewsNotice.NewsType = 5;
            }
            string ShowType = "";
            if (CkShowType1.Checked)
            {
                ShowType = "1";
            }
            if (CkShowType2.Checked)
            {
                if (ShowType == "")
                {
                    ShowType = "2";
                }
                else
                {
                    ShowType += "," + "2";
                }
            }
            NewsNotice.ShowType = ShowType;
            //标准参数
            NewsNotice.CreateDate = DateTime.Now;
            NewsNotice.CreateUserID = UserID;
            NewsNotice.ts = DateTime.Now;
            NewsNotice.modifyuser = UserID;
            int newsrid = 0;
            newsrid = new Hi.BLL.BD_CompNews().Add(NewsNotice);
            if (newsrid > 0)
            {
                WebReference.AppService app = new AppService();
                try
                {
                    app.MsgPush(newsrid.ToString(), "1");
                }
                catch
                {
                    app.Abort();
                }

                if (this.RdTop3.Checked)
                {
                    List<Hi.Model.BD_CompNews> CompNew = new Hi.BLL.BD_CompNews().GetList("", " isnull(dr,0)=0 and Compid=" + CompID + " and IsEnabled=1 and istop=1 and PMID=0", " createdate desc");
                    if (CompNew.Count > 3)
                    {
                        for (int i = 0; i < CompNew.Count; i++)
                        {
                            if (i > 1 && CompNew[i].ID != newsrid)
                            {
                                CompNew[i].IsTop = 0;
                                new Hi.BLL.BD_CompNews().Update(CompNew[i]);
                            }
                        }
                    }
                }
                    Response.Redirect("NewsInfo.aspx?KeyID=" + newsrid);
            }
        }
    }

}
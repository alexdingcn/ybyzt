using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_SysManager_NewsPreview : CompPageBase
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
        List<Hi.Model.BD_CompNews> CompNewS = new Hi.BLL.BD_CompNews().GetList("", " isnull(dr,0)=0 and CompID=" + CompID + " and ID='" + KeyID + "'", "");
        if (CompNewS.Count > 0)
        {
            Hi.Model.BD_CompNews CompNew = CompNewS[0];
            lblNewTitle.InnerHtml = CompNew.NewsTitle + IsEnd(CompNew.PmID);
            lblNewContent.InnerHtml = CompNew.NewsContents;
            lblCreateDate.InnerText = CompNew.CreateDate.ToString("yyy-MM-dd HH:mm") + " 【" + Common.GetCPNewStateName(CompNew.NewsType) + "】" + (CompNew.IsTop == 1 ? "【置顶】" : "");
        }
        else
        {
            Response.Write("数据不存在");
            Response.End();
        }
    }

    public string IsEnd(int PMID)
    {
        string str = "";
        if (PMID > 0)
        {
            Hi.Model.BD_Promotion ProM = new Hi.BLL.BD_Promotion().GetModel(PMID);
            if (ProM != null)
            {
                if (ProM.ProEndTime.Date < DateTime.Now.Date.Date)
                {
                    str = "<i  style='margin-left:5px;color: #aaaaaa;font-style: normal;'>【已结束】</i>";
                }
            }
        }
        return str;
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_CompNewInfo : DisPageBase
{
    //Hi.Model.SYS_Users user = null;
    //public int KeyID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);
        if (!IsPostBack)
        {
            //if (Request["KeyID"] != null)
            //{
            //    string Id = Common.DesDecrypt(Request["KeyID"].ToString(), Common.EncryptKey);
            //    KeyID = Id.ToInt(0);
            //}
            Bind();
        }
    }

    public void Bind()
    {
        Hi.Model.BD_CompNews CompNew = new Hi.BLL.BD_CompNews().GetModel(KeyID);
        if (CompNew != null)
        {
            if (CompNew.CompID != this.CompID)
            {
                Response.Write("数据不存在");
                Response.End();
            }
            lblNewTitle.InnerHtml = CompNew.NewsTitle + IsEnd(CompNew.PmID);
            lblCreateDate.InnerText = CompNew.CreateDate.ToString("yyy-MM-dd HH:mm") + " 【"+Common.GetCPNewStateName(CompNew.NewsType)+"】" +(CompNew.IsTop == 1 ? "【置顶】" : "");
            lblNewContent.InnerHtml = CompNew.NewsContents;
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
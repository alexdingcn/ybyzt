using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.Services;
using System.Web.Script.Serialization;

public partial class Company_CMerchants_authMvg : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
      
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void bind()
    {
        if (KeyID != 0)
        {
            List<Hi.Model.YZT_Annex> annlist = new Hi.BLL.YZT_Annex().GetList("", " fcID=" + this.KeyID + " and type=2 and fileAlias='2' ", "");

            if (annlist != null && annlist.Count > 0)
            {
                string linkFile = string.Empty;
                if (annlist[0].fileName.LastIndexOf("_") != -1)
                {
                    string text = annlist[0].fileName.Substring(0, annlist[0].fileName.LastIndexOf("_")) + Path.GetExtension(annlist[0].fileName);
                    if (text.Length < 15)
                        linkFile = text;
                    else
                    {
                        linkFile = text.Substring(0, 15) + "...";
                    }
                   
                }
                else
                {
                    string text = annlist[0].fileName.Substring(0, annlist[0].fileName.LastIndexOf("-")) + Path.GetExtension(annlist[0].fileName);
                    if (text.Length < 15)
                        linkFile = text;
                    else
                    {
                        linkFile = text.Substring(0, 15) + "...";
                    }
                }

                string UpText = string.Format("<dl class=\"teamList\"><dd><a style=\"cursor: pointer;\" class=\"bt\" title=\"" + annlist[0].fileName + "\" href=\"../../UploadFile/" + annlist[0].fileName + "\" download=" + annlist[0].fileName + " target=\"_blank\">" + linkFile + "</a><a class=\"red\" onclick=\"Cancel()\">删除</a></dd></dl>");
                this.UpFileText.InnerHtml = UpText;
                this.HidFfileName.Value = annlist[0].fileName;
            }
        }
    }

    [WebMethod]
    public static string Edit(string KeyID, string HidFfileName,string UserID)
    {
        Common.ResultMessage Msg = new Common.ResultMessage();
        Hi.Model.YZT_FirstCamp fcmodel = new Hi.BLL.YZT_FirstCamp().GetModel(KeyID.ToInt(0));

        if (fcmodel != null)
        {
            //医疗器械经营许可证
            Hi.Model.YZT_Annex annexModel = new Hi.Model.YZT_Annex();
            annexModel.fcID = fcmodel.ID;
            annexModel.type = 2;
            annexModel.fileName = HidFfileName;
            annexModel.fileAlias = "2";
            annexModel.validDate = DateTime.MinValue;
            annexModel.CreateDate = DateTime.Now;
            annexModel.dr = 0;
            annexModel.ts = DateTime.Now;
            annexModel.modifyuser = UserID.ToInt(0);
            annexModel.CreateUserID = UserID.ToInt(0);

            if (new Hi.BLL.YZT_Annex().Add(annexModel) > 0)
            {
                Msg.result = true;
            }
        }
        else
        {
            Msg.code = "未查找到数据";
        }
        return new JavaScriptSerializer().Serialize(Msg);
    }
}
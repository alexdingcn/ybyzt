using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.Services;
using System.Web.Script.Serialization;

public partial class Distributor_CMerchants_FirstCampEdit: DisPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
        DataBindLink();
    }

    

    /// <summary>
    /// 招商信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void bind()
    {
        if (KeyID != 0)
        {
            Hi.Model.YZT_FirstCamp fcmodel = new Hi.BLL.YZT_FirstCamp().GetModel(KeyID);

            if (fcmodel != null)
            {
                //需要上传的资料
                string ProvideData = new Hi.BLL.YZT_CMerchants().GetModel(fcmodel.CMID).ProvideData;
                this.hidProvideData.Value = ProvideData;
                hidcompID.Value = fcmodel.CompID.ToString();

                this.txtHtname.Value = new Hi.BLL.SYS_Hospital().GetModel(fcmodel.HtID).HospitalName;
                this.txtInvalidDate.Value = fcmodel.InvalidDate == DateTime.MinValue ? "" : fcmodel.InvalidDate.ToString("yyyy-MM-dd");
                this.txtForceDate.Value = fcmodel.ForceDate == DateTime.MinValue ? "" : fcmodel.ForceDate.ToString("yyyy-MM-dd");
                this.txtApplyRemark.Value = fcmodel.Applyremark;
            }
        }
    }


    /// <summary>
    /// 附件绑定
    /// </summary>
    public void DataBindLink()
    {
        //查询需要提供的资料
        List<Hi.Model.YZT_Annex> annexlist = new Hi.BLL.YZT_Annex().GetList("", " fcID=" + this.KeyID + " and fileAlias in (1) and dr=0", "");
        if (annexlist != null && annexlist.Count > 0)
        {
            foreach (Hi.Model.YZT_Annex item in annexlist)
            {
                if (!string.IsNullOrEmpty(item.fileName))
                {
                    string linkFile = string.Empty;
                    if (item.fileName.LastIndexOf("_") != -1)
                    {
                        string text = item.fileName.Substring(0, item.fileName.LastIndexOf("_")) + Path.GetExtension(item.fileName);
                        if (text.Length < 15)
                            linkFile = text;
                        else
                        {
                            linkFile = text.Substring(0, 15) + "...";
                        }
                      
                    }
                    else
                    {
                        string text = item.fileName.Substring(0, item.fileName.LastIndexOf("-")) + Path.GetExtension(item.fileName);
                        if (text.Length < 15)
                            linkFile = text;
                        else
                        {
                            linkFile = text.Substring(0, 15) + "...";
                        }
                    }

                    if (item.fileAlias == "1" && item.type == 5)
                    {//营业执照
                        this.UpFile1.Visible = true;
                        this.txtvalidDate1.Value = item.validDate == DateTime.MinValue ? "" : item.validDate.ToString("yyyy-MM-dd");
                        UpFileText1.InnerHtml = "<dl class=\"teamList\"><dd><a style=\"cursor: pointer;\" class=\"bt\" href=\"../../UploadFile/" + item.fileName + "\" download=" + item.fileName + " target=\"_blank\">" + linkFile + "</a><a class=\"red\" onclick=\"Cancel(1)\">删除</a></dd></dl>";
                        this.HidFfileName1.Value = item.fileName;
                    }
                    else if (item.fileAlias == "1" && item.type == 7)
                    {//医疗器械经营许可证
                        this.UpFile2.Visible = true;
                        UpFileText2.InnerHtml = "<dl class=\"teamList\"><dd><a style=\"cursor: pointer;\" class=\"bt\" href=\"../../UploadFile/" + item.fileName + "\" download=" + item.fileName + " target=\"_blank\">" + linkFile + "</a><a class=\"red\" onclick=\"Cancel(2)\">删除</a></dd></dl>";
                        this.txtvalidDate2.Value = item.validDate == DateTime.MinValue ? "" : item.validDate.ToString("yyyy-MM-dd");
                        this.HidFfileName2.Value = item.fileName;
                    }
                    else if (item.fileAlias == "1" && item.type == 9)
                    {//开户许可证
                        this.UpFile3.Visible = true;
                        UpFileText3.InnerHtml = "<dl class=\"teamList\"><dd><a style=\"cursor: pointer;\" class=\"bt\" href=\"../../UploadFile/" + item.fileName + "\" download=" + item.fileName + " target=\"_blank\">" + linkFile + "</a><a class=\"red\" onclick=\"Cancel(3)\">删除</a></dd></dl>";
                        this.txtvalidDate3.Value = item.validDate == DateTime.MinValue ? "" : item.validDate.ToString("yyyy-MM-dd");
                        this.HidFfileName3.Value = item.fileName;
                    }
                    else if (item.fileAlias == "1" && item.type == 8)
                    {//医疗器械备案
                        this.UpFile4.Visible = true;
                        UpFileText4.InnerHtml = "<dl class=\"teamList\"><dd><a style=\"cursor: pointer;\" class=\"bt\" href=\"../../UploadFile/" + item.fileName + "\" download=" + item.fileName + " target=\"_blank\">" + linkFile + "</a><a class=\"red\" onclick=\"Cancel(4)\">删除</a></dd></dl>";
                        this.txtvalidDate4.Value = item.validDate == DateTime.MinValue ? "" : item.validDate.ToString("yyyy-MM-dd");
                        this.HidFfileName4.Value = item.fileName;
                    }

                }
            }
        }
    }

    [WebMethod]
    public static string Edit(string KeyID, string UserID, string ForceDate, string InvalidDate, string HidFfileName1, string validDate1, string HidFfileName2, string validDate2, string HidFfileName3, string validDate3, string HidFfileName4, string validDate4,string ApplyRemark)
    {
        //
        Common.ResultMessage Msg = new Common.ResultMessage();

        Hi.Model.YZT_FirstCamp firstcamp = new Hi.BLL.YZT_FirstCamp().GetModel(KeyID.ToInt(0));

        string ProvideData = new Hi.BLL.YZT_CMerchants().GetModel(firstcamp.CMID).ProvideData;

        //首营信息
        string sql = "select an.*,fc.ID from YZT_Annex an left join YZT_FCmaterials fc  on an.fcID =fc.ID and an.fileAlias in (4) and an.type in(5,7,8,9) where fc.DisID=" + firstcamp.DisID + " and ISNULL(fc.dr,0)=0 and fc.type=2 and ISNULL(an.dr,0)=0";
        DataTable dt = SqlHelper.GetTable(SqlHelper.LocalSqlServer, sql);

        SqlTransaction Tran = null;
        try
        {
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();

            firstcamp.ForceDate = ForceDate != "" ? Convert.ToDateTime(ForceDate) : DateTime.MinValue;
            firstcamp.InvalidDate = InvalidDate != "" ? Convert.ToDateTime(InvalidDate) : DateTime.MinValue;
            firstcamp.Applyremark = ApplyRemark;

            firstcamp.ts = DateTime.Now;
            firstcamp.modifyuser = UserID.ToInt(0);

            bool id = new Hi.BLL.YZT_FirstCamp().Update(firstcamp, Tran);

            if (id)
            {
                string annexdel = " fileAlias=1 and type in(5,7,9,8) and fcID=" + KeyID;
                new Hi.BLL.YZT_Annex().AnnexDelete(annexdel, Tran);

                List<Hi.Model.YZT_FCmaterials> fcmlist = new Hi.BLL.YZT_FCmaterials().GetList("", " type=2 and DisID=" + firstcamp.DisID, "");
                int fcmid = 0;
                Hi.Model.YZT_FCmaterials fcmmodel = null;
                if (fcmlist != null && fcmlist.Count > 0)
                {
                    fcmmodel = fcmlist[0];
                    fcmid = fcmmodel.ID;
                }
                else
                {
                    fcmmodel = new Hi.Model.YZT_FCmaterials();
                    fcmmodel.CompID = 0;
                    fcmmodel.DisID = firstcamp.DisID;
                    fcmmodel.type = 2;
                    fcmmodel.ts = DateTime.Now;
                    fcmmodel.modifyuser = UserID.ToInt(0);
                    fcmmodel.CreateUserID = UserID.ToInt(0);
                    fcmmodel.CreateDate = DateTime.Now;
                    fcmmodel.dr = 0;
                    fcmid = new Hi.BLL.YZT_FCmaterials().Add(fcmmodel, Tran);
                }

                bool count = id;
                if (ProvideData.IndexOf("1") > -1)
                {
                    //营业执照
                    Hi.Model.YZT_Annex annexModel1 = insertAnnex(KeyID.ToInt(0), 5, HidFfileName1, validDate1, UserID, "1");
                    count = new Hi.BLL.YZT_Annex().Add(annexModel1, Tran) > 0;

                    Hi.Model.YZT_Annex updateModel1 = UpFCmaterials(dt, 5, HidFfileName1, validDate1, UserID);
                    if (updateModel1 != null)
                    {
                        new Hi.BLL.YZT_Annex().Update(updateModel1, Tran);
                    }
                    else
                    {
                        Hi.Model.YZT_Annex annexModel11 = insertAnnex(fcmid, 5, HidFfileName1, validDate1, UserID, "4");
                        new Hi.BLL.YZT_Annex().Add(annexModel11, Tran);
                    }
                }
                if (ProvideData.IndexOf("2") > -1)
                {
                    //医疗器械经营许可证
                    Hi.Model.YZT_Annex annexModel2 = insertAnnex(KeyID.ToInt(0), 7, HidFfileName2, validDate2, UserID, "1");
                    count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran) > 0;

                    Hi.Model.YZT_Annex updateModel2 = UpFCmaterials(dt, 7, HidFfileName2, validDate2, UserID);
                    if (updateModel2 != null)
                    {
                        new Hi.BLL.YZT_Annex().Update(updateModel2, Tran);
                    }
                    else
                    {
                        Hi.Model.YZT_Annex annexModel11 = insertAnnex(fcmid, 7, HidFfileName2, validDate2, UserID, "4");
                        new Hi.BLL.YZT_Annex().Add(annexModel11, Tran);
                    }
                }
                if (ProvideData.IndexOf("3") > -1)
                {
                    //开户许可证
                    Hi.Model.YZT_Annex annexModel3 = insertAnnex(KeyID.ToInt(0), 9, HidFfileName3, validDate3, UserID, "1");
                    count = new Hi.BLL.YZT_Annex().Add(annexModel3, Tran) > 0;

                    Hi.Model.YZT_Annex updateModel3 = UpFCmaterials(dt, 9, HidFfileName3, validDate3, UserID);
                    if (updateModel3 != null)
                    {
                        new Hi.BLL.YZT_Annex().Update(updateModel3, Tran);
                    }
                    else
                    {
                        Hi.Model.YZT_Annex annexModel11 = insertAnnex(fcmid, 9, HidFfileName3, validDate3, UserID, "4");
                        new Hi.BLL.YZT_Annex().Add(annexModel11, Tran);
                    }
                }
                if (ProvideData.IndexOf("4") > -1)
                {
                    //医疗器械备案
                    Hi.Model.YZT_Annex annexModel4 = insertAnnex(KeyID.ToInt(0), 8, HidFfileName4, validDate4, UserID,"1");
                    count = new Hi.BLL.YZT_Annex().Add(annexModel4, Tran) > 0;

                    Hi.Model.YZT_Annex updateModel4 = UpFCmaterials(dt, 8, HidFfileName4, validDate4, UserID);
                    if (updateModel4 != null)
                    {
                        new Hi.BLL.YZT_Annex().Update(updateModel4, Tran);
                    }
                    else
                    {
                        Hi.Model.YZT_Annex annexModel11 = insertAnnex(fcmid, 8, HidFfileName4, validDate4, UserID, "4");
                        new Hi.BLL.YZT_Annex().Add(annexModel11, Tran);
                    }
                }
                if (count)
                {
                    Tran.Commit();
                    Msg.result = true;
                }
                else
                {
                    Tran.Rollback();
                    Msg.code = "编辑异常！";
                }
            }
            else
            {
                Tran.Rollback();
                Msg.code = "编辑异常！";
            }
        }
        catch (Exception)
        {
            Msg.code = "编辑异常！";
            throw;
        }
        return new JavaScriptSerializer().Serialize(Msg);
    }

    public static Hi.Model.YZT_Annex UpFCmaterials(DataTable dt, int type, string name, string date, string UserID)
    {
        DateTime time = DateTime.Now;
        Hi.Model.YZT_Annex annexModel = null;

        if (dt != null && dt.Rows.Count > 0)
        {

            DataRow[] dr1 = dt.Select(string.Format(" type='{0}' and fileAlias=4", type));

            if (dr1.Length > 0)
            {
                annexModel = new Hi.Model.YZT_Annex();

                annexModel.fileName = name;
                annexModel.validDate = date == "" ? DateTime.MinValue : Convert.ToDateTime(date);
                annexModel.ts = time;
                annexModel.modifyuser = UserID.ToInt(0);

                annexModel.ID = dr1[0]["ID"].ToString().ToInt(0);
                annexModel.type = type;
                annexModel.fileAlias = dr1[0]["fileAlias"].ToString();
                annexModel.fcID = dr1[0]["fcID"].ToString().ToInt(0);
                annexModel.dr = dr1[0]["dr"].ToString().ToInt(0);
                annexModel.CreateUserID = dr1[0]["CreateUserID"].ToString().ToInt(0);
                annexModel.CreateDate = Convert.ToDateTime(dr1[0]["CreateDate"].ToString());
            }
        }
        return annexModel;
    }

    /// <summary>
    /// 新增首营信息附件资料
    /// </summary>
    /// <param name="id">首营信息ID</param>
    /// <param name="type">附件资料类型</param>
    /// <param name="name">文件名称</param>
    /// <param name="date">有效期</param>
    /// <returns></returns>
    public static Hi.Model.YZT_Annex insertAnnex(int id, int type, string name, string date, string UserID, string fileAlias)
    {
        DateTime time = DateTime.Now;
        Hi.Model.YZT_Annex annexModel = new Hi.Model.YZT_Annex();
        annexModel.fcID = id;
        annexModel.type = type;
        annexModel.fileName = name;
        annexModel.fileAlias = fileAlias;
        annexModel.validDate = date == "" ? DateTime.MinValue : Convert.ToDateTime(date);
        annexModel.CreateDate = time;
        annexModel.dr = 0;
        annexModel.ts = time;
        annexModel.modifyuser = UserID.ToInt(0);
        annexModel.CreateUserID = UserID.ToInt(0);
        return annexModel;
    }
}
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
using System.Web.Script.Serialization;
using System.Web.Services;

public partial class Distributor_CMerchants_FirstCampAdd : DisPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            if (Request["action"] + "" == "GetHt")
            {
                Response.Write(GetHt());
                Response.End();
            }
            bind();
            AnnexBind();
        }

        
    }

    /// <summary>
    /// 绑定可以选择医院
    /// </summary>
    private void bindHt(int CompId)
    {
        string sql = @"select ht.ID,ht.HospitalName from SYS_Hospital ht where ht.ID not in (select distinct 
HtID from YZT_FirstCamp fc where isnull(fc.dr,0)=0 and fc.State not in (0,1) and fc.CMID=" + this.KeyID  + ")";

        //+ " and fc.CompID=" + CompId

        DataTable htDt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (htDt != null && htDt.Rows.Count > 0)
        {
            ddrHt.DataSource = htDt;
            ddrHt.DataValueField = "ID";
            ddrHt.DataTextField = "HospitalName";
            ddrHt.DataBind();
        }
    }

    private string GetHt()
    {
        string wherestr = @" dr=0 and ID in (select ht.ID from SYS_Hospital ht where ht.ID not in (select distinct 
HtID from YZT_FirstCamp fc where isnull(fc.dr,0)=0 and fc.State not in (0,1) and fc.CMID=" + this.KeyID + "))";

        if (Request["Province"] + "" != "" && Request["Province"] + "" != "选择省")
        {
            wherestr += "and Province ='" + Request["Province"] + "'";
        }
        if (Request["City"] + "" != "")
        {
            wherestr += "and City ='" + Request["City"] + "'";
        }
        if (Request["Area"] + "" != "")
        {
            wherestr += "and Area ='" + Request["Area"] + "'";
        }

        List<Hi.Model.SYS_Hospital> htlist = new Hi.BLL.SYS_Hospital().GetList("", wherestr, "");


        return new JavaScriptSerializer().Serialize(htlist);
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
            Hi.Model.YZT_CMerchants cmModel = new Hi.BLL.YZT_CMerchants().GetModel(this.KeyID);

            bindHt(cmModel.CompID);
            hidcompID.Value = cmModel.CompID.ToString();
            hidProvideData.Value = cmModel.ProvideData;
            if (cmModel.ProvideData.IndexOf("1") > -1)
            {//营业执照
                this.chk1.Visible = true;
            }
            if (cmModel.ProvideData.IndexOf("2") > -1)
            {//医疗器械经营许可证
                this.chk2.Visible = true;
            }
            if (cmModel.ProvideData.IndexOf("3") > -1)
            {//开户许可证
                this.chk3.Visible = true;
            }
            if (cmModel.ProvideData.IndexOf("4") > -1)
            {//医疗器械备案
                this.chk4.Visible = true;
            }

            // 判断是否厂商自己申请自己
            LoginModel uModel = null;
            if (HttpContext.Current.Session["UserModel"] is LoginModel)
            {
                uModel = HttpContext.Current.Session["UserModel"] as LoginModel;
                if (LoginModel.GetUserCompID(uModel.UserID.ToString()) == cmModel.CompID)
                {
                    tr.Visible = false;
                }
            }
        }
    }


    public void AnnexBind()
    {
        //查询需要提供的资料
        string sql="select an.*,fc.ID from YZT_Annex an left join YZT_FCmaterials fc  on an.fcID =fc.ID and an.fileAlias in (4) and an.type in(5,7,8,9) where fc.DisID="+this.DisID+" and ISNULL(fc.dr,0)=0 and fc.type=2 and ISNULL(an.dr,0)=0";

        DataTable dt = SqlHelper.GetTable(SqlHelper.LocalSqlServer, sql);

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                if (!string.IsNullOrEmpty(item["fileName"].ToString()))
                {
                    string linkFile = string.Empty;
                    if (item["fileName"].ToString().LastIndexOf("_") != -1)
                    {
                        string text = item["fileName"].ToString().Substring(0, item["fileName"].ToString().LastIndexOf("_")) + Path.GetExtension(item["fileName"].ToString());
                        if (text.Length < 15)
                            linkFile = text;
                        else
                        {
                            linkFile = text.Substring(0, 15) + "...";
                        }

                    }
                    else
                    {
                        string text = item["fileName"].ToString().Substring(0, item["fileName"].ToString().LastIndexOf("-")) + Path.GetExtension(item["fileName"].ToString());
                        if (text.Length < 15)
                            linkFile = text;
                        else
                        {
                            linkFile = text.Substring(0, 15) + "...";
                        }
                    }

                    if (item["fileAlias"].ToString() == "4" && item["type"].ToString().ToInt(0) == 5)
                    {//营业执照
                        this.txtvalidDate1.Value = item["validDate"].ToString() == "" ? "" : Convert.ToDateTime(item["validDate"].ToString()) == DateTime.MinValue ? "" : Convert.ToDateTime(item["validDate"].ToString()).ToString("yyyy-MM-dd");
                        UpFileText1.InnerHtml = "<dl class=\"teamList\"><dd><a style=\"cursor: pointer;\" class=\"bt\" href=\"../../UploadFile/" + item["fileName"].ToString() + "\" download=" + item["fileName"].ToString() + " target=\"_blank\">" + linkFile + "</a><a class=\"red\" onclick=\"Cancel(1)\">删除</a></dd></dl>";
                        this.HidFfileName1.Value = item["fileName"].ToString();
                    }
                    else if (item["fileAlias"].ToString() == "4" && item["type"].ToString().ToInt(0) == 7)
                    {//医疗器械经营许可证
                        UpFileText2.InnerHtml = "<dl class=\"teamList\"><dd><a style=\"cursor: pointer;\" class=\"bt\" href=\"../../UploadFile/" + item["fileName"].ToString() + "\" download=" + item["fileName"].ToString() + " target=\"_blank\">" + linkFile + "</a><a class=\"red\" onclick=\"Cancel(2)\">删除</a></dd></dl>";
                        this.txtvalidDate2.Value = item["validDate"].ToString() == "" ? "" : Convert.ToDateTime(item["validDate"].ToString()) == DateTime.MinValue ? "" : Convert.ToDateTime(item["validDate"].ToString()).ToString("yyyy-MM-dd");
                        this.HidFfileName2.Value = item["fileName"].ToString();
                    }
                    else if (item["fileAlias"].ToString() == "4" && item["type"].ToString().ToInt(0) == 9)
                    {//开户许可证
                        UpFileText3.InnerHtml = "<dl class=\"teamList\"><dd><a style=\"cursor: pointer;\" class=\"bt\" href=\"../../UploadFile/" + item["fileName"].ToString() + "\" download=" + item["fileName"].ToString() + " target=\"_blank\">" + linkFile + "</a><a class=\"red\" onclick=\"Cancel(3)\">删除</a></dd></dl>";
                        this.txtvalidDate3.Value = item["validDate"].ToString() == "" ? "" : Convert.ToDateTime(item["validDate"].ToString()) == DateTime.MinValue ? "" : Convert.ToDateTime(item["validDate"].ToString()).ToString("yyyy-MM-dd");
                        this.HidFfileName3.Value = item["fileName"].ToString();
                    }
                    else if (item["fileAlias"].ToString() == "4" && item["type"].ToString().ToInt(0) == 8)
                    {//医疗器械备案
                        UpFileText4.InnerHtml = "<dl class=\"teamList\"><dd><a style=\"cursor: pointer;\" class=\"bt\" href=\"../../UploadFile/" + item["fileName"].ToString() + "\" download=" + item["fileName"].ToString() + " target=\"_blank\">" + linkFile + "</a><a class=\"red\" onclick=\"Cancel(4)\">删除</a></dd></dl>";
                        this.txtvalidDate4.Value = item["validDate"].ToString() == "" ? "" : Convert.ToDateTime(item["validDate"].ToString()) == DateTime.MinValue ? "" : Convert.ToDateTime(item["validDate"].ToString()).ToString("yyyy-MM-dd");
                        this.HidFfileName4.Value = item["fileName"].ToString();
                    }
                }
            }
        }

    }

    [WebMethod]
    public static string Edit(string KeyID, string CompID, string DisID, string UserID, string HtID, string ForceDate, string InvalidDate, string HidFfileName1, string validDate1, string HidFfileName2, string validDate2, string HidFfileName3, string validDate3, string HidFfileName4, string validDate4, string ApplyRemark)
    {
        //
        Common.ResultMessage Msg = new Common.ResultMessage();
        Hi.Model.YZT_CMerchants cmModel = new Hi.BLL.YZT_CMerchants().GetModel(KeyID.ToInt(0));
        Hi.Model.YZT_FirstCamp firstcamp = new Hi.Model.YZT_FirstCamp();

        //判断是否是该厂商的代理商
        List<Hi.Model.SYS_CompUser> compulist = new Hi.BLL.SYS_CompUser().GetList("", " UserID=" + UserID + " and CompID=" + CompID + " and DisID=" + DisID + "", "");
        Hi.Model.SYS_Users usersModel = new Hi.BLL.SYS_Users().GetModel(UserID);

        //首营信息
        string sql = "select an.*,fc.ID from YZT_Annex an left join YZT_FCmaterials fc  on an.fcID =fc.ID and an.fileAlias in (4) and an.type in(5,7,8,9) where fc.DisID=" + DisID + " and ISNULL(fc.dr,0)=0 and fc.type=2 and ISNULL(an.dr,0)=0";
        DataTable dt = SqlHelper.GetTable(SqlHelper.LocalSqlServer, sql);
       
        LoginModel uModel = null;
        if (HttpContext.Current.Session["UserModel"] is LoginModel)
        {
            uModel = HttpContext.Current.Session["UserModel"] as LoginModel;
        }

        SqlTransaction Tran = null;
        try
        {
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();

            if (DisID == "0")
            {
                Hi.Model.BD_Distributor Distributor = new Hi.Model.BD_Distributor();
                Distributor.CompID = 0;
                Distributor.DisName = uModel.CompName;
                Distributor.IsEnabled = 1;
                Distributor.Paypwd = Util.md5("123456");
                Distributor.Phone = uModel.Phone;
                Distributor.AuditState = 0;
                Distributor.CreateDate = DateTime.Now;
                Distributor.CreateUserID = uModel.UserID;
                Distributor.ts = DateTime.Now;
                Distributor.modifyuser = uModel.UserID;
                Distributor.IsCheck = 0;
                Distributor.CreditType = 0;
                Distributor.pic = "";
                //Distributor.creditCode = txt_creditCode;
                Distributor.Leading = "";
                Distributor.Licence = "";
                int DistributorID = 0;
                if ((DistributorID = new Hi.BLL.BD_Distributor().Add(Distributor, Tran)) > 0)
                {
                    DisID = DistributorID.ToString();

                    //代理商账户登录
                    Hi.Model.SYS_CompUser compuser = new Hi.Model.SYS_CompUser();
                    compuser.UserID = UserID.ToInt(0);
                    compuser.CompID = CompID.ToInt(0);
                    compuser.DisID = DisID.ToInt(0);
                    compuser.AreaID = 0;
                    compuser.RoleID = usersModel == null ? 0 : usersModel.RoleID;
                    compuser.CType = 2;
                    compuser.UType = 5;
                    compuser.IsAudit = 0;
                    compuser.IsEnabled = 1;
                    compuser.ts = DateTime.Now;
                    compuser.CreateUserID = UserID.ToInt(0);
                    compuser.modifyuser = UserID.ToInt(0);
                    compuser.CreateDate = DateTime.Now;

                    if (new Hi.BLL.SYS_CompUser().Add(compuser, Tran) <= 0)
                    {
                        Tran.Rollback();
                        Msg.code = "用户信息添加失败";
                        return new JavaScriptSerializer().Serialize(Msg);
                    }
                }
                else
                {
                    Tran.Rollback();
                    Msg.code = "用户信息添加失败";
                    return new JavaScriptSerializer().Serialize(Msg);
                }
            }
            else
            {
                if (compulist != null && compulist.Count <= 0)
                {
                    //代理商账户登录
                    Hi.Model.SYS_CompUser compuser = new Hi.Model.SYS_CompUser();
                    compuser.UserID = UserID.ToInt(0);
                    compuser.CompID = CompID.ToInt(0);
                    compuser.DisID = DisID.ToInt(0);
                    compuser.AreaID = 0;
                    compuser.RoleID = usersModel == null ? 0 : usersModel.RoleID;
                    compuser.CType = 2;
                    compuser.UType = 5;
                    compuser.IsAudit = 0;
                    compuser.IsEnabled = 1;
                    compuser.ts = DateTime.Now;
                    compuser.CreateUserID = UserID.ToInt(0);
                    compuser.modifyuser = UserID.ToInt(0);
                    compuser.CreateDate = DateTime.Now;

                    if (new Hi.BLL.SYS_CompUser().Add(compuser, Tran) <= 0)
                    {
                        Tran.Rollback();
                        Msg.code = "用户信息添加失败";
                        return new JavaScriptSerializer().Serialize(Msg);
                    }
                }
            }

            List<Hi.Model.YZT_FirstCamp> fcamplist = new Hi.BLL.YZT_FirstCamp().GetList("", " CMID=" + KeyID + " and DisID=" + DisID + " and CompID=" + CompID + " and HtID=" + HtID, "");
            if (fcamplist != null && fcamplist.Count > 0)
            {

                Tran.Rollback();
                Msg.code = "已申请合作";
                return new JavaScriptSerializer().Serialize(Msg);
            }

            firstcamp.CMID = KeyID.ToInt(0);
            firstcamp.CompID = CompID.ToInt(0);
            firstcamp.DisID = DisID.ToInt(0);
            firstcamp.ForceDate = ForceDate != "" ? Convert.ToDateTime(ForceDate) : DateTime.MinValue;
            firstcamp.InvalidDate = InvalidDate != "" ? Convert.ToDateTime(InvalidDate) : DateTime.MinValue;
            firstcamp.State = 0;
            firstcamp.HtID = HtID.ToInt(0);
            firstcamp.Applyremark = ApplyRemark;

            firstcamp.ts = DateTime.Now;
            firstcamp.modifyuser = UserID.ToInt(0);
            firstcamp.CreateDate = DateTime.Now;
            firstcamp.CreateUserID = UserID.ToInt(0);

            int id = new Hi.BLL.YZT_FirstCamp().Add(firstcamp, Tran);

            if (id > 0)
            {
                //string annexdel = " fileAlias=1 and type in(5,7,9,8) and fcID=" + id;
                //new Hi.BLL.YZT_Annex().AnnexDelete(annexdel, Tran);

                List<Hi.Model.YZT_FCmaterials> fcmlist = new Hi.BLL.YZT_FCmaterials().GetList("", "DisID=" + DisID + " and type=2", "");
                int fcmid=0;
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
                    fcmmodel.DisID = DisID.ToInt(0);
                    fcmmodel.type = 2;
                    fcmmodel.ts = DateTime.Now;
                    fcmmodel.modifyuser = UserID.ToInt(0);
                    fcmmodel.CreateUserID = UserID.ToInt(0);
                    fcmmodel.CreateDate = DateTime.Now;
                    fcmmodel.dr = 0;
                    fcmid = new Hi.BLL.YZT_FCmaterials().Add(fcmmodel, Tran);
                }

                int count = id;
                string ProvideData = cmModel.ProvideData;
                if (ProvideData.IndexOf("1") > -1)
                {
                    //营业执照
                    Hi.Model.YZT_Annex annexModel1 = insertAnnex(id, 5, HidFfileName1, validDate1, UserID,"1");
                    count = new Hi.BLL.YZT_Annex().Add(annexModel1, Tran);

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
                    Hi.Model.YZT_Annex annexModel2 = insertAnnex(id, 7, HidFfileName2, validDate2, UserID, "1");
                    count = new Hi.BLL.YZT_Annex().Add(annexModel2, Tran);

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
                    Hi.Model.YZT_Annex annexModel3 = insertAnnex(id, 9, HidFfileName3, validDate3, UserID, "1");
                    count = new Hi.BLL.YZT_Annex().Add(annexModel3, Tran);

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
                    Hi.Model.YZT_Annex annexModel4 = insertAnnex(id, 8, HidFfileName4, validDate4, UserID, "1");
                    count = new Hi.BLL.YZT_Annex().Add(annexModel4, Tran);

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
                if (count <= 0)
                {
                    Tran.Rollback();
                    Msg.code = "编辑异常！";
                    return new JavaScriptSerializer().Serialize(Msg);
                }
                Tran.Commit();
                Msg.result = true;
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
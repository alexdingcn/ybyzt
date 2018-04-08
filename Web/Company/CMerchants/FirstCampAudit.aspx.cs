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
using System.Data;
using DBUtility;
using System.Data.SqlClient;
using System.Text;

public partial class Company_CMerchants_FirstCampAudit : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
       
    }

    public string DisArea = string.Empty;//代理商区域数据源
    /// <summary>
    /// 
    /// </summary>
    public void bind()
    {
        if (KeyID != 0)
        {
            Hi.Model.YZT_FirstCamp fcmodel = new Hi.BLL.YZT_FirstCamp().GetModel(KeyID);

            if (fcmodel != null)
            {
                //string htid = Request["htid"] + "";
                //if (QrFC(fcmodel.CMID.ToString(), htid) > 0)
                //{
                //    this.btnConfirm.Visible = false;
                //    this.liDisArea.Visible = false;
                //}
            }

            //绑定区域
            StringBuilder sbare = new StringBuilder();
            List<Hi.Model.BD_DisArea> are = new Hi.BLL.BD_DisArea().GetList("top 12 * ", "isnull(dr,0)=0 and  ParentId=0  and CompanyID=" + this.CompID, " SortIndex");
            if (are.Count > 0)
            {
                sbare.Append("[");
                int num = 0;
                foreach (var model in are)
                {
                    num++;
                    sbare.Append("{code:'" + model.ID + "',value: '" + model.ID + "',label: '" + model.AreaName + "'");
                    List<Hi.Model.BD_DisArea> aret1 = new Hi.BLL.BD_DisArea().GetList("Areacode,ID,AreaName", "isnull(dr,0)=0  and ParentId=" + model.ID, "");
                    if (aret1.Count > 0)
                    {
                        sbare.Append(",children: [");
                        int num2 = 0;
                        foreach (var model2 in aret1)
                        {
                            num2++;
                            sbare.Append("{code:'" + model2.ID + "',value: '" + model2.ID + "',label: '" + model2.AreaName + "'");
                            List<Hi.Model.BD_DisArea> are3 = new Hi.BLL.BD_DisArea().GetList("Areacode,ID,AreaName", "isnull(dr,0)=0  and ParentId=" + model2.ID, "");
                            if (are3.Count > 0)
                            {
                                sbare.Append(",children: [");
                                int num3 = 0;
                                foreach (var item3 in are3)
                                {
                                    num3++;
                                    if (num3 == are3.Count)
                                        sbare.Append("{code:'" + item3.ID + "',value: '" + item3.ID + "',label: '" + item3.AreaName + "'}");
                                    else
                                        sbare.Append("{code:'" + item3.ID + "',value: '" + item3.ID + "',label: '" + item3.AreaName + "'},");

                                }
                                sbare.Append("]");

                            }

                            if (num2 == aret1.Count)
                                sbare.Append("}");
                            else
                                sbare.Append("},");
                        }
                        sbare.Append("]");

                    }
                    if (num == are.Count)
                        sbare.Append("}");
                    else
                        sbare.Append("},");
                }
                sbare.Append("]");
                DisArea = sbare.ToString();
            }
        }
    }

   
    /// <summary>
    /// 判断是否已存在通过的代理商
    /// </summary>
    /// <param name="cmID"></param>
    /// <returns></returns>
    public int QrFC(string cmID, string htid)
    {
        string sql = "select * from YZT_FirstCamp fc where isnull(fc.dr,0)=0 and fc.State=2 and fc.CMID=" + cmID + " and HtID=" + htid + " and fc.CompID=" + this.CompID;

        DataTable htDt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (htDt != null && htDt.Rows.Count <= 0)
        {
            return 0;
        }
        return 1;
    }

    [WebMethod]
    public static string Edit(string KeyID, string Remark, string DisAreaID, string CompID, string UserID)
    {
        Common.ResultMessage Msg = new Common.ResultMessage();
        Hi.Model.YZT_FirstCamp fcmodel = new Hi.BLL.YZT_FirstCamp().GetModel(KeyID.ToInt(0));

        List<Hi.Model.YZT_Annex> annlist = new Hi.BLL.YZT_Annex().GetList("", " fcID=" + KeyID + " and type=2 and fileAlias='2' ", "");

        if (annlist != null && annlist.Count <= 0)
        {
            Msg.code = "请先上传授权书";
        }
        else
        {
            if (fcmodel != null)
            {
                //判断是否是该厂商的代理商
                List<Hi.Model.SYS_CompUser> compulist = new Hi.BLL.SYS_CompUser().GetList("", " UserID=" + fcmodel.CreateUserID + " and CompID=" + CompID + " and DisID=" + fcmodel.DisID + "", "");
              
                Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(CompID.ToInt(0));
                string compName = comp == null ? "" : comp.CompName;

                SqlTransaction Tran = DBUtility.SqlHelper.CreateStoreTranSaction();

                fcmodel.AreaID = DisAreaID.ToInt(0);
                fcmodel.Remark = Remark;
                fcmodel.State = 2;
                fcmodel.ts = DateTime.Now;

                if (new Hi.BLL.YZT_FirstCamp().Update(fcmodel, Tran))
                {
                    if (compulist != null && compulist.Count <= 0)
                    {
                        Hi.Model.SYS_Users usersModel = new Hi.BLL.SYS_Users().GetModel(fcmodel.CreateUserID);
                        string Phone = usersModel == null ? "" : usersModel.Phone;
                        string UserName = usersModel == null ? "" : usersModel.UserName;

                        Hi.Model.SYS_CompUser compuser = new Hi.Model.SYS_CompUser();
                        compuser.UserID = fcmodel.CreateUserID;
                        compuser.CompID = CompID.ToInt(0);
                        compuser.DisID = fcmodel.DisID;
                        compuser.AreaID = DisAreaID.ToInt(0);
                        compuser.RoleID = usersModel == null ? 0 : usersModel.RoleID;
                        compuser.CType = 2;
                        compuser.UType = 5;
                        compuser.IsAudit = 2;
                        compuser.IsEnabled = 1;
                        compuser.ts = DateTime.Now;
                        compuser.CreateUserID = UserID.ToInt(0);
                        compuser.modifyuser = UserID.ToInt(0);
                        compuser.CreateDate = DateTime.Now;

                        if (new Hi.BLL.SYS_CompUser().Add(compuser, Tran) > 0)
                        {
                            Tran.Commit();
                            Msg.result = true;

                            GetPhoneCode pc = new GetPhoneCode();
                            pc.SendConfirm(Phone, compName, UserName);
                            
                        }
                        else
                        {
                            Tran.Rollback();
                            Msg.code = "用户信息添加失败";
                        }
                    }
                    else
                    {
                        Hi.Model.SYS_CompUser compuser = compulist[0];
                        compuser.IsAudit = 2;
                        compuser.AreaID = DisAreaID.ToInt(0);
                        compuser.ts = DateTime.Now;
                        compuser.modifyuser = UserID.ToInt(0);

                        if (new Hi.BLL.SYS_CompUser().Update(compuser, Tran))
                        {
                            Tran.Commit();
                            Msg.result = true;

                            Hi.Model.SYS_Users usersModel = new Hi.BLL.SYS_Users().GetModel(compulist[0].UserID);
                            string Phone = usersModel == null ? "" : usersModel.Phone;
                            string UserName = usersModel == null ? "" : usersModel.UserName;

                            GetPhoneCode pc = new GetPhoneCode();
                            pc.SendConfirm(Phone, compName, UserName);
                        }
                        else
                        {
                            Tran.Rollback();
                            Msg.code = "用户信息添加失败";
                        }
                    }
                }
                else
                {
                    Tran.Rollback();
                    Msg.code = "通过失败";
                }
            }
            else
            {
                Msg.code = "未查找到数据";
            }
        }
        return new JavaScriptSerializer().Serialize(Msg);
    }

    [WebMethod]
    public static string RejectEdit(string KeyID, string Remark)
    {
        Common.ResultMessage Msg = new Common.ResultMessage();
        Hi.Model.YZT_FirstCamp fcmodel = new Hi.BLL.YZT_FirstCamp().GetModel(KeyID.ToInt(0));

        if (fcmodel != null)
        {
            fcmodel.Remark = Remark;
            fcmodel.State = 1;
            fcmodel.ts = DateTime.Now;

            if (new Hi.BLL.YZT_FirstCamp().Update(fcmodel))
            {
                Msg.result = true;

                Hi.Model.SYS_Users usersModel = new Hi.BLL.SYS_Users().GetModel(fcmodel.CreateUserID);
                string Phone = usersModel == null ? "" : usersModel.Phone;
                Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(fcmodel.CompID);
                string compName = comp == null ? "" : comp.CompName;

                GetPhoneCode pc = new GetPhoneCode();
                pc.SendReject(Phone, compName, Remark);
            }
        }
        else
        {
            Msg.code = "未查找到数据";
        }
        return new JavaScriptSerializer().Serialize(Msg);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Company_ImportDis2 : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (HttpContext.Current.Session["DisTable"] != null)
            {
                DataTable dt = HttpContext.Current.Session["DisTable"] as DataTable;
                rptDis.DataSource = dt;
                rptDis.DataBind();
            }
            else
            {
                JScript.AlertMethod(this, "请先导入", JScript.IconOption.错误, "function(){location.href='ImportGoods.aspx'}");
            }
        }
    }
    /// <summary>
    /// 确定导入
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnImport_Click(object sender, EventArgs e)
    {
        if (HttpContext.Current.Session["DisTable"] != null)
        {
            SqlTransaction Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            try
            {
                DataTable dt = HttpContext.Current.Session["DisTable"] as DataTable;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["chkstr"].ToString() == "数据正确！")
                    {
                        Hi.Model.BD_Distributor Dis = new Hi.Model.BD_Distributor();
                        //Dis.DisCode = Discode;
                        Dis.CompID = CompID;
                        Dis.DisName = dt.Rows[i]["disname"].ToString().Trim();
                        Dis.Province = dt.Rows[i]["pro"].ToString().Trim();
                        Dis.City = dt.Rows[i]["city"].ToString().Trim();
                        Dis.Area = dt.Rows[i]["quxian"].ToString().Trim();
                        Dis.Address = dt.Rows[i]["address"].ToString().Trim();
                        Dis.Principal = dt.Rows[i]["principal"].ToString().Trim();
                        Dis.Phone = dt.Rows[i]["phone"].ToString().Trim();
                        Dis.DisTypeID = Convert.ToInt32(dt.Rows[i]["distypeid"].ToString().Trim());//add by 2016.5.9
                        Dis.AreaID = Convert.ToInt32(dt.Rows[i]["areaid"].ToString().Trim());//add by 2016.5.10
                        Dis.Remark = dt.Rows[i]["remark"].ToString().Trim(); 
                        Dis.IsCheck = 0;
                        Dis.CreditType = 0;  //不可以赊销
                        Dis.Paypwd = Util.md5("123456");
                        Dis.IsEnabled = 1;
                        Dis.AuditState = 2;
                        Dis.CreateDate = DateTime.Now;
                        Dis.CreateUserID = UserID;
                        Dis.ts = DateTime.Now;
                        Dis.modifyuser = UserID;
                        int disid = 0;
                        if ((disid = new Hi.BLL.BD_Distributor().Add(Dis, Tran)) > 0)
                        {
                            List<Hi.Model.SYS_Role> l = new Hi.BLL.SYS_Role().GetList("", "isnull(dr,0)=0 and isenabled=1 and DisID=" + disid + " and RoleName='企业管理员'", "", Tran);
                            if (l.Count == 0)
                            {
                                //新增角色（企业管理员）
                                Hi.Model.SYS_Role role = new Hi.Model.SYS_Role();
                                role.CompID = CompID;
                                role.DisID = disid;
                                role.RoleName = "企业管理员";
                                role.IsEnabled = 1;
                                role.SortIndex = "1";
                                role.CreateDate = DateTime.Now;
                                role.CreateUserID = UserID;
                                role.ts = DateTime.Now;
                                role.modifyuser = UserID;
                                role.dr = 0;
                                int Roid = new Hi.BLL.SYS_Role().Add(role, Tran);
                                //新增管理员用户和角色
                                Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
                                user.UserName = dt.Rows[i]["username"].ToString().Trim();
                                user.Phone = dt.Rows[i]["phone"].ToString().Trim();
                                user.TrueName = dt.Rows[i]["principal"].ToString().Trim();
                                user.UserPwd = Util.md5("123456");
                                user.IsEnabled = 1;
                                user.AuditState = 2;
                                user.CreateDate = DateTime.Now;
                                user.CreateUserID = UserID;
                                user.ts = DateTime.Now;
                                user.modifyuser = UserID;
                                int AddUserid = new Hi.BLL.SYS_Users().Add(user, Tran);

                                ///用户明细表
                                Hi.Model.SYS_CompUser CompUser = new Hi.Model.SYS_CompUser();
                                CompUser.CompID = CompID;
                                CompUser.DisID = disid;
                                CompUser.CreateDate = DateTime.Now;
                                CompUser.CreateUserID = UserID;
                                CompUser.modifyuser = UserID;
                                CompUser.CType = 2;
                                CompUser.UType = 5;
                                CompUser.IsEnabled = 1;
                                CompUser.IsAudit = 2;
                                CompUser.RoleID = Roid;
                                CompUser.ts = DateTime.Now;
                                CompUser.dr = 0;
                                CompUser.UserID = AddUserid;
                                new Hi.BLL.SYS_CompUser().Add(CompUser, Tran);
                                //新增角色权限表   //暂时屏蔽下
                                Hi.Model.SYS_RoleSysFun rolesys = null;
                                List<Hi.Model.SYS_SysFun> funList = new Hi.BLL.SYS_SysFun().GetList("", " Type=2", "", Tran);
                                foreach (Hi.Model.SYS_SysFun sys in funList)
                                {
                                    rolesys = new Hi.Model.SYS_RoleSysFun();
                                    rolesys.CompID = CompID;
                                    rolesys.DisID = disid;
                                    rolesys.RoleID = Roid;
                                    rolesys.FunCode = sys.FunCode;
                                    rolesys.FunName = sys.FunName;
                                    rolesys.IsEnabled = 1;
                                    rolesys.CreateUserID = UserID;
                                    rolesys.CreateDate = DateTime.Now;
                                    rolesys.ts = DateTime.Now;
                                    rolesys.modifyuser = UserID;
                                    new Hi.BLL.SYS_RoleSysFun().Add(rolesys, Tran);
                                }
                            }

                            Hi.Model.BD_DisAddr addr = new Hi.Model.BD_DisAddr();
                            addr.Province = dt.Rows[i]["pro"].ToString().Trim();
                            addr.City = dt.Rows[i]["city"].ToString().Trim();
                            addr.Area = dt.Rows[i]["quxian"].ToString().Trim();
                            addr.DisID = disid;
                            addr.Principal = dt.Rows[i]["principal"].ToString().Trim();
                            addr.Phone = dt.Rows[i]["phone"].ToString().Trim();
                            addr.Address = dt.Rows[i]["pro"].ToString().Trim() + dt.Rows[i]["city"].ToString().Trim() + dt.Rows[i]["quxian"].ToString().Trim() + dt.Rows[i]["address"].ToString().Trim();
                            addr.IsDefault = 1;
                            addr.ts = DateTime.Now;
                            addr.CreateDate = DateTime.Now;
                            addr.CreateUserID = UserID;
                            addr.modifyuser = UserID;
                            new Hi.BLL.BD_DisAddr().Add(addr, Tran);
                        }
                        else
                        {
                            throw new ApplicationException("导入失败，服务器异常请重试。");
                        }
                    }
                }
                Tran.Commit();
                Response.Redirect("ImportDis3.aspx", false);
                //ClientScript.RegisterStartupScript(this.GetType(), "Add", "<script>addlis(" + count + "," + count2 + ",'" + str + "');</script>");
            }
            catch (Exception ex)
            {
                if (Tran != null)
                {
                    if (Tran.Connection != null)
                    {
                        Tran.Rollback();
                    }
                }
                HttpContext.Current.Session["DisTable"] = null;
                JScript.AlertMethod(this, ex.Message, JScript.IconOption.错误, "function(){location.href='ImportDis.aspx'}");
            }
        }
        else
        {
            JScript.AlertMethod(this, "Excel没有数据，请重新导入", JScript.IconOption.错误, "function(){location.href='ImportDis.aspx'}");
        }
    }
}
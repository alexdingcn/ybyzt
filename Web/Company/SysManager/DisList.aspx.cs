using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.IO;
using System.Data;

public partial class Company_SysManager_DisList : CompPageBase
{
    public string date = null;
    public string page = "1";//默认初始页
    int TitleIndex = 2;
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>$(\".txt_txtAreaname\").css(\"width\", \"150px\");</script>");
        if (!IsPostBack)
        {
            //txtDisArea.CompID = CompID.ToString();
            
            if (Request.QueryString["page"] != null && Request.QueryString["page"]!="")
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            if (Request["cus"] == "1")
            {
                date = " and dis.Createdate>'" + DateTime.Now.AddDays(1 - DateTime.Now.Day).ToShortDateString() + "' and dis.Createdate<='" + DateTime.Now.AddDays(1).ToShortDateString() + "'";
            }
            if (Request["nextstep"] != null && Request["nextstep"].ToString() == "1")
            {
                atitle.InnerText = "我要开通";
                ToExcel.Visible = false;
                libtnNext.Style.Add("display", "block;");
            }
            DataBinds();

            if (!Common.HasRight(this.CompID, this.UserID, "1311"))
            {
                this.btnnpoi.Visible = false;
                this.btnAdd.Visible = false;
            }

        }
    }

    public void DataBinds()
    {
        int pageCount = 0;
        int Counts = 0;
        if (this.txtPageSize.Value.ToString() != "")
        {
            if (this.txtPageSize.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPageSize.Value = "100";
            }
            else
            {
                Pager.PageSize = this.txtPageSize.Value.Trim().ToInt(0);
            }
        }
       // List<Hi.Model.BD_Distributor> LDis = new Hi.BLL.BD_Distributor().GetList(Pager.PageSize, Pager.CurrentPageIndex, "Createdate", false, SearchWhere()+date, out pageCount, out Counts);

        DataTable LDis = new Hi.BLL.BD_Distributor().getDataTable(Pager.PageSize, Pager.CurrentPageIndex, SearchWhere() + date, out pageCount, out Counts, Pager.RecordCount);

        this.Rpt_Distribute.DataSource = LDis;
        this.Rpt_Distribute.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    public void Pager_PageChanged(object sender, EventArgs e)
    {
        DataBinds();
        page = Pager.CurrentPageIndex.ToString();
    }
    public void btn_SearchClick(object sender, EventArgs e)
    {
        DataBinds();
    }

    public void btn_DelClick(object sender, EventArgs e)
    {
        DataBinds();
        page = Pager.CurrentPageIndex.ToString();
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string where = " and cu.compid=" + CompID + " and cu.IsAudit>0 and isnull(cu.dr,0)=0 and isnull(dis.dr,0)=0";
        string IDlist = string.Empty;//销售经理下属 员工ID集合
        if (DisSalesManID != 0)
        {
            if (Common.GetDisSalesManType(DisSalesManID.ToString(), out IDlist))
            {
                //销售经理
                where += "and cu.DisID in(select ID from BD_Distributor where smid in(" + IDlist + "))";
            }
            else
            {
                where += "and cu.DisID in(select ID from BD_Distributor where smid = " + DisSalesManID + ")";
            }
        }
        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            where += " and (dis.DisCode like '%" + txtDisName.Value.Trim().Replace("'", "''") + "%' or dis.DisName like '%" + txtDisName.Value.Trim().Replace("'", "''") + "%')";
        }
        //if (!string.IsNullOrEmpty(txtDisArea.areaId))
        //{
        //    where += " and areaid='" + txtDisArea.areaId + "'";
        //}
        if (!string.IsNullOrEmpty(txtPhone.Value.Trim()))
        {
            where += " and dis.Phone like '%" + txtPhone.Value.Trim().Replace("'", "''") + "%'";
        }
        if (!string.IsNullOrEmpty(txtPrincipal.Value.Trim()))
        {
            where += " and dis.Principal like '%" + txtPrincipal.Value.Trim().Replace("'", "''") + "%'";
        }
        if (ddlAUState.SelectedValue != "-1") {
            where += " and cu.IsAudit='" + ddlAUState.SelectedValue + "'";
        }
        if (ddlState.SelectedValue != "-1") {
            where += " and cu.IsEnabled='" + ddlState.SelectedValue + "'";
        }
        if (txtCreateDate.Value.Trim() != "")
        {
            where += " and dis.createdate>='" + txtCreateDate.Value.Trim() + "' ";
        }
        if (txtEndCreateDate.Value.Trim() != "")
        {
            where += " and dis.createdate<='" + txtEndCreateDate.Value.Trim() + "' ";
        }
        return where;
    }

    #region 代理商导入

    bool Eroor = false;
    string TitleError = string.Empty;
    public void btnAddList_Click(object sender, EventArgs e)
    {
        string path = "";
        int count = 0;
        int index = 0;
        SqlTransaction Tran = null;
        try
        {
            if (FileUpload1.HasFile == false)//HasFile用来检查FileUpload是否有指定文件
            {
                JScript.AlertMsgOne(this, "请您选择代理商Excel模板文件", JScript.IconOption.错误);
                return;//当无文件时,返回
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();//System.IO.Path.GetExtension获得文件的扩展名
            if (IsXls != ".xls" && IsXls != ".xlsx")
            {
                JScript.AlertMsgOne(this, "请您选择代理商Excel模板文件", JScript.IconOption.错误);
                return;//当选择的不是Excel文件时,返回
            }
            if (!Directory.Exists(Server.MapPath("TemplateFile")))
            {
                Directory.CreateDirectory(Server.MapPath("TemplateFile"));
            }
            string filename = FileUpload1.FileName;
            string name = filename.Replace(IsXls, "");
            path = Server.MapPath("TemplateFile/") + name + "-" + DateTime.Now.ToString("yyyyMMddhhmmssffff") + IsXls;
            FileUpload1.SaveAs(path);
            DataTable dt = Common.ExcelToDataTable(path, TitleIndex);
            if (dt == null)
            {
                throw new Exception("Excel表中无数据");
            }
            if (dt.Rows.Count == 0)
            {
                throw new Exception("Excel表中无数据");
            }
            string Discode = string.Empty;
            string DisName = string.Empty;
            string DisUserName = string.Empty;
            string DisAddrees = string.Empty;
            string DisPerson = string.Empty;
            string DisPhone = string.Empty;
            string DisRemark = string.Empty;
            string Provice = string.Empty;
            string City = string.Empty;
            string Area = string.Empty;
            string DisCategory = string.Empty;
            string DisLevel = string.Empty;
            DataRow[] rows = dt.Select();
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            Eroor = false;
            TitleError = string.Empty;
            foreach (DataRow row in rows)
              {
                int typeID = 0;
                int AreaID = 0;
                try
                {
                    //这个判断有bug呀，是遇到空行就停止的意思吗？
                    if (row["代理商名称 *\n（2-20个汉字或字母，推荐使用中文名称）"].ToString().Trim() == "" && row["管理员姓名 *\n（请填写真实姓名，以便更好地为您服务）"].ToString().Trim() == "" && row["详细地址 *\n（常用收货地址）"].ToString().Trim() == "")
                    {
                        break;
                    }
                    index++;
                    if (row["代理商名称 *\n（2-20个汉字或字母，推荐使用中文名称）"].ToString().Trim() == "示例代理商名称1" || row["代理商名称 *\n（2-20个汉字或字母，推荐使用中文名称）"].ToString().Trim() == "示例代理商名称2" || row["代理商名称 *\n（2-20个汉字或字母，推荐使用中文名称）"].ToString().Trim() == "示例代理商名称3")
                    {
                        continue;
                    }
                    DisName = DisExistsAttribute("DisName", CheckDisLen(CheckVal(row["代理商名称 *\n（2-20个汉字或字母，推荐使用中文名称）"].ToString().Trim(), "代理商名称", index), index), "代理商名称", index, Tran);
                    DisPerson = CheckVal(row["管理员姓名 *\n（请填写真实姓名，以便更好地为您服务）"].ToString().Trim(), "管理员姓名", index);
                    DisUserName = UserExistsAttribute("username", CheckVal(row["管理员登录帐号 *\n（2-20个文字、字母、数字，可以录入代理商姓名、简称等，一经设定无法更改，将来可用手机号进行登录）"].ToString().Trim(), "管理员登录帐号", index), "管理员登录帐号", index, Tran);
                    DisPhone = CheckPhone(CheckVal(row["管理员手机 *\n（登录、发送验证短信）"].ToString().Trim(), "管理员手机", index), "管理员手机", index, Tran);
                    Provice = CheckVal(row["所在省*"].ToString().Trim(), "省", index);
                    City = CheckVal(row["所在市*"].ToString().Trim(), "市", index);
                    if (City.IndexOf("_") > 0)
                    {
                        City = City.Substring(City.IndexOf("_") + 1, City.Length - City.IndexOf("_") - 1);
                    }
                    Area = CheckVal(row["所在区*"].ToString().Trim(), "区", index);
                    DisAddrees = CheckVal(row["详细地址 *\n（常用收货地址）"].ToString().Trim(), "详细地址（常用收货地址）", index);
                    DisCategory = row["代理商分类"].ToString().Trim();
                    DisLevel = row["代理商区域"].ToString().Trim();
                    bool disType = true;
                    if (!string.IsNullOrEmpty(DisCategory))
                    {
                        disType = CheckDisCategory(DisCategory, index, out typeID);
                    }
                    if (!string.IsNullOrEmpty(DisLevel))
                    {
                        CheckDisLevel(DisLevel, index, out AreaID);
                    }
                    DisRemark = row["备注"].ToString().Trim();
                    if(Eroor)  continue;
                }
                catch (Exception ex)
                {
                    if (ex is ApplicationException)
                    {
                        Eroor = true;
                        TitleError += ex.Message;
                        continue;
                    }
                    else
                        throw new Exception("代理商Excel模版格式错误，请重新下载模版填入数据后导入。");
                }

                Hi.Model.BD_Distributor Dis = new Hi.Model.BD_Distributor();
                //Dis.DisCode = Discode;
                Dis.CompID = CompID;
                Dis.DisName = DisName;
                Dis.Province = Provice;
                Dis.City = City;
                Dis.Area = Area;
                Dis.Address = DisAddrees;
                Dis.Principal = DisPerson;
                Dis.Phone = DisPhone;
                Dis.DisTypeID = typeID;//add by 2016.5.9
                Dis.AreaID = AreaID;//add by 2016.5.10
                Dis.Remark = DisRemark;
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
                        user.UserName = DisUserName;
                        user.Phone = DisPhone;
                        user.TrueName = DisPerson;
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
                    addr.Province = Provice;
                    addr.City = City;
                    addr.Area = Area;
                    addr.DisID = disid;
                    addr.Principal = DisPerson;
                    addr.Phone = DisPhone;
                    addr.Address = Provice + City + Area + DisAddrees;
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
                count++;
            }
            if (!Eroor)
            {
                Tran.Commit();
                if (Request["nextstep"] + "" == "1")
                    JScript.AlertMethod(this, "导入成功,共导入" + count + "条代理商",JScript.IconOption.笑脸, "function(){  window.location.href=window.location.href+'?nextstep=1'; /* $(window.parent.leftFrame.document).find('.menuson li.active').removeClass('active');window.parent.leftFrame.document.getElementById('ktxzjxs').className = 'active';*/}");
                else
                    JScript.AlertMethod(this, "导入成功,共导入" + count + "条代理商", JScript.IconOption.笑脸, "function(){  window.location.href=window.location.href; }");
            }
            else
            {
                Tran.Rollback();
                JScript.AlertMethod(this, TitleError, JScript.IconOption.错误, "function(){ addList(); }");
            }

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
            JScript.AlertMethod(this, ex.Message, JScript.IconOption.错误, "function(){ $('a.bulk').trigger('click'); }");
        }
        finally
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }



    public string DisExistsAttribute(string name, string value, string str, int index, SqlTransaction Tran)
    {
        if (string.IsNullOrEmpty(value))
            return value;
        List<Hi.Model.BD_Distributor> Dis = new Hi.BLL.BD_Distributor().GetList("", " " + name + "='" + value + "' and CompID=" + CompID + " and isnull(dr,0)=0 ", "", Tran);
        if (Dis.Count > 0)
        {
            Eroor = true;
            TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "已存在，请修改后重新导入。<br/>";
            //throw new ApplicationException("");
        }
        return value;
    }

    public string CheckVal(string value, string str, int index)
    {
        if (string.IsNullOrEmpty(value))
        {
            Eroor = true;
            TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "不能为空！请修改后重新导入。<br/>";
            //throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "不能为空！请修改后重新导入。<br/>");
        }
        return value;
    }

    public string CheckDisLen(string DisName, int index)
    {
        if (string.IsNullOrEmpty(DisName))
            return DisName;
        if (DisName.Length < 2 || DisName.Length > 20)
        {
            Eroor = true;
            TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。代理商名称只能为2-20个汉字或字母！请修改后重新导入。<br/>";
        }
        return DisName;
    }

    public string CheckPhone(string value, string str, int index, SqlTransaction Tran)
    {
        if (string.IsNullOrEmpty(value))
            return value;
        if (!Regex.IsMatch(value, "^0?1[0-9]{10}$"))
        {
            Eroor = true;
            TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + " </i>  &nbsp;&nbsp;的数据有误。" + str + "手机号码不正确，请修改后重新导入。<br/>";
            //throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 1) + " </i>  &nbsp;&nbsp;的数据有误。" + str + "手机号码不正确，请修改后重新导入。<br/>");
        }
        else
        {
            if (Common.GetUserExists("Phone", value, Tran))
            {
                Eroor = true;
                TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + " </i> &nbsp;&nbsp;的数据有误。" + str + "手机号码已存在，请修改后重新导入。<br/>";
                //throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 1) + " </i> &nbsp;&nbsp;的数据有误。" + str + "手机号码已存在，请修改后重新导入。<br/>");
            }
        }
        return value;
    }

    public string UserExistsAttribute(string name, string value, string str, int index, SqlTransaction Tran)
    {
        if (string.IsNullOrEmpty(value))
            return value;
        List<Hi.Model.SYS_Users> Dis = new Hi.BLL.SYS_Users().GetList("", " " + name + "='" + value + "'  and isnull(dr,0)=0 ", "", Tran);
        if (Dis.Count > 0)
        {
            Eroor = true;
            TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "已存在，请修改后重新导入。<br/>";
            //throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "已存在，请修改后重新导入。<br/>");
        }
        return value;
    }

    public bool CheckDisCategory(string value, int index ,out int typeID)
    {
        List<Hi.Model.BD_DisType> disList = new Hi.BLL.BD_DisType().GetList("", " CompId=" + CompID + " and TypeName='" + value + "' and IsEnabled = 0 and dr =0", "");
        if (disList != null && disList.Count == 1)
        {
            typeID = disList[0].ID;
            return true;
        }
        else
        {
            Eroor = true;
            TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。代理商分类：“" + value + "”错误！请修改后重新导入。<br/>";
            typeID = 0;
            return false;
        }
    }

    public bool CheckDisLevel(string value, int index ,out int AreaID)
    {
        List<Hi.Model.BD_DisArea> disList = new Hi.BLL.BD_DisArea().GetList("", "isnull(dr,0)=0 and CompanyID=" + this.CompID + " and AreaName ='" + value + "'", "");
        if (disList != null && disList.Count == 1)
        {
            AreaID = disList[0].ID;
            return true;
        }
        else
        {
            Eroor = true;
            TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。代理商区域：“" + value + "”不存在！请修改后重新导入。<br/>";
            AreaID = 0;
            return false;
        }
    }
    #endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Data;

public partial class Company_SysManager_DisAdd : CompPageBase
{
    int TitleIndex = 2;
    protected void Page_Load(object sender, EventArgs e)
    {
        Hi.Model.SYS_Users user = this.CompUser;
        if (Request["nextstep"] != null && Request["nextstep"].ToString() == "1")
        {
            atitle.InnerText = "我要开通";
            btitle.InnerText = "新增代理商";

        }
        if (user.IsFirst == 0)
        {
            user.IsFirst = 4;
            user.modifyuser = user.ID;
            user.ts = DateTime.Now;
            new Hi.BLL.SYS_Users().Update(user);
        }
    }


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
                JScript.AlertMsg(this, "请您选择Excel文件", "");
                return;//当无文件时,返回
            }
            string IsXls = System.IO.Path.GetExtension(FileUpload1.FileName).ToString().ToLower();//System.IO.Path.GetExtension获得文件的扩展名
            if (IsXls != ".xls" && IsXls != ".xlsx")
            {
                JScript.AlertMsg(this, "只可以选择Excel文件", "");
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
            DataRow[] rows = dt.Select();
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            Eroor = false;
            TitleError = string.Empty;
            foreach (DataRow row in rows)
            {
                try
                {
                    if (row["代理商(代理商)名称 *\n（2-20个汉字或字母，推荐使用中文名称）"].ToString().Trim() == "" && row["管理员姓名 *\n（请填写真实姓名，以便更好地为您服务）"].ToString().Trim() == "" && row["详细地址 *\n（常用收货地址）"].ToString().Trim() == "")
                    {
                        break;
                    }
                    index++;
                    if (row["代理商(代理商)名称 *\n（2-20个汉字或字母，推荐使用中文名称）"].ToString().Trim() == "示例代理商名称1" || row["代理商(代理商)名称 *\n（2-20个汉字或字母，推荐使用中文名称）"].ToString().Trim() == "示例代理商名称2" || row["代理商(代理商)名称 *\n（2-20个汉字或字母，推荐使用中文名称）"].ToString().Trim() == "示例代理商名称3")
                    {
                        continue;
                    }
                    DisName = DisExistsAttribute("DisName", CheckDisLen(CheckVal(row["代理商(代理商)名称 *\n（2-20个汉字或字母，推荐使用中文名称）"].ToString().Trim(), "代理商(代理商)名称", index), index), "代理商(代理商)名称", index, Tran);
                    DisPerson = CheckVal(row["管理员姓名 *\n（请填写真实姓名，以便更好地为您服务）"].ToString().Trim(), "管理员姓名", index);
                    DisUserName = UserExistsAttribute("username", CheckVal(row["管理员登录帐号 *\n（2-20个文字、字母、数字，可以录入代理商姓名、简称等，一经设定无法更改，将来可用手机号进行登录）"].ToString().Trim(), "管理员登录帐号", index), "管理员登录帐号", index, Tran);
                    DisPhone = CheckPhone(CheckVal(row["管理员手机 *\n（登录、发送验证短信）"].ToString().Trim(), "管理员手机", index), "管理员手机", index, Tran);
                    Provice = CheckVal(row["所在省*"].ToString().Trim(), "省", index);
                    City = CheckVal(row["所在市*"].ToString().Trim(), "市", index);
                    if (City.IndexOf("_") > 0) {
                        City = City.Substring(City.IndexOf("_")+1, City.Length - City.IndexOf("_")-1);
                    }
                    Area = CheckVal(row["所在区*"].ToString().Trim(), "区", index);
                    DisAddrees = CheckVal(row["详细地址 *\n（常用收货地址）"].ToString().Trim(), "详细地址（常用收货地址）", index);
                    DisRemark = row["备注"].ToString().Trim();
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
                    List<Hi.Model.SYS_Role> l = new Hi.BLL.SYS_Role().GetList("", "isnull(dr,0)=0 and isenabled=1 and DisID=" + disid + " and RoleName='企业管理员'", "",Tran);
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
                        user.Type = 5;
                        user.DisID = disid;
                        user.CompID = CompID;
                        user.UserName = DisUserName;
                        user.Phone = DisPhone;
                        user.TrueName = DisPerson;
                        user.UserPwd = Util.md5("123456");
                        user.IsEnabled = 1;
                        user.AuditState = 2;
                        user.RoleID = Roid;
                        user.CreateDate = DateTime.Now;
                        user.CreateUserID = UserID;
                        user.ts = DateTime.Now;
                        user.modifyuser = UserID;
                        new Hi.BLL.SYS_Users().Add(user, Tran);
                        //新增角色权限表  在审核通过时会加
                        
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
                    JScript.AlertMsgMo(this, "导入成功,共导入" + count + "条代理商", "function(){  $(window.parent.leftFrame.document).find('.menuson li.active').removeClass('active');window.parent.leftFrame.document.getElementById('ktxzjxs').className = 'active'; window.location.href='DisList.aspx?nextstep=1'; }");
                else
                    JScript.AlertMsgMo(this, "导入成功,共导入" + count + "条代理商", "function(){  $(window.parent.leftFrame.document).find('.menuson .lista1 div.al').removeClass('active');window.parent.leftFrame.document.getElementById('jxsxxwh').className = 'al active'; window.location.href='DisList.aspx'; }");
            }
            else
            {
                Tran.Rollback();
                JScript.AlertMsgMo(this, TitleError, "function(){ $('a.bulk').trigger('click'); }");
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
            JScript.AlertMsgMo(this, ex.Message, "function(){ $('a.bulk').trigger('click'); }");
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
        if(string.IsNullOrEmpty(value))
            return value;
        List<Hi.Model.BD_Distributor> Dis = new Hi.BLL.BD_Distributor().GetList("", " " + name + "='" + value + "' and CompID=" + CompID + " and isnull(dr,0)=0 ", "", Tran);
        if (Dis.Count > 0)
        {
            Eroor = true;
            TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex+1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "已存在，请修改后重新导入。<br/>";
            //throw new ApplicationException("");
        }
        return value;
    }

    public string CheckVal(string value, string str, int index)
    {
        if (string.IsNullOrEmpty(value))
        {
            Eroor = true;
            TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex+1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "不能为空！请修改后重新导入。<br/>";
            //throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "不能为空！请修改后重新导入。<br/>");
        }
        return value;
    }

    public string CheckDisLen(string DisName, int index) {
        if (string.IsNullOrEmpty(DisName))
            return DisName;
        if (DisName.Length < 2 || DisName.Length > 20) {
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
            TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex+1) + " </i>  &nbsp;&nbsp;的数据有误。" + str + "手机号码不正确，请修改后重新导入。<br/>";
            //throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 1) + " </i>  &nbsp;&nbsp;的数据有误。" + str + "手机号码不正确，请修改后重新导入。<br/>");
        }
        else
        {
            if (Common.GetUserExists("Phone", value, Tran))
            {
                Eroor = true;
                TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex+1) + " </i> &nbsp;&nbsp;的数据有误。" + str + "手机号码已存在，请修改后重新导入。<br/>";
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
            TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex+1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "已存在，请修改后重新导入。<br/>";
            //throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "已存在，请修改后重新导入。<br/>");
        }
        return value;
    }
}
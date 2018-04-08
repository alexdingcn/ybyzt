using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

public partial class Company_ImportDis : CompPageBase
{

    int TitleIndex = 2;
    string categoryId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    /// <summary>
    /// 批量导入datetable
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddList_Click(object sender, EventArgs e)
    {
        string path = "";
        int count = 0;
        int count2 = 0;
        int index = 0;
        try
        {
            if (upload.HasFile == false)//HasFile用来检查FileUpload是否有指定文件
            {
                JScript.AlertMethod(this, "请您选择Excel文件", JScript.IconOption.错误);
                return;//当无文件时,返回
            }
            string IsXls = System.IO.Path.GetExtension(upload.FileName).ToString().ToLower();//System.IO.Path.GetExtension获得文件的扩展名
            if (IsXls != ".xls" && IsXls != ".xlsx")
            {
                JScript.AlertMethod(this, "只可以选择Excel文件", JScript.IconOption.错误);
                return;//当选择的不是Excel文件时,返回
            }
            if (!Directory.Exists(Server.MapPath("GoodsNew/TemplateFile")))
            {
                Directory.CreateDirectory(Server.MapPath("GoodsNew/TemplateFile"));
            }
            string filename = upload.FileName;
            string name = filename.Replace(IsXls, "");
            path = Server.MapPath("SysManager/TemplateFile/") + name + "-" + DateTime.Now.ToString("yyyyMMddhhmmssffff") + IsXls;
            upload.SaveAs(path);
            System.Data.DataTable dt = Common.ExcelToDataTable(path, TitleIndex);
            if (dt == null)
            {
                throw new Exception("Excel表中无数据");
            }
            if (dt.Rows.Count == 0)
            {
                throw new Exception("Excel表中无数据");
            }

            DataRow[] rows = dt.Select();
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("disname", typeof(string));     //代理商名称
            dt2.Columns.Add("principal", typeof(string));     //管理员姓名
            dt2.Columns.Add("username", typeof(string));   //登录帐号
            dt2.Columns.Add("phone", typeof(string));    //手机
            dt2.Columns.Add("pro", typeof(string));   //省
            dt2.Columns.Add("city", typeof(string)); //市
            dt2.Columns.Add("quxian", typeof(string)); //区县
            dt2.Columns.Add("address", typeof(string)); //地址
            dt2.Columns.Add("distypeid", typeof(string)); //分类id
            dt2.Columns.Add("areaid", typeof(string)); //区域id
            dt2.Columns.Add("distype", typeof(string)); //分类
            dt2.Columns.Add("area", typeof(string)); //区域
            dt2.Columns.Add("remark", typeof(string)); //备注
            dt2.Columns.Add("chkstr", typeof(string)); //检查结果

            HttpContext.Current.Session["DisTable"] = null;
            foreach (DataRow row in rows)
            {
                List<string[]> al = new List<string[]>();
                string goodsAttrValue = string.Empty;
                string goodsAttr = string.Empty;
                try
                {
                    if (row["代理商名称 *（2-20个汉字或字母，推荐使用中文名称）"].ToString().Trim() == "" && row["管理员姓名 *（请填写真实姓名，以便更好地为您服务）"].ToString().Trim() == "" && row["管理员登录账号 *（2-20个文字、字母、数字，可以录入代理商姓名、简称等）"].ToString().Trim() == "" && row["管理员手机 *（登录、发送验证短信）"].ToString().Trim() == "" && row["所在省*"].ToString().Trim() == "" && row["所在市*"].ToString().Trim() == "" && row["所在区*"].ToString().Trim() == "" && row["详细地址 *（常用收货地址）"].ToString().Trim() == "")
                    {
                        continue;
                    }
                    index++;
                    if (row["代理商名称 *（2-20个汉字或字母，推荐使用中文名称）"].ToString().Trim() == "示例代理商名称1" || row["代理商名称 *（2-20个汉字或字母，推荐使用中文名称）"].ToString().Trim() == "示例代理商名称2" || row["代理商名称 *（2-20个汉字或字母，推荐使用中文名称）"].ToString().Trim() == "示例代理商名称3")
                    {
                        continue;
                    }
                    if (row["代理商名称 *（2-20个汉字或字母，推荐使用中文名称）"].ToString().Trim() == "" && row["管理员姓名 *（请填写真实姓名，以便更好地为您服务）"].ToString().Trim() == "" && row["管理员登录账号 *（2-20个文字、字母、数字，可以录入代理商姓名、简称等）"].ToString().Trim() == "" && row["管理员手机 *（登录、发送验证短信）"].ToString().Trim() == "" && row["所在省*"].ToString().Trim() == "" && row["所在市*"].ToString().Trim() == "" && row["所在区*"].ToString().Trim() == "" && row["详细地址 *（常用收货地址）"].ToString().Trim() == "" && row["代理商分类"].ToString().Trim() == "" && row["代理商区域"].ToString().Trim() == "" && row["备注"].ToString().Trim() == "")
                    {
                        continue;
                    }
                    else
                    {
                        DataRow dr1 = dt2.NewRow();
                        int typeID = 0;
                        int AreaID = 0;
                        dr1["chkstr"] = "数据正确！";
                        dr1["address"] = CheckVal(row["详细地址 *（常用收货地址)"].ToString().Trim(), "详细地址（常用收货地址）", dr1);
                        dr1["quxian"] = CheckVal(row["所在区*"].ToString().Trim(), "区", dr1);
                        string City = CheckVal(row["所在市*"].ToString().Trim(), "市", dr1);
                        // CheckVal(row["所在市*"].ToString().Trim(), "市", dr1);
                        if (City.IndexOf("_") > 0)
                        {
                            City = City.Substring(City.IndexOf("_") + 1, City.Length - City.IndexOf("_") - 1);
                        }
                        dr1["city"] = City;
                        dr1["pro"] = CheckVal(row["所在省*"].ToString().Trim(), "省", dr1);

                        dr1["phone"] = CheckPhone(CheckVal(row["管理员手机 *（登录、发送验证短信）"].ToString().Trim(), "管理员手机", dr1), "管理员手机", dr1);
                        dr1["username"] = UserExistsAttribute("username", CheckVal(row["管理员登录账号 *（2-20个文字、字母、数字，可以录入代理商姓名、简称等）"].ToString().Trim(), "管理员登录帐号", dr1), "管理员登录帐号", dr1, dt2);
                        dr1["principal"] = CheckVal(row["管理员姓名 *（请填写真实姓名，以便更好地为您服务）"].ToString().Trim(), "管理员姓名", dr1);
                        dr1["disname"] = DisExistsAttribute("DisName", CheckDisLen(CheckVal(row["代理商名称 *（2-20个汉字或字母，推荐使用中文名称）"].ToString().Trim(), "代理商名称", dr1), dr1), "代理商名称", dr1, dt2);
                        dr1["distype"] = row["代理商分类"].ToString().Trim();
                        dr1["area"] = row["代理商区域"].ToString().Trim();
                        bool disType = true;
                        if (!string.IsNullOrEmpty(row["代理商分类"].ToString().Trim()))
                        {
                            disType = CheckDisCategory(row["代理商分类"].ToString().Trim(), out typeID, dr1);
                        }
                        if (!string.IsNullOrEmpty(row["代理商区域"].ToString().Trim()))
                        {
                            CheckDisLevel(row["代理商区域"].ToString().Trim(), out AreaID, dr1);
                        }
                        dr1["distypeid"] = typeID;
                        dr1["areaid"] = AreaID;
                        dr1["remark"] = row["备注"].ToString().Trim();
                        dt2.Rows.Add(dr1);
                    }
                }
                catch (Exception ex)
                {
                    dt2.Rows.Clear();
                    if (ex is ApplicationException)
                    {
                        continue;
                    }
                    else
                    {
                        throw new Exception("代理商Excel模版格式错误，请重新下载模版填入数据后导入。");
                    }
                }
            }
            if (dt2.Rows.Count == 0)
            {
                throw new Exception("Excel表中无数据");
            }
            else
            {
                HttpContext.Current.Session["DisTable"] = dt2;
                Response.Redirect("ImportDis2.aspx", false);
            }
        }
        catch (Exception ex)
        {
            HttpContext.Current.Session["DisTable"] = null;
            JScript.AlertMethod(this, ex.Message, JScript.IconOption.错误, "");
        }
        finally
        {
            if (!Util.IsEmpty(path))
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
    /// <summary>
    /// 分类验证
    /// </summary>
    /// <param name="value"></param>
    /// <param name="typeID"></param>
    /// <param name="dr"></param>
    /// <returns></returns>
    public bool CheckDisCategory(string value, out int typeID, DataRow dr)
    {
        List<Hi.Model.BD_DisType> disList = new Hi.BLL.BD_DisType().GetList("", " CompId=" + CompID + " and TypeName='" + value + "' and IsEnabled = 0 and dr =0", "");
        if (disList != null && disList.Count == 1)
        {
            typeID = disList[0].ID;
            return true;
        }
        else
        {
            dr["chkstr"] = "代理商分类：”" + value + "“错误";
            // Eroor = true;
            // TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。代理商分类：“" + value + "”错误！请修改后重新导入。<br/>";
            typeID = 0;
            return false;
        }
    }
    /// <summary>
    /// 区域验证
    /// </summary>
    /// <param name="value"></param>
    /// <param name="AreaID"></param>
    /// <param name="dr"></param>
    /// <returns></returns>
    public bool CheckDisLevel(string value, out int AreaID, DataRow dr)
    {
        List<Hi.Model.BD_DisArea> disList = new Hi.BLL.BD_DisArea().GetList("", "isnull(dr,0)=0 and CompanyID=" + this.CompID + " and AreaName ='" + value + "'", "");
        if (disList != null && disList.Count == 1)
        {
            AreaID = disList[0].ID;
            return true;
        }
        else
        {
            dr["chkstr"] = "代理商区域：”" + value + "“不存在";
            // Eroor = true;
            // TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。代理商区域：“" + value + "”不存在！请修改后重新导入。<br/>";
            AreaID = 0;
            return false;
        }
    }
    /// <summary>
    /// 手机验证
    /// </summary>
    /// <param name="value"></param>
    /// <param name="str"></param>
    /// <param name="index"></param>
    /// <param name="Tran"></param>
    /// <returns></returns>
    public string CheckPhone(string value, string str, DataRow dr)
    {
        if (string.IsNullOrEmpty(value))
            return value;
        if (!Regex.IsMatch(value, "^0?1[0-9]{10}$"))
        {
            dr["chkstr"] = str + "号码不正确";
            //Eroor = true;
            // TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + " </i>  &nbsp;&nbsp;的数据有误。" + str + "手机号码不正确，请修改后重新导入。<br/>";
            //throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 1) + " </i>  &nbsp;&nbsp;的数据有误。" + str + "手机号码不正确，请修改后重新导入。<br/>");
        }
        else
        {
            if (Common.GetUserExists("Phone", value))
            {
                dr["chkstr"] = str + "号码已存在";
                // Eroor = true;
                //TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + " </i> &nbsp;&nbsp;的数据有误。" + str + "手机号码已存在，请修改后重新导入。<br/>";
                //throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 1) + " </i> &nbsp;&nbsp;的数据有误。" + str + "手机号码已存在，请修改后重新导入。<br/>");
            }
        }
        return value;
    }
    /// <summary>
    /// 检查登录用户名是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="str"></param>
    /// <param name="index"></param>
    /// <param name="Tran"></param>
    /// <returns></returns>
    public string UserExistsAttribute(string name, string value, string str, DataRow dr, DataTable dt)
    {
        if (string.IsNullOrEmpty(value))
            return value;
        List<Hi.Model.SYS_Users> Dis = new Hi.BLL.SYS_Users().GetList("", " " + name + "='" + value + "'  and isnull(dr,0)=0 ", "");
        if (Dis.Count > 0)
        {
            dr["chkstr"] = value + str + "已存在";
            // Eroor = true;
            //TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "已存在，请修改后重新导入。<br/>";
            //throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "已存在，请修改后重新导入。<br/>");
        }
        else
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["username"].ToString() == value.Trim())
                {
                    dr["chkstr"] = value + str + "Excel已存在";
                    return value;
                }
            }
        }
        return value;
    }
    /// <summary>
    /// 验证超过一定长度
    /// </summary>
    /// <param name="DisName"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public string CheckDisLen(string DisName, DataRow dr)
    {
        if (string.IsNullOrEmpty(DisName))
            return DisName;
        if (DisName.Length < 2 || DisName.Length > 20)
        {
            dr["chkstr"] = "代理商名称只能为2-20个汉字或字母";
            // Eroor = true;
            // TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。代理商名称只能为2-20个汉字或字母！请修改后重新导入。<br/>";
        }
        return DisName;
    }
    /// <summary>
    /// 验证是否为空
    /// </summary>
    /// <param name="value"></param>
    /// <param name="str"></param>
    /// <param name="dr"></param>
    /// <returns></returns>
    public string CheckVal(string value, string str, DataRow dr)
    {
        if (string.IsNullOrEmpty(value))
        {
            dr["chkstr"] = str + "不能为空";
            // Eroor = true;
            // TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "不能为空！请修改后重新导入。<br/>";
            //throw new ApplicationException("Excel行号为：&nbsp;<i error>" + (index + 1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "不能为空！请修改后重新导入。<br/>");
        }
        return value;
    }
    /// <summary>
    /// 验证代理商名字是否重复
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="str"></param>
    /// <param name="index"></param>
    /// <param name="Tran"></param>
    /// <returns></returns>
    public string DisExistsAttribute(string name, string value, string str, DataRow dr, DataTable dt)
    {
        if (string.IsNullOrEmpty(value))
            return value;
        List<Hi.Model.BD_Distributor> Dis = new Hi.BLL.BD_Distributor().GetList("", " " + name + "='" + value + "' and CompID=" + CompID + " and isnull(dr,0)=0 ", "");
        if (Dis.Count > 0)
        {
            dr["chkstr"] = value + str + "已存在";
            // Eroor = true;
            // TitleError += "Excel行号为：&nbsp;<i error>" + (index + TitleIndex + 1) + "</i> &nbsp;&nbsp;的数据有误。" + str + "已存在，请修改后重新导入。<br/>";
            //throw new ApplicationException("");
        }
        else
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["disname"].ToString() == value.Trim())
                {
                    dr["chkstr"] = value + str + "Excel已存在";
                    return value;
                }
            }
        }
        return value;
    }
}
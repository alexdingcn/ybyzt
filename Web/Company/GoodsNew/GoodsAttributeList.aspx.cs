using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Data;
using DBUtility;
using System.Reflection;
using System.Data.SqlClient;

public partial class Company_Goods_GoodsAttributeList : CompPageBase
{
    public string page = "1";//默认初始页
    public string attrvalue = string.Empty;
    public string sortindex = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        string action = Request["action"];
        if (action == "show")//属性值列表
        {
            string AttrId =Common.NoHTML( Request["id"]);
            Response.Write(Bind2(AttrId));
            Response.End();
        }
        if (action == "addValue")//新增属性值
        {
            string value =Common.NoHTML( Request["value"]);
            string id =Common.NoHTML( Request["id"]);
            string type =Common.NoHTML( Request["type"]);
            string attrName =Common.NoHTML( Request["attrName"]);//属性名称
            if (type == "1")
            {
                Response.Write(AddAttributeValue(value, id));//编辑时的添加属性值
                Response.End();
            }
            else
            {
                Response.Write(AddAttributeValue(value, id, attrName));//添加时的添加属性值
                Response.End();
            }
        }
        if (action == "Del")//删除属性值
        {
            string id = Request["id"];
            Response.Write(DelValue(id));
            Response.End();
        }
        if (action == "Enable")//禁用属性值
        {
            string id =Common.NoHTML( Request["id"]);
            Response.Write(EnableValue(id));
            Response.End();
        }
        if (action == "Save")//禁用属性值
        {
            string id =Common.NoHTML( Request["id"]);//属性值ID
            string value =Common.NoHTML( Request["value"]);//属性值
            string attributeId =Common.NoHTML( Request["attributeId"]);//属性ID
            Response.Write(SaveValue(id, attributeId, value));
            Response.End();
        }
        if (action == "IsExist")
        { //属性是否存在
            int id = Convert.ToInt32(Request["id"] == "" ? "0" : Request["id"]);//属性值ID
            string value =Common.NoHTML( Request["value"]);//属性值
            Response.Write(IsExist(id, value));
            Response.End();
        }
        if (action == "delattr")
        {
            int id = Convert.ToInt32(Request["id"]);
            Response.Write(DelAttr(id));
            Response.End();
        }
        if (!IsPostBack)
        {
            Bind();//属性列表
            //  Bind2(AttrId);//属性值列表
        }
    }
    /// <summary>
    /// 取消删除最新插入
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string DelAttr(int id)
    {
        SqlTransaction Tran = Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
        try
        {
            Hi.Model.BD_Attribute model = new Hi.BLL.BD_Attribute().GetModel(id, Tran);
            model.dr = 1;
            model.modifyuser = this.UserID;
            model.ts = DateTime.Now;
            new Hi.BLL.BD_Attribute().Update(model, Tran);
            List<Hi.Model.BD_AttributeValues> l = new Hi.BLL.BD_AttributeValues().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and attributeid=" + id, "", Tran);
            if (l.Count > 0)
            {
                foreach (Hi.Model.BD_AttributeValues item in l)
                {
                    Hi.Model.BD_AttributeValues model2 = new Hi.BLL.BD_AttributeValues().GetModel(item.ID, Tran);
                    model2.dr = 1;
                    model2.modifyuser = this.UserID;
                    model2.ts = DateTime.Now;
                    new Hi.BLL.BD_AttributeValues().Update(model2, Tran);
                }
            }
            Tran.Commit();
            return "cg";
        }
        catch (Exception ex)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                    Tran.Rollback();
            }
            JScript.AlertMethod(this, "失败", JScript.IconOption.错误);
            return "";
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }
    }
    /// <summary>
    /// 绑定属性列表
    /// </summary>
    /// <param name="id"></param>
    private void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strWhere = "and ISNULL(dr,0)=0 and ComPid=" + this.CompID;
        if (ViewState["strWhere"] != null)
        {
            strWhere += ViewState["strWhere"].ToString();
        }
        //每页显示的数据设置
        if (this.txtPageSize.Value.Trim().ToString() != "" && this.txtPageSize.Value.Trim().ToString() != "0")
        {
            if (this.txtPageSize.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPageSize.Value = "100";
            }
            else
            {
                this.Pager.PageSize = this.txtPageSize.Value.Trim().ToInt(0);
            }
        }
        List<Hi.Model.BD_Attribute> l = new Hi.BLL.BD_Attribute().GetList(Pager.PageSize, Pager.CurrentPageIndex, "id", false, strWhere, out pageCount, out Counts);
        this.rptAttribute.DataSource = l;
        this.rptAttribute.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }
    /// <summary>
    /// 根据属性ID得到属性值
    /// </summary>
    /// <returns></returns>
    public string GetAttributeValueList(int str)
    {
        string list = string.Empty;
        List<Hi.Model.BD_AttributeValues> ll = new Hi.BLL.BD_AttributeValues().GetList("", "AttributeID=" +str + " and ISNULL(dr,0)=0 and ComPid=" + this.CompID, "");
        if (ll.Count > 0)
        {
            foreach (Hi.Model.BD_AttributeValues item in ll)
            {
                string str2 = item.AttrValue;
                if (item.IsEnabled == 0)//如果禁用这灰色
                {
                    str2 = "<label style=\"color:#aaaaaa\">" + str2 + "</label>";
                }
                list += str2 + "；";
            }
            if (list.Length > 65)
            {
                list = list.Substring(0, list.Substring(0, 65).LastIndexOf('；')) + "<br>&nbsp;&nbsp;&nbsp;" + list.Substring(list.Substring(0, 65).LastIndexOf('；'));
            }
        }
        return list != "" ? list.Substring(0, list.Length - 1) : list;
    }
    /// <summary>
    /// 搜索按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strWhere = Where();
        ViewState["strWhere"] = strWhere;
        Bind();
    }
    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    private string Where()
    {
        string strWhere = string.Empty;
        //赋值
        string attribute =Common.NoHTML( this.txtAttribute.Value.Trim().Replace("'", "''"));//属性名称
        if (!Util.IsEmpty(attribute))
        {
            strWhere += string.Format(" and attributename like '%{0}%' and ISNULL(dr,0)=0 and Compid=" + this.CompID,Common.NoHTML( attribute));

        }
        return strWhere;
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDel_Click(object sender, EventArgs e)
    {
        SqlTransaction Tran = Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
        try
        {
            List<int> l = new List<int>();
            foreach (RepeaterItem row in this.rptAttribute.Items)
            {
                CheckBox cb = row.FindControl("CB_SelItem") as CheckBox;
                if (cb != null && cb.Checked)
                {
                    HiddenField fld = row.FindControl("HF_Id") as HiddenField;
                    if (fld != null)
                    {
                        int id = Convert.ToInt32(fld.Value);
                        l.Add(id);
                        Hi.Model.BD_Attribute model = new Hi.BLL.BD_Attribute().GetModel(id);
                        if (model != null)
                        {
                            var lll = new Hi.BLL.BD_GoodsAttrs().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and  AttrsName ='" + model.AttributeName + "'", "");
                            if (lll.Count > 0)
                            {
                                JScript.AlertMethod(this, "属性已被使用，不能删除", JScript.IconOption.错误);
                                break;
                            }
                            model.ts = DateTime.Now;
                            model.modifyuser = this.UserID;
                            model.dr = 1;
                            model.CompID = this.CompID;
                            bool bol = new Hi.BLL.BD_Attribute().Update(model);
                            List<Hi.Model.BD_AttributeValues> ll = new Hi.BLL.BD_AttributeValues().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and attributeid=" + id, "", Tran);
                            if (ll.Count > 0)
                            {
                                foreach (Hi.Model.BD_AttributeValues item in ll)
                                {
                                    Hi.Model.BD_AttributeValues model2 = new Hi.BLL.BD_AttributeValues().GetModel(item.ID, Tran);
                                    model2.dr = 1;
                                    model2.modifyuser = this.UserID;
                                    model2.ts = DateTime.Now;
                                    new Hi.BLL.BD_AttributeValues().Update(model2, Tran);
                                }
                            }
                        }
                    }
                }
            }
            Tran.Commit();
        }
        catch (Exception ex)
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                    Tran.Rollback();
            }
            JScript.AlertMethod(this, "失败", JScript.IconOption.错误);
            return;
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
        }
        //  new Hi.BLL.BD_Attribute().Updates(this.UserID.ToString(), l, this.CompID.ToString());
        Bind();
    }
    /// <summary>
    /// 删除属性值
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string DelValue(string id)
    {
        //查看该属性值是否被使用
        var ll = new Hi.BLL.BD_AttributeValues().GetModel(Convert.ToInt32(id));
        if (ll != null)
        {
            string valueName = ll.AttrValue;
            var lll = new Hi.BLL.BD_GoodsAttrsInfo().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and attrsInfoName='" +Common.NoHTML( valueName) + "'", "");
            if (lll.Count > 0)
            {
                return "ycz";
            }
        }
        string str = string.Empty;
        if (id != "" && id != null)
        {
            Hi.Model.BD_AttributeValues model = new Hi.BLL.BD_AttributeValues().GetModel(Convert.ToInt32(id));
            if (model != null)
            {
                model.ts = DateTime.Now;
                model.modifyuser = this.UserID;
                model.dr = 1;
                model.CompID = this.CompID;
                bool bol = new Hi.BLL.BD_AttributeValues().Update(model);
                return "cg";
            }
        }
        return str;
    }
    /// <summary>
    /// 禁用属性值
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string EnableValue(string id)
    {
        string str = string.Empty;
        if (id != "" && id != null)
        {
            Hi.Model.BD_AttributeValues model = new Hi.BLL.BD_AttributeValues().GetModel(Convert.ToInt32(id));
            if (model != null)
            {
                model.ts = DateTime.Now;
                model.modifyuser = this.UserID;

                model.CompID = this.CompID;
                var ll = new Hi.BLL.BD_AttributeValues().GetList("", "id=" + id + " and IsEnabled=1", "");
                if (ll.Count > 0)
                {
                    model.IsEnabled = 0;
                    //if (new Hi.BLL.BD_AttributeValues().Updatess(this.UserID.ToString(), id, "0") > 0)
                    //{
                    //    Utils.EditLog("用户" + this.UserName + "禁用属性值成功", "禁用属性值成功", "商品属性管理模块", "Company/Goods/GoodsAttributeList.aspx", 1);
                    str = "jycg";//禁用
                    // }
                }
                else
                {
                    model.IsEnabled = 1;
                    //if (new Hi.BLL.BD_AttributeValues().Updatess(this.UserID.ToString(), id, "1") > 0)
                    //{
                    //    Utils.EditLog("用户" + this.UserName + "启用属性值成功", "启用属性值成功", "商品属性管理模块", "Company/Goods/GoodsAttributeList.aspx", 1);
                    str = "qycg";//启用
                    // }
                }
                bool bol = new Hi.BLL.BD_AttributeValues().Update(model);
            }
        }
        return str;
    }
    /// <summary>
    /// 编辑属性值
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string SaveValue(string id, string attributeId, string value)
    {
        //查看该属性值是否被使用
        var ll = new Hi.BLL.BD_AttributeValues().GetList("", "id<>" + id + " and isnull(dr,0)=0 and attrvalue='" + value + "' and attributeId=" + attributeId, "");
        if (ll.Count > 0)
        {
            return "ycz";//已存在
        }
        string str = string.Empty;
        if (id != "" && id != null)
        {
            Hi.Model.BD_AttributeValues model = new Hi.BLL.BD_AttributeValues().GetModel(Convert.ToInt32(id));
            if (model != null)
            {
                model.ts = DateTime.Now;
                model.modifyuser = this.UserID;
                model.AttrValue = value;
                model.CompID = this.CompID;
                bool bol = new Hi.BLL.BD_AttributeValues().Update(model);
                return "cg";
            }
        }
        return str;
    }
    /// <summary>
    /// 绑定
    /// </summary>
    /// <param name="id"></param>
    public string Bind2(string id)
    {
        DataTable dt = new Hi.BLL.BD_Attribute().GetAttributrList(id, this.CompID.ToString());//获取属性以及属性值
        if (dt.Rows.Count != 0)
        {
            return ConvertJson.ToJson(dt);
        }
        else
        {
            Hi.Model.BD_Attribute model = new Hi.BLL.BD_Attribute().GetModel(Convert.ToInt32(id));
            List<Hi.Model.BD_Attribute> lsit = new List<Hi.Model.BD_Attribute>();
            lsit.Add(model);
            DataTable dt2 = Common.FillDataTable<Hi.Model.BD_Attribute>(lsit);
            if (dt2.Rows.Count != 0)
            {
                return ConvertJson.ToJson(dt2);
            }
            return "[{}]";
        }
    }
    /// <summary>
    /// 属性值添加
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public string AddAttributeValue(string value, string id)
    {
        value = Common.NoHTML(value);
        var num = new Hi.BLL.BD_AttributeValues().GetList("", "attributeid=" + id + " and attrvalue='" + value + "' and ISNULL(dr,0)=0 and compid=" + this.CompID, "");
        if (num.Count > 0)
        {
            return "ycz";//已存在
        }
        Hi.Model.BD_AttributeValues model = new Hi.Model.BD_AttributeValues();
        model.AttrValue = value;
        model.AttributeID = Convert.ToInt32(id);
        model.CreateDate = DateTime.Now;
        model.CreateUserID = this.UserID;
        model.ts = DateTime.Now;
        model.CompID = this.CompID;
        model.modifyuser = this.UserID;
        model.IsEnabled = 1;
        int count = new Hi.BLL.BD_AttributeValues().Add(model);
        if (count > 0)
        {
            return count.ToString();
        }
        else
        {
            return "";
        }
    }
    /// <summary>
    /// 属性以及属性值添加
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public string AddAttributeValue(string value, string id, string attrName)
    {
        value = Common.NoHTML(value);
        attrName = Common.NoHTML(attrName);
        int attrId = 0;
        if (id == "" || id == null)
        {
            if (IsExist("attributeName", attrName, ""))
            {
                return "sxycz";
            }
            Hi.Model.BD_Attribute l = new Hi.Model.BD_Attribute();
            l.AttributeName = attrName;
            l.CreateDate = DateTime.Now;
            l.CreateUserID = this.UserID;
            l.ts = DateTime.Now;
            l.CompID = this.CompID;
            l.modifyuser = this.UserID;
            l.IsEnabled = 1;
            attrId = new Hi.BLL.BD_Attribute().Add(l);
        }
        else
        {
            attrId = Convert.ToInt32(id);
        }
        var num = new Hi.BLL.BD_AttributeValues().GetList("", "attributeid=" + attrId + " and attrvalue='" + value + "' and ISNULL(dr,0)=0 and Compid=" + this.CompID, "");
        if (num.Count > 0)
        {
            return "ycz";//已存在
        }
        Hi.Model.BD_AttributeValues model = new Hi.Model.BD_AttributeValues();
        model.AttrValue = value;
        model.AttributeID = Convert.ToInt32(attrId);
        model.CreateDate = DateTime.Now;
        model.CreateUserID = this.UserID;
        model.ts = DateTime.Now;
        model.CompID = this.CompID;
        model.modifyuser = this.UserID;
        model.IsEnabled = 1;
        int count = new Hi.BLL.BD_AttributeValues().Add(model);
        if (count > 0)
        {
            return attrId.ToString();
        }
        else
        {
            return "";
        }
    }
    /// <summary>
    /// 分页控件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();

        string strWhere = Where();
        ViewState["strWhere"] = strWhere;
        Bind();
    }
    protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        var attrItemId = Convert.ToInt32(e.CommandArgument.ToString());
        switch (e.CommandName)
        {
            case "delete":
                new Hi.BLL.BD_AttributeValues().Delete(attrItemId);
                break;
        }
        Response.Redirect(this.Request.Url.ToString());
    }
    /// <summary>
    /// 判断是否为纯数字
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    public bool bolNum(string temp)
    {
        if (temp == "")
        {
            return false;
        }
        for (int i = 0; i < temp.Length; i++)
        {
            byte tempbyte = Convert.ToByte(temp[i]);
            if ((tempbyte < 48) || tempbyte > 57)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 添加属性
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (bolNum(hideadd.Value))
            {
                Hi.Model.BD_Attribute attribute = new Hi.BLL.BD_Attribute().GetModel(Convert.ToInt32(hideadd.Value));
                if (IsExist("attributeName", this.txtAttributeName.Value.Trim(), hideadd.Value))
                {
                    JScript.AlertMethod(this, "属性已存在", JScript.IconOption.错误);
                    return;
                }
                attribute.AttributeName =Common.NoHTML( this.txtAttributeName.Value.Trim());
                attribute.Memo =Common.NoHTML( this.txtMemo.Value.Trim());
                attribute.CreateUserID = this.UserID;
                attribute.ID = Convert.ToInt32(hideadd.Value);
                attribute.ts = DateTime.Now;
                attribute.CompID = this.CompID;
                attribute.modifyuser = this.UserID;
                bool k = new Hi.BLL.BD_Attribute().Update(attribute);

                Bind();
                txtAttributeName.Value = string.Empty;
                txtMemo.Value = string.Empty;
                hideadd.Value = "";

            }
            else
            {
                //赋值
                string attributeName =Common.NoHTML( txtAttributeName.Value.Trim());//属性名称
                string memo =Common.NoHTML( txtMemo.Value.Trim());//排序编号
                if (IsExist("attributeName", attributeName, ""))
                {
                    JScript.AlertMethod(this, "属性已存在", JScript.IconOption.错误);
                    return;
                }
                Hi.Model.BD_Attribute attribute = new Hi.Model.BD_Attribute();
                attribute.AttributeName = attributeName;
                attribute.Memo = memo;
                attribute.CreateUserID = this.UserID;
                attribute.CreateDate = DateTime.Now;
                attribute.CreateDate = DateTime.Now;//创建时间
                attribute.ts = DateTime.Now;
                attribute.CompID = this.CompID;
                attribute.modifyuser = this.UserID;
                attribute.IsEnabled = 1;
                /*****执行*****/
                int k = new Hi.BLL.BD_Attribute().Add(attribute);
                if (k > 0)
                {
                    Bind();
                    txtAttributeName.Value = string.Empty;
                    hideadd.Value = "";
                }
                else
                {
                    JScript.AlertMethod(this, "属性添加失败", JScript.IconOption.错误);
                    return;
                }
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    /// <summary>
    /// 判断某属性某值是否存在
    /// </summary>
    /// <param name="Id">ID</param>
    /// <returns></returns>
    public bool IsExist(string value, string text, string id)
    {
        string where = string.Empty;
        if (!Util.IsEmpty(id))
        {
            where = " and id!=" + id;
        }
        bool bfg = false;
        if (!string.IsNullOrEmpty(text))
        {
            List<Hi.Model.BD_Attribute> List = new Hi.BLL.BD_Attribute().GetList(" top 1 *", value + "='" + text + "' and ISNULL(dr,0)=0 and compid=" + this.CompID + where, null);
            if (List.Count > 0)
            {
                bfg = true;
            }
        }
        return bfg;
    }
    /// <summary>
    /// 判断某属性某值是否存在
    /// </summary>
    /// <param name="Id">ID</param>
    /// <returns></returns>
    public bool IsExist(int id, string value)
    {
        bool bfg = false;
        if (id != 0)
        {
            var list = new Hi.BLL.BD_Attribute().GetList("", "id<>" + id + " and attributename='" + value + "' and ISNULL(dr,0)=0 and compid=" + this.CompID, "");
            if (list.Count > 0)
            {
                bfg = true;
            }
        }
        else
        {
            if (IsExist("attributeName", value, ""))
            {
                bfg = true;
            }
        }
        return bfg;
    }
    /// <summary>
    /// 编辑属性
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            //赋值
            string attributeName =Common.NoHTML( txtAttributeNames.Value.Trim());//属性名称
            string memo =Common.NoHTML( txtMemos.Value.Trim());//属性备注
            Hi.Model.BD_Attribute attribute = new Hi.BLL.BD_Attribute().GetModel(Convert.ToInt32(hideAttrId.Value));
            var list = new Hi.BLL.BD_Attribute().GetList("", "id<>" + hideAttrId.Value + " and attributename='" + attributeName + "' and ISNULL(dr,0)=0 and compid=" + this.CompID, "");
            if (list.Count > 0)
            {
                JScript.AlertMethod(this, "属性名称已存在", JScript.IconOption.错误);
                return;
            }
            else
            {
                attribute.AttributeName = attributeName;
            }
            attribute.AttributeName = attributeName;
            attribute.Memo = memo;
            attribute.CreateUserID = this.UserID;
            attribute.ts = DateTime.Now;
            attribute.CompID = this.CompID;
            attribute.modifyuser = this.UserID;
            attribute.IsEnabled = 1;
            /*****执行*****/
            bool k = new Hi.BLL.BD_Attribute().Update(attribute);
            if (k)
            {
                Bind();
                txtAttributeNames.Value = string.Empty;
            }
            else
            {
                JScript.AlertMethod(this, "属性编辑失败", JScript.IconOption.错误);
                return;
            }

        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
    /// <summary>
    /// 备注
    /// </summary>
    /// <returns></returns>
    public string GetMemo(object obj)
    {
        if (obj == null)
        {
            return "";
        }
        else
        {
            if (obj.ToString() == "")
            {
                return "";
            }
            else
            {
                return "(" + obj.ToString() + ")";
            }

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Company_Goods_GoodsTemplateList : CompPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            Bind();//模板列表
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
        List<Hi.Model.BD_Template> l = new Hi.BLL.BD_Template().GetList(Pager.PageSize, Pager.CurrentPageIndex, "id", false, strWhere, out pageCount, out Counts);
        this.rptTemplate.DataSource = l;
        this.rptTemplate.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }
    /// <summary>
    /// 根据属性
    /// </summary>
    /// <returns></returns>
    public string GetTemplateList(int str)
    {
        string list = string.Empty;
        List<Hi.Model.BD_TemplateAttribute> ll = new Hi.BLL.BD_TemplateAttribute().GetList("", "TemplateID=" + str + " and ISNULL(dr,0)=0 and ComPid=" + this.CompID, "");
        if (ll.Count > 0)
        {
            foreach (Hi.Model.BD_TemplateAttribute item in ll)
            {
                Hi.Model.BD_Attribute model = new Hi.BLL.BD_Attribute().GetModel(item.AttributeID);
                string str2 = model.AttributeName;
                if (model.Memo != "")
                {
                    str2 += "(" + model.Memo + ")";
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
    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    private string Where()
    {
        string strWhere = string.Empty;
        //赋值
        string TempName =Common.NoHTML( this.txtTemplate.Value.Trim().Replace("'", "''"));//模板名称
        if (!Util.IsEmpty(TempName))
        {
            strWhere += string.Format(" and Templatename like '%{0}%' and ISNULL(dr,0)=0 and Compid=" + this.CompID, TempName);

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
        SqlTransaction Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
        try
        {
            List<int> l = new List<int>();
            foreach (RepeaterItem row in this.rptTemplate.Items)
            {
                CheckBox cb = row.FindControl("CB_SelItem") as CheckBox;
                if (cb != null && cb.Checked)
                {
                    HiddenField fld = row.FindControl("HF_Id") as HiddenField;
                    if (fld != null)
                    {
                        int id = Convert.ToInt32(fld.Value);
                        l.Add(id);
                        Hi.Model.BD_Template model = new Hi.BLL.BD_Template().GetModel(id, Tran);

                        if (model != null)
                        {
                            model.ts = DateTime.Now;
                            model.modifyuser = this.UserID;
                            model.dr = 1;
                            model.CompID = this.CompID;
                            bool bol = new Hi.BLL.BD_Template().Update(model, Tran);
                            List<Hi.Model.BD_TemplateAttribute> ll = new Hi.BLL.BD_TemplateAttribute().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and templateid=" + model.ID, "", Tran);
                            if (ll.Count > 0)
                            {
                                foreach (Hi.Model.BD_TemplateAttribute item in ll)
                                {
                                    Hi.Model.BD_TemplateAttribute model2 = new Hi.BLL.BD_TemplateAttribute().GetModel(item.ID);
                                    model2.CompID = this.CompID;
                                    model2.modifyuser = this.UserID;
                                    model2.ts = DateTime.Now;
                                    model2.dr = 1;
                                    new Hi.BLL.BD_TemplateAttribute().Update(model2, Tran);
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
            JScript.AlertMethod(this, "删除失败了",JScript.IconOption.错误);
            return;
        }
        finally
        {
            DBUtility.SqlHelper.ConnectionClose();
            Bind();
        }

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
}
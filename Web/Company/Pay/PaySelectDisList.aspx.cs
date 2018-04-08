using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Company_Pay_PaySelectDisList : CompPageBase
{
    Hi.BLL.BD_Distributor DbutorBll = new Hi.BLL.BD_Distributor();
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtPager.Value = "7";
            IniPage();
            Bind();
        }
    }

    /// <summary>
    /// 初始化页面
    /// </summary>
    protected void IniPage()
    {
        //返回函数名
        string ReturnFunc = Request.QueryString["ReturnFunc"] + "";
        if (ReturnFunc == "")
        {
            ReturnFunc = "SelectMaterialsReturn";
        }
        ViewState["ReturnFunc"] = ReturnFunc;

      
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    protected void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strWhere = string.Empty;
        string quer_code=Request.QueryString["code"].ToString();
        string quer_stor=Request.QueryString["Store"] ;

        if (quer_code!="")
        {
            string code = Request.QueryString["code"].ToString();
            strWhere += " and ID not in (" + code.Substring(0, code.Length - 1) + ")";
            

        }
        if (this.txtPager.Value.Trim().ToString() != "")
        {
            if (this.txtPager.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPager.Value = "100";
            }
            else
            {
                Pager.PageSize = this.txtPager.Value.Trim().ToInt(0);
            }          
        }
        
        if (ViewState["strWhere"] != null)
        {
            strWhere += ViewState["strWhere"].ToString();
        }
        strWhere += "  and AuditState=2 and IsEnabled=1 and CompID=" + CompID;

        List<Hi.Model.BD_Distributor> BdutorModel = DbutorBll.GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, strWhere, out pageCount, out Counts);

        this.rpStockApplyList.DataSource = BdutorModel;
        this.rpStockApplyList.DataBind();

        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    /// <summary>
    /// 获取选中的Id
    /// </summary>
    /// <returns></returns>
    public string GetAllStr()
    {
        string str = null;
        if (ViewState["chb"] == null)
        {
            List<int> l = new List<int>();
            foreach (RepeaterItem row in rpStockApplyList.Items)
            {
                CheckBox cb = row.FindControl("CB_SelItem") as CheckBox;
                if (cb != null && cb.Checked)
                {
                    HiddenField fld = row.FindControl("Hd_Ids") as HiddenField;
                    if (fld != null)
                    {
                        int id = Convert.ToInt32(fld.Value);
                        str += id + ",";
                    }
                }
            }
        }
        else
        {
            foreach (RepeaterItem row in rpStockApplyList.Items)
            {
                CheckBox cb = row.FindControl("CB_SelItem") as CheckBox;
                if (cb != null && cb.Checked)
                {
                    HiddenField fld = row.FindControl("Hd_Ids") as HiddenField;
                    if (fld != null)
                    {
                        int id = Convert.ToInt32(fld.Value);
                        str += id + ",";
                    }
                }
            }

            List<string> l = ViewState["chb"] as List<string>;
            foreach (var item in l)
            {
                str += item + ",";
            }
        }

        return str;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SubOk_Click(object sender, EventArgs e)
    {
        string Id = GetAllStr();
       
        this.ScriptManage.InnerHtml = string.Format("<script>doSelectMaterials('{0}');</script>", Id);
    }

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        PageChB();
        Bind();
    }

    /// <summary>
    /// 分页保存选中的checkbox中的
    /// </summary>
    public void PageChB()
    {

        if (ViewState["chb"] == null)
        {
            List<string> l = new List<string>();
            foreach (RepeaterItem row in rpStockApplyList.Items)
            {
                CheckBox cb = row.FindControl("CB_SelItem") as CheckBox;
                if (cb != null && cb.Checked)
                {
                    HiddenField fld = row.FindControl("HF_Id") as HiddenField;
                    if (fld != null)
                    {
                        int id = Convert.ToInt32(fld.Value);

                        //if (cb.Checked)
                        //{
                        l.Add(id.ToString());
                        //}
                        //else
                        //{
                        //    if (l.Contains(id.ToString()))
                        //    {
                        //        l.Remove(id.ToString());
                        //    }
                        //}

                    }
                }
            }
            ViewState["chb"] = l;
        }
        else
        {
            List<string> l = ViewState["chb"] as List<string>;
            foreach (RepeaterItem row in rpStockApplyList.Items)
            {
                CheckBox cb = row.FindControl("CB_SelItem") as CheckBox;
                if (cb != null && cb.Checked)
                {
                    HiddenField fld = row.FindControl("HF_Id") as HiddenField;
                    if (fld != null)
                    {
                        int id = Convert.ToInt32(fld.Value);
                        l.Add(id.ToString());
                    }
                }
            }
            ViewState["chb"] = l;
        }       
    }

    ///<summary>
    /// 绑定数据时做判断,再设置checkbox的选中状态
    ///</summary>
    ///<param name="sender"></param>
    ///<param name="e"></param>
    protected void rpStockApplyList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        List<string> checkedModelIdList;
        var obj = ViewState["chb"];
        if (obj == null || (obj as List<string>) == null)
            checkedModelIdList = new List<string>();
        else
        {
            checkedModelIdList = obj as List<string>;
        }
        CheckBox chk = e.Item.FindControl("CB_SelItem") as CheckBox;
        if (chk != null)
        {
            HiddenField fld = e.Item.FindControl("HF_Id") as HiddenField;
            if (fld != null)
            {
                int id = Convert.ToInt32(fld.Value);
                if (checkedModelIdList.Contains(id.ToString()))
                {
                    chk.Checked = true;
                }
            }
        }
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strwhere = string.Empty;



        if (this.txtDisID.Value!= "")  //风格
        {

            strwhere += " and disname like '%" +Common.NoHTML( this.txtDisID.Value.Replace("'", "''")) + "%'";
        }

        ViewState["strWhere"] = strwhere;
        Bind();
    }

}
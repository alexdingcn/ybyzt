using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Admin_Systems_PaySelectBank : AdminPageBase
{
    
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
          //  this.txtPager.Value = "7";
            Bind();
        }
    }

   

    /// <summary>
    /// 绑定数据
    /// </summary>
    protected void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strWhere = string.Empty;
        Hi.BLL.PAY_BankInfo bankinfobll = new Hi.BLL.PAY_BankInfo();


        if (ViewState["strWhere"] != null)
        {
            strWhere += ViewState["strWhere"].ToString();
        }
        //strWhere += "  and AuditState=2 and IsEnabled=1 and CompID=" + LoginModel.IsLogined(this).CompID;
        //List<Hi.Model.PAY_BankInfo> list=bankinfobll.GetAllList();

        Pager.PageSize =20;  
        if (ViewState["strWhere"] != null)
        {
            strWhere += ViewState["strWhere"].ToString();
        }
        strWhere += "  and vdef1 in (0,1,2) ";

        List<Hi.Model.PAY_BankInfo> BdutorModel = bankinfobll.GetList(Pager.PageSize, Pager.CurrentPageIndex, "bankcode", true, strWhere, out pageCount, out Counts);

        this.rptOtherBank.DataSource = BdutorModel;
        this.rptOtherBank.DataBind();

        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }


    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        
        Bind();
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SubOk_Click(object sender, EventArgs e)
    {
        int  Id =Convert.ToInt32(Request["Selectbank"]);

        Hi.Model.PAY_BankInfo bankModel = new Hi.BLL.PAY_BankInfo().GetModel(Id);
        string code = string.Empty;
        string name = string.Empty; 
        //Response.Redirect("StockOutAdd.aspx?Id=" + Id);
        if (bankModel == null)
        {

            JScript.AlertAndRedirect("请选择银行！", "PaySelectBank.aspx");
            return;
        }
        else 
        {
             code = bankModel.BankCode;
             name = bankModel.BankName;
        }
        this.ScriptManage.InnerHtml = string.Format("<script>doSelectMaterials('{0}','{1}');</script>",code,name);
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
        string strWhere = string.Empty;
        string bankname=Common.NoHTML(this.txtbankname.Value);
        if (!string.IsNullOrEmpty(bankname))  //风格
        {
            strWhere += " and bankname like '%" + bankname + "%'";
        }

        ViewState["strWhere"] = strWhere;
        Bind();
    }
   

}
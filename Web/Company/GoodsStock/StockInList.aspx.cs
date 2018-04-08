using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Company_GoodsStock_StockInList : CompPageBase
{
    public string PageIndex = "1";//当前页
    public string strwhere = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
            string type = Request["type"];//判断是入库还是出库
            if (type == "2")//商品出库
            {   
                this.Title = "商品出库列表";
                this.tiele.InnerHtml = "商品出库列表";
                this.thNO.InnerHtml = "出库单号";
                this.thDate.InnerHtml = "出库时间";
                this.thType.InnerHtml = "出库类型";
                this.btnAdd.InnerHtml="<span><img src = \"../images/t01.png\"/></span><font>新增出库</font>";
                ListItem item0 = new ListItem("全部", "全部");
                ListItem item = new ListItem("销售出库", "销售出库");
                ListItem item1 = new ListItem("盘点出库", "盘点出库");
                ListItem item2 = new ListItem("其他出库", "其他出库");
                this.DropDownList1.Items.Clear();
                this.DropDownList1.Items.Insert(0, item0);
                this.DropDownList1.Items.Insert(1, item);
                this.DropDownList1.Items.Insert(2, item1);
                this.DropDownList1.Items.Insert(3, item2);
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
        //根据单号搜索
        if (!string.IsNullOrWhiteSpace(this.txtReceiptNo.Value.Trim()))
        {
            strwhere += " and OrderNO like '%"+ this.txtReceiptNo.Value.Trim() + "%'";
        }
        if (!string.IsNullOrWhiteSpace(this.txtCreateDate.Value.Trim()))
        {
            strwhere += " and ChkDate>'"+ this.txtCreateDate.Value.Trim() + "'";
        }
        if (!string.IsNullOrWhiteSpace(this.txtEndCreateDate.Value.Trim()))
        {
            strwhere += " and ChkDate<'" +(Convert.ToDateTime( this.txtEndCreateDate.Value.Trim()).AddDays(1)) + "'";
        }
        if (this.DropDownList1.Text != "全部")
        {
            strwhere += " and StockType='" + this.DropDownList1.Text + "'";
        }
        if (this.DropDownList2.Text != "1")
        {
            strwhere += " and State='" + this.DropDownList2.Text + "'";
        }
        PageIndex = Pager.CurrentPageIndex.ToString();
        Bind();
    }

    /// <summary>
    /// 分页控件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        PageIndex = Pager.CurrentPageIndex.ToString();
        Bind();
    }
   /// <summary>
   /// 绑定事件
   /// </summary>
    public void Bind()
    {
        int Counts = 0;
        string type = Request["type"];
        strwhere += " and Type='" + type + "' and dr=0 and CompID='" + this.CompID + "'";
        DataTable dt = new Hi.BLL.DIS_StockOrder().GetListPage(PageSize,Convert.ToInt32(PageIndex), strwhere);
        this.Repeater1.DataSource = dt;
        this.Repeater1.DataBind();
        Counts = new Hi.BLL.DIS_StockOrder().GetPageCount(strwhere);
        Pager.RecordCount = Counts;
        PageIndex = Pager.CurrentPageIndex.ToString();
    }

    /// <summary>
    /// 根据ID获取名称
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public string GetName(int ID)
    {
       Hi.Model.SYS_Users user= new Hi.BLL.SYS_Users().GetModel(ID);
        if (user != null)
        {
            return user.TrueName;
        }
        else
        {
            return ID.ToString();
        }
    }
}
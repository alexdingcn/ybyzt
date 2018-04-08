using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_Systems_GoodsInfoList : AdminPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        string Action = Request["Action"] + "";
        string OrgID = Request["OrgID"] + "";
        if (Action == "Action")
        {
            Response.Write(Common.getsaleman(OrgID));
            Response.End();
        }
        
        if (!IsPostBack)
        {
            Common.BindOrgSale(Org, SaleMan, "全部");
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            Bind();
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
        Bind();
    }
    /// <summary>
    /// 搜索按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Bind();
    }
    /// <summary>
    /// 商品列表绑定
    /// </summary>
    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strWhere = Where();
        //每页显示的数据设置
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
        string JoinTableStr = " BD_Goods inner join BD_GoodsInfo bginfo on BD_Goods.ID= bginfo.GoodsID and BD_Goods.dr=0 and  bginfo.dr=0  left join bd_company bcp on bginfo.compid=bcp.id and bcp.dr=0  left join BD_GoodsCategory bgcate on BD_goods.CategoryID=bgcate.id ";
        DataTable DtGoodsInfo = new Hi.BLL.BD_Goods().GetList(Pager.PageSize, Pager.CurrentPageIndex, "BD_Goods.IsFirstShow desc,BD_Goods.Sortindex desc,bginfo.ID", true, " IsFirstShow,BD_Goods.Sortindex, BD_Goods.GoodsName,bginfo.GoodsID,bginfo.Compid,bginfo.ID,bcp.CompName,BD_goods.Unit,BD_goods.Pic2,CategoryName,ValueInfo,bginfo.isOffline ", JoinTableStr, strWhere, out pageCount, out Counts, "", false);
        this.rptGoods.DataSource = DtGoodsInfo;
        this.rptGoods.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
        this.Org.SelectedValue = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();

    }
    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    private string Where()
    {
        string strWhere = " ";
        //赋值
        string idlist = string.Empty;
        string goodsName = Common.NoHTML(this.txtGoodsName.Value.Trim().Replace("'", "''"));//商品名称
        string isoffline = Common.NoHTML(this.ddlState.SelectedItem.Value.Trim());//状态
        if (!Util.IsEmpty(goodsName))
        {
            strWhere += " and BD_Goods.goodsName like '%" + goodsName + "%'"; ;
        }
        if (isoffline != "")
        {
            strWhere += " and bginfo.isoffline=" + isoffline;
        }
        if (!string.IsNullOrEmpty(txtCompName.Value.Trim()))
        {
            strWhere += " and bcp.CompName like '%" + Common.NoHTML(txtCompName.Value.Trim()) + "%' ";
        }
        if (Org.SelectedValue != "-1")
        {
            string org = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
            strWhere += " and  bcp.OrgID='" + Common.NoHTML(org) + "' ";
        }
        if (salemanid.Value != "-1" && salemanid.Value != "")
        {
            strWhere += " and bcp.SalesManID='" + salemanid.Value + "' ";
        }

        //if (SalesManID != null && SalesManID != 0)
        //{
        //    strWhere += " and BD_Goods.CompID in (select ID from [dbo].[BD_Company] where orgid=" + OrgID + " and SalesManID=" + SalesManID + " and isnull(dr,0)=0) ";
        //}
        if (SalesManID > 0 || OrgID > 0)
        {
            string whereIn = string.Empty;
            if (OrgID > 0)
            {
                whereIn += "  and OrgID=" + OrgID + "";
            }
            if (SalesManID > 0)
            {
                whereIn += " and SalesManID=" + SalesManID + "";
            }
            strWhere += " and BD_Goods.CompID in (select ID from [dbo].[BD_Company] where  isnull(dr,0)=0  "+ whereIn + ") ";
        }
        return strWhere;
    }
    /// <summary>
    /// 得到属性
    /// </summary>
    /// <returns></returns>
    public string GoodsAttr2(string ValueInfo)
    {
        string str = ValueInfo;
        string str2 = string.Empty;
        if (!Util.IsEmpty(str))
        {
            string[] lsit = { };
            string[] lsit2 = { };
            lsit = str.Replace(';', '；').Split('；');
            for (int i = 0; i < lsit.Length; i++)
            {
                if (lsit[i] != "")
                {
                    lsit2 = lsit[i].Split(':');
                    str2 += lsit2[0] + "：" + "<label style='color:#0080b8'>" + lsit2[1] + "</label>" + "；";
                }
            }
        }
        return str2;
    }

   

    public string GetPicURL(string Pic)
    {
        if (!Util.IsEmpty(Pic))
        {
            if (Pic != "X")
            {
                return Common.GetWebConfigKey("ImgViewPath") + "GoodsImg/" + Pic;
            }
            else
            {
                return "../../images/havenopicsmallest.gif";
            }
        }
        return "../../images/havenopicsmallest.gif";

    }
}
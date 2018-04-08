using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class Company_Goods_DisPrice : CompPageBase
{
    public int num = 0;
    public string page;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //下拉代理商的绑定
            List<Hi.Model.BD_Distributor> ll = new Hi.BLL.BD_Distributor().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID, "");
            ddlDisList.DataSource = ll;
            ddlDisList.DataTextField = "disname";
            ddlDisList.DataValueField = "id";
            ddlDisList.DataBind();
            ddlDisList.Items.Insert(0, new ListItem("请选择", ""));
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
        }
    }
    /// <summary>
    /// 调价记录列表绑定
    /// </summary>
    public void Bind()
    {
        string strWhere =string.Empty;
        if (this.txtPager.Value.Trim().ToString() != "" && this.txtPager.Value.Trim().ToString() != "0")
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
        string sql = string.Format(@"select a.ID,a.SalePrice,b.GoodsName from bd_goodsinfo as a 
                                    left join BD_Goods as b on a.GoodsID=b.ID 
                                    where a.CompID={0} and b.CompID={0}  and a.IsEnabled=1
                                     and isnull(a.dr,0)=0 and b.IsEnabled=1 
                                     and ISNULL(b.dr,0)=0 {1} order by a.id desc", this.CompID, strWhere);
        //DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql);
        //this.rptDisPrice.DataSource = ds;
        //this.rptDisPrice.DataBind();

        Pagger pagger = new Pagger(sql);
        Pager.RecordCount = pagger.getDataCount();
        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);

        this.rptDisPrice.DataSource = dt;
        this.rptDisPrice.DataBind();
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
        num = 1;
    }
    /// <summary>
    /// 得到属性
    /// </summary>
    /// <returns></returns>
    public string GoodsAttr(string id)
    {
        string str = string.Empty;
        string str2 = string.Empty;
        Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(id));
        if (model != null)
        {
            str = model.ValueInfo;
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
            else
            {
                str2 = Common.GetGoodsMemo(Convert.ToInt32(model.GoodsID));
            }
        }
        return str2;
    }
    /// <summary>
    /// 搜索条件
    /// </summary>
    /// <returns></returns>
    public string Where()
    {
        string where = string.Empty;
        string disId = this.ddlDisList.SelectedValue.Trim();//代理商
        string goodsName = this.txtGoodsName.Value.Trim().Replace("'", "''");//商品名称
        string hideID = this.txtCategory.treeId.Trim();//商品分类id
        if (!Util.IsEmpty(goodsName))
        {
            where += "and b.GoodsName like '%" + goodsName + "%'";
        }

        if (!Util.IsEmpty(hideID) && txtCategory.treeName != "")
        {
            string cateID = Common.CategoryId(Convert.ToInt32(hideID), this.CompID);//商品分类递归
            where += " and b.categoryID in(" + cateID + ") ";
        }

        //string str = string.Empty;
        //if (!Util.IsEmpty(disId))
        //{
        //    Hi.Model.BD_Distributor model2 = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(disId));
        //    if (model2 != null)
        //    {
        //        List<Hi.Model.BD_GoodsAreas> l = new Hi.BLL.BD_GoodsAreas().GetList("", "isnull(dr,0)=0 and compid=" + this.CompID + " and areaid=" + model2.AreaID, "");
        //        if (l.Count > 0)
        //        {
        //            foreach (Hi.Model.BD_GoodsAreas item in l)
        //            {
        //                str += item.GoodsID + ",";
        //            }
        //            if (!Util.IsEmpty(str))
        //            {
        //                str = str.Substring(0, str.Length - 1);
        //            }
        //            where += " and a.goodsId not in('" + str + "')";
        //        }
        //        else {
        //            where += " and a.goodsId not in('" + str + "')";
        //        }
        //    }
        //}

        return where;
    }
    /// <summary>
    /// 获取代理商价格
    /// </summary>
    /// <returns></returns>
    public string GetPrice(string id, string price)
    {
        if (!Util.IsEmpty(this.ddlDisList.SelectedValue.Trim()))
        {
            List<Hi.Model.BD_GoodsPrice> l = new Hi.BLL.BD_GoodsPrice().GetList("", "isnull(dr,0)=0 and isenabled=1 and disid=" + this.ddlDisList.SelectedValue.Trim() + " and compid=" + this.CompID + " and goodsinfoid=" + id, "");
            if (l.Count > 0)
            {
                return l[0].TinkerPrice.ToString();
            }
            else
            {
                Hi.Model.BD_GoodsInfo lll = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(id));
                if (lll != null)
                {
                    return lll.TinkerPrice.ToString();
                }
                else
                {
                    return price;
                }
            }
        }
        else
        {
            return price;
        }
    }
    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        string strWhere = Where();
        ViewState["strWhere"] = strWhere;
        Bind();
        num = 1;
    }

}
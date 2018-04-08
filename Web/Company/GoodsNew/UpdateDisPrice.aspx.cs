using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Company_Goods_UpdateDisPrice : CompPageBase
{
    public string page;
    public int num = 0;
    public int goodsInfoId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>$(\".txt_txtTypename\").css(\"width\", \"150px\");</script>");
        object obj = Request["action"];
        if (obj != null)
        {
            string action = obj.ToString();
            if (action == "save")//销售价格调整
            {
                string id = Request["id"];//goodsinfo 表id
                string price = Request["price"];//销售价格
                Response.Write(UpdateGoodsInfo(id, price));
                Response.End();
            }
        }
        object obj2 = Request["goodsInfoId"];
        if (obj2 != null)
        {
            goodsInfoId = Convert.ToInt32(obj2.ToString() != "" ? obj2.ToString() : "0");
        }
        if (!IsPostBack)
        {
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            Bind();
        }
    }
    /// <summary>
    /// 绑定
    /// </summary>
    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strWhere = " and ISNULL(dr,0)=0 and IsEnabled=1 and ComPid=" + this.CompID;
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
        //List<Hi.Model.BD_Distributor> LDis = new Hi.BLL.BD_Distributor().GetList("", strWhere, "");
        //num = LDis.Count;
        //this.rptDis.DataSource = LDis;
        //this.rptDis.DataBind();
        List<Hi.Model.BD_Distributor> l = new Hi.BLL.BD_Distributor().GetList(Pager.PageSize, Pager.CurrentPageIndex, "id", true, strWhere, out pageCount, out Counts);
        num = l.Count;
        this.rptDis.DataSource = l;
        this.rptDis.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();

        if (goodsInfoId != 0)
        {
            Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(goodsInfoId);
            if (model != null)
            {
                Hi.Model.BD_Goods model2 = new Hi.BLL.BD_Goods().GetModel(model.GoodsID);
                if (model2 != null)
                {
                    this.lblGoodsName.InnerText = model2.GoodsName;
                    this.lblAttribute.InnerHtml = GoodsAttr(goodsInfoId);
                }
            }
        }
    }
    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    private string Where()
    {
        string typeId = this.txtDisType.typeId;//分类
        string strWhere = string.Empty;
        //赋值
        string disName = this.txtDisName.Value.Trim().Replace("'", "''");//代理商名称
        if (!Util.IsEmpty(typeId))
        {
            string typeIdList = Common.DisTypeId(Convert.ToInt32(typeId), this.CompID);//递归得到分类id
            strWhere += string.Format(" and distypeId in({0})", typeIdList);
        }
        if (!Util.IsEmpty(disName))
        {
            strWhere += string.Format(" and disname like '%{0}%'", disName);
        }
        return strWhere;
    }
    /// <summary>
    /// 搜索按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        txtDisType.CompID = CompID.ToString();
        string strWhere = Where();
        ViewState["strWhere"] = strWhere;
        Bind();
    }
    /// <summary>
    /// 分页控件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        txtDisType.CompID = CompID.ToString();
        page = Pager.CurrentPageIndex.ToString();
        string strWhere = Where();
        ViewState["strWhere"] = strWhere;
        Bind();
    }
    /// <summary>
    /// 得到属性
    /// </summary>
    /// <returns></returns>
    public string GoodsAttr(int id)
    {
        string str = string.Empty;
        string str2 = string.Empty;
        Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(id);
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
    /// 最新价格
    /// </summary>
    /// <returns></returns>
    public string GoodsTinkerPrice(int id, string disId, int type)
    {
        string tinkerprice = string.Empty;
        Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(id);
        if (model != null)
        {
            tinkerprice = model.TinkerPrice.ToString();
        }
        string price = string.Empty;
        if (type == 1)
        {
            price = decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(tinkerprice.ToString()).ToString())).ToString("#,##0.00");
        }
        else
        {
            price = decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(tinkerprice.ToString()).ToString())).ToString("0.00");
        }
        //if (disId.Split(',').Length == 1)
        //{
        //    List<Hi.Model.BD_DisPrice> ll = new Hi.BLL.BD_DisPrice().GetList("", "isnull(dr,0)=0 and isenabled=1 and compid=" + CompID + " and disids   like '%" + disId + "%'", " id desc");
        //    if (ll.Count > 0)
        //    {
        //        string disIdlist = string.Empty;
        //        foreach (Hi.Model.BD_DisPrice item in ll)
        //        {
        //            disIdlist += item.ID + ",";
        //        }
        List<Hi.Model.BD_GoodsPrice> l = new Hi.BLL.BD_GoodsPrice().GetList("", "isnull(dr,0)=0 and isenabled=1 and goodsinfoid =" + id + " and disId=" + disId + " and compId=" + this.CompID, "id desc");
        if (l.Count > 0)
        {
            foreach (Hi.Model.BD_GoodsPrice item in l)
            {
                if (type == 1)
                {
                    price = decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(item.TinkerPrice.ToString()).ToString())).ToString("#,##0.00");

                }
                else
                {
                    price = decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(item.TinkerPrice.ToString()).ToString())).ToString("0.00");

                }
            }
        }
        else
        {
            Hi.Model.BD_GoodsInfo lll = new Hi.BLL.BD_GoodsInfo().GetModel(id);
            if (lll != null)
            {
                if (type == 1)
                {
                    price = decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(lll.TinkerPrice.ToString()).ToString())).ToString("#,##0.00");
                }
                else
                {
                    price = decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(lll.TinkerPrice.ToString()).ToString())).ToString("0.00");

                }
            }
        }
        //}
        // }
        return price;
    }
    /// <summary>
    /// 调整价格
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string UpdateGoodsInfo(string id, string price)
    {
        string str = string.Empty;
        Hi.Model.BD_GoodsInfo model = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(id));
        if (model != null)
        {
            if (!Util.IsEmpty(price))
            {
                model.TinkerPrice = Convert.ToDecimal(price);
            }
            model.ts = DateTime.Now;
            model.modifyuser = this.UserID;
            bool bol = new Hi.BLL.BD_GoodsInfo().Update(model);
            if (bol)
            {
                str = "cg";
            }
        }
        return str;
    }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        SqlTransaction Tran = null;
        try
        {
            Tran = DBUtility.SqlHelper.CreateStoreTranSaction();
            List<Hi.Model.BD_GoodsPrice> llll = new List<Hi.Model.BD_GoodsPrice>();//修改
            List<Hi.Model.BD_GoodsPrice> llll2 = new List<Hi.Model.BD_GoodsPrice>();//新增
            string[] disIdList = Request["lblDisId"].Split(',');// this.hidDisId.Value.Trim();//代理商列表
            string[] priceList = Request["txtPrice"].Split(',');//价格
            for (int i = 0; i < disIdList.Length; i++)
            {
                List<Hi.Model.BD_GoodsPrice> lll = new Hi.BLL.BD_GoodsPrice().GetList("", "isnull(dr,0)=0 and isenabled=1 and disid = " + disIdList[i] + "and compid=" + this.CompID + " and  goodsinfoid=" + goodsInfoId, "", Tran);
                if (lll.Count > 0)
                {
                    foreach (Hi.Model.BD_GoodsPrice item2 in lll)
                    {
                        item2.IsEnabled = false;
                        item2.modifyuser = this.UserID;
                        item2.ts = DateTime.Now;
                        item2.CompID = this.CompID;
                        llll.Add(item2);
                    }
                }
                //新增
                Hi.Model.BD_GoodsPrice model3 = new Hi.Model.BD_GoodsPrice();
                //model3.DisPriceID = 0;
                model3.DisID = Convert.ToInt32(disIdList[i]);
                model3.CompID = this.CompID;
                model3.GoodsInfoID = goodsInfoId;
                model3.TinkerPrice = Convert.ToDecimal(priceList[i]);
                model3.IsEnabled = true;
                model3.CreateUserID = this.UserID;
                model3.CreateDate = DateTime.Now;
                model3.ts = DateTime.Now;
                model3.modifyuser = this.UserID;
                llll2.Add(model3);
            }
            new Hi.BLL.BD_GoodsPrice().Update(llll, Tran);
            new Hi.BLL.BD_GoodsPrice().Add(llll2, Tran);
            Tran.Commit();
            ClientScript.RegisterStartupScript(this.GetType(), "msg2", "<script>$(function(){ window.parent.layerCommon.layerClose('hid_Alert');})</script>");
        }
        catch (Exception)
        {

            if (Tran != null)
            {
                if (Tran.Connection != null)
                {
                    Tran.Rollback();
                }
            }
            JScript.AlertMethod(this, "价格调整失败", JScript.IconOption.错误);
            return;
        }
    }
}
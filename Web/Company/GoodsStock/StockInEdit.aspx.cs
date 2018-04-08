using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using DBUtility;

public partial class Company_GoodsStock_StockInEdit : CompPageBase
{
    public string TitleType = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        object obj = Request["action"];
        if (obj != null)
        {
            //默认绑定
            if (obj.ToString() == "dislist") 
            {
                Response.Write(disBing());
                Response.End();
            }
            //选中的商品
            if (obj.ToString() == "goodsInfo")
            {
                string goodsinfoid = Request["goodsInfoId"] + "";
                Response.Write(disBing(goodsinfoid));
                Response.End();
            }
        }
        if (!IsPostBack)
        {
            string  type = Request["type"];
            string no = Common.DesDecrypt(Request["no"], Common.EncryptKey);//有值是修改 
            if (type == "2")//商品出库
            {
                TitleType = "出库";
                this.Title = "商品新增出库单";
                ListItem item = new ListItem("销售出库", "销售出库");
                ListItem item1 = new ListItem("盘点出库", "盘点出库");
                ListItem item2 = new ListItem("其他出库", "其他出库");
                this.DropDownList1.Items.Clear();
                this.DropDownList1.Items.Insert(0, item);
                this.DropDownList1.Items.Insert(1, item1);
                this.DropDownList1.Items.Insert(2, item2);
                this.hid_Type.Value = "2";
                if (!string.IsNullOrWhiteSpace(no))
                {
                    this.Title = "修改出库单";
                    this.title.InnerText = "修改出库单";
                    this.hid_No.Value = no;
                    
                    Bind(2, no);
                }
                else
                {
                    this.Notex.Value = "自动生成";
                    this.title.InnerText = "新增出库";
                }
                
            }
            else if (type == "1")
            {
                TitleType = "入库";
                this.Title = "商品新增入库单";
                if (!string.IsNullOrWhiteSpace(no))
                {
                    this.title.InnerText = "修改入库单";
                    this.Title = "修改入库单";
                    this.hid_No.Value = no;
                    
                    Bind(1, no);
                }
                else
                {   
                    this.Notex.Value = "自动生成" ;
                }
            }
            this.hidCompId.Value = this.CompID.ToString();
        }
    }
    /// <summary>
    /// 选择商品
    /// </summary>
    /// <returns></returns>
    public string disBing(string goodsinfoid = "")
    {
        List<Hi.Model.DIS_GoodsStock> gstocklist = null;
        StringBuilder strwhere = new StringBuilder();
        //启用库存
        strwhere.AppendFormat("and Inventory>=0 and a.compid=" + this.CompID + " and b.compid=" + this.CompID);
        if (!Util.IsEmpty(goodsinfoid))
        {
            strwhere.AppendFormat(" and a.id in(" + goodsinfoid + ")");
            gstocklist = new Hi.BLL.DIS_GoodsStock().GetList("", " GoodsInfo in( " + goodsinfoid + " )", "");
        }
        DataTable dt = new Hi.BLL.BD_GoodsInfo().GetGoodsModel(strwhere.ToString()).Tables[0];

        if (dt != null && dt.Rows.Count > 0)
        {
            if (gstocklist != null && gstocklist.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    List<Hi.Model.DIS_GoodsStock> stock = gstocklist.FindAll(p => p.GoodsInfo == item["ID"].ToString());
                    if (stock != null && stock.Count > 0)
                        item["Inventory"] = stock[0].StockTotalNum;
                }
            }
        }
        return ConvertJson.ToJson(dt);
    }

    /// <summary>
    /// 修改绑定
    /// </summary>
    /// <param name="type">入库or出库</param>
    /// <param name="no">单据ID</param>
    public void Bind(int type,string no)
    {
        no = Common.NoHTML(no);
        List<Hi.Model.DIS_StockOrder> order = new Hi.BLL.DIS_StockOrder().GetList("OrderNO,StockType,Remark", " ID='" + no + "'", "ID");
        if (order.Count>0)
        {
            this.Notex.Value = order[0].OrderNO;
            this.DropDownList1.SelectedValue = order[0].StockType;
            this.Remarktex.Value = order[0].Remark;

            string sql = string.Format(@"select BD_GoodsInfo.ValueInfo,BD_Goods.Unit,DIS_StockInOut.ID,DIS_StockInOut.StockNum,DIS_StockInOut.Remark,DIS_StockInOut.GoodsID,DIS_StockInOut.BatchNO,DIS_StockInOut.validDate,
         BD_Goods.GoodsName, BD_Goods.Pic, BD_GoodsInfo.BarCode, BD_GoodsInfo.ID as GoodsinfoID from DIS_StockInOut  join BD_GoodsInfo on
         DIS_StockInOut.GoodsInfoID = BD_GoodsInfo.ID 
        join BD_Goods on BD_GoodsInfo.GoodsID = BD_Goods.ID and DIS_StockInOut.StockOrderID={0}", no);
        DataTable dt = new Hi.BLL.DIS_StockOrder().GetDataTable(sql);
            if(dt.Rows.Count>0)
             {
                this.tbodyTR.Visible = false;
                this.Repeater1.DataSource = dt;
                this.Repeater1.DataBind();
            }
        
        }
    }

}
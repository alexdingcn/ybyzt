using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using DBUtility;

public partial class Company_GoodsStock_InventoryEdit : CompPageBase
{
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
            string type = Request["type"];
            string no = Common.DesDecrypt(Request["no"], Common.EncryptKey); ;//解密详情ID;
            if (type=="0")
            {
                this.txt_orderno.Value = "自动生成";
                this.txt_ChkDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            } 
            if (type=="1")
            {
                this.title.InnerText = "库存盘点单修改";
                this.Title = "商品盘点修改";
                this.hid_No.Value = no;
                Bind(no);
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

    public void Bind(string no)
    {
        no = Common.NoHTML(no);
        List<Hi.Model.DIS_StockOrder> order = new Hi.BLL.DIS_StockOrder().GetList("OrderNO,ChkDate,Remark", " ID='" + no + "'", "ID");
        if (order.Count > 0)
        {
            this.txt_orderno.Value = order[0].OrderNO;
            this.txt_ChkDate.Value = order[0].ChkDate.ToShortDateString();
            this.txt_remark.Value = order[0].Remark;
            string sql = string.Format(@"select BD_GoodsInfo.ValueInfo,BD_Goods.Unit,DIS_StockChk.ID,DIS_StockChk.StockOldNum,DIS_StockChk.StockNum,DIS_StockChk.Remark,DIS_StockChk.GoodsID,
         BD_Goods.GoodsName, BD_Goods.Pic, BD_GoodsInfo.BarCode, BD_GoodsInfo.ID as GoodsinfoID from DIS_StockChk  join BD_GoodsInfo on
         DIS_StockChk.GoodsInfoID = BD_GoodsInfo.ID 
        join BD_Goods on DIS_StockChk.GoodsID = BD_Goods.ID and DIS_StockChk.StockOrderID={0}", no);
            DataTable dt = new Hi.BLL.DIS_StockOrder().GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                this.tbodyTR.Visible = false;
                this.Repeater1.DataSource = dt;
                this.Repeater1.DataBind();
            }
        }
    }
}
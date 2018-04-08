using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_GoodsStock_StockInInfo : CompPageBase
{
    public string TitleType = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string type = Request["type"];
            string no = Common.DesDecrypt(Request["no"], Common.EncryptKey);//有值是修改 
            hidOrderID.Value = Common.DesEncrypt(no.ToString(), Common.EncryptKey);
            if (type == "2")//商品出库
            {
                TitleType = "出库";
                this.Title = "商品出库单详细";
                this.title1.InnerText = "商品出库列表";
                this.title1.HRef = "StockInList.aspx?type=2";
                this.title2.InnerText = "商品出库明细";
                this.hid_No.Value = Server.UrlEncode(Request["no"]);
                this.hid_Type.Value = "2";
                Bind(2, no);
                if (!Common.HasRight(this.CompID, this.UserID, "1224"))
                    this.Sh.Visible = false;

            }
            else if (type == "1")
            {
                TitleType = "入库";
                this.Title = "商品入库单详细";
                this.title1.InnerText = "商品入库列表";
                this.title1.HRef = "StockInList.aspx?type=1";
                this.title2.InnerText = "商品入库明细";
                this.hid_No.Value = Server.UrlEncode(Request["no"]);
                this.hid_Type.Value = "1";
                Bind(1, no);
                if (!Common.HasRight(this.CompID, this.UserID, "1222"))
                    this.Sh.Visible = false;
            }
            this.hidCompId.Value = this.CompID.ToString();
            
        }
    }
    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 修改绑定
    /// </summary>
    /// <param name="type">入库or出库</param>
    /// <param name="no">单据ID</param>
    public void Bind(int type, string no)
    {
        no = Common.NoHTML(no);
        List<Hi.Model.DIS_StockOrder> order = new Hi.BLL.DIS_StockOrder().GetList("State,OrderNO,StockType,Remark,ChkDate", " ID='" + no + "'", "ID");
        if (order.Count > 0)
        {
            if (order[0].State == 2)
            {
                this.orderaudit.Visible = false;
                this.edit.Visible = false;
                this.del.Visible = false;
            }
            else
            {
                this.edit.Visible = true;
                this.orderaudit.Visible = true;
                this.del.Visible = true;
            }
            this.lblNo.InnerText = order[0].OrderNO;
            this.lblType.InnerText = order[0].StockType;
            this.lbldate.InnerText = order[0].ChkDate.ToString("yyyy-MM-dd");
            if (order[0].Remark != null && order[0].Remark.Length > 70)
            {
                string rmk = order[0].Remark.Substring(0, 70).Trim();
                this.lblRmk.InnerText = rmk + "......";
            }
            else
            {
                this.lblRmk.InnerText = order[0].Remark;
            }
            this.txtbox.InnerText = order[0].Remark;

            string sql = string.Format(@"select BD_GoodsInfo.ValueInfo,BD_Goods.Unit,DIS_StockInOut.ID,DIS_StockInOut.StockNum,DIS_StockInOut.BatchNO,DIS_StockInOut.validDate,DIS_StockInOut.Remark,DIS_StockInOut.GoodsID,
         BD_Goods.GoodsName, BD_Goods.Pic, BD_GoodsInfo.BarCode, BD_GoodsInfo.ID as GoodsinfoID from DIS_StockInOut  join BD_GoodsInfo on
         DIS_StockInOut.GoodsInfoID = BD_GoodsInfo.ID 
        join BD_Goods on BD_GoodsInfo.GoodsID = BD_Goods.ID and DIS_StockInOut.StockOrderID={0}", no);
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
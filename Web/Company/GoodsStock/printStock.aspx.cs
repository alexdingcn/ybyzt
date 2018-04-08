using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Company_GoodsStock_printStock : System.Web.UI.Page
{
    public string TitleType = "";
    public int KeyID = 0;
    public string codeno = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            databind();
        }
    }

    public void databind()
    {
        if (!string.IsNullOrEmpty(Request["KeyID"]))
            KeyID = Common.DesDecrypt((Request["KeyID"] + ""), Common.EncryptKey).ToInt(0);

        string where = " ISNULL(dr,0)=0 and ID=" + KeyID;

        List<Hi.Model.DIS_StockOrder> solist = new Hi.BLL.DIS_StockOrder().GetList("", where, "");

        if (solist != null && solist.Count > 0)
        {
            // 订单编号
            lblReceiptNo.InnerText = solist[0].OrderNO;
            codeno = solist[0].OrderNO.ToString();
            this.Image1.ImageUrl = "~/Distributor/newOrder/Code39.aspx?KeyID=" + codeno;
            lblCreateDate.InnerText =solist[0].CreateDate.ToString().ToDateTime().ToString("yyyy-MM-dd");
            this.lblType.InnerText = solist[0].StockType;

            if (solist[0].Type == 2)//商品出库
            {
                TitleType = "出库";
                this.Title = "商品出库单详细";
            }
            else if (solist[0].Type == 1)
            {
                TitleType = "入库";
                this.Title = "商品入库单详细";
            }

            //商品明细
            string goods = string.Format(@"select i.*,info.ValueInfo,g.GoodsCode,g.GoodsName,g.Unit,info.TinkerPrice from DIS_StockInOut i left join BD_GoodsInfo info on i.GoodsInfoID=info.ID left join BD_Goods g on info.GoodsID=g.ID where ISNULL(i.dr,0)=0 and i.StockOrderID={0}", KeyID);
            DataTable l = new Hi.BLL.DIS_StockInOut().GetDataTable(goods);
            if (l != null && l.Rows.Count > 0)
            {
                rptOrderD.DataSource = l;
                rptOrderD.DataBind();
            }
        }
    }

}
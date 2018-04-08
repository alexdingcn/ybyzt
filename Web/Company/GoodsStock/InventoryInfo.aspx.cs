using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_GoodsStock_InventoryInfo : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string no = Common.DesDecrypt(Request["no"], Common.EncryptKey);//解密详情ID
        if (!IsPostBack)
        {
            this.hid_No.Value = Common.DesEncrypt(no, Common.EncryptKey);
            Bind(no);
            if (!Common.HasRight(this.CompID, this.UserID, "1226"))
                this.SH.Visible = false;
        }
    }


    public void Bind(string no)
    {
        no = Common.NoHTML(no);
        List<Hi.Model.DIS_StockOrder> order = new Hi.BLL.DIS_StockOrder().GetList("OrderNO,ChkDate,Remark,State", " ID='" + no + "' and dr=0 ", "ID");
        if (order.Count > 0)
        {
            if (order[0].State==2)
            {
                this.edit.Visible = false;
                this.orderaudit.Visible = false;
                this.del.Visible = false;
            }
            else
            {
                this.edit.Visible = true;
                this.orderaudit.Visible = true;
                this.del.Visible = true;
            }
            this.lblNo.InnerText = order[0].OrderNO;
            this.lblType.InnerText = order[0].ChkDate.ToShortDateString();
            if (order[0].Remark.Length > 70)
            {
                string rmk = order[0].Remark.Substring(0, 70).Trim();
                this.lblRmk.InnerText = rmk + "......";
            }
            else
            {
                this.lblRmk.InnerText = order[0].Remark;
            }
            this.txtbox.InnerText = order[0].Remark;
            string sql = string.Format(@"select BD_GoodsInfo.ValueInfo,BD_Goods.Unit,DIS_StockChk.ID,DIS_StockChk.StockNum,DIS_StockChk.StockOldNum,DIS_StockChk.Remark,DIS_StockChk.GoodsID,
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_newOrder_amountof : System.Web.UI.Page
{
    //订单ID
    public int KeyID = 0;
    //代理商ID
    public int DisID = 0;
    //修改的类型  0、应付总额  1、修改运费
    public int type = 0;
    public string title = "应付总额";
    //原有金额
    public decimal t = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["type"]))
            type = Request["type"].ToString().ToInt(0);

        copetotal.Visible = false;
        PostFee.Visible = false;
        if (!string.IsNullOrEmpty(Request["t"]))
            t = (Request["t"] + "").ToDecimal(0);

        if (!string.IsNullOrEmpty(Request["KeyID"]))
            KeyID = Common.DesDecrypt(Request["KeyID"] + "", Common.EncryptKey).ToInt(0);

        if (type == 0)
        {
            copetotal.Visible = true;
            title = "应付总额";
            txttotal.Value = t.ToString("0.00");

            Hi.Model.DIS_Order ordermodel = new Hi.BLL.DIS_Order().GetModel(KeyID);
            if (ordermodel != null)
                hidts.Value = ordermodel.ts.ToString();

        }
        else if (type == 1)
        {
            PostFee.Visible = true;
            title = "修改运费";
            txtPostFee.Value = t.ToString("0.00");

            Hi.Model.DIS_Order ordermodel = new Hi.BLL.DIS_Order().GetModel(KeyID);
            if (ordermodel != null)
                hidts.Value = ordermodel.ts.ToString();
        }
        else if (type == 2)
        {
            PostFee.Visible = true;
            title = "修改运费";
            txtPostFee.Value = t.ToString("0.00");
        }

        this.hidOrderID.Value = (Request["KeyID"] + "").ToString();
        this.hidType.Value = type.ToString();
        this.hidT.Value = t.ToString();
    }
}
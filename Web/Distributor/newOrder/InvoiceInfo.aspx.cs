using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LitJson;
using System.Data;

public partial class Distributor_newOrder_InvoiceInfo : System.Web.UI.Page
{
    //订单ID
    public int KeyID = 0;
    //代理商ID
    public int DisID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //当订单ID为0时、新增订单，否则修改订单开票信息
            databind();
        }
    }

    public void databind()
    {
        if (!string.IsNullOrEmpty(Request["DisID"]))
            DisID = (Request["DisID"] + "").ToInt(0);

        hidDisID.Value = DisID.ToString();

        if ((Request["val"] + "") != "0")
        {
            int val = Request["val"].ToString().ToInt(0);

            if (val == 0)
            {
                this.checkbox_2_1.Checked = true;
            }
            else if (val == 1)
            {
                this.checkbox_2_2.Checked = true;
            }
            else if (val == 2)
            {
                this.checkbox_2_3.Checked = true;
            }

            this.txtLookUp.Value =Common.NoHTML( Request["Rise"].ToString());
            this.txtContext.Value =Common.NoHTML( Request["Context"].ToString());
            this.txtBank.Value = Common.NoHTML(Request["Bank"].ToString());
            this.txtAccount.Value = Common.NoHTML(Request["Account"].ToString());
            this.txtRegNo.Value = Common.NoHTML(Request["RegNo"].ToString());
        }
        else
        {
            //List<Hi.Model.BD_DisAccount> l = new Hi.BLL.BD_DisAccount().GetList("", " DisID=" + DisID + "and isnull(dr,0)=0", "");
            List<Hi.Model.YZT_FCmaterials> fCmaterialsList = new Hi.BLL.YZT_FCmaterials().GetList("", " DisID =" + DisID + " and dr=0 ", "");

            this.checkbox_2_1.Checked = true;

            if (fCmaterialsList != null && fCmaterialsList.Count > 0)
            {
                //hidDisAccID.Value = Common.NoHTML(l[0].ID.ToString());
                txtLookUp.Value = Common.NoHTML(fCmaterialsList[0].Rise.ToString());
                txtContext.Value = Common.NoHTML(fCmaterialsList[0].Content.ToString());

                DataTable dt = new Hi.BLL.PAY_PrePayment().GetDate("BankCode ,BankName", "PAY_BankInfo", "BankCode=" + fCmaterialsList[0].OBank);
                if (dt.Rows.Count > 0)
                {
                    txtBank.Value = Common.NoHTML(dt.Rows[0]["BankName"].ToString());
                }

                txtAccount.Value = Common.NoHTML(fCmaterialsList[0].OAccount.ToString());
                txtRegNo.Value = Common.NoHTML(fCmaterialsList[0].TRNumber.ToString());
            }
            else
            {
                Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(DisID);

                txtLookUp.Value = disModel.DisName;
                txtContext.Value = "商品明细";
            }
        }
    }
}
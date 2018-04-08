using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_CMerchants_CMerchantsInfo : DisPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bind();
        }
    }

    /// <summary>
    /// 招商信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void bind()
    {
        if (KeyID != 0)
        {
            Hi.Model.YZT_CMerchants cmModel = new Hi.BLL.YZT_CMerchants().GetModel(this.KeyID);

            this.lblCMCode.InnerText = cmModel.CMCode;
            this.lblCMName.InnerText = cmModel.CMName;
            this.lblCompName.InnerText = Common.GetCompValue(cmModel.CompID, "CompName").ToString();
            this.lblGoodsCode.InnerText = Common.GetGoodsName(cmModel.GoodsID.ToString(), "GoodsCode");
            this.lblGoodsName.InnerText = Common.GetGoodsName(cmModel.GoodsID.ToString(), "GoodsName");
            this.lblRemark.InnerText = cmModel.Remark;
            this.lblValueInfo.InnerText = Common.GetGoodsInfo(cmModel.GoodsID.ToString());
            this.lblInvalidDate.InnerText = cmModel.InvalidDate == DateTime.MinValue ? "" : cmModel.InvalidDate.ToString("yyyy-MM-dd");
            this.lblForceDate.InnerText = cmModel.ForceDate == DateTime.MinValue ? "" : cmModel.ForceDate.ToString("yyyy-MM-dd");
            
            if (cmModel.ProvideData.IndexOf("1") > -1)
            {
                this.lblchk1.Visible = true;
                this.chk1.Checked = true;
            }
            if (cmModel.ProvideData.IndexOf("2") > -1)
            {
                this.lblchk2.Visible = true;
                this.chk2.Checked = true;
            }
            if (cmModel.ProvideData.IndexOf("3") > -1)
            {
                this.lblchk3.Visible = true;
                this.chk3.Checked = true;
            }
            if (cmModel.ProvideData.IndexOf("4") > -1)
            {
                this.lblchk4.Visible = true;
                this.chk4.Checked = true;
            }
            //判断是否已存在通过的代理商
            //if (Common.isFC(KeyID.ToString(), cmModel.CompID.ToString()))
            //{
            //    this.btnCo.Visible = false;
            //}
        }
    }
}
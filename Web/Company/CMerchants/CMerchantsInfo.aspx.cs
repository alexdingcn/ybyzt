using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;
using System.Data;

/// <summary>
/// 招商信息新增
/// </summary>
public partial class Company_CMerchants_CMerchantsInfo : CompPageBase
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

            this.txtCMCode.Value = cmModel.CMCode;
            this.txtCMName.Value = cmModel.CMName;
            this.txtGoodsCode.Value = Common.GetGoodsName(cmModel.GoodsID.ToString(), "GoodsCode");
            this.txtGoodsName.Value = Common.GetGoodsName(cmModel.GoodsID.ToString(), "GoodsName");
            this.txtRemark.Value = cmModel.Remark;
            this.txtValueInfo.Value = Common.GetGoodsInfo(cmModel.GoodsID.ToString());
            this.txtInvalidDate.Value = cmModel.InvalidDate == DateTime.MinValue ? "" : cmModel.InvalidDate.ToString("yyyy-MM-dd");
            this.txtForceDate.Value = cmModel.ForceDate == DateTime.MinValue ? "" : cmModel.ForceDate.ToString("yyyy-MM-dd");
            this.txtCategoryID.Value=Common.GetCategoryName(Common.GetGoodsName(cmModel.GoodsID.ToString(), "CategoryID"));
            this.ddrtype.Value = cmModel.Type.ToString();

            this.libtnXianjia.Visible = false;
            this.libtnShangjia.Visible = false;
            if (cmModel.IsEnabled == 1)
            {
                this.libtnEdit.Visible = false;
                this.libtnXianjia.Visible = true;
            }
            else
            {
                this.libtnEdit.Visible = true;
                this.libtnShangjia.Visible = true;
            }

            if (cmModel.Type.ToString() == "2" || cmModel.Type.ToString() == "3")
            {
                string sql = @"select cm.ID,d.DisID,dis.DisName,a.AreaID,da.AreaName,a.Province,a.City,a.Area from YZT_CMerchants cm left join 
YZT_FCArea a on cm.ID=a.CMID left join BD_DisArea da on a.AreaID=da.ID  left join YZT_FCDis d left join BD_Distributor dis on d.DisID=dis.ID on cm.ID=d.CMID where cm.ID=" + this.KeyID;
                string fc = string.Empty;
                DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        if (cmModel.Type.ToString() == "2")
                        {
                            fc += fc == "" ? item["Province"] + "|" + item["City"] + "|" + item["Area"] : "," + item["Province"] + "|" + item["City"] + "|" + item["Area"];
                        }
                        else {
                            fc += fc == "" ? item["DisName"] : "," + item["DisName"];
                        }
                    }
                }
                this.txtFc.Value = fc;
                this.litype.Visible = true;
            }

            if (cmModel.ProvideData.IndexOf("1") > -1)
            {
                this.chk1.Checked = true;
            }
            if (cmModel.ProvideData.IndexOf("2") > -1)
            {
                this.chk2.Checked = true;
            }
            if (cmModel.ProvideData.IndexOf("3") > -1)
            {
                this.chk3.Checked = true;
            }
            if (cmModel.ProvideData.IndexOf("4") > -1)
            {
                this.chk4.Checked = true;
            }

        }
    }

    /// <summary>
    /// 上架
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShangjia_Click(object sender, EventArgs e) {
        if (KeyID != 0)
        {
            Hi.Model.YZT_CMerchants cmModel = new Hi.BLL.YZT_CMerchants().GetModel(this.KeyID);
            cmModel.IsEnabled = 1;
            cmModel.modifyuser = this.UserID;
            cmModel.ts = DateTime.Now;
            cmModel.CreateDate = DateTime.Now;

            new Hi.BLL.YZT_CMerchants().Update(cmModel);
            bind();
        }
    }

    /// <summary>
    /// 下架
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnXianjia_Click(object sender, EventArgs e)
    {
        if (KeyID != 0)
        {
            Hi.Model.YZT_CMerchants cmModel = new Hi.BLL.YZT_CMerchants().GetModel(this.KeyID);
            cmModel.IsEnabled = 0;
            cmModel.modifyuser = this.UserID;
            cmModel.ts = DateTime.Now;
            new Hi.BLL.YZT_CMerchants().Update(cmModel);
            bind();

            //工商四元素
            //GetBusines bu = new GetBusines();
            //string ss = bu.GetBus("", "", "", "");
        }
    }
}
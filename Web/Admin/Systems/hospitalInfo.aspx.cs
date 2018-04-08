using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Admin_Systems_PaybankInfo : AdminPageBase
{
    public string hid = "0";
    private bool isEnabledText = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Paid = Convert.ToInt32(Request.QueryString["paid"]);
        if (!IsPostBack)
        {
            Bind();
        }
    }

    protected void Bind()
    {
        hid = Request.QueryString["hid"] + "";
        if (!string.IsNullOrWhiteSpace(hid)) {
        Hi.Model.SYS_Hospital hospital = new Hi.BLL.SYS_Hospital().GetModel(Convert.ToInt32(hid));
            if (hospital != null) {
                this.hospitalCode.InnerText = hospital.HospitalCode;//医院编码
                this.hospitalName.InnerText = hospital.HospitalName;//医院全称
                this.hospitalLevel.InnerText = hospital.HospitalLevel;//医院级别
                this.hidProvince.InnerText = hospital.Province;//省
                this.hidCity.InnerText = hospital.City;//市
                this.hidArea.InnerText = hospital.Area;//区
                this.address.InnerText = hospital.Address;//详细地址
                if (hospital.IsEnabled)
                {
                    this.IsEnabled.InnerHtml = "<span><img src='../../Company/images/t06.png'/></span>停用";
                    isEnabledText = false;
                }
                else
                {
                    this.IsEnabled.InnerHtml = "<span><img src='../../Company/images/t06.png '/></span>启用";
                    isEnabledText = true;
                }
            }
        }
    }




    /// <summary>
    /// 启用或停用方法
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void IsEnabledbtn_Click(object sender, EventArgs e)
    {
        hid = Request.QueryString["hid"] + "";
        Hi.Model.SYS_Hospital hospital = new Hi.BLL.SYS_Hospital().GetModel(Convert.ToInt32(hid));
        hospital.IsEnabled = hospital.IsEnabled?false :true;
        if (new Hi.BLL.SYS_Hospital().Update(hospital))
            {
            JScript.AlertMsg(this, ""+ this.IsEnabled .InnerText+ "成功", "");
            Response.Redirect("hospitalInfo.aspx?hid=" + hospital.ID);
            }
            else
            {
                JScript.AlertMsg(this, "操作失败", "function (){ window.location.reload(); }");
            }

    }
}
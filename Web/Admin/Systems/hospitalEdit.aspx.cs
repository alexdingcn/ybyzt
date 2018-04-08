using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Admin_Systems_PaybankEdit : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string hid = Request["hid"] + "";
            if (!string.IsNullOrWhiteSpace(hid))
            {
                this.btnSave.Text = "修改";
            }
            Bind();
        }
    }

    protected void Bind()
    {
        string hid = Request["hid"] + "";
        if (!string.IsNullOrWhiteSpace(hid)) {
        Hi.Model.SYS_Hospital hospital = new Hi.BLL.SYS_Hospital().GetModel(Convert.ToInt32(hid));
            if (hospital != null) {
                this.hospitalCode.Value = hospital.HospitalCode;//医院编码
                this.hospitalName.Value = hospital.HospitalName;//医院全称
                this.hospitalLevel.Value = hospital.HospitalLevel;//医院级别
                this.hidProvince.Value = hospital.Province;//省
                this.hidCity.Value = hospital.City;//市
                this.hidArea.Value = hospital.Area;//区
                this.address.Value = hospital.Address;//详细地址
            }
        }
    }


    /// <summary>
    /// 新增修改方法
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
       
        string hid = Request["hid"] + "";
        if (!string.IsNullOrWhiteSpace(hid))
        {
            Hi.Model.SYS_Hospital hospital = new Hi.BLL.SYS_Hospital().GetModel(Convert.ToInt32(hid));
            hospital.HospitalCode = this.hospitalCode.Value.Trim();//医院编码
            hospital.HospitalName = this.hospitalName.Value.Trim();//医院全称
            hospital.HospitalLevel = this.hospitalLevel.Value.Trim();//医院级别
            hospital.Province = this.hidProvince.Value.Trim();//省
            hospital.City = this.hidCity.Value.Trim();//市
            hospital.Area = this.hidArea.Value.Trim();//区
            hospital.Address = this.address.Value.Trim();//详细地址
            if (new Hi.BLL.SYS_Hospital().Update(hospital))
            {
                Response.Redirect("hospitalInfo.aspx?hid=" + hospital.ID);
            }
            else
            {
                JScript.AlertMsgMo(this, "医院编辑失败", "function (){ window.location.reload(); }");
            }

        }
        else {

            List<Hi.Model.SYS_Hospital> hoslist = new Hi.BLL.SYS_Hospital().GetList("", " HospitalName='" + this.hospitalName.Value.Trim() + "'", "");

            if (hoslist != null && hoslist.Count > 0)
            {
                JScript.AlertMsgMo(this, "医院名称已存在", "function (){  }");
            }
            else
            {

                Hi.Model.SYS_Hospital hospital = new Hi.Model.SYS_Hospital();
                hospital.HospitalCode = this.hospitalCode.Value.Trim();//医院编码
                hospital.HospitalName = this.hospitalName.Value.Trim();//医院全称
                hospital.HospitalLevel = this.hospitalLevel.Value.Trim();//医院级别
                hospital.Province = this.hidProvince.Value.Trim();//省
                hospital.City = this.hidCity.Value.Trim();//市
                hospital.Area = this.hidArea.Value.Trim();//区
                hospital.Address = this.address.Value.Trim();//详细地址
                hospital.CreateDate = DateTime.Now;
                hospital.dr = false;
                hospital.CreateUser = UserID.ToString();
                hospital.IsEnabled = true;
                hospital.ts = DateTime.Now;
                int id = new Hi.BLL.SYS_Hospital().Add(hospital);
                if (id > 0)
                {

                    Response.Redirect("hospitalInfo.aspx?hid=" + id);
                }
                else
                {
                    JScript.AlertMsgMo(this, "医院编辑失败", "function (){ window.location.reload(); }");
                }
            }
        }
    }
}
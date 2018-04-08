using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hi.BLL;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Serialization;
public partial class Distributor_newOrder_remarkview : System.Web.UI.Page
{
    //订单、订单明细ID
    public int KeyID = 0;
    //商品ID
    public int infoid = 0;
    //代理商ID
    public int DisID = 0;
    //修改类型  0、订单备注， 1、商品备注 2、下单商品备注
    public int type = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            List<Hi.Model.SYS_Hospital> HtList = new Hi.BLL.SYS_Hospital().GetList("", " dr=0","");
            HtDrop.DataSource = HtList;
            HtDrop.DataTextField = "HospitalName";
            HtDrop.DataValueField = "ID";
            HtDrop.DataBind();
            this.txtHtDropID.Value = this.HtDrop.SelectedValue;
            this.txtHtDrop.Value = this.HtDrop.SelectedItem.Text;
            if (!string.IsNullOrEmpty(Request["TrId"]))
                this.hidTrId.Value = Request["TrId"].ToString();
            if (!string.IsNullOrEmpty(Request["indexs"]))
                this.hidIndex.Value = Request["indexs"].ToString();
            if (!string.IsNullOrEmpty(Request["htId"]))
                this.HtDrop.SelectedValue = Request["htId"].ToString();
        }

    }

    /// <summary>
    /// 医院下拉 值 改变事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void HtDrop_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.txtHtDropID.Value = this.HtDrop.SelectedValue;
        this.txtHtDrop.Value = this.HtDrop.SelectedItem.Text;
    }
}
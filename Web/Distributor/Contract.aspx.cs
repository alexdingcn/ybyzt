using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_Contract : System.Web.UI.Page
{
    public string SignDay = "15";
    public string PayDay = "30";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["KeyID"] != null)
            {
                int compID = Convert.ToInt32(Common.DesDecrypt(Request.QueryString["KeyID"].ToString(), Common.EncryptKey));
                Hi.Model.DIS_Order OrderModel = new Hi.BLL.DIS_Order().GetModel(compID);
                if (OrderModel != null)
                {
                    SignDay = GetDay(OrderModel.CompID, "订单自动签收");
                    PayDay = GetDay(OrderModel.CompID, "超时未付款自动作废订单");
                }
            }
        }
    }

    public string GetDay(int compID,string strWhat)
    {
        string strwhere = string.Empty;
        if (compID == 0)
            strwhere = " isnull(dr,0)=0 and Name='" + strWhat + "' ";
        else
            strwhere = "isnull(dr,0)=0 and Name='" + strWhat + "' and CompID=" + compID;
        List<Hi.Model.SYS_SysName> LSName = new Hi.BLL.SYS_SysName().GetList("", strwhere, "");
        return LSName.Count > 0 ? LSName[0].Value : "0";
    }
}
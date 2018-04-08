using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EShop_UserControl_EshopBttom : System.Web.UI.UserControl
{
    public int Compid = 0;//厂商id
    public string phone = string.Empty;//厂商手机
    public string qqtrueOrfalse = "";//店主是否维护QQ
    public string qq = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["Comid"].Trim() != "")
        {
            Compid = Convert.ToInt32(Request["Comid"].Trim());
            storeCode.Src = ConfigurationManager.AppSettings["ImgViewPath"] + "CompImg/c" + Request["Comid"].Trim() + ".png";
            Hi.Model.BD_Company compl = new Hi.BLL.BD_Company().GetModel(Convert.ToInt32(Request["Comid"].Trim()));
            if (compl != null)
            {
                phone = compl.Phone == "" || compl.Phone == null ? compl.Tel : compl.Phone;
                qqtrueOrfalse = string.IsNullOrWhiteSpace(compl.QQ) ? "none" : "block";
                qq = compl.QQ;
            }
        }

    }
}
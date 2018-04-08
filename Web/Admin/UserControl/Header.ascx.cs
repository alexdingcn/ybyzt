using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_UserControl_Header : System.Web.UI.UserControl
{
    public string login_name = string.Empty;
    public string login_Tname = string.Empty;
    public string compid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AdminUser"] != null)
        {
            Hi.Model.SYS_AdminUser model = Session["AdminUser"] as Hi.Model.SYS_AdminUser;
            if (model != null)
            {
                login_name = model.LoginName;
                login_Tname = model.TrueName;
            }

        }
        if (!IsPostBack)
        {
            SendShip();
            OffShip();
        }
    }

    public string SendShip()
    {
        //企业设置自动签收天数
        if (IsRun(0, "订单自动签收") == 0)
        {
            return Hi.BLL.autoOrder.SendShip();
        }
        return "";
    }

    public string OffShip()
    {

        //企业设置超时未付款自动作废订单
        if (IsRun(0, "超时未付款自动作废订单") == 0)
        {
            return Hi.BLL.autoOrder.OffShip();
        }
        return "";
    }

    /// <summary>
    /// 判断当天登录时服务是否执行
    /// </summary>
    /// <param name="Name"></param>
    /// <returns>0、没有执行， 1、执行</returns>
    public int IsRun(int CompID, string Name)
    {
        int IsExecute = 1;

        string strwhere = string.Empty;
        if (CompID == 0)
            strwhere = " isnull(dr,0)=0 and Name='" + Name + "' ";
        else
            strwhere = "isnull(dr,0)=0 and Name='" + Name + "' and CompID=" + CompID;

        List<Hi.Model.SYS_SysName> LSName = new Hi.BLL.SYS_SysName().GetList("", strwhere, "");
        if (LSName.Count > 0)
        {
            if (DateTime.Now.Date > LSName[0].ts.Date)
            {
                IsExecute = 0;
                LSName[0].ts = DateTime.Now.Date;
                new Hi.BLL.SYS_SysName().Update(LSName[0]);
            }
        }
        else
        {
            Hi.Model.SYS_SysName model = new Hi.Model.SYS_SysName();
            model.CompID = CompID;
            model.Code = "";
            model.Name = Name;
            if (CompID == 0)
                model.Value = "";
            else
            {
                if (Name.Equals("超时未付款自动作废订单"))
                    model.Value = "30";
                else
                    model.Value = "15";
            }
            model.ts = DateTime.Now.Date.AddDays(-1);
            model.dr = 0;
            model.modifyuser = 0;
            new Hi.BLL.SYS_SysName().Add(model);
            IsRun(CompID, Name);
        }
        return IsExecute;
    }
}
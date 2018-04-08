using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
///autoOrderService 自动完成订单服务
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class autoOrderService : System.Web.Services.WebService {

    public autoOrderService () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod(Description = "订单自动签收")]
    public string SendShip()
    {
        //企业设置自动签收天数
        if (IsRun(0, "订单自动签收") == 0)
        {
            return Hi.BLL.autoOrder.SendShip();
        }
        return "";
    }

    [WebMethod(Description = "超时未付款自动作废订单")]
    public string OffShip()
    {

        //企业设置超时未付款自动作废订单
        if (IsRun(0, "超时未付款自动作废订单") == 0)
        {
           return  Hi.BLL.autoOrder.OffShip();
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

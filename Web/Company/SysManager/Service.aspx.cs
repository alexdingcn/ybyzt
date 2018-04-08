using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_SysManager_Service : System.Web.UI.Page
{
    public string KeyID = "";
    public string CompID = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        object obj = Request["action"];
        if (obj != null)
        {
            //选中的商品
            if (obj.ToString() == "Pay")
            {
                string type = Request["type"] + "";
                Response.Write(AddOrder(type));
                Response.End();
            }
        }
        if (!IsPostBack)
        {
            
            LoginModel logUser = Session["UserModel"] as LoginModel;
            if (logUser != null) {
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Convert.ToInt32(logUser.CompID));
            if (comp != null) { 
            if (comp.EnabledEndDate.ToString() == "0001/1/1 0:00:00")
            {
                //没有任何服务记录
            }
            else if (comp.EnabledEndDate < DateTime.Now.AddDays(1))
            {
                //服务已过期
               // Msg.InnerHtml = "您的服务期限已过,请尽快购买服务";
            }
            else
            {
                //服务日期有效
              //  Msg.InnerHtml = "您当前服务期限："+ comp.EnabledEndDate.ToString("yyyy-MM-dd") + "";
            }
            }

            //绑定支付记录
            List<Hi.Model.Pay_Service> deleteList = new Hi.BLL.Pay_Service().GetList("*", " CompID=" + logUser.CompID + "  and IsAudit=1 ", " createdate desc");
            Services.DataSource = deleteList;
            Services.DataBind();
            }
        }

    }

    /// <summary>
    /// 购买服务
    /// </summary>
    /// <param name="type">服务种类</param>
    /// <returns></returns>
    public string AddOrder(string type)
    {
        try
        {
            LoginModel logUser = Session["UserModel"] as LoginModel;

            List<Hi.Model.Pay_Service> deleteList = new Hi.BLL.Pay_Service().GetList("*", " CompID=" + logUser.CompID + " and  PayedPrice=0 and IsAudit=2 ", " createdate desc");
            if (deleteList.Count>0)
            {
                foreach (var item in deleteList)
                {
                    new Hi.BLL.Pay_Service().Delete(item.ID);
                }
            }

            string outdata = "0";//判断是否已经存在有效的服务(获取已存在的服务过期日期)
            string CreateDate = "0";//同上
           
            Hi.Model.Pay_Service service = new Hi.Model.Pay_Service();
            service.CompID = logUser.CompID;
            service.CompName = logUser.CompName;
            outdata =Common.GetCompService(logUser.CompID.ToString(),out CreateDate);// 存在有效服务 有效日期累加
            //List<Hi.Model.Pay_Service> serviceord = new Hi.BLL.Pay_Service().GetList("*", " compid=" + logUser.CompID + " and isaudit=1 and outofdata=0 and OutData>'" + DateTime.Now.ToString("yyyy-MM-dd") + "' ", " OutData desc");
            //if (serviceord.Count>0)
            //{
            //    outdata = serviceord[0].OutData.ToString();//存在有效服务  有效日期累加
            //    CreateDate= serviceord[0].CreateDate.ToString();
            //}


            if (type == "1")//月服务
            {
                service.ServiceType = 2;//服务类别 2:月  1:年
                service.OutData = outdata=="0"?DateTime.Now.AddMonths(1):Convert.ToDateTime(outdata).AddMonths(1);//服务到期日期
                service.Price = 499;//服务金额
            }
            else//年服务
            {
                service.ServiceType = 1;//服务类别 2:月  1:年
                service.OutData = outdata == "0" ? DateTime.Now.AddYears(1): Convert.ToDateTime(outdata).AddYears(1);// 服务到期日期
                service.Price = 4999;
            }
            service.CreateDate = CreateDate=="0"? DateTime.Now:Convert.ToDateTime(CreateDate);
            service.CreateUser = logUser.UserID;
            service.OutOfData = 0;//是否过期 0否 1是
            service.PayedPrice = 0;//已经支付金额
            service.IsAudit = 2;//支付状态 1成功  2失败
            service.ts = DateTime.Now;
            service.dr = 0;
            service.modifyuser = logUser.UserID;
            int OrderId = new Hi.BLL.Pay_Service().Add(service);
            KeyID = Common.DesEncrypt(OrderId.ToString(), Common.EncryptKey);
            if (OrderId > 0)
            {
                return "{\"rel\":\"OK\",\"Orderid\":\"" + KeyID + "\"}";
            }
            else
            {
                return "{\"rel\":\"NO\",\"Msg\":\"网络异常 请稍后再试\"}";
            }
            //return "{\"rel\":\"OK\",\"Orderid\":\"2\"}";
        }
        catch (Exception e)
        {
            return "{\"rel\":\"NO\",\"Msg\":\"" + e.Message + "\"}";
            throw;
        }
    }
}
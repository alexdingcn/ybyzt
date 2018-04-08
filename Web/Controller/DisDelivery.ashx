<%@ WebHandler Language="C#" Class="DisDelivery" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Data;

public class DisDelivery : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        if (!string.IsNullOrEmpty(context.Request.QueryString["type"]))
        {
            if (context.Request.QueryString["type"] == "up")
            {
                A_Update(context);
            }
            else if (context.Request.QueryString["type"] == "add" || context.Request.QueryString["type"] == "savedis")
            {
                A_AddUpdate(context);
            }
            else if (context.Request.QueryString["type"] == "disadd")
            {
                A_AddDis(context);
            }
            else if (context.Request.QueryString["type"] == "disadd1")
            {
                A_AddDis1(context);
            }
        }
    }

    public void A_Update(HttpContext context)
    {
        if (context.Request["updateid"] != null)
        {
            int updateid = int.Parse(context.Request["updateid"].ToString());
            Hi.Model.BD_DisAddr disaddr = new Hi.BLL.BD_DisAddr().GetModel(updateid);
            if (disaddr != null)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                string str = js.Serialize(disaddr);
                context.Response.Clear();
                context.Response.Write(str);
                context.Response.End();
            }
        }
    }


    public void A_AddUpdate(HttpContext context)
    {
        if (!string.IsNullOrEmpty(context.Request["code"].ToString()))
        {
            string dis = context.Request["disId"];
            string user = context.Request["user"];
            string code = context.Request["code"];
            string disphone = context.Request["disphone"];
            long outin = 0;
            if(!long.TryParse(disphone,out outin))
            disphone=AESHelper.Decrypt_php(disphone);
            if (disphone==null) disphone = "";
            Hi.Model.SYS_PhoneCode getphonecode = new Hi.BLL.SYS_PhoneCode().GetModel("修改地址", disphone, code);
            // Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetDisID(disId);
            Hi.Model.SYS_Users userModel = new Hi.BLL.SYS_Users().GetModel(Convert.ToInt32(user));
            DataSet ds = new Hi.BLL.BD_DisAddr().GetModel(dis);
            if (ds.Tables[0].Rows.Count >= 10)
            {
                string str = "\"type\":false,\"str\":\"一个代理商收货地址最多为10个\"";
                str = "{" + str + "}";
                context.Response.Write(str);
                return;
            }

            if (dis == "")
            {
                string str = "\"type\":false,\"str\":\"该账户还不是代理商\"";
                str = "{" + str + "}";
                context.Response.Write(str);
                return;
            }
            if (getphonecode != null)
            {

                Hi.Model.BD_DisAddr disaddr = null;
                if (context.Request.QueryString["type"] == "add")
                {
                    if (Common.DisAddrExistsAttribute("Address", context.Request["address"].ToString().Trim(), dis.ToString()))
                    {
                        string str = "\"type\":false,\"str\":\"请勿添加重复的收货地址\"";
                        str = "{" + str + "}";
                        context.Response.Write(str);
                        return;
                    }
                    disaddr = new Hi.Model.BD_DisAddr();
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        disaddr.IsDefault = 1;
                    }
                }
                else if (context.Request.QueryString["type"] == "savedis")
                {
                    if (Common.DisAddrExistsAttribute("Address", context.Request["address"].ToString().Trim(), dis.ToString(), context.Request["updateid"].ToString()))
                    {
                        string str = "\"type\":false,\"str\":\"请勿添加重复的收货地址\"";
                        str = "{" + str + "}";
                        context.Response.Write(str);
                        return;
                    }
                    disaddr = new Hi.BLL.BD_DisAddr().GetModel(int.Parse(context.Request["updateid"].ToString()));
                }
                disaddr.Name = "0";
                disaddr.DisID = Convert.ToInt32(dis);
                disaddr.Principal =Common.NoHTML( context.Request["username"].ToString());
                disaddr.Phone =Common.NoHTML( context.Request["userphone"].ToString());
                disaddr.Address =Common.NoHTML( context.Request["address"].ToString());
                disaddr.Province = context.Request["Province"].ToString();
                disaddr.City = context.Request["City"].ToString();
                disaddr.Area = context.Request["Area"].ToString();
                disaddr.CreateUserID = userModel.ID;
                disaddr.CreateDate = DateTime.Now;
                disaddr.ts = DateTime.Now;
                disaddr.modifyuser = userModel.ID;
                if (context.Request.QueryString["type"] == "add")
                {
                    int addrId = new Hi.BLL.BD_DisAddr().Add(disaddr);
                    if (addrId <= 0)
                    {
                        string str = "\"type\":false,\"str\":\"新增地址失败\"";
                        str = "{" + str + "}";
                        context.Response.Write(str);
                        return;
                    }
                    else
                    {

                        getphonecode.IsPast = 1;
                        getphonecode.ts = DateTime.Now;
                        getphonecode.modifyuser = userModel.ID;
                        new Hi.BLL.SYS_PhoneCode().Update(getphonecode);
                        string str = "\"type\":true,\"str\":\"" + addrId + "\"";
                        str = "{" + str + "}";
                        context.Response.Write(str);
                        return;
                    }
                }
                else if (context.Request.QueryString["type"] == "savedis")
                {
                    if (!(new Hi.BLL.BD_DisAddr().Update(disaddr)))
                    {
                        string str = "\"type\":false,\"str\":\"修改地址失败\"";
                        str = "{" + str + "}";
                        context.Response.Write(str);
                        return;
                    }
                    else
                    {

                        getphonecode.IsPast = 1;
                        getphonecode.ts = DateTime.Now;
                        getphonecode.modifyuser = userModel.ID;
                        new Hi.BLL.SYS_PhoneCode().Update(getphonecode);
                        string str = "\"type\":true";
                        str = "{" + str + "}";
                        context.Response.Write(str);
                        return;
                    }
                }
            }
            else
            {
                string str = "\"type\":false,\"str\":\"验证码错误\"";
                str = "{" + str + "}";
                context.Response.Write(str);
            }
        }
        else
        {
            string str = "\"type\":false,\"str\":\"验证码错误\"";
            str = "{" + str + "}";
            context.Response.Write(str);
        }
    }

    public void A_AddDis(HttpContext context)
    {

        //string name = context.Request["name"].ToString() + "";
        string user = context.Request["user"].ToString() + "";
        //string code = context.Request["code"].ToString();
        //string disphone = context.Request["disphone"].ToString();
        Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(user.ToInt(0));

        /// Hi.Model.SYS_Users userModel = new Hi.BLL.SYS_Users().GetDisid(user);
        DataSet ds = new Hi.BLL.BD_DisAddr().GetModel(user);

        if (ds.Tables[0].Rows.Count >= 10)
        {
            string str = "\"type\":false,\"str\":\"一个代理商收货地址最多为10个\"";
            str = "{" + str + "}";
            context.Response.Write(str);
            return;
        }
        if (dis == null)
        {
            string str = "\"type\":false,\"str\":\"该账户还不是代理商\"";
            str = "{" + str + "}";
            context.Response.Write(str);
            return;
        }
        if (Common.DisAddrExistsAttribute("Address", context.Request["address"].ToString().Trim(), dis.ID.ToString()))
        {
            string str = "\"type\":false,\"str\":\"请勿添加重复的收货地址\"";
            str = "{" + str + "}";
            context.Response.Write(str);
            return;
        }


        Hi.Model.BD_DisAddr disaddr = null;
        if (context.Request.QueryString["type"] == "disadd")
        {
            disaddr = new Hi.Model.BD_DisAddr();
        }
        else if (context.Request.QueryString["type"] == "savedis")
        {
            disaddr = new Hi.BLL.BD_DisAddr().GetModel(int.Parse(context.Request["updateid"].ToString()));
        }
        disaddr.DisID = dis.ID;
        disaddr.Principal = context.Request["username"].ToString();
        disaddr.Phone = context.Request["userphone"].ToString();
        disaddr.Address = context.Request["address"].ToString();
        disaddr.Province = context.Request["Province"].ToString();
        disaddr.City = context.Request["City"].ToString();
        disaddr.Area = context.Request["Area"].ToString();
        disaddr.CreateUserID = dis.CreateUserID;
        disaddr.CreateDate = DateTime.Now;
        disaddr.ts = DateTime.Now;
        disaddr.modifyuser = dis.CreateUserID;
        if (context.Request.QueryString["type"] == "disadd")
        {
            int Id = new Hi.BLL.BD_DisAddr().Add(disaddr);
            if (Id <= 0)
            {
                string str = "\"type\":false,\"str\":\"新增地址失败\"";
                str = "{" + str + "}";
                context.Response.Write(str);
                return;
            }
            else
            {
                string str = "\"type\":true,\"str\":\"" + Id + "\"";
                str = "{" + str + "}";
                context.Response.Write(str);
            }
        }
    }


    public void A_AddDis1(HttpContext context)
    {

        //string name = context.Request["name"].ToString() + "";
        string userid = context.Request["userid"].ToString() + "";
        //string code = context.Request["code"].ToString();
        //string disphone = context.Request["disphone"].ToString();
        Hi.Model.SYS_Users userModel = new Hi.BLL.SYS_Users().GetModel(Convert.ToInt32(userid.ToString()));
        Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(userModel.DisID);


        DataSet ds = new Hi.BLL.BD_DisAddr().GetModel(userModel.UserName);

        if (ds.Tables[0].Rows.Count >= 10)
        {
            string str = "\"type\":false,\"str\":\"一个代理商收货地址最多为10个\"";
            str = "{" + str + "}";
            context.Response.Write(str);
            return;
        }
        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //{
        //    if (name == ds.Tables[0].Rows[i]["Name"].ToString().Trim())
        //    {
        //        string str = "\"type\":false,\"str\":\"为了更好的区分地址,请不要为地址取相同的别名\"";
        //        str = "{" + str + "}";
        //        context.Response.Write(str);
        //        return;
        //    }
        //}
        if (dis == null)
        {
            string str = "\"type\":false,\"str\":\"该账户还不是代理商\"";
            str = "{" + str + "}";
            context.Response.Write(str);
            return;
        }



        Hi.Model.BD_DisAddr disaddr = null;

        disaddr = new Hi.Model.BD_DisAddr();

        disaddr.DisID = dis.ID;
        disaddr.Principal = context.Request["username"].ToString();
        disaddr.Phone = context.Request["userphone"].ToString();
        disaddr.Address = context.Request["useraddress"].ToString();
        disaddr.Province = context.Request["userProvince"].ToString();
        disaddr.City = context.Request["userCity"].ToString();
        disaddr.Area = context.Request["userArea"].ToString();
        disaddr.CreateUserID = userModel.ID;
        disaddr.CreateDate = DateTime.Now;
        disaddr.ts = DateTime.Now;
        disaddr.modifyuser = userModel.ID;
        if (context.Request.QueryString["type"] == "disadd1")
        {
            int Id = new Hi.BLL.BD_DisAddr().Add(disaddr);
            if (Id <= 0)
            {
                string str = "\"type\":false,\"str\":\"新增地址失败\"";
                str = "{" + str + "}";
                context.Response.Write(str);
                return;
            }
            else
            {
                string str = "\"type\":true,\"str\":\"" + Id + "\"";
                str = "{" + str + "}";
                context.Response.Write(str);
            }
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
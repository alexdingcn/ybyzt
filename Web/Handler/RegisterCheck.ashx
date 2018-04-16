<%@ WebHandler Language="C#" Class="RegisterCheck" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using LitJson;
using System.Web.SessionState;
using System.Data;
using DBUtility;

public class RegisterCheck : loginInfoMation, IReadOnlySessionState, IHttpHandler
{
    private object Locks = new object();
    Regex Phonereg = new Regex("^0?1[0-9]{10}$");
    public void ProcessRequest(HttpContext context)
    {
        string Requst_REFERER = context.Request.ServerVariables["HTTP_REFERER"];
        if (string.IsNullOrWhiteSpace(Requst_REFERER))
        {
            context.Response.Write("禁止地址栏访问处理程序");
            context.Response.End();
        }
        JavaScriptSerializer Js = new JavaScriptSerializer();
        context.Response.ContentType = "text/plain";
        string Phone = context.Request["phone"] + "";
        string GetAction = context.Request["GetAction"];
        string Value = context.Request["Value"] + "";
        string Compid = context.Request["Compid"] + "";
        string CallBack = context.Request["callback"] + "";
        string RegisterType = context.Request["RegisterType"] + "";
        switch (GetAction)
        {
            case "Getuser":
                context.Response.Write(Js.Serialize(ExistsUsername(Value)));
                context.Response.End(); break;
            case "GetComp":
                context.Response.Write(Js.Serialize(ExistsCompName(Value)));
                context.Response.End();
                break;
            case "GetPhone":
                context.Response.Write(Js.Serialize(ExistsUserPhone(Value, RegisterType, Compid, context)));
                context.Response.End(); break;
            case "CheckPhoneCode":
                context.Response.Write(Js.Serialize(CheckPhoneCode(Phone, Value, context)));
                context.Response.End();
                break;
            case "GetDis":
                context.Response.Write(Js.Serialize(ExistsDisName(Value, Compid)));
                context.Response.End();
                break;
            case "ChckCode":
                context.Response.Write(Js.Serialize(CheckCode(Value, context)));
                context.Response.End();
                break;
            case "SubmitCheckNo1":
                context.Response.Write(Js.Serialize(SubmitCheckNo1(Value, RegisterType, Compid, context)));
                context.Response.End();
                break;
            case "SubmitCheckNo2":
                lock (Locks)
                {
                    context.Response.Write(Js.Serialize(SubmitCheckNo2(Value, context, RegisterType, Compid)));
                    context.Response.End();
                }
                break;
        }
    }


    public Common.ReturnMessge SubmitCheckNo1(string Value, string RegisterType, string Compid, HttpContext context)
    {
        Common.ReturnMessge Msg = new Common.ReturnMessge();
        try
        {
            JsonData Json = JsonMapper.ToObject(Value);
            Json = Json["Data"];
            List<Common.ReturnMessge> ListMsg = new List<Common.ReturnMessge>();
            Common.ReturnMessge Ms = null;
            string Phone = "";
            bool IsRegister = false;
            bool IsCheck = true;
            if (Json.Count != 3)
            {
                throw new ApplicationException("请求参数异常，请重试或刷新页面");
            }
            foreach (JsonData Jdata in Json)
            {
                switch (Jdata["ContorlId"].ToString())
                {
                    case "txt_Phone":
                        Ms = ExistsUserPhone(Jdata["Value"].ToString(), RegisterType, Compid, context);
                        IsRegister = Ms.IsRegis;
                        Ms.Attr1 = Jdata["ContorlId"].ToString();
                        ListMsg.Add(Ms);
                        Phone = Jdata["Value"].ToString();
                        if (!Phonereg.IsMatch(Jdata["Value"].ToString()))
                        {
                            Ms.Result = false;
                            Ms.Msg = "手机号码格式不正确";
                        }
                        IsCheck = IsCheck ? Ms.Result : IsCheck;
                        break;
                    case "txt_CheckCode":
                        Ms = CheckCode(Jdata["Value"].ToString(), context);
                        Ms.Attr1 = Jdata["ContorlId"].ToString();
                        ListMsg.Add(Ms);
                        if (IsCheck)
                        {
                            IsCheck = Ms.Result;
                        }
                        break;
                    case "txt_PhoneCode":
                        Ms = CheckPhoneCode(Phone, Jdata["Value"].ToString(), context, IsCheck);
                        Ms.Attr1 = Jdata["ContorlId"].ToString();
                        ListMsg.Add(Ms);
                        if (IsCheck)
                            IsCheck = Ms.Result;
                        break;
                    default: throw new ApplicationException("请求参数异常，请重试或刷新页面");
                }
            }

            Msg.Result = IsCheck;
            Msg.IsRegis = IsRegister;
            if (RegisterType == "RegiDis")
            {
                Msg.CompName = CompName;
            }
            Msg.Name = UserName;
            JavaScriptSerializer Js = new JavaScriptSerializer();
            Msg.Code = Js.Serialize(ListMsg);
        }
        catch (Exception ex)
        {
            Tiannuo.LogHelper.LogHelper.Error("Error", ex);
            Msg.Code = ex.Message;
            Msg.Result = false;
            Msg.Msg = "验证失败，请稍候再试";
            Msg.Error = true;
            if (ex is System.InvalidOperationException)
            {
                Msg.Msg = "请求参数异常，请重试或刷新页面";
            }
            else if (ex is ApplicationException)
            {
                Msg.Msg = ex.Message;
            }
        }
        return Msg;
    }

    public Common.ReturnMessge SubmitCheckNo2(string Value, HttpContext context, string RegisterType, string Compid)
    {

        string txt_Licence = null;//法人身份证
        string txt_creditCode = null;//统一社会信用代码
        string txt_Leading = null;//法人身姓名


        Common.ReturnMessge Msg = new Common.ReturnMessge();
        SqlTransaction Tran = null;
        try
        {
            string FileValue = context.Request["FileValue"] + "";
            List<Hi.Model.SYS_PhoneCode> Lphone = null;
            JsonData Json = JsonMapper.ToObject(Value);
            Json = Json["Data"];
            List<Common.ReturnMessge> ListMsg = new List<Common.ReturnMessge>();
            Common.ReturnMessge Ms = new Common.ReturnMessge();
            bool IsCheck = false;
            string PassWord = "";
            string Phone = "";
            string CompDisName = "";
            string UserName = "";
            string TrueName = "";
            Common.ReturnMessge ReturnMessge = null;
            JavaScriptSerializer Js = new JavaScriptSerializer();
            if (string.IsNullOrWhiteSpace(Compid) && RegisterType == "RegiDis")
            {
                throw new ApplicationException("请求参数错误，请重试。");
            }
            //登录帐号 企业/代理商 密码 校验 start
            foreach (JsonData Jdata in Json)
            {
                switch (Jdata["ContorlId"].ToString())
                {
                    case "txt_CompName":
                        if (RegisterType == "RegiComp")
                        {
                            Ms = ExistsCompName(Jdata["Value"].ToString());
                        }
                        else
                        {
                            Ms = ExistsDisName(Jdata["Value"].ToString(), Compid);
                        }
                        Ms.Attr1 = Jdata["ContorlId"].ToString();
                        CompDisName = Common.NoHTML(Jdata["Value"].ToString());
                        if (Jdata["Value"].ToString().Length > 20 || Jdata["Value"].ToString().Length < 2)
                        {
                            Ms.Result = false;
                            Ms.Msg = RegisterType == "RegiComp" ? "厂商名称必须在2-20字符之间。" : "代理商名称必须在2-20字符之间。";
                        }
                        ListMsg.Add(Ms);
                        IsCheck = Ms.Result;
                        break;
                    case "txt_Account":
                        Ms = ExistsUsername(Common.NoHTML(Jdata["Value"].ToString()));
                        Ms.Attr1 = Jdata["ContorlId"].ToString();
                        UserName = Common.NoHTML(Jdata["Value"].ToString());
                        if (Jdata["Value"].ToString().Length > 20 || Jdata["Value"].ToString().Length < 2)
                        {
                            Ms.Result = false;
                            Ms.Msg = "登录帐号必须在2-20字符之间。";
                        }
                        if (Common.GetUserExists("Username", UserName)) {
                            Ms.Result = false;
                            Ms.Msg = "登陆帐号已存在，请重新输入";
                        }
                        ListMsg.Add(Ms);
                        if (IsCheck)
                            IsCheck = Ms.Result;
                        break;
                    case "txt_creditCode":
                        Ms = ExistsUsername(Common.NoHTML(Jdata["Value"].ToString()));
                        Ms.Attr1 = Jdata["ContorlId"].ToString();
                        txt_creditCode = Common.NoHTML(Jdata["Value"].ToString());
                        if (string.IsNullOrWhiteSpace(Jdata["Value"].ToString()))
                        {
                            Ms.Result = false;
                            Ms.Msg = "统一社会信用代码不能为空";
                        }
                        ListMsg.Add(Ms);
                        if (IsCheck)
                            IsCheck = Ms.Result;
                        break;
                    case "txt_TrueName":
                        Ms = ExistsUsername(Common.NoHTML(Jdata["Value"].ToString()));
                        Ms.Attr1 = Jdata["ContorlId"].ToString();
                        TrueName = Common.NoHTML(Jdata["Value"].ToString());
                        if (string.IsNullOrWhiteSpace(Jdata["Value"].ToString()))
                        {
                            Ms.Result = false;
                            Ms.Msg = "联系人姓名不能为空";
                        }
                        ListMsg.Add(Ms);
                        if (IsCheck)
                            IsCheck = Ms.Result;
                        break;
                    case "txt_Licence":
                        Ms = ExistsUsername(Common.NoHTML(Jdata["Value"].ToString()));
                        Ms.Attr1 = Jdata["ContorlId"].ToString();
                        txt_Licence = Common.NoHTML(Jdata["Value"].ToString());
                        if (string.IsNullOrWhiteSpace(Jdata["Value"].ToString()))
                        {
                            Ms.Result = false;
                            Ms.Msg = "法人身份证不能为空";
                        }
                        ListMsg.Add(Ms);
                        if (IsCheck)
                            IsCheck = Ms.Result;
                        break;
                    case "txt_Leading":
                        Ms = ExistsUsername(Common.NoHTML(Jdata["Value"].ToString()));
                        Ms.Attr1 = Jdata["ContorlId"].ToString();
                        txt_Leading = Common.NoHTML(Jdata["Value"].ToString());
                        if (Jdata["Value"].ToString().Length > 20 || Jdata["Value"].ToString().Length < 2)
                        {
                            Ms.Result = false;
                            Ms.Msg = "法人名称必须在2-20字符之间。";
                        }

                        ListMsg.Add(Ms);
                        if (IsCheck)
                            IsCheck = Ms.Result;
                        break;
                    case "txt_PassWord":
                        Ms = new Common.ReturnMessge();
                        Ms.Result = true;
                        Ms.Attr1 = Jdata["ContorlId"].ToString();
                        if (Jdata["Value"].ToString().Length > 20 || Jdata["Value"].ToString().Length < 6)
                        {
                            Ms.Result = false;
                            Ms.Msg = "登陆密码必须在6-20字符之间。";
                        }
                        PassWord = Common.NoHTML(Jdata["Value"].ToString());
                        ListMsg.Add(Ms);
                        if (IsCheck)
                            IsCheck = Ms.Result;
                        break;
                    case "txt_CheckPassWord":
                        Ms = new Common.ReturnMessge();
                        Ms.Result = true;
                        Ms.Attr1 = Jdata["ContorlId"].ToString();
                        if (Jdata["Value"].ToString() != PassWord)
                        {
                            Ms.Result = false;
                            Ms.Msg = "确认密码不一致，请重新输入。";
                        }
                        ListMsg.Add(Ms);
                        if (IsCheck)
                            IsCheck = Ms.Result;
                        break;
                    case "txt_Phone":
                        Ms = new Common.ReturnMessge();
                        Ms.Attr1 = Jdata["ContorlId"].ToString();
                        Phone = Common.NoHTML(Jdata["Value"].ToString());
                        if (!Phonereg.IsMatch(Jdata["Value"].ToString()))
                        {
                            Msg.RegisterOrder = "RegisterNo1";
                            Ms.Msg = "手机号码格式不正确。";
                            ListMsg.Add(Ms);
                            Msg.Code = Js.Serialize(ListMsg);
                            throw new ApplicationException("手机号码格式不正确。");
                        }
                        else
                        {
                            Lphone = new Hi.BLL.SYS_PhoneCode().GetList("", " dr=0 and isPast=0 and UserName='" + RegisterOrder.已完成第一步注册.ToString() + "' and Module='企业注册' and phone='" + Phone + "' and '" + DateTime.Now + "' between  createdate and DATEADD(MI,60,CreateDate) ", " CreateDate desc ");
                            if (Lphone.Count == 0)
                            {
                                Msg.RegisterOrder = "RegisterNo1";
                                Ms.Msg = "请完成手机校验后再注册。";
                                ListMsg.Add(Ms);
                                Msg.Code = Js.Serialize(ListMsg);
                                throw new ApplicationException("请完成手机校验后再注册。");
                            }
                        }
                        ReturnMessge = ExistsUserPhone(Phone, RegisterType, Compid, context);
                        Ms.Result = !ReturnMessge.Error;
                        Ms.Msg = ReturnMessge.Msg;
                        ListMsg.Add(Ms);
                        if (ReturnMessge.Error)
                        {
                            Msg.Code = Js.Serialize(ListMsg);
                            Msg.RegisterOrder = "RegisterNo1";
                            throw new ApplicationException(ReturnMessge.Msg);
                        }
                        if (ReturnMessge.IsRegis)
                        {
                            if (Json.Count != 9)
                            {
                                throw new ApplicationException("请求参数异常，请重试或刷新页面");
                            }
                        }
                        else
                        {
                            if (Json.Count != 9)
                            {
                                throw new ApplicationException("请求参数异常，请重试或刷新页面");
                            }
                        }
                        if (IsCheck)
                            IsCheck = Ms.Result;
                        break;
                    default: throw new ApplicationException("请求参数异常，请重试或刷新页面");
                }
            }
            //登录帐号 企业/代理商 密码 校验 end
            if (IsCheck)
            {
                Msg.Href = RegisterType;
                if (RegisterType!="RegiComp") {
                    RegisterType = Compid == "0" ? "disRegi" : RegisterType;//如果Compid为0 则是代理商首页注册
                }
                SqlConnection Connection = new SqlConnection(SqlHelper.LocalSqlServer);
                Connection.Open();
                Tran = Connection.BeginTransaction();
                switch (RegisterType)
                {
                    case "RegiComp":
                        Hi.Model.BD_Company comp = new Hi.Model.BD_Company();
                        comp.CompName = CompDisName;
                        if (comp.CompName.Length <= 12 && comp.ShortName == "")
                        {
                            comp.ShortName = comp.CompName;
                        }
                        comp.LegalTel = Phone;
                        comp.Phone = Phone;
                        comp.AuditState = 0;
                        comp.IsEnabled = 1;
                        comp.FirstShow = 1;
                        comp.Erptype = 0;
                        comp.SortIndex = "001";
                        comp.HotShow = 1;
                        comp.Phone = Phone;
                        comp.CustomCompinfo = "本公司产品种类丰富、质量优良、价格公道、服务周到。感谢您长期的支持与厚爱，您的满意是我们最高的追求，我们将竭诚为您提供优质、贴心的服务！";
                        comp.Attachment = FileValue;
                        comp.CreateDate = DateTime.Now;
                        comp.CreateUserID = UserID;
                        comp.ts = DateTime.Now;
                        comp.modifyuser = UserID;
                        comp.Legal = txt_Leading;
                        comp.Identitys = txt_Licence;
                        comp.creditCode = txt_creditCode;

                        int compid = 0;
                        if ((compid = new Hi.BLL.BD_Company().Add(comp, Tran)) > 0)
                        {
                            if (!ReturnMessge.IsRegis)
                            {
                                Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
                                user.UserName = UserName;
                                user.TrueName = TrueName;
                                user.UserPwd = Util.md5(PassWord);
                                user.Phone = Phone;
                                user.CreateDate = DateTime.Now;
                                user.CreateUserID = UserID;
                                user.ts = DateTime.Now;
                                user.modifyuser = UserID;
                                user.IsEnabled = 1;
                                user.AuditState = 2;
                                int userid = 0;
                                userid = new Hi.BLL.SYS_Users().Add(user, Tran);
                                Hi.Model.SYS_CompUser CompUser = new Hi.Model.SYS_CompUser();
                                CompUser.CompID = compid;
                                CompUser.DisID = 0;
                                CompUser.CreateDate = DateTime.Now;
                                CompUser.CreateUserID = UserID;
                                CompUser.modifyuser = UserID;
                                CompUser.CType = 1;
                                CompUser.UType = 4;
                                CompUser.ts = DateTime.Now;
                                CompUser.dr = 0;
                                CompUser.IsAudit = 0;
                                CompUser.IsEnabled = 1;
                                CompUser.UserID = userid;
                                new Hi.BLL.SYS_CompUser().Add(CompUser, Tran);
                                Lphone[0].IsPast = 1;
                                Lphone[0].ts = DateTime.Now;
                                Lphone[0].UserName = RegisterOrder.已完成第二步注册.ToString();
                                new Hi.BLL.SYS_PhoneCode().Update(Lphone[0], Tran);
                                Tran.Commit();
                            }
                            else
                            {
                                Hi.Model.SYS_CompUser CompUser = new Hi.Model.SYS_CompUser();
                                CompUser.CompID = compid;
                                CompUser.DisID = 0;
                                CompUser.CreateDate = DateTime.Now;
                                CompUser.CreateUserID = UserID;
                                CompUser.modifyuser = UserID;
                                CompUser.CType = 1;
                                CompUser.UType = 4;
                                CompUser.IsAudit = 0;
                                CompUser.IsEnabled = 1;
                                CompUser.ts = DateTime.Now;
                                CompUser.dr = 0;
                                CompUser.UserID = ReturnMessge.Id;
                                Lphone[0].IsPast = 1;
                                Lphone[0].ts = DateTime.Now;
                                Lphone[0].UserName = RegisterOrder.已完成第二步注册.ToString();
                                new Hi.BLL.SYS_PhoneCode().Update(Lphone[0], Tran);
                                new Hi.BLL.SYS_CompUser().Add(CompUser, Tran);
                                Tran.Commit();
                            }
                            string PhoneCodeAccount = System.Configuration.ConfigurationManager.AppSettings["PhoneCodeAccount"].ToString();
                            string PhoneCodePwd = System.Configuration.ConfigurationManager.AppSettings["PhoneCodePwd"].ToString();
                            string SendRegiPhone = System.Configuration.ConfigurationManager.AppSettings["SendTels"].ToString();
                            string[] Phones = SendRegiPhone.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            string Result = "";
                            GetPhoneCode getphonecode = new GetPhoneCode();
                            getphonecode.GetUser(PhoneCodeAccount, PhoneCodePwd);
                            Result = getphonecode.SendComp(Phone);
                            if (Result != "Success")
                            {

                            }
                            foreach (string tel in Phones)
                            {
                                Result = getphonecode.ReturnComp(tel, comp.CompName);
                                if (Result != "Success")
                                {

                                }
                            }
                        }
                        else
                        {
                            Tran.Rollback();
                            IsCheck = false;
                            Msg.Error = true;
                            Msg.Msg = "注册失败请重试。";
                        }
                        ; break;
                    case "disRegi":
                        try
                        {
                            bool falg =  true;
                            //工商四元素
                            GetBusines bu = new GetBusines();
                            string ss = bu.GetBus(CompDisName, txt_Licence, txt_creditCode, txt_Leading);
                            if (ss != "SUCCESS")
                            {
                                falg = false;
                                Msg.Result = false;
                                Msg.Msg = "该企业不合法，无法注册完成";
                                Msg.Error = true;
                                Msg.Code = ss;
                                throw new ApplicationException("该企业不合法，无法注册完成");
                            }

                            Hi.Model.BD_Distributor Distributor = new Hi.Model.BD_Distributor();
                            Distributor.CompID = Compid.ToInt(0);
                            Distributor.DisName = CompDisName;
                            Distributor.IsEnabled = falg == false ? 0 : 1;
                            Distributor.Paypwd = Util.md5("123456");
                            Distributor.Phone = Phone;
                            Distributor.AuditState = falg == false ? 0 : 2;
                            Distributor.CreateDate = DateTime.Now;
                            Distributor.CreateUserID = UserID;
                            Distributor.ts = DateTime.Now;
                            Distributor.modifyuser = UserID;
                            Distributor.IsCheck = 0;
                            Distributor.CreditType = 0;
                            Distributor.pic = FileValue;
                            //Distributor.creditCode = txt_creditCode;
                            Distributor.Leading = txt_Leading;
                            Distributor.Licence = txt_Licence;
                            int DistributorID = 0;
                            if ((DistributorID = new Hi.BLL.BD_Distributor().Add(Distributor, Tran)) > 0)
                            {
                                int Roid = 0;
                                Msg.Href = "RegiOK";
                                //新增角色（企业管理员）
                                Hi.Model.SYS_Role role = new Hi.Model.SYS_Role();
                                role.CompID = Compid.ToInt(0);
                                role.DisID = DistributorID;
                                role.RoleName = "企业管理员";
                                role.IsEnabled = 1;
                                role.SortIndex = "1";
                                role.CreateDate = DateTime.Now;
                                role.CreateUserID = UserID;
                                role.ts = DateTime.Now;
                                role.modifyuser = UserID;
                                role.dr = 0;
                                Roid = new Hi.BLL.SYS_Role().Add(role, Tran);

                                //新增角色权限表
                                Hi.Model.SYS_RoleSysFun rolesys = null;
                                List<Hi.Model.SYS_SysFun> funList = new Hi.BLL.SYS_SysFun().GetList("FunCode,FunName", " Type=2", "");
                                foreach (Hi.Model.SYS_SysFun sys in funList)
                                {
                                    rolesys = new Hi.Model.SYS_RoleSysFun();
                                    rolesys.CompID = Compid.ToInt(0);
                                    rolesys.DisID = DistributorID;
                                    rolesys.RoleID = Roid;
                                    rolesys.FunCode = sys.FunCode;
                                    rolesys.FunName = sys.FunName;
                                    rolesys.IsEnabled = 1;
                                    rolesys.CreateUserID = UserID;
                                    rolesys.CreateDate = DateTime.Now;
                                    rolesys.ts = DateTime.Now;
                                    rolesys.modifyuser = UserID;
                                    new Hi.BLL.SYS_RoleSysFun().Add(rolesys, Tran);
                                }

                                Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
                                user.UserName = UserName;
                                user.TrueName = TrueName;
                                user.DisID = DistributorID;
                                user.TrueName = "";
                                user.UserPwd = Util.md5(PassWord);
                                user.IsEnabled = 1;
                                user.Phone = Phone;
                                user.CreateDate = DateTime.Now;
                                user.CreateUserID = UserID;
                                user.ts = DateTime.Now;
                                user.modifyuser = UserID;
                                user.AuditState = falg == false ? 0 : 2;
                                int userid = 0;
                                userid = new Hi.BLL.SYS_Users().Add(user, Tran);
                            }
                            Tran.Commit();

                        }
                        catch (Exception)
                        {
                            if (Tran != null)
                            {
                                if (Tran.Connection != null)
                                {
                                    Tran.Rollback();
                                }
                            }
                            throw;
                        }
                        finally {

                            if (Tran != null)
                            {
                                if (Tran.Connection != null)
                                {
                                    Tran.Rollback();
                                }
                            }
                        }

                        ; break;
                    case "RegiDis":
                        string CreateDate = "0";
                        Hi.Model.BD_Company Comp = new Hi.BLL.BD_Company().GetModel(Compid.ToInt(0));
                        if (Comp == null)
                        {
                            IsCheck = false;
                            throw new ApplicationException("加盟的企业不存在，请确认");
                        }
                        else if (Comp.HotShow == 0)
                        {
                            IsCheck = false;
                            throw new ApplicationException("该企业暂不允许加盟，请确认");
                        }

                        else if (Comp.dr == 1)
                        {
                            IsCheck = false;
                            throw new ApplicationException("加盟的企业不存在，请确认");
                        }
                        else if (Comp.AuditState != 2)
                        {
                            IsCheck = false;
                            throw new ApplicationException("该企业未通过审核，无法加盟");
                        }
                        else if (Comp.IsEnabled != 1)
                        {
                            IsCheck = false;
                            throw new ApplicationException("该企业已被禁用，无法加盟");
                        }
                        List<Hi.Model.SYS_SysName> ListName = new Hi.BLL.SYS_SysName().GetList("id", " dr=0 and Compid=" + Compid + " and Name='代理商加盟是否需要审核' and  value='1' ", "");
                        Hi.Model.BD_Distributor Dis = new Hi.Model.BD_Distributor();
                        Dis.CompID = Compid.ToInt(0);
                        Dis.DisName = CompDisName;
                        Dis.IsEnabled = 1;
                        Dis.Paypwd = Util.md5("123456");
                        Dis.Phone = Phone;
                        Dis.AuditState = ListName.Count > 0 ? 2 : 0;
                        Dis.CreateDate = DateTime.Now;
                        Dis.CreateUserID = UserID;
                        Dis.ts = DateTime.Now;
                        Dis.modifyuser = UserID;
                        Dis.IsCheck = 0;
                        Dis.CreditType = 0;
                        Dis.pic = FileValue;
                        int Disid = 0;
                        List<Hi.Model.BD_Distributor> ListDis = new List<Hi.Model.BD_Distributor>();
                        if (ReturnMessge.IsRegis)
                        {
                            switch (Ctype)
                            {
                                case 2:
                                    ListDis = new Hi.BLL.BD_Distributor().GetList("", " isnull(dr,0)=0 and id=" + DisID + "", "");
                                    if (ListDis.Count > 0)
                                    {
                                        Dis.Address = ListDis[0].Address;
                                        Dis.Province = ListDis[0].Province;
                                        Dis.City = ListDis[0].City;
                                        Dis.Area = ListDis[0].Area;
                                        if (!string.IsNullOrWhiteSpace(ListDis[0].Phone))
                                        {
                                            Dis.Phone = ListDis[0].Phone;
                                        }
                                        Dis.Tel = ListDis[0].Tel;
                                        Dis.Leading = ListDis[0].Leading;
                                        Dis.LeadingPhone = ListDis[0].LeadingPhone;
                                        Dis.Licence = ListDis[0].Licence;
                                        Dis.Zip = ListDis[0].Zip;
                                        Dis.Fax = ListDis[0].Fax;
                                    }
                                    ; break;
                            }
                        }
                        if ((Disid = new Hi.BLL.BD_Distributor().Add(Dis, Tran)) > 0)
                        {
                            int Roid = 0;
                            if (ListName.Count > 0)
                            {
                                Msg.Href = "RegiOK";
                                //新增角色（企业管理员）
                                Hi.Model.SYS_Role role = new Hi.Model.SYS_Role();
                                role.CompID = Compid.ToInt(0);
                                role.DisID = Disid;
                                role.RoleName = "企业管理员";
                                role.IsEnabled = 1;
                                role.SortIndex = "1";
                                role.CreateDate = DateTime.Now;
                                role.CreateUserID = UserID;
                                role.ts = DateTime.Now;
                                role.modifyuser = UserID;
                                role.dr = 0;
                                Roid = new Hi.BLL.SYS_Role().Add(role, Tran);

                                //新增角色权限表
                                Hi.Model.SYS_RoleSysFun rolesys = null;
                                List<Hi.Model.SYS_SysFun> funList = new Hi.BLL.SYS_SysFun().GetList("FunCode,FunName", " Type=2", "");
                                foreach (Hi.Model.SYS_SysFun sys in funList)
                                {
                                    rolesys = new Hi.Model.SYS_RoleSysFun();
                                    rolesys.CompID = Compid.ToInt(0);
                                    rolesys.DisID = Disid;
                                    rolesys.RoleID = Roid;
                                    rolesys.FunCode = sys.FunCode;
                                    rolesys.FunName = sys.FunName;
                                    rolesys.IsEnabled = 1;
                                    rolesys.CreateUserID = UserID;
                                    rolesys.CreateDate = DateTime.Now;
                                    rolesys.ts = DateTime.Now;
                                    rolesys.modifyuser = UserID;
                                    new Hi.BLL.SYS_RoleSysFun().Add(rolesys, Tran);
                                }
                                if (ListDis.Count > 0)
                                {
                                    Hi.Model.BD_DisAddr addr = new Hi.Model.BD_DisAddr();
                                    addr.Province = ListDis[0].Province;
                                    addr.City = ListDis[0].City;
                                    addr.Area = ListDis[0].Area;
                                    addr.DisID = Disid;
                                    addr.Principal = Dis.Province;
                                    addr.Phone = Phone;
                                    addr.Address = ListDis[0].Province + ListDis[0].City + ListDis[0].Area + ListDis[0].Address;
                                    addr.IsDefault = 1;
                                    addr.ts = DateTime.Now;
                                    addr.CreateDate = DateTime.Now;
                                    addr.CreateUserID = UserID;
                                    addr.modifyuser = UserID;
                                    new Hi.BLL.BD_DisAddr().Add(addr, Tran);
                                }
                            }
                            List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("UserID", " isnull(dr,0)=0 and ctype=1 and  utype=4 and  CompID='" + Compid + "'", "");
                            if (!ReturnMessge.IsRegis)
                            {
                                Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
                                user.UserName = UserName;
                                user.TrueName = TrueName;
                                user.UserPwd = Util.md5(PassWord);
                                user.IsEnabled = 1;
                                user.Phone = Phone;
                                user.CreateDate = DateTime.Now;
                                user.CreateUserID = UserID;
                                user.ts = DateTime.Now;
                                user.modifyuser = UserID;
                                user.IsEnabled = 1;
                                user.AuditState = 2;
                                int userid = 0;
                                userid = new Hi.BLL.SYS_Users().Add(user, Tran);
                                Hi.Model.SYS_CompUser CompUser = new Hi.Model.SYS_CompUser();
                                CompUser.CompID = Compid.ToInt(0);
                                CompUser.DisID = Disid;
                                CompUser.CreateDate = DateTime.Now;
                                CompUser.CreateUserID = UserID;
                                CompUser.modifyuser = UserID;
                                CompUser.CType = 2;
                                CompUser.UType = 5;
                                CompUser.ts = DateTime.Now;
                                CompUser.dr = 0;
                                CompUser.IsEnabled = 1;
                                CompUser.IsAudit = ListName.Count > 0 ? 2 : 0;
                                CompUser.UserID = userid;
                                CompUser.RoleID = Roid;
                                new Hi.BLL.SYS_CompUser().Add(CompUser, Tran);
                                Lphone[0].IsPast = 1;
                                Lphone[0].ts = DateTime.Now;
                                Lphone[0].UserName = RegisterOrder.已完成第二步注册.ToString();
                                new Hi.BLL.SYS_PhoneCode().Update(Lphone[0], Tran);
                            }
                            else
                            {
                                Hi.Model.SYS_CompUser CompUser = new Hi.Model.SYS_CompUser();
                                CompUser.CompID = Compid.ToInt(0);
                                CompUser.DisID = Disid;
                                CompUser.CreateDate = DateTime.Now;
                                CompUser.CreateUserID = UserID;
                                CompUser.modifyuser = UserID;
                                CompUser.CType = 2;
                                CompUser.UType = 5;
                                CompUser.IsEnabled = 1;
                                CompUser.IsAudit = ListName.Count > 0 ? 2 : 0;
                                CompUser.ts = DateTime.Now;
                                CompUser.dr = 0;
                                CompUser.UserID = ReturnMessge.Id;
                                CompUser.RoleID = Roid;
                                Lphone[0].IsPast = 1;
                                Lphone[0].ts = DateTime.Now;
                                Lphone[0].UserName = RegisterOrder.已完成第二步注册.ToString();
                                new Hi.BLL.SYS_PhoneCode().Update(Lphone[0], Tran);
                                new Hi.BLL.SYS_CompUser().Add(CompUser, Tran);
                            }
                            if (ListCompUser.Count > 0)
                            {
                                List<Hi.Model.SYS_Users> ListUser = new Hi.BLL.SYS_Users().GetList("", " isnull(dr,0)=0 and id=" + ListCompUser[0].UserID + "", "", Tran);
                                if (ListUser.Count > 0)
                                {
                                    SendPhone(ListUser[0].Phone, context, CompDisName, true);
                                    string PhoneCodeAccount = System.Configuration.ConfigurationManager.AppSettings["PhoneCodeAccount"].ToString();
                                    string PhoneCodePwd = System.Configuration.ConfigurationManager.AppSettings["PhoneCodePwd"].ToString();
                                    GetPhoneCode getphonecode = new GetPhoneCode();
                                    getphonecode.GetUser(PhoneCodeAccount, PhoneCodePwd);
                                    string Result = getphonecode.SendDis(Phone, "联系企业负责人：" + ListUser[0].Phone + "");
                                    if (Result != "Success")
                                    {

                                    }
                                }
                            }
                            Tran.Commit();
                        }
                        else
                        {
                            Tran.Rollback();
                            IsCheck = false;
                            Msg.Error = true;
                            Msg.Msg = "加盟失败请重试。";
                        }
                        ; break;
                    default: throw new ApplicationException("请求参数异常，请重试或刷新页面");
                }
                Connection.Close();
            }
            Msg.Result = IsCheck;
            Msg.Code = Js.Serialize(ListMsg);
        }
        catch (Exception ex)
        {
            Tiannuo.LogHelper.LogHelper.Error("Error", ex);
            Msg.Result = false;
            Msg.Msg = "验证失败，请稍候再试";
            Msg.Error = true;
            if (ex is System.InvalidOperationException)
            {
                Msg.Msg = "请求参数异常，请重试或刷新页面";
            }
            else if (ex is ApplicationException)
            {
                if (Msg.RegisterOrder == "RegisterNo1")
                {
                    Msg.Error = false;
                }
                Msg.Msg = ex.Message;
            }
            if (Tran != null)
            {
                if (Tran.Connection != null)
                {
                    Tran.Rollback();
                }
            }
        }
        finally
        {
            if (Tran != null)
            {
                if (Tran.Connection != null)
                {
                    Tran.Rollback();
                }
            }
        }
        return Msg;
    }


    public Common.ReturnMessge ExistsDisName(string Name, string Compid)
    {
        Common.ReturnMessge Msg = new Common.ReturnMessge();
        if (!Common.DisExistsAttribute("DisName", Name, Compid))
        {
            Msg.Result = true;
        }
        else
        {
            Msg.Msg = "厂商名称已存在，请重新输入";
        }
        return Msg;
    }

    public Common.ReturnMessge CheckPhoneCode(string Phone, string Phonecode, HttpContext context, bool Ischek = false)
    {
        new loginInfoMation().LoadData();
        Common.ReturnMessge Msg = new Common.ReturnMessge();
        try
        {
            string CheckRegir = "and username!='" + RegisterOrder.已完成第一步注册.ToString() + "'";
            List<Hi.Model.SYS_PhoneCode> Lphone = new Hi.BLL.SYS_PhoneCode().GetList("" + (Ischek ? "*" : "Phonecode") + "", " dr=0 and isPast=0 and '" + DateTime.Now + "' between  createdate and DATEADD(MI,30,CreateDate) and Module='企业注册'  " + CheckRegir + "  and phone='" + Phone + "' ", " CreateDate desc");
            if (Lphone.Count > 0)
            {
                var CheckPhone = Lphone.Where(T => T.PhoneCode == Phonecode).ToList();
                if (CheckPhone.Count > 0)
                {
                    Msg.Result = true;
                    if (Ischek)
                    {
                        CheckPhone[0].modifyuser = UserID;
                        CheckPhone[0].UserName = RegisterOrder.已完成第一步注册.ToString();
                        CheckPhone[0].ts = DateTime.Now;
                        if (!new Hi.BLL.SYS_PhoneCode().Update(CheckPhone[0]))
                        {
                            throw new ApplicationException("修改数据失败");
                        }
                    }
                }
                else
                {
                    Msg.Msg = "验证码错误，请重新输入";
                }
            }
            else
            {
                Msg.Msg = "验证码错误或已失效，请重新获取。";
            }
        }
        catch (Exception ex)
        {
            Tiannuo.LogHelper.LogHelper.Error("Error", ex);
            Msg.Msg = "验证码校验失败，请稍候重试";
        }
        return Msg;
    }

    public Common.ReturnMessge CheckCode(string checkcode, HttpContext context)
    {
        Common.ReturnMessge Msg = new Common.ReturnMessge();
        try
        {
            if (context.Session != null)
            {
                if (checkcode.Equals(context.Session["CheckCode"].ToString()))
                {
                    Msg.Result = true;
                }
                else
                {
                    Msg.Msg = "图文验证码错误";
                }
            }
        }
        catch (Exception ex)
        {
            Tiannuo.LogHelper.LogHelper.Error("Error", ex);
            Msg.Msg = "读取验证码失败。";
        }
        return Msg;
    }


    public Common.ReturnMessge ExistsCompName(string Name)
    {
        Common.ReturnMessge Msg = new Common.ReturnMessge();
        if (!Common.CompExistsAttribute("CompName", Name))
        {
            Msg.Result = true;
        }
        else
        {
            Msg.Msg = "厂商名称已经存在，请重新输入";
        }
        return Msg;
    }

    public Common.ReturnMessge ExistsUsername(string Name)
    {
        Common.ReturnMessge Msg = new Common.ReturnMessge();
        if (!Common.GetUserExists("Username", Name))
        {
            Msg.Result = true;
        }
        else
        {
            Msg.Msg = "登陆帐号已存在，请重新输入";
        }
        return Msg;
    }

    public bool SendPhone(string Phone, HttpContext context, string Msg, bool IsSendComp = false)
    {
        try
        {
            string PhoneCodeAccount = System.Configuration.ConfigurationManager.AppSettings["PhoneCodeAccount"].ToString();
            string PhoneCodePwd = System.Configuration.ConfigurationManager.AppSettings["PhoneCodePwd"].ToString();
            DBUtility.GetPhoneCode getphonecode = new DBUtility.GetPhoneCode();
            getphonecode.GetUser(PhoneCodeAccount, PhoneCodePwd);
            if (IsSendComp)
            {
                string str = getphonecode.ReturnSTRS1(Phone, Msg);
                if (str == "Success")
                {
                    return true;
                }
            }
            else
            {
                string str = getphonecode.ReturnSTR(Phone, Msg);
                if (str == "Success")
                {
                    Hi.Model.SYS_PhoneCode Phonecode = new Hi.Model.SYS_PhoneCode();
                    Phonecode.CreateDate = DateTime.Now;
                    Phonecode.dr = 0;
                    Phonecode.IsPast = 0;
                    Phonecode.modifyuser = 0;
                    Phonecode.ts = DateTime.Now;
                    Phonecode.Phone = Phone;
                    Phonecode.PhoneCode = Msg;
                    Phonecode.Module = "企业注册";
                    Phonecode.UserName = "";
                    Phonecode.Type = 0;
                    new Hi.BLL.SYS_PhoneCode().Add(Phonecode);
                    return true;
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            Tiannuo.LogHelper.LogHelper.Error("Error", ex);
            throw ex;
        }
    }

    public Common.ReturnMessge ExistsUserPhone(string Phone, string RegisterType, string Compid, HttpContext context)
    {
        Common.ReturnMessge Msg = new Common.ReturnMessge();
        new loginInfoMation().LoadData();
        int UserId = 0;
        if (!Common.GetUserExistsWhere("Phone", Phone, " and isenabled=1 ", ref UserId))
        {
            Msg.Result = true;
            return Msg;
        }
        Msg.Id = UserId;
        Msg.IsRegis = true;
        int CompId = Compid.ToInt(0);
        if (RegisterType == "RegiDis" && CompId > 0)
        {
            DataTable table = new Hi.BLL.SYS_CompUser().GetComUser(" isnull(cu.dr,0)=0 and isnull(u.dr,0)=0 and cu.compid=" + CompId + " and u.phone='" + Phone + "' and (cu.ctype=2 or cu.ctype=1) ", "cu.id,cu.IsAudit,cu.UType,cu.ctype");
            if (table.Rows.Count > 0)
            {
                Msg.Error = true;
                if (table.Rows[0]["ctype"].ToString() == "1")
                {
                    Msg.Msg = "您无法申请合作自己的厂商。";
                }
                else
                {
                    if (table.Rows[0]["IsAudit"].ToString() == "2")
                    {
                        if (UserModel != null)
                        {
                            Msg.Msg = "您已申请合作该厂商。";
                        }
                        else
                        {
                            Msg.Msg = "绑定该手机号的用户已申请合作，请直接<a href='/login.aspx' style='color:rgb(66,166,193)'> 登录</a>。";
                        }
                    }
                    else
                    {
                        Msg.Msg = "绑定该手机号的用户已申请合作，请等待审核。";
                    }
                }
                return Msg;
            }
            else
            {
                if (UserModel != null)
                {
                    if (UserId != UserID)
                    {
                        Msg.Result = false;
                        Msg.Msg = "手机号码已注册，请先<a id='RegisLogin' href='javascript:;' style='color:rgb(66,166,193)'> 登录</a>。";
                        Msg.Error = true;
                    }
                    else
                    {
                        if (UserModel.TypeID != 4 && UserModel.TypeID != 5)
                        {
                            Msg.Error = true;
                            Msg.Msg = "您不是管理员，不能进行加盟操作。";
                            return Msg;
                        }
                        else
                        {
                            Msg.Id = UserModel.UserID;
                            Msg.Result = true;
                        }
                    }
                }
                else
                {
                    Msg.Result = false;
                    Msg.Msg = "手机号码已注册，请先<a id='RegisLogin' href='javascript:;' style='color:rgb(66,166,193)'> 登录</a>。";
                    Msg.Error = true;
                }
            }
        }
        else
        {
            if (UserModel == null)
            {
                Msg.Result = false;
                Msg.Error = true;
                Msg.Msg = "手机号码已注册，请先<a id='RegisLogin' href='javascript:;' style='color:rgb(66,166,193)'> 登录</a>。";
            }
            else
            {
                if (UserId != UserID)
                {
                    Msg.Msg = "手机号码已注册，请先<a id='RegisLogin' href='javascript:;' style='color:rgb(66,166,193)'> 登录</a>。";
                    Msg.Error = true;
                    Msg.Result = false;
                }
                else
                {
                    Msg.Id = UserModel.UserID;
                    Msg.Result = true;
                }
            }
        }
        return Msg;
    }
    public enum RegisterOrder
    {
        已完成第一步注册 = 1,
        已完成第二步注册 = 2,
        手机号码已被注册 = 3
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}


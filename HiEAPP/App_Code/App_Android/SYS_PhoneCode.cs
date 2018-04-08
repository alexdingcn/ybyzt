using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBUtility;
using LitJson;

/// <summary>
///SYS_PhoneCode 的摘要说明
/// </summary>
public class SYS_PhoneCode
{
	public SYS_PhoneCode()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 发送验证码
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public PhoneCode GetPhoneCode(string JSon)
    {
        try
        {
            string disID = string.Empty;
            string userID = string.Empty;
            string type = string.Empty;
            string compID = string.Empty;
            int typeInt = 0;
            string Phone = string.Empty;
            string PhoneCode = new Common().CreateRandomCode(6);
            Hi.Model.SYS_Users user = null;
            Hi.Model.SYS_Users userMaster = null;

            #region 赋值、验证

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["Type"].ToString() != "" &&
                JInfo["ResellerID"].ToString() != "" && JInfo["CompanyID"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString();
                type = JInfo["Type"].ToString();
                disID = JInfo["ResellerID"].ToString();
                compID = JInfo["CompanyID"].ToString();
            }
            else
            {
                return new PhoneCode() { Result = "F", Description = "参数不正确" };
            }

            if (!new Common().IsLegitUser(int.Parse(userID), out user,int.Parse(compID),int.Parse(disID == "" ? "0" : disID)))
                return new PhoneCode() { Result = "F", Description = "登录信息异常" };
            if (disID != "")
            {
                userMaster = new Hi.BLL.SYS_Users().GetList("", " Type = 5 and compID='" + compID + "' and disID = '" + disID + "' and dr = 0 and IsEnabled = 1", "")[0];
                if (userMaster == null)
                    return new PhoneCode() { Result = "F", Description = "经销商管理员异常" };
            }

            switch (type.Trim())
            {
                case "10":
                    typeInt = 10;
                    type = "App企业钱包密码修改";
                    Phone = userMaster.Phone;
                    break;
                case "11":
                    typeInt = 11;
                    type = "App修改地址";
                    Phone = userMaster.Phone;
                    break;
                case "12":
                    typeInt = -1;
                    type = "App手机号码登录";
                    Phone = user.Phone;
                    break;
            }

            #endregion

            GetPhoneCode getphonecode = new GetPhoneCode();
            getphonecode.GetUser(
                System.Configuration.ConfigurationManager.AppSettings["PhoneCodeAccount"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["PhoneCodePwd"].ToString());
            string rstr = getphonecode.ReturnSTR(Phone, PhoneCode);
            if (rstr == "Success")
            {
                Hi.Model.SYS_PhoneCode phonecode = new Hi.Model.SYS_PhoneCode();
                phonecode.Type = typeInt;
                phonecode.Module = type;
                phonecode.Phone = Phone;
                phonecode.PhoneCode = PhoneCode;
                phonecode.IsPast = 0;
                phonecode.UserID = int.Parse(userID);
                phonecode.UserName = user.UserName;
                phonecode.CreateDate = DateTime.Now;
                phonecode.ts = DateTime.Now;
                phonecode.modifyuser = int.Parse(userID);
                int i = new Hi.BLL.SYS_PhoneCode().Add(phonecode);
                if (i > 0)
                {
                    return new PhoneCode() { Result = "T", Description = "发送成功", ChangePasswordID = i.ToString() };
                }
                else
                {
                    return new PhoneCode() { Result = "F", Description = "验证码异常" };
                }
            }
            else
            {
                return new PhoneCode() { Result = "F", Description = "发送失败" };
            }
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetPhoneCode:" + JSon);
            return new PhoneCode() { Result = "F", Description = "参数异常" };
        }
    }

    /// <summary>
    /// 根据验证码，修改密码
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public PhoneCode ChangePwdByCode(string JSon)
    {
        try
        {
            string UserID = string.Empty;
            string disID = string.Empty;
            string Type = string.Empty;
            string Password = string.Empty;
            string MessageCode = string.Empty;
            string ChangePasswordID = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "" &&
                JInfo["Type"].ToString() != "" && JInfo["Password"].ToString() != "" &&
                JInfo["MessageCode"].ToString() != "" && JInfo["ChangePasswordID"].ToString() != "")
            {
                UserID = JInfo["UserID"].ToString();
                Type = JInfo["Type"].ToString();
                disID = JInfo["ResellerID"].ToString();
                Password = JInfo["Password"].ToString();
                MessageCode = JInfo["MessageCode"].ToString();
                ChangePasswordID = JInfo["ChangePasswordID"].ToString();
            }
            else
            {
                return new PhoneCode() { Result = "F", Description = "参数不正确" };
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out user, 0,int.Parse(disID == "" ? "0" : disID)))
                return new PhoneCode() { Result = "F", Description = "登录信息异常" };

            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(disID));
            if (dis == null)
                return new PhoneCode() { Result = "F", Description = "经销商异常" };
            
            Hi.Model.SYS_PhoneCode code = new Hi.BLL.SYS_PhoneCode().GetModel(int.Parse(ChangePasswordID));
            if (code != null)
            {
                if (code.ts.AddMinutes(30) < DateTime.Now || code.IsPast == 1)
                    return new PhoneCode() { Result = "F", Description = "验证码过期" };
                if (code.UserID.ToString() != UserID)
                    return new PhoneCode() { Result = "F", Description = "非本人操作" };
                if (code.PhoneCode != MessageCode)
                    return new PhoneCode() { Result = "F", Description = "验证码错误" };
            }
            else
            {
                return new PhoneCode() { Result = "F", Description = "验证码异常" };
            }
            dis.Paypwd = new GetPhoneCode().md5(Password);
            dis.ts = DateTime.Now;
            dis.modifyuser = user.ID;
            if (new Hi.BLL.BD_Distributor().Update(dis))
            {
                code.IsPast = 1;
                code.ts = DateTime.Now;
                code.modifyuser = user.ID;
                if (new Hi.BLL.SYS_PhoneCode().Update(code))
                {
                    return new PhoneCode() { Result = "T", Description = "修改成功" };
                }
                else
                {
                    return new PhoneCode() { Result = "F", Description = "验证失败" };
                }
            }
            return new PhoneCode() { Result = "F", Description = "修改失败" }; ;
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "ChangePwdByCode:" + JSon);
            return new PhoneCode() { Result = "F", Description = "参数异常" };
        }
    }

    /// <summary>
    /// 获取修改登录密码，核心企业入驻的短信验证码
    /// </summary>
    /// <param name="JSon"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public ResultCaptcha GetCaptcha(string JSon, string version)
    {
        string phonenumb = string.Empty;
        string Type = string.Empty;
        int typeint = 0;
        string typename = string.Empty;
        string phonenum = string.Empty;
        List<Hi.Model.SYS_Users> userList = null;
        Hi.BLL.SYS_Users bll_user = new Hi.BLL.SYS_Users();
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["PhoneNumb"].ToString().Trim() != "" && JInfo["Type"].ToString().Trim() != "")
            {
                phonenumb = JInfo["PhoneNumb"].ToString();
                Type = JInfo["Type"].ToString();

            }
            else
            {
                return new ResultCaptcha() { Result = "T", Description = "参数异常" };
            }
            #endregion
            //double loginnum = 0;
            //if (double.TryParse(phonenumb, out loginnum))//如果输入的登录名是整数，则先判断是否是手机号登录，不是手机号登录的话，再判断是否是账号名登录
            //{
            //    userList = bll_user.GetList("",
            //           "Phone = " + phonenumb + " and AuditState =2 and dr=0 and isnull(IsEnabled,0) =1", "");
            //}
            //if (userList == null || userList.Count <= 0)//不是手机号登录的话，再判断是不是账号名登录
            //{
            //    userList = bll_user.GetList("",
            //           "UserName = '" + phonenumb + "' and AuditState =2 and dr=0 and isnull(IsEnabled,0) =1", "");
            //    //如果两种情况都没找到，就是账号不存在
            //    if (userList == null || userList.Count <= 0)
            //        return new ResultCaptcha() { Result = "F", Description = "账号不存在" };
            //}
            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            switch (Type)
            {
                case "1":
                    double loginnum = 0;
                    if (double.TryParse(phonenumb, out loginnum))//如果输入的登录名是整数，则先判断是否是手机号登录，不是手机号登录的话，再判断是否是账号名登录
                    {
                           userList = bll_user.GetList("",
                           "Phone = " + phonenumb + " and AuditState =2 and dr=0 and isnull(IsEnabled,0) =1", "");
                     }
                     if (userList == null || userList.Count <= 0)//不是手机号登录的话，再判断是不是账号名登录
                     {
                           userList = bll_user.GetList("",
                           "UserName = '" + phonenumb + "' and AuditState =2 and dr=0 and isnull(IsEnabled,0) =1", "");
                           //如果两种情况都没找到，就是账号不存在
                           if (userList == null || userList.Count <= 0)
                               return new ResultCaptcha() { Result = "F", Description = "账号不存在" };
                     }
                     user = userList[0];
                    typeint = -5;
                    typename = "修改登录密码";
                    phonenum = user.Phone;
                    break;
                case "2":
                    //首先验证这手机号是否已经注册
                    List<Hi.Model.SYS_Users> list_users = bll_user.GetList("",
                           "Phone = " + phonenumb + " and AuditState =2 and dr=0 and isnull(IsEnabled,0) =1", "");
                    //如果已经注册，就不发送验证码，并提示
                    if(list_users != null && list_users.Count>0)
                        return new ResultCaptcha() { Result = "F", Description = "该手机号码已注册过账号，请使用未注册的手机号码注册" };
                    typeint = -10;
                    typename = "核心企业注册";
                    phonenum = phonenumb;
                    break;
                default:
                    return new ResultCaptcha() { Result = "F", Description = "操作类型不存在" };

            }
            //获取六位随机数
            string PhoneCode = new Common().CreateRandomCode(6);
            GetPhoneCode getphonecode = new GetPhoneCode();
            getphonecode.GetUser(
                System.Configuration.ConfigurationManager.AppSettings["PhoneCodeAccount"].ToString(),
                System.Configuration.ConfigurationManager.AppSettings["PhoneCodePwd"].ToString());
            //手机发送验证码
            string rstr = getphonecode.ReturnSTR(phonenum, PhoneCode);
            //信息发送成功的话需要在sys_phonecode表中插入一条数据
            if (rstr == "Success")
            {
                Hi.Model.SYS_PhoneCode phonecode = new Hi.Model.SYS_PhoneCode();
                phonecode.Type = typeint;
                phonecode.Module = typename;
                phonecode.Phone = phonenum;
                phonecode.PhoneCode = PhoneCode;
                phonecode.IsPast = 0;
                phonecode.UserID = user.ID;
                phonecode.UserName = "";
                phonecode.CreateDate = DateTime.Now;
                phonecode.ts = DateTime.Now;
                phonecode.modifyuser = user.ID;
                int i = new Hi.BLL.SYS_PhoneCode().Add(phonecode);
                if (i > 0)//新增成功的话拼接返回参数
                {

                    ResultCaptcha resultcaptcha = new ResultCaptcha();
                    resultcaptcha.Result = "T";
                    resultcaptcha.Description = "返回成功";
                    resultcaptcha.SendId = i.ToString();
                    resultcaptcha.Captcha = PhoneCode;
                    resultcaptcha.PhoneNumb = phonenum;
                    return resultcaptcha;
                }
                else
                {
                    return new ResultCaptcha() { Result = "F", Description = "验证码异常" };
                }

            }
            else
            {
                return new ResultCaptcha() { Result = "F",Description = "发送失败"};
            }
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetCaptcha:" + JSon);
            return new ResultCaptcha() { Result = "F", Description = "参数异常" };
        }

    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="JSon"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public ResultLoginCaptcha GetLoginCaptcha(string JSon, string version)
    {
        string phonenumb = string.Empty;
        string password = string.Empty;
        string SendID = string.Empty;
        string Captcha = string.Empty;
        List<Hi.Model.SYS_Users> userList = new List<Hi.Model.SYS_Users>();
        Hi.BLL.SYS_Users bll_user = new Hi.BLL.SYS_Users();
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["PhoneNumb"].ToString().Trim() != "" && JInfo["Pwd"].ToString().Trim() != ""
                && JInfo["SendId"].ToString().Trim() != "" && JInfo["Captcha"].ToString().Trim() != "")
            {
                phonenumb = JInfo["PhoneNumb"].ToString();
                password = JInfo["Pwd"].ToString();
                SendID = JInfo["SendId"].ToString();
                Captcha = JInfo["Captcha"].ToString();
            }
            else
            {
                return new ResultLoginCaptcha() { Result ="F",Description = "参数异常"};
            }
#endregion
            #region
            //判断登录账号是否存在
            double loginnum = 0;
            //如果传入的登录账号是整数，先判断是否手机号登录
            if (double.TryParse(phonenumb, out loginnum))
            {
                userList = bll_user.GetList("",
                      "Phone = " + phonenumb + " and AuditState =2 and dr=0 and isnull(IsEnabled,0) =1", "");
            }
            if (userList.Count <= 0)
            {
                userList = bll_user.GetList("",
                     "UserName = '" + phonenumb + "' and AuditState =2 and dr=0 and isnull(IsEnabled,0) =1", "");
                //如果两种情况都没找到，就是账号不存在
                if (userList == null || userList.Count <= 0)
                    return new ResultLoginCaptcha() { Result = "F", Description = "账号不存在" };
            }
            Hi.Model.SYS_Users user = userList[0];
            //验证验证码是否正确
            Hi.Model.SYS_PhoneCode code = new Hi.BLL.SYS_PhoneCode().GetModel(int.Parse(SendID));
            if (code != null && code.dr == 0)
            {
                if (code.ts.AddMinutes(30) < DateTime.Now || code.IsPast == 1)
                    return new ResultLoginCaptcha() { Result = "F", Description = "验证码过期" };
                if (code.UserID.ToString() != user.ID.ToString())
                    return new ResultLoginCaptcha() { Result = "F", Description = "非本人操作" };
                if (code.PhoneCode != Captcha)
                    return new ResultLoginCaptcha() { Result = "F", Description = "验证码错误" };
            }
            else
            {
                return new ResultLoginCaptcha() { Result = "F", Description = "验证码不可用" };
            }
            if (user.UserPwd == new GetPhoneCode().md5(password))
            {
                return new ResultLoginCaptcha() { Result = "F", Description = "新密码不能与老密码相同" };
            }
            code.IsPast = 1;
            code.ts = DateTime.Now;
            code.modifyuser = user.ID;
            //更新sys_phonecode中此验证码的状态为已使用
            if (new Hi.BLL.SYS_PhoneCode().Update(code))
            {
                user.UserPwd = new GetPhoneCode().md5(password);
                user.ts = DateTime.Now;
                user.modifyuser = user.ID;
                //更新登录密码
                if (new Hi.BLL.SYS_Users().Update(user))
                {
                    return new ResultLoginCaptcha() { Result = "T", Description = "修改成功" };
                }
                else
                {
                    return new ResultLoginCaptcha() { Result = "F", Description = "修改失败" };
                }
            }
            else
            {
                return new ResultLoginCaptcha() { Result = "F",Description = "验证码异常"};
            }

            #endregion
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetLoginCaptcha:" + JSon);
            return new ResultLoginCaptcha() { Result = "F", Description = "参数异常" };
        }
    }
    public class PhoneCode
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public String ChangePasswordID { get; set; }
    }
    //短信验证码的返回实体
    public class ResultCaptcha
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public String SendId { get; set; }//验证码在sys_phonecode表中的id
        public String Captcha { get; set; }//验证码
        public String PhoneNumb { get; set; }//手机号
    }
    //修改密码的返回参数
    public class ResultLoginCaptcha
    {
        public String Result { get; set; }
        public String Description { get; set; }
    }
}
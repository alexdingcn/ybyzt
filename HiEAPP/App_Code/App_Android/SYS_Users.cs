using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Hosting;
using LitJson;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Configuration;

public class SYS_Users
{
    public SYS_Users()
    {
    }

    #region

    /// <summary>
    /// 通过openid获取用户id
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultUserID GetUserIDByWXOpenid(string JSon)
    {
        try
        {
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 || JInfo["OpenID"].ToString() != "")
            {
                Hi.Model.SYS_Users user = new Common().GetUserByOpenID(JInfo["OpenID"].ToString());
                //|| user.Type == 2 || user.Type == 4 || user.Type == 5 ||user.IsEnabled==0
                if (user == null || user.dr == 1)
                    return new ResultUserID() {Result = "F", Description = "绑定用户信息异常"};
                return new ResultUserID()
                {
                    Result = "T",
                    Description = "获取成功",
                    UserID = user.ID.ToString(),
                    //BussID = user.Type == 1 ? user.CompID.ToString() : user.DisID.ToString(),
                    UserType = user.Type == 1 ? "1" : "0"
                };
            }
            return new ResultUserID() {Result = "F", Description = "参数异常"};
        }
        catch
        {
            Common.CatchInfo(JSon, "GetUserIDByWXOpenid");
            return new ResultUserID() {Result = "F", Description = "参数异常"};
        }
    }

    /// <summary>
    /// 绑定微信用户登录 
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultWXLogin WXLogin(string JSon)
    {
        try
        {
            #region JSon取值

            string LoginName = string.Empty;
            string PassWord = string.Empty;
            string OpenID = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["LoginName"].ToString() != "" && JInfo["PassWord"].ToString() != "" &&
                JInfo["OpenID"].ToString().Trim() != "")
            {
                LoginName = JInfo["LoginName"].ToString();
                PassWord = JInfo["PassWord"].ToString();
                OpenID = JInfo["OpenID"].ToString();
                //"{"LoginName":"def","PassWord":"e10adc3949ba59abbe56e057f20f883e","OpenID":"def"}"
            }
            else
            {
                return new ResultWXLogin() {Result = "F", Description = "参数为空异常"};
            }

            #endregion

            //List<Hi.Model.SYS_Users> list = new Hi.BLL.SYS_Users().GetList("", " OpenID='" + OpenID.Trim() + "'", "");
            //if (list.Count > 0)
            //    return new ResultWXLogin() {Result = "F", Description = "微信号不能重复绑定"};
            //List<Hi.Model.SYS_Users> userList = new Hi.BLL.SYS_Users().GetList("",
            //    " UserName='" + LoginName.Trim() + "'", "");
            //if (userList.Count > 1)
            //    return new ResultWXLogin() {Result = "F", Description = "用户重名异常"};
            //if (userList.Count == 0)
            //    return new ResultWXLogin() {Result = "F", Description = "未找到用户"};
            //Hi.Model.SYS_Users user = userList[0];

            //if (user.IsEnabled == 0 || user.dr == 1)
            //    return new ResultWXLogin() {Result = "F", Description = "用户已删除或已禁用"};
            //if (user.UserPwd != PassWord)
            //    return new ResultWXLogin() {Result = "F", Description = "登录密码不对"};
            //if (user.Type == 2)
            //    return new ResultWXLogin() {Result = "F", Description = "用户类型异常"};
            //if (!string.IsNullOrEmpty(user.OpenID))
            //    return new ResultWXLogin() {Result = "F", Description = "该账号已绑定"};
            //user.OpenID = OpenID;
            //user.ts = DateTime.Now;
            //user.modifyuser = 100000;
            //bool res = new Hi.BLL.SYS_Users().Update(user);
            //if (!res)
            //    return new ResultWXLogin() {Result = "F", Description = "绑定失败"};

            //string UserType = (user.Type == 1 || user.Type == 5) ? "0" : "1";
            //string BussID = (user.Type == 1 || user.Type == 5) ? user.DisID.ToString() : user.CompID.ToString();
            //string ParentName = string.Empty;
            //string CompName = string.Empty;
            //Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(user.CompID);
            //if (comp == null)
            //    return new ResultWXLogin() {Result = "F", Description = "企业信息异常"};
            //CompName = comp.CompName;
            //if (UserType == "0")
            //{
            //    Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(user.DisID);
            //    if (dis == null || dis.CompID != user.CompID)
            //        return new ResultWXLogin() {Result = "F", Description = "经销商信息异常"};
            //    ParentName = comp.CompName;
            //}
            //return new ResultWXLogin()
            //{
            //    Result = "T",
            //    Description = "绑定成功",
            //    UserID = user.ID.ToString(),
            //    TrueName = user.TrueName,
            //    Sex = user.Sex,
            //    Phone = user.Phone,
            //    Email = user.Email,
            //    UserType = UserType,
            //    BussID = BussID,
            //    ParentName = ParentName,
            //    CompName = CompName
            //};

            return null;
        }
        catch
        {
            Common.CatchInfo(JSon, "WXLogin");
            return new ResultWXLogin() {Result = "F", Description = "参数异常"};
        }
    }

    /// <summary>
    /// 解除微信用户绑定
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultWXLogin WXRelieveUser(string JSon)
    {
        try
        {
            #region JSon取值

            string UserID = string.Empty;
            string OpenID = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" &&
                JInfo["OpenID"].ToString().Trim() != "")
            {
                UserID = JInfo["UserID"].ToString();
                OpenID = JInfo["OpenID"].ToString();
                //"{"LoginName":"def","PassWord":"e10adc3949ba59abbe56e057f20f883e","OpenID":"def"}"
            }
            else
            {
                return new ResultWXLogin() {Result = "F", Description = "参数为空异常"};
            }

            #endregion

            List<Hi.Model.SYS_Users> list = new Hi.BLL.SYS_Users().GetList("", " OpenID='" + OpenID.Trim() + "'", "");
            if (list == null || list.Count == 0)
                return new ResultWXLogin() {Result = "F", Description = "该微信号未关联用户"};
            Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(int.Parse(UserID));
            if (user == null)
                return new ResultWXLogin() {Result = "F", Description = "用户名异常"};

            if (user.IsEnabled == 0 || user.dr == 1)
                return new ResultWXLogin() {Result = "F", Description = "用户已删除或已禁用"};
            if (user.Type == 2)
                return new ResultWXLogin() {Result = "F", Description = "账号密码错误"};
            if (string.IsNullOrEmpty(user.OpenID))
                return new ResultWXLogin() {Result = "F", Description = "该用户未绑定微信"};
            user.OpenID = null;
            user.ts = DateTime.Now;
            user.modifyuser = user.ID;
            bool res = new Hi.BLL.SYS_Users().Update(user);
            if (!res)
                return new ResultWXLogin() {Result = "F", Description = "解除绑定失败"};

            //string UserType = (user.Type == 1 || user.Type == 5) ? "0" : "1";
            //string BussID = (user.Type == 1 || user.Type == 5) ? user.CompID.ToString() : user.DisID.ToString();
            //string ParentName = string.Empty;
            //if (new Hi.BLL.BD_Company().GetModel(user.CompID) != null && user.Type == 1)
            //    ParentName = new Hi.BLL.BD_Company().GetModel(user.CompID).CompName;
            //string CompName = string.Empty;
            //if (user.Type == 1 && new Hi.BLL.BD_Distributor().GetModel(user.DisID) != null)
            //    CompName = new Hi.BLL.BD_Distributor().GetModel(user.DisID).DisName;
            //else if (new Hi.BLL.BD_Company().GetModel(user.CompID) != null)
            //    CompName = new Hi.BLL.BD_Company().GetModel(user.CompID).CompName;

            return new ResultWXLogin()
            {
                Result = "T",
                Description = "解除成功",
                UserID = user.ID.ToString()
                //TrueName = user.TrueName,
                //Sex = user.Sex,
                //Phone = user.Phone,
                //Email = user.Email,
                //UserType = UserType,
                //BussID = BussID,
                //ParentName = ParentName,
                //CompName = CompName
            };
        }
        catch
        {
            Common.CatchInfo(JSon, "WXRelieveUser");
            return new ResultWXLogin() {Result = "F", Description = "参数异常"};
        }
    }

    /// <summary>
    /// 经销商获取个人信息
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultDisInfo GetResellerInfo(string JSon)
    {
        try
        {
            #region JSon取值

            string userID = string.Empty;
            string disID = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString();
                disID = JInfo["ResellerID"].ToString();
                //"{"LoginName":"def","PassWord":"e10adc3949ba59abbe56e057f20f883e","OpenID":"def"}"
            }
            else
            {
                return new ResultDisInfo() {Result = "F", Description = "参数异常"};
            }

            #endregion

            Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(int.Parse(userID));
            if (user == null || user.IsEnabled == 0 || user.dr == 1)
                return new ResultDisInfo() {Result = "F", Description = "用户异常"};
            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(disID));
            if (dis == null)
                return new ResultDisInfo() {Result = "F", Description = "经销商异常"};
            Hi.Model.BD_DisType disType = new Hi.BLL.BD_DisType().GetModel(dis.DisTypeID);
            string DisTypeName = string.Empty;
            if (disType != null)
            {
                DisTypeName = disType.TypeName;
            }
            Hi.Model.BD_DisArea area = new Hi.BLL.BD_DisArea().GetModel(dis.AreaID);
            string AreaName = string.Empty;
            if (area != null)
            {
                AreaName = area.AreaName;
            }
            return new ResultDisInfo()
            {
                Result = "T",
                Description = "获取成功",
                DisName = dis.DisName,
                DisTypeName = DisTypeName,
                AreaName = AreaName,
                DisPrincipal = dis.Principal,
                DisPhone = dis.Phone,
                IsCheck = dis.IsCheck.ToString(),
                CreditType = dis.CreditType.ToString(),
                UserName = user.UserName,
                UserSex = user.Sex,
                UserPhone = user.Phone,
                PrePaymentMoney = new Hi.BLL.PAY_PrePayment().sums(dis.ID, dis.CompID).ToString()
            };
        }
        catch
        {
            Common.CatchInfo(JSon, "GetResellerInfo");
            return new ResultDisInfo() {Result = "F", Description = "参数异常"};
        }
    }

    /// <summary>
    /// 通过OpenID获取经销商个人信息
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultDisInfo GetUserInfo(string JSon)
    {
        try
        {
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count == 0 && JInfo["OpenID"].ToString().Trim() == "")
            {
                return new ResultDisInfo() {Result = "F", Description = "参数异常"};
            }

            Hi.Model.SYS_Users user = new Common().GetUserByOpenID(JInfo["OpenID"].ToString());
            if (user == null || user.IsEnabled == 0 || user.dr == 1)
                return new ResultDisInfo() {Result = "T", Description = "用户异常"};
            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(JInfo["DisID"].ToString()));
            if (dis == null || dis.IsEnabled == 0 || dis.dr == 1)
                return new ResultDisInfo() {Result = "T", Description = "经销商异常"};
            Hi.Model.BD_DisType disType = new Hi.BLL.BD_DisType().GetModel(dis.DisTypeID);
            string DisTypeName = string.Empty;
            if (disType != null)
            {
                DisTypeName = disType.TypeName;
            }
            Hi.Model.BD_DisArea area = new Hi.BLL.BD_DisArea().GetModel(dis.AreaID);
            string AreaName = string.Empty;
            if (area != null)
            {
                AreaName = area.AreaName;
            }
            return new ResultDisInfo()
            {
                Result = "T",
                Description = "获取成功",
                DisName = dis.DisName,
                DisTypeName = DisTypeName,
                AreaName = AreaName,
                DisPrincipal = dis.Principal,
                DisPhone = dis.Phone,
                IsCheck = dis.IsCheck.ToString(),
                CreditType = dis.CreditType.ToString(),
                UserName = user.UserName,
                UserSex = user.Sex,
                UserPhone = user.Phone,
                PrePaymentMoney = new Hi.BLL.PAY_PrePayment().sums(dis.ID, dis.CompID).ToString()
            };
        }
        catch
        {
            Common.CatchInfo(JSon, "GetResellerInfo");
            return new ResultDisInfo() {Result = "F", Description = "参数异常"};
        }
    }

    /// <summary>
    /// 经销商登录
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultDisLog ResellerLogin(string JSon)
    {
        try
        {
            #region JSon取值

            string LoginName = string.Empty;
            string PassWord = string.Empty;
            string AndroidKey = string.Empty;
            string IOSKey = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["LoginName"].ToString() != "" && JInfo["PassWord"].ToString() != "")
            {
                LoginName = JInfo["LoginName"].ToString();
                PassWord = JInfo["PassWord"].ToString();
                //AndroidKey = JInfo["AndroidKey"].ToString();
                //IOSKey = JInfo["IOSKey"].ToString();
            }
            else
            {
                return new ResultDisLog() {Result = "F", Description = "参数为空异常"};
            }

            #endregion

            List<Hi.Model.SYS_Users> userList = new Hi.BLL.SYS_Users().GetList("",
                " UserName='" + LoginName.Trim() + "' and AuditState =2 and dr=0 and IsEnabled = 1", "");
            if (userList == null || userList.Count != 1)
                return new ResultDisLog() {Result = "F", Description = "账号密码错误"};

            List<ResultUser> UserList = new List<ResultUser>();
            var user = userList[0];

            if (user.UserPwd != PassWord)
                return new ResultDisLog() {Result = "F", Description = "账号密码错误"};

            List<Hi.Model.SYS_CompUser> compUserList = new Hi.BLL.SYS_CompUser().GetList("",
                "UserID='" + user.ID + "' and IsNull(dr,0)=0 ", "");
            if (compUserList != null && compUserList.Count > 0)
            {
                foreach (var compUser in compUserList)
                {
                    Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(compUser.CompID);
                    if (comp == null)
                        return new ResultDisLog() {Result = "F", Description = "企业异常"};

                    ResultUser resultUser = new ResultUser();
                    resultUser.UserID = user.ID.ToString();
                    resultUser.TrueName = user.TrueName;
                    resultUser.Sex = user.Sex;
                    resultUser.Phone = user.Phone;
                    if (compUser.DisID != 0)
                    {
                        Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(compUser.DisID);
                        if (dis == null)
                            return new ResultDisLog() {Result = "F", Description = "经销商异常"};
                        resultUser.ResellerID = compUser.DisID.ToString();
                        resultUser.ResellerName = dis.DisName;
                    }
                    resultUser.IsEnabled = user.IsEnabled.ToString();
                    resultUser.Erptype = comp.Erptype.ToString();
                    resultUser.ConpamyID = compUser.CompID.ToString();
                    resultUser.CompanyName = comp.CompName;
                    if (user.Type == 1 || user.Type == 5)
                        resultUser.UserType = "0"; //0：经销商 1：核心企业
                    else if (user.Type == 3 || user.Type == 4)
                        resultUser.UserType = "1"; //0：经销商 1：核心企业
                    UserList.Add(resultUser);
                }
            }
            return new ResultDisLog()
            {
                Result = "T",
                Description = "登录成功",
                UserList = UserList
            };
        }
        catch
        {
            Common.CatchInfo(JSon, "ResellerLogin");
            return new ResultDisLog() {Result = "F", Description = "参数异常"};
        }
    }

    /// <summary>
    /// 企业登录
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultCompLog CompanyLogin(string JSon)
    {
        try
        {
            #region JSon取值

            string LoginName = string.Empty;
            string PassWord = string.Empty;
            string AndroidKey = string.Empty;
            string IOSKey = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["LoginName"].ToString() != "" && JInfo["PassWord"].ToString() != "")
            {
                LoginName = JInfo["LoginName"].ToString();
                PassWord = JInfo["PassWord"].ToString();
                //AndroidKey = JInfo["AndroidKey"].ToString();
                //IOSKey = JInfo["IOSKey"].ToString();
            }
            else
            {
                return new ResultCompLog() {Result = "F", Description = "参数为空异常"};
            }

            #endregion

            List<Hi.Model.SYS_Users> userList = new Hi.BLL.SYS_Users().GetList("",
                " UserName='" + LoginName.Trim() + "'", "");
            if (userList == null || userList.Count == 0)
                return new ResultCompLog() {Result = "F", Description = "用户不存在"};
            if (userList.Count > 1)
                return new ResultCompLog() {Result = "F", Description = "用户重名异常"};
            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            foreach (var auser in userList)
            {
                user = auser;
            }
            if (user.AuditState != 2)
                return new ResultCompLog() {Result = "F", Description = "请等待审核通过"};
            if (user.dr == 1)
                return new ResultCompLog() {Result = "F", Description = "用户已删除"};
            if (user.IsEnabled == 0)
                return new ResultCompLog() {Result = "F", Description = "用户已禁用"};
            if (user.UserPwd != PassWord)
                return new ResultCompLog() {Result = "F", Description = "登录密码不对"};
            if (user.Type != 3 && user.Type != 4)
                return new ResultCompLog() {Result = "F", Description = "用户名或密码错误"};
            List<Hi.Model.SYS_CompUser> compUserList = new Hi.BLL.SYS_CompUser().GetList("",
                "UserID='" + user.ID + "' and IsNull(dr,0)=0 ", "");
            return null;

            //Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(user.CompID);
            //if (comp == null)
            //    return new ResultCompLog() {Result = "F", Description = "企业异常"};

            //return new ResultCompLog()
            //{
            //    Result = "T",
            //    Description = "登录成功",
            //    UserID = user.ID.ToString(),
            //    TrueName = user.TrueName,
            //    Sex = user.Sex,
            //    Phone = user.Phone,
            //    ConpamyID = user.CompID.ToString(),
            //    CompanyName = comp.CompName,
            //    IsEnabled = user.IsEnabled.ToString(),
            //    Erptype = comp.Erptype.ToString()
            //};
        }
        catch
        {
            Common.CatchInfo(JSon, "CompanyLogin");
            return new ResultCompLog() {Result = "F", Description = "参数异常"};
        }
    }

    #endregion

    #region 多用户的代码

    public ResultLogin Login(string JSon,string version)
    {
        try
        {
            #region JSon取值

            string LoginName = string.Empty;
            string PassWord = string.Empty;
            List<Hi.Model.SYS_Users> userList = null;
            Hi.BLL.SYS_Users bll_user = new Hi.BLL.SYS_Users();
            int phone =0 ;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["LoginName"].ToString() != "" && JInfo["PassWord"].ToString() != "")
            {
                LoginName = JInfo["LoginName"].ToString();
                PassWord = JInfo["PassWord"].ToString();
            }
            else
            {
                return new ResultLogin() {Result = "F", Description = "参数为空异常"};
            }

            #endregion
            //if (int.TryParse(LoginName, out phone))//如果登录账号输入的是整数，先判断是否手机号登录，没对应的手机号，再判断账号登录
            //{
            //    userList = bll_user.GetList("",
            //        "Phone = " + LoginName + " and AuditState =2 and dr=0 and isnull(IsEnabled,0) =1", "");
            //}
            if (version.ToLower() != "android" && version.ToLower() != "ios" && float.Parse(version) >= 8)
                PassWord = AESHelper.Encrypt_MD5(PassWord);
            double phone_double = 0;
            if (double.TryParse(LoginName,out phone_double))//如果登录账号输入的是整数，先判断是否手机号登录，没对应的手机号，再判断账号登录
            {
                userList = bll_user.GetList("",
                    "Phone = '" + LoginName + "' and AuditState =2 and dr=0 and isnull(IsEnabled,0) =1", "");
            }
            if (userList == null || userList.Count <= 0)
            {
                userList = new Hi.BLL.SYS_Users().GetList("",
                    " UserName='" + LoginName.Trim() + "' and AuditState =2 and dr=0 and isnull(IsEnabled,0) =1", "");
                if (userList == null || userList.Count == 0)
                    return new ResultLogin() { Result = "F", Description = "账号密码错误" };
            }
            //if (userList == null || userList.Count == 0)
            //    return new ResultLogin() { Result = "F", Description = "账号密码错误" };
            

            List<User> UserList = new List<User>();
            foreach (var user in userList)
            {
                if (user.UserPwd !=PassWord)
                    return new ResultLogin() {Result = "F", Description = "账号密码错误"};

                List<Hi.Model.SYS_CompUser> compUserList = new Hi.BLL.SYS_CompUser().GetList("",
                    "UserID='" + user.ID + "' and IsAudit=2 and IsNull(dr,0)=0  and isnull(IsEnabled,0) = 1", "");
                if (compUserList != null && compUserList.Count > 0)
                {
                    foreach (var compUser in compUserList)
                    {
                        User resultUser = new User();

                        resultUser.UserID = compUser.UserID;
                        resultUser.CompID = compUser.CompID;

                        resultUser.TrueName = user.TrueName;
                        resultUser.Sex = user.Sex;
                        resultUser.Phone = user.Phone;
                        resultUser.IsEnabled = user.IsEnabled; //列表无判断，前台要使用此状态

                        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(compUser.CompID);
                        if (comp == null)
                            return new ResultLogin() { Result = "F", Description = "核心企业异常" };
                        resultUser.Erptype = comp.Erptype;
                        resultUser.CompName = comp.CompName;

                        resultUser.UType = compUser.UType;
                        resultUser.CType = compUser.CType; // 1：核心企业  2：经销商
                        resultUser.CompUserID = compUser.ID;
                        if (compUser.CType == 2)
                        {
                            resultUser.DisID = compUser.DisID;
                            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(compUser.DisID);
                            if (dis == null)
                                return new ResultLogin() { Result = "F", Description = "经销商异常" };
                            if (dis.IsEnabled == 0 || dis.dr == 1)
                                continue;

                            resultUser.DisName = dis.DisName;
                        }

                        UserList.Add(resultUser);
                    }
                }
                else
                {
                    return new ResultLogin() { Result = "F", Description = "未找到登录信息" };
                }
            }
            return new ResultLogin()
            {
                Result = "T",
                Description = "登录成功",
                UserList = UserList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "Login:" + JSon);
            return new ResultLogin() {Result = "F", Description = "参数异常"};
        }
    }

    public ResultLogin WXGetUserinfo(string JSon, string version)
    {
        List<Hi.Model.SYS_Users> userList = null;
        try
        {
            string code = "";
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["code"].ToString() != "")
            {
                code = JInfo["code"].ToString();
            }
            else
            {
                return new ResultLogin() { Result = "F", Description = "参数异常" };
            }

            string openid = getopenid(code);
            if(openid =="")
                return new ResultLogin() { Result = "1", OpenID = openid }; 
            int length_openid = openid.Length;

            userList = new Hi.BLL.SYS_Users().GetList("",
    "LEFT(ISNULL(OpenID,'')," + length_openid + ") ='" + openid + "' and AuditState =2 and dr=0 and isnull(IsEnabled,0) =1", "");
            if (userList == null || userList.Count == 0)
                return new ResultLogin() { Result = "1", OpenID = openid };

            List<User> UserList = new List<User>();
            foreach (var user in userList)
            {
                string openidandcompuserid = user.OpenID;
                int length = openidandcompuserid.LastIndexOf("&&");
                Common.CatchInfo(openidandcompuserid.Substring(length + 2), "");
                int lastcompuserid = Int32.Parse(openidandcompuserid.Substring(length+2));

                List<Hi.Model.SYS_CompUser> compUserList = new Hi.BLL.SYS_CompUser().GetList("",
                    "UserID='" + user.ID + "' and IsAudit=2 and IsNull(dr,0)=0  and isnull(IsEnabled,0) = 1", "");
                if (compUserList != null && compUserList.Count > 0)
                {
                    foreach (var compUser in compUserList)
                    {
                        User resultUser = new User();

                        if (compUser.ID == lastcompuserid)
                            resultUser.isLastTime = "1";
                        else
                            resultUser.isLastTime = "0";


                        resultUser.UserID = compUser.UserID;
                        resultUser.CompID = compUser.CompID;

                        resultUser.TrueName = user.TrueName;
                        resultUser.Sex = user.Sex;
                        resultUser.Phone = user.Phone;
                        resultUser.IsEnabled = user.IsEnabled; //列表无判断，前台要使用此状态

                        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(compUser.CompID);
                        if (comp == null)
                            return new ResultLogin() { Result = "F", Description = "核心企业异常" };
                        resultUser.Erptype = comp.Erptype;
                        resultUser.CompName = comp.CompName;

                        resultUser.UType = compUser.UType;
                        resultUser.CType = compUser.CType; // 1：核心企业  2：经销商
                        resultUser.CompUserID = compUser.ID;
                        if (compUser.CType == 2)
                        {
                            resultUser.DisID = compUser.DisID;
                            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(compUser.DisID);
                            if (dis == null)
                                return new ResultLogin() { Result = "F", Description = "经销商异常" };
                            if (dis.IsEnabled == 0 || dis.dr == 1)
                                continue;

                            resultUser.DisName = dis.DisName;
                        }

                        UserList.Add(resultUser);
                    }
                }
                else
                {
                    return new ResultLogin() { Result = "F", Description = "未找到登录信息" };
                }
            }

            return new ResultLogin()
            {
                Result = "T",
                Description = "登录成功",
                OpenID = openid,
                UserList = UserList

            };

        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "WXGetUserinfo:" + JSon);
            return new ResultLogin() { Result = "F", Description = "参数异常" };
        }
    }

    public void EditOpenID(string JSon, string version)
    {
        try
        {
            string openid = string.Empty;
            string CompUserID = string.Empty;
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0  && JInfo["CompUserID"].ToString()!="")
            {
                openid = JInfo["OpenID"].ToString().Trim();
                CompUserID = JInfo["CompUserID"].ToString().Trim();
            }
            else
            {
                return;
            }
            Hi.Model.SYS_CompUser compuser = new Hi.BLL.SYS_CompUser().GetModel(int.Parse(CompUserID));
            if (compuser == null || compuser.IsAudit != 2 || compuser.dr == 1 || compuser.IsEnabled != 1)
                return;
            Hi.BLL.SYS_Users bll_user = new Hi.BLL.SYS_Users();
            Hi.Model.SYS_Users user = bll_user.GetModel(compuser.UserID);
            if (user == null || user.AuditState != 2 || user.dr == 1 || user.IsEnabled != 1)
                return;

            if (openid == "")
                user.OpenID = "";
            else
                user.OpenID = openid + "&&" + compuser.ID;
            user.ts = DateTime.Now;
            user.modifyuser = user.ID;

            

            bll_user.Update(user);
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "EditOpenID:" + JSon);
            return;
        }
    }

    public string getopenid(string code)
    {
        try
        {

            string appid = ConfigurationManager.AppSettings["WXAppID"];
            string secret = ConfigurationManager.AppSettings["WXSecret"];
            //string code = "0817tpx51pVaO1q97x51aomx517tpxY";
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appid + "&secret=" + secret + "&code=" + code + "&grant_type=authorization_code";



            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";







            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);

            string add = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                add = reader.ReadToEnd();
            }
            if (add == null || add == "")
                return "";
            JsonData js_token = JsonMapper.ToObject(add);
            string openid = js_token["openid"].ToString();
            return openid;



        }
        catch (Exception ex)
        {
            throw ex;
        } 
    }

   

    public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    {   // 总是接受  
        return true;
    }

    //public ResultAddLogin AddLoginLog(string JSon)
    //{
    //    string UserID = string.Empty;
    //    string CompUserID = string.Empty;
    //    #region//JSon取值
    //    try
    //    {
    //    JsonData JInfo = JsonMapper.ToObject(JSon);
    //    if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompUserID"].ToString().Trim() == "")
    //    {
    //        return new ResultAddLogin() { Result = "F", Description = "参数异常" };
    //    }
    //    else
    //    {
    //        UserID = JInfo["UserID"].ToString();
    //        CompUserID = JInfo["CompUserID"].ToString();
    //    }
    //    #endregion
    //    Hi.Model.SYS_Users use = new Hi.BLL.SYS_Users().GetModel(Int32.Parse(UserID));
    //         Hi.Model.SYS_CompUser compuser = new Hi.BLL.SYS_CompUser().GetModel(Int32.Parse(CompUserID));
    //    if (use == null || compuser == null)
    //    {
    //        return new ResultAddLogin() { Result = "F", Description = "参数异常" };
    //    }
    //    if (use.dr == 1 || use.IsEnabled != 1 || use.AuditState != 2)
    //    {
    //        Common.EditLog("安全日志", use.UserName, "用户" + use.UserName + "登录管理系统失败用户状态异常。", "系统安全模块", "", 0, 0, compuser.UType);
    //        return new ResultAddLogin() { Result="F",Description = "参数异常"};
    //    }
    //    if (compuser.dr == 1 || compuser.IsEnabled != 1 || compuser.IsAudit == 0)
    //    {
    //        Common.EditLog("安全日志", use.UserName, "用户" + use.UserName + "登录管理系统失败核心企业管理员状态异常。", "系统安全模块", "", 0, 0, compuser.UType);
    //        return new ResultAddLogin() { Result = "F", Description = "参数异常" };
    //    }
    //    Common.EditLog("安全日志", use.UserName, "用户" + use.UserName + "登录管理系统成功。", "系统安全模块", "", 0, 1, compuser.UType);
    //    return new ResultAddLogin() { Result = "T", Description = "登录成功" };
    //    }
    //    catch(Exception ex)
    //    {
    //        Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "AddLoginLog:" + JSon);
    //        return new ResultAddLogin() {Result = "F", Description = "参数异常"};
    //    }
        
    //}

    public ResultLogin LoginByPhone(string JSon)
    {
        try
        {
            #region JSon取值

            string phone = string.Empty;
            string code = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["LoginPhone"].ToString() != "" && JInfo["CheckCode"].ToString() != "")
            {
                phone = JInfo["LoginPhone"].ToString();
                code = JInfo["CheckCode"].ToString();
            }
            else
            {
                return new ResultLogin() { Result = "F", Description = "参数为空异常" };
            }

            #endregion

            List<Hi.Model.SYS_Users> userList = new Hi.BLL.SYS_Users().GetList("",
                " Phone='" + phone.Trim() + "' and AuditState =2 and dr=0 ", "");
            if (userList == null || userList.Count == 0)
                return new ResultLogin() { Result = "F", Description = "该手机未绑定用户" };

            List<User> UserList = new List<User>();
            foreach (var user in userList)
            {
                Hi.Model.SYS_PhoneCode userphone = new Hi.BLL.SYS_PhoneCode().GetModel("手机登录", phone, code);
                if (userphone == null)
                {
                    return new ResultLogin() { Result = "F", Description = "手机验证码错误" };
                }

                List<Hi.Model.SYS_CompUser> compUserList = new Hi.BLL.SYS_CompUser().GetList("",
                    "UserID='" + user.ID + "' and IsNull(dr,0)=0 ", "");
                if (compUserList != null && compUserList.Count > 0)
                {
                    foreach (var compUser in compUserList)
                    {
                        User resultUser = new User();

                        resultUser.UserID = compUser.UserID;
                        resultUser.CompID = compUser.CompID;

                        resultUser.TrueName = user.TrueName;
                        resultUser.Sex = user.Sex;
                        resultUser.Phone = user.Phone;
                        resultUser.IsEnabled = user.IsEnabled;//列表无判断，前台要使用此状态

                        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(compUser.CompID);
                        if (comp == null)
                            return new ResultLogin() { Result = "F", Description = "核心企业异常" };
                        resultUser.Erptype = comp.Erptype;
                        resultUser.CompName = comp.CompName;

                        resultUser.UType = compUser.UType;
                        resultUser.CType = compUser.CType; // 1：核心企业  2：经销商
                        if (compUser.CType == 2)
                        {
                            resultUser.DisID = compUser.DisID;
                            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(compUser.DisID);
                            if (dis == null)
                                return new ResultLogin() { Result = "F", Description = "经销商异常" };

                            resultUser.DisName = dis.DisName;
                        }

                        UserList.Add(resultUser);
                    }
                }
            }
            return new ResultLogin()
            {
                Result = "T",
                Description = "登录成功",
                UserList = UserList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "LoginByPhone:" + JSon);
            return new ResultLogin() { Result = "F", Description = "参数异常" };
        }
    }

    public class ResultLogin
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public string OpenID { get; set; }
        public List<User> UserList { get; set; }
    }

    public class ResultWXUserinfo
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public User User { get; set; }
    }

    #endregion

    #region 返回

    public class ResultDisLog
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public List<ResultUser> UserList { get; set; }
    }

    public class ResultAddLogin
    {
        public string Result { get; set; }
        public string Description { get; set; }
    }

    public class ResultUser
    {
        public string UserID { get; set; }
        public string TrueName { get; set; }
        public string Sex { get; set; }
        public string Phone { get; set; }
        public string ResellerID { get; set; }
        public string ResellerName { get; set; }
        public string IsEnabled { get; set; }
        public string Erptype { get; set; }
        public string ConpamyID { get; set; }
        public string CompanyName { get; set; }
        public string UserType { get; set; }//0：经销商 1：核心企业
    }

    public class ResultCompLog
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public string UserID { get; set; }
        public string TrueName { get; set; }
        public string Sex { get; set; }
        public string Phone { get; set; }
        public string ConpamyID { get; set; }
        public string CompanyName { get; set; }
        public string IsEnabled { get; set; }
        public string Erptype { get; set; }
    }

    public class ResultUserID
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public string UserID { get; set; }
        public string BussID { get; set; }
        public string UserType { get; set; }
    }

    public class ResultWXLogin
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public string UserID { get; set; }
        public string TrueName { get; set; }
        public string Sex { get; set; }
        public string Phone { get; set; }
        public string BussID { get; set; }
        public string UserType { get; set; }
        public string Email { get; set; }
        public string ParentName { get; set; }
        public string CompName { get; set; }
    }

    public class ResultDisInfo
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public string DisName { get; set; }
        public string DisTypeName { get; set; }
        public string AreaName { get; set; }
        public string DisPrincipal { get; set; }
        public string DisPhone { get; set; }
        public string IsCheck { get; set; }
        public string CreditType { get; set; }
        public string UserName { get; set; }
        public string UserSex { get; set; }
        public string UserPhone { get; set; }
        public string PrePaymentMoney { get; set; }
    }

    #endregion
}
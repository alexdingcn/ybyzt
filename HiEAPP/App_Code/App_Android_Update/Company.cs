using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LitJson;
using DBUtility;
using System.Configuration;
using System.Data;

/// <summary>
///Company 的摘要说明
/// </summary>
public class Company
{
    public Company()
    {
    }
        public  class_ver3.CompanyInfo GetCompanyInfo(string JSon)
        {
            string UserID = string.Empty;
            string CompID = string.Empty;
            try
            {
                    #region JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count <= 0 || JInfo["UserID"].ToString() == "" || JInfo["CompID"].ToString() == "")
                return new class_ver3.CompanyInfo(){Result="F",Description = "参数异常"};
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            //判断登录信息是否正确, 
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(Int32.Parse(UserID), out one, Int32.Parse(CompID == "" ? "0" : CompID)))
            {
                //所有代理商可以查看厂家资料，所以不检查
                //return new class_ver3.CompanyInfo() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new class_ver3.CompanyInfo() { Result = "F", Description = "核心企业信息异常" };
            
#endregion
            //返回参数
            class_ver3.CompanyInfo ResultCompInfo = new class_ver3.CompanyInfo();
            ResultCompInfo.Result = "T";
            ResultCompInfo.Description = "返回成功";
            ResultCompInfo.CompName = comp.CompName;
            ResultCompInfo.SPhone = comp.Phone;
            //获取返回的核心企业信息
            class_ver3.CompInfo compinfo = new class_ver3.CompInfo();
            compinfo.CompName = comp.CompName;
            compinfo.InduName = comp.Trade;
            compinfo.Address = comp.Address;
            compinfo.Zip = comp.Zip;
            compinfo.Tel = comp.Tel;
            compinfo.Fax = comp.Fax;
            compinfo.Principal =comp.Principal;
            compinfo.Phone = comp.Phone;
            compinfo.ManageInfo = comp.ManageInfo;
            compinfo.Ts = comp.ts.ToString();
            ResultCompInfo.CompInfo = compinfo;
            //获取此用户的登录信息返回
            class_ver3.CompAccount compAccount = new class_ver3.CompAccount();
            if (one != null)
            {
                compAccount.UserName = one.UserName;
                compAccount.TrueName = one.TrueName;
                compAccount.Phone = one.Phone;
                compAccount.Email = one.Email;
                compAccount.Ts = one.ts.ToString();
            }

            ResultCompInfo.CompAccount = compAccount;
             //获取核心企业的系统配置
             class_ver3.SysSettings sysSettings = new class_ver3.SysSettings();
            int IsInve = Common.rdoOrderAudit("商品是否启用库存", comp.ID).ToInt(0);//判断此核心企业是否启用库存
            int IsReseller = Common.rdoOrderAudit("经销商加盟是否需要审核", comp.ID).ToInt(0);//判断此核心企业是否启用库存
            int IsRebate = Common.rdoOrderAudit("订单支付返利是否启用", comp.ID).ToInt(0);//判断此核心企业是否启用库存 
            int PayWay = Common.rdoOrderAudit("支付方式", comp.ID).ToInt(0);//判断此核心企业是否启用库存
                sysSettings.IsReseller = IsReseller ==0?"1":"0";
                sysSettings.PayWay = PayWay==0?"0":"1";
                sysSettings.IsRebate = IsRebate ==0?"0":"1";
                sysSettings.IsInv = IsInve ==0?"1":"0";
                ResultCompInfo.SysSettings = sysSettings;
                //获取核心企业的收款账号
                List<Hi.Model.PAY_PaymentBank> list_payBank = new Hi.BLL.PAY_PaymentBank().GetList("", "ISNULL(dr,0)=0 and CompID= " + CompID + "", "");
                List<class_ver3.PayAccount> list_payAccount = new List<class_ver3.PayAccount>();
                class_ver3.PayAccount payAccount = null;
                List<Hi.Model.PAY_BankInfo> bankInfo = null;
                Hi.BLL.PAY_BankInfo bll_bankinfo = new Hi.BLL.PAY_BankInfo();
                //循环取出的收款账号信息，拼接到返回信息
                foreach (Hi.Model.PAY_PaymentBank payBank in list_payBank)
                {
                    payAccount = new class_ver3.PayAccount();
                    payAccount.IsEnable = payBank.Isno ==-1?"0":"1";
                    payAccount.AccountName = payBank.AccountName;
                    payAccount.AccountCode = payBank.bankcode;
                    payAccount.BankAddress = payBank.bankAddress;
                    payAccount.AccountType = payBank.type ==11?"个人账户":"企业账户";
                    payAccount.BankPrivate = payBank.bankPrivate;
                    payAccount.BankCity = payBank.bankCity;
                    //判断证件类型是不是"x",是x表示其他证件，不是用枚举取出证件类型
                    if(payBank.vdef2=="x")
                        payAccount.CateType = "其他证件";
                    else
                        payAccount.CateType = Enum.GetName(typeof(CateType),int.Parse(payBank.vdef2));
                    payAccount.CateCode = payBank.vdef3;
                    payAccount.PayAccountID = payBank.ID.ToString();
                    payAccount.Ts = payBank.ts.ToString();
                    //获取银行名称,根据paymentbank中BankID在表bankinfo中用bankcode对应
                    bankInfo = bll_bankinfo.GetList("BankName","BankCode = '"+payBank.BankID+"'","");
                    payAccount.BankName = bankInfo[0].BankName;
                    list_payAccount.Add(payAccount);
                    break;
                }
                ResultCompInfo.PayAccountList = list_payAccount;
                return ResultCompInfo;

            }
         catch (Exception ex)
            {
                Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetCompanyInfo:" + JSon);
            return new class_ver3.CompanyInfo() { Result = "F", Description = "获取失败" };
         }

        }

        public EditResult EditCompanyAccount(string JSon)
        {
            string UserID = string.Empty;
            string CompID = string.Empty;
            try
            {
                #region JSon取值
                JsonData JInfo = JsonMapper.ToObject(JSon);
                if (JInfo.Count <= 0 || JInfo["UserID"].ToString() == "" || JInfo["CompID"].ToString() == "" || JInfo["CompAccount"].ToString() == "")
                    return new EditResult() { Result = "F", Description = "参数异常" };
                UserID = JInfo["UserID"].ToString();
                CompID = JInfo["CompID"].ToString();
                JsonData CompAccount = JInfo["CompAccount"];
                //判断登录信息是否正确
                Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
                if (!new Common().IsLegitUser(Int32.Parse(UserID), out one, Int32.Parse(CompID == "" ? "0" : CompID)))
                {
                    return new EditResult() { Result = "F", Description = "登录信息异常" };
                }
                //判断核心企业是否异常
                Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
                if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                    return new EditResult() { Result = "F", Description = "核心企业信息异常" };

                #endregion
                //判断时间戳是否正确
                if (one.ts.ToString() != CompAccount["Ts"].ToString())
                    return new EditResult() { Result = "F", Description = "登录信息已被他人修改，请重新再试" };
                one.TrueName = CompAccount["TrueName"].ToString();
                one.Email = CompAccount["Email"].ToString();
                one.ts = DateTime.Now;
                one.modifyuser = one.ID;
                if(new Hi.BLL.SYS_Users().Update(one))
                    return new EditResult() { Result = "T", Description = "修改成功" };
                return new EditResult() { Result = "F", Description = "修改失败" };
            }
            catch (Exception ex)
            {
                Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "EditCompanyAccount:" + JSon);
                return new EditResult() { Result = "F", Description = "修改失败" };
            }
        }


          public EditResult EditCompanySysSettings(string JSon)
          {
              string UserID = string.Empty;
              string CompID = string.Empty;
              try
              {
                  #region JSon取值
                  JsonData JInfo = JsonMapper.ToObject(JSon);
                  if (JInfo.Count <= 0 || JInfo["UserID"].ToString() == "" || JInfo["CompID"].ToString() == "" || JInfo["SysSettings"].ToString() == "")
                      return new EditResult() { Result = "F", Description = "参数异常" };
                  UserID = JInfo["UserID"].ToString();
                  CompID = JInfo["CompID"].ToString();
                  JsonData sysSettings = JInfo["SysSettings"];
                  //判断登录信息是否正确
                  Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
                  if (!new Common().IsLegitUser(Int32.Parse(UserID), out one, Int32.Parse(CompID == "" ? "0" : CompID)))
                  {
                      return new EditResult() { Result = "F", Description = "登录信息异常" };
                  }
                  //判断核心企业是否异常
                  Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
                  if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                      return new EditResult() { Result = "F", Description = "核心企业信息异常" };

                  #endregion

                  //分别获取库存，经销商是否审核，返利，支付方式的配置信息
                  Hi.BLL.SYS_SysName bll_sysName = new Hi.BLL.SYS_SysName();
                  List<Hi.Model.SYS_SysName> list_sysName = bll_sysName.GetList("", "ISNULL(dr,0)=0 and CompID =" + comp.ID + " and Name ='商品是否启用库存'", "");
                  if (list_sysName == null || list_sysName.Count <= 0)
                      return new EditResult() { Result = "F", Description = "此核心企业不存在启用商品库存的系统设置" };
                  Hi.Model.SYS_SysName sysName = list_sysName[0];
                  sysName.ts = DateTime.Now;
                  sysName.modifyuser = one.ID;
                  if (sysSettings["IsInv"].ToString() == "0")
                      sysName.Value = "1";
                  else
                      sysName.Value = "0";
                  if (!bll_sysName.Update(sysName))
                      return new EditResult() { Result ="F",Description = "修改失败"};
                  //更新经销商是否需要审核的配置信息
                  list_sysName = bll_sysName.GetList("", "ISNULL(dr,0)=0 and CompID ="+comp.ID+" and Name ='经销商加盟是否需要审核'", "");
                  if (list_sysName == null || list_sysName.Count <= 0)
                      return new EditResult() { Result = "F", Description = "此核心企业不存在启用经销商加盟审核的系统设置" };
                  sysName = list_sysName[0];
                  sysName.ts = DateTime.Now;
                  sysName.modifyuser = one.ID;
                  if (sysSettings["IsReseller"].ToString() == "0")
                      sysName.Value = "1";
                  else
                      sysName.Value = "0";
                  if (!bll_sysName.Update(sysName))
                      return new EditResult() { Result = "F", Description = "修改失败" };
                  //更新是否启用返利的配置信息
                  list_sysName = bll_sysName.GetList("", "ISNULL(dr,0)=0 and CompID =" + comp.ID + " and Name ='订单支付返利是否启用'", "");
                  if (list_sysName == null || list_sysName.Count <= 0)
                      return new EditResult() { Result = "F", Description = "此核心企业不存在启用订单返利的系统设置" };
                  sysName = list_sysName[0];
                  sysName.ts = DateTime.Now;
                  sysName.modifyuser = one.ID;
                  if (sysSettings["IsRebate"].ToString() == "0")
                      sysName.Value = "0";
                  else
                      sysName.Value = "1";
                  if (!bll_sysName.Update(sysName))
                      return new EditResult() { Result = "F", Description = "修改失败" };
                  //更新支付方式的配置信息
                  list_sysName = bll_sysName.GetList("", "ISNULL(dr,0)=0 and CompID =" + comp.ID + " and Name ='支付方式'", "");
                  if (list_sysName == null || list_sysName.Count <= 0)
                      return new EditResult() { Result = "F", Description = "此核心企业不存在支付方式的系统设置" };
                  sysName = list_sysName[0];
                  sysName.ts = DateTime.Now;
                  sysName.modifyuser = one.ID;
                  if (sysSettings["PayWay"].ToString() == "0")
                      sysName.Value = "0";
                  else
                      sysName.Value = "1";
                  if (!bll_sysName.Update(sysName))
                      return new EditResult() { Result = "F", Description = "修改失败" };
                  return new EditResult() { Result = "T", Description = "修改成功" };

              }
              catch(Exception ex)
              {
                  Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "EditCompanyInfo:" + JSon);
                  return new EditResult() { Result = "F", Description = "修改失败" };
              }
          }

          public EditResult EditCompanySysSettings_delete(string JSon)
          {
              string UserID = string.Empty;
              string CompID = string.Empty;
              string type = string.Empty;
              string value = string.Empty;
              try
              {
                  #region JSon取值
                  JsonData JInfo = JsonMapper.ToObject(JSon);
                  if (JInfo.Count <= 0 || JInfo["UserID"].ToString() == "" || JInfo["CompID"].ToString() == "" || JInfo["Type"].ToString() == "" ||
                      JInfo["Value"].ToString() == "")
                      return new EditResult() { Result = "F", Description = "参数异常" };
                  UserID = JInfo["UserID"].ToString();
                  CompID = JInfo["CompID"].ToString();
                  type = JInfo["Type"].ToString();
                  value = JInfo["Value"].ToString();
                  //判断登录信息是否正确
                  Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
                  if (!new Common().IsLegitUser(Int32.Parse(UserID), out one, Int32.Parse(CompID == "" ? "0" : CompID)))
                  {
                      return new EditResult() { Result = "F", Description = "登录信息异常" };
                  }
                  //判断核心企业是否异常
                  Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
                  if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                      return new EditResult() { Result = "F", Description = "核心企业信息异常" };

                  #endregion

                  List<Hi.Model.SYS_SysName> list_sysName = null;
                  Hi.Model.SYS_SysName sysName = null;
                  Hi.BLL.SYS_SysName bll_sysName = new Hi.BLL.SYS_SysName();
                  switch (type)
                  {
                      case "库存":
                          list_sysName = bll_sysName.GetList("", "ISNULL(dr,0)=0 and Name ='商品是否启用库存'", "");
                          if (list_sysName == null || list_sysName.Count <= 0)
                              return new EditResult() { Result = "F", Description = "此核心企业不存在启用商品库存的系统设置" };
                          sysName = list_sysName[0];
                          //根据value值更新数据库value值
                          if (value == "0")
                              sysName.Value = "1";
                          else
                              sysName.Value = "0";
                          break;
                      case "经销商加盟审核":
                          list_sysName = bll_sysName.GetList("", "ISNULL(dr,0)=0 and Name ='经销商加盟是否需要审核'", "");
                          if (list_sysName == null || list_sysName.Count <= 0)
                              return new EditResult() { Result = "F", Description = "此核心企业不存在启用经销商加盟审核的系统设置" };
                          sysName = list_sysName[0];
                          //根据value值更新数据库value值
                          if (value == "0")
                              sysName.Value = "1";
                          else
                              sysName.Value = "0";
                          break;
                      case "返利":
                          list_sysName = bll_sysName.GetList("", "ISNULL(dr,0)=0 and Name ='订单支付返利是否启用'", "");
                          if (list_sysName == null || list_sysName.Count <= 0)
                              return new EditResult() { Result = "F", Description = "此核心企业不存在启用订单返利的系统设置" };
                          sysName = list_sysName[0];
                          //根据value值更新数据库value值
                          if (value == "0")
                              sysName.Value = "0";
                          else
                              sysName.Value = "1";
                          break;
                      case "支付方式":
                          list_sysName = bll_sysName.GetList("", "ISNULL(dr,0)=0 and Name ='支付方式'", "");
                          if (list_sysName == null || list_sysName.Count <= 0)
                              return new EditResult() { Result = "F", Description = "此核心企业不存在支付方式的系统设置" };
                          sysName = list_sysName[0];
                          //根据value值更新数据库value值
                          if (value == "0")
                              sysName.Value = "0";
                          else
                              sysName.Value = "1";
                          break;
                      default:
                          return new EditResult() { Result = "F", Description = "类型错误" };
                          break;

                  }
                  sysName.ts = DateTime.Now;
                  sysName.modifyuser = one.ID;
                  //更新字段
                  if (bll_sysName.Update(sysName))
                      return new EditResult() { Result = "T", Description = "修改成功" };
                  return new EditResult() { Result = "F", Description = "修改失败" };
              }
              catch (Exception ex)
              {
                  Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "EditCompanySysSettings:" + JSon);
                  return new EditResult() { Result = "F", Description = "修改失败" };
              }
          }


    public EditResult EditCompanyInfo(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        try
        {
            #region JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count <= 0 || JInfo["UserID"].ToString() == "" || JInfo["CompID"].ToString() == "" || JInfo["CompInfo"].ToString() == "")
                return new EditResult() { Result = "F", Description = "参数异常" };
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            JsonData compInfo = JInfo["CompInfo"];
            //判断登录信息是否正确
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(Int32.Parse(UserID), out one, Int32.Parse(CompID == "" ? "0" : CompID)))
            {
                return new EditResult() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new EditResult() { Result = "F", Description = "核心企业信息异常" };

            #endregion
            if (comp.ts.ToString() != compInfo["Ts"].ToString())
                return new EditResult() { Result = "F", Description = "核心企业信息已被他人修改，请重新再试" };
            comp.ts = DateTime.Now;
            comp.modifyuser = one.ID;
            comp.Address = compInfo["Address"].ToString();
            comp.Zip = compInfo["Zip"].ToString();
            comp.Tel = compInfo["Tel"].ToString();
            comp.Fax = compInfo["Fax"].ToString();
            comp.Principal = compInfo["Principal"].ToString();
            comp.Phone = compInfo["Phone"].ToString();
            //进行更新
            if (new Hi.BLL.BD_Company().Update(comp))
                return new EditResult() { Result = "T", Description = "修改成功" };
            return new EditResult() { Result = "F", Description = "修改失败" };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "EditCompanyInfo:" + JSon);
            return new EditResult() { Result = "F", Description = "修改失败" };
        }
  
    }

    public EditResult EditCompanyLoginPassword(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string oldPassword = string.Empty;
        string newPassword = string.Empty;
        try
        {
            #region JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count <= 0 || JInfo["UserID"].ToString() == "" || JInfo["CompID"].ToString() == "" || JInfo["OldPassword"].ToString() == ""||
                JInfo["NewPassword"].ToString() == "")
                return new EditResult() { Result = "F", Description = "参数异常" };
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            oldPassword = JInfo["OldPassword"].ToString();
            newPassword = JInfo["NewPassword"].ToString();
            //判断登录信息是否正确
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(Int32.Parse(UserID), out one, Int32.Parse(CompID == "" ? "0" : CompID)))
            {
                return new EditResult() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new EditResult() { Result = "F", Description = "核心企业信息异常" };

            #endregion
            //判断旧密码是否正确
            if(one.UserPwd!=new GetPhoneCode().md5(oldPassword))
                return new EditResult() { Result = "F", Description = "原密码错误" };
            //更新
           if( new Hi.BLL.SYS_Users().UpdatePassWord(new GetPhoneCode().md5(newPassword),UserID))
               return new EditResult() { Result = "T", Description = "修改成功" };
           return new EditResult() { Result = "F", Description = "修改失败" };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "EditCompanyLoginPassword:" + JSon);
            return new EditResult() { Result = "F", Description = "修改失败" };
        }
    }


    public EditResult EditCompanyPayAccount(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string PayAccountID = string.Empty;
        string value = string.Empty;
        try
        {
            #region JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count <= 0 || JInfo["UserID"].ToString() == "" || JInfo["CompID"].ToString() == "" || JInfo["PayAccountID"].ToString() == "" ||
                JInfo["Value"].ToString() == "")
                return new EditResult() { Result = "F", Description = "参数异常" };
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            PayAccountID = JInfo["PayAccountID"].ToString();
            value = JInfo["Value"].ToString();
            //判断登录信息是否正确
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(Int32.Parse(UserID), out one, Int32.Parse(CompID == "" ? "0" : CompID)))
            {
                return new EditResult() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new EditResult() { Result = "F", Description = "核心企业信息异常" };

            #endregion
            //根据收款账号ID获取收款账号实体
            Hi.Model.PAY_PaymentBank payBank = new Hi.BLL.PAY_PaymentBank().GetModel(Int32.Parse(PayAccountID));
            if (payBank == null || payBank.dr == 1)
                return new EditResult() { Result = "F", Description = "收款账号错误" };
            #region 判断是否以有开启的收款账号
            //获取核心企业的除了此收款账号的其他收款账号
            if (value != "0")
            {
                List<Hi.Model.PAY_PaymentBank> list_payBank = new Hi.BLL.PAY_PaymentBank().GetList("", "ISNULL(dr,0)=0 and CompID= " + CompID + " and ID <> '" + PayAccountID + "'", "");
                foreach (Hi.Model.PAY_PaymentBank paybank_other in list_payBank)
                {
                    if (paybank_other.Isno == 1)
                        return new EditResult() { Result = "F", Description = "已有开启的收款账号，请先将其关闭" };
                }
            }
            #endregion
            //根据value值开启或关闭收款账号，value为0表示停用，即pay_paymentBank表中Isno字段值为-1，value值为1表示启用，pay_paymentBank表中Isno字段值为1
            if (value == "0")
                payBank.Isno = -1;
            else
                payBank.Isno = 1;
            payBank.ts = DateTime.Now;
            payBank.modifyuser = one.ID;
            //更新
            if (new Hi.BLL.PAY_PaymentBank().Update(payBank))
                return new EditResult() { Result = "T", Description = "修改成功" };
            return new EditResult() { Result = "F", Description = "修改失败" };
            
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "EditCompanyPayAccount:" + JSon);
            return new EditResult() { Result = "F", Description = "修改失败" };
        }
    }

    //微信首页获取店铺图片
    public ResultCompanyImg WXGetCompanyImg(string JSon, string version)
    {
        try
        {
            string CompID = string.Empty;
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count <= 0 || JInfo["CompID"].ToString() == "")
                return new ResultCompanyImg() { Result = "F", Description = "参数异常" };
            CompID = JInfo["CompID"].ToString();
            //判断核心企业是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultCompanyImg() { Result = "F", Description = "核心企业信息异常" };
            List<string> list_url = new List<string>();
            if (comp.FirstBanerImg == null || comp.FirstBanerImg == "")
            {
                list_url.Add("");
                return new ResultCompanyImg() { Result = "T", Description="获取成功",ImgUrlList = list_url };
            }
            string[] PathArry = comp.FirstBanerImg.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string path in PathArry)
            {
                string url = (ConfigurationManager.AppSettings["ImgViewPath"] + "CompImage/") + "/" + path;
                list_url.Add(url);
            }
            return new ResultCompanyImg() { Result = "T", Description = "获取成功", ImgUrlList = list_url, CompName = comp.CompName };


        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "WXGetCompanyImg:" + JSon);
            return new ResultCompanyImg() { Result = "F", Description = "参数异常" };
        }
    }


    //微信获取商品详细信息
    public ResultWXProduct WXGetProductInfo(string JSon,string version)
    {
        try
        {
            string CompID = string.Empty;
            string GoodsID = string.Empty;
            string strsql = string.Empty;
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if(JInfo.Count > 0&& JInfo["GoodsID"].ToString() != ""&&JInfo["CompID"].ToString() != "")
            {
                CompID = JInfo["CompID"].ToString();
                GoodsID = JInfo["GoodsID"].ToString();
            }
            else
                return new ResultWXProduct() { Result = "F", Description = "参数异常" };
            //判断核心企业是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultWXProduct() { Result = "F", Description = "核心企业信息异常" };
            Hi.Model.BD_Goods good = new Hi.BLL.BD_Goods().GetModel(Int32.Parse(GoodsID));
            if(good.dr==1||good.IsEnabled !=1||good.CompID != comp.ID)
                return new ResultWXProduct() { Result = "F", Description = "商品信息异常" };
            Product product = new Product();
            product.ProductID = good.ID.ToString();
            product.ProductName = good.GoodsName;
            product.Title = good.Title;
            product.Details = good.Details;
            product.Unit = good.Unit;
            product.InStock = ClsSystem.gnvl(good.IsOffline,"0");

            List<BD_GoodsCategory.Pic> Pic = new List<BD_GoodsCategory.Pic>();

                #region List<Pic> Pic

                if (good.Pic.ToString() != "" && good.Pic.ToString() != "X")
                {
                    BD_GoodsCategory.Pic pic = new BD_GoodsCategory.Pic();
                    pic.ProductID = good.ID.ToString();
                    pic.IsDeafult = "1";
                    pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                 good.Pic;
                    Pic.Add(pic);

                }
                //Pic.AddRange(GetPicList(row["ID"].ToString()));
                BD_Goods bd_goods = new BD_Goods();
                Pic.AddRange(bd_goods.GetPicList(good.ID.ToString()));
                //图片二
                //if (row["Pic2"].ToString() != "" && row["Pic2"].ToString() != "X")
                //{
                //    Pic pic = new Pic();
                //    pic.ProductID = row["ID"].ToString();
                //    pic.IsDeafult = "0";
                //    pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                //                 row["Pic2"];
                //    Pic.Add(pic);
                //}
                ////图片三
                //if (row["Pic3"].ToString() != "" && row["Pic3"].ToString() != "X")
                //{
                //    Pic pic = new Pic();
                //    pic.ProductID = row["ID"].ToString();
                //    pic.IsDeafult = "0";
                //    pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                //                 row["Pic3"];
                //    Pic.Add(pic);
                //}

                #endregion

            product.ProductPicUrlList = Pic;
            List<ProductAttribute> ProductAttributeList = new List<ProductAttribute>();

                #region 通过商品类别ID和属性ID关联表，找到属性ID

                //List<Hi.Model.BD_CategoryAttribute> val = new Hi.BLL.BD_CategoryAttribute().GetList("",
                //    " ID in (" + strgoodsAttr + ") and CompID='" + dis.CompID + "' and ISNULL(dr,0)=0", "");
                //if (val == null)
                //    return new ResultProductList() { Result = "F", Description = "未找到商品属性" };

                //foreach (Hi.Model.BD_CategoryAttribute goodsAttr in val)
                //{
                //    ProductAttribute proAttr = new ProductAttribute();

                //    proAttr.ProductID = row["ID"].ToString();
                //    proAttr.ProductAttributeID = goodsAttr.AttributeID.ToString(); //属性ID
                //    Hi.Model.BD_Attribute attr = new Hi.BLL.BD_Attribute().GetModel(goodsAttr.AttributeID);
                //    proAttr.ProductAttributeName = attr.AttributeName; //属性名称

                //    List<ProductAttValue> ProductAttValueList = new List<ProductAttValue>();
                //    List<Hi.Model.BD_AttributeValues> attrList = new Hi.BLL.BD_AttributeValues().GetList("",
                //        " AttributeID='" + goodsAttr.ID + "' and CompID='" + dis.CompID + "' and ISNULL(dr,0)=0" +
                //        " and ID in (" + strID + ")", "ID"); //todo:商品属性表修改咨询商品结构
                //    if (attrList == null)
                //        return new ResultProductList() { Result = "F", Description = "未找到商品属性名字" };
                //    foreach (Hi.Model.BD_AttributeValues attribute in attrList)
                //    {
                //        ProductAttValue productAttValue = new ProductAttValue();
                //        productAttValue.ProductID = row["ID"].ToString().ToString();
                //        productAttValue.ProductAttributeID = goodsAttr.AttributeID.ToString();
                //        productAttValue.ProductAttValueID = attribute.ID.ToString();
                //        productAttValue.ProductAttValueName = attribute.AttrValue;

                //        ProductAttValueList.Add(productAttValue);
                //    }
                //    proAttr.ProductAttValueList = ProductAttValueList;
                //    ProductAttributeList.Add(proAttr);
                //}
                //ProductAttribute productattr = new ProductAttribute();
                strsql = "select ID,AttrsName from BD_GoodsAttrs where goodsid = " + good.ID.ToString() + " and isnull(dr,0) =0 and compid = " + comp.ID + "";
                DataTable dt_attr = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
                for (int i = 0; i < dt_attr.Rows.Count; i++)
                {
                    ProductAttribute productattr = new ProductAttribute();
                    productattr.ProductID = good.ID.ToString();
                    productattr.ProductAttributeID = ClsSystem.gnvl(dt_attr.Rows[i][0], "");
                    productattr.ProductAttributeName = ClsSystem.gnvl(dt_attr.Rows[i][1], "");
                    List<ProductAttValue> ProductAttValueList = new List<ProductAttValue>();

                    strsql = "select ID,AttrsInfoName from BD_GoodsAttrsInfo where attrsid = " + dt_attr.Rows[i][0] + " and goodsid = " + good.ID.ToString() + " ";
                    strsql += " and isnull(dr,0) = 0 and compid = " + comp.ID + "";
                    DataTable dt_attrinfo = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);

                    for (int j = 0; j < dt_attrinfo.Rows.Count; j++)
                    {
                        ProductAttValue productattvalue = new ProductAttValue();
                        productattvalue.ProductID = good.ID.ToString();
                        productattvalue.ProductAttributeID = ClsSystem.gnvl(dt_attr.Rows[i][0], "");
                        productattvalue.ProductAttValueID = ClsSystem.gnvl(dt_attrinfo.Rows[j][0], "");
                        productattvalue.ProductAttValueName = ClsSystem.gnvl(dt_attrinfo.Rows[j][1], "");
                        ProductAttValueList.Add(productattvalue);

                    }
                    productattr.ProductAttValueList = ProductAttValueList;
                    ProductAttributeList.Add(productattr);
                }

                #endregion

            product.ProductAttributeList = ProductAttributeList;

              //获取商品标签
                List<Hi.Model.BD_GoodsLabels> list_goodslables = new Hi.BLL.BD_GoodsLabels().GetList("LabelName", "GoodsID=" +good.ID + " and isnull(dr,0)=0", "");
            List<GoodsSpan> list_goodsspan = new List<GoodsSpan>();
            foreach (Hi.Model.BD_GoodsLabels span in list_goodslables)
            {
               GoodsSpan goodsspan = new GoodsSpan();
               goodsspan.GoodsSpanValue = span.LabelName;
               list_goodsspan.Add(goodsspan);
            }
            product.GoodsSpanList = list_goodsspan;
            string url = ConfigurationManager.AppSettings["url"].ToString();
            url = url + good.ID.ToString();
            product.url = url;
            List<Hi.Model.BD_GoodsInfo> list_goodsinfo =new Hi.BLL.BD_GoodsInfo().GetList("BarCode","GoodsID = "+good.ID+" and dr=0 and isnull(isenabled,0)=1","ID");
            if(list_goodsinfo ==null||list_goodsinfo.Count<=0)
                product.BarCode="";
            product.BarCode = list_goodsinfo[0].BarCode;

            return new ResultWXProduct(){Result= "T",Description = "获取成功",Product =product};


        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "WXGetProductInfo:" + JSon);
            return new ResultWXProduct() { Result = "F", Description = "参数异常" };
        }
    }

    //微信获取商品列表
    public ResultWXProductList WXGetProductList(string JSon, string version)
    {
        try
        {
            string CompID = string.Empty;
            string strsql = string.Empty;
            string CriticalProductID = string.Empty;
            string gettype = string.Empty;
            string RowNum = string.Empty;
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count<=0|| JInfo["CompID"].ToString() == ""  || JInfo["CriticalProductID"].ToString() == "" ||
                JInfo["GetType"].ToString() == "" || JInfo["RowNum"].ToString() == "")
                return new ResultWXProductList() { Result = "F", Description = "参数异常" };
            CompID = JInfo["CompID"].ToString();
            CriticalProductID = JInfo["CriticalProductID"].ToString();
            gettype = JInfo["GetType"].ToString();
            RowNum = ClsSystem.gnvl( JInfo["RowNum"],"0");
            //判断经销商对应的核心企业是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultWXProductList() { Result = "F", Description = "核心企业信息异常" };
    //        string strWhere = "where  ISNULL(IsOffline,0) =1 and ISNULL(isenabled,0) =1 and dr=0 and compid = "+comp.ID+" ";
    //        string strsql = new Common().PageSqlString(CriticalProductID, "ID", "BD_Goods", "ID",
    //"1", strWhere, gettype, RowNum);
            if (CriticalProductID != "-1")
            {
                strsql = "SELECT rowNum FROM (select row_number() over (order by ID  asc) as rowNum,* from BD_Goods where  ISNULL(IsOffline,0) =1 and ISNULL(isenabled,0) =1 and dr=0 and compid = " + comp.ID + ")x where ID = " + CriticalProductID + "";
                DataTable rownum = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];
                if (rownum == null || rownum.Rows.Count <= 0)
                    return new ResultWXProductList() { Result = "F", Description = "参数异常" };
                int num = int.Parse(ClsSystem.gnvl(rownum.Rows[0]["rowNum"], "0"));

                if (gettype == "1") //加载更多:新数据
                {
                    strsql = "SELECT * FROM (select row_number() over (order by ID asc";
                    strsql += ") as rowNum,* from BD_Goods where   ISNULL(IsOffline,0) =1 and ISNULL(isenabled,0) =1 and dr=0 and compid = " + comp.ID + ")x ";
                    strsql += " where rowNum between " + (num + 1) + " and " + (num + int.Parse(RowNum)) + "";
                }
                else //加载老数据
                {
                    strsql = "SELECT * FROM (select row_number() over (order by ID asc";
                    strsql += ") as rowNum,* from BD_Goods where   ISNULL(IsOffline,0) =1 and ISNULL(isenabled,0) =1 and dr=0 and compid = " + comp.ID + ")x ";
                    strsql += "where rowNum between " + (num - int.Parse(RowNum)) + " and " + (num - 1) + "";
                }
            }
            else
            {
                strsql = "SELECT * FROM (select row_number() over (order by ID asc";
                strsql += ") as rowNum,* from BD_Goods where   ISNULL(IsOffline,0) =1 and ISNULL(isenabled,0) =1 and dr=0 and compid = " + comp.ID + ")x ";
                strsql += "where rowNum between 1 and " + int.Parse(RowNum) + "";
            }
            //执行sql，取出满足条件的数据
            DataTable dsList = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql).Tables[0];
            if(dsList ==null||dsList.Rows.Count <= 0)
                return new ResultWXProductList() { Result = "T", Description = "没有更多数据"};
            List<WXProductSimple> ProductList = new List<WXProductSimple>();
            foreach (DataRow row in dsList.Rows)
            {
                WXProductSimple product = new WXProductSimple();
                product.ProductID = row["ID"].ToString(); //商品ID
                product.ProductName = row["GoodsName"].ToString();
                List<class_ver3.Pic> Pic = new List<class_ver3.Pic>();
                #region //获取图片

                if (row["Pic"].ToString() != "" && row["Pic"].ToString() != "X")
                {
                    class_ver3.Pic pic = new class_ver3.Pic();
                    pic.ProductID = row["ID"].ToString();
                    pic.IsDeafult = "1";
                    pic.PicUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "GoodsImg/" +
                                 row["Pic"].ToString();
                    Pic.Add(pic);
                }
                BD_Goods_ver3 bd_goods_ver3 = new BD_Goods_ver3();
                Pic.AddRange(bd_goods_ver3.GetPicList(row["ID"].ToString()));

                #endregion

                product.ProductPicUrlList = Pic;

                ProductList.Add(product);
            }
            return new ResultWXProductList() { Result = "T", Description = "获取成功", ProductList = ProductList };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "WXGetProductList:" + JSon);
            return new ResultWXProductList() { Result = "F", Description = "参数异常" };
        }
    }
    //微信获取店铺信息
    public ResultWXCompInfo WXGetCompInfo(string JSon, string version)
    {
        try
        {
            string CompID = string.Empty;
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count <= 0 || JInfo["CompID"].ToString() == "")
                return new ResultWXCompInfo() { Result = "F", Description = "参数异常" };
            CompID = JInfo["CompID"].ToString();
            //判断经销商对应的核心企业是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultWXCompInfo() { Result = "F", Description = "核心企业信息异常" };
            ResultWXCompInfo result = new ResultWXCompInfo();
            result.Result = "T";
            result.Description = "获取成功";
            result.Principal = ClsSystem.gnvl(comp.Principal,"");
            result.Phone = ClsSystem.gnvl(comp.Phone, "");
            result.Address = ClsSystem.gnvl(comp.Address, "");
            result.CompInfo = ClsSystem.gnvl(comp.CompInfo, "");
            return result;

        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "WXGetCompInfo:" + JSon);
            return new ResultWXCompInfo() { Result = "F", Description = "参数异常" };
        }
    }
    //证件类型枚举
    public enum CateType
    {
        身份证 = 0,
        户口簿= 1,
        护照=2,
        军官证=3,
        士兵证=4,
        港澳居民来往内地通行证=5,
        台湾同胞来往内地通行证=6,
        临时身份证 = 7,
        外国人居留证 =8,
        警官证 =9
    }

    public class EditResult
    {
        public string Result { get; set; }
        public string Description { get; set; }
    }


    public class ResultCompanyImg
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public string CompName { get; set; }
        public List<string> ImgUrlList { get; set; }
    }

    public class ResultWXProduct
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public Product Product{get;set;}
    }

    public class Product
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string Title { get; set; }
        public String Details { get; set; }
        public string Unit { get; set; }
        public string InStock { get; set; }
        public string BarCode {get;set;}
        public List<BD_GoodsCategory.Pic> ProductPicUrlList { get; set; }
        public List<ProductAttribute> ProductAttributeList { get; set; }
        public List<GoodsSpan> GoodsSpanList { get; set; }//商品标签
        public String url { get; set; }//商品介绍url

    }

      public class GoodsSpan
    {
        public String GoodsSpanValue { get; set; }
    }

 
    public class ProductAttribute
    {
        public string ProductID { get; set; }
        public string ProductAttributeID { get; set; }
        public string ProductAttributeName { get; set; }
        public List<ProductAttValue> ProductAttValueList { get; set; }
    }

    public class ProductAttValue
    {
        public string ProductID { get; set; }
        public string ProductAttributeID { get; set; }
        public string ProductAttValueID { get; set; }
        public string ProductAttValueName { get; set; }
    }

    public class ResultWXProductList
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public List<WXProductSimple> ProductList { get; set; }

    }

    public class WXProductSimple
    {
        public List<class_ver3.Pic> ProductPicUrlList { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string BarCode { get; set; }
    }

    public class ResultWXCompInfo
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public string CompInfo { get; set; }
        public string Principal { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
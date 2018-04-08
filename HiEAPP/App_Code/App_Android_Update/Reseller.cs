using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.UI.WebControls;
using DBUtility;
using Hi.Model;
using LitJson;
using System.Web.Script.Serialization;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;


/// <summary>
///Reseller 的摘要说明
/// </summary>
public class Reseller
{
	public Reseller()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 获取经销商分类列表
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultResellerClassify GetResellerClassifyList(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        try
        {

        #region//JSon取值
        JsonData JInfo = JsonMapper.ToObject(JSon);
        if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "")
            return new ResultResellerClassify() { Result="F",Description = "参数异常"};
        UserID = JInfo["UserID"].ToString();
        CompID = JInfo["CompID"].ToString();
        //判断登录信息是否正确
        Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
        if (!new Common().IsLegitUser(int.Parse(UserID), out one, Int32.Parse(CompID)))
        {
            return new ResultResellerClassify() { Result = "F", Description = "登录信息异常" };
        }
        //判断核心企业信息是否异常
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
        if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
            return new ResultResellerClassify() { Result = "F", Description = "核心企业信息异常" };
        #endregion
        //获取对应核心企业的经销商分类列表
        List<Hi.Model.BD_DisType> list_distype = new Hi.BLL.BD_DisType().GetList("","CompID = "+CompID+" and isnull(dr,0)=0 ","SortIndex");
        if (list_distype != null && list_distype.Count > 0)
        {
            //循环经销商分类列表，将值赋给返回参数
            List<class_ver3.ResellerClassify> list_resellerclassify = new List<class_ver3.ResellerClassify>();
            class_ver3.ResellerClassify resellerclassify = null;
            foreach (Hi.Model.BD_DisType distype in list_distype)
            {
                resellerclassify = new class_ver3.ResellerClassify();
                resellerclassify.ClassifyID = distype.ID.ToString();
                resellerclassify.ClassifyName = distype.TypeName;
                resellerclassify.ClassifyCode =ClsSystem.gnvl(distype.TypeCode,"");
                resellerclassify.ParentID = distype.ParentId.ToString();
                resellerclassify.SortIndex = distype.SortIndex.ToString();
                resellerclassify.Remark = ClsSystem.gnvl(distype.Remark,"");
                list_resellerclassify.Add(resellerclassify);
            }
            return new ResultResellerClassify() { Result = "T", Description = "获取成功", ResellerClassifyList = list_resellerclassify };
        }
        return new ResultResellerClassify(){Result = "T",Description = "获取成功"};
        }
        catch(Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerClassifyList:" + JSon);
            return new ResultResellerClassify() { Result = "F", Description = "获取失败" };
        }
    }


    /// <summary>
    /// 获取经销商详情
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultResellerDetail GetResellerDetail(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string DisID = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "" || JInfo["ResellerID"].ToString().Trim() == "")
                return new ResultResellerDetail() { Result = "F", Description = "参数异常" };
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            DisID = JInfo["ResellerID"].ToString();
            //判断登录信息是否异常
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, Int32.Parse(CompID)))
            {
                return new ResultResellerDetail() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业信息是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultResellerDetail() { Result = "F", Description = "核心企业信息异常" };
            //判断经销商信息是否异常
            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(Int32.Parse(DisID));
            if(dis==null)
                return new ResultResellerDetail() { Result = "F", Description = "经销商信息异常" };
            if(dis.dr ==1)
                return new ResultResellerDetail() { Result = "F", Description = "经销商已被删除" };
            if(dis.AuditState ==0)
                return new ResultResellerDetail() { Result = "F", Description = "经销商未审核" };
            #endregion
            //获取经销商的返回信息
            class_ver3.Reseller res = new class_ver3.Reseller();
            res.ResellerID = dis.ID.ToString();
            res.ResellerName = dis.DisName;
            //拼接经销商地址
            string area = string.Empty;
            area +=dis.Province;
            if (dis.City != "市辖区")
                area += dis.City;
            area += dis.Area;
            area += dis.Address;
            res.ResellerAddr = area;
            res.ResellerProvince = dis.Province;
            res.ResellerCity = dis.City;
            res.ResellerArea = dis.Area;
            res.Address = dis.Address;
            res.ResellerCode = dis.DisCode;
            res.ResellerClassifyID = dis.DisTypeID.ToString();
            //根据经销商分类ID获取分类名称
            Hi.Model.BD_DisType dis_type = new Hi.BLL.BD_DisType().GetModel(dis.DisTypeID);
            if (dis_type != null)
                res.ResellerClassify = dis_type.TypeName;
            else
                res.ResellerClassify = "";
            res.Zip = dis.Zip;
            res.Tel = dis.Tel;
            res.Fax = dis.Fax;
            res.Principal = dis.Principal;
            res.Phone = dis.Phone;
            res.AreaID = dis.AreaID.ToString();
            //根据经销商区域ID获取区域名称
            Hi.Model.BD_DisArea dis_area = new Hi.BLL.BD_DisArea().GetModel(dis.AreaID);
            if (dis_area != null)
                res.AreaName = dis_area.AreaName;
            else
                res.AreaName = "";
            res.IsEnabled = dis.IsEnabled.ToString();
            res.ts = dis.ts.ToString();
            #region //获取此经销商的开票信息
            List<Hi.Model.BD_DisAccount> list_disinvoce = new Hi.BLL.BD_DisAccount().GetList("","DisID="+dis.ID+" and isnull(dr,0)=0","CreateDate desc");
            List<class_ver3.Invoce> list_invoce = new List<class_ver3.Invoce>();
            if (list_disinvoce != null && list_disinvoce.Count > 0)
            {
                //循环赋值开票信息的返回信息
                foreach (Hi.Model.BD_DisAccount dis_invoce in list_disinvoce)
                {
                    class_ver3.Invoce invoce = new class_ver3.Invoce();
                    invoce.InvoceID = dis_invoce.ID.ToString();
                    invoce.TRNumber = ClsSystem.gnvl(dis_invoce.TRNumber,"");
                    invoce.InvoceType = invoce.TRNumber == "" ? "普通发票" : "增值税发票";
                    invoce.Rise = dis_invoce.Rise;
                    invoce.Content = dis_invoce.Content;
                    invoce.OBank = dis_invoce.OBank;
                    invoce.OAccount = dis_invoce.OAccount;
                    list_invoce.Add(invoce);
                }
            }
            #endregion

            #region//获取经销商的管理员登录账号
            //先从SYS_CompUser表中取出对应sys_user表的ID
            List<Hi.Model.SYS_CompUser> list_compuser = new Hi.BLL.SYS_CompUser().GetList("UserID",
                "DisID=" + dis.ID + " and UType = 5 and isnull(dr,0)=0 and isnull(IsEnabled,0)=1 and isnull(IsAudit,0)=2", "");
            //根据userid取出sys_user表数据,赋值给返回信息
            Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(list_compuser[0].UserID);
            class_ver3.Account account = new class_ver3.Account();
            if (user != null && user.AuditState == 2 && user.IsEnabled == 1 && user.dr == 0)
            {
                account.AccountID = user.ID.ToString();
                account.UserName = user.UserName;
                account.TrueName = user.TrueName;
                account.Phone = user.Phone;
                account.ts = user.ts.ToString();
            }
            #endregion
            res.InvoceList = list_invoce;
            res.Account = account;
            return new ResultResellerDetail() { Result ="T",Description = "获取成功",Reseller= res};
        }
        catch(Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerDetail:" + JSon);
            return new ResultResellerDetail() { Result = "F", Description = "获取失败" };

        }
    }


    /// <summary>
    /// 获取经销商区域
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultResellerArea GetResellerAreaList(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "")
                return new ResultResellerArea() { Result ="F",Description ="参数异常"};
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            //判断登录信息是否异常
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, Int32.Parse(CompID)))
            {
                return new ResultResellerArea() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业信息是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultResellerArea() { Result = "F", Description = "核心企业信息异常" };
            #endregion
            //赋值返回值
            List<class_ver3.Area> list_area = new List<class_ver3.Area>();
            //根据核心企业ID获取其经销商区域list
            List<Hi.Model.BD_DisArea> list_areamodel = new Hi.BLL.BD_DisArea().GetList("", "CompanyID=" + comp.ID + " and isnull(dr,0)=0", "SortIndex");
            if (list_areamodel != null && list_areamodel.Count > 0)
            {
                foreach (Hi.Model.BD_DisArea disarea in list_areamodel)
                {
                    class_ver3.Area area = new class_ver3.Area();
                    area.AreaID = disarea.ID.ToString();
                    area.AreaName = disarea.AreaName;
                    area.AreaCode = disarea.Areacode;
                    area.ParentID = disarea.ParentID.ToString();
                    area.SortIndex = disarea.SortIndex;
                    list_area.Add(area);
                }
            }
            return new ResultResellerArea() { Result="T",Description = "获取成功",AreaList = list_area};
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerAreaList:" + JSon);
            return new ResultResellerArea() { Result = "F", Description = "获取失败" };
        }
    }

    /// <summary>
    ///核心企业修改经销商登录信息
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultEditResellerAccount EditResellerAccount(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string AccountID = string.Empty;
        string TrueName = string.Empty;
        string ts = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "" || JInfo["AccountID"].ToString().Trim() == ""||
                JInfo["TrueName"].ToString().Trim()==""||JInfo["ts"].ToString().Trim()=="")
            {
                return new ResultEditResellerAccount() { Result = "F",Description="参数异常"};
            }
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            AccountID = JInfo["AccountID"].ToString();
            TrueName = Common.NoHTML(JInfo["TrueName"].ToString());
            ts = JInfo["ts"].ToString();
            //判断登录信息是否异常
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, Int32.Parse(CompID)))
            {
                return new ResultEditResellerAccount() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业信息是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ResultEditResellerAccount() { Result = "F", Description = "核心企业信息异常" };
            #endregion
            //根据登录信息ID获取对应的sys_users中数据
            Hi.BLL.SYS_Users bll_users = new Hi.BLL.SYS_Users();
            Hi.Model.SYS_Users account = bll_users.GetModel(Int32.Parse(AccountID));
            //判断用户信息是否正常
            if (account == null)
                return new ResultEditResellerAccount() { Result = "F", Description = "登录信息异常" };
            if(account.dr==1)
                return new ResultEditResellerAccount() { Result = "F", Description = "登录信息已被删除" };
            if(ts!=account.ts.ToString())
                return new ResultEditResellerAccount() { Result = "F", Description = "登录信息已被他人修改，请稍后再试" };
            //修改姓名
            account.TrueName = TrueName;
            if (!bll_users.Update(account))
                return new ResultEditResellerAccount() {Result="F",Description = "修改失败" };
            //成功需要将新的登录信息返回回去
            class_ver3.Account result_account = new class_ver3.Account();
            result_account.AccountID = account.ID.ToString();
            result_account.UserName = account.UserName;
            result_account.TrueName = account.TrueName;
            result_account.Phone = account.Phone;
            return new ResultEditResellerAccount() { Result="T",Description = "返回成功",Account = result_account};

        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "EidtResellerAccount:" + JSon);
            return new ResultEditResellerAccount() { Result = "F", Description = "修改失败" };
        }
    }

    /// <summary>
    ///核心企业修改经销商信息
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ReseltResellerEdit EditReseller(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        string DisID = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "" || JInfo["Reseller"].ToString().Trim() == "")
                return new ReseltResellerEdit() { Result ="F",Description = "参数异常"};
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            JsonData Reseller = JInfo["Reseller"];
            //判断登录信息是否异常
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, Int32.Parse(CompID)))
            {
                return new ReseltResellerEdit() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业信息是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ReseltResellerEdit() { Result = "F", Description = "核心企业信息异常" };
            #endregion
            //判断经销商信息是否正常
            DisID = Reseller["ResellerID"].ToString();
            Hi.BLL.BD_Distributor bll_dis = new Hi.BLL.BD_Distributor();
            Hi.Model.BD_Distributor dis = bll_dis.GetModel(Int32.Parse(DisID));
            if(dis==null)
                return new ReseltResellerEdit() { Result = "F", Description = "经销商信息异常" };
            if(dis.dr==1)
                return new ReseltResellerEdit() { Result = "F", Description = "经销商已被删除" };
            if(Reseller["ts"].ToString() != dis.ts.ToString())
                return new ReseltResellerEdit() { Result = "F", Description = "经销商信息已被他人修改，请稍后再试" };
            string resellername = Common.NoHTML(Reseller["ResellerName"].ToString().Trim());
            string resellercode = Common.NoHTML(Reseller["ResellerCode"].ToString().Trim());
            string address = Common.NoHTML(Reseller["Address"].ToString().Trim());
            //判断经销商修改的必填项
            if (resellername == "")
                return new ReseltResellerEdit() { Result = "F", Description = "请输入经销商名称" };
            //判断经销商名字是否已存在
            //if (Common.DisExistsAttribute("DisName", Reseller["ResellerName"].ToString(), CompID.ToString()))
            //List<Hi.Model.BD_Distributor> list_dis = bll_dis.GetList("", "DisName = '" + resellername + "' and ID <> " + dis.ID + "", "");
            //if(list_dis!=null&&list_dis.Count>0)
            if (Common.DisExistsAttribute("DisName", resellername, CompID.ToString(),dis.ID.ToString()))
                return new ReseltResellerEdit() { Result = "F", Description = "经销商名称已经存在" };
            if(Reseller["ResellerProvince"].ToString().Trim()=="")
                return new ReseltResellerEdit() { Result = "F", Description = "请选择经销商地址中的省" };
            if (Reseller["ResellerCity"].ToString().Trim() == "")
                return new ReseltResellerEdit() { Result = "F", Description = "请选择经销商地址中的市" };
            if (Reseller["ResellerArea"].ToString().Trim() == "")
                return new ReseltResellerEdit() { Result = "F", Description = "请选择经销商地址中的区" };
            if (address == "")
                return new ReseltResellerEdit() { Result = "F", Description = "请输入经销商地址中的详细地址" };
            //修改经销商信息
            dis.ts = DateTime.Now;
            dis.modifyuser = one.ID;
            dis.DisCode = resellercode;
            dis.DisName = resellername;
            //Int32 classifyid= Reseller["ResellerClassifyID"].ToString().Trim() == "" ? 0 : Int32.Parse(Reseller["ResellerClassifyID"].ToString().Trim());
            //传入分类ID的话，判断分类ID是否正确
            if (Reseller["ResellerClassifyID"].ToString().Trim() != "" && Reseller["ResellerClassifyID"].ToString()!="0")
            {
                Hi.Model.BD_DisType distpye = new Hi.BLL.BD_DisType().GetModel(Int32.Parse(Reseller["ResellerClassifyID"].ToString().Trim()));
                if (distpye == null||distpye.CompID != comp.ID)
                    return new ReseltResellerEdit() { Result = "F", Description = "经销商分类异常" };
                if (distpye.dr == 1)
                    return new ReseltResellerEdit() { Result = "F", Description = "此经销商分类已被删除" };
                //if (distpye.IsEnabled !=0)
                //    return new ReseltResellerEdit() { Result = "F", Description = "此经销商分类已被禁用" };
                
                dis.DisTypeID = distpye.ID;
            }
            else
                dis.DisTypeID = 0;
            //dis.AreaID = Reseller["AreaID"].ToString().Trim() == "" ? 0 : Int32.Parse(Reseller["AreaID"].ToString().Trim());
            //传入区域ID的话，判断区域ID是否正确
            if (Reseller["AreaID"].ToString().Trim() != "" && Reseller["AreaID"].ToString()!="0")
            {
                Hi.Model.BD_DisArea disarea = new Hi.BLL.BD_DisArea().GetModel(Int32.Parse(Reseller["AreaID"].ToString().Trim()));
                if (disarea == null || disarea.CompanyID != comp.ID)
                    return new ReseltResellerEdit() { Result = "F", Description = "经销商区域异常" };
                if (disarea.dr == 1)
                    return new ReseltResellerEdit() { Result = "F", Description = "此经销商区域已被删除" };
                dis.AreaID = disarea.ID;

            }
            else
                dis.AreaID = 0;
            dis.Province = Reseller["ResellerProvince"].ToString();
            dis.City = Reseller["ResellerCity"].ToString();
            dis.Area = Reseller["ResellerArea"].ToString();
            dis.Address = address;
            dis.Zip = Common.NoHTML(Reseller["Zip"].ToString());
            dis.Tel = Common.NoHTML(Reseller["Tel"].ToString());
            dis.Fax = Common.NoHTML(Reseller["Fax"].ToString());
            string principal = Common.NoHTML(Reseller["Principal"].ToString().Trim());
            string phone = Common.NoHTML(Reseller["Phone"].ToString().Trim());
            //如果联系人或联系人手机没输入，需要将登陆信息的联系人或手机号，赋值给联系人或手机
            if (principal == "" || phone == "")
            {
                //先获取sys_compuser表中disid对应的数据，一对一关系
                List<Hi.Model.SYS_CompUser> compuser = new Hi.BLL.SYS_CompUser().GetList("UserID",
                "DisID=" + dis.ID + " and UType = 5 and isnull(dr,0)=0 and isnull(IsEnabled,0)=1 and isnull(IsAudit,0)=2", "");
                //通过Userid获取sys_users表数据
                Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(compuser[0].ID);
                if (user != null && user.AuditState == 2 && user.IsEnabled == 1 && user.dr == 0)
                {
                    //需要判断到底是传入的联系人为空，还是手机为空
                    if (principal == "")
                        dis.Principal = user.TrueName;
                    else
                        dis.Principal = principal;
                    if (phone == "")
                        dis.Phone = user.Phone;
                    else
                        dis.Phone = phone;
                }

            }
            else
            {
                dis.Principal = principal;
                dis.Phone = phone;
            }
            //更新数据库
            if (bll_dis.Update(dis))
                return new ReseltResellerEdit() { Result = "T", Description = "修改成功" };
            else
                return new ReseltResellerEdit() { Result="F",Description ="修改失败"};
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "EditReseller:" + JSon);
            return new ReseltResellerEdit() { Result = "F", Description = "修改失败" };
        }
    }

    /// <summary>
    ///核心企业新增经销商
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ReseltResellerEdit AddReseller(string JSon)
    {
        string UserID = string.Empty;
        string CompID = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo["UserID"].ToString().Trim() == "" || JInfo["CompID"].ToString().Trim() == "" || JInfo["Reseller"].ToString().Trim() == "")
                return new ReseltResellerEdit() { Result = "F", Description = "参数异常" };
            UserID = JInfo["UserID"].ToString();
            CompID = JInfo["CompID"].ToString();
            JsonData Reseller = JInfo["Reseller"];
            //判断登录信息是否异常
            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out one, Int32.Parse(CompID)))
            {
                return new ReseltResellerEdit() { Result = "F", Description = "登录信息异常" };
            }
            //判断核心企业信息是否异常
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Int32.Parse(CompID));
            if (comp == null || comp.dr == 1 || comp.IsEnabled == 0 || comp.AuditState == 0)
                return new ReseltResellerEdit() { Result = "F", Description = "核心企业信息异常" };
            #endregion
            #region//判断传入的经销商信息是否正确
            string resellername = Common.NoHTML(Reseller["ResellerName"].ToString().Trim());
            string resellercode = Common.NoHTML(Reseller["ResellerCode"].ToString().Trim());
            string address = Common.NoHTML(Reseller["Address"].ToString().Trim());
            if (resellername == "")
                return new ReseltResellerEdit() { Result = "F", Description = "请输入经销商名称" };
            //判断经销商名字是否已存在
            if (Common.DisExistsAttribute("DisName", resellername, CompID.ToString()))
                return new ReseltResellerEdit() { Result = "F", Description = "经销商名称已经存在" };
            if (Reseller["ResellerProvince"].ToString().Trim() == "")
                return new ReseltResellerEdit() { Result = "F", Description = "请选择经销商地址中的省" };
            if (Reseller["ResellerCity"].ToString().Trim() == "")
                return new ReseltResellerEdit() { Result = "F", Description = "请选择经销商地址中的市" };
            if (Reseller["ResellerArea"].ToString().Trim() == "")
                return new ReseltResellerEdit() { Result = "F", Description = "请选择经销商地址中的区" };
            if (address == "")
                return new ReseltResellerEdit() { Result = "F", Description = "请输入经销商地址中的详细地址" };
            #endregion
            #region//判断登录信息的正确信
            JsonData account = Reseller["Account"];
            string username = Common.NoHTML(account["UserName"].ToString().Trim());
            string truename = Common.NoHTML(account["TrueName"].ToString().Trim());
            if(account["UserName"].ToString().Trim()=="")
                return new ReseltResellerEdit() { Result = "F", Description = "请输入登录账号" };
            //判断登录账号是否已经存在
            if (Common.GetUserExists(username))
                return new ReseltResellerEdit() { Result = "F", Description = "该登录账号已存在" };
            if (truename == "")
                return new ReseltResellerEdit() { Result = "F", Description = "请输入姓名" };
            if(account["Phone"].ToString().Trim()=="")
                return new ReseltResellerEdit() { Result = "F", Description = "请登录信息中的手机号码" };
            //判断手机号有没被注册过
            Regex Phonereg = new Regex("^0?(13[0-9]|15[012356789]|18[0-9]|14[57]|17[7])[0-9]{8}$");
            if (!Phonereg.IsMatch(account["Phone"].ToString()))
                return new ReseltResellerEdit() { Result = "F", Description = "登录信息中的手机号格式错误" };
            if (Common.GetUserExists("Phone", account["Phone"].ToString()))
                return new ReseltResellerEdit() { Result = "F",Description = "登录信息中的手机号已被注册"};
            #endregion
            //创建需要导入的经销商实体
            Hi.Model.BD_Distributor disModel = new Hi.Model.BD_Distributor();
            disModel.CompID = comp.ID;
            disModel.DisCode = resellercode;
            disModel.DisName = resellername;
            //传入分类ID的话，判断分类ID是否正确
            if (Reseller["ResellerClassifyID"].ToString().Trim() != "" && Reseller["ResellerClassifyID"].ToString() != "0")
            {
                Hi.Model.BD_DisType distpye = new Hi.BLL.BD_DisType().GetModel(Int32.Parse(Reseller["ResellerClassifyID"].ToString().Trim()));
                if (distpye == null || distpye.CompID != comp.ID)
                    return new ReseltResellerEdit() { Result = "F", Description = "经销商分类异常" };
                if (distpye.dr == 1)
                    return new ReseltResellerEdit() { Result = "F", Description = "此经销商分类已被删除" };
                //if (distpye.IsEnabled != 0)
                //    return new ReseltResellerEdit() { Result = "F", Description = "此经销商分类已被禁用" };

                disModel.DisTypeID = distpye.ID;
            }
            else
                disModel.DisTypeID = 0;
            //传入区域ID的话，判断区域ID是否正确
            if (Reseller["AreaID"].ToString().Trim() != "" && Reseller["AreaID"].ToString() != "0")
            {
                Hi.Model.BD_DisArea disarea = new Hi.BLL.BD_DisArea().GetModel(Int32.Parse(Reseller["AreaID"].ToString().Trim()));
                if (disarea == null || disarea.CompanyID != comp.ID)
                    return new ReseltResellerEdit() { Result = "F", Description = "经销商区域异常" };
                if (disarea.dr == 1)
                    return new ReseltResellerEdit() { Result = "F", Description = "此经销商区域已被删除" };
                disModel.AreaID = disarea.ID;

            }
            else
                disModel.AreaID = 0;
            disModel.DisLevel = "";
            disModel.Province = Reseller["ResellerProvince"].ToString();
            disModel.City = Reseller["ResellerCity"].ToString();
            disModel.Area = Reseller["ResellerArea"].ToString();
            disModel.Address = address;
            //没输入经销商中的联系人，需要将登录信息的姓名赋值给联系人
            string principal = Common.NoHTML(Reseller["Principal"].ToString().Trim());
            string phone = Common.NoHTML(Reseller["Phone"].ToString().Trim());
            if (principal == "")
                disModel.Principal = truename;
            else
                disModel.Principal = principal;
            //没输入联系人手机号，需要将登录信息中的手机号赋值给联系人手机号
            if (phone == "")
                disModel.Phone = account["Phone"].ToString();
            else
                disModel.Phone = phone;
            disModel.Leading = "";
            disModel.LeadingPhone = "";
            disModel.Licence = "";
            disModel.Tel = Common.NoHTML(Reseller["Tel"].ToString());
            disModel.Zip = Common.NoHTML(Reseller["Zip"].ToString());
            disModel.Fax = Common.NoHTML(Reseller["Fax"].ToString());
            disModel.Remark = "";
            disModel.DisAccount = 0;
            disModel.IsCheck = 1;
            disModel.CreditType = 0;
            disModel.CreditAmount = 0;
            disModel.Paypwd = Common.md5("123456");
            disModel.AuditState = 2;
            disModel.IsEnabled = 1;
            disModel.CreateUserID = one.ID;
            disModel.CreateDate = DateTime.Now;
            disModel.ts = DateTime.Now;
            disModel.dr = 0;
            disModel.modifyuser = one.ID;
            //开启事务，并将dismodel插入经销商表中
            SqlConnection conn = new SqlConnection(SqlHelper.LocalSqlServer);
            //开启数据库连接
            if (conn.State.ToString().ToLower() != "open")
                conn.Open();
            //开启事务
            SqlTransaction mytran = conn.BeginTransaction();
            int DisID = 0;
            try
            {
                //在经销商表中插入一条数据
                if ((DisID = new Hi.BLL.BD_Distributor().Add(disModel, mytran)) > 0)
                {
                    //经销商表插入成功的话继续新增角色
                    List<Hi.Model.SYS_Role> list_role = new Hi.BLL.SYS_Role().GetList("", "isnull(dr,0)=0 and isenabled=1 and DisID=" + DisID + " and RoleName='企业管理员'", "");
                    if (list_role==null|| list_role.Count == 0)
                    {
                        //新增角色（企业管理员）
                        Hi.Model.SYS_Role role = new Hi.Model.SYS_Role();
                        role.CompID = comp.ID;
                        role.DisID = DisID;
                        role.RoleName = "企业管理员";
                        role.IsEnabled = 1;
                        role.SortIndex = "1";
                        role.CreateDate = DateTime.Now;
                        role.CreateUserID = one.ID;
                        role.ts = DateTime.Now;
                        role.modifyuser = one.ID;
                        role.dr = 0;
                        int Roid = new Hi.BLL.SYS_Role().Add(role, mytran);
                        //新增管理员用户和角色
                        Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
                        user.UserName = username;
                        // user.CompID = CompID;
                        // user.Type = 5;
                        // user.RoleID = Roid;
                        user.TrueName = truename;
                        user.UserPwd =Common.md5("123456");
                        user.Phone = account["Phone"].ToString();
                        user.AuditState = 2;
                        user.IsEnabled = 1;
                        user.CreateUserID = one.ID;
                        user.CreateDate = DateTime.Now;
                        user.ts = DateTime.Now;
                        user.modifyuser = one.ID;
                        int AddUserid = new Hi.BLL.SYS_Users().Add(user, mytran);
                        ///用户明细表
                        Hi.Model.SYS_CompUser CompUser = new Hi.Model.SYS_CompUser();
                        CompUser.CompID = comp.ID;
                        CompUser.DisID = DisID;
                        CompUser.CreateDate = DateTime.Now;
                        CompUser.CreateUserID = one.ID;
                        CompUser.modifyuser = one.ID;
                        CompUser.CType = 2;
                        CompUser.UType = 5;
                        CompUser.IsEnabled = 1;
                        CompUser.IsAudit = 2;
                        CompUser.RoleID = 0;
                        CompUser.ts = DateTime.Now;
                        CompUser.dr = 0;
                        CompUser.UserID = AddUserid;
                        int compuserid =  new Hi.BLL.SYS_CompUser().Add(CompUser, mytran);
                        //新增角色用户
                        Hi.Model.SYS_RoleUser RoleUser = new Hi.Model.SYS_RoleUser();
                        RoleUser.FunType = 1;
                        RoleUser.UserID = AddUserid;
                        RoleUser.RoleID = Roid;
                        RoleUser.IsEnabled = true;
                        RoleUser.CreateUser = UserID;
                        RoleUser.CreateDate = DateTime.Now;
                        RoleUser.ts = DateTime.Now;
                        RoleUser.dr = 0;
                        int roleuserid = new Hi.BLL.SYS_RoleUser().Add(RoleUser, mytran);
                        //新增角色权限表
                        Hi.Model.SYS_RoleSysFun rolesys = null;
                        List<Hi.Model.SYS_SysFun> funList = new Hi.BLL.SYS_SysFun().GetList("", " Type=2", "");
                        foreach (Hi.Model.SYS_SysFun sys in funList)
                        {
                            rolesys = new Hi.Model.SYS_RoleSysFun();
                            rolesys.CompID = comp.ID;
                            rolesys.DisID = DisID;
                            rolesys.RoleID = Roid;
                            rolesys.FunCode = sys.FunCode;
                            rolesys.FunName = sys.FunName;
                            rolesys.IsEnabled = 1;
                            rolesys.CreateUserID = one.ID;
                            rolesys.CreateDate = DateTime.Now;
                            rolesys.ts = DateTime.Now;
                            rolesys.modifyuser = one.ID;
                            if (new Hi.BLL.SYS_RoleSysFun().Add(rolesys, mytran)<=0)
                            {
                                mytran.Rollback();
                                return new ReseltResellerEdit() { Result = "F", Description = "新增失败" };
                            }
                        }
                        //新增收货地址
                        Hi.Model.BD_DisAddr addr = new Hi.Model.BD_DisAddr();
                        addr.Province = disModel.Province;
                        addr.City = disModel.City;
                        addr.Area = disModel.Area;
                        addr.DisID = DisID;
                        addr.Principal = disModel.Principal;
                        addr.Phone = disModel.Phone;
                        addr.Address = disModel.Province + disModel.City + disModel.Area + disModel.Address;
                        addr.IsDefault = 1;
                        addr.ts = DateTime.Now;
                        addr.CreateDate = DateTime.Now;
                        addr.CreateUserID = one.ID;
                        addr.modifyuser = one.ID;
                        int addrid = new Hi.BLL.BD_DisAddr().Add(addr, mytran);
                        //判断所有表是否都插入成功了吗
                        if (Roid <= 0 || AddUserid <= 0 || compuserid <= 0 || roleuserid <= 0 || addrid <= 0)
                        {
                            mytran.Rollback();
                            return new ReseltResellerEdit() { Result = "F", Description = "新增失败" };
                        }
                    }
                    else
                    {
                        mytran.Rollback();
                        return new ReseltResellerEdit() { Result = "F", Description = "新增失败" };
                    }
                }
                else
                {
                    mytran.Rollback();
                    return new ReseltResellerEdit() { Result = "F", Description = "新增失败" };
                }
                mytran.Commit();
            }
            catch (Exception ex)
            {
                mytran.Rollback();
                Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "AddReseller:" + JSon);
                return new ReseltResellerEdit() { Result = "F", Description = "新增失败" };
            }
            finally
            {
                conn.Close();
                mytran.Dispose();
            }
            return new ReseltResellerEdit() { Result="T",Description ="新增成功"};
     
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "AddReseller:" + JSon);
            return new ReseltResellerEdit() { Result = "F", Description = "新增失败" };
        }
    }
    #region//返回参数
    public class ReseltResellerEdit
    {
        public String Result { get; set; }
        public String Description { get; set; }
    }
    public class ResultResellerClassify
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public List<class_ver3.ResellerClassify> ResellerClassifyList { get; set; }
    }

    public class ResultResellerDetail
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public class_ver3.Reseller Reseller { get; set; }
    }

    //获取经销商区域的返回参数
    public class ResultResellerArea
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public List<class_ver3.Area> AreaList { get; set; }
    }

    //修改经销商登录信息的返回参数
    public class ResultEditResellerAccount
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public class_ver3.Account Account { get; set; }
    }
    #endregion


}
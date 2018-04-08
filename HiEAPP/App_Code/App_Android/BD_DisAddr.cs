using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using LitJson;
using Newtonsoft.Json;


public class BD_DisAddr
{
    public BD_DisAddr()
    {
    }

    /// <summary>
    /// 获取收货地址列表
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultAddr GetResellerShippingAddressList(string JSon,string version)
    {
        try
        {
            #region JSon取值

            string userID = string.Empty;
            string compID = string.Empty;
            string disID = string.Empty; //当前列表最临界点产品ID:初始-1

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" ||
                (JInfo["CompanyID"].ToString() != "" && JInfo["ResellerID"].ToString() != ""))
            {
                userID = JInfo["UserID"].ToString();
                compID = JInfo["CompanyID"].ToString();
                disID = JInfo["ResellerID"].ToString();
            }
            else
            {
                return new ResultAddr() {Result = "F", Description = "参数异常"};
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (version.ToLower() != "android" && version.ToLower() != "ios" && float.Parse(version) >= 6)
            {
                if (disID != "")
                {
                    if (!new Common().IsLegitUser(int.Parse(userID), out user, 0, int.Parse(disID == "" ? "0" : disID)))
                        return new ResultAddr() { Result = "F", Description = "登录信息异常" };
                }
                else
                {
                    if (!new Common().IsLegitUser(int.Parse(userID), out user, int.Parse(compID), 0))
                        return new ResultAddr() { Result = "F", Description = "登录信息异常" };
                }
            }
            else
            {
                if (!new Common().IsLegitUser(int.Parse(userID), out user, 0, int.Parse(disID == "" ? "0" : disID)))
                    return new ResultAddr() { Result = "F", Description = "登录信息异常" };
            }
            #endregion

            List<Hi.Model.BD_DisAddr> addrList = new Hi.BLL.BD_DisAddr().GetList("",
                " DisID='" + disID + "' and ISNULL(dr,0)=0", "");
            if (addrList.Count == 0)
                return new ResultAddr() {Result = "F", Description = "地址为空异常"};
            List<DisAddress> DisAddressList = new List<DisAddress>();
            foreach (var addr in addrList)
            {
                DisAddress disAdress = new DisAddress();
                disAdress.DisAddressID = addr.ID.ToString();
                disAdress.Name = addr.Name;
                disAdress.DisID = disID;
                disAdress.Principal = addr.Principal;
                disAdress.Phone = addr.Phone;
                disAdress.Tel = addr.Tel;
                disAdress.Province = addr.Province;
                disAdress.City = addr.City;
                disAdress.Area = addr.Area;
                disAdress.Zip = addr.Zip;
                disAdress.Address = addr.Address;
                disAdress.Remark = addr.Remark;
                disAdress.IsDefault = addr.IsDefault.ToString();
                DisAddressList.Add(disAdress);
            }
            return new ResultAddr()
            {
                Result = "T",
                Description = "获取成功",
                DisAddressList = DisAddressList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerShippingAddressList ：" + JSon);
            return new ResultAddr() {Result = "F", Description = "参数异常"};
        }
    }

    public ResultAddrAdd ResellerAddrAdd(string JSon)
    {
        try
        {
            #region JSon取值

            string userID = string.Empty;
            string disID = string.Empty;
            string phoneCodeID = string.Empty;
            string MessageCode = string.Empty;
            string Principal = string.Empty;
            string Phone = string.Empty;
            string Province = string.Empty;
            string City = string.Empty;
            string Area = string.Empty;
            string Address = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "" &&
                JInfo["ChangePasswordID"].ToString() != "" && JInfo["MessageCode"].ToString() != "" &&
                JInfo["Phone"].ToString() != "" && JInfo["Principal"].ToString() != "" &&
                JInfo["Province"].ToString() != "" && JInfo["City"].ToString() != "" &&
                JInfo["Area"].ToString() != "" && JInfo["Address"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString();
                disID = JInfo["ResellerID"].ToString();
                phoneCodeID = JInfo["ChangePasswordID"].ToString();
                MessageCode = JInfo["MessageCode"].ToString();
                Phone = JInfo["Phone"].ToString();
                Principal = JInfo["Principal"].ToString();
                Province = JInfo["Province"].ToString();
                City = JInfo["City"].ToString();
                Area = JInfo["Area"].ToString();
                Address = JInfo["Address"].ToString();
            }
            else
            {
                return new ResultAddrAdd() {Result = "F", Description = "参数异常"};
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user, 0, int.Parse(disID == "" ? "0" : disID)))
                return new ResultAddrAdd() {Result = "F", Description = "登录信息异常"};

            #endregion

            Hi.Model.SYS_PhoneCode code = new Hi.BLL.SYS_PhoneCode().GetModel(int.Parse(phoneCodeID));
            if (code != null)
            {
                if (code.ts.AddMinutes(30) < DateTime.Now || code.IsPast == 1)
                    return new ResultAddrAdd() {Result = "F", Description = "验证码过期"};
                if (code.UserID.ToString() != userID)
                    return new ResultAddrAdd() {Result = "F", Description = "非本人操作"};
                if (code.PhoneCode != MessageCode)
                    return new ResultAddrAdd() {Result = "F", Description = "验证码错误"};
            }
            else
            {
                return new ResultAddrAdd() {Result = "F", Description = "验证码异常"};
            }

            List<Hi.Model.BD_DisAddr> addrList = new Hi.BLL.BD_DisAddr().GetList("", "disid='" + disID + "' and ", "");
            if (addrList != null)
            {
                if (addrList.Count >= 10)
                    return new ResultAddrAdd() {Result = "F", Description = "一个经销商收货地址最多为10个"};
                if (addrList.Select(p => p.Address).Contains(Province + City + Area + Address))
                    return new ResultAddrAdd() {Result = "F", Description = "经销商收货地址已经存在"};
            }

            Hi.Model.BD_DisAddr addr = new Hi.Model.BD_DisAddr()
            {
                IsDefault = 0,
                Principal = Principal,
                Phone = Phone,
                Province = Province,
                City = City,
                Area = Area,
                Address = Province + City + Area + Address,
                CreateDate = DateTime.Now,
                CreateUserID = Convert.ToInt32(userID),
                modifyuser = Convert.ToInt32(userID),
                dr = 0
            };

            int count = new Hi.BLL.BD_DisAddr().Add(addr);
            if (count > 0)
                return new ResultAddrAdd() {Result = "T", Description = "新增成功", AddrID = count};
            else
            {
                return new ResultAddrAdd() {Result = "F", Description = "新增失败"};
            }
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "ResellerAddrAdd ：" + JSon);
            return new ResultAddrAdd { Result = "F", Description = "异常" };
        }
    }

    #region 返回

    public class ResultAddrAdd
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public int AddrID { get; set; }
    }

    public class ResultAddr
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public List<DisAddress> DisAddressList { get; set; }
    }

    public class DisAddress
    {
        public string DisAddressID { get; set; }
        public string Name { get; set; }
        public string DisID { get; set; }
        public string Principal { get; set; }
        public string Phone { get; set; }
        public string Tel { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public string IsDefault { get; set; }
    }

    public class ResultExpress
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public Express Express { get; set; }
    }

    public class Express
    {
        public string ComPName { get; set; }
        public string LogisticsNo { get; set; }
        public string Context { get; set; }
        public string Type { get; set; }
    }

    #endregion
}

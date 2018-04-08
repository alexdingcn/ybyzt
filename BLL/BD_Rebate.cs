using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using DBUtility;

namespace Hi.BLL
{
    public partial class BD_Rebate
    {
        /// <summary>
        /// 更新一条记录，带事务
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Tran"></param>
        /// <returns></returns>
        public bool Update(Hi.Model.BD_Rebate model, SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }

        /// <summary>
        /// 获取返利列表，带事务
        /// </summary>
        /// <param name="strWhat"></param>
        /// <param name="strWhere"></param>
        /// <param name="strOrderby"></param>
        /// <param name="TranSaction">可不传值</param>
        /// <returns></returns>
        public List<Hi.Model.BD_Rebate> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction TranSaction = null)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, TranSaction) as List<Hi.Model.BD_Rebate>;
        }

        /// <summary>
        /// 保存订单，获取当前可用返利余额
        /// </summary>
        /// <param name="disID"></param>
        /// <param name="sqltans">可不传值</param>
        /// <returns></returns>
        public decimal GetRebateEnableAmount(int disID, SqlTransaction sqltans = null)
        {
            decimal res = 0;
            string strWhere = " disID = " + disID + " and IsNull(dr,0) = 0 and RebateState = 1 and EnableAmount <> 0 and getdate() between StartDate and dateadd(day,1,EndDate)";
            List<Hi.Model.BD_Rebate> list = sqltans == null ? GetList("", strWhere, "EndDate") : GetList("", strWhere, "EndDate", sqltans);
            if (list != null && list.Count > 0)
            {
                res += list.Sum(item => item.EnableAmount);
            }
            return res;
        }

        /// <summary>
        /// 获取即将使用的返利单列表，带事务
        /// </summary>
        /// <param name="disID"></param>
        /// <param name="orderAmount"></param>
        /// <param name="sqltans">可不传值</param>
        /// <returns></returns>
        private List<Hi.Model.BD_Rebate> GetToBeUserdList(int disID, decimal orderAmount, SqlTransaction sqltans = null)
        {
            List<Hi.Model.BD_Rebate> list = new List<Model.BD_Rebate>();

            string str = "and IsNull(dr,0) = 0 and RebateState = 1 and EnableAmount <> 0 and getdate() between StartDate and dateadd(day,1,EndDate)";
            List<Hi.Model.BD_Rebate> rebateList = sqltans == null ? new Hi.BLL.BD_Rebate().GetList("", " disID = " + disID + str, "EndDate")
                : GetList("*", " disID = " + disID + str, "EndDate", sqltans);
            if (rebateList != null && rebateList.Count > 0)
            {
                decimal amount = 0;
                foreach (var item in rebateList)
                {
                    amount += item.EnableAmount;
                    list.Add(item);
                    if (amount >= orderAmount)
                    {
                        break;
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 保存订单，使用返利，带事务
        /// </summary>
        /// <param name="disID"></param>
        /// <param name="orderAmount"></param>
        /// <param name="orderID"></param>
        /// <param name="sqltans">可空</param>
        /// <returns></returns>
        public bool TransRebate(int disID, decimal orderAmount, int orderID, int userID, SqlTransaction sqltans = null)
        {
            decimal nowAmount = orderAmount;
            try
            {
                if (orderAmount == 0)
                    return true;

                if (GetRebateEnableAmount(disID, sqltans) < orderAmount)
                    return false;

                List<Hi.Model.BD_Rebate> list = GetToBeUserdList(disID, orderAmount, sqltans).OrderBy(p => p.EndDate).ToList();
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].EnableAmount < nowAmount)
                        {
                            nowAmount -= list[i].EnableAmount;

                            Hi.Model.BD_RebateDetail rebateDetail = new Model.BD_RebateDetail
                            {
                                RebateID = list[i].ID,
                                Amount = list[i].EnableAmount,
                                CreateDate = DateTime.Now,
                                CreateUserID = userID,
                                EnableAmount = list[i].EnableAmount,
                                OrderID = orderID,
                                dr = 0,
                                modifyuser = userID,
                                ts = DateTime.Now
                            };

                            list[i].EnableAmount = 0;
                            list[i].UserdAmount = list[i].RebateAmount;
                            list[i].ts = DateTime.Now;
                            list[i].modifyuser = userID;

                            bool res = Update(list[i], sqltans);
                            if (!res)
                            {
                                return false;
                            }
                            int re = Add(rebateDetail, sqltans);
                            if (re <= 0)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            Hi.Model.BD_RebateDetail rebateDetail = new Model.BD_RebateDetail
                            {
                                RebateID = list[i].ID,
                                Amount = nowAmount,
                                CreateDate = DateTime.Now,
                                CreateUserID = userID,
                                EnableAmount = list[i].EnableAmount,
                                OrderID = orderID,
                                dr = 0,
                                modifyuser = userID,
                                ts = DateTime.Now
                            };

                            list[i].EnableAmount = list[i].EnableAmount - nowAmount;
                            list[i].UserdAmount = list[i].UserdAmount + nowAmount;
                            list[i].ts = DateTime.Now;
                            list[i].modifyuser = userID;

                            bool res = Update(list[i], sqltans);
                            if (!res)
                            {
                                return false;
                            }

                            int re = Add(rebateDetail, sqltans);
                            if (re <= 0)
                            {
                                return false;
                            }

                            return true;
                        }

                        #region
                        //if (i == list.Count - 1)
                        //{
                        //    Hi.Model.BD_RebateDetail rebateDetail = new Model.BD_RebateDetail
                        //    {
                        //        RebateID = list[i].ID,
                        //        Amount = orderAmount - nowAmount,
                        //        CreateDate = DateTime.Now,
                        //        CreateUserID = userID,
                        //        EnableAmount = list[i].EnableAmount,
                        //        OrderID = orderID,
                        //        dr = 0,
                        //        modifyuser = userID,
                        //        ts = DateTime.Now
                        //    };

                        //    list[i].EnableAmount = list[i].EnableAmount - (orderAmount - nowAmount);
                        //    list[i].UserdAmount = list[i].UserdAmount + (orderAmount - nowAmount);
                        //    list[i].ts = DateTime.Now;
                        //    list[i].modifyuser = userID;

                        //    bool res = Update(list[i], sqltans);
                        //    if (!res)
                        //    {
                        //        return false;
                        //    }

                        //    int re = Add(rebateDetail, sqltans);
                        //    if (re <= 0)
                        //    {
                        //        return false;
                        //    }
                        //}
                        //else
                        //{
                        //    nowAmount += list[i].EnableAmount;

                        //    Hi.Model.BD_RebateDetail rebateDetail = new Model.BD_RebateDetail
                        //    {
                        //        RebateID = list[i].ID,
                        //        Amount = list[i].EnableAmount,
                        //        CreateDate = DateTime.Now,
                        //        CreateUserID = userID,
                        //        EnableAmount = list[i].EnableAmount,
                        //        OrderID = orderID,
                        //        dr = 0,
                        //        modifyuser = userID,
                        //        ts = DateTime.Now
                        //    };

                        //    list[i].EnableAmount = 0;
                        //    list[i].UserdAmount = list[i].RebateAmount;
                        //    list[i].ts = DateTime.Now;
                        //    list[i].modifyuser = userID;

                        //    bool res = Update(list[i], sqltans);
                        //    if (!res)
                        //    {
                        //        return false;
                        //    }
                        //    int re = Add(rebateDetail, sqltans);
                        //    if (re <= 0)
                        //    {
                        //        return false;
                        //    }
                        //}
                        //return true;
                        #endregion
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 取消订单，回退返利金额，删除返利使用记录
        /// </summary>
        /// <param name="disID"></param>
        /// <param name="orderID"></param>
        /// <param name="userID"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public bool TransCancel(int disID, int orderID, int userID, SqlTransaction sqltans = null)
        {
            try
            {
                List<Hi.Model.BD_RebateDetail> detailList = GetRebateDetailList("", "OrderID =" + orderID + " and IsNull(dr,0) = 0", "", sqltans);
                if (detailList != null && detailList.Count > 0)
                {
                    foreach (var item in detailList)
                    {
                        item.dr = 1;
                        item.modifyuser = userID;
                        item.ts = DateTime.Now;

                        bool res = Update(item, sqltans);
                        if (!res)
                        {
                            return false;
                        }

                        Hi.Model.BD_Rebate rebate = new Hi.SQLServerDAL.BD_Rebate().GetModel(item.RebateID, sqltans);
                        rebate.EnableAmount += item.Amount;
                        rebate.ts = DateTime.Now;
                        rebate.UserdAmount -= item.Amount;
                        rebate.modifyuser = userID;
                        bool re = Update(rebate, sqltans);
                        if (!re)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 修改订单，获取返利可用余额
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="disID"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public decimal GetEditEnableAmount(int orderID, int disID, SqlTransaction sqltans = null)
        {
            decimal res = GetRebateEnableAmount(disID, sqltans);

            List<Hi.Model.BD_RebateDetail> list = sqltans == null ?
                new Hi.BLL.BD_RebateDetail().GetList("", " OrderID = " + orderID + " and IsNull(dr,0) = 0", "") :
                GetRebateDetailList("", " OrderID = " + orderID + " and IsNull(dr,0) = 0", "", sqltans);
            if (list != null && list.Count > 0)
            {
                res += list.Sum(item => item.Amount);
            }

            return res;
        }

        /// <summary>
        /// 修改订单，使用返利，带事务
        /// </summary>
        /// <param name="disID"></param>
        /// <param name="orderAmount"></param>
        /// <param name="orderID"></param>
        /// <param name="sqltans">可空</param>
        /// <returns></returns>
        public bool TransEditRebate(int disID, decimal orderAmount, int orderID, int userID, SqlTransaction sqltans = null)
        {
            try
            {
                bool res = TransCancel(disID, orderID, userID, sqltans);
                if (!res) return false;

                bool re = TransRebate(disID, orderAmount, orderID, userID, sqltans);
                if (!re) return false;

                return true;
            }
            catch
            {
                return false;
            }
        }


        ///////明细操作////////

        public int Add(Hi.Model.BD_RebateDetail model, SqlTransaction Tran)
        {
            return new Hi.SQLServerDAL.BD_RebateDetail().Add(model, Tran);
        }

        public bool Update(Hi.Model.BD_RebateDetail model, SqlTransaction Tran)
        {
            return new Hi.SQLServerDAL.BD_RebateDetail().Update(model, Tran);
        }

        /// <summary>
        /// 获得泛型数据列表,不建议直接使用此方法,请根据业务逻辑重写
        /// </summary>
        public List<Hi.Model.BD_RebateDetail> GetRebateDetailList(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
        {
            return new Hi.SQLServerDAL.BD_RebateDetail().GetList(strWhat, strWhere, strOrderby, Tran) as List<Hi.Model.BD_RebateDetail>;
        }
    }
}

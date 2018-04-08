using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;


namespace Hi.BLL
{
    public partial class PAY_Payment
    {
        /// <summary>
        /// 支付成功，修改支付表状态 
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="orderid"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int updatePayState(SqlConnection sqlconn, int payid, SqlTransaction sqltans)
        {
            return dal.updatePayState(sqlconn, payid, sqltans);
        }
          /// <summary>
        /// 线下支付作废，修改支付表状态
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="orderid"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int updatePayToVoid(SqlConnection sqlconn, int payid, SqlTransaction sqltans)
        {
            return dal.updatePayToVoid(sqlconn, payid, sqltans);
        }
        /// <summary>
        /// 结算成功，修改支付表的结算状态
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="orderid"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int updatePayState_JS(SqlConnection sqlconn, int payid, SqlTransaction sqltans)
        {
            return dal.updatePayState_JS(sqlconn, payid, sqltans);
        }
        /// <summary>
        /// 结算成功，修改订单的支付状态，已结算，部分结算状态
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="id"></param>
        /// <param name="sumPrice"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int UpdateOrderPaystate_JS(SqlConnection sqlconn, int ordid, string vdef9, SqlTransaction sqltans)
        {
            return dal.UpdateOrderPaystate_JS(sqlconn, ordid, vdef9, sqltans);
        }

        /// <summary>
        /// 结算成功，修改企业钱包表状态，已结算，部分结算状态
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="id"></param>
        /// <param name="sumPrice"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int UpdatePrePaystate_JS(SqlConnection sqlconn, int preid, string vdef2, SqlTransaction sqltans)
        {
            return dal.UpdatePrePaystate_JS(sqlconn, preid, vdef2, sqltans);
        }
    }
}

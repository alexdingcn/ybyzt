using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.BLL
{
    public partial class PAY_PrePayment
    {
        /// <summary>
        /// sum一个字段
        /// </summary>
        public decimal sums(int disid, int compid)
        {
            return dal.sums(disid, compid);
        }
        /// <summary>
        /// 支付成功，修改企业钱包状态
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="orderid"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int updatePrepayState(SqlConnection sqlconn, int prepayid, SqlTransaction sqltans)
        {
            return dal.updatePrepayState(sqlconn, prepayid, sqltans);
        }

        /// <summary>
        /// 根据相应的ID获取，相对应的数据集合--销售订单结算
        /// </summary>
        /// <param name="Id">表ID</param>
        /// <returns></returns>
        public DataTable GetdataTable(int type, string strwhere, int typeTable)
        {
            return dal.GetdataTable(type, strwhere, typeTable);
        }

        /// <summary>
        /// 根据相应的ID获取，相对应的数据集合--转账汇款结算
        /// </summary>
        /// <param name="Id">表ID</param>
        /// <returns></returns>
        public DataTable GetdataTable_pre(int type, string strwhere)
        {
            return dal.GetdataTable_pre(type, strwhere);
        }
      
        /// <summary>
        /// 判断guid是否重复
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string Number_repeat(string guid)
        {
            return dal.Number_repeat(guid);
        }

        /// <summary>
        /// 退款成功后，在企业钱包表中增加一条退款的企业钱包记录
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="orderid"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int InsertPrepay(SqlConnection sqlconn, Hi.Model.PAY_PrePayment model, SqlTransaction sqltans)
        {
            return dal.InsertPrepay(sqlconn, model, sqltans);
        }

        /// <summary>
        /// 创建人：耿国行
        /// 创建时间：2015-06-16
        ///重新分页方法
        /// </summary>
        public DataTable GetList_ggh(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            return dal.GetList_ggh(pageSize, pageIndex, fldSort, sort, strCondition, out pageCount, out count);

        }
        /// <summary>
        /// 根据订单id得到付款清单
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public DataTable GetPay(int orderID)
        {
            return dal.GetPay(orderID);
        }

         /// <summary>
        /// 根据订单Id获取订单的支付明细（包括收款账号）
        /// </summary>
        /// <param name="ordid">订单id</param>
        /// <returns></returns>
        public DataTable GetPayedItem(int ordid)
        {
            return dal.GetPayedItem(ordid);
        }
        /// <summary>
        /// 结算账户管理用，（为了避免企业下面的经销商重复关联多个银行卡）
        /// </summary>
        /// <param name="CompID"></param>
        /// <returns></returns>
        public string GetDisIDBYCompID(int CompID)
        {
            return dal.GetDisIDBYCompID(CompID);
        }

        /// <summary>
        /// 结算账户管理用，（为了避免企业有多默认账户）
        /// </summary>
        /// <param name="CompID"></param>
        /// <returns></returns>
        public int GetBankBYCompID(int CompID)
        {
            return dal.GetBankBYCompID(CompID);
        }

        /// <summary>
        /// 修改第一收款账号
        /// </summary>
        /// <param name="CompID"></param>
        /// <returns></returns>
        public int UpisnoBYCompID(int CompID)
        {
            return dal.UpisnoBYCompID(CompID);
        }

        /// <summary>
        /// 获取管理员手机号码
        /// </summary>
        /// <param name="CompID"></param>
        /// <returns></returns>
        public string GetPhoneBYCompID(int CompID)
        {
            return dal.GetPhoneBYCompID(CompID);
        }
        /// <summary>
        /// 获取银行名称
        /// </summary>
        /// <param name="CompID"></param>
        /// <returns></returns>
        public string GetBankNameBYbankID(string bankID)
        {
            return dal.GetBankNameBYbankID(bankID);
        }
        /// <summary>
        /// 判断是否修改过，这三个值
        /// </summary>
        /// <param name="personcode">身份证号码</param>
        /// <param name="bankname">账户名称</param>
        /// <param name="bankcode">账户号码</param>
        /// <param name="id">单据Id</param>
        /// <returns></returns>
        public bool GetIsTure(string personcode, string bankname, string bankcode, int id, int bankID)
        {

            return dal.GetIsTure(personcode, bankname, bankcode, id, bankID);
        }
        /// <summary>
        /// 获取table数据
        /// </summary>
        /// <param name="clounms">列名</param>
        /// <param name="tabname">表名</param>
        /// <param name="wheres">条件</param>
        /// <returns></returns>
        public DataTable GetDate(string clounms, string tabname, string wheres)
        {
            return dal.GetDate(clounms, tabname, wheres);
        }
        /// <summary>
        /// 获取table数据
        /// </summary>
        /// <param name="clounms">列名</param>
        /// <param name="tabname">表名</param>
        /// <param name="wheres">条件</param>
        /// <returns></returns>
        public DataTable GetDate(string clounms, string tabname, string wheres, SqlTransaction Tran)
        {
            return dal.GetDate(clounms, tabname, wheres, Tran);
        }
        /// <summary>
        /// 修改第一收款账号
        /// </summary>       
        /// <returns></returns>
        public int Upisno()
        {
            return dal.Upisno();
        }
        
        /// <summary>
        /// 修改结算表状态
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Up_PayLog(string sql) 
        {
            return dal.Up_PayLog(sql);
        }
        /// <summary>
        /// 清算手续费
        /// </summary>
        /// <param name="receiptNo">清算的订单编号，payment表的vdef4字段</param>
        /// <returns></returns>
        public DataTable GetdataTable_sxf(string receiptNo)
        {
            return dal.GetdataTable_sxf(receiptNo);
        }

        /// <summary>
        /// 清算手续费后，修改支付记录手续费已清算标记
        /// </summary>
        /// <param name="receiptNo">清算的订单编号，payment表的vdef4字段</param>
        /// <returns></returns>
        public int Updatejsxf_no(string receiptNo)
        {
            return dal.Updatejsxf_no(receiptNo);
        }

    }
}

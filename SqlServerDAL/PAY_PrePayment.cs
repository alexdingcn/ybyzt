//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/5/20 12:44:04
//
// 功能描述: 
//
// 修改标识: 
// 修改描述: 
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using DBUtility;
using System.Collections;
using System.Collections.Generic;

namespace Hi.SQLServerDAL
{
    /// <summary>
    /// 数据访问类 PAY_PrePayment
    /// </summary>
    public partial class PAY_PrePayment
    {
        /// <summary>
        /// 根据经销商sum所有金额字段（在企业钱包表）
        /// </summary>
        public decimal sums(int disid, int compid)
        {
            //string sql = string.Format(@"select SUM(isnull(price,0))  as sum from PAY_PrePayment where DisID={0} and compid={1} and AuditState=2 and IsEnabled=1 and ( Start=1  or (Start=3 and PreType=5))", disid, compid);
            string sql = string.Format(@"select (case when SUM(isnull(price,0)) IS NULL  then 0 else SUM(isnull(price,0)) end )  as sum from PAY_PrePayment where DisID={0} and compid={1} and AuditState=2 and IsEnabled=1 and Start=1 and PreType in (1,2,3,4,5)", disid, compid);
            decimal price = 0;
            try
            {
                price = Convert.ToDecimal(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, sql));
            }
            catch
            {

            }
            return price;
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update pay_prepayment set start = 1 where ID =@prepayid and IsEnabled=1 and isnull(dr,0)=0");
            SqlParameter[] parameters = { new SqlParameter("@prepayid", SqlDbType.Int) };
            parameters[0].Value = prepayid;

            SqlCommand cmd = new SqlCommand(strSql.ToString(), sqlconn, sqltans);
            cmd.CommandType = CommandType.Text;

            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    if (parameter.SqlDbType == SqlDbType.DateTime)
                    {
                        if (parameter.Value == DBNull.Value)
                        {
                            parameter.Value = DBNull.Value;
                        }

                    }
                    cmd.Parameters.Add(parameter);
                }
            }
            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());

            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;
        }

        /// <summary>
        /// 根据相应的ID获取，相对应的数据集合 ---销售订单结算方法
        /// </summary>
        /// <param name="Id">表ID</param>
        /// <returns></returns>
        public DataTable GetdataTable(int type, string strwhere, int typeTable)
        {
            string strSql = string.Empty;
            switch (type)
            {
                case 1:
                    strSql = string.Format(@"select dis_order.ID,PAY_Payment.Type, Dis_Order.OState, ostate,dis_order.CompID,dis_order.DisID,PAY_Payment.GUID, PAY_Payment.vdef4 as ReceiptNo,PAY_Payment.PayPrice,dis_order.Remark,PAY_Payment.ID as paymentID,
PAY_Payment.vdef5,PAY_Payment.Channel,PAY_Payment.jsxf_no,PAY_Payment.vdef6 from dis_order 
                                        join PAY_Payment  on dis_order.id=pay_payment.OrderID
                                        where PAY_Payment.vdef3='{0}'   and pay_payment.IsAudit=1   and pay_payment.PrintNum=0 and dis_order.dr=0 {1}", typeTable, strwhere);//and dis_order.ID ={0}  and dis_order.vdef9 in (0,2)
                    break;
                case 2://结算接口，银行信息--已经销商为核心  经销商ID
                    strSql = string.Format(@"select '' as payName,'' as PayCode,  PAY_PaymentAccountdtl.DisID,
PAY_PaymentBank.type,
 pay_paymentbank.BankID,pay_paymentbank.AccountName,pay_paymentbank.bankcode
 ,pay_paymentbank.bankAddress,pay_paymentbank.bankprivate,pay_paymentbank.bankcity
 from PAY_PaymentAccountdtl 
 join PAY_PaymentBank on PAY_PaymentAccountdtl.PBID=PAY_PaymentBank.ID
 where 1=1  and PAY_PaymentBank.Start=1  and PAY_PaymentBank.dr=0   {0}", strwhere);//PAY_PaymentAccountdtl.DisID={0}
                    break;
                case 3://结算接口，银行信息--已核心企业为主，  核心企业ID
                    strSql = string.Format(@"select top 1  '001520' as  OrgCode,'' as payName,'' as PayCode, pay_paymentbank.Isno
,PAY_PaymentBank.type,
pay_paymentbank.BankID,pay_paymentbank.AccountName,pay_paymentbank.bankcode
,pay_paymentbank.bankAddress,pay_paymentbank.bankprivate,pay_paymentbank.bankcity
from  PAY_PaymentBank 
where 1=1  and PAY_PaymentBank.Start=1  and PAY_PaymentBank.dr=0    {0}  order by pay_paymentbank.isno desc ", strwhere);//PAY_PaymentAccount.CompID={0}
                    break;
            }
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql).Tables[0];

        }

        /// <summary>
        /// 根据相应的ID获取，相对应的数据集合 ---转账汇款结算方法
        /// </summary>
        /// <param name="Id">表ID</param>
        /// <returns></returns>
        public DataTable GetdataTable_pre(int type, string strwhere)
        {
            string strSql = string.Empty;
            switch (type)
            {
                case 1://PAY_PrePayment.ID as ReceiptNo
                    strSql = string.Format(@"select  PAY_PrePayment.ID, PAY_PrePayment.CompID,PAY_PrePayment.DisID,PAY_Payment.GUID,PAY_Payment.vdef4 as ReceiptNo,PAY_Payment.PayPrice,PAY_PrePayment.vdef1 as Remark ,PAY_Payment.ID as paymentID 
,PAY_Payment.vdef5,PAY_Payment.Channel,PAY_Payment.jsxf_no,PAY_Payment.vdef6
                            from PAY_PrePayment 
                            join PAY_Payment  on PAY_PrePayment.id=pay_payment.OrderID
                            where PAY_Payment.vdef3='2' and  PAY_PrePayment.Start=1 and pay_payment.IsAudit=1  and pay_payment.PrintNum=0 and (PAY_PrePayment.vdef2 is null or PAY_PrePayment.vdef2 ='')  and PAY_PrePayment.dr=0 {0}", strwhere);//and dis_order.ID ={0}
                    break;
                case 2://结算接口，银行信息--已经销商为核心  经销商ID
                    strSql = string.Format(@"select 
                                    '001520' as  OrgCode,'' as payName,'' as PayCode,  PAY_PaymentAccountdtl.DisID,PAY_PaymentBank.type,
                                     pay_paymentbank.BankID,pay_paymentbank.AccountName,pay_paymentbank.bankcode
                                     ,pay_paymentbank.bankAddress,pay_paymentbank.bankprivate,pay_paymentbank.bankcity
                                     from PAY_PaymentAccountdtl 
                                     join PAY_PaymentBank on PAY_PaymentAccountdtl.PBID=PAY_PaymentBank.ID                                    
                                     where 1=1 and PAY_PaymentBank.Start=1  and PAY_PaymentBank.dr=0   {0}", strwhere);//PAY_PaymentAccountdtl.DisID={0}
                    break;
                case 3://结算接口，银行信息--已核心企业为主，  核心企业ID
                    strSql = string.Format(@"select top 1   '001520' as  OrgCode,'' as payName,'' as PayCode, pay_paymentbank.Isno, 
                                             PAY_PaymentBank.type,
                                             pay_paymentbank.BankID,pay_paymentbank.AccountName,pay_paymentbank.bankcode
                                             ,pay_paymentbank.bankAddress,pay_paymentbank.bankprivate,pay_paymentbank.bankcity
                                             from  PAY_PaymentBank 
                                             where 1=1  and PAY_PaymentBank.Start=1 and PAY_PaymentBank.dr=0    {0}  order by pay_paymentbank.isno desc ", strwhere);//PAY_PaymentAccount.CompID={0}
                    break;
            }
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql).Tables[0];

        }

        /// <summary>
        /// 清算手续费
        /// </summary>
        /// <param name="receiptNo">清算的订单编号，payment表的vdef4字段</param>
        /// <returns></returns>
        public DataTable GetdataTable_sxf(string receiptNo)
        {

            string strSql = string.Format(@"select PAY_Payment.vdef4  as ReceiptNo,SUM(ISNULL(convert(decimal(18,2),vdef5),0)) price_sumsxf from PAY_Payment 
where pay_payment.IsAudit=1 and pay_payment.vdef4='{0}' and pay_payment.jsxf_no=0 group by PAY_Payment.vdef4  ", receiptNo);//PAY_PaymentAccount.CompID={0}

            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql).Tables[0];
        }

        /// <summary>
        /// 清算手续费后，修改支付记录手续费已清算标记
        /// </summary>
        /// <param name="receiptNo">清算的订单编号，payment表的vdef4字段</param>
        /// <returns></returns>
        public int Updatejsxf_no(string receiptNo)
        {
            string strSql = string.Format(@"update pay_payment set jsxf_no=1 where  IsAudit=1 and  vdef4='{0}'", receiptNo);
            return Convert.ToInt32(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql));
        }

        /// <summary>
        /// 判断guid是否重复
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string Number_repeat(string guid)
        {
            string StrGuid = string.Empty;
            string Sql = string.Format(@"select COUNT(1) as num from PAY_Payment where guid='{0}'", guid);
            int num = Convert.ToInt32(SqlHelper.Query(SqlHelper.LocalSqlServer, Sql).Tables[0].Rows[0][0]);

            if (num > 0)

                return Number_repeat(Guid.NewGuid().ToString().Replace("-", ""));

            else
                return guid;


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

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [PAY_PrePayment](");
            strSql.Append("[CompID],[DisID],[OrderID],[Start],[PreType],[price],[CreatDate],[OldId],[CrateUser],[AuditState],[AuditUser],[IsEnabled],[AuditDate],[ts],[dr],[modifyuser],[vdef1],[vdef2],[vdef3],[vdef4],[vdef5],[Paytime])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@OrderID,@Start,@PreType,@price,@CreatDate,@OldId,@CrateUser,@AuditState,@AuditUser,@IsEnabled,@AuditDate,@ts,@dr,@modifyuser,@vdef1,@vdef2,@vdef3,@vdef4,@vdef5,@Paytime)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@Start", SqlDbType.Int),
                    new SqlParameter("@PreType", SqlDbType.Int),
                    new SqlParameter("@price", SqlDbType.Decimal),
                    new SqlParameter("@CreatDate", SqlDbType.DateTime),
                    new SqlParameter("@OldId", SqlDbType.BigInt),
                    new SqlParameter("@CrateUser", SqlDbType.Int),
                    new SqlParameter("@AuditState", SqlDbType.Int),
                    new SqlParameter("@AuditUser", SqlDbType.Int),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@vdef1", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef4", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef5", SqlDbType.NVarChar,128),
                    new SqlParameter("@Paytime", SqlDbType.DateTime)
            };

            SqlCommand cmd = new SqlCommand(strSql.ToString(), sqlconn, sqltans);
            cmd.CommandType = CommandType.Text;

            parameters[0].Value = model.CompID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.OrderID;
            parameters[3].Value = model.Start;
            parameters[4].Value = model.PreType;
            parameters[5].Value = model.price;
            parameters[6].Value = model.CreatDate;
            parameters[7].Value = model.OldId;
            parameters[8].Value = model.CrateUser;
            parameters[9].Value = model.AuditState;
            parameters[10].Value = model.AuditUser;
            parameters[11].Value = model.IsEnabled;

            if (model.AuditDate != DateTime.MinValue)
                parameters[12].Value = model.AuditDate;
            else
                parameters[12].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[13].Value = model.ts;
            else
                parameters[13].Value = DBNull.Value;

            parameters[14].Value = model.dr;
            parameters[15].Value = model.modifyuser;

            if (model.vdef1 != null)
                parameters[16].Value = model.vdef1;
            else
                parameters[16].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[17].Value = model.vdef2;
            else
                parameters[17].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[18].Value = model.vdef3;
            else
                parameters[18].Value = DBNull.Value;


            if (model.vdef4 != null)
                parameters[19].Value = model.vdef4;
            else
                parameters[19].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[20].Value = model.vdef5;
            else
                parameters[20].Value = DBNull.Value;

            parameters[21].Value = model.Paytime;

            //SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());
            int rowsAffected = SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;
        }

        /// <summary>
        /// 创建人：耿国行
        /// 创建时间：2015-06-16
        ///重新分页方法
        /// </summary>
        public DataTable GetList_ggh(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[Order_view]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据订单id得到付款清单
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public DataTable GetPay(int orderID)
        {
            string strSql = string.Format(@"select * from (select CompID,DisID,price*-1 as PayPrice,'企业钱包支付' as PayType,Paytime,'0' as sxf from PAY_PrePayment 
            where PreType=5 and Start=1 and ISNULL(dr,0)=0 and OrderID={0} 
            union all
            select CompID,DisID,price as PayPrice,'收款补录-'+vdef3 as PayType,Paytime,'0' as sxf from PAY_PrePayment 
            where PreType=7 and Start=1 and ISNULL(dr,0)=0 and OrderID={0} 
            union all
            select o.CompID as CompID,pay.DisID as DisID,pay.PayPrice as PayPrice,case when verifystatus=0 then '网银支付' else '快捷支付' end as PayType,pay.CreateDate as Paytime,pay.vdef5 as sxf from PAY_Payment pay
            left join DIS_Order o on pay.OrderID=o.ID
            where pay.vdef3=1 and pay.IsAudit=1 and ISNULL(pay.dr,0)=0 and OrderID={0}
 union all
            select o.CompID as CompID,pay.DisID as DisID,pay.PayPrice as PayPrice, '线下支付'  as PayType,pay.PayDate as Paytime,pay.vdef5 as sxf from PAY_Payment pay
            left join DIS_Order o on pay.OrderID=o.ID
            where pay.vdef3=5 and pay.IsAudit=1 and ISNULL(pay.dr,0)=0 and OrderID={0}  
            union all
            select CompID,DisID,AclAmt as PayPrice,'融资账户余额支付' as PayType,ts as Paytime,'0' as sxf from PAY_Financing
            where ISNULL(dr,0)=0 and Type=3 and [State]=1 and OrderID={0}) as PayLog order by Paytime", orderID);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql).Tables[0];
        }

        /// <summary>
        /// 根据订单Id获取订单的支付明细（包括收款账号）
        /// </summary>
        /// <param name="ordid">订单id</param>
        /// <returns></returns>
        public DataTable GetPayedItem(int ordid) 
        {
            string sql = string.Format(@"select * from (select CompID,DisID,price*-1 as PayPrice,'企业钱包支付' as PayType,Paytime,'0.00' as sxf 
, '' as payName ,'' as  paycode,case when PAY_PrePayment.[guid] is null then 'MYKJ'+CONVERT(varchar,id)else PAY_PrePayment.[guid] end as guids, PAY_PrePayment.id as paymentID
,PreType,Start as status,'1' as vdef9
from PAY_PrePayment 
            where PreType=5 and Start=1 and ISNULL(dr,0)=0 and OrderID={0} 
            union all

            select CompID,DisID,price as PayPrice,'收款补录-'+vdef3 as PayType,Paytime,'0' as sxf,
             '' as payName ,'' as  paycode,case when PAY_PrePayment.[guid] is null then 'MYKJ'+CONVERT(varchar,id) else PAY_PrePayment.[guid] end as guids, PAY_PrePayment.id as paymentID
,PreType,Start as status ,'1' as vdef9
             from PAY_PrePayment 
            where PreType=7 and Start=1 and ISNULL(dr,0)=0 and OrderID={0} 
            union all
            select o.CompID as CompID,pay.DisID as DisID,pay.PayPrice as PayPrice,            
            case 
            when Channel=1 then '快捷支付' 
           when Channel=2 then '银联支付' 
            when Channel=3 then '网银支付' 
            when Channel=4 then 'B2B网银支付' 
            when Channel=5 then '线下支付' 
            when Channel=6 then '支付宝支付' 
            when Channel=7 then '微信支付' 
when Channel=8 then '信用卡支付' 
            end as PayType,
            pay.PayDate as Paytime,pay.vdef5 as sxf,
              AccountName as payName ,bankcode as  paycode,pay.guid as guids,
 pay.id as paymentID
,case when pay.vdef3 ='1' then '1' else '4' end PreType,pay.IsAudit as status,pay.vdef9
             from PAY_Payment pay
            left join DIS_Order o on pay.OrderID=o.ID
            left join PAY_PayLog on PAY_PayLog.number=pay.guid and PAY_PayLog.Start=2000
            where pay.vdef3=1  and ( pay.IsAudit in (1,3) or (pay.IsAudit=2 and pay.Channel=5)  ) and ISNULL(pay.dr,0)=0 and pay.OrderID={0}             
            
            ) as PayLog order by Paytime desc", ordid);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        
        }


        /// <summary>
        /// 结算账户管理用，（为了避免核心企业下面的经销商重复关联多个银行卡）
        /// </summary>
        /// <param name="CompID"></param>
        /// <returns></returns>
        public string GetDisIDBYCompID(int CompID)
        {
            string strSql = string.Format(@"SELECT  STUFF(( 
select  ',' + CONVERT(VARCHAR(10), c.DisID) from  PAY_PaymentBank b 
join PAY_PaymentAccountdtl c on c.pbid=b.ID where b.CompID={0}
FOR
                XML PATH('')
              ), 1, 1, '') AS Str", CompID);
            return Convert.ToString(SqlHelper.Query(SqlHelper.LocalSqlServer, strSql).Tables[0].Rows[0][0]);
        }


        /// <summary>
        /// 结算账户管理用，（为了避免核心企业有多默认账户）
        /// </summary>
        /// <param name="CompID"></param>
        /// <returns></returns>
        public int GetBankBYCompID(int CompID)
        {
            string strSql = string.Format(@"
select COUNT(1) as num  from PAY_PaymentBank  where CompID ={0} and isno=1 and dr=0 ", CompID);
            return Convert.ToInt32(SqlHelper.Query(SqlHelper.LocalSqlServer, strSql).Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 修改第一收款账号
        /// </summary>
        /// <param name="CompID"></param>
        /// <returns></returns>
        public int UpisnoBYCompID(int CompID)
        {
            string strSql = string.Format(@"update pay_paymentbank set Isno=0 where  CompID ={0} and dr=0 ", CompID);
            return Convert.ToInt32(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql));
        }


        /// <summary>
        /// 获取管理员手机号码
        /// </summary>
        /// <param name="CompID"></param>
        /// <returns></returns>
        public string GetPhoneBYCompID(int CompID)
        {
            string strSql = string.Format(@"select top 1 sys_users.Phone from SYS_CompUser  join  SYS_Users   on SYS_CompUser.UserID=sys_users.id
where SYS_CompUser.CompID={0}", CompID);
            return Convert.ToString(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql));
        }

        /// <summary>
        /// 获取银行名称
        /// </summary>
        /// <param name="CompID"></param>
        /// <returns></returns>
        public string GetBankNameBYbankID(string bankID)
        {
            string strSql = string.Format(@"select bankname from PAY_BankInfo  where bankcode='{0}'", bankID);
            return Convert.ToString(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql));
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
            bool fal = false;
            string strSql = string.Format(@"select COUNT(1) as num from PAY_PaymentBank where dr=0 and  vdef3='{0}' and AccountName='{1}' and bankcode='{2}' and ID={3} and BankID={4}", personcode, bankname, bankcode, id, bankID);
            int num = Convert.ToInt32(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql));
            if (num > 0)
            {
                fal = true;
            }

            return fal;
        }
        /// <summary>
        /// 获取table数据
        /// </summary>
        /// <param name="clounms">列名</param>
        /// <param name="tabname">表名</param>
        /// <param name="wheres">条件</param>
        /// <returns></returns>
        public DataTable GetDate(string clounms,string tabname,string wheres) 
        {
            string strSql = string.Format(@"select {0} from {1} where {2}",clounms,tabname,wheres);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql).Tables[0];
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
            string strSql = string.Format(@"select {0} from {1} where {2}", clounms, tabname, wheres);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql, Tran).Tables[0];
        }

        /// <summary>
        /// 修改第一收款账号
        /// </summary>       
        /// <returns></returns>
        public int Upisno()
        {
            string strSql = string.Format(@"update SYS_PaymentBank set Isno=0 ");
            return Convert.ToInt32(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql));
        }

        /// <summary>
        /// 修改结算表状态
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Up_PayLog(string sql) 
        {
          
            return Convert.ToInt32(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, sql));
        }

    }
}

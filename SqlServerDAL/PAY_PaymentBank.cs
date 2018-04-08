//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/5/19 13:10:06
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
    /// 数据访问类 PAY_PaymentBank
    /// </summary>
    public partial class PAY_PaymentBank
    {
        /// <summary>
        /// 根据收款账号表ID获取收款银行表信息
        /// PaymentaccountID  收款账号表主键ID
        /// </summary>
        public DataTable GetBankBYacountID(int  PaymentaccountID)
        {
            string strSql = string.Format(@"select *  from PAY_PaymentBank where dr=0 and paymentAccountID={0}",PaymentaccountID);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql).Tables[0];
            //return GetList(GetDataSet(strSql));
        }

       
    }
}

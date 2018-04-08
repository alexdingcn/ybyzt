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
    /// 数据访问类 PAY_PaymentAccountdtl
    /// </summary>
    public partial class PAY_PaymentAccountdtl
    {
        /// <summary>
        /// 根据收款银行表ID获取关联经销商表信息
        /// PbID  收款银行卡表主键ID
        /// </summary>
        public DataTable GetDisBYpbID(int PbID)
        {
            string strSql = string.Format(@"select * from PAY_PaymentAccountdtl join BD_Distributor  on
                                PAY_PaymentAccountdtl.DisID=BD_Distributor.ID where PAY_PaymentAccountdtl.PBID={0}", PbID);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql).Tables[0];

        }

        /// <summary>
        /// 根据收款银行表ID,删除和他管理的经销商信息
        /// PbID  收款银行卡表主键ID
        /// </summary>
        public bool DeldtlBYpbID(int pbID) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from [PAY_PaymentAccountdtl] ");
            strSql.Append(" where PBID=@pbID");
            SqlParameter[] parameters = {
                    new SqlParameter("@pbID", SqlDbType.Int)};
            parameters[0].Value = pbID;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) >= 0;
        }
    }
}

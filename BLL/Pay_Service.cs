//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2017 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2017/3/1 15:15:23
//
// 功能描述: 
//
// 修改标识: 
// 修改描述: 
//------------------------------------------------------------------------------

using System;
using System.Data;
using Hi.Model;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Data.SqlClient;

namespace Hi.BLL
{
    /// <summary>
    /// 业务逻辑类 Pay_Service
    /// </summary>
    public partial class Pay_Service
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
    }
}

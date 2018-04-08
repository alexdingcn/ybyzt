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
using Hi.Model;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Hi.BLL
{
    /// <summary>
    /// 业务逻辑类 PAY_PaymentBank
    /// </summary>
    public partial class PAY_PaymentBank
    {
        /// <summary>
        /// 根据收款账号表ID获取收款银行表信息
        /// PaymentaccountID  收款账号表主键ID
        /// </summary>
        public DataTable GetBankBYacountID(int PaymentaccountID)
        {
            return dal.GetBankBYacountID(PaymentaccountID);
        }
    }
}

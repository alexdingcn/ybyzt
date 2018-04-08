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
    /// 业务逻辑类 PAY_PaymentAccountdtl
    /// </summary>
    public partial class PAY_PaymentAccountdtl
    {
        /// <summary>
        /// 根据收款银行表ID获取关联经销商表信息
        /// PbID  收款银行卡表主键ID
        /// </summary>
        public DataTable GetDisBYpbID(int PbID)
        {
            return dal.GetDisBYpbID(PbID);
        }

          /// <summary>
        /// 根据收款银行表ID,删除和他管理的经销商信息
        /// PbID  收款银行卡表主键ID
        /// </summary>
        public bool DeldtlBYpbID(int pbID)
        {
            return dal.DeldtlBYpbID(pbID);
        }
    }
}

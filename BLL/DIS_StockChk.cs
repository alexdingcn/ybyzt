//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2017 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2017/2/6 13:03:24
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
    /// 业务逻辑类 DIS_StockChk
    /// </summary>
    public partial class DIS_StockChk
    {
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_StockChk model, SqlTransaction tran)
        {
            return dal.Update(model, tran);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID, SqlTransaction tran)
        {
            return dal.Delete(ID, tran);
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        public bool Delete(List<int> l, SqlTransaction tran)
        {
            bool returns = true;
            foreach (int ID in l)
                returns = dal.Delete(ID, tran);

            return returns;
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_StockChk model, SqlTransaction tran)
        {
            return dal.Add(model, tran);
        }
    }
}

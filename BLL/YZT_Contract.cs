//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2017 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2017/12/23 16:31:42
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
    /// 业务逻辑类 YZT_Contract
    /// </summary>
    public partial class YZT_Contract
    {


        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_Contract model, SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }

       

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_Contract model, SqlTransaction Tran)
        {
            return dal.Update(model,Tran);
        }



        #endregion
        public DataTable getDataTable(int pageSize, int pageIndex, string strWhere, out int pageCount, out int Counts)
        {
            return dal.getDataTable(pageSize, pageIndex, strWhere, out pageCount, out Counts);
        }


    }
}

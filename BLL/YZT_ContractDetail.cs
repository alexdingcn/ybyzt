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
    /// 业务逻辑类 YZT_ContractDetail
    /// </summary>
    public partial class YZT_ContractDetail
    {

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_ContractDetail model, SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }

      

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_ContractDetail model, SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }


        /// <summary>
        /// 删除多条数据
        /// </summary>
        public void Delete(List<int> l, SqlTransaction Tran)
        {
            foreach (int ID in l)
                dal.Delete(ID, Tran);
        }

        #endregion

        /// <summary>
        /// 获取详情页列表数据
        /// </summary>
        /// <param name="ContID">合同ID</param>
        /// <returns></returns>
        public DataTable getDataTable(string ContID) {
            return dal.getDataTable(ContID);
        }


    }
}

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
    /// 业务逻辑类 DIS_StockOrder
    /// </summary>
    public partial class DIS_StockOrder
    {
        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public DataTable GetListPage(int pageSize, int pageIndex, string where)
        {
            return dal.GetListPage(pageSize, pageIndex, where);
        }
        /// <summary>
        /// 执行sql语局获取数据
        /// </summary>
        public DataTable GetDataTable(string sql)
        {
            return dal.GetDataTable(sql); ;
        }
        /// <summary>
        /// 获取总行数
        /// </summary>
        /// <returns></returns>
        public int GetPageCount(string where)
        {
            return dal.GetPageCount(where);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_StockOrder model, SqlTransaction tran)
        {
            return dal.Update(model, tran);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_StockOrder model, SqlTransaction tran)
        {
            return dal.Add(model, tran);
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

    }
}

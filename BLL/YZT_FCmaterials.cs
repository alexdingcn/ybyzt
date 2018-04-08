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

namespace Hi.BLL
{
    /// <summary>
    /// 业务逻辑类 YZT_FCmaterials 
    /// </summary>
    public partial class YZT_FCmaterials
    {

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_FCmaterials model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_FCmaterials model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }

        /// <summary>
        /// 获取包含 代理商名称 编码 的  代理商首营资料 分页数据
        /// </summary>
        /// <param name="pageSize">一页几行</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="Compid">厂商ID</param>
        /// <param name="disName">厂商名称（可为空）</param>
        /// <param name="pageCount">一共几页</param>
        /// <param name="Counts">总行数</param>
        /// <returns></returns>
        public DataTable getDataTable(int pageSize, int pageIndex, string Compid, string disName, out int pageCount, out int Counts)
        {
            return dal.getDataTable(pageSize, pageIndex, Compid, disName, out pageCount, out Counts);
        }


        /// <summary>
        /// 获取包含  厂商名称 编码 的  厂商首营资料 分页数据
        /// </summary>
        /// <param name="pageSize">一页几行</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="disID">代理商ID</param>
        /// <param name="strWhere">其他 条件</param>
        /// <param name="pageCount">一共几页</param>
        /// <param name="Counts">总行数</param>
        /// <returns></returns>
        public DataTable getCompDataTable(int pageSize, int pageIndex, string disID, string strWhere, out int pageCount, out int Counts)
        {
            return dal.getCompDataTable(pageSize, pageIndex, disID, strWhere, out pageCount, out Counts);
        }


        /// <summary>
        /// 根据首营资料ID  获取  厂商首营资料
        /// </summary>
        /// <param name="fid">首营资料ID</param>
        /// <returns></returns>
        public DataTable getDataModel(string Disid, string strWhere)
        {
            return dal.getDataModel(Disid, strWhere);
        }

        /// <summary>
        /// 根据首营资料ID  获取  代理商首营资料 及其 代理商信息
        /// </summary>
        /// <param name="fid">首营资料ID</param>
        /// <returns></returns>
        public DataTable getDataModel(string fid)
        {
            return dal.getDataModel(fid);
        }


    }
}

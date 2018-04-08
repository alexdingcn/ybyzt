//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/10/21 11:18:05
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
    /// 业务逻辑类 BF_ZD_PROVINCE
    /// </summary>
    public class BF_ZD_PROVINCE
    {
        private readonly Hi.SQLServerDAL.BF_ZD_PROVINCE dal = new Hi.SQLServerDAL.BF_ZD_PROVINCE();
        public BF_ZD_PROVINCE()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BF_ZD_PROVINCE model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 增加多条数据
        /// </summary>
        public void Add(List<Hi.Model.BF_ZD_PROVINCE> l)
        {
            foreach (Hi.Model.BF_ZD_PROVINCE model in l)
                dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BF_ZD_PROVINCE model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        public void Update(List<Hi.Model.BF_ZD_PROVINCE> l)
        {
            foreach (Hi.Model.BF_ZD_PROVINCE model in l)
                dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int PROVID)
        {
            return dal.Delete(PROVID);
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        public void Delete(List<int> l)
        {
            foreach (int PROVID in l)
                dal.Delete(PROVID);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int PROVID)
        {
            return dal.Exists(PROVID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BF_ZD_PROVINCE GetModel(int PROVID)
        {
            return dal.GetModel(PROVID);
        }

        /// <summary>
        /// 得到数据总条数
        /// </summary>
        public int GetCount()
        {
            DataSet ds = dal.GetDataSet("count(*)", null, null);
            return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        }

        /// <summary>
        /// 获得泛型数据列表,不建议直接使用此方法,请根据业务逻辑重写
        /// </summary>
        public List<Hi.Model.BF_ZD_PROVINCE> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return dal.GetList(strWhat, strWhere, strOrderby) as List<Hi.Model.BF_ZD_PROVINCE>;
        }

        /// <summary>
        /// 分页获取泛型数据列表,不建议直接使用此方法,请根据业务逻辑重写
        /// </summary>
        public List<Hi.Model.BF_ZD_PROVINCE> GetList(int pageSize, int pageIndex, string fldSort, bool Sort, string strCondition, out int pageCount, out int Counts)
        {
            return dal.GetList(pageSize, pageIndex, fldSort, Sort, strCondition, out pageCount, out Counts) as List<Hi.Model.BF_ZD_PROVINCE>;
        }
        #endregion

        #region  扩展方法
        /// <summary>
        /// 获得全部泛型数据列表
        /// </summary>
        public List<Hi.Model.BF_ZD_PROVINCE> GetAllList()
        {
            return GetList(null, null, null);
        }
        #endregion
    }
}

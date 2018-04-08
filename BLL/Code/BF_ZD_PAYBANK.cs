//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/11/23 18:18:43
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
    /// 业务逻辑类 BF_ZD_PAYBANK
    /// </summary>
    public class BF_ZD_PAYBANK
    {
        private readonly Hi.SQLServerDAL.BF_ZD_PAYBANK dal = new Hi.SQLServerDAL.BF_ZD_PAYBANK();
        public BF_ZD_PAYBANK()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Add(Hi.Model.BF_ZD_PAYBANK model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 增加多条数据
        /// </summary>
        public void Add(List<Hi.Model.BF_ZD_PAYBANK> l)
        {
            foreach (Hi.Model.BF_ZD_PAYBANK model in l)
                dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BF_ZD_PAYBANK model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        public void Update(List<Hi.Model.BF_ZD_PAYBANK> l)
        {
            foreach (Hi.Model.BF_ZD_PAYBANK model in l)
                dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string FQHHO2)
        {
            return dal.Delete(FQHHO2);
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        public void Delete(List<string> l)
        {
            foreach (string FQHHO2 in l)
                dal.Delete(FQHHO2);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string FQHHO2)
        {
            return dal.Exists(FQHHO2);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BF_ZD_PAYBANK GetModel(string FQHHO2)
        {
            return dal.GetModel(FQHHO2);
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
        public List<Hi.Model.BF_ZD_PAYBANK> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return dal.GetList(strWhat, strWhere, strOrderby) as List<Hi.Model.BF_ZD_PAYBANK>;
        }

        /// <summary>
        /// 分页获取泛型数据列表,不建议直接使用此方法,请根据业务逻辑重写
        /// </summary>
        public List<Hi.Model.BF_ZD_PAYBANK> GetList(int pageSize, int pageIndex, string fldSort, bool Sort, string strCondition, out int pageCount, out int Counts,string fldname="")
        {
            return dal.GetList(pageSize, pageIndex, fldSort, Sort, strCondition, out pageCount, out Counts,fldname) as List<Hi.Model.BF_ZD_PAYBANK>;
        }
        #endregion

        #region  扩展方法
        /// <summary>
        /// 获得全部泛型数据列表
        /// </summary>
        public List<Hi.Model.BF_ZD_PAYBANK> GetAllList()
        {
            return GetList(null, null, null);
        }
        #endregion
    }
}

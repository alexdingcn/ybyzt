using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Hi.BLL
{
    public partial class BD_GoodsAttrs
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_GoodsAttrs model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
        /// <summary>
        /// 修改一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_GoodsAttrs model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }
         /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        /// <param name="strWhat">字段</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrderby">排序字段</param>
        /// <returns></returns>
        /// 
        public List<Hi.Model.BD_GoodsAttrs> GetList(string strWhat, string strWhere, string strOrderby, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, Tran) as List<Hi.Model.BD_GoodsAttrs>;
        }
        public DataTable GetAttrToAttrInfoDt(string strWhat, string strWhere, string strOrderby)
        {
            return dal.GetAttrToAttrInfoDt(strWhat, strWhere, strOrderby);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Hi.BLL
{
    public partial class DIS_OrderOutDetail
    {
        /// <summary>
        /// 增加一条数据,带事务
        /// </summary>
        public int Add(Hi.Model.DIS_OrderOutDetail model, SqlTransaction TranSaction = null)
        {
            return dal.Add(model, TranSaction);
        }

        /// <summary>
        /// 修改一条数据,带事务
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TranSaction"></param>
        /// <returns></returns>
        public int Update(Hi.Model.DIS_OrderOutDetail model, SqlTransaction TranSaction)
        {
            return dal.Update(model, TranSaction);
        }

         /// <summary>
        /// 查询订单商品明细
        /// </summary>
        /// <param name="strWhat"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetOrderOutDe(string strWhat, string strWhere)
        {
            return dal.GetOrderOutDe(strWhat, strWhere);
        }
    }
}

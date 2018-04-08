using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hi.BLL
{
    public partial class BD_GoodsLabels
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_GoodsLabels model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
        /// <summary>
        /// 删除GoodsLabels
        /// </summary>
        /// <returns></returns>
        public int Delete(int goodsId, int compId, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Delete(goodsId, compId, Tran);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public string Adds(Hi.Model.BD_GoodsLabels model)
        {
            return dal.Adds(model);
        }
    }
}

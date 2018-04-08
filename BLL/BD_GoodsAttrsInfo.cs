using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Hi.BLL
{
    public partial class BD_GoodsAttrsInfo
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_GoodsAttrsInfo model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
        /// <summary>
        /// 修改一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_GoodsAttrsInfo model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }

        public List<Hi.Model.BD_GoodsAttrsInfo> GetList(string strWhat, string strWhere, string strOrderby, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, Tran) as List<Hi.Model.BD_GoodsAttrsInfo>;
        }
    }
}

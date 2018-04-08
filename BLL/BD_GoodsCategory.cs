using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Hi.BLL
{
    public partial class BD_GoodsCategory
    {
        public List<Hi.Model.BD_GoodsCategory> GetList(string strWhat, string strWhere, string strOrderby, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, Tran) as List<Hi.Model.BD_GoodsCategory>;
        }

        public bool Update(Hi.Model.BD_GoodsCategory GoodsType,SqlTransaction TranSaction)
        {
            return dal.Update(GoodsType, TranSaction);
        }

        public int Add(Hi.Model.BD_GoodsCategory GoodsType, SqlTransaction TranSaction)
        {
            return dal.Add(GoodsType, TranSaction);
        }
    }
}

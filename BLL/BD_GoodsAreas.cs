using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Hi.BLL
{
    public partial class BD_GoodsAreas
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_GoodsAreas model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
        public List<Hi.Model.BD_GoodsAreas> GetList(string strWhat, string strWhere, string strOrderby, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, Tran) as List<Hi.Model.BD_GoodsAreas>;
        }
    }
}

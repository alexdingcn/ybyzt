using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
namespace Hi.BLL
{
    public partial class BD_PromotionDetail2
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_PromotionDetail2 model, SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID, SqlTransaction Tran)
        {
            return dal.Delete(ID, Tran);
        }
        public List<Hi.Model.BD_PromotionDetail2> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, Tran) as List<Hi.Model.BD_PromotionDetail2>;
        }

    }
}

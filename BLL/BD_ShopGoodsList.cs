using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
namespace Hi.BLL
{
    public partial class BD_ShopGoodsList
    {
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Tran"></param>
        /// <returns></returns>
        public int Add(Hi.Model.BD_ShopGoodsList model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="compId"></param>
        /// <param name="Tran"></param>
        /// <returns></returns>
        public bool Delete(int compId, string title, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Delete(compId, title, Tran);
        }
        public List<Hi.Model.BD_ShopGoodsList> GetList(string strWhat, string strWhere, string strOrderby, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, Tran) as List<Hi.Model.BD_ShopGoodsList>;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_ShopGoodsList GetModel(int ID, SqlTransaction Tran)
        {
            return dal.GetModel(ID, Tran);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Tran"></param>
        /// <returns></returns>
        public bool Update(Hi.Model.BD_ShopGoodsList model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }
    }
}

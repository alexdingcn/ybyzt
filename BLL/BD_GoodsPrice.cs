using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Hi.BLL
{
    public partial class BD_GoodsPrice
    {
        /// <summary>
        /// 批量插入GoodsPrice表
        /// </summary>
        /// <param name="ll"></param>
        public bool InserGoodsPrice(List<Hi.Model.BD_GoodsPrice> ll)
        {
            return dal.InserGoodsPrice(ll);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_GoodsPrice model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_GoodsPrice model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
        public List<Hi.Model.BD_GoodsPrice> GetList(string strWhat, string strWhere, string strOrderby, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, Tran) as List<Hi.Model.BD_GoodsPrice>;
        }

        /// <summary>
        /// 增加多条数据
        /// </summary>
        public void Add(List<Hi.Model.BD_GoodsPrice> l, System.Data.SqlClient.SqlTransaction Tran)
        {
            foreach (Hi.Model.BD_GoodsPrice model in l)
                dal.Add(model, Tran);
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        public void Update(List<Hi.Model.BD_GoodsPrice> l, System.Data.SqlClient.SqlTransaction Tran)
        {
            foreach (Hi.Model.BD_GoodsPrice model in l)
                dal.Update(model, Tran);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_GoodsPrice GetModel(int ID, SqlTransaction Tran)
        {
            return dal.GetModel(ID, Tran);
        }
        /// <summary>
        /// 删除多条数据
        /// </summary>
        public bool Delete(int compid, int disid, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Delete(compid, disid, Tran);
        }
    }
}

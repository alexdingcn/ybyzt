using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;


namespace Hi.BLL
{
    public partial class BD_DisPrice
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_DisPrice model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_DisPrice GetModel(int ID, SqlTransaction Tran)
        {
            return dal.GetModel(ID, Tran);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_DisPrice model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }
        /// <summary>
        /// 更新多条数据
        /// </summary>
        public void Update(List<Hi.Model.BD_DisPrice> l, System.Data.SqlClient.SqlTransaction Tran)
        {
            foreach (Hi.Model.BD_DisPrice model in l)
                dal.Update(model, Tran);
        }
        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public List<Hi.Model.BD_DisPrice> GetList(string strWhat, string strWhere, string strOrderby, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, Tran) as List<Hi.Model.BD_DisPrice>;
        }
        /// <summary>
        /// 删除多条数据
        /// </summary>
        public bool Delete(int ID, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Delete(ID, Tran);
        }
    }
}

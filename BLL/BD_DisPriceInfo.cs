using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
namespace Hi.BLL
{
    public partial class BD_DisPriceInfo
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_DisPriceInfo model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }

        /// <summary>
        /// 增加多条数据
        /// </summary>
        public void Add(List<Hi.Model.BD_DisPriceInfo> l, System.Data.SqlClient.SqlTransaction Tran)
        {
            foreach (Hi.Model.BD_DisPriceInfo model in l)
                dal.Add(model, Tran);
        }
        /// <summary>
        /// 删除多条数据
        /// </summary>
        public bool Delete(int ID, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Delete(ID, Tran);
        }

        /// <summary>
        /// 更新多条数据
        /// </summary>
        public void Update(List<Hi.Model.BD_DisPriceInfo> l, System.Data.SqlClient.SqlTransaction Tran)
        {
            foreach (Hi.Model.BD_DisPriceInfo model in l)
                dal.Update(model,Tran);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_DisPriceInfo GetModel(int ID, SqlTransaction Tran)
        {
            return dal.GetModel(ID, Tran);
        }
    }
}

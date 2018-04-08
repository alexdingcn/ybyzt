using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hi.BLL
{
    public partial class BD_ShopImageList
    {
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_ShopImageList model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Tran"></param>
        /// <returns></returns>
        public int Add(Hi.Model.BD_ShopImageList model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="compId"></param>
        /// <param name="Tran"></param>
        /// <returns></returns>
        public bool Delete(int compId, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Delete(compId, Tran);
        }
    }
}

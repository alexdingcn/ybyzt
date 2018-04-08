using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hi.BLL
{
    /// <summary>
    /// 业务逻辑类 YZT_Storage
    /// </summary>
    public partial class YZT_Storage
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_Storage model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_Storage model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Hi.BLL
{
    /// <summary>
    /// 业务逻辑类 YZT_FCArea 
    /// </summary>
    public partial class YZT_FCArea
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_FCArea model, SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_FCArea model, SqlTransaction Tran)
        {
            return dal.Update(model, Tran);
        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool CMDelete(int ID, SqlTransaction Tran)
        {
            return dal.CMDelete(ID, Tran);
        }
    }
}

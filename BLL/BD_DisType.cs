using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hi.BLL
{
    /// <summary>
    /// 业务逻辑类 BD_DisType
    /// </summary>
    public partial class BD_DisType
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_DisType model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model,Tran);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_DisType model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model,Tran);
        }
    }
}

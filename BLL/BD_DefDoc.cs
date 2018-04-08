using System;
using System.Collections.Generic;
using System.Text;

namespace Hi.BLL
{
    public partial class BD_DefDoc
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_DefDoc model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model, Tran);
        }
    }
}

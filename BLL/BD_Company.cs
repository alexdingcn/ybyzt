using System;
using System.Collections.Generic;
using System.Text;

namespace Hi.BLL
{
    public partial class BD_Company
    {
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Company model,System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Update(model,Tran);
        }

        public int Add(Hi.Model.BD_Company model, System.Data.SqlClient.SqlTransaction Tran)
        {
            return dal.Add(model,Tran);
        }
    }
}

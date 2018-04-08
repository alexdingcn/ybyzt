using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Hi.BLL
{
    public partial class DIS_OrderReturn
    {
        public Hi.Model.DIS_OrderReturn GetModel(string orderid)
        {
            return dal.GetModel(orderid);
        }

        /// <summary>
        /// 增加一条数据,带事务
        /// </summary>
        public int Add(SqlConnection sqlconn, Hi.Model.DIS_OrderReturn model, SqlTransaction sqltans)
        {
            return dal.Add(sqlconn, model, sqltans);
        }

        /// <summary>
        /// 更新一条数据,带事务
        /// </summary>
        public int Update(SqlConnection sqlconn, Hi.Model.DIS_OrderReturn model, SqlTransaction sqltans)
        {
            return dal.Update(sqlconn, model, sqltans);
        }
    }
}

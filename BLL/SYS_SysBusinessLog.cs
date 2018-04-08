using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Hi.BLL
{
    public partial class SYS_SysBusinessLog
    {
        /// <summary>
        /// 增加一条数据,带事务
        /// </summary>
        public int Add(Hi.Model.SYS_SysBusinessLog model, SqlTransaction TranSaction = null)
        {
            return dal.Add(model, TranSaction);
        }

        /// <summary>
        /// 更新一条数据,带事务
        /// </summary>
        public bool Update(Hi.Model.SYS_SysBusinessLog model, SqlTransaction TranSaction = null)
        {
            return dal.Update(model, TranSaction);
        }

        public List<Hi.Model.SYS_SysBusinessLog> GetList(string strWhat, string strWhere, string strOrderby,SqlTransaction TranSaction = null)
        {
            return dal.GetList(strWhat, strWhere, strOrderby, TranSaction) as List<Hi.Model.SYS_SysBusinessLog>;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using System.Data.SqlClient;
using System.Data;


namespace Hi.SQLServerDAL
{
    public partial class BD_DefDoc_B
    {
        /// <summary>
        /// 删除单位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Updates(string user, string id, string compid)
        {
            string sql = string.Format(@"Update BD_DefDoc_B set dr=1,modifyuser={0},ts=getdate() where ID={1} and compid={2}", user, id, compid);
            int dt = SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, sql);
            return dt;
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_DefDoc_B model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_DefDoc_B](");
            strSql.Append("[CompID],[DefID],[AtName],[AtVal],[SortIndex],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DefID,@AtName,@AtVal,@SortIndex,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DefID", SqlDbType.Int),
                    new SqlParameter("@AtName", SqlDbType.VarChar,50),
                    new SqlParameter("@AtVal", SqlDbType.VarChar,50),
                    new SqlParameter("@SortIndex", SqlDbType.VarChar,50),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.DefID;

            if (model.AtName != null)
                parameters[2].Value = model.AtName;
            else
                parameters[2].Value = DBNull.Value;


            if (model.AtVal != null)
                parameters[3].Value = model.AtVal;
            else
                parameters[3].Value = DBNull.Value;


            if (model.SortIndex != null)
                parameters[4].Value = model.SortIndex;
            else
                parameters[4].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[5].Value = model.ts;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.modifyuser;
            //  return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }
    }
}

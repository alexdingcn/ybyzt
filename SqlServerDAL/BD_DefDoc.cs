using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class BD_DefDoc
    {
        /// <summary>
        /// 增加一条数据 带有事物
        /// </summary>
        public int Add(Hi.Model.BD_DefDoc model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_DefDoc](");
            strSql.Append("[CompID],[AtCode],[AtName],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@AtCode,@AtName,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@AtCode", SqlDbType.VarChar,50),
                    new SqlParameter("@AtName", SqlDbType.VarChar,50),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;

            if (model.AtCode != null)
                parameters[1].Value = model.AtCode;
            else
                parameters[1].Value = DBNull.Value;


            if (model.AtName != null)
                parameters[2].Value = model.AtName;
            else
                parameters[2].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[3].Value = model.ts;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.modifyuser;
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

    }
}

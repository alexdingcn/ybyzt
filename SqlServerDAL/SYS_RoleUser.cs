using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class SYS_RoleUser
    {
        /// <summary>
        /// 增加一条数据 带有事务
        /// </summary>
        public int Add(Hi.Model.SYS_RoleUser model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [SYS_RoleUser](");
            strSql.Append("[FunType],[UserID],[RoleID],[IsEnabled],[CreateUser],[CreateDate],[ts])");
            strSql.Append(" values (");
            strSql.Append("@FunType,@UserID,@RoleID,@IsEnabled,@CreateUser,@CreateDate,@ts)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@FunType", SqlDbType.Int),
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@RoleID", SqlDbType.Int),
                    new SqlParameter("@IsEnabled", SqlDbType.Bit),
                    new SqlParameter("@CreateUser", SqlDbType.NVarChar,50),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime)
            };
            parameters[0].Value = model.FunType;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.RoleID;
            parameters[3].Value = model.IsEnabled;
            parameters[4].Value = model.CreateUser;
            parameters[5].Value = model.CreateDate;
            parameters[6].Value = model.ts;
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

    }
}

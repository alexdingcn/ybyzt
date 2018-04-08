using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class SYS_RoleSysFun
    {
        /// <summary>
        /// 增加一条数据 带有事物
        /// </summary>
        public int Add(Hi.Model.SYS_RoleSysFun model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [SYS_RoleSysFun](");
            strSql.Append("[CompID],[DisID],[RoleID],[FunCode],[FunName],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@RoleID,@FunCode,@FunName,@IsEnabled,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@RoleID", SqlDbType.Int),
                    new SqlParameter("@FunCode", SqlDbType.VarChar,50),
                    new SqlParameter("@FunName", SqlDbType.VarChar,50),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;

            parameters[1].Value = model.DisID;

            parameters[2].Value = model.RoleID;

            if (model.FunCode != null)
                parameters[3].Value = model.FunCode;
            else
                parameters[3].Value = DBNull.Value;

            if (model.FunName != null)
                parameters[4].Value = model.FunName;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.IsEnabled;
            parameters[6].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[7].Value = model.CreateDate;
            else
                parameters[7].Value = DBNull.Value;

            if (model.ts != DateTime.MinValue)
                parameters[8].Value = model.ts;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.modifyuser;
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

    }
}

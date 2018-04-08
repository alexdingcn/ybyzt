using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class SYS_AdminUser
    {
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.SYS_AdminUser GetModelByName(string LoginId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [SYS_AdminUser] ");
            strSql.Append(" where [LoginName]=@LoginName");
            SqlParameter[] parameters = {
                    new SqlParameter("@LoginName", SqlDbType.VarChar)};
            parameters[0].Value = LoginId;
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow r = ds.Tables[0].Rows[0];
                return GetModel(r);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 修改密码
        /// <param name="Id">用户ID</param>
        /// <param name="NewPassWord">新密码</param>
        /// </summary>
        public bool UpdatePassWord(string NewPassWord, string Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SYS_AdminUser set ");
            strSql.Append("LoginPwd=@UserPwd");
            strSql.Append(" where id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.VarChar),
                    new SqlParameter("@UserPwd", SqlDbType.VarChar),
            };
            parameters[0].Value = Id;
            parameters[1].Value = NewPassWord;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }
    }
}

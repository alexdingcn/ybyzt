using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class SYS_CompUser
    {
        /// <summary>
        /// 增加一条数据 带有事务
        /// </summary>
        public int Add(Hi.Model.SYS_CompUser model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [SYS_CompUser](");
            strSql.Append("[UserID],[CompID],[DisID],[CType],[UType],[RoleID],[IsAudit],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser],[DisSalesManID],[DisTypeID],[AreaID],[IsCheck],[CreditType],[CreditAmount],[AuditUser],[AuditDate])");
            strSql.Append(" values (");
            strSql.Append("@UserID,@CompID,@DisID,@CType,@UType,@RoleID,@IsAudit,@IsEnabled,@CreateUserID,@CreateDate,@ts,@modifyuser,@DisSalesManID,@DisTypeID,@AreaID,@IsCheck,@CreditType,@CreditAmount,@AuditUser,@AuditDate)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@CType", SqlDbType.Int),
                    new SqlParameter("@UType", SqlDbType.Int),
                    new SqlParameter("@RoleID", SqlDbType.Int),
                    new SqlParameter("@IsAudit", SqlDbType.Int),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@DisSalesManID", SqlDbType.Int),
                    new SqlParameter("@DisTypeID", SqlDbType.Int),
                    new SqlParameter("@AreaID", SqlDbType.Int),
                    new SqlParameter("@IsCheck", SqlDbType.Int),
                    new SqlParameter("@CreditType", SqlDbType.Int),
                    new SqlParameter("@CreditAmount", SqlDbType.Decimal),
                    new SqlParameter("@AuditUser", SqlDbType.VarChar),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime)
            };
            parameters[0].Value = model.UserID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.CType;
            parameters[4].Value = model.UType;
            parameters[5].Value = model.RoleID;
            parameters[6].Value = model.IsAudit;
            parameters[7].Value = model.IsEnabled;
            parameters[8].Value = model.CreateUserID;
            parameters[9].Value = model.CreateDate;
            parameters[10].Value = model.ts;
            parameters[11].Value = model.modifyuser;
            parameters[12].Value = model.DisSalesManID;

            parameters[13].Value = model.DisTypeID;
            parameters[14].Value = model.AreaID;
            parameters[15].Value = model.IsCheck;
            parameters[16].Value = model.CreditType;
            parameters[17].Value = model.CreditAmount;

            if (model.AuditUser != null)
                parameters[18].Value = model.AuditUser;
            else
                parameters[18].Value = DBNull.Value;


            if (model.AuditDate != DateTime.MinValue)
                parameters[19].Value = model.AuditDate;
            else
                parameters[19].Value = DBNull.Value;

            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        public bool Update(Hi.Model.SYS_CompUser model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [SYS_CompUser] set ");
            strSql.Append("[UserID]=@UserID,");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[CType]=@CType,");
            strSql.Append("[UType]=@UType,");
            strSql.Append("[RoleID]=@RoleID,");
            strSql.Append("[IsAudit]=@IsAudit,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[DisSalesManID]=@DisSalesManID,");
            strSql.Append("[DisTypeID]=@DisTypeID,");
            strSql.Append("[AreaID]=@AreaID,");
            strSql.Append("[IsCheck]=@IsCheck,");
            strSql.Append("[CreditType]=@CreditType,");
            strSql.Append("[CreditAmount]=@CreditAmount,");
            strSql.Append("[AuditUser]=@AuditUser,");
            strSql.Append("[AuditDate]=@AuditDate");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@CType", SqlDbType.Int),
                    new SqlParameter("@UType", SqlDbType.Int),
                    new SqlParameter("@RoleID", SqlDbType.Int),
                    new SqlParameter("@IsAudit", SqlDbType.Int),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@DisSalesManID", SqlDbType.Int),
                    new SqlParameter("@DisTypeID", SqlDbType.Int),
                    new SqlParameter("@AreaID", SqlDbType.Int),
                    new SqlParameter("@IsCheck", SqlDbType.Int),
                    new SqlParameter("@CreditType", SqlDbType.Int),
                    new SqlParameter("@CreditAmount", SqlDbType.Decimal),
                    new SqlParameter("@AuditUser", SqlDbType.VarChar),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.CompID;
            parameters[3].Value = model.DisID;
            parameters[4].Value = model.CType;
            parameters[5].Value = model.UType;
            parameters[6].Value = model.RoleID;
            parameters[7].Value = model.IsAudit;
            parameters[8].Value = model.IsEnabled;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.CreateDate;
            parameters[11].Value = model.ts;
            parameters[12].Value = model.dr;
            parameters[13].Value = model.modifyuser;
            parameters[14].Value = model.DisSalesManID;

            parameters[15].Value = model.DisTypeID;
            parameters[16].Value = model.AreaID;
            parameters[17].Value = model.IsCheck;
            parameters[18].Value = model.CreditType;
            parameters[19].Value = model.CreditAmount;

            if (model.AuditUser != null)
                parameters[20].Value = model.AuditUser;
            else
                parameters[20].Value = DBNull.Value;


            if (model.AuditDate != DateTime.MinValue)
                parameters[21].Value = model.AuditDate;
            else
                parameters[21].Value = DBNull.Value;

            if (Tran!=null)
                return SqlHelper.ExecuteSql(strSql.ToString(),Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 返回用户账户
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetComUser(string strWhere, string StrWhat = "")
        {
            StringBuilder sql = new StringBuilder();
            if (string.IsNullOrEmpty(StrWhat))
            {
                StrWhat = "cu.ID,cu.UserID,cu.DisID,cu.CompID,cu.CType,cu.UType,cu.IsAudit,cu.IsEnabled,u.UserName,u.TrueName,u.IsEnabled uIsEnabled,u.AuditState uAuditState,u.dr udr,u.Phone,dis.DisName,dis.AuditState disAuditState,dis.IsEnabled disIsEnabled, com.CompName,com.IsEnabled comIsEnabled,com.AuditState comAuditState,com.Erptype";
            }
            sql.Append(@"select " + StrWhat + " from SYS_CompUser as cu  join Sys_Users as u on cu.UserID=u.ID  left join BD_Distributor as dis on cu.DisID=dis.ID and isnull(dis.dr,0)=0 left join BD_Company as com on cu.CompID=com.ID and isnull(com.dr,0)=0");
            if (strWhere != "")
            {
                sql.AppendFormat("where {0}", strWhere);
            }
            sql.Append(" order by cu.ctype desc,cu.createdate ");
            return SqlHelper.Query(SqlHelper.LocalSqlServer, sql.ToString()).Tables[0];
        }


        /// <summary>
        /// 获取用户以及用户明细表数据
        /// </summary>
        public DataTable GetList(int pageSize, int pageIndex, string fldSort, bool sort, string fldName, string TbName, string strCondition, out int pageCount, out int count,bool IsDistinct=false)
        {
            if (string.IsNullOrEmpty(TbName))
            {
                TbName = "[SYS_CompUser]";
            }
            if (string.IsNullOrEmpty(fldName))
            {
                fldName = " * ";
            }
            string strSql;
            DataSet ds = SqlHelper.PageList2(SqlHelper.LocalSqlServer, TbName, fldName, pageSize, pageIndex, fldSort, sort, strCondition, "SYS_CompUser.ID", IsDistinct, out pageCount, out count, out strSql);
            return ds.Tables[0];
        }
    }
}

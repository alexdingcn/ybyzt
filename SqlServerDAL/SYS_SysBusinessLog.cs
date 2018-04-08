using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class SYS_SysBusinessLog
    {
        /// <summary>
        /// 更新一条数据，带事务
        /// </summary>
        public bool Update(Hi.Model.SYS_SysBusinessLog model, SqlTransaction TranSaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [SYS_SysBusinessLog] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[LogClass]=@LogClass,");
            strSql.Append("[ApplicationId]=@ApplicationId,");
            strSql.Append("[LogType]=@LogType,");
            strSql.Append("[LogTime]=@LogTime,");
            strSql.Append("[OperatePersonId]=@OperatePersonId,");
            strSql.Append("[OperatePerson]=@OperatePerson,");
            strSql.Append("[LogRemark]=@LogRemark,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@LogClass", SqlDbType.VarChar,50),
                    new SqlParameter("@ApplicationId", SqlDbType.BigInt),
                    new SqlParameter("@LogType", SqlDbType.VarChar,50),
                    new SqlParameter("@LogTime", SqlDbType.DateTime),
                    new SqlParameter("@OperatePersonId", SqlDbType.Int),
                    new SqlParameter("@OperatePerson", SqlDbType.NVarChar,50),
                    new SqlParameter("@LogRemark", SqlDbType.NVarChar,500),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;

            if (model.LogClass != null)
                parameters[2].Value = model.LogClass;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.ApplicationId;
            parameters[4].Value = model.LogType;
            parameters[5].Value = model.LogTime;
            parameters[6].Value = model.OperatePersonId;

            if (model.OperatePerson != null)
                parameters[7].Value = model.OperatePerson;
            else
                parameters[7].Value = DBNull.Value;


            if (model.LogRemark != null)
                parameters[8].Value = model.LogRemark;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.ts;
            parameters[10].Value = model.dr;
            parameters[11].Value = model.modifyuser;

            if (TranSaction != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), TranSaction, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 增加一条数据，带事务
        /// </summary>
        public int Add(Hi.Model.SYS_SysBusinessLog model, SqlTransaction TranSaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [SYS_SysBusinessLog](");
            strSql.Append("[CompID],[LogClass],[ApplicationId],[LogType],[LogTime],[OperatePersonId],[OperatePerson],[LogRemark],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@LogClass,@ApplicationId,@LogType,@LogTime,@OperatePersonId,@OperatePerson,@LogRemark,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@LogClass", SqlDbType.VarChar,50),
                    new SqlParameter("@ApplicationId", SqlDbType.BigInt),
                    new SqlParameter("@LogType", SqlDbType.VarChar,50),
                    new SqlParameter("@LogTime", SqlDbType.DateTime),
                    new SqlParameter("@OperatePersonId", SqlDbType.Int),
                    new SqlParameter("@OperatePerson", SqlDbType.NVarChar,50),
                    new SqlParameter("@LogRemark", SqlDbType.NVarChar,500),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;

            if (model.LogClass != null)
                parameters[1].Value = model.LogClass;
            else
                parameters[1].Value = DBNull.Value;

            parameters[2].Value = model.ApplicationId;
            parameters[3].Value = model.LogType;
            parameters[4].Value = model.LogTime;
            parameters[5].Value = model.OperatePersonId;

            if (model.OperatePerson != null)
                parameters[6].Value = model.OperatePerson;
            else
                parameters[6].Value = DBNull.Value;


            if (model.LogRemark != null)
                parameters[7].Value = model.LogRemark;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.ts;
            parameters[9].Value = model.modifyuser;

            if (TranSaction != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), TranSaction, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        public IList<Hi.Model.SYS_SysBusinessLog> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction TranSaction)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby, TranSaction));
        }

        public DataSet GetDataSet(string strWhat, string strWhere, string strOrderby, SqlTransaction TranSaction)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [SYS_SysBusinessLog]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);

            if (TranSaction != null)
                return SqlHelper.Query(SqlHelper.LocalSqlServer,strSql.ToString(), TranSaction);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }
    }
}

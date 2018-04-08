//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/5/18 12:07:26
//
// 功能描述: 
//
// 修改标识: 
// 修改描述: 
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using DBUtility;
using System.Collections;
using System.Collections.Generic;

namespace Hi.SQLServerDAL
{
    /// <summary>
    /// 数据访问类 SYS_SysBusinessLog
    /// </summary>
    public partial class SYS_SysBusinessLog
    {
        public SYS_SysBusinessLog()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.SYS_SysBusinessLog model)
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
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.SYS_SysBusinessLog model)
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

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [SYS_SysBusinessLog] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[SYS_SysBusinessLog]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [SYS_SysBusinessLog]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.SYS_SysBusinessLog GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [SYS_SysBusinessLog] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
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
        /// 获取数据集,建议只在多表联查时使用
        /// </summary>
        public DataSet GetDataSet(string strSql)
        {
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql);
        }

        /// <summary>
        /// 获取泛型数据列表,建议只在多表联查时使用
        /// </summary>
        public IList<Hi.Model.SYS_SysBusinessLog> GetList(string strSql)
        {
            return GetList(GetDataSet(strSql));
        }

        /// <summary>
        /// 获取数据集,在单表查询时使用
        /// </summary>
        public DataSet GetDataSet(string strWhat, string strWhere, string strOrderby)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [SYS_SysBusinessLog]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.SYS_SysBusinessLog> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.SYS_SysBusinessLog> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[SYS_SysBusinessLog]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.SYS_SysBusinessLog GetModel(DataRow r)
        {
            Hi.Model.SYS_SysBusinessLog model = new Hi.Model.SYS_SysBusinessLog();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.LogClass = SqlHelper.GetString(r["LogClass"]);
            model.ApplicationId = SqlHelper.GetInt(r["ApplicationId"]);
            model.LogType = SqlHelper.GetString(r["LogType"]);
            model.LogTime = SqlHelper.GetDateTime(r["LogTime"]);
            model.OperatePersonId = SqlHelper.GetInt(r["OperatePersonId"]);
            model.OperatePerson = SqlHelper.GetString(r["OperatePerson"]);
            model.LogRemark = SqlHelper.GetString(r["LogRemark"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.SYS_SysBusinessLog> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.SYS_SysBusinessLog>(ds.Tables[0]);
        }
    }
}

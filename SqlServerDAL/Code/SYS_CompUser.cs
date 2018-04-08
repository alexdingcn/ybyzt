//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/5/23 15:54:05
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
    /// 数据访问类 SYS_CompUser
    /// </summary>
    public partial class SYS_CompUser
    {
        public SYS_CompUser()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.SYS_CompUser model)
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
                    new SqlParameter("@AuditUser", SqlDbType.Int),
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

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.SYS_CompUser model)
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
                    new SqlParameter("@AuditUser", SqlDbType.Int),
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

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [SYS_CompUser] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[SYS_CompUser]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [SYS_CompUser]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.SYS_CompUser GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [SYS_CompUser] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
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
        public IList<Hi.Model.SYS_CompUser> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [SYS_CompUser]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.SYS_CompUser> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.SYS_CompUser> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[SYS_CompUser]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.SYS_CompUser GetModel(DataRow r)
        {
            Hi.Model.SYS_CompUser model = new Hi.Model.SYS_CompUser();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.UserID = SqlHelper.GetInt(r["UserID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.CType = SqlHelper.GetInt(r["CType"]);
            model.UType = SqlHelper.GetInt(r["UType"]);
            model.RoleID = SqlHelper.GetInt(r["RoleID"]);
            model.IsAudit = SqlHelper.GetInt(r["IsAudit"]);
            model.IsEnabled = SqlHelper.GetInt(r["IsEnabled"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.DisSalesManID = SqlHelper.GetInt(r["DisSalesManID"]);
            model.DisTypeID = SqlHelper.GetInt(r["DisTypeID"]);
            model.AreaID = SqlHelper.GetInt(r["AreaID"]);
            model.IsCheck = SqlHelper.GetInt(r["IsCheck"]);
            model.CreditType = SqlHelper.GetInt(r["CreditType"]);
            model.CreditAmount = SqlHelper.GetDecimal(r["CreditAmount"]);
            model.AuditUser = SqlHelper.GetString(r["AuditUser"]);
            model.AuditDate = SqlHelper.GetDateTime(r["AuditDate"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.SYS_CompUser> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.SYS_CompUser>(ds.Tables[0]);
        }
    }
}

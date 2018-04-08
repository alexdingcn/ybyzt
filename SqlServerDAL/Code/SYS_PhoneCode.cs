//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/5/22 15:54:44
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
    /// 数据访问类 SYS_PhoneCode
    /// </summary>
    public partial class SYS_PhoneCode
    {
        public SYS_PhoneCode()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.SYS_PhoneCode model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [SYS_PhoneCode](");
            strSql.Append("[Type],[Module],[UserID],[UserName],[Phone],[PhoneCode],[IsPast],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@Type,@Module,@UserID,@UserName,@Phone,@PhoneCode,@IsPast,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@Module", SqlDbType.VarChar,50),
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@UserName", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,11),
                    new SqlParameter("@PhoneCode", SqlDbType.VarChar,50),
                    new SqlParameter("@IsPast", SqlDbType.SmallInt),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.Type;

            if (model.Module != null)
                parameters[1].Value = model.Module;
            else
                parameters[1].Value = DBNull.Value;

            parameters[2].Value = model.UserID;
            parameters[3].Value = model.UserName;
            parameters[4].Value = model.Phone;
            parameters[5].Value = model.PhoneCode;
            parameters[6].Value = model.IsPast;
            parameters[7].Value = model.CreateDate;
            parameters[8].Value = model.ts;
            parameters[9].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.SYS_PhoneCode model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [SYS_PhoneCode] set ");
            strSql.Append("[Type]=@Type,");
            strSql.Append("[Module]=@Module,");
            strSql.Append("[UserID]=@UserID,");
            strSql.Append("[UserName]=@UserName,");
            strSql.Append("[Phone]=@Phone,");
            strSql.Append("[PhoneCode]=@PhoneCode,");
            strSql.Append("[IsPast]=@IsPast,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@Module", SqlDbType.VarChar,50),
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@UserName", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,11),
                    new SqlParameter("@PhoneCode", SqlDbType.VarChar,50),
                    new SqlParameter("@IsPast", SqlDbType.SmallInt),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Type;

            if (model.Module != null)
                parameters[2].Value = model.Module;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.UserID;
            parameters[4].Value = model.UserName;
            parameters[5].Value = model.Phone;
            parameters[6].Value = model.PhoneCode;
            parameters[7].Value = model.IsPast;
            parameters[8].Value = model.CreateDate;
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
            strSql.Append("delete [SYS_PhoneCode] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[SYS_PhoneCode]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [SYS_PhoneCode]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.SYS_PhoneCode GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [SYS_PhoneCode] ");
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
        public IList<Hi.Model.SYS_PhoneCode> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [SYS_PhoneCode]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.SYS_PhoneCode> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.SYS_PhoneCode> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[SYS_PhoneCode]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.SYS_PhoneCode GetModel(DataRow r)
        {
            Hi.Model.SYS_PhoneCode model = new Hi.Model.SYS_PhoneCode();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.Type = SqlHelper.GetInt(r["Type"]);
            model.Module = SqlHelper.GetString(r["Module"]);
            model.UserID = SqlHelper.GetInt(r["UserID"]);
            model.UserName = SqlHelper.GetString(r["UserName"]);
            model.Phone = SqlHelper.GetString(r["Phone"]);
            model.PhoneCode = SqlHelper.GetString(r["PhoneCode"]);
            model.IsPast = SqlHelper.GetInt(r["IsPast"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.SYS_PhoneCode> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.SYS_PhoneCode>(ds.Tables[0]);
        }
    }
}

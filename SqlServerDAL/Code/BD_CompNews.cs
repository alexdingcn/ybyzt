﻿//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/10/16 14:24:54
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
    /// 数据访问类 BD_CompNews
    /// </summary>
    public partial class BD_CompNews
    {
        public BD_CompNews()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_CompNews model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_CompNews](");
            strSql.Append("[CompID],[IsEnabled],[NewsType],[NewsTitle],[NewsContents],[IsTop],[ShowType],[CreateUserID],[CreateDate],[ts],[modifyuser],[PMID])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@IsEnabled,@NewsType,@NewsTitle,@NewsContents,@IsTop,@ShowType,@CreateUserID,@CreateDate,@ts,@modifyuser,@PMID)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@NewsType", SqlDbType.Int),
                    new SqlParameter("@NewsTitle", SqlDbType.VarChar,200),
                    new SqlParameter("@NewsContents", SqlDbType.Text),
                    new SqlParameter("@IsTop", SqlDbType.Int),
                    new SqlParameter("@ShowType", SqlDbType.VarChar,50),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@PMID", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.NewsType;
            parameters[2].Value = model.NewsTitle;

            if (model.NewsContents != null)
                parameters[3].Value = model.NewsContents;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.IsTop;

            if (model.ShowType != null)
                parameters[5].Value = model.ShowType;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.IsEnabled;
            parameters[7].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[8].Value = model.CreateDate;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.ts;
            parameters[10].Value = model.modifyuser;
            parameters[11].Value = model.PmID;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_CompNews model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_CompNews] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[PmID]=@PmID,");
            strSql.Append("[NewsType]=@NewsType,");
            strSql.Append("[NewsTitle]=@NewsTitle,");
            strSql.Append("[NewsContents]=@NewsContents,");
            strSql.Append("[IsTop]=@IsTop,");
            strSql.Append("[ShowType]=@ShowType,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@PmID", SqlDbType.Int),
                    new SqlParameter("@NewsType", SqlDbType.Int),
                    new SqlParameter("@NewsTitle", SqlDbType.VarChar,200),
                    new SqlParameter("@NewsContents", SqlDbType.Text),
                    new SqlParameter("@IsTop", SqlDbType.Int),
                    new SqlParameter("@ShowType", SqlDbType.VarChar,50),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.PmID;
            parameters[3].Value = model.NewsType;
            parameters[4].Value = model.NewsTitle;

            if (model.NewsContents != null)
                parameters[5].Value = model.NewsContents;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.IsTop;

            if (model.ShowType != null)
                parameters[7].Value = model.ShowType;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.IsEnabled;
            parameters[9].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[10].Value = model.CreateDate;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.ts;
            parameters[12].Value = model.dr;
            parameters[13].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_CompNews] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[BD_CompNews]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BD_CompNews]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_CompNews GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_CompNews] ");
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
        public IList<Hi.Model.BD_CompNews> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_CompNews]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_CompNews> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BD_CompNews> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BD_CompNews]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BD_CompNews GetModel(DataRow r)
        {
            Hi.Model.BD_CompNews model = new Hi.Model.BD_CompNews();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.PmID = SqlHelper.GetInt(r["PmID"]);
            model.NewsType = SqlHelper.GetInt(r["NewsType"]);
            model.NewsTitle = SqlHelper.GetString(r["NewsTitle"]);
            model.NewsContents = SqlHelper.GetString(r["NewsContents"]);
            model.IsTop = SqlHelper.GetInt(r["IsTop"]);
            model.ShowType = SqlHelper.GetString(r["ShowType"]);
            model.IsEnabled = SqlHelper.GetInt(r["IsEnabled"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.BD_CompNews> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BD_CompNews>(ds.Tables[0]);
        }
    }
}

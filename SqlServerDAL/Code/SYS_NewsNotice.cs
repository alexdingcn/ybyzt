//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/12/5 16:09:57
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
    /// 数据访问类 SYS_NewsNotice
    /// </summary>
    public partial class SYS_NewsNotice
    {
        public SYS_NewsNotice()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.SYS_NewsNotice model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [SYS_NewsNotice](");
            strSql.Append("[IsEnabled],[NewsType],[NewsTitle],[NewsContents],[IsTop],[CreateUserID],[CreateDate],[ts],[modifyuser],[NewsInfo],[KeyWords])");
            strSql.Append(" values (");
            strSql.Append("@IsEnabled,@NewsType,@NewsTitle,@NewsContents,@IsTop,@CreateUserID,@CreateDate,@ts,@modifyuser,@NewsInfo,@KeyWords)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@NewsType", SqlDbType.Int),
                    new SqlParameter("@NewsTitle", SqlDbType.VarChar,200),
                    new SqlParameter("@NewsContents", SqlDbType.Text),
                    new SqlParameter("@IsTop", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@NewsInfo", SqlDbType.VarChar,500),
                    new SqlParameter("@KeyWords", SqlDbType.VarChar,100)
            };
            parameters[0].Value = model.IsEnabled;
            parameters[1].Value = model.NewsType;

            if (model.NewsTitle != null)
                parameters[2].Value = model.NewsTitle;
            else
                parameters[2].Value = DBNull.Value;


            if (model.NewsContents != null)
                parameters[3].Value = model.NewsContents;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.IsTop;
            parameters[5].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[6].Value = model.CreateDate;
            else
                parameters[6].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[7].Value = model.ts;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.modifyuser;

            if (model.NewsInfo != null)
                parameters[9].Value = model.NewsInfo;
            else
                parameters[9].Value = DBNull.Value;


            if (model.KeyWords != null)
                parameters[10].Value = model.KeyWords;
            else
                parameters[10].Value = DBNull.Value;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.SYS_NewsNotice model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [SYS_NewsNotice] set ");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[NewsType]=@NewsType,");
            strSql.Append("[NewsTitle]=@NewsTitle,");
            strSql.Append("[NewsContents]=@NewsContents,");
            strSql.Append("[IsTop]=@IsTop,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[NewsInfo]=@NewsInfo,");
            strSql.Append("[KeyWords]=@KeyWords");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@NewsType", SqlDbType.Int),
                    new SqlParameter("@NewsTitle", SqlDbType.VarChar,200),
                    new SqlParameter("@NewsContents", SqlDbType.Text),
                    new SqlParameter("@IsTop", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@NewsInfo", SqlDbType.VarChar,500),
                    new SqlParameter("@KeyWords", SqlDbType.VarChar,100)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.IsEnabled;
            parameters[2].Value = model.NewsType;

            if (model.NewsTitle != null)
                parameters[3].Value = model.NewsTitle;
            else
                parameters[3].Value = DBNull.Value;


            if (model.NewsContents != null)
                parameters[4].Value = model.NewsContents;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.IsTop;
            parameters[6].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[7].Value = model.CreateDate;
            else
                parameters[7].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[8].Value = model.ts;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.dr;
            parameters[10].Value = model.modifyuser;

            if (model.NewsInfo != null)
                parameters[11].Value = model.NewsInfo;
            else
                parameters[11].Value = DBNull.Value;


            if (model.KeyWords != null)
                parameters[12].Value = model.KeyWords;
            else
                parameters[12].Value = DBNull.Value;


            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [SYS_NewsNotice] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[SYS_NewsNotice]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [SYS_NewsNotice]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.SYS_NewsNotice GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [SYS_NewsNotice] ");
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
        public IList<Hi.Model.SYS_NewsNotice> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [SYS_NewsNotice]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.SYS_NewsNotice> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.SYS_NewsNotice> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[SYS_NewsNotice]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.SYS_NewsNotice GetModel(DataRow r)
        {
            Hi.Model.SYS_NewsNotice model = new Hi.Model.SYS_NewsNotice();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.IsEnabled = SqlHelper.GetInt(r["IsEnabled"]);
            model.NewsType = SqlHelper.GetInt(r["NewsType"]);
            model.NewsTitle = SqlHelper.GetString(r["NewsTitle"]);
            model.NewsContents = SqlHelper.GetString(r["NewsContents"]);
            model.IsTop = SqlHelper.GetInt(r["IsTop"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.NewsInfo = SqlHelper.GetString(r["NewsInfo"]);
            model.KeyWords = SqlHelper.GetString(r["KeyWords"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.SYS_NewsNotice> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.SYS_NewsNotice>(ds.Tables[0]);
        }
    }
}

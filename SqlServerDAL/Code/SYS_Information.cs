//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/5/30 11:58:20
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
    /// 数据访问类 SYS_Information
    /// </summary>
    public class SYS_Information
    {
        public SYS_Information()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.SYS_Information model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [SYS_Information](");
            strSql.Append("[CompID],[Type],[DisID],[UserID],[Title],[Contents],[Url],[CreateDate],[IsRead],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@Type,@DisID,@UserID,@Title,@Contents,@Url,@CreateDate,@IsRead,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@Title", SqlDbType.VarChar,200),
                    new SqlParameter("@Contents", SqlDbType.VarChar,1000),
                    new SqlParameter("@Url", SqlDbType.VarChar,200),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@IsRead", SqlDbType.Int),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.Type;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.UserID;

            if (model.Title != null)
                parameters[4].Value = model.Title;
            else
                parameters[4].Value = DBNull.Value;


            if (model.Contents != null)
                parameters[5].Value = model.Contents;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Url != null)
                parameters[6].Value = model.Url;
            else
                parameters[6].Value = DBNull.Value;


            if (model.CreateDate != DateTime.MinValue)
                parameters[7].Value = model.CreateDate;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.IsRead;
            parameters[9].Value = model.ts;
            parameters[10].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.SYS_Information model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [SYS_Information] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[Type]=@Type,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[UserID]=@UserID,");
            strSql.Append("[Title]=@Title,");
            strSql.Append("[Contents]=@Contents,");
            strSql.Append("[Url]=@Url,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[IsRead]=@IsRead,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@Title", SqlDbType.VarChar,200),
                    new SqlParameter("@Contents", SqlDbType.VarChar,1000),
                    new SqlParameter("@Url", SqlDbType.VarChar,200),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@IsRead", SqlDbType.Int),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.Type;
            parameters[3].Value = model.DisID;
            parameters[4].Value = model.UserID;

            if (model.Title != null)
                parameters[5].Value = model.Title;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Contents != null)
                parameters[6].Value = model.Contents;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Url != null)
                parameters[7].Value = model.Url;
            else
                parameters[7].Value = DBNull.Value;


            if (model.CreateDate != DateTime.MinValue)
                parameters[8].Value = model.CreateDate;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.IsRead;
            parameters[10].Value = model.ts;
            parameters[11].Value = model.dr;
            parameters[12].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [SYS_Information] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[SYS_Information]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [SYS_Information]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.SYS_Information GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [SYS_Information] ");
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
        public IList<Hi.Model.SYS_Information> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [SYS_Information]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.SYS_Information> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.SYS_Information> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[SYS_Information]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.SYS_Information GetModel(DataRow r)
        {
            Hi.Model.SYS_Information model = new Hi.Model.SYS_Information();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.Type = SqlHelper.GetInt(r["Type"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.UserID = SqlHelper.GetInt(r["UserID"]);
            model.Title = SqlHelper.GetString(r["Title"]);
            model.Contents = SqlHelper.GetString(r["Contents"]);
            model.Url = SqlHelper.GetString(r["Url"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.IsRead = SqlHelper.GetInt(r["IsRead"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.SYS_Information> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.SYS_Information>(ds.Tables[0]);
        }
    }
}

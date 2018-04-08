//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/8/31 9:27:51
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
    /// 数据访问类 BD_DisAccount
    /// </summary>
    public class BD_DisAccount
    {
        public BD_DisAccount()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_DisAccount model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_DisAccount](");
            strSql.Append("[DisID],[Rise],[Content],[OBank],[OAccount],[TRNumber],[CreateUserID],[CreateDate],[ts],[dr],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@DisID,@Rise,@Content,@OBank,@OAccount,@TRNumber,@CreateUserID,@CreateDate,@ts,@dr,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@Rise", SqlDbType.VarChar,100),
                    new SqlParameter("@Content", SqlDbType.VarChar,200),
                    new SqlParameter("@OBank", SqlDbType.VarChar,100),
                    new SqlParameter("@OAccount", SqlDbType.VarChar,100),
                    new SqlParameter("@TRNumber", SqlDbType.VarChar,100),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.DisID;

            if (model.Rise != null)
                parameters[1].Value = model.Rise;
            else
                parameters[1].Value = DBNull.Value;


            if (model.Content != null)
                parameters[2].Value = model.Content;
            else
                parameters[2].Value = DBNull.Value;


            if (model.OBank != null)
                parameters[3].Value = model.OBank;
            else
                parameters[3].Value = DBNull.Value;


            if (model.OAccount != null)
                parameters[4].Value = model.OAccount;
            else
                parameters[4].Value = DBNull.Value;


            if (model.TRNumber != null)
                parameters[5].Value = model.TRNumber;
            else
                parameters[5].Value = DBNull.Value;

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
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_DisAccount model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_DisAccount] set ");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[Rise]=@Rise,");
            strSql.Append("[Content]=@Content,");
            strSql.Append("[OBank]=@OBank,");
            strSql.Append("[OAccount]=@OAccount,");
            strSql.Append("[TRNumber]=@TRNumber,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@Rise", SqlDbType.VarChar,100),
                    new SqlParameter("@Content", SqlDbType.VarChar,200),
                    new SqlParameter("@OBank", SqlDbType.VarChar,100),
                    new SqlParameter("@OAccount", SqlDbType.VarChar,100),
                    new SqlParameter("@TRNumber", SqlDbType.VarChar,100),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.DisID;

            if (model.Rise != null)
                parameters[2].Value = model.Rise;
            else
                parameters[2].Value = DBNull.Value;


            if (model.Content != null)
                parameters[3].Value = model.Content;
            else
                parameters[3].Value = DBNull.Value;


            if (model.OBank != null)
                parameters[4].Value = model.OBank;
            else
                parameters[4].Value = DBNull.Value;


            if (model.OAccount != null)
                parameters[5].Value = model.OAccount;
            else
                parameters[5].Value = DBNull.Value;


            if (model.TRNumber != null)
                parameters[6].Value = model.TRNumber;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[8].Value = model.CreateDate;
            else
                parameters[8].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[9].Value = model.ts;
            else
                parameters[9].Value = DBNull.Value;

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
            strSql.Append("delete [BD_DisAccount] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[BD_DisAccount]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BD_DisAccount]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_DisAccount GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_DisAccount] ");
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
        public IList<Hi.Model.BD_DisAccount> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_DisAccount]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_DisAccount> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BD_DisAccount> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BD_DisAccount]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BD_DisAccount GetModel(DataRow r)
        {
            Hi.Model.BD_DisAccount model = new Hi.Model.BD_DisAccount();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.Rise = SqlHelper.GetString(r["Rise"]);
            model.Content = SqlHelper.GetString(r["Content"]);
            model.OBank = SqlHelper.GetString(r["OBank"]);
            model.OAccount = SqlHelper.GetString(r["OAccount"]);
            model.TRNumber = SqlHelper.GetString(r["TRNumber"]);
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
        private IList<Hi.Model.BD_DisAccount> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BD_DisAccount>(ds.Tables[0]);
        }
    }
}

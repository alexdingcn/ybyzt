//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/7/14 16:39:46
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
    /// 数据访问类 PAY_BankInfo
    /// </summary>
    public class PAY_BankInfo
    {
        public PAY_BankInfo()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.PAY_BankInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [PAY_BankInfo](");
            strSql.Append("[BankName],[BankCode],[vdef1],[vdef2],[vdef3],[vdef4],[vdef5],[vdef6])");
            strSql.Append(" values (");
            strSql.Append("@BankName,@BankCode,@vdef1,@vdef2,@vdef3,@vdef4,@vdef5,@vdef6)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@BankName", SqlDbType.NVarChar,50),
                    new SqlParameter("@BankCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@vdef1", SqlDbType.NVarChar,50),
                    new SqlParameter("@vdef2", SqlDbType.NVarChar,50),
                    new SqlParameter("@vdef3", SqlDbType.NVarChar,50),
                    new SqlParameter("@vdef4", SqlDbType.NVarChar,50),
                    new SqlParameter("@vdef5", SqlDbType.NVarChar,50),
                    new SqlParameter("@vdef6", SqlDbType.NVarChar,50)
            };

            if (model.BankName != null)
                parameters[0].Value = model.BankName;
            else
                parameters[0].Value = DBNull.Value;


            if (model.BankCode != null)
                parameters[1].Value = model.BankCode;
            else
                parameters[1].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[2].Value = model.vdef1;
            else
                parameters[2].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[3].Value = model.vdef2;
            else
                parameters[3].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[4].Value = model.vdef3;
            else
                parameters[4].Value = DBNull.Value;


            if (model.vdef4 != null)
                parameters[5].Value = model.vdef4;
            else
                parameters[5].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[6].Value = model.vdef5;
            else
                parameters[6].Value = DBNull.Value;


            if (model.vdef6 != null)
                parameters[7].Value = model.vdef6;
            else
                parameters[7].Value = DBNull.Value;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.PAY_BankInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [PAY_BankInfo] set ");
            strSql.Append("[BankName]=@BankName,");
            strSql.Append("[BankCode]=@BankCode,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[vdef4]=@vdef4,");
            strSql.Append("[vdef5]=@vdef5,");
            strSql.Append("[vdef6]=@vdef6");
            strSql.Append(" where [Id]=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int),
                    new SqlParameter("@BankName", SqlDbType.NVarChar,50),
                    new SqlParameter("@BankCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@vdef1", SqlDbType.NVarChar,50),
                    new SqlParameter("@vdef2", SqlDbType.NVarChar,50),
                    new SqlParameter("@vdef3", SqlDbType.NVarChar,50),
                    new SqlParameter("@vdef4", SqlDbType.NVarChar,50),
                    new SqlParameter("@vdef5", SqlDbType.NVarChar,50),
                    new SqlParameter("@vdef6", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = model.Id;

            if (model.BankName != null)
                parameters[1].Value = model.BankName;
            else
                parameters[1].Value = DBNull.Value;


            if (model.BankCode != null)
                parameters[2].Value = model.BankCode;
            else
                parameters[2].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[3].Value = model.vdef1;
            else
                parameters[3].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[4].Value = model.vdef2;
            else
                parameters[4].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[5].Value = model.vdef3;
            else
                parameters[5].Value = DBNull.Value;


            if (model.vdef4 != null)
                parameters[6].Value = model.vdef4;
            else
                parameters[6].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[7].Value = model.vdef5;
            else
                parameters[7].Value = DBNull.Value;


            if (model.vdef6 != null)
                parameters[8].Value = model.vdef6;
            else
                parameters[8].Value = DBNull.Value;


            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [PAY_BankInfo] ");
            strSql.Append(" where [Id]=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int)};
            parameters[0].Value = Id;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[Id]", "[PAY_BankInfo]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [PAY_BankInfo]");
            strSql.Append(" where [Id]= @Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int)};
            parameters[0].Value = Id;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.PAY_BankInfo GetModel(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [PAY_BankInfo] ");
            strSql.Append(" where [Id]=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int)};
            parameters[0].Value = Id;
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
        public IList<Hi.Model.PAY_BankInfo> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [PAY_BankInfo]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.PAY_BankInfo> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.PAY_BankInfo> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[PAY_BankInfo]", null, pageSize, pageIndex, fldSort, sort, strCondition, "Id", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.PAY_BankInfo GetModel(DataRow r)
        {
            Hi.Model.PAY_BankInfo model = new Hi.Model.PAY_BankInfo();
            model.Id = SqlHelper.GetInt(r["Id"]);
            model.BankName = SqlHelper.GetString(r["BankName"]);
            model.BankCode = SqlHelper.GetString(r["BankCode"]);
            model.vdef1 = SqlHelper.GetString(r["vdef1"]);
            model.vdef2 = SqlHelper.GetString(r["vdef2"]);
            model.vdef3 = SqlHelper.GetString(r["vdef3"]);
            model.vdef4 = SqlHelper.GetString(r["vdef4"]);
            model.vdef5 = SqlHelper.GetString(r["vdef5"]);
            model.vdef6 = SqlHelper.GetString(r["vdef6"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.PAY_BankInfo> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.PAY_BankInfo>(ds.Tables[0]);
        }
    }
}

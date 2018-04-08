//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/10/21 11:18:05
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
    /// 数据访问类 BF_ZD_PROVINCE
    /// </summary>
    public class BF_ZD_PROVINCE
    {
        public BF_ZD_PROVINCE()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BF_ZD_PROVINCE model)
        {
            model.PROVID = GetMaxId() + 1;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BF_ZD_PROVINCE](");
            strSql.Append("[PROVID])");
            strSql.Append(" values (");
            strSql.Append("@PROVID)");
            SqlParameter[] parameters = {
                    new SqlParameter("@PROVID", SqlDbType.Int)
            };
            parameters[0].Value = model.PROVID;

            if (SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0)
                   return model.PROVID;
            else
                return 0;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BF_ZD_PROVINCE model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BF_ZD_PROVINCE] set ");
            strSql.Append("[PROVCODE]=@PROVCODE,");
            strSql.Append("[PROVNAME]=@PROVNAME,");
            strSql.Append("[PROVORD]=@PROVORD,");
            strSql.Append("[SPEC16]=@SPEC16,");
            strSql.Append("[SPEC32]=@SPEC32");
            strSql.Append(" where [PROVID]=@PROVID");
            SqlParameter[] parameters = {
                    new SqlParameter("@PROVID", SqlDbType.Int),
                    new SqlParameter("@PROVCODE", SqlDbType.VarChar,100),
                    new SqlParameter("@PROVNAME", SqlDbType.VarChar,100),
                    new SqlParameter("@PROVORD", SqlDbType.Int),
                    new SqlParameter("@SPEC16", SqlDbType.VarChar,16),
                    new SqlParameter("@SPEC32", SqlDbType.VarChar,32)
            };
            parameters[0].Value = model.PROVID;
            parameters[1].Value = model.PROVCODE;
            parameters[2].Value = model.PROVNAME;
            parameters[3].Value = model.PROVORD;
            parameters[4].Value = model.SPEC16;
            parameters[5].Value = model.SPEC32;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int PROVID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BF_ZD_PROVINCE] ");
            strSql.Append(" where [PROVID]=@PROVID");
            SqlParameter[] parameters = {
                    new SqlParameter("@PROVID", SqlDbType.Int)};
            parameters[0].Value = PROVID;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[PROVID]", "[BF_ZD_PROVINCE]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int PROVID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BF_ZD_PROVINCE]");
            strSql.Append(" where [PROVID]= @PROVID");
            SqlParameter[] parameters = {
                    new SqlParameter("@PROVID", SqlDbType.Int)};
            parameters[0].Value = PROVID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BF_ZD_PROVINCE GetModel(int PROVID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BF_ZD_PROVINCE] ");
            strSql.Append(" where [PROVID]=@PROVID");
            SqlParameter[] parameters = {
                    new SqlParameter("@PROVID", SqlDbType.Int)};
            parameters[0].Value = PROVID;
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
        public IList<Hi.Model.BF_ZD_PROVINCE> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BF_ZD_PROVINCE]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BF_ZD_PROVINCE> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BF_ZD_PROVINCE> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BF_ZD_PROVINCE]", null, pageSize, pageIndex, fldSort, sort, strCondition, "PROVID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BF_ZD_PROVINCE GetModel(DataRow r)
        {
            Hi.Model.BF_ZD_PROVINCE model = new Hi.Model.BF_ZD_PROVINCE();
            model.PROVID = SqlHelper.GetInt(r["PROVID"]);
            model.PROVCODE = SqlHelper.GetString(r["PROVCODE"]);
            model.PROVNAME = SqlHelper.GetString(r["PROVNAME"]);
            model.PROVORD = SqlHelper.GetInt(r["PROVORD"]);
            model.SPEC16 = SqlHelper.GetString(r["SPEC16"]);
            model.SPEC32 = SqlHelper.GetString(r["SPEC32"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.BF_ZD_PROVINCE> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BF_ZD_PROVINCE>(ds.Tables[0]);
        }
    }
}

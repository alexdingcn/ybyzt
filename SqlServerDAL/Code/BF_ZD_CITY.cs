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
    /// 数据访问类 BF_ZD_CITY
    /// </summary>
    public class BF_ZD_CITY
    {
        public BF_ZD_CITY()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BF_ZD_CITY model)
        {
            model.CITYID = GetMaxId() + 1;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BF_ZD_CITY](");
            strSql.Append("[CITYID])");
            strSql.Append(" values (");
            strSql.Append("@CITYID)");
            SqlParameter[] parameters = {
                    new SqlParameter("@CITYID", SqlDbType.Int)
            };
            parameters[0].Value = model.CITYID;

            if (SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0)
                   return model.CITYID;
            else
                return 0;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BF_ZD_CITY model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BF_ZD_CITY] set ");
            strSql.Append("[PROVID]=@PROVID,");
            strSql.Append("[CITYCODE]=@CITYCODE,");
            strSql.Append("[CITYNAME]=@CITYNAME,");
            strSql.Append("[CITYORD]=@CITYORD,");
            strSql.Append("[SPEC16]=@SPEC16,");
            strSql.Append("[SPEC32]=@SPEC32");
            strSql.Append(" where [CITYID]=@CITYID");
            SqlParameter[] parameters = {
                    new SqlParameter("@CITYID", SqlDbType.Int),
                    new SqlParameter("@PROVID", SqlDbType.Int),
                    new SqlParameter("@CITYCODE", SqlDbType.VarChar,100),
                    new SqlParameter("@CITYNAME", SqlDbType.VarChar,100),
                    new SqlParameter("@CITYORD", SqlDbType.Int),
                    new SqlParameter("@SPEC16", SqlDbType.VarChar,16),
                    new SqlParameter("@SPEC32", SqlDbType.VarChar,32)
            };
            parameters[0].Value = model.CITYID;
            parameters[1].Value = model.PROVID;
            parameters[2].Value = model.CITYCODE;
            parameters[3].Value = model.CITYNAME;
            parameters[4].Value = model.CITYORD;
            parameters[5].Value = model.SPEC16;
            parameters[6].Value = model.SPEC32;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int CITYID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BF_ZD_CITY] ");
            strSql.Append(" where [CITYID]=@CITYID");
            SqlParameter[] parameters = {
                    new SqlParameter("@CITYID", SqlDbType.Int)};
            parameters[0].Value = CITYID;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[CITYID]", "[BF_ZD_CITY]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int CITYID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BF_ZD_CITY]");
            strSql.Append(" where [CITYID]= @CITYID");
            SqlParameter[] parameters = {
                    new SqlParameter("@CITYID", SqlDbType.Int)};
            parameters[0].Value = CITYID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BF_ZD_CITY GetModel(int CITYID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BF_ZD_CITY] ");
            strSql.Append(" where [CITYID]=@CITYID");
            SqlParameter[] parameters = {
                    new SqlParameter("@CITYID", SqlDbType.Int)};
            parameters[0].Value = CITYID;
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
        public IList<Hi.Model.BF_ZD_CITY> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BF_ZD_CITY]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BF_ZD_CITY> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BF_ZD_CITY> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BF_ZD_CITY]", null, pageSize, pageIndex, fldSort, sort, strCondition, "CITYID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BF_ZD_CITY GetModel(DataRow r)
        {
            Hi.Model.BF_ZD_CITY model = new Hi.Model.BF_ZD_CITY();
            model.CITYID = SqlHelper.GetInt(r["CITYID"]);
            model.PROVID = SqlHelper.GetInt(r["PROVID"]);
            model.CITYCODE = SqlHelper.GetString(r["CITYCODE"]);
            model.CITYNAME = SqlHelper.GetString(r["CITYNAME"]);
            model.CITYORD = SqlHelper.GetInt(r["CITYORD"]);
            model.SPEC16 = SqlHelper.GetString(r["SPEC16"]);
            model.SPEC32 = SqlHelper.GetString(r["SPEC32"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.BF_ZD_CITY> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BF_ZD_CITY>(ds.Tables[0]);
        }
    }
}

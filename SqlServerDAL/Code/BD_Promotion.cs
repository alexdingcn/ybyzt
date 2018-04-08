//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/3/21 14:10:31
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
    /// 数据访问类 BD_Promotion
    /// </summary>
    public partial class BD_Promotion
    {
        public BD_Promotion()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_Promotion model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_Promotion](");
            strSql.Append("[CompID],[ProTitle],[Type],[ProType],[Discount],[ProInfos],[IsEnabled],[ProStartTime],[ProEndTime],[CreateUserID],[CreateDate],[ts],[dr],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@ProTitle,@Type,@ProType,@Discount,@ProInfos,@IsEnabled,@ProStartTime,@ProEndTime,@CreateUserID,@CreateDate,@ts,@dr,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@ProTitle", SqlDbType.NVarChar,200),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@ProType", SqlDbType.Int),
                    new SqlParameter("@Discount", SqlDbType.Decimal),
                    new SqlParameter("@ProInfos", SqlDbType.NVarChar,2000),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@ProStartTime", SqlDbType.DateTime),
                    new SqlParameter("@ProEndTime", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.Char,19),
                    new SqlParameter("@dr", SqlDbType.Int),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;

            if (model.ProTitle != null)
                parameters[1].Value = model.ProTitle;
            else
                parameters[1].Value = DBNull.Value;

            parameters[2].Value = model.Type;
            parameters[3].Value = model.ProType;
            parameters[4].Value = model.Discount;

            if (model.ProInfos != null)
                parameters[5].Value = model.ProInfos;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.IsEnabled;
            parameters[7].Value = model.ProStartTime;
            parameters[8].Value = model.ProEndTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.CreateDate;
            parameters[11].Value = model.ts;
            parameters[12].Value = model.dr;
            parameters[13].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Promotion model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_Promotion] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[ProTitle]=@ProTitle,");
            strSql.Append("[Type]=@Type,");
            strSql.Append("[ProType]=@ProType,");
            strSql.Append("[Discount]=@Discount,");
            strSql.Append("[ProInfos]=@ProInfos,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[ProStartTime]=@ProStartTime,");
            strSql.Append("[ProEndTime]=@ProEndTime,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@ProTitle", SqlDbType.NVarChar,200),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@ProType", SqlDbType.Int),
                    new SqlParameter("@Discount", SqlDbType.Decimal),
                    new SqlParameter("@ProInfos", SqlDbType.NVarChar,2000),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@ProStartTime", SqlDbType.DateTime),
                    new SqlParameter("@ProEndTime", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.Char,19),
                    new SqlParameter("@dr", SqlDbType.Int),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;

            if (model.ProTitle != null)
                parameters[2].Value = model.ProTitle;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.Type;
            parameters[4].Value = model.ProType;
            parameters[5].Value = model.Discount;

            if (model.ProInfos != null)
                parameters[6].Value = model.ProInfos;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.IsEnabled;
            parameters[8].Value = model.ProStartTime;
            parameters[9].Value = model.ProEndTime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.CreateDate;
            parameters[12].Value = model.ts;
            parameters[13].Value = model.dr;
            parameters[14].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_Promotion] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[BD_Promotion]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BD_Promotion]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_Promotion GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_Promotion] ");
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
        public IList<Hi.Model.BD_Promotion> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_Promotion]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_Promotion> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BD_Promotion> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BD_Promotion]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BD_Promotion GetModel(DataRow r)
        {
            Hi.Model.BD_Promotion model = new Hi.Model.BD_Promotion();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.ProTitle = SqlHelper.GetString(r["ProTitle"]);
            model.Type = SqlHelper.GetInt(r["Type"]);
            model.ProType = SqlHelper.GetInt(r["ProType"]);
            model.Discount = SqlHelper.GetDecimal(r["Discount"]);
            model.ProInfos = SqlHelper.GetString(r["ProInfos"]);
            model.IsEnabled = SqlHelper.GetInt(r["IsEnabled"]);
            model.ProStartTime = SqlHelper.GetDateTime(r["ProStartTime"]);
            model.ProEndTime = SqlHelper.GetDateTime(r["ProEndTime"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetString(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.BD_Promotion> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BD_Promotion>(ds.Tables[0]);
        }
    }
}

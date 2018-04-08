//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/10/20 17:36:24
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
    /// 数据访问类 DIS_Integral
    /// </summary>
    public class DIS_Integral
    {
        public DIS_Integral()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_Integral model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_Integral](");
            strSql.Append("[CompID],[DisID],[OrderID],[IntegralType],[OldIntegral],[Integral],[NewIntegral],[Source],[Remarks],[CreateDate],[type],[IsView],[ts],[dr],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@OrderID,@IntegralType,@OldIntegral,@Integral,@NewIntegral,@Source,@Remarks,@CreateDate,@type,@IsView,@ts,@dr,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@OrderID", SqlDbType.Int),
                    new SqlParameter("@IntegralType", SqlDbType.NVarChar,200),
                    new SqlParameter("@OldIntegral", SqlDbType.Decimal),
                    new SqlParameter("@Integral", SqlDbType.Decimal),
                    new SqlParameter("@NewIntegral", SqlDbType.Decimal),
                    new SqlParameter("@Source", SqlDbType.NVarChar,200),
                    new SqlParameter("@Remarks", SqlDbType.NVarChar,800),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@type", SqlDbType.Int),
                    new SqlParameter("@IsView", SqlDbType.Int),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.OrderID;

            if (model.IntegralType != null)
                parameters[3].Value = model.IntegralType;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.OldIntegral;
            parameters[5].Value = model.Integral;
            parameters[6].Value = model.NewIntegral;

            if (model.Source != null)
                parameters[7].Value = model.Source;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Remarks != null)
                parameters[8].Value = model.Remarks;
            else
                parameters[8].Value = DBNull.Value;


            if (model.CreateDate != DateTime.MinValue)
                parameters[9].Value = model.CreateDate;
            else
                parameters[9].Value = DBNull.Value;

            parameters[10].Value = model.type;
            parameters[11].Value = model.IsView;
            parameters[12].Value = model.ts;
            parameters[13].Value = model.dr;
            parameters[14].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_Integral model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_Integral] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[OrderID]=@OrderID,");
            strSql.Append("[IntegralType]=@IntegralType,");
            strSql.Append("[OldIntegral]=@OldIntegral,");
            strSql.Append("[Integral]=@Integral,");
            strSql.Append("[NewIntegral]=@NewIntegral,");
            strSql.Append("[Source]=@Source,");
            strSql.Append("[Remarks]=@Remarks,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[type]=@type,");
            strSql.Append("[IsView]=@IsView,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@OrderID", SqlDbType.Int),
                    new SqlParameter("@IntegralType", SqlDbType.NVarChar,200),
                    new SqlParameter("@OldIntegral", SqlDbType.Decimal),
                    new SqlParameter("@Integral", SqlDbType.Decimal),
                    new SqlParameter("@NewIntegral", SqlDbType.Decimal),
                    new SqlParameter("@Source", SqlDbType.NVarChar,200),
                    new SqlParameter("@Remarks", SqlDbType.NVarChar,800),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@type", SqlDbType.Int),
                    new SqlParameter("@IsView", SqlDbType.Int),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.OrderID;

            if (model.IntegralType != null)
                parameters[4].Value = model.IntegralType;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.OldIntegral;
            parameters[6].Value = model.Integral;
            parameters[7].Value = model.NewIntegral;

            if (model.Source != null)
                parameters[8].Value = model.Source;
            else
                parameters[8].Value = DBNull.Value;


            if (model.Remarks != null)
                parameters[9].Value = model.Remarks;
            else
                parameters[9].Value = DBNull.Value;


            if (model.CreateDate != DateTime.MinValue)
                parameters[10].Value = model.CreateDate;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.type;
            parameters[12].Value = model.IsView;
            parameters[13].Value = model.ts;
            parameters[14].Value = model.dr;
            parameters[15].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [DIS_Integral] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[DIS_Integral]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [DIS_Integral]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.DIS_Integral GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [DIS_Integral] ");
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
        public IList<Hi.Model.DIS_Integral> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [DIS_Integral]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.DIS_Integral> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.DIS_Integral> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[DIS_Integral]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.DIS_Integral GetModel(DataRow r)
        {
            Hi.Model.DIS_Integral model = new Hi.Model.DIS_Integral();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.OrderID = SqlHelper.GetInt(r["OrderID"]);
            model.IntegralType = SqlHelper.GetString(r["IntegralType"]);
            model.OldIntegral = SqlHelper.GetDecimal(r["OldIntegral"]);
            model.Integral = SqlHelper.GetDecimal(r["Integral"]);
            model.NewIntegral = SqlHelper.GetDecimal(r["NewIntegral"]);
            model.Source = SqlHelper.GetString(r["Source"]);
            model.Remarks = SqlHelper.GetString(r["Remarks"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.type = SqlHelper.GetInt(r["type"]);
            model.IsView = SqlHelper.GetInt(r["IsView"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.DIS_Integral> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.DIS_Integral>(ds.Tables[0]);
        }
    }
}

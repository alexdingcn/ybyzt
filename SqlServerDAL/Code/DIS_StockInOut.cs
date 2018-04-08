//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2017 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2017/2/6 13:03:24
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
    /// 数据访问类 DIS_StockInOut
    /// </summary>
    public partial class DIS_StockInOut
    {
        public DIS_StockInOut()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_StockInOut model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_StockInOut](");
            strSql.Append("[CompID],[StockOrderID],[GoodsID],[GoodsInfoID],[StockNum],[Remark],[CreateUserID],[CreateDate],[ts],[modifyuser],[BatchNO],[validDate])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@StockOrderID,@GoodsID,@GoodsInfoID,@StockNum,@Remark,@CreateUserID,@CreateDate,@ts,@modifyuser,@BatchNO,@validDate)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@StockOrderID", SqlDbType.BigInt),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@GoodsInfoID", SqlDbType.BigInt),
                    new SqlParameter("@StockNum", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1000),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@BatchNO", SqlDbType.VarChar),
                    new SqlParameter("@validDate", SqlDbType.DateTime)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.StockOrderID;
            parameters[2].Value = model.GoodsID;
            parameters[3].Value = model.GoodsInfoID;
            parameters[4].Value = model.StockNum;

            if (model.Remark != null)
                parameters[5].Value = model.Remark;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.CreateDate;
            parameters[8].Value = model.ts;
            parameters[9].Value = model.modifyuser;

            parameters[10].Value = model.Batchno;

            if (model.Validdate != DateTime.MinValue)
                parameters[11].Value = model.Validdate;
            else
                parameters[11].Value = DBNull.Value;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_StockInOut model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_StockInOut] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[StockOrderID]=@StockOrderID,");
            strSql.Append("[GoodsID]=@GoodsID,");
            strSql.Append("[GoodsInfoID]=@GoodsInfoID,");
            strSql.Append("[StockNum]=@StockNum,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[BatchNO]=@BatchNO,");
            strSql.Append("[validDate]=@validDate");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@StockOrderID", SqlDbType.BigInt),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@GoodsInfoID", SqlDbType.BigInt),
                    new SqlParameter("@StockNum", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1000),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@BatchNO", SqlDbType.VarChar),
                    new SqlParameter("@validDate", SqlDbType.DateTime)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.StockOrderID;
            parameters[3].Value = model.GoodsID;
            parameters[4].Value = model.GoodsInfoID;
            parameters[5].Value = model.StockNum;

            if (model.Remark != null)
                parameters[6].Value = model.Remark;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.CreateUserID;
            parameters[8].Value = model.CreateDate;
            parameters[9].Value = model.ts;
            parameters[10].Value = model.dr;
            parameters[11].Value = model.modifyuser;
            parameters[12].Value = model.Batchno;

            if (model.Validdate != DateTime.MinValue)
                parameters[13].Value = model.Validdate;
            else
                parameters[13].Value = DBNull.Value;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [DIS_StockInOut] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[DIS_StockInOut]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [DIS_StockInOut]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.DIS_StockInOut GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [DIS_StockInOut] ");
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
        public IList<Hi.Model.DIS_StockInOut> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [DIS_StockInOut]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.DIS_StockInOut> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.DIS_StockInOut> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[DIS_StockInOut]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.DIS_StockInOut GetModel(DataRow r)
        {
            Hi.Model.DIS_StockInOut model = new Hi.Model.DIS_StockInOut();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.StockOrderID = SqlHelper.GetInt(r["StockOrderID"]);
            model.GoodsID = SqlHelper.GetInt(r["GoodsID"]);
            model.GoodsInfoID = SqlHelper.GetInt(r["GoodsInfoID"]);
            model.StockNum = SqlHelper.GetDecimal(r["StockNum"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.Batchno = SqlHelper.GetString(r["BatchNO"]);
            model.Validdate = SqlHelper.GetDateTime(r["ValidDate"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.DIS_StockInOut> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.DIS_StockInOut>(ds.Tables[0]);
        }
    }
}

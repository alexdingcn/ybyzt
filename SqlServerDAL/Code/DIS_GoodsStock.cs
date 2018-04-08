//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2018 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2018/1/29 13:10:54
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
    /// 数据访问类 DIS_GoodsStock
    /// </summary>
    public partial class DIS_GoodsStock
    {
        public DIS_GoodsStock()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_GoodsStock model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_GoodsStock](");
            strSql.Append("[DisID],[CompID],[GoodsID],[IsSale],[GoodsInfo],[BatchNO],[validDate],[StockTotalNum],[StockUseNum],[StockNum],[MinAlertNum],[MaxAlertNum],[Price],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@DisID,@CompID,@GoodsID,@IsSale,@GoodsInfo,@BatchNO,@validDate,@StockTotalNum,@StockUseNum,@StockNum,@MinAlertNum,@MaxAlertNum,@Price,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.BigInt),
                    new SqlParameter("@IsSale", SqlDbType.Int),
                    new SqlParameter("@GoodsInfo", SqlDbType.VarChar,200),
                    new SqlParameter("@BatchNO", SqlDbType.VarChar,100),
                    new SqlParameter("@validDate", SqlDbType.DateTime),
                    new SqlParameter("@StockTotalNum", SqlDbType.Decimal),
                    new SqlParameter("@StockUseNum", SqlDbType.Decimal),
                    new SqlParameter("@StockNum", SqlDbType.Decimal),
                    new SqlParameter("@MinAlertNum", SqlDbType.Decimal),
                    new SqlParameter("@MaxAlertNum", SqlDbType.Decimal),
                    new SqlParameter("@Price", SqlDbType.Decimal),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.DisID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.GoodsID;
            parameters[3].Value = model.IsSale;

            if (model.GoodsInfo != null)
                parameters[4].Value = model.GoodsInfo;
            else
                parameters[4].Value = DBNull.Value;


            if (model.BatchNO != null)
                parameters[5].Value = model.BatchNO;
            else
                parameters[5].Value = DBNull.Value;


            if (model.validDate != DateTime.MinValue)
                parameters[6].Value = model.validDate;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.StockTotalNum;
            parameters[8].Value = model.StockUseNum;
            parameters[9].Value = model.StockNum;
            parameters[10].Value = model.MinAlertNum;
            parameters[11].Value = model.MaxAlertNum;
            parameters[12].Value = model.Price;
            parameters[13].Value = model.CreateUserID;
            parameters[14].Value = model.CreateDate;
            parameters[15].Value = model.ts;
            parameters[16].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_GoodsStock model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_GoodsStock] set ");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[GoodsID]=@GoodsID,");
            strSql.Append("[IsSale]=@IsSale,");
            strSql.Append("[GoodsInfo]=@GoodsInfo,");
            strSql.Append("[BatchNO]=@BatchNO,");
            strSql.Append("[validDate]=@validDate,");
            strSql.Append("[StockTotalNum]=@StockTotalNum,");
            strSql.Append("[StockUseNum]=@StockUseNum,");
            strSql.Append("[StockNum]=@StockNum,");
            strSql.Append("[MinAlertNum]=@MinAlertNum,");
            strSql.Append("[MaxAlertNum]=@MaxAlertNum,");
            strSql.Append("[Price]=@Price,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.BigInt),
                    new SqlParameter("@IsSale", SqlDbType.Int),
                    new SqlParameter("@GoodsInfo", SqlDbType.VarChar,200),
                    new SqlParameter("@BatchNO", SqlDbType.VarChar,100),
                    new SqlParameter("@validDate", SqlDbType.DateTime),
                    new SqlParameter("@StockTotalNum", SqlDbType.Decimal),
                    new SqlParameter("@StockUseNum", SqlDbType.Decimal),
                    new SqlParameter("@StockNum", SqlDbType.Decimal),
                    new SqlParameter("@MinAlertNum", SqlDbType.Decimal),
                    new SqlParameter("@MaxAlertNum", SqlDbType.Decimal),
                    new SqlParameter("@Price", SqlDbType.Decimal),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.CompID;
            parameters[3].Value = model.GoodsID;
            parameters[4].Value = model.IsSale;

            if (model.GoodsInfo != null)
                parameters[5].Value = model.GoodsInfo;
            else
                parameters[5].Value = DBNull.Value;


            if (model.BatchNO != null)
                parameters[6].Value = model.BatchNO;
            else
                parameters[6].Value = DBNull.Value;


            if (model.validDate != DateTime.MinValue)
                parameters[7].Value = model.validDate;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.StockTotalNum;
            parameters[9].Value = model.StockUseNum;
            parameters[10].Value = model.StockNum;
            parameters[11].Value = model.MinAlertNum;
            parameters[12].Value = model.MaxAlertNum;
            parameters[13].Value = model.Price;
            parameters[14].Value = model.CreateUserID;
            parameters[15].Value = model.CreateDate;
            parameters[16].Value = model.ts;
            parameters[17].Value = model.dr;
            parameters[18].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [DIS_GoodsStock] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[DIS_GoodsStock]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [DIS_GoodsStock]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.DIS_GoodsStock GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [DIS_GoodsStock] ");
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
        public IList<Hi.Model.DIS_GoodsStock> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [DIS_GoodsStock]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.DIS_GoodsStock> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.DIS_GoodsStock> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[DIS_GoodsStock]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.DIS_GoodsStock GetModel(DataRow r)
        {
            Hi.Model.DIS_GoodsStock model = new Hi.Model.DIS_GoodsStock();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.GoodsID = SqlHelper.GetInt(r["GoodsID"]);
            model.IsSale = SqlHelper.GetInt(r["IsSale"]);
            model.GoodsInfo = SqlHelper.GetString(r["GoodsInfo"]);
            model.BatchNO = SqlHelper.GetString(r["BatchNO"]);
            model.validDate = SqlHelper.GetDateTime(r["validDate"]);
            model.StockTotalNum = SqlHelper.GetDecimal(r["StockTotalNum"]);
            model.StockUseNum = SqlHelper.GetDecimal(r["StockUseNum"]);
            model.StockNum = SqlHelper.GetDecimal(r["StockNum"]);
            model.MinAlertNum = SqlHelper.GetDecimal(r["MinAlertNum"]);
            model.MaxAlertNum = SqlHelper.GetDecimal(r["MaxAlertNum"]);
            model.Price = SqlHelper.GetDecimal(r["Price"]);
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
        private IList<Hi.Model.DIS_GoodsStock> GetList(DataSet ds)
        {
            List<Hi.Model.DIS_GoodsStock> l = new List<Hi.Model.DIS_GoodsStock>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                l.Add(GetModel(r));
            }
            return l;
        }
    }
}

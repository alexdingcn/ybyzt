using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBUtility;
using System.Data;
using System.Data.SqlClient;

namespace Hi.SQLServerDAL
{
    /// <summary>
    /// 数据访问类 DIS_GoodsStock
    /// </summary>
    public partial  class DIS_GoodsStock
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_GoodsStock model, SqlTransaction Tran)
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

            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_GoodsStock model, SqlTransaction Tran)
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

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 修改订单删除商品时，返还库存
        /// </summary>
        /// <param name="str">修改库存的sql</param>
        /// <returns></returns>
        public int GetUpdateStock(string str, SqlConnection sqlconn, SqlTransaction sqltans)
        {
            SqlCommand cmd = new SqlCommand(str, sqlconn, sqltans);

            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());

            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;
        }
    }
}

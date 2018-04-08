using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using DBUtility;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Hi.SQLServerDAL
{
    public partial class DIS_StockChk
    {
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_StockChk model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_StockChk] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[StockOrderID]=@StockOrderID,");
            strSql.Append("[GoodsID]=@GoodsID,");
            strSql.Append("[GoodsInfoID]=@GoodsInfoID,");
            strSql.Append("[StockOldNum]=@StockOldNum,");
            strSql.Append("[StockNum]=@StockNum,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@StockOrderID", SqlDbType.BigInt),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@GoodsInfoID", SqlDbType.BigInt),
                    new SqlParameter("@StockOldNum", SqlDbType.Decimal),
                    new SqlParameter("@StockNum", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1000),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.StockOrderID;
            parameters[3].Value = model.GoodsID;
            parameters[4].Value = model.GoodsInfoID;
            parameters[5].Value = model.StockOldNum;
            parameters[6].Value = model.StockNum;

            if (model.Remark != null)
                parameters[7].Value = model.Remark;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.CreateUserID;
            parameters[9].Value = model.CreateDate;
            parameters[10].Value = model.ts;
            parameters[11].Value = model.dr;
            parameters[12].Value = model.modifyuser;
            if (tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [DIS_StockChk] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;

            if (tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_StockChk model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_StockChk](");
            strSql.Append("[CompID],[StockOrderID],[GoodsID],[GoodsInfoID],[StockOldNum],[StockNum],[Remark],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@StockOrderID,@GoodsID,@GoodsInfoID,@StockOldNum,@StockNum,@Remark,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@StockOrderID", SqlDbType.BigInt),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@GoodsInfoID", SqlDbType.BigInt),
                    new SqlParameter("@StockOldNum", SqlDbType.Decimal),
                    new SqlParameter("@StockNum", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1000),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.StockOrderID;
            parameters[2].Value = model.GoodsID;
            parameters[3].Value = model.GoodsInfoID;
            parameters[4].Value = model.StockOldNum;
            parameters[5].Value = model.StockNum;

            if (model.Remark != null)
                parameters[6].Value = model.Remark;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.CreateUserID;
            parameters[8].Value = model.CreateDate;
            parameters[9].Value = model.ts;
            parameters[10].Value = model.modifyuser;
            if (tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

    }
}

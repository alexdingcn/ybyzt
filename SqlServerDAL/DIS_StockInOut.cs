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
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_StockInOut model, SqlTransaction tran)
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
            strSql.Append("delete [DIS_StockInOut] ");
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
        public int Add(Hi.Model.DIS_StockInOut model, SqlTransaction tran)
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

            if (tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }


        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public DataTable GetListPage(int pageSize, int pageIndex, string where)
        {
            string strSql = string.Format(@" select * from (select ROW_NUMBER()over(order by ChkDate desc) as rw, BD_GoodsInfo.BarCode,BD_Goods.GoodsName,BD_GoodsInfo.ValueInfo,BD_Goods.Unit,DIS_StockOrder.CreateDate,DIS_StockOrder.Type,DIS_StockOrder.OrderNO,DIS_StockOrder.ChkDate,DIS_StockInOut.StockNum,DIS_StockOrder.ID from DIS_StockInOut JOIN DIS_StockOrder ON DIS_StockInOut.StockOrderID=DIS_StockOrder.ID JOIN BD_GoodsInfo ON DIS_StockInOut.GoodsInfoID=BD_GoodsInfo.ID JOIN BD_Goods ON BD_GoodsInfo.GoodsID=BD_Goods.ID where {0} ) a where a.rw between({1} - 1) * {2} + 1 and {3} * {4} order by ChkDate desc", where, pageIndex, pageSize, pageIndex, pageSize);
            return SqlHelper.GetTable(SqlHelper.LocalSqlServer, strSql);
        }

        /// <summary>
        /// 执行sql语局获取数据
        /// </summary>
        public DataTable GetDataTable(string sql)
        {
            return SqlHelper.GetTable(SqlHelper.LocalSqlServer, sql);
        }


        /// <summary>
        /// 获取总行数
        /// </summary>
        /// <returns></returns>
        public int GetPageCount(string where)
        {
            string strSql = string.Format("select dbo.DIS_StockInOut.ID FROM DIS_StockInOut JOIN DIS_StockOrder ON DIS_StockInOut.StockOrderID=DIS_StockOrder.ID JOIN BD_GoodsInfo ON DIS_StockInOut.GoodsInfoID=BD_GoodsInfo.ID JOIN BD_Goods ON BD_GoodsInfo.GoodsID=BD_Goods.ID where {0}", where);
            DataTable dt = SqlHelper.GetTable(SqlHelper.LocalSqlServer, strSql);
            return dt.Rows.Count;
        }

    }
}

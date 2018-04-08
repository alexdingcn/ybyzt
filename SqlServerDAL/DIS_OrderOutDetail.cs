using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class DIS_OrderOutDetail
    {
         /// <summary>
        /// 增加一条数据,带事务
        /// </summary>
        public int Add(Hi.Model.DIS_OrderOutDetail model, SqlTransaction TranSaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_OrderOutDetail](");
            strSql.Append("[OrderOutID],[DisID],[GoodsinfoID],[OrderID],[OutNum],[SignNum],[Remark],[ts],[dr],[modifyuser],[BatchNO],[validDate],[StorageNum])");
            strSql.Append(" values (");
            strSql.Append("@OrderOutID,@DisID,@GoodsinfoID,@OrderID,@OutNum,@SignNum,@Remark,@ts,@dr,@modifyuser,@BatchNO,@validDate,@StorageNum)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderOutID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@GoodsinfoID", SqlDbType.BigInt),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@OutNum", SqlDbType.Decimal),
                    new SqlParameter("@SignNum", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,300),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@BatchNO", SqlDbType.VarChar),
                    new SqlParameter("@validDate", SqlDbType.DateTime),
                    new SqlParameter("@StorageNum", SqlDbType.Decimal)
            };
            parameters[0].Value = model.OrderOutID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.GoodsinfoID;
            parameters[3].Value = model.OrderID;
            parameters[4].Value = model.OutNum;
            parameters[5].Value = model.SignNum;

            if (model.Remark != null)
                parameters[6].Value = model.Remark;
            else
                parameters[6].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[7].Value = model.ts;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.dr;
            parameters[9].Value = model.modifyuser;

            parameters[10].Value = model.Batchno;

            if (model.Validdate != DateTime.MinValue)
                parameters[11].Value = model.Validdate;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.StorageNum;

            if (TranSaction != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), TranSaction, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 修改一条数据,带事务
        /// </summary>
        /// <param name="model"></param>
        /// <param name="TranSaction"></param>
        /// <returns></returns>
        public int Update(Hi.Model.DIS_OrderOutDetail model, SqlTransaction TranSaction)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_OrderOutDetail] set ");
            strSql.Append("[OrderOutID]=@OrderOutID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[GoodsinfoID]=@GoodsinfoID,");
            strSql.Append("[OrderID]=@OrderID,");
            strSql.Append("[OutNum]=@OutNum,");
            strSql.Append("[SignNum]=@SignNum,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[BatchNO]=@BatchNO,");
            strSql.Append("[validDate]=@validDate,");
            strSql.Append("[StorageNum]=@StorageNum");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@OrderOutID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@GoodsinfoID", SqlDbType.BigInt),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@OutNum", SqlDbType.Decimal),
                    new SqlParameter("@SignNum", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,300),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@BatchNO", SqlDbType.VarChar),
                    new SqlParameter("@validDate", SqlDbType.DateTime),
                    new SqlParameter("@StorageNum", SqlDbType.Decimal)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.OrderOutID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.GoodsinfoID;
            parameters[4].Value = model.OrderID;
            parameters[5].Value = model.OutNum;
            parameters[6].Value = model.SignNum;

            if (model.Remark != null)
                parameters[7].Value = model.Remark;
            else
                parameters[7].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[8].Value = model.ts;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.dr;
            parameters[10].Value = model.modifyuser;

            parameters[11].Value = model.Batchno;

            if (model.Validdate != DateTime.MinValue)
                parameters[12].Value = model.Validdate;
            else
                parameters[12].Value = DBNull.Value;

            parameters[13].Value = model.StorageNum;

            if (TranSaction != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), TranSaction, parameters);
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 查询订单商品明细
        /// </summary>
        /// <param name="strWhat"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetOrderOutDe(string strWhat, string strWhere)
        {
            string sql = "select ";
            if (strWhat != "")
                sql += strWhat;
            else
                sql += " od.*,od.OutNum odOutNum,o.OutNum oNum,o.*,g.Pic,info.GoodsID";

            sql += " from DIS_OrderOutDetail od left join DIS_OrderDetail o on od.GoodsinfoID=o.GoodsinfoID and od.OrderID=o.OrderID left join  BD_Goodsinfo info on o.goodsInfoID=info.ID left join BD_Goods g on info.GoodsID=g.ID";

            if (strWhere != "")
                sql += " where " + strWhere;

            return SqlHelper.Query(SqlHelper.LocalSqlServer, sql.ToString()).Tables[0];

        }
    }
}

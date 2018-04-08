using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DBUtility;
using System.Data.SqlClient;

namespace Hi.SQLServerDAL
{
    public partial class DIS_OrderDetail
    {
        public DataSet GetDs(string orderid)
        {
            string sqlstr = string.Format("select goodsinfoid from DIS_OrderDetail where orderid={0} and dr=0", orderid);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr);
        }

        public Hi.Model.DIS_OrderDetail GetModel(string orderid, string goodsid)
        {
            string sqlstr = string.Format("select * from DIS_OrderDetail where orderid={0} and goodsinfoid={1} and dr=0", orderid, goodsid);
            if (SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr).Tables[0].Rows.Count == 1)
            {
                return GetModel(SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr).Tables[0].Rows[0]);
            }
            return null;
        }

        /// <summary>
        /// 删除订单商品明细
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetDel(string str, SqlConnection sqlconn, SqlTransaction sqltans)
        {
            string sqlstr = string.Format("delete DIS_OrderDetail where {0}", str);

            SqlCommand cmd = new SqlCommand(sqlstr, sqlconn, sqltans);

            // int rowsAffected = SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, sqlstr);
            //return rowsAffected;

            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());

            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;
        }

        /// <summary>
        /// 修改订单删除商品时，返还库存
        /// </summary>
        /// <param name="str">修改库存的sql</param>
        /// <returns></returns>
        public int GetUpdateInventory(string str, SqlConnection sqlconn, SqlTransaction sqltans)
        {
            SqlCommand cmd = new SqlCommand(str, sqlconn, sqltans);

            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());

            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;
        }

        /// <summary>
        /// 删除订单商品明细
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public int GetDel(SqlConnection sqlconn, string str, SqlTransaction sqltans)
        {
            string sqlstr = string.Format("delete DIS_OrderDetail where {0}", str);

            SqlCommand cmd = new SqlCommand(sqlstr.ToString(), sqlconn, sqltans);
            cmd.CommandType = CommandType.Text;

            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());
            return rowsAffected;
        }

        /// <summary>
        /// 订单明细与商品的连接查询
        /// </summary>
        /// <returns></returns>
        public DataTable GetOrderDetail(string strWhere, string strOrderby)
        {
            StringBuilder strSql = new StringBuilder("select od.[ID],od.[DisID],od.[OrderID],od.[GoodsInfoID],od.[GoodsinfoID],od.[GoodsInfos],od.[GoodsNum],od.[Price],od.[AuditAmount],od.[sumAmount],od.[Remark],od.[vdef1],od.[vdef2],g.[GoodsName],g.[Pic],g.[Unit] from DIS_OrderDetail as od left join BD_GoodsInfo as gd on od.[GoodsInfoID]=gd.ID left join BD_Goods as g on gd.GoodsID=g.ID");

            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);

            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0];
        }

        /// <summary>
        /// 修改单个商品信息
        /// </summary>
        /// <param name="strWhat">修改的信息</param>
        /// <param name="parameters">修改的参数</param>
        /// <param name="strWhere">修改条件</param>
        /// <returns></returns>
        public bool UpdateOrderDetail(string strWhat, SqlParameter[] parameters, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_OrderDetail] set ");
            strSql.Append(strWhat);
            strSql.Append(" where " + strWhere);

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 查询订单商品明细
        /// </summary>
        /// <param name="strWhat"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetOrderDe(string strWhat, string strWhere)
        {
            string sql = "select ";
            if (strWhat != "")
                sql += strWhat;
            else
                sql += " o.*,g.Pic,info.GoodsID,info.BatchNO,info.validDate";

            sql += " from DIS_OrderDetail o left join  BD_Goodsinfo info on o.goodsInfoID=info.ID left join BD_Goods g on info.GoodsID=g.ID";

            if (strWhere != "")
                sql +=" where "+ strWhere;

            return SqlHelper.Query(SqlHelper.LocalSqlServer, sql.ToString()).Tables[0];

        }
    }
}

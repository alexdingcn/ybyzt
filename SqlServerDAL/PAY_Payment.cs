using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class PAY_Payment
    {
        /// <summary>
        /// 支付成功，修改支付表状态
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="orderid"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int updatePayState(SqlConnection sqlconn, int payid, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [pay_payment] set [isaudit]=1 where  [ID] = @payid and isaudit<>1 and isnull(dr,0)=0");
            SqlParameter[] parameters = { new SqlParameter("@payid", SqlDbType.Int) };
            parameters[0].Value = payid;

            SqlCommand cmd = new SqlCommand(strSql.ToString(), sqlconn, sqltans);
            cmd.CommandType = CommandType.Text;

            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    if (parameter.SqlDbType == SqlDbType.DateTime)
                    {
                        if (parameter.Value == DBNull.Value)
                        {
                            parameter.Value = DBNull.Value;
                        }

                    }
                    cmd.Parameters.Add(parameter);
                }
            }
            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());

            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;
        }


        /// <summary>
        /// 线下支付作废，修改支付表状态
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="orderid"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int updatePayToVoid(SqlConnection sqlconn, int payid, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [pay_payment] set [isaudit]=3 where  [ID] = @payid and isaudit<>2 and isnull(dr,0)=0");
            SqlParameter[] parameters = { new SqlParameter("@payid", SqlDbType.Int) };
            parameters[0].Value = payid;

            SqlCommand cmd = new SqlCommand(strSql.ToString(), sqlconn, sqltans);
            cmd.CommandType = CommandType.Text;

            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    if (parameter.SqlDbType == SqlDbType.DateTime)
                    {
                        if (parameter.Value == DBNull.Value)
                        {
                            parameter.Value = DBNull.Value;
                        }

                    }
                    cmd.Parameters.Add(parameter);
                }
            }
            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());

            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;
        }

        /// <summary>
        /// 结算成功，修改支付表的结算状态
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="orderid"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int updatePayState_JS(SqlConnection sqlconn, int payid, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [pay_payment] set [PrintNum]=1 where  [ID] = @payid ");
            SqlParameter[] parameters = { new SqlParameter("@payid", SqlDbType.Int) };
            parameters[0].Value = payid;

            SqlCommand cmd = new SqlCommand(strSql.ToString(), sqlconn, sqltans);
            cmd.CommandType = CommandType.Text;

            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    if (parameter.SqlDbType == SqlDbType.DateTime)
                    {
                        if (parameter.Value == DBNull.Value)
                        {
                            parameter.Value = DBNull.Value;
                        }

                    }
                    cmd.Parameters.Add(parameter);
                }
            }
            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());

            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;
        }


        /// <summary>
        /// 结算成功，修改订单的支付状态，已结算，部分结算状态
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="id"></param>
        /// <param name="sumPrice"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int UpdateOrderPaystate_JS(SqlConnection sqlconn, int ordid, string vdef9, SqlTransaction sqltans)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_Order] set [IsSettl]=@vdef9  where  [ID] = @ID ");
            SqlParameter[] parameters = { new SqlParameter("@vdef9", SqlDbType.VarChar, 50), new SqlParameter("@ID", SqlDbType.Int) };
            parameters[0].Value = vdef9;
            parameters[1].Value = ordid;

            SqlCommand cmd = new SqlCommand(strSql.ToString(), sqlconn, sqltans);
            cmd.CommandType = CommandType.Text;

            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    if (parameter.SqlDbType == SqlDbType.DateTime)
                    {
                        if (parameter.Value == DBNull.Value)
                        {
                            parameter.Value = DBNull.Value;
                        }

                    }
                    cmd.Parameters.Add(parameter);
                }
            }

            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());

            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;

        }

        /// <summary>
        /// 结算成功，修改企业钱包表状态，已结算，部分结算状态
        /// </summary>
        /// <param name="sqlconn"></param>
        /// <param name="id"></param>
        /// <param name="sumPrice"></param>
        /// <param name="sqltans"></param>
        /// <returns></returns>
        public int UpdatePrePaystate_JS(SqlConnection sqlconn, int preid, string vdef2, SqlTransaction sqltans)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [PAY_PrePayment] set [vdef2]=@vdef2  where  [ID] = @ID ");
            SqlParameter[] parameters = { new SqlParameter("@vdef2", SqlDbType.VarChar, 50), new SqlParameter("@ID", SqlDbType.Int) };
            parameters[0].Value = vdef2;
            parameters[1].Value = preid;

            SqlCommand cmd = new SqlCommand(strSql.ToString(), sqlconn, sqltans);
            cmd.CommandType = CommandType.Text;

            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    if (parameter.SqlDbType == SqlDbType.DateTime)
                    {
                        if (parameter.Value == DBNull.Value)
                        {
                            parameter.Value = DBNull.Value;
                        }

                    }
                    cmd.Parameters.Add(parameter);
                }
            }

            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());

            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;

        }



    }
}

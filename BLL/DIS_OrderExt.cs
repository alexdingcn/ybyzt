using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
namespace Hi.BLL
{
    public partial class DIS_OrderExt
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(SqlConnection sqlconn, Hi.Model.DIS_OrderExt model, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_OrderExt](");
            strSql.Append("[OrderID],[DisAccID],[Rise],[Content],[OBank],[OAccount],[TRNumber],[BillNo],[IsBill],[IsOBill],[ProID],[ProAmount],[ProDID],[Protype],[vdef1],[vdef2],[vdef3],[vdef4],[vdef5],[vdef6],[vdef7])");
            strSql.Append(" values (");
            strSql.Append("@OrderID,@DisAccID,@Rise,@Content,@OBank,@OAccount,@TRNumber,@BillNo,@IsBill,@IsOBill,@ProID,@ProAmount,@ProDID,@Protype,@vdef1,@vdef2,@vdef3,@vdef4,@vdef5,@vdef6,@vdef7)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisAccID", SqlDbType.NChar,10),
                    new SqlParameter("@Rise", SqlDbType.VarChar,100),
                    new SqlParameter("@Content", SqlDbType.VarChar,200),
                    new SqlParameter("@OBank", SqlDbType.VarChar,100),
                    new SqlParameter("@OAccount", SqlDbType.VarChar,100),
                    new SqlParameter("@TRNumber", SqlDbType.VarChar,100),
                    new SqlParameter("@BillNo", SqlDbType.VarChar,50),
                    new SqlParameter("@IsBill", SqlDbType.Int),
                    new SqlParameter("@IsOBill", SqlDbType.Int),
                    new SqlParameter("@ProID", SqlDbType.Int),
                    new SqlParameter("@ProAmount", SqlDbType.Decimal),
                    new SqlParameter("@ProDID", SqlDbType.Int),
                    new SqlParameter("@Protype", SqlDbType.VarChar,134),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef4", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef5", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef6", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef7", SqlDbType.VarChar,128)
            };
            parameters[0].Value = model.OrderID;

            if (model.DisAccID != null)
                parameters[1].Value = model.DisAccID;
            else
                parameters[1].Value = DBNull.Value;


            if (model.Rise != null)
                parameters[2].Value = model.Rise;
            else
                parameters[2].Value = DBNull.Value;


            if (model.Content != null)
                parameters[3].Value = model.Content;
            else
                parameters[3].Value = DBNull.Value;


            if (model.OBank != null)
                parameters[4].Value = model.OBank;
            else
                parameters[4].Value = DBNull.Value;


            if (model.OAccount != null)
                parameters[5].Value = model.OAccount;
            else
                parameters[5].Value = DBNull.Value;


            if (model.TRNumber != null)
                parameters[6].Value = model.TRNumber;
            else
                parameters[6].Value = DBNull.Value;


            if (model.BillNo != null)
                parameters[7].Value = model.BillNo;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.IsBill;
            parameters[9].Value = model.IsOBill;
            parameters[10].Value = model.ProID;
            parameters[11].Value = model.ProAmount;
            parameters[12].Value = model.ProDID;

            if (model.Protype != null)
                parameters[13].Value = model.Protype;
            else
                parameters[13].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[14].Value = model.vdef1;
            else
                parameters[14].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[15].Value = model.vdef2;
            else
                parameters[15].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[16].Value = model.vdef3;
            else
                parameters[16].Value = DBNull.Value;


            if (model.vdef4 != null)
                parameters[17].Value = model.vdef4;
            else
                parameters[17].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[18].Value = model.vdef5;
            else
                parameters[18].Value = DBNull.Value;


            if (model.vdef6 != null)
                parameters[19].Value = model.vdef6;
            else
                parameters[19].Value = DBNull.Value;


            if (model.vdef7 != null)
                parameters[20].Value = model.vdef7;
            else
                parameters[20].Value = DBNull.Value;

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
            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery());

            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;
            //if (SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0)
            //    return model.ID;
            //else
            //    return 0;
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(SqlConnection sqlconn, Hi.Model.DIS_OrderExt model, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_OrderExt] set ");
            strSql.Append("[OrderID]=@OrderID,");
            strSql.Append("[DisAccID]=@DisAccID,");
            strSql.Append("[Rise]=@Rise,");
            strSql.Append("[Content]=@Content,");
            strSql.Append("[OBank]=@OBank,");
            strSql.Append("[OAccount]=@OAccount,");
            strSql.Append("[TRNumber]=@TRNumber,");
            strSql.Append("[BillNo]=@BillNo,");
            strSql.Append("[IsBill]=@IsBill,");
            strSql.Append("[IsOBill]=@IsOBill,");
            strSql.Append("[ProID]=@ProID,");
            strSql.Append("[ProAmount]=@ProAmount,");
            strSql.Append("[ProDID]=@ProDID,");
            strSql.Append("[Protype]=@Protype,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[vdef4]=@vdef4,");
            strSql.Append("[vdef5]=@vdef5,");
            strSql.Append("[vdef6]=@vdef6,");
            strSql.Append("[vdef7]=@vdef7");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisAccID", SqlDbType.NChar,10),
                    new SqlParameter("@Rise", SqlDbType.VarChar,100),
                    new SqlParameter("@Content", SqlDbType.VarChar,200),
                    new SqlParameter("@OBank", SqlDbType.VarChar,100),
                    new SqlParameter("@OAccount", SqlDbType.VarChar,100),
                    new SqlParameter("@TRNumber", SqlDbType.VarChar,100),
                    new SqlParameter("@BillNo", SqlDbType.VarChar,50),
                    new SqlParameter("@IsBill", SqlDbType.Int),
                    new SqlParameter("@IsOBill", SqlDbType.Int),
                    new SqlParameter("@ProID", SqlDbType.Int),
                    new SqlParameter("@ProAmount", SqlDbType.Decimal),
                    new SqlParameter("@ProDID", SqlDbType.Int),
                    new SqlParameter("@Protype", SqlDbType.VarChar,134),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef4", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef5", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef6", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef7", SqlDbType.VarChar,128)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.OrderID;

            if (model.DisAccID != null)
                parameters[2].Value = model.DisAccID;
            else
                parameters[2].Value = DBNull.Value;


            if (model.Rise != null)
                parameters[3].Value = model.Rise;
            else
                parameters[3].Value = DBNull.Value;


            if (model.Content != null)
                parameters[4].Value = model.Content;
            else
                parameters[4].Value = DBNull.Value;


            if (model.OBank != null)
                parameters[5].Value = model.OBank;
            else
                parameters[5].Value = DBNull.Value;


            if (model.OAccount != null)
                parameters[6].Value = model.OAccount;
            else
                parameters[6].Value = DBNull.Value;


            if (model.TRNumber != null)
                parameters[7].Value = model.TRNumber;
            else
                parameters[7].Value = DBNull.Value;


            if (model.BillNo != null)
                parameters[8].Value = model.BillNo;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.IsBill;
            parameters[10].Value = model.IsOBill;
            parameters[11].Value = model.ProID;
            parameters[12].Value = model.ProAmount;
            parameters[13].Value = model.ProDID;

            if (model.Protype != null)
                parameters[14].Value = model.Protype;
            else
                parameters[14].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[15].Value = model.vdef1;
            else
                parameters[15].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[16].Value = model.vdef2;
            else
                parameters[16].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[17].Value = model.vdef3;
            else
                parameters[17].Value = DBNull.Value;


            if (model.vdef4 != null)
                parameters[18].Value = model.vdef4;
            else
                parameters[18].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[19].Value = model.vdef5;
            else
                parameters[19].Value = DBNull.Value;


            if (model.vdef6 != null)
                parameters[20].Value = model.vdef6;
            else
                parameters[20].Value = DBNull.Value;


            if (model.vdef7 != null)
                parameters[21].Value = model.vdef7;
            else
                parameters[21].Value = DBNull.Value;
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
            return rowsAffected > 0;
            //return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

    }
}

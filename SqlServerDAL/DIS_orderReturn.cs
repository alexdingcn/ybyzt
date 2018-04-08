using System;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using System.Data;
using System.Data.SqlClient;

namespace Hi.SQLServerDAL
{
    public partial class DIS_OrderReturn
    {
        public Hi.Model.DIS_OrderReturn GetModel(string orderid)
        {
            string sqlstr = "select * from DIS_OrderReturn where dr=0 and orderid=" + orderid;
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return GetModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }

        /// <summary>
        /// 增加一条数据,带事务
        /// </summary>
        public int Add(SqlConnection sqlconn, Hi.Model.DIS_OrderReturn model, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_OrderReturn](");
            strSql.Append("[ReceiptNo],[OrderID],[DisID],[CompID],[ReturnDate],[ReturnUserID],[ReturnContent],[Express],[ExpressNo],[ReturnState],[AuditUserID],[AuditDate],[AuditRemark],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@ReceiptNo,@OrderID,@DisID,@CompID,@ReturnDate,@ReturnUserID,@ReturnContent,@Express,@ExpressNo,@ReturnState,@AuditUserID,@AuditDate,@AuditRemark,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@ReturnDate", SqlDbType.DateTime),
                    new SqlParameter("@ReturnUserID", SqlDbType.Int),
                    new SqlParameter("@ReturnContent", SqlDbType.Text),
                    new SqlParameter("@Express", SqlDbType.VarChar,50),
                    new SqlParameter("@ExpressNo", SqlDbType.VarChar,50),
                    new SqlParameter("@ReturnState", SqlDbType.Int),
                    new SqlParameter("@AuditUserID", SqlDbType.Int),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@AuditRemark", SqlDbType.VarChar,800),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };

            if (model.ReceiptNo != null)
                parameters[0].Value = model.ReceiptNo;
            else
                parameters[0].Value = DBNull.Value;

            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.CompID;

            if (model.ReturnDate != DateTime.MinValue)
                parameters[4].Value = model.ReturnDate;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.ReturnUserID;
            parameters[6].Value = model.ReturnContent;

            if (model.Express != null)
                parameters[7].Value = model.Express;
            else
                parameters[7].Value = DBNull.Value;


            if (model.ExpressNo != null)
                parameters[8].Value = model.ExpressNo;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.ReturnState;
            parameters[10].Value = model.AuditUserID;

            if (model.AuditDate != DateTime.MinValue)
                parameters[11].Value = model.AuditDate;
            else
                parameters[11].Value = DBNull.Value;


            if (model.AuditRemark != null)
                parameters[12].Value = model.AuditRemark;
            else
                parameters[12].Value = DBNull.Value;

            parameters[13].Value = model.CreateUserID;
            parameters[14].Value = model.CreateDate;
            parameters[15].Value = model.ts;
            parameters[16].Value = model.modifyuser;

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
            int rowsAffected = SqlHelper.GetInt(cmd.ExecuteScalar().ToString());

            if (rowsAffected > 0)
                return rowsAffected;
            else
                return 0;
        }

        /// <summary>
        /// 更新一条数据,带事务
        /// </summary>
        public int Update(SqlConnection sqlconn,Hi.Model.DIS_OrderReturn model, SqlTransaction sqltans)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_OrderReturn] set ");
            strSql.Append("[ReceiptNo]=@ReceiptNo,");
            strSql.Append("[OrderID]=@OrderID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[ReturnDate]=@ReturnDate,");
            strSql.Append("[ReturnUserID]=@ReturnUserID,");
            strSql.Append("[ReturnContent]=@ReturnContent,");
            strSql.Append("[Express]=@Express,");
            strSql.Append("[ExpressNo]=@ExpressNo,");
            strSql.Append("[ReturnState]=@ReturnState,");
            strSql.Append("[AuditUserID]=@AuditUserID,");
            strSql.Append("[AuditDate]=@AuditDate,");
            strSql.Append("[AuditRemark]=@AuditRemark,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@ReturnDate", SqlDbType.DateTime),
                    new SqlParameter("@ReturnUserID", SqlDbType.Int),
                    new SqlParameter("@ReturnContent", SqlDbType.Text),
                    new SqlParameter("@Express", SqlDbType.VarChar,50),
                    new SqlParameter("@ExpressNo", SqlDbType.VarChar,50),
                    new SqlParameter("@ReturnState", SqlDbType.Int),
                    new SqlParameter("@AuditUserID", SqlDbType.Int),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@AuditRemark", SqlDbType.VarChar,800),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;

            if (model.ReceiptNo != null)
                parameters[1].Value = model.ReceiptNo;
            else
                parameters[1].Value = DBNull.Value;

            parameters[2].Value = model.OrderID;
            parameters[3].Value = model.DisID;
            parameters[4].Value = model.CompID;

            if (model.ReturnDate != DateTime.MinValue)
                parameters[5].Value = model.ReturnDate;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.ReturnUserID;
            parameters[7].Value = model.ReturnContent;

            if (model.Express != null)
                parameters[8].Value = model.Express;
            else
                parameters[8].Value = DBNull.Value;


            if (model.ExpressNo != null)
                parameters[9].Value = model.ExpressNo;
            else
                parameters[9].Value = DBNull.Value;

            parameters[10].Value = model.ReturnState;
            parameters[11].Value = model.AuditUserID;

            if (model.AuditDate != DateTime.MinValue)
                parameters[12].Value = model.AuditDate;
            else
                parameters[12].Value = DBNull.Value;


            if (model.AuditRemark != null)
                parameters[13].Value = model.AuditRemark;
            else
                parameters[13].Value = DBNull.Value;

            parameters[14].Value = model.CreateUserID;
            parameters[15].Value = model.CreateDate;
            parameters[16].Value = model.ts;
            parameters[17].Value = model.dr;
            parameters[18].Value = model.modifyuser;

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

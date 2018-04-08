using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class DIS_Logistics
    {

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_Logistics model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_Logistics] set ");
            strSql.Append("[OrderID]=@OrderID,");
            strSql.Append("[OrderOutID]=@OrderOutID,");
            strSql.Append("[ComPName]=@ComPName,");
            strSql.Append("[LogisticsNo]=@LogisticsNo,");
            strSql.Append("[CarUser]=@CarUser,");
            strSql.Append("[CarNo]=@CarNo,");
            strSql.Append("[Car]=@Car,");
            strSql.Append("[Context]=@Context,");
            strSql.Append("[Type]=@Type,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [Id]=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@OrderOutID", SqlDbType.BigInt),
                    new SqlParameter("@ComPName", SqlDbType.VarChar,50),
                    new SqlParameter("@LogisticsNo", SqlDbType.VarChar,50),
                    new SqlParameter("@CarUser", SqlDbType.VarChar,50),
                    new SqlParameter("@CarNo", SqlDbType.VarChar,50),
                    new SqlParameter("@Car", SqlDbType.VarChar,50),
                    new SqlParameter("@Context", SqlDbType.Text),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.Int),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.Id;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.OrderOutID;

            if (model.ComPName != null)
                parameters[3].Value = model.ComPName;
            else
                parameters[3].Value = DBNull.Value;


            if (model.LogisticsNo != null)
                parameters[4].Value = model.LogisticsNo;
            else
                parameters[4].Value = DBNull.Value;


            if (model.CarUser != null)
                parameters[5].Value = model.CarUser;
            else
                parameters[5].Value = DBNull.Value;


            if (model.CarNo != null)
                parameters[6].Value = model.CarNo;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Car != null)
                parameters[7].Value = model.Car;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Context != null)
                parameters[8].Value = model.Context;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.Type;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.CreateDate;

            if (model.ts != DateTime.MinValue)
                parameters[12].Value = model.ts;
            else
                parameters[12].Value = DBNull.Value;

            parameters[13].Value = model.dr;
            parameters[14].Value = model.modifyuser;

            //return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;

        }
        public int Add(Hi.Model.DIS_Logistics model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_Logistics](");
            strSql.Append("[OrderID],[OrderOutID],[ComPName],[LogisticsNo],[CarUser],[CarNo],[Car],[Context],[Type],[CreateUserID],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@OrderID,@OrderOutID,@ComPName,@LogisticsNo,@CarUser,@CarNo,@Car,@Context,@Type,@CreateUserID,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@OrderOutID", SqlDbType.BigInt),
                    new SqlParameter("@ComPName", SqlDbType.VarChar,50),
                    new SqlParameter("@LogisticsNo", SqlDbType.VarChar,50),
                    new SqlParameter("@CarUser", SqlDbType.VarChar,50),
                    new SqlParameter("@CarNo", SqlDbType.VarChar,50),
                    new SqlParameter("@Car", SqlDbType.VarChar,50),
                    new SqlParameter("@Context", SqlDbType.Text),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.OrderID;
            parameters[1].Value = model.OrderOutID;

            if (model.ComPName != null)
                parameters[2].Value = model.ComPName;
            else
                parameters[2].Value = DBNull.Value;


            if (model.LogisticsNo != null)
                parameters[3].Value = model.LogisticsNo;
            else
                parameters[3].Value = DBNull.Value;


            if (model.CarUser != null)
                parameters[4].Value = model.CarUser;
            else
                parameters[4].Value = DBNull.Value;


            if (model.CarNo != null)
                parameters[5].Value = model.CarNo;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Car != null)
                parameters[6].Value = model.Car;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Context != null)
                parameters[7].Value = model.Context;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.Type;
            parameters[9].Value = model.CreateUserID;

            if (model.ts != DateTime.MinValue)
                parameters[10].Value = model.ts;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.modifyuser;
            // return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));

        }

    }
}

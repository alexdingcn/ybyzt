using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class BD_DisPriceInfo
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_DisPriceInfo model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_DisPriceInfo](");
            strSql.Append("[DisPriceID],[CompID],[GoodsInfoID],[GoodsName],[BarCode],[InfoValue],[Unit],[TinkerPrice],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@DisPriceID,@CompID,@GoodsInfoID,@GoodsName,@BarCode,@InfoValue,@Unit,@TinkerPrice,@IsEnabled,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@DisPriceID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsInfoID", SqlDbType.Int),
                    new SqlParameter("@GoodsName", SqlDbType.NVarChar,100),
                    new SqlParameter("@BarCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@InfoValue", SqlDbType.NVarChar,300),
                    new SqlParameter("@Unit", SqlDbType.NVarChar,30),
                    new SqlParameter("@TinkerPrice", SqlDbType.Decimal),
                    new SqlParameter("@IsEnabled", SqlDbType.Bit),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.DisPriceID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.GoodsInfoID;

            if (model.GoodsName != null)
                parameters[3].Value = model.GoodsName;
            else
                parameters[3].Value = DBNull.Value;


            if (model.BarCode != null)
                parameters[4].Value = model.BarCode;
            else
                parameters[4].Value = DBNull.Value;


            if (model.InfoValue != null)
                parameters[5].Value = model.InfoValue;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Unit != null)
                parameters[6].Value = model.Unit;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.TinkerPrice;
            parameters[8].Value = model.IsEnabled;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.CreateDate;
            parameters[11].Value = model.ts;
            parameters[12].Value = model.modifyuser;
            // return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));

        }
        /// <summary>
        /// 删除多条数据
        /// </summary>
        public bool Delete(int ID, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_DisPriceInfo] ");
            strSql.Append(" where [DisPriceID]=@DisPriceID");
            SqlParameter[] parameters = {
                    new SqlParameter("@DisPriceID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;

            // return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_DisPriceInfo model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_DisPriceInfo] set ");
            strSql.Append("[DisPriceID]=@DisPriceID,");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[GoodsInfoID]=@GoodsInfoID,");
            strSql.Append("[GoodsName]=@GoodsName,");
            strSql.Append("[BarCode]=@BarCode,");
            strSql.Append("[InfoValue]=@InfoValue,");
            strSql.Append("[Unit]=@Unit,");
            strSql.Append("[TinkerPrice]=@TinkerPrice,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@DisPriceID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsInfoID", SqlDbType.Int),
                    new SqlParameter("@GoodsName", SqlDbType.NVarChar,100),
                    new SqlParameter("@BarCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@InfoValue", SqlDbType.NVarChar,300),
                    new SqlParameter("@Unit", SqlDbType.NVarChar,30),
                    new SqlParameter("@TinkerPrice", SqlDbType.Decimal),
                    new SqlParameter("@IsEnabled", SqlDbType.Bit),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.DisPriceID;
            parameters[2].Value = model.CompID;
            parameters[3].Value = model.GoodsInfoID;

            if (model.GoodsName != null)
                parameters[4].Value = model.GoodsName;
            else
                parameters[4].Value = DBNull.Value;


            if (model.BarCode != null)
                parameters[5].Value = model.BarCode;
            else
                parameters[5].Value = DBNull.Value;


            if (model.InfoValue != null)
                parameters[6].Value = model.InfoValue;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Unit != null)
                parameters[7].Value = model.Unit;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.TinkerPrice;
            parameters[9].Value = model.IsEnabled;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.CreateDate;
            parameters[12].Value = model.ts;
            parameters[13].Value = model.dr;
            parameters[14].Value = model.modifyuser;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;

           // return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_DisPriceInfo GetModel(int ID, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_DisPriceInfo] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            DataSet ds = SqlHelper.Query(strSql.ToString(), Tran, parameters);
           // DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
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
    }
}

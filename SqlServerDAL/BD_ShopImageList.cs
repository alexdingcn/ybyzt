using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class BD_ShopImageList
    {

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_ShopImageList model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_ShopImageList](");
            strSql.Append("[CompID],[Type],[ImageUrl],[ImageName],[ImageTitle],[GoodsID],[GoodsUrl],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@Type,@ImageUrl,@ImageName,@ImageTitle,@GoodsID,@GoodsUrl,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@ImageUrl", SqlDbType.VarChar,150),
                    new SqlParameter("@ImageName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ImageTitle", SqlDbType.NVarChar,50),
                    new SqlParameter("@GoodsID", SqlDbType.BigInt),
                    new SqlParameter("@GoodsUrl", SqlDbType.VarChar,150),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.Type;
            parameters[2].Value = model.ImageUrl;

            if (model.ImageName != null)
                parameters[3].Value = model.ImageName;
            else
                parameters[3].Value = DBNull.Value;


            if (model.ImageTitle != null)
                parameters[4].Value = model.ImageTitle;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.GoodsID;

            if (model.GoodsUrl != null)
                parameters[6].Value = model.GoodsUrl;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.CreateUserID;
            parameters[8].Value = model.CreateDate;
            parameters[9].Value = model.ts;
            parameters[10].Value = model.modifyuser;
            // return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_ShopImageList model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_ShopImageList] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[Type]=@Type,");
            strSql.Append("[ImageUrl]=@ImageUrl,");
            strSql.Append("[ImageName]=@ImageName,");
            strSql.Append("[ImageTitle]=@ImageTitle,");
            strSql.Append("[GoodsID]=@GoodsID,");
            strSql.Append("[GoodsUrl]=@GoodsUrl,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@ImageUrl", SqlDbType.VarChar,150),
                    new SqlParameter("@ImageName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ImageTitle", SqlDbType.NVarChar,50),
                    new SqlParameter("@GoodsID", SqlDbType.BigInt),
                    new SqlParameter("@GoodsUrl", SqlDbType.VarChar,150),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.Type;
            parameters[3].Value = model.ImageUrl;

            if (model.ImageName != null)
                parameters[4].Value = model.ImageName;
            else
                parameters[4].Value = DBNull.Value;


            if (model.ImageTitle != null)
                parameters[5].Value = model.ImageTitle;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.GoodsID;

            if (model.GoodsUrl != null)
                parameters[7].Value = model.GoodsUrl;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.CreateUserID;
            parameters[9].Value = model.CreateDate;
            parameters[10].Value = model.ts;
            parameters[11].Value = model.dr;
            parameters[12].Value = model.modifyuser;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int comPid, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_ShopImageList] ");
            strSql.Append(" where [comPid]=@comPid and type=2");
            SqlParameter[] parameters = {
                    new SqlParameter("@comPid", SqlDbType.Int)};
            parameters[0].Value = comPid;

            // return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }
    }
}

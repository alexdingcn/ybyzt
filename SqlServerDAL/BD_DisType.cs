using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class BD_DisType
    {
        public int Add(Hi.Model.BD_DisType model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_DisType](");
            strSql.Append("[CompID],[TypeName],[TypeCode],[ParentId],[SortIndex],[Remark],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@TypeName,@TypeCode,@ParentId,@SortIndex,@Remark,@IsEnabled,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@TypeName", SqlDbType.VarChar,50),
                    new SqlParameter("@TypeCode", SqlDbType.VarChar,50),
                    new SqlParameter("@ParentId", SqlDbType.Int),
                    new SqlParameter("@SortIndex", SqlDbType.VarChar,50),
                    new SqlParameter("@Remark", SqlDbType.VarChar,500),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.TypeName;

            if (model.TypeCode != null)
                parameters[2].Value = model.TypeCode;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.ParentId;

            if (model.SortIndex != null)
                parameters[4].Value = model.SortIndex;
            else
                parameters[4].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[5].Value = model.Remark;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.IsEnabled;
            parameters[7].Value = model.CreateUserID;
            parameters[8].Value = model.CreateDate;
            parameters[9].Value = model.ts;
            parameters[10].Value = model.modifyuser;
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters)); 
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_DisType model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_DisType] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[TypeName]=@TypeName,");
            strSql.Append("[TypeCode]=@TypeCode,");
            strSql.Append("[ParentId]=@ParentId,");
            strSql.Append("[SortIndex]=@SortIndex,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@TypeName", SqlDbType.VarChar,50),
                    new SqlParameter("@TypeCode", SqlDbType.VarChar,50),
                    new SqlParameter("@ParentId", SqlDbType.Int),
                    new SqlParameter("@SortIndex", SqlDbType.VarChar,50),
                    new SqlParameter("@Remark", SqlDbType.VarChar,500),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.TypeName;

            if (model.TypeCode != null)
                parameters[3].Value = model.TypeCode;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.ParentId;

            if (model.SortIndex != null)
                parameters[5].Value = model.SortIndex;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[6].Value = model.Remark;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.IsEnabled;
            parameters[8].Value = model.CreateUserID;
            parameters[9].Value = model.CreateDate;
            parameters[10].Value = model.ts;
            parameters[11].Value = model.dr;
            parameters[12].Value = model.modifyuser;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }
    }
}

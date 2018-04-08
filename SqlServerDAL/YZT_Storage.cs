using System;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using System.Data;
using System.Data.SqlClient;

namespace Hi.SQLServerDAL
{
    /// <summary>
    /// 数据访问类 YZT_Storage
    /// </summary>
    public partial class YZT_Storage
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_Storage model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [YZT_Storage](");
            strSql.Append("[CompID],[DisID],[StorageNO],[StorageDate],[StorageType],[IState],[Remark],[vdef1],[vdef2],[vdef3],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@StorageNO,@StorageDate,@StorageType,@IState,@Remark,@vdef1,@vdef2,@vdef3,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@StorageNO", SqlDbType.VarChar,50),
                    new SqlParameter("@StorageDate", SqlDbType.DateTime),
                    new SqlParameter("@StorageType", SqlDbType.Int),
                    new SqlParameter("@IState", SqlDbType.Int),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.DisID;

            if (model.StorageNO != null)
                parameters[2].Value = model.StorageNO;
            else
                parameters[2].Value = DBNull.Value;


            if (model.StorageDate != DateTime.MinValue)
                parameters[3].Value = model.StorageDate;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.StorageType;
            parameters[5].Value = model.IState;

            if (model.Remark != null)
                parameters[6].Value = model.Remark;
            else
                parameters[6].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[7].Value = model.vdef1;
            else
                parameters[7].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[8].Value = model.vdef2;
            else
                parameters[8].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[9].Value = model.vdef3;
            else
                parameters[9].Value = DBNull.Value;

            parameters[10].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[11].Value = model.CreateDate;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.ts;
            parameters[13].Value = model.modifyuser;

            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_Storage model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [YZT_Storage] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[StorageNO]=@StorageNO,");
            strSql.Append("[StorageDate]=@StorageDate,");
            strSql.Append("[StorageType]=@StorageType,");
            strSql.Append("[IState]=@IState,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@StorageNO", SqlDbType.VarChar,50),
                    new SqlParameter("@StorageDate", SqlDbType.DateTime),
                    new SqlParameter("@StorageType", SqlDbType.Int),
                    new SqlParameter("@IState", SqlDbType.Int),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.DisID;

            if (model.StorageNO != null)
                parameters[3].Value = model.StorageNO;
            else
                parameters[3].Value = DBNull.Value;


            if (model.StorageDate != DateTime.MinValue)
                parameters[4].Value = model.StorageDate;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.StorageType;
            parameters[6].Value = model.IState;

            if (model.Remark != null)
                parameters[7].Value = model.Remark;
            else
                parameters[7].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[8].Value = model.vdef1;
            else
                parameters[8].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[9].Value = model.vdef2;
            else
                parameters[9].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[10].Value = model.vdef3;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[12].Value = model.CreateDate;
            else
                parameters[12].Value = DBNull.Value;

            parameters[13].Value = model.ts;
            parameters[14].Value = model.dr;
            parameters[15].Value = model.modifyuser;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }
    }
}

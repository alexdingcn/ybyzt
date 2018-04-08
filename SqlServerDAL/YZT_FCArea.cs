using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    /// <summary>
    /// 数据访问类 YZT_FCArea 
    /// </summary>
    public partial class YZT_FCArea
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_FCArea model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [YZT_FCArea](");
            strSql.Append("[CompID],[CMID],[AreaID],[vdef1],[vdef2],[vdef3],[CreateUserID],[CreateDate],[ts],[modifyuser],[Province],[City],[Area])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@CMID,@AreaID,@vdef1,@vdef2,@vdef3,@CreateUserID,@CreateDate,@ts,@modifyuser,@Province,@City,@Area)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@CMID", SqlDbType.Int),
                    new SqlParameter("@AreaID", SqlDbType.Int),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@Province", SqlDbType.VarChar,50),
                    new SqlParameter("@City", SqlDbType.VarChar,50),
                    new SqlParameter("@Area", SqlDbType.VarChar,50)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.CMID;
            parameters[2].Value = model.AreaID;

            if (model.vdef1 != null)
                parameters[3].Value = model.vdef1;
            else
                parameters[3].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[4].Value = model.vdef2;
            else
                parameters[4].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[5].Value = model.vdef3;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[7].Value = model.CreateDate;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.ts;
            parameters[9].Value = model.modifyuser;

            if (model.Province != null)
                parameters[10].Value = model.Province;
            else
                parameters[10].Value = DBNull.Value;

            if (model.City != null)
                parameters[11].Value = model.City;
            else
                parameters[11].Value = DBNull.Value;

            if (model.Area != null)
                parameters[12].Value = model.Area;
            else
                parameters[12].Value = DBNull.Value;

            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_FCArea model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [YZT_FCArea] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[CMID]=@CMID,");
            strSql.Append("[AreaID]=@AreaID,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[Province]=@Province,");
            strSql.Append("[City]=@City,");
            strSql.Append("[Area]=@Area");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@CMID", SqlDbType.Int),
                    new SqlParameter("@AreaID", SqlDbType.Int),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@Province", SqlDbType.VarChar,50),
                    new SqlParameter("@City", SqlDbType.VarChar,50),
                    new SqlParameter("@Area", SqlDbType.VarChar,50)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.CMID;
            parameters[3].Value = model.AreaID;

            if (model.vdef1 != null)
                parameters[4].Value = model.vdef1;
            else
                parameters[4].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[5].Value = model.vdef2;
            else
                parameters[5].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[6].Value = model.vdef3;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[8].Value = model.CreateDate;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.ts;
            parameters[10].Value = model.dr;
            parameters[11].Value = model.modifyuser;

            if (model.Province != null)
                parameters[11].Value = model.Province;
            else
                parameters[11].Value = DBNull.Value;


            if (model.City != null)
                parameters[12].Value = model.City;
            else
                parameters[12].Value = DBNull.Value;

            if (model.Area != null)
                parameters[13].Value = model.Area;
            else
                parameters[13].Value = DBNull.Value;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool CMDelete(int ID, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [YZT_FCArea] ");
            strSql.Append(" where [CMID]=@CMID");
            SqlParameter[] parameters = {
                    new SqlParameter("@CMID", SqlDbType.Int)};
            parameters[0].Value = ID;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    /// <summary>
    /// 数据访问类 YZT_FirstCamp 
    /// </summary>
    public partial class YZT_FirstCamp
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_FirstCamp model ,SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [YZT_FirstCamp](");
            strSql.Append("[CMID],[CompID],[DisID],[HtID],[State],[ForceDate],[InvalidDate],[Remark],[vdef1],[vdef2],[vdef3],[CreateUserID],[CreateDate],[ts],[modifyuser],[ApplyRemark],[AreaID])");
            strSql.Append(" values (");
            strSql.Append("@CMID,@CompID,@DisID,@HtID,@State,@ForceDate,@InvalidDate,@Remark,@vdef1,@vdef2,@vdef3,@CreateUserID,@CreateDate,@ts,@modifyuser,@ApplyRemark,@AreaID)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CMID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@HtID", SqlDbType.Int),
                    new SqlParameter("@State", SqlDbType.Int),
                    new SqlParameter("@ForceDate", SqlDbType.DateTime),
                    new SqlParameter("@InvalidDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@ApplyRemark", SqlDbType.VarChar),
                    new SqlParameter("@AreaID", SqlDbType.Int)
            };
            parameters[0].Value = model.CMID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.HtID;
            parameters[4].Value = model.State;

            if (model.ForceDate != DateTime.MinValue)
                parameters[5].Value = model.ForceDate;
            else
                parameters[5].Value = DBNull.Value;


            if (model.InvalidDate != DateTime.MinValue)
                parameters[6].Value = model.InvalidDate;
            else
                parameters[6].Value = DBNull.Value;


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
            parameters[14].Value = model.modifyuser;
            parameters[15].Value = model.Applyremark;
            parameters[16].Value = model.AreaID;

            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_FirstCamp model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [YZT_FirstCamp] set ");
            strSql.Append("[CMID]=@CMID,");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[HtID]=@HtID,");
            strSql.Append("[State]=@State,");
            strSql.Append("[ForceDate]=@ForceDate,");
            strSql.Append("[InvalidDate]=@InvalidDate,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[ApplyRemark]=@Applyremark,");
            strSql.Append("[AreaID]=@AreaID");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CMID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@HtID", SqlDbType.Int),
                    new SqlParameter("@State", SqlDbType.Int),
                    new SqlParameter("@ForceDate", SqlDbType.DateTime),
                    new SqlParameter("@InvalidDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@ApplyRemark", SqlDbType.VarChar),
                    new SqlParameter("@AreaID", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CMID;
            parameters[2].Value = model.CompID;
            parameters[3].Value = model.DisID;
            parameters[4].Value = model.HtID;
            parameters[5].Value = model.State;

            if (model.ForceDate != DateTime.MinValue)
                parameters[6].Value = model.ForceDate;
            else
                parameters[6].Value = DBNull.Value;


            if (model.InvalidDate != DateTime.MinValue)
                parameters[7].Value = model.InvalidDate;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[8].Value = model.Remark;
            else
                parameters[8].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[9].Value = model.vdef1;
            else
                parameters[9].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[10].Value = model.vdef2;
            else
                parameters[10].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[11].Value = model.vdef3;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[13].Value = model.CreateDate;
            else
                parameters[13].Value = DBNull.Value;

            parameters[14].Value = model.ts;
            parameters[15].Value = model.dr;
            parameters[16].Value = model.modifyuser;
            parameters[17].Value = model.Applyremark;
            parameters[18].Value = model.AreaID;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }
    }
}

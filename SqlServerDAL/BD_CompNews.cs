using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class BD_CompNews
    {
        public int Add(Hi.Model.BD_CompNews model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_CompNews](");
            strSql.Append("[CompID],[IsEnabled],[NewsType],[NewsTitle],[NewsContents],[IsTop],[ShowType],[CreateUserID],[CreateDate],[ts],[modifyuser],[PMID])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@IsEnabled,@NewsType,@NewsTitle,@NewsContents,@IsTop,@ShowType,@CreateUserID,@CreateDate,@ts,@modifyuser,@PMID)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@NewsType", SqlDbType.Int),
                    new SqlParameter("@NewsTitle", SqlDbType.VarChar,200),
                    new SqlParameter("@NewsContents", SqlDbType.Text),
                    new SqlParameter("@IsTop", SqlDbType.Int),
                    new SqlParameter("@ShowType", SqlDbType.VarChar,50),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@PMID", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.NewsType;
            parameters[2].Value = model.NewsTitle;

            if (model.NewsContents != null)
                parameters[3].Value = model.NewsContents;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.IsTop;

            if (model.ShowType != null)
                parameters[5].Value = model.ShowType;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.IsEnabled;
            parameters[7].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[8].Value = model.CreateDate;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.ts;
            parameters[10].Value = model.modifyuser;
            parameters[11].Value = model.PmID;
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }
    }
}

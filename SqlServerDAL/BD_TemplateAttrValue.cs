using System;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using System.Data.SqlClient;
using System.Data;
namespace Hi.SQLServerDAL
{
    public partial class BD_TemplateAttrValue
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_TemplateAttrValue model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_TemplateAttrValue](");
            strSql.Append("[CompID],[TemplateAttrID],[AttributeValueID],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@TemplateAttrID,@AttributeValueID,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@TemplateAttrID", SqlDbType.Int),
                    new SqlParameter("@AttributeValueID", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.TemplateAttrID;
            parameters[2].Value = model.AttributeValueID;
            parameters[3].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[4].Value = model.CreateDate;
            else
                parameters[4].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[5].Value = model.ts;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.modifyuser;
           // return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_TemplateAttrValue] ");
            strSql.Append(" where [TemplateAttrID]=@TemplateAttrID");
            SqlParameter[] parameters = {
                    new SqlParameter("@TemplateAttrID", SqlDbType.Int)};
            parameters[0].Value = ID;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;

            //return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }
    }
}

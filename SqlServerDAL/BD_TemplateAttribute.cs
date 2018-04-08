using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class BD_TemplateAttribute
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_TemplateAttribute model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_TemplateAttribute](");
            strSql.Append("[CompID],[TemplateID],[AttributeID],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@TemplateID,@AttributeID,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@TemplateID", SqlDbType.Int),
                    new SqlParameter("@AttributeID", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.TemplateID;
            parameters[2].Value = model.AttributeID;
            parameters[3].Value = model.CreateUserID;
            parameters[4].Value = model.CreateDate;
            parameters[5].Value = model.ts;
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
            strSql.Append("delete [BD_TemplateAttribute] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;

            // return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;

        }
        /// <summary>
        /// 获取数据集,在单表查询时使用
        /// </summary>
        public DataSet GetDataSet(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_TemplateAttribute]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), Tran);
        }
        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_TemplateAttribute> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby, Tran));
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_TemplateAttribute model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_TemplateAttribute] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[TemplateID]=@TemplateID,");
            strSql.Append("[AttributeID]=@AttributeID,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@TemplateID", SqlDbType.Int),
                    new SqlParameter("@AttributeID", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.TemplateID;
            parameters[3].Value = model.AttributeID;
            parameters[4].Value = model.CreateUserID;
            parameters[5].Value = model.CreateDate;
            parameters[6].Value = model.ts;
            parameters[7].Value = model.dr;
            parameters[8].Value = model.modifyuser;

            // return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;

        }

    }
}

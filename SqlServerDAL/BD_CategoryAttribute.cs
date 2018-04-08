using System;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using System.Data;
using System.Data.SqlClient;

namespace Hi.SQLServerDAL
{
    public partial class BD_CategoryAttribute
    {
        /// <summary>
        /// 删除类别属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Deletes(string id, string compid)
        {
            string sql = string.Format(@"delete BD_CategoryAttribute where categoryid={0} and compid={1}", id, compid);
            int dt = SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, sql);
            return dt;
        }
        /// <summary>
        /// 类别和属性显示
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public string Bind(string where)
        {
            string sql = string.Format(@"SELECT CategoryID, STUFF((SELECT ',' + CONVERT( varchar(50), AttributeID)
                     FROM BD_CategoryAttribute  
                     WHERE CategoryID=A.CategoryID FOR XML PATH('')),1, 1, '')AS AttributeID
                     FROM BD_CategoryAttribute A where 1=1 {0}
                     GROUP BY CategoryID order by categoryid", where);
            return sql;
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_CategoryAttribute model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_CategoryAttribute](");
            strSql.Append("[CompID],[CategoryID],[AttributeID],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@CategoryID,@AttributeID,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@AttributeID", SqlDbType.Int),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.CategoryID;
            parameters[2].Value = model.AttributeID;
            parameters[3].Value = model.ts;
            parameters[4].Value = model.modifyuser;
            // return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_CategoryAttribute model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_CategoryAttribute] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[CategoryID]=@CategoryID,");
            strSql.Append("[AttributeID]=@AttributeID,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@AttributeID", SqlDbType.Int),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.CategoryID;
            parameters[3].Value = model.AttributeID;
            parameters[4].Value = model.ts;
            parameters[5].Value = model.dr;
            parameters[6].Value = model.modifyuser;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;

            //return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }
        public IList<Hi.Model.BD_CategoryAttribute> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby, Tran));
        }

        /// <summary>
        /// 获取数据集,在单表查询时使用
        /// </summary>
        public DataSet GetDataSet(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_CategoryAttribute]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), Tran);
        }
    }
}
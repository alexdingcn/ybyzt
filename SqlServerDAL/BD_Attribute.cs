using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DBUtility;


namespace Hi.SQLServerDAL
{
    public partial class BD_Attribute
    {
        /// <summary>
        /// 获取属性以及属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable GetAttrbuteList(string id, string ComPid)
        {

            string sql = string.Format(@"select a.AttributeName,a.SortIndex,b.AttrValue,b.AttributeID,b.IsEnabled,a.Memo,
                        b.ID from BD_Attribute as a  left join BD_AttributeValues as b
                        on a.ID=b.AttributeID
                        where a.ID={0}  and ISNULL(a.dr,0)=0  and a.compid={1} and ISNULL(b.dr,0)=0 and  b.compid={1}", id, ComPid);
            DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
            return dt;
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_Attribute model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_Attribute](");
            strSql.Append("[CompID],[AttributeName],[AttributeCode],[AttributeType],[SortIndex],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@AttributeName,@AttributeCode,@AttributeType,@SortIndex,@IsEnabled,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@AttributeName", SqlDbType.VarChar,128),
                    new SqlParameter("@AttributeCode", SqlDbType.VarChar,32),
                    new SqlParameter("@AttributeType", SqlDbType.Int),
                    new SqlParameter("@SortIndex", SqlDbType.VarChar,50),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;

            if (model.AttributeName != null)
                parameters[1].Value = model.AttributeName;
            else
                parameters[1].Value = DBNull.Value;


            if (model.AttributeCode != null)
                parameters[2].Value = model.AttributeCode;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.AttributeType;

            if (model.SortIndex != null)
                parameters[4].Value = model.SortIndex;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.IsEnabled;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.CreateDate;
            parameters[8].Value = model.ts;
            parameters[9].Value = model.modifyuser;
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));

        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Attribute model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_Attribute] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[AttributeName]=@AttributeName,");
            strSql.Append("[AttributeCode]=@AttributeCode,");
            strSql.Append("[AttributeType]=@AttributeType,");
            strSql.Append("[SortIndex]=@SortIndex,");
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
                    new SqlParameter("@AttributeName", SqlDbType.VarChar,128),
                    new SqlParameter("@AttributeCode", SqlDbType.VarChar,32),
                    new SqlParameter("@AttributeType", SqlDbType.Int),
                    new SqlParameter("@SortIndex", SqlDbType.VarChar,50),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;

            if (model.AttributeName != null)
                parameters[2].Value = model.AttributeName;
            else
                parameters[2].Value = DBNull.Value;


            if (model.AttributeCode != null)
                parameters[3].Value = model.AttributeCode;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.AttributeType;

            if (model.SortIndex != null)
                parameters[5].Value = model.SortIndex;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.IsEnabled;
            parameters[7].Value = model.CreateUserID;
            parameters[8].Value = model.CreateDate;
            parameters[9].Value = model.ts;
            parameters[10].Value = model.dr;
            parameters[11].Value = model.modifyuser;

            //return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;

        }
        public IList<Hi.Model.BD_Attribute> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_Attribute]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), Tran);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_Attribute GetModel(int ID, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_Attribute] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            // DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
            DataSet ds = SqlHelper.Query(strSql.ToString(), Tran, parameters);
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

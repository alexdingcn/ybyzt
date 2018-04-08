using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class BD_AttributeValues
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_AttributeValues model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_AttributeValues](");
            strSql.Append("[CompID],[AttributeID],[AttrCode],[AttrValue],[SortIndex],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@AttributeID,@AttrCode,@AttrValue,@SortIndex,@IsEnabled,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@AttributeID", SqlDbType.BigInt),
                    new SqlParameter("@AttrCode", SqlDbType.VarChar,32),
                    new SqlParameter("@AttrValue", SqlDbType.VarChar,128),
                    new SqlParameter("@SortIndex", SqlDbType.VarChar,32),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.AttributeID;

            if (model.AttrCode != null)
                parameters[2].Value = model.AttrCode;
            else
                parameters[2].Value = DBNull.Value;


            if (model.AttrValue != null)
                parameters[3].Value = model.AttrValue;
            else
                parameters[3].Value = DBNull.Value;


            if (model.SortIndex != null)
                parameters[4].Value = model.SortIndex;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.IsEnabled;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.CreateDate;
            parameters[8].Value = model.ts;
            parameters[9].Value = model.modifyuser;
            // return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_AttributeValues model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_AttributeValues] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[AttributeID]=@AttributeID,");
            strSql.Append("[AttrCode]=@AttrCode,");
            strSql.Append("[AttrValue]=@AttrValue,");
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
                    new SqlParameter("@AttributeID", SqlDbType.BigInt),
                    new SqlParameter("@AttrCode", SqlDbType.VarChar,32),
                    new SqlParameter("@AttrValue", SqlDbType.VarChar,128),
                    new SqlParameter("@SortIndex", SqlDbType.VarChar,32),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.AttributeID;

            if (model.AttrCode != null)
                parameters[3].Value = model.AttrCode;
            else
                parameters[3].Value = DBNull.Value;


            if (model.AttrValue != null)
                parameters[4].Value = model.AttrValue;
            else
                parameters[4].Value = DBNull.Value;


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

            // return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;

        }
        public IList<Hi.Model.BD_AttributeValues> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_AttributeValues]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), Tran);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_AttributeValues GetModel(int ID, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_AttributeValues] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
          //  DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
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

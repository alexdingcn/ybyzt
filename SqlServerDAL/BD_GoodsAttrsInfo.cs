using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using DBUtility;
using System.Data;
namespace Hi.SQLServerDAL
{
    public partial class BD_GoodsAttrsInfo
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_GoodsAttrsInfo model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_GoodsAttrsInfo](");
            strSql.Append("[CompID],[GoodsID],[AttrsID],[AttrsInfoName],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@GoodsID,@AttrsID,@AttrsInfoName,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@AttrsID", SqlDbType.Int),
                    new SqlParameter("@AttrsInfoName", SqlDbType.VarChar,50),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.GoodsID;
            parameters[2].Value = model.AttrsID;
            parameters[3].Value = model.AttrsInfoName;
            parameters[4].Value = model.ts;
            parameters[5].Value = model.modifyuser;
           // return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));

        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_GoodsAttrsInfo model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_GoodsAttrsInfo] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[GoodsID]=@GoodsID,");
            strSql.Append("[AttrsID]=@AttrsID,");
            strSql.Append("[AttrsInfoName]=@AttrsInfoName,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@AttrsID", SqlDbType.Int),
                    new SqlParameter("@AttrsInfoName", SqlDbType.VarChar,50),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.GoodsID;
            parameters[3].Value = model.AttrsID;
            parameters[4].Value = model.AttrsInfoName;
            parameters[5].Value = model.ts;
            parameters[6].Value = model.dr;
            parameters[7].Value = model.modifyuser;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;

            //return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }
        /// <summary>
        /// 获取数据集,在单表查询时使用
        /// </summary>
        public DataSet GetDataSet(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_GoodsAttrsInfo]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(),  Tran);
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_GoodsAttrsInfo> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby,  Tran));
        }
    }
}

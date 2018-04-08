using System;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using System.Data.SqlClient;
using System.Data;
namespace Hi.SQLServerDAL
{
    /// <summary>
    /// 数据访问类 BD_ShopImageList
    /// </summary>
    public partial class BD_ShopGoodsList
    {
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_ShopGoodsList GetModel(int ID, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_ShopGoodsList] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            //  DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), parameters, Tran);
            DataSet ds = SqlHelper.Query(strSql.ToString(), Tran, parameters);// SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
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
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_ShopGoodsList model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_ShopGoodsList] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[Title]=@Title,");
            strSql.Append("[GoodsID]=@GoodsID,");
            strSql.Append("[ShowName]=@ShowName,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@Title", SqlDbType.NVarChar,50),
                    new SqlParameter("@GoodsID", SqlDbType.BigInt),
                    new SqlParameter("@ShowName", SqlDbType.NVarChar,80),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.GoodsID;
            parameters[4].Value = model.ShowName;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.CreateDate;
            parameters[7].Value = model.ts;
            parameters[8].Value = model.dr;
            parameters[9].Value = model.modifyuser;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
            //return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_ShopGoodsList model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_ShopGoodsList](");
            strSql.Append("[CompID],[Title],[GoodsID],[ShowName],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@Title,@GoodsID,@ShowName,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@Title", SqlDbType.NVarChar,50),
                    new SqlParameter("@GoodsID", SqlDbType.BigInt),
                    new SqlParameter("@ShowName", SqlDbType.NVarChar,80),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.GoodsID;
            parameters[3].Value = model.ShowName;
            parameters[4].Value = model.CreateUserID;
            parameters[5].Value = model.CreateDate;
            parameters[6].Value = model.ts;
            parameters[7].Value = model.modifyuser;
            //  return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID, string title, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_ShopGoodsList] ");
            strSql.Append(" where [comPid]=@ID and [Title]=@Title");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                     new SqlParameter("@Title",SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = title;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
            // return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }
        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_ShopGoodsList> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_ShopGoodsList]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), Tran);
        }
    }
}

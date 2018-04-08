using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
namespace Hi.SQLServerDAL
{
    public partial class BD_PromotionDetail2
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_PromotionDetail2 model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_PromotionDetail2](");
            strSql.Append("[ProID],[CompID],[OrderPrice],[Discount],[modifyuser],[CreateUserID])");
            strSql.Append(" values (");
            strSql.Append("@ProID,@CompID,@OrderPrice,@Discount,@modifyuser,@CreateUserID)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@OrderPrice", SqlDbType.Decimal),
                    new SqlParameter("@Discount", SqlDbType.Decimal),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int)
            };
            parameters[0].Value = model.ProID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.OrderPrice;
            parameters[3].Value = model.Discount;
            parameters[4].Value = model.modifyuser;
            parameters[5].Value = model.CreateUserID;
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
            strSql.Append("delete [BD_PromotionDetail2] ");
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_PromotionDetail2]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), Tran);
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_PromotionDetail2> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby, Tran));
        }
    }
}

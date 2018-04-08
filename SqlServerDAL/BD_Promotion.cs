using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DBUtility;
namespace Hi.SQLServerDAL
{
    public partial class BD_Promotion
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_Promotion model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_Promotion](");
            strSql.Append("[CompID],[ProTitle],[Type],[ProType],[Discount],[ProInfos],[IsEnabled],[ProStartTime],[ProEndTime],[CreateUserID],[CreateDate],[ts],[dr],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@ProTitle,@Type,@ProType,@Discount,@ProInfos,@IsEnabled,@ProStartTime,@ProEndTime,@CreateUserID,@CreateDate,@ts,@dr,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@ProTitle", SqlDbType.NVarChar,200),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@ProType", SqlDbType.Int),
                    new SqlParameter("@Discount", SqlDbType.Decimal),
                    new SqlParameter("@ProInfos", SqlDbType.NVarChar,2000),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@ProStartTime", SqlDbType.DateTime),
                    new SqlParameter("@ProEndTime", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.Char,19),
                    new SqlParameter("@dr", SqlDbType.Int),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;

            if (model.ProTitle != null)
                parameters[1].Value = model.ProTitle;
            else
                parameters[1].Value = DBNull.Value;

            parameters[2].Value = model.Type;
            parameters[3].Value = model.ProType;
            parameters[4].Value = model.Discount;

            if (model.ProInfos != null)
                parameters[5].Value = model.ProInfos;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.IsEnabled;
            parameters[7].Value = model.ProStartTime;
            parameters[8].Value = model.ProEndTime;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.CreateDate;
            parameters[11].Value = model.ts;
            parameters[12].Value = model.dr;
            parameters[13].Value = model.modifyuser;
            //return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));

            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));

        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_Promotion GetModel(int ID, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_Promotion] ");
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
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Promotion model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_Promotion] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[ProTitle]=@ProTitle,");
            strSql.Append("[Type]=@Type,");
            strSql.Append("[ProType]=@ProType,");
            strSql.Append("[Discount]=@Discount,");
            strSql.Append("[ProInfos]=@ProInfos,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[ProStartTime]=@ProStartTime,");
            strSql.Append("[ProEndTime]=@ProEndTime,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@ProTitle", SqlDbType.NVarChar,200),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@ProType", SqlDbType.Int),
                    new SqlParameter("@Discount", SqlDbType.Decimal),
                    new SqlParameter("@ProInfos", SqlDbType.NVarChar,2000),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@ProStartTime", SqlDbType.DateTime),
                    new SqlParameter("@ProEndTime", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.Char,19),
                    new SqlParameter("@dr", SqlDbType.Int),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;

            if (model.ProTitle != null)
                parameters[2].Value = model.ProTitle;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.Type;
            parameters[4].Value = model.ProType;
            parameters[5].Value = model.Discount;

            if (model.ProInfos != null)
                parameters[6].Value = model.ProInfos;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.IsEnabled;
            parameters[8].Value = model.ProStartTime;
            parameters[9].Value = model.ProEndTime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.CreateDate;
            parameters[12].Value = model.ts;
            parameters[13].Value = model.dr;
            parameters[14].Value = model.modifyuser;

           // return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;

        }

        /// <summary>
        /// 查询商品使用的促销方式
        /// </summary>
        /// <param name="proID">促销ID</param>
        /// <returns></returns>
        public DataTable ProType(string proID)
        {
            string sql = string.Format(@"select pro.ID,pro.CompID,pro.Type,pro.ProType,pro.DisCount,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID,prod.GoodsPrice from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID where pro.ID={0} and isnull(pro.dr,0)=0 and ISNULL(pro.IsEnabled,0)=1 order by pro.CreateDate desc", proID);

            DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, sql).Tables[0];
            return dt;
        }
    }
}

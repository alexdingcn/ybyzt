using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DBUtility;

namespace Hi.SQLServerDAL
{
    public partial class BD_GoodsCategory
    {
        public IList<Hi.Model.BD_GoodsCategory> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_GoodsCategory]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), Tran);
        }

        public bool Update(Hi.Model.BD_GoodsCategory model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_GoodsCategory] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[GoodsTypeID]=@GoodsTypeID,");
            strSql.Append("[CategoryCode]=@CategoryCode,");
            strSql.Append("[CategoryName]=@CategoryName,");
            strSql.Append("[ParentId]=@ParentId,");
            strSql.Append("[SortIndex]=@SortIndex,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[Code]=@Code,");
            strSql.Append("[ParCode]=@ParCode,");
            strSql.Append("[Deep]=@Deep,");
            strSql.Append("[OtherCode]=@OtherCode,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsTypeID", SqlDbType.Int),
                    new SqlParameter("@CategoryCode", SqlDbType.VarChar,24),
                    new SqlParameter("@CategoryName", SqlDbType.VarChar,64),
                    new SqlParameter("@ParentId", SqlDbType.Int),
                    new SqlParameter("@SortIndex", SqlDbType.VarChar,64),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@Deep", SqlDbType.Int),
                    new SqlParameter("@Code", SqlDbType.VarChar,50),
                    new SqlParameter("@ParCode", SqlDbType.VarChar,50),
                    new SqlParameter("@OtherCode", SqlDbType.VarChar,50)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.GoodsTypeID;

            if (model.CategoryCode != null)
                parameters[3].Value = model.CategoryCode;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.CategoryName;
            parameters[5].Value = model.ParentId;

            if (model.SortIndex != null)
                parameters[6].Value = model.SortIndex;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.IsEnabled;
            parameters[8].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[9].Value = model.CreateDate;
            else
                parameters[9].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[10].Value = model.ts;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.dr;
            parameters[12].Value = model.modifyuser;

            
            parameters[13].Value = model.Deep;

            if (model.Code != null)
                parameters[14].Value = model.Code;
            else
                parameters[14].Value = DBNull.Value;

            if (model.ParCode != null)
                parameters[15].Value = model.ParCode;
            else
                parameters[15].Value = DBNull.Value;

            if (model.OtherCode != null)
                parameters[16].Value = model.OtherCode;
            else
                parameters[16].Value = DBNull.Value;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        public int Add(Hi.Model.BD_GoodsCategory model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_GoodsCategory](");
            strSql.Append("[CompID],[GoodsTypeID],[CategoryCode],[CategoryName],[ParentId],[SortIndex],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser],[Deep],[Code],[ParCode],[OtherCode])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@GoodsTypeID,@CategoryCode,@CategoryName,@ParentId,@SortIndex,@IsEnabled,@CreateUserID,@CreateDate,@ts,@modifyuser,@Deep,@Code,@ParCode,@OtherCode)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsTypeID", SqlDbType.Int),
                    new SqlParameter("@CategoryCode", SqlDbType.VarChar,24),
                    new SqlParameter("@CategoryName", SqlDbType.VarChar,64),
                    new SqlParameter("@ParentId", SqlDbType.Int),
                    new SqlParameter("@SortIndex", SqlDbType.VarChar,64),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@Deep", SqlDbType.Int),
                    new SqlParameter("@Code", SqlDbType.VarChar,50),
                    new SqlParameter("@ParCode", SqlDbType.VarChar,50),
                    new SqlParameter("@OtherCode", SqlDbType.VarChar,50)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.GoodsTypeID;

            if (model.CategoryCode != null)
                parameters[2].Value = model.CategoryCode;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.CategoryName;
            parameters[4].Value = model.ParentId;

            if (model.SortIndex != null)
                parameters[5].Value = model.SortIndex;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.IsEnabled;
            parameters[7].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[8].Value = model.CreateDate;
            else
                parameters[8].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[9].Value = model.ts;
            else
                parameters[9].Value = DBNull.Value;

            parameters[10].Value = model.modifyuser;

            parameters[11].Value = model.Deep;

            if (model.Code != null)
                parameters[12].Value = model.Code;
            else
                parameters[12].Value = DBNull.Value;

            if (model.ParCode != null)
                parameters[13].Value = model.ParCode;
            else
                parameters[13].Value = DBNull.Value;

            if (model.OtherCode != null)
                parameters[14].Value = model.OtherCode;
            else
                parameters[14].Value = DBNull.Value;

            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }
    }
}

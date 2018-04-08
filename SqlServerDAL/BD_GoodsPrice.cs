using System;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using System.Data;
using System.Data.SqlClient;

namespace Hi.SQLServerDAL
{
    public partial class BD_GoodsPrice
    {
        /// <summary>
        /// 批量插入GoodsPrice表
        /// </summary>
        /// <param name="ll"></param>
        public bool InserGoodsPrice(List<Hi.Model.BD_GoodsPrice> ll)
        {
            DataTable dataTable = GetTableSchema();
            foreach (Hi.Model.BD_GoodsPrice item in ll)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow[1] = item.DisID;
                dataRow[2] = item.CompID;
                dataRow[3] = item.GoodsInfoID;
                dataRow[4] = item.TinkerPrice;
                dataRow[5] = item.IsEnabled;
                dataRow[6] = item.CreateUserID;
                dataRow[7] = item.CreateDate;
                dataRow[8] = item.ts;
                dataRow[9] = item.dr;
                dataRow[10] = item.modifyuser;
                dataTable.Rows.Add(dataRow);
            }
            return SqlHelper.SqlBulkCopyInsert(SqlHelper.LocalSqlServer, dataTable, "BD_GoodsPrice");
        }
        private static DataTable GetTableSchema()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[] { new DataColumn("ID"), new DataColumn("DisID"), new DataColumn("CompID"), new DataColumn("GoodsInfoID"), new DataColumn("TinkerPrice", typeof(System.Decimal)), new DataColumn("IsEnabled"), new DataColumn("CreateUserID"), new DataColumn("CreateDate"), new DataColumn("ts"), new DataColumn("dr"), new DataColumn("modifyuser") });
            return dataTable;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_GoodsPrice model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_GoodsPrice](");
            strSql.Append("[DisID],[CompID],[GoodsInfoID],[GoodsName],[BarCode],[InfoValue],[Unit],[TinkerPrice],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@DisID,@CompID,@GoodsInfoID,@GoodsName,@BarCode,@InfoValue,@Unit,@TinkerPrice,@IsEnabled,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsInfoID", SqlDbType.Int),
                    new SqlParameter("@GoodsName", SqlDbType.NVarChar,100),
                    new SqlParameter("@BarCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@InfoValue", SqlDbType.NVarChar,300),
                    new SqlParameter("@Unit", SqlDbType.NVarChar,30),
                    new SqlParameter("@TinkerPrice", SqlDbType.Decimal),
                    new SqlParameter("@IsEnabled", SqlDbType.Bit),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.DisID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.GoodsInfoID;

            if (model.GoodsName != null)
                parameters[3].Value = model.GoodsName;
            else
                parameters[3].Value = DBNull.Value;


            if (model.BarCode != null)
                parameters[4].Value = model.BarCode;
            else
                parameters[4].Value = DBNull.Value;


            if (model.InfoValue != null)
                parameters[5].Value = model.InfoValue;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Unit != null)
                parameters[6].Value = model.Unit;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.TinkerPrice;
            parameters[8].Value = model.IsEnabled;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.CreateDate;
            parameters[11].Value = model.ts;
            parameters[12].Value = model.modifyuser;
            // return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_GoodsPrice model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_GoodsPrice] set ");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[GoodsInfoID]=@GoodsInfoID,");
            strSql.Append("[GoodsName]=@GoodsName,");
            strSql.Append("[BarCode]=@BarCode,");
            strSql.Append("[InfoValue]=@InfoValue,");
            strSql.Append("[Unit]=@Unit,");
            strSql.Append("[TinkerPrice]=@TinkerPrice,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsInfoID", SqlDbType.Int),
                    new SqlParameter("@GoodsName", SqlDbType.NVarChar,100),
                    new SqlParameter("@BarCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@InfoValue", SqlDbType.NVarChar,300),
                    new SqlParameter("@Unit", SqlDbType.NVarChar,30),
                    new SqlParameter("@TinkerPrice", SqlDbType.Decimal),
                    new SqlParameter("@IsEnabled", SqlDbType.Bit),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.CompID;
            parameters[3].Value = model.GoodsInfoID;

            if (model.GoodsName != null)
                parameters[4].Value = model.GoodsName;
            else
                parameters[4].Value = DBNull.Value;


            if (model.BarCode != null)
                parameters[5].Value = model.BarCode;
            else
                parameters[5].Value = DBNull.Value;


            if (model.InfoValue != null)
                parameters[6].Value = model.InfoValue;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Unit != null)
                parameters[7].Value = model.Unit;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.TinkerPrice;
            parameters[9].Value = model.IsEnabled;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.CreateDate;
            parameters[12].Value = model.ts;
            parameters[13].Value = model.dr;
            parameters[14].Value = model.modifyuser;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;

            // return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int compid, int disid, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_GoodsPrice] ");
            strSql.Append(" where [compid]=@compid and [disid]=@disid");
            SqlParameter[] parameters = {
                    new SqlParameter("@compid", SqlDbType.Int), new SqlParameter("@disid", SqlDbType.Int)};
            parameters[0].Value = compid;
            parameters[1].Value = disid;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;

           // return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_GoodsPrice> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction Tran)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_GoodsPrice]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), Tran);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_GoodsPrice GetModel(int ID, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_GoodsPrice] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
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

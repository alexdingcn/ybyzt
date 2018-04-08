//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/9/5 18:48:32
//
// 功能描述: 
//
// 修改标识: 
// 修改描述: 
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using DBUtility;
using System.Collections;
using System.Collections.Generic;

namespace Hi.SQLServerDAL
{
    /// <summary>
    /// 数据访问类 PAY_PrePayment
    /// </summary>
    public partial class PAY_PrePayment
    {
        public PAY_PrePayment()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.PAY_PrePayment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [PAY_PrePayment](");
            strSql.Append("[CompID],[DisID],[OrderID],[Start],[PreType],[price],[CreatDate],[OldId],[CrateUser],[AuditState],[AuditUser],[IsEnabled],[AuditDate],[ts],[dr],[modifyuser],[vdef1],[vdef2],[vdef3],[vdef4],[vdef5],[Paytime],[vdef6],[guid])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@OrderID,@Start,@PreType,@price,@CreatDate,@OldId,@CrateUser,@AuditState,@AuditUser,@IsEnabled,@AuditDate,@ts,@dr,@modifyuser,@vdef1,@vdef2,@vdef3,@vdef4,@vdef5,@Paytime,@vdef6,@guid)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@Start", SqlDbType.Int),
                    new SqlParameter("@PreType", SqlDbType.Int),
                    new SqlParameter("@price", SqlDbType.Decimal),
                    new SqlParameter("@CreatDate", SqlDbType.DateTime),
                    new SqlParameter("@OldId", SqlDbType.BigInt),
                    new SqlParameter("@CrateUser", SqlDbType.Int),
                    new SqlParameter("@AuditState", SqlDbType.Int),
                    new SqlParameter("@AuditUser", SqlDbType.Int),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@vdef1", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef4", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef5", SqlDbType.VarChar,2000),
                    new SqlParameter("@Paytime", SqlDbType.DateTime),
                    new SqlParameter("@vdef6", SqlDbType.VarChar,200),
                    new SqlParameter("@guid", SqlDbType.NVarChar,200)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.OrderID;
            parameters[3].Value = model.Start;
            parameters[4].Value = model.PreType;
            parameters[5].Value = model.price;
            parameters[6].Value = model.CreatDate;
            parameters[7].Value = model.OldId;
            parameters[8].Value = model.CrateUser;
            parameters[9].Value = model.AuditState;
            parameters[10].Value = model.AuditUser;
            parameters[11].Value = model.IsEnabled;

            if (model.AuditDate != DateTime.MinValue)
                parameters[12].Value = model.AuditDate;
            else
                parameters[12].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[13].Value = model.ts;
            else
                parameters[13].Value = DBNull.Value;

            parameters[14].Value = model.dr;
            parameters[15].Value = model.modifyuser;

            if (model.vdef1 != null)
                parameters[16].Value = model.vdef1;
            else
                parameters[16].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[17].Value = model.vdef2;
            else
                parameters[17].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[18].Value = model.vdef3;
            else
                parameters[18].Value = DBNull.Value;


            if (model.vdef4 != null)
                parameters[19].Value = model.vdef4;
            else
                parameters[19].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[20].Value = model.vdef5;
            else
                parameters[20].Value = DBNull.Value;

            parameters[21].Value = model.Paytime;

            if (model.vdef6 != null)
                parameters[22].Value = model.vdef6;
            else
                parameters[22].Value = DBNull.Value;


            if (model.guid != null)
                parameters[23].Value = model.guid;
            else
                parameters[23].Value = DBNull.Value;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.PAY_PrePayment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [PAY_PrePayment] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[OrderID]=@OrderID,");
            strSql.Append("[Start]=@Start,");
            strSql.Append("[PreType]=@PreType,");
            strSql.Append("[price]=@price,");
            strSql.Append("[CreatDate]=@CreatDate,");
            strSql.Append("[OldId]=@OldId,");
            strSql.Append("[CrateUser]=@CrateUser,");
            strSql.Append("[AuditState]=@AuditState,");
            strSql.Append("[AuditUser]=@AuditUser,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[AuditDate]=@AuditDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[vdef4]=@vdef4,");
            strSql.Append("[vdef5]=@vdef5,");
            strSql.Append("[Paytime]=@Paytime,");
            strSql.Append("[vdef6]=@vdef6,");
            strSql.Append("[guid]=@guid");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@Start", SqlDbType.Int),
                    new SqlParameter("@PreType", SqlDbType.Int),
                    new SqlParameter("@price", SqlDbType.Decimal),
                    new SqlParameter("@CreatDate", SqlDbType.DateTime),
                    new SqlParameter("@OldId", SqlDbType.BigInt),
                    new SqlParameter("@CrateUser", SqlDbType.Int),
                    new SqlParameter("@AuditState", SqlDbType.Int),
                    new SqlParameter("@AuditUser", SqlDbType.Int),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@vdef1", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef4", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef5", SqlDbType.VarChar,2000),
                    new SqlParameter("@Paytime", SqlDbType.DateTime),
                    new SqlParameter("@vdef6", SqlDbType.VarChar,200),
                    new SqlParameter("@guid", SqlDbType.NVarChar,200)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.OrderID;
            parameters[4].Value = model.Start;
            parameters[5].Value = model.PreType;
            parameters[6].Value = model.price;
            parameters[7].Value = model.CreatDate;
            parameters[8].Value = model.OldId;
            parameters[9].Value = model.CrateUser;
            parameters[10].Value = model.AuditState;
            parameters[11].Value = model.AuditUser;
            parameters[12].Value = model.IsEnabled;

            if (model.AuditDate != DateTime.MinValue)
                parameters[13].Value = model.AuditDate;
            else
                parameters[13].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[14].Value = model.ts;
            else
                parameters[14].Value = DBNull.Value;

            parameters[15].Value = model.dr;
            parameters[16].Value = model.modifyuser;

            if (model.vdef1 != null)
                parameters[17].Value = model.vdef1;
            else
                parameters[17].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[18].Value = model.vdef2;
            else
                parameters[18].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[19].Value = model.vdef3;
            else
                parameters[19].Value = DBNull.Value;


            if (model.vdef4 != null)
                parameters[20].Value = model.vdef4;
            else
                parameters[20].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[21].Value = model.vdef5;
            else
                parameters[21].Value = DBNull.Value;

            parameters[22].Value = model.Paytime;

            if (model.vdef6 != null)
                parameters[23].Value = model.vdef6;
            else
                parameters[23].Value = DBNull.Value;


            if (model.guid != null)
                parameters[24].Value = model.guid;
            else
                parameters[24].Value = DBNull.Value;


            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [PAY_PrePayment] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[PAY_PrePayment]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [PAY_PrePayment]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.PAY_PrePayment GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [PAY_PrePayment] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
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
        /// 获取数据集,建议只在多表联查时使用
        /// </summary>
        public DataSet GetDataSet(string strSql)
        {
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql);
        }

        /// <summary>
        /// 获取泛型数据列表,建议只在多表联查时使用
        /// </summary>
        public IList<Hi.Model.PAY_PrePayment> GetList(string strSql)
        {
            return GetList(GetDataSet(strSql));
        }

        /// <summary>
        /// 获取数据集,在单表查询时使用
        /// </summary>
        public DataSet GetDataSet(string strWhat, string strWhere, string strOrderby)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [PAY_PrePayment]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.PAY_PrePayment> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.PAY_PrePayment> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[PAY_PrePayment]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.PAY_PrePayment GetModel(DataRow r)
        {
            Hi.Model.PAY_PrePayment model = new Hi.Model.PAY_PrePayment();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.OrderID = SqlHelper.GetInt(r["OrderID"]);
            model.Start = SqlHelper.GetInt(r["Start"]);
            model.PreType = SqlHelper.GetInt(r["PreType"]);
            model.price = SqlHelper.GetDecimal(r["price"]);
            model.CreatDate = SqlHelper.GetDateTime(r["CreatDate"]);
            model.OldId = SqlHelper.GetInt(r["OldId"]);
            model.CrateUser = SqlHelper.GetInt(r["CrateUser"]);
            model.AuditState = SqlHelper.GetInt(r["AuditState"]);
            model.AuditUser = SqlHelper.GetInt(r["AuditUser"]);
            model.IsEnabled = SqlHelper.GetInt(r["IsEnabled"]);
            model.AuditDate = SqlHelper.GetDateTime(r["AuditDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.vdef1 = SqlHelper.GetString(r["vdef1"]);
            model.vdef2 = SqlHelper.GetString(r["vdef2"]);
            model.vdef3 = SqlHelper.GetString(r["vdef3"]);
            model.vdef4 = SqlHelper.GetString(r["vdef4"]);
            model.vdef5 = SqlHelper.GetString(r["vdef5"]);
            model.Paytime = SqlHelper.GetDateTime(r["Paytime"]);
            model.vdef6 = SqlHelper.GetString(r["vdef6"]);
            model.guid = SqlHelper.GetString(r["guid"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.PAY_PrePayment> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.PAY_PrePayment>(ds.Tables[0]);
        }
    }
}

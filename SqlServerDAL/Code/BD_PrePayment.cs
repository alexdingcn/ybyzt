//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/5/18 12:07:26
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
    /// 数据访问类 BD_PrePayment
    /// </summary>
    public class BD_PrePayment
    {
        public BD_PrePayment()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_PrePayment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_PrePayment](");
            strSql.Append("[CompID],[DisID],[OrderID],[Start],[PreType],[Money],[CreatDate],[OldId],[CrateUser],[AuditState],[AuditUser],[IsEnabled],[AuditDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@OrderID,@Start,@PreType,@Money,@CreatDate,@OldId,@CrateUser,@AuditState,@AuditUser,@IsEnabled,@AuditDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@Start", SqlDbType.Int),
                    new SqlParameter("@PreType", SqlDbType.Int),
                    new SqlParameter("@Money", SqlDbType.Decimal),
                    new SqlParameter("@CreatDate", SqlDbType.DateTime),
                    new SqlParameter("@OldId", SqlDbType.BigInt),
                    new SqlParameter("@CrateUser", SqlDbType.Int),
                    new SqlParameter("@AuditState", SqlDbType.Int),
                    new SqlParameter("@AuditUser", SqlDbType.VarChar,50),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.OrderID;
            parameters[3].Value = model.Start;
            parameters[4].Value = model.PreType;
            parameters[5].Value = model.Money;

            if (model.CreatDate != DateTime.MinValue)
                parameters[6].Value = model.CreatDate;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.OldId;
            parameters[8].Value = model.CrateUser;
            parameters[9].Value = model.AuditState;

            if (model.AuditUser != null)
                parameters[10].Value = model.AuditUser;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.IsEnabled;

            if (model.AuditDate != DateTime.MinValue)
                parameters[12].Value = model.AuditDate;
            else
                parameters[12].Value = DBNull.Value;

            parameters[13].Value = model.ts;
            parameters[14].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_PrePayment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_PrePayment] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[OrderID]=@OrderID,");
            strSql.Append("[Start]=@Start,");
            strSql.Append("[PreType]=@PreType,");
            strSql.Append("[Money]=@Money,");
            strSql.Append("[CreatDate]=@CreatDate,");
            strSql.Append("[OldId]=@OldId,");
            strSql.Append("[CrateUser]=@CrateUser,");
            strSql.Append("[AuditState]=@AuditState,");
            strSql.Append("[AuditUser]=@AuditUser,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[AuditDate]=@AuditDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@Start", SqlDbType.Int),
                    new SqlParameter("@PreType", SqlDbType.Int),
                    new SqlParameter("@Money", SqlDbType.Decimal),
                    new SqlParameter("@CreatDate", SqlDbType.DateTime),
                    new SqlParameter("@OldId", SqlDbType.BigInt),
                    new SqlParameter("@CrateUser", SqlDbType.Int),
                    new SqlParameter("@AuditState", SqlDbType.Int),
                    new SqlParameter("@AuditUser", SqlDbType.VarChar,50),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.OrderID;
            parameters[4].Value = model.Start;
            parameters[5].Value = model.PreType;
            parameters[6].Value = model.Money;

            if (model.CreatDate != DateTime.MinValue)
                parameters[7].Value = model.CreatDate;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.OldId;
            parameters[9].Value = model.CrateUser;
            parameters[10].Value = model.AuditState;

            if (model.AuditUser != null)
                parameters[11].Value = model.AuditUser;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.IsEnabled;

            if (model.AuditDate != DateTime.MinValue)
                parameters[13].Value = model.AuditDate;
            else
                parameters[13].Value = DBNull.Value;

            parameters[14].Value = model.ts;
            parameters[15].Value = model.dr;
            parameters[16].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_PrePayment] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[BD_PrePayment]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BD_PrePayment]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_PrePayment GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_PrePayment] ");
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
        public IList<Hi.Model.BD_PrePayment> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_PrePayment]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_PrePayment> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BD_PrePayment> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BD_PrePayment]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BD_PrePayment GetModel(DataRow r)
        {
            Hi.Model.BD_PrePayment model = new Hi.Model.BD_PrePayment();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.OrderID = SqlHelper.GetInt(r["OrderID"]);
            model.Start = SqlHelper.GetInt(r["Start"]);
            model.PreType = SqlHelper.GetInt(r["PreType"]);
            model.Money = SqlHelper.GetDecimal(r["Money"]);
            model.CreatDate = SqlHelper.GetDateTime(r["CreatDate"]);
            model.OldId = SqlHelper.GetInt(r["OldId"]);
            model.CrateUser = SqlHelper.GetInt(r["CrateUser"]);
            model.AuditState = SqlHelper.GetInt(r["AuditState"]);
            model.AuditUser = SqlHelper.GetString(r["AuditUser"]);
            model.IsEnabled = SqlHelper.GetInt(r["IsEnabled"]);
            model.AuditDate = SqlHelper.GetDateTime(r["AuditDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.BD_PrePayment> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BD_PrePayment>(ds.Tables[0]);
        }
    }
}

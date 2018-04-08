//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/6/22 16:38:56
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
    /// 数据访问类 BD_Rebate
    /// </summary>
    public partial class BD_Rebate
    {
        public BD_Rebate()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_Rebate model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_Rebate](");
            strSql.Append("[ReceiptNo],[CompID],[DisID],[RebateType],[RebateAmount],[UserdAmount],[EnableAmount],[StartDate],[EndDate],[RebateState],[CreateUserID],[CreateDate],[Remark],[ts],[dr],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@ReceiptNo,@CompID,@DisID,@RebateType,@RebateAmount,@UserdAmount,@EnableAmount,@StartDate,@EndDate,@RebateState,@CreateUserID,@CreateDate,@Remark,@ts,@dr,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@RebateType", SqlDbType.Int),
                    new SqlParameter("@RebateAmount", SqlDbType.Decimal),
                    new SqlParameter("@UserdAmount", SqlDbType.Decimal),
                    new SqlParameter("@EnableAmount", SqlDbType.Decimal),
                    new SqlParameter("@StartDate", SqlDbType.DateTime),
                    new SqlParameter("@EndDate", SqlDbType.DateTime),
                    new SqlParameter("@RebateState", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.BigInt),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.VarChar,800),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };

            if (model.ReceiptNo != null)
                parameters[0].Value = model.ReceiptNo;
            else
                parameters[0].Value = DBNull.Value;

            parameters[1].Value = model.CompID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.RebateType;
            parameters[4].Value = model.RebateAmount;
            parameters[5].Value = model.UserdAmount;
            parameters[6].Value = model.EnableAmount;
            parameters[7].Value = model.StartDate;
            parameters[8].Value = model.EndDate;
            parameters[9].Value = model.RebateState;
            parameters[10].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[11].Value = model.CreateDate;
            else
                parameters[11].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[12].Value = model.Remark;
            else
                parameters[12].Value = DBNull.Value;

            parameters[13].Value = model.ts;
            parameters[14].Value = model.dr;
            parameters[15].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Rebate model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_Rebate] set ");
            strSql.Append("[ReceiptNo]=@ReceiptNo,");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[RebateType]=@RebateType,");
            strSql.Append("[RebateAmount]=@RebateAmount,");
            strSql.Append("[UserdAmount]=@UserdAmount,");
            strSql.Append("[EnableAmount]=@EnableAmount,");
            strSql.Append("[StartDate]=@StartDate,");
            strSql.Append("[EndDate]=@EndDate,");
            strSql.Append("[RebateState]=@RebateState,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@RebateType", SqlDbType.Int),
                    new SqlParameter("@RebateAmount", SqlDbType.Decimal),
                    new SqlParameter("@UserdAmount", SqlDbType.Decimal),
                    new SqlParameter("@EnableAmount", SqlDbType.Decimal),
                    new SqlParameter("@StartDate", SqlDbType.DateTime),
                    new SqlParameter("@EndDate", SqlDbType.DateTime),
                    new SqlParameter("@RebateState", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.BigInt),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.VarChar,800),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;

            if (model.ReceiptNo != null)
                parameters[1].Value = model.ReceiptNo;
            else
                parameters[1].Value = DBNull.Value;

            parameters[2].Value = model.CompID;
            parameters[3].Value = model.DisID;
            parameters[4].Value = model.RebateType;
            parameters[5].Value = model.RebateAmount;
            parameters[6].Value = model.UserdAmount;
            parameters[7].Value = model.EnableAmount;
            parameters[8].Value = model.StartDate;
            parameters[9].Value = model.EndDate;
            parameters[10].Value = model.RebateState;
            parameters[11].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[12].Value = model.CreateDate;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[13].Value = model.Remark;
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
            strSql.Append("delete [BD_Rebate] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[BD_Rebate]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BD_Rebate]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_Rebate GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_Rebate] ");
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
        public IList<Hi.Model.BD_Rebate> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_Rebate]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_Rebate> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BD_Rebate> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BD_Rebate]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BD_Rebate GetModel(DataRow r)
        {
            Hi.Model.BD_Rebate model = new Hi.Model.BD_Rebate();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.ReceiptNo = SqlHelper.GetString(r["ReceiptNo"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.RebateType = SqlHelper.GetInt(r["RebateType"]);
            model.RebateAmount = SqlHelper.GetDecimal(r["RebateAmount"]);
            model.UserdAmount = SqlHelper.GetDecimal(r["UserdAmount"]);
            model.EnableAmount = SqlHelper.GetDecimal(r["EnableAmount"]);
            model.StartDate = SqlHelper.GetDateTime(r["StartDate"]);
            model.EndDate = SqlHelper.GetDateTime(r["EndDate"]);
            model.RebateState = SqlHelper.GetInt(r["RebateState"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.BD_Rebate> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BD_Rebate>(ds.Tables[0]);
        }
    }
}

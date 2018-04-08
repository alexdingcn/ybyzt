//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/6/20 16:29:43
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
        #region 带事务的操作

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Rebate model, SqlTransaction Tran)
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

            //return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        public IList<Hi.Model.BD_Rebate> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction sqltans)
        {
            DataSet ds = GetDataSet(strWhat, strWhere, strOrderby, sqltans);
            return GetList(ds);
        }

        /// <summary>
        /// 获取数据集,在单表查询时使用
        /// </summary>
        public DataSet GetDataSet(string strWhat, string strWhere, string strOrderby, SqlTransaction sqltans)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_Rebate]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            if (sqltans != null)
                return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), sqltans);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_Rebate GetModel(int ID, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_Rebate] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            DataSet ds = null;
            if (Tran != null)
                ds = SqlHelper.Query(strSql.ToString(), Tran, parameters);
            else
                ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);

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

        #endregion
    }
}

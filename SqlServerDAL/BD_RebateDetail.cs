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
    /// 数据访问类 BD_RebateDetail
    /// </summary>
    public　partial class BD_RebateDetail
    {
        #region  成员方法

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_RebateDetail model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_RebateDetail](");
            strSql.Append("[RebateID],[OrderID],[EnableAmount],[Amount],[CreateDate],[CreateUserID],[ts],[dr],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@RebateID,@OrderID,@EnableAmount,@Amount,@CreateDate,@CreateUserID,@ts,@dr,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@RebateID", SqlDbType.BigInt),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@EnableAmount", SqlDbType.Decimal),
                    new SqlParameter("@Amount", SqlDbType.Decimal),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID", SqlDbType.BigInt),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.RebateID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.EnableAmount;
            parameters[3].Value = model.Amount;
            parameters[4].Value = model.CreateDate;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.ts;
            parameters[7].Value = model.dr;
            parameters[8].Value = model.modifyuser;

            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }
        
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_RebateDetail model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_RebateDetail] set ");
            strSql.Append("[RebateID]=@RebateID,");
            strSql.Append("[OrderID]=@OrderID,");
            strSql.Append("[EnableAmount]=@EnableAmount,");
            strSql.Append("[Amount]=@Amount,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@RebateID", SqlDbType.BigInt),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@EnableAmount", SqlDbType.Decimal),
                    new SqlParameter("@Amount", SqlDbType.Decimal),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID", SqlDbType.BigInt),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.RebateID;
            parameters[2].Value = model.OrderID;
            parameters[3].Value = model.EnableAmount;
            parameters[4].Value = model.Amount;
            parameters[5].Value = model.CreateDate;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Value = model.ts;
            parameters[8].Value = model.dr;
            parameters[9].Value = model.modifyuser;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_RebateDetail] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }
        
        /// <summary>
        /// 获取数据集,在单表查询时使用
        /// </summary>
        public DataSet GetDataSet(string strWhat, string strWhere, string strOrderby, SqlTransaction sqltans)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_RebateDetail]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            if (sqltans != null)
                return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), sqltans);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }
        
        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_RebateDetail> GetList(string strWhat, string strWhere, string strOrderby, SqlTransaction sqltans)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby, sqltans));
        }


        /// <summary>
        /// 获取用户以及用户明细表数据
        /// </summary>
        public DataTable GetList(int pageSize, int pageIndex, string fldSort, bool sort, string fldName, string TbName, string strCondition, out int pageCount, out int count, bool IsDistinct = false, string CustomFldName="")
        {
            if (string.IsNullOrEmpty(TbName))
            {
                TbName = "[BD_RebateDetail]";
            }
            if (string.IsNullOrEmpty(fldName))
            {
                fldName = " * ";
            }
            string strSql;
            DataSet ds = SqlHelper.PageList2(SqlHelper.LocalSqlServer, TbName, fldName, pageSize, pageIndex, fldSort, sort, strCondition, " BD_RebateDetail.ID ", IsDistinct, out pageCount, out count, out strSql, CustomFldName);
            return ds.Tables[0];
        }

        #endregion
    }
}

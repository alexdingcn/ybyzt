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
    /// 数据访问类 DIS_OrderLog
    /// </summary>
    public class DIS_OrderLog
    {
        public DIS_OrderLog()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_OrderLog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_OrderLog](");
            strSql.Append("[ReceiptID],[ReceiptNo],[ReceiptType],[ReceiptState],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@ReceiptID,@ReceiptNo,@ReceiptType,@ReceiptState,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@ReceiptID", SqlDbType.BigInt),
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@ReceiptType", SqlDbType.Int),
                    new SqlParameter("@ReceiptState", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ReceiptID;

            if (model.ReceiptNo != null)
                parameters[1].Value = model.ReceiptNo;
            else
                parameters[1].Value = DBNull.Value;

            parameters[2].Value = model.ReceiptType;
            parameters[3].Value = model.ReceiptState;
            parameters[4].Value = model.CreateUserID;
            parameters[5].Value = model.CreateDate;
            parameters[6].Value = model.ts;
            parameters[7].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_OrderLog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_OrderLog] set ");
            strSql.Append("[ReceiptID]=@ReceiptID,");
            strSql.Append("[ReceiptNo]=@ReceiptNo,");
            strSql.Append("[ReceiptType]=@ReceiptType,");
            strSql.Append("[ReceiptState]=@ReceiptState,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@ReceiptID", SqlDbType.BigInt),
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@ReceiptType", SqlDbType.Int),
                    new SqlParameter("@ReceiptState", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ReceiptID;

            if (model.ReceiptNo != null)
                parameters[2].Value = model.ReceiptNo;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.ReceiptType;
            parameters[4].Value = model.ReceiptState;
            parameters[5].Value = model.CreateUserID;
            parameters[6].Value = model.CreateDate;
            parameters[7].Value = model.ts;
            parameters[8].Value = model.dr;
            parameters[9].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [DIS_OrderLog] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[DIS_OrderLog]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [DIS_OrderLog]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.DIS_OrderLog GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [DIS_OrderLog] ");
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
        public IList<Hi.Model.DIS_OrderLog> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [DIS_OrderLog]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.DIS_OrderLog> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.DIS_OrderLog> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[DIS_OrderLog]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.DIS_OrderLog GetModel(DataRow r)
        {
            Hi.Model.DIS_OrderLog model = new Hi.Model.DIS_OrderLog();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.ReceiptID = SqlHelper.GetInt(r["ReceiptID"]);
            model.ReceiptNo = SqlHelper.GetString(r["ReceiptNo"]);
            model.ReceiptType = SqlHelper.GetInt(r["ReceiptType"]);
            model.ReceiptState = SqlHelper.GetInt(r["ReceiptState"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.DIS_OrderLog> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.DIS_OrderLog>(ds.Tables[0]);
        }
    }
}

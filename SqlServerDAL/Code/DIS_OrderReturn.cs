//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/4/29 17:45:56
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
    /// 数据访问类 DIS_OrderReturn
    /// </summary>
    public partial class DIS_OrderReturn
    {
        public DIS_OrderReturn()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_OrderReturn model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_OrderReturn](");
            strSql.Append("[ReceiptNo],[OrderID],[DisID],[CompID],[ReturnDate],[ReturnUserID],[ReturnContent],[Express],[ExpressNo],[ReturnState],[AuditUserID],[AuditDate],[AuditRemark],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@ReceiptNo,@OrderID,@DisID,@CompID,@ReturnDate,@ReturnUserID,@ReturnContent,@Express,@ExpressNo,@ReturnState,@AuditUserID,@AuditDate,@AuditRemark,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@ReturnDate", SqlDbType.DateTime),
                    new SqlParameter("@ReturnUserID", SqlDbType.Int),
                    new SqlParameter("@ReturnContent", SqlDbType.Text),
                    new SqlParameter("@Express", SqlDbType.VarChar,50),
                    new SqlParameter("@ExpressNo", SqlDbType.VarChar,50),
                    new SqlParameter("@ReturnState", SqlDbType.Int),
                    new SqlParameter("@AuditUserID", SqlDbType.Int),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@AuditRemark", SqlDbType.VarChar,800),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };

            if (model.ReceiptNo != null)
                parameters[0].Value = model.ReceiptNo;
            else
                parameters[0].Value = DBNull.Value;

            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.CompID;

            if (model.ReturnDate != DateTime.MinValue)
                parameters[4].Value = model.ReturnDate;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.ReturnUserID;
            parameters[6].Value = model.ReturnContent;

            if (model.Express != null)
                parameters[7].Value = model.Express;
            else
                parameters[7].Value = DBNull.Value;


            if (model.ExpressNo != null)
                parameters[8].Value = model.ExpressNo;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.ReturnState;
            parameters[10].Value = model.AuditUserID;

            if (model.AuditDate != DateTime.MinValue)
                parameters[11].Value = model.AuditDate;
            else
                parameters[11].Value = DBNull.Value;


            if (model.AuditRemark != null)
                parameters[12].Value = model.AuditRemark;
            else
                parameters[12].Value = DBNull.Value;

            parameters[13].Value = model.CreateUserID;
            parameters[14].Value = model.CreateDate;
            parameters[15].Value = model.ts;
            parameters[16].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_OrderReturn model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_OrderReturn] set ");
            strSql.Append("[ReceiptNo]=@ReceiptNo,");
            strSql.Append("[OrderID]=@OrderID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[ReturnDate]=@ReturnDate,");
            strSql.Append("[ReturnUserID]=@ReturnUserID,");
            strSql.Append("[ReturnContent]=@ReturnContent,");
            strSql.Append("[Express]=@Express,");
            strSql.Append("[ExpressNo]=@ExpressNo,");
            strSql.Append("[ReturnState]=@ReturnState,");
            strSql.Append("[AuditUserID]=@AuditUserID,");
            strSql.Append("[AuditDate]=@AuditDate,");
            strSql.Append("[AuditRemark]=@AuditRemark,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@ReturnDate", SqlDbType.DateTime),
                    new SqlParameter("@ReturnUserID", SqlDbType.Int),
                    new SqlParameter("@ReturnContent", SqlDbType.Text),
                    new SqlParameter("@Express", SqlDbType.VarChar,50),
                    new SqlParameter("@ExpressNo", SqlDbType.VarChar,50),
                    new SqlParameter("@ReturnState", SqlDbType.Int),
                    new SqlParameter("@AuditUserID", SqlDbType.Int),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@AuditRemark", SqlDbType.VarChar,800),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;

            if (model.ReceiptNo != null)
                parameters[1].Value = model.ReceiptNo;
            else
                parameters[1].Value = DBNull.Value;

            parameters[2].Value = model.OrderID;
            parameters[3].Value = model.DisID;
            parameters[4].Value = model.CompID;

            if (model.ReturnDate != DateTime.MinValue)
                parameters[5].Value = model.ReturnDate;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.ReturnUserID;
            parameters[7].Value = model.ReturnContent;

            if (model.Express != null)
                parameters[8].Value = model.Express;
            else
                parameters[8].Value = DBNull.Value;


            if (model.ExpressNo != null)
                parameters[9].Value = model.ExpressNo;
            else
                parameters[9].Value = DBNull.Value;

            parameters[10].Value = model.ReturnState;
            parameters[11].Value = model.AuditUserID;

            if (model.AuditDate != DateTime.MinValue)
                parameters[12].Value = model.AuditDate;
            else
                parameters[12].Value = DBNull.Value;


            if (model.AuditRemark != null)
                parameters[13].Value = model.AuditRemark;
            else
                parameters[13].Value = DBNull.Value;

            parameters[14].Value = model.CreateUserID;
            parameters[15].Value = model.CreateDate;
            parameters[16].Value = model.ts;
            parameters[17].Value = model.dr;
            parameters[18].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [DIS_OrderReturn] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[DIS_OrderReturn]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [DIS_OrderReturn]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.DIS_OrderReturn GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [DIS_OrderReturn] ");
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
        public IList<Hi.Model.DIS_OrderReturn> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [DIS_OrderReturn]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.DIS_OrderReturn> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.DIS_OrderReturn> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[DIS_OrderReturn]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.DIS_OrderReturn GetModel(DataRow r)
        {
            Hi.Model.DIS_OrderReturn model = new Hi.Model.DIS_OrderReturn();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.ReceiptNo = SqlHelper.GetString(r["ReceiptNo"]);
            model.OrderID = SqlHelper.GetInt(r["OrderID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.ReturnDate = SqlHelper.GetDateTime(r["ReturnDate"]);
            model.ReturnUserID = SqlHelper.GetInt(r["ReturnUserID"]);
            model.ReturnContent = SqlHelper.GetString(r["ReturnContent"]);
            model.Express = SqlHelper.GetString(r["Express"]);
            model.ExpressNo = SqlHelper.GetString(r["ExpressNo"]);
            model.ReturnState = SqlHelper.GetInt(r["ReturnState"]);
            model.AuditUserID = SqlHelper.GetInt(r["AuditUserID"]);
            model.AuditDate = SqlHelper.GetDateTime(r["AuditDate"]);
            model.AuditRemark = SqlHelper.GetString(r["AuditRemark"]);
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
        private IList<Hi.Model.DIS_OrderReturn> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.DIS_OrderReturn>(ds.Tables[0]);
        }
    }
}

//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/10/12 17:27:21
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
    /// 数据访问类 DIS_OrderOut
    /// </summary>
    public partial class DIS_OrderOut
    {
        public DIS_OrderOut()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_OrderOut model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_OrderOut](");
            strSql.Append("[OrderID],[DisID],[CompID],[ReceiptNo],[SendDate],[ActionUser],[Remark],[IsAudit],[AuditUserID],[AuditDate],[AuditRemark],[SignDate],[IsSign],[SignUserId],[SignUser],[SignRemark],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@OrderID,@DisID,@CompID,@ReceiptNo,@SendDate,@ActionUser,@Remark,@IsAudit,@AuditUserID,@AuditDate,@AuditRemark,@SignDate,@IsSign,@SignUserId,@SignUser,@SignRemark,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@SendDate", SqlDbType.DateTime),
                    new SqlParameter("@ActionUser", SqlDbType.VarChar,50),
                    new SqlParameter("@Remark", SqlDbType.VarChar,800),
                    new SqlParameter("@IsAudit", SqlDbType.Int),
                    new SqlParameter("@AuditUserID", SqlDbType.Int),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@AuditRemark", SqlDbType.VarChar,800),
                    new SqlParameter("@SignDate", SqlDbType.DateTime),
                    new SqlParameter("@IsSign", SqlDbType.Int),
                    new SqlParameter("@SignUserId", SqlDbType.Int),
                    new SqlParameter("@SignUser", SqlDbType.VarChar,50),
                    new SqlParameter("@SignRemark", SqlDbType.VarChar,400),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.OrderID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.CompID;

            if (model.ReceiptNo != null)
                parameters[3].Value = model.ReceiptNo;
            else
                parameters[3].Value = DBNull.Value;


            if (model.SendDate != DateTime.MinValue)
                parameters[4].Value = model.SendDate;
            else
                parameters[4].Value = DBNull.Value;


            if (model.ActionUser != null)
                parameters[5].Value = model.ActionUser;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[6].Value = model.Remark;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.IsAudit;
            parameters[8].Value = model.AuditUserID;

            if (model.AuditDate != DateTime.MinValue)
                parameters[9].Value = model.AuditDate;
            else
                parameters[9].Value = DBNull.Value;


            if (model.AuditRemark != null)
                parameters[10].Value = model.AuditRemark;
            else
                parameters[10].Value = DBNull.Value;


            if (model.SignDate != DateTime.MinValue)
                parameters[11].Value = model.SignDate;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.IsSign;
            parameters[13].Value = model.SignUserId;

            if (model.SignUser != null)
                parameters[14].Value = model.SignUser;
            else
                parameters[14].Value = DBNull.Value;


            if (model.SignRemark != null)
                parameters[15].Value = model.SignRemark;
            else
                parameters[15].Value = DBNull.Value;

            parameters[16].Value = model.CreateUserID;
            parameters[17].Value = model.CreateDate;
            parameters[18].Value = model.ts;
            parameters[19].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_OrderOut model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_OrderOut] set ");
            strSql.Append("[OrderID]=@OrderID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[ReceiptNo]=@ReceiptNo,");
            strSql.Append("[SendDate]=@SendDate,");
            strSql.Append("[ActionUser]=@ActionUser,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[IsAudit]=@IsAudit,");
            strSql.Append("[AuditUserID]=@AuditUserID,");
            strSql.Append("[AuditDate]=@AuditDate,");
            strSql.Append("[AuditRemark]=@AuditRemark,");
            strSql.Append("[SignDate]=@SignDate,");
            strSql.Append("[IsSign]=@IsSign,");
            strSql.Append("[SignUserId]=@SignUserId,");
            strSql.Append("[SignUser]=@SignUser,");
            strSql.Append("[SignRemark]=@SignRemark,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@SendDate", SqlDbType.DateTime),
                    new SqlParameter("@ActionUser", SqlDbType.VarChar,50),
                    new SqlParameter("@Remark", SqlDbType.VarChar,800),
                    new SqlParameter("@IsAudit", SqlDbType.Int),
                    new SqlParameter("@AuditUserID", SqlDbType.Int),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@AuditRemark", SqlDbType.VarChar,800),
                    new SqlParameter("@SignDate", SqlDbType.DateTime),
                    new SqlParameter("@IsSign", SqlDbType.Int),
                    new SqlParameter("@SignUserId", SqlDbType.Int),
                    new SqlParameter("@SignUser", SqlDbType.VarChar,50),
                    new SqlParameter("@SignRemark", SqlDbType.VarChar,400),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.CompID;

            if (model.ReceiptNo != null)
                parameters[4].Value = model.ReceiptNo;
            else
                parameters[4].Value = DBNull.Value;


            if (model.SendDate != DateTime.MinValue)
                parameters[5].Value = model.SendDate;
            else
                parameters[5].Value = DBNull.Value;


            if (model.ActionUser != null)
                parameters[6].Value = model.ActionUser;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[7].Value = model.Remark;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.IsAudit;
            parameters[9].Value = model.AuditUserID;

            if (model.AuditDate != DateTime.MinValue)
                parameters[10].Value = model.AuditDate;
            else
                parameters[10].Value = DBNull.Value;


            if (model.AuditRemark != null)
                parameters[11].Value = model.AuditRemark;
            else
                parameters[11].Value = DBNull.Value;


            if (model.SignDate != DateTime.MinValue)
                parameters[12].Value = model.SignDate;
            else
                parameters[12].Value = DBNull.Value;

            parameters[13].Value = model.IsSign;
            parameters[14].Value = model.SignUserId;

            if (model.SignUser != null)
                parameters[15].Value = model.SignUser;
            else
                parameters[15].Value = DBNull.Value;


            if (model.SignRemark != null)
                parameters[16].Value = model.SignRemark;
            else
                parameters[16].Value = DBNull.Value;

            parameters[17].Value = model.CreateUserID;
            parameters[18].Value = model.CreateDate;
            parameters[19].Value = model.ts;
            parameters[20].Value = model.dr;
            parameters[21].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [DIS_OrderOut] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[DIS_OrderOut]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [DIS_OrderOut]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.DIS_OrderOut GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [DIS_OrderOut] ");
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
        public IList<Hi.Model.DIS_OrderOut> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [DIS_OrderOut]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.DIS_OrderOut> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.DIS_OrderOut> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[DIS_OrderOut]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.DIS_OrderOut GetModel(DataRow r)
        {
            Hi.Model.DIS_OrderOut model = new Hi.Model.DIS_OrderOut();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.OrderID = SqlHelper.GetInt(r["OrderID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.ReceiptNo = SqlHelper.GetString(r["ReceiptNo"]);
            model.SendDate = SqlHelper.GetDateTime(r["SendDate"]);
            model.ActionUser = SqlHelper.GetString(r["ActionUser"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.IsAudit = SqlHelper.GetInt(r["IsAudit"]);
            model.AuditUserID = SqlHelper.GetInt(r["AuditUserID"]);
            model.AuditDate = SqlHelper.GetDateTime(r["AuditDate"]);
            model.AuditRemark = SqlHelper.GetString(r["AuditRemark"]);
            model.SignDate = SqlHelper.GetDateTime(r["SignDate"]);
            model.IsSign = SqlHelper.GetInt(r["IsSign"]);
            model.SignUserId = SqlHelper.GetInt(r["SignUserId"]);
            model.SignUser = SqlHelper.GetString(r["SignUser"]);
            model.SignRemark = SqlHelper.GetString(r["SignRemark"]);
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
        private IList<Hi.Model.DIS_OrderOut> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.DIS_OrderOut>(ds.Tables[0]);
        }
    }
}

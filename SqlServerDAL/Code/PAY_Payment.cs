//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/8/31 9:27:51
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
    /// 数据访问类 PAY_Payment
    /// </summary>
    public partial class PAY_Payment
    {
        public PAY_Payment()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.PAY_Payment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [PAY_Payment](");
            strSql.Append("[OrderID],[DisID],[Type],[Channel],[PayDate],[PayUser],[PayPrice],[Remark],[IsAudit],[State],[PrintNum],[AuditUserID],[AuditDate],[CreateUserID],[CreateDate],[ts],[modifyuser],[guid],[verifystatus],[status],[vdef3],[vdef4],[vdef5],[jsxf_no],[vdef6],[vdef7],[vdef8],[vdef9],[attach],[payName],[paycode],[paybank])");
            strSql.Append(" values (");
            strSql.Append("@OrderID,@DisID,@Type,@Channel,@PayDate,@PayUser,@PayPrice,@Remark,@IsAudit,@State,@PrintNum,@AuditUserID,@AuditDate,@CreateUserID,@CreateDate,@ts,@modifyuser,@guid,@verifystatus,@status,@vdef3,@vdef4,@vdef5,@jsxf_no,@vdef6,@vdef7,@vdef8,@vdef9,@attach,@payName,@paycode,@paybank)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@Channel", SqlDbType.VarChar,50),
                    new SqlParameter("@PayDate", SqlDbType.DateTime),
                    new SqlParameter("@PayUser", SqlDbType.VarChar,100),
                    new SqlParameter("@PayPrice", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,800),
                    new SqlParameter("@IsAudit", SqlDbType.Int),
                    new SqlParameter("@State", SqlDbType.Int),
                    new SqlParameter("@PrintNum", SqlDbType.Int),
                    new SqlParameter("@AuditUserID", SqlDbType.BigInt),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID", SqlDbType.BigInt),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@guid", SqlDbType.NVarChar,50),
                    new SqlParameter("@verifystatus", SqlDbType.Int),
                    new SqlParameter("@status", SqlDbType.Int),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,131),
                    new SqlParameter("@vdef4", SqlDbType.VarChar,132),
                    new SqlParameter("@vdef5", SqlDbType.VarChar,133),
                    new SqlParameter("@jsxf_no", SqlDbType.Int),
                    new SqlParameter("@vdef6", SqlDbType.NVarChar,132),
                    new SqlParameter("@vdef7", SqlDbType.NVarChar,132),
                    new SqlParameter("@vdef8", SqlDbType.NVarChar,132),
                    new SqlParameter("@vdef9", SqlDbType.NVarChar,132),
                    new SqlParameter("@attach", SqlDbType.NVarChar,1000),
                    new SqlParameter("@payName", SqlDbType.NVarChar,100),
                    new SqlParameter("@paycode", SqlDbType.NVarChar,100),
                    new SqlParameter("@paybank", SqlDbType.NVarChar,100)
            };
            parameters[0].Value = model.OrderID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.Type;

            if (model.Channel != null)
                parameters[3].Value = model.Channel;
            else
                parameters[3].Value = DBNull.Value;


            if (model.PayDate != DateTime.MinValue)
                parameters[4].Value = model.PayDate;
            else
                parameters[4].Value = DBNull.Value;


            if (model.PayUser != null)
                parameters[5].Value = model.PayUser;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.PayPrice;

            if (model.Remark != null)
                parameters[7].Value = model.Remark;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.IsAudit;
            parameters[9].Value = model.State;
            parameters[10].Value = model.PrintNum;
            parameters[11].Value = model.AuditUserID;

            if (model.AuditDate != DateTime.MinValue)
                parameters[12].Value = model.AuditDate;
            else
                parameters[12].Value = DBNull.Value;

            parameters[13].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[14].Value = model.CreateDate;
            else
                parameters[14].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[15].Value = model.ts;
            else
                parameters[15].Value = DBNull.Value;

            parameters[16].Value = model.modifyuser;

            if (model.guid != null)
                parameters[17].Value = model.guid;
            else
                parameters[17].Value = DBNull.Value;

            parameters[18].Value = model.verifystatus;
            parameters[19].Value = model.status;

            if (model.vdef3 != null)
                parameters[20].Value = model.vdef3;
            else
                parameters[20].Value = DBNull.Value;


            if (model.vdef4 != null)
                parameters[21].Value = model.vdef4;
            else
                parameters[21].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[22].Value = model.vdef5;
            else
                parameters[22].Value = DBNull.Value;

            parameters[23].Value = model.jsxf_no;

            if (model.vdef6 != null)
                parameters[24].Value = model.vdef6;
            else
                parameters[24].Value = DBNull.Value;


            if (model.vdef7 != null)
                parameters[25].Value = model.vdef7;
            else
                parameters[25].Value = DBNull.Value;


            if (model.vdef8 != null)
                parameters[26].Value = model.vdef8;
            else
                parameters[26].Value = DBNull.Value;


            if (model.vdef9 != null)
                parameters[27].Value = model.vdef9;
            else
                parameters[27].Value = DBNull.Value;


            if (model.attach != null)
                parameters[28].Value = model.attach;
            else
                parameters[28].Value = DBNull.Value;


            if (model.payName != null)
                parameters[29].Value = model.payName;
            else
                parameters[29].Value = DBNull.Value;


            if (model.paycode != null)
                parameters[30].Value = model.paycode;
            else
                parameters[30].Value = DBNull.Value;


            if (model.paybank != null)
                parameters[31].Value = model.paybank;
            else
                parameters[31].Value = DBNull.Value;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.PAY_Payment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [PAY_Payment] set ");
            strSql.Append("[OrderID]=@OrderID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[Type]=@Type,");
            strSql.Append("[Channel]=@Channel,");
            strSql.Append("[PayDate]=@PayDate,");
            strSql.Append("[PayUser]=@PayUser,");
            strSql.Append("[PayPrice]=@PayPrice,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[IsAudit]=@IsAudit,");
            strSql.Append("[State]=@State,");
            strSql.Append("[PrintNum]=@PrintNum,");
            strSql.Append("[AuditUserID]=@AuditUserID,");
            strSql.Append("[AuditDate]=@AuditDate,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[guid]=@guid,");
            strSql.Append("[verifystatus]=@verifystatus,");
            strSql.Append("[status]=@status,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[vdef4]=@vdef4,");
            strSql.Append("[vdef5]=@vdef5,");
            strSql.Append("[jsxf_no]=@jsxf_no,");
            strSql.Append("[vdef6]=@vdef6,");
            strSql.Append("[vdef7]=@vdef7,");
            strSql.Append("[vdef8]=@vdef8,");
            strSql.Append("[vdef9]=@vdef9,");
            strSql.Append("[attach]=@attach,");
            strSql.Append("[payName]=@payName,");
            strSql.Append("[paycode]=@paycode,");
            strSql.Append("[paybank]=@paybank");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@Channel", SqlDbType.VarChar,50),
                    new SqlParameter("@PayDate", SqlDbType.DateTime),
                    new SqlParameter("@PayUser", SqlDbType.VarChar,100),
                    new SqlParameter("@PayPrice", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,800),
                    new SqlParameter("@IsAudit", SqlDbType.Int),
                    new SqlParameter("@State", SqlDbType.Int),
                    new SqlParameter("@PrintNum", SqlDbType.Int),
                    new SqlParameter("@AuditUserID", SqlDbType.BigInt),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID", SqlDbType.BigInt),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@guid", SqlDbType.NVarChar,50),
                    new SqlParameter("@verifystatus", SqlDbType.Int),
                    new SqlParameter("@status", SqlDbType.Int),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,131),
                    new SqlParameter("@vdef4", SqlDbType.VarChar,132),
                    new SqlParameter("@vdef5", SqlDbType.VarChar,133),
                    new SqlParameter("@jsxf_no", SqlDbType.Int),
                    new SqlParameter("@vdef6", SqlDbType.NVarChar,132),
                    new SqlParameter("@vdef7", SqlDbType.NVarChar,132),
                    new SqlParameter("@vdef8", SqlDbType.NVarChar,132),
                    new SqlParameter("@vdef9", SqlDbType.NVarChar,132),
                    new SqlParameter("@attach", SqlDbType.NVarChar,1000),
                    new SqlParameter("@payName", SqlDbType.NVarChar,100),
                    new SqlParameter("@paycode", SqlDbType.NVarChar,100),
                    new SqlParameter("@paybank", SqlDbType.NVarChar,100)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.Type;

            if (model.Channel != null)
                parameters[4].Value = model.Channel;
            else
                parameters[4].Value = DBNull.Value;


            if (model.PayDate != DateTime.MinValue)
                parameters[5].Value = model.PayDate;
            else
                parameters[5].Value = DBNull.Value;


            if (model.PayUser != null)
                parameters[6].Value = model.PayUser;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.PayPrice;

            if (model.Remark != null)
                parameters[8].Value = model.Remark;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.IsAudit;
            parameters[10].Value = model.State;
            parameters[11].Value = model.PrintNum;
            parameters[12].Value = model.AuditUserID;

            if (model.AuditDate != DateTime.MinValue)
                parameters[13].Value = model.AuditDate;
            else
                parameters[13].Value = DBNull.Value;

            parameters[14].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[15].Value = model.CreateDate;
            else
                parameters[15].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[16].Value = model.ts;
            else
                parameters[16].Value = DBNull.Value;

            parameters[17].Value = model.dr;
            parameters[18].Value = model.modifyuser;

            if (model.guid != null)
                parameters[19].Value = model.guid;
            else
                parameters[19].Value = DBNull.Value;

            parameters[20].Value = model.verifystatus;
            parameters[21].Value = model.status;

            if (model.vdef3 != null)
                parameters[22].Value = model.vdef3;
            else
                parameters[22].Value = DBNull.Value;


            if (model.vdef4 != null)
                parameters[23].Value = model.vdef4;
            else
                parameters[23].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[24].Value = model.vdef5;
            else
                parameters[24].Value = DBNull.Value;

            parameters[25].Value = model.jsxf_no;

            if (model.vdef6 != null)
                parameters[26].Value = model.vdef6;
            else
                parameters[26].Value = DBNull.Value;


            if (model.vdef7 != null)
                parameters[27].Value = model.vdef7;
            else
                parameters[27].Value = DBNull.Value;


            if (model.vdef8 != null)
                parameters[28].Value = model.vdef8;
            else
                parameters[28].Value = DBNull.Value;


            if (model.vdef9 != null)
                parameters[29].Value = model.vdef9;
            else
                parameters[29].Value = DBNull.Value;


            if (model.attach != null)
                parameters[30].Value = model.attach;
            else
                parameters[30].Value = DBNull.Value;


            if (model.payName != null)
                parameters[31].Value = model.payName;
            else
                parameters[31].Value = DBNull.Value;


            if (model.paycode != null)
                parameters[32].Value = model.paycode;
            else
                parameters[32].Value = DBNull.Value;


            if (model.paybank != null)
                parameters[33].Value = model.paybank;
            else
                parameters[33].Value = DBNull.Value;


            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [PAY_Payment] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[PAY_Payment]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [PAY_Payment]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.PAY_Payment GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [PAY_Payment] ");
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
        public IList<Hi.Model.PAY_Payment> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [PAY_Payment]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.PAY_Payment> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.PAY_Payment> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[PAY_Payment]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.PAY_Payment GetModel(DataRow r)
        {
            Hi.Model.PAY_Payment model = new Hi.Model.PAY_Payment();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.OrderID = SqlHelper.GetInt(r["OrderID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.Type = SqlHelper.GetInt(r["Type"]);
            model.Channel = SqlHelper.GetString(r["Channel"]);
            model.PayDate = SqlHelper.GetDateTime(r["PayDate"]);
            model.PayUser = SqlHelper.GetString(r["PayUser"]);
            model.PayPrice = SqlHelper.GetDecimal(r["PayPrice"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.IsAudit = SqlHelper.GetInt(r["IsAudit"]);
            model.State = SqlHelper.GetInt(r["State"]);
            model.PrintNum = SqlHelper.GetInt(r["PrintNum"]);
            model.AuditUserID = SqlHelper.GetInt(r["AuditUserID"]);
            model.AuditDate = SqlHelper.GetDateTime(r["AuditDate"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.guid = SqlHelper.GetString(r["guid"]);
            model.verifystatus = SqlHelper.GetInt(r["verifystatus"]);
            model.status = SqlHelper.GetInt(r["status"]);
            model.vdef3 = SqlHelper.GetString(r["vdef3"]);
            model.vdef4 = SqlHelper.GetString(r["vdef4"]);
            model.vdef5 = SqlHelper.GetString(r["vdef5"]);
            model.jsxf_no = SqlHelper.GetInt(r["jsxf_no"]);
            model.vdef6 = SqlHelper.GetString(r["vdef6"]);
            model.vdef7 = SqlHelper.GetString(r["vdef7"]);
            model.vdef8 = SqlHelper.GetString(r["vdef8"]);
            model.vdef9 = SqlHelper.GetString(r["vdef9"]);
            model.attach = SqlHelper.GetString(r["attach"]);
            model.payName = SqlHelper.GetString(r["payName"]);
            model.paycode = SqlHelper.GetString(r["paycode"]);
            model.paybank = SqlHelper.GetString(r["paybank"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.PAY_Payment> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.PAY_Payment>(ds.Tables[0]);
        }
    }
}

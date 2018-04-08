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
    /// 数据访问类 DIS_Order
    /// </summary>
    public partial class DIS_Order
    {
        public DIS_Order()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_Order model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_Order](");
            strSql.Append("[CompID],[DisID],[DisUserID],[AddType],[Otype],[IsAudit],[AddrID],[ReceiptNo],[GUID],[ArriveDate],[TotalAmount],[AuditAmount],[OtherAmount],[PayedAmount],[Principal],[Phone],[Address],[Remark],[OState],[PayState],[ReturnState],[AuditUserID],[AuditDate],[AuditRemark],[CreateUserID],[CreateDate],[ReturnMoneyDate],[ReturnMoneyUserId],[ReturnMoneyUser],[ts],[modifyuser],[Atta],[IsOutState],[IsPayColl],[CostSub],[bateAmount],[IsSettl],[GiveMode],[PostFee],[vdef1],[vdef2],[vdef3])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@DisUserID,@AddType,@Otype,@IsAudit,@AddrID,@ReceiptNo,@GUID,@ArriveDate,@TotalAmount,@AuditAmount,@OtherAmount,@PayedAmount,@Principal,@Phone,@Address,@Remark,@OState,@PayState,@ReturnState,@AuditUserID,@AuditDate,@AuditRemark,@CreateUserID,@CreateDate,@ReturnMoneyDate,@ReturnMoneyUserId,@ReturnMoneyUser,@ts,@modifyuser,@Atta,@IsOutState,@IsPayColl,@CostSub,@bateAmount,@IsSettl,@GiveMode,@PostFee,@vdef1,@vdef2,@vdef3)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@DisUserID", SqlDbType.Int),
                    new SqlParameter("@AddType", SqlDbType.Int),
                    new SqlParameter("@Otype", SqlDbType.Int),
                    new SqlParameter("@IsAudit", SqlDbType.Int),
                    new SqlParameter("@AddrID", SqlDbType.Int),
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@GUID", SqlDbType.VarChar,50),
                    new SqlParameter("@ArriveDate", SqlDbType.DateTime),
                    new SqlParameter("@TotalAmount", SqlDbType.Decimal),
                    new SqlParameter("@AuditAmount", SqlDbType.Decimal),
                    new SqlParameter("@OtherAmount", SqlDbType.Decimal),
                    new SqlParameter("@PayedAmount", SqlDbType.Decimal),
                    new SqlParameter("@Principal", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Address", SqlDbType.VarChar,300),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@OState", SqlDbType.Int),
                    new SqlParameter("@PayState", SqlDbType.Int),
                    new SqlParameter("@ReturnState", SqlDbType.Int),
                    new SqlParameter("@AuditUserID", SqlDbType.Int),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@AuditRemark", SqlDbType.VarChar,800),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ReturnMoneyDate", SqlDbType.DateTime),
                    new SqlParameter("@ReturnMoneyUserId", SqlDbType.Int),
                    new SqlParameter("@ReturnMoneyUser", SqlDbType.VarChar,50),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@Atta", SqlDbType.VarChar,1024),
                    new SqlParameter("@IsOutState", SqlDbType.Int),
                    new SqlParameter("@IsPayColl", SqlDbType.VarChar,128),
                    new SqlParameter("@CostSub", SqlDbType.VarChar,128),
                    new SqlParameter("@bateAmount", SqlDbType.Decimal),
                    new SqlParameter("@IsSettl", SqlDbType.VarChar,134),
                    new SqlParameter("@GiveMode", SqlDbType.VarChar,50),
                    new SqlParameter("@PostFee", SqlDbType.Decimal),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.DisUserID;
            parameters[3].Value = model.AddType;
            parameters[4].Value = model.Otype;
            parameters[5].Value = model.IsAudit;
            parameters[6].Value = model.AddrID;
            parameters[7].Value = model.ReceiptNo;

            if (model.GUID != null)
                parameters[8].Value = model.GUID;
            else
                parameters[8].Value = DBNull.Value;


            if (model.ArriveDate != DateTime.MinValue)
                parameters[9].Value = model.ArriveDate;
            else
                parameters[9].Value = DBNull.Value;

            parameters[10].Value = model.TotalAmount;
            parameters[11].Value = model.AuditAmount;
            parameters[12].Value = model.OtherAmount;
            parameters[13].Value = model.PayedAmount;

            if (model.Principal != null)
                parameters[14].Value = model.Principal;
            else
                parameters[14].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[15].Value = model.Phone;
            else
                parameters[15].Value = DBNull.Value;


            if (model.Address != null)
                parameters[16].Value = model.Address;
            else
                parameters[16].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[17].Value = model.Remark;
            else
                parameters[17].Value = DBNull.Value;

            parameters[18].Value = model.OState;
            parameters[19].Value = model.PayState;
            parameters[20].Value = model.ReturnState;
            parameters[21].Value = model.AuditUserID;

            if (model.AuditDate != DateTime.MinValue)
                parameters[22].Value = model.AuditDate;
            else
                parameters[22].Value = DBNull.Value;


            if (model.AuditRemark != null)
                parameters[23].Value = model.AuditRemark;
            else
                parameters[23].Value = DBNull.Value;

            parameters[24].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[25].Value = model.CreateDate;
            else
                parameters[25].Value = DBNull.Value;


            if (model.ReturnMoneyDate != DateTime.MinValue)
                parameters[26].Value = model.ReturnMoneyDate;
            else
                parameters[26].Value = DBNull.Value;

            parameters[27].Value = model.ReturnMoneyUserId;

            if (model.ReturnMoneyUser != null)
                parameters[28].Value = model.ReturnMoneyUser;
            else
                parameters[28].Value = DBNull.Value;

            parameters[29].Value = model.ts;
            parameters[30].Value = model.modifyuser;

            if (model.Atta != null)
                parameters[31].Value = model.Atta;
            else
                parameters[31].Value = DBNull.Value;

            parameters[32].Value = model.IsOutState;

            if (model.IsPayColl != null)
                parameters[33].Value = model.IsPayColl;
            else
                parameters[33].Value = DBNull.Value;


            if (model.CostSub != null)
                parameters[34].Value = model.CostSub;
            else
                parameters[34].Value = DBNull.Value;

            parameters[35].Value = model.bateAmount;

            if (model.IsSettl != null)
                parameters[36].Value = model.IsSettl;
            else
                parameters[36].Value = DBNull.Value;


            if (model.GiveMode != null)
                parameters[37].Value = model.GiveMode;
            else
                parameters[37].Value = DBNull.Value;

            parameters[38].Value = model.PostFee;

            if (model.vdef1 != null)
                parameters[39].Value = model.vdef1;
            else
                parameters[39].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[40].Value = model.vdef2;
            else
                parameters[40].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[41].Value = model.vdef3;
            else
                parameters[41].Value = DBNull.Value;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_Order model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_Order] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[DisUserID]=@DisUserID,");
            strSql.Append("[AddType]=@AddType,");
            strSql.Append("[Otype]=@Otype,");
            strSql.Append("[IsAudit]=@IsAudit,");
            strSql.Append("[AddrID]=@AddrID,");
            strSql.Append("[ReceiptNo]=@ReceiptNo,");
            strSql.Append("[GUID]=@GUID,");
            strSql.Append("[ArriveDate]=@ArriveDate,");
            strSql.Append("[TotalAmount]=@TotalAmount,");
            strSql.Append("[AuditAmount]=@AuditAmount,");
            strSql.Append("[OtherAmount]=@OtherAmount,");
            strSql.Append("[PayedAmount]=@PayedAmount,");
            strSql.Append("[Principal]=@Principal,");
            strSql.Append("[Phone]=@Phone,");
            strSql.Append("[Address]=@Address,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[OState]=@OState,");
            strSql.Append("[PayState]=@PayState,");
            strSql.Append("[ReturnState]=@ReturnState,");
            strSql.Append("[AuditUserID]=@AuditUserID,");
            strSql.Append("[AuditDate]=@AuditDate,");
            strSql.Append("[AuditRemark]=@AuditRemark,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ReturnMoneyDate]=@ReturnMoneyDate,");
            strSql.Append("[ReturnMoneyUserId]=@ReturnMoneyUserId,");
            strSql.Append("[ReturnMoneyUser]=@ReturnMoneyUser,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[Atta]=@Atta,");
            strSql.Append("[IsOutState]=@IsOutState,");
            strSql.Append("[IsPayColl]=@IsPayColl,");
            strSql.Append("[CostSub]=@CostSub,");
            strSql.Append("[bateAmount]=@bateAmount,");
            strSql.Append("[IsSettl]=@IsSettl,");
            strSql.Append("[GiveMode]=@GiveMode,");
            strSql.Append("[PostFee]=@PostFee,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@DisUserID", SqlDbType.Int),
                    new SqlParameter("@AddType", SqlDbType.Int),
                    new SqlParameter("@Otype", SqlDbType.Int),
                    new SqlParameter("@IsAudit", SqlDbType.Int),
                    new SqlParameter("@AddrID", SqlDbType.Int),
                    new SqlParameter("@ReceiptNo", SqlDbType.VarChar,50),
                    new SqlParameter("@GUID", SqlDbType.VarChar,50),
                    new SqlParameter("@ArriveDate", SqlDbType.DateTime),
                    new SqlParameter("@TotalAmount", SqlDbType.Decimal),
                    new SqlParameter("@AuditAmount", SqlDbType.Decimal),
                    new SqlParameter("@OtherAmount", SqlDbType.Decimal),
                    new SqlParameter("@PayedAmount", SqlDbType.Decimal),
                    new SqlParameter("@Principal", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Address", SqlDbType.VarChar,300),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@OState", SqlDbType.Int),
                    new SqlParameter("@PayState", SqlDbType.Int),
                    new SqlParameter("@ReturnState", SqlDbType.Int),
                    new SqlParameter("@AuditUserID", SqlDbType.Int),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@AuditRemark", SqlDbType.VarChar,800),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ReturnMoneyDate", SqlDbType.DateTime),
                    new SqlParameter("@ReturnMoneyUserId", SqlDbType.Int),
                    new SqlParameter("@ReturnMoneyUser", SqlDbType.VarChar,50),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@Atta", SqlDbType.VarChar,1024),
                    new SqlParameter("@IsOutState", SqlDbType.Int),
                    new SqlParameter("@IsPayColl", SqlDbType.VarChar,128),
                    new SqlParameter("@CostSub", SqlDbType.VarChar,128),
                    new SqlParameter("@bateAmount", SqlDbType.Decimal),
                    new SqlParameter("@IsSettl", SqlDbType.VarChar,134),
                    new SqlParameter("@GiveMode", SqlDbType.VarChar,50),
                    new SqlParameter("@PostFee", SqlDbType.Decimal),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.DisUserID;
            parameters[4].Value = model.AddType;
            parameters[5].Value = model.Otype;
            parameters[6].Value = model.IsAudit;
            parameters[7].Value = model.AddrID;
            parameters[8].Value = model.ReceiptNo;

            if (model.GUID != null)
                parameters[9].Value = model.GUID;
            else
                parameters[9].Value = DBNull.Value;


            if (model.ArriveDate != DateTime.MinValue)
                parameters[10].Value = model.ArriveDate;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.TotalAmount;
            parameters[12].Value = model.AuditAmount;
            parameters[13].Value = model.OtherAmount;
            parameters[14].Value = model.PayedAmount;

            if (model.Principal != null)
                parameters[15].Value = model.Principal;
            else
                parameters[15].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[16].Value = model.Phone;
            else
                parameters[16].Value = DBNull.Value;


            if (model.Address != null)
                parameters[17].Value = model.Address;
            else
                parameters[17].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[18].Value = model.Remark;
            else
                parameters[18].Value = DBNull.Value;

            parameters[19].Value = model.OState;
            parameters[20].Value = model.PayState;
            parameters[21].Value = model.ReturnState;
            parameters[22].Value = model.AuditUserID;

            if (model.AuditDate != DateTime.MinValue)
                parameters[23].Value = model.AuditDate;
            else
                parameters[23].Value = DBNull.Value;


            if (model.AuditRemark != null)
                parameters[24].Value = model.AuditRemark;
            else
                parameters[24].Value = DBNull.Value;

            parameters[25].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[26].Value = model.CreateDate;
            else
                parameters[26].Value = DBNull.Value;


            if (model.ReturnMoneyDate != DateTime.MinValue)
                parameters[27].Value = model.ReturnMoneyDate;
            else
                parameters[27].Value = DBNull.Value;

            parameters[28].Value = model.ReturnMoneyUserId;

            if (model.ReturnMoneyUser != null)
                parameters[29].Value = model.ReturnMoneyUser;
            else
                parameters[29].Value = DBNull.Value;

            parameters[30].Value = model.ts;
            parameters[31].Value = model.dr;
            parameters[32].Value = model.modifyuser;

            if (model.Atta != null)
                parameters[33].Value = model.Atta;
            else
                parameters[33].Value = DBNull.Value;

            parameters[34].Value = model.IsOutState;

            if (model.IsPayColl != null)
                parameters[35].Value = model.IsPayColl;
            else
                parameters[35].Value = DBNull.Value;


            if (model.CostSub != null)
                parameters[36].Value = model.CostSub;
            else
                parameters[36].Value = DBNull.Value;

            parameters[37].Value = model.bateAmount;

            if (model.IsSettl != null)
                parameters[38].Value = model.IsSettl;
            else
                parameters[38].Value = DBNull.Value;


            if (model.GiveMode != null)
                parameters[39].Value = model.GiveMode;
            else
                parameters[39].Value = DBNull.Value;

            parameters[40].Value = model.PostFee;

            if (model.vdef1 != null)
                parameters[41].Value = model.vdef1;
            else
                parameters[41].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[42].Value = model.vdef2;
            else
                parameters[42].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[43].Value = model.vdef3;
            else
                parameters[43].Value = DBNull.Value;


            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [DIS_Order] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[DIS_Order]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [DIS_Order]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.DIS_Order GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [DIS_Order] ");
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
        public IList<Hi.Model.DIS_Order> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [DIS_Order]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.DIS_Order> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.DIS_Order> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[DIS_Order]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.DIS_Order GetModel(DataRow r)
        {
            Hi.Model.DIS_Order model = new Hi.Model.DIS_Order();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.DisUserID = SqlHelper.GetInt(r["DisUserID"]);
            model.AddType = SqlHelper.GetInt(r["AddType"]);
            model.Otype = SqlHelper.GetInt(r["Otype"]);
            model.IsAudit = SqlHelper.GetInt(r["IsAudit"]);
            model.AddrID = SqlHelper.GetInt(r["AddrID"]);
            model.ReceiptNo = SqlHelper.GetString(r["ReceiptNo"]);
            model.GUID = SqlHelper.GetString(r["GUID"]);
            model.ArriveDate = SqlHelper.GetDateTime(r["ArriveDate"]);
            model.TotalAmount = SqlHelper.GetDecimal(r["TotalAmount"]);
            model.AuditAmount = SqlHelper.GetDecimal(r["AuditAmount"]);
            model.OtherAmount = SqlHelper.GetDecimal(r["OtherAmount"]);
            model.PayedAmount = SqlHelper.GetDecimal(r["PayedAmount"]);
            model.Principal = SqlHelper.GetString(r["Principal"]);
            model.Phone = SqlHelper.GetString(r["Phone"]);
            model.Address = SqlHelper.GetString(r["Address"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.OState = SqlHelper.GetInt(r["OState"]);
            model.PayState = SqlHelper.GetInt(r["PayState"]);
            model.ReturnState = SqlHelper.GetInt(r["ReturnState"]);
            model.AuditUserID = SqlHelper.GetInt(r["AuditUserID"]);
            model.AuditDate = SqlHelper.GetDateTime(r["AuditDate"]);
            model.AuditRemark = SqlHelper.GetString(r["AuditRemark"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ReturnMoneyDate = SqlHelper.GetDateTime(r["ReturnMoneyDate"]);
            model.ReturnMoneyUserId = SqlHelper.GetInt(r["ReturnMoneyUserId"]);
            model.ReturnMoneyUser = SqlHelper.GetString(r["ReturnMoneyUser"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.Atta = SqlHelper.GetString(r["Atta"]);
            model.IsOutState = SqlHelper.GetInt(r["IsOutState"]);
            model.IsPayColl = SqlHelper.GetString(r["IsPayColl"]);
            model.CostSub = SqlHelper.GetString(r["CostSub"]);
            model.bateAmount = SqlHelper.GetDecimal(r["bateAmount"]);
            model.IsSettl = SqlHelper.GetString(r["IsSettl"]);
            model.GiveMode = SqlHelper.GetString(r["GiveMode"]);
            model.PostFee = SqlHelper.GetDecimal(r["PostFee"]);
            model.vdef1 = SqlHelper.GetString(r["vdef1"]);
            model.vdef2 = SqlHelper.GetString(r["vdef2"]);
            model.vdef3 = SqlHelper.GetString(r["vdef3"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.DIS_Order> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.DIS_Order>(ds.Tables[0]);
        }
    }
}

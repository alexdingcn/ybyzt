//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/6/5 14:53:11
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
    /// 数据访问类 PAY_FastPayMent
    /// </summary>
    public class PAY_FastPayMent
    {
        public PAY_FastPayMent()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.PAY_FastPayMent model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [PAY_FastPayMent](");
            strSql.Append("[DisID],[BankID],[Number],[AccountName],[bankcode],[bankName],[IdentityCode],[phone],[BankLogo],[Remark],[Start],[CreateUser],[CreateDate],[ts],[dr],[modifyuser],[vdef1],[vdef2],[vdef3],[vdef4],[vdef5],[verifystatus],[status],[vdef6],[vdef7],[vdef8])");
            strSql.Append(" values (");
            strSql.Append("@DisID,@BankID,@Number,@AccountName,@bankcode,@bankName,@IdentityCode,@phone,@BankLogo,@Remark,@Start,@CreateUser,@CreateDate,@ts,@dr,@modifyuser,@vdef1,@vdef2,@vdef3,@vdef4,@vdef5,@verifystatus,@status,@vdef6,@vdef7,@vdef8)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@BankID", SqlDbType.Int),
                    new SqlParameter("@Number", SqlDbType.NVarChar,50),
                    new SqlParameter("@AccountName", SqlDbType.NVarChar,20),
                    new SqlParameter("@bankcode", SqlDbType.NVarChar,50),
                    new SqlParameter("@bankName", SqlDbType.NVarChar,50),
                    new SqlParameter("@IdentityCode", SqlDbType.NVarChar,250),
                    new SqlParameter("@phone", SqlDbType.NVarChar,50),
                    new SqlParameter("@BankLogo", SqlDbType.NVarChar,100),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,200),
                    new SqlParameter("@Start", SqlDbType.Int),
                    new SqlParameter("@CreateUser", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@vdef1", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef4", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef5", SqlDbType.NVarChar,128),
                    new SqlParameter("@verifystatus", SqlDbType.Int),
                    new SqlParameter("@status", SqlDbType.Int),
                    new SqlParameter("@vdef6", SqlDbType.VarChar,131),
                    new SqlParameter("@vdef7", SqlDbType.VarChar,132),
                    new SqlParameter("@vdef8", SqlDbType.VarChar,133)
            };
            parameters[0].Value = model.DisID;
            parameters[1].Value = model.BankID;
            parameters[2].Value = model.Number;
            parameters[3].Value = model.AccountName;
            parameters[4].Value = model.bankcode;
            parameters[5].Value = model.bankName;
            parameters[6].Value = model.IdentityCode;
            parameters[7].Value = model.phone;
            parameters[8].Value = model.BankLogo;

            if (model.Remark != null)
                parameters[9].Value = model.Remark;
            else
                parameters[9].Value = DBNull.Value;

            parameters[10].Value = model.Start;
            parameters[11].Value = model.CreateUser;

            if (model.CreateDate != DateTime.MinValue)
                parameters[12].Value = model.CreateDate;
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

            parameters[21].Value = model.verifystatus;
            parameters[22].Value = model.status;

            if (model.vdef6 != null)
                parameters[23].Value = model.vdef6;
            else
                parameters[23].Value = DBNull.Value;


            if (model.vdef7 != null)
                parameters[24].Value = model.vdef7;
            else
                parameters[24].Value = DBNull.Value;


            if (model.vdef8 != null)
                parameters[25].Value = model.vdef8;
            else
                parameters[25].Value = DBNull.Value;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.PAY_FastPayMent model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [PAY_FastPayMent] set ");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[BankID]=@BankID,");
            strSql.Append("[Number]=@Number,");
            strSql.Append("[AccountName]=@AccountName,");
            strSql.Append("[bankcode]=@bankcode,");
            strSql.Append("[bankName]=@bankName,");
            strSql.Append("[IdentityCode]=@IdentityCode,");
            strSql.Append("[phone]=@phone,");
            strSql.Append("[BankLogo]=@BankLogo,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[Start]=@Start,");
            strSql.Append("[CreateUser]=@CreateUser,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[vdef4]=@vdef4,");
            strSql.Append("[vdef5]=@vdef5,");
            strSql.Append("[verifystatus]=@verifystatus,");
            strSql.Append("[status]=@status,");
            strSql.Append("[vdef6]=@vdef6,");
            strSql.Append("[vdef7]=@vdef7,");
            strSql.Append("[vdef8]=@vdef8");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@BankID", SqlDbType.Int),
                    new SqlParameter("@Number", SqlDbType.NVarChar,50),
                    new SqlParameter("@AccountName", SqlDbType.NVarChar,20),
                    new SqlParameter("@bankcode", SqlDbType.NVarChar,50),
                    new SqlParameter("@bankName", SqlDbType.NVarChar,50),
                    new SqlParameter("@IdentityCode", SqlDbType.NVarChar,250),
                    new SqlParameter("@phone", SqlDbType.NVarChar,50),
                    new SqlParameter("@BankLogo", SqlDbType.NVarChar,100),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,200),
                    new SqlParameter("@Start", SqlDbType.Int),
                    new SqlParameter("@CreateUser", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@vdef1", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef4", SqlDbType.NVarChar,128),
                    new SqlParameter("@vdef5", SqlDbType.NVarChar,128),
                    new SqlParameter("@verifystatus", SqlDbType.Int),
                    new SqlParameter("@status", SqlDbType.Int),
                    new SqlParameter("@vdef6", SqlDbType.VarChar,131),
                    new SqlParameter("@vdef7", SqlDbType.VarChar,132),
                    new SqlParameter("@vdef8", SqlDbType.VarChar,133)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.BankID;
            parameters[3].Value = model.Number;
            parameters[4].Value = model.AccountName;
            parameters[5].Value = model.bankcode;
            parameters[6].Value = model.bankName;
            parameters[7].Value = model.IdentityCode;
            parameters[8].Value = model.phone;
            parameters[9].Value = model.BankLogo;

            if (model.Remark != null)
                parameters[10].Value = model.Remark;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.Start;
            parameters[12].Value = model.CreateUser;

            if (model.CreateDate != DateTime.MinValue)
                parameters[13].Value = model.CreateDate;
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

            parameters[22].Value = model.verifystatus;
            parameters[23].Value = model.status;

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


            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [PAY_FastPayMent] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[PAY_FastPayMent]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [PAY_FastPayMent]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.PAY_FastPayMent GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [PAY_FastPayMent] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
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
        public IList<Hi.Model.PAY_FastPayMent> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [PAY_FastPayMent]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.PAY_FastPayMent> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.PAY_FastPayMent> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[PAY_FastPayMent]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.PAY_FastPayMent GetModel(DataRow r)
        {
            Hi.Model.PAY_FastPayMent model = new Hi.Model.PAY_FastPayMent();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.BankID = SqlHelper.GetInt(r["BankID"]);
            model.Number = SqlHelper.GetString(r["Number"]);
            model.AccountName = SqlHelper.GetString(r["AccountName"]);
            model.bankcode = SqlHelper.GetString(r["bankcode"]);
            model.bankName = SqlHelper.GetString(r["bankName"]);
            model.IdentityCode = SqlHelper.GetString(r["IdentityCode"]);
            model.phone = SqlHelper.GetString(r["phone"]);
            model.BankLogo = SqlHelper.GetString(r["BankLogo"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.Start = SqlHelper.GetInt(r["Start"]);
            model.CreateUser = SqlHelper.GetInt(r["CreateUser"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.vdef1 = SqlHelper.GetString(r["vdef1"]);
            model.vdef2 = SqlHelper.GetString(r["vdef2"]);
            model.vdef3 = SqlHelper.GetString(r["vdef3"]);
            model.vdef4 = SqlHelper.GetString(r["vdef4"]);
            model.vdef5 = SqlHelper.GetString(r["vdef5"]);
            model.verifystatus = SqlHelper.GetInt(r["verifystatus"]);
            model.status = SqlHelper.GetInt(r["status"]);
            model.vdef6 = SqlHelper.GetString(r["vdef6"]);
            model.vdef7 = SqlHelper.GetString(r["vdef7"]);
            model.vdef8 = SqlHelper.GetString(r["vdef8"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.PAY_FastPayMent> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.PAY_FastPayMent>(ds.Tables[0]);
        }
    }
}

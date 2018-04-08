//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/10/12 11:19:53
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
    /// 数据访问类 PAY_OpenAccount
    /// </summary>
    public class PAY_OpenAccount
    {
        public PAY_OpenAccount()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.PAY_OpenAccount model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [PAY_OpenAccount](");
            strSql.Append("[DisID],[CompID],[State],[AccNumver],[AccName],[AccountName],[AccountNature],[DocumentType],[DocumentCode],[OrgCode],[BusinessLicense],[Sex],[PhoneNumbe],[Fax],[Phone],[Email],[Postcode],[AccAddress],[vdef1],[vdef2],[vdef3],[ts],[dr],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@DisID,@CompID,@State,@AccNumver,@AccName,@AccountName,@AccountNature,@DocumentType,@DocumentCode,@OrgCode,@BusinessLicense,@Sex,@PhoneNumbe,@Fax,@Phone,@Email,@Postcode,@AccAddress,@vdef1,@vdef2,@vdef3,@ts,@dr,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@State", SqlDbType.Int),
                    new SqlParameter("@AccNumver", SqlDbType.VarChar,50),
                    new SqlParameter("@AccName", SqlDbType.VarChar,50),
                    new SqlParameter("@AccountName", SqlDbType.VarChar,50),
                    new SqlParameter("@AccountNature", SqlDbType.Int),
                    new SqlParameter("@DocumentType", SqlDbType.VarChar,50),
                    new SqlParameter("@DocumentCode", SqlDbType.VarChar,50),
                    new SqlParameter("@OrgCode", SqlDbType.VarChar,50),
                    new SqlParameter("@BusinessLicense", SqlDbType.VarChar,50),
                    new SqlParameter("@Sex", SqlDbType.Int),
                    new SqlParameter("@PhoneNumbe", SqlDbType.VarChar,50),
                    new SqlParameter("@Fax", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Email", SqlDbType.VarChar,50),
                    new SqlParameter("@Postcode", SqlDbType.VarChar,50),
                    new SqlParameter("@AccAddress", SqlDbType.VarChar,50),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,1000),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,1000),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,1000),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.DisID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.State;
            parameters[3].Value = model.AccNumver;
            parameters[4].Value = model.AccName;
            parameters[5].Value = model.AccountName;
            parameters[6].Value = model.AccountNature;
            parameters[7].Value = model.DocumentType;
            parameters[8].Value = model.DocumentCode;

            if (model.OrgCode != null)
                parameters[9].Value = model.OrgCode;
            else
                parameters[9].Value = DBNull.Value;


            if (model.BusinessLicense != null)
                parameters[10].Value = model.BusinessLicense;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.Sex;

            if (model.PhoneNumbe != null)
                parameters[12].Value = model.PhoneNumbe;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Fax != null)
                parameters[13].Value = model.Fax;
            else
                parameters[13].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[14].Value = model.Phone;
            else
                parameters[14].Value = DBNull.Value;


            if (model.Email != null)
                parameters[15].Value = model.Email;
            else
                parameters[15].Value = DBNull.Value;


            if (model.Postcode != null)
                parameters[16].Value = model.Postcode;
            else
                parameters[16].Value = DBNull.Value;


            if (model.AccAddress != null)
                parameters[17].Value = model.AccAddress;
            else
                parameters[17].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[18].Value = model.vdef1;
            else
                parameters[18].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[19].Value = model.vdef2;
            else
                parameters[19].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[20].Value = model.vdef3;
            else
                parameters[20].Value = DBNull.Value;

            parameters[21].Value = model.ts;
            parameters[22].Value = model.dr;
            parameters[23].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.PAY_OpenAccount model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [PAY_OpenAccount] set ");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[State]=@State,");
            strSql.Append("[AccNumver]=@AccNumver,");
            strSql.Append("[AccName]=@AccName,");
            strSql.Append("[AccountName]=@AccountName,");
            strSql.Append("[AccountNature]=@AccountNature,");
            strSql.Append("[DocumentType]=@DocumentType,");
            strSql.Append("[DocumentCode]=@DocumentCode,");
            strSql.Append("[OrgCode]=@OrgCode,");
            strSql.Append("[BusinessLicense]=@BusinessLicense,");
            strSql.Append("[Sex]=@Sex,");
            strSql.Append("[Nationality]=@Nationality,");
            strSql.Append("[PhoneNumbe]=@PhoneNumbe,");
            strSql.Append("[Fax]=@Fax,");
            strSql.Append("[Phone]=@Phone,");
            strSql.Append("[Email]=@Email,");
            strSql.Append("[Postcode]=@Postcode,");
            strSql.Append("[AccAddress]=@AccAddress,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@State", SqlDbType.Int),
                    new SqlParameter("@AccNumver", SqlDbType.VarChar,50),
                    new SqlParameter("@AccName", SqlDbType.VarChar,50),
                    new SqlParameter("@AccountName", SqlDbType.VarChar,50),
                    new SqlParameter("@AccountNature", SqlDbType.Int),
                    new SqlParameter("@DocumentType", SqlDbType.VarChar,50),
                    new SqlParameter("@DocumentCode", SqlDbType.VarChar,50),
                    new SqlParameter("@OrgCode", SqlDbType.VarChar,50),
                    new SqlParameter("@BusinessLicense", SqlDbType.VarChar,50),
                    new SqlParameter("@Sex", SqlDbType.Int),
                    new SqlParameter("@Nationality", SqlDbType.VarChar,50),
                    new SqlParameter("@PhoneNumbe", SqlDbType.VarChar,50),
                    new SqlParameter("@Fax", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Email", SqlDbType.VarChar,50),
                    new SqlParameter("@Postcode", SqlDbType.VarChar,50),
                    new SqlParameter("@AccAddress", SqlDbType.VarChar,50),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,1000),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,1000),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,1000),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.CompID;
            parameters[3].Value = model.State;
            parameters[4].Value = model.AccNumver;
            parameters[5].Value = model.AccName;
            parameters[6].Value = model.AccountName;
            parameters[7].Value = model.AccountNature;
            parameters[8].Value = model.DocumentType;
            parameters[9].Value = model.DocumentCode;

            if (model.OrgCode != null)
                parameters[10].Value = model.OrgCode;
            else
                parameters[10].Value = DBNull.Value;


            if (model.BusinessLicense != null)
                parameters[11].Value = model.BusinessLicense;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.Sex;
            parameters[13].Value = model.Nationality;

            if (model.PhoneNumbe != null)
                parameters[14].Value = model.PhoneNumbe;
            else
                parameters[14].Value = DBNull.Value;


            if (model.Fax != null)
                parameters[15].Value = model.Fax;
            else
                parameters[15].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[16].Value = model.Phone;
            else
                parameters[16].Value = DBNull.Value;


            if (model.Email != null)
                parameters[17].Value = model.Email;
            else
                parameters[17].Value = DBNull.Value;


            if (model.Postcode != null)
                parameters[18].Value = model.Postcode;
            else
                parameters[18].Value = DBNull.Value;


            if (model.AccAddress != null)
                parameters[19].Value = model.AccAddress;
            else
                parameters[19].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[20].Value = model.vdef1;
            else
                parameters[20].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[21].Value = model.vdef2;
            else
                parameters[21].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[22].Value = model.vdef3;
            else
                parameters[22].Value = DBNull.Value;

            parameters[23].Value = model.ts;
            parameters[24].Value = model.dr;
            parameters[25].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [PAY_OpenAccount] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[PAY_OpenAccount]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [PAY_OpenAccount]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.PAY_OpenAccount GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [PAY_OpenAccount] ");
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
        public IList<Hi.Model.PAY_OpenAccount> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [PAY_OpenAccount]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.PAY_OpenAccount> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.PAY_OpenAccount> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[PAY_OpenAccount]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.PAY_OpenAccount GetModel(DataRow r)
        {
            Hi.Model.PAY_OpenAccount model = new Hi.Model.PAY_OpenAccount();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.State = SqlHelper.GetInt(r["State"]);
            model.AccNumver = SqlHelper.GetString(r["AccNumver"]);
            model.AccName = SqlHelper.GetString(r["AccName"]);
            model.AccountName = SqlHelper.GetString(r["AccountName"]);
            model.AccountNature = SqlHelper.GetInt(r["AccountNature"]);
            model.DocumentType = SqlHelper.GetString(r["DocumentType"]);
            model.DocumentCode = SqlHelper.GetString(r["DocumentCode"]);
            model.OrgCode = SqlHelper.GetString(r["OrgCode"]);
            model.BusinessLicense = SqlHelper.GetString(r["BusinessLicense"]);
            model.Sex = SqlHelper.GetInt(r["Sex"]);
            model.Nationality = SqlHelper.GetString(r["Nationality"]);
            model.PhoneNumbe = SqlHelper.GetString(r["PhoneNumbe"]);
            model.Fax = SqlHelper.GetString(r["Fax"]);
            model.Phone = SqlHelper.GetString(r["Phone"]);
            model.Email = SqlHelper.GetString(r["Email"]);
            model.Postcode = SqlHelper.GetString(r["Postcode"]);
            model.AccAddress = SqlHelper.GetString(r["AccAddress"]);
            model.vdef1 = SqlHelper.GetString(r["vdef1"]);
            model.vdef2 = SqlHelper.GetString(r["vdef2"]);
            model.vdef3 = SqlHelper.GetString(r["vdef3"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.PAY_OpenAccount> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.PAY_OpenAccount>(ds.Tables[0]);
        }
    }
}

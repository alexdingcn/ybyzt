//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/10/23 13:26:21
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
    /// 数据访问类 BD_Distributor
    /// </summary>
    public partial class BD_Distributor
    {
        public BD_Distributor()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_Distributor model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_Distributor](");
            strSql.Append("[CompID],[DisCode],[DisName],[SMID],[ShortName],[ShortIndex],[DisTypeID],[AreaID],[DisLevel],[Province],[City],[Area],[Address],[Principal],[Phone],[Leading],[LeadingPhone],[Licence],[Tel],[Zip],[Fax],[Remark],[IsCheck],[CreditType],[CreditAmount],[Integral],[zz_flag],[ka_flag],[sdfx_flag],[Paypwd],[AuditState],[AuditUser],[AuditDate],[IsEnabled],[FinancingRatio],[CreateUserID],[CreateDate],[ts],[modifyuser],[pic],[creditCode])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisCode,@DisName,@SMID,@ShortName,@ShortIndex,@DisTypeID,@AreaID,@DisLevel,@Province,@City,@Area,@Address,@Principal,@Phone,@Leading,@LeadingPhone,@Licence,@Tel,@Zip,@Fax,@Remark,@IsCheck,@CreditType,@CreditAmount,@Integral,@zz_flag,@ka_flag,@sdfx_flag,@Paypwd,@AuditState,@AuditUser,@AuditDate,@IsEnabled,@FinancingRatio,@CreateUserID,@CreateDate,@ts,@modifyuser,@pic,@creditCode)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisCode", SqlDbType.VarChar,50),
                    new SqlParameter("@DisName", SqlDbType.VarChar,50),
                    new SqlParameter("@SMID", SqlDbType.Int),
                    new SqlParameter("@ShortName", SqlDbType.VarChar,50),
                    new SqlParameter("@ShortIndex", SqlDbType.VarChar,50),
                    new SqlParameter("@DisTypeID", SqlDbType.Int),
                    new SqlParameter("@AreaID", SqlDbType.Int),
                    new SqlParameter("@DisLevel", SqlDbType.VarChar,50),
                    new SqlParameter("@Province", SqlDbType.VarChar,50),
                    new SqlParameter("@City", SqlDbType.VarChar,50),
                    new SqlParameter("@Area", SqlDbType.VarChar,50),
                    new SqlParameter("@Address", SqlDbType.VarChar,200),
                    new SqlParameter("@Principal", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Leading", SqlDbType.VarChar,50),
                    new SqlParameter("@LeadingPhone", SqlDbType.VarChar,50),
                    new SqlParameter("@Licence", SqlDbType.VarChar,50),
                    new SqlParameter("@Tel", SqlDbType.VarChar,50),
                    new SqlParameter("@Zip", SqlDbType.VarChar,50),
                    new SqlParameter("@Fax", SqlDbType.VarChar,50),
                    new SqlParameter("@Remark", SqlDbType.VarChar,800),
                    new SqlParameter("@IsCheck", SqlDbType.Int),
                    new SqlParameter("@CreditType", SqlDbType.Int),
                    new SqlParameter("@CreditAmount", SqlDbType.Decimal),
                    new SqlParameter("@Integral", SqlDbType.Decimal),
                    new SqlParameter("@zz_flag", SqlDbType.Int),
                    new SqlParameter("@ka_flag", SqlDbType.Int),
                    new SqlParameter("@sdfx_flag", SqlDbType.Int),
                    new SqlParameter("@Paypwd", SqlDbType.VarChar,50),
                    new SqlParameter("@AuditState", SqlDbType.Int),
                    new SqlParameter("@AuditUser", SqlDbType.VarChar,50),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@FinancingRatio", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@pic", SqlDbType.VarChar,1024),
                    new SqlParameter("@creditCode", SqlDbType.VarChar,50)
            };
            parameters[0].Value = model.CompID;

            if (model.DisCode != null)
                parameters[1].Value = model.DisCode;
            else
                parameters[1].Value = DBNull.Value;

            parameters[2].Value = model.DisName;
            parameters[3].Value = model.SMID;

            if (model.ShortName != null)
                parameters[4].Value = model.ShortName;
            else
                parameters[4].Value = DBNull.Value;


            if (model.ShortIndex != null)
                parameters[5].Value = model.ShortIndex;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.DisTypeID;
            parameters[7].Value = model.AreaID;

            if (model.DisLevel != null)
                parameters[8].Value = model.DisLevel;
            else
                parameters[8].Value = DBNull.Value;


            if (model.Province != null)
                parameters[9].Value = model.Province;
            else
                parameters[9].Value = DBNull.Value;


            if (model.City != null)
                parameters[10].Value = model.City;
            else
                parameters[10].Value = DBNull.Value;


            if (model.Area != null)
                parameters[11].Value = model.Area;
            else
                parameters[11].Value = DBNull.Value;


            if (model.Address != null)
                parameters[12].Value = model.Address;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Principal != null)
                parameters[13].Value = model.Principal;
            else
                parameters[13].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[14].Value = model.Phone;
            else
                parameters[14].Value = DBNull.Value;


            if (model.Leading != null)
                parameters[15].Value = model.Leading;
            else
                parameters[15].Value = DBNull.Value;


            if (model.LeadingPhone != null)
                parameters[16].Value = model.LeadingPhone;
            else
                parameters[16].Value = DBNull.Value;


            if (model.Licence != null)
                parameters[17].Value = model.Licence;
            else
                parameters[17].Value = DBNull.Value;


            if (model.Tel != null)
                parameters[18].Value = model.Tel;
            else
                parameters[18].Value = DBNull.Value;


            if (model.Zip != null)
                parameters[19].Value = model.Zip;
            else
                parameters[19].Value = DBNull.Value;


            if (model.Fax != null)
                parameters[20].Value = model.Fax;
            else
                parameters[20].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[21].Value = model.Remark;
            else
                parameters[21].Value = DBNull.Value;

            parameters[22].Value = model.IsCheck;
            parameters[23].Value = model.CreditType;
            parameters[24].Value = model.CreditAmount;
            parameters[25].Value = model.Integral;
            parameters[26].Value = model.zz_flag;
            parameters[27].Value = model.ka_flag;
            parameters[28].Value = model.sdfx_flag;

            if (model.Paypwd != null)
                parameters[29].Value = model.Paypwd;
            else
                parameters[29].Value = DBNull.Value;

            parameters[30].Value = model.AuditState;

            if (model.AuditUser != null)
                parameters[31].Value = model.AuditUser;
            else
                parameters[31].Value = DBNull.Value;


            if (model.AuditDate != DateTime.MinValue)
                parameters[32].Value = model.AuditDate;
            else
                parameters[32].Value = DBNull.Value;

            parameters[33].Value = model.IsEnabled;
            parameters[34].Value = model.FinancingRatio;
            parameters[35].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[36].Value = model.CreateDate;
            else
                parameters[36].Value = DBNull.Value;

            parameters[37].Value = model.ts;
            parameters[38].Value = model.modifyuser;

            if (model.pic != null)
                parameters[39].Value = model.pic;
            else
                parameters[39].Value = DBNull.Value;

            if (model.creditCode != null)
                parameters[40].Value = model.creditCode;
            else
                parameters[40].Value = DBNull.Value;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Distributor model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_Distributor] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisCode]=@DisCode,");
            strSql.Append("[DisName]=@DisName,");
            strSql.Append("[SMID]=@SMID,");
            strSql.Append("[ShortName]=@ShortName,");
            strSql.Append("[ShortIndex]=@ShortIndex,");
            strSql.Append("[DisTypeID]=@DisTypeID,");
            strSql.Append("[AreaID]=@AreaID,");
            strSql.Append("[DisLevel]=@DisLevel,");
            strSql.Append("[Province]=@Province,");
            strSql.Append("[City]=@City,");
            strSql.Append("[Area]=@Area,");
            strSql.Append("[Address]=@Address,");
            strSql.Append("[Principal]=@Principal,");
            strSql.Append("[Phone]=@Phone,");
            strSql.Append("[Leading]=@Leading,");
            strSql.Append("[LeadingPhone]=@LeadingPhone,");
            strSql.Append("[Licence]=@Licence,");
            strSql.Append("[Tel]=@Tel,");
            strSql.Append("[Zip]=@Zip,");
            strSql.Append("[Fax]=@Fax,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[DisAccount]=@DisAccount,");
            strSql.Append("[IsCheck]=@IsCheck,");
            strSql.Append("[CreditType]=@CreditType,");
            strSql.Append("[CreditAmount]=@CreditAmount,");
            strSql.Append("[Integral]=@Integral,");
            strSql.Append("[zz_flag]=@zz_flag,");
            strSql.Append("[ka_flag]=@ka_flag,");
            strSql.Append("[sdfx_flag]=@sdfx_flag,");
            strSql.Append("[Paypwd]=@Paypwd,");
            strSql.Append("[AuditState]=@AuditState,");
            strSql.Append("[AuditUser]=@AuditUser,");
            strSql.Append("[AuditDate]=@AuditDate,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[FinancingRatio]=@FinancingRatio,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[pic]=@pic,");
            strSql.Append("[creditCode]=@creditCode");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisCode", SqlDbType.VarChar,50),
                    new SqlParameter("@DisName", SqlDbType.VarChar,50),
                    new SqlParameter("@SMID", SqlDbType.Int),
                    new SqlParameter("@ShortName", SqlDbType.VarChar,50),
                    new SqlParameter("@ShortIndex", SqlDbType.VarChar,50),
                    new SqlParameter("@DisTypeID", SqlDbType.Int),
                    new SqlParameter("@AreaID", SqlDbType.Int),
                    new SqlParameter("@DisLevel", SqlDbType.VarChar,50),
                    new SqlParameter("@Province", SqlDbType.VarChar,50),
                    new SqlParameter("@City", SqlDbType.VarChar,50),
                    new SqlParameter("@Area", SqlDbType.VarChar,50),
                    new SqlParameter("@Address", SqlDbType.VarChar,200),
                    new SqlParameter("@Principal", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Leading", SqlDbType.VarChar,50),
                    new SqlParameter("@LeadingPhone", SqlDbType.VarChar,50),
                    new SqlParameter("@Licence", SqlDbType.VarChar,50),
                    new SqlParameter("@Tel", SqlDbType.VarChar,50),
                    new SqlParameter("@Zip", SqlDbType.VarChar,50),
                    new SqlParameter("@Fax", SqlDbType.VarChar,50),
                    new SqlParameter("@Remark", SqlDbType.VarChar,800),
                    new SqlParameter("@DisAccount", SqlDbType.Decimal),
                    new SqlParameter("@IsCheck", SqlDbType.Int),
                    new SqlParameter("@CreditType", SqlDbType.Int),
                    new SqlParameter("@CreditAmount", SqlDbType.Decimal),
                    new SqlParameter("@Integral", SqlDbType.Decimal),
                    new SqlParameter("@zz_flag", SqlDbType.Int),
                    new SqlParameter("@ka_flag", SqlDbType.Int),
                    new SqlParameter("@sdfx_flag", SqlDbType.Int),
                    new SqlParameter("@Paypwd", SqlDbType.VarChar,50),
                    new SqlParameter("@AuditState", SqlDbType.Int),
                    new SqlParameter("@AuditUser", SqlDbType.VarChar,50),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@FinancingRatio", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@pic", SqlDbType.VarChar,1024),
                    new SqlParameter("@creditCode", SqlDbType.VarChar,50)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;

            if (model.DisCode != null)
                parameters[2].Value = model.DisCode;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.DisName;
            parameters[4].Value = model.SMID;

            if (model.ShortName != null)
                parameters[5].Value = model.ShortName;
            else
                parameters[5].Value = DBNull.Value;


            if (model.ShortIndex != null)
                parameters[6].Value = model.ShortIndex;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.DisTypeID;
            parameters[8].Value = model.AreaID;

            if (model.DisLevel != null)
                parameters[9].Value = model.DisLevel;
            else
                parameters[9].Value = DBNull.Value;


            if (model.Province != null)
                parameters[10].Value = model.Province;
            else
                parameters[10].Value = DBNull.Value;


            if (model.City != null)
                parameters[11].Value = model.City;
            else
                parameters[11].Value = DBNull.Value;


            if (model.Area != null)
                parameters[12].Value = model.Area;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Address != null)
                parameters[13].Value = model.Address;
            else
                parameters[13].Value = DBNull.Value;


            if (model.Principal != null)
                parameters[14].Value = model.Principal;
            else
                parameters[14].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[15].Value = model.Phone;
            else
                parameters[15].Value = DBNull.Value;


            if (model.Leading != null)
                parameters[16].Value = model.Leading;
            else
                parameters[16].Value = DBNull.Value;


            if (model.LeadingPhone != null)
                parameters[17].Value = model.LeadingPhone;
            else
                parameters[17].Value = DBNull.Value;


            if (model.Licence != null)
                parameters[18].Value = model.Licence;
            else
                parameters[18].Value = DBNull.Value;


            if (model.Tel != null)
                parameters[19].Value = model.Tel;
            else
                parameters[19].Value = DBNull.Value;


            if (model.Zip != null)
                parameters[20].Value = model.Zip;
            else
                parameters[20].Value = DBNull.Value;


            if (model.Fax != null)
                parameters[21].Value = model.Fax;
            else
                parameters[21].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[22].Value = model.Remark;
            else
                parameters[22].Value = DBNull.Value;

            parameters[23].Value = model.DisAccount;
            parameters[24].Value = model.IsCheck;
            parameters[25].Value = model.CreditType;
            parameters[26].Value = model.CreditAmount;
            parameters[27].Value = model.Integral;
            parameters[28].Value = model.zz_flag;
            parameters[29].Value = model.ka_flag;
            parameters[30].Value = model.sdfx_flag;

            if (model.Paypwd != null)
                parameters[31].Value = model.Paypwd;
            else
                parameters[31].Value = DBNull.Value;

            parameters[32].Value = model.AuditState;

            if (model.AuditUser != null)
                parameters[33].Value = model.AuditUser;
            else
                parameters[33].Value = DBNull.Value;


            if (model.AuditDate != DateTime.MinValue)
                parameters[34].Value = model.AuditDate;
            else
                parameters[34].Value = DBNull.Value;

            parameters[35].Value = model.IsEnabled;
            parameters[36].Value = model.FinancingRatio;
            parameters[37].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[38].Value = model.CreateDate;
            else
                parameters[38].Value = DBNull.Value;

            parameters[39].Value = model.ts;
            parameters[40].Value = model.dr;
            parameters[41].Value = model.modifyuser;

            if (model.pic != null)
                parameters[42].Value = model.pic;
            else
                parameters[42].Value = DBNull.Value;

            if (model.creditCode != null)
                parameters[43].Value = model.creditCode;
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
            strSql.Append("delete [BD_Distributor] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[BD_Distributor]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BD_Distributor]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_Distributor GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_Distributor] ");
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
        public IList<Hi.Model.BD_Distributor> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_Distributor]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_Distributor> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BD_Distributor> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BD_Distributor]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BD_Distributor GetModel(DataRow r)
        {
            Hi.Model.BD_Distributor model = new Hi.Model.BD_Distributor();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.DisCode = SqlHelper.GetString(r["DisCode"]);
            model.DisName = SqlHelper.GetString(r["DisName"]);
            model.SMID = SqlHelper.GetInt(r["SMID"]);
            model.ShortName = SqlHelper.GetString(r["ShortName"]);
            model.ShortIndex = SqlHelper.GetString(r["ShortIndex"]);
            model.DisTypeID = SqlHelper.GetInt(r["DisTypeID"]);
            model.AreaID = SqlHelper.GetInt(r["AreaID"]);
            model.DisLevel = SqlHelper.GetString(r["DisLevel"]);
            model.Province = SqlHelper.GetString(r["Province"]);
            model.City = SqlHelper.GetString(r["City"]);
            model.Area = SqlHelper.GetString(r["Area"]);
            model.Address = SqlHelper.GetString(r["Address"]);
            model.Principal = SqlHelper.GetString(r["Principal"]);
            model.Phone = SqlHelper.GetString(r["Phone"]);
            model.Leading = SqlHelper.GetString(r["Leading"]);
            model.LeadingPhone = SqlHelper.GetString(r["LeadingPhone"]);
            model.Licence = SqlHelper.GetString(r["Licence"]);
            model.Tel = SqlHelper.GetString(r["Tel"]);
            model.Zip = SqlHelper.GetString(r["Zip"]);
            model.Fax = SqlHelper.GetString(r["Fax"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.DisAccount = SqlHelper.GetDecimal(r["DisAccount"]);
            model.IsCheck = SqlHelper.GetInt(r["IsCheck"]);
            model.CreditType = SqlHelper.GetInt(r["CreditType"]);
            model.CreditAmount = SqlHelper.GetDecimal(r["CreditAmount"]);
            model.Integral = SqlHelper.GetDecimal(r["Integral"]);
            model.zz_flag = SqlHelper.GetInt(r["zz_flag"]);
            model.ka_flag = SqlHelper.GetInt(r["ka_flag"]);
            model.sdfx_flag = SqlHelper.GetInt(r["sdfx_flag"]);
            model.Paypwd = SqlHelper.GetString(r["Paypwd"]);
            model.AuditState = SqlHelper.GetInt(r["AuditState"]);
            model.AuditUser = SqlHelper.GetString(r["AuditUser"]);
            model.AuditDate = SqlHelper.GetDateTime(r["AuditDate"]);
            model.IsEnabled = SqlHelper.GetInt(r["IsEnabled"]);
            model.FinancingRatio = SqlHelper.GetInt(r["FinancingRatio"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.pic = SqlHelper.GetString(r["pic"]);
            model.creditCode = SqlHelper.GetString(r["creditCode"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.BD_Distributor> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BD_Distributor>(ds.Tables[0]);
        }
    }
}

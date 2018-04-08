//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/12/28 12:43:36
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
    /// 数据访问类 BD_Company
    /// </summary>
    public partial class BD_Company
    {
        public BD_Company()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_Company model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_Company](");
            strSql.Append("[OrgID],[SalesManID],[IndID],[CompCode],[CompName],[SortIndex],[ShortName],[Tel],[Legal],[Identitys],[LegalTel],[Zip],[ManageInfo],[Fax],[Codes],[Trade],[Principal],[Phone],[Licence],[Account],[OrganizationCode],[CompAddr],[Address],[CompLogo],[ShopLogo],[BrandInfo],[CompInfo],[Remark],[IsEnabled],[FirstBanerImg],[HotShow],[FirstShow],[CustomCompinfo],[CustomAddress],[FinanceCode],[FinanceName],[CreateUserID],[CreateDate],[AuditUser],[AuditDate],[ts],[modifyuser],[Attachment],[Erptype],[CompNewLogo],[QQ],[creditCode])");
            strSql.Append(" values (");
            strSql.Append("@OrgID,@SalesManID,@IndID,@CompCode,@CompName,@SortIndex,@ShortName,@Tel,@Legal,@Identitys,@LegalTel,@Zip,@ManageInfo,@Fax,@Codes,@Trade,@Principal,@Phone,@Licence,@Account,@OrganizationCode,@CompAddr,@Address,@CompLogo,@ShopLogo,@BrandInfo,@CompInfo,@Remark,@IsEnabled,@FirstBanerImg,@HotShow,@FirstShow,@CustomCompinfo,@CustomAddress,@FinanceCode,@FinanceName,@CreateUserID,@CreateDate,@AuditUser,@AuditDate,@ts,@modifyuser,@Attachment,@Erptype,@CompNewLogo,@QQ,@creditCode)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrgID", SqlDbType.Int),
                    new SqlParameter("@SalesManID", SqlDbType.Int),
                    new SqlParameter("@IndID", SqlDbType.Int),
                    new SqlParameter("@CompCode", SqlDbType.VarChar,50),
                    new SqlParameter("@CompName", SqlDbType.VarChar,50),
                    new SqlParameter("@SortIndex", SqlDbType.VarChar,50),
                    new SqlParameter("@ShortName", SqlDbType.VarChar,50),
                    new SqlParameter("@Tel", SqlDbType.VarChar,50),
                    new SqlParameter("@Legal", SqlDbType.VarChar,50),
                    new SqlParameter("@Identitys", SqlDbType.VarChar,50),
                    new SqlParameter("@LegalTel", SqlDbType.VarChar,50),
                    new SqlParameter("@Zip", SqlDbType.VarChar,50),
                    new SqlParameter("@ManageInfo", SqlDbType.VarChar,500),
                    new SqlParameter("@Fax", SqlDbType.VarChar,50),
                    new SqlParameter("@Codes", SqlDbType.VarChar,50),
                    new SqlParameter("@Trade", SqlDbType.VarChar,50),
                    new SqlParameter("@Principal", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Licence", SqlDbType.VarChar,50),
                    new SqlParameter("@Account", SqlDbType.VarChar,50),
                    new SqlParameter("@OrganizationCode", SqlDbType.VarChar,50),
                    new SqlParameter("@CompAddr", SqlDbType.VarChar,200),
                    new SqlParameter("@Address", SqlDbType.VarChar,200),
                    new SqlParameter("@CompLogo", SqlDbType.VarChar,200),
                    new SqlParameter("@ShopLogo", SqlDbType.VarChar,200),
                    new SqlParameter("@BrandInfo", SqlDbType.VarChar,2000),
                    new SqlParameter("@CompInfo", SqlDbType.VarChar,4000),
                    new SqlParameter("@Remark", SqlDbType.VarChar,2000),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@FirstBanerImg", SqlDbType.VarChar,200),
                    new SqlParameter("@HotShow", SqlDbType.Int),
                    new SqlParameter("@FirstShow", SqlDbType.Int),
                    new SqlParameter("@CustomCompinfo", SqlDbType.Text),
                    new SqlParameter("@CustomAddress", SqlDbType.VarChar,8000),
                    new SqlParameter("@FinanceCode", SqlDbType.VarChar,200),
                    new SqlParameter("@FinanceName", SqlDbType.VarChar,200),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@AuditUser", SqlDbType.VarChar,50),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@Attachment", SqlDbType.VarChar,4000),
                    new SqlParameter("@Erptype", SqlDbType.Int),
                    new SqlParameter("@CompNewLogo", SqlDbType.VarChar,128),
                    new SqlParameter("@QQ", SqlDbType.VarChar,20),
                    new SqlParameter("@creditCode", SqlDbType.VarChar,50),
            };
            parameters[0].Value = model.OrgID;
            parameters[1].Value = model.SalesManID;
            parameters[2].Value = model.IndID;

            if (model.CompCode != null)
                parameters[3].Value = model.CompCode;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.CompName;

            if (model.SortIndex != null)
                parameters[5].Value = model.SortIndex;
            else
                parameters[5].Value = DBNull.Value;


            if (model.ShortName != null)
                parameters[6].Value = model.ShortName;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Tel != null)
                parameters[7].Value = model.Tel;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Legal != null)
                parameters[8].Value = model.Legal;
            else
                parameters[8].Value = DBNull.Value;


            if (model.Identitys != null)
                parameters[9].Value = model.Identitys;
            else
                parameters[9].Value = DBNull.Value;


            if (model.LegalTel != null)
                parameters[10].Value = model.LegalTel;
            else
                parameters[10].Value = DBNull.Value;


            if (model.Zip != null)
                parameters[11].Value = model.Zip;
            else
                parameters[11].Value = DBNull.Value;


            if (model.ManageInfo != null)
                parameters[12].Value = model.ManageInfo;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Fax != null)
                parameters[13].Value = model.Fax;
            else
                parameters[13].Value = DBNull.Value;


            if (model.Codes != null)
                parameters[14].Value = model.Codes;
            else
                parameters[14].Value = DBNull.Value;


            if (model.Trade != null)
                parameters[15].Value = model.Trade;
            else
                parameters[15].Value = DBNull.Value;


            if (model.Principal != null)
                parameters[16].Value = model.Principal;
            else
                parameters[16].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[17].Value = model.Phone;
            else
                parameters[17].Value = DBNull.Value;


            if (model.Licence != null)
                parameters[18].Value = model.Licence;
            else
                parameters[18].Value = DBNull.Value;


            if (model.Account != null)
                parameters[19].Value = model.Account;
            else
                parameters[19].Value = DBNull.Value;


            if (model.OrganizationCode != null)
                parameters[20].Value = model.OrganizationCode;
            else
                parameters[20].Value = DBNull.Value;


            if (model.CompAddr != null)
                parameters[21].Value = model.CompAddr;
            else
                parameters[21].Value = DBNull.Value;


            if (model.Address != null)
                parameters[22].Value = model.Address;
            else
                parameters[22].Value = DBNull.Value;


            if (model.CompLogo != null)
                parameters[23].Value = model.CompLogo;
            else
                parameters[23].Value = DBNull.Value;


            if (model.ShopLogo != null)
                parameters[24].Value = model.ShopLogo;
            else
                parameters[24].Value = DBNull.Value;


            if (model.BrandInfo != null)
                parameters[25].Value = model.BrandInfo;
            else
                parameters[25].Value = DBNull.Value;


            if (model.CompInfo != null)
                parameters[26].Value = model.CompInfo;
            else
                parameters[26].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[27].Value = model.Remark;
            else
                parameters[27].Value = DBNull.Value;

            parameters[28].Value = model.IsEnabled;

            if (model.FirstBanerImg != null)
                parameters[29].Value = model.FirstBanerImg;
            else
                parameters[29].Value = DBNull.Value;

            parameters[30].Value = model.HotShow;
            parameters[31].Value = model.FirstShow;

            if (model.CustomCompinfo != null)
                parameters[32].Value = model.CustomCompinfo;
            else
                parameters[32].Value = DBNull.Value;


            if (model.CustomAddress != null)
                parameters[33].Value = model.CustomAddress;
            else
                parameters[33].Value = DBNull.Value;


            if (model.FinanceCode != null)
                parameters[34].Value = model.FinanceCode;
            else
                parameters[34].Value = DBNull.Value;


            if (model.FinanceName != null)
                parameters[35].Value = model.FinanceName;
            else
                parameters[35].Value = DBNull.Value;

            parameters[36].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[37].Value = model.CreateDate;
            else
                parameters[37].Value = DBNull.Value;


            if (model.AuditUser != null)
                parameters[38].Value = model.AuditUser;
            else
                parameters[38].Value = DBNull.Value;


            if (model.AuditDate != DateTime.MinValue)
                parameters[39].Value = model.AuditDate;
            else
                parameters[39].Value = DBNull.Value;

            parameters[40].Value = model.ts;
            parameters[41].Value = model.modifyuser;

            if (model.Attachment != null)
                parameters[42].Value = model.Attachment;
            else
                parameters[42].Value = DBNull.Value;

            parameters[43].Value = model.Erptype;

            if (model.CompNewLogo != null)
                parameters[44].Value = model.CompNewLogo;
            else
                parameters[44].Value = DBNull.Value;


            if (model.QQ != null)
                parameters[45].Value = model.QQ;
            else
                parameters[45].Value = DBNull.Value;

            if (model.creditCode != null)
                parameters[46].Value = model.creditCode;
            else
                parameters[46].Value = DBNull.Value;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Company model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_Company] set ");
            strSql.Append("[OrgID]=@OrgID,");
            strSql.Append("[SalesManID]=@SalesManID,");
            strSql.Append("[IndID]=@IndID,");
            strSql.Append("[CompCode]=@CompCode,");
            strSql.Append("[CompName]=@CompName,");
            strSql.Append("[SortIndex]=@SortIndex,");
            strSql.Append("[ShortName]=@ShortName,");
            strSql.Append("[Tel]=@Tel,");
            strSql.Append("[Legal]=@Legal,");
            strSql.Append("[Identitys]=@Identitys,");
            strSql.Append("[LegalTel]=@LegalTel,");
            strSql.Append("[Zip]=@Zip,");
            strSql.Append("[ManageInfo]=@ManageInfo,");
            strSql.Append("[Fax]=@Fax,");
            strSql.Append("[Codes]=@Codes,");
            strSql.Append("[Trade]=@Trade,");
            strSql.Append("[Principal]=@Principal,");
            strSql.Append("[Phone]=@Phone,");
            strSql.Append("[Licence]=@Licence,");
            strSql.Append("[Account]=@Account,");
            strSql.Append("[OrganizationCode]=@OrganizationCode,");
            strSql.Append("[CompAddr]=@CompAddr,");
            strSql.Append("[Address]=@Address,");
            strSql.Append("[CompLogo]=@CompLogo,");
            strSql.Append("[ShopLogo]=@ShopLogo,");
            strSql.Append("[BrandInfo]=@BrandInfo,");
            strSql.Append("[CompInfo]=@CompInfo,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[FirstBanerImg]=@FirstBanerImg,");
            strSql.Append("[HotShow]=@HotShow,");
            strSql.Append("[FirstShow]=@FirstShow,");
            strSql.Append("[CustomCompinfo]=@CustomCompinfo,");
            strSql.Append("[CustomAddress]=@CustomAddress,");
            strSql.Append("[FinanceCode]=@FinanceCode,");
            strSql.Append("[FinanceName]=@FinanceName,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[AuditState]=@AuditState,");
            strSql.Append("[AuditUser]=@AuditUser,");
            strSql.Append("[AuditDate]=@AuditDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[Attachment]=@Attachment,");
            strSql.Append("[Erptype]=@Erptype,");
            strSql.Append("[CompNewLogo]=@CompNewLogo,");
            strSql.Append("[QQ]=@QQ,");
            strSql.Append("[EnabledStartDate]=@EnabledStartDate,");
            strSql.Append("[EnabledEndDate]=@EnabledEndDate,");
            strSql.Append("[Capital]=@Capital,");
            strSql.Append("[CompType]=@CompType,");
            strSql.Append("[IsZXAudit]=@IsZXAudit,");
            strSql.Append("[ZXAuditDate]=@ZXAuditDate,");
            strSql.Append("[ZXAuditUser]=@ZXAuditUser,");
            strSql.Append("[IsOrgZX]=@IsOrgZX,");
            strSql.Append("[OrgZXDate]=@OrgZXDate,"); 
            strSql.Append("[creditCode]=@creditCode");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@OrgID", SqlDbType.Int),
                    new SqlParameter("@SalesManID", SqlDbType.Int),
                    new SqlParameter("@IndID", SqlDbType.Int),
                    new SqlParameter("@CompCode", SqlDbType.VarChar,50),
                    new SqlParameter("@CompName", SqlDbType.VarChar,50),
                    new SqlParameter("@SortIndex", SqlDbType.VarChar,50),
                    new SqlParameter("@ShortName", SqlDbType.VarChar,50),
                    new SqlParameter("@Tel", SqlDbType.VarChar,50),
                    new SqlParameter("@Legal", SqlDbType.VarChar,50),
                    new SqlParameter("@Identitys", SqlDbType.VarChar,50),
                    new SqlParameter("@LegalTel", SqlDbType.VarChar,50),
                    new SqlParameter("@Zip", SqlDbType.VarChar,50),
                    new SqlParameter("@ManageInfo", SqlDbType.VarChar,500),
                    new SqlParameter("@Fax", SqlDbType.VarChar,50),
                    new SqlParameter("@Codes", SqlDbType.VarChar,50),
                    new SqlParameter("@Trade", SqlDbType.VarChar,50),
                    new SqlParameter("@Principal", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Licence", SqlDbType.VarChar,50),
                    new SqlParameter("@Account", SqlDbType.VarChar,50),
                    new SqlParameter("@OrganizationCode", SqlDbType.VarChar,50),
                    new SqlParameter("@CompAddr", SqlDbType.VarChar,200),
                    new SqlParameter("@Address", SqlDbType.VarChar,200),
                    new SqlParameter("@CompLogo", SqlDbType.VarChar,200),
                    new SqlParameter("@ShopLogo", SqlDbType.VarChar,200),
                    new SqlParameter("@BrandInfo", SqlDbType.VarChar,2000),
                    new SqlParameter("@CompInfo", SqlDbType.VarChar,4000),
                    new SqlParameter("@Remark", SqlDbType.VarChar,2000),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@FirstBanerImg", SqlDbType.VarChar,200),
                    new SqlParameter("@HotShow", SqlDbType.Int),
                    new SqlParameter("@FirstShow", SqlDbType.Int),
                    new SqlParameter("@CustomCompinfo", SqlDbType.Text),
                    new SqlParameter("@CustomAddress", SqlDbType.VarChar,8000),
                    new SqlParameter("@FinanceCode", SqlDbType.VarChar,200),
                    new SqlParameter("@FinanceName", SqlDbType.VarChar,200),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@AuditState", SqlDbType.Int),
                    new SqlParameter("@AuditUser", SqlDbType.VarChar,50),
                    new SqlParameter("@AuditDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@Attachment", SqlDbType.VarChar,4000),
                    new SqlParameter("@Erptype", SqlDbType.Int),
                    new SqlParameter("@CompNewLogo", SqlDbType.VarChar,20),
                    new SqlParameter("@QQ", SqlDbType.VarChar,128),
                    new SqlParameter("@EnabledStartDate", SqlDbType.DateTime),
                    new SqlParameter("@EnabledEndDate", SqlDbType.DateTime),
                    new SqlParameter("@Capital", SqlDbType.VarChar,50),
                    new SqlParameter("@CompType", SqlDbType.Int),
                    new SqlParameter("@IsZXAudit", SqlDbType.Int),
                    new SqlParameter("@ZXAuditDate", SqlDbType.DateTime),
                    new SqlParameter("@ZXAuditUser", SqlDbType.Int),
                    new SqlParameter("@IsOrgZX", SqlDbType.Int),
                    new SqlParameter("@OrgZXDate", SqlDbType.DateTime),
                    new SqlParameter("@creditCode", SqlDbType.VarChar,50)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.OrgID;
            parameters[2].Value = model.SalesManID;
            parameters[3].Value = model.IndID;

            if (model.CompCode != null)
                parameters[4].Value = model.CompCode;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.CompName;

            if (model.SortIndex != null)
                parameters[6].Value = model.SortIndex;
            else
                parameters[6].Value = DBNull.Value;


            if (model.ShortName != null)
                parameters[7].Value = model.ShortName;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Tel != null)
                parameters[8].Value = model.Tel;
            else
                parameters[8].Value = DBNull.Value;


            if (model.Legal != null)
                parameters[9].Value = model.Legal;
            else
                parameters[9].Value = DBNull.Value;


            if (model.Identitys != null)
                parameters[10].Value = model.Identitys;
            else
                parameters[10].Value = DBNull.Value;


            if (model.LegalTel != null)
                parameters[11].Value = model.LegalTel;
            else
                parameters[11].Value = DBNull.Value;


            if (model.Zip != null)
                parameters[12].Value = model.Zip;
            else
                parameters[12].Value = DBNull.Value;


            if (model.ManageInfo != null)
                parameters[13].Value = model.ManageInfo;
            else
                parameters[13].Value = DBNull.Value;


            if (model.Fax != null)
                parameters[14].Value = model.Fax;
            else
                parameters[14].Value = DBNull.Value;


            if (model.Codes != null)
                parameters[15].Value = model.Codes;
            else
                parameters[15].Value = DBNull.Value;


            if (model.Trade != null)
                parameters[16].Value = model.Trade;
            else
                parameters[16].Value = DBNull.Value;


            if (model.Principal != null)
                parameters[17].Value = model.Principal;
            else
                parameters[17].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[18].Value = model.Phone;
            else
                parameters[18].Value = DBNull.Value;


            if (model.Licence != null)
                parameters[19].Value = model.Licence;
            else
                parameters[19].Value = DBNull.Value;


            if (model.Account != null)
                parameters[20].Value = model.Account;
            else
                parameters[20].Value = DBNull.Value;


            if (model.OrganizationCode != null)
                parameters[21].Value = model.OrganizationCode;
            else
                parameters[21].Value = DBNull.Value;


            if (model.CompAddr != null)
                parameters[22].Value = model.CompAddr;
            else
                parameters[22].Value = DBNull.Value;


            if (model.Address != null)
                parameters[23].Value = model.Address;
            else
                parameters[23].Value = DBNull.Value;


            if (model.CompLogo != null)
                parameters[24].Value = model.CompLogo;
            else
                parameters[24].Value = DBNull.Value;


            if (model.ShopLogo != null)
                parameters[25].Value = model.ShopLogo;
            else
                parameters[25].Value = DBNull.Value;


            if (model.BrandInfo != null)
                parameters[26].Value = model.BrandInfo;
            else
                parameters[26].Value = DBNull.Value;


            if (model.CompInfo != null)
                parameters[27].Value = model.CompInfo;
            else
                parameters[27].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[28].Value = model.Remark;
            else
                parameters[28].Value = DBNull.Value;

            parameters[29].Value = model.IsEnabled;

            if (model.FirstBanerImg != null)
                parameters[30].Value = model.FirstBanerImg;
            else
                parameters[30].Value = DBNull.Value;

            parameters[31].Value = model.HotShow;
            parameters[32].Value = model.FirstShow;

            if (model.CustomCompinfo != null)
                parameters[33].Value = model.CustomCompinfo;
            else
                parameters[33].Value = DBNull.Value;


            if (model.CustomAddress != null)
                parameters[34].Value = model.CustomAddress;
            else
                parameters[34].Value = DBNull.Value;


            if (model.FinanceCode != null)
                parameters[35].Value = model.FinanceCode;
            else
                parameters[35].Value = DBNull.Value;


            if (model.FinanceName != null)
                parameters[36].Value = model.FinanceName;
            else
                parameters[36].Value = DBNull.Value;

            parameters[37].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[38].Value = model.CreateDate;
            else
                parameters[38].Value = DBNull.Value;

            parameters[39].Value = model.AuditState;

            if (model.AuditUser != null)
                parameters[40].Value = model.AuditUser;
            else
                parameters[40].Value = DBNull.Value;


            if (model.AuditDate != DateTime.MinValue)
                parameters[41].Value = model.AuditDate;
            else
                parameters[41].Value = DBNull.Value;

            parameters[42].Value = model.ts;
            parameters[43].Value = model.dr;
            parameters[44].Value = model.modifyuser;

            if (model.Attachment != null)
                parameters[45].Value = model.Attachment;
            else
                parameters[45].Value = DBNull.Value;

            parameters[46].Value = model.Erptype;

            if (model.CompNewLogo != null)
                parameters[47].Value = model.CompNewLogo;
            else
                parameters[47].Value = DBNull.Value;


            if (model.QQ != null)
                parameters[48].Value = model.QQ;
            else
                parameters[48].Value = DBNull.Value;

            if (model.EnabledStartDate != DateTime.MinValue)
                parameters[49].Value = model.EnabledStartDate;
            else
                parameters[49].Value = DBNull.Value;

            if (model.EnabledEndDate != DateTime.MinValue)
                parameters[50].Value = model.EnabledEndDate;
            else
                parameters[50].Value = DBNull.Value;

            if (model.Capital != null)
                parameters[51].Value = model.Capital;
            else
                parameters[51].Value = DBNull.Value;

            parameters[52].Value = model.CompType;

            parameters[53].Value = model.IsZXAudit;

            if (model.ZXAuditDate != DateTime.MinValue)
                parameters[54].Value = model.ZXAuditDate;
            else
                parameters[54].Value = DBNull.Value;

            parameters[55].Value = model.Zxaudituser;
            parameters[56].Value = model.Isorgzx;

            if (model.Orgzxdate != DateTime.MinValue)
                parameters[57].Value = model.Orgzxdate;
            else
                parameters[57].Value = DBNull.Value;

            if (model.creditCode != null)
                parameters[58].Value = model.creditCode;
            else
                parameters[58].Value = DBNull.Value;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_Company] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[BD_Company]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BD_Company]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_Company GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_Company] ");
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
        public IList<Hi.Model.BD_Company> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_Company]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_Company> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BD_Company> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BD_Company]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BD_Company GetModel(DataRow r)
        {
            Hi.Model.BD_Company model = new Hi.Model.BD_Company();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.OrgID = SqlHelper.GetInt(r["OrgID"]);
            model.SalesManID = SqlHelper.GetInt(r["SalesManID"]);
            model.IndID = SqlHelper.GetInt(r["IndID"]);
            model.CompCode = SqlHelper.GetString(r["CompCode"]);
            model.CompName = SqlHelper.GetString(r["CompName"]);
            model.SortIndex = SqlHelper.GetString(r["SortIndex"]);
            model.ShortName = SqlHelper.GetString(r["ShortName"]);
            model.Tel = SqlHelper.GetString(r["Tel"]);
            model.Legal = SqlHelper.GetString(r["Legal"]);
            model.Identitys = SqlHelper.GetString(r["Identitys"]);
            model.LegalTel = SqlHelper.GetString(r["LegalTel"]);
            model.Zip = SqlHelper.GetString(r["Zip"]);
            model.ManageInfo = SqlHelper.GetString(r["ManageInfo"]);
            model.Fax = SqlHelper.GetString(r["Fax"]);
            model.Codes = SqlHelper.GetString(r["Codes"]);
            model.Trade = SqlHelper.GetString(r["Trade"]);
            model.Principal = SqlHelper.GetString(r["Principal"]);
            model.Phone = SqlHelper.GetString(r["Phone"]);
            model.Licence = SqlHelper.GetString(r["Licence"]);
            model.Account = SqlHelper.GetString(r["Account"]);
            model.OrganizationCode = SqlHelper.GetString(r["OrganizationCode"]);
            model.CompAddr = SqlHelper.GetString(r["CompAddr"]);
            model.Address = SqlHelper.GetString(r["Address"]);
            model.CompLogo = SqlHelper.GetString(r["CompLogo"]);
            model.ShopLogo = SqlHelper.GetString(r["ShopLogo"]);
            model.BrandInfo = SqlHelper.GetString(r["BrandInfo"]);
            model.CompInfo = SqlHelper.GetString(r["CompInfo"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.IsEnabled = SqlHelper.GetInt(r["IsEnabled"]);
            model.FirstBanerImg = SqlHelper.GetString(r["FirstBanerImg"]);
            model.HotShow = SqlHelper.GetInt(r["HotShow"]);
            model.FirstShow = SqlHelper.GetInt(r["FirstShow"]);
            model.CustomCompinfo = SqlHelper.GetString(r["CustomCompinfo"]);
            model.CustomAddress = SqlHelper.GetString(r["CustomAddress"]);
            model.FinanceCode = SqlHelper.GetString(r["FinanceCode"]);
            model.FinanceName = SqlHelper.GetString(r["FinanceName"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.AuditState = SqlHelper.GetInt(r["AuditState"]);
            model.AuditUser = SqlHelper.GetString(r["AuditUser"]);
            model.AuditDate = SqlHelper.GetDateTime(r["AuditDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.Attachment = SqlHelper.GetString(r["Attachment"]);
            model.Erptype = SqlHelper.GetInt(r["Erptype"]);
            model.CompNewLogo = SqlHelper.GetString(r["CompNewLogo"]);
            model.QQ = SqlHelper.GetString(r["QQ"]);
            model.EnabledStartDate = SqlHelper.GetDateTime(r["EnabledStartDate"]);
            model.EnabledEndDate = SqlHelper.GetDateTime(r["EnabledEndDate"]);
            model.Capital = SqlHelper.GetString(r["Capital"]);
            model.CompType = SqlHelper.GetInt(r["CompType"]);
            model.IsZXAudit = SqlHelper.GetInt(r["IsZXAudit"]);
            model.ZXAuditDate = SqlHelper.GetDateTime(r["ZXAuditDate"]);
            model.Zxaudituser = SqlHelper.GetInt(r["ZXAuditUser"]);
            model.Isorgzx = SqlHelper.GetInt(r["IsOrgZX"]);
            model.Orgzxdate = SqlHelper.GetDateTime(r["OrgZXDate"]);
            model.creditCode = SqlHelper.GetString(r["creditCode"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.BD_Company> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BD_Company>(ds.Tables[0]);
        }
    }
}

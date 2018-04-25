using System;
using System.Collections.Generic;
using System.Text;
using DBUtility;
using System.Data;
using System.Data.SqlClient;

namespace Hi.SQLServerDAL
{
    public partial class BD_Distributor
    {
        /// <summary>
        /// 通过账号获取经销商ID
        /// 调用通过经销商ID获取经销商实体
        /// </summary>
        public Hi.Model.BD_Distributor GetDisID(string username)
        {
            string sqlstr = "select DisID from SYS_Users where dr=0 and UserName='" + username + "'";
            int disid = SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr).Tables[0].Rows.Count == 0 ? 0 : (int)SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr).Tables[0].Rows[0]["DisID"];
            if (disid != 0)
            {
                return GetModel(disid);
            }
            return null;
        }

        /// <summary>
        /// 获取数据集,在单表查询时使用
        /// </summary>
        public DataSet GetDataSet(string strWhat, string strWhere, string strOrderby,SqlTransaction Tran)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_Distributor]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), Tran);
        }

        public IList<Hi.Model.BD_Distributor> GetList(string strWhat, string strWhere, string strOrderby,SqlTransaction Tran)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby,Tran));
        }

        /// <summary>
        /// 增加一条数据带有事物
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(Hi.Model.BD_Distributor model, SqlTransaction Tran)
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

            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_Distributor model, SqlTransaction Tran)
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

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 通过账号获取经销商地区,分类,资料
        /// </summary>
        public DataSet GetDis(string username)
        {
            string sqlstr = string.Format(@"select * from BD_Distributor dis 
join SYS_Users [users] on dis.ID=users.DisID 
join BD_DisType Dtype on dis.DisTypeID=Dtype.ID
join BD_DisArea area on dis.AreaID=area.ID
where dis.dr=0 and users.dr=0 and dtype.dr=0 and area.dr=0
and users.UserName='{0}'", username);
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr);
            return ds;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_Distributor GetModel(int ID, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_Distributor] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            DataSet ds = SqlHelper.Query(strSql.ToString(), Tran, parameters);
           // DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
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
        /// 获取用户以及用户明细表数据
        /// </summary>
        public DataTable GetList(int pageSize, int pageIndex, string fldSort, bool sort, string fldName, string TbName, string strCondition, out int pageCount, out int count)
        {
            if (string.IsNullOrEmpty(TbName))
            {
                TbName = "[BD_Distributor]";
            }
            if (string.IsNullOrEmpty(fldName))
            {
                fldName = " * ";
            }
            string strSql;
            DataSet ds = SqlHelper.PageList2(SqlHelper.LocalSqlServer, TbName, fldName, pageSize, pageIndex, fldSort, sort, strCondition, "A.ID", true, out pageCount, out count, out strSql);
            return ds.Tables[0];
        }

        /// <summary>
        /// 获取商品库存分页列表数据
        /// </summary>
        /// <param name="pageSize">一页几行</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="pageCount">一共几页</param>
        /// <param name="Counts">总行数</param>
        /// <param name="CountNum">是否已知总行数</param>
        /// <returns></returns>
        public DataTable getDataTable(int pageSize, int pageIndex, string strWhere, out int pageCount, out int Counts, int CountNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select cu.disID ID,cu.compID,dis.DisName disname,cu.IsAudit AuditState,dis.CreateDate,cu.IsEnabled,dis.Principal,dis.phone,cu.AreaID,cu.DisTypeID ");
            strSql.Append(" from SYS_CompUser cu left join BD_Distributor dis on cu.DisID=dis.ID");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.AppendFormat(" where cu.CType=2 and cu.UType=5 {0}", strWhere);
            }
            if (CountNum == 0)
            {
                DataTable pageCountTable = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0];
                Counts = pageCountTable.Rows.Count;
            }
            else
            {
                Counts = CountNum;
            }
            if (Counts % pageSize == 0)
                pageCount = Counts / pageSize;
            else
                pageCount = (Counts / pageSize) + 1;

            StringBuilder sb = new StringBuilder();
            DataTable Table = new DataTable();

            sb.AppendFormat("select * from (select ROW_NUMBER() over( order by  ID desc) rwo,* from ");
            sb.AppendFormat(" ({0}) tab )b", strSql.ToString());
            sb.AppendFormat(" where b.rwo between ({0}-1)*{1}+1 and {2}*{3}", pageIndex, pageSize, pageIndex, pageSize);
            Table = SqlHelper.Query(SqlHelper.LocalSqlServer, sb.ToString()).Tables[0];
            return Table;

        }

    }
}

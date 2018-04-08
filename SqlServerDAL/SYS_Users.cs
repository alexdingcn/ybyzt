using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DBUtility;
using System;

namespace Hi.SQLServerDAL
{
    public partial class SYS_Users
    {
        public Hi.Model.SYS_Users GetModel(string username)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from SYS_Users ");
            strSql.Append(" where dr=0 and username=@username");
            SqlParameter[] parameters ={
                                     new SqlParameter("@username",SqlDbType.NVarChar,50)};
            parameters[0].Value = username;
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

        public Hi.Model.SYS_Users GetModelphone(string phone)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from SYS_Users ");
            strSql.Append(" where dr=0 and phone=@phone order by id");
            SqlParameter[] parameters ={
                                     new SqlParameter("@phone",SqlDbType.NVarChar,50)};
            parameters[0].Value = phone;
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

        public IList<Hi.Model.SYS_Users> GetList(string strWhat, string strWhere, string strOrderby,SqlTransaction Tran)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby,Tran));
        }

        public DataSet GetDataSet(string strWhat, string strWhere, string strOrderby,SqlTransaction Tran)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [SYS_Users]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), Tran);
        }

        public Hi.Model.SYS_Users GetDisid(string disid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from SYS_Users ");
            strSql.Append(" where dr=0 and DisID=@disid");
            SqlParameter[] parameters ={
                                     new SqlParameter("@disid",SqlDbType.Int)};
            parameters[0].Value = disid;
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

        public int Add(Hi.Model.SYS_Users model,SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [SYS_Users](");
            strSql.Append("[CompID],[DisID],[Type],[OpenID],[AddType],[RoleID],[UserName],[UserPwd],[UserLoginName],[TrueName],[Sex],[Phone],[Tel],[Identitys],[Address],[Email],[AuditState],[AuditUser],[IsFirst],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@Type,@OpenID,@AddType,@RoleID,@UserName,@UserPwd,@UserLoginName,@TrueName,@Sex,@Phone,@Tel,@Identitys,@Address,@Email,@AuditState,@AuditUser,@IsFirst,@IsEnabled,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@OpenID", SqlDbType.VarChar,50),
                    new SqlParameter("@AddType", SqlDbType.Int),
                    new SqlParameter("@RoleID", SqlDbType.Int),
                    new SqlParameter("@UserName", SqlDbType.VarChar,50),
                    new SqlParameter("@UserPwd", SqlDbType.VarChar,50),
                    new SqlParameter("@UserLoginName", SqlDbType.VarChar,50),
                    new SqlParameter("@TrueName", SqlDbType.VarChar,50),
                    new SqlParameter("@Sex", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Tel", SqlDbType.VarChar,50),
                    new SqlParameter("@Identitys", SqlDbType.VarChar,50),
                    new SqlParameter("@Address", SqlDbType.VarChar,200),
                    new SqlParameter("@Email", SqlDbType.VarChar,50),
                    new SqlParameter("@AuditState", SqlDbType.Int),
                    new SqlParameter("@AuditUser", SqlDbType.VarChar,50),
                    new SqlParameter("@IsFirst", SqlDbType.Int),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.Type;

            if (model.OpenID != null)
                parameters[3].Value = model.OpenID;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.AddType;
            parameters[5].Value = model.RoleID;

            if (model.UserName != null)
                parameters[6].Value = model.UserName;
            else
                parameters[6].Value = DBNull.Value;


            if (model.UserPwd != null)
                parameters[7].Value = model.UserPwd;
            else
                parameters[7].Value = DBNull.Value;


            if (model.UserLoginName != null)
                parameters[8].Value = model.UserLoginName;
            else
                parameters[8].Value = DBNull.Value;


            if (model.TrueName != null)
                parameters[9].Value = model.TrueName;
            else
                parameters[9].Value = DBNull.Value;


            if (model.Sex != null)
                parameters[10].Value = model.Sex;
            else
                parameters[10].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[11].Value = model.Phone;
            else
                parameters[11].Value = DBNull.Value;


            if (model.Tel != null)
                parameters[12].Value = model.Tel;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Identitys != null)
                parameters[13].Value = model.Identitys;
            else
                parameters[13].Value = DBNull.Value;


            if (model.Address != null)
                parameters[14].Value = model.Address;
            else
                parameters[14].Value = DBNull.Value;


            if (model.Email != null)
                parameters[15].Value = model.Email;
            else
                parameters[15].Value = DBNull.Value;

            parameters[16].Value = model.AuditState;

            if (model.AuditUser != null)
                parameters[17].Value = model.AuditUser;
            else
                parameters[17].Value = DBNull.Value;

            parameters[18].Value = model.IsFirst;
            parameters[19].Value = model.IsEnabled;
            parameters[20].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[21].Value = model.CreateDate;
            else
                parameters[21].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[22].Value = model.ts;
            else
                parameters[22].Value = DBNull.Value;

            parameters[23].Value = model.modifyuser;
            if(Tran!=null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(),Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }
        /// <summary>
        /// 修改密码
        /// <param name="Id">用户ID</param>
        /// <param name="NewPassWord">新密码</param>
        /// </summary>
        public bool UpdatePassWord(string NewPassWord, string Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update SYS_Users set ");
            strSql.Append("UserPwd=@UserPwd");
            strSql.Append(" where id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.VarChar),
                    new SqlParameter("@UserPwd", SqlDbType.VarChar),
            };
            parameters[0].Value = Id;
            parameters[1].Value = NewPassWord;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.SYS_Users model,SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [SYS_Users] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[Type]=@Type,");
            strSql.Append("[OpenID]=@OpenID,");
            strSql.Append("[AddType]=@AddType,");
            strSql.Append("[RoleID]=@RoleID,");
            strSql.Append("[UserName]=@UserName,");
            strSql.Append("[UserPwd]=@UserPwd,");
            strSql.Append("[UserLoginName]=@UserLoginName,");
            strSql.Append("[TrueName]=@TrueName,");
            strSql.Append("[Sex]=@Sex,");
            strSql.Append("[Phone]=@Phone,");
            strSql.Append("[Tel]=@Tel,");
            strSql.Append("[Identitys]=@Identitys,");
            strSql.Append("[Address]=@Address,");
            strSql.Append("[Email]=@Email,");
            strSql.Append("[AuditState]=@AuditState,");
            strSql.Append("[AuditUser]=@AuditUser,");
            strSql.Append("[IsFirst]=@IsFirst,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@OpenID", SqlDbType.VarChar,50),
                    new SqlParameter("@AddType", SqlDbType.Int),
                    new SqlParameter("@RoleID", SqlDbType.Int),
                    new SqlParameter("@UserName", SqlDbType.VarChar,50),
                    new SqlParameter("@UserPwd", SqlDbType.VarChar,50),
                    new SqlParameter("@UserLoginName", SqlDbType.VarChar,50),
                    new SqlParameter("@TrueName", SqlDbType.VarChar,50),
                    new SqlParameter("@Sex", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Tel", SqlDbType.VarChar,50),
                    new SqlParameter("@Identitys", SqlDbType.VarChar,50),
                    new SqlParameter("@Address", SqlDbType.VarChar,200),
                    new SqlParameter("@Email", SqlDbType.VarChar,50),
                    new SqlParameter("@AuditState", SqlDbType.Int),
                    new SqlParameter("@AuditUser", SqlDbType.VarChar,50),
                    new SqlParameter("@IsFirst", SqlDbType.Int),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.Type;

            if (model.OpenID != null)
                parameters[4].Value = model.OpenID;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.AddType;
            parameters[6].Value = model.RoleID;

            if (model.UserName != null)
                parameters[7].Value = model.UserName;
            else
                parameters[7].Value = DBNull.Value;


            if (model.UserPwd != null)
                parameters[8].Value = model.UserPwd;
            else
                parameters[8].Value = DBNull.Value;


            if (model.UserLoginName != null)
                parameters[9].Value = model.UserLoginName;
            else
                parameters[9].Value = DBNull.Value;


            if (model.TrueName != null)
                parameters[10].Value = model.TrueName;
            else
                parameters[10].Value = DBNull.Value;


            if (model.Sex != null)
                parameters[11].Value = model.Sex;
            else
                parameters[11].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[12].Value = model.Phone;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Tel != null)
                parameters[13].Value = model.Tel;
            else
                parameters[13].Value = DBNull.Value;


            if (model.Identitys != null)
                parameters[14].Value = model.Identitys;
            else
                parameters[14].Value = DBNull.Value;


            if (model.Address != null)
                parameters[15].Value = model.Address;
            else
                parameters[15].Value = DBNull.Value;


            if (model.Email != null)
                parameters[16].Value = model.Email;
            else
                parameters[16].Value = DBNull.Value;

            parameters[17].Value = model.AuditState;

            if (model.AuditUser != null)
                parameters[18].Value = model.AuditUser;
            else
                parameters[18].Value = DBNull.Value;

            parameters[19].Value = model.IsFirst;
            parameters[20].Value = model.IsEnabled;
            parameters[21].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[22].Value = model.CreateDate;
            else
                parameters[22].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[23].Value = model.ts;
            else
                parameters[23].Value = DBNull.Value;

            parameters[24].Value = model.dr;
            parameters[25].Value = model.modifyuser;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }


        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.SYS_Users> GetListUser(string strWhat,string Key, string Value, string strOrderby)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [SYS_Users]");
            strSql.Append(" where " + Key + "=@Value and isnull(dr,0)=0 ");
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            SqlParameter[] parameters ={
                 new SqlParameter("@Value", SqlDbType.VarChar,50),  
                                       };
            parameters[0].Value = Value;
            return GetList(SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));

        }
    }
}

//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2015 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2015/6/15 14:16:27
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
    /// 数据访问类 SYS_Users
    /// </summary>
    public partial class SYS_Users
    {
        public SYS_Users()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.SYS_Users model)
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
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.SYS_Users model)
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

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [SYS_Users] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[SYS_Users]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [SYS_Users]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.SYS_Users GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [SYS_Users] ");
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
        public IList<Hi.Model.SYS_Users> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [SYS_Users]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.SYS_Users> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.SYS_Users> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[SYS_Users]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.SYS_Users GetModel(DataRow r)
        {
            Hi.Model.SYS_Users model = new Hi.Model.SYS_Users();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.Type = SqlHelper.GetInt(r["Type"]);
            model.OpenID = SqlHelper.GetString(r["OpenID"]);
            model.AddType = SqlHelper.GetInt(r["AddType"]);
            model.RoleID = SqlHelper.GetInt(r["RoleID"]);
            model.UserName = SqlHelper.GetString(r["UserName"]);
            model.UserPwd = SqlHelper.GetString(r["UserPwd"]);
            model.UserLoginName = SqlHelper.GetString(r["UserLoginName"]);
            model.TrueName = SqlHelper.GetString(r["TrueName"]);
            model.Sex = SqlHelper.GetString(r["Sex"]);
            model.Phone = SqlHelper.GetString(r["Phone"]);
            model.Tel = SqlHelper.GetString(r["Tel"]);
            model.Identitys = SqlHelper.GetString(r["Identitys"]);
            model.Address = SqlHelper.GetString(r["Address"]);
            model.Email = SqlHelper.GetString(r["Email"]);
            model.AuditState = SqlHelper.GetInt(r["AuditState"]);
            model.AuditUser = SqlHelper.GetString(r["AuditUser"]);
            model.IsFirst = SqlHelper.GetInt(r["IsFirst"]);
            model.IsEnabled = SqlHelper.GetInt(r["IsEnabled"]);
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
        private IList<Hi.Model.SYS_Users> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.SYS_Users>(ds.Tables[0]);
        }
    }
}

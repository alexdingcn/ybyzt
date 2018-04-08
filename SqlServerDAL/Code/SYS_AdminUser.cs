//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/8/12 18:08:14
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
    /// 数据访问类 SYS_AdminUser
    /// </summary>
    public partial class SYS_AdminUser
    {
        public SYS_AdminUser()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.SYS_AdminUser model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [SYS_AdminUser](");
            strSql.Append("[OrgID],[LoginName],[LoginPwd],[TrueName],[Phone],[UserType],[RoleID],[Remark],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser],[SalesManID])");
            strSql.Append(" values (");
            strSql.Append("@OrgID,@LoginName,@LoginPwd,@TrueName,@Phone,@UserType,@RoleID,@Remark,@IsEnabled,@CreateUserID,@CreateDate,@ts,@modifyuser,@SalesManID)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrgID", SqlDbType.Int),
                    new SqlParameter("@LoginName", SqlDbType.VarChar,50),
                    new SqlParameter("@LoginPwd", SqlDbType.VarChar,50),
                    new SqlParameter("@TrueName", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@UserType", SqlDbType.Int),
                    new SqlParameter("@RoleID", SqlDbType.Int),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@SalesManID", SqlDbType.Int)
            };
            parameters[0].Value = model.OrgID;
            parameters[1].Value = model.LoginName;
            parameters[2].Value = model.LoginPwd;

            if (model.TrueName != null)
                parameters[3].Value = model.TrueName;
            else
                parameters[3].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[4].Value = model.Phone;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.UserType;
            parameters[6].Value = model.RoleID;

            if (model.Remark != null)
                parameters[7].Value = model.Remark;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.IsEnabled;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.CreateDate;
            parameters[11].Value = model.ts;
            parameters[12].Value = model.modifyuser;
            parameters[13].Value = model.SalesManID;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.SYS_AdminUser model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [SYS_AdminUser] set ");
            strSql.Append("[OrgID]=@OrgID,");
            strSql.Append("[LoginName]=@LoginName,");
            strSql.Append("[LoginPwd]=@LoginPwd,");
            strSql.Append("[TrueName]=@TrueName,");
            strSql.Append("[Phone]=@Phone,");
            strSql.Append("[UserType]=@UserType,");
            strSql.Append("[RoleID]=@RoleID,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[SalesManID]=@SalesManID");
            strSql.Append(" where [ID]=@ID");
           
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@OrgID", SqlDbType.Int),
                    new SqlParameter("@LoginName", SqlDbType.VarChar,50),
                    new SqlParameter("@LoginPwd", SqlDbType.VarChar,50),
                    new SqlParameter("@TrueName", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@UserType", SqlDbType.Int),
                    new SqlParameter("@RoleID", SqlDbType.Int),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@IsEnabled", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@SalesManID", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.OrgID;
            parameters[2].Value = model.LoginName;
            parameters[3].Value = model.LoginPwd;

            if (model.TrueName != null)
                parameters[4].Value = model.TrueName;
            else
                parameters[4].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[5].Value = model.Phone;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.UserType;
            parameters[7].Value = model.RoleID;

            if (model.Remark != null)
                parameters[8].Value = model.Remark;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.IsEnabled;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.CreateDate;
            parameters[12].Value = model.ts;
            parameters[13].Value = model.dr;
            parameters[14].Value = model.modifyuser;
            parameters[15].Value = model.SalesManID;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [SYS_AdminUser] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[SYS_AdminUser]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [SYS_AdminUser]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.SYS_AdminUser GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [SYS_AdminUser] ");
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
        public IList<Hi.Model.SYS_AdminUser> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [SYS_AdminUser]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.SYS_AdminUser> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.SYS_AdminUser> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[SYS_AdminUser]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.SYS_AdminUser GetModel(DataRow r)
        {
            Hi.Model.SYS_AdminUser model = new Hi.Model.SYS_AdminUser();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.OrgID = SqlHelper.GetInt(r["OrgID"]);
            model.LoginName = SqlHelper.GetString(r["LoginName"]);
            model.LoginPwd = SqlHelper.GetString(r["LoginPwd"]);
            model.TrueName = SqlHelper.GetString(r["TrueName"]);
            model.Phone = SqlHelper.GetString(r["Phone"]);
            model.UserType = SqlHelper.GetInt(r["UserType"]);
            model.RoleID = SqlHelper.GetInt(r["RoleID"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.IsEnabled = SqlHelper.GetInt(r["IsEnabled"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.SalesManID = SqlHelper.GetInt(r["SalesManID"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.SYS_AdminUser> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.SYS_AdminUser>(ds.Tables[0]);
        }
    }
}

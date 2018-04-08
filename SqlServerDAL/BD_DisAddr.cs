using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DBUtility;
using System.Data.SqlClient;

namespace Hi.SQLServerDAL
{
    public partial class BD_DisAddr
    {
        /// <summary>
        /// 根据UserName查询当前经销商下的所有收货地址
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public DataSet GetModel(string username)
        {
            string sqlstr = string.Format("select * from BD_DisAddr where dr=0 and disid={0} order by IsDefault desc,ts desc", username);
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr);
            return ds;
        }

        /// <summary>
        /// 根据用户修改当前经销商下的默认地址
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool UpdateS(string username)
        {
            string sqlstr = string.Format("update BD_DisAddr set IsDefault=0 where dr=0 and disid={0}", username);
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, sqlstr) > 0;
        }

        public int Add(Hi.Model.BD_DisAddr model,SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_DisAddr](");
            strSql.Append("[Name],[DisID],[Principal],[Phone],[Tel],[Province],[City],[Area],[Zip],[Address],[Remark],[IsDefault],[CreateUserID],[CreateDate],[ts],[modifyuser],[Code])");
            strSql.Append(" values (");
            strSql.Append("@Name,@DisID,@Principal,@Phone,@Tel,@Province,@City,@Area,@Zip,@Address,@Remark,@IsDefault,@CreateUserID,@CreateDate,@ts,@modifyuser,@Code)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@Name", SqlDbType.NChar,50),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@Principal", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Tel", SqlDbType.VarChar,50),
                    new SqlParameter("@Province", SqlDbType.VarChar,50),
                    new SqlParameter("@City", SqlDbType.VarChar,50),
                    new SqlParameter("@Area", SqlDbType.VarChar,100),
                    new SqlParameter("@Zip", SqlDbType.VarChar,50),
                    new SqlParameter("@Address", SqlDbType.VarChar,100),
                    new SqlParameter("@Remark", SqlDbType.VarChar,800),
                    new SqlParameter("@IsDefault", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@Code", SqlDbType.VarChar,50)
            };

            if (model.Name != null)
                parameters[0].Value = model.Name;
            else
                parameters[0].Value = DBNull.Value;

            parameters[1].Value = model.DisID;

            if (model.Principal != null)
                parameters[2].Value = model.Principal;
            else
                parameters[2].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[3].Value = model.Phone;
            else
                parameters[3].Value = DBNull.Value;


            if (model.Tel != null)
                parameters[4].Value = model.Tel;
            else
                parameters[4].Value = DBNull.Value;


            if (model.Province != null)
                parameters[5].Value = model.Province;
            else
                parameters[5].Value = DBNull.Value;


            if (model.City != null)
                parameters[6].Value = model.City;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Area != null)
                parameters[7].Value = model.Area;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Zip != null)
                parameters[8].Value = model.Zip;
            else
                parameters[8].Value = DBNull.Value;


            if (model.Address != null)
                parameters[9].Value = model.Address;
            else
                parameters[9].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[10].Value = model.Remark;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.IsDefault;
            parameters[12].Value = model.CreateUserID;
            parameters[13].Value = model.CreateDate;
            parameters[14].Value = model.ts;
            parameters[15].Value = model.modifyuser;
            if (model.Code != null)
                parameters[16].Value = model.Code;
            else
                parameters[16].Value = DBNull.Value;

            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        public bool Update(Hi.Model.BD_DisAddr model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_DisAddr] set ");
            strSql.Append("[Name]=@Name,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[Principal]=@Principal,");
            strSql.Append("[Phone]=@Phone,");
            strSql.Append("[Tel]=@Tel,");
            strSql.Append("[Province]=@Province,");
            strSql.Append("[City]=@City,");
            strSql.Append("[Area]=@Area,");
            strSql.Append("[Zip]=@Zip,");
            strSql.Append("[Address]=@Address,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[IsDefault]=@IsDefault,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[Code]=@Code");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@Name", SqlDbType.NChar,50),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@Principal", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,50),
                    new SqlParameter("@Tel", SqlDbType.VarChar,50),
                    new SqlParameter("@Province", SqlDbType.VarChar,50),
                    new SqlParameter("@City", SqlDbType.VarChar,50),
                    new SqlParameter("@Area", SqlDbType.VarChar,100),
                    new SqlParameter("@Zip", SqlDbType.VarChar,50),
                    new SqlParameter("@Address", SqlDbType.VarChar,100),
                    new SqlParameter("@Remark", SqlDbType.VarChar,800),
                    new SqlParameter("@IsDefault", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@Code", SqlDbType.VarChar,50)
            };
            parameters[0].Value = model.ID;

            if (model.Name != null)
                parameters[1].Value = model.Name;
            else
                parameters[1].Value = DBNull.Value;

            parameters[2].Value = model.DisID;

            if (model.Principal != null)
                parameters[3].Value = model.Principal;
            else
                parameters[3].Value = DBNull.Value;


            if (model.Phone != null)
                parameters[4].Value = model.Phone;
            else
                parameters[4].Value = DBNull.Value;


            if (model.Tel != null)
                parameters[5].Value = model.Tel;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Province != null)
                parameters[6].Value = model.Province;
            else
                parameters[6].Value = DBNull.Value;


            if (model.City != null)
                parameters[7].Value = model.City;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Area != null)
                parameters[8].Value = model.Area;
            else
                parameters[8].Value = DBNull.Value;


            if (model.Zip != null)
                parameters[9].Value = model.Zip;
            else
                parameters[9].Value = DBNull.Value;


            if (model.Address != null)
                parameters[10].Value = model.Address;
            else
                parameters[10].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[11].Value = model.Remark;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.IsDefault;
            parameters[13].Value = model.CreateUserID;
            parameters[14].Value = model.CreateDate;
            parameters[15].Value = model.ts;
            parameters[16].Value = model.dr;
            parameters[17].Value = model.modifyuser;
            if (model.Code != null)
                parameters[18].Value = model.Code;
            else
                parameters[18].Value = DBNull.Value;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 根据用户查询当前经销商下的默认地址
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Hi.Model.BD_DisAddr GetDefaultAddr(string username)
        {
            string sqlstr = string.Format("select * from BD_DisAddr where DisID=(select DisID from SYS_Users where dr=0 and username='{0}') and IsDefault=1 and dr=0", username);
            if (SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr).Tables[0].Rows.Count == 0)
            {
                return null;
            }
            return GetModel(SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr).Tables[0].Rows[0]);
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        /// <param name="strWhat">字段</param>
        /// <param name="TbName">表名</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrderby">排序字段</param>
        /// <returns></returns>
        public DataTable GetList(string strWhat, string TbName, string strWhere, string strOrderby)
        {
            if (string.IsNullOrEmpty(strWhat))
                strWhat = "*";
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from ");

            if (!string.IsNullOrEmpty(TbName))
                strSql.Append(TbName);
            else
                strSql.Append("[BD_DisAddr]");

            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0];
        }
    }
}

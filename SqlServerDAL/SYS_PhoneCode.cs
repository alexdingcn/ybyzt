using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DBUtility;
using System.Data.SqlClient;

namespace Hi.SQLServerDAL
{
    public partial class SYS_PhoneCode
    {
        /// <summary>
        /// 根据用户与手机获取手机验证码实体
        /// </summary>
        /// <param name="username"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Hi.Model.SYS_PhoneCode GetModel(string username, string phone)
        {
            string sqlstr = string.Format("select top 1 * from SYS_PhoneCode where UserName='{0}' and Phone='{1}' and dr=0 order by createdate desc", username, phone);
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr);
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

        public Hi.Model.SYS_PhoneCode GetModel(string phone)
        {
            string sqlstr = string.Format("select top 1 * from sys_phonecode where phone='{0}' and dr=0 order by createdate desc", phone);
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr);
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


        public bool Update(Hi.Model.SYS_PhoneCode model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [SYS_PhoneCode] set ");
            strSql.Append("[Type]=@Type,");
            strSql.Append("[Module]=@Module,");
            strSql.Append("[UserID]=@UserID,");
            strSql.Append("[UserName]=@UserName,");
            strSql.Append("[Phone]=@Phone,");
            strSql.Append("[PhoneCode]=@PhoneCode,");
            strSql.Append("[IsPast]=@IsPast,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@Module", SqlDbType.VarChar,50),
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@UserName", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,11),
                    new SqlParameter("@PhoneCode", SqlDbType.VarChar,50),
                    new SqlParameter("@IsPast", SqlDbType.SmallInt),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Type;

            if (model.Module != null)
                parameters[2].Value = model.Module;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.UserID;
            parameters[4].Value = model.UserName;
            parameters[5].Value = model.Phone;
            parameters[6].Value = model.PhoneCode;
            parameters[7].Value = model.IsPast;
            parameters[8].Value = model.CreateDate;
            parameters[9].Value = model.ts;
            parameters[10].Value = model.dr;
            parameters[11].Value = model.modifyuser;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }


        /// <summary>
        /// kb  验证手机验证码
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="Phone">手机号码</param>
        /// <param name="PhoneCode">验证码</param>
        /// <returns></returns>
        public Hi.Model.SYS_PhoneCode GetModel(string module, string Phone, string PhoneCode)
        {
            string sqlstr = "select top 1 * from SYS_PhoneCode where DATEDIFF(minute,CreateDate,GETDATE()) between 0 and 30 and ispast=0 and module=@Module and Phone=@Phone and PhoneCode=@PhoneCode order by createdate desc";
            SqlParameter[] Sqlparams = { 
                    new SqlParameter("@Module", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,11),
                     new SqlParameter("@PhoneCode", SqlDbType.VarChar,50)
                                       };
            Sqlparams[0].Value = module;
            Sqlparams[1].Value = Phone;
            Sqlparams[2].Value = PhoneCode;
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr, Sqlparams);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow r = ds.Tables[0].Rows[0];
                return Common.GetListEntity<Hi.Model.SYS_PhoneCode>(ds.Tables[0])[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// kb  验证手机验证码
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="Phone">手机号码</param>
        /// <param name="PhoneCode">验证码</param>
        /// <returns></returns>
        public List<Hi.Model.SYS_PhoneCode> GetListModel(string module, string Phone)
        {
            string sqlstr = "select  * from SYS_PhoneCode where DATEDIFF(minute,CreateDate,GETDATE()) between 0 and 60 and ispast=0 and module=@Module and Phone=@Phone  order by createdate desc";
            SqlParameter[] Sqlparams = new SqlParameter[] { 
                    new SqlParameter("@Module", SqlDbType.VarChar,50),
                    new SqlParameter("@Phone", SqlDbType.VarChar,11),
                                       };
            Sqlparams[0].Value = module;
            Sqlparams[1].Value = Phone;
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr, Sqlparams);
            return Common.GetListEntity<Hi.Model.SYS_PhoneCode>(ds.Tables[0]);
        }

    }
}

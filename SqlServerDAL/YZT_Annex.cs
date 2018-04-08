//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2017 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2017/12/23 16:31:42
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
    /// 数据访问类 YZT_Annex
    /// </summary>
    public partial class YZT_Annex
    {
     
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_Annex model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [YZT_Annex](");
            strSql.Append("[fcID],[type],[validDate],[fileName],[fileAlias],[vdef1],[vdef2],[vdef3],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@fcID,@type,@validDate,@fileName,@fileAlias,@vdef1,@vdef2,@vdef3,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@fcID", SqlDbType.Int),
                    new SqlParameter("@type", SqlDbType.Int),
                    new SqlParameter("@validDate", SqlDbType.DateTime),
                    new SqlParameter("@fileName", SqlDbType.VarChar,128),
                    new SqlParameter("@fileAlias", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.fcID;
            parameters[1].Value = model.type;

            if (model.validDate != DateTime.MinValue)
                parameters[2].Value = model.validDate;
            else
                parameters[2].Value = DBNull.Value;


            if (model.fileName != null)
                parameters[3].Value = model.fileName;
            else
                parameters[3].Value = DBNull.Value;


            if (model.fileAlias != null)
                parameters[4].Value = model.fileAlias;
            else
                parameters[4].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[5].Value = model.vdef1;
            else
                parameters[5].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[6].Value = model.vdef2;
            else
                parameters[6].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[7].Value = model.vdef3;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[9].Value = model.CreateDate;
            else
                parameters[9].Value = DBNull.Value;

            parameters[10].Value = model.ts;
            parameters[11].Value = model.modifyuser;
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_Annex model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [YZT_Annex] set ");
            strSql.Append("[fcID]=@fcID,");
            strSql.Append("[type]=@type,");
            strSql.Append("[validDate]=@validDate,");
            strSql.Append("[fileName]=@fileName,");
            strSql.Append("[fileAlias]=@fileAlias,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@fcID", SqlDbType.Int),
                    new SqlParameter("@type", SqlDbType.Int),
                    new SqlParameter("@validDate", SqlDbType.DateTime),
                    new SqlParameter("@fileName", SqlDbType.VarChar,128),
                    new SqlParameter("@fileAlias", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.fcID;
            parameters[2].Value = model.type;

            if (model.validDate != DateTime.MinValue)
                parameters[3].Value = model.validDate;
            else
                parameters[3].Value = DBNull.Value;


            if (model.fileName != null)
                parameters[4].Value = model.fileName;
            else
                parameters[4].Value = DBNull.Value;


            if (model.fileAlias != null)
                parameters[5].Value = model.fileAlias;
            else
                parameters[5].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[6].Value = model.vdef1;
            else
                parameters[6].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[7].Value = model.vdef2;
            else
                parameters[7].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[8].Value = model.vdef3;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[10].Value = model.CreateDate;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.ts;
            parameters[12].Value = model.dr;
            parameters[13].Value = model.modifyuser;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="strwhere">删除条件</param>
        /// <param name="Tran"></param>
        /// <returns></returns>
        public bool AnnexDelete(string strwhere, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [YZT_Annex] ");
            strSql.Append(" where ");
            if ("".Equals(strwhere))
            {
                strSql.Append(" 1=1 ");
            }
            else {
                strSql.Append(strwhere);
            }

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString()) > 0;
        }
    }
}

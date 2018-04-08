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
    /// 数据访问类 YZT_Contract
    /// </summary>
    public partial class YZT_Contract
    {
        

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_Contract model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [YZT_Contract](");
            strSql.Append("[CompID],[DisID],[contractNO],[contractDate],[CState],[ForceDate],[InvalidDate],[Remark],[vdef1],[vdef2],[vdef3],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@contractNO,@contractDate,@CState,@ForceDate,@InvalidDate,@Remark,@vdef1,@vdef2,@vdef3,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@contractNO", SqlDbType.VarChar,50),
                    new SqlParameter("@contractDate", SqlDbType.DateTime),
                    new SqlParameter("@CState", SqlDbType.Int),
                    new SqlParameter("@ForceDate", SqlDbType.DateTime),
                    new SqlParameter("@InvalidDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.DisID;

            if (model.contractNO != null)
                parameters[2].Value = model.contractNO;
            else
                parameters[2].Value = DBNull.Value;


            if (model.contractDate != DateTime.MinValue)
                parameters[3].Value = model.contractDate;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.CState;

            if (model.ForceDate != DateTime.MinValue)
                parameters[5].Value = model.ForceDate;
            else
                parameters[5].Value = DBNull.Value;


            if (model.InvalidDate != DateTime.MinValue)
                parameters[6].Value = model.InvalidDate;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[7].Value = model.Remark;
            else
                parameters[7].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[8].Value = model.vdef1;
            else
                parameters[8].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[9].Value = model.vdef2;
            else
                parameters[9].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[10].Value = model.vdef3;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[12].Value = model.CreateDate;
            else
                parameters[12].Value = DBNull.Value;

            parameters[13].Value = model.ts;
            parameters[14].Value = model.modifyuser;
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_Contract model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [YZT_Contract] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[contractNO]=@contractNO,");
            strSql.Append("[contractDate]=@contractDate,");
            strSql.Append("[CState]=@CState,");
            strSql.Append("[ForceDate]=@ForceDate,");
            strSql.Append("[InvalidDate]=@InvalidDate,");
            strSql.Append("[Remark]=@Remark,");
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
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@contractNO", SqlDbType.VarChar,50),
                    new SqlParameter("@contractDate", SqlDbType.DateTime),
                    new SqlParameter("@CState", SqlDbType.Int),
                    new SqlParameter("@ForceDate", SqlDbType.DateTime),
                    new SqlParameter("@InvalidDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1024),
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
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.DisID;

            if (model.contractNO != null)
                parameters[3].Value = model.contractNO;
            else
                parameters[3].Value = DBNull.Value;


            if (model.contractDate != DateTime.MinValue)
                parameters[4].Value = model.contractDate;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.CState;

            if (model.ForceDate != DateTime.MinValue)
                parameters[6].Value = model.ForceDate;
            else
                parameters[6].Value = DBNull.Value;


            if (model.InvalidDate != DateTime.MinValue)
                parameters[7].Value = model.InvalidDate;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[8].Value = model.Remark;
            else
                parameters[8].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[9].Value = model.vdef1;
            else
                parameters[9].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[10].Value = model.vdef2;
            else
                parameters[10].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[11].Value = model.vdef3;
            else
                parameters[11].Value = DBNull.Value;

            parameters[12].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[13].Value = model.CreateDate;
            else
                parameters[13].Value = DBNull.Value;

            parameters[14].Value = model.ts;
            parameters[15].Value = model.dr;
            parameters[16].Value = model.modifyuser;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }


        #endregion


        /// <summary>
        /// 获取合同列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="strWhere"></param>
        /// <param name="pageCount"></param>
        /// <param name="Counts"></param>
        /// <returns></returns>
        public DataTable getDataTable(int pageSize, int pageIndex, string strWhere, out int pageCount, out int Counts)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select c.ID,c.contractNO,d.DisName,c.ForceDate,c.CState,m.CompName from YZT_Contract c left join BD_Distributor d on c.DisID=d.ID  left join BD_Company m on c.CompID=m.id   ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.AppendFormat("where 1=1 {0}", strWhere);
            }
            DataTable pageCountTable = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0];
            Counts = pageCountTable.Rows.Count;
            if (Counts % pageSize == 0)
                pageCount = Counts / pageSize;
            else
                pageCount = (Counts / pageSize) + 1;

            StringBuilder sb = new StringBuilder();
            DataTable Table = new DataTable();
            if (Counts > 0)
            {
                sb.AppendFormat("select * from (select ROW_NUMBER() over( order by  ID) rwo,* from ");
                sb.AppendFormat(" ({0}) tab )b", strSql.ToString());
                sb.AppendFormat(" where b.rwo between ({0}-1)*{1}+1 and {2}*{3}", pageIndex, pageSize, pageIndex, pageSize);
                Table = SqlHelper.Query(SqlHelper.LocalSqlServer, sb.ToString()).Tables[0];
                return Table;
            }
            return pageCountTable;
        }



    }
}

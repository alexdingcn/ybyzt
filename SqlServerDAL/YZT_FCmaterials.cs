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
    /// 数据访问类 YZT_FCmaterials 
    /// </summary>
    public partial class YZT_FCmaterials
    {



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_FCmaterials model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [YZT_FCmaterials](");
            strSql.Append("[CompID],[DisID],[type],[Rise],[Content],[OBank],[OAccount],[TRNumber],[vdef1],[vdef2],[vdef3],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@type,@Rise,@Content,@OBank,@OAccount,@TRNumber,@vdef1,@vdef2,@vdef3,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@type", SqlDbType.Int),
                    new SqlParameter("@Rise", SqlDbType.VarChar,100),
                    new SqlParameter("@Content", SqlDbType.VarChar,200),
                    new SqlParameter("@OBank", SqlDbType.VarChar,100),
                    new SqlParameter("@OAccount", SqlDbType.VarChar,100),
                    new SqlParameter("@TRNumber", SqlDbType.VarChar,100),
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
            parameters[2].Value = model.type;

            if (model.Rise != null)
                parameters[3].Value = model.Rise;
            else
                parameters[3].Value = DBNull.Value;


            if (model.Content != null)
                parameters[4].Value = model.Content;
            else
                parameters[4].Value = DBNull.Value;


            if (model.OBank != null)
                parameters[5].Value = model.OBank;
            else
                parameters[5].Value = DBNull.Value;


            if (model.OAccount != null)
                parameters[6].Value = model.OAccount;
            else
                parameters[6].Value = DBNull.Value;


            if (model.TRNumber != null)
                parameters[7].Value = model.TRNumber;
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
        public bool Update(Hi.Model.YZT_FCmaterials model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [YZT_FCmaterials] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[type]=@type,");
            strSql.Append("[Rise]=@Rise,");
            strSql.Append("[Content]=@Content,");
            strSql.Append("[OBank]=@OBank,");
            strSql.Append("[OAccount]=@OAccount,");
            strSql.Append("[TRNumber]=@TRNumber,");
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
                    new SqlParameter("@type", SqlDbType.Int),
                    new SqlParameter("@Rise", SqlDbType.VarChar,100),
                    new SqlParameter("@Content", SqlDbType.VarChar,200),
                    new SqlParameter("@OBank", SqlDbType.VarChar,100),
                    new SqlParameter("@OAccount", SqlDbType.VarChar,100),
                    new SqlParameter("@TRNumber", SqlDbType.VarChar,100),
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
            parameters[3].Value = model.type;

            if (model.Rise != null)
                parameters[4].Value = model.Rise;
            else
                parameters[4].Value = DBNull.Value;


            if (model.Content != null)
                parameters[5].Value = model.Content;
            else
                parameters[5].Value = DBNull.Value;


            if (model.OBank != null)
                parameters[6].Value = model.OBank;
            else
                parameters[6].Value = DBNull.Value;


            if (model.OAccount != null)
                parameters[7].Value = model.OAccount;
            else
                parameters[7].Value = DBNull.Value;


            if (model.TRNumber != null)
                parameters[8].Value = model.TRNumber;
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

        /// <summary>
        /// 获取包含 代理商名称 编码 的  代理商首营资料 分页数据
        /// </summary>
        /// <param name="pageSize">一页几行</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="Compid">厂商ID</param>
        /// <param name="disName">厂商名称（可为空）</param>
        /// <param name="pageCount">一共几页</param>
        /// <param name="Counts">总行数</param>
        /// <returns></returns>
        /// 
        public DataTable getDataTable(int pageSize, int pageIndex,string Compid, string disName,out int pageCount, out int Counts) {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select d.DisCode,d.DisName,f.ID from YZT_FCmaterials f join SYS_CompUser c on   ");
            strSql.Append("c.DisID= f.DisID and c.dr=0 and  c.IsAudit=2  ");
            strSql.AppendFormat("and c.CompID='{0}'    ", Compid);
            strSql.Append(" join BD_Distributor d on f.DisID =d.ID and d.dr=0    ");
            if (!string.IsNullOrEmpty(disName))
            {
                strSql.AppendFormat(" and d.DisName like '%{0}%' ",disName);
            }
            DataTable pageCountTable= SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0];
            Counts = pageCountTable.Rows.Count;
            if (Counts % pageSize==0)
            pageCount = Counts / pageSize;
            else
                pageCount = (Counts / pageSize)+1;

            StringBuilder sb = new StringBuilder();
            DataTable Table = new DataTable();
            if (Counts > 0)
            {
                sb.AppendFormat("select * from (select ROW_NUMBER() over( order by  ID) rwo,* from ");
                sb.AppendFormat(" ({0}) tab )b", strSql.ToString());
                sb.AppendFormat(" where b.rwo between ({0}-1)*{1}+1 and {2}*{3}", pageIndex, pageSize, pageIndex, pageSize);
                Table= SqlHelper.Query(SqlHelper.LocalSqlServer, sb.ToString()).Tables[0];
                return Table;
            }
            return pageCountTable;
        }


        /// <summary>
        /// 获取包含  厂商名称 编码 的  厂商首营资料 分页数据
        /// </summary>
        /// <param name="pageSize">一页几行</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="Disid">代理商ID</param>
        /// <param name="strWhere">其他 条件</param>
        /// <param name="pageCount">一共几页</param>
        /// <param name="Counts">总行数</param>
        /// <returns></returns>
        public DataTable getCompDataTable(int pageSize, int pageIndex, string Disid, string strWhere, out int pageCount, out int Counts)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select d.ID CompID,d.CompCode,d.CompName,f.ID from YZT_FCmaterials f join SYS_CompUser c on   ");
            strSql.Append("c.DisID= f.DisId and c.dr=0 and  c.IsAudit=2  ");
            strSql.AppendFormat("and c.disid='{0}'    ",Disid);
            strSql.Append(" join BD_Company  d on c.CompID =d.ID and d.dr=0    ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.AppendFormat("where 1=1 {0}",strWhere);
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

        /// <summary>
        /// 根据首营资料ID  获取  厂商首营资料
        /// </summary>
        /// <param name="fid">首营资料ID</param>
        /// <returns></returns>
        public DataTable getDataModel(string Disid, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select d.ID CompID,d.CompCode,d.CompName,f.ID from YZT_FCmaterials f join SYS_CompUser c on   ");
            strSql.Append("c.DisID= f.DisId and c.dr=0 and  c.IsAudit=2  ");
            strSql.AppendFormat("and c.disid='{0}'    ", Disid);
            strSql.Append(" join BD_Company  d on c.CompID =d.ID and d.dr=0    ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.AppendFormat("where 1=1 {0}", strWhere);
            }
            DataTable dataModel = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0];
            return dataModel;
        }


        /// <summary>
        /// 根据首营资料ID  获取  代理商首营资料 及其 代理商信息
        /// </summary>
        /// <param name="fid">首营资料ID</param>
        /// <returns></returns>
        public DataTable getDataModel(string fid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  d.DisCode,d.DisName,f.ID from YZT_FCmaterials f     ");
            strSql.Append("join BD_Distributor d on f.DisID =d.ID and d.dr=0   ");
            strSql.AppendFormat("where f.id='{0}'    ", fid);
            DataTable dataModel = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString()).Tables[0];

            return dataModel;
        }

    }
    
}

//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2017 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2017/2/6 13:03:24
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
    /// 数据访问类 DIS_StockOrder
    /// </summary>
    public partial class DIS_StockOrder
    {
        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public DataTable GetListPage(int pageSize, int pageIndex,string where)
        {
         string strSql=string.Format(@" select  ID, OrderNO, Type, StockType, ChkDate, State, CreateUserID from (select ROW_NUMBER() 
         over(order by ChkDate desc) as rw, * from DIS_StockOrder where 1 = 1 {0} )
         a where a.rw between({1} - 1) * {2} + 1 and {3} * {4} order by ChkDate desc", where, pageIndex, pageSize, pageIndex, pageSize);
         return SqlHelper.GetTable(SqlHelper.LocalSqlServer,strSql);
        }

        /// <summary>
        /// 执行sql语局获取数据
        /// </summary>
        public DataTable GetDataTable(string sql)
        {
            return SqlHelper.GetTable(SqlHelper.LocalSqlServer, sql);
        }

        /// <summary>
        /// 获取总行数
        /// </summary>
        /// <returns></returns>
        public int GetPageCount(string where)
        {
            string strSql = string.Format("select  ID from  DIS_StockOrder where 1=1 {0}",where);
            DataTable dt=  SqlHelper.GetTable(SqlHelper.LocalSqlServer, strSql);
            return dt.Rows.Count;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_StockOrder model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_StockOrder] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[OrderNO]=@OrderNO,");
            strSql.Append("[Type]=@Type,");
            strSql.Append("[StockType]=@StockType,");
            strSql.Append("[ChkDate]=@ChkDate,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[State]=@State,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[OrderID]=@OrderID");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@OrderNO", SqlDbType.VarChar,100),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@StockType", SqlDbType.VarChar,100),
                    new SqlParameter("@ChkDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1000),
                    new SqlParameter("@State", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@OrderID", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;

            if (model.OrderNO != null)
                parameters[2].Value = model.OrderNO;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.Type;

            if (model.StockType != null)
                parameters[4].Value = model.StockType;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.ChkDate;

            if (model.Remark != null)
                parameters[6].Value = model.Remark;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.State;
            parameters[8].Value = model.CreateUserID;
            parameters[9].Value = model.CreateDate;
            parameters[10].Value = model.ts;
            parameters[11].Value = model.dr;
            parameters[12].Value = model.modifyuser;
            parameters[13].Value = model.OrderID;

            if (tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_StockOrder model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_StockOrder](");
            strSql.Append("[CompID],[OrderNO],[Type],[StockType],[ChkDate],[Remark],[State],[CreateUserID],[CreateDate],[ts],[modifyuser],[OrderID])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@OrderNO,@Type,@StockType,@ChkDate,@Remark,@State,@CreateUserID,@CreateDate,@ts,@modifyuser,@OrderID)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@OrderNO", SqlDbType.VarChar,100),
                    new SqlParameter("@Type", SqlDbType.Int),
                    new SqlParameter("@StockType", SqlDbType.VarChar,100),
                    new SqlParameter("@ChkDate", SqlDbType.DateTime),
                    new SqlParameter("@Remark", SqlDbType.VarChar,1000),
                    new SqlParameter("@State", SqlDbType.Int),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@OrderID", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;

            if (model.OrderNO != null)
                parameters[1].Value = model.OrderNO;
            else
                parameters[1].Value = DBNull.Value;

            parameters[2].Value = model.Type;

            if (model.StockType != null)
                parameters[3].Value = model.StockType;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.ChkDate;

            if (model.Remark != null)
                parameters[5].Value = model.Remark;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.State;
            parameters[7].Value = model.CreateUserID;
            parameters[8].Value = model.CreateDate;
            parameters[9].Value = model.ts;
            parameters[10].Value = model.modifyuser;
            parameters[11].Value = model.OrderID;

            if (tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

     
    }
}

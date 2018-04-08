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
    /// 数据访问类 YZT_GoodsStock
    /// </summary>
    public  partial class YZT_GoodsStock
    {

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.YZT_GoodsStock model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [YZT_GoodsStock](");
            strSql.Append("[CompID],[DisID],[GoodsID],[GoodsCode],[GoodsName],[ValueInfo],[CategoryID],[Unit],[BatchNO],[validDate],[StockUseNum],[StockNum],[MinAlertNum],[MaxAlertNum],[Price],[vdef1],[vdef2],[vdef3],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@GoodsID,@GoodsCode,@GoodsName,@ValueInfo,@CategoryID,@Unit,@BatchNO,@validDate,@StockUseNum,@StockNum,@MinAlertNum,@MaxAlertNum,@Price,@vdef1,@vdef2,@vdef3,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,100),
                    new SqlParameter("@ValueInfo", SqlDbType.VarChar,100),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@Unit", SqlDbType.VarChar,30),
                    new SqlParameter("@BatchNO", SqlDbType.VarChar,100),
                    new SqlParameter("@validDate", SqlDbType.DateTime),
                    new SqlParameter("@StockUseNum", SqlDbType.Decimal),
                    new SqlParameter("@StockNum", SqlDbType.Decimal),
                    new SqlParameter("@MinAlertNum", SqlDbType.Int),
                    new SqlParameter("@MaxAlertNum", SqlDbType.Int),
                    new SqlParameter("@Price", SqlDbType.Decimal),
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
            parameters[2].Value = model.GoodsID;

            if (model.GoodsCode != null)
                parameters[3].Value = model.GoodsCode;
            else
                parameters[3].Value = DBNull.Value;


            if (model.GoodsName != null)
                parameters[4].Value = model.GoodsName;
            else
                parameters[4].Value = DBNull.Value;


            if (model.ValueInfo != null)
                parameters[5].Value = model.ValueInfo;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.CategoryID;

            if (model.Unit != null)
                parameters[7].Value = model.Unit;
            else
                parameters[7].Value = DBNull.Value;


            if (model.BatchNO != null)
                parameters[8].Value = model.BatchNO;
            else
                parameters[8].Value = DBNull.Value;


            if (model.validDate != DateTime.MinValue)
                parameters[9].Value = model.validDate;
            else
                parameters[9].Value = DBNull.Value;

            parameters[10].Value = model.StockUseNum;
            parameters[11].Value = model.StockNum;
            parameters[12].Value = model.MinAlertNum;
            parameters[13].Value = model.MaxAlertNum;
            parameters[14].Value = model.Price;

            if (model.vdef1 != null)
                parameters[15].Value = model.vdef1;
            else
                parameters[15].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[16].Value = model.vdef2;
            else
                parameters[16].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[17].Value = model.vdef3;
            else
                parameters[17].Value = DBNull.Value;

            parameters[18].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[19].Value = model.CreateDate;
            else
                parameters[19].Value = DBNull.Value;

            parameters[20].Value = model.ts;
            parameters[21].Value = model.modifyuser;
            if (Tran != null)
                return SqlHelper.GetInt(SqlHelper.GetSingle(strSql.ToString(), Tran, parameters));
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.YZT_GoodsStock model, SqlTransaction Tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [YZT_GoodsStock] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[GoodsID]=@GoodsID,");
            strSql.Append("[GoodsCode]=@GoodsCode,");
            strSql.Append("[GoodsName]=@GoodsName,");
            strSql.Append("[ValueInfo]=@ValueInfo,");
            strSql.Append("[CategoryID]=@CategoryID,");
            strSql.Append("[Unit]=@Unit,");
            strSql.Append("[BatchNO]=@BatchNO,");
            strSql.Append("[validDate]=@validDate,");
            strSql.Append("[StockUseNum]=@StockUseNum,");
            strSql.Append("[StockNum]=@StockNum,");
            strSql.Append("[MinAlertNum]=@MinAlertNum,");
            strSql.Append("[MaxAlertNum]=@MaxAlertNum,");
            strSql.Append("[Price]=@Price,");
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
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,50),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,100),
                    new SqlParameter("@ValueInfo", SqlDbType.VarChar,100),
                    new SqlParameter("@CategoryID", SqlDbType.Int),
                    new SqlParameter("@Unit", SqlDbType.VarChar,30),
                    new SqlParameter("@BatchNO", SqlDbType.VarChar,100),
                    new SqlParameter("@validDate", SqlDbType.DateTime),
                    new SqlParameter("@StockUseNum", SqlDbType.Decimal),
                    new SqlParameter("@StockNum", SqlDbType.Decimal),
                    new SqlParameter("@MinAlertNum", SqlDbType.Int),
                    new SqlParameter("@MaxAlertNum", SqlDbType.Int),
                    new SqlParameter("@Price", SqlDbType.Decimal),
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
            parameters[3].Value = model.GoodsID;

            if (model.GoodsCode != null)
                parameters[4].Value = model.GoodsCode;
            else
                parameters[4].Value = DBNull.Value;


            if (model.GoodsName != null)
                parameters[5].Value = model.GoodsName;
            else
                parameters[5].Value = DBNull.Value;


            if (model.ValueInfo != null)
                parameters[6].Value = model.ValueInfo;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.CategoryID;

            if (model.Unit != null)
                parameters[8].Value = model.Unit;
            else
                parameters[8].Value = DBNull.Value;


            if (model.BatchNO != null)
                parameters[9].Value = model.BatchNO;
            else
                parameters[9].Value = DBNull.Value;


            if (model.validDate != DateTime.MinValue)
                parameters[10].Value = model.validDate;
            else
                parameters[10].Value = DBNull.Value;

            parameters[11].Value = model.StockUseNum;
            parameters[12].Value = model.StockNum;
            parameters[13].Value = model.MinAlertNum;
            parameters[14].Value = model.MaxAlertNum;
            parameters[15].Value = model.Price;

            if (model.vdef1 != null)
                parameters[16].Value = model.vdef1;
            else
                parameters[16].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[17].Value = model.vdef2;
            else
                parameters[17].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[18].Value = model.vdef3;
            else
                parameters[18].Value = DBNull.Value;

            parameters[19].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[20].Value = model.CreateDate;
            else
                parameters[20].Value = DBNull.Value;

            parameters[21].Value = model.ts;
            parameters[22].Value = model.dr;
            parameters[23].Value = model.modifyuser;

            if (Tran != null)
                return SqlHelper.ExecuteSql(strSql.ToString(), Tran, parameters) > 0;
            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
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
        public DataTable getDataTable(int pageSize, int pageIndex, string strWhere, out int pageCount, out int Counts,int CountNum)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select s.ID,s.GoodsID,s.GoodsCode,s.GoodsName, s.BatchNO,s.validDate,c.CategoryName,s.ValueInfo,s.Unit,s.StockNum,s.StockUseNum ");
            strSql.Append(" from YZT_GoodsStock s left join BD_GoodsCategory c on s.CategoryID=c.id ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.AppendFormat("where 1=1 {0}", strWhere);
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

            sb.AppendFormat("select * from (select ROW_NUMBER() over( order by  ID) rwo,* from ");
            sb.AppendFormat(" ({0}) tab )b", strSql.ToString());
            sb.AppendFormat(" where b.rwo between ({0}-1)*{1}+1 and {2}*{3}", pageIndex, pageSize, pageIndex, pageSize);
            Table = SqlHelper.Query(SqlHelper.LocalSqlServer, sb.ToString()).Tables[0];
            return Table;

        }


    }
}

//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/8/31 9:27:51
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
    /// 数据访问类 DIS_OrderExt
    /// </summary>
    public class DIS_OrderExt
    {
        public DIS_OrderExt()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_OrderExt model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_OrderExt](");
            strSql.Append("[OrderID],[DisAccID],[Rise],[Content],[OBank],[OAccount],[TRNumber],[BillNo],[IsBill],[IsOBill],[ProID],[ProAmount],[ProDID],[Protype],[vdef1],[vdef2],[vdef3],[vdef4],[vdef5],[vdef6],[vdef7])");
            strSql.Append(" values (");
            strSql.Append("@OrderID,@DisAccID,@Rise,@Content,@OBank,@OAccount,@TRNumber,@BillNo,@IsBill,@IsOBill,@ProID,@ProAmount,@ProDID,@Protype,@vdef1,@vdef2,@vdef3,@vdef4,@vdef5,@vdef6,@vdef7)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisAccID", SqlDbType.NChar,10),
                    new SqlParameter("@Rise", SqlDbType.VarChar,100),
                    new SqlParameter("@Content", SqlDbType.VarChar,200),
                    new SqlParameter("@OBank", SqlDbType.VarChar,100),
                    new SqlParameter("@OAccount", SqlDbType.VarChar,100),
                    new SqlParameter("@TRNumber", SqlDbType.VarChar,100),
                    new SqlParameter("@BillNo", SqlDbType.VarChar,50),
                    new SqlParameter("@IsBill", SqlDbType.Int),
                    new SqlParameter("@IsOBill", SqlDbType.Int),
                    new SqlParameter("@ProID", SqlDbType.Int),
                    new SqlParameter("@ProAmount", SqlDbType.Decimal),
                    new SqlParameter("@ProDID", SqlDbType.Int),
                    new SqlParameter("@Protype", SqlDbType.VarChar,134),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef4", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef5", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef6", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef7", SqlDbType.VarChar,128)
            };
            parameters[0].Value = model.OrderID;

            if (model.DisAccID != null)
                parameters[1].Value = model.DisAccID;
            else
                parameters[1].Value = DBNull.Value;


            if (model.Rise != null)
                parameters[2].Value = model.Rise;
            else
                parameters[2].Value = DBNull.Value;


            if (model.Content != null)
                parameters[3].Value = model.Content;
            else
                parameters[3].Value = DBNull.Value;


            if (model.OBank != null)
                parameters[4].Value = model.OBank;
            else
                parameters[4].Value = DBNull.Value;


            if (model.OAccount != null)
                parameters[5].Value = model.OAccount;
            else
                parameters[5].Value = DBNull.Value;


            if (model.TRNumber != null)
                parameters[6].Value = model.TRNumber;
            else
                parameters[6].Value = DBNull.Value;


            if (model.BillNo != null)
                parameters[7].Value = model.BillNo;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.IsBill;
            parameters[9].Value = model.IsOBill;
            parameters[10].Value = model.ProID;
            parameters[11].Value = model.ProAmount;
            parameters[12].Value = model.ProDID;

            if (model.Protype != null)
                parameters[13].Value = model.Protype;
            else
                parameters[13].Value = DBNull.Value;


            if (model.vdef1 != null)
                parameters[14].Value = model.vdef1;
            else
                parameters[14].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[15].Value = model.vdef2;
            else
                parameters[15].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[16].Value = model.vdef3;
            else
                parameters[16].Value = DBNull.Value;


            if (model.vdef4 != null)
                parameters[17].Value = model.vdef4;
            else
                parameters[17].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[18].Value = model.vdef5;
            else
                parameters[18].Value = DBNull.Value;


            if (model.vdef6 != null)
                parameters[19].Value = model.vdef6;
            else
                parameters[19].Value = DBNull.Value;


            if (model.vdef7 != null)
                parameters[20].Value = model.vdef7;
            else
                parameters[20].Value = DBNull.Value;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_OrderExt model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_OrderExt] set ");
            strSql.Append("[OrderID]=@OrderID,");
            strSql.Append("[DisAccID]=@DisAccID,");
            strSql.Append("[Rise]=@Rise,");
            strSql.Append("[Content]=@Content,");
            strSql.Append("[OBank]=@OBank,");
            strSql.Append("[OAccount]=@OAccount,");
            strSql.Append("[TRNumber]=@TRNumber,");
            strSql.Append("[BillNo]=@BillNo,");
            strSql.Append("[IsBill]=@IsBill,");
            strSql.Append("[IsOBill]=@IsOBill,");
            strSql.Append("[ProID]=@ProID,");
            strSql.Append("[ProAmount]=@ProAmount,");
            strSql.Append("[ProDID]=@ProDID,");
            strSql.Append("[Protype]=@Protype,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[vdef4]=@vdef4,");
            strSql.Append("[vdef5]=@vdef5,");
            strSql.Append("[vdef6]=@vdef6,");
            strSql.Append("[vdef7]=@vdef7");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisAccID", SqlDbType.NChar,10),
                    new SqlParameter("@Rise", SqlDbType.VarChar,100),
                    new SqlParameter("@Content", SqlDbType.VarChar,200),
                    new SqlParameter("@OBank", SqlDbType.VarChar,100),
                    new SqlParameter("@OAccount", SqlDbType.VarChar,100),
                    new SqlParameter("@TRNumber", SqlDbType.VarChar,100),
                    new SqlParameter("@BillNo", SqlDbType.VarChar,50),
                    new SqlParameter("@IsBill", SqlDbType.Int),
                    new SqlParameter("@IsOBill", SqlDbType.Int),
                    new SqlParameter("@ProID", SqlDbType.Int),
                    new SqlParameter("@ProAmount", SqlDbType.Decimal),
                    new SqlParameter("@ProDID", SqlDbType.Int),
                    new SqlParameter("@Protype", SqlDbType.VarChar,134),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef4", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef5", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef6", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef7", SqlDbType.VarChar,128)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.OrderID;

            if (model.DisAccID != null)
                parameters[2].Value = model.DisAccID;
            else
                parameters[2].Value = DBNull.Value;


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


            if (model.BillNo != null)
                parameters[8].Value = model.BillNo;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.IsBill;
            parameters[10].Value = model.IsOBill;
            parameters[11].Value = model.ProID;
            parameters[12].Value = model.ProAmount;
            parameters[13].Value = model.ProDID;

            if (model.Protype != null)
                parameters[14].Value = model.Protype;
            else
                parameters[14].Value = DBNull.Value;


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


            if (model.vdef4 != null)
                parameters[18].Value = model.vdef4;
            else
                parameters[18].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[19].Value = model.vdef5;
            else
                parameters[19].Value = DBNull.Value;


            if (model.vdef6 != null)
                parameters[20].Value = model.vdef6;
            else
                parameters[20].Value = DBNull.Value;


            if (model.vdef7 != null)
                parameters[21].Value = model.vdef7;
            else
                parameters[21].Value = DBNull.Value;


            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [DIS_OrderExt] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[DIS_OrderExt]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [DIS_OrderExt]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.DIS_OrderExt GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [DIS_OrderExt] ");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
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
        public IList<Hi.Model.DIS_OrderExt> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [DIS_OrderExt]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.DIS_OrderExt> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.DIS_OrderExt> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[DIS_OrderExt]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.DIS_OrderExt GetModel(DataRow r)
        {
            Hi.Model.DIS_OrderExt model = new Hi.Model.DIS_OrderExt();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.OrderID = SqlHelper.GetInt(r["OrderID"]);
            model.DisAccID = SqlHelper.GetString(r["DisAccID"]);
            model.Rise = SqlHelper.GetString(r["Rise"]);
            model.Content = SqlHelper.GetString(r["Content"]);
            model.OBank = SqlHelper.GetString(r["OBank"]);
            model.OAccount = SqlHelper.GetString(r["OAccount"]);
            model.TRNumber = SqlHelper.GetString(r["TRNumber"]);
            model.BillNo = SqlHelper.GetString(r["BillNo"]);
            model.IsBill = SqlHelper.GetInt(r["IsBill"]);
            model.IsOBill = SqlHelper.GetInt(r["IsOBill"]);
            model.ProID = SqlHelper.GetInt(r["ProID"]);
            model.ProAmount = SqlHelper.GetDecimal(r["ProAmount"]);
            model.ProDID = SqlHelper.GetInt(r["ProDID"]);
            model.Protype = SqlHelper.GetString(r["Protype"]);
            model.vdef1 = SqlHelper.GetString(r["vdef1"]);
            model.vdef2 = SqlHelper.GetString(r["vdef2"]);
            model.vdef3 = SqlHelper.GetString(r["vdef3"]);
            model.vdef4 = SqlHelper.GetString(r["vdef4"]);
            model.vdef5 = SqlHelper.GetString(r["vdef5"]);
            model.vdef6 = SqlHelper.GetString(r["vdef6"]);
            model.vdef7 = SqlHelper.GetString(r["vdef7"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.DIS_OrderExt> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.DIS_OrderExt>(ds.Tables[0]);
        }
    }
}

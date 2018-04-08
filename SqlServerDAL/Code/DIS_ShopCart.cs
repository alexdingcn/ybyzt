//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/3/24 14:09:28
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
    /// 数据访问类 DIS_ShopCart
    /// </summary>
    public partial class DIS_ShopCart
    {
        public DIS_ShopCart()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_ShopCart model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_ShopCart](");
            strSql.Append("[CompID],[DisID],[GoodsID],[GoodsinfoID],[GoodsInfos],[GoodsNum],[Price],[Remark],[AuditAmount],[sumAmount],[ProID],[ProType],[DisCount],[ProNum],[CreateUserID],[CreateDate],[ts],[dr],[modifyuser],[vdef1],[vdef2],[vdef3])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@DisID,@GoodsID,@GoodsinfoID,@GoodsInfos,@GoodsNum,@Price,@Remark,@AuditAmount,@sumAmount,@ProID,@ProType,@DisCount,@ProNum,@CreateUserID,@CreateDate,@ts,@dr,@modifyuser,@vdef1,@vdef2,@vdef3)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@GoodsinfoID", SqlDbType.Int),
                    new SqlParameter("@GoodsInfos", SqlDbType.VarChar,200),
                    new SqlParameter("@GoodsNum", SqlDbType.Decimal),
                    new SqlParameter("@Price", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,500),
                    new SqlParameter("@AuditAmount", SqlDbType.Decimal),
                    new SqlParameter("@sumAmount", SqlDbType.Decimal),
                    new SqlParameter("@ProID", SqlDbType.Int),
                    new SqlParameter("@ProType", SqlDbType.Int),
                    new SqlParameter("@DisCount", SqlDbType.Decimal),
                    new SqlParameter("@ProNum", SqlDbType.Decimal),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,200),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,200),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,200)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.GoodsID;
            parameters[3].Value = model.GoodsinfoID;

            if (model.GoodsInfos != null)
                parameters[4].Value = model.GoodsInfos;
            else
                parameters[4].Value = DBNull.Value;

            parameters[5].Value = model.GoodsNum;
            parameters[6].Value = model.Price;

            if (model.Remark != null)
                parameters[7].Value = model.Remark;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.AuditAmount;
            parameters[9].Value = model.sumAmount;
            parameters[10].Value = model.ProID;
            parameters[11].Value = model.ProType;
            parameters[12].Value = model.DisCount;
            parameters[13].Value = model.ProNum;
            parameters[14].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[15].Value = model.CreateDate;
            else
                parameters[15].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[16].Value = model.ts;
            else
                parameters[16].Value = DBNull.Value;

            parameters[17].Value = model.dr;
            parameters[18].Value = model.modifyuser;

            if (model.vdef1 != null)
                parameters[19].Value = model.vdef1;
            else
                parameters[19].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[20].Value = model.vdef2;
            else
                parameters[20].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[21].Value = model.vdef3;
            else
                parameters[21].Value = DBNull.Value;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_ShopCart model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_ShopCart] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[GoodsID]=@GoodsID,");
            strSql.Append("[GoodsinfoID]=@GoodsinfoID,");
            strSql.Append("[GoodsInfos]=@GoodsInfos,");
            strSql.Append("[GoodsNum]=@GoodsNum,");
            strSql.Append("[Price]=@Price,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[AuditAmount]=@AuditAmount,");
            strSql.Append("[sumAmount]=@sumAmount,");
            strSql.Append("[ProID]=@ProID,");
            strSql.Append("[ProType]=@ProType,");
            strSql.Append("[DisCount]=@DisCount,");
            strSql.Append("[ProNum]=@ProNum,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.Int),
                    new SqlParameter("@GoodsinfoID", SqlDbType.Int),
                    new SqlParameter("@GoodsInfos", SqlDbType.VarChar,200),
                    new SqlParameter("@GoodsNum", SqlDbType.Decimal),
                    new SqlParameter("@Price", SqlDbType.Decimal),
                    new SqlParameter("@Remark", SqlDbType.VarChar,500),
                    new SqlParameter("@AuditAmount", SqlDbType.Decimal),
                    new SqlParameter("@sumAmount", SqlDbType.Decimal),
                    new SqlParameter("@ProID", SqlDbType.Int),
                    new SqlParameter("@ProType", SqlDbType.Int),
                    new SqlParameter("@DisCount", SqlDbType.Decimal),
                    new SqlParameter("@ProNum", SqlDbType.Decimal),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,200),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,200),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,200)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.GoodsID;
            parameters[4].Value = model.GoodsinfoID;

            if (model.GoodsInfos != null)
                parameters[5].Value = model.GoodsInfos;
            else
                parameters[5].Value = DBNull.Value;

            parameters[6].Value = model.GoodsNum;
            parameters[7].Value = model.Price;

            if (model.Remark != null)
                parameters[8].Value = model.Remark;
            else
                parameters[8].Value = DBNull.Value;

            parameters[9].Value = model.AuditAmount;
            parameters[10].Value = model.sumAmount;
            parameters[11].Value = model.ProID;
            parameters[12].Value = model.ProType;
            parameters[13].Value = model.DisCount;
            parameters[14].Value = model.ProNum;
            parameters[15].Value = model.CreateUserID;

            if (model.CreateDate != DateTime.MinValue)
                parameters[16].Value = model.CreateDate;
            else
                parameters[16].Value = DBNull.Value;


            if (model.ts != DateTime.MinValue)
                parameters[17].Value = model.ts;
            else
                parameters[17].Value = DBNull.Value;

            parameters[18].Value = model.dr;
            parameters[19].Value = model.modifyuser;

            if (model.vdef1 != null)
                parameters[20].Value = model.vdef1;
            else
                parameters[20].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[21].Value = model.vdef2;
            else
                parameters[21].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[22].Value = model.vdef3;
            else
                parameters[22].Value = DBNull.Value;


            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [DIS_ShopCart] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[DIS_ShopCart]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [DIS_ShopCart]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.DIS_ShopCart GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [DIS_ShopCart] ");
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
        public IList<Hi.Model.DIS_ShopCart> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [DIS_ShopCart]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.DIS_ShopCart> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.DIS_ShopCart> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[DIS_ShopCart]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.DIS_ShopCart GetModel(DataRow r)
        {
            Hi.Model.DIS_ShopCart model = new Hi.Model.DIS_ShopCart();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.GoodsID = SqlHelper.GetInt(r["GoodsID"]);
            model.GoodsinfoID = SqlHelper.GetInt(r["GoodsinfoID"]);
            model.GoodsInfos = SqlHelper.GetString(r["GoodsInfos"]);
            model.GoodsNum = SqlHelper.GetDecimal(r["GoodsNum"]);
            model.Price = SqlHelper.GetDecimal(r["Price"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.AuditAmount = SqlHelper.GetDecimal(r["AuditAmount"]);
            model.sumAmount = SqlHelper.GetDecimal(r["sumAmount"]);
            model.ProID = SqlHelper.GetInt(r["ProID"]);
            model.ProType = SqlHelper.GetInt(r["ProType"]);
            model.DisCount = SqlHelper.GetDecimal(r["DisCount"]);
            model.ProNum = SqlHelper.GetDecimal(r["ProNum"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.vdef1 = SqlHelper.GetString(r["vdef1"]);
            model.vdef2 = SqlHelper.GetString(r["vdef2"]);
            model.vdef3 = SqlHelper.GetString(r["vdef3"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.DIS_ShopCart> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.DIS_ShopCart>(ds.Tables[0]);
        }
    }
}

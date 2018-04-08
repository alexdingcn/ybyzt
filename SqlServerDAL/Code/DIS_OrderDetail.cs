//------------------------------------------------------------------------------
// 创建标识: Copyright (C) 2016 Socansoft.com 版权所有
// 创建描述: SocanCode代码生成器自动创建于 2016/10/13 16:50:29
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
    /// 数据访问类 DIS_OrderDetail
    /// </summary>
    public partial class DIS_OrderDetail
    {
        public DIS_OrderDetail()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.DIS_OrderDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [DIS_OrderDetail](");
            strSql.Append("[OrderID],[DisID],[GoodsinfoID],[GoodsName],[GoodsCode],[Unit],[GoodsInfos],[GoodsNum],[OutNum],[IsOut],[GoodsPrice],[AuditAmount],[sumAmount],[Price],[SharePrice],[ProID],[ProNum],[Protype],[Remark],[ts],[modifyuser],[vdef1],[vdef2],[vdef3],[vdef4],[vdef5])");
            strSql.Append(" values (");
            strSql.Append("@OrderID,@DisID,@GoodsinfoID,@GoodsName,@GoodsCode,@Unit,@GoodsInfos,@GoodsNum,@OutNum,@IsOut,@GoodsPrice,@AuditAmount,@sumAmount,@Price,@SharePrice,@ProID,@ProNum,@Protype,@Remark,@ts,@modifyuser,@vdef1,@vdef2,@vdef3,@vdef4,@vdef5)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@GoodsinfoID", SqlDbType.BigInt),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,200),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,200),
                    new SqlParameter("@Unit", SqlDbType.VarChar,50),
                    new SqlParameter("@GoodsInfos", SqlDbType.VarChar,500),
                    new SqlParameter("@GoodsNum", SqlDbType.Decimal),
                    new SqlParameter("@OutNum", SqlDbType.Decimal),
                    new SqlParameter("@IsOut", SqlDbType.Int),
                    new SqlParameter("@GoodsPrice", SqlDbType.Decimal),
                    new SqlParameter("@AuditAmount", SqlDbType.Decimal),
                    new SqlParameter("@sumAmount", SqlDbType.Decimal),
                    new SqlParameter("@Price", SqlDbType.Decimal),
                    new SqlParameter("@SharePrice", SqlDbType.Decimal),
                    new SqlParameter("@ProID", SqlDbType.VarChar,128),
                    new SqlParameter("@ProNum", SqlDbType.VarChar,128),
                    new SqlParameter("@Protype", SqlDbType.VarChar,128),
                    new SqlParameter("@Remark", SqlDbType.VarChar,300),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef4", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef5", SqlDbType.VarChar,128)
            };
            parameters[0].Value = model.OrderID;
            parameters[1].Value = model.DisID;
            parameters[2].Value = model.GoodsinfoID;

            if (model.GoodsName != null)
                parameters[3].Value = model.GoodsName;
            else
                parameters[3].Value = DBNull.Value;


            if (model.GoodsCode != null)
                parameters[4].Value = model.GoodsCode;
            else
                parameters[4].Value = DBNull.Value;


            if (model.Unit != null)
                parameters[5].Value = model.Unit;
            else
                parameters[5].Value = DBNull.Value;


            if (model.GoodsInfos != null)
                parameters[6].Value = model.GoodsInfos;
            else
                parameters[6].Value = DBNull.Value;

            parameters[7].Value = model.GoodsNum;
            parameters[8].Value = model.OutNum;
            parameters[9].Value = model.IsOut;
            parameters[10].Value = model.GoodsPrice;
            parameters[11].Value = model.AuditAmount;
            parameters[12].Value = model.sumAmount;
            parameters[13].Value = model.Price;
            parameters[14].Value = model.SharePrice;

            if (model.ProID != null)
                parameters[15].Value = model.ProID;
            else
                parameters[15].Value = DBNull.Value;


            if (model.ProNum != null)
                parameters[16].Value = model.ProNum;
            else
                parameters[16].Value = DBNull.Value;


            if (model.Protype != null)
                parameters[17].Value = model.Protype;
            else
                parameters[17].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[18].Value = model.Remark;
            else
                parameters[18].Value = DBNull.Value;

            parameters[19].Value = model.ts;
            parameters[20].Value = model.modifyuser;

            if (model.vdef1 != null)
                parameters[21].Value = model.vdef1;
            else
                parameters[21].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[22].Value = model.vdef2;
            else
                parameters[22].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[23].Value = model.vdef3;
            else
                parameters[23].Value = DBNull.Value;


            if (model.vdef4 != null)
                parameters[24].Value = model.vdef4;
            else
                parameters[24].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[25].Value = model.vdef5;
            else
                parameters[25].Value = DBNull.Value;

            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.DIS_OrderDetail model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [DIS_OrderDetail] set ");
            strSql.Append("[OrderID]=@OrderID,");
            strSql.Append("[DisID]=@DisID,");
            strSql.Append("[GoodsinfoID]=@GoodsinfoID,");
            strSql.Append("[GoodsName]=@GoodsName,");
            strSql.Append("[GoodsCode]=@GoodsCode,");
            strSql.Append("[Unit]=@Unit,");
            strSql.Append("[GoodsInfos]=@GoodsInfos,");
            strSql.Append("[GoodsNum]=@GoodsNum,");
            strSql.Append("[OutNum]=@OutNum,");
            strSql.Append("[IsOut]=@IsOut,");
            strSql.Append("[GoodsPrice]=@GoodsPrice,");
            strSql.Append("[AuditAmount]=@AuditAmount,");
            strSql.Append("[sumAmount]=@sumAmount,");
            strSql.Append("[Price]=@Price,");
            strSql.Append("[SharePrice]=@SharePrice,");
            strSql.Append("[ProID]=@ProID,");
            strSql.Append("[ProNum]=@ProNum,");
            strSql.Append("[Protype]=@Protype,");
            strSql.Append("[Remark]=@Remark,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser,");
            strSql.Append("[vdef1]=@vdef1,");
            strSql.Append("[vdef2]=@vdef2,");
            strSql.Append("[vdef3]=@vdef3,");
            strSql.Append("[vdef4]=@vdef4,");
            strSql.Append("[vdef5]=@vdef5");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@OrderID", SqlDbType.BigInt),
                    new SqlParameter("@DisID", SqlDbType.Int),
                    new SqlParameter("@GoodsinfoID", SqlDbType.BigInt),
                    new SqlParameter("@GoodsName", SqlDbType.VarChar,200),
                    new SqlParameter("@GoodsCode", SqlDbType.VarChar,200),
                    new SqlParameter("@Unit", SqlDbType.VarChar,50),
                    new SqlParameter("@GoodsInfos", SqlDbType.VarChar,500),
                    new SqlParameter("@GoodsNum", SqlDbType.Decimal),
                    new SqlParameter("@OutNum", SqlDbType.Decimal),
                    new SqlParameter("@IsOut", SqlDbType.Int),
                    new SqlParameter("@GoodsPrice", SqlDbType.Decimal),
                    new SqlParameter("@AuditAmount", SqlDbType.Decimal),
                    new SqlParameter("@sumAmount", SqlDbType.Decimal),
                    new SqlParameter("@Price", SqlDbType.Decimal),
                    new SqlParameter("@SharePrice", SqlDbType.Decimal),
                    new SqlParameter("@ProID", SqlDbType.VarChar,128),
                    new SqlParameter("@ProNum", SqlDbType.VarChar,128),
                    new SqlParameter("@Protype", SqlDbType.VarChar,128),
                    new SqlParameter("@Remark", SqlDbType.VarChar,300),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int),
                    new SqlParameter("@vdef1", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef2", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef3", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef4", SqlDbType.VarChar,128),
                    new SqlParameter("@vdef5", SqlDbType.VarChar,128)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.DisID;
            parameters[3].Value = model.GoodsinfoID;

            if (model.GoodsName != null)
                parameters[4].Value = model.GoodsName;
            else
                parameters[4].Value = DBNull.Value;


            if (model.GoodsCode != null)
                parameters[5].Value = model.GoodsCode;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Unit != null)
                parameters[6].Value = model.Unit;
            else
                parameters[6].Value = DBNull.Value;


            if (model.GoodsInfos != null)
                parameters[7].Value = model.GoodsInfos;
            else
                parameters[7].Value = DBNull.Value;

            parameters[8].Value = model.GoodsNum;
            parameters[9].Value = model.OutNum;
            parameters[10].Value = model.IsOut;
            parameters[11].Value = model.GoodsPrice;
            parameters[12].Value = model.AuditAmount;
            parameters[13].Value = model.sumAmount;
            parameters[14].Value = model.Price;
            parameters[15].Value = model.SharePrice;

            if (model.ProID != null)
                parameters[16].Value = model.ProID;
            else
                parameters[16].Value = DBNull.Value;


            if (model.ProNum != null)
                parameters[17].Value = model.ProNum;
            else
                parameters[17].Value = DBNull.Value;


            if (model.Protype != null)
                parameters[18].Value = model.Protype;
            else
                parameters[18].Value = DBNull.Value;


            if (model.Remark != null)
                parameters[19].Value = model.Remark;
            else
                parameters[19].Value = DBNull.Value;

            parameters[20].Value = model.ts;
            parameters[21].Value = model.dr;
            parameters[22].Value = model.modifyuser;

            if (model.vdef1 != null)
                parameters[23].Value = model.vdef1;
            else
                parameters[23].Value = DBNull.Value;


            if (model.vdef2 != null)
                parameters[24].Value = model.vdef2;
            else
                parameters[24].Value = DBNull.Value;


            if (model.vdef3 != null)
                parameters[25].Value = model.vdef3;
            else
                parameters[25].Value = DBNull.Value;


            if (model.vdef4 != null)
                parameters[26].Value = model.vdef4;
            else
                parameters[26].Value = DBNull.Value;


            if (model.vdef5 != null)
                parameters[27].Value = model.vdef5;
            else
                parameters[27].Value = DBNull.Value;


            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [DIS_OrderDetail] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[DIS_OrderDetail]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [DIS_OrderDetail]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.DIS_OrderDetail GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [DIS_OrderDetail] ");
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
        public IList<Hi.Model.DIS_OrderDetail> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [DIS_OrderDetail]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.DIS_OrderDetail> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.DIS_OrderDetail> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[DIS_OrderDetail]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.DIS_OrderDetail GetModel(DataRow r)
        {
            Hi.Model.DIS_OrderDetail model = new Hi.Model.DIS_OrderDetail();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.OrderID = SqlHelper.GetInt(r["OrderID"]);
            model.DisID = SqlHelper.GetInt(r["DisID"]);
            model.GoodsinfoID = SqlHelper.GetInt(r["GoodsinfoID"]);
            model.GoodsName = SqlHelper.GetString(r["GoodsName"]);
            model.GoodsCode = SqlHelper.GetString(r["GoodsCode"]);
            model.Unit = SqlHelper.GetString(r["Unit"]);
            model.GoodsInfos = SqlHelper.GetString(r["GoodsInfos"]);
            model.GoodsNum = SqlHelper.GetDecimal(r["GoodsNum"]);
            model.OutNum = SqlHelper.GetDecimal(r["OutNum"]);
            model.IsOut = SqlHelper.GetInt(r["IsOut"]);
            model.GoodsPrice = SqlHelper.GetDecimal(r["GoodsPrice"]);
            model.AuditAmount = SqlHelper.GetDecimal(r["AuditAmount"]);
            model.sumAmount = SqlHelper.GetDecimal(r["sumAmount"]);
            model.Price = SqlHelper.GetDecimal(r["Price"]);
            model.SharePrice = SqlHelper.GetDecimal(r["SharePrice"]);
            model.ProID = SqlHelper.GetString(r["ProID"]);
            model.ProNum = SqlHelper.GetString(r["ProNum"]);
            model.Protype = SqlHelper.GetString(r["Protype"]);
            model.Remark = SqlHelper.GetString(r["Remark"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.vdef1 = SqlHelper.GetString(r["vdef1"]);
            model.vdef2 = SqlHelper.GetString(r["vdef2"]);
            model.vdef3 = SqlHelper.GetString(r["vdef3"]);
            model.vdef4 = SqlHelper.GetString(r["vdef4"]);
            model.vdef5 = SqlHelper.GetString(r["vdef5"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.DIS_OrderDetail> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.DIS_OrderDetail>(ds.Tables[0]);
        }
    }
}

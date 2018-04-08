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
    /// 数据访问类 BD_GoodsInfo
    /// </summary>
    public partial class BD_GoodsInfo
    {
        public BD_GoodsInfo()
        { }

        #region  成员方法
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Hi.Model.BD_GoodsInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into [BD_GoodsInfo](");
            strSql.Append("[CompID],[GoodsID],[BarCode],[IsOffline],[Value1],[Value2],[Value3],[Value4],[Value5],[Value6],[Value7],[Value8],[Value9],[Value10],[ValueInfo],[SalePrice],[TinkerPrice],[IsEnabled],[CreateUserID],[CreateDate],[ts],[modifyuser])");
            strSql.Append(" values (");
            strSql.Append("@CompID,@GoodsID,@BarCode,@IsOffline,@Value1,@Value2,@Value3,@Value4,@Value5,@Value6,@Value7,@Value8,@Value9,@Value10,@ValueInfo,@SalePrice,@TinkerPrice,@IsEnabled,@CreateUserID,@CreateDate,@ts,@modifyuser)");
            strSql.Append(";select @@Identity");
            SqlParameter[] parameters = {
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.BigInt),
                    new SqlParameter("@BarCode", SqlDbType.VarChar,50),
                    new SqlParameter("@IsOffline", SqlDbType.Int),
                    new SqlParameter("@Value1", SqlDbType.VarChar,50),
                    new SqlParameter("@Value2", SqlDbType.VarChar,50),
                    new SqlParameter("@Value3", SqlDbType.VarChar,50),
                    new SqlParameter("@Value4", SqlDbType.VarChar,50),
                    new SqlParameter("@Value5", SqlDbType.VarChar,50),
                    new SqlParameter("@Value6", SqlDbType.VarChar,50),
                    new SqlParameter("@Value7", SqlDbType.VarChar,50),
                    new SqlParameter("@Value8", SqlDbType.VarChar,50),
                    new SqlParameter("@Value9", SqlDbType.VarChar,50),
                    new SqlParameter("@Value10", SqlDbType.VarChar,50),
                    new SqlParameter("@ValueInfo", SqlDbType.VarChar,500),
                    new SqlParameter("@SalePrice", SqlDbType.Decimal),
                    new SqlParameter("@TinkerPrice", SqlDbType.Decimal),
                    new SqlParameter("@IsEnabled", SqlDbType.Bit),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.CompID;
            parameters[1].Value = model.GoodsID;

            if (model.BarCode != null)
                parameters[2].Value = model.BarCode;
            else
                parameters[2].Value = DBNull.Value;

            parameters[3].Value = model.IsOffline;

            if (model.Value1 != null)
                parameters[4].Value = model.Value1;
            else
                parameters[4].Value = DBNull.Value;


            if (model.Value2 != null)
                parameters[5].Value = model.Value2;
            else
                parameters[5].Value = DBNull.Value;


            if (model.Value3 != null)
                parameters[6].Value = model.Value3;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Value4 != null)
                parameters[7].Value = model.Value4;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Value5 != null)
                parameters[8].Value = model.Value5;
            else
                parameters[8].Value = DBNull.Value;


            if (model.Value6 != null)
                parameters[9].Value = model.Value6;
            else
                parameters[9].Value = DBNull.Value;


            if (model.Value7 != null)
                parameters[10].Value = model.Value7;
            else
                parameters[10].Value = DBNull.Value;


            if (model.Value8 != null)
                parameters[11].Value = model.Value8;
            else
                parameters[11].Value = DBNull.Value;


            if (model.Value9 != null)
                parameters[12].Value = model.Value9;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Value10 != null)
                parameters[13].Value = model.Value10;
            else
                parameters[13].Value = DBNull.Value;


            if (model.ValueInfo != null)
                parameters[14].Value = model.ValueInfo;
            else
                parameters[14].Value = DBNull.Value;

            parameters[15].Value = model.SalePrice;
            parameters[16].Value = model.TinkerPrice;
            parameters[17].Value = model.IsEnabled;
            parameters[18].Value = model.CreateUserID;
            parameters[19].Value = model.CreateDate;
            parameters[20].Value = model.ts;
            parameters[21].Value = model.modifyuser;
            return SqlHelper.GetInt(SqlHelper.GetSingle(SqlHelper.LocalSqlServer, strSql.ToString(), parameters));
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Hi.Model.BD_GoodsInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update [BD_GoodsInfo] set ");
            strSql.Append("[CompID]=@CompID,");
            strSql.Append("[GoodsID]=@GoodsID,");
            strSql.Append("[BarCode]=@BarCode,");
            strSql.Append("[IsOffline]=@IsOffline,");
            strSql.Append("[Inventory]=@Inventory,");
            strSql.Append("[Value1]=@Value1,");
            strSql.Append("[Value2]=@Value2,");
            strSql.Append("[Value3]=@Value3,");
            strSql.Append("[Value4]=@Value4,");
            strSql.Append("[Value5]=@Value5,");
            strSql.Append("[Value6]=@Value6,");
            strSql.Append("[Value7]=@Value7,");
            strSql.Append("[Value8]=@Value8,");
            strSql.Append("[Value9]=@Value9,");
            strSql.Append("[Value10]=@Value10,");
            strSql.Append("[ValueInfo]=@ValueInfo,");
            strSql.Append("[SalePrice]=@SalePrice,");
            strSql.Append("[TinkerPrice]=@TinkerPrice,");
            strSql.Append("[IsEnabled]=@IsEnabled,");
            strSql.Append("[CreateUserID]=@CreateUserID,");
            strSql.Append("[CreateDate]=@CreateDate,");
            strSql.Append("[ts]=@ts,");
            strSql.Append("[dr]=@dr,");
            strSql.Append("[modifyuser]=@modifyuser");
            strSql.Append(" where [ID]=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt),
                    new SqlParameter("@CompID", SqlDbType.Int),
                    new SqlParameter("@GoodsID", SqlDbType.BigInt),
                    new SqlParameter("@BarCode", SqlDbType.VarChar,50),
                    new SqlParameter("@IsOffline", SqlDbType.Int),
                    new SqlParameter("@Inventory", SqlDbType.Decimal),
                    new SqlParameter("@Value1", SqlDbType.VarChar,50),
                    new SqlParameter("@Value2", SqlDbType.VarChar,50),
                    new SqlParameter("@Value3", SqlDbType.VarChar,50),
                    new SqlParameter("@Value4", SqlDbType.VarChar,50),
                    new SqlParameter("@Value5", SqlDbType.VarChar,50),
                    new SqlParameter("@Value6", SqlDbType.VarChar,50),
                    new SqlParameter("@Value7", SqlDbType.VarChar,50),
                    new SqlParameter("@Value8", SqlDbType.VarChar,50),
                    new SqlParameter("@Value9", SqlDbType.VarChar,50),
                    new SqlParameter("@Value10", SqlDbType.VarChar,50),
                    new SqlParameter("@ValueInfo", SqlDbType.VarChar,500),
                    new SqlParameter("@SalePrice", SqlDbType.Decimal),
                    new SqlParameter("@TinkerPrice", SqlDbType.Decimal),
                    new SqlParameter("@IsEnabled", SqlDbType.Bit),
                    new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@CreateDate", SqlDbType.DateTime),
                    new SqlParameter("@ts", SqlDbType.DateTime),
                    new SqlParameter("@dr", SqlDbType.SmallInt),
                    new SqlParameter("@modifyuser", SqlDbType.Int)
            };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.CompID;
            parameters[2].Value = model.GoodsID;

            if (model.BarCode != null)
                parameters[3].Value = model.BarCode;
            else
                parameters[3].Value = DBNull.Value;

            parameters[4].Value = model.IsOffline;
            parameters[5].Value = model.Inventory;

            if (model.Value1 != null)
                parameters[6].Value = model.Value1;
            else
                parameters[6].Value = DBNull.Value;


            if (model.Value2 != null)
                parameters[7].Value = model.Value2;
            else
                parameters[7].Value = DBNull.Value;


            if (model.Value3 != null)
                parameters[8].Value = model.Value3;
            else
                parameters[8].Value = DBNull.Value;


            if (model.Value4 != null)
                parameters[9].Value = model.Value4;
            else
                parameters[9].Value = DBNull.Value;


            if (model.Value5 != null)
                parameters[10].Value = model.Value5;
            else
                parameters[10].Value = DBNull.Value;


            if (model.Value6 != null)
                parameters[11].Value = model.Value6;
            else
                parameters[11].Value = DBNull.Value;


            if (model.Value7 != null)
                parameters[12].Value = model.Value7;
            else
                parameters[12].Value = DBNull.Value;


            if (model.Value8 != null)
                parameters[13].Value = model.Value8;
            else
                parameters[13].Value = DBNull.Value;


            if (model.Value9 != null)
                parameters[14].Value = model.Value9;
            else
                parameters[14].Value = DBNull.Value;


            if (model.Value10 != null)
                parameters[15].Value = model.Value10;
            else
                parameters[15].Value = DBNull.Value;


            if (model.ValueInfo != null)
                parameters[16].Value = model.ValueInfo;
            else
                parameters[16].Value = DBNull.Value;

            parameters[17].Value = model.SalePrice;
            parameters[18].Value = model.TinkerPrice;
            parameters[19].Value = model.IsEnabled;
            parameters[20].Value = model.CreateUserID;
            parameters[21].Value = model.CreateDate;
            parameters[22].Value = model.ts;
            parameters[23].Value = model.dr;
            parameters[24].Value = model.modifyuser;

            return SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete [BD_GoodsInfo] ");
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
            return SqlHelper.GetMaxID(SqlHelper.LocalSqlServer, "[ID]", "[BD_GoodsInfo]");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from [BD_GoodsInfo]");
            strSql.Append(" where [ID]= @ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.BigInt)};
            parameters[0].Value = ID;
            return SqlHelper.Exists(SqlHelper.LocalSqlServer, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Hi.Model.BD_GoodsInfo GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from [BD_GoodsInfo] ");
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
        public IList<Hi.Model.BD_GoodsInfo> GetList(string strSql)
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
            StringBuilder strSql = new StringBuilder("select " + strWhat + " from [BD_GoodsInfo]");
            if (!string.IsNullOrEmpty(strWhere))
                strSql.Append(" where " + strWhere);
            if (!string.IsNullOrEmpty(strOrderby))
                strSql.Append(" order by " + strOrderby);
            return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());
        }

        /// <summary>
        /// 获取泛型数据列表,在单表查询时使用
        /// </summary>
        public IList<Hi.Model.BD_GoodsInfo> GetList(string strWhat, string strWhere, string strOrderby)
        {
            return GetList(GetDataSet(strWhat, strWhere, strOrderby));
        }

        /// <summary>
        /// 分页获取泛型数据列表
        /// </summary>
        public IList<Hi.Model.BD_GoodsInfo> GetList(int pageSize, int pageIndex, string fldSort, bool sort, string strCondition, out int pageCount, out int count)
        {
            string strSql;
            DataSet ds = SqlHelper.PageList(SqlHelper.LocalSqlServer, "[BD_GoodsInfo]", null, pageSize, pageIndex, fldSort, sort, strCondition, "ID", false, out pageCount, out count, out strSql);
            return GetList(ds);
        }
        #endregion

        /// <summary>
        /// 由一行数据得到一个实体
        /// </summary>
        private Hi.Model.BD_GoodsInfo GetModel(DataRow r)
        {
            Hi.Model.BD_GoodsInfo model = new Hi.Model.BD_GoodsInfo();
            model.ID = SqlHelper.GetInt(r["ID"]);
            model.CompID = SqlHelper.GetInt(r["CompID"]);
            model.GoodsID = SqlHelper.GetInt(r["GoodsID"]);
            model.BarCode = SqlHelper.GetString(r["BarCode"]);
            model.IsOffline = SqlHelper.GetInt(r["IsOffline"]);
            model.Inventory = SqlHelper.GetDecimal(r["Inventory"]);
            model.Value1 = SqlHelper.GetString(r["Value1"]);
            model.Value2 = SqlHelper.GetString(r["Value2"]);
            model.Value3 = SqlHelper.GetString(r["Value3"]);
            model.Value4 = SqlHelper.GetString(r["Value4"]);
            model.Value5 = SqlHelper.GetString(r["Value5"]);
            model.Value6 = SqlHelper.GetString(r["Value6"]);
            model.Value7 = SqlHelper.GetString(r["Value7"]);
            model.Value8 = SqlHelper.GetString(r["Value8"]);
            model.Value9 = SqlHelper.GetString(r["Value9"]);
            model.Value10 = SqlHelper.GetString(r["Value10"]);
            model.ValueInfo = SqlHelper.GetString(r["ValueInfo"]);
            model.SalePrice = SqlHelper.GetDecimal(r["SalePrice"]);
            model.TinkerPrice = SqlHelper.GetDecimal(r["TinkerPrice"]);
            model.IsEnabled = SqlHelper.GetBool(r["IsEnabled"]);
            model.CreateUserID = SqlHelper.GetInt(r["CreateUserID"]);
            model.CreateDate = SqlHelper.GetDateTime(r["CreateDate"]);
            model.ts = SqlHelper.GetDateTime(r["ts"]);
            model.dr = SqlHelper.GetInt(r["dr"]);
            model.modifyuser = SqlHelper.GetInt(r["modifyuser"]);
            model.Batchno = SqlHelper.GetString(r["BatchNO"]);
            model.Validdate = SqlHelper.GetDateTime(r["validDate"]);
            return model;
        }

        /// <summary>
        /// 由数据集得到泛型数据列表
        /// </summary>
        private IList<Hi.Model.BD_GoodsInfo> GetList(DataSet ds)
        {
            return Common.GetListEntity<Hi.Model.BD_GoodsInfo>(ds.Tables[0]);
        }
    }
}
